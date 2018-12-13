
Option Strict On
Option Explicit On

Imports System.Linq
Imports Contensive.Addons.aoRssFeed2.Controllers
Imports Contensive.BaseClasses

Namespace Views
    '
    Public Class RssFeedLinkListClass
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
            Try
                Dim rssfeedList As List(Of Models.Db.RSSFeedModel) = Models.Db.RSSFeedModel.createList(CP, "", "id desc")
                If (rssfeedList.Count = 0) Then
                    result = CP.Html.p("There are currently no public RSS Feeds available.")
                Else
                    Dim liList As String = ""
                    For Each rssfeed As Models.Db.RSSFeedModel In rssfeedList
                        liList += CP.Html.li(genericController.getFeedLink(CP, rssfeed), "", "RSSFeedListItem")
                    Next
                    result = CP.Html.ul(liList, "", "RSSFeedList")
                    result = CP.Html.div(result, "", "RSSFeedWrapper")
                End If
            Catch ex As Exception
                CP.Site.ErrorReport(ex)
            End Try
            Return result
        End Function
    End Class
End Namespace
