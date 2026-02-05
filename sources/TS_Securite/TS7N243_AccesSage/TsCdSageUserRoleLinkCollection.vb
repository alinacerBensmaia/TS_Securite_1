Public Class TsCdSageUserRoleLinkCollection
    Inherits TsCdCollectionSage(Of TsCdSageUserRoleLink)

    Public ReadOnly Property Links() As List(Of TsCdSageUserRoleLink)
        Get
            Return _list
        End Get
    End Property
End Class