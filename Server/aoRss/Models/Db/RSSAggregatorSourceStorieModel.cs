
using Contensive.BaseClasses;
using Contensive.Models.Db;
using System;
using System.Collections.Generic;

namespace Contensive.Addons.Rss.Models.Db {
    public class RSSAggregatorSourceStorieModel : DbBaseModel {
        // 
        // ====================================================================================================
        // -- const
        public const string contentName = "RSS Aggregator Source Stories";                   // <------ set content name
        public const string contentTableName = "rssAggregatorSourceStories";            // <------ set to tablename for the primary content (used for cache names)
        private new const string contentDataSource = "default";   // <------ set to datasource if not default
        // 
        // ====================================================================================================
        // -- instance properties
        public string description { get; set; }
        public string itemGuid { get; set; }
        public string link { get; set; }
        public DateTime pubDate { get; set; }
        public int sourceId { get; set; }

        /// <summary>
        /// Create a list of stories for the aggregator
        /// </summary>
        /// <param name="cp"></param>
        /// <param name="AggregatorID">The id of the aggrigator</param>
        /// <returns></returns>
        public static List<RSSAggregatorSourceStorieModel> createStoryList(CPBaseClass cp, int AggregatorID) {
            var result = new List<RSSAggregatorSourceStorieModel>();
            try {
                // result = createList(cp, "(BlogID=" & blogId & ")", "year(dateadded) desc, Month(DateAdded) desc")
                string sql = "select top 100 a.id  from ((aoRSSAggregatorSourceRules r" + " left join aorssaggregatorsources s on s.id=r.SourceID)" + " left join rssaggregatorsourcestories a on a.sourceid=r.SourceID)" + " Where r.AggregatorId = " + AggregatorID + " order by a.pubdate desc,a.id desc";



                result = createList<RSSAggregatorSourceStorieModel>(cp, "(id in (" + sql + "))", "pubdate desc");
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
            }
            return result;
        }

    }
}