



Imports Contensive.Addons.Rss
Imports Contensive.Addons.Rss.Controllers
Imports Contensive.Addons.Rss.Models.Db
Imports Contensive.Addons.Rss.Models.Domain
Imports Contensive.BaseClasses

Namespace Views
    '
    Public Class AggregatorClass
        Inherits AddonBaseClass
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
                Dim AggregatorName As String = ""
                Dim cr1 As String = vbCrLf & vbTab
                Dim cr2 As String = cr1 & vbTab
                Dim cr3 As String = cr2 & vbTab
                Dim cr4 As String = cr3 & vbTab
                '
                Dim instanceId As String = CP.Doc.GetText("instanceId")
                CP.Utils.AppendLog("First Instance ID=" & instanceId)
                Dim StoryCnt As Long = CP.Utils.EncodeInteger(CP.Doc.GetText("Story count"))
                If StoryCnt = 0 Then
                    StoryCnt = 5
                End If
                Dim RSSAggregator As RSSAggregatorModel = BaseModel.create(Of RSSAggregatorModel)(CP, (instanceId))
                Dim AggregatorId As Integer = 0
                'cs = csv.opencsContent("RSS aggregators", "(name=" & CP.Utils.EncodeText(AggregatorName) & ")")
                If (RSSAggregator IsNot Nothing) Then
                    AggregatorId = RSSAggregator.id
                End If
                CP.Utils.AppendLog("Aggregator ID=" & AggregatorId)
                If (AggregatorId = 0) Or (instanceId Is Nothing) Then
                    '
                    ' Create new aggregator
                    '
                    RSSAggregator = BaseModel.add(Of RSSAggregatorModel)(CP)
                    RSSAggregator.name = "New RSS Aggregator create on-demand " & Now
                    AggregatorId = RSSAggregator.id
                    RSSAggregator.ccguid = instanceId
                    RSSAggregator.save(Of RSSAggregatorModel)(CP)
                    CP.Utils.AppendLog("instanceId=" & instanceId)

                    'End If
                End If
                '
                Dim cacheName As String = "rssAggregator:" & AggregatorId & ":" & StoryCnt
                Execute = CP.Doc.GetText(cacheName)
                If True Then
                    '
                    '
                    '
                    Dim storyList As List(Of RSSAggregatorSourceStorieModel) = RSSAggregatorSourceStorieModel.createStoryList(CP, CInt(AggregatorId))
                    '
                    Dim Ptr As Long = 1
                    Dim list As String = ""
                    For Each story In storyList
                        If StoryCnt >= Ptr Then
                            Dim Cell As String = ""
                            '
                            Dim Link As String = story.link
                            Dim Copy As String = Trim(story.name)
                            If Copy <> "" Then
                                If Link <> "" Then
                                    Copy = "<a href=""" & Link & """>" & Copy & "</a>"
                                End If
                                Cell = Cell & cr3 & "<h3 class=""raCaption"">" & Copy & "</h3>"
                            End If
                            CP.Utils.AppendLog("copy=" & Copy)
                            '
                            Dim pubDate As String = CType(story.pubDate, String)
                            Dim sourceName As String = Trim(story.name)
                            '
                            Dim Delimiter As String = ""
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

                            story.save(Of RSSAggregatorSourceStorieModel)(CP)
                            Ptr = Ptr + 1
                        Else
                            Exit For
                        End If

                    Next
                    If list <> "" Then
                        Execute = "" _
                            & cr1 & "<ul class=""rssAggregator"">" _
                            & list _
                            & cr1 & "</ul>" _
                            & ""
                    End If
                    result = list
                End If
            Catch ex As Exception
                CP.Site.ErrorReport(ex)
            End Try
            Return result
        End Function
        '
        '=====================================================================================
        '
        Friend Function GetXMLAttribute(cp As CPBaseClass, Found As Boolean, Node As Xml.XmlNode, Name As String) As String
            Dim result As String = ""
            Try
                Dim NodeAttribute As Xml.XmlAttribute
                Dim REsultNode As Xml.XmlNode
                Dim UcaseName As String
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
    End Class
    '
    '=====================================================================================
    '
    Friend Class EnclosureType
        Friend URL As String
        Public Property Type As String
        Public Property Length As String
    End Class
    '
End Namespace
