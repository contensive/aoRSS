
Namespace Models.Db
    Public Class RSSAggregatorSourceRuleModel
        Inherits BaseModel
        Implements ICloneable
        '
        '====================================================================================================
        '-- const
        Public Const contentName As String = "RSS Aggregator Source Rules"                   '<------ set content name
        Public Const contentTableName As String = "aoRSSAggregatorSourceRules"            '<------ set to tablename for the primary content (used for cache names)
        Private Shadows Const contentDataSource As String = "default"   '<------ set to datasource if not default
        '
        '====================================================================================================
        ' -- instance properties
        Public Property AggregatorID As Integer
        Public Property SourceID As Integer
        Public Property link As String
        Public Property articleName As String
        Public Property pubdate As String
        Public Property description As String
    End Class
End Namespace
