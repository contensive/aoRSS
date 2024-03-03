using Contensive.Models.Db;
using System;

namespace Contensive.Addons.Rss.Models.Db {
    public class RSSAggregatorSourceRuleModel : DbBaseModel {
        //
        //====================================================================================================
        /// <summary>
        /// table definition
        /// </summary>
        public static DbBaseTableMetadataModel tableMetadata { get; } = new DbBaseTableMetadataModel("RSS Aggregator Source Rules", "aoRSSAggregatorSourceRules", "default", false);
        // 
        // ====================================================================================================
        // -- instance properties
        public int AggregatorID { get; set; }
        public int SourceID { get; set; }
        public string link { get; set; }
        public string articleName { get; set; }
        public string pubdate { get; set; }
        public string description { get; set; }
    }
}