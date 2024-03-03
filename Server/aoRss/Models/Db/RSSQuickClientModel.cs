
using Contensive.Models.Db;
using System;

namespace Contensive.Addons.Rss.Models.Db {
    public class RSSClientModel : DbBaseModel {
        //
        //====================================================================================================
        /// <summary>
        /// table definition
        /// </summary>
        public static DbBaseTableMetadataModel tableMetadata { get; } = new DbBaseTableMetadataModel("RSS Clients", "ccRssClients", "default", false);
        // 
        // ====================================================================================================
        // -- instance properties
        // 
        public string url { get; set; }
        public int refreshhours { get; set; }
        public int numberOfStories { get; set; }
        // 
    }
}