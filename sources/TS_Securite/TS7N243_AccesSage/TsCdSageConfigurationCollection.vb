Public Class TsCdSageConfigurationCollection
    Inherits TsCdCollectionSage(Of TsCdSageConfiguration)

    Public ReadOnly Property Configuration() As List(Of TsCdSageConfiguration)
        Get
            Return _list
        End Get
    End Property

End Class