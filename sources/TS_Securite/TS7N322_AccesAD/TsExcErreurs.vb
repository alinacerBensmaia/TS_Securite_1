''' <summary>
''' Le membre n'existe pas dans l'AD.
''' </summary>
''' <remarks></remarks>
<Serializable()> _
Public Class TsExcMembreInexistant
    Inherits ApplicationException

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

''' <summary>
''' L'utilisateur n'existe pas dans l'AD.
''' </summary>
''' <remarks></remarks>
<Serializable()> _
Public Class TsExcUtilisateurInexistant
    Inherits ApplicationException

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

''' <summary>
''' Le groupe n'existe pas dans l'AD.
''' </summary>
''' <remarks></remarks>
<Serializable()> _
Public Class TsExcGroupeInexistant
    Inherits ApplicationException

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

''' <summary>
''' L'objet existe déja dans l'AD.
''' HResult reference: -2147019886
''' </summary>
''' <remarks></remarks>
<Serializable()> _
Public Class TsExcObjetDejaExistantAD
    Inherits ApplicationException

#Region "Constante"
    Public Const CODE_HRESULT As Integer = -2147019886
#End Region

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

''' <summary>
''' L'attribut ou la valeur de service d'annuaire spécifié existe déjà.
''' HResult reference: -2147016691
''' </summary>
''' <remarks></remarks>
<Serializable()> _
Public Class TsExcLienDejaExistantAD
    Inherits ApplicationException

#Region "Constante"
    Public Const CODE_HRESULT As Integer = -2147016691
#End Region

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

''' <summary>
''' L'attribut ou la valeur de service d'annuaire spécifié n'existe pas.
'''  HResult reference: -2147016694
''' </summary>
''' <remarks></remarks>
<Serializable()> _
Public Class TsExcLienInexistantAD
    Inherits ApplicationException

#Region "Constante"
    Public Const CODE_HRESULT As Integer = -2147016694
#End Region

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

''' <summary>
''' Le serveur refuse de performer l'opération demandé.
'''  HResult reference: -2147016651
''' </summary>
''' <remarks>Cette erreur se produit lorsque le serveur a des règles spécial qui ne concerne pas de aucune erreur spécifique.</remarks>
<Serializable()> _
Public Class TsExcServeurRefuseOperation
    Inherits ApplicationException

#Region "Constante"
    Public Const CODE_HRESULT As Integer = -2147016651
#End Region

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