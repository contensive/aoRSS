
Option Strict On
Option Explicit On

Imports Contensive.Addons.aoRssFeed2.Controllers
Imports Contensive.BaseClasses

Namespace Views
    '
    Public Class RSSFeedLinkClass
        Inherits AddonBaseClass
        Public RSSLink As New RSSLinkClass
        Private ReadOnly MainObj As Object
        Public Property CsvObj As Object
        Public Property Optionstring As Object
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
                Execute = RSSLink.Execute(CP)
            Catch ex As Exception
                CP.Site.ErrorReport(ex)
            End Try
        End Function
        '
        '
        'Public Sub Init(MainObject As Object)
        '    Call RSSLink.Init(MainObject)
        'End Sub
        '
        '=================================================================================
        '   Aggregate Object Interface
        '=================================================================================
        '
        Public Function GetContent(cp As CPBaseClass, Optionstring As String) As String
            GetContent = RSSLink.GetContent(cp, Optionstring)
        End Function
    End Class
End Namespace
