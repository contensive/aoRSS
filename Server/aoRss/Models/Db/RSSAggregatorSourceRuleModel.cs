using System;

namespace Contensive.Addons.Rss.Models.Db {
    public class RSSAggregatorSourceRuleModel : BaseModel, ICloneable {
        // 
        // ====================================================================================================
        // -- const
        public const string contentName = "RSS Aggregator Source Rules";                   // <------ set content name
        public const string contentTableName = "aoRSSAggregatorSourceRules";            // <------ set to tablename for the primary content (used for cache names)
        private new const string contentDataSource = "default";   // <------ set to datasource if not default
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