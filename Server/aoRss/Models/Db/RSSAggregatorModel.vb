
Namespace Models.Db
    Public Class RSSAggregatorModel
        Inherits BaseModel
        Implements ICloneable
        '
        '====================================================================================================
        '-- const
        Public Const contentName As String = "RSS Aggregators"                   '<------ set content name
        Public Const contentTableName As String = "aoRSSAggregators"            '<------ set to tablename for the primary content (used for cache names)
        Private Shadows Const contentDataSource As String = "default"   '<------ set to datasource if not default
        '
        '====================================================================================================
        ' -- instance properties
        ' Public Property DataSourceID As Integer                         '<------ replace this with a list all model fields not part of the base model
        '
        ''====================================================================================================
        'Public Overloads Shared Function add(cp As CPBaseClass) As RSSAggregatorModel
        '    Return add(Of RSSAggregatorModel)(cp)
        'End Function
        ''
        ''====================================================================================================
        'Public Overloads Shared Function create(cp As CPBaseClass, recordId As Integer) As RSSAggregatorModel
        '    Return create(Of RSSAggregatorModel)(cp, recordId)
        'End Function
        ''
        ''====================================================================================================
        'Public Overloads Shared Function create(cp As CPBaseClass, recordGuid As String) As RSSAggregatorModel
        '    Return create(Of RSSAggregatorModel)(cp, recordGuid)
        'End Function
        ''
        ''====================================================================================================
        'Public Overloads Shared Function createByName(cp As CPBaseClass, recordName As String) As RSSAggregatorModel
        '    Return createByName(Of RSSAggregatorModel)(cp, recordName)
        'End Function
        ''
        ''====================================================================================================
        'Public Overloads Sub save(cp As CPBaseClass)
        '    MyBase.save(cp)
        'End Sub
        ''
        ''====================================================================================================
        'Public Overloads Shared Sub delete(cp As CPBaseClass, recordId As Integer)
        '    delete(Of RSSAggregatorModel)(cp, recordId)
        'End Sub
        ''
        ''====================================================================================================
        'Public Overloads Shared Sub delete(cp As CPBaseClass, ccGuid As String)
        '    delete(Of RSSAggregatorModel)(cp, ccGuid)
        'End Sub
        ''
        ''====================================================================================================
        'Public Overloads Shared Function createList(cp As CPBaseClass, sqlCriteria As String, Optional sqlOrderBy As String = "id") As List(Of RSSAggregatorModel)
        '    Return createList(Of RSSAggregatorModel)(cp, sqlCriteria, sqlOrderBy)
        'End Function
        ''
        ''====================================================================================================
        'Public Overloads Shared Function getRecordName(cp As CPBaseClass, recordId As Integer) As String
        '    Return BaseModel.getRecordName(Of RSSAggregatorModel)(cp, recordId)
        'End Function
        ''
        ''====================================================================================================
        'Public Overloads Shared Function getRecordName(cp As CPBaseClass, ccGuid As String) As String
        '    Return BaseModel.getRecordName(Of RSSAggregatorModel)(cp, ccGuid)
        'End Function
        ''
        ''====================================================================================================
        'Public Overloads Shared Function getRecordId(cp As CPBaseClass, ccGuid As String) As Integer
        '    Return BaseModel.getRecordId(Of RSSAggregatorModel)(cp, ccGuid)
        'End Function
        ''
        ''====================================================================================================
        'Public Overloads Shared Function getCount(cp As CPBaseClass, sqlCriteria As String) As Integer
        '    Return BaseModel.getCount(Of RSSAggregatorModel)(cp, sqlCriteria)
        'End Function
        ''
        ''====================================================================================================
        'Public Overloads Function getUploadPath(fieldName As String) As String
        '    Return MyBase.getUploadPath(Of RSSAggregatorModel)(fieldName)
        'End Function
        ''
        ''====================================================================================================
        ''
        'Public Function Clone(cp As CPBaseClass) As RSSAggregatorModel
        '    Dim result As RSSAggregatorModel = DirectCast(Me.Clone(), RSSAggregatorModel)
        '    result.id = cp.Content.AddRecord(contentName)
        '    result.ccguid = cp.Utils.CreateGuid()
        '    result.save(cp)
        '    Return result
        'End Function
        ''
        ''====================================================================================================
        ''
        'Public Function Clone() As Object Implements ICloneable.Clone
        '    Return Me.MemberwiseClone()
        'End Function

    End Class
End Namespace
