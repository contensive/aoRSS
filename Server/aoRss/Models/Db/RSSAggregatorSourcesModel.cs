



using Contensive.Models.Db;
using System;

namespace Contensive.Addons.Rss.Models.Db {
    public class RSSAggregatorSourcesModel : DbBaseModel {
        //
        //====================================================================================================
        /// <summary>
        /// table definition
        /// </summary>
        public static DbBaseTableMetadataModel tableMetadata { get; } = new DbBaseTableMetadataModel("RSS Aggregator Sources", "aoRSSAggregatorSources", "default", false);
        // 
        // ====================================================================================================
        // -- instance properties
        public string Link { get; set; }

    }
}