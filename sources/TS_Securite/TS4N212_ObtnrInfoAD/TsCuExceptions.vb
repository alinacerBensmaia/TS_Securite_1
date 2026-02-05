''' <summary>
'''   Exception lancé lorsque le code utilisateur n'existe pas.
''' </summary>
Public Class TsCuCodeUtilisateurInexistantException
    Inherits ApplicationException

    Public Sub New(ByVal CodeUtilisateur As String)
        MyBase.New(String.Format("Le code utilisateur '{0}' est inexistant.", CodeUtilisateur))
    End Sub
End Class

''' <summary>
'''   Exception lancé lorsque le utilisateur existe dans l'AD RQ, mais il n'est pas encore basculé.
''' </summary>
Public Class TsCuUtilisateurNonBaculeException
    Inherits ApplicationException

    Public Sub New(ByVal CodeUtilisateur As String)
        MyBase.New(String.Format("L'utilisateur '{0}' existe dans l'AD RQ, mais il n'est pas encore basculé.", CodeUtilisateur))
    End Sub
End Class

''' <summary>
'''   Exception lancé lorsqu'un paramètre est absent.
''' </summary>
Public Class TsCuParametreAbsentException
    Inherits ApplicationException

    Public Sub New(ByVal NomParametre As String)
        MyBase.New(String.Format("Le paramètre '{0}' n'est pas renseigné.", NomParametre))
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
End Class

''' <summary>
'''   Exception lancé lorsqu'une méthode n'est pas disponible pour le mode multi-domaine.
''' </summary>
Public Class TsCuMethodeNonSupporteMultipleDomaine
    Inherits ApplicationException

    Public Sub New(ByVal NomMethode As String)
        MyBase.New(String.Format("La méthode '{0}' n'est pas supporté dans le mode multi-domaines.", NomMethode))
    End Sub
End Class
