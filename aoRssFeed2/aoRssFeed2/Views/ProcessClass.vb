
'Option Strict On
Option Explicit On
Imports Contensive.Addons.aoRssFeed2
Imports Contensive.Addons.aoRssFeed2.Contensive.Addons.aoRssFeed2
Imports Contensive.BaseClasses

Namespace Views
    '
    Public Class ProcessClass
        Inherits AddonBaseClass
        Private FeedCnt As Integer
        Private GetFirstPointer As String
        Public Property FeedSize As Integer
        Public Property Feeds As Models.Domain.FeedModel()
        Public Property GetNextPointer As String
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
                Dim RuleContentID As Integer
                Dim ContentName As String
                Dim SQLDateNow As String
                Dim CS As Integer
                Dim RSSFilename As String
                Dim s As String
                Dim Ext As String
                Dim MimeType As String
                Dim EnclosureLength As Integer
                Dim Ptr As Integer
                Dim Enclosure As String
                Dim ItemName As String
                Dim ItemDescription As String
                Dim Link As String
                Dim Pos As Integer
                Dim LastTableID As Integer
                Dim ServerContentWatchPrefix As String
                Dim DomainName As String
                Dim ItemDatePublishText As String
                Dim Hint As String
                Dim RightNow As Date
                Dim DaysAgo As Double
                Dim Doc As Xml.XmlDocument
                Dim BaseNode As Xml.XmlNode
                Dim Node As Xml.XmlElement
                Dim ChannelNode As Xml.XmlNode
                Dim ImageNode As Xml.XmlNode
                Dim ItemNode As Xml.XmlNode
                Dim IndexPtr As Integer
                Dim usedFilenames As String = ""
                Dim CS1 As CPCSBaseClass = CP.CSNew()
                Dim CS2 As CPCSBaseClass = CP.CSNew()
                Dim CS3 As CPCSBaseClass = CP.CSNew()
                '
                Hint = "entering"
                ' If Not Csv.IsContentFieldSupported("RSS Feeds", "ID") Then
                If Not CP.Content.IsField("RSS Feeds", "ID") Then
                    Hint = "This site does not have an RSS Feed content."
                Else
                    RightNow = Now
                    SQLDateNow = CP.Db.EncodeSQLDate(RightNow)
                    '
                    ' Get Primary domain from domain list
                    '
                    DomainName = CP.Site.DomainPrimary
                    Pos = InStr(1, DomainName, ",")
                    If Pos > 2 Then
                        DomainName = Mid(DomainName, 1, Pos - 1)
                    End If
                    '
                    ' Build Prefix for content watch
                    '
                    ServerContentWatchPrefix = "http://" & DomainName & CP.Site.AppRootPath
                    ServerContentWatchPrefix = Mid(ServerContentWatchPrefix, 1, Len(ServerContentWatchPrefix) - 1)
                    '
                    ' Load the feeds
                    '
                    ' Dim Feeds As New Dictionary(Of Integer, Models.Domain.FeedModel)
                    Dim rssFeedList As List(Of Models.RSSFeedModel) = Models.RSSFeedModel.createList(CP, "id<>0")
                    'CS = Csv.OpenCSContent("RSS Feeds")
                    If (rssFeedList.Count = 0) Then
                        Hint = Hint & "," & "RSS Feeds does not support ID (content not there)"
                    Else
                        Dim FeedPtr As Integer = 0
                        For Each rssfeed In rssFeedList
                            ReDim Preserve Feeds(FeedPtr + 1)
                            Feeds(FeedPtr) = New Models.Domain.FeedModel()
                            Feeds(FeedPtr).Id = rssfeed.id
                            Feeds(FeedPtr).Name = rssfeed.name
                            Feeds(FeedPtr).Description = rssfeed.Description
                            Feeds(FeedPtr).Link = rssfeed.Link
                            Feeds(FeedPtr).LogoFilename = rssfeed.Link
                            If Feeds(FeedPtr).Link = "" Then
                                Feeds(FeedPtr).Link = ServerContentWatchPrefix
                            End If
                            Feeds(FeedPtr).EntryDateIndex = New Dictionary(Of String, Integer)
                            RSSFilename = rssfeed.RSSFilename
                            Dim testfilename As String = RSSFilename
                            If testfilename = "" Then
                                testfilename = rssfeed.name
                            End If
                            If testfilename = "" Then
                                testfilename = "RSSFeed" & rssfeed.id
                            End If
                            Pos = InStr(1, testfilename, ".xml", vbTextCompare)
                            If Pos > 0 Then
                                testfilename = Mid(testfilename, 1, Pos - 1)
                            End If
                            testfilename = testEncodeFilename(testfilename)
                            Dim suffixNumber As Integer = 1
                            testfilename = CP.Utils.EncodeText(testfilename)
                            Dim testFilenameRoot As String = testfilename
                            Do While (suffixNumber < 100) And (InStr(1, "," & usedFilenames & ",", "," & testfilename & ",", vbTextCompare) <> 0)
                                testfilename = testFilenameRoot & CStr(suffixNumber)
                                suffixNumber = suffixNumber + 1
                            Loop
                            usedFilenames = usedFilenames & "," & testfilename
                            testfilename = testfilename & ".xml"
                            If testfilename <> RSSFilename Then
                                RSSFilename = testfilename
                                rssfeed.RSSFilename = RSSFilename
                            End If
                            Feeds(FeedPtr).RSSFilename = RSSFilename
                            rssfeed.RSSDateUpdated = RightNow
                            rssfeed.save(CP)
                            FeedPtr += 1
                        Next
                        FeedCnt = FeedPtr
                        Hint = Hint & "," & "RSS Feed Cnt=" & FeedPtr
                    End If

                    '
                    ' Find all the content that implements rss feeds
                    '
                    Dim FeedContentPtr As Integer = 0
                    LastTableID = -1
                    ' CSFields = Csv.OpenCSContent("Content Fields", "(name='RSSFeeds')and(type=" & FieldTypeManyToMany & ")and(authorable<>0)", "ContentID")
                    Dim contentFieldList As List(Of Models.ContentFieldModel) = Models.ContentFieldModel.createList(CP, "(name='RSSFeeds')and(type=" & FieldTypeManyToMany & ")and(authorable<>0)", "ContentID")
                    Dim EntryPtr As Integer = 1
                    If (contentFieldList.Count = 0) Then
                        Hint = Hint & "," & "There are no 'content fields' named RSSFields with type=manytomany"
                    Else
                        Hint = Hint & "," & "going through all content fields records with name=RSSFields and type=manytomany"
                        For Each contentFields In contentFieldList

                            Hint = Hint & "," & "ccfields record " & contentFields.id
                            Dim ContentID As Integer = contentFields.ContentID
                            If ContentID <> 0 Then
                                ContentName = CP.Content.GetRecordName("content", ContentID)
                                If ContentName <> "" Then
                                    ' TableID = Csv.GetContentTableID(ContentName)
                                    Dim TableID As Integer = CP.Content.GetRecordID("content", ContentName)
                                    If TableID = LastTableID Then
                                        Hint = Hint & "," & "field contentid matchs the last row, skipping"
                                    Else
                                        '
                                        ' Populate the FeedContent Array (might not be necessary)
                                        '
                                        LastTableID = TableID
                                        Dim feedContent As Models.Domain.FeedContentType()
                                        ReDim Preserve feedContent(FeedContentPtr + 1)
                                        feedContent(FeedContentPtr) = New Models.Domain.FeedContentType()
                                        With feedContent(FeedContentPtr)
                                            .ContentID = ContentID
                                            .TableID = TableID
                                            RuleContentID = contentFields.ManyToManyRuleContentID 'Csv.GetCSText(CSFields, "ManyToManyRuleContentID")
                                            .ManyToManyRuleContent = CP.Content.GetRecordName("content", RuleContentID) 'Csv.GetContentNameByID(RuleContentID)
                                            .ManyToManyRulePrimaryField = contentFields.ManyToManyRulePrimaryField ' Csv.GetCSText(CSFields, "ManyToManyRulePrimaryField")
                                            .ManyToManyRuleSecondaryField = contentFields.ManyToManyRuleSecondaryField ' Csv.GetCSText(CSFields, "ManyToManyRuleSecondaryField")
                                            If (.ManyToManyRuleContent <> "") And (.ManyToManyRulePrimaryField <> "") And (.ManyToManyRuleSecondaryField <> "") Then
                                                '
                                                ' Populate the Feed Entries
                                                '
                                                ContentName = CP.Content.GetRecordName("content", .ContentID)
                                                ' Do cs open here 
                                                If CS1.Open(.ManyToManyRuleContent) Then
                                                    'CSRules = Csv.OpenCSContent(.ManyToManyRuleContent)
                                                    Do While CS1.OK
                                                        Dim FeedID As Integer = CS1.GetInteger(.ManyToManyRuleSecondaryField)
                                                        Dim RecordID As Integer = CS1.GetInteger(.ManyToManyRulePrimaryField)
                                                        If CS2.OpenRecord(ContentName, CType(RecordID, String)) Then
                                                            ' CSEntries = Csv.OpenCSContentRecord(ContentName, RecordID)
                                                            If CS2.OK Then
                                                                Dim RecordContentControlID As Integer = CS2.GetInteger("ContentControlID")
                                                                For FeedPtr = 0 To FeedCnt - 1
                                                                    If FeedID = Feeds(FeedPtr).Id Then
                                                                        With Feeds(FeedPtr)

                                                                            '
                                                                            ' Search for this contentid/recordid in this feed already.
                                                                            ' this can happen if blog entries and blog copy both have
                                                                            ' an RSS Feeds field. Both ccfields records get added to the
                                                                            ' feed, which duplicates the entries
                                                                            '
                                                                            Dim blogEntries As Models.Domain.BlogEntrieModel()
                                                                            blogEntries(EntryPtr) = New Contensive.Addons.aoRssFeed2.Models.Domain.BlogEntrieModel()
                                                                            For EntryPtr = 0 To .EntryCnt Step -1
                                                                                If (.Entries(EntryPtr).RecordID = RecordID) And (.Entries(EntryPtr).ContentID = RecordContentControlID) Then
                                                                                    Exit For
                                                                                End If
                                                                            Next
                                                                            If EntryPtr < .EntryCnt Then
                                                                                '
                                                                                ' dup found and skipped
                                                                                '
                                                                            Else
                                                                                EntryPtr = .EntryCnt
                                                                                If EntryPtr >= .EntrySize Then
                                                                                    .EntrySize = .EntrySize + 30
                                                                                    ReDim Preserve .Entries(.EntrySize)
                                                                                End If
                                                                                Dim EntryDateAdded As Date = CDate("0")
                                                                                With .Entries(EntryPtr)
                                                                                    Dim EntryName As String = ""
                                                                                    .TableID = TableID
                                                                                    .ContentID = RecordContentControlID
                                                                                    .RecordID = RecordID
                                                                                    'CSEntries = Csv.OpenCSContentRecord(ContentName, RecordID)
                                                                                    'If Csv.iscsok(CSEntries) Then
                                                                                    EntryDateAdded = CS2.GetDate("DateAdded")
                                                                                    Dim CSEntries As Integer
                                                                                    If CP.Content.IsField(CType(CSEntries, String), "RSSTitle") Then
                                                                                        EntryName = CS2.GetText("RSSTitle")
                                                                                    End If
                                                                                    If EntryName = "" Then
                                                                                        EntryName = CS2.GetText("Name")
                                                                                    End If
                                                                                    .Title = EntryName
                                                                                    If CP.Content.IsField(CType(CSEntries, String), "RSSDateExpire") Then
                                                                                        .DateExpires = CS2.GetDate("RSSDateExpire")
                                                                                    End If
                                                                                    .DatePublish = CDate("0")
                                                                                    If CP.Content.IsField(CType(CSEntries, String), "RSSDatePublish") Then
                                                                                        .DatePublish = CS2.GetDate("RSSDatePublish")
                                                                                        If .DatePublish < #8/7/1990# Then
                                                                                            .DatePublish = CDate("0")
                                                                                        End If
                                                                                    End If
                                                                                    If (.DatePublish = CDate("0")) Then
                                                                                        .DatePublish = CS2.GetDate("dateAdded")
                                                                                        If (.DatePublish = CDate("0")) Then
                                                                                            .DatePublish = RightNow
                                                                                            Call CS2.SetField("dateAdded", CType(.DatePublish, String))
                                                                                            If CP.Content.IsField(CType(CSEntries, String), "RSSDatePublish") Then
                                                                                                Call CS2.SetField("RSSDatePublish", CType(.DatePublish, String))
                                                                                            End If
                                                                                        End If
                                                                                    End If
                                                                                    If CP.Content.IsField(CType(CSEntries, String), "RSSDescription") Then
                                                                                        .Description = CS1.GetText("RSSDescription")
                                                                                    End If
                                                                                    If CP.Content.IsField(CType(CSEntries, String), "RSSLink") Then
                                                                                        .Link = CS2.GetText("RSSLink")
                                                                                    End If
                                                                                    If CP.Content.IsField(CType(CSEntries, String), "PodcastMediaLink") Then
                                                                                        .PodcastMediaLink = CS2.GetText("PodcastMediaLink")
                                                                                    End If
                                                                                    DaysAgo = (CDbl(RightNow.ToString) - CDbl(EntryDateAdded.ToString))
                                                                                    Dim OrderBy As String = CStr(Int(DaysAgo * 86400.0!))
                                                                                    OrderBy = CStr(12 - Len(OrderBy)) & OrderBy
                                                                                    Hint = Hint & "," & "Entry [" & EntryName & "], EntryDateAdded [" & EntryDateAdded & "], DaysAgo [" & DaysAgo & "], orderby [" & OrderBy & "]"
                                                                                    Call Feeds(FeedPtr).EntryDateIndex.Add(OrderBy, EntryPtr)
                                                                                End With
                                                                                .EntryCnt = EntryPtr + 1
                                                                            End If
                                                                        End With
                                                                        Exit For
                                                                    End If
                                                                Next
                                                            End If

                                                        End If
                                                        CS2.Close()
                                                        CS1.GoNext
                                                    Loop
                                                End If
                                                CS1.Close()
                                            End If
                                        End With
                                    End If
                                End If
                            End If
                        Next
                    End If
                    Dim FeedContentCnt As Integer
                    'Call Csv.CloseCS(CSFields)
                    FeedContentCnt = FeedContentPtr
                    Hint = Hint & "," & "FeedContentCnt=" & FeedContentCnt
                    '
                    ' Build the Feeds from the Feed Arrays
                    '
                    For FeedPtr = 0 To FeedCnt - 1
                        Doc = New Xml.XmlDocument
                        With Feeds(FeedPtr)
                            Hint = Hint & "," & "Building Feed " & FeedPtr
                            '
                            ' Build this feed
                            '
                            If (.Link <> "") And (InStr(1, .Link, "://") = 0) Then
                                .Link = "http://" & .Link
                            End If
                            '
                            BaseNode = Doc.CreateElement("rss")
                            Call BaseNode.Attributes.GetNamedItem("version", "2.0")
                            Call BaseNode.Attributes.GetNamedItem("xmlns:atom", "http://www.w3.org/2005/Atom")
                            Call Doc.AppendChild(BaseNode)
                            '
                            ChannelNode = Doc.CreateElement("channel")
                            Call BaseNode.AppendChild(ChannelNode)
                            '
                            Node = Doc.CreateElement("title")
                            Node.InnerText = .Name
                            Call ChannelNode.AppendChild(Node)
                            '
                            Node = Doc.CreateElement("description")
                            Node.InnerText = .Description
                            Call ChannelNode.AppendChild(Node)
                            '
                            ' per validator.org, atom:link should be in the channel
                            ' <atom:link href="http://dallas.example.com/rss.xml" rel="self" type="application/rss+xml" />
                            '
                            Node = Doc.CreateElement("atom:link")
                            Call Node.Attributes.GetNamedItem("href", "http://" & DomainName & "/RSS/" & .RSSFilename)
                            Call Node.Attributes.GetNamedItem("rel", "self")
                            Call Node.Attributes.GetNamedItem("type", "application/rss+xml")
                            Call ChannelNode.AppendChild(Node)
                            '
                            If .Link <> "" Then
                                Node = Doc.CreateElement("link")
                                Node.InnerText = .Link
                                Call ChannelNode.AppendChild(Node)
                            End If
                            Hint = Hint & "," & "100"
                            If .LogoFilename <> "" Then
                                '
                                ' Create Image
                                '
                                Hint = Hint & "," & "110"
                                ImageNode = Doc.CreateElement("image")
                                Call ChannelNode.AppendChild(ImageNode)
                                '
                                Hint = Hint & "," & "120"
                                Node = Doc.CreateElement("url")
                                Node.InnerText = "http://" & DomainName & "/" & CP.Site.AppPath & "/files/" & .LogoFilename
                                Call ImageNode.AppendChild(Node)
                                '
                                Hint = Hint & "," & "130"
                                Node = Doc.CreateElement("title")
                                Node.InnerText = .Name
                                Call ImageNode.AppendChild(Node)
                                '
                                Hint = Hint & "," & "140"
                                Node = Doc.CreateElement("description")
                                Node.InnerText = .Description
                                Call ImageNode.AppendChild(Node)
                                '
                                Hint = Hint & "," & "150"
                                If .Link <> "" Then
                                    Node = Doc.CreateElement("link")
                                    Node.InnerText = .Link
                                    Call ImageNode.AppendChild(Node)
                                End If
                            End If
                            Hint = Hint & "," & "200"
                            If .EntryCnt <= 0 Then
                                Hint = Hint & "," & "Feed has no entries"
                            Else
                                Hint = Hint & "," & "210"
                                LastTableID = -1
                                Dim LastRecordID As Integer = -1
                                IndexPtr = .EntryDateIndex(GetFirstPointer)
                                Hint = Hint & "," & "looking up all entries"
                                For EntryPtr = 0 To .EntryCnt - 1
                                    With .Entries(IndexPtr)
                                        Hint = Hint & "," & "220"
                                        If (LastTableID = .TableID) And (LastRecordID = .RecordID) Then
                                            '
                                            ' repeated entry - might be because content with RSS field has a parent content
                                            '
                                        Else
                                            Hint = Hint & "," & "230"
                                            ItemName = .Title
                                            ItemName = Trim(CP.Db.EncodeSQLText(ItemName))
                                            Hint = Hint & "," & "Adding item [" & ItemName & "]"
                                            If ItemName <> "" Then
                                                Hint = Hint & "," & "240"
                                                If (.DateExpires = CDate("0")) Or (.DateExpires > RightNow) Then
                                                    Hint = Hint & "," & "250"
                                                    If (.DatePublish = CDate("0")) Or (.DatePublish < RightNow) Then
                                                        Hint = Hint & "," & "260"
                                                        '
                                                        ' create Item Node
                                                        '
                                                        ItemNode = Doc.CreateElement("item")
                                                        Call ChannelNode.AppendChild(ItemNode)
                                                        '
                                                        Node = Doc.CreateElement("title")
                                                        Node.InnerText = ItemName
                                                        Call ItemNode.AppendChild(Node)
                                                        '
                                                        ItemDescription = .Description
                                                        ItemDescription = CP.Db.EncodeSQLText(ItemDescription)
                                                        Node = Doc.CreateElement("description")
                                                        Node.InnerText = ItemDescription
                                                        Call ItemNode.AppendChild(Node)
                                                        '
                                                        If (.DatePublish <> CDate("0")) Then
                                                            ItemDatePublishText = Controllers.genericController.GetGMTFromDate(.DatePublish)
                                                            Node = Doc.CreateElement("pubDate")
                                                            Node.InnerText = ItemDatePublishText
                                                            Call ItemNode.AppendChild(Node)
                                                        End If
                                                        '
                                                        Link = Trim(.Link)
                                                        If Link = "" Then
                                                            CS = CInt(CS3.OpenSQL("default", "select link from ccContentWatch where contentid=" & .ContentID & " and recordid=" & .RecordID, 0))
                                                            If CS3.OK Then
                                                                Link = CS3.GetText("link")
                                                            End If
                                                            Call CS3.Close()
                                                            Link = Trim(Link)
                                                            If Link = "" Then
                                                                If LCase(CP.Content.GetRecordName("", .ContentID)) = "page content" Then
                                                                    Link = Trim(CP.Site.GetProperty("SERVERPAGEDEFAULT", "") & "?bid=" & .RecordID)
                                                                End If
                                                            End If
                                                        End If
                                                        If InStr(1, Link, "://") = 0 Then
                                                            If Left(Link, 1) <> "/" Then
                                                                Link = "/" & Link
                                                            End If
                                                            Link = CP.Site.EncodeAppRootPath(Link)
                                                            Link = "http://" & DomainName & Link
                                                        End If
                                                        If Link <> "" Then
                                                            '
                                                            ' create link node
                                                            '
                                                            Node = Doc.CreateElement("link")
                                                            Node.InnerText = Link
                                                            Call ItemNode.AppendChild(Node)
                                                            '
                                                            ' create guid node
                                                            '
                                                            Node = Doc.CreateElement("guid")
                                                            Node.InnerText = Link
                                                            Call ItemNode.AppendChild(Node)
                                                        End If
                                                        '
                                                        '
                                                        Enclosure = .PodcastMediaLink
                                                        If Enclosure <> "" Then
                                                            Ptr = InStrRev(Enclosure, ".")
                                                            If Ptr > 0 Then
                                                                Ext = LCase(Mid(Enclosure, Ptr + 1))
                                                                Select Case Ext
                                                                    Case "mp3"
                                                                        MimeType = "audio/mpeg"
                                                                    Case "m4v"
                                                                        MimeType = "video/x-m4v"
                                                                    Case "m4a"
                                                                        MimeType = "audio/x-m4a"
                                                                    Case "avi"
                                                                        MimeType = "video/avi"
                                                                    Case "mpeg", "mpg"
                                                                        MimeType = "video/mpeg"
                                                                    Case "mp4"
                                                                        MimeType = "audio/mpeg"
                                                                    Case "qt", "mov"
                                                                        MimeType = "video/quicktime"
                                                                    Case "wma", "wmv", "asf"
                                                                        MimeType = "application/x-oleobject"
                                                                    Case "swf"
                                                                        MimeType = "application/x-shockwave-flash"
                                                                    Case "flv"
                                                                        MimeType = "application/x-shockwave-flash"
                                                                    Case "rm"
                                                                        MimeType = "audio/x-pn-realaudio-plugin"
                                                                    Case "youtube"
                                                                        MimeType = "application/x-shockwave-flash"
                                                                End Select
                                                            End If
                                                            '
                                                            Node = Doc.CreateElement("enclosure")

                                                            Node.SetAttribute("url", Enclosure)
                                                            If EnclosureLength <> 0 Then
                                                                Call Node.SetAttribute("length", "100000")
                                                            End If
                                                            If MimeType <> "" Then
                                                                Call Node.SetAttribute("type", MimeType)
                                                            End If
                                                            Call ItemNode.AppendChild(Node)
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                        LastTableID = .TableID
                                        LastRecordID = .RecordID
                                    End With
                                    IndexPtr = CP.Utils.EncodeInteger(.EntryDateIndex(GetNextPointer))
                                Next
                            End If
                            '
                            ' -- initialize application. If authentication needed and not login page, pass true
                            Dim encoding As String = CP.Site.GetProperty("Site Character Encoding", "windows-1252")
                            If encoding = "" Then
                                encoding = "windows-1252"
                            End If
                            s = "" _
                    & "<?xml version=""1.0"" encoding=""" & encoding & """?>" _
                    & vbCrLf & Doc.InnerXml
                            Hint = Hint & "," & "saving feed as [" & "RSS/" & .RSSFilename & "], len(s)=" & Len(s)
                            Call CP.File.Save("RSS/" & .RSSFilename, s)
                        End With
                    Next
                End If
                Call LogEvent(CP, "Execute", "hint=" & Hint)
                '
            Catch ex As Exception
                CP.Site.ErrorReport(ex)
            End Try
            Return result
        End Function
        '
        '

        Public Sub LogEvent(cp As CPBaseClass, MethodName As String, LogCopy As String)
            Dim result As String = ""
            Try
                '
                ' ----- Append to the Content Server Log File
                '
                'Call AppendLogFile(App.EXEName & "." & MethodName & ", " & LogCopy)
                Exit Sub
                '
            Catch ex As Exception
                cp.Site.ErrorReport(ex)
            End Try
            'Return result
        End Sub
        '
        '
        '
        Private Function GetPrimaryDomainName(cp As CPBaseClass, DomainNameList As String) As String
            Dim result As String = ""
            Try
                '
                Dim CopySplit As Object
                '
                GetPrimaryDomainName = DomainNameList
                If InStr(1, GetPrimaryDomainName, ",", CType(1, CompareMethod)) <> 0 Then
                    CopySplit = Split(cp.Site.DomainPrimary(), ",")
                    GetPrimaryDomainName = CopySplit(0)
                End If
                '
            Catch ex As Exception
                cp.Site.ErrorReport(ex)
            End Try
            Return result
            '

        End Function
        '
        '
        '
        Private Function GetEmailStyles(cp As CPBaseClass, CSEmail As String) As String
            Dim result As String = ""
            Try

                '
                Dim BuildVersion As String
                '
                BuildVersion = cp.Site.GetProperty("BuildVersion", "0")
                If BuildVersion >= "3.3.291" Then
                    GetEmailStyles = cp.Doc.GetText(CSEmail, "InlineStyles")
                End If
                If GetEmailStyles = "" Then
                    GetEmailStyles = "<link rel=""stylesheet"" href=""http://" & GetPrimaryDomainName(cp, cp.Site.DomainList) & cp.Site.PhysicalWWWPath & "styles.css"" type=""text/css"">"
                End If
                '
                result = GetEmailStyles
            Catch ex As Exception
                cp.Site.ErrorReport(ex)
            End Try
            Return result
        End Function
        '
        '
        '
        Private Sub BuildRSS(cp As CPBaseClass, RSSFeedName As String, RSSFeedID As Integer, RSFilename As String)
            Dim result As String = ""
            Try
                '
                '
                '
                '
                Exit Sub
                ' 
            Catch ex As Exception
                cp.Site.ErrorReport(ex)
            End Try

        End Sub
        '
        '
        '
        Private Function EncodeXMLText(cp As CPBaseClass, src As String) As String
            Dim result As String = ""
            Try
                '
                result = "<![CDATA[" & src & "]]>"
                '

                '
            Catch ex As Exception
                cp.Site.ErrorReport(ex)
            End Try
            Return result
        End Function
        '
        '
        '
        Private Function testEncodeFilename(Source) As String
            Dim allowed As String
            Dim chr As String
            Dim Ptr As Integer
            Dim cnt As Integer
            Dim returnString As String
            '
            'returnString = source
            returnString = ""
            cnt = Len(Source)
            If cnt > 254 Then
                cnt = 254
            End If
            allowed = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ^&'@{}[],$-#()%.+~_"
            For Ptr = 1 To cnt
                chr = Mid(CType(Source, String), Ptr, 1)
                If CBool(InStr(1, allowed, chr, vbBinaryCompare)) Then
                    returnString = returnString & chr
                End If
            Next
            testEncodeFilename = returnString
        End Function
        '
        '
        '
        'Private Sub HandleClassError(Cause As String, Method As String)
        '    Call HandleError2(Csv.ApplicationNameLocal, Cause, App.EXEName, "ProcessClass", Method, Err.Number, Err.Source, Err.Description, True, False, "")
        'End Sub

    End Class
End Namespace
