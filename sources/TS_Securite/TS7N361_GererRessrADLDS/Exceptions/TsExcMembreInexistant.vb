Imports System.Runtime.Serialization

''' <summary>
''' Le membre n'existe pas dans l'AD.
''' </summary>
''' <remarks></remarks>
<Serializable()>
Public Class TsExcMembreInexistant
    Inherits ApplicationException


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