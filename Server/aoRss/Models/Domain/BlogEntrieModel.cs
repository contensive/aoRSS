
using Contensive.Models.Db;
using System;

namespace Contensive.Addons.Rss.Models.Domain {
    public class BlogEntrieModel : DbBaseModel {
        // 
        // ====================================================================================================
        //
        public const string contentName = "Blog Entries";      // <------ set content name
        public const string contentTableName = "ccBlogCopy";   // <------ set to tablename for the primary content (used for cache names)
        private new const string contentDataSource = "default";             // <------ set to datasource if not default
        // 
        // ====================================================================================================
        // -- instance properties
        // instancePropertiesGoHere
        public bool AllowComments { get; set; }
        public bool Approved { get; set; }
        public int articlePrimaryImagePositionId { get; set; }
        public int AuthorMemberID { get; set; }
        public int blogCategoryID { get; set; }
        public int BlogID { get; set; }
        public string Copy { get; set; }
        public string CopyText { get; set; }
        public int EntryID { get; set; }
        public int imageDisplayTypeId { get; set; }
        public string PodcastMediaLink { get; set; }
        public int PodcastSize { get; set; }
        public int primaryImagePositionId { get; set; }
        public DateTime RSSDateExpire { get; set; }
        public DateTime RSSDatePublish { get; set; }
        public string RSSDescription { get; set; }
        public string RSSLink { get; set; }
        public string RSSTitle { get; set; }
        public string TagList { get; set; }
        public int Viewings { get; set; }
    }
}