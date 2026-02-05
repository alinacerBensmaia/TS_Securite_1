''' <summary>
''' Exception lancé lorsque le groupe n'existe pas.
''' </summary>
<System.Serializable()> _
Public Class TsCuGroupeInexistantException
    Inherits ApplicationException

    Public Sub New(ByVal Groupe As String)
        MyBase.New(String.Format("Le groupe '{0}' est inexistant.", Groupe))
    End Sub

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, _
               ByVal context As System.Runtime.Serialization.StreamingContext)
        MyBase.New(info, context)
    End Sub
End Class


''' <summary>
''' Exception lancé lorsque le domaine infomré n'est pas reconnu.
''' </summary>
<System.Serializable()> _
Public Class TsCuDomaineActiveDirectoryInconnu
    Inherits ApplicationException

    Public Sub New(ByVal Domaine As String)
        MyBase.New(String.Format("Le domaine '{0}' est inconnu.", Domaine))
    End Sub

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, _
               ByVal context As System.Runtime.Serialization.StreamingContext)
        MyBase.New(info, context)
    End Sub
End Class

''' <summary>
''' Exception lancé lorsque la sécurité applicative a été migré à ADLDS
''' </summary>
<System.Serializable()>
Public Class TsCuSecuriteApplicativeMigreVersADLDS
    Inherits ApplicationException

    Public Sub New()
        MyBase.New("La sécurité applicative de cette infrastructure a été migré vers ADLDS. " +
                   "Le composant TS4N231_AccesAD ne doit plus être utilisé. Remplacer ce composant par TS4N311_VerfrSecrtApplicative.")
    End Sub

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo,
               ByVal context As System.Runtime.Serialization.StreamingContext)
        MyBase.New(info, context)
    End Sub
End Class


''' <summary>
''' Exception lancé lorsqu'un utilisateur est inexistant dans l'AD.
''' </summary>
<System.Serializable()>
Public Class TsCuUtilisateurInexistantException
    Inherits ApplicationException

    Public Sub New(ByVal usager As String, ByVal domaine As String)
        MyBase.New(String.Format("L'utilisateur '{0}' est inexistant dans le domaine {1}.", usager, domaine))
    End Sub

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo,
               ByVal context As System.Runtime.Serialization.StreamingContext)
        MyBase.New(info, context)
    End Sub
End Class


''' <summary>
''' Exception lancé lorsqu'un utilisateur est inexistant dans l'AD.
''' </summary>
<System.Serializable()>
Public Class TsCuSIdInexistantException
    Inherits ApplicationException

    Public Sub New(ByVal SId As String)
        MyBase.New(String.Format("Le SID '{0}' est inexistant dans l'AD RQ et RRQ_QC.", SId))
    End Sub

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo,
               ByVal context As System.Runtime.Serialization.StreamingContext)
        MyBase.New(info, context)
    End Sub
End Class