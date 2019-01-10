
Option Explicit On
Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Models.Db
    Public Class PeopleModel        '<------ set set model Name and everywhere that matches this string
        Inherits BaseModel
        Implements ICloneable
        '
        '====================================================================================================
        '-- const
        Public Const contentName As String = "People"      '<------ set content name
        Public Const contentTableName As String = "ccMembers"   '<------ set to tablename for the primary content (used for cache names)
        Private Shadows Const contentDataSource As String = "default"             '<------ set to datasource if not default
        '
        '====================================================================================================
        ' -- instance properties

        Public Property Address As String
        Public Property Address2 As String
        Public Property Admin As Boolean
        Public Property AdminMenuModeID As Integer
        Public Property AllowBulkEmail As Boolean
        Public Property AllowToolsPanel As Boolean
        Public Property AutoLogin As Boolean
        Public Property BillAddress As String
        Public Property BillAddress2 As String
        Public Property BillCity As String
        Public Property BillCompany As String
        Public Property BillCountry As String
        Public Property BillEmail As String
        Public Property BillFax As String
        Public Property BillName As String
        Public Property BillPhone As String
        Public Property BillState As String
        Public Property BillZip As String
        Public Property BirthdayDay As Integer
        Public Property BirthdayMonth As Integer
        Public Property BirthdayYear As Integer
        Public Property City As String
        Public Property Company As String
        Public Property Country As String
        Public Property CreatedByVisit As Boolean
        Public Property DateExpires As Date
        Public Property Developer As Boolean
        Public Property Email As String
        Public Property ExcludeFromAnalytics As Boolean
        Public Property Fax As String
        Public Property FirstName As String
        Public Property ImageFilename As String
        Public Property LanguageID As Integer
        Public Property LastName As String
        Public Property LastVisit As Date
        Public Property nickName As String
        Public Property NotesFilename As String
        Public Property OrganizationID As Integer
        Public Property Password As String
        Public Property Phone As String
        Public Property ResumeFilename As String
        Public Property ShipAddress As String
        Public Property ShipAddress2 As String
        Public Property ShipCity As String
        Public Property ShipCompany As String
        Public Property ShipCountry As String
        Public Property ShipName As String
        Public Property ShipPhone As String
        Public Property ShipState As String
        Public Property ShipZip As String
        Public Property State As String
        Public Property StyleFilename As String
        Public Property ThumbnailFilename As String
        Public Property Title As String
        Public Property Username As String
        Public Property Visits As Integer
        Public Property Zip As String
    End Class
End Namespace
