Public Class TsCdSageResourceCollection
    Inherits TsCdCollectionSage(Of TsCdSageResource)

    Public ReadOnly Property Resources() As List(Of TsCdSageResource)
        Get
            Return _list
        End Get
    End Property

End Class
