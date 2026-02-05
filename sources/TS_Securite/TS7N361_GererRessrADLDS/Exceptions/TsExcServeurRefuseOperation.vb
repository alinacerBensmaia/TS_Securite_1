Imports System.Runtime.Serialization

''' <summary>
''' Le serveur refuse de performer l'opération demandé.
'''  HResult reference: -2147016651
''' </summary>
''' <remarks>Cette erreur se produit lorsque le serveur a des règles spécial qui ne concerne pas de aucune erreur spécifique.</remarks>
<Serializable()>
Public Class TsExcServeurRefuseOperation
    Inherits ApplicationException
    Public Const CODE_HRESULT As Integer = -2147016651


    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(ByVal message As String, ByVal innerException As Exception)
        MyBase.New(message, innerException)
    End Sub

    Public Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        MyBase.New(info, context)
    End Sub

    Public Overrides Sub GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        MyBase.GetObjectData(info, context)
    End Sub

End Class