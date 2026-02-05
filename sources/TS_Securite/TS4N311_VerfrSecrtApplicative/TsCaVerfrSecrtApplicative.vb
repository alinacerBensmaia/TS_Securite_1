Imports Rrq.InfrastructureCommune.Parametres
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Security.Principal
Imports System.Text.RegularExpressions

Public Class TsCaVerfrSecrtApplicative

#Region "--- Variables ---"

    Private Const GABARIT_ROX As String = "ROX_{0}_{1}*"
    Private Const GABARIT_PERSONALISE As String = "{0}*"
    Private securiteApplicative As TsISecrtApplicative
    Private ressourceSecurite As Integer = ParseEnum(Of TsRessourceSecurite)(XuCuPolitiqueConfig.ConfigDomaine.ValeurSysteme("TS4", "TS4\TS4N311\RessourceSecurite"))

#End Region

#Region "--- Méthodes ---"

#Region "--- Publiques ---"

    Public Sub New()
        ' Les filtres sont imbriqués de la façon suivante:
        ' securiteApplicative -> mémorisation -> acces

        ' Dans tous les cas, on instancie d'abord l'accesseur
        Dim accesseur As TsISecrtApplicative = Nothing
        Select Case ressourceSecurite
            Case TsRessourceSecurite.ADLDS
                accesseur = CreerAccesseurADLDS()
            Case TsRessourceSecurite.WCFIIS
                If EstPoolTS() Or AppelPourCreationPoolWXCF() Then
                    '1 - Nous sommes déjàs dans le POOL TS, dans ce cas, on fait une lecture ADLDS. Cette condition a été ajouté pour SAW (Systèmes administrafifs Web)
                    '2 - Il s'agit d'un appel pour la création de pool. Comme les pool n'existe pas encore, il faut lire directement ADLDS. Aussi ajouté pour pour SAW (Systèmes administrafifs Web)
                    accesseur = CreerAccesseurADLDS()
                Else
                    accesseur = CreerAccesseurWCF()
                End If
            Case Else
                ' Garde-fou au cas où un autre accesseur soit ajouté ou que la propriété contienne du garbage 
                Throw New TsCuRessourceSecuriteInconnuException
        End Select

        securiteApplicative = accesseur
    End Sub


    ''' <summary>
    '''     Vérifie si un utilisateur est membre directement ou indirectement d'un groupe.
    ''' </summary>
    ''' <param name="NomGroupe">
    ''' 	Nom du groupe à vérifier. 
    ''' </param>
    ''' <param name="codeUtilisateur">
    ''' 	Compte de l'utilisateur. 
    ''' </param>
    ''' <returns>True si l'utilisateur est un membre direct ou indirect du groupe.</returns>
    ''' 
    <Obsolete("Cette méthode est désuette. Elle est remplacée par EstMembreGroupeV2(NomGroupes, codeUtilisateur).")>
    Public Function EstMembreGroupe(ByVal NomGroupe As String,
                                    ByVal codeUtilisateur As String) As Boolean

        Dim listeGroupe As IList(Of String) = New List(Of String)
        listeGroupe.Add(NomGroupe)

        Return EstMembreGroupeV2(codeUtilisateur, listeGroupe).Item(NomGroupe)

    End Function

    ''' <summary>
    '''     Vérifie si un utilisateur est membre directement ou indirectement d'un groupe.
    ''' </summary>
    ''' <param name="NomGroupe">
    ''' 	Nom du groupe à vérifier. 
    ''' </param>
    ''' <param name="UserToken">
    ''' 	Token de l'utilisateur. 
    ''' </param>
    ''' <remarks>Le compte de l'utilisateur est extrait du token et c'est lui qui est utiliser pour vérifier l'appartenance  au groupe spécifié.</remarks>
    ''' <returns>True si l'utilisateur est un membre direct ou indirect du groupe.</returns>
    <Obsolete("Cette méthode est désuette. Elle est remplacée par EstMembreGroupeV2(NomGroupes, UserToken).")>
    Public Function EstMembreGroupe(ByVal NomGroupe As String,
                                    ByVal UserToken As WindowsIdentity) As Boolean

        Dim listeGroupe As IList(Of String) = New List(Of String)
        listeGroupe.Add(NomGroupe)

        Return EstMembreGroupeV2(UserToken, listeGroupe).Item(NomGroupe)

    End Function

    ''' <summary>
    '''     Vérifie si un utilisateur est membre directement ou indirectement d'un groupe.
    ''' </summary>
    ''' <param name="codeUtilisateur">
    ''' 	Compte de l'utilisateur. 
    ''' </param>
    ''' <param name="NomGroupes">
    ''' 	Noms des groupes à vérifier. 
    ''' </param>
    ''' <returns>Un dictionnaire contenant le code du groupe et True si l'utilisateur est un membre direct ou indirect du groupe</returns>
    ''' 
    Public Function EstMembreGroupeV2(ByVal codeUtilisateur As String, ByVal NomGroupes As IList(Of String)) As IDictionary(Of String, Boolean)

        Return securiteApplicative.EstMembreGroupeV2(codeUtilisateur, NomGroupes)

    End Function

    ''' <summary>
    '''     Vérifie si un utilisateur est membre directement ou indirectement d'un groupe.
    ''' </summary>
    ''' <param name="UserToken">
    ''' 	Token de l'utilisateur. 
    ''' </param>
    ''' <param name="NomGroupes">
    ''' 	Noms des groupes à vérifier. 
    ''' </param>
    ''' <remarks>Le compte de l'utilisateur est extrait du token et c'est lui qui est utiliser pour vérifier l'appartenance  au groupe spécifié.</remarks>
    ''' <returns>Un dictionnaire contenant le code du groupe et True si l'utilisateur est un membre direct ou indirect du groupe</returns>
    ''' 
    Public Function EstMembreGroupeV2(ByVal UserToken As WindowsIdentity, ByVal NomGroupes As IList(Of String)) As IDictionary(Of String, Boolean)

        Dim codeUtilisateur As String = UserToken.Name.Substring(UserToken.Name.IndexOf("\"c) + 1)

        Return EstMembreGroupeV2(codeUtilisateur, NomGroupes)

    End Function
    ''' <summary>
    ''' Cette méthode obtient la liste de tous les utilisateurs membres d'au moins un des groupes spécifiés.
    ''' </summary>
    ''' <param name="LsNomGroupe">Liste des noms des groupes pour lesquels obtenir les utilisateurs.</param>
    ''' <param name="Recursif">Valeur indiquant s'il faut ou non obtenir les utilisateurs membres des groupes spécifiés de façon indirecte (via un autre groupe membre d'un des groupes spécifiés).</param>
    ''' <returns>Liste de tous les utilisateurs membres d'au moins un des groupes spécifiés.</returns>
    ''' <remarks>Un utilisateur est considéré membre d'un groupe s'il en est directe d'un des groupe spécifiés ou via un autre groupe membre d'un des groupes spécifiés.</remarks>
    Function ObtenirUtilisateurGroupe(ByVal LsNomGroupe As IList(Of String),
                                      ByVal Recursif As Boolean) As IList(Of TsDtUtilisateur)

        Return securiteApplicative.ObtenirUtilisateurGroupe(LsNomGroupe, Recursif)

    End Function

    ''' <summary>
    ''' Cette méthode obtient la liste de tous les groupes de sécurité applicative pour l'utilisateur spécifié.
    ''' </summary>
    ''' <param name="codeUtilisateur">Compte de l'utilisateur.</param>
    ''' <returns>Liste de tous les groupes de sécurité applicative pour l'utilisateur en paramètre.</returns>
    ''' <remarks>Seulement le premier niveau de groupe de distribution est obtenu.</remarks>
    Function ObtenirGroupeUtilisateur(ByVal codeUtilisateur As String) As IList(Of TsDtGroupe)

        Return securiteApplicative.ObtenirGroupeUtilisateur(codeUtilisateur)

    End Function

    ''' <summary>
    ''' Cette méthode obtient la liste de tous les groupes de sécurité applicative pour l'utilisateur spécifié.
    ''' </summary>
    ''' <param name="UserToken">Token de l'utilisateur.</param>
    ''' <returns>Liste de tous les groupes de sécurité applicative pour l'utilisateur en paramètre.</returns>
    ''' <remarks>Seulement le premier niveau de groupe de distribution est obtenu.</remarks>
    Function ObtenirGroupeUtilisateur(ByVal UserToken As WindowsIdentity) As IList(Of TsDtGroupe)

        Dim CodeUsager As String = UserToken.Name.Substring(UserToken.Name.IndexOf("\"c) + 1)

        Return securiteApplicative.ObtenirGroupeUtilisateur(CodeUsager)

    End Function


    ''' <summary>
    ''' Cette méthode obtient la liste de tous les groupes de sécurité applicative pour l'utilisateur spécifié sans faire de mémorisation.
    ''' Cette a été créé spécifiquement pour le navigateur de mission, afin d'acé 
    ''' </summary>
    ''' <param name="UserToken">Token de l'utilisateur.</param>
    ''' <returns>Liste de tous les groupes de sécurité applicative pour l'utilisateur en paramètre.</returns>
    ''' <remarks>Seulement le premier niveau de groupe de distribution est obtenu.</remarks>
    Function ObtenirGroupeUtilisateurSansMemo(ByVal UserToken As WindowsIdentity) As IList(Of TsDtGroupe)

        Dim CodeUsager As String = UserToken.Name.Substring(UserToken.Name.IndexOf("\"c) + 1)

        Return securiteApplicative.ObtenirGroupeUtilisateur(CodeUsager)

    End Function

    ''' <summary>
    ''' Cette fonction permet d'obtenir les informations d'un groupe.
    ''' </summary>
    ''' <param name="NomGroupe">Le nom du groupe à obtenir</param>
    ''' <returns>Retourne le groupe</returns>
    ''' <remarks></remarks>
    Function ObtenirGroupe(ByVal NomGroupe As String) As TsDtGroupe

        Return securiteApplicative.ObtenirGroupe(NomGroupe)

    End Function

    ''' <summary>
    ''' Cette fonction permet de vérifier si un groupe existe dans le dépôt de sécurité applicatif
    ''' </summary>
    ''' <param name="NomGroupe">Le nom du groupe à vérifier</param>
    ''' <returns>"Vrai" si le groupe existe et "faux" si le groupe n'existe pas</returns>
    ''' <remarks></remarks>
    Function GroupeExiste(ByVal NomGroupe As String) As Boolean
        Dim resultat As Boolean = False
        Try
            If Not securiteApplicative.ObtenirGroupe(NomGroupe).NmGrpSec = String.Empty Then
                resultat = True
            End If
        Catch ex As TsCuGroupeSecuriteInexistantException
        End Try

        Return resultat
    End Function

    ''' <summary>
    ''' Cette méthode obtient la liste de groupes de sécurité applicative dont le groupe passé en paramètre est membre.
    ''' </summary>
    ''' <param name="NomGroupe">Nom du groupe pour lesquel obtenir sa liste de membre de.</param>
    ''' <param name="Recursif">Valeur indiquant s'il faut ou non obtenir les groupes de façon indirecte.</param>
    ''' <returns>Liste de tous les groupes de sécurité applicative dont le groupe spécifié est membre.</returns>
    ''' <remarks>.</remarks>
    Function ObtenirGroupeMembreDe(ByVal NomGroupe As String,
                                   ByVal Recursif As Boolean) As IList(Of TsDtGroupe)

        Return securiteApplicative.ObtenirGroupeMembreDe(NomGroupe, Recursif)

    End Function

    ''' <summary>
    ''' Cette fonction permet d'obtenir une liste de groupes répondant aux critères du nom recherché.
    ''' </summary>
    ''' <param name="Filtre">Le filtre de noms des groupe à obtenir.</param>
    ''' <returns>Retourne la liste des groupes recherchés selon le critère.</returns>
    ''' <remarks></remarks>
    Function RechercherGroupes(ByVal Filtre As String) As IList(Of String)

        Return securiteApplicative.RechercherGroupes(Filtre)

    End Function


    ''' <summary>
    ''' Cette fonction permet d'obtenir une liste de profil de sécurité preventive pour un environnement et un modèle donnée.
    ''' </summary>
    ''' <param name="codeUtilisateur">Compte de l'utilisateur.</param>
    ''' <param name="modeleSecurite">Modèle de sécurité.</param>
    ''' <param name="environnement">Environnement d'éxécution.</param>
    ''' <returns>Retourne une liste des profil à 4 lettre auxquels l'utilisateur à accès.</returns>
    ''' <remarks></remarks>
    Function ObtenirProfilSecrtPrevt(ByVal codeUtilisateur As String, ByVal modeleSecurite As TsVsaModeleSecurite, ByVal environnement As TsVsaEnvironnement) As IList(Of String)
        Dim rox_env_model As String = ObtenirPrefixProfl(environnement, modeleSecurite)

        Dim listeGrpUtl As IDictionary(Of String, Boolean) = securiteApplicative.EstMembreGroupeV2(codeUtilisateur, securiteApplicative.RechercherGroupes(rox_env_model))
        Dim x = From groupe In listeGrpUtl
                Where groupe.Value = True
                Select groupe.Key.Substring(groupe.Key.IndexOf(NomModele(modeleSecurite)) + NomModele(modeleSecurite).Length + 1)

        Return x.ToList
    End Function

    ''' <summary>
    ''' Cette fonction permet d'obtenir une liste de profil de sécurité preventive pour un environnement et un modèle donnée.
    ''' </summary>
    ''' <param name="codeUtilisateur">Compte de l'utilisateur.</param>
    ''' <param name="modeleSecurite">Modèle de sécurité.</param>
    ''' <param name="lettreEnvironnement">Lettre correspondant à l'environnement d'éxécution.</param>
    ''' <returns>Retourne une liste des profil à 4 lettre auxquels l'utilisateur à accès.</returns>
    ''' <remarks></remarks>
    Function ObtenirProfilSecrtPrevt(ByVal codeUtilisateur As String, ByVal modeleSecurite As TsVsaModeleSecurite, ByVal lettreEnvironnement As String) As IList(Of String)
        Dim rox_env_model As String = ObtenirPrefixProfl(lettreEnvironnement, modeleSecurite)

        Dim listeGrpUtl As IDictionary(Of String, Boolean) = securiteApplicative.EstMembreGroupeV2(codeUtilisateur, securiteApplicative.RechercherGroupes(rox_env_model))
        Dim x = From groupe In listeGrpUtl
                Where groupe.Value = True
                Select groupe.Key.Substring(groupe.Key.IndexOf(NomModele(modeleSecurite)) + NomModele(modeleSecurite).Length + 1)

        Return x.ToList
    End Function


    ''' <summary>
    ''' Cette fonction permet d'obtenir une liste de profil de sécurité débutant par un prefix donnée.
    ''' </summary>
    ''' <param name="codeUtilisateur">Compte de l'utilisateur.</param>
    ''' <param name="prefixGroupeSecurite">Prefix du nom des groupes de sécurité</param>
    ''' <returns>Retourne une liste des profil à auxquels l'utilisateur à accès.</returns>
    ''' <remarks></remarks>
    Function ObtenirProfilSecrtPrevt(ByVal codeUtilisateur As String, ByVal prefixGroupeSecurite As String) As IList(Of String)
        Dim prefix As String = String.Format(GABARIT_PERSONALISE, prefixGroupeSecurite)

        Dim listeGrpUtl As IDictionary(Of String, Boolean) = securiteApplicative.EstMembreGroupeV2(codeUtilisateur, securiteApplicative.RechercherGroupes(prefix))
        Dim x = From groupe In listeGrpUtl
                Where groupe.Value = True
                Select groupe.Key

        Return x.ToList
    End Function

#End Region


#Region "--- Privées ---"

    <MethodImpl(MethodImplOptions.NoInlining)>
    Private Function CreerAccesseurADLDS() As TsISecrtApplicative
        Return New TsCuAccesADLDS
    End Function

    <MethodImpl(MethodImplOptions.NoInlining)>
    Private Function CreerAccesseurWCF() As TsISecrtApplicative
        Return New TsCuAccesWCF()
    End Function


    ' Idéalement, on déclarerait T As [Enum], mais "'Enum' ne peut pas être utilisé en tant que contrainte de type."
    Private Function ParseEnum(Of T)(ByVal valeur As String) As T
        If GetType(T).BaseType IsNot GetType([Enum]) Then
            Throw New FormatException("T doit être une énumération")
        End If

        ' On est gentil et on ignore la casse dans le parsing
        Return DirectCast([Enum].Parse(GetType(T), valeur, True), T)
    End Function

    Private Function ObtenirPrefixProfl(ByVal valeurEnv As TsVsaEnvironnement, ByVal valeurModele As TsVsaModeleSecurite) As String
        Return String.Format(GABARIT_ROX, LettreEnvironnement(valeurEnv), NomModele(valeurModele))
    End Function

    Private Function ObtenirPrefixProfl(ByVal lettreEnvironnement As String, ByVal valeurModele As TsVsaModeleSecurite) As String
        Return String.Format(GABARIT_ROX, lettreEnvironnement, NomModele(valeurModele))
    End Function

    Private Function LettreEnvironnement(ByVal valeurEnv As TsVsaEnvironnement) As String

        Select Case valeurEnv
            Case TsVsaEnvironnement.tsVsaEUnitaire
                Return "U"
            Case TsVsaEnvironnement.tsVsaEIntegration
                Return "I"
            Case TsVsaEnvironnement.tsVsaEAcceptation
                Return "A"
            Case TsVsaEnvironnement.tsVsaEFormationAcception
                Return "B"
            Case TsVsaEnvironnement.tsVsaEFormationProduction
                Return "Q"
            Case TsVsaEnvironnement.tsVsaESimulation
                Return "S"
            Case TsVsaEnvironnement.tsVsaEProduction
                Return "P"
            Case Else
                Return String.Empty
        End Select

    End Function

    Private Function NomModele(ByVal valeurModele As TsVsaModeleSecurite) As String

        Return [Enum].GetName(GetType(TsVsaModeleSecurite), valeurModele).Substring(7)

    End Function


    Private Function EstPoolTS() As Boolean
        'Le nom de l'usager est composé comme tel:  Domaine\CodeUtilisateur 
        Dim strDomCodeUtil As String() = WindowsIdentity.GetCurrent.Name.Split("\".ToCharArray())
        Dim regexComptePool As Regex = New Regex("ZAP[IABSP]WTS")
        If regexComptePool.IsMatch(strDomCodeUtil(1)) Then
            Return True
        End If

        Return False
    End Function

    Private Function AppelPourCreationPoolWXCF() As Boolean
        Dim assemblyCourant As Assembly = Assembly.GetExecutingAssembly()
        Dim pileAppel As StackTrace = New StackTrace()
        Dim methodes() As StackFrame = pileAppel.GetFrames
        For Each methode As StackFrame In methodes
            If methode.GetMethod.DeclaringType IsNot Nothing AndAlso methode.GetMethod.DeclaringType.Assembly.GetName.Name.ToUpper.Trim.Equals("XU7N043_GERERSITESIISWCF") Then
                Return True
            End If
        Next
        Return False
    End Function

#End Region
#End Region

#Region "--- Propriétés ---"

#Region "--- Privées ---"

#End Region

#End Region


End Class


''' <summary>
''' Identifie les types d'application.
''' </summary>
<Flags()>
Public Enum TsVsaModeleSecurite

    ''' <summary>
    ''' Indique l'utilisation du modèle Web.
    ''' </summary>
    tsVsaMsNavigWeb = 0

    ''' <summary>
    ''' Indique l'utilisation du modèle Navigateur CS.
    ''' </summary>
    tsVsaMsNavigCS = 1

    ''' <summary>
    ''' Indique l'utilisation du modèle Navigateur de Support.
    ''' </summary>
    tsVsaMsNavigSU = 2

    ''' <summary>
    ''' Indique l'utilisation du modèle Navigateur 3270.
    ''' </summary>
    tsVsaMsNavig3270 = 3

    ''' <summary>
    ''' Indique l'utilisation du modèle Navigateur Service RQ.
    ''' </summary>
    tsVsaMsNavigServicesRQ = 4


End Enum

Public Enum TsVsaEnvironnement

    ''' <summary>
    ''' Environnement Unitaire.
    ''' </summary>
    tsVsaEUnitaire = 0

    ''' <summary>
    ''' Environnement Intégration.
    ''' </summary>
    tsVsaEIntegration = 1

    ''' <summary>
    ''' Environnement Acceptation.
    ''' </summary>
    tsVsaEAcceptation = 2

    ''' <summary>
    ''' Environnement Formation-Acception.
    ''' </summary>
    tsVsaEFormationAcception = 3

    ''' <summary>
    ''' Environnement Formation-Production.
    ''' </summary>
    tsVsaEFormationProduction = 4

    ''' <summary>
    ''' Environnement Formation-Production.
    ''' </summary>
    tsVsaESimulation = 5

    ''' <summary>
    ''' Environnement Formation-Production.
    ''' </summary>
    tsVsaEProduction = 6

End Enum