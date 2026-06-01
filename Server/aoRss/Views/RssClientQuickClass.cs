using Contensive.Addons.Rss.Models.Db;
using Contensive.Addons.Rss.Models.View;
using Contensive.BaseClasses;
using Contensive.Models.Db;
using System;
using System.IO;

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
                var rssClient = DbBaseModel.create<RSSClientModel>(CP, request.instanceId);
                if (rssClient is null) {
                    //
                    // -- create default record
                    rssClient = DbBaseModel.addDefault<RSSClientModel>(CP);
                    rssClient.ccguid = request.instanceId;
                    rssClient.name = $"Quick Client created {DateTime.Now}";
                    //
                    // -- pickup either the legacy -wrench' values, or the addon's feature arguments
                    rssClient.url = CP.Doc.GetText("URL").Trim();
                    if (string.IsNullOrWhiteSpace(rssClient.url)) {
                        rssClient.url = "http://www.contensive.com/rss/OpenUp.xml";
                    }
                    rssClient.refreshhours = CP.Utils.EncodeInteger(CP.Doc.GetText("RefreshHours"));
                    if (rssClient.refreshhours == 0) {
                        rssClient.refreshhours = 1;
                    }
                    rssClient.numberOfStories = CP.Utils.EncodeInteger(CP.Doc.GetText("Number of Stories"));
                    if (rssClient.numberOfStories == 0) {
                        rssClient.numberOfStories = 99;
                    }
                    rssClient.save(CP);
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
                            if (cacheLine1.Trim().ToLowerInvariant() == "rss client quick reader") {
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
                        CP.Site.ErrorReport(ex, $"Exception during fetch, rssClient.url [{rssClient.url}]");
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
                        if (string.Equals(withBlock.Name, RSSRootNode, StringComparison.OrdinalIgnoreCase)) {
                            //
                            // RSS Feed
                            //
                            result = GetRSS(CP, doc.InnerXml, rssClient.numberOfStories);
                        } else if (string.Equals(withBlock.Name, AtomRootNode, StringComparison.OrdinalIgnoreCase)) {
                            //
                            // Atom Feed
                            //
                            result = GetAtom(CP, doc.InnerXml, rssClient.numberOfStories);
                        } else {
                            //
                            // Bad Feed
                            //
                            SaveCache = false;
                            if (CP.User.IsAdmin) {
                                result = CP.Html.adminHint($"The RSS Feed [{rssClient.url}] returned an incompatible file.");
                            }
                        }
                    }
                    hint = 70;
                    //
                    // Save this feed into the cache
                    //
                    if (SaveCache) {
                        string FeedHeader = $"RSS Client Quick Reader\r\n{DateTime.Now.ToLongDateString()}";
                        CP.CdnFiles.Save(feedCacheFilename, $"{FeedHeader}\r\n{feedContent}");
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
                CP.Site.ErrorReport(ex, $"hint [{hint}]");
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
                            switch ((RootNode.Name ?? "").ToLowerInvariant()) {
                                case "channel": {
                                        ChannelTitle = "";
                                        ChannelDescription = "";
                                        ChannelLink = "";
                                        ChannelImage = "";
                                        ChannelItem = "";
                                        foreach (System.Xml.XmlNode ChannelNode in RootNode.ChildNodes) {
                                            switch ((ChannelNode.Name ?? "").ToLowerInvariant()) {
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
                                                            switch ((ImageNode.Name ?? "").ToLowerInvariant()) {
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
                                                            NewChannelImage = $"{NewChannelImage}<img class=ChannelImage src=\"{ImageURL}\"";
                                                            if (!string.IsNullOrEmpty(ImageWidth)) {
                                                                NewChannelImage = $"{NewChannelImage} width=\"{ImageWidth}\"";
                                                            }
                                                            if (!string.IsNullOrEmpty(ImageHeight)) {
                                                                NewChannelImage = $"{NewChannelImage} height=\"{ImageHeight}\"";
                                                            }
                                                            if (!string.IsNullOrEmpty(ImageTitle)) {
                                                                NewChannelImage = $"{NewChannelImage} title=\"{ImageTitle}\"";
                                                            }
                                                            NewChannelImage = $"{NewChannelImage} style=\"float:left\" border=0>";
                                                            if (!string.IsNullOrEmpty(ImageLink)) {
                                                                NewChannelImage = $"<a href=\"{ImageLink}\" target=_blank>{NewChannelImage}</a>";
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
                                                            switch ((ItemNode.Name ?? "").ToLowerInvariant()) {
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
                                                                        Enclosure[EnclosureCnt].URL = GetXMLAttribute(cp, Found, ItemNode, "url");
                                                                        Enclosure[EnclosureCnt].Type = GetXMLAttribute(cp, Found, ItemNode, "type");
                                                                        Enclosure[EnclosureCnt].Length = GetXMLAttribute(cp, Found, ItemNode, "length");
                                                                        EnclosureCnt = EnclosureCnt + 1;
                                                                        break;
                                                                    }
                                                            }
                                                        }
                                                        string[] DateSplit;
                                                        if (!string.IsNullOrEmpty(ItemPubDate)) {
                                                            DateSplit = ItemPubDate.Split(' ');
                                                            if (DateSplit.Length - 1 > 2) {
                                                                ItemPubDate = $"{DateSplit[0]} {DateSplit[1]} {DateSplit[2]} {DateSplit[3]}";
                                                            }
                                                            ItemPubDate = $"\r\n\t\t<div class=ItemPubDate>{ItemPubDate}</div>";
                                                        }
                                                        if (!string.IsNullOrEmpty(ItemTitle)) {
                                                            if (!string.IsNullOrEmpty(ItemLink)) {
                                                                ItemTitle = $"<a href=\"{ItemLink}\" target=_blank>{ItemTitle}</a>";
                                                            }
                                                            ItemTitle = $"\r\n\t\t<h3>{ItemTitle}</h3>";
                                                        }
                                                        if (!string.IsNullOrEmpty(ItemDescription)) {
                                                            ItemDescription = $"\r\n\t\t<div class=ItemDescription>{ItemDescription}</div>";
                                                        }
                                                        //
                                                        EnclosureRow = "";
                                                        if (EnclosureCnt > 0) {
                                                            for (Ptr = 0; Ptr <= EnclosureCnt - 1; Ptr++) {
                                                                if (!string.IsNullOrEmpty(Enclosure[Ptr].URL)) {
                                                                    EnclosureRow = $"{EnclosureRow}\r\n\t\t\t<div class=ItemEnclosure><a href=\"{Enclosure[Ptr].URL}\">Media</a></div>";
                                                                }
                                                            }
                                                            if (!string.IsNullOrEmpty(EnclosureRow)) {
                                                                EnclosureRow = $"\r\n\t\t<div class=ItemEnclosureRow>{EnclosureRow}\r\n\t\t</div>";
                                                            }
                                                        }
                                                        result = $"{result}\r\n\t<hr style=\"clear:both\"><div class=ChannelItem>{ItemTitle}{ItemPubDate}{ItemDescription}{EnclosureRow}\r\n\t</div>";

                                                        StoryCnt = StoryCnt + 1;
                                                        break;
                                                    }
                                            }
                                            if (StoryCnt >= MaxStories) {
                                                break;
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(ChannelLink)) {
                                            ChannelTitle = $"<a href=\"{ChannelLink}\" target=_blank>{ChannelTitle}</a>";
                                        }
                                        if (!string.IsNullOrEmpty(ChannelImage)) {
                                            ChannelDescription = ChannelImage + ChannelDescription;
                                        }
                                        result = $"\r\n\t<h2>{ChannelTitle}</h2>\r\n\t<div class=ChannelPubdate>{ChannelPubDate}</div>\r\n\t<div class=ChannelDescription>{ChannelDescription}</div>{result}";

                                        break;
                                    }
                            }
                        }
                        result = $"\r\n<div class=RSSQuickClient>{result}\r\n</div>";
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
        private string GetAtom(CPBaseClass cp, string Feed, int MaxStories) {
            string result = "";
            try {
                //
                var StoryCnt = default(int);
                int Pos;
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
                            switch ((RootNode.Name ?? "").ToLowerInvariant()) {
                                case "updated": {
                                        ChannelPubDate = RootNode.InnerText;
                                        Pos = ChannelPubDate.IndexOf("T", StringComparison.OrdinalIgnoreCase);
                                        if (Pos >= 0) {
                                            ChannelPubDate = ChannelPubDate.Substring(0, Pos);
                                            Pos = ChannelPubDate.IndexOf("-");
                                            if (Pos >= 0) {
                                                DateSplit = ChannelPubDate.Split('-');
                                                if (DateSplit.Length - 1 == 2) {
                                                    var parsedDate = cp.Utils.EncodeDate($"{DateSplit[1]}/{DateSplit[2]}/{DateSplit[0]}");
                                                    ChannelPubDate = parsedDate.ToLongDateString();
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
                                        linkType = GetXMLAttribute(cp, isFound, RootNode, "type");
                                        if (string.Equals(linkType, "text/html", StringComparison.OrdinalIgnoreCase)) {
                                            ChannelLink = GetXMLAttribute(cp, isFound, RootNode, "href");
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
                                            switch ((ImageNode.Name ?? "").ToLowerInvariant()) {
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
                                            NewChannelImage = $"{NewChannelImage}<img class=ChannelImage src=\"{ImageURL}\"";
                                            if (!string.IsNullOrEmpty(ImageWidth)) {
                                                NewChannelImage = $"{NewChannelImage} width=\"{ImageWidth}\"";
                                            }
                                            if (!string.IsNullOrEmpty(ImageHeight)) {
                                                NewChannelImage = $"{NewChannelImage} height=\"{ImageHeight}\"";
                                            }
                                            if (!string.IsNullOrEmpty(ImageTitle)) {
                                                NewChannelImage = $"{NewChannelImage} title=\"{ImageTitle}\"";
                                            }
                                            NewChannelImage = $"{NewChannelImage} style=\"float:left\" border=0>";
                                            if (!string.IsNullOrEmpty(ImageLink)) {
                                                NewChannelImage = $"<a href=\"{ImageLink}\" target=_blank>{NewChannelImage}</a>";
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
                                            switch ((ItemNode.Name ?? "").ToLowerInvariant()) {
                                                case "title": {
                                                        ItemTitle = ItemNode.InnerText;
                                                        break;
                                                    }
                                                case "link": {
                                                        linkType = GetXMLAttribute(cp, isFound, ItemNode, "type");

                                                        if (string.Equals(linkType, "text/html", StringComparison.OrdinalIgnoreCase)) {
                                                            ItemLink = GetXMLAttribute(cp, isFound, ItemNode, "href");
                                                        }

                                                        break;
                                                    }
                                                case "updated": {
                                                        ItemPubDate = ItemNode.InnerText;
                                                        Pos = ItemPubDate.IndexOf("T", StringComparison.OrdinalIgnoreCase);
                                                        if (Pos >= 0) {
                                                            ItemPubDate = ItemPubDate.Substring(0, Pos);
                                                            Pos = ItemPubDate.IndexOf("-");
                                                            if (Pos >= 0) {
                                                                DateSplit = ItemPubDate.Split('-');
                                                                if (DateSplit.Length - 1 == 2) {
                                                                    var parsedDate = cp.Utils.EncodeDate($"{DateSplit[1]}/{DateSplit[2]}/{DateSplit[0]}");
                                                                    ItemPubDate = parsedDate.ToLongDateString();
                                                                }
                                                            }
                                                        }

                                                        break;
                                                    }
                                                case "summary": {
                                                        ItemDescription = ItemNode.InnerText;
                                                        break;
                                                    }
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(ItemPubDate)) {
                                            DateSplit = ItemPubDate.Split(' ');
                                            if (DateSplit.Length - 1 > 2) {
                                                ItemPubDate = $"{DateSplit[0]} {DateSplit[1]} {DateSplit[2]} {DateSplit[3]}";
                                            }
                                            ItemPubDate = $"\r\n\t\t<div class=ItemPubDate>{ItemPubDate}</div>";
                                        }
                                        if (!string.IsNullOrEmpty(ItemTitle)) {
                                            if (!string.IsNullOrEmpty(ItemLink)) {
                                                ItemTitle = $"<a href=\"{ItemLink}\" target=_blank>{ItemTitle}</a>";
                                            }
                                            ItemTitle = $"\r\n\t\t<div class=ItemTitle>{ItemTitle}</div>";
                                        }
                                        if (!string.IsNullOrEmpty(ItemDescription)) {
                                            ItemDescription = $"\r\n\t\t<div class=ItemDescription>{ItemDescription}</div>";
                                        }
                                        //
                                        EnclosureRow = "";
                                        result = $"{result}\r\n\t<div class=ChannelItem>{ItemTitle}{ItemPubDate}{ItemDescription}{EnclosureRow}\r\n\t</div>";

                                        StoryCnt = StoryCnt + 1;
                                        if (StoryCnt >= MaxStories) {
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
                            ChannelTitle = $"<a href=\"{ChannelLink}\" target=_blank>{ChannelTitle}</a>";
                        }
                        if (!string.IsNullOrEmpty(ChannelImage)) {
                            ChannelDescription = ChannelImage + ChannelDescription;
                        }
                        result = $"\r\n\t<div class=ChannelTitle>{ChannelTitle}</div>\r\n\t<div class=ChannelPubdate>{ChannelPubDate}</div>\r\n\t<div class=ChannelDescription>{ChannelDescription}</div>{result}";

                        result = $"\r\n<div class=RSSQuickClient>{result}\r\n</div>";
                    }
                }

                //
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
            }
            return result;
        }
        //
        // ========================================================================
        // ----- Get an XML nodes attribute based on its name
        // ========================================================================
        //
        internal string GetXMLAttribute(CPBaseClass cp, bool Found, System.Xml.XmlNode Node, string Name) {
            string result = "";
            //
            try {
                System.Xml.XmlNode resultNode;
                string UcaseName;
                //
                Found = false;
                if (Node.Attributes == null) {
                    return result;
                }
                resultNode = Node.Attributes.GetNamedItem(Name);
                if (resultNode is null) {
                    UcaseName = Name.ToUpperInvariant();
                    foreach (System.Xml.XmlAttribute NodeAttribute in Node.Attributes) {
                        if (string.Equals(NodeAttribute.Name, UcaseName, StringComparison.OrdinalIgnoreCase)) {
                            result = NodeAttribute.Value;
                            Found = true;
                            break;
                        }
                    }
                } else {
                    result = resultNode.Value;
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
            foreach (char c in Path.GetInvalidFileNameChars()) {
                result = result.Replace(c.ToString(), "");
            }
            return result;
        }
    }
}
