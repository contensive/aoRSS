VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "RSSFeedLinkClass"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True

Option Explicit
'
'========================================================================
'   This is a compatibility object. Not advertised/
'========================================================================
'
Private RSSLink As New RSSLinkClass
'
'========================================================================
'   Execute
'========================================================================
'
Public Function execute(CsvObj As Object, MainObj As Object, Optionstring As String, ignore As String) As String
    execute = RSSLink.execute(CsvObj, MainObj, Optionstring, "")
End Function
'
'========================================================================
'   Init()
'========================================================================
'
Public Sub Init(MainObject As Object)
    Call RSSLink.Init(MainObject)
End Sub
'
'=================================================================================
'   Aggregate Object Interface
'=================================================================================
'
Public Function GetContent(Optionstring As String) As String
    GetContent = RSSLink.GetContent(Optionstring)
End Function

