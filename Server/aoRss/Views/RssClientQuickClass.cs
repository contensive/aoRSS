using System;
using System.IO;
using Contensive.Addons.Rss.Models.Db;
using Contensive.Addons.Rss.Models.View;
using Contensive.BaseClasses;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Contensive.Addons.Rss.Views {
    // 
    public class RssClientQuickClass : AddonBaseClass {
        // 
        private const string RSSRootNode = "rss";
        private const string AtomRootNode = "feed";

        public override object Execute(CPBaseClass CP) {
            int hint = 10;
            try {
                var request = new RequestModel(CP);
                if (string.IsNullOrEmpty(request.instanceId)) {
                    CP.Site.ErrorReport("RSSQuickClient the instanceId is empty");
                    return "";
                }
                // 
                var rssClient = BaseModel.create<RSSClientModel>(CP, request.instanceId);
                if (rssClient is null) {
                    // 
                    // -- create default record
                    rssClient = BaseModel.@add<RSSClientModel>(CP);
                    rssClient.ccguid = request.instanceId;
                    rssClient.name = "Quick Client created " + DateTime.Now.ToString();
                    // 
                    // -- pickup either the legacy -wrench' values, or the addon's feature arguments
                    rssClient.url = CP.Doc.GetText("URL").Trim();
                    if (string.IsNullOrWhiteSpace(rssClient.url))
                        rssClient.url = "http://www.contensive.com/rss/OpenUp.xml";
                    rssClient.refreshhours = CP.Utils.EncodeInteger(CP.Doc.GetText("RefreshHours"));
                    if (rssClient.refreshhours == 0)
                        rssClient.refreshhours = 1;
                    rssClient.numberOfStories = CP.Utils.EncodeInteger(CP.Doc.GetText("Number of Stories"));
                    if (rssClient.numberOfStories == 0)
                        rssClient.numberOfStories = 99;
                    rssClient.save<RSSClientModel>(CP);
                }
                hint = 20;
                if (string.IsNullOrWhiteSpace(rssClient.url)) {
                    return "";
                }
                // 
                bool SaveCache = true;
                string feedContent = "";
                string feedCacheFilename = encodeFilename(rssClient.url);
                feedCacheFilename = @"aoRSSClientFiles\" + "" + ".txt";
                string feedCache = CP.CdnFiles.Read(feedCacheFilename);
                if (string.IsNullOrEmpty(feedCache)) {
                    hint = 30;
                    // 
                    // -- feed cache has content, check if valid
                    using (var cacheReader = new StringReader(feedCache)) {
                        hint = 31;
                        string cacheLine1 = cacheReader.ReadLine();
                        if (!string.IsNullOrEmpty(cacheLine1)) {
                            if (Strings.Trim(cacheLine1.ToLowerInvariant()) == "rss client quick reader") {
                                var cacheLastRefresh = CP.Utils.EncodeDate(cacheReader.ReadLine());
                                if (cacheLastRefresh > DateTime.MinValue) {
                                    if (cacheLastRefresh.AddHours(rssClient.refreshhours) > DateTime.Now) {
                                        // 
                                        // Use the cached feed
                                        // 
                                        feedContent = cacheReader.ReadToEnd();
                                        SaveCache = false;
                                    }
                                }
                            }
                        }
                    }
                }
                hint = 40;
                if (string.IsNullOrEmpty(feedContent)) {
                    try {
                        // 
                        // Get a new copy of the feed (hack the & out until we find out why its there)
                        var doc = new System.Xml.XmlDocument();
                        doc.Load(rssClient.url.Replace("&", "%26"));
                        feedContent = doc.InnerXml;
                        SaveCache = true;
                    } catch (Exception ex) {
                        CP.Site.ErrorReport(ex, "Exception during fetch, rssClient.url [" + rssClient.url + "]");
                        throw;
                    }
                }
                hint = 50;
                string result = "";
                if (!string.IsNullOrEmpty(feedContent)) {
                    hint = 60;
                    var doc = new System.Xml.XmlDocument();
                    doc.LoadXml(feedContent);
                    {
                        var withBlock = doc.DocumentElement;
                        // 
                        if ((Strings.LCase(withBlock.Name) ?? "") == (Strings.LCase(RSSRootNode) ?? "")) {
                            // 
                            // RSS Feed
                            // 
                            bool IsRSS = true;
                            result = GetRSS(CP, doc.InnerXml, rssClient.numberOfStories);
                        } else if ((Strings.LCase(withBlock.Name) ?? "") == (Strings.LCase(AtomRootNode) ?? "")) {
                            // 
                            // Atom Feed
                            // 
                            bool isAtom = true;
                            result = GetAtom(CP, doc.InnerXml, rssClient.numberOfStories.ToString());
                        } else {
                            // 
                            // Bad Feed
                            // 
                            SaveCache = false;
                            if (CP.User.IsAdmin) {
                                result = CP.Html.adminHint("The RSS Feed [" + rssClient.url + "] returned an incompatible file.");
                            }
                        }
                    }
                    hint = 70;
                    // 
                    // Save this feed into the cache
                    // 
                    if (SaveCache) {
                        string FeedHeader = "RSS Client Quick Reader" + Constants.vbCrLf + Conversions.ToString(DateTime.Now);
                        CP.CdnFiles.Save(feedCacheFilename, FeedHeader + Constants.vbCrLf + feedContent);
                    }
                    hint = 80;
                }
                hint = 90;
                // 
                if (CP.User.IsEditingAnything) {
                    result = CP.Content.GetEditLink("RSS Clients", rssClient.id.ToString(), false, "RSS Quick Client Settings", CP.User.IsAdmin) + result;
                }
                return result;
            } catch (Exception ex) {
                CP.Site.ErrorReport(ex, "hint [" + hint + "]");
                throw;
            }
        }
        // 
        // =================================================================================
        // Read RSS Feed
        // =================================================================================
        // 
        private string GetRSS(CPBaseClass cp, string Feed, long MaxStories) {
            try {
                string result = "";
                // 
                var StoryCnt = default(int);
                string ItemPubDate;
                string EnclosureRow;
                int Ptr;
                var Found = default(bool);
                int EnclosureCnt;
                string ChannelImage;
                string ChannelTitle;
                string ChannelDescription;
                string ChannelPubDate = "";
                string ChannelItem;
                string ChannelLink;
                string NewChannelImage;
                string ItemLink;
                string ItemTitle;
                string ItemDescription;
                string ImageWidth;
                string ImageHeight;
                string ImageTitle;
                string ImageURL;
                string ImageLink;
                System.Xml.XmlDocument doc;
                // 
                // Convert the feed to HTML
                // 
                if (!string.IsNullOrEmpty(Feed)) {
                    doc = new System.Xml.XmlDocument();
                    doc.LoadXml(Feed);
                    {
                        var withBlock = doc.DocumentElement;
                        ChannelTitle = "";
                        ChannelDescription = "";
                        ChannelLink = "";
                        foreach (System.Xml.XmlNode RootNode in withBlock.ChildNodes) {
                            switch (Strings.LCase(RootNode.Name) ?? "") {
                                case "channel": {
                                        ChannelTitle = "";
                                        ChannelDescription = "";
                                        ChannelLink = "";
                                        ChannelImage = "";
                                        ChannelItem = "";
                                        foreach (System.Xml.XmlNode ChannelNode in RootNode.ChildNodes) {
                                            switch (Strings.LCase(ChannelNode.Name) ?? "") {
                                                case "pubdate": {
                                                        ChannelPubDate = ChannelNode.InnerText;
                                                        break;
                                                    }
                                                case "title": {
                                                        ChannelTitle = ChannelNode.InnerText;
                                                        break;
                                                    }
                                                case "description": {
                                                        ChannelDescription = ChannelNode.InnerText;
                                                        break;
                                                    }
                                                case "link": {
                                                        ChannelLink = ChannelNode.InnerText;
                                                        break;
                                                    }
                                                case "image": {
                                                        ImageWidth = "";
                                                        ImageHeight = "";
                                                        ImageTitle = "";
                                                        ImageURL = "";
                                                        ImageLink = "";
                                                        NewChannelImage = "";
                                                        foreach (System.Xml.XmlNode ImageNode in ChannelNode.ChildNodes) {
                                                            switch (Strings.LCase(ImageNode.Name) ?? "") {
                                                                case "title": {
                                                                        ImageTitle = ImageNode.InnerText;
                                                                        break;
                                                                    }
                                                                case "url": {
                                                                        ImageURL = ImageNode.InnerText;
                                                                        break;
                                                                    }
                                                                case "link": {
                                                                        ImageLink = ImageNode.InnerText;
                                                                        break;
                                                                    }
                                                                case "width": {
                                                                        ImageWidth = ImageNode.InnerText;
                                                                        break;
                                                                    }
                                                                case "height": {
                                                                        ImageHeight = ImageNode.InnerText;
                                                                        break;
                                                                    }
                                                            }
                                                        }

                                                        if (!string.IsNullOrEmpty(ImageURL)) {
                                                            NewChannelImage = NewChannelImage + "<img class=ChannelImage src=\"" + ImageURL + "\"";
                                                            if (!string.IsNullOrEmpty(ImageWidth)) {
                                                                NewChannelImage = NewChannelImage + " width=\"" + ImageWidth + "\"";
                                                            }
                                                            if (!string.IsNullOrEmpty(ImageHeight)) {
                                                                NewChannelImage = NewChannelImage + " height=\"" + ImageHeight + "\"";
                                                            }
                                                            if (!string.IsNullOrEmpty(ImageTitle)) {
                                                                NewChannelImage = NewChannelImage + " title=\"" + ImageTitle + "\"";
                                                            }
                                                            NewChannelImage = NewChannelImage + " style=\"float:left\" border=0>";
                                                            if (!string.IsNullOrEmpty(ImageLink)) {
                                                                NewChannelImage = "<a href=\"" + ImageLink + "\" target=_blank>" + NewChannelImage + "</a>";
                                                            }
                                                            ChannelImage = ChannelImage + NewChannelImage;
                                                        }

                                                        break;
                                                    }
                                                case "item": {
                                                        ItemTitle = "";
                                                        ItemLink = "";
                                                        ItemDescription = "";
                                                        ItemPubDate = "";
                                                        EnclosureCnt = 0;
                                                        EnclosureType[] Enclosure = new EnclosureType[1];
                                                        foreach (System.Xml.XmlNode ItemNode in ChannelNode.ChildNodes) {
                                                            switch (Strings.LCase(ItemNode.Name) ?? "") {
                                                                case "title": {
                                                                        ItemTitle = ItemNode.InnerText;
                                                                        break;
                                                                    }
                                                                case "description": {
                                                                        ItemDescription = ItemNode.InnerText;
                                                                        break;
                                                                    }
                                                                case "link": {
                                                                        ItemLink = ItemNode.InnerText;
                                                                        break;
                                                                    }
                                                                case "pubdate": {
                                                                        ItemPubDate = ItemNode.InnerText;
                                                                        break;
                                                                    }
                                                                case "enclosure": {
                                                                        Array.Resize(ref Enclosure, EnclosureCnt + 1);
                                                                        Enclosure[EnclosureCnt].URL = GetXMLAttribute(cp, Found, (System.Xml.XmlDocument)ItemNode, "url");
                                                                        Enclosure[EnclosureCnt].Type = GetXMLAttribute(cp, Found, (System.Xml.XmlDocument)ItemNode, "type");
                                                                        Enclosure[EnclosureCnt].Length = GetXMLAttribute(cp, Found, (System.Xml.XmlDocument)ItemNode, "length");
                                                                        EnclosureCnt = EnclosureCnt + 1;
                                                                        break;
                                                                    }
                                                            }
                                                        }
                                                        string[] DateSplit;
                                                        if (!string.IsNullOrEmpty(ItemPubDate)) {
                                                            DateSplit = Strings.Split(ItemPubDate, " ");
                                                            if (Information.UBound(DateSplit) > 2) {
                                                                ItemPubDate = DateSplit[0] + " " + DateSplit[1] + " " + DateSplit[2] + " " + DateSplit[3];
                                                            }
                                                            ItemPubDate = Constants.vbCrLf + Constants.vbTab + Constants.vbTab + "<div class=ItemPubDate>" + ItemPubDate + "</div>";
                                                        }
                                                        if (!string.IsNullOrEmpty(ItemTitle)) {
                                                            if (!string.IsNullOrEmpty(ItemLink)) {
                                                                ItemTitle = "<a href=\"" + ItemLink + "\" target=_blank>" + ItemTitle + "</a>";
                                                            }
                                                            ItemTitle = Constants.vbCrLf + Constants.vbTab + Constants.vbTab + "<h3>" + ItemTitle + "</h3>";
                                                        }
                                                        if (!string.IsNullOrEmpty(ItemDescription)) {
                                                            ItemDescription = Constants.vbCrLf + Constants.vbTab + Constants.vbTab + "<div class=ItemDescription>" + ItemDescription + "</div>";
                                                        }
                                                        // 
                                                        EnclosureRow = "";
                                                        if (EnclosureCnt > 0) {
                                                            var loopTo = EnclosureCnt - 1;
                                                            for (Ptr = 0; Ptr <= loopTo; Ptr++) {
                                                                {
                                                                    ref var withBlock1 = ref Enclosure[Ptr];
                                                                    if (!string.IsNullOrEmpty(withBlock1.URL)) {
                                                                        EnclosureRow = EnclosureRow + Constants.vbCrLf + Constants.vbTab + Constants.vbTab + Constants.vbTab + "<div class=ItemEnclosure><a href=\"" + withBlock1.URL + "\">Media</a></div>";
                                                                    }
                                                                }
                                                            }
                                                            if (!string.IsNullOrEmpty(EnclosureRow)) {
                                                                EnclosureRow = "" + Constants.vbCrLf + Constants.vbTab + Constants.vbTab + "<div class=ItemEnclosureRow>" + EnclosureRow + Constants.vbCrLf + Constants.vbTab + Constants.vbTab + "</div>";


                                                            }
                                                        }
                                                        result = result + Constants.vbCrLf + Constants.vbTab + "<hr style=\"clear:both\"><div class=ChannelItem>" + ItemTitle + ItemPubDate + ItemDescription + EnclosureRow + Constants.vbCrLf + Constants.vbTab + "</div>" + "";






                                                        StoryCnt = StoryCnt + 1;
                                                        break;
                                                    }
                                            }
                                            if (StoryCnt >= MaxStories) {
                                                break;
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(ChannelLink)) {
                                            ChannelTitle = "<a href=\"" + ChannelLink + "\" target=_blank>" + ChannelTitle + "</a>";
                                        }
                                        if (!string.IsNullOrEmpty(ChannelImage)) {
                                            ChannelDescription = ChannelImage + ChannelDescription;
                                        }
                                        result = "" + Constants.vbCrLf + Constants.vbTab + "<h2>" + ChannelTitle + "</h2>" + Constants.vbCrLf + Constants.vbTab + "<div class=ChannelPubdate>" + ChannelPubDate + "</div>" + Constants.vbCrLf + Constants.vbTab + "<div class=ChannelDescription>" + ChannelDescription + "</div>" + result;



                                        break;
                                    }
                            }
                        }
                        result = "" + Constants.vbCrLf + "<div class=RSSQuickClient>" + result + Constants.vbCrLf + "</div>";


                    }
                }
                return result;
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
        }
        // 
        // =================================================================================
        // Read Atom Feed
        // =================================================================================
        // 
        private string GetAtom(CPBaseClass cp, string Feed, string MaxStories) {
            string result = "";
            try {
                // 
                var StoryCnt = default(int);
                string Pos;
                string[] DateSplit;
                // 
                string ItemPubDate;
                string EnclosureRow;
                int Ptr;
                int EnclosureCnt;
                string ChannelImage;
                string ChannelTitle;
                string ChannelDescription;
                string ChannelPubDate = "";
                string ChannelItem;
                string ChannelLink;
                string NewChannelImage;
                // 
                string ItemLink;
                string ItemTitle;
                string ItemDescription;
                // 
                string ImageWidth;
                string ImageHeight;
                string ImageTitle;
                string ImageURL;
                string ImageLink;
                System.Xml.XmlDocument doc;
                // 
                // Convert the feed to HTML
                // 
                if (!string.IsNullOrEmpty(Feed)) {
                    doc = new System.Xml.XmlDocument();
                    doc.LoadXml(Feed);
                    {
                        var withBlock = doc.DocumentElement;
                        ChannelTitle = "";
                        ChannelDescription = "";
                        ChannelLink = "";
                        ChannelImage = "";
                        ChannelItem = "";
                        bool isFound = false;
                        foreach (System.Xml.XmlNode RootNode in withBlock.ChildNodes) {
                            // 
                            // Atom Feed only has one channel, so there is no Channel element
                            // 
                            bool exitFor = false;
                            switch (Strings.LCase(RootNode.Name) ?? "") {
                                case "updated": {
                                        ChannelPubDate = RootNode.InnerText;
                                        Pos = Strings.InStr(1, ChannelPubDate, "T", Constants.vbTextCompare).ToString();
                                        if (Conversions.ToInteger(Pos) > 0) {
                                            ChannelPubDate = Strings.Mid(ChannelPubDate, 1, Conversions.ToInteger(Pos) - 1);
                                            Pos = Strings.InStr(1, ChannelPubDate, "-").ToString();
                                            if (Conversions.ToInteger(Pos) > 0) {
                                                DateSplit = Strings.Split(ChannelPubDate, "-");
                                                if (Information.UBound(DateSplit) == 2) {
                                                    ChannelPubDate = Strings.FormatDateTime(cp.Utils.EncodeDate(DateSplit[1] + "/" + DateSplit[2] + "/" + DateSplit[0]), Constants.vbLongDate);
                                                }
                                            }
                                        }

                                        break;
                                    }
                                case "title": {
                                        ChannelTitle = RootNode.InnerText;
                                        break;
                                    }
                                case "subtitle": {
                                        ChannelDescription = RootNode.InnerText;
                                        break;
                                    }
                                case "link": {
                                        string linkType;
                                        linkType = GetXMLAttribute(cp, isFound, (System.Xml.XmlDocument)RootNode, "type");
                                        if (Strings.LCase(linkType) == "text/html") {
                                            ChannelLink = GetXMLAttribute(cp, isFound, (System.Xml.XmlDocument)RootNode, "href");
                                        }

                                        break;
                                    }
                                case "image": {
                                        ImageWidth = "";
                                        ImageHeight = "";
                                        ImageTitle = "";
                                        ImageURL = "";
                                        ImageLink = "";
                                        NewChannelImage = "";
                                        foreach (System.Xml.XmlNode ImageNode in RootNode.ChildNodes) {
                                            switch (Strings.LCase(ImageNode.Name) ?? "") {
                                                case "title": {
                                                        ImageTitle = ImageNode.InnerText;
                                                        break;
                                                    }
                                                case "url": {
                                                        ImageURL = ImageNode.InnerText;
                                                        break;
                                                    }
                                                case "link": {
                                                        ImageLink = ImageNode.InnerText;
                                                        break;
                                                    }
                                                case "width": {
                                                        ImageWidth = ImageNode.InnerText;
                                                        break;
                                                    }
                                                case "height": {
                                                        ImageHeight = ImageNode.InnerText;
                                                        break;
                                                    }
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(ImageURL)) {
                                            NewChannelImage = NewChannelImage + "<img class=ChannelImage src=\"" + ImageURL + "\"";
                                            if (!string.IsNullOrEmpty(ImageWidth)) {
                                                NewChannelImage = NewChannelImage + " width=\"" + ImageWidth + "\"";
                                            }
                                            if (!string.IsNullOrEmpty(ImageHeight)) {
                                                NewChannelImage = NewChannelImage + " height=\"" + ImageHeight + "\"";
                                            }
                                            if (!string.IsNullOrEmpty(ImageTitle)) {
                                                NewChannelImage = NewChannelImage + " title=\"" + ImageTitle + "\"";
                                            }
                                            NewChannelImage = NewChannelImage + " style=\"float:left\" border=0>";
                                            if (!string.IsNullOrEmpty(ImageLink)) {
                                                NewChannelImage = "<a href=\"" + ImageLink + "\" target=_blank>" + NewChannelImage + "</a>";
                                            }
                                            ChannelImage = ChannelImage + NewChannelImage;
                                        }

                                        break;
                                    }
                                case "entry": {
                                        ItemTitle = "";
                                        ItemLink = "";
                                        ItemDescription = "";
                                        ItemPubDate = "";
                                        EnclosureCnt = 0;
                                        foreach (System.Xml.XmlNode ItemNode in RootNode.ChildNodes) {
                                            string linkType = null;
                                            switch (Strings.LCase(ItemNode.Name) ?? "") {
                                                case "title": {
                                                        ItemTitle = ItemNode.InnerText;
                                                        break;
                                                    }
                                                case "link": {
                                                        linkType = GetXMLAttribute(cp, isFound, (System.Xml.XmlDocument)ItemNode, "type");


                                                        if (Strings.LCase(linkType) == "text/html") {
                                                            ItemLink = GetXMLAttribute(cp, isFound, (System.Xml.XmlDocument)ItemNode, "href");
                                                        }

                                                        break;
                                                    }
                                                case "updated": {
                                                        ItemPubDate = ItemNode.InnerText;
                                                        Pos = Strings.InStr(1, ItemPubDate, "T", Constants.vbTextCompare).ToString();
                                                        if (Conversions.ToInteger(Pos) > 0) {
                                                            ItemPubDate = Strings.Mid(ItemPubDate, 1, Conversions.ToInteger(Pos) - 1);
                                                            Pos = Strings.InStr(1, ItemPubDate, "-").ToString();
                                                            if (Conversions.ToInteger(Pos) > 0) {
                                                                DateSplit = Strings.Split(ItemPubDate, "-");
                                                                if (Information.UBound(DateSplit) == 2) {
                                                                    // ItemPubDate = FormatDateTime(KmaEncodeDate(CStr(DateSplit(2) & "/" & DateSplit(1) & "/" & DateSplit(0))), vbLongDate)
                                                                    ItemPubDate = Strings.FormatDateTime(cp.Utils.EncodeDate(DateSplit[1] + "/" + DateSplit[2] + "/" + DateSplit[0]), Constants.vbLongDate);
                                                                }
                                                            }
                                                        }

                                                        break;
                                                    }
                                                case "summary": {
                                                        ItemDescription = ItemNode.InnerText;
                                                        break;
                                                    }
                                                    // Case "enclosure"
                                                    // ReDim Preserve Enclosure(EnclosureCnt)
                                                    // Enclosure(EnclosureCnt).URL = GetXMLAttribute(Found, ItemNode, "url")
                                                    // Enclosure(EnclosureCnt).Type = GetXMLAttribute(Found, ItemNode, "type")
                                                    // Enclosure(EnclosureCnt).Length = GetXMLAttribute(Found, ItemNode, "length")
                                                    // EnclosureCnt = EnclosureCnt + 1
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(ItemPubDate)) {
                                            DateSplit = Strings.Split(ItemPubDate, " ");
                                            if (Information.UBound(DateSplit) > 2) {
                                                ItemPubDate = DateSplit[0] + " " + DateSplit[1] + " " + DateSplit[2] + " " + DateSplit[3];
                                            }
                                            ItemPubDate = Constants.vbCrLf + Constants.vbTab + Constants.vbTab + "<div class=ItemPubDate>" + ItemPubDate + "</div>";
                                        }
                                        if (!string.IsNullOrEmpty(ItemTitle)) {
                                            if (!string.IsNullOrEmpty(ItemLink)) {
                                                ItemTitle = "<a href=\"" + ItemLink + "\" target=_blank>" + ItemTitle + "</a>";
                                            }
                                            ItemTitle = Constants.vbCrLf + Constants.vbTab + Constants.vbTab + "<div class=ItemTitle>" + ItemTitle + "</div>";
                                        }
                                        if (!string.IsNullOrEmpty(ItemDescription)) {
                                            ItemDescription = Constants.vbCrLf + Constants.vbTab + Constants.vbTab + "<div class=ItemDescription>" + ItemDescription + "</div>";
                                        }
                                        // 
                                        EnclosureRow = "";
                                        if (EnclosureCnt > 0) {
                                            var loopTo = EnclosureCnt - 1;
                                            for (Ptr = 0; Ptr <= loopTo; Ptr++) {
                                                EnclosureType[] Enclosure = new EnclosureType[1];
                                                {
                                                    ref var withBlock1 = ref Enclosure[Ptr];
                                                    if (!string.IsNullOrEmpty(withBlock1.URL)) {
                                                        EnclosureRow = EnclosureRow + Constants.vbCrLf + Constants.vbTab + Constants.vbTab + Constants.vbTab + "<div class=ItemEnclosure><a href=\"" + withBlock1.URL + "\">Media</a></div>";
                                                    }
                                                }
                                            }
                                            if (!string.IsNullOrEmpty(EnclosureRow)) {
                                                EnclosureRow = "" + Constants.vbCrLf + Constants.vbTab + Constants.vbTab + "<div class=ItemEnclosureRow>" + EnclosureRow + Constants.vbCrLf + Constants.vbTab + Constants.vbTab + "</div>";


                                            }
                                        }
                                        result = result + Constants.vbCrLf + Constants.vbTab + "<div class=ChannelItem>" + ItemTitle + ItemPubDate + ItemDescription + EnclosureRow + Constants.vbCrLf + Constants.vbTab + "</div>" + "";






                                        StoryCnt = StoryCnt + 1;
                                        if (StoryCnt >= Conversions.ToInteger(MaxStories)) {
                                            exitFor = true;
                                            break;
                                        }

                                        break;
                                    }
                            }

                            if (exitFor) {
                                break;
                            }
                        }
                        if (!string.IsNullOrEmpty(ChannelLink)) {
                            ChannelTitle = "<a href=\"" + ChannelLink + "\" target=_blank>" + ChannelTitle + "</a>";
                        }
                        if (!string.IsNullOrEmpty(ChannelImage)) {
                            ChannelDescription = ChannelImage + ChannelDescription;
                        }
                        result = "" + Constants.vbCrLf + Constants.vbTab + "<div class=ChannelTitle>" + ChannelTitle + "</div>" + Constants.vbCrLf + Constants.vbTab + "<div class=ChannelPubdate>" + ChannelPubDate + "</div>" + Constants.vbCrLf + Constants.vbTab + "<div class=ChannelDescription>" + ChannelDescription + "</div>" + result;



                        result = "" + Constants.vbCrLf + "<div class=RSSQuickClient>" + result + Constants.vbCrLf + "</div>";


                    }
                }

                // 
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
            }
            return result;

            // HandleError
        }
        // 
        // ========================================================================
        // ----- Get an XML nodes attribute based on its name
        // ========================================================================
        // 
        internal string GetXMLAttribute(CPBaseClass cp, bool Found, System.Xml.XmlDocument Node, string Name) {
            string GetXMLAttributeRet = default;
            string result = "";
            // 
            try {
                System.Xml.XmlNode REsultNode;
                string UcaseName;
                // 
                Found = false;
                REsultNode = Node.Attributes.GetNamedItem(Name);
                if (REsultNode is null) {
                    UcaseName = Strings.UCase(Name);
                    foreach (System.Xml.XmlAttribute NodeAttribute in Node.Attributes) {
                        if ((Strings.UCase(NodeAttribute.Name) ?? "") == (UcaseName ?? "")) {
                            GetXMLAttributeRet = NodeAttribute.Value;
                            Found = true;
                            break;
                        }
                    }
                } else {
                    GetXMLAttributeRet = REsultNode.Value;
                    Found = true;
                }
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
            }
            return result;
        }
        // 
        // 
        private string encodeFilename(string Filename) {
            string result = Filename.ToLower().Replace("http://", "").Replace("https://", "").Replace("/", "-");
            foreach (var c in Path.GetInvalidFileNameChars())
                result = result.Replace(Conversions.ToString(c), "");
            return result;
        }
        // 
        // 
        // Public Overrides Function Execute(CP As CPBaseClass) As Object
        // Dim result As String = ""

        // Try
        // Throw New NotImplementedException()
        // Catch ex As Exception

        // End Try
        // Return result
        // End Function
    }
}