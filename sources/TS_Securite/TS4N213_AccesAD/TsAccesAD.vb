Imports System.Collections.Generic
Imports System.Security.Principal
Imports Rrq.InfrastructureCommune.Parametres


''' <summary>
''' Cette classe utilitaire récupère et stocke des informations sur le contenu de l'active directory.
''' </summary>
<Obsolete("Utilisez la classe TsCuAccesAD.", True)>
Public Class TsAccesAD
    Implements TsIAccesAD
    Private _cuAccesAD As TsIAccesAD = New TsCuAccesAD

    Public Sub New()
        ' Si la config existe et n'est pas vide alors on lance une exception
        Dim migreVersADLDS As Boolean = Not String.IsNullOrEmpty(XuCuPolitiqueConfig.ConfigDomaine.ObtenirValeurSystemeOptionnelle("TS4", "TS4\TS4N213\SecrtApplcMigreVersADLDS"))

        If migreVersADLDS Then
            Throw New TsCuSecuriteApplicativeMigreVersADLDS
        End If
    End Sub


    ''' <summary>
    ''' Cette méthode vérifie si un utilisateur est membre directement ou indirectement d'un groupe
    ''' </summary>
    ''' <remarks>Dans le UserToken, nous avons la liste de ses groupes de sécurité (direct et indirect). 
    ''' On prend en charge le cas que le NomGroupe est un groupe de distribution et donc non présent dans le UserToken.
    ''' Dans ce cas, on gére qu'un seul niveau de hierarchie. L'utilisateur lui même ou un de ses groupes de sécurité (issu du UserToken) doit être membre du groupe de distribution à vérifier.</remarks>
    ''' <param name="NomGroupe">Nom du groupe à vérifier</param>
    ''' <returns>True si l'utilisateur est un membre direct ou indirect du groupe</returns>
    Public Function EstMembreGroupe(ByVal NomGroupe As String, ByVal UserToken As WindowsIdentity) As Boolean Implements TsIAccesAD.EstMembreGroupe
        Return _cuAccesAD.EstMembreGroupe(NomGroupe, UserToken)
    End Function

    ''' <summary>
    ''' Cette méthode obtient la liste de tous les utilisateurs membres d'au moins un des groupes spécifiés.
    ''' </summary>
    ''' <param name="LsNomGroupe">Liste des noms des groupes pour lesquels obtenir les utilisateurs.</param>
    ''' <param name="Recursif">Valeur indiquant s'il faut ou non obtenir les utilisateurs membres des groupes spécifiés de façon indirecte (via un autre groupe membre d'un des groupes spécifiés).</param>
    ''' <param name="InfoRetour">Valeur indiquant les informations à retourner pour chaque utilisateur. Plusieurs valeurs peuvent être combinées à l'aide d'un "Or"</param>
    ''' <returns>Liste de tous les utilisateurs membres d'au moins un des groupes spécifiés.</returns>
    ''' <remarks>Un utilisateur est considéré membre d'un groupe s'il en est "membre" au niveau de l'active directory ou si ce groupe est son groupe principal.</remarks>
    Public Function ObtenirUtilisateurGroupe(ByVal LsNomGroupe As IList(Of String), ByVal Recursif As Boolean, ByVal InfoRetour As TsAadInfoUtilisateur) As IList(Of TsDtUtilisateur) Implements TsIAccesAD.ObtenirUtilisateurGroupe
        Return _cuAccesAD.ObtenirUtilisateurGroupe(LsNomGroupe, Recursif, InfoRetour)
    End Function

    ''' <summary>
    ''' Cette méthode obtient la liste de tous les groupes pour l'utilisateur spécifié.
    ''' </summary>
    ''' <param name="UserToken">Token de l'utilisateur.</param>
    ''' <returns>Liste de tous les groupes pour l'utilisateur en paramètre.</returns>
    ''' <remarks>Seulement le premier niveau de groupe de distribution est obtenu.</remarks>
    Public Function ObtenirGroupeUtilisateur(ByVal UserToken As WindowsIdentity) As IList(Of TsDtGroupe) Implements TsIAccesAD.ObtenirGroupeUtilisateur
        Return _cuAccesAD.ObtenirGroupeUtilisateur(UserToken)
    End Function

    ''' <summary>
    ''' Cette méthode obtient la liste de tous les groupes pour l'utilisateur spécifié.
    ''' </summary>
    ''' <param name="Usager">Compte de l'utilisateur.</param>
    ''' <returns>Liste de tous les groupes pour l'utilisateur en paramètre.</returns>
    ''' <remarks>Seulement le premier niveau de groupe de distribution est obtenu.</remarks>
    Public Function ObtenirGroupeUtilisateur(ByVal Usager As String) As IList(Of TsDtGroupe) Implements TsIAccesAD.ObtenirGroupeUtilisateur
        Return _cuAccesAD.ObtenirGroupeUtilisateur(Usager)
    End Function

    ''' <summary>
    ''' Cette fonction permet d'obtenir les informations d'un groupe.
    ''' </summary>
    ''' <param name="NomGroupe">Le nom du groupe à obtenir</param>
    ''' <param name="InfoRetour">Valeur indiquant les informations à retourner pour le groupe utilisateur. Plusieurs valeurs peuvent être combinées à l'aide d'un "Or"</param>
    ''' <returns>Retourne le groupe</returns>
    ''' <remarks>Seulement le premier niveau de groupe de distribution est recherché.</remarks>
    Public Function ObtenirGroupe(ByVal NomGroupe As String, ByVal InfoRetour As TsAadInfoGroupe) As TsDtGroupe Implements TsIAccesAD.ObtenirGroupe
        Return _cuAccesAD.ObtenirGroupe(NomGroupe, InfoRetour)
    End Function

    ''' <summary>
    ''' Cette méthode retourne une liste de groupe pour lesquels le groupe fourni est membre de.
    ''' </summary>
    ''' <remarks></remarks>
    ''' <param name="NomGroupe">Nom du groupe à vérifier</param>
    ''' <returns>Liste de groupes pour lesquels le groupe est membre de</returns>
    Function ObtenirGroupeMembreDe(ByVal NomGroupe As String, ByVal Recursif As Boolean) As IList(Of TsDtGroupe) Implements TsIAccesAD.ObtenirGroupeMembreDe
        Return _cuAccesAD.ObtenirGroupeMembreDe(NomGroupe, Recursif)
    End Function

    ''' <summary>
    ''' Cette fonction permet d'obtenir une liste de groupes répondant aux critères du nom recherché.
    ''' </summary>
    ''' <param name="NomGroupe">Le nom du groupe à obtenir. Peut être un nom générique pour obtenir une liste</param>
    ''' <returns>Retourne la liste des groupes recherchés selon le critère.</returns>
    ''' <remarks></remarks>
    Public Function RechercherGroupes(ByVal NomGroupe As String) As IList(Of String) Implements TsIAccesAD.RechercherGroupes
        Return _cuAccesAD.RechercherGroupes(NomGroupe)
    End Function

End Class
