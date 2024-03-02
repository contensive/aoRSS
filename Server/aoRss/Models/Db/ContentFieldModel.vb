
Namespace Models.Db
    Public Class ContentFieldModel        '<------ set set model Name and everywhere that matches this string
        Inherits BaseModel
        Public Const contentName As String = "Content Fields"      '<------ set content name
        Public Const contentTableName As String = "ccFields"   '<------ set to tablename for the primary content (used for cache names)
        Private Shadows Const contentDataSource As String = "default"             '<------ set to datasource if not default
        Public Property ContentID As Integer
        Public Property ManyToManyContentID As Integer
        Public Property ManyToManyRuleContentID As Integer
        Public Property ManyToManyRulePrimaryField As String
        Public Property ManyToManyRuleSecondaryField As String
    End Class
End Namespace
