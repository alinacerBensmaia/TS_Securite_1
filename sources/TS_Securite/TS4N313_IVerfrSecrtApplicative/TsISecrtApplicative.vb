
Public Interface TsISecrtApplicative

    ''' <summary>
    ''' Cette méthode vérifie si un utilisateur est membre directement ou indirectement d'un groupe
    ''' </summary>
    ''' <remarks></remarks>
    ''' <param name="NomGroupe">Nom du groupe à vérifier</param>
    ''' <param name="CodeUsager">Compte de l'utilisateur </param>
    ''' <returns>True si l'utilisateur est un membre direct ou indirect du groupe</returns>
    Function EstMembreGroupe(ByVal NomGroupe As String, _
                             ByVal CodeUsager As String) As Boolean


    ''' <summary>
    ''' Cette méthode vérifie si un utilisateur est membre directement ou indirectement d'un groupe
    ''' </summary>
    ''' <remarks></remarks>
    ''' <param name="CodeUsager">Compte de l'utilisateur </param>
    ''' <param name="NomGroupes">Noms des groupes à vérifier</param>
    ''' <returns>Un dictionnaire contenant le code du groupe et True si l'utilisateur est un membre direct ou indirect du groupe</returns>
    Function EstMembreGroupeV2(ByVal CodeUsager As String,
                               ByVal NomGroupes As IList(Of String)) As IDictionary(Of String, Boolean)

    ' ''' <summary>
    ' ''' Obtiens la liste des membres du groupe qui sont de type Utilisateur
    ' ''' </summary>
    ' ''' <param name="NomGroupe">Le nom du groupe</param>
    ' ''' <returns>La liste des membres du groupe qui sont de type Utilisateur</returns>
    ' ''' <remarks>Un cache est utilisé pour stocker les informations</remarks>
    ''Function ObtenirUtilisateurGroupe(ByVal NomGroupe As String) As List(Of String)
    'Function ObtenirUtilisateurGroupe(ByVal NomGroupe As String) As List(Of TsDtUtilisateur)


    ''' <summary>
    ''' Cette méthode obtient la liste de tous les utilisateurs membres d'au moins un des groupes spécifiés.
    ''' </summary>
    ''' <param name="LsNomGroupe">Liste des noms des groupes pour lesquels obtenir les utilisateurs.</param>
    ''' <param name="Recursif">Valeur indiquant s'il faut ou non obtenir les utilisateurs membres des groupes spécifiés de façon indirecte (via un autre groupe membre d'un des groupes spécifiés).</param>
    ''' <returns>Liste de tous les utilisateurs membres d'au moins un des groupes spécifiés.</returns>
    ''' <remarks>
    ''' Un utilisateur est considéré membre d'un groupe s'il en est directe d'un des groupe spécifiés ou via un autre groupe membre d'un des groupes spécifiés.
    '''</remarks>
    Function ObtenirUtilisateurGroupe(ByVal LsNomGroupe As IList(Of String), _
                                      ByVal Recursif As Boolean) _
                                        As IList(Of TsDtUtilisateur)


    ''' <summary>
    ''' Cette méthode obtient la liste de tous les groupes de sécurité applicative pour l'utilisateur spécifié.
    ''' </summary>
    ''' <param name="CodeUsager">Compte de l'utilisateur.</param>
    ''' <returns>Liste de tous les groupes de sécurité applicative pour l'utilisateur en paramètre.</returns>
    ''' <remarks>Seulement le premier niveau de groupe de distribution est obtenu.</remarks>
    Function ObtenirGroupeUtilisateur(ByVal CodeUsager As String) As IList(Of TsDtGroupe)

    ''' <summary>
    ''' Cette fonction permet d'obtenir les informations d'un groupe.
    ''' </summary>
    ''' <param name="NomGroupe">Le nom du groupe à obtenir</param>
    ''' <returns>Retourne le groupe</returns>
    ''' <remarks></remarks>
    Function ObtenirGroupe(ByVal NomGroupe As String) As TsDtGroupe

    ''' <summary>
    ''' Cette méthode retourne une liste de groupe pour lesquels le groupe fourni est un membre.
    ''' </summary>
    ''' <param name="NomGroupe"></param>
    ''' <param name="Recursif"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function ObtenirGroupeMembreDe(ByVal NomGroupe As String, _
                                   ByVal Recursif As Boolean) As IList(Of TsDtGroupe)

    ''' <summary>
    ''' Cette fonction permet d'obtenir une liste de groupes répondant aux critères du nom recherché.
    ''' </summary>
    ''' <param name="Filtre">Le nom du groupe à obtenir. Peut être un nom générique pour obtenir une liste</param>
    ''' <returns>Retourne la liste des groupes recherchés selon le critère.</returns>
    ''' <remarks></remarks>
    Function RechercherGroupes(ByVal Filtre As String) As IList(Of String)

End Interface



''' <summary>
''' Identifie les types d'application.
''' </summary>
<Flags()> _
Public Enum TsATypeApplication

    ''' <summary>
    ''' Retourner les profil du web.
    ''' </summary>
    TsAadIuWeb

    ''' <summary>
    ''' Retourner les profil du Navigateur CS.
    ''' </summary>
    TsAadIuNavigCS

    ''' <summary>
    ''' Retourner les profil du Navigateur de Support.
    ''' </summary>
    TsAadIuNagigSupport

End Enum


''' <summary>
''' Identifie les Ressources de securite.
''' </summary>
<Flags()> _
Public Enum TsRessourceSecurite


    ''' <summary>
    ''' AD-LDS
    ''' </summary>
    ADLDS

    ''' <summary>
    ''' WCF - Logique d'affaire.
    ''' </summary>
    WCFIIS

End Enum



