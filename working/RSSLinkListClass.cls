VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "RSSLinkListClass"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True

Option Explicit
'Private Main As ccWeb3.MainClass
Private Csv As Object
Private Main As Object

'
'=================================================================================
'   Aggregate Object Interface
'=================================================================================
'
Public Function execute(CsvObj As Object, MainObj As Object, Optionstring As String, ignore As String) As String
    Set Csv = CsvObj
    Call Init(MainObj)
    execute = GetContent(Optionstring)
End Function
'
'========================================================================
'   Init()
'========================================================================
'
Public Sub Init(MainObject As Object)
    Set Main = MainObject
End Sub
'
'=================================================================================
'   Aggregate Object Interface
'=================================================================================
'
Public Function GetContent(Optionstring As String) As String
    On Error GoTo ErrorTrap
    '
    Const RSSRootNode = "rss"
    '
    Dim RSSFilename As String
    Dim RSSName As String
    Dim CS As Long
    Dim ATag As String
    Dim IsList As Boolean
    '
    ' RSSLink tag
    '
    If Not (Main Is Nothing) Then
        CS = Main.OpenCSContent("RSS Feeds", , , , , , "Name,RSSFilename")
        IsList = True
        GetContent = GetContent & vbCrLf & vbTab & "<ul class=""RSSFeedList"">"
        Do While Main.iscsok(CS)
            RSSName = Main.GetcsText(CS, "Name")
            RSSFilename = Main.GetcsText(CS, "RSSFilename")
            GetContent = GetContent & vbCrLf & vbTab & vbTab & "<li class=""RSSFeedListItem"">"
            If RSSFilename = "" Then
                GetContent = "<img src=""/cclib/images/IconXML-25x13.gif"" width=25 height=13 border=0 class=""RSSFeedImage"">&nbsp;" & RSSName & "&nbsp;(Coming Soon)"
            Else
                ATag = "<a class=""RSSFeedLink"" href=""http://" & Main.ServerDomain & "/RSS/" & RSSFilename & """>"
                GetContent = GetContent & "<img src=""/cclib/images/IconXML-25x13.gif"" width=25 height=13 border=0 class=""RSSFeedImage"">&nbsp;" & ATag & RSSName & "</a>"
            End If
            GetContent = GetContent & "</li>"
            Call Main.nextcsrecord(CS)
        Loop
        Call Main.closecs(CS)
        GetContent = GetContent & vbCrLf & "</ul>"
    End If
    '
    Exit Function
ErrorTrap:
    'HandleError
End Function

