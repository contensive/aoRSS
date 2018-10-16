
Option Strict On
Option Explicit On

Imports System.Linq
Imports Contensive.Addons.aoRssFeed2.Contensive.Addons.aoRssFeed2
Imports Contensive.Addons.aoRssFeed2.Controllers
Imports Contensive.BaseClasses

Namespace Views
    '
    Public Class RSSLinkListClass
        Inherits AddonBaseClass

        Public Property GetContent As Object
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
                Dim RSSFilename As String = ""
                Dim RSSName As String = ""
                Dim CS As Integer
                Dim ATag As String = ""
                Dim IsList As Boolean
                Dim GetContent As String = ""
                '
                'RSSLink tag


                'CS = CP.OpenCSContent("RSS Feeds", , , , , , "Name,RSSFilename")
                Dim rssfeedList As List(Of Models.RSSFeedModel) = Models.RSSFeedModel.createList(CP, "id<>" & 0)
                'CS = Main.OpenCSContent("RSS Feeds", "name=" & KmaEncodeSQLText(RSSName), , , , , "Name,RSSFilename")
                If (rssfeedList.Count <> 0) Then
                    Dim rssfeed As Models.RSSFeedModel = rssfeedList.First
                    IsList = True
                    GetContent = GetContent & vbCrLf & vbTab & "<ul class=""RSSFeedList"">"
                    For Each rssfeed In rssfeedList
                        RSSName = rssfeed.name 'Main.GetCSText(CS, "Name")
                        RSSFilename = rssfeed.RSSFilename
                        GetContent = GetContent & vbCrLf & vbTab & vbTab & "<li class=""RSSFeedListItem"">"
                        If RSSFilename = "" Then
                            GetContent = "<img src=""/cclib/images/IconXML-25x13.gif"" width=25 height=13 border=0 class=""RSSFeedImage"">&nbsp;" & RSSName & "&nbsp;(Coming Soon)"
                        Else
                            ATag = "<a class=""RSSFeedLink"" href=""http://" & CP.Site.Domain & "/RSS/" & RSSFilename & """>"
                            GetContent = GetContent & "<img src=""/cclib/images/IconXML-25x13.gif"" width=25 height=13 border=0 class=""RSSFeedImage"">&nbsp;" & ATag & RSSName & "</a>"
                        End If
                        GetContent = GetContent & "</li>"
                    Next
                End If

                GetContent = GetContent & vbCrLf & "</ul>"
                result = GetContent
                '
            Catch ex As Exception
                CP.Site.ErrorReport(ex)
            End Try
            Return result
        End Function
    End Class
End Namespace
