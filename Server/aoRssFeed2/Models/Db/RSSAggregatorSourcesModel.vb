﻿
Option Explicit On
Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Models.Db
    Public Class RSSAggregatorSourcesModel
        Inherits BaseModel
        Implements ICloneable
        '
        '====================================================================================================
        '-- const
        Public Const contentName As String = "RSS Aggregator Sources"                   '<------ set content name
        Public Const contentTableName As String = "aoRSSAggregatorSources"            '<------ set to tablename for the primary content (used for cache names)
        Private Shadows Const contentDataSource As String = "default"   '<------ set to datasource if not default
        '
        '====================================================================================================
        ' -- instance properties
        Public Property Link As String                      '<------ replace this with a list all model fields not part of the base model
        ''
        ''====================================================================================================
        'Public Overloads Shared Function add(cp As CPBaseClass) As RSSAggregatorSourcesModel
        '    Return add(Of RSSAggregatorSourcesModel)(cp)
        'End Function
        ''
        ''====================================================================================================
        'Public Overloads Shared Function create(cp As CPBaseClass, recordId As Integer) As RSSAggregatorSourcesModel
        '    Return create(Of RSSAggregatorSourcesModel)(cp, recordId)
        'End Function
        ''
        ''====================================================================================================
        'Public Overloads Shared Function create(cp As CPBaseClass, recordGuid As String) As RSSAggregatorSourcesModel
        '    Return create(Of RSSAggregatorSourcesModel)(cp, recordGuid)
        'End Function
        ''
        ''====================================================================================================
        'Public Overloads Shared Function createByName(cp As CPBaseClass, recordName As String) As RSSAggregatorSourcesModel
        '    Return createByName(Of RSSAggregatorSourcesModel)(cp, recordName)
        'End Function
        ''
        ''====================================================================================================
        'Public Overloads Sub save(cp As CPBaseClass)
        '    MyBase.save(cp)
        'End Sub
        ''
        ''====================================================================================================
        'Public Overloads Shared Sub delete(cp As CPBaseClass, recordId As Integer)
        '    delete(Of RSSAggregatorSourcesModel)(cp, recordId)
        'End Sub
        ''
        ''====================================================================================================
        'Public Overloads Shared Sub delete(cp As CPBaseClass, ccGuid As String)
        '    delete(Of RSSAggregatorSourcesModel)(cp, ccGuid)
        'End Sub
        ''
        ''====================================================================================================
        'Public Overloads Shared Function createList(cp As CPBaseClass, sqlCriteria As String, Optional sqlOrderBy As String = "id") As List(Of RSSAggregatorSourcesModel)
        '    Return createList(Of RSSAggregatorSourcesModel)(cp, sqlCriteria, sqlOrderBy)
        'End Function
        ''
        ''====================================================================================================
        'Public Overloads Shared Function getRecordName(cp As CPBaseClass, recordId As Integer) As String
        '    Return BaseModel.getRecordName(Of RSSAggregatorSourcesModel)(cp, recordId)
        'End Function
        ''
        ''====================================================================================================
        'Public Overloads Shared Function getRecordName(cp As CPBaseClass, ccGuid As String) As String
        '    Return BaseModel.getRecordName(Of RSSAggregatorSourcesModel)(cp, ccGuid)
        'End Function
        ''
        ''====================================================================================================
        'Public Overloads Shared Function getRecordId(cp As CPBaseClass, ccGuid As String) As Integer
        '    Return BaseModel.getRecordId(Of RSSAggregatorSourcesModel)(cp, ccGuid)
        'End Function
        ''
        ''====================================================================================================
        'Public Overloads Shared Function getCount(cp As CPBaseClass, sqlCriteria As String) As Integer
        '    Return BaseModel.getCount(Of RSSAggregatorSourcesModel)(cp, sqlCriteria)
        'End Function
        ''
        ''====================================================================================================
        'Public Overloads Function getUploadPath(fieldName As String) As String
        '    Return MyBase.getUploadPath(Of RSSAggregatorSourcesModel)(fieldName)
        'End Function
        ''
        ''====================================================================================================
        ''
        'Public Function Clone(cp As CPBaseClass) As RSSAggregatorSourcesModel
        '    Dim result As RSSAggregatorSourcesModel = DirectCast(Me.Clone(), RSSAggregatorSourcesModel)
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

    End Class
End Namespace
