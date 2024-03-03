using Contensive.Models.Db;
using System;

namespace Contensive.Addons.Rss.Models.Db {
    public class RSSAggregatorModel : DbBaseModel {
        //
        //====================================================================================================
        /// <summary>
        /// table definition
        /// </summary>
        public static DbBaseTableMetadataModel tableMetadata { get; } = new DbBaseTableMetadataModel("RSS Aggregators", "aoRSSAggregators", "default", false);
    }
}