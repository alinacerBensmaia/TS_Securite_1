Public Class TsCdSageRoleResLinkCollection
    Inherits TsCdCollectionSage(Of TsCdSageRoleResLink)

    Public ReadOnly Property Links() As List(Of TsCdSageRoleResLink)
        Get
            Return _list
        End Get
    End Property

End Class
