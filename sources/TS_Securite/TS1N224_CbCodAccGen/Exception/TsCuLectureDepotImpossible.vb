<Serializable()> Public Class TsCuLectureDepotImpossible
    Inherits ApplicationException

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
        MyBase.New(info, context)
    End Sub

    Public Sub New()
        MyBase.New("Nombre maximum d'essai de lecture dépassé pour le fichier.")
    End Sub

    Public Sub New(ByVal ex As Exception, ByVal strFichier As String)
        MyBase.New("Nombre maximum d'essai de lecture dépassé pour le fichier '" + strFichier + "'", ex)
    End Sub
End Class