
Option Strict On
Option Explicit On

Imports System.Linq
Imports Contensive.Addons.Rss
Imports Contensive.Addons.Rss.Controllers
Imports Contensive.Addons.Rss.Models.Db
Imports Contensive.BaseClasses

Namespace Views
    '
    Public Class RefreshProcessClass
        Inherits AddonBaseClass

        Public Property CsvObject As Object
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
                Dim linkRel As String = ""
                Dim Pos As String
                Dim isFound As Boolean
                Dim sourceId As Integer
                Dim csStory As String
                Dim LoopPtr As String = ""
                Dim csv As Object
                Dim cs As String
                Dim Link As String = ""
                Dim doc As Xml.XmlDocument
                Dim ItemTitle As String = ""
                Dim ItemLink As String = ""
                Dim ItemDescription As String = ""
                Dim ItemPubDate As String = ""
                Dim RootNode As Xml.XmlNode
                Dim ChannelNode As Xml.XmlNode
                Dim ItemNode As Xml.XmlNode
                Dim DateSplit() As String
                Dim isAtom As Boolean
                Dim itemGuid As String = ""
                Dim rssFeedId As Integer

                '
                csv = CsvObject
                ' 
                Dim RSSFeedModelList As List(Of Models.Db.RSSAggregatorSourcesModel) = BaseModel.createList(Of RSSAggregatorSourcesModel)(CP, "id<>0", "")
                If (RSSFeedModelList.Count <> 0) Then
                    For Each RSSFeedxml In RSSFeedModelList


                        'Dim RSSFeed As Models.Db.RSSAggregatorSourcesModel = RSSFeedModelList.First
                        sourceId = RSSFeedxml.id
                        Link = RSSFeedxml.Link

                        '
                        ' Convert the feed to HTML
                        '
                        'sourceId = csv.getcsinteger(cs, "id")
                        'Link = csv.getcstext(cs, "link")
                        If Link <> "" Then
                            doc = New Xml.XmlDocument
                            doc.Load(Link)
                            'Do While doc.readyState <> 4 And LoopPtr < 100
                            '    Sleep(100)
                            '    DoEvents
                            '    LoopPtr = LoopPtr + 1
                            'Loop
                            'If doc.parseError.errorCode <> 0 Then
                            '    '
                            '    ' error - Need a way to reach the user that submitted the file
                            '    '
                            '    Call csv.reportError("The RSS Feed [" & Link & "] caused an error, " & doc.parseError.reason, App.EXEName & ".refreshProcessClass.execute", True)
                            'Else
                            isAtom = (LCase(doc.DocumentElement.Name) = "feed")
                            If isAtom Then
                                '
                                ' atom feed
                                '
                                With doc.DocumentElement
                                    For Each RootNode In .ChildNodes
                                        Select Case LCase(RootNode.Name)
                                            Case "entry"
                                                ChannelNode = RootNode
                                                ItemTitle = ""
                                                ItemLink = ""
                                                ItemDescription = ""
                                                ItemPubDate = ""
                                                itemGuid = ""
                                                For Each ItemNode In ChannelNode.ChildNodes
                                                    Select Case LCase(ItemNode.Name)
                                                        Case "id"
                                                            itemGuid = ItemNode.InnerText
                                                        Case "title"
                                                            ItemTitle = ItemNode.InnerText
                                                        Case "content"
                                                            ItemDescription = ItemNode.InnerText
                                                            '
                                                            '   clear any styles out of the description
                                                            '
                                                            ItemDescription = ItemDescription
                                                        Case "link"

                                                            Dim linkType As String

                                                            isFound = False
                                                            linkType = GetXMLAttribute(CP, isFound, CType(ItemNode, Xml.XmlDocument), "type")
                                                            If (ItemLink = "") And ((Not isFound) Or (linkType = "text/html")) Then
                                                                ItemLink = GetXMLAttribute(CP, isFound, CType(ItemNode, Xml.XmlDocument), "href")
                                                            End If
                                                        Case "updated"
                                                            ItemPubDate = ItemNode.InnerText
                                                    End Select
                                                Next
                                                If ItemPubDate <> "" Then
                                                    Pos = CType(InStr(1, ItemPubDate, "T"), String)
                                                    If CInt(Pos) > 1 Then
                                                        ItemPubDate = Left(ItemPubDate, CInt(CInt(Pos) - 1))
                                                    End If
                                                End If
                                                If itemGuid = "" Then
                                                    itemGuid = ItemTitle
                                                End If

                                                Dim RSSAggregatorSourceStoryList As List(Of Models.Db.RSSAggregatorSourceStorieModel) = BaseModel.createList(Of RSSAggregatorSourceStorieModel)(CP, "(itemGuid=" & CP.Db.EncodeSQLText(itemGuid) & ")and(sourceId=" & sourceId & "))", "")
                                                'csStory = csv.opencsContent("RSS Aggregator Source Stories", "(itemGuid=" & KmaEncodeSQLText(itemGuid) & ")and(sourceId=" & sourceId & ")")
                                                If (RSSAggregatorSourceStoryList Is Nothing) Then
                                                    Dim SourceStory = BaseModel.add(Of RSSAggregatorSourceStorieModel)(CP)
                                                    SourceStory.pubDate = Now
                                                    SourceStory.sourceId = CInt(sourceId)
                                                    SourceStory.itemGuid = itemGuid

                                                End If
                                                If (RSSAggregatorSourceStoryList IsNot Nothing) Then
                                                    Dim SourceStory As Models.Db.RSSAggregatorSourceStorieModel = RSSAggregatorSourceStoryList.First
                                                    If SourceStory.name <> ItemTitle Then
                                                        SourceStory.name = ItemTitle
                                                    End If
                                                    If SourceStory.description <> ItemDescription Then
                                                        SourceStory.description = ItemDescription
                                                    End If
                                                    If SourceStory.link <> ItemLink Then
                                                        SourceStory.link = ItemLink
                                                    End If

                                                    If CP.Utils.EncodeDate(SourceStory.pubDate) <> CDate("0") Then
                                                        If SourceStory.pubDate <> CP.Utils.EncodeDate(ItemPubDate) Then
                                                            SourceStory.pubDate = CDate(ItemPubDate)
                                                        End If
                                                    End If
                                                    SourceStory.save(Of RSSAggregatorSourcesModel)(CP)
                                                End If
                                        End Select
                                    Next
                                End With
                            Else
                                '
                                ' RSS
                                '
                                With doc.DocumentElement
                                    For Each RootNode In .ChildNodes
                                        Select Case LCase(RootNode.Name)
                                            Case "channel"
                                                For Each ChannelNode In RootNode.ChildNodes
                                                    Select Case LCase(ChannelNode.Name)
                                                        Case "item"
                                                            ItemTitle = ""
                                                            ItemLink = ""
                                                            ItemDescription = ""
                                                            ItemPubDate = ""
                                                            itemGuid = ""
                                                            For Each ItemNode In ChannelNode.ChildNodes
                                                                Select Case LCase(ItemNode.Name)
                                                                    Case "guid"
                                                                        itemGuid = ItemNode.InnerText
                                                                    Case "title"
                                                                        ItemTitle = ItemNode.InnerText
                                                                    Case "description"
                                                                        ItemDescription = ItemNode.InnerText
                                                                        '
                                                                        '   clear any styles out of the description
                                                                        '
                                                                        ItemDescription = ItemDescription
                                                                    Case "link"
                                                                        ItemLink = ItemNode.InnerText
                                                                    Case "pubdate"
                                                                        ItemPubDate = ItemNode.InnerText
                                                                End Select
                                                            Next
                                                            If ItemPubDate <> "" Then
                                                                DateSplit = Split(ItemPubDate, " ")
                                                                If UBound(DateSplit) > 2 Then
                                                                    ItemPubDate = DateSplit(1) & " " & DateSplit(2) & " " & DateSplit(3)
                                                                    'ItemPubDate = DateSplit(0) & " " & DateSplit(1) & " " & DateSplit(2) & " " & DateSplit(3)
                                                                End If
                                                            End If
                                                            If itemGuid = "" Then
                                                                itemGuid = ItemTitle
                                                            End If
                                                            Dim RSSAggregatorSourceStoryList As List(Of Models.Db.RSSAggregatorSourceStorieModel) = BaseModel.createList(Of RSSAggregatorSourceStorieModel)(CP, "(name=" & CP.Db.EncodeSQLText(ItemTitle) & ")and(sourceId=" & sourceId & ")", "")
                                                            'csStory = csv.opencsContent("RSS Aggregator Source Stories", "(name=" & KmaEncodeSQLText(ItemTitle) & ")and(sourceId=" & sourceId & ")")
                                                            If (RSSAggregatorSourceStoryList.Count = 0) Then
                                                                Dim SourceStory = Models.Db.BaseModel.add(Of RSSAggregatorSourceStorieModel)(CP)
                                                                SourceStory.pubDate = Now
                                                                SourceStory.sourceId = CInt(sourceId)
                                                                SourceStory.itemGuid = itemGuid
                                                                SourceStory.save(Of RSSAggregatorSourceStorieModel)(CP)
                                                                RSSAggregatorSourceStoryList.Add(SourceStory)
                                                            End If
                                                            If (RSSAggregatorSourceStoryList.Count > 0) Then
                                                                Dim SourceStory As Models.Db.RSSAggregatorSourceStorieModel = RSSAggregatorSourceStoryList.First
                                                                If SourceStory.name <> ItemTitle Then
                                                                    SourceStory.name = ItemTitle
                                                                End If
                                                                If SourceStory.description <> ItemDescription Then
                                                                    SourceStory.description = ItemDescription
                                                                End If
                                                                If SourceStory.link <> ItemLink Then
                                                                    SourceStory.link = ItemLink
                                                                End If
                                                                If SourceStory.pubDate <> CP.Utils.EncodeDate(ItemPubDate) Then
                                                                    SourceStory.pubDate = CDate(ItemPubDate)
                                                                End If
                                                                SourceStory.save(Of RSSAggregatorSourcesModel)(CP)
                                                            End If

                                                            '
                                                    End Select
                                                Next
                                        End Select
                                    Next

                                End With
                            End If
                        End If
                    Next
                End If

                'End If
                'Call csv.nextcsRecord(cs)
                'Loop
                'Call csv.closecs(cs)
                '
                '
            Catch ex As Exception
                CP.Site.ErrorReport(ex)
            End Try
            Return result

        End Function


        '
        '========================================================================
        ' ----- Get an XML nodes attribute based on its name
        '========================================================================
        '
        Friend Function GetXMLAttribute(cp As CPBaseClass, Found As Boolean, Node As Xml.XmlDocument, Name As String) As String
            Dim result As String = ""
            Try
                '
                Dim NodeAttribute As Xml.XmlAttribute
                Dim REsultNode As Xml.XmlNode
                Dim UcaseName As String
                '
                Found = False
                REsultNode = CType(Node.Attributes.GetNamedItem(Name), Xml.XmlNode)
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
            '

        End Function
        '
        '   clear anything in between and including <style> tags and from description
        '
        Private Function clearStyles(cp As CPBaseClass, givenString As String) As String
            Dim Result As String = ""
            '
            Try
                Dim output As String
                Dim posStart As String
                Dim posEnd As String
                Dim styles As String
                '
                posStart = CType(InStr(givenString, "<style>"), String)
                posEnd = CType(InStr(givenString, "</style>") + Len("</style>") - 1, String)
                '
                If CInt(posStart) <> 0 Then
                    styles = Mid(givenString, CInt(posStart), CInt(posEnd))
                    output = Replace(givenString, styles, "")
                Else
                    output = givenString
                End If
                '
                clearStyles = output
                Result = clearStyles
                '
            Catch ex As Exception
                cp.Site.ErrorReport(ex)
            End Try
            Return Result
        End Function
    End Class
End Namespace
