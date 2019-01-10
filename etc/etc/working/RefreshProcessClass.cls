VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "RefreshProcessClass"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'
'========================================================================
'   v3.3 Compatibility
'       To make an Add-on that works the same in v3.3 and v3.4, use this adapter instead of the execute above
'========================================================================
'
Public Function Execute(CsvObject As Object, MainObject As Object, OptionString As String) As String
    On Error GoTo ErrorTrap
    '
    Dim linkRel As String
    Dim Pos As Long
    Dim isFound As Boolean
    Dim sourceId As Long
    Dim csStory As Long
    Dim LoopPtr As Long
    Dim csv As Object
    Dim cs As Long
    Dim Link As String
    Dim doc As DOMDocument30
    Dim ItemTitle As String
    Dim ItemLink As String
    Dim ItemDescription As String
    Dim ItemPubDate As String
    Dim RootNode As IXMLDOMNode
    Dim ChannelNode As IXMLDOMNode
    Dim ItemNode As IXMLDOMNode
    Dim DateSplit() As String
    Dim isAtom As Boolean
    Dim itemGuid As String
    '
    Set csv = CsvObject
    '
    cs = csv.opencsContent("RSS Aggregator Sources")
    Do While csv.iscsok(cs)
        '
        ' Convert the feed to HTML
        '
        sourceId = csv.getcsinteger(cs, "id")
        Link = csv.getcstext(cs, "link")
        If Link <> "" Then
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
                Call csv.reportError("The RSS Feed [" & Link & "] caused an error, " & doc.parseError.reason, App.EXEName & ".refreshProcessClass.execute", True)
            Else
                isAtom = (LCase(doc.documentElement.baseName) = "feed")
                If isAtom Then
                    '
                    ' atom feed
                    '
                    With doc.documentElement
                        For Each RootNode In .childNodes
                            Select Case LCase(RootNode.baseName)
                                Case "entry"
                                    Set ChannelNode = RootNode
                                    ItemTitle = ""
                                    ItemLink = ""
                                    ItemDescription = ""
                                    ItemPubDate = ""
                                    itemGuid = ""
                                    For Each ItemNode In ChannelNode.childNodes
                                        Select Case LCase(ItemNode.baseName)
                                            Case "id"
                                                itemGuid = ItemNode.Text
                                            Case "title"
                                                ItemTitle = ItemNode.Text
                                            Case "content"
                                                ItemDescription = ItemNode.Text
                                                '
                                                '   clear any styles out of the description
                                                '
                                                ItemDescription = clearStyles(ItemDescription)
                                            Case "link"

Dim linkType As String

                                                isFound = False
                                                linkType = GetXMLAttribute(isFound, ItemNode, "type")
                                                If (ItemLink = "") And ((Not isFound) Or (linkType = "text/html")) Then
                                                    ItemLink = GetXMLAttribute(isFound, ItemNode, "href")
                                                End If
                                            Case "updated"
                                                ItemPubDate = ItemNode.Text
                                        End Select
                                    Next
                                    If ItemPubDate <> "" Then
                                        Pos = InStr(1, ItemPubDate, "T")
                                        If Pos > 1 Then
                                            ItemPubDate = Left(ItemPubDate, Pos - 1)
                                        End If
                                    End If
                                    If itemGuid = "" Then
                                        itemGuid = ItemTitle
                                    End If
                                    csStory = csv.opencsContent("RSS Aggregator Source Stories", "(itemGuid=" & KmaEncodeSQLText(itemGuid) & ")and(sourceId=" & sourceId & ")")
                                    If Not csv.iscsok(csStory) Then
                                        Call csv.closecs(csStory)
                                        csStory = csv.insertCsRecord("RSS Aggregator Source Stories", 0)
                                        Call csv.setcs(csStory, "pubDate", Now)
                                        Call csv.setcs(csStory, "sourceId", sourceId)
                                        Call csv.setcs(csStory, "itemGuid", itemGuid)
                                    End If
                                    If csv.iscsok(csStory) Then
                                        If csv.getcs(csStory, "name") <> ItemTitle Then
                                            Call csv.setcs(csStory, "name", ItemTitle)
                                        End If
                                        If csv.getcs(csStory, "description") <> ItemDescription Then
                                            Call csv.setcs(csStory, "description", ItemDescription)
                                        End If
                                        If csv.getcs(csStory, "link") <> ItemLink Then
                                            Call csv.setcs(csStory, "link", ItemLink)
                                        End If
                                        
                                        If KmaEncodeDate(ItemPubDate) <> CDate(0) Then
                                            If csv.getcs(csStory, "pubDate") <> KmaEncodeDate(ItemPubDate) Then
                                                Call csv.setcs(csStory, "pubDate", ItemPubDate)
                                            End If
                                        End If
                                    End If
                                    Call csv.closecs(csStory)
                                    '
'                                        End Select
'                                    Next
                            End Select
                        Next
                    End With
                Else
                    '
                    ' RSS
                    '
                    With doc.documentElement
                        For Each RootNode In .childNodes
                            Select Case LCase(RootNode.baseName)
                                Case "channel"
                                    For Each ChannelNode In RootNode.childNodes
                                        Select Case LCase(ChannelNode.baseName)
                                            Case "item"
                                                ItemTitle = ""
                                                ItemLink = ""
                                                ItemDescription = ""
                                                ItemPubDate = ""
                                                itemGuid = ""
                                                For Each ItemNode In ChannelNode.childNodes
                                                    Select Case LCase(ItemNode.baseName)
                                                        Case "guid"
                                                            itemGuid = ItemNode.Text
                                                        Case "title"
                                                            ItemTitle = ItemNode.Text
                                                        Case "description"
                                                            ItemDescription = ItemNode.Text
                                                            '
                                                            '   clear any styles out of the description
                                                            '
                                                            ItemDescription = clearStyles(ItemDescription)
                                                        Case "link"
                                                            ItemLink = ItemNode.Text
                                                        Case "pubdate"
                                                            ItemPubDate = ItemNode.Text
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
                                                csStory = csv.opencsContent("RSS Aggregator Source Stories", "(name=" & KmaEncodeSQLText(ItemTitle) & ")and(sourceId=" & sourceId & ")")
                                                If Not csv.iscsok(csStory) Then
                                                    Call csv.closecs(csStory)
                                                    csStory = csv.insertCsRecord("RSS Aggregator Source Stories", 0)
                                                    Call csv.setcs(csStory, "pubDate", Now)
                                                    Call csv.setcs(csStory, "sourceId", sourceId)
                                                    Call csv.setcs(csStory, "itemGuid", itemGuid)
                                                End If
                                                If csv.iscsok(csStory) Then
                                                    If csv.getcs(csStory, "name") <> ItemTitle Then
                                                        Call csv.setcs(csStory, "name", ItemTitle)
                                                    End If
                                                    If csv.getcs(csStory, "description") <> ItemDescription Then
                                                        Call csv.setcs(csStory, "description", ItemDescription)
                                                    End If
                                                    If csv.getcs(csStory, "link") <> ItemLink Then
                                                        Call csv.setcs(csStory, "link", ItemLink)
                                                    End If
                                                    
                                                    If KmaEncodeDate(ItemPubDate) <> CDate(0) Then
                                                        If csv.getcs(csStory, "pubDate") <> KmaEncodeDate(ItemPubDate) Then
                                                            Call csv.setcs(csStory, "pubDate", ItemPubDate)
                                                        End If
                                                    End If
                                                End If
                                                Call csv.closecs(csStory)
                                                '
                                        End Select
                                    Next
                            End Select
                        Next
                    End With
                End If
            End If
        End If
        Call csv.nextcsRecord(cs)
    Loop
    Call csv.closecs(cs)
    '
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
'
'   clear anything in between and including <style> tags and from description
'
Private Function clearStyles(givenString As String) As String
    On Error GoTo ErrorTrap
    '
    Dim output As String
    Dim posStart As Long
    Dim posEnd As Long
    Dim styles As String
    '
    posStart = InStr(givenString, "<style>")
    posEnd = InStr(givenString, "</style>") + Len("</style>") - 1
    '
    If posStart <> 0 Then
        styles = Mid(givenString, posStart, posEnd)
        output = Replace(givenString, styles, "")
    Else
        output = givenString
    End If
    '
    clearStyles = output
    '
ErrorTrap:
    'HandleError - not sure why, but error hadling is comment out in all functions
End Function


