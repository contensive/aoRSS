using Contensive.Models.Db;
using System;

namespace Contensive.Addons.Rss.Models.Db {
    public class RSSAggregatorModel : DbBaseModel {
        // 
        // ====================================================================================================
        // -- const
        public const string contentName = "RSS Aggregators";                   // <------ set content name
        public const string contentTableName = "aoRSSAggregators";            // <------ set to tablename for the primary content (used for cache names)
        private new const string contentDataSource = "default";   // <------ set to datasource if not default
    }
}