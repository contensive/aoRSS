
Option Explicit On
Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Models.Db
    Public Class RSSFeedModel        '<------ set set model Name and everywhere that matches this string
        Inherits BaseModel
        Implements ICloneable
        '
        '====================================================================================================
        '-- const
        Public Const contentName As String = "RSS Feeds"      '<------ set content name
        Public Const contentTableName As String = "ccRSSFeeds"   '<------ set to tablename for the primary content (used for cache names)
        Private Shadows Const contentDataSource As String = "default"             '<------ set to datasource if not default
        '
        '====================================================================================================
        ' -- instance properties
        Public Property Copyright As String
        Public Property Description As String
        Public Property Link As String
        Public Property LogoFilename As String
        Public Property RSSDateUpdated As Date
        Public Property RSSFilename As String
        ''
        ''====================================================================================================
        'Public Overloads Shared Function add(cp As CPBaseClass) As RSSFeedModel
        '    Return add(Of RSSFeedModel)(cp)
        'End Function


    End Class
End Namespace
