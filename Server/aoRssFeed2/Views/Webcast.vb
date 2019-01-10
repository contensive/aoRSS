
Option Strict On
Option Explicit On

Imports Contensive.Addons.aoRssFeed2
Imports Contensive.Addons.aoRssFeed2.Controllers
Imports Contensive.BaseClasses

Namespace Views
    '
    Public Class WebcastClass
        Inherits AddonBaseClass
        '
        '=====================================================================================
        ''' <summary>
        ''' AddonDescription
        ''' </summary>
        ''' <param name="CP"></param>
        ''' <returns></returns>
        Public Overrides Function Execute(ByVal CP As CPBaseClass) As Object
            Dim result As String = ""
            Dim sw As New Stopwatch : sw.Start()
            Try
                '
                Using ae As New applicationController(CP, False)
                    '
                    result = vb6Code(CP)
                End Using
            Catch ex As Exception
                CP.Site.ErrorReport(ex)
            End Try
            Return result
        End Function
        '
        Private Function vb6Code(cp As CPBaseClass) As String
            Dim getContent As String = ""
            Try
                '
                Const RSSRootNode = "rss"
                Const cr1 = vbCrLf & vbTab
                Const cr2 = cr1 & vbTab
                Const cr3 = cr2 & vbTab
                Const cr4 = cr3 & vbTab
                '
                Dim Pos As Long
                Dim RSSFilename As String
                Dim RSSName As String
                Dim CS As Long
                Dim ATag As String
                Dim Link As String
                Dim s As String
                Dim Size As Integer
                Dim Copy As String
                Dim WebcastWidth As Long
                Dim WebcastHeight As Long
                Dim TestWidth As Long
                Dim TestHeight As Long
                Dim SizeString As String
                Dim HidePlayer As Boolean
                Dim HidePlayerJS As String
                Dim ShowPlayerJS As String
                Dim Instance As String
                Dim AutoStart As Boolean
                Dim AutoStartString As Boolean
                Dim loopMode As Boolean
                Dim posterUrl As String
                Dim player As String
                '
                Link = Main.GetAggrOption("Media Link", OptionString)
                SizeString = LCase(Main.GetAggrOption("Media Size", OptionString))
                HidePlayer = kmaEncodeBoolean(Main.GetAggrOption("Hide Player", OptionString))
                AutoStart = kmaEncodeBoolean(Main.GetAggrOption("Auto Start", OptionString))
                If Link <> "" Then
                    Instance = GetRandomInteger
                    '
                    ' determine size
                    '
                    Select Case SizeString
                        Case "small-160x120"
                            WebcastWidth = 160
                            WebcastHeight = 120
                        Case "medium-320x240"
                            WebcastWidth = 320
                            WebcastHeight = 240
                        Case "large-640x480"
                            WebcastWidth = 640
                            WebcastHeight = 480
                        Case Else
                            WebcastWidth = 320
                            WebcastHeight = 240
                    End Select
                    '
                    ' ----- Webcast Atachments to the bottom
                    '
                    '
                    player = "" _
                        & cr2 & "<div class=""PlayerContainer"" id=""Player" & Instance & """ style=""display:none;width:" & WebcastWidth & ";height:" & WebcastHeight & ";"">" _
                        & cr3 & "<video" _
                        & " autoplay=""" & getTrueFalse(AutoStart) & """" _
                        & " controls=""true""" _
                        & " height=""" & WebcastHeight & """" _
                        & " width=""" & WebcastWidth & """" _
                        & " loop=""" & getTrueFalse(loopMode) & """" _
                        & " muted=""false""" _
                        & " poster=""" & posterUrl & """" _
                        & " preload=""auto""" _
                        & " src=""" & Link & """" _
                        & "/>" _
                        & cr2 & "</div>" _
                        & ""

                    'player = "<div class=""PlayerContainer"" id=""Player" & Instance & """ style=""display:none;width:" & WebcastWidth & ";height:" & WebcastHeight & ";"">&nbsp;</div>"
                    ShowPlayerJS = "ShowWebcastPlayer('" & Instance & "','" & Link & "'," & WebcastWidth & "," & WebcastHeight & ",'" & AutoStart & "')"
                    HidePlayerJS = "HideWebcastPlayer('" & Instance & "','" & Link & "'," & WebcastWidth & "," & WebcastHeight & ",'" & AutoStart & "')"
                    s = s & cr1 & "<div class=""WebcastContainer"" style=""height:" & WebcastHeight & "px;width:" & WebcastWidth & "px;"">"
                    s = s & player
                    s = s & cr2 & "<div class=""ToolsContainer"">"
                    s = s & cr3 & "&nbsp;Webcast:&nbsp;"
                    If HidePlayer Then
                        s = s & "<a ID=""Show" & Instance & """ class=""Show"" style=""display:inline;"" href=""#"" onclick=""javascript:" & ShowPlayerJS & "; return false;"">Play Now</a>"
                        s = s & "<a ID=""Hide" & Instance & """ class=""Hide"" style=""display:none;"" href=""#"" onclick=""javascript:" & HidePlayerJS & "; return false;"">Hide Player</a>"
                    Else
                        s = s & "<a ID=""Show" & Instance & """ class=""Show"" style=""display:none;"" href=""#"" onclick=""javascript:" & ShowPlayerJS & "; return false;"">Play Now</a>"
                        s = s & "<a ID=""Hide" & Instance & """ class=""Hide"" style=""display:inline;"" href=""#"" onclick=""javascript:" & HidePlayerJS & "; return false;"">Hide Player</a>"
                        Call Main.AddOnLoadJavascript(ShowPlayerJS)
                    End If
                    s = s & "&nbsp;|&nbsp;<a class=""Popup"" href=""#"" onclick=""javascript: ShowWebcastPopupPlayer('" & Instance & "', '" & Link & "'," & WebcastWidth & "," & WebcastHeight & "); return false;"">Play in Popup</a>"
                    s = s & "&nbsp;|&nbsp;<a class=""Download"" href=""" & Link & """ target=""new"">Download</a>"
                    s = s & cr2 & "</div>" 'toolscontainer
                    s = s & cr1 & "</div>" 'webcastcontainer
                    getContent = s
                End If
            Catch ex As Exception
                cp.Site.ErrorReport(ex)
            End Try
            Return result
        End Function
    End Class
End Namespace
