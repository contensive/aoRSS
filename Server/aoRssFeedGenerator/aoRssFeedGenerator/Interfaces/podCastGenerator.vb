
Option Strict On
Option Explicit On

Imports Contensive.BaseClasses
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Xml
Imports System.Xml.Schema
Imports System.IO
Imports System.Text

Namespace Interfaces
    '
    Public Class podCastGenerator
        Inherits AddonBaseClass
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
                '
                ' -- initialize application. If authentication needed and not login page, pass true
                Using ae As New Controllers.applicationController(CP, False)
                    '
                    ' -- create just one channel
                    Dim channelargument As New RSSFeed.channelArgumentsClass()
                    channelargument.categories.Add("Comedy", "somethingElseWeDontKnow")
                    channelargument.title = "The Big O And Dukes"
                    channelargument.link = "http://www.bigoanddukes.com/home"
                    channelargument.language = "en-us"
                    channelargument.description = "With a 10 year passionate terrestrial radio following, The Big O and Dukes Show has a mind-blowing 4 million downloads to date. This current incarnation is its purest form to date. From pop-culture to politics, each approaches subject matter from very different perspectives. So pull up a seat, plug in your headphones and step into the studio with Chad Dukes, Oscar Santana and Drab T-Shirt. By the end of each show, you\'ll have laughed yourself right out of your chair, but have gained a window into the lives of three new friends."
                    channelargument.copyRight = " &#x2117; &amp; &#xA9; 2011 Big O And Dukes"
                    channelargument.itunessubtitle = "A show about everything"
                    channelargument.itunesauthor = "Big O And Dukes"
                    channelargument.itunessummary = "With a 10 year passionate terrestrial radio following, The Big O and Dukes Show has a mind-blowing 4 million downloads to date. This current incarnation is its purest form to date. From pop-culture to politics, each approaches subject matter from very different perspectives. So pull up a seat, plug in your headphones and step into the studio with Chad Dukes, Oscar Santana and Drab T-Shirt. By the end of each show, you\'ll have laughed yourself right out of your chair, but have gained a window into the lives of three new friends."
                    channelargument.ituneskeywords = "big o and dukes, big o and dukes show, oscar santana, chad dukes, mike o\'meara show, opie and anthony, howard stern, adam carolla show, kevin smith, tom leykis, gary and dino, don geronimo"
                    channelargument.itunesexplicit = "Yes"
                    channelargument.itunesnewfeedurl = "http://bigoanddukes.com/feed/podcast"
                    'channelargument.dateLastChanged = ?
                    channelargument.ownerName = "Big O And Dukes"
                    channelargument.ownerEmail = "bigoanddukes@gmail.com"
                    channelargument.feedimage = " href=""http://bigoanddukes.com/feed/podcast/boad-itunes.png"""
                    Dim rssfeed As New RSSFeed()
                    rssfeed.CreateChannel(channelargument)
                    '
                    ' -- add items
                    Dim sqlCriteria As String = "((isBlack=0)or(isBlack is null))and(isNetwork=1)and(isPremium<>1)and(link<>'')and(link is not null)and(name<>'')and(name is not null)"
                    Dim orderBy As String = "dateadded desc"
                    Dim createdbyAuth As String = "Big O and Dukes"
                    Dim podCastList As List(Of Models.BoadPodcastModel) = Models.BoadPodcastModel.createList(CP, "", orderBy)
                    For Each podCast As Models.BoadPodcastModel In podCastList

                        rssfeed.WriteRSSItem(podCast.name, podCast.link, "Big O And Dukes", podCast.RSSDescription, "", "", "Comedy", podCast.link, "Yes", podCast.RSSDatePublish, channelargument.ituneskeywords, podCast.imageFilename,)
                        If podCast = 750 Then
                            Exit For
                        End If
                    Next
                    result = CP.Utils.EncodeHTML(rssfeed.ToString)
                    If ae.packageErrorList.Count > 0 Then
                        result = "Hey user, this happened - " & Join(ae.packageErrorList.ToArray, "<br>")
                    End If
                End Using
            Catch ex As Exception
                CP.Site.ErrorReport(ex)
            End Try
            Return result
        End Function
    End Class
End Namespace

