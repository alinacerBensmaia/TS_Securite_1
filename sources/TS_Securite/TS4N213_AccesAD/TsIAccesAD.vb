Imports System.Security.Principal
Imports System.Collections.Generic

''' <summary>
''' Interface pour récupèrer des informations sur le contenu de l'active directory.
''' </summary>
Public Interface TsIAccesAD

    ''' <summary>
    ''' Cette méthode vérifie si un utilisateur est membre directement ou indirectement d'un groupe
    ''' </summary>
    ''' <remarks>Dans le UserToken, nous avons la liste de ses groupes de sécurité (direct et indirect). 
    ''' On prend en charge le cas que le NomGroupe est un groupe de distribution et donc non présent dans le UserToken.
    ''' Dans ce cas, on gére qu'un seul niveau de hierarchie. L'utilisateur lui même ou un de ses groupes de sécurité (issu du UserToken) doit être membre du groupe de distribution à vérifier.</remarks>
    ''' <param name="NomGroupe">Nom du groupe à vérifier</param>
    ''' <returns>True si l'utilisateur est un membre direct ou indirect du groupe</returns>
    Function EstMembreGroupe(ByVal NomGroupe As String, ByVal UserToken As WindowsIdentity) As Boolean

    ''' <summary>
    ''' Cette méthode obtient la liste de tous les utilisateurs membres d'au moins un des groupes spécifiés.
    ''' </summary>
    ''' <param name="LsNomGroupe">Liste des noms des groupes pour lesquels obtenir les utilisateurs.</param>
    ''' <param name="Recursif">Valeur indiquant s'il faut ou non obtenir les utilisateurs membres des groupes spécifiés de façon indirecte (via un autre groupe membre d'un des groupes spécifiés).</param>
    ''' <param name="InfoRetour">Valeur indiquant les informations à retourner pour chaque utilisateur. Plusieurs valeurs peuvent être combinées à l'aide d'un "Or"</param>
    ''' <returns>Liste de tous les utilisateurs membres d'au moins un des groupes spécifiés.</returns>
    ''' <remarks>Un utilisateur est considéré membre d'un groupe s'il en est "membre" au niveau de l'active directory ou si ce groupe est son groupe principal.</remarks>
    Function ObtenirUtilisateurGroupe(ByVal LsNomGroupe As IList(Of String), ByVal Recursif As Boolean, ByVal InfoRetour As TsAadInfoUtilisateur) As IList(Of TsDtUtilisateur)


    ''' <summary>
    ''' Cette méthode obtient la liste de tous les groupes pour l'utilisateur spécifié.
    ''' </summary>
    ''' <param name="UserToken">Token de l'utilisateur.</param>
    ''' <returns>Liste de tous les groupes pour l'utilisateur en paramètre.</returns>
    ''' <remarks>Seulement le premier niveau de groupe de distribution est obtenu.</remarks>
    Function ObtenirGroupeUtilisateur(ByVal UserToken As WindowsIdentity) As IList(Of TsDtGroupe)

    ''' <summary>
    ''' Cette méthode obtient la liste de tous les groupes pour l'utilisateur spécifié.
    ''' </summary>
    ''' <param name="Usager">Compte de l'utilisateur.</param>
    ''' <returns>Liste de tous les groupes pour l'utilisateur en paramètre.</returns>
    ''' <remarks>Seulement le premier niveau de groupe de distribution est obtenu.</remarks>
    Function ObtenirGroupeUtilisateur(ByVal Usager As String) As IList(Of TsDtGroupe)

    ''' <summary>
    ''' Cette méthode obtient la liste de tous les groupes pour l'utilisateur spécifié.
    ''' </summary>
    ''' <param name="NomGroupe">Nom du groupe</param>
    ''' <returns>Le groupe avec les information complémentaire.</returns>
    ''' <remarks>Seulement le premier niveau de groupe de distribution est obtenu.</remarks>
    Function ObtenirGroupe(ByVal NomGroupe As String, ByVal InfoRetour As TsAadInfoGroupe) As TsDtGroupe

    ''' <summary>
    ''' Cette méthode retourne une liste de groupe pour lesquels le groupe fourni est un membre.
    ''' </summary>
    ''' <param name="NomGroupe"></param>
    ''' <param name="Recursif"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function ObtenirGroupeMembreDe(ByVal NomGroupe As String, ByVal Recursif As Boolean) As IList(Of TsDtGroupe)

    ''' <summary>
    ''' Cette fonction permet d'obtenir une liste de groupes répondant aux critères du nom recherché.
    ''' </summary>
    ''' <param name="NomGroupe">Le nom du groupe à obtenir. Peut être un nom générique pour obtenir une liste</param>
    ''' <returns>Retourne la liste des groupes recherchés selon le critère.</returns>
    ''' <remarks></remarks>
    Function RechercherGroupes(ByVal NomGroupe As String) As IList(Of String)

End Interface

''' <summary>
''' Identifie les informations retournables sur les utilisateurs.
''' </summary>
<Flags()> _
Public Enum TsAadInfoUtilisateur

    ''' <summary>
    ''' Retourner le code de l'utilisateur.
    ''' </summary>
    TsAadIuCode = 1

    ''' <summary>
    ''' Retourner le nom complet de l'utilisateur.
    ''' </summary>
    TsAadIuNomComplet = 2

    ''' <summary>
    ''' Retourner le SID de l'utilisateur.
    ''' </summary>
    TsAadIuSID = 4

End Enum

''' <summary>
''' Identifie les informations retournables sur les utilisateurs.
''' </summary>
<Flags()> _
Public Enum TsAadInfoGroupe

    ''' <summary>
    ''' Retourner le code du groupe.
    ''' </summary>
    TsAadIuCode = 1

    ''' <summary>
    ''' Retourner la description du groupe.
    ''' </summary>
    TsAadIuDescription = 2

End Enum

''' <summary>
''' Identifie le domaine à accéder.
''' </summary>
<Flags()>
Public Enum TsAadNomDomaine

    ''' <summary>
    ''' Domaine Régie de Rente du Québec (RRQ_QC).
    ''' </summary>
    TsDomaineRRQ = 1

    ''' <summary>
    ''' Domaine CARRA (INRA).
    ''' </summary>
    TsDomaineCARRA = 2

    ''' <summary>
    ''' Domaine Retraite Québec (RQ).
    ''' </summary>
    TsDomaineRQ = 3

End Enum