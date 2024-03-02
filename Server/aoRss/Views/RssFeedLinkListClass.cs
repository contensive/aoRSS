using System;
using Contensive.Addons.Rss.Controllers;
using Contensive.BaseClasses;
using Contensive.Models.Db;

namespace Contensive.Addons.Rss.Views {
    // 
    public class RssFeedLinkListClass : AddonBaseClass {
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
                var rssfeedList = DbBaseModel.createList<Models.Db.RSSFeedModel>(CP, "", "id desc");
                if (rssfeedList.Count == 0) {
                    result = CP.Html.p("There are currently no public RSS Feeds available.");
                } else {
                    string liList = "";
                    foreach (Models.Db.RSSFeedModel rssfeed in rssfeedList)
                        liList += CP.Html.li(GenericController.getFeedLink(CP, rssfeed), "", "RSSFeedListItem");
                    result = CP.Html.ul(liList, "", "RSSFeedList");
                    result = CP.Html.div(result, "", "RSSFeedWrapper");
                }
            } catch (Exception ex) {
                CP.Site.ErrorReport(ex);
            }
            return result;
        }
    }
}