
Option Strict On
Option Explicit On

Imports Contensive.Addons.aoRSSClient2.Controllers
Imports Contensive.Addons.aoRSSClient2
Imports Contensive.BaseClasses

Namespace Views
    '
    Public Class AggregatorClass
        Inherits AddonBaseClass
        '
        Private main As Object
        Private csv As Object
        Public Property OptionString As String
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
        Public Overrides Function Execute(CP As CPBaseClass) As Object
            Dim result As String = ""
            Dim sw As New Stopwatch : sw.Start()
            Try
                '
                Dim Delimiter As String
                Dim pubDate As String
                Dim sourceName As String
                Dim StoryCnt As Long

                Dim Cell As String
                Dim list As String = ""
                Dim sql As String
                Dim Copy As String
                '
                Dim Ptr As Long
                Dim Link As String
                Dim AggregatorName As String = ""
                Dim AggregatorId As Integer = 0
                Dim cs As String
                Dim cacheName As String
                Const cacheList = "RSS Aggregators,RSS Aggregator Sources,RSS Aggregator Source Stories,RSS Aggregator Source Rules"
                '
                Dim cr1 As String
                Dim cr2 As String
                Dim cr3 As String
                Dim cr4 As String
                '
                cr1 = vbCrLf & vbTab
                cr2 = cr1 & vbTab
                cr3 = cr2 & vbTab
                cr4 = cr3 & vbTab
                '
                Dim instanceId As String = CP.Doc.GetText("instanceId")
                CP.Utils.AppendLogFile("First Instance ID=" & instanceId)
                If True Then
                    StoryCnt = CP.Utils.EncodeInteger(CP.Doc.GetText("Story count"))
                    If StoryCnt = 0 Then
                        StoryCnt = 5
                    End If
                    Dim RSSAggregator As Models.RSSAggregatorModel = Models.RSSAggregatorModel.create(CP, (instanceId))
                    'cs = csv.opencsContent("RSS aggregators", "(name=" & CP.Utils.EncodeText(AggregatorName) & ")")
                    If (RSSAggregator IsNot Nothing) Then
                        AggregatorId = RSSAggregator.id
                    End If
                    CP.Utils.AppendLogFile("Aggregator ID=" & AggregatorId)
                    If (AggregatorId = 0) Or (instanceId Is Nothing) Then
                        '
                        ' Create new aggregator
                        '
                        RSSAggregator = Models.RSSAggregatorModel.add(CP)
                        'cs = main.insertCsRecord("RSS Aggregators")
                        'If main.iscsok(cs) Then
                        RSSAggregator.name = "New RSS Aggregator create on-demand " & Now
                        'Call main.setcs(cs, "name", "New RSS Aggregator create on-demand for " & main.memberName & ", " & Now)
                        AggregatorId = RSSAggregator.id
                        RSSAggregator.ccguid = instanceId
                        RSSAggregator.save(CP)
                        CP.Utils.AppendLogFile("instanceId=" & instanceId)

                        'End If
                    End If
                    '
                    cacheName = "rssAggregator:" & AggregatorId & ":" & StoryCnt
                    Execute = CP.Doc.GetText(cacheName)
                    If True Then
                        '
                        '
                        '
                        Dim storyList As List(Of Models.RSSAggregatorSourceStorieModel) = Models.RSSAggregatorSourceStorieModel.createStoryList(CP, CInt(AggregatorId))
                        Ptr = 1
                        For Each story In storyList
                            If StoryCnt >= Ptr Then
                                Cell = ""
                                '
                                Link = Trim(story.link)
                                Copy = Trim(story.name)
                                If Copy <> "" Then
                                    If Link <> "" Then
                                        Copy = "<a href=""" & Link & """>" & Copy & "</a>"
                                    End If
                                    Cell = Cell & cr3 & "<h3 class=""raCaption"">" & Copy & "</h3>"
                                End If
                                CP.Utils.AppendLogFile("copy=" & Copy)
                                '
                                pubDate = CType(story.pubDate, String)
                                sourceName = Trim(story.name)
                                Delimiter = ""
                                If sourceName <> "" Then
                                    sourceName = "<span class=""raSourceName"">" & sourceName & "</span>"
                                End If
                                If pubDate <> "" Then
                                    pubDate = "<span  class=""raPubDate"">" & pubDate & "</span >"
                                End If
                                If (pubDate <> "") And (sourceName <> "") Then
                                    Delimiter = "<span class=""raDelimiter"">|</span>"
                                End If
                                Copy = sourceName & Delimiter & pubDate
                                If Copy <> "" Then
                                    Cell = Cell & cr3 & "<p class=""raByLine"">" & Copy & "</p>"
                                End If
                                '
                                Copy = Trim(story.description)
                                If Copy <> "" Then
                                    Cell = Cell & cr3 & "<p class=""raDescription"">" & Copy & "</p>"
                                    ' Cell = Cell & cr3 & "<p class=""raDescription"">" & Copy & "</p><p><a href=""" & Link & """>Continue Reading</a></p>"
                                End If
                                '
                                list = list _
                                & cr2 & "<ul><li class=""raItem"">" _
                                & Cell _
                                & cr2 & "</li></ul>"
                                '

                                story.save(CP)
                                Ptr = Ptr + 1
                            Else
                                Exit For
                            End If

                        Next
                        'Dim createRssAggregatorCache2List As List(Of Models.RSSAggregatorSourceRuleModel) = Models.RSSAggregatorSourceRuleModel.createRssAggregatorCache2(CP, CInt(AggregatorId))
                        ''sql = "select s.name,a.name as articleName, a.pubdate, a.description, a.link  from ((aoRSSAggregatorSourceRules r" _
                        ''        & " left join aorssaggregatorsources s on s.id=r.SourceID)" _
                        ''        & " left join rssaggregatorsourcestories a on a.sourceid=r.SourceID)" _
                        ''        & " Where r.AggregatorId = " & AggregatorId _
                        ''        & " order by a.pubdate desc,a.id desc"
                        ''cs = csv.opencsSql("", sql, 0)
                        'Ptr = 0
                        'For Each Aggregator In createRssAggregatorCache2List
                        '    If (Ptr < StoryCnt) Then

                        '    End If
                        'Next

                        ''Do While csv.iscsok(cs) And (Ptr < StoryCnt)

                        ''Loop

                        If list <> "" Then
                            Execute = "" _
                            & cr1 & "<ul class=""rssAggregator"">" _
                            & list _
                            & cr1 & "</ul>" _
                            & ""
                        End If
                        result = list
                        'Call csv.saveBake(cacheName, Execute, cacheList)
                    End If
                End If
            Catch ex As Exception
                CP.Site.ErrorReport(ex)
            End Try
            Return result
        End Function
        '
        Friend Function GetXMLAttribute(cp As CPBaseClass, Found As Boolean, Node As Xml.XmlNode, Name As String) As String

            Dim result As String = ""
            Try
                '
                Dim NodeAttribute As Xml.XmlAttribute
                Dim REsultNode As Xml.XmlNode
                Dim UcaseName As String
                '
                Found = False
                REsultNode = Node.Attributes.GetNamedItem(Name)
                If (REsultNode Is Nothing) Then
                    UcaseName = UCase(Name)
                    For Each NodeAttribute In Node.Attributes
                        If UCase(NodeAttribute.Name) = UcaseName Then
                            GetXMLAttribute = NodeAttribute.Value
                            Found = True
                            Exit For
                        End If
                    Next
                Else
                    GetXMLAttribute = REsultNode.Value
                    Found = True
                End If
            Catch ex As Exception
                cp.Site.ErrorReport(ex)
            End Try
            Return result
        End Function
        '
    End Class

    Friend Class EnclosureType
        Friend URL As String
        Public Property Type As String
        Public Property Length As String
    End Class
End Namespace
