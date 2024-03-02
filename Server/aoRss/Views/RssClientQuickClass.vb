Imports System.IO
Imports Contensive.Addons.Rss.Models.Db
Imports Contensive.Addons.Rss.Models.View
Imports Contensive.BaseClasses

Namespace Views
    '
    Public Class RssClientQuickClass
        Inherits AddonBaseClass
        '
        Const RSSRootNode = "rss"
        Const AtomRootNode = "feed"

        Public Overrides Function Execute(CP As CPBaseClass) As Object
            Dim hint As Integer = 10
            Try
                Dim request As New RequestModel(CP)
                If (String.IsNullOrEmpty(request.instanceId)) Then
                    CP.Site.ErrorReport("RSSQuickClient the instanceId is empty")
                    Return ""
                End If
                '
                Dim rssClient = BaseModel.create(Of RSSClientModel)(CP, request.instanceId)
                If (rssClient Is Nothing) Then
                    '
                    ' -- create default record
                    rssClient = BaseModel.add(Of RSSClientModel)(CP)
                    rssClient.ccguid = request.instanceId
                    rssClient.name = "Quick Client created " & Now.ToString()
                    '
                    ' -- pickup either the legacy -wrench' values, or the addon's feature arguments
                    rssClient.url = CP.Doc.GetText("URL").Trim()
                    If (String.IsNullOrWhiteSpace(rssClient.url)) Then rssClient.url = "http://www.contensive.com/rss/OpenUp.xml"
                    rssClient.refreshhours = CP.Utils.EncodeInteger(CP.Doc.GetText("RefreshHours"))
                    If (rssClient.refreshhours = 0) Then rssClient.refreshhours = 1
                    rssClient.numberOfStories = CP.Utils.EncodeInteger(CP.Doc.GetText("Number of Stories"))
                    If rssClient.numberOfStories = 0 Then rssClient.numberOfStories = 99
                    rssClient.save(Of RSSClientModel)(CP)
                End If
                hint = 20
                If String.IsNullOrWhiteSpace(rssClient.url) Then
                    Return ""
                End If
                '
                Dim SaveCache As Boolean = True
                Dim feedContent As String = ""
                Dim feedCacheFilename As String = encodeFilename(rssClient.url)
                feedCacheFilename = "aoRSSClientFiles\" & "" & ".txt"
                Dim feedCache As String = CP.CdnFiles.Read(feedCacheFilename)
                If String.IsNullOrEmpty(feedCache) Then
                    hint = 30
                    '
                    ' -- feed cache has content, check if valid
                    Using cacheReader = New StringReader(feedCache)
                        hint = 31
                        Dim cacheLine1 As String = cacheReader.ReadLine()
                        If (Not String.IsNullOrEmpty(cacheLine1)) Then
                            If Trim(cacheLine1.ToLowerInvariant()) = "rss client quick reader" Then
                                Dim cacheLastRefresh As Date = CP.Utils.EncodeDate(cacheReader.ReadLine())
                                If cacheLastRefresh > Date.MinValue Then
                                    If (cacheLastRefresh.AddHours(rssClient.refreshhours) > Now()) Then
                                        '
                                        ' Use the cached feed
                                        '
                                        feedContent = cacheReader.ReadToEnd()
                                        SaveCache = False
                                    End If
                                End If
                            End If
                        End If
                    End Using
                End If
                hint = 40
                If String.IsNullOrEmpty(feedContent) Then
                    Try
                        '
                        ' Get a new copy of the feed (hack the & out until we find out why its there)
                        Dim doc As New Xml.XmlDocument
                        doc.Load(rssClient.url.Replace("&", "%26"))
                        feedContent = doc.InnerXml
                        SaveCache = True
                    Catch ex As Exception
                        CP.Site.ErrorReport(ex, "Exception during fetch, rssClient.url [" & rssClient.url & "]")
                        Throw
                    End Try
                End If
                hint = 50
                Dim result As String = ""
                If Not String.IsNullOrEmpty(feedContent) Then
                    hint = 60
                    Dim doc As New Xml.XmlDocument
                    doc.LoadXml(feedContent)
                    With doc.DocumentElement
                        '
                        If (LCase(.Name) = LCase(RSSRootNode)) Then
                            '
                            ' RSS Feed
                            '
                            Dim IsRSS As Boolean = True
                            result = GetRSS(CP, doc.InnerXml, rssClient.numberOfStories)
                        ElseIf (LCase(.Name) = LCase(AtomRootNode)) Then
                            '
                            ' Atom Feed
                            '
                            Dim isAtom As Boolean = True
                            result = GetAtom(CP, doc.InnerXml, CType(rssClient.numberOfStories, String))
                        Else
                            '
                            ' Bad Feed
                            '
                            SaveCache = False
                            If CP.User.IsAdmin Then
                                result = CP.Html.adminHint("The RSS Feed [" & rssClient.url & "] returned an incompatible file.")
                            End If
                        End If
                    End With
                    hint = 70
                    '
                    ' Save this feed into the cache
                    '
                    If SaveCache Then
                        Dim FeedHeader As String = "RSS Client Quick Reader" & vbCrLf & CStr(Now())
                        Call CP.CdnFiles.Save(feedCacheFilename, FeedHeader & vbCrLf & feedContent)
                    End If
                    hint = 80
                End If
                hint = 90
                '
                If (CP.User.IsEditingAnything()) Then
                    result = CP.Content.GetEditLink("RSS Clients", rssClient.id.ToString(), False, "RSS Quick Client Settings", CP.User.IsAdmin()) & result
                End If
                Return result
            Catch ex As Exception
                CP.Site.ErrorReport(ex, "hint [" & hint & "]")
                Throw
            End Try
        End Function
        '
        '=================================================================================
        '   Read RSS Feed
        '=================================================================================
        '
        Private Function GetRSS(cp As CPBaseClass, Feed As String, MaxStories As Long) As String
            Try
                Dim result As String = ""
                '
                Dim StoryCnt As Integer
                Dim ItemPubDate As String
                Dim EnclosureRow As String
                Dim Ptr As Integer
                Dim Found As Boolean
                Dim EnclosureCnt As Integer
                Dim ChannelImage As String
                Dim ChannelTitle As String
                Dim ChannelDescription As String
                Dim ChannelPubDate As String = ""
                Dim ChannelItem As String
                Dim ChannelLink As String
                Dim NewChannelImage As String
                Dim ItemLink As String
                Dim ItemTitle As String
                Dim ItemDescription As String
                Dim ImageWidth As String
                Dim ImageHeight As String
                Dim ImageTitle As String
                Dim ImageURL As String
                Dim ImageLink As String
                Dim doc As Xml.XmlDocument
                Dim RootNode As Xml.XmlNode
                Dim ChannelNode As Xml.XmlNode
                Dim ItemNode As Xml.XmlNode
                Dim ImageNode As Xml.XmlNode
                '
                ' Convert the feed to HTML
                '
                If Feed <> "" Then
                    doc = New Xml.XmlDocument
                    doc.LoadXml(Feed)
                    With doc.DocumentElement
                        ChannelTitle = ""
                        ChannelDescription = ""
                        ChannelLink = ""
                        For Each RootNode In .ChildNodes
                            Select Case LCase(RootNode.Name)
                                Case "channel"
                                    ChannelTitle = ""
                                    ChannelDescription = ""
                                    ChannelLink = ""
                                    ChannelImage = ""
                                    ChannelItem = ""
                                    For Each ChannelNode In RootNode.ChildNodes
                                        Select Case LCase(ChannelNode.Name)
                                            Case "pubdate"
                                                ChannelPubDate = ChannelNode.InnerText
                                            Case "title"
                                                ChannelTitle = ChannelNode.InnerText
                                            Case "description"
                                                ChannelDescription = ChannelNode.InnerText
                                            Case "link"
                                                ChannelLink = ChannelNode.InnerText
                                            Case "image"
                                                ImageWidth = ""
                                                ImageHeight = ""
                                                ImageTitle = ""
                                                ImageURL = ""
                                                ImageLink = ""
                                                NewChannelImage = ""
                                                For Each ImageNode In ChannelNode.ChildNodes
                                                    Select Case LCase(ImageNode.Name)
                                                        Case "title"
                                                            ImageTitle = ImageNode.InnerText
                                                        Case "url"
                                                            ImageURL = ImageNode.InnerText
                                                        Case "link"
                                                            ImageLink = ImageNode.InnerText
                                                        Case "width"
                                                            ImageWidth = ImageNode.InnerText
                                                        Case "height"
                                                            ImageHeight = ImageNode.InnerText
                                                    End Select
                                                Next

                                                If ImageURL <> "" Then
                                                    NewChannelImage = NewChannelImage & "<img class=ChannelImage src=""" & ImageURL & """"
                                                    If ImageWidth <> "" Then
                                                        NewChannelImage = NewChannelImage & " width=""" & ImageWidth & """"
                                                    End If
                                                    If ImageHeight <> "" Then
                                                        NewChannelImage = NewChannelImage & " height=""" & ImageHeight & """"
                                                    End If
                                                    If ImageTitle <> "" Then
                                                        NewChannelImage = NewChannelImage & " title=""" & ImageTitle & """"
                                                    End If
                                                    NewChannelImage = NewChannelImage & " style=""float:left"" border=0>"
                                                    If ImageLink <> "" Then
                                                        NewChannelImage = "<a href=""" & ImageLink & """ target=_blank>" & NewChannelImage & "</a>"
                                                    End If
                                                    ChannelImage = ChannelImage & NewChannelImage
                                                End If
                                            Case "item"
                                                ItemTitle = ""
                                                ItemLink = ""
                                                ItemDescription = ""
                                                ItemPubDate = ""
                                                EnclosureCnt = 0
                                                Dim Enclosure() As EnclosureType = New EnclosureType(0) {}
                                                For Each ItemNode In ChannelNode.ChildNodes
                                                    Select Case LCase(ItemNode.Name)
                                                        Case "title"
                                                            ItemTitle = ItemNode.InnerText
                                                        Case "description"
                                                            ItemDescription = ItemNode.InnerText
                                                        Case "link"
                                                            ItemLink = ItemNode.InnerText
                                                        Case "pubdate"
                                                            ItemPubDate = ItemNode.InnerText
                                                        Case "enclosure"
                                                            ReDim Preserve Enclosure(EnclosureCnt)
                                                            Enclosure(EnclosureCnt).URL = GetXMLAttribute(cp, Found, CType(ItemNode, Xml.XmlDocument), "url")
                                                            Enclosure(EnclosureCnt).Type = GetXMLAttribute(cp, Found, CType(ItemNode, Xml.XmlDocument), "type")
                                                            Enclosure(EnclosureCnt).Length = GetXMLAttribute(cp, Found, CType(ItemNode, Xml.XmlDocument), "length")
                                                            EnclosureCnt = EnclosureCnt + 1
                                                    End Select
                                                Next
                                                Dim DateSplit() As String
                                                If ItemPubDate <> "" Then
                                                    DateSplit = Split(ItemPubDate, " ")
                                                    If UBound(DateSplit) > 2 Then
                                                        ItemPubDate = DateSplit(0) & " " & DateSplit(1) & " " & DateSplit(2) & " " & DateSplit(3)
                                                    End If
                                                    ItemPubDate = vbCrLf & vbTab & vbTab & "<div class=ItemPubDate>" & ItemPubDate & "</div>"
                                                End If
                                                If ItemTitle <> "" Then
                                                    If ItemLink <> "" Then
                                                        ItemTitle = "<a href=""" & ItemLink & """ target=_blank>" & ItemTitle & "</a>"
                                                    End If
                                                    ItemTitle = vbCrLf & vbTab & vbTab & "<h3>" & ItemTitle & "</h3>"
                                                End If
                                                If ItemDescription <> "" Then
                                                    ItemDescription = vbCrLf & vbTab & vbTab & "<div class=ItemDescription>" & ItemDescription & "</div>"
                                                End If
                                                '
                                                EnclosureRow = ""
                                                If EnclosureCnt > 0 Then
                                                    For Ptr = 0 To EnclosureCnt - 1
                                                        With Enclosure(Ptr)
                                                            If .URL <> "" Then
                                                                EnclosureRow = EnclosureRow & vbCrLf & vbTab & vbTab & vbTab & "<div class=ItemEnclosure><a href=""" & .URL & """>Media</a></div>"
                                                            End If
                                                        End With
                                                    Next
                                                    If EnclosureRow <> "" Then
                                                        EnclosureRow = "" _
                                                                & vbCrLf & vbTab & vbTab & "<div class=ItemEnclosureRow>" _
                                                                & EnclosureRow _
                                                                & vbCrLf & vbTab & vbTab & "</div>"
                                                    End If
                                                End If
                                                result = result _
                                                        & vbCrLf & vbTab & "<hr style=""clear:both""><div class=ChannelItem>" _
                                                        & ItemTitle _
                                                        & ItemPubDate _
                                                        & ItemDescription _
                                                        & EnclosureRow _
                                                        & vbCrLf & vbTab & "</div>" _
                                                        & ""
                                                StoryCnt = StoryCnt + 1
                                        End Select
                                        If StoryCnt >= MaxStories Then
                                            Exit For
                                        End If
                                    Next
                                    If ChannelLink <> "" Then
                                        ChannelTitle = "<a href=""" & ChannelLink & """ target=_blank>" & ChannelTitle & "</a>"
                                    End If
                                    If ChannelImage <> "" Then
                                        ChannelDescription = ChannelImage & ChannelDescription
                                    End If
                                    result = "" _
                                        & vbCrLf & vbTab & "<h2>" & ChannelTitle & "</h2>" _
                                        & vbCrLf & vbTab & "<div class=ChannelPubdate>" & ChannelPubDate & "</div>" _
                                        & vbCrLf & vbTab & "<div class=ChannelDescription>" & ChannelDescription & "</div>" _
                                        & result
                            End Select
                        Next
                        result = "" _
                                & vbCrLf & "<div class=RSSQuickClient>" _
                                & result _
                                & vbCrLf & "</div>"
                    End With
                End If
                Return result
            Catch ex As Exception
                cp.Site.ErrorReport(ex)
                Throw
            End Try
        End Function
        '
        '=================================================================================
        '   Read Atom Feed
        '=================================================================================
        '
        Private Function GetAtom(cp As CPBaseClass, Feed As String, MaxStories As String) As String
            Dim result As String = ""
            Try
                '
                Dim StoryCnt As Integer
                Dim Pos As String
                Dim DateSplit() As String
                '
                Dim ItemPubDate As String
                Dim EnclosureRow As String
                Dim Ptr As Integer
                Dim EnclosureCnt As Integer
                Dim ChannelImage As String
                Dim ChannelTitle As String
                Dim ChannelDescription As String
                Dim ChannelPubDate As String = ""
                Dim ChannelItem As String
                Dim ChannelLink As String
                Dim NewChannelImage As String
                '
                Dim ItemLink As String
                Dim ItemTitle As String
                Dim ItemDescription As String
                '
                Dim ImageWidth As String
                Dim ImageHeight As String
                Dim ImageTitle As String
                Dim ImageURL As String
                Dim ImageLink As String
                Dim doc As Xml.XmlDocument
                Dim RootNode As Xml.XmlNode
                Dim ItemNode As Xml.XmlNode
                Dim ImageNode As Xml.XmlNode
                '
                ' Convert the feed to HTML
                '
                If Feed <> "" Then
                    doc = New Xml.XmlDocument
                    doc.LoadXml(Feed)
                    With doc.DocumentElement
                        ChannelTitle = ""
                        ChannelDescription = ""
                        ChannelLink = ""
                        ChannelImage = ""
                        ChannelItem = ""
                        For Each RootNode In .ChildNodes
                            '
                            ' Atom Feed only has one channel, so there is no Channel element
                            '
                            Select Case LCase(RootNode.Name)
                                Case "updated"
                                    ChannelPubDate = RootNode.InnerText
                                    Pos = CType(InStr(1, ChannelPubDate, "T", vbTextCompare), String)
                                    If CInt(Pos) > 0 Then
                                        ChannelPubDate = Mid(ChannelPubDate, 1, CInt(CInt(Pos) - 1))
                                        Pos = CType(InStr(1, ChannelPubDate, "-"), String)
                                        If CInt(Pos) > 0 Then
                                            DateSplit = Split(ChannelPubDate, "-")
                                            If UBound(DateSplit) = 2 Then
                                                ChannelPubDate = FormatDateTime(cp.Utils.EncodeDate(CStr(DateSplit(1) & "/" & DateSplit(2) & "/" & DateSplit(0))), vbLongDate)
                                            End If
                                        End If
                                    End If
                                Case "title"
                                    ChannelTitle = RootNode.InnerText
                                Case "subtitle"
                                    ChannelDescription = RootNode.InnerText
                                Case "link"
                                    Dim linkType As String
                                    Dim isFound As Boolean
                                    linkType = GetXMLAttribute(cp, isFound, CType(RootNode, Xml.XmlDocument), "type")
                                    If LCase(linkType) = "text/html" Then
                                        ChannelLink = GetXMLAttribute(cp, isFound, CType(RootNode, Xml.XmlDocument), "href")
                                    End If
                                Case "image"
                                    ImageWidth = ""
                                    ImageHeight = ""
                                    ImageTitle = ""
                                    ImageURL = ""
                                    ImageLink = ""
                                    NewChannelImage = ""
                                    For Each ImageNode In RootNode.ChildNodes
                                        Select Case LCase(ImageNode.Name)
                                            Case "title"
                                                ImageTitle = ImageNode.InnerText
                                            Case "url"
                                                ImageURL = ImageNode.InnerText
                                            Case "link"
                                                ImageLink = ImageNode.InnerText
                                            Case "width"
                                                ImageWidth = ImageNode.InnerText
                                            Case "height"
                                                ImageHeight = ImageNode.InnerText
                                        End Select
                                    Next

                                    If ImageURL <> "" Then
                                        NewChannelImage = NewChannelImage & "<img class=ChannelImage src=""" & ImageURL & """"
                                        If ImageWidth <> "" Then
                                            NewChannelImage = NewChannelImage & " width=""" & ImageWidth & """"
                                        End If
                                        If ImageHeight <> "" Then
                                            NewChannelImage = NewChannelImage & " height=""" & ImageHeight & """"
                                        End If
                                        If ImageTitle <> "" Then
                                            NewChannelImage = NewChannelImage & " title=""" & ImageTitle & """"
                                        End If
                                        NewChannelImage = NewChannelImage & " style=""float:left"" border=0>"
                                        If ImageLink <> "" Then
                                            NewChannelImage = "<a href=""" & ImageLink & """ target=_blank>" & NewChannelImage & "</a>"
                                        End If
                                        ChannelImage = ChannelImage & NewChannelImage
                                    End If
                                Case "entry"
                                    ItemTitle = ""
                                    ItemLink = ""
                                    ItemDescription = ""
                                    ItemPubDate = ""
                                    EnclosureCnt = 0
                                    For Each ItemNode In RootNode.ChildNodes
                                        Dim linkType As String = Nothing
                                        Select Case LCase(ItemNode.Name)
                                            Case "title"
                                                ItemTitle = ItemNode.InnerText
                                            Case "link"
                                                Dim isFound As Boolean = Nothing
                                                linkType = GetXMLAttribute(cp, isFound, CType(ItemNode, Xml.XmlDocument), "type")


                                                If LCase(linkType) = "text/html" Then
                                                    ItemLink = GetXMLAttribute(cp, isFound, CType(ItemNode, Xml.XmlDocument), "href")
                                                End If
                                            Case "updated"
                                                ItemPubDate = ItemNode.InnerText
                                                Pos = CType(InStr(1, ItemPubDate, "T", vbTextCompare), String)
                                                If CInt(Pos) > 0 Then
                                                    ItemPubDate = Mid(ItemPubDate, 1, CInt(Pos) - 1)
                                                    Pos = CType(InStr(1, ItemPubDate, "-"), String)
                                                    If CInt(Pos) > 0 Then
                                                        DateSplit = Split(ItemPubDate, "-")
                                                        If UBound(DateSplit) = 2 Then
                                                            'ItemPubDate = FormatDateTime(KmaEncodeDate(CStr(DateSplit(2) & "/" & DateSplit(1) & "/" & DateSplit(0))), vbLongDate)
                                                            ItemPubDate = FormatDateTime(cp.Utils.EncodeDate(CStr(DateSplit(1) & "/" & DateSplit(2) & "/" & DateSplit(0))), vbLongDate)
                                                        End If
                                                    End If
                                                End If
                                            Case "summary"
                                                ItemDescription = ItemNode.InnerText
                                                'Case "enclosure"
                                                '    ReDim Preserve Enclosure(EnclosureCnt)
                                                '    Enclosure(EnclosureCnt).URL = GetXMLAttribute(Found, ItemNode, "url")
                                                '    Enclosure(EnclosureCnt).Type = GetXMLAttribute(Found, ItemNode, "type")
                                                '    Enclosure(EnclosureCnt).Length = GetXMLAttribute(Found, ItemNode, "length")
                                                '    EnclosureCnt = EnclosureCnt + 1
                                        End Select
                                    Next
                                    If ItemPubDate <> "" Then
                                        DateSplit = Split(ItemPubDate, " ")
                                        If UBound(DateSplit) > 2 Then
                                            ItemPubDate = DateSplit(0) & " " & DateSplit(1) & " " & DateSplit(2) & " " & DateSplit(3)
                                        End If
                                        ItemPubDate = vbCrLf & vbTab & vbTab & "<div class=ItemPubDate>" & ItemPubDate & "</div>"
                                    End If
                                    If ItemTitle <> "" Then
                                        If ItemLink <> "" Then
                                            ItemTitle = "<a href=""" & ItemLink & """ target=_blank>" & ItemTitle & "</a>"
                                        End If
                                        ItemTitle = vbCrLf & vbTab & vbTab & "<div class=ItemTitle>" & ItemTitle & "</div>"
                                    End If
                                    If ItemDescription <> "" Then
                                        ItemDescription = vbCrLf & vbTab & vbTab & "<div class=ItemDescription>" & ItemDescription & "</div>"
                                    End If
                                    '
                                    EnclosureRow = ""
                                    If EnclosureCnt > 0 Then
                                        For Ptr = 0 To EnclosureCnt - 1
                                            Dim Enclosure() As EnclosureType = New EnclosureType(0) {}
                                            With Enclosure(CInt(Ptr))
                                                If .URL <> "" Then
                                                    EnclosureRow = EnclosureRow & vbCrLf & vbTab & vbTab & vbTab & "<div class=ItemEnclosure><a href=""" & .URL & """>Media</a></div>"
                                                End If
                                            End With
                                        Next
                                        If EnclosureRow <> "" Then
                                            EnclosureRow = "" _
                                                    & vbCrLf & vbTab & vbTab & "<div class=ItemEnclosureRow>" _
                                                    & EnclosureRow _
                                                    & vbCrLf & vbTab & vbTab & "</div>"
                                        End If
                                    End If
                                    result = result _
                                            & vbCrLf & vbTab & "<div class=ChannelItem>" _
                                            & ItemTitle _
                                            & ItemPubDate _
                                            & ItemDescription _
                                            & EnclosureRow _
                                            & vbCrLf & vbTab & "</div>" _
                                            & ""
                                    StoryCnt = StoryCnt + 1
                                    If StoryCnt >= CInt(MaxStories) Then
                                        Exit For
                                    End If
                            End Select
                        Next
                        If ChannelLink <> "" Then
                            ChannelTitle = "<a href=""" & ChannelLink & """ target=_blank>" & ChannelTitle & "</a>"
                        End If
                        If ChannelImage <> "" Then
                            ChannelDescription = ChannelImage & ChannelDescription
                        End If
                        result = "" _
                                & vbCrLf & vbTab & "<div class=ChannelTitle>" & ChannelTitle & "</div>" _
                                & vbCrLf & vbTab & "<div class=ChannelPubdate>" & ChannelPubDate & "</div>" _
                                & vbCrLf & vbTab & "<div class=ChannelDescription>" & ChannelDescription & "</div>" _
                                & result
                        result = "" _
                                & vbCrLf & "<div class=RSSQuickClient>" _
                                & result _
                                & vbCrLf & "</div>"
                    End With
                End If

                '
            Catch ex As Exception
                cp.Site.ErrorReport(ex)
            End Try
            Return result

            'HandleError
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
        '
        Private Function encodeFilename(Filename As String) As String
            Dim result As String = Filename.ToLower().Replace("http://", "").Replace("https://", "").Replace("/", "-")
            For Each c In IO.Path.GetInvalidFileNameChars
                result = result.Replace(c, "")
            Next
            Return result
        End Function
        '
        '
        'Public Overrides Function Execute(CP As CPBaseClass) As Object
        '    Dim result As String = ""

        '    Try
        '        Throw New NotImplementedException()
        '    Catch ex As Exception

        '    End Try
        '    Return result
        'End Function
    End Class
End Namespace
