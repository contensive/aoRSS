
Option Explicit On
Option Strict On

Imports Contensive.BaseClasses

Namespace Controllers
    Public NotInheritable Class genericController
        Private Sub New()
        End Sub
        '
        '====================================================================================================
        ''' <summary>
        ''' if date is invalid, set to minValue
        ''' </summary>
        ''' <param name="srcDate"></param>
        ''' <returns></returns>
        Public Shared Function encodeMinDate(srcDate As DateTime) As DateTime
            Dim returnDate As DateTime = srcDate
            If srcDate < New DateTime(1900, 1, 1) Then
                returnDate = DateTime.MinValue
            End If
            Return returnDate
        End Function
        '
        '====================================================================================================
        ''' <summary>
        ''' if valid date, return the short date, else return blank string 
        ''' </summary>
        ''' <param name="srcDate"></param>
        ''' <returns></returns>
        Public Shared Function getShortDateString(srcDate As DateTime) As String
            Dim returnString As String = ""
            Dim workingDate As DateTime = encodeMinDate(srcDate)
            If Not isDateEmpty(srcDate) Then
                returnString = workingDate.ToShortDateString()
            End If
            Return returnString
        End Function
        '
        '====================================================================================================
        Public Shared Function isDateEmpty(srcDate As DateTime) As Boolean
            Return (srcDate < New DateTime(1900, 1, 1))
        End Function
        '
        '====================================================================================================
        Public Shared Function getSortOrderFromInteger(id As Integer) As String
            Return id.ToString().PadLeft(7, "0"c)
        End Function
        '
        '====================================================================================================
        Public Shared Function getDateForHtmlInput(source As DateTime) As String
            If isDateEmpty(source) Then
                Return ""
            Else
                Return source.Year.ToString() + "-" + source.Month.ToString().PadLeft(2, "0"c) + "-" + source.Day.ToString().PadLeft(2, "0"c)
            End If
        End Function
        '
        '====================================================================================================
        Public Shared Function convertToDosPath(sourcePath As String) As String
            Return sourcePath.Replace("/", "\")
        End Function
        '
        '====================================================================================================
        Public Shared Function convertToUnixPath(sourcePath As String) As String
            Return sourcePath.Replace("\", "/")
        End Function
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
                Return "<img src=""/rssFeeds/IconXML-25x13.gif"" width=25 height=13 border=0 class=""RSSFeedImage"">&nbsp;<a class=""RSSFeedLink"" href=""" & cp.Site.FilePath & rssfeed.RSSFilename & """>" & rssfeed.name & "</a>"
            End If
        End Function


    End Class
End Namespace

