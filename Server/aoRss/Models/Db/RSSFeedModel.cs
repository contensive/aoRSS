
using Contensive.Models.Db;
using System;

namespace Contensive.Addons.Rss.Models.Db {
    public class RSSFeedModel : DbBaseModel {
        //
        //====================================================================================================
        /// <summary>
        /// table definition
        /// </summary>
        public static DbBaseTableMetadataModel tableMetadata { get; } = new DbBaseTableMetadataModel("RSS Feeds", "ccRSSFeeds", "default", false);
        // 
        // ====================================================================================================
        // -- instance properties
        public string Copyright { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string LogoFilename { get; set; }
        public DateTime RSSDateUpdated { get; set; }
        public string RSSFilename { get; set; }
    }
}