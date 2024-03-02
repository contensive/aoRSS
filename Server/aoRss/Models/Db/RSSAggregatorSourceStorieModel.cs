



using System;
using System.Collections.Generic;
using Contensive.BaseClasses;

namespace Contensive.Addons.Rss.Models.Db {
    public class RSSAggregatorSourceStorieModel : BaseModel, ICloneable {
        // 
        // ====================================================================================================
        // -- const
        public const string contentName = "RSS Aggregator Source Stories";                   // <------ set content name
        public const string contentTableName = "rssAggregatorSourceStories";            // <------ set to tablename for the primary content (used for cache names)
        private new const string contentDataSource = "default";   // <------ set to datasource if not default
        // 
        // ====================================================================================================
        // -- instance properties
        public string description { get; set; }
        public string itemGuid { get; set; }
        public string link { get; set; }
        public DateTime pubDate { get; set; }
        public int sourceId { get; set; }                       // <------ replace this with a list all model fields not part of the base model
        // '
        // '====================================================================================================
        // Public Overloads Shared Function add(cp As CPBaseClass) As RSSAggregatorSourceStorieModel
        // Return add(Of RSSAggregatorSourceStorieModel)(cp)
        // End Function
        // '
        // '====================================================================================================
        // Public Overloads Shared Function create(cp As CPBaseClass, recordId As Integer) As RSSAggregatorSourceStorieModel
        // Return create(Of RSSAggregatorSourceStorieModel)(cp, recordId)
        // End Function
        // '
        // '====================================================================================================
        // Public Overloads Shared Function create(cp As CPBaseClass, recordGuid As String) As RSSAggregatorSourceStorieModel
        // Return create(Of RSSAggregatorSourceStorieModel)(cp, recordGuid)
        // End Function
        // '
        // '====================================================================================================
        // Public Overloads Shared Function createByName(cp As CPBaseClass, recordName As String) As RSSAggregatorSourceStorieModel
        // Return createByName(Of RSSAggregatorSourceStorieModel)(cp, recordName)
        // End Function
        // '
        // '====================================================================================================
        // Public Overloads Sub save(cp As CPBaseClass)
        // MyBase.save(cp)
        // End Sub
        // '
        // '====================================================================================================
        // Public Overloads Shared Sub delete(cp As CPBaseClass, recordId As Integer)
        // delete(Of RSSAggregatorSourceStorieModel)(cp, recordId)
        // End Sub
        // '
        // '====================================================================================================
        // Public Overloads Shared Sub delete(cp As CPBaseClass, ccGuid As String)
        // delete(Of RSSAggregatorSourceStorieModel)(cp, ccGuid)
        // End Sub
        // '
        // '====================================================================================================
        // Public Overloads Shared Function createList(cp As CPBaseClass, sqlCriteria As String, Optional sqlOrderBy As String = "id") As List(Of RSSAggregatorSourceStorieModel)
        // Return createList(Of RSSAggregatorSourceStorieModel)(cp, sqlCriteria, sqlOrderBy)
        // End Function
        // '
        // '====================================================================================================
        // Public Overloads Shared Function getRecordName(cp As CPBaseClass, recordId As Integer) As String
        // Return BaseModel.getRecordName(Of RSSAggregatorSourceStorieModel)(cp, recordId)
        // End Function
        // '
        // '====================================================================================================
        // Public Overloads Shared Function getRecordName(cp As CPBaseClass, ccGuid As String) As String
        // Return BaseModel.getRecordName(Of RSSAggregatorSourceStorieModel)(cp, ccGuid)
        // End Function
        // '
        // '====================================================================================================
        // Public Overloads Shared Function getRecordId(cp As CPBaseClass, ccGuid As String) As Integer
        // Return BaseModel.getRecordId(Of RSSAggregatorSourceStorieModel)(cp, ccGuid)
        // End Function
        // '
        // '====================================================================================================
        // Public Overloads Shared Function getCount(cp As CPBaseClass, sqlCriteria As String) As Integer
        // Return BaseModel.getCount(Of RSSAggregatorSourceStorieModel)(cp, sqlCriteria)
        // End Function
        // '
        // '====================================================================================================
        // Public Overloads Function getUploadPath(fieldName As String) As String
        // Return MyBase.getUploadPath(Of RSSAggregatorSourceStorieModel)(fieldName)
        // End Function
        // '
        // '====================================================================================================
        // '
        // Public Function Clone(cp As CPBaseClass) As RSSAggregatorSourceStorieModel
        // Dim result As RSSAggregatorSourceStorieModel = DirectCast(Me.Clone(), RSSAggregatorSourceStorieModel)
        // result.id = cp.Content.AddRecord(contentName)
        // result.ccguid = cp.Utils.CreateGuid()
        // result.save(cp)
        // Return result
        // End Function
        // '
        // '====================================================================================================
        // '
        // Public Function Clone() As Object Implements ICloneable.Clone
        // Return Me.MemberwiseClone()
        // End Function
        /// <summary>
        /// Create a list of stories for the aggregator
        /// </summary>
        /// <param name="cp"></param>
        /// <param name="AggregatorID">The id of the aggrigator</param>
        /// <returns></returns>
        public static List<RSSAggregatorSourceStorieModel> createStoryList(CPBaseClass cp, int AggregatorID) {
            var result = new List<RSSAggregatorSourceStorieModel>();
            try {
                // result = createList(cp, "(BlogID=" & blogId & ")", "year(dateadded) desc, Month(DateAdded) desc")
                string sql = "select top 100 a.id  from ((aoRSSAggregatorSourceRules r" + " left join aorssaggregatorsources s on s.id=r.SourceID)" + " left join rssaggregatorsourcestories a on a.sourceid=r.SourceID)" + " Where r.AggregatorId = " + AggregatorID + " order by a.pubdate desc,a.id desc";



                result = createList<RSSAggregatorSourceStorieModel>(cp, "(id in (" + sql + "))", "pubdate desc");
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
            }
            return result;
        }

    }
}