''' <summary>
''' Le rôle n'existe pas dans la configuration Sage.
''' </summary>
''' <remarks></remarks>
<Serializable()>
Public Class TsExcRoleInexistant
    Inherits ApplicationException

#Region "Constructeurs"
    Public Sub New()

        MyBase.New("Une erreur est survenue: 'Le rôle est inexistant'.")

    End Sub

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo,
                   ByVal context As System.Runtime.Serialization.StreamingContext)

        MyBase.New(info, context)
    End Sub
#End Region

#Region "Méthode"
    Public Overrides Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo,
                                       ByVal context As System.Runtime.Serialization.StreamingContext)

        MyBase.GetObjectData(info, context)

    End Sub
#End Region
End Class

''' <summary>
''' L'utilisateur n'existe pas dans la configuration Sage.
''' </summary>
''' <remarks></remarks>
<Serializable()>
Public Class TsExcUtilisateurInexistant
    Inherits ApplicationException

#Region "Constructeurs"
    Public Sub New()

        MyBase.New("Une erreur est survenue: 'L'utilisateur est inexistant'")

    End Sub

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo,
                   ByVal context As System.Runtime.Serialization.StreamingContext)

        MyBase.New(info, context)
    End Sub
#End Region

#Region "Méthode"
    Public Overrides Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo,
                                       ByVal context As System.Runtime.Serialization.StreamingContext)

        MyBase.GetObjectData(info, context)

    End Sub
#End Region
End Class

''' <summary>
''' Impossible de modifiée l'utilisateur.
''' </summary>
''' <remarks></remarks>
<Serializable()>
Public Class TsExcModificationImpossible
    Inherits ApplicationException

#Region "Constructeurs"
    Public Sub New()

        MyBase.New("Une condition d'utilisation n'a pas été respecter: 'Paramètre d'entrée FinPrevue n'est pas égale à False'")

    End Sub

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo,
                   ByVal context As System.Runtime.Serialization.StreamingContext)

        MyBase.New(info, context)
    End Sub
#End Region

#Region "Méthode"
    Public Overrides Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo,
                                       ByVal context As System.Runtime.Serialization.StreamingContext)

        MyBase.GetObjectData(info, context)

    End Sub
#End Region
End Class

''' <summary>
''' Des duplicatas ont été trouvés.
''' </summary>
''' <remarks></remarks>
<Serializable()>
Public Class TsExcRedondanceTrouve
    Inherits ApplicationException

#Region "Constructeurs"
    Public Sub New()

        MyBase.New("Une erreur est survenue: Une Opération d'ajout essaie d'ajouté un élément déja présent")

    End Sub

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo,
                   ByVal context As System.Runtime.Serialization.StreamingContext)

        MyBase.New(info, context)
    End Sub
#End Region

#Region "Méthode"
    Public Overrides Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo,
                                       ByVal context As System.Runtime.Serialization.StreamingContext)

        MyBase.GetObjectData(info, context)

    End Sub
#End Region
End Class

''' <summary>
''' Impossible d'ajouté le rôle.
''' </summary>
''' <remarks></remarks>
<Serializable()>
Public Class TsExcAjoutImpossible
    Inherits ApplicationException

#Region "Constructeurs"
    Public Sub New()

        MyBase.New("Une erreur est survenue lors de l'ajout.")

    End Sub

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo,
                   ByVal context As System.Runtime.Serialization.StreamingContext)

        MyBase.New(info, context)
    End Sub
#End Region

#Region "Méthode"
    Public Overrides Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo,
                                       ByVal context As System.Runtime.Serialization.StreamingContext)

        MyBase.GetObjectData(info, context)

    End Sub
#End Region
End Class

''' <summary>
''' Le rôle n'a pas été trouvé sur le serveur Sage.
''' </summary>
''' <remarks></remarks>
<Serializable()>
Public Class TsExcModificationIntrouvable
    Inherits ApplicationException

#Region "Constructeurs"
    Public Sub New()

        MyBase.New("Une erreur est survenue lors de l'oppération de modification.")

    End Sub

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo,
                   ByVal context As System.Runtime.Serialization.StreamingContext)

        MyBase.New(info, context)
    End Sub
#End Region

#Region "Méthode"
    Public Overrides Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo,
                                       ByVal context As System.Runtime.Serialization.StreamingContext)

        MyBase.GetObjectData(info, context)

    End Sub
#End Region
End Class

''' <summary>
''' Le rôle n'a pas été trouver sur le serveur sage.
''' </summary>
''' <remarks></remarks>
<Serializable()>
Public Class TsExcSupressionIntrouvable
    Inherits ApplicationException

