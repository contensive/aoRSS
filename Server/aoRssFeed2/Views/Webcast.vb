
Option Strict On
Option Explicit On

Imports Contensive.Addons.Rss
Imports Contensive.Addons.Rss.Controllers
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
                Dim link As String = CP.Doc.GetText("Media Link")
                If (String.IsNullOrWhiteSpace(link)) Then Return String.Empty
                '
                Dim SizeString As String = CP.Doc.GetText("Media Size").ToLower()
                Dim HidePlayer As Boolean = CP.Doc.GetBoolean("Hide Player")
                Dim AutoStart As Boolean = CP.Doc.GetBoolean("Auto Start")
                Dim Instance As Integer = CP.Utils.GetRandomInteger
                Dim posterUrl As String = ""
                Dim WebcastWidth As Integer = 320
                Dim WebcastHeight As Integer = 240
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
                End Select
                '
                ' -- Webcast Attachments to the bottom
                Dim player As String = "" _
                    & vbCrLf & "<div class=""PlayerContainer"" id=""Player" & Instance & """ style=""display:none;width:" & WebcastWidth & ";height:" & WebcastHeight & ";"">" _
                    & vbCrLf & "<video" _
                    & " autoplay=""" & If(AutoStart, "true", "false") & """" _
                    & " controls=""true""" _
                    & " height=""" & WebcastHeight.ToString() & """" _
                    & " width=""" & WebcastWidth.ToString() & """" _
                    & " loop=""false""" _
                    & " muted=""false""" _
                    & " poster=""" & posterUrl & """" _
                    & " preload=""auto""" _
                    & " src=""" & link & """" _
                    & "/>" _
                    & vbCrLf & "</div>" _
                    & ""

                'player = "<div class=""PlayerContainer"" id=""Player" & Instance & """ style=""display:none;width:" & WebcastWidth & ";height:" & WebcastHeight & ";"">&nbsp;</div>"
                Dim ShowPlayerJS As String = "ShowWebcastPlayer('" & Instance & "','" & link & "'," & WebcastWidth & "," & WebcastHeight & ",'" & AutoStart & "')"
                Dim HidePlayerJS As String = "HideWebcastPlayer('" & Instance & "','" & link & "'," & WebcastWidth & "," & WebcastHeight & ",'" & AutoStart & "')"
                Dim resultHtml As String = vbCrLf & "<div class=""WebcastContainer"" style=""height:" & WebcastHeight & "px;width:" & WebcastWidth & "px;"">"
                resultHtml = resultHtml & player
                resultHtml = resultHtml & vbCrLf & "<div class=""ToolsContainer"">"
                resultHtml = resultHtml & vbCrLf & "&nbsp;Webcast:&nbsp;"
                If HidePlayer Then
                    resultHtml = resultHtml & "<a ID=""Show" & Instance & """ class=""Show"" style=""display:inline;"" href=""#"" onclick=""javascript:" & ShowPlayerJS & "; return false;"">Play Now</a>"
                    resultHtml = resultHtml & "<a ID=""Hide" & Instance & """ class=""Hide"" style=""display:none;"" href=""#"" onclick=""javascript:" & HidePlayerJS & "; return false;"">Hide Player</a>"
                    CP.Doc.AddHeadJavascript(HidePlayerJS)
                Else
                    resultHtml = resultHtml & "<a ID=""Show" & Instance & """ class=""Show"" style=""display:none;"" href=""#"" onclick=""javascript:" & ShowPlayerJS & "; return false;"">Play Now</a>"
                    resultHtml = resultHtml & "<a ID=""Hide" & Instance & """ class=""Hide"" style=""display:inline;"" href=""#"" onclick=""javascript:" & HidePlayerJS & "; return false;"">Hide Player</a>"
                    CP.Doc.AddHeadJavascript(ShowPlayerJS)
                End If
                resultHtml = resultHtml & "&nbsp;|&nbsp;<a class=""Popup"" href=""#"" onclick=""javascript: ShowWebcastPopupPlayer('" & Instance & "', '" & link & "'," & WebcastWidth & "," & WebcastHeight & "); return false;"">Play in Popup</a>"
                resultHtml = resultHtml & "&nbsp;|&nbsp;<a class=""Download"" href=""" & link & """ target=""new"">Download</a>"
                resultHtml = resultHtml & vbCrLf & "</div>"
                resultHtml = resultHtml & vbCrLf & "</div>"
                Return resultHtml
            Catch ex As Exception
                CP.Site.ErrorReport(ex)
                Return String.Empty
            End Try
        End Function
    End Class
End Namespace
