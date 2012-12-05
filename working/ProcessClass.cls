VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ProcessClass"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True

Option Explicit
'
'========================================================================
'
'========================================================================
'
'Private Main As ccWeb3.MainClass
'Private Csv As ccCSrvr3.ContentServerClass
Private Csv As Object
Private Main As Object
'
'
'
Private Type FeedContentType
    ContentID As Long
    TableID As Long
    ManyToManyRuleContent As String
    ManyToManyRulePrimaryField As String
    ManyToManyRuleSecondaryField As String
End Type
'
'
'
Private Type EntryType
    Title As String
    Description As String
    DatePublish As Date
    DateExpires As Date
    Link As String
    ContentID As Long
    TableID As Long
    RecordID As Long
    PodcastMediaLink As String
End Type
'
'
'
Private Type FeedType
    Id As Long
    Name As String
    Description As String
    Link As String
    LogoFilename As String
    RSSFilename As String
    Entries() As EntryType
    EntryCnt As Long
    EntrySize As Long
    EntryDateIndex As FastIndex4.FastIndexClass
End Type
'
Private Feeds() As FeedType
Private FeedCnt As Long
Private FeedSize As Long
'
Private FeedContent() As FeedContentType
Private FeedContentCnt As Long
Private FeedContentSize As Long
'
'=================================================================================
'   Execute Method, v3.4 Interface
'
'   To Process RSS Feeds
'
'   - Open ccFields looking for "RSS Feeds" many-to-many fields
'       - ContentID tells us where the records are
'       - ManyToManyRuleContent tells us the Rule table that connections RSSFeedID to RecordID
'
'   - content watch gives us all the links
'   - Load a User Type array for each feed, indexing the DateAdded (Modifieddate?)
'   - construct the feeds from the arrays after all content is loaded
'
'=================================================================================
'
Public Function execute(CsvObject As Object, MainObject As Object, Optionstring As String, FilterInput As String) As String
    On Error GoTo ErrorTrap
    '
    Dim encoding As String
    Dim testFilenameRoot As String
    Dim suffixNumber As Long
    Dim testfilename As String
    Dim LastRecordID As Long
    Dim RecordContentControlID As Long
    Dim TableID As Long
    Dim EntryDateAdded As Date
    Dim EntryName As String
    Dim OrderBy As String
    Dim FeedContentPtr As Long
    Dim ContentID As Long
    Dim CSEntries As Long
    Dim CSRules As Long
    Dim FeedID As Long
    Dim EntryPtr As Long
    Dim CSFields As Long
    Dim RecordID As Long
    Dim RuleContentID As Long
    Dim ContentName As String
    Dim BlogURL As String
    Dim SQLDateNow As String
    Dim LoopPtr As Long
    Dim LoopCnt As Long
    Dim CS As Long
    Dim RSSFilename As String
    Dim RSSLink As String
    Dim s As String
    Dim PageID As Long
    Dim Ext As String
    Dim MimeType As String
    Dim EnclosureLength As Long
    Dim Ptr As Long
    Dim Enclosure As String
    Dim ItemName As String
    Dim ItemDescription As String
    Dim SQL As String
    Dim Link As String
    Dim Pos As Long
    Dim Name As String
    Dim Description As String
    Dim LastTableID As Long
    Dim ServerContentWatchPrefix As String
    Dim DomainName As String
    Dim LogoFilename As String
    Dim ItemDatePublishText As String
    Dim SQLNow As String
    Dim FeedPtr As Long
    Dim Hint As String
    Dim RightNow As Date
    Dim DaysAgo As Double
    Dim Doc As DOMDocument60
    Dim BaseNode As IXMLDOMElement
    Dim Node As IXMLDOMElement
    Dim ChannelNode As IXMLDOMElement
    Dim ImageNode As IXMLDOMElement
    Dim ItemNode As IXMLDOMElement
    Dim IndexPtr As Long
    Dim usedFilenames As String
    '
    Set Main = MainObject
    Set Csv = CsvObject
    '
    Call LogEvent("execute", "App [" & Csv.ApplicationNameLocal & "], RSS Feed version " & App.Major & "." & App.Minor & "." & App.Revision & ", entering execute")
    Hint = "entering"
    If Not Csv.IsContentFieldSupported("RSS Feeds", "ID") Then
        Hint = "This site does not have an RSS Feed content."
    Else
        RightNow = Now
        SQLDateNow = KmaEncodeSQLDate(RightNow)
        '
        ' Get Primary domain from domain list
        '
        DomainName = Csv.DomainName
        Pos = InStr(1, DomainName, ",")
        If Pos > 2 Then
            DomainName = Mid(DomainName, 1, Pos - 1)
        End If
        '
        ' Build Prefix for content watch
        '
        ServerContentWatchPrefix = "http://" & DomainName & Csv.RootPath
        ServerContentWatchPrefix = Mid(ServerContentWatchPrefix, 1, Len(ServerContentWatchPrefix) - 1)
        '
        ' Load the feeds
        '
        CS = Csv.OpenCSContent("RSS Feeds")
        If Not Csv.IsCSOK(CS) Then
            Hint = Hint & "," & "RSS Feeds does not support ID (content not there)"
        Else
            Do While Csv.IsCSOK(CS)
                If FeedSize <= FeedPtr Then
                    FeedSize = FeedSize + 10
                    ReDim Preserve Feeds(FeedSize)
                End If
                Feeds(FeedPtr).Id = Csv.GetCSInteger(CS, "id")
                Feeds(FeedPtr).Name = Csv.GetCSText(CS, "name")
                Feeds(FeedPtr).Description = Csv.GetCSText(CS, "Description")
                Feeds(FeedPtr).Link = Csv.GetCSText(CS, "Link")
                Feeds(FeedPtr).LogoFilename = Csv.GetCSText(CS, "LogoFilename")
                If Feeds(FeedPtr).Link = "" Then
                    Feeds(FeedPtr).Link = ServerContentWatchPrefix
                End If
                Set Feeds(FeedPtr).EntryDateIndex = New FastIndexClass
                RSSFilename = Csv.GetCSText(CS, "RSSFilename")
                testfilename = RSSFilename
                If testfilename = "" Then
                    testfilename = Csv.GetCSText(CS, "Name")
                End If
                If testfilename = "" Then
                    testfilename = "RSSFeed" & Csv.GetCSText(CS, "id")
                End If
                Pos = InStr(1, testfilename, ".xml", vbTextCompare)
                If Pos > 0 Then
                    testfilename = Mid(testfilename, 1, Pos - 1)
                End If
                testfilename = testEncodeFilename(testfilename)
'                testfilename = Replace(testfilename, " ", "")
'                testfilename = Replace(testfilename, "/", "")
'                testfilename = Replace(testfilename, "\", "")
'                'testFilename = Replace(testFilename, ".", "")
'                testfilename = Replace(testfilename, ",", "")
'                testfilename = Replace(testfilename, "'", "")
'                testfilename = Replace(testfilename, """", "")
                suffixNumber = 1
                testfilename = encodeFilename(testfilename)
                testFilenameRoot = testfilename
                Do While (suffixNumber < 100) And (InStr(1, "," & usedFilenames & ",", "," & testfilename & ",", vbTextCompare) <> 0)
                    testfilename = testFilenameRoot & CStr(suffixNumber)
                    suffixNumber = suffixNumber + 1
                Loop
                usedFilenames = usedFilenames & "," & testfilename
                testfilename = testfilename & ".xml"
                If testfilename <> RSSFilename Then
                    RSSFilename = testfilename
                    Call Csv.SetCS(CS, "RSSFilename", RSSFilename)
                End If
                Feeds(FeedPtr).RSSFilename = RSSFilename
                FeedPtr = FeedPtr + 1
                Call Csv.SetCS(CS, "RSSDateUpdated", RightNow)
                Call Csv.SaveCSRecord(CS)
                Call Csv.NextCSRecord(CS)
            Loop
            Hint = Hint & "," & "RSS Feed Cnt=" & FeedPtr
        End If
        Call Csv.CloseCS(CS)
        FeedCnt = FeedPtr
        '
        ' Find all the content that implements rss feeds
        '
        FeedContentPtr = 0
        LastTableID = -1
        CSFields = Csv.OpenCSContent("Content Fields", "(name='RSSFeeds')and(type=" & FieldTypeManyToMany & ")and(authorable<>0)", "ContentID")
        If Not Csv.IsCSOK(CSFields) Then
            Hint = Hint & "," & "There are no 'content fields' named RSSFields with type=manytomany"
        Else
            Hint = Hint & "," & "going through all content fields records with name=RSSFields and type=manytomany"
            Do While Csv.IsCSOK(CSFields)
                Hint = Hint & "," & "ccfields record " & Csv.GetCSInteger(CSFields, "id")
                ContentID = Csv.GetCSInteger(CSFields, "ContentID")
                If ContentID <> 0 Then
                    ContentName = Csv.GetContentNameByID(ContentID)
                    If ContentName <> "" Then
                        TableID = Csv.GetContentTableID(ContentName)
                        If TableID = LastTableID Then
                            Hint = Hint & "," & "field contentid matchs the last row, skipping"
                        Else
                            '
                            ' Populate the FeedContent Array (might not be necessary)
                            '
                            LastTableID = TableID
                            ReDim Preserve FeedContent(FeedContentPtr)
                            With FeedContent(FeedContentPtr)
                                .ContentID = ContentID
                                .TableID = TableID
                                RuleContentID = Csv.GetCSText(CSFields, "ManyToManyRuleContentID")
                                .ManyToManyRuleContent = Csv.GetContentNameByID(RuleContentID)
                                .ManyToManyRulePrimaryField = Csv.GetCSText(CSFields, "ManyToManyRulePrimaryField")
                                .ManyToManyRuleSecondaryField = Csv.GetCSText(CSFields, "ManyToManyRuleSecondaryField")
                                If (.ManyToManyRuleContent <> "") And (.ManyToManyRulePrimaryField <> "") And (.ManyToManyRuleSecondaryField <> "") Then
                                    '
                                    ' Populate the Feed Entries
                                    '
                                    ContentName = Csv.GetContentNameByID(.ContentID)
                                    CSRules = Csv.OpenCSContent(.ManyToManyRuleContent)
                                    Do While Csv.IsCSOK(CSRules)
                                        FeedID = Csv.GetCSInteger(CSRules, .ManyToManyRuleSecondaryField)
                                        RecordID = Csv.GetCSInteger(CSRules, .ManyToManyRulePrimaryField)
                                        CSEntries = Csv.OpenCSContentRecord(ContentName, RecordID)
                                        If Csv.IsCSOK(CSEntries) Then
                                            RecordContentControlID = Csv.GetCSInteger(CSEntries, "ContentControlID")
                                            For FeedPtr = 0 To FeedCnt - 1
                                                If FeedID = Feeds(FeedPtr).Id Then
                                                    With Feeds(FeedPtr)
                                                        '
                                                        ' Search for this contentid/recordid in this feed already.
                                                        ' this can happen if blog entries and blog copy both have
                                                        ' an RSS Feeds field. Both ccfields records get added to the
                                                        ' feed, which duplicates the entries
                                                        '
                                                        For EntryPtr = 0 To .EntryCnt - 1
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
                                                            EntryDateAdded = CDate(0)
                                                            With .Entries(EntryPtr)
                                                                EntryName = ""
                                                                .TableID = TableID
                                                                .ContentID = RecordContentControlID
                                                                .RecordID = RecordID
                                                                'CSEntries = Csv.OpenCSContentRecord(ContentName, RecordID)
                                                                'If Csv.iscsok(CSEntries) Then
                                                                EntryDateAdded = Csv.GetCSDate(CSEntries, "DateAdded")
                                                                If Csv.IsCSFieldSupported(CSEntries, "RSSTitle") Then
                                                                    EntryName = Csv.GetCSText(CSEntries, "RSSTitle")
                                                                End If
                                                                If EntryName = "" Then
                                                                    EntryName = Csv.GetCSText(CSEntries, "Name")
                                                                End If
                                                                .Title = EntryName
                                                                If Csv.IsCSFieldSupported(CSEntries, "RSSDateExpire") Then
                                                                    .DateExpires = Csv.GetCSDate(CSEntries, "RSSDateExpire")
                                                                End If
                                                                .DatePublish = CDate(0)
                                                                If Csv.IsCSFieldSupported(CSEntries, "RSSDatePublish") Then
                                                                    .DatePublish = Csv.GetCSDate(CSEntries, "RSSDatePublish")
                                                                    If .DatePublish < #8/7/1990# Then
                                                                        .DatePublish = CDate(0)
                                                                    End If
                                                                End If
                                                                If (.DatePublish = CDate(0)) Then
                                                                    .DatePublish = Csv.GetCSDate(CSEntries, "dateAdded")
                                                                    If (.DatePublish = CDate(0)) Then
                                                                        .DatePublish = RightNow
                                                                        Call Csv.SetCS(CSEntries, "dateAdded", .DatePublish)
                                                                        If Csv.IsCSFieldSupported(CSEntries, "RSSDatePublish") Then
                                                                            Call Csv.SetCS(CSEntries, "RSSDatePublish", .DatePublish)
                                                                        End If
                                                                    End If
                                                                End If
                                                                If Csv.IsCSFieldSupported(CSEntries, "RSSDescription") Then
                                                                    .Description = Csv.GetCSText(CSEntries, "RSSDescription")
                                                                End If
                                                                If Csv.IsCSFieldSupported(CSEntries, "RSSLink") Then
                                                                    .Link = Csv.GetCSText(CSEntries, "RSSLink")
                                                                End If
                                                                If Csv.IsCSFieldSupported(CSEntries, "PodcastMediaLink") Then
                                                                    .PodcastMediaLink = Csv.GetCSText(CSEntries, "PodcastMediaLink")
                                                                End If
                                                                DaysAgo = (CDbl(RightNow) - CDbl(EntryDateAdded))
                                                                OrderBy = CStr(Int(DaysAgo * 86400!))
                                                                OrderBy = String(12 - Len(OrderBy), "0") & OrderBy
                                                                Hint = Hint & "," & "Entry [" & EntryName & "], EntryDateAdded [" & EntryDateAdded & "], DaysAgo [" & DaysAgo & "], orderby [" & OrderBy & "]"
                                                                Call Feeds(FeedPtr).EntryDateIndex.SetPointer(OrderBy, EntryPtr)
                                                                'End If
                                                                'Call Csv.closecs(CSEntries)
                                                            End With
                                                            .EntryCnt = EntryPtr + 1
                                                        End If
                                                    End With
                                                    Exit For
                                                End If
                                            Next
                                        End If
                                        Call Csv.CloseCS(CSEntries)
                                        Call Csv.NextCSRecord(CSRules)
                                    Loop
                                    Call Csv.CloseCS(CSRules)
                                End If
                            End With
                        End If
                    End If
                End If
                Call Csv.NextCSRecord(CSFields)
            Loop
        End If
        Call Csv.CloseCS(CSFields)
        FeedContentCnt = FeedContentPtr
        Hint = Hint & "," & "FeedContentCnt=" & FeedContentCnt
        '
        ' Build the Feeds from the Feed Arrays
        '
        For FeedPtr = 0 To FeedCnt - 1
            Set Doc = New DOMDocument60
            With Feeds(FeedPtr)
                Hint = Hint & "," & "Building Feed " & FeedPtr
                '
                ' Build this feed
                '
                If (.Link <> "") And (InStr(1, .Link, "://") = 0) Then
                    .Link = "http://" & .Link
                End If
                '
                Set BaseNode = Doc.createElement("rss")
                Call BaseNode.setAttribute("version", "2.0")
                Call BaseNode.setAttribute("xmlns:atom", "http://www.w3.org/2005/Atom")
                Call Doc.appendChild(BaseNode)
                '
                Set ChannelNode = Doc.createElement("channel")
                Call BaseNode.appendChild(ChannelNode)
                '
                Set Node = Doc.createElement("title")
                Node.Text = .Name
                Call ChannelNode.appendChild(Node)
                '
                Set Node = Doc.createElement("description")
                Node.Text = .Description
                Call ChannelNode.appendChild(Node)
                '
                ' per validator.org, atom:link should be in the channel
                ' <atom:link href="http://dallas.example.com/rss.xml" rel="self" type="application/rss+xml" />
                '
                Set Node = Doc.createElement("atom:link")
                Call Node.setAttribute("href", "http://" & DomainName & "/RSS/" & .RSSFilename)
                Call Node.setAttribute("rel", "self")
                Call Node.setAttribute("type", "application/rss+xml")
                Call ChannelNode.appendChild(Node)
                '
                If .Link <> "" Then
                    Set Node = Doc.createElement("link")
                    Node.Text = .Link
                    Call ChannelNode.appendChild(Node)
                End If
                Hint = Hint & "," & "100"
                If .LogoFilename <> "" Then
                    '
                    ' Create Image
                    '
                    Hint = Hint & "," & "110"
                    Set ImageNode = Doc.createElement("image")
                    Call ChannelNode.appendChild(ImageNode)
                    '
                    Hint = Hint & "," & "120"
                    Set Node = Doc.createElement("url")
                    Node.Text = "http://" & DomainName & "/" & Csv.ApplicationNameLocal & "/files/" & .LogoFilename
                    Call ImageNode.appendChild(Node)
                    '
                    Hint = Hint & "," & "130"
                    Set Node = Doc.createElement("title")
                    Node.Text = .Name
                    Call ImageNode.appendChild(Node)
                    '
                    Hint = Hint & "," & "140"
                    Set Node = Doc.createElement("description")
                    Node.Text = .Description
                    Call ImageNode.appendChild(Node)
                    '
                    Hint = Hint & "," & "150"
                    If .Link <> "" Then
                        Set Node = Doc.createElement("link")
                        Node.Text = .Link
                        Call ImageNode.appendChild(Node)
                    End If
                End If
                Hint = Hint & "," & "200"
                If .EntryCnt <= 0 Then
                    Hint = Hint & "," & "Feed has no entries"
                Else
                    Hint = Hint & "," & "210"
                    LastTableID = -1
                    LastRecordID = -1
                    IndexPtr = .EntryDateIndex.GetFirstPointer
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
                                ItemName = Trim(Csv.EncodeContent(ItemName))
                                Hint = Hint & "," & "Adding item [" & ItemName & "]"
                                If ItemName <> "" Then
                                    Hint = Hint & "," & "240"
                                    If (.DateExpires = CDate(0)) Or (.DateExpires > RightNow) Then
                                        Hint = Hint & "," & "250"
                                        If (.DatePublish = CDate(0)) Or (.DatePublish < RightNow) Then
                                            Hint = Hint & "," & "260"
                                            '
                                            ' create Item Node
                                            '
                                            Set ItemNode = Doc.createElement("item")
                                            Call ChannelNode.appendChild(ItemNode)
                                            '
                                            Set Node = Doc.createElement("title")
                                            Node.Text = ItemName
                                            Call ItemNode.appendChild(Node)
                                            '
                                            ItemDescription = .Description
                                            ItemDescription = Csv.EncodeContent(ItemDescription)
                                            Set Node = Doc.createElement("description")
                                            Node.Text = ItemDescription
                                            Call ItemNode.appendChild(Node)
                                            '
                                            If (.DatePublish <> CDate(0)) Then
                                                ItemDatePublishText = GetGMTFromDate(.DatePublish)
                                                Set Node = Doc.createElement("pubDate")
                                                Node.Text = ItemDatePublishText
                                                Call ItemNode.appendChild(Node)
                                            End If
                                            '
                                            Link = Trim(.Link)
                                            If Link = "" Then
                                                CS = Csv.OpenCSSQL("default", "select link from ccContentWatch where contentid=" & .ContentID & " and recordid=" & .RecordID, 0)
                                                If Csv.IsCSOK(CS) Then
                                                    Link = Csv.GetCSText(CS, "link")
                                                End If
                                                Call Csv.CloseCS(CS)
                                                Link = Trim(Link)
                                                If Link = "" Then
                                                    If LCase(Csv.GetContentNameByID(.ContentID)) = "page content" Then
                                                        Link = Trim(Csv.GetSiteProperty("SERVERPAGEDEFAULT", "", 0) & "?bid=" & .RecordID)
                                                    End If
                                                End If
                                            End If
                                            If InStr(1, Link, "://") = 0 Then
                                                If Left(Link, 1) <> "/" Then
                                                    Link = "/" & Link
                                                End If
                                                Link = kmaEncodeAppRootPath(Link, "/" & Csv.ApplicationNameLocal, Csv.RootPath, DomainName)
                                                Link = "http://" & DomainName & Link
                                            End If
                                            If Link <> "" Then
                                                '
                                                ' create link node
                                                '
                                                Set Node = Doc.createElement("link")
                                                Node.Text = Link
                                                Call ItemNode.appendChild(Node)
                                                '
                                                ' create guid node
                                                '
                                                Set Node = Doc.createElement("guid")
                                                Node.Text = Link
                                                Call ItemNode.appendChild(Node)
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
                                                Set Node = Doc.createElement("enclosure")
                                                Call Node.setAttribute("url", Enclosure)
                                                If EnclosureLength <> 0 Then
                                                    Call Node.setAttribute("length", "100000")
                                                End If
                                                If MimeType <> "" Then
                                                    Call Node.setAttribute("type", MimeType)
                                                End If
                                                Call ItemNode.appendChild(Node)
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                            LastTableID = .TableID
                            LastRecordID = .RecordID
                        End With
                        IndexPtr = kmaEncodeInteger(.EntryDateIndex.GetNextPointer)
                    Next
                End If
                encoding = Csv.GetSiteProperty("Site Character Encoding", "windows-1252", 0)
                If encoding = "" Then
                    encoding = "windows-1252"
                End If
                s = "" _
                    & "<?xml version=""1.0"" encoding=""" & encoding & """?>" _
                    & vbCrLf & Doc.xml
                Hint = Hint & "," & "saving feed as [" & "RSS/" & .RSSFilename & "], len(s)=" & Len(s)
                Call Csv.SaveVirtualFile("RSS/" & .RSSFilename, s)
            End With
        Next
    End If
    Call LogEvent("Execute", "hint=" & Hint)
    '
    Exit Function
ErrorTrap:
    Call HandleClassError("trap", "execute, hint=" & Hint)
End Function
'
'======================================================================================
'   Log a reported error
'======================================================================================
'
Public Sub LogEvent(MethodName As String, LogCopy As String)
    On Error GoTo ErrorTrap
    '
    ' ----- Append to the Content Server Log File
    '
    Call AppendLogFile(App.EXEName & "." & MethodName & ", " & LogCopy)
    Exit Sub
    '
ErrorTrap:
    Call HandleClassError("trap", "LogEvent")
End Sub
'
'
'
Private Function GetPrimaryDomainName(DomainNameList As String) As String
    On Error GoTo ErrorTrap
    '
    Dim CopySplit
    '
    GetPrimaryDomainName = DomainNameList
    If InStr(1, GetPrimaryDomainName, ",", 1) <> 0 Then
        CopySplit = Split(GetPrimaryDomainName, ",")
        GetPrimaryDomainName = CopySplit(0)
    End If
    '
    Exit Function
    '
ErrorTrap:
    Call HandleClassError("trap", "GetPrimaryDomainName")
End Function
'
'
'
Private Function GetEmailStyles(CSEmail As Long) As String
    On Error GoTo ErrorTrap
    '
    Dim BuildVersion As String
    '
    BuildVersion = Csv.GetSiteProperty("BuildVersion", 0, 0)
    If BuildVersion >= "3.3.291" Then
        GetEmailStyles = Csv.GetCS(CSEmail, "InlineStyles")
    End If
    If GetEmailStyles = "" Then
        GetEmailStyles = "<link rel=""stylesheet"" href=""http://" & GetPrimaryDomainName(Csv.DomainName) & Csv.RootPath & "styles.css"" type=""text/css"">"
    End If
    '
    Exit Function
    '
ErrorTrap:
    Call HandleClassError("trap", "GetEmailStyles")
End Function
'
'
'
Private Sub BuildRSS(RSSFeedName As String, RSSFeedID As Long, RSFilename As String)
    On Error GoTo ErrorTrap
    '
    '
    '
    '
    Exit Sub
    '
ErrorTrap:
    Call HandleClassError("trap", "BuildRSS")
End Sub
'
'
'
Private Function EncodeXMLText(src As String) As String
    On Error GoTo ErrorTrap
    '
    EncodeXMLText = "<![CDATA[" & src & "]]>"
    '
    Exit Function
    '
ErrorTrap:
    Call HandleClassError("trap", "EncodeXMLText")
End Function
'
'
'
Private Function testEncodeFilename(Source) As String
    Dim allowed As String
    Dim chr As String
    Dim Ptr As Long
    Dim cnt As Long
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
        chr = Mid(Source, Ptr, 1)
        If InStr(1, allowed, chr, vbBinaryCompare) Then
            returnString = returnString & chr
        End If
    Next
    testEncodeFilename = returnString
End Function
'
'
'
Private Sub HandleClassError(Cause As String, Method As String)
    Call HandleError2(Csv.ApplicationNameLocal, Cause, App.EXEName, "ProcessClass", Method, Err.Number, Err.Source, Err.Description, True, False, "")
End Sub

