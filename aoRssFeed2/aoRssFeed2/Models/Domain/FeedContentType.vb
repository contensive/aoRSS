
Option Explicit On
Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Contensive.Addons.aoRssFeed2.Models.Domain     '<------ set namespace
    Public Class FeedContentType
        Public ContentID As Integer
        Public TableID As Integer
        Public ManyToManyRuleContent As String
        Public ManyToManyRulePrimaryField As String
        Public ManyToManyRuleSecondaryField As String
    End Class
End Namespace
