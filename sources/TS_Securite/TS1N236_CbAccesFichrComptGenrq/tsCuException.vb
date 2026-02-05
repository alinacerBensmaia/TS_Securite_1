<Serializable()> Public Class TsCuDroitsInsuffisants
    Inherits ApplicationException

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
        MyBase.New(info, context)
    End Sub

    Public Sub New()
        MyBase.New("Erreur lors de la validation d'accès.  Demande de récupération du code d'usager/mot de passe impossible, car le demandeur n'a pas les droits suffisants.  Contactez l'équipe de la sécurité.")
    End Sub

    Public Sub New(ByVal strUtilisateur As String)
        MyBase.New("Erreur lors de la validation d'accès.  Demande de récupération du code d'usager/mot de passe pour l'utilisateur " & strUtilisateur & " impossible, car le demandeur n'a pas les droits suffisants.  Contactez l'équipe de la sécurité.")
    End Sub

    Friend Sub AssignerRaison(ByRef compte As String, ByRef motPasse As String)
        compte = "<Non Valide>"
        motPasse = "<Opération non permise>"
    End Sub
End Class

<Serializable()> Public Class TsCuDroitsGestionInsuffisants
    Inherits ApplicationException

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
        MyBase.New(info, context)
    End Sub

    Public Sub New()
        MyBase.New("Erreur lors de la validation d'accès.  Le demandeur n'a pas les droits nécessaires pour effectuer la gestion des codes d'accès génériques.  Contactez l'équipe de la sécurité.")
    End Sub

    Public Sub New(ByVal strUtilisateur As String)
        MyBase.New("Erreur lors de la validation d'accès.  Le demandeur (" & strUtilisateur & ") n'a pas les droits nécessaires pour effectuer la gestion des codes d'accès génériques.  Contactez l'équipe de la sécurité.")
    End Sub
End Class

<Serializable()> Public Class TsCuCodeInexistant
    Inherits ApplicationException

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
        MyBase.New(info, context)
    End Sub

    Public Sub New()
        MyBase.New("La clé d'accès n'a retourné aucun code d'usager/mot de passe.")
    End Sub

    Public Sub New(ByVal strCleAcces As String)
        MyBase.New("La clé d'accès " & strCleAcces & " n'a retourné aucun code d'usager/mot de passe.")
    End Sub
    Friend Sub AssignerRaison(ByRef compte As String, ByRef motPasse As String)
        compte = "<inexistant>"
        motPasse = "<aucun>"
    End Sub
End Class

<Serializable()> Public Class TsCuResultatMultiple
    Inherits ApplicationException

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
        MyBase.New(info, context)
    End Sub

    Public Sub New()
        MyBase.New("La recherche a retourné plus d'un code d'usager/mot de passe.  Précisez votre recherche.")
    End Sub

    Public Sub New(ByVal strCleAcces As String)
        MyBase.New("La recherche a retourné plus d'un code d'usager/mot de passe pour la clé " & strCleAcces & ".  Précisez votre recherche.")
    End Sub

    Friend Sub AssignerRaison(ByRef compte As String, ByRef motPasse As String)
        compte = "<Cle multiple>"
        motPasse = "<aucun>"
    End Sub
End Class

<Serializable()> Public Class TsCuRaisonObligatoire
    Inherits ApplicationException

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
        MyBase.New(info, context)
    End Sub

    Public Sub New()
        MyBase.New("Demande de récupération du mot de passe impossible, car la raison de la demande n'est pas spécifiée.")
    End Sub

    Friend Sub AssignerRaison(ByRef compte As String, ByRef motPasse As String)
        compte = "<Non Valide>"
        motPasse = "<Raison obligatoire>"
    End Sub
End Class

<Serializable()> Public Class TsCuInfoADInaccessible
    Inherits ApplicationException

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
        MyBase.New(info, context)
    End Sub

    Public Sub New()
        MyBase.New("Impossible d'accéder aux informations de l'utilisateur.")
    End Sub

    Public Sub New(ByVal strACID As String)
        MyBase.New("Impossible d'accéder aux informations de l'utilisateur " & strACID)
    End Sub
End Class

<Serializable()> Public Class TsCuCodeVerifAbsent
    Inherits ApplicationException

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
        MyBase.New(info, context)
    End Sub

    Public Sub New()
        MyBase.New("Erreur lors de la validation d'accès.  Compte de type inforoute avec vérification sans code de vérification passé en paramètre.")
    End Sub

    Friend Sub AssignerRaison(ByRef compte As String, ByRef motPasse As String)
        compte = "<Non Valide>"
        motPasse = "<Opération non permise>"
    End Sub
End Class

<Serializable()> Public Class TsCuCodeVerifInvalide
    Inherits ApplicationException

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
        MyBase.New(info, context)
    End Sub

    Public Sub New()
        MyBase.New("Erreur lors de la validation d'accès.  Le code de vérification passé en paramètre est invalide.")
    End Sub

    Friend Sub AssignerRaison(ByRef compte As String, ByRef motPasse As String)
        compte = "<Non Valide>"
        motPasse = "<Opération non permise>"
    End Sub
End Class

<Serializable()> Public Class TsCuLectureXMLImpossible
    Inherits ApplicationException

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
        MyBase.New(info, context)
    End Sub

    Public Sub New()
        MyBase.New("Impossible de lire le fichier XML contenant les paramètres nécessaire à l'obtention des codes usager/mot de passe.")
    End Sub

    Public Sub New(ByVal ex As Exception)
        MyBase.New("Impossible de lire le fichier XML contenant les paramètres nécessaire à l'obtention des codes usager/mot de passe.", ex)
    End Sub
End Class

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

<Serializable()> Public Class TsCuNomFichInforouteAbsent
    Inherits ApplicationException

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
        MyBase.New(info, context)
    End Sub
    Public Sub New()
        MyBase.New("Les chemins des fichiers de l'inforoute et ZDE sont obligatoires pour un code d'accès générique de type <Inforoute>.")
    End Sub
End Class
