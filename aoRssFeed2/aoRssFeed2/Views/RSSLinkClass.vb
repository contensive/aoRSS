
Option Strict On
Option Explicit On

Imports System.Linq
Imports Contensive.Addons.aoRssFeed2.Contensive.Addons.aoRssFeed2
Imports Contensive.BaseClasses

Namespace Views
    '
    Public Class RSSLinkClass
        Inherits AddonBaseClass

        Private Optionstring As String
        '
        ' - if nuget references are not there, open nuget command line and click the 'reload' message at the top, or run "Update-Package -reinstall" - close/open
        ' - Verify project root name space is empty
        ' - Change the namespace (AddonCollectionVb) to the collection name
        ' - Change this class name to the addon name
        ' - Create a Contensive Addon record with the namespace apCollectionName.ad
        '
        '=====================================================================================
        ''' <summary>
        ''' AddonDescription
        ''' </summary>
        ''' <param name="CP"></param>
        ''' <returns></returns>
        Public Overrides Function Execute(ByVal CP As CPBaseClass) As Object
            Dim result As String = ""

            Try
                '.Set Csv = CsvObj
                'Call Init(MainObj)
                result = GetContent(CP, Optionstring) '

            Catch ex As Exception
                CP.Site.ErrorReport(ex)
            End Try
            Return result
        End Function
        '
        '
        'Public Sub Init(MainObject As Object)
        'Set Main = MainObject
        'Exit Sub
        '    '
        '    ' ----- Error Trap
        '    '

        '    'Call HandleError("AggrSampleClass", "Init", Err.Number, Err.Source, Err.Description, True, False)
        'End Sub
        '
        '=================================================================================
        '   Aggregate Object Interface
        '=================================================================================
        '
        Public Function GetContent(cp As CPBaseClass, Optionstring As String) As String
            Dim result As String = ""
            Try
                '
                Const RSSRootNode = "rss"
                '
                Dim RSSFilename As String
                Dim RSSName As String
                Dim CS As Integer
                Dim ATag As String
                Dim IsList As Boolean
                '
                ' RSSLink tag
                '
                RSSName = cp.Doc.GetText("Feed")
                If RSSName <> "" Then
                    Dim rssfeedList As List(Of Models.RSSFeedModel) = Models.RSSFeedModel.createList(cp, "name=" & cp.Db.EncodeSQLText(RSSName))
                    'CS = Main.OpenCSContent("RSS Feeds", "name=" & KmaEncodeSQLText(RSSName), , , , , "Name,RSSFilename")
                    If (rssfeedList IsNot Nothing) Then
                        Dim rssfeed As Models.RSSFeedModel = rssfeedList.First
                        RSSName = rssfeed.name 'main.GetCSText(CS, "Name")
                        RSSFilename = rssfeed.RSSFilename 'Main.GetCSText(CS, "RSSFilename")
                        If RSSFilename = "" Then
                            GetContent = "<img src=""/cclib/images/IconXML-25x13.gif"" width=25 height=13 border=0 class=""RSSFeedImage"">&nbsp;" & RSSName & "&nbsp;(Coming Soon)"
                        Else
                            ATag = "<a href=""http://" & cp.Site.Domain & "/RSS/" & RSSFilename & """>"
                            'ATag = "<a href=""http://" & Main.ServerDomain & "/" & Main.ApplicationName & "/files/RSS/" & RSSFilename & """>"
                            GetContent = "" _
                            & ATag & "<img src=""/cclib/images/IconXML-25x13.gif"" width=25 height=13 border=0 class=""RSSFeedImage""></a>" _
                            & "&nbsp;" _
                            & ATag & RSSName & "</a>"
                        End If
                        GetContent = "<div class=""RSSFeedLink"">" & GetContent & "</div>"
                    End If

                End If
                result = GetContent
                '
            Catch ex As Exception
                cp.Site.ErrorReport(ex)
            End Try
            Return result
        End Function
    End Class
End Namespace
