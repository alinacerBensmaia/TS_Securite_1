Imports System.Runtime.Serialization

''' <summary>
''' L'attribut ou la valeur de service d'annuaire spécifié existe déjà.
''' HResult reference: -2147016691
''' </summary>
''' <remarks></remarks>
<Serializable()>
Public Class TsExcLienDejaExistantADLDS
    Inherits ApplicationException
    Public Const CODE_HRESULT As Integer = -2147016691


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