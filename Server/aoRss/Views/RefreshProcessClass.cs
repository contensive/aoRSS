using System;
using System.Linq;
using System.Net;
using Contensive.Addons.Rss.Models.Db;
using Contensive.BaseClasses;
using Contensive.Models.Db;

namespace Contensive.Addons.Rss.Views {
    //
    public class RefreshProcessClass : AddonBaseClass {
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
                string linkRel = "";
                string LoopPtr = "";
                var RSSFeedModelList = DbBaseModel.createList<RSSAggregatorSourcesModel>(CP, "id<>0");
                if (RSSFeedModelList.Count() != 0) {
                    foreach (var RSSFeedxml in RSSFeedModelList) {
                        int sourceId = RSSFeedxml.id;
                        string Link = RSSFeedxml.Link;
                        //
                        // Convert the feed to HTML
                        if (!string.IsNullOrEmpty(Link)) {
                            var doc = new System.Xml.XmlDocument();
                            try {
                                doc.Load(Link);
                            } catch (WebException ex) {
                                string shortmsg = "RSS Aggregator error";
                                string longmsg = $"RSS Aggrgator Sources contains a link that fails with [{ex}]";
                                CP.Site.LogWarning(shortmsg, longmsg, shortmsg, shortmsg);
                            }
                            string ItemTitle = "";
                            string ItemLink = "";
                            string ItemDescription = "";
                            string ItemPubDate = "";
                            System.Xml.XmlNode RootNode;
                            System.Xml.XmlNode ChannelNode;
                            System.Xml.XmlNode ItemNode;
                            string itemGuid = "";
                            bool isAtom = string.Equals(doc.DocumentElement.Name, "feed", StringComparison.OrdinalIgnoreCase);
                            if (isAtom) {
                                //
                                // atom feed
                                {
                                    var withBlock = doc.DocumentElement;
                                    foreach (System.Xml.XmlNode currentRootNode in withBlock.ChildNodes) {
                                        RootNode = currentRootNode;
                                        switch ((RootNode.Name ?? "").ToLowerInvariant()) {
                                            case "entry": {
                                                    ChannelNode = RootNode;
                                                    ItemTitle = "";
                                                    ItemLink = "";
                                                    ItemDescription = "";
                                                    ItemPubDate = "";
                                                    itemGuid = "";
                                                    foreach (System.Xml.XmlNode currentItemNode in ChannelNode.ChildNodes) {
                                                        ItemNode = currentItemNode;
                                                        switch ((ItemNode.Name ?? "").ToLowerInvariant()) {
                                                            case "id": {
                                                                    itemGuid = ItemNode.InnerText;
                                                                    break;
                                                                }
                                                            case "title": {
                                                                    ItemTitle = ItemNode.InnerText;
                                                                    break;
                                                                }
                                                            case "content": {
                                                                    ItemDescription = ItemNode.InnerText;
                                                                    //
                                                                    // clear any styles out of the description
                                                                    //
                                                                    ItemDescription = ItemDescription;
                                                                    break;
                                                                }
                                                            case "link": {

                                                                    string linkType;
                                                                    bool isFound = false;
                                                                    linkType = GetXMLAttribute(CP, isFound, ItemNode, "type");
                                                                    if (string.IsNullOrEmpty(ItemLink) & (!isFound | linkType == "text/html")) {
                                                                        ItemLink = GetXMLAttribute(CP, isFound, ItemNode, "href");
                                                                    }

                                                                    break;
                                                                }
                                                            case "updated": {
                                                                    ItemPubDate = ItemNode.InnerText;
                                                                    break;
                                                                }
                                                        }
                                                    }
                                                    if (!string.IsNullOrEmpty(ItemPubDate)) {
                                                        int tPos = ItemPubDate.IndexOf("T", StringComparison.OrdinalIgnoreCase);
                                                        if (tPos > 0) {
                                                            ItemPubDate = ItemPubDate.Substring(0, tPos);
                                                        }
                                                    }
                                                    if (string.IsNullOrEmpty(itemGuid)) {
                                                        itemGuid = ItemTitle;
                                                    }
                                                    var RSSAggregatorSourceStoryList = DbBaseModel.createList<RSSAggregatorSourceStorieModel>(CP, $"(itemGuid={CP.Db.EncodeSQLText(itemGuid)})and(sourceId={sourceId}))", "");
                                                    if (RSSAggregatorSourceStoryList is null) {
                                                        var SourceStory = DbBaseModel.addDefault<RSSAggregatorSourceStorieModel>(CP);
                                                        SourceStory.pubDate = DateTime.Now;
                                                        SourceStory.sourceId = sourceId;
                                                        SourceStory.itemGuid = itemGuid;

                                                    }
                                                    if (RSSAggregatorSourceStoryList is not null) {
                                                        var SourceStory = RSSAggregatorSourceStoryList.First();
                                                        if ((SourceStory.name ?? "") != (ItemTitle ?? "")) {
                                                            SourceStory.name = ItemTitle;
                                                        }
                                                        if ((SourceStory.description ?? "") != (ItemDescription ?? "")) {
                                                            SourceStory.description = ItemDescription;
                                                        }
                                                        if ((SourceStory.link ?? "") != (ItemLink ?? "")) {
                                                            SourceStory.link = ItemLink;
                                                        }

                                                        if (CP.Utils.EncodeDate(SourceStory.pubDate) != DateTime.MinValue) {
                                                            if (SourceStory.pubDate != CP.Utils.EncodeDate(ItemPubDate)) {
                                                                DateTime.TryParse(ItemPubDate, out DateTime parsedDate);
                                                                SourceStory.pubDate = parsedDate;
                                                            }
                                                        }
                                                        SourceStory.save(CP);
                                                    }

                                                    break;
                                                }
                                        }
                                    }
                                }
                            } else {
                                //
                                // RSS
                                {
                                    var withBlock1 = doc.DocumentElement;
                                    foreach (System.Xml.XmlNode currentRootNode1 in withBlock1.ChildNodes) {
                                        RootNode = currentRootNode1;
                                        switch ((RootNode.Name ?? "").ToLowerInvariant()) {
                                            case "channel": {
                                                    foreach (System.Xml.XmlNode currentChannelNode in RootNode.ChildNodes) {
                                                        ChannelNode = currentChannelNode;
                                                        switch ((ChannelNode.Name ?? "").ToLowerInvariant()) {
                                                            case "item": {
                                                                    ItemTitle = "";
                                                                    ItemLink = "";
                                                                    ItemDescription = "";
                                                                    ItemPubDate = "";
                                                                    itemGuid = "";
                                                                    foreach (System.Xml.XmlNode currentItemNode1 in ChannelNode.ChildNodes) {
                                                                        ItemNode = currentItemNode1;
                                                                        switch ((ItemNode.Name ?? "").ToLowerInvariant()) {
                                                                            case "guid": {
                                                                                    itemGuid = ItemNode.InnerText;
                                                                                    break;
                                                                                }
                                                                            case "title": {
                                                                                    ItemTitle = ItemNode.InnerText;
                                                                                    break;
                                                                                }
                                                                            case "description": {
                                                                                    ItemDescription = ItemNode.InnerText;
                                                                                    //
                                                                                    // clear any styles out of the description
                                                                                    //
                                                                                    ItemDescription = ItemDescription;
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
                                                                        }
                                                                    }
                                                                    if (!string.IsNullOrEmpty(ItemPubDate)) {
                                                                        string[] DateSplit = ItemPubDate.Split(' ');
                                                                        if (DateSplit.Length - 1 > 2) {
                                                                            ItemPubDate = $"{DateSplit[1]} {DateSplit[2]} {DateSplit[3]}";
                                                                        }
                                                                    }
                                                                    if (string.IsNullOrEmpty(itemGuid)) {
                                                                        itemGuid = ItemTitle;
                                                                    }
                                                                    var RSSAggregatorSourceStoryList = DbBaseModel.createList<RSSAggregatorSourceStorieModel>(CP, $"(name={CP.Db.EncodeSQLText(ItemTitle)})and(sourceId={sourceId})", "");
                                                                    if (RSSAggregatorSourceStoryList.Count() == 0) {
                                                                        var SourceStory = DbBaseModel.addDefault<RSSAggregatorSourceStorieModel>(CP);
                                                                        SourceStory.pubDate = DateTime.Now;
                                                                        SourceStory.sourceId = sourceId;
                                                                        SourceStory.itemGuid = itemGuid;
                                                                        SourceStory.save(CP);
                                                                        RSSAggregatorSourceStoryList.Add(SourceStory);
                                                                    }
                                                                    if (RSSAggregatorSourceStoryList.Count > 0) {
                                                                        var SourceStory = RSSAggregatorSourceStoryList.First();
                                                                        if ((SourceStory.name ?? "") != (ItemTitle ?? "")) {
                                                                            SourceStory.name = ItemTitle;
                                                                        }
                                                                        if ((SourceStory.description ?? "") != (ItemDescription ?? "")) {
                                                                            SourceStory.description = ItemDescription;
                                                                        }
                                                                        if ((SourceStory.link ?? "") != (ItemLink ?? "")) {
                                                                            SourceStory.link = ItemLink;
                                                                        }
                                                                        if (SourceStory.pubDate != CP.Utils.EncodeDate(ItemPubDate)) {
                                                                            DateTime.TryParse(ItemPubDate, out DateTime parsedDate);
                                                                            SourceStory.pubDate = parsedDate;
                                                                        }
                                                                        SourceStory.save(CP);
                                                                    }

                                                                    break;
                                                                }
                                                        }
                                                    }

                                                    break;
                                                }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                CP.Site.ErrorReport(ex);
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
                //
                Found = false;
                if (Node.Attributes == null) {
                    return result;
                }
                resultNode = Node.Attributes.GetNamedItem(Name);
                if (resultNode is null) {
                    string UcaseName = Name.ToUpperInvariant();
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
            //

        }
        //
        // clear anything in between and including <style> tags and from description
        //
        private string clearStyles(CPBaseClass cp, string givenString) {
            string Result = "";
            //
            try {
                string output;
                //
                int posStart = givenString.IndexOf("<style>", StringComparison.OrdinalIgnoreCase);
                int posEnd = givenString.IndexOf("</style>", StringComparison.OrdinalIgnoreCase);
                //
                if (posStart >= 0 && posEnd >= 0) {
                    posEnd = posEnd + "</style>".Length;
                    string styles = givenString.Substring(posStart, posEnd - posStart);
                    output = givenString.Replace(styles, "");
                } else {
                    output = givenString;
                }
                //
                Result = output;
                //
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
            }
            return Result;
        }
    }
}
