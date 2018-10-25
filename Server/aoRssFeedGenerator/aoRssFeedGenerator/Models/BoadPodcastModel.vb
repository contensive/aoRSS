
Option Explicit On
Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Models
    Public Class BoadPodcastModel
        Inherits baseModel
        Implements ICloneable
        '
        '====================================================================================================
        '-- const
        Public Const contentName As String = "Boad Podcasts"                   '<------ set content name
        Public Const contentTableName As String = "boadPodcasts"            '<------ set to tablename for the primary content (used for cache names)
        Private Shadows Const contentDataSource As String = "default"   '<------ set to datasource if not default
        '
        '====================================================================================================
        ' -- instance properties
        Public Property catalogItemId As Integer
        Public Property ContentCategoryID As Integer
        Public Property durationtext As String
        Public Property filesize As Integer
        Public Property imageFilename As String
        Public Property isBlack As Boolean
        Public Property isFree As Boolean
        Public Property isNetwork As Boolean
        Public Property isPremium As Boolean
        Public Property iTunesDownloadLink As String
        Public Property lastLinkAlias As String
        Public Property link As String
        Public Property paypalButton As String
        Public Property podcastDate As Date
        Public Property PodcastMediaLink As String
        Public Property RSSDateExpire As Date
        Public Property RSSDatePublish As Date
        Public Property RSSDescription As String
        Public Property RSSLink As String
        Public Property createdBy As String
        '
        '====================================================================================================
        Public Overloads Shared Function add(cp As CPBaseClass) As BoadPodcastModel
            Return add(Of BoadPodcastModel)(cp)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function create(cp As CPBaseClass, recordId As Integer) As BoadPodcastModel
            Return create(Of BoadPodcastModel)(cp, recordId)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function create(cp As CPBaseClass, recordGuid As String) As BoadPodcastModel
            Return create(Of BoadPodcastModel)(cp, recordGuid)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function createByName(cp As CPBaseClass, recordName As String) As BoadPodcastModel
            Return createByName(Of BoadPodcastModel)(cp, recordName)
        End Function
        '
        '====================================================================================================
        Public Overloads Sub save(cp As CPBaseClass)
            MyBase.save(cp)
        End Sub
        '
        '====================================================================================================
        Public Overloads Shared Sub delete(cp As CPBaseClass, recordId As Integer)
            delete(Of BoadPodcastModel)(cp, recordId)
        End Sub
        '
        '====================================================================================================
        Public Overloads Shared Sub delete(cp As CPBaseClass, ccGuid As String)
            delete(Of BoadPodcastModel)(cp, ccGuid)
        End Sub
        '
        '====================================================================================================
        Public Overloads Shared Function createList(cp As CPBaseClass, sqlCriteria As String, Optional sqlOrderBy As String = "id") As List(Of BoadPodcastModel)
            Return createList(Of BoadPodcastModel)(cp, sqlCriteria, sqlOrderBy)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function getRecordName(cp As CPBaseClass, recordId As Integer) As String
            Return baseModel.getRecordName(Of BoadPodcastModel)(cp, recordId)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function getRecordName(cp As CPBaseClass, ccGuid As String) As String
            Return baseModel.getRecordName(Of BoadPodcastModel)(cp, ccGuid)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function getRecordId(cp As CPBaseClass, ccGuid As String) As Integer
            Return baseModel.getRecordId(Of BoadPodcastModel)(cp, ccGuid)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function getCount(cp As CPBaseClass, sqlCriteria As String) As Integer
            Return baseModel.getCount(Of BoadPodcastModel)(cp, sqlCriteria)
        End Function
        '
        '====================================================================================================
        Public Overloads Function getUploadPath(fieldName As String) As String
            Return MyBase.getUploadPath(Of BoadPodcastModel)(fieldName)
        End Function
        '
        '====================================================================================================
        '
        Public Function Clone(cp As CPBaseClass) As BoadPodcastModel
            Dim result As BoadPodcastModel = DirectCast(Me.Clone(), BoadPodcastModel)
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

    End Class
End Namespace
