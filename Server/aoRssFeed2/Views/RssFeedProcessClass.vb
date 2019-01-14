
Option Strict On
Option Explicit On

Imports Contensive.BaseClasses
Imports Contensive.Addons.Rss.Models.Db

Namespace Views
    '
    Public Class RssFeedProcessClass
        Inherits AddonBaseClass
        '
        '
        '=====================================================================================
        ''' <summary>
        ''' AddonDescription
        ''' </summary>
        ''' <param name="CP"></param>
        ''' <returns></returns>
        Overrides Function Execute(ByVal CP As CPBaseClass) As Object
            Dim result As String = ""
            Try
                Dim RightNow As Date = Now
                Dim SQLDateNow As String = CP.Db.EncodeSQLDate(RightNow)
                '
                ' Get Primary domain from domain list
                Dim DomainName As String = CP.Site.DomainPrimary
                Dim Pos As Integer = InStr(1, DomainName, ",")
                If Pos > 2 Then
                    DomainName = Mid(DomainName, 1, Pos - 1)
                End If
                '
                ' Load the feeds
                Dim feedList As New List(Of Models.Domain.FeedModel)
                For Each dbFeed As RSSFeedModel In BaseModel.createList(Of RSSFeedModel)(CP, "", "")
                    Dim feed As New Models.Domain.FeedModel With {
                        .Id = dbFeed.id,
                        .Name = dbFeed.name,
                        .Description = dbFeed.Description,
                        .Link = dbFeed.Link,
                        .LogoFilename = dbFeed.Link,
                        .entryList = New List(Of Models.Domain.FeedEntryModel)
                    }
                    If String.IsNullOrWhiteSpace(feed.Link) Then feed.Link = "http://" & DomainName & CP.Site.AppRootPath
                    Dim RSSFilename As String = dbFeed.RSSFilename
                    Dim testfilename As String = RSSFilename
                    If testfilename = "" Then
                        testfilename = dbFeed.name
                    End If
                    If testfilename = "" Then
                        testfilename = "RSSFeed" & dbFeed.id
                    End If
                    Dim testFilenameNoExt As String = testfilename
                    Pos = InStr(1, testFilenameNoExt, ".xml", vbTextCompare)
                    If Pos > 0 Then
                        testFilenameNoExt = Mid(testFilenameNoExt, 1, Pos - 1)
                    End If
                    testFilenameNoExt = testEncodeFilename(testFilenameNoExt)
                    Dim suffixNumber As Integer = 1
                    Dim testFilenameRoot As String = testFilenameNoExt
                    Dim usedFilenames As String = ""
                    Do While (suffixNumber < 100) And (InStr(1, "," & usedFilenames & ",", "," & testFilenameNoExt & ",", vbTextCompare) <> 0)
                        testFilenameNoExt = testFilenameRoot & CStr(suffixNumber)
                        suffixNumber = suffixNumber + 1
                    Loop
                    usedFilenames = usedFilenames & "," & testFilenameNoExt
                    testfilename = "rssfeeds/" & testFilenameNoExt & ".xml"
                    If testfilename <> RSSFilename Then
                        RSSFilename = testfilename
                        dbFeed.RSSFilename = RSSFilename
                    End If
                    feed.RSSFilename = RSSFilename
                    feedList.Add(feed)
                    dbFeed.RSSDateUpdated = RightNow
                    dbFeed.save(Of RSSFeedModel)(CP)
                Next
                '
                ' -- create a list of all RSSFeeds content fields - these are the many-to-many fields that point to story records for each feed
                Dim LastTableID As Integer = -1
                Dim manyToManyFieldList As List(Of ContentFieldModel) = BaseModel.createList(Of ContentFieldModel)(CP, "(name='RSSFeeds')and(type=" & FieldTypeManyToMany & ")and(authorable<>0)", "contentid")
                For Each manyToManyField In manyToManyFieldList
                    '
                    ' -- each many-to-many field represents a checked-box in the RSS Feed tab, associating a story to a feed.
                    ' -- go through all records with this many-to-many field checked and add that story to that feed.
                    Dim storyContent As ContentModel = BaseModel.create(Of ContentModel)(CP, manyToManyField.ContentID)
                    If (storyContent IsNot Nothing) Then
                        Dim manyToManyRuleContent As ContentModel = BaseModel.create(Of ContentModel)(CP, manyToManyField.ManyToManyRuleContentID)
                        If (manyToManyRuleContent IsNot Nothing) Then
                            '
                            ' -- open all records in this rule table and associate stories to feeds in the feedlist
                            Dim csRuleRecord As CPCSBaseClass = CP.CSNew()
                            If csRuleRecord.Open(manyToManyRuleContent.name) Then
                                Do
                                    '
                                    ' -- find the feed selected in the rule record for this content 
                                    Dim feedId As Integer = csRuleRecord.GetInteger(manyToManyField.ManyToManyRuleSecondaryField)
                                    For Each feed In feedList
                                        If feedId = feed.Id Then
                                            '
                                            ' -- add the story to this feed
                                            Dim storyRecordID As Integer = csRuleRecord.GetInteger(manyToManyField.ManyToManyRulePrimaryField)
                                            With feed
                                                '
                                                ' -- prevent duplicate stories in the same feed
                                                Dim dupFound As Boolean = False
                                                For Each entry In feed.entryList
                                                    If (entry.storyRecordID = storyRecordID) And (entry.storyContentID = storyContent.id) Then
                                                        dupFound = True
                                                        Exit For
                                                    End If
                                                Next
                                                If Not dupFound Then
                                                    '
                                                    ' -- this story is not in the feed, add the story to the feed
                                                    Dim csStory As CPCSBaseClass = CP.CSNew()
                                                    If csStory.OpenRecord(storyContent.name, storyRecordID) Then
                                                        Dim entry As New Models.Domain.FeedEntryModel()
                                                        feed.entryList.Add(entry)
                                                        With entry
                                                            .storyContentID = storyContent.id
                                                            .storyRecordID = storyRecordID
                                                            If CP.Content.IsField(storyContent.name, "RSSTitle") Then
                                                                .Title = csStory.GetText("RSSTitle")
                                                            End If
                                                            If .Title = "" Then
                                                                .Title = csStory.GetText("Name")
                                                            End If
                                                            If CP.Content.IsField(storyContent.name, "RSSDateExpire") Then
                                                                .DateExpires = csStory.GetDate("RSSDateExpire")
                                                            End If
                                                            .DatePublish = DateTime.MinValue
                                                            If CP.Content.IsField(storyContent.name, "RSSDatePublish") Then
                                                                .DatePublish = csStory.GetDate("RSSDatePublish")
                                                                If .DatePublish < #8/7/1990# Then
                                                                    .DatePublish = DateTime.MinValue
                                                                End If
                                                            End If
                                                            If (.DatePublish = DateTime.MinValue) Then
                                                                .DatePublish = csStory.GetDate("dateAdded")
                                                                If (.DatePublish = DateTime.MinValue) Then
                                                                    .DatePublish = RightNow
                                                                    Call csStory.SetField("dateAdded", CType(.DatePublish, String))
                                                                    If CP.Content.IsField(storyContent.name, "RSSDatePublish") Then
                                                                        Call csStory.SetField("RSSDatePublish", CType(.DatePublish, String))
                                                                    End If
                                                                End If
                                                            End If
                                                            If CP.Content.IsField(storyContent.name, "RSSDescription") Then
                                                                .Description = csStory.GetText("RSSDescription")
                                                            End If
                                                            If CP.Content.IsField(storyContent.name, "RSSLink") Then
                                                                .Link = csStory.GetText("RSSLink")
                                                            End If
                                                            If CP.Content.IsField(storyContent.name, "PodcastMediaLink") Then
                                                                .PodcastMediaLink = csStory.GetText("PodcastMediaLink")
                                                            End If
                                                        End With
                                                    End If
                                                    csStory.Close()
                                                End If
                                            End With
                                            Exit For
                                        End If
                                    Next
                                    csRuleRecord.GoNext()
                                Loop While csRuleRecord.OK
                            End If
                            csRuleRecord.Close()
                        End If
                    End If
                Next
                '.
                ' sort the entries in each feed in feedlist
                ' tbd
                For Each feed In feedList

                    feed.entryList.Sort(Function(a, b) a.DatePublish.CompareTo(b.DatePublish))
                Next
                '
                '
                '
                ' Build the Feeds from the Feed Arrays
                '
                Dim serverPageDefault As String = CP.Site.GetProperty("SERVERPAGEDEFAULT", "")
                Dim pageContentContentId As Integer = CP.Content.GetID("page content")
                For Each feed In feedList
                    Dim Doc As Xml.XmlDocument = New Xml.XmlDocument
                    With feed
                        '
                        ' Build this feed
                        If (.Link <> "") And (InStr(1, .Link, "://") = 0) Then
                            .Link = "http://" & .Link
                        End If
                        '
                        Dim BaseNode As Xml.XmlNode = Doc.CreateElement("rss")
                        Call BaseNode.Attributes.GetNamedItem("version", "2.0")
                        Call BaseNode.Attributes.GetNamedItem("xmlns:atom", "http://www.w3.org/2005/Atom")
                        Call Doc.AppendChild(BaseNode)
                        '
                        Dim ChannelNode As Xml.XmlNode = Doc.CreateElement("channel")
                        Call BaseNode.AppendChild(ChannelNode)
                        '
                        Dim Node As Xml.XmlElement = Doc.CreateElement("title")
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
                        Call Node.Attributes.GetNamedItem("href", "http://" & DomainName & CP.Site.AppRootPath & CP.Site.FilePath & .RSSFilename)
                        Call Node.Attributes.GetNamedItem("rel", "self")
                        Call Node.Attributes.GetNamedItem("type", "application/rss+xml")
                        Call ChannelNode.AppendChild(Node)
                        '
                        If .Link <> "" Then
                            Node = Doc.CreateElement("link")
                            Node.InnerText = .Link
                            Call ChannelNode.AppendChild(Node)
                        End If
                        If .LogoFilename <> "" Then
                            '
                            ' Create Image
                            '
                            Dim ImageNode As Xml.XmlNode = Doc.CreateElement("image")
                            Call ChannelNode.AppendChild(ImageNode)
                            '
                            Node = Doc.CreateElement("url")
                            Node.InnerText = "http://" & DomainName & CP.Site.AppRootPath & CP.Site.FilePath & .LogoFilename
                            Call ImageNode.AppendChild(Node)
                            '
                            Node = Doc.CreateElement("title")
                            Node.InnerText = .Name
                            Call ImageNode.AppendChild(Node)
                            '
                            Node = Doc.CreateElement("description")
                            Node.InnerText = .Description
                            Call ImageNode.AppendChild(Node)
                            '
                            If .Link <> "" Then
                                Node = Doc.CreateElement("link")
                                Node.InnerText = .Link
                                Call ImageNode.AppendChild(Node)
                            End If
                        End If
                        For Each entry In .entryList
                            With entry
                                If (.DateExpires = Date.MinValue) Or (.DateExpires > RightNow) Then
                                    If (.DatePublish = Date.MinValue) Or (.DatePublish < RightNow) Then
                                        '
                                        ' create Item Node
                                        '
                                        Dim ItemNode As Xml.XmlNode = Doc.CreateElement("item")
                                        Call ChannelNode.AppendChild(ItemNode)
                                        '
                                        Node = Doc.CreateElement("title")
                                        Node.InnerText = .Title.Trim()
                                        Call ItemNode.AppendChild(Node)
                                        '
                                        Node = Doc.CreateElement("description")
                                        Node.InnerText = .Description
                                        Call ItemNode.AppendChild(Node)
                                        '
                                        If (.DatePublish <> Date.MinValue) Then
                                            Dim ItemDatePublishText As String = Controllers.genericController.GetGMTFromDate(.DatePublish)
                                            Node = Doc.CreateElement("pubDate")
                                            Node.InnerText = ItemDatePublishText
                                            Call ItemNode.AppendChild(Node)
                                        End If
                                        '
                                        Dim Link As String = Trim(.Link)
                                        If Link = "" Then
                                            Dim CS3 As CPCSBaseClass = CP.CSNew()
                                            CS3.OpenSQL("select link from ccContentWatch where contentid=" & .storyContentID & " and recordid=" & .storyRecordID)
                                            If CS3.OK Then
                                                Link = CS3.GetText("link").Trim()
                                            End If
                                            Call CS3.Close()
                                            If Link = "" Then
                                                If (.storyContentID = pageContentContentId) Then
                                                    Link = Trim(serverPageDefault & "?bid=" & .storyRecordID)
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
                                        Dim Enclosure As String = .PodcastMediaLink
                                        If Enclosure <> "" Then
                                            Dim Ptr As Integer = InStrRev(Enclosure, ".")
                                            Dim MimeType As String = ""
                                            If Ptr > 0 Then
                                                Dim Ext As String = LCase(Mid(Enclosure, Ptr + 1))
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
                                            Dim EnclosureLength As Integer
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
                            End With
                        Next
                        '
                        ' -- initialize application. If authentication needed and not login page, pass true
                        Dim encoding As String = CP.Site.GetProperty("Site Character Encoding", "utf-8")
                        If encoding = "" Then
                            encoding = "utf-8"
                        End If
                        '
                        ' -- there may be legacy systems that have hardcoded a path from wwwRoot/RSS to these files. In those legacy cases, create an IIS virtual path /RSS to the cden/rss folder
                        Call CP.File.SaveVirtual(.RSSFilename, "<?xml version=""1.0"" encoding=""" & encoding & """?>" & Doc.InnerXml)
                    End With
                Next
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
                Dim CopySplit As String()
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
                Dim BuildVersion As String = cp.Site.GetProperty("BuildVersion", "0")
                If BuildVersion >= "3.3.291" Then
                    result = cp.Doc.GetText(CSEmail, "InlineStyles")
                Else
                    result = "<link rel=""stylesheet"" href=""http://" & GetPrimaryDomainName(cp, cp.Site.DomainList) & cp.Site.PhysicalWWWPath & "styles.css"" type=""text/css"">"
                End If
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
        Private Function testEncodeFilename(Source As String) As String
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
