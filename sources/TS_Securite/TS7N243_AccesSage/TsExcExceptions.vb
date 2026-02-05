''' <summary>
''' Classe d'exception. Est lancé lorsque l'ajout d'un rôle est demandé, 
''' mais il est déja présent dans la configuration ciblé dans Sage.
''' </summary>
''' <remarks></remarks>
Public Class TsExcSageRoleDejaExistant
    Inherits Exception

#Region "Constructeurs"

    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(ByVal message As String, ByVal innerException As Exception)
        MyBase.New(message, innerException)
    End Sub

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, _
                   ByVal context As System.Runtime.Serialization.StreamingContext)

        MyBase.New(info, context)
    End Sub

#End Region

#Region "Méthode"

    Public Overrides Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, _
                                       ByVal context As System.Runtime.Serialization.StreamingContext)

        MyBase.GetObjectData(info, context)

    End Sub

#End Region

End Class


