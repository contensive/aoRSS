using System;
using System.Linq;
using Contensive.Addons.Rss.Models.Db;
using Contensive.BaseClasses;
using Contensive.Models.Db;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

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
                            doc.Load(Link);
                            string ItemTitle = "";
                            string ItemLink = "";
                            string ItemDescription = "";
                            string ItemPubDate = "";
                            System.Xml.XmlNode RootNode;
                            System.Xml.XmlNode ChannelNode;
                            System.Xml.XmlNode ItemNode;
                            string itemGuid = "";
                            bool isAtom = Strings.LCase(doc.DocumentElement.Name) == "feed";
                            if (isAtom) {
                                // 
                                // atom feed
                                {
                                    var withBlock = doc.DocumentElement;
                                    foreach (System.Xml.XmlNode currentRootNode in withBlock.ChildNodes) {
                                        RootNode = currentRootNode;
                                        switch (Strings.LCase(RootNode.Name) ?? "") {
                                            case "entry": {
                                                    ChannelNode = RootNode;
                                                    ItemTitle = "";
                                                    ItemLink = "";
                                                    ItemDescription = "";
                                                    ItemPubDate = "";
                                                    itemGuid = "";
                                                    foreach (System.Xml.XmlNode currentItemNode in ChannelNode.ChildNodes) {
                                                        ItemNode = currentItemNode;
                                                        switch (Strings.LCase(ItemNode.Name) ?? "") {
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
                                                                    linkType = GetXMLAttribute(CP, isFound, (System.Xml.XmlDocument)ItemNode, "type");
                                                                    if (string.IsNullOrEmpty(ItemLink) & (!isFound | linkType == "text/html")) {
                                                                        ItemLink = GetXMLAttribute(CP, isFound, (System.Xml.XmlDocument)ItemNode, "href");
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
                                                        string Pos = Strings.InStr(1, ItemPubDate, "T").ToString();
                                                        if (Conversions.ToInteger(Pos) > 1) {
                                                            ItemPubDate = Strings.Left(ItemPubDate, Conversions.ToInteger(Pos) - 1);
                                                        }
                                                    }
                                                    if (string.IsNullOrEmpty(itemGuid)) {
                                                        itemGuid = ItemTitle;
                                                    }
                                                    var RSSAggregatorSourceStoryList = DbBaseModel.createList<RSSAggregatorSourceStorieModel>(CP, "(itemGuid=" + CP.Db.EncodeSQLText(itemGuid) + ")and(sourceId=" + sourceId + "))", "");
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

                                                        if (CP.Utils.EncodeDate(SourceStory.pubDate) != Conversions.ToDate("0")) {
                                                            if (SourceStory.pubDate != CP.Utils.EncodeDate(ItemPubDate)) {
                                                                SourceStory.pubDate = Conversions.ToDate(ItemPubDate);
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
                                        switch (Strings.LCase(RootNode.Name) ?? "") {
                                            case "channel": {
                                                    foreach (System.Xml.XmlNode currentChannelNode in RootNode.ChildNodes) {
                                                        ChannelNode = currentChannelNode;
                                                        switch (Strings.LCase(ChannelNode.Name) ?? "") {
                                                            case "item": {
                                                                    ItemTitle = "";
                                                                    ItemLink = "";
                                                                    ItemDescription = "";
                                                                    ItemPubDate = "";
                                                                    itemGuid = "";
                                                                    foreach (System.Xml.XmlNode currentItemNode1 in ChannelNode.ChildNodes) {
                                                                        ItemNode = currentItemNode1;
                                                                        switch (Strings.LCase(ItemNode.Name) ?? "") {
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
                                                                        string[] DateSplit = Strings.Split(ItemPubDate, " ");
                                                                        if (Information.UBound(DateSplit) > 2) {
                                                                            ItemPubDate = DateSplit[1] + " " + DateSplit[2] + " " + DateSplit[3];
                                                                            // ItemPubDate = DateSplit(0) & " " & DateSplit(1) & " " & DateSplit(2) & " " & DateSplit(3)
                                                                        }
                                                                    }
                                                                    if (string.IsNullOrEmpty(itemGuid)) {
                                                                        itemGuid = ItemTitle;
                                                                    }
                                                                    var RSSAggregatorSourceStoryList = DbBaseModel.createList<RSSAggregatorSourceStorieModel>(CP, "(name=" + CP.Db.EncodeSQLText(ItemTitle) + ")and(sourceId=" + sourceId + ")", "");
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
                                                                            SourceStory.pubDate = Conversions.ToDate(ItemPubDate);
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
            // 

        }
        // 
        // clear anything in between and including <style> tags and from description
        // 
        private string clearStyles(CPBaseClass cp, string givenString) {
            string clearStylesRet = default;
            string Result = "";
            // 
            try {
                string output;
                string posStart;
                string posEnd;
                string styles;
                // 
                posStart = Strings.InStr(givenString, "<style>").ToString();
                posEnd = (Strings.InStr(givenString, "</style>") + Strings.Len("</style>") - 1).ToString();
                // 
                if (Conversions.ToInteger(posStart) != 0) {
                    styles = Strings.Mid(givenString, Conversions.ToInteger(posStart), Conversions.ToInteger(posEnd));
                    output = Strings.Replace(givenString, styles, "");
                } else {
                    output = givenString;
                }
                // 
                clearStylesRet = output;
                Result = clearStylesRet;
                // 
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
            }
            return Result;
        }
    }
}