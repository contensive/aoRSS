
namespace Contensive.Addons.Rss.Models.Db {
    public class ContentModel : BaseModel {        // <------ set set model Name and everywhere that matches this string
        // Implements ICloneable
        // 
        // ====================================================================================================
        // -- const
        public const string contentName = "Content";      // <------ set content name
        public const string contentTableName = "ccContent";   // <------ set to tablename for the primary content (used for cache names)
        private new const string contentDataSource = "default";             // <------ set to datasource if not default
        public bool AdminOnly { get; set; }
        public bool AllowAdd { get; set; }
        public bool AllowContentChildTool { get; set; }
        public bool AllowContentTracking { get; set; }
        public bool AllowDelete { get; set; }
        public bool AllowMetaContent { get; set; }
        public bool AllowTopicRules { get; set; }
        public bool AllowWorkflowAuthoring { get; set; }
        public int AuthoringTableID { get; set; }
        public int ContentTableID { get; set; }
        public int DefaultSortMethodID { get; set; }
        public bool DeveloperOnly { get; set; }
        public string DropDownFieldList { get; set; }
        public int EditorGroupID { get; set; }
        public int IconHeight { get; set; }
        public string IconLink { get; set; }
        public int IconSprites { get; set; }
        public int IconWidth { get; set; }
        public int InstalledByCollectionID { get; set; }
        public bool IsBaseContent { get; set; }
        public int ParentID { get; set; }
    }
}