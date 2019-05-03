
Option Explicit On
Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Models.Db
    Public Class ContentFieldModel        '<------ set set model Name and everywhere that matches this string
        Inherits BaseModel
        'Implements ICloneable
        '
        '====================================================================================================
        '-- const
        Public Const contentName As String = "Content Fields"      '<------ set content name
        Public Const contentTableName As String = "ccFields"   '<------ set to tablename for the primary content (used for cache names)
        Private Shadows Const contentDataSource As String = "default"             '<------ set to datasource if not default
        '
        '====================================================================================================
        ' -- instance properties

        'Public Property AdminOnly As Boolean
        'Public Property Authorable As Boolean
        'Public Property Caption As String
        Public Property ContentID As Integer
        'Public Property DefaultValue As String
        'Public Property DeveloperOnly As Boolean
        'Public Property editorAddonID As Integer
        'Public Property EditSortPriority As Integer
        'Public Property EditTab As String
        'Public Property HTMLContent As Boolean
        'Public Property IndexColumn As Integer
        'Public Property IndexSortDirection As Integer
        'Public Property IndexSortPriority As Integer
        'Public Property IndexWidth As String
        'Public Property InstalledByCollectionID As Integer
        'Public Property IsBaseField As Boolean
        'Public Property LookupContentID As Integer
        'Public Property LookupList As String
        Public Property ManyToManyContentID As Integer
        Public Property ManyToManyRuleContentID As Integer
        Public Property ManyToManyRulePrimaryField As String
        Public Property ManyToManyRuleSecondaryField As String
        'Public Property MemberSelectGroupID As Integer
        'Public Property NotEditable As Boolean
        'Public Property Password As Boolean
        'Public Property prefixForRootResourceFiles As String
        ''Public Property ReadOnly As Boolean
        'Public Property RedirectContentID As Integer
        'Public Property RedirectID As String
        'Public Property RedirectPath As String
        'Public Property Required As Boolean
        'Public Property RSSDescriptionField As Boolean
        'Public Property RSSTitleField As Boolean
        'Public Property Scramble As Boolean
        'Public Property TextBuffered As Boolean
        'Public Property Type As Integer
        'Public Property UniqueName As Boolean        '
    End Class
End Namespace
