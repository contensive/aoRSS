



using Contensive.Models.Db;
using System;

namespace Contensive.Addons.Rss.Models.Db {
    public class RSSAggregatorSourcesModel : DbBaseModel  {
        // 
        // ====================================================================================================
        // -- const
        public const string contentName = "RSS Aggregator Sources";                   // <------ set content name
        public const string contentTableName = "aoRSSAggregatorSources";            // <------ set to tablename for the primary content (used for cache names)
        private new const string contentDataSource = "default";   // <------ set to datasource if not default
        // 
        // ====================================================================================================
        // -- instance properties
        public string Link { get; set; }

    }
}