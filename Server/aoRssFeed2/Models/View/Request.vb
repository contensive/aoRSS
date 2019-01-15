

Option Explicit On
Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Models.View
    Public Class RequestModel
        '
        '====================================================================================================
        '
        Private cp As CPBaseClass
        ''' <summary>
        ''' string that represents the guid of the blog record to be displayed
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property instanceId As String
            Get
                If (_instanceId Is Nothing) Then
                    _instanceId = cp.Doc.GetText("instanceId")
                    If (String.IsNullOrWhiteSpace(_instanceId)) Then _instanceId = "RssClientWithoutinstanceId-PageId-" & cp.Doc.PageId
                End If
                Return _instanceId
            End Get
        End Property
        Private _instanceId As String = Nothing
        ''' <summary>
        ''' Sample request property
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property sampleProperty As Integer
            Get
                If (_sampleProperty Is Nothing) Then
                    _sampleProperty = cp.Doc.GetInteger("sample1")
                End If
                Return Convert.ToInt32(_sampleProperty)
            End Get
        End Property
        Private _sampleProperty As Integer? = Nothing
        '
        ' todo - convert these to ondemand properties
        '
        Public ReadOnly Property sampleQuickProperty As String
            Get
                Return cp.Doc.GetText("sample2")
            End Get
        End Property
        '
        '
        '
        Public Sub New(cp As CPBaseClass)
            Me.cp = cp
        End Sub
    End Class
End Namespace
