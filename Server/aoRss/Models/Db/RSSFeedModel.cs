



using System;

namespace Contensive.Addons.Rss.Models.Db {
    public class RSSFeedModel : BaseModel, ICloneable {        // <------ set set model Name and everywhere that matches this string
        // 
        // ====================================================================================================
        // -- const
        public const string contentName = "RSS Feeds";      // <------ set content name
        public const string contentTableName = "ccRSSFeeds";   // <------ set to tablename for the primary content (used for cache names)
        private new const string contentDataSource = "default";             // <------ set to datasource if not default
        // 
        // ====================================================================================================
        // -- instance properties
        public string Copyright { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string LogoFilename { get; set; }
        public DateTime RSSDateUpdated { get; set; }
        public string RSSFilename { get; set; }
        // '
        // '====================================================================================================
        // Public Overloads Shared Function add(cp As CPBaseClass) As RSSFeedModel
        // Return add(Of RSSFeedModel)(cp)
        // End Function


    }
}