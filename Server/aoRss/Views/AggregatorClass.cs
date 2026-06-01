using System;
using System.Diagnostics;
using Contensive.Addons.Rss.Models.Db;
using Contensive.BaseClasses;
using Contensive.Models.Db;

namespace Contensive.Addons.Rss.Views {
    //
    public class AggregatorClass : AddonBaseClass {
        //
        // =====================================================================================
        /// <summary>
        /// AddonDescription
        /// </summary>
        /// <param name="CP"></param>
        /// <returns></returns>
        public override object Execute(CPBaseClass CP) {
            object ExecuteRet = default;
            string result = "";
            var sw = new Stopwatch();
            sw.Start();
            try {
                string AggregatorName = "";
                string cr1 = "\r\n\t";
                string cr2 = cr1 + "\t";
                string cr3 = cr2 + "\t";
                string cr4 = cr3 + "\t";
                //
                string instanceId = CP.Doc.GetText("instanceId");
                CP.Utils.AppendLog($"First Instance ID={instanceId}");
                long StoryCnt = CP.Utils.EncodeInteger(CP.Doc.GetText("Story count"));
                if (StoryCnt == 0L) {
                    StoryCnt = 5L;
                }
                var RSSAggregator = DbBaseModel.create<RSSAggregatorModel>(CP, instanceId);
                int AggregatorId = 0;
                if (RSSAggregator is not null) {
                    AggregatorId = RSSAggregator.id;
                }
                CP.Utils.AppendLog($"Aggregator ID={AggregatorId}");
                if (AggregatorId == 0 | instanceId is null) {
                    //
                    // Create new aggregator
                    //
                    RSSAggregator = DbBaseModel.addDefault<RSSAggregatorModel>(CP);
                    RSSAggregator.name = $"New RSS Aggregator create on-demand {DateTime.Now}";
                    AggregatorId = RSSAggregator.id;
                    RSSAggregator.ccguid = instanceId;
                    RSSAggregator.save(CP);
                    CP.Utils.AppendLog($"instanceId={instanceId}");
                }
                //
                string cacheName = $"rssAggregator:{AggregatorId}:{StoryCnt}";
                ExecuteRet = CP.Doc.GetText(cacheName);
                if (true) {
                    //
                    //
                    //
                    var storyList = RSSAggregatorSourceStorieModel.createStoryList(CP, AggregatorId);
                    //
                    long Ptr = 1L;
                    string list = "";
                    foreach (var story in storyList) {
                        if (StoryCnt >= Ptr) {
                            string Cell = "";
                            //
                            string Link = story.link;
                            string Copy = (story.name ?? "").Trim();
                            if (!string.IsNullOrEmpty(Copy)) {
                                if (!string.IsNullOrEmpty(Link)) {
                                    Copy = $"<a href=\"{Link}\">{Copy}</a>";
                                }
                                Cell = $"{Cell}{cr3}<h3 class=\"raCaption\">{Copy}</h3>";
                            }
                            CP.Utils.AppendLog($"copy={Copy}");
                            //
                            string pubDate = story.pubDate.ToString();
                            string sourceName = (story.name ?? "").Trim();
                            //
                            string Delimiter = "";
                            if (!string.IsNullOrEmpty(sourceName)) {
                                sourceName = $"<span class=\"raSourceName\">{sourceName}</span>";
                            }
                            if (!string.IsNullOrEmpty(pubDate)) {
                                pubDate = $"<span  class=\"raPubDate\">{pubDate}</span >";
                            }
                            if (!string.IsNullOrEmpty(pubDate) & !string.IsNullOrEmpty(sourceName)) {
                                Delimiter = "<span class=\"raDelimiter\">|</span>";
                            }
                            Copy = sourceName + Delimiter + pubDate;
                            if (!string.IsNullOrEmpty(Copy)) {
                                Cell = $"{Cell}{cr3}<p class=\"raByLine\">{Copy}</p>";
                            }
                            //
                            Copy = (story.description ?? "").Trim();
                            if (!string.IsNullOrEmpty(Copy)) {
                                Cell = $"{Cell}{cr3}<p class=\"raDescription\">{Copy}</p>";
                            }
                            //
                            list = $"{list}{cr2}<ul><li class=\"raItem\">{Cell}{cr2}</li></ul>";

                            //

                            story.save(CP);
                            Ptr = Ptr + 1L;
                        } else {
                            break;
                        }

                    }
                    if (!string.IsNullOrEmpty(list)) {
                        ExecuteRet = $"{cr1}<ul class=\"rssAggregator\">{list}{cr1}</ul>";
                    }
                    result = list;
                }
            } catch (Exception ex) {
                CP.Site.ErrorReport(ex);
            }
            return result;
        }
        //
        // =====================================================================================
        //
        internal string GetXMLAttribute(CPBaseClass cp, bool Found, System.Xml.XmlNode Node, string Name) {
            string result = "";
            try {
                System.Xml.XmlNode resultNode;
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
        }
    }
    //
    // =====================================================================================
    //
    internal class EnclosureType {
        internal string URL;
        public string Type { get; set; }
        public string Length { get; set; }
    }
    //
}
