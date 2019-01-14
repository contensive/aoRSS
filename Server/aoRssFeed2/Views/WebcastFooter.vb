
Option Strict On
Option Explicit On

Imports Contensive.Addons.Rss
Imports Contensive.Addons.Rss.Controllers
Imports Contensive.BaseClasses

Namespace Views
    '
    Public Class WebcastFooterClass
        Inherits AddonBaseClass
        '
        '=====================================================================================
        ''' <summary>
        ''' Execute the webscase Addon at the bottom of every page that has a PodcastMediaLink doc property
        ''' </summary>
        ''' <param name="CP"></param>
        ''' <returns></returns>
        Public Overrides Function Execute(ByVal CP As CPBaseClass) As Object
            If (Not String.IsNullOrEmpty(CP.Doc.GetText("PodcastMediaLink"))) Then
                Return CP.Utils.ExecuteAddon("Webcast Add-on")
            End If
            Return String.Empty
        End Function
        ''
        'Private Function vb6Code(cp As CPBaseClass) As String
        '    Dim result As String = ""
        '    Try
        '        Const RSSRootNode = "rss"
        '        '
        '        Dim Pos As Long
        '        Dim RSSFilename As String
        '        Dim RSSName As String
        '        Dim CS As Long
        '        Dim ATag As String
        '        Dim Link As String
        '        Dim s As String
        '        Dim Size As Integer
        '        Dim Copy As String
        '        Dim WebcastWidth As Long
        '        Dim WebcastHeight As Long
        '        Dim TestWidth As Long
        '        Dim TestHeight As Long

        '        '
        '        ' RSSLink tag
        '        '
        '        If Not (Main Is Nothing) Then
        '            CS = kmaEncodeInteger(Main.getAddonOption("CSPage", OptionString))
        '            If Main.IsCSOK(CS) Then
        '                Link = Main.GetCSText(CS, "PodcastMediaLink")
        '                Size = Main.GetCSInteger(CS, "PodcastSize")
        '            End If
        '            Call Main.closecs(CS)
        '            If Link <> "" Then
        '                '
        '                ' Call the other addon to pickup javascript, etc.
        '                '
        '                OptionString = "" _
        '        & "Media Link=" & Link _
        '        & vbCrLf & "Media Size=" & Size _
        '        & vbCrLf & "Hide Player=1" _
        '        & vbCrLf & "Auto Start=1"
        '                GetContent = Main.GetAddonContent(0, "Webcast Add-on", OptionString, 1, "", 0, "", 0)
        '            End If
        '        End If
        '    Catch ex As Exception
        '        cp.Site.ErrorReport(ex)
        '    End Try
        '    Return result
        'End Function
    End Class
End Namespace
