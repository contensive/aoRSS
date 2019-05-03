
Option Explicit On
Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Models.Domain
    Public Class FeedModel
        Public Id As Integer
        Public Name As String
        Public Description As String
        Public Link As String
        Public LogoFilename As String
        Public RSSFilename As String
        Public entryList As List(Of FeedEntryModel)
        'Public EntryCnt As Integer
        'Public EntrySize As Integer
        'Public EntryDateIndex As New Dictionary(Of String, Integer)
    End Class

    Public Class FeedEntryModel
        Public Title As String
        Public Description As String
        Public DatePublish As Date
        Public DateExpires As Date
        Public Link As String
        Public storyContentID As Integer
        'Public storyTableID As Integer
        Public storyRecordID As Integer
        Public PodcastMediaLink As String
    End Class
End Namespace
