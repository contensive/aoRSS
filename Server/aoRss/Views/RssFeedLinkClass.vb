



Imports System.Linq
Imports Contensive.Addons.Rss.Controllers
Imports Contensive.Addons.Rss.Models.Db
Imports Contensive.BaseClasses

Namespace Views
    '
    Public Class RssFeedLinkClass
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
                Dim rssName As String = CP.Doc.GetText("Feed")
                If String.IsNullOrWhiteSpace(rssName) Then
                    Dim rssfeedList As List(Of RSSFeedModel) = BaseModel.createList(Of RSSFeedModel)(CP, "name=" & CP.Db.EncodeSQLText(rssName), "")
                    If (rssfeedList.Count > 0) Then
                        Return CP.Html.div(GenericController.getFeedLink(CP, rssfeedList.First), "", "RSSFeedWrapper")
                    End If
                End If
            Catch ex As Exception
                CP.Site.ErrorReport(ex)
            End Try
            Return result
        End Function
    End Class
End Namespace