#Region "Constructeurs"
    Public Sub New()

        MyBase.New("Une erreur est survenue: Une Opération de suppression essaie de supprimé un élément inexistant")

    End Sub

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo,
                   ByVal context As System.Runtime.Serialization.StreamingContext)

        MyBase.New(info, context)
    End Sub
#End Region

#Region "Méthode"
    Public Overrides Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo,
                                       ByVal context As System.Runtime.Serialization.StreamingContext)

        MyBase.GetObjectData(info, context)

    End Sub
#End Region
End Class

''' <summary>
''' Impossible d'effacer le rôle.
''' </summary>
<Serializable()>
Public Class TsExcSupressionImpossible
    Inherits ApplicationException

#Region "Constructeurs"
    Public Sub New()

        MyBase.New("Une erreur est survenue lors de la supression. ")

    End Sub

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo,
                   ByVal context As System.Runtime.Serialization.StreamingContext)

        MyBase.New(info, context)
    End Sub
#End Region

#Region "Méthode"
    Public Overrides Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo,
                                       ByVal context As System.Runtime.Serialization.StreamingContext)

        MyBase.GetObjectData(info, context)

    End Sub
#End Region
End Class

''' <summary>
''' Une erreur provoquée durant une demande de création d'un utilisateur.
''' </summary>
''' <remarks></remarks>
<Serializable()>
Public Class TsExcErreurDemandeAjout
    Inherits ApplicationException

#Region "Constructeurs"
    Public Sub New()

        MyBase.New("Une erreur est survenue lors de la demande d'ajoute d'utilisateur.")

    End Sub

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo,
                   ByVal context As System.Runtime.Serialization.StreamingContext)

        MyBase.New(info, context)
    End Sub
#End Region

#Region "Méthode"
    Public Overrides Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo,
                                       ByVal context As System.Runtime.Serialization.StreamingContext)

        MyBase.GetObjectData(info, context)

    End Sub
#End Region
End Class

''' <summary>
''' Une erreur provoquée durant une demande de destruction d'un utilisateur.
''' </summary>
''' <remarks></remarks>
<Serializable()>
Public Class TsExcErreurDemandeDestruction
    Inherits ApplicationException

#Region "Constructeurs"
    Public Sub New()

        MyBase.New("Une erreur est survenue lors de la demande de modification de l'utilisateur.")

    End Sub

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo,
                   ByVal context As System.Runtime.Serialization.StreamingContext)

        MyBase.New(info, context)
    End Sub
#End Region

#Region "Méthode"
    Public Overrides Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo,
                                       ByVal context As System.Runtime.Serialization.StreamingContext)

        MyBase.GetObjectData(info, context)

    End Sub
#End Region
End Class

''' <summary>
''' Une erreur provoquée durant une demande de modification d'un utilisateur.
''' </summary>
''' <remarks></remarks>
<Serializable()>
Public Class TsExcErreurDemandeModification
    Inherits ApplicationException

#Region "Constructeurs"
    Public Sub New()

        MyBase.New("Une erreur est survenue lors de la demande de modification de l'utilisateur.")

    End Sub

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo,
                   ByVal context As System.Runtime.Serialization.StreamingContext)

        MyBase.New(info, context)
    End Sub
#End Region

#Region "Méthode"
    Public Overrides Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo,
                                       ByVal context As System.Runtime.Serialization.StreamingContext)

        MyBase.GetObjectData(info, context)

    End Sub
#End Region
End Class

''' <summary>
''' Class d'erreur générique ne renvois pas d'erreur spécifique.
''' Peut être typé pour spécifié des erreurs.
''' </summary>
''' <remarks></remarks>
<Serializable()>
Public Class TsExcErreurGeneral
    Inherits ApplicationException

#Region "COUNSTANTES"

    Private Const TYPE_DEFAULT As String = "AUCUN TYPE"

#End Region

#Region "Public vars"

    Private mType As String

#End Region

#Region "Property"
    Public ReadOnly Property Type() As String
        Get
            Return mType
        End Get
    End Property


#End Region

#Region "Constructeurs"
    Public Sub New(ByVal info As String, Optional ByVal type As String = TYPE_DEFAULT)
        MyBase.New(info)
        Me.mType = type
    End Sub

    Public Sub New(ByVal info As String, ByVal innerException As Exception, Optional ByVal type As String = TYPE_DEFAULT)
        MyBase.New(info, innerException)
        Me.mType = type
    End Sub

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo,
                   ByVal context As System.Runtime.Serialization.StreamingContext)

        MyBase.New(info, context)
        'TODO: revoir Pat
        'mType = info.GetString("mType")
    End Sub
#End Region

#Region "Méthode"
    Public Overrides Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo,
                                       ByVal context As System.Runtime.Serialization.StreamingContext)

        MyBase.GetObjectData(info, context)
        'info.AddValue("mType", mType)
    End Sub
#End Region
End Class