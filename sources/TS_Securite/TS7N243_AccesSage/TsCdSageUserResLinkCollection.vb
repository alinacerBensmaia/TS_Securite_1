Public Class TsCdSageUserResLinkCollection
    Inherits TsCdCollectionSage(Of TsCdSageUserResLink)

    Public ReadOnly Property Links() As List(Of TsCdSageUserResLink)
        Get
            Return _list
        End Get
    End Property

End Class