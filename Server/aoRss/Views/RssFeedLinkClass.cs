using System;
using System.Linq;
using Contensive.Addons.Rss.Controllers;
using Contensive.Addons.Rss.Models.Db;
using Contensive.BaseClasses;
using Contensive.Models.Db;

namespace Contensive.Addons.Rss.Views {
    // 
    public class RssFeedLinkClass : AddonBaseClass {

        private string Optionstring;
        // 
        // - if nuget references are not there, open nuget command line and click the 'reload' message at the top, or run "Update-Package -reinstall" - close/open
        // - Verify project root name space is empty
        // - Change the namespace (AddonCollectionVb) to the collection name
        // - Change this class name to the addon name
        // - Create a Contensive Addon record with the namespace apCollectionName.ad
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
                string rssName = CP.Doc.GetText("Feed");
                if (string.IsNullOrWhiteSpace(rssName)) {
                    var rssfeedList = DbBaseModel.createList<RSSFeedModel>(CP, "name=" + CP.Db.EncodeSQLText(rssName), "");
                    if (rssfeedList.Count() > 0) {
                        return CP.Html.div(GenericController.getFeedLink(CP, rssfeedList.First()), "", "RSSFeedWrapper");
                    }
                }
            } catch (Exception ex) {
                CP.Site.ErrorReport(ex);
            }
            return result;
        }
    }
}