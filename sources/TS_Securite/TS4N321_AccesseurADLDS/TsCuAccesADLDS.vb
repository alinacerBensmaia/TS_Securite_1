Imports System.DirectoryServices.AccountManagement
Imports Rrq.InfrastructureCommune.Parametres
Imports System.DirectoryServices
Imports System.Text
Imports System.Runtime.CompilerServices
Imports Rrq.InfrastructureCommune.UtilitairesCommuns

Public Class TsCuAccesADLDS
    'teste commit
    Implements TsISecrtApplicative
    Private Const UTILISATEUR_GROUPE As String = "UtlGrp_{0}_{1}"
    Private Const GROUPE_MEMBREDE As String = "GrpMembreDe_{0}_{1}"

    Private ReadOnly _cacheSecuriteApplicative As TsCaHelperMemorisation
    Private ReadOnly _gererMemoirePerimee As Boolean = False
    Private ReadOnly _serveurADLDS As String
    Private _derniereCleMemorise As String

#Region "Constructeur"

    Public Sub New()
        _cacheSecuriteApplicative = New TsCaHelperMemorisation
        _serveurADLDS = XuCuPolitiqueConfig.ConfigDomaine.ValeurSysteme("TS4", "TS4\TS4N321\ServeurADLDS")

        Dim valeur As String = XuCuPolitiqueConfig.ConfigDomaine.ObtenirValeurSystemeOptionnelle("TS4", "TS4\TS4N321\GererMemoirePerimee")
        If valeur IsNot Nothing Then _gererMemoirePerimee = (valeur.ToUpper = "OUI")
    End Sub
    Public Sub New(serveurADLDS As String, gererMemoirePerimee As Boolean)
        _cacheSecuriteApplicative = New TsCaHelperMemorisation
        _gererMemoirePerimee = gererMemoirePerimee
        _serveurADLDS = serveurADLDS
    End Sub

#End Region

#Region "Méthodes publiques"

    Public Function EstMembreGroupe(NomGroupe As String, CodeUsager As String) As Boolean _
        Implements TsISecrtApplicative.EstMembreGroupe

        DerniereCleMemorise = String.Empty
        Dim lsNomGroupe As New List(Of String)({NomGroupe})
        Dim utilisateurGroupe As IList(Of TsDtUtilisateur) = ObtenirUtilisateurGroupe(lsNomGroupe, True)
        Dim estMembre As Boolean = utilisateurGroupe.ToList().Exists(Function(x) x.CoUtl.Est(CodeUsager))

        If GererMemoirePerimee Then
            If Not estMembre And Not EstDernierCleMemorise(lsNomGroupe, True) Then
                EffacerMemUtilisateurGroupe(NomGroupe, True)
                utilisateurGroupe = ObtenirUtilisateurGroupe(lsNomGroupe, True)
                estMembre = utilisateurGroupe.ToList().Exists(Function(x) x.CoUtl.Est(CodeUsager))
            End If
        End If

        Return estMembre
    End Function

    Public Function EstMembreGroupeV2(CodeUsager As String, NomGroupes As IList(Of String)) As IDictionary(Of String, Boolean) _
        Implements TsISecrtApplicative.EstMembreGroupeV2

        Dim membreGroupes As IDictionary(Of String, Boolean) = Nothing

        Using accesSecurite As New TsCuAccesServiceAnnuaire()
            membreGroupes = accesSecurite.EstMembreDe(CodeUsager, NomGroupes)
        End Using

        Return membreGroupes
    End Function

    Public Function ObtenirGroupe(NomGroupe As String) As TsDtGroupe _
        Implements TsISecrtApplicative.ObtenirGroupe

        Dim groupe As New TsDtGroupe
        Using contexteGroupe As New PrincipalContext(ContextType.ApplicationDirectory, ServeurADLDS, ReprtRacine)
            Using grpPrincipal As GroupPrincipal = ObtenirGroupePrincipal(NomGroupe, contexteGroupe)
                If grpPrincipal IsNot Nothing Then
                    groupe.NmGrpSec = grpPrincipal.Name
                    groupe.DsGrpSec = grpPrincipal.Description
                End If
            End Using
        End Using

        Return groupe
    End Function

    Public Function ObtenirGroupeMembreDe(NomGroupe As String, Recursif As Boolean) As IList(Of TsDtGroupe) _
        Implements TsISecrtApplicative.ObtenirGroupeMembreDe

        Dim cleMemoire As String = creerCleGroupeMembreDe(NomGroupe, Recursif)
        Dim ObjetMemoire As Object = _cacheSecuriteApplicative.ObtenirObjetMemoire(cleMemoire)

        If ObjetMemoire IsNot Nothing Then
            Return DirectCast(ObjetMemoire, List(Of TsDtGroupe))
        End If

        Dim LsGroupes As New List(Of TsDtGroupe)
        Using deRoot As New DirectoryEntry
            deRoot.AuthenticationType = AuthenticationTypes.Secure
            deRoot.Path = String.Format("LDAP://{0}/{1}", ServeurADLDS, ReprtRacine)

            'Transformer le nom du groupe en DistinguishedName avant de le passer à la méthode privée.
            Dim lstDN As New List(Of String)
            Dim ResultatRecherche As SearchResult

            Using dsGroupe As New DirectorySearcher(deRoot)
                dsGroupe.Filter = String.Format("(&(objectCategory=group)(Name={0}))", NomGroupe)
                dsGroupe.PropertiesToLoad.Add("distinguishedName")

                ResultatRecherche = dsGroupe.FindOne()
            End Using

            'Si le groupe fourni est trouvé, 
            If Not ResultatRecherche Is Nothing Then
                lstDN.Add(ResultatRecherche.distinguishedName)
                ObtenirGroupeMembreDe(deRoot, lstDN, Recursif, LsGroupes)
            Else
                Throw New TsCuGroupeSecuriteInexistantException(NomGroupe)
            End If
        End Using

        _cacheSecuriteApplicative.Memoriser(cleMemoire, LsGroupes)
        Return LsGroupes
    End Function

    Public Function ObtenirGroupeUtilisateur(CodeUsager As String) As IList(Of TsDtGroupe) _
        Implements TsISecrtApplicative.ObtenirGroupeUtilisateur

        Dim LsGroupes As New List(Of TsDtGroupe)
        Using deRoot As New DirectoryEntry
            deRoot.AuthenticationType = AuthenticationTypes.Secure
            deRoot.Path = String.Format("LDAP://{0}/{1}", ServeurADLDS, ReprtUtilisateurs)

            ' *** Obtenir les groupes directs ***
            Using DsRecherche As New DirectorySearcher(deRoot)
                'On va chercher ici un objet de type user et dont le nom d'ouverture de session = <username>.
                DsRecherche.Filter = String.Format("(&(objectClass=User) (userPrincipalName={0}))", CodeUsager)

                DsRecherche.CacheResults = False
                DsRecherche.PageSize = 1000

                'Ne récupère que la propritée MemberOf.
                DsRecherche.PropertiesToLoad.Add("MemberOf")
                'Et aussi primaryGroupID
                DsRecherche.PropertiesToLoad.Add("primaryGroupID")

                'Recherche et retourne la première entrée trouvé.
                Dim result As SearchResult = DsRecherche.FindOne
                If result Is Nothing Then
                    Throw New TsCuUtilisateurInexistantException(CodeUsager)
                End If

                deRoot.Path = String.Format("LDAP://{0}/{1}", ServeurADLDS, ReprtRacine)
                'Parcours le contenu de la propritée MemberOf 
                For Each Valeur As Object In result.Properties("MemberOf")
                    Dim Parties() As String = Valeur.ToString().Split(","c)
                    Dim nomgroupe As String = Parties(0).Substring(3)

                    If Not LsGroupes.Exists(Function(x) x.NmGrpSec.Est(nomgroupe)) Then
                        Dim nouveauGrpDirect As New TsDtGroupe
                        nouveauGrpDirect.NmGrpSec = nomgroupe
                        LsGroupes.Add(nouveauGrpDirect)
                        Dim lstDN As New List(Of String)
                        lstDN.Add(Valeur.ToString)

                        Dim cleMemoire As String = creerCleGroupeMembreDe(nomgroupe, True)
                        Dim ObjetMemoire As Object = _cacheSecuriteApplicative.ObtenirObjetMemoire(cleMemoire)

                        Dim nouveauxgroupestmp As New List(Of TsDtGroupe)
                        If ObjetMemoire IsNot Nothing Then
                            nouveauxgroupestmp = DirectCast(ObjetMemoire, List(Of TsDtGroupe))
                        Else
                            ObtenirGroupeMembreDe(deRoot, lstDN, True, nouveauxgroupestmp)
                            _cacheSecuriteApplicative.Memoriser(cleMemoire, nouveauxgroupestmp)
                        End If

                        ' Ajout des groupes de sécurité imbriquées dans les listes membres de
                        For Each grp As TsDtGroupe In nouveauxgroupestmp
                            Dim nmSousGroupe As String = grp.NmGrpSec
                            If Not LsGroupes.Exists(Function(x) x.NmGrpSec.Est(nmSousGroupe)) Then
                                LsGroupes.Add(grp)
                            End If
                        Next
                    End If
                Next
            End Using
        End Using

        Return LsGroupes
    End Function

    Public Function RechercherGroupes(NomGroupe As String) As IList(Of String) _
        Implements TsISecrtApplicative.RechercherGroupes

        Dim lsGroupe As New List(Of String)
        Using contexteGroupe As New PrincipalContext(ContextType.ApplicationDirectory, ServeurADLDS, ReprtRacine)
            Using filtreGroupe As New GroupPrincipal(contexteGroupe)
                filtreGroupe.Name = NomGroupe
                Using recherche As New PrincipalSearcher(filtreGroupe)
                    Using resulat As PrincipalSearchResult(Of Principal) = recherche.FindAll()

                        For Each groupe As Principal In resulat
                            Using groupe
                                lsGroupe.Add(groupe.Name)
                            End Using
                        Next

                    End Using
                End Using
            End Using
        End Using

        Return lsGroupe
    End Function

    Public Function ObtenirUtilisateurGroupe(ByVal LsNomGroupe As IList(Of String), ByVal Recursif As Boolean) As IList(Of TsDtUtilisateur) _
        Implements TsISecrtApplicative.ObtenirUtilisateurGroupe

        Dim cleMemoire As String = creerCleUtilisateurGroupe(LsNomGroupe, Recursif)
        Dim ObjetMemoire As Object = _cacheSecuriteApplicative.ObtenirObjetMemoire(cleMemoire)

        If ObjetMemoire IsNot Nothing Then
            Return DirectCast(ObjetMemoire, List(Of TsDtUtilisateur))
        End If

        Dim LsUtils As New List(Of TsDtUtilisateur)()
        Dim LsNomGroupeFait As New List(Of String)()
        Dim LsChamp As New List(Of String)()

        If LsNomGroupe.Count > 0 Then
            Using deRoot As New DirectoryEntry
                deRoot.AuthenticationType = AuthenticationTypes.Secure
                deRoot.Path = String.Format("LDAP://{0}/{1}", ServeurADLDS, ReprtRacine)

                ObtenirUtilisateurGroupe(deRoot,
                                         LsNomGroupe,
                                         LsNomGroupeFait,
                                         Recursif,
                                         LsChamp,
                                         LsUtils)
            End Using
        End If

        _cacheSecuriteApplicative.Memoriser(cleMemoire, LsUtils)
        DerniereCleMemorise = cleMemoire

        Return LsUtils
    End Function

