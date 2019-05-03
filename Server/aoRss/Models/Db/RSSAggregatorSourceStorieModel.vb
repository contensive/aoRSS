
Option Explicit On
Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Models.Db
    Public Class RSSAggregatorSourceStorieModel
        Inherits BaseModel
        Implements ICloneable
        '
        '====================================================================================================
        '-- const
        Public Const contentName As String = "RSS Aggregator Source Stories"                   '<------ set content name
        Public Const contentTableName As String = "rssAggregatorSourceStories"            '<------ set to tablename for the primary content (used for cache names)
        Private Shadows Const contentDataSource As String = "default"   '<------ set to datasource if not default
        '
        '====================================================================================================
        ' -- instance properties
        Public Property description As String
        Public Property itemGuid As String
        Public Property link As String
        Public Property pubDate As Date
        Public Property sourceId As Integer                       '<------ replace this with a list all model fields not part of the base model
        ''
        ''====================================================================================================
        'Public Overloads Shared Function add(cp As CPBaseClass) As RSSAggregatorSourceStorieModel
        '    Return add(Of RSSAggregatorSourceStorieModel)(cp)
        'End Function
        ''
        ''====================================================================================================
        'Public Overloads Shared Function create(cp As CPBaseClass, recordId As Integer) As RSSAggregatorSourceStorieModel
        '    Return create(Of RSSAggregatorSourceStorieModel)(cp, recordId)
        'End Function
        ''
        ''====================================================================================================
        'Public Overloads Shared Function create(cp As CPBaseClass, recordGuid As String) As RSSAggregatorSourceStorieModel
        '    Return create(Of RSSAggregatorSourceStorieModel)(cp, recordGuid)
        'End Function
        ''
        ''====================================================================================================
        'Public Overloads Shared Function createByName(cp As CPBaseClass, recordName As String) As RSSAggregatorSourceStorieModel
        '    Return createByName(Of RSSAggregatorSourceStorieModel)(cp, recordName)
        'End Function
        ''
        ''====================================================================================================
        'Public Overloads Sub save(cp As CPBaseClass)
        '    MyBase.save(cp)
        'End Sub
        ''
        ''====================================================================================================
        'Public Overloads Shared Sub delete(cp As CPBaseClass, recordId As Integer)
        '    delete(Of RSSAggregatorSourceStorieModel)(cp, recordId)
        'End Sub
        ''
        ''====================================================================================================
        'Public Overloads Shared Sub delete(cp As CPBaseClass, ccGuid As String)
        '    delete(Of RSSAggregatorSourceStorieModel)(cp, ccGuid)
        'End Sub
        ''
        ''====================================================================================================
        'Public Overloads Shared Function createList(cp As CPBaseClass, sqlCriteria As String, Optional sqlOrderBy As String = "id") As List(Of RSSAggregatorSourceStorieModel)
        '    Return createList(Of RSSAggregatorSourceStorieModel)(cp, sqlCriteria, sqlOrderBy)
        'End Function
        ''
        ''====================================================================================================
        'Public Overloads Shared Function getRecordName(cp As CPBaseClass, recordId As Integer) As String
        '    Return BaseModel.getRecordName(Of RSSAggregatorSourceStorieModel)(cp, recordId)
        'End Function
        ''
        ''====================================================================================================
        'Public Overloads Shared Function getRecordName(cp As CPBaseClass, ccGuid As String) As String
        '    Return BaseModel.getRecordName(Of RSSAggregatorSourceStorieModel)(cp, ccGuid)
        'End Function
        ''
        ''====================================================================================================
        'Public Overloads Shared Function getRecordId(cp As CPBaseClass, ccGuid As String) As Integer
        '    Return BaseModel.getRecordId(Of RSSAggregatorSourceStorieModel)(cp, ccGuid)
        'End Function
        ''
        ''====================================================================================================
        'Public Overloads Shared Function getCount(cp As CPBaseClass, sqlCriteria As String) As Integer
        '    Return BaseModel.getCount(Of RSSAggregatorSourceStorieModel)(cp, sqlCriteria)
        'End Function
        ''
        ''====================================================================================================
        'Public Overloads Function getUploadPath(fieldName As String) As String
        '    Return MyBase.getUploadPath(Of RSSAggregatorSourceStorieModel)(fieldName)
        'End Function
        ''
        ''====================================================================================================
        ''
        'Public Function Clone(cp As CPBaseClass) As RSSAggregatorSourceStorieModel
        '    Dim result As RSSAggregatorSourceStorieModel = DirectCast(Me.Clone(), RSSAggregatorSourceStorieModel)
        '    result.id = cp.Content.AddRecord(contentName)
        '    result.ccguid = cp.Utils.CreateGuid()
        '    result.save(cp)
        '    Return result
        'End Function
        ''
        ''====================================================================================================
        ''
        'Public Function Clone() As Object Implements ICloneable.Clone
        '    Return Me.MemberwiseClone()
        'End Function
        ''' <summary>
        ''' Create a list of stories for the aggregator
        ''' </summary>
        ''' <param name="cp"></param>
        ''' <param name="AggregatorID">The id of the aggrigator</param>
        ''' <returns></returns>
        Public Shared Function createStoryList(cp As CPBaseClass, AggregatorID As Integer) As List(Of RSSAggregatorSourceStorieModel)
            Dim result As New List(Of RSSAggregatorSourceStorieModel)
            Try
                'result = createList(cp, "(BlogID=" & blogId & ")", "year(dateadded) desc, Month(DateAdded) desc")
                Dim sql = "select top 100 a.id  from ((aoRSSAggregatorSourceRules r" _
                                & " left join aorssaggregatorsources s on s.id=r.SourceID)" _
                                & " left join rssaggregatorsourcestories a on a.sourceid=r.SourceID)" _
                                & " Where r.AggregatorId = " & AggregatorID _
                                & " order by a.pubdate desc,a.id desc"
                result = BaseModel.createList(Of RSSAggregatorSourceStorieModel)(cp, "(id in (" & sql & "))", "pubdate desc")
            Catch ex As Exception
                cp.Site.ErrorReport(ex)
            End Try
            Return result
        End Function

    End Class
End Namespace
