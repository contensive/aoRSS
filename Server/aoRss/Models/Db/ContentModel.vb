
Option Explicit On
Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Models.Db
    Public Class ContentModel        '<------ set set model Name and everywhere that matches this string
        Inherits BaseModel
        'Implements ICloneable
        '
        '====================================================================================================
        '-- const
        Public Const contentName As String = "Content"      '<------ set content name
        Public Const contentTableName As String = "ccContent"   '<------ set to tablename for the primary content (used for cache names)
        Private Shadows Const contentDataSource As String = "default"             '<------ set to datasource if not default
        '
        '====================================================================================================
        ' -- instance properties

        Public Property AdminOnly As Boolean
        Public Property AllowAdd As Boolean
        Public Property AllowContentChildTool As Boolean
        Public Property AllowContentTracking As Boolean
        Public Property AllowDelete As Boolean
        Public Property AllowMetaContent As Boolean
        Public Property AllowTopicRules As Boolean
        Public Property AllowWorkflowAuthoring As Boolean
        Public Property AuthoringTableID As Integer
        Public Property ContentTableID As Integer
        Public Property DefaultSortMethodID As Integer
        Public Property DeveloperOnly As Boolean
        Public Property DropDownFieldList As String
        Public Property EditorGroupID As Integer
        Public Property IconHeight As Integer
        Public Property IconLink As String
        Public Property IconSprites As Integer
        Public Property IconWidth As Integer
        Public Property InstalledByCollectionID As Integer
        Public Property IsBaseContent As Boolean
        Public Property ParentID As Integer
    End Class
End Namespace