#End Region

#Region "Méthodes privées"

    Private Sub EffacerMemUtilisateurGroupe(ByVal NomGroupe As String, ByVal Recursif As Boolean)
        Dim cleMemoire As String = creerCleUtilisateurGroupe(New List(Of String)({NomGroupe}), Recursif)
        _cacheSecuriteApplicative.Dememoriser(cleMemoire)
    End Sub


    Private Function EstDernierCleMemorise(ByVal LsNomGroupe As IList(Of String), ByVal Recursif As Boolean) As Boolean
        Dim cleMemoire As String = creerCleUtilisateurGroupe(LsNomGroupe, Recursif)
        Return cleMemoire = DerniereCleMemorise
    End Function

    Private Sub ObtenirUtilisateurGroupe(ByVal deRoot As DirectoryEntry,
                                 ByVal LsNomGroupe As IList(Of String),
                                 ByVal LsNomGroupeFait As List(Of String),
                                 ByVal Recursif As Boolean,
                                 ByVal LsChamp As List(Of String),
                                 ByVal LsUtils As List(Of TsDtUtilisateur))

        ' Préparer une liste de groupes pour lesquels il faut obtenir les utilisateurs
        ' (ceux pour lesquels l'obtention n'a pas encore été faite)
        Dim sbCritere As New StringBuilder
        For Each NomGroupe As String In LsNomGroupe
            If LsNomGroupeFait.IndexOf(NomGroupe) = -1 Then
                sbCritere.AppendFormat("(Name={0})", NomGroupe)
                LsNomGroupeFait.Add(NomGroupe)
            End If
        Next

        If sbCritere.Length > 0 Then
            ' Obtenir les distinguishedName et primaryGroupToken des groupes de la liste
            ' (nécessaire pour l'obtention des membres des groupes)
            Using dsGroupe As New DirectorySearcher(deRoot)
                dsGroupe.CacheResults = False
                dsGroupe.PageSize = 1000

                dsGroupe.Filter = String.Format("(&(objectCategory=group)(|{0}))", sbCritere.ToString())
                dsGroupe.PropertiesToLoad.Add("distinguishedName")
                dsGroupe.PropertiesToLoad.Add("primaryGroupToken")

                sbCritere = New StringBuilder
                Using collectionResultats As SearchResultCollection = dsGroupe.FindAll()
                    For Each Resultat As SearchResult In collectionResultats
                        sbCritere.AppendFormat("(memberOf={0})(primaryGroupID={1})",
                                               Resultat.distinguishedName,
                                               Resultat.primaryGroupToken)
                    Next
                End Using

            End Using

            If sbCritere.Length > 0 Then
                ' Obtenir les membres des groupes de la liste
                Using dsMembre As New DirectorySearcher(deRoot)
                    dsMembre.CacheResults = False
                    dsMembre.PageSize = 1000

                    dsMembre.Filter = String.Format("(&(|(objectCategory=person)(objectCategory=group))(|{0}))", sbCritere.ToString())
                    dsMembre.PropertiesToLoad.Add("userPrincipalName")
                    dsMembre.PropertiesToLoad.Add("Name")
                    dsMembre.PropertiesToLoad.Add("givenName")
                    dsMembre.PropertiesToLoad.Add("sn")
                    dsMembre.PropertiesToLoad.Add("objectCategory")
                    dsMembre.PropertiesToLoad.Add("distinguishedName")

                    Dim LsSousGroupe As New List(Of String)
                    Using collectionResultats As SearchResultCollection = dsMembre.FindAll()
                        For Each Resultat As SearchResult In collectionResultats
                            If Resultat.Properties("objectCategory")(0).ToString().StartsWith("CN=Person") Then
                                Dim CodeUtils As String = String.Empty

                                'Vérifie si la propriété userPincipalName est vide. Si oui, lancer un erreur car tous les utilisateurs doivent avoir
                                'le compte windows dans la propriété userPrincipalName dans ADLDS
                                If Resultat.Properties.Item("userPrincipalName").Count <= 0 Then
                                    JournaliserErreur(New TsCuUtilisateurSansUserPrincipalName(Resultat.distinguishedName))
                                Else
                                    CodeUtils = Resultat.Properties.Item("userPrincipalName")(0).ToString()

                                    If Not LsUtils.Exists(Function(x) x.CoUtl.Est(CodeUtils)) Then
                                        Dim nouveauUtl As New TsDtUtilisateur
                                        nouveauUtl.CoUtl = CodeUtils
                                        nouveauUtl.NmComUtl = CodeUtils 'si le compte na pas de nom et prénom, le défaut est le compte utilisateur

                                        If Not String.IsNullOrWhiteSpace(Resultat.givenName) AndAlso Not String.IsNullOrWhiteSpace(Resultat.sn) Then
                                            nouveauUtl.NmComUtl = String.Format("{0} {1}", Resultat.sn, Resultat.givenName)
                                        End If
                                        LsUtils.Add(nouveauUtl)
                                    End If

                                End If

                            Else
                                LsSousGroupe.Add(Resultat.Properties.Item("Name")(0).ToString())
                            End If
                        Next
                    End Using


                    ' Obtenir (en récursif) les membres des sous-groupes
                    If Recursif AndAlso LsSousGroupe.Count > 0 Then
                        ObtenirUtilisateurGroupe(deRoot, LsSousGroupe, LsNomGroupeFait, Recursif, LsChamp, LsUtils)
                    End If
                End Using
            End If
        End If
    End Sub

    Private Sub ObtenirGroupeMembreDe(ByRef deRoot As DirectoryEntry, ByRef LsDN As List(Of String), ByVal Recursif As Boolean, ByRef DtGroupes As List(Of TsDtGroupe))
        Dim sbCritere As New StringBuilder
        Dim LsSousGroupe As New List(Of String)

        Using dsGroupe As New DirectorySearcher(deRoot)
            dsGroupe.CacheResults = False
            dsGroupe.PageSize = 1000
            dsGroupe.SearchScope = SearchScope.Subtree

            'Ajouter chaque groupe à rechercher au filtre
            For Each DN As String In LsDN
                sbCritere.AppendFormat("(member={0})", DN)
            Next

            'Préparer la requête pour obtenir tous les groupes qui ont comme membres 
            'un des groupes fournit plus haut
            dsGroupe.Filter = String.Format("(&(objectCategory=Group)(|{0}))", sbCritere.ToString())
            dsGroupe.PropertiesToLoad.Add("distinguishedName")
            dsGroupe.PropertiesToLoad.Add("Name")

            Using collectionResultats As SearchResultCollection = dsGroupe.FindAll()

                'Ajouter un objet TsDtGroupe à la liste de retour DtGroupes
                For Each Resultat As SearchResult In collectionResultats
                    Dim ItemGroupe As New TsDtGroupe
                    ItemGroupe.NmGrpSec = Resultat.Properties("Name")(0).ToString

                    ' On ajoute le groupe que s'il n'est pas déjà dans notre liste de groupes
                    ' Prévient ainsi les boucles infinies lorsque 2 groupes se réfèrent mutuellement
                    If Not DtGroupes.Exists(Function(x) x.NmGrpSec.Est(ItemGroupe.NmGrpSec)) Then
                        'Conserver les DN pour refaire une requête dans le cas où on est récursif
                        LsSousGroupe.Add(Resultat.distinguishedName)

                        DtGroupes.Add(ItemGroupe)
                    End If
                Next
            End Using
        End Using

        ' Obtenir (en récursif) les groupes aillant les sous-groupes trouvés comme membres
        If Recursif AndAlso LsSousGroupe.Count > 0 Then
            ObtenirGroupeMembreDe(deRoot, LsSousGroupe, Recursif, DtGroupes)
        End If
    End Sub

    Private Function ObtenirGroupePrincipal(ByVal NomGroupe As String, ByVal contexte As PrincipalContext) As GroupPrincipal
        Dim groupe As GroupPrincipal = GroupPrincipal.FindByIdentity(contexte, NomGroupe)

        If groupe Is Nothing Then
            Throw New TsCuGroupeSecuriteInexistantException(NomGroupe)
        End If

        Return groupe
    End Function


    <MethodImpl(MethodImplOptions.NoInlining)>
    Private Sub JournaliserErreur(ByVal erreur As Exception)
        XuCuGestionEvent.AjouterEvenmNormalise(XuGeJournalEvenement.XuGeJeApplicationRRQ,
                                               XuGeTypeEvenement.XuGeTeErreur,
                                               String.Format("Cette erreur n'a pas provoqué de fin anormale pour le client.{0}{1}", Environment.NewLine, erreur),
                                               "TS4N321_AccesseurADLDS",
                                               erreur.StackTrace,
                                               String.Empty)
    End Sub

    Private Function creerCleUtilisateurGroupe(LsNomGroupe As IList(Of String), Recursif As Boolean) As String
        Dim NomGroupeCle As New StringBuilder
        For Each nomGroupe As String In LsNomGroupe
            NomGroupeCle.Append(nomGroupe)
        Next

        Return String.Format(UTILISATEUR_GROUPE, NomGroupeCle, Recursif.ToString).ToUpper()
    End Function

    Private Function creerCleGroupeMembreDe(NomGroupe As String, Recursif As Boolean) As String
        Return String.Format(GROUPE_MEMBREDE, NomGroupe, Recursif.ToString).ToUpper()
    End Function

