



using System.Collections.Generic;
using Contensive.BaseClasses;

namespace Contensive.Addons.Rss.Models.Domain {
    public class xxxxxmodelNameGoesHerexxxxx : baseDomainModel {
        // 
        // ====================================================================================================
        // -- const
        private new const string contentDataSource = "default";             // <------ set to datasource if not default
        // 
        // ====================================================================================================
        // -- instance properties
        public string personName { get; set; }
        public string organizationName { get; set; }
        // 
        // ====================================================================================================
        /// <summary>
        /// get a list of objects matching the organizationId
        /// </summary>
        /// <param name="cp"></param>
        /// <param name="organizationId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public static List<xxxxxmodelNameGoesHerexxxxx> createList(CPBaseClass cp, int organizationId, int pageSize = 999999, int pageNumber = 1) {
            string sql = My.Resources.Resources.sampleSql.Replace("{0}", organizationId.ToString());
            return createListFromSql<xxxxxmodelNameGoesHerexxxxx>(cp, sql, pageSize, pageNumber);
        }

    }
}