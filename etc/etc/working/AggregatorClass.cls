VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "AggregatorClass"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True

Option Explicit
'
'========================================================================
' This add-on aggregates several RSS feeds into one list, ordered newest first
'
' two tables, Aggregators and AggregatorArticles
'   Aggregator Table holds name, like 'Technology', or 'Auto' and list of URLs as a text field
'   Phase 1 - public page gets AggregatorID from Optionstring and looks up
'   RSS Sources are saved in a config file, edited Public with Advanced Edit
'   When the list needs to be refreshed, it only refreshes the oldest source,
'       and assumes the list will be hit often enough to keep the others fresh.
'
'   To add items to the list, you have to be an admin. When you are, you get
'   an edit icon. Click it, and the edit form slides down. You can add new lists,
'   remove current lists, or change the refresh period.
'
'========================================================================
'
'Private Main As ccWeb3.MainClass
'
'Type EnclosureType
'    URL As String
'    Type As String
'    Length As String
'End Type
Private main As Object
Private csv As Object
'
'========================================================================
'   v3.3 Compatibility
'       To make an Add-on that works the same in v3.3 and v3.4, use this adapter instead of the execute above
'========================================================================
'
Public Function Execute(CsvObject As Object, MainObject As Object, OptionString As String, FilterInput As String) As String
    On Error GoTo ErrorTrap
    '
    Set csv = CsvObject
    Set main = MainObject
    '
    Dim delimiter As String
    Dim pubDate As String
    Dim sourceName As String
    Dim StoryCnt As Long
    
    Dim Cell As String
    Dim list As String
    Dim sql As String
    Dim Copy As String
    '
    Const RSSRootNode = "rss"
    '
    Dim ItemPubDate As String
    Dim DateSplit() As String
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
    Dim AggregatorName As String
    Dim AggregatorId As Long
    Dim cs As Long
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
    If Not (main Is Nothing) Then
        StoryCnt = kmaEncodeInteger(main.GetAggrOption("Story count", OptionString))
        If StoryCnt = 0 Then
            StoryCnt = 5
        End If
        AggregatorName = Trim(main.GetAggrOption("Aggregator", OptionString))
        If AggregatorName <> "" Then
            '
            ' get the Id of the one selected
            '
            cs = csv.opencsContent("RSS aggregators", "(name=" & KmaEncodeSQLText(AggregatorName) & ")")
            If csv.iscsok(cs) Then
                AggregatorId = csv.getcsinteger(cs, "id")
            End If
            Call csv.closecs(cs)
            '
        End If
        If AggregatorId = 0 Then
            '
            ' the default is the first one in the list
            '
            cs = csv.opencsContent("RSS aggregators", , "name")
            If csv.iscsok(cs) Then
                AggregatorId = csv.getcsinteger(cs, "id")
            End If
            Call csv.closecs(cs)
            '
        End If
        If AggregatorId = 0 Then
            '
            ' Create new aggregator
            '
            cs = main.insertCsRecord("RSS Aggregators")
            If main.iscsok(cs) Then
                Call main.setcs(cs, "name", "New RSS Aggregator create on-demand for " & main.memberName & ", " & Now)
                AggregatorId = csv.getcsinteger(cs, "id")
            End If
        End If
        '
        cacheName = "rssAggregator:" & AggregatorId & ":" & StoryCnt
        Execute = csv.readBake(cacheName)
        If Execute = "" Then
            '
            '
            '
            sql = "select s.name,a.name as articleName, a.pubdate, a.description, a.link  from ((aoRSSAggregatorSourceRules r" _
                & " left join aorssaggregatorsources s on s.id=r.SourceID)" _
                & " left join rssaggregatorsourcestories a on a.sourceid=r.SourceID)" _
                & " Where r.AggregatorId = " & AggregatorId _
                & " order by a.pubdate desc,a.id desc"
            cs = csv.opencsSql("", sql, 0)
            Ptr = 0
            Do While csv.iscsok(cs) And (Ptr < StoryCnt)
                Cell = ""
                '
                Link = Trim(csv.getcstext(cs, "link"))
                Copy = Trim(csv.getcstext(cs, "articleName"))
                If Copy <> "" Then
                    If Link <> "" Then
                        Copy = "<a href=""" & Link & """>" & Copy & "</a>"
                    End If
                    Cell = Cell & cr3 & "<h2 class=""raCaption"">" & Copy & "</h2>"
                End If
                '
                pubDate = Trim(csv.getcstext(cs, "pubdate"))
                sourceName = Trim(csv.getcstext(cs, "name"))
                delimiter = ""
                If sourceName <> "" Then
                    sourceName = "<span class=""raSourceName"">" & sourceName & "</span>"
                End If
                If pubDate <> "" Then
                    pubDate = "<span class=""raPubDate"">" & pubDate & "</span>"
                End If
                If (pubDate <> "") And (sourceName <> "") Then
                    delimiter = "<span class=""raDelimiter"">|</span>"
                End If
                Copy = sourceName & delimiter & pubDate
                If Copy <> "" Then
                    Cell = Cell & cr3 & "<div class=""raByLine"">" & Copy & "</div>"
                End If
                '
                Copy = Trim(csv.getcstext(cs, "description"))
                If Copy <> "" Then
                    Cell = Cell & cr3 & "<p class=""raDescription"">" & Copy & "</p>"
                End If
                '
                list = list _
                    & cr2 & "<li class=""raItem"">" _
                    & Cell _
                    & cr2 & "</li>"
                '
                Ptr = Ptr + 1
                Call csv.nextcsRecord(cs)
            Loop
            '
            If list <> "" Then
                Execute = "" _
                    & cr1 & "<ul class=""rssAggregator"">" _
                    & list _
                    & cr1 & "</ul>" _
                    & ""
            End If
            Call csv.saveBake(cacheName, Execute, cacheList)
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



