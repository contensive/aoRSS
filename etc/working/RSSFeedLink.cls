VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "RSSLinkClass"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True

Option Explicit
'
'========================================================================
'
'========================================================================
'
'Private Main As ccWeb3.MainClass
Private Csv As Object
Private Main As Object
'
'========================================================================
'   Execute
'========================================================================
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
    Exit Sub
    '
    ' ----- Error Trap
    '
ErrorTrap:
    'Call HandleError("AggrSampleClass", "Init", Err.Number, Err.Source, Err.Description, True, False)
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
        RSSName = Main.GetAggrOption("Feed", Optionstring)
        If RSSName <> "" Then
            CS = Main.OpenCSContent("RSS Feeds", "name=" & KmaEncodeSQLText(RSSName), , , , , "Name,RSSFilename")
            If Main.iscsok(CS) Then
                RSSName = Main.GetcsText(CS, "Name")
                RSSFilename = Main.GetcsText(CS, "RSSFilename")
                If RSSFilename = "" Then
                    GetContent = "<img src=""/cclib/images/IconXML-25x13.gif"" width=25 height=13 border=0 class=""RSSFeedImage"">&nbsp;" & RSSName & "&nbsp;(Coming Soon)"
                Else
                    ATag = "<a href=""http://" & Main.ServerDomain & "/RSS/" & RSSFilename & """>"
                    'ATag = "<a href=""http://" & Main.ServerDomain & "/" & Main.ApplicationName & "/files/RSS/" & RSSFilename & """>"
                    GetContent = "" _
                        & ATag & "<img src=""/cclib/images/IconXML-25x13.gif"" width=25 height=13 border=0 class=""RSSFeedImage""></a>" _
                        & "&nbsp;" _
                        & ATag & RSSName & "</a>"
                End If
                GetContent = "<div class=""RSSFeedLink"">" & GetContent & "</div>"
            End If
            Call Main.closecs(CS)
        End If
    End If
    '
    Exit Function
ErrorTrap:
    'HandleError
End Function
