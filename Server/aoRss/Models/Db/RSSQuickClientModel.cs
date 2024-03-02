
using Contensive.Models.Db;
using System;

namespace Contensive.Addons.Rss.Models.Db {
    public class RSSClientModel : DbBaseModel {
        // 
        // ====================================================================================================
        // -- const
        public const string contentName = "RSS Clients";
        public const string contentTableName = "ccRssClients";
        private new const string contentDataSource = "default";             // <------ set to datasource if not default
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