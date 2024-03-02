using System;
using System.Collections.Generic;
using Contensive.Addons.Rss.Controllers;
using Contensive.Addons.Rss.Models.Db;
using Contensive.BaseClasses;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Contensive.Addons.Rss.Views {
    // 
    public class RssFeedProcessClass : AddonBaseClass {
        // 
        // 
        // =====================================================================================
        /// <summary>
        /// AddonDescription
        /// </summary>
        /// <param name="CP"></param>
        /// <returns></returns>
        public override object Execute(CPBaseClass CP) {
            string result = "";
            try {
                var RightNow = DateTime.Now;
                string SQLDateNow = CP.Db.EncodeSQLDate(RightNow);
                // 
                // Get Primary domain from domain list
                string DomainName = CP.Site.DomainPrimary;
                int Pos = Strings.InStr(1, DomainName, ",");
                if (Pos > 2) {
                    DomainName = Strings.Mid(DomainName, 1, Pos - 1);
                }
                // 
                // Load the feeds
                var feedList = new List<Models.Domain.FeedModel>();
                foreach (RSSFeedModel feed in BaseModel.createList<RSSFeedModel>(CP, "", "id desc")) {
                    var rssFeed = new Models.Domain.FeedModel() {
                        Id = feed.id,
                        Name = feed.name,
                        Description = feed.Description,
                        Link = feed.Link,
                        LogoFilename = feed.LogoFilename,
                        entryList = new List<Models.Domain.FeedEntryModel>()
                    };
                    if (string.IsNullOrWhiteSpace(rssFeed.Link))
                        rssFeed.Link = "http://" + DomainName;
                    string testfilename = feed.RSSFilename;
                    if (string.IsNullOrEmpty(testfilename)) {
                        testfilename = feed.name;
                    }
                    if (string.IsNullOrEmpty(testfilename)) {
                        testfilename = "RSSFeed" + feed.id;
                    }
                    string testFilenameNoExt = testfilename;
                    Pos = Strings.InStr(1, testFilenameNoExt, ".xml", Constants.vbTextCompare);
                    if (Pos > 0) {
                        testFilenameNoExt = Strings.Mid(testFilenameNoExt, 1, Pos - 1);
                    }
                    // 
                    // -- 20190114, from VS, removes everything before the first Unix slash
                    Pos = Strings.InStr(1, testFilenameNoExt, "/", Constants.vbTextCompare);
                    if (Pos > 0) {
                        testFilenameNoExt = Strings.Mid(testFilenameNoExt, Pos + 1);
                    }
                    // 
                    testFilenameNoExt = testEncodeFilename(testFilenameNoExt);
                    int suffixNumber = 1;
                    string testFilenameRoot = testFilenameNoExt;
                    string usedFilenames = "";
                    while (suffixNumber < 100 & Strings.InStr(1, "," + usedFilenames + ",", "," + testFilenameNoExt + ",", Constants.vbTextCompare) != 0) {
                        testFilenameNoExt = testFilenameRoot + suffixNumber.ToString();
                        suffixNumber = suffixNumber + 1;
                    }
                    usedFilenames = usedFilenames + "," + testFilenameNoExt;
                    testfilename = "rssfeeds/" + testFilenameNoExt + ".xml";
                    feed.RSSFilename = testfilename;
                    rssFeed.RSSFilename = testfilename;
                    feedList.Add(rssFeed);
                    feed.RSSDateUpdated = RightNow;
                    feed.save<RSSFeedModel>(CP);
                }
                // 
                // -- create a list of all RSSFeeds content fields - these are the many-to-many fields that point to story records for each feed
                int LastTableID = -1;
                var manyToManyFieldList = BaseModel.createList<ContentFieldModel>(CP, "(name='RSSFeeds')and(type=" + constants.FieldTypeManyToMany + ")and(authorable<>0)", "id desc");
                foreach (var manyToManyField in manyToManyFieldList) {
                    // 
                    // -- each many-to-many field represents a checked-box in the RSS Feed tab, associating a story to a feed.
                    // -- go through all records with this many-to-many field checked and add that story to that feed.
                    var storyContent = BaseModel.create<ContentModel>(CP, manyToManyField.ContentID);
                    if (storyContent is not null) {
                        var manyToManyRuleContent = BaseModel.create<ContentModel>(CP, manyToManyField.ManyToManyRuleContentID);
                        if (manyToManyRuleContent is not null) {
                            // 
                            // -- open all records in this rule table and associate stories to feeds in the feedlist
                            var csRuleRecord = CP.CSNew();
                            if (csRuleRecord.Open(manyToManyRuleContent.name)) {
                                do {
                                    // 
                                    // -- find the feed selected in the rule record for this content 
                                    int feedId = csRuleRecord.GetInteger(manyToManyField.ManyToManyRuleSecondaryField);
                                    foreach (var feed in feedList) {
                                        if (feedId == feed.Id) {
                                            // 
                                            // -- add the story to this feed
                                            int storyRecordID = csRuleRecord.GetInteger(manyToManyField.ManyToManyRulePrimaryField);
                                            // 
                                            // -- prevent duplicate stories in the same feed
                                            bool dupFound = false;
                                            foreach (var entry in feed.entryList) {
                                                if (entry.storyRecordID == storyRecordID & entry.storyContentID == storyContent.id) {
                                                    dupFound = true;
                                                    break;
                                                }
                                            }
                                            if (!dupFound) {
                                                // 
                                                // -- this story is not in the feed, add the story to the feed
                                                var csStory = CP.CSNew();
                                                if (csStory.OpenRecord(storyContent.name, storyRecordID)) {
                                                    var entry = new Models.Domain.FeedEntryModel();
                                                    feed.entryList.Add(entry);
                                                    entry.storyContentID = storyContent.id;
                                                    entry.storyRecordID = storyRecordID;
                                                    if (CP.Content.IsField(storyContent.name, "RSSTitle")) {
                                                        entry.Title = csStory.GetText("RSSTitle");
                                                    }
                                                    if (string.IsNullOrEmpty(entry.Title)) {
                                                        entry.Title = csStory.GetText("Name");
                                                    }
                                                    if (CP.Content.IsField(storyContent.name, "RSSDateExpire")) {
                                                        entry.DateExpires = csStory.GetDate("RSSDateExpire");
                                                    }
                                                    entry.DatePublish = DateTime.MinValue;
                                                    if (CP.Content.IsField(storyContent.name, "RSSDatePublish")) {
                                                        entry.DatePublish = csStory.GetDate("RSSDatePublish");
                                                        if (entry.DatePublish < DateTime.Parse("1990-08-07")) {
                                                            entry.DatePublish = DateTime.MinValue;
                                                        }
                                                    }
                                                    if (entry.DatePublish == DateTime.MinValue) {
                                                        entry.DatePublish = csStory.GetDate("dateAdded");
                                                        if (entry.DatePublish == DateTime.MinValue) {
                                                            entry.DatePublish = RightNow;
                                                            csStory.SetField("dateAdded", Conversions.ToString(entry.DatePublish));
                                                            if (CP.Content.IsField(storyContent.name, "RSSDatePublish")) {
                                                                csStory.SetField("RSSDatePublish", Conversions.ToString(entry.DatePublish));
                                                            }
                                                        }
                                                    }
                                                    if (CP.Content.IsField(storyContent.name, "RSSDescription")) {
                                                        entry.Description = csStory.GetText("RSSDescription");
                                                    }
                                                    if (CP.Content.IsField(storyContent.name, "RSSLink")) {
                                                        entry.Link = csStory.GetText("RSSLink");
                                                    }
                                                    if (CP.Content.IsField(storyContent.name, "PodcastMediaLink")) {
                                                        entry.PodcastMediaLink = csStory.GetText("PodcastMediaLink");
                                                    }
                                                }
                                                csStory.Close();
                                            }
                                            break;
                                        }
                                    }
                                    csRuleRecord.GoNext();
                                }
                                while (csRuleRecord.OK());
                            }
                            csRuleRecord.Close();
                        }
                    }
                }
                // .
                // sort the entries in each feed in feedlist
                // tbd
                foreach (var feed in feedList)

                    feed.entryList.Sort((a, b) => b.DatePublish.CompareTo(a.DatePublish));
                // 
                // 
                // 
                // Build the Feeds from the Feed Arrays
                // 
                string serverPageDefault = CP.Site.GetText("SERVERPAGEDEFAULT", "");
                int pageContentContentId = CP.Content.GetID("page content");
                var EnclosureLength = default(int);
                foreach (var feed in feedList) {
                    var Doc = new System.Xml.XmlDocument();
                    // 
                    // Build this feed
                    if (!string.IsNullOrEmpty(feed.Link) & Strings.InStr(1, feed.Link, "://") == 0) {
                        feed.Link = "http://" + feed.Link;
                    }
                    // 
                    System.Xml.XmlNode BaseNode = Doc.CreateElement("rss");
                    BaseNode.Attributes.GetNamedItem("version", "2.0");
                    BaseNode.Attributes.GetNamedItem("xmlns:atom", "http://www.w3.org/2005/Atom");
                    Doc.AppendChild(BaseNode);
                    // 
                    System.Xml.XmlNode ChannelNode = Doc.CreateElement("channel");
                    BaseNode.AppendChild(ChannelNode);
                    // 
                    var Node = Doc.CreateElement("title");
                    Node.InnerText = feed.Name;
                    ChannelNode.AppendChild(Node);
                    // 
                    Node = Doc.CreateElement("description");
                    Node.InnerText = feed.Description;
                    ChannelNode.AppendChild(Node);
                    // 
                    // per validator.org, atom:link should be in the channel
                    // <atom:link href="http://dallas.example.com/rss.xml" rel="self" type="application/rss+xml" />
                    // 
                    Node = Doc.CreateElement("atom:link");
                    Node.Attributes.Append(Doc.CreateAttribute("href", GenericController.getCdnFilePathPrefixAbsolute(CP) + feed.RSSFilename));
                    Node.Attributes.Append(Doc.CreateAttribute("rel", "self"));
                    Node.Attributes.Append(Doc.CreateAttribute("type", "application/rss+xml"));
                    ChannelNode.AppendChild(Node);
                    // 
                    if (!string.IsNullOrEmpty(feed.Link)) {
                        Node = Doc.CreateElement("link");
                        Node.InnerText = feed.Link;
                        ChannelNode.AppendChild(Node);
                    }
                    if (!string.IsNullOrEmpty(feed.LogoFilename)) {
                        // 
                        // Create Image
                        // 
                        System.Xml.XmlNode ImageNode = Doc.CreateElement("image");
                        ChannelNode.AppendChild(ImageNode);
                        // 
                        Node = Doc.CreateElement("url");
                        Node.InnerText = CP.Http.CdnFilePathPrefixAbsolute + feed.LogoFilename;
                        ImageNode.AppendChild(Node);
                        // 
                        Node = Doc.CreateElement("title");
                        Node.InnerText = feed.Name;
                        ImageNode.AppendChild(Node);
                        // 
                        Node = Doc.CreateElement("description");
                        Node.InnerText = feed.Description;
                        ImageNode.AppendChild(Node);
                        // 
                        if (!string.IsNullOrEmpty(feed.Link)) {
                            Node = Doc.CreateElement("link");
                            Node.InnerText = feed.Link;
                            ImageNode.AppendChild(Node);
                        }
                    }
                    foreach (var entry in feed.entryList) {
                        if (entry.DateExpires == DateTime.MinValue | entry.DateExpires > RightNow) {
                            if (entry.DatePublish == DateTime.MinValue | entry.DatePublish < RightNow) {
                                // 
                                // create Item Node
                                // 
                                System.Xml.XmlNode ItemNode = Doc.CreateElement("item");
                                ChannelNode.AppendChild(ItemNode);
                                // 
                                Node = Doc.CreateElement("title");
                                Node.InnerText = entry.Title.Trim();
                                ItemNode.AppendChild(Node);
                                // 
                                Node = Doc.CreateElement("description");
                                Node.InnerText = entry.Description;
                                ItemNode.AppendChild(Node);
                                // 
                                if (entry.DatePublish != DateTime.MinValue) {
                                    string ItemDatePublishText = GenericController.GetGMTFromDate(entry.DatePublish);
                                    Node = Doc.CreateElement("pubDate");
                                    Node.InnerText = ItemDatePublishText;
                                    ItemNode.AppendChild(Node);
                                }
                                // 
                                string Link = Strings.Trim(entry.Link);
                                if (string.IsNullOrEmpty(Link)) {
                                    var CS3 = CP.CSNew();
                                    CS3.OpenSQL("select link from ccContentWatch where contentid=" + entry.storyContentID + " and recordid=" + entry.storyRecordID);
                                    if (CS3.OK()) {
                                        Link = CS3.GetText("link").Trim();
                                    }
                                    CS3.Close();
                                    if (string.IsNullOrEmpty(Link)) {
                                        if (entry.storyContentID == pageContentContentId) {
                                            Link = Strings.Trim(serverPageDefault + "?bid=" + entry.storyRecordID);
                                        }
                                    }
                                }
                                if (Strings.InStr(1, Link, "://") == 0) {
                                    if (Strings.Left(Link, 1) != "/") {
                                        Link = "/" + Link;
                                    }
                                    Link = CP.Utils.EncodeAppRootPath(Link);
                                    Link = "http://" + DomainName + Link;
                                }
                                if (!string.IsNullOrEmpty(Link)) {
                                    // 
                                    // create link node
                                    // 
                                    Node = Doc.CreateElement("link");
                                    Node.InnerText = Link;
                                    ItemNode.AppendChild(Node);
                                    // 
                                    // create guid node
                                    // 
                                    Node = Doc.CreateElement("guid");
                                    Node.InnerText = Link;
                                    ItemNode.AppendChild(Node);
                                }
                                // 
                                // 
                                string Enclosure = entry.PodcastMediaLink;
                                if (!string.IsNullOrEmpty(Enclosure)) {
                                    int Ptr = Strings.InStrRev(Enclosure, ".");
                                    string MimeType = "";
                                    if (Ptr > 0) {
                                        string Ext = Strings.LCase(Strings.Mid(Enclosure, Ptr + 1));
                                        switch (Ext ?? "") {
                                            case "mp3": {
                                                    MimeType = "audio/mpeg";
                                                    break;
                                                }
                                            case "m4v": {
                                                    MimeType = "video/x-m4v";
                                                    break;
                                                }
                                            case "m4a": {
                                                    MimeType = "audio/x-m4a";
                                                    break;
                                                }
                                            case "avi": {
                                                    MimeType = "video/avi";
                                                    break;
                                                }
                                            case "mpeg":
                                            case "mpg": {
                                                    MimeType = "video/mpeg";
                                                    break;
                                                }
                                            case "mp4": {
                                                    MimeType = "audio/mpeg";
                                                    break;
                                                }
                                            case "qt":
                                            case "mov": {
                                                    MimeType = "video/quicktime";
                                                    break;
                                                }
                                            case "wma":
                                            case "wmv":
                                            case "asf": {
                                                    MimeType = "application/x-oleobject";
                                                    break;
                                                }
                                            case "swf": {
                                                    MimeType = "application/x-shockwave-flash";
                                                    break;
                                                }
                                            case "flv": {
                                                    MimeType = "application/x-shockwave-flash";
                                                    break;
                                                }
                                            case "rm": {
                                                    MimeType = "audio/x-pn-realaudio-plugin";
                                                    break;
                                                }
                                            case "youtube": {
                                                    MimeType = "application/x-shockwave-flash";
                                                    break;
                                                }
                                        }
                                    }
                                    // 
                                    Node = Doc.CreateElement("enclosure");

                                    Node.SetAttribute("url", Enclosure);
                                    if (EnclosureLength != 0) {
                                        Node.SetAttribute("length", "100000");
                                    }
                                    if (!string.IsNullOrEmpty(MimeType)) {
                                        Node.SetAttribute("type", MimeType);
                                    }
                                    ItemNode.AppendChild(Node);
                                }
                            }
                        }
                    }
                    // 
                    // -- initialize application. If authentication needed and not login page, pass true
                    string encoding = CP.Site.GetText("Site Character Encoding", "utf-8");
                    if (string.IsNullOrEmpty(encoding)) {
                        encoding = "utf-8";
                    }
                    // 
                    // -- there may be legacy systems that have hardcoded a path from wwwRoot/RSS to these files. In those legacy cases, create an IIS virtual path /RSS to the cden/rss folder
                    CP.CdnFiles.Save(feed.RSSFilename, "<?xml version=\"1.0\" encoding=\"" + encoding + "\"?>" + Doc.InnerXml);
                }
            } catch (Exception ex) {
                CP.Site.ErrorReport(ex);
            }
            return result;
        }
        // 
        // 
        // 
        private string GetPrimaryDomainName(CPBaseClass cp, string DomainNameList) {
            string GetPrimaryDomainNameRet = default;
            string result = "";
            try {
                string[] CopySplit;
                GetPrimaryDomainNameRet = DomainNameList;
                if (Strings.InStr(1, GetPrimaryDomainNameRet, ",", (CompareMethod)1) != 0) {
                    CopySplit = Strings.Split(cp.Site.DomainPrimary, ",");
                    GetPrimaryDomainNameRet = CopySplit[0];
                }
                // 
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
            }
            return result;
            // 

        }
        // 
        // 
        // 
        private string testEncodeFilename(string Source) {
            string testEncodeFilenameRet = default;
            string allowed;
            string chr;
            int Ptr;
            int cnt;
            string returnString;
            // 
            // returnString = source
            returnString = "";
            cnt = Strings.Len(Source);
            if (cnt > 254) {
                cnt = 254;
            }
            allowed = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ^&'@{}[],$-#()%.+~_";
            var loopTo = cnt;
            for (Ptr = 1; Ptr <= loopTo; Ptr++) {
                chr = Strings.Mid(Source, Ptr, 1);
                if (Conversions.ToBoolean(Strings.InStr(1, allowed, chr, Constants.vbBinaryCompare))) {
                    returnString = returnString + chr;
                }
            }
            testEncodeFilenameRet = returnString;
            return testEncodeFilenameRet;
        }

    }
}