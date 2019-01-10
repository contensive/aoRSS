VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "QuickClass"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True

Option Explicit
'
Const RSSRootNode = "rss"
Const AtomRootNode = "feed"
'
'Private Main As ccWeb3.MainClass
Private main As Object
Private csv As Object
'
Type EnclosureType
    URL As String
    Type As String
    Length As String
End Type
'
'========================================================================
'   v3.3 Compatibility
'       To make an Add-on that works the same in v3.3 and v3.4, use this adapter instead of the execute above
'========================================================================
'
Public Function Execute(CsvObject As Object, MainObject As Object, OptionString As String, FilterInput As String) As String
    Set csv = CsvObject
    Call Init(MainObject)
    Execute = GetContent(OptionString)
End Function
'
'========================================================================
'   Init()
'========================================================================
'
Public Sub Init(MainObject As Object)
    Set main = MainObject
    Exit Sub
    '
    ' ----- Error Trap
    '
ErrorTrap:
    'Call HandleError("AggrSampleClass", "Init", Err.Number, Err.Source, Err.Description, True, False)
End Sub
'
'=================================================================================
'   Aggregate Object Interface
'=================================================================================
'
Public Function GetContent(OptionString As String) As String
    On Error GoTo ErrorTrap
    '
    Dim IsRSS As Boolean
    Dim isAtom As Boolean
    '
    Dim ItemPubDate As String
    Dim EnclosureRow As String
    Dim Ptr As Long
    Dim Found As Boolean
    Dim EnclosureCnt As Long
    Dim Enclosure() As EnclosureType
    Dim ChannelImage As String
    Dim ChannelTitle As String
    Dim ChannelDescription As String
    Dim ChannelPubDate As String
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
    Dim FeedHeader As String
    Dim VersionString As String
    Dim UserError As String
    Dim LastRefresh As Date
    Dim RefreshHours As Double
    Dim Feed As String
    Dim FeedConfig As String
    Dim ConfigHeader As String
    Dim ConfigSplit() As String
    Dim doc As DOMDocument30
    Dim RootNode As IXMLDOMNode
    Dim ChannelNode As IXMLDOMNode
    Dim ItemNode As IXMLDOMNode
    Dim ImageNode As IXMLDOMNode
    Dim LoopPtr As Long
    Dim FeedFilename As String
    Dim Link As String
    Dim SaveCache As Boolean
    Dim MaxStories As Long
    Dim instanceId As String
    '
    If Not (main Is Nothing) Then
        SaveCache = False
        Link = Trim(main.GetAggrOption("URL", OptionString))
        RefreshHours = KmaEncodeNumber(main.GetAggrOption("RefreshHours", OptionString))
        MaxStories = kmaEncodeInteger(main.GetAggrOption("Number of Stories", OptionString))
        instanceId = main.getaddonOption("instanceId", OptionString)
        If MaxStories = 0 Then
            MaxStories = 99
        End If
        If Link = "" Then
            '
            ' No link provided
            '
            If main.IsAdmin Then
                GetContent = main.GetAdminHintWrapper("The RSS Quick Client requires a URL to continue.")
            End If
        Else
            '
            ' Link Provided
            '
            SaveCache = True
            VersionString = App.Major & "." & App.Minor & "." & App.Revision
            FeedFilename = EncodeFilename(Link)
            FeedFilename = "aoRSSClientFiles\" & FeedFilename & ".txt"
            FeedConfig = main.ReadVirtualFile(FeedFilename)
            If FeedConfig <> "" Then
                ConfigHeader = KmaGetLine(FeedConfig)
                If ConfigHeader <> "" Then
                    ConfigSplit = Split(ConfigHeader, ":")
                    If Trim(LCase(ConfigSplit(0))) = "rss client quick reader" Then
                        If Trim(LCase(ConfigSplit(1))) = VersionString Then
                            LastRefresh = KmaEncodeDate(KmaGetLine(FeedConfig))
                            If LastRefresh <> CDate(0) Then
                                If (LastRefresh + (RefreshHours / 24)) > Now() Then
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
            '
            If Feed = "" Then
                '
                ' Get a new copy of the feed
                '
                Set doc = New DOMDocument30
                doc.Load (Link)
                Do While doc.readyState <> 4 And LoopPtr < 100
                    Sleep (100)
                    DoEvents
                    LoopPtr = LoopPtr + 1
                Loop
                If doc.parseError.errorCode <> 0 Then
                    '
                    ' error - Need a way to reach the user that submitted the file
                    '
                    If main.IsAdmin Then
                        GetContent = main.GetAdminHintWrapper("The RSS Feed [" & Link & "] caused an error, " & doc.parseError.reason)
                    End If
                Else
                    '
                    ' Retrieved document OK
                    '
                    Feed = doc.xml
                    SaveCache = True
                End If
            End If
            '
            ' Convert the feed to HTML
            '
            If Feed <> "" Then
                Set doc = New DOMDocument30
                doc.loadXML (Feed)
                Do While doc.readyState <> 4 And LoopPtr < 100
                    Sleep (100)
                    DoEvents
                    LoopPtr = LoopPtr + 1
                Loop
                If doc.parseError.errorCode <> 0 Then
                    '
                    ' error - Need a way to reach the user that submitted the file
                    '
                    If main.IsAdmin Then
                        GetContent = main.GetAdminHintWrapper("The RSS Feed [" & Link & "] caused an error, " & doc.parseError.reason)
                    End If
                Else
                    With doc.documentElement
                        '
                        If (LCase(.baseName) = LCase(RSSRootNode)) Then
                            '
                            ' RSS Feed
                            '
                            IsRSS = True
                            GetContent = GetRSS(doc.xml, MaxStories)
                        ElseIf (LCase(.baseName) = LCase(AtomRootNode)) Then
                            '
                            ' Atom Feed
                            '
                            isAtom = True
                            GetContent = GetAtom(doc.xml, MaxStories)
                        Else
                            '
                            ' Bad Feed
                            '
                            If main.IsAdmin Then
                                GetContent = main.GetAdminHintWrapper("The RSS Feed [" & Link & "] returned an incompatible file.")
                            End If
                        End If
                    End With
                    '
                    ' Save this feed into the cache
                    '
                    If SaveCache Then
                        FeedHeader = "RSS Client Quick Reader : " & VersionString _
                            & vbCrLf & CStr(Now())
                        Call main.SaveVirtualFile(FeedFilename, FeedHeader & vbCrLf & Feed)
                    End If
                End If
            End If
        End If
    End If
    '
    Exit Function
ErrorTrap:
    'HandleError
End Function
'
'=================================================================================
'   Read RSS Feed
'=================================================================================
'
Private Function GetRSS(Feed As String, MaxStories As Long) As String
    On Error GoTo ErrorTrap
    '
    Dim StoryCnt As Long
    '
    Dim IsRSS As Boolean
    Dim isAtom As Boolean
    '
    Dim ItemPubDate As String
    Dim EnclosureRow As String
    Dim Ptr As Long
    Dim Found As Boolean
    Dim EnclosureCnt As Long
    Dim Enclosure() As EnclosureType
    Dim ChannelImage As String
    Dim ChannelTitle As String
    Dim ChannelDescription As String
    Dim ChannelPubDate As String
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
    Dim FeedHeader As String
    Dim VersionString As String
    Dim UserError As String
    Dim LastRefresh As Date
    Dim RefreshHours As Double
    Dim FeedConfig As String
    Dim ConfigHeader As String
    Dim ConfigSplit() As String
    Dim doc As DOMDocument30
    Dim RootNode As IXMLDOMNode
    Dim ChannelNode As IXMLDOMNode
    Dim ItemNode As IXMLDOMNode
    Dim ImageNode As IXMLDOMNode
    Dim LoopPtr As Long
    Dim FeedFilename As String
    Dim Link As String
    '
    '
    ' Convert the feed to HTML
    '
    If Feed <> "" Then
        Set doc = New DOMDocument30
        doc.loadXML (Feed)
        Do While doc.readyState <> 4 And LoopPtr < 100
            Sleep (100)
            DoEvents
            LoopPtr = LoopPtr + 1
        Loop
        If doc.parseError.errorCode <> 0 Then
            '
            ' error - Need a way to reach the user that submitted the file
            '
            If main.IsAdmin Then
                GetRSS = main.GetAdminHintWrapper("The RSS Feed caused an error, " & doc.parseError.reason)
            End If
        Else
            With doc.documentElement
                ChannelTitle = ""
                ChannelDescription = ""
                ChannelLink = ""
                For Each RootNode In .childNodes
                    Select Case LCase(RootNode.baseName)
                        Case "channel"
                            ChannelTitle = ""
                            ChannelDescription = ""
                            ChannelLink = ""
                            ChannelImage = ""
                            ChannelItem = ""
                            For Each ChannelNode In RootNode.childNodes
                                Select Case LCase(ChannelNode.baseName)
                                    Case "pubdate"
                                        ChannelPubDate = ChannelNode.Text
                                    Case "title"
                                        ChannelTitle = ChannelNode.Text
                                    Case "description"
                                        ChannelDescription = ChannelNode.Text
                                    Case "link"
                                        ChannelLink = ChannelNode.Text
                                    Case "image"
                                        ImageWidth = ""
                                        ImageHeight = ""
                                        ImageTitle = ""
                                        ImageURL = ""
                                        ImageLink = ""
                                        NewChannelImage = ""
                                        For Each ImageNode In ChannelNode.childNodes
                                            Select Case LCase(ImageNode.baseName)
                                                Case "title"
                                                    ImageTitle = ImageNode.Text
                                                Case "url"
                                                    ImageURL = ImageNode.Text
                                                Case "link"
                                                    ImageLink = ImageNode.Text
                                                Case "width"
                                                    ImageWidth = ImageNode.Text
                                                Case "height"
                                                    ImageHeight = ImageNode.Text
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
                                        For Each ItemNode In ChannelNode.childNodes
                                            Select Case LCase(ItemNode.baseName)
                                                Case "title"
                                                    ItemTitle = ItemNode.Text
                                                Case "description"
                                                    ItemDescription = ItemNode.Text
                                                Case "link"
                                                    ItemLink = ItemNode.Text
                                                Case "pubdate"
                                                    ItemPubDate = ItemNode.Text
                                                Case "enclosure"
                                                    ReDim Preserve Enclosure(EnclosureCnt)
                                                    Enclosure(EnclosureCnt).URL = GetXMLAttribute(Found, ItemNode, "url")
                                                    Enclosure(EnclosureCnt).Type = GetXMLAttribute(Found, ItemNode, "type")
                                                    Enclosure(EnclosureCnt).Length = GetXMLAttribute(Found, ItemNode, "length")
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
                                            ItemTitle = vbCrLf & vbTab & vbTab & "<div class=ItemTitle>" & ItemTitle & "</div>"
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
                                            & vbCrLf & vbTab & "<div class=ChannelItem>" _
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
                            & vbCrLf & vbTab & "<div class=ChannelTitle>" & ChannelTitle & "</div>" _
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
    End If
    '
    Exit Function
ErrorTrap:
    'HandleError
End Function
'
'=================================================================================
'   Read Atom Feed
'=================================================================================
'
Private Function GetAtom(Feed As String, MaxStories) As String
    On Error GoTo ErrorTrap
    '
    Dim StoryCnt As Long
    Dim Pos As Long
    Dim DateSplit() As String
    Dim IsRSS As Boolean
    Dim isAtom As Boolean
    '
    Dim ItemPubDate As String
    Dim EnclosureRow As String
    Dim Ptr As Long
    Dim Found As Boolean
    Dim EnclosureCnt As Long
    Dim Enclosure() As EnclosureType
    Dim ChannelImage As String
    Dim ChannelTitle As String
    Dim ChannelDescription As String
    Dim ChannelPubDate As String
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
    Dim FeedHeader As String
    Dim VersionString As String
    Dim UserError As String
    Dim LastRefresh As Date
    Dim RefreshHours As Double
    Dim FeedConfig As String
    Dim ConfigHeader As String
    Dim ConfigSplit() As String
    Dim doc As DOMDocument30
    Dim RootNode As IXMLDOMNode
    Dim ItemNode As IXMLDOMNode
    Dim ImageNode As IXMLDOMNode
    Dim LoopPtr As Long
    Dim FeedFilename As String
    Dim Link As String
    '
    '
    ' Convert the feed to HTML
    '
    If Feed <> "" Then
        Set doc = New DOMDocument30
        doc.loadXML (Feed)
        Do While doc.readyState <> 4 And LoopPtr < 100
            Sleep (100)
            DoEvents
            LoopPtr = LoopPtr + 1
        Loop
        If doc.parseError.errorCode <> 0 Then
            '
            ' error - Need a way to reach the user that submitted the file
            '
            If main.IsAdmin Then
                GetAtom = main.GetAdminHintWrapper("The RSS Feed caused an error, " & doc.parseError.reason)
            End If
        Else
            With doc.documentElement
                ChannelTitle = ""
                ChannelDescription = ""
                ChannelLink = ""
                ChannelImage = ""
                ChannelItem = ""
                For Each RootNode In .childNodes
                    '
                    ' Atom Feed only has one channel, so there is no Channel element
                    '
                    'pubdate should be updated
                    'description should be subtitle
                    Select Case LCase(RootNode.baseName)
                        Case "updated"
                            ChannelPubDate = RootNode.Text
                            Pos = InStr(1, ChannelPubDate, "T", vbTextCompare)
                            If Pos > 0 Then
                                ChannelPubDate = Mid(ChannelPubDate, 1, Pos - 1)
                                Pos = InStr(1, ChannelPubDate, "-")
                                If Pos > 0 Then
                                    DateSplit = Split(ChannelPubDate, "-")
                                    If UBound(DateSplit) = 2 Then
                                        ChannelPubDate = FormatDateTime(KmaEncodeDate(CStr(DateSplit(1) & "/" & DateSplit(2) & "/" & DateSplit(0))), vbLongDate)
                                    End If
                                End If
                            End If
                        Case "title"
                            ChannelTitle = RootNode.Text
                        Case "subtitle"
                            ChannelDescription = RootNode.Text
                        Case "link"
Dim linkType As String
Dim isFound As Boolean
                            linkType = GetXMLAttribute(isFound, RootNode, "type")
                            If LCase(linkType) = "text/html" Then
                                ChannelLink = GetXMLAttribute(isFound, RootNode, "href")
                            End If
                        Case "image"
                            ImageWidth = ""
                            ImageHeight = ""
                            ImageTitle = ""
                            ImageURL = ""
                            ImageLink = ""
                            NewChannelImage = ""
                            For Each ImageNode In RootNode.childNodes
                                Select Case LCase(ImageNode.baseName)
                                    Case "title"
                                        ImageTitle = ImageNode.Text
                                    Case "url"
                                        ImageURL = ImageNode.Text
                                    Case "link"
                                        ImageLink = ImageNode.Text
                                    Case "width"
                                        ImageWidth = ImageNode.Text
                                    Case "height"
                                        ImageHeight = ImageNode.Text
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
                            For Each ItemNode In RootNode.childNodes
                                Select Case LCase(ItemNode.baseName)
                                    Case "title"
                                        ItemTitle = ItemNode.Text
                                    Case "link"
                                        linkType = GetXMLAttribute(isFound, ItemNode, "type")
                                        If LCase(linkType) = "text/html" Then
                                            ItemLink = GetXMLAttribute(isFound, ItemNode, "href")
                                        End If
                                    Case "updated"
                                        ItemPubDate = ItemNode.Text
                                        Pos = InStr(1, ItemPubDate, "T", vbTextCompare)
                                        If Pos > 0 Then
                                            ItemPubDate = Mid(ItemPubDate, 1, Pos - 1)
                                            Pos = InStr(1, ItemPubDate, "-")
                                            If Pos > 0 Then
                                                DateSplit = Split(ItemPubDate, "-")
                                                If UBound(DateSplit) = 2 Then
                                                    'ItemPubDate = FormatDateTime(KmaEncodeDate(CStr(DateSplit(2) & "/" & DateSplit(1) & "/" & DateSplit(0))), vbLongDate)
                                                    ItemPubDate = FormatDateTime(KmaEncodeDate(CStr(DateSplit(1) & "/" & DateSplit(2) & "/" & DateSplit(0))), vbLongDate)
                                                End If
                                            End If
                                        End If
                                    Case "summary"
                                        ItemDescription = ItemNode.Text
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
                            GetAtom = GetAtom _
                                & vbCrLf & vbTab & "<div class=ChannelItem>" _
                                & ItemTitle _
                                & ItemPubDate _
                                & ItemDescription _
                                & EnclosureRow _
                                & vbCrLf & vbTab & "</div>" _
                                & ""
                            StoryCnt = StoryCnt + 1
                            If StoryCnt >= MaxStories Then
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
    End If
    '
    Exit Function
ErrorTrap:
    'HandleError
End Function
'
'========================================================================
' ----- Get an XML nodes attribute based on its name
'========================================================================
'
Friend Function GetXMLAttribute(Found As Boolean, Node As IXMLDOMNode, Name As String) As String
    On Error GoTo ErrorTrap
    '
    Dim NodeAttribute As IXMLDOMAttribute
    Dim REsultNode As IXMLDOMNode
    Dim UcaseName As String
    '
    Found = False
    Set REsultNode = Node.Attributes.getNamedItem(Name)
    If (REsultNode Is Nothing) Then
        UcaseName = UCase(Name)
        For Each NodeAttribute In Node.Attributes
            If UCase(NodeAttribute.nodeName) = UcaseName Then
                GetXMLAttribute = NodeAttribute.nodeValue
                Found = True
                Exit For
                End If
            Next
    Else
        GetXMLAttribute = REsultNode.nodeValue
        Found = True
    End If
    Exit Function
    '
    ' ----- Error Trap
    '
ErrorTrap:
    End Function

