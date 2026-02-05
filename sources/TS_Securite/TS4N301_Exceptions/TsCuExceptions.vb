''' <summary>
''' Exception lancé lorsque le groupe n'existe pas.
''' </summary>
<System.Serializable()> _
Public Class TsCuGroupeSecuriteInexistantException
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
''' Exception lancé lorsque l'utilisateur n'existe pas.
''' </summary>
<System.Serializable()> _
Public Class TsCuUtilisateurInexistantException
    Inherits ApplicationException

    Public Sub New(ByVal CodeUsager As String)
        MyBase.New(String.Format("Le compte usager '{0}' n'a pas été trouvé", CodeUsager))
    End Sub

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, _
               ByVal context As System.Runtime.Serialization.StreamingContext)
        MyBase.New(info, context)
    End Sub
End Class


''' <summary>
''' Exception lancé lorsque l'utilisateur n'a pas l'attribut userPrincipalName.
''' </summary>
<System.Serializable()> _
Public Class TsCuUtilisateurSansUserPrincipalName
    Inherits ApplicationException

    Public Sub New(ByVal distinguishedName As String)
        MyBase.New(String.Format("La propriété userPrincipalName est vide pour l'utilisateur '{0}'. S.V.P contacter l'équipe de sécurité.", distinguishedName))
    End Sub

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, _
               ByVal context As System.Runtime.Serialization.StreamingContext)
        MyBase.New(info, context)
    End Sub
End Class


''' <summary>
''' Exception lancé lorsque le type de mémorisation n'est pas connu.
''' </summary>
<System.Serializable()> _
Public Class TsCuRessourceSecuriteInconnuException
    Inherits ApplicationException

    Public Sub New()
        MyBase.New(String.Format("Le ressource de sécurité saisi dans le fichier de config TS4\TS4N311\RessourceSecurite est inconnu."))
    End Sub

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, _
               ByVal context As System.Runtime.Serialization.StreamingContext)
        MyBase.New(info, context)
    End Sub
End Class


''' <summary>
''' Exception lancé lorsque le type de mémorisation n'est pas connu.
''' </summary>
<System.Serializable()> _
Public Class TsCuTypeMemorisationInconnuException
    Inherits ApplicationException

    Public Sub New()
        MyBase.New(String.Format("Le type memorisation saisi dans le fichier de config TS4\TS4N331\TypeMemorisation est inconnu."))
    End Sub

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, _
               ByVal context As System.Runtime.Serialization.StreamingContext)
        MyBase.New(info, context)
    End Sub
End Class
