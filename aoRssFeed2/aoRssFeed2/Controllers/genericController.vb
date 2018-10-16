
Option Explicit On
Option Strict On

'Imports System.Collections.Generic
'Imports System.Text
'Imports Contensive.BaseClasses
'Imports Contensive.Addons.aoRssFeed2
'Imports AddonCollectionVb.Views
'Imports AddonCollectionVb.Controllers

Namespace Contensive.Addons.aoRssFeed2.Controllers
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
            '
            Dim WorkString As String
            Dim WorkLong As Integer
            '
            If IsDate(DateValue) Then
                Select Case Weekday(DateValue)
                    Case vbSunday
                        GetGMTFromDate = "Sun, "
                    Case vbMonday
                        GetGMTFromDate = "Mon, "
                    Case vbTuesday
                        GetGMTFromDate = "Tue, "
                    Case vbWednesday
                        GetGMTFromDate = "Wed, "
                    Case vbThursday
                        GetGMTFromDate = "Thu, "
                    Case vbFriday
                        GetGMTFromDate = "Fri, "
                    Case vbSaturday
                        GetGMTFromDate = "Sat, "
                End Select
                '
                WorkLong = Day(DateValue)
                If WorkLong < 10 Then
                    GetGMTFromDate = GetGMTFromDate & "0" & CStr(WorkLong) & " "
                Else
                    GetGMTFromDate = GetGMTFromDate & CStr(WorkLong) & " "
                End If
                '
                Select Case Month(DateValue)
                    Case 1
                        GetGMTFromDate = GetGMTFromDate & "Jan "
                    Case 2
                        GetGMTFromDate = GetGMTFromDate & "Feb "
                    Case 3
                        GetGMTFromDate = GetGMTFromDate & "Mar "
                    Case 4
                        GetGMTFromDate = GetGMTFromDate & "Apr "
                    Case 5
                        GetGMTFromDate = GetGMTFromDate & "May "
                    Case 6
                        GetGMTFromDate = GetGMTFromDate & "Jun "
                    Case 7
                        GetGMTFromDate = GetGMTFromDate & "Jul "
                    Case 8
                        GetGMTFromDate = GetGMTFromDate & "Aug "
                    Case 9
                        GetGMTFromDate = GetGMTFromDate & "Sep "
                    Case 10
                        GetGMTFromDate = GetGMTFromDate & "Oct "
                    Case 11
                        GetGMTFromDate = GetGMTFromDate & "Nov "
                    Case 12
                        GetGMTFromDate = GetGMTFromDate & "Dec "
                End Select
                '
                GetGMTFromDate = GetGMTFromDate & CStr(Year(DateValue)) & " "
                '
                WorkLong = Hour(DateValue)
                If WorkLong < 10 Then
                    GetGMTFromDate = GetGMTFromDate & "0" & CStr(WorkLong) & ":"
                Else
                    GetGMTFromDate = GetGMTFromDate & CStr(WorkLong) & ":"
                End If
                '
                WorkLong = Minute(DateValue)
                If WorkLong < 10 Then
                    GetGMTFromDate = GetGMTFromDate & "0" & CStr(WorkLong) & ":"
                Else
                    GetGMTFromDate = GetGMTFromDate & CStr(WorkLong) & ":"
                End If
                '
                WorkLong = Second(DateValue)
                If WorkLong < 10 Then
                    GetGMTFromDate = GetGMTFromDate & "0" & CStr(WorkLong)
                Else
                    GetGMTFromDate = GetGMTFromDate & CStr(WorkLong)
                End If
                '
                GetGMTFromDate = GetGMTFromDate & " GMT"
            End If
            '
        End Function


    End Class
End Namespace

