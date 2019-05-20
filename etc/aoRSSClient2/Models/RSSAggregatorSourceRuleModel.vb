
Option Explicit On
Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Models
    Public Class RSSAggregatorSourceRuleModel
        Inherits baseModel
        Implements ICloneable
        '
        '====================================================================================================
        '-- const
        Public Const contentName As String = "RSS Aggregator Source Rules"                   '<------ set content name
        Public Const contentTableName As String = "aoRSSAggregatorSourceRules"            '<------ set to tablename for the primary content (used for cache names)
        Private Shadows Const contentDataSource As String = "default"   '<------ set to datasource if not default
        '
        '====================================================================================================
        ' -- instance properties
        Public Property AggregatorID As Integer
        Public Property SourceID As Integer
        Public Property link As String
        Public Property articleName As String
        Public Property pubdate As String
        Public Property description As String
        '<------ replace this with a list all model fields not part of the base model
        '
        '====================================================================================================
        Public Overloads Shared Function add(cp As CPBaseClass) As RSSAggregatorSourceRuleModel
            Return add(Of RSSAggregatorSourceRuleModel)(cp)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function create(cp As CPBaseClass, recordId As Integer) As RSSAggregatorSourceRuleModel
            Return create(Of RSSAggregatorSourceRuleModel)(cp, recordId)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function create(cp As CPBaseClass, recordGuid As String) As RSSAggregatorSourceRuleModel
            Return create(Of RSSAggregatorSourceRuleModel)(cp, recordGuid)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function createByName(cp As CPBaseClass, recordName As String) As RSSAggregatorSourceRuleModel
            Return createByName(Of RSSAggregatorSourceRuleModel)(cp, recordName)
        End Function
        '
        '====================================================================================================
        Public Overloads Sub save(cp As CPBaseClass)
            MyBase.save(cp)
        End Sub
        '
        '====================================================================================================
        Public Overloads Shared Sub delete(cp As CPBaseClass, recordId As Integer)
            delete(Of RSSAggregatorSourceRuleModel)(cp, recordId)
        End Sub
        '
        '====================================================================================================
        Public Overloads Shared Sub delete(cp As CPBaseClass, ccGuid As String)
            delete(Of RSSAggregatorSourceRuleModel)(cp, ccGuid)
        End Sub
        '
        '====================================================================================================
        Public Overloads Shared Function createList(cp As CPBaseClass, sqlCriteria As String, Optional sqlOrderBy As String = "id") As List(Of RSSAggregatorSourceRuleModel)
            Return createList(Of RSSAggregatorSourceRuleModel)(cp, sqlCriteria, sqlOrderBy)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function getRecordName(cp As CPBaseClass, recordId As Integer) As String
            Return baseModel.getRecordName(Of RSSAggregatorSourceRuleModel)(cp, recordId)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function getRecordName(cp As CPBaseClass, ccGuid As String) As String
            Return baseModel.getRecordName(Of RSSAggregatorSourceRuleModel)(cp, ccGuid)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function getRecordId(cp As CPBaseClass, ccGuid As String) As Integer
            Return baseModel.getRecordId(Of RSSAggregatorSourceRuleModel)(cp, ccGuid)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function getCount(cp As CPBaseClass, sqlCriteria As String) As Integer
            Return baseModel.getCount(Of RSSAggregatorSourceRuleModel)(cp, sqlCriteria)
        End Function
        '
        '====================================================================================================
        Public Overloads Function getUploadPath(fieldName As String) As String
            Return MyBase.getUploadPath(Of RSSAggregatorSourceRuleModel)(fieldName)
        End Function
        '
        '====================================================================================================
        '
        Public Function Clone(cp As CPBaseClass) As RSSAggregatorSourceRuleModel
            Dim result As RSSAggregatorSourceRuleModel = DirectCast(Me.Clone(), RSSAggregatorSourceRuleModel)
            result.id = cp.Content.AddRecord(contentName)
            result.ccguid = cp.Utils.CreateGuid()
            result.save(cp)
            Return result
        End Function
        '
        '====================================================================================================
        '
        Public Function Clone() As Object Implements ICloneable.Clone
            Return Me.MemberwiseClone()
        End Function
        ''' <summary>
        '''' Return a list of Archive Blog Copy
        '''' </summary>
        '''' <param name="cp"></param>
        '''' <param name="AggregatorID">The id of the aggrigator</param>
        '''' <returns></returns>
        'Public Shared Function createRssAggregatorCache(cp As CPBaseClass, AggregatorID As Integer) As List(Of RSSAggregatorSourceRuleModel)
        '    Dim result As New List(Of RSSAggregatorSourceRuleModel)
        '    Try
        '        'result = createList(cp, "(BlogID=" & blogId & ")", "year(dateadded) desc, Month(DateAdded) desc")
        '        Dim sql = "select a.id  from ((aoRSSAggregatorSourceRules r" _
        '                        & " left join aorssaggregatorsources s on s.id=r.SourceID)" _
        '                        & " left join rssaggregatorsourcestories a on a.sourceid=r.SourceID)" _
        '                        & " Where r.AggregatorId = " & AggregatorID _
        '                        & " order by a.pubdate desc,a.id desc"
        '        result = createList(cp, "(id in (" & sql & "))")
        '    Catch ex As Exception
        '        cp.Site.ErrorReport(ex)
        '    End Try
        '    Return result
        'End Function
        '''' <summary>
        '''' Return a list of Archive Blog Copy
        '''' </summary>
        '''' <param name="cp"></param>
        '''' <param name="AggregatorID">The id of the aggrigator</param>
        '''' <returns></returns>
        'Public Shared Function createRssAggregatorCache2(cp As CPBaseClass, AggregatorID As Integer) As List(Of RSSAggregatorSourceRuleModel)
        '    Dim result As New List(Of RSSAggregatorSourceRuleModel)
        '    Try
        '        'result = createList(cp, "(BlogID=" & blogId & ")", "year(dateadded) desc, Month(DateAdded) desc")
        '        Dim sql = "select s.name,a.name as articleName, a.pubdate, a.description, a.link  from ((aoRSSAggregatorSourceRules r" _
        '                        & " left join aorssaggregatorsources s on s.id=r.SourceID)" _
        '                        & " left join rssaggregatorsourcestories a on a.sourceid=r.SourceID)" _
        '                        & " Where r.AggregatorId = " & AggregatorID _
        '                        & " order by a.pubdate desc,a.id desc"
        '        result = createList(cp, sql)
        '    Catch ex As Exception
        '        cp.Site.ErrorReport(ex)
        '    End Try
        '    Return result
        'End Function

    End Class
End Namespace
