Public Class TsCdSageUserCollection
    Inherits TsCdCollectionSage(Of TsCdSageUser)

    Public ReadOnly Property Users() As List(Of TsCdSageUser)
        Get
            Return _list
        End Get
    End Property

End Class