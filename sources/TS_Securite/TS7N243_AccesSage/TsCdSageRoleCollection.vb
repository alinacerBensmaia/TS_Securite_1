Public Class TsCdSageRoleCollection
    Inherits TsCdCollectionSage(Of TsCdSageRole)

    Public ReadOnly Property Roles() As List(Of TsCdSageRole)
        Get
            Return _list
        End Get
    End Property

End Class
