
namespace Contensive.Addons.Rss.Models.Db {
    public class ContentFieldModel : BaseModel {        // <------ set set model Name and everywhere that matches this string
        public const string contentName = "Content Fields";      // <------ set content name
        public const string contentTableName = "ccFields";   // <------ set to tablename for the primary content (used for cache names)
        private new const string contentDataSource = "default";             // <------ set to datasource if not default
        public int ContentID { get; set; }
        public int ManyToManyContentID { get; set; }
        public int ManyToManyRuleContentID { get; set; }
        public string ManyToManyRulePrimaryField { get; set; }
        public string ManyToManyRuleSecondaryField { get; set; }
    }
}