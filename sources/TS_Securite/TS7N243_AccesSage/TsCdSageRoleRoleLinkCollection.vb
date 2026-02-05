Public Class TsCdSageRoleRoleLinkCollection
    Inherits TsCdCollectionSage(Of TsCdSageRoleRoleLink)

    Public ReadOnly Property Links() As List(Of TsCdSageRoleRoleLink)
        Get
            Return _list
        End Get
    End Property
End Class
