Imports System.Runtime.Serialization

''' <summary>
''' L'objet existe déja dans l'AD.
''' HResult reference: -2147019886
''' </summary>
''' <remarks></remarks>
<Serializable()>
Public Class TsExcObjetDejaExistantADLDS
    Inherits ApplicationException
    Public Const CODE_HRESULT As Integer = -2147019886


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