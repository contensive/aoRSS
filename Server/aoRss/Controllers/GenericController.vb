
Imports Contensive.BaseClasses

Namespace Controllers
    Public NotInheritable Class GenericController
        '
        '
        Public Shared Function GetGMTFromDate(DateValue As Date) As String
            Dim result As String = ""
            Dim WorkLong As Integer
            '
            If IsDate(DateValue) Then
                Select Case Weekday(DateValue)
                    Case vbSunday
                        result = "Sun, "
                    Case vbMonday
                        result = "Mon, "
                    Case vbTuesday
                        result = "Tue, "
                    Case vbWednesday
                        result = "Wed, "
                    Case vbThursday
                        result = "Thu, "
                    Case vbFriday
                        result = "Fri, "
                    Case vbSaturday
                        result = "Sat, "
                End Select
                '
                WorkLong = Day(DateValue)
                If WorkLong < 10 Then
                    result = result & "0" & CStr(WorkLong) & " "
                Else
                    result = result & CStr(WorkLong) & " "
                End If
                '
                Select Case Month(DateValue)
                    Case 1
                        result = result & "Jan "
                    Case 2
                        result = result & "Feb "
                    Case 3
                        result = result & "Mar "
                    Case 4
                        result = result & "Apr "
                    Case 5
                        result = result & "May "
                    Case 6
                        result = result & "Jun "
                    Case 7
                        result = result & "Jul "
                    Case 8
                        result = result & "Aug "
                    Case 9
                        result = result & "Sep "
                    Case 10
                        result = result & "Oct "
                    Case 11
                        result = result & "Nov "
                    Case 12
                        result = result & "Dec "
                End Select
                '
                result = result & CStr(Year(DateValue)) & " "
                '
                WorkLong = Hour(DateValue)
                If WorkLong < 10 Then
                    result = result & "0" & CStr(WorkLong) & ":"
                Else
                    result = result & CStr(WorkLong) & ":"
                End If
                '
                WorkLong = Minute(DateValue)
                If WorkLong < 10 Then
                    result = result & "0" & CStr(WorkLong) & ":"
                Else
                    result = result & CStr(WorkLong) & ":"
                End If
                '
                WorkLong = Second(DateValue)
                If WorkLong < 10 Then
                    result = result & "0" & CStr(WorkLong)
                Else
                    result = result & CStr(WorkLong)
                End If
                '
                result = result & " GMT"
            End If
            Return result
        End Function
        '
        '====================================================================================================
        ''' <summary>
        ''' return an anchor tag for the provided feed
        ''' </summary>
        ''' <param name="cp"></param>
        ''' <param name="rssfeed"></param>
        ''' <returns></returns>
        Public Shared Function getFeedLink(cp As CPBaseClass, rssfeed As Models.Db.RSSFeedModel) As String
            If rssfeed.RSSFilename = "" Then
                Return "<img src=""/rssFeeds/IconXML-25x13.gif"" width=25 height=13 border=0 class=""RSSFeedImage"">&nbsp;" & rssfeed.name & "&nbsp;(Coming Soon)"
            Else
                Return "<img src=""/rssFeeds/IconXML-25x13.gif"" width=25 height=13 border=0 class=""RSSFeedImage"">&nbsp;<a class=""RSSFeedLink"" href=""" & cp.Http.CdnFilePathPrefixAbsolute & rssfeed.RSSFilename & """>" & rssfeed.name & "</a>"
            End If
        End Function
        '
        '====================================================================================================
        ''' <summary>
        ''' tmp Placeholder for CP.Http.CdnFilePathPrefixAbsolute
        ''' </summary>
        ''' <param name="cp"></param>
        ''' <returns></returns>
        Public Shared Function getCdnFilePathPrefixAbsolute(cp As CPBaseClass) As String

            If (Not cp.ServerConfig.isLocalFileSystem) Then
                '
                ' -- remote file system, return cdnfileurl
                Return cp.GetAppConfig().cdnFileUrl
            Else
                '
                ' -- local file system
                Dim cdnFilePathPrefixAbsolute As String = cp.Site.GetText("CdnFilePathPrefixAbsolute")?.Replace("\", "/")
                If (Not String.IsNullOrWhiteSpace(cdnFilePathPrefixAbsolute)) Then
                    If (Not cdnFilePathPrefixAbsolute.Substring(cdnFilePathPrefixAbsolute.Length, 1).Equals("/")) Then
                        cdnFilePathPrefixAbsolute &= "/"
                    End If
                Else
                    cdnFilePathPrefixAbsolute = "https://" & cp.Site.DomainPrimary & "/" & cp.GetAppConfig().cdnFileUrl
                End If
                Return cdnFilePathPrefixAbsolute
            End If
        End Function

    End Class
End Namespace

