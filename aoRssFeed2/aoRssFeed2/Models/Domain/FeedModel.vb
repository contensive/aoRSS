
Option Explicit On
Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Contensive.Addons.aoRssFeed2.Models.Domain
    Public Class FeedModel
        Public Id as Integer
        Public Name As String
        Public Description As String
        Public Link As String
        Public LogoFilename As String
        Public RSSFilename As String
        Public Entries() As EntryType
        Public EntryCnt as Integer
        Public EntrySize as Integer
        Public EntryDateIndex As New Dictionary(Of String, Integer)
    End Class

    Public Class EntryType
        Public Title As String
        Public Description As String
        Public DatePublish As Date
        Public DateExpires As Date
        Public Link As String
        Public ContentID as Integer
        Public TableID as Integer
        Public RecordID as Integer
        Public PodcastMediaLink As String
    End Class
End Namespace