#End Region

#Region "--- Propriétés ---"

    ''' <summary>
    ''' Cette propriété retourne le nom du serveur ADLDS
    ''' </summary>
    Private ReadOnly Property ServeurADLDS() As String
        Get
            Return _serveurADLDS
        End Get
    End Property

    Private ReadOnly Property ReprtUtilisateurs() As String
        Get
            Return XuCuPolitiqueConfig.ConfigDomaine.ValeurSysteme("TS4", "TS4\TS4N321\RepertoireUtilisateurs")
        End Get
    End Property

    Private ReadOnly Property ReprtRacine() As String
        Get
            Return XuCuPolitiqueConfig.ConfigDomaine.ValeurSysteme("TS4", "TS4\TS4N321\RepertoireRacine")
        End Get
    End Property

    Private ReadOnly Property Environnement() As String
        Get
            Return XuCuPolitiqueConfig.ConfigDomaine.ValeurSysteme("General", "General\Environnement")
        End Get
    End Property

    Private ReadOnly Property GererMemoirePerimee() As Boolean
        Get
            Return _gererMemoirePerimee
        End Get
    End Property

    Private Property DerniereCleMemorise As String
        Get
            Return _derniereCleMemorise
        End Get
        Set(value As String)
            _derniereCleMemorise = value
        End Set
    End Property

#End Region

End Class

Friend Module Extensions

    <Extension>
    Public Function Est(source As String, valeur As String) As Boolean
        Return source.Equals(valeur, StringComparison.InvariantCultureIgnoreCase)
    End Function

    <Extension>
    Public Function distinguishedName(source As SearchResult) As String
        Return source.Properties.Item("distinguishedName")(0).ToString()
    End Function

    <Extension>
    Public Function primaryGroupToken(source As SearchResult) As String
        Return source.Properties.Item("primaryGroupToken")(0).ToString()
    End Function

    <Extension>
    Public Function givenName(source As SearchResult) As String
        If source.Properties("givenName").Count = 0 Then Return String.Empty
        Return source.Properties.Item("givenName")(0).ToString()
    End Function

    <Extension>
    Public Function sn(source As SearchResult) As String
        If source.Properties("sn").Count = 0 Then Return String.Empty
        Return source.Properties.Item("sn")(0).ToString()
    End Function

End Module