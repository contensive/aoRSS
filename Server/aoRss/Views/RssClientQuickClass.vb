
Option Strict On
Option Explicit On

Imports System.Web.ClientServices
Imports Contensive.Addons.Rss.Controllers
Imports Contensive.Addons.Rss.Models.Db
Imports Contensive.Addons.Rss.Models.View
Imports Contensive.BaseClasses

'Contensive.Addons.Rss.Views.RssClientQuickClass
Namespace Views
    '
    Public Class RssClientQuickClass
        Inherits AddonBaseClass
        '
        Const RSSRootNode = "rss"
        Const AtomRootNode = "feed"

        Public Overrides Function Execute(CP As CPBaseClass) As Object
            Return GetContent(CP)
        End Function
        '
        Public Function GetContent(cp As CPBaseClass) As String
            Dim result As String = ""
            Try
                Dim IsRSS As Boolean
                Dim isAtom As Boolean
                Dim FeedHeader As String
                Dim LastRefresh As Date
                Dim Feed As String = ""
                Dim FeedConfig As String
                Dim ConfigHeader As String
                Dim ConfigSplit() As String
                Dim doc As Xml.XmlDocument
                Dim FeedFilename As String = ""
                Dim SaveCache As Boolean
                '
                Dim request As New RequestModel(cp)
                Dim rssClient = BaseModel.create(Of RSSClientModel)(cp, request.instanceId)
                If (rssClient Is Nothing) Then
                    rssClient = BaseModel.add(Of RSSClientModel)(cp)
                    rssClient.ccguid = request.instanceId
                    rssClient.name = "Quick Client created " & Now.ToString()
                    '
                    ' -- pickup either the legacy -wrench' values, or the addon's feature arguments
                    rssClient.url = cp.Doc.GetText("URL").Trim()
                    If (String.IsNullOrWhiteSpace(rssClient.url)) Then rssClient.url = "http://www.contensive.com/rss/OpenUp.xml"
                    rssClient.refreshhours = cp.Utils.EncodeInteger(cp.Doc.GetText("RefreshHours"))
                    If (rssClient.refreshhours = 0) Then rssClient.refreshhours = 1
                    rssClient.numberOfStories = cp.Utils.EncodeInteger(cp.Doc.GetText("Number of Stories"))
                    If rssClient.numberOfStories = 0 Then rssClient.numberOfStories = 99
                    rssClient.save(Of RSSClientModel)(cp)
                End If
                '
                SaveCache = False
                If rssClient.url <> "" Then
                    SaveCache = True
                    FeedFilename = encodeFilename(rssClient.url)
                    FeedFilename = "aoRSSClientFiles\" & FeedFilename & ".txt"
                    FeedConfig = cp.File.ReadVirtual(FeedFilename)
                    If Not FeedConfig <> "" Then
                        ConfigHeader = cp.Doc.GetText(FeedConfig)
                        If ConfigHeader <> "" Then
                            ConfigSplit = Split(ConfigHeader, ":")
                            If Trim(LCase(ConfigSplit(0))) = "rss client quick reader" Then
                                If True Then
                                    LastRefresh = cp.Utils.EncodeDate(cp.Doc.GetDate(FeedConfig))
                                    If LastRefresh <> CDate("0") Then
                                        If (LastRefresh.AddHours(rssClient.refreshhours) > Now()) Then
                                            '
                                            ' Use the cached feed
                                            '
                                            Feed = FeedConfig
                                            SaveCache = False
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                    If Feed = "" Then
                        '
                        ' Get a new copy of the feed
                        doc = New Xml.XmlDocument
                        doc.Load(rssClient.url)
                        Feed = doc.InnerXml
                        SaveCache = True
                    End If
                End If

                If Feed <> "" Then
                    doc = New Xml.XmlDocument
                    doc.LoadXml(Feed)
                    With doc.DocumentElement
                        '
                        If (LCase(.Name) = LCase(RSSRootNode)) Then
                            '
                            ' RSS Feed
                            '
                            IsRSS = True
                            result = GetRSS(cp, doc.InnerXml, rssClient.numberOfStories)
                        ElseIf (LCase(.Name) = LCase(AtomRootNode)) Then
                            '
                            ' Atom Feed
                            '
                            isAtom = True
                            result = GetAtom(cp, doc.InnerXml, CType(rssClient.numberOfStories, String))
                        Else
                            '
                            ' Bad Feed
                            '
                            If cp.User.IsAdmin Then
                                result = cp.Html.adminHint("The RSS Feed [" & rssClient.url & "] returned an incompatible file.")
                            End If
                        End If
                    End With
                    '
                    ' Save this feed into the cache
                    '
                    If SaveCache Then
                        FeedHeader = "RSS Client Quick Reader : " _
                                        & vbCrLf & CStr(Now())
                        Call cp.File.SaveVirtual(FeedFilename, FeedHeader & vbCrLf & Feed)
                    End If
                End If
                '
                If (cp.User.IsEditingAnything()) Then
                    result = cp.Content.GetEditLink("RSS Clients", rssClient.id.ToString(), False, "RSS Quick Client Settings", cp.User.IsAdmin()) & result
                End If
            Catch ex As Exception
                cp.Site.ErrorReport(ex)
            End Try
            Return result
        End Function

        Private Sub Link(providers As Object)
            Throw New NotImplementedException()
        End Sub
        '
        '=================================================================================
        '   Read RSS Feed
        '=================================================================================
        '
        Private Function GetRSS(cp As CPBaseClass, Feed As String, MaxStories As Long) As String
            Dim result As String = ""
            Try
                '
                Dim StoryCnt As Integer
                '
                'Dim IsRSS As Boolean
                'Dim isAtom As Boolean
                '
                Dim ItemPubDate As String
                Dim EnclosureRow As String
                Dim Ptr As Integer
                Dim Found As Boolean
                Dim EnclosureCnt As Integer
                Dim Enclosure() As EnclosureType
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
                '
                'Dim FeedHeader As String
                'Dim VersionString As String
                'Dim UserError As String
                'Dim LastRefresh As Date
                'Dim RefreshHours As Double
                'Dim FeedConfig As String
                'Dim ConfigHeader As String
                'Dim ConfigSplit() As String
                Dim doc As Xml.XmlDocument
                Dim RootNode As Xml.XmlNode
                Dim ChannelNode As Xml.XmlNode
                Dim ItemNode As Xml.XmlNode
                Dim ImageNode As Xml.XmlNode
                'Dim LoopPtr As Long
                'Dim FeedFilename As String
                'Dim Link As String
                '
                '
                ' Convert the feed to HTML
                '
                If Feed <> "" Then
                    doc = New Xml.XmlDocument
                    doc.LoadXml(Feed)
                    'Do While doc.readyState <> 4 And LoopPtr < 100
                    '    Sleep(100)
                    '    DoEvents
                    '    LoopPtr = LoopPtr + 1
                    'Loop
                    'If doc.parseError.errorCode <> 0 Then
                    '    '
                    '    ' error - Need a way to reach the user that submitted the file
                    '    '
                    '    If main.IsAdmin Then
                    '        GetRSS = main.GetAdminHintWrapper("The RSS Feed caused an error, " & doc.parseError.reason)
                    '    End If
                    'Else
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
                                                GetRSS = GetRSS _
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
                                    GetRSS = "" _
                                            & vbCrLf & vbTab & "<h2>" & ChannelTitle & "</h2>" _
                                            & vbCrLf & vbTab & "<div class=ChannelPubdate>" & ChannelPubDate & "</div>" _
                                            & vbCrLf & vbTab & "<div class=ChannelDescription>" & ChannelDescription & "</div>" _
                                            & GetRSS
                            End Select
                        Next
                        GetRSS = "" _
                                & vbCrLf & "<div class=RSSQuickClient>" _
                                & GetRSS _
                                & vbCrLf & "</div>"
                    End With
                End If
                result = GetRSS
                '
            Catch ex As Exception
                cp.Site.ErrorReport(ex)
            End Try
            Return result
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
                Dim IsRSS As Boolean
                Dim isAtom As Boolean
                '
                Dim ItemPubDate As String
                Dim EnclosureRow As String
                Dim Ptr As Integer
                Dim Found As Boolean
                Dim EnclosureCnt As Integer
                Dim Enclosure() As EnclosureType
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
                '
                'Dim FeedHeader As String
                'Dim VersionString As String
                'Dim UserError As String
                'Dim LastRefresh As Date
                'Dim RefreshHours As Double
                'Dim FeedConfig As String
                'Dim ConfigHeader As String
                'Dim ConfigSplit() As String
                Dim doc As Xml.XmlDocument
                Dim RootNode As Xml.XmlNode
                Dim ItemNode As Xml.XmlNode
                Dim ImageNode As Xml.XmlNode
                'Dim LoopPtr As Long
                'Dim FeedFilename As String
                'Dim Link As String
                '
                '
                ' Convert the feed to HTML
                '
                If Feed <> "" Then
                    doc = New Xml.XmlDocument
                    doc.LoadXml(Feed)
                    'Do While doc.readyState <> 4 And LoopPtr < 100
                    '    Sleep(100)
                    '    DoEvents
                    '    LoopPtr = LoopPtr + 1
                    'Loop
                    'If doc.parseError.errorCode <> 0 Then
                    '    '
                    '    ' error - Need a way to reach the user that submitted the file
                    '    '
                    '    If main.IsAdmin Then
                    '        GetAtom = main.GetAdminHintWrapper("The RSS Feed caused an error, " & doc.parseError.reason)
                    '    End If
                    'Else
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
                            'pubdate should be updated
                            'description should be subtitle
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
                                    GetAtom = GetAtom _
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
                        GetAtom = "" _
                                & vbCrLf & vbTab & "<div class=ChannelTitle>" & ChannelTitle & "</div>" _
                                & vbCrLf & vbTab & "<div class=ChannelPubdate>" & ChannelPubDate & "</div>" _
                                & vbCrLf & vbTab & "<div class=ChannelDescription>" & ChannelDescription & "</div>" _
                                & GetAtom
                        GetAtom = "" _
                                & vbCrLf & "<div class=RSSQuickClient>" _
                                & GetAtom _
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
