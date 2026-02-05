Imports System.Collections.Generic
Imports System.Security.Principal
Imports System.DirectoryServices
Imports System.Threading
Imports System.Text
Imports Rrq.InfrastructureCommune.Parametres
Imports Rrq.InfrastructureCommune.ScenarioTransactionnel
Imports System.Runtime.CompilerServices

''' <summary>
''' Cette classe utilitaire récupère et stocke des informations sur le contenu de l'active directory.
''' </summary>
Public Class TsCuAccesAD
    Implements TsIAccesAD

    Private Const NOM_DOMAINE_RRQ As String = "RRQ_QC"
    Private Const NOM_DOMAINE_CARRA As String = "INTRA"
    Private Const NOM_DOMAINE_RQ As String = "RQ"

    '' <summary>Aiguillage des serveurs active directory. Par défaut, on utilise celui de la RRQ</summary>
    Private _domaine As TsAadNomDomaine = TsAadNomDomaine.TsDomaineRQ
    Private _domaineRQ As String
    Private _domaineRRQ As String
    Private _domaineCARRA As String


#Region "--- Variables de cache ---"

    ''' <summary>Date à laquelle les caches seront périmées et devront être rafraîchies</summary>
    Private Shared DateExpirationCaches As DateTime
    ''' <summary>On met en cache les membres du groupe de type Utilisateur.</summary>
    Private Shared CacheUtilisateurMembresDuGroupe As New Dictionary(Of String, List(Of IdentityReference))
    ''' <summary>On met en cache les membres du groupe de type Groupe.</summary>
    Private Shared CacheGroupeMembresDuGroupe As New Dictionary(Of String, List(Of IdentityReference))
    ''' <summary>On met en cache les membres du groupe de type Groupe et Liste.</summary>
    Private Shared CacheGroupesEtListesMembreDuGroupe As New Dictionary(Of String, List(Of TsDtGroupe))
    ''' <summary>On met en cache l'utilisateur et le groupe.</summary>
    Private Shared CacheUtilisateurGroupe As New Dictionary(Of String, Boolean)
    ''' <summary>Pour synchroniser les lectures/ecritures dans le cache</summary>
    Private Shared objLockCacheUtilisateurMembresDuGroupe As ReaderWriterLock = New ReaderWriterLock()
    Private Shared objLockCacheGroupeMembresDuGroupe As ReaderWriterLock = New ReaderWriterLock()
    Private Shared objLockCacheGroupesEtListesMembreDuGroupe As ReaderWriterLock = New ReaderWriterLock()
    Private Shared objLockCacheUtilisateurGroupe As ReaderWriterLock = New ReaderWriterLock()

#End Region

#Region "--- Implémentation : TsIAccesAD ---"

    ''' <summary>
    ''' Cette méthode vérifie si un utilisateur est membre directement ou indirectement d'un groupe
    ''' </summary>
    ''' <remarks>Dans le UserToken, nous avons la liste de ses groupes de sécurité (direct et indirect). 
    ''' On prend en charge le cas que le NomGroupe est un groupe de distribution et donc non présent dans le UserToken.</remarks>
    ''' <param name="NomGroupe">Nom du groupe à vérifier</param>
    ''' <returns>True si l'utilisateur est un membre direct ou indirect du groupe</returns>
    Public Function EstMembreGroupe(ByVal NomGroupe As String, ByVal UserToken As WindowsIdentity) As Boolean Implements TsIAccesAD.EstMembreGroupe
        Try
            Return estMembreGroupeInterne(NomGroupe, UserToken)
        Catch ex As TsCuUtilisateurInexistantException
            Return False
        End Try
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
        ' On retourne une structure Dt, mais on travaille temporairement avec un DataTable
        Using dt As New DataTable()
            Dim lesChamps As New List(Of String)()

            dt.PrimaryKey = New DataColumn() {dt.Columns.Add("sAMAccountName", GetType(String))}
            If InfoRetour.Contient(TsAadInfoUtilisateur.TsAadIuCode) Then
                lesChamps.Add("sAMAccountName")
            End If

            If InfoRetour.Contient(TsAadInfoUtilisateur.TsAadIuNomComplet) Then
                dt.Columns.Add("displayName", GetType(String))
                lesChamps.Add("displayName")
            End If

            If InfoRetour.Contient(TsAadInfoUtilisateur.TsAadIuSID) Then
                dt.Columns.Add("objectSid", GetType(String))
                lesChamps.Add("objectSid")
            End If

            Dim nomsDesGroupesFait As New List(Of String)()
            If LsNomGroupe.Count > 0 Then
                Using deRoot As New DirectoryEntry()
                    deRoot.AuthenticationType = AuthenticationTypes.Secure
                    deRoot.Path = String.Format("LDAP://{0}", serveurActiveDirectory)

                    obtenirUtilisateurGroupe(deRoot, LsNomGroupe, nomsDesGroupesFait, Recursif, lesChamps, dt)
                End Using
            End If

            Dim lesUtilisateurs As New List(Of TsDtUtilisateur)()
            For Each row As DataRow In dt.Rows
                Dim Utils As New TsDtUtilisateur()

                If InfoRetour.Contient(TsAadInfoUtilisateur.TsAadIuCode) Then
                    Utils.Code = row("sAMAccountName").ToString()
                End If

                If InfoRetour.Contient(TsAadInfoUtilisateur.TsAadIuNomComplet) Then
                    Utils.NomComplet = row("displayName").ToString()
                End If

                If InfoRetour.Contient(TsAadInfoUtilisateur.TsAadIuSID) Then
                    Utils.SID = row("objectSid").ToString()
                End If

                lesUtilisateurs.Add(Utils)
            Next

            Return lesUtilisateurs
        End Using
    End Function

    ''' <summary>
    ''' Cette fonction permet d'obtenir la liste des Groupes (de sécurité et liste de distribution) pour un Utilisateur donné
    ''' </summary>
    ''' <param name="UserToken">Token de l'utilisateur</param>
    ''' <returns>Retourne la liste des groupes auquel l'utilisateur est membre</returns>
    ''' <remarks>Seulement le code du groupe.</remarks>
    Public Function ObtenirGroupeUtilisateur(ByVal UserToken As WindowsIdentity) As IList(Of TsDtGroupe) Implements TsIAccesAD.ObtenirGroupeUtilisateur
        Dim CodeUsager As String = UserToken.Name.Substring(UserToken.Name.IndexOf("\"c) + 1)
        Return ObtenirGroupeUtilisateur(CodeUsager)
    End Function

    ''' <summary>
    ''' Cette fonction permet d'obtenir la liste des Groupes (de sécurité et liste de distribution) pour un Utilisateur donné
    ''' </summary>
    ''' <param name="Usager">Compte de l'utilisateur</param>
    ''' <returns>Retourne la liste des groupes auquel l'utilisateur est membre</returns>
    ''' <remarks>Seulement le code du groupe.</remarks>
    Public Function ObtenirGroupeUtilisateur(ByVal Usager As String) As IList(Of TsDtGroupe) Implements TsIAccesAD.ObtenirGroupeUtilisateur
        Dim LsGlobal As New List(Of String)
        Dim LsGroupes As New List(Of TsDtGroupe)

        Using deRoot As New DirectoryEntry
            deRoot.AuthenticationType = AuthenticationTypes.Secure
            deRoot.Path = String.Format("LDAP://{0}", serveurActiveDirectory)

            ' *** Obtenir les groupes directs ***
            obtenirGroupeDirect(deRoot, Usager, LsGlobal)
        End Using

        'Tri de la liste pour l'ajouter de façon alphabétique à la liste de retour.
        LsGlobal.Sort()

        'Ajout finale dans la liste de retour
        For Each itemListe As String In LsGlobal
            Dim element As New TsDtGroupe()
            element.NomGroupe = itemListe
            LsGroupes.Add(element)
        Next

        Return LsGroupes
    End Function

    ''' <summary>
    ''' Cette fonction permet d'obtenir les informations d'un groupe.
    ''' </summary>
    ''' <param name="NomGroupe">Le nom du groupe à obtenir</param>
    ''' <param name="InfoRetour">Valeur indiquant les informations à retourner pour le groupe utilisateur. Plusieurs valeurs peuvent être combinées à l'aide d'un "Or"</param>
    ''' <returns>Retourne le groupe</returns>
    ''' <remarks>Seulement le premier niveau de groupe de distribution est recherché.</remarks>
    Public Function ObtenirGroupe(ByVal NomGroupe As String, ByVal InfoRetour As TsAadInfoGroupe) As TsDtGroupe Implements TsIAccesAD.ObtenirGroupe
        Dim Groupe As New TsDtGroupe
        Groupe.NomGroupe = NomGroupe

        Using deRoot As New DirectoryEntry
            deRoot.AuthenticationType = AuthenticationTypes.Secure
            deRoot.Path = String.Format("LDAP://{0}", serveurActiveDirectory)

            Using dsGroupe As New DirectorySearcher(deRoot)
                dsGroupe.CacheResults = False
                dsGroupe.PageSize = 1000

                dsGroupe.Filter = String.Format("(&(objectCategory=group)(sAmAccountName={0}))", NomGroupe)

                If InfoRetour.Contient(TsAadInfoGroupe.TsAadIuDescription) Then
                    dsGroupe.PropertiesToLoad.Add("Description")
                End If

                Dim ResultatRecherche As SearchResult = dsGroupe.FindOne()

                If InfoRetour.Contient(TsAadInfoGroupe.TsAadIuDescription) Then
                    Groupe.DescriptionGroupe = ResultatRecherche.Properties.Item("Description")(0).ToString
                End If
            End Using
        End Using

        Return Groupe
    End Function

    ''' <summary>
    ''' Cette fonction permet d'obtenir le nom d'un groupe à partir de son SID. La fonction recherche dans les domaine RQ
    ''' </summary>
    ''' <param name="SId">Le SId du groupe recherché</param>
    ''' <returns>Retourne le nom du groupe</returns>
    Public Function ObtenirGroupeParSID(ByVal SId As String) As String
        Using deRoot As New DirectoryEntry
            deRoot.AuthenticationType = AuthenticationTypes.Secure
            deRoot.Path = String.Format("LDAP://{0}", serveurActiveDirectory)

            Using dsGroupe As New DirectorySearcher(deRoot)
                dsGroupe.CacheResults = False
                dsGroupe.PageSize = 1000

                dsGroupe.Filter = String.Format("(&(objectCategory=group)(objectSid={0}))", SId)

                Dim ResultatRecherche As SearchResult = dsGroupe.FindOne()

                If ResultatRecherche IsNot Nothing Then
                    Return "RQ\" & ResultatRecherche.Properties.Item("SamAccountName")(0).ToString
                End If
            End Using
        End Using

        Throw New TsCuSIdInexistantException(SId)

    End Function

    ''' <summary>
    ''' Cette méthode obtient la liste de tous les utilisateurs membres d'au moins un des groupes spécifiés.
    ''' </summary>
    ''' <param name="NomGroupe">Liste des noms des groupes pour lesquels obtenir les utilisateurs.</param>
    ''' <param name="Recursif">Valeur indiquant s'il faut ou non obtenir les utilisateurs membres des groupes spécifiés de façon indirecte (via un autre groupe membre d'un des groupes spécifiés).</param>
    ''' <returns>Liste de tous les utilisateurs membres d'au moins un des groupes spécifiés.</returns>
    ''' <remarks>Un utilisateur est considéré membre d'un groupe s'il en est "membre" au niveau de l'active directory ou si ce groupe est son groupe principal.</remarks>
    Public Function ObtenirGroupeMembreDe(ByVal NomGroupe As String, ByVal Recursif As Boolean) As IList(Of TsDtGroupe) Implements TsIAccesAD.ObtenirGroupeMembreDe
        ' On retourne une structure Dt, mais on travaille temporairement avec un DataTable
        Dim LsGroupes As New List(Of TsDtGroupe)

        'implanter la cache ici pour voir
        Dim DonneesDansLeCache As Boolean = False

        ' Si on a pas besoin d'être récursif, on n'utilise pas la cache pour ne pas retourner trop de groupes
        If (Not estCacheExpiree()) And Recursif Then
            ' Les caches ne sont pas expirées
            objLockCacheGroupesEtListesMembreDuGroupe.AcquireReaderLock(1000)
            Try
                DonneesDansLeCache = CacheGroupesEtListesMembreDuGroupe.ContainsKey(NomGroupe)
            Finally
                objLockCacheGroupesEtListesMembreDuGroupe.ReleaseReaderLock()
            End Try
        End If

        ' Si les données ne sont pas en cache, on va les chercher
        If Not DonneesDansLeCache Then
            Using deRoot As New DirectoryEntry

                deRoot.AuthenticationType = AuthenticationTypes.Secure
                deRoot.Path = String.Format("LDAP://{0}", serveurActiveDirectory)

                'Transformer le nom du groupe en DistinguishedName avant de le passer à la méthode privée.
                Dim lstDN As New List(Of String)
                Dim ResultatRecherche As SearchResult

                Using dsGroupe As New DirectorySearcher(deRoot)
                    dsGroupe.Filter = String.Format("(&(objectCategory=group)(sAmAccountName={0}))", NomGroupe)
                    dsGroupe.PropertiesToLoad.Add("distinguishedName")

                    ResultatRecherche = dsGroupe.FindOne()
                End Using

                'Si le groupe fourni est trouvé, 
                If Not ResultatRecherche Is Nothing Then
                    lstDN.Add(ResultatRecherche.Properties.Item("distinguishedName")(0).ToString)

                    obtenirGroupeMembreDe(deRoot, lstDN, Recursif, LsGroupes)
                    'Mis en commentaire lors de la livraison du cycle de novembre. Les utilisateurs INTRA
                    'avait des erreurs car ils étaient membres de groupes de distribuition.
                    'Else
                    '    Throw New TsCuGroupeInexistantException(NomGroupe)
                End If

            End Using

            ' Si on est récursif, on ajoute dans le cache, sinon on pourrait ne pas retourner assez de groupes la prochaine fois que ce groupe est demandé
            If Recursif Then
                objLockCacheGroupesEtListesMembreDuGroupe.AcquireWriterLock(1000)

                Try
                    CacheGroupesEtListesMembreDuGroupe.Item(NomGroupe) = LsGroupes
                Finally
                    objLockCacheGroupesEtListesMembreDuGroupe.ReleaseWriterLock()
                End Try
            End If
        Else
            objLockCacheGroupesEtListesMembreDuGroupe.AcquireReaderLock(1000)
            Try
                LsGroupes = CacheGroupesEtListesMembreDuGroupe(NomGroupe)
            Finally
                objLockCacheGroupesEtListesMembreDuGroupe.ReleaseReaderLock()
            End Try
        End If

        Return LsGroupes
    End Function

    ''' <summary>
    ''' Cette fonction permet d'obtenir une liste de groupes répondant aux critères du nom recherché.
    ''' </summary>
    ''' <param name="NomGroupe">Le nom du groupe à obtenir. Peut être un nom générique avec '*' pour obtenir une liste</param>
    ''' <returns>Retourne la liste des groupes recherchés selon le critère.</returns>
    ''' <remarks></remarks>
    Public Function RechercherGroupes(ByVal NomGroupe As String) As IList(Of String) Implements TsIAccesAD.RechercherGroupes
        Dim Groupes As New List(Of String)

        Using deRoot As New DirectoryEntry
            deRoot.AuthenticationType = AuthenticationTypes.Secure
            deRoot.Path = String.Format("LDAP://{0}", serveurActiveDirectory)

            Using dsGroupe As New DirectorySearcher(deRoot)
                dsGroupe.CacheResults = False
                dsGroupe.PageSize = 1000

                dsGroupe.Filter = String.Format("(&(objectCategory=group)(sAmAccountName={0}))", NomGroupe)

                Dim oResults As SearchResultCollection = dsGroupe.FindAll
                For Each oResult As SearchResult In oResults
                    If Not oResult.GetDirectoryEntry.Properties("sAMAccountName").Value Is Nothing Then
                        Groupes.Add(oResult.GetDirectoryEntry.Properties("sAMAccountName").Value.ToString)
                    End If
                Next
            End Using
        End Using

        Return Groupes
    End Function

#End Region

#Region "--- Méthodes Publiques ---"

    Public Function EstMembreGroupe2(ByVal NomGroupe As String, ByVal UserToken As WindowsIdentity) As Boolean
        Try
            Return estMembreGroupe2Interne(NomGroupe, UserToken)
        Catch ex As TsCuUtilisateurInexistantException
            Return False
        End Try

    End Function

    ''' <summary>
    ''' Cette méthode permet de determiner le domaine de l'active directory à accéder
    ''' </summary>
    <Obsolete("Tous les groupes sont migrés au domaine RQ. Cette méthode ne doit plus être ", True)>
    Public Sub Domaine(ByVal codeDomaine As TsAadNomDomaine)
        Select Case codeDomaine
            Case TsAadNomDomaine.TsDomaineRRQ, TsAadNomDomaine.TsDomaineCARRA, TsAadNomDomaine.TsDomaineRQ
                _domaine = codeDomaine
            Case Else
                Throw New TsCuDomaineActiveDirectoryInconnu(codeDomaine.ToString)
        End Select
    End Sub

    ''' <summary>
    ''' Cette méthode permet de determiner le domaine de l'active directory à accéder
    ''' </summary>
    ''' <param name="nomDomaine">Nom du domaine à être consulté:
    ''' "RRQ"    - Pour le domaine RRQ
    ''' "INTRA"  - Pour le domaine CARRA 
    ''' </param>
    <Obsolete("Tous les groupes sont migrés au domaine RQ. Cette méthode ne doit plus être ", True)>
    Public Sub Domaine(ByVal nomDomaine As String)
        Select Case nomDomaine.ToUpper
            Case NOM_DOMAINE_RRQ
                _domaine = TsAadNomDomaine.TsDomaineRRQ
            Case NOM_DOMAINE_CARRA
                _domaine = TsAadNomDomaine.TsDomaineCARRA
            Case NOM_DOMAINE_RQ
                _domaine = TsAadNomDomaine.TsDomaineRQ
            Case Else
                Throw New TsCuDomaineActiveDirectoryInconnu(nomDomaine)
        End Select
    End Sub

    ''' <summary>
    ''' Cette méthode permet d'obtenir le domaine de l'active directory utilisé
    ''' </summary>
    <Obsolete("Tous les groupes sont migrés au domaine RQ. Cette méthode ne doit plus être ", True)>
    Public Function ObtenirDomaine() As String
        Select Case _domaine
            Case TsAadNomDomaine.TsDomaineCARRA
                Return NOM_DOMAINE_CARRA
            Case TsAadNomDomaine.TsDomaineRRQ
                Return NOM_DOMAINE_RRQ
            Case Else
        End Select

        Return NOM_DOMAINE_RQ
    End Function

    Public Function GroupeExiste(nomGroupe As String) As Boolean
        Dim groupes As IList(Of String) = RechercherGroupes(nomGroupe)
        Return groupes.Count > 0
    End Function

#End Region

#Region "--- Méthode privées ---"

    Private Function estMembreGroupe2Interne(ByVal NomGroupe As String, ByVal UserToken As WindowsIdentity) As Boolean
        Dim cle As String = String.Format("{0}_{1}", UserToken.Name, NomGroupe)

        Dim besoinAllerAD As Boolean = True
        Dim estMembre As Boolean = False

        If (Not estCacheExpiree()) Then
            ' Les caches ne sont pas expirées
            objLockCacheUtilisateurGroupe.AcquireReaderLock(1000)
            Try
                If CacheUtilisateurGroupe.ContainsKey(cle) Then
                    besoinAllerAD = False
                    estMembre = CacheUtilisateurGroupe(cle)
                End If
            Finally
                objLockCacheUtilisateurGroupe.ReleaseReaderLock()
            End Try
        End If

        If besoinAllerAD Then
            estMembre = estMembreGroupeInterne(NomGroupe, UserToken)
            objLockCacheUtilisateurGroupe.AcquireWriterLock(1000)
            Try
                CacheUtilisateurGroupe(cle) = estMembre
            Finally
                objLockCacheUtilisateurGroupe.ReleaseWriterLock()
            End Try
        End If

        Return estMembre
    End Function

    Private Function estMembreGroupeInterne(ByVal NomGroupe As String, ByVal UserToken As WindowsIdentity) As Boolean
        Dim ListeUtilisateurMembre As List(Of IdentityReference) = obtenirListeMembreDeTypeUtilisateur(NomGroupe)
        Dim ListeGroupeMembre As New List(Of IdentityReference)()

        'Si l'utilisateur lui même est membre du groupe
        If ListeUtilisateurMembre.Contains(UserToken.User) Then
            Return True
        Else
            ListeGroupeMembre = obtenirListeMembreDeTypeGroupe(NomGroupe)
        End If

        'Si l'utilisateur appartient à un groupe membre du groupe en paramètre
        For Each Groupe As IdentityReference In UserToken.Groups
            If ListeGroupeMembre.Contains(Groupe) Then
                Return True
            End If
        Next

        ' En dernier recours
        Dim groupesduuser As IList(Of TsDtGroupe) = ObtenirGroupeUtilisateur(UserToken)
        For Each groupe As TsDtGroupe In groupesduuser
            If groupe.NomGroupe.Equals(NomGroupe, StringComparison.InvariantCultureIgnoreCase) Then
                Return True
            End If
        Next

        ' L'utilisateur n'est pas lié au groupe en paramètre
        Return False
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="deRoot"></param>
    ''' <param name="LsDN"></param>
    ''' <param name="Recursif"></param>
    ''' <param name="DtGroupes"></param>
    ''' <remarks></remarks>
    Private Sub obtenirGroupeMembreDe(ByRef deRoot As DirectoryEntry, ByRef LsDN As List(Of String), ByVal Recursif As Boolean, ByRef DtGroupes As List(Of TsDtGroupe))
        Dim LsSousGroupe As New List(Of String)

        Using dsGroupe As New DirectorySearcher(deRoot)
            dsGroupe.CacheResults = False
            dsGroupe.PageSize = 1000

            Dim criteres As New StringBuilder
            'Ajouter chaque groupe à rechercher au filtre
            For Each DN As String In LsDN
                criteres.AppendFormat("(member={0})", DN)
            Next

            'Préparer la requête pour obtenir tous les groupes qui ont comme membres 
            'un des groupes fournit plus haut
            dsGroupe.Filter = String.Format("(&(objectCategory=Group)(|{0}))", criteres.ToString())
            dsGroupe.PropertiesToLoad.Add("primaryGroupToken")
            dsGroupe.PropertiesToLoad.Add("distinguishedName")
            dsGroupe.PropertiesToLoad.Add("sAMAccountName")

            Using collectionResultats As SearchResultCollection = dsGroupe.FindAll()

                'Ajouter un objet TsDtGroupe à la liste de retour DtGroupes
                For Each Resultat As SearchResult In collectionResultats
                    Dim ItemGroupe As New TsDtGroupe
                    ItemGroupe.NomGroupe = Resultat.Properties("sAMAccountName")(0).ToString

                    ' On ajoute le groupe que s'il n'est pas déjà dans notre liste de groupes
                    ' Prévient ainsi les boucles infinies lorsque 2 groupes se réfèrent mutuellement
                    If Not DtGroupes.Exists(Function(x) x.NomGroupe = ItemGroupe.NomGroupe) Then
                        'Conserver les DN pour refaire une requête dans le cas où on est récursif
                        LsSousGroupe.Add(Resultat.Properties("distinguishedName")(0).ToString)

                        DtGroupes.Add(ItemGroupe)
                    End If
                Next
            End Using
        End Using

        ' Obtenir (en récursif) les groupes aillant les sous-groupes trouvés comme membres
        If Recursif AndAlso LsSousGroupe.Count > 0 Then
            obtenirGroupeMembreDe(deRoot, LsSousGroupe, Recursif, DtGroupes)
        End If
    End Sub

    ''' <summary>
    ''' Fonction de service. Permet de trouver un element par sa propriété sAMAccountName.
    ''' </summary>
    ''' <param name="nomEntry">Le sAMAccountName du DirectoryEntry recherché.</param>
    ''' <param name="DETete">L'objet DirectoryEntry dans lequel sera effectué la recherche.</param>
    ''' <returns>Retourne nothing si aucun ne correspond au sAMAccountName demandé.</returns>
    Private Function trouverParSAMAccountName(ByVal nomEntry As String, ByVal DETete As DirectoryEntry) As SearchResult
        Using searcher As New DirectorySearcher(DETete, String.Format("(sAMAccountName={0})", nomEntry))
            Return searcher.FindOne()
        End Using
    End Function

    ''' <summary>
    ''' Obtiens la liste des membres d'un groupe de l'AD
    ''' </summary>
    ''' <param name="NomGroupe">Le nom du groupe</param>
    ''' <returns>La liste des membres du groupe</returns>
    Private Function rechercherMembresGroupeAD(ByVal NomGroupe As String) As SearchResultCollection
        Using DebutDeRecherche As New DirectoryEntry()
            DebutDeRecherche.AuthenticationType = AuthenticationTypes.Secure
            DebutDeRecherche.Path = String.Format("LDAP://{0}", serveurActiveDirectory())

            Dim groupeSearchResult As SearchResult = trouverParSAMAccountName(NomGroupe, DebutDeRecherche)

            If groupeSearchResult Is Nothing Then
                Throw New TsCuGroupeInexistantException(NomGroupe)
            End If

            Dim distinguishedName As String = groupeSearchResult.Properties("distinguishedName").Item(0).ToString()

            Using DShearcher As New DirectorySearcher(DebutDeRecherche, String.Format("(&(|(objectCategory=person)(objectCategory=group))(memberof={0}))", distinguishedName))
                Return DShearcher.FindAll()
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Met en cache les informations sur les membres du groupe
    ''' </summary>
    ''' <param name="NomGroupe">Le groupe à mettre en cache</param>
    ''' <remarks></remarks>
    Private Sub chargerCache(ByVal NomGroupe As String)
        Dim ListeUtilisateur As New List(Of IdentityReference)
        Dim ListeGroupe As New List(Of IdentityReference)

        Using ListeMembres As SearchResultCollection = rechercherMembresGroupeAD(NomGroupe)
            For Each Membre As SearchResult In ListeMembres
                If Membre.Properties("objectClass").Contains("person") Then
                    ListeUtilisateur.Add(New SecurityIdentifier(DirectCast(Membre.Properties("objectSid").Item(0), Byte()), 0))
                ElseIf Membre.Properties("objectClass").Contains("group") Then
                    ListeGroupe.Add(New SecurityIdentifier(DirectCast(Membre.Properties("objectSid").Item(0), Byte()), 0))
                End If
            Next
        End Using

        objLockCacheUtilisateurMembresDuGroupe.AcquireWriterLock(1000)

        Try
            CacheUtilisateurMembresDuGroupe.Item(NomGroupe) = ListeUtilisateur
        Finally
            objLockCacheUtilisateurMembresDuGroupe.ReleaseWriterLock()
        End Try

        objLockCacheGroupeMembresDuGroupe.AcquireWriterLock(1000)

        Try
            CacheGroupeMembresDuGroupe.Item(NomGroupe) = ListeGroupe
        Finally
            objLockCacheGroupeMembresDuGroupe.ReleaseWriterLock()
        End Try
    End Sub

    ''' <summary>
    ''' Obtiens la liste des membres du groupe qui sont de type Utilisateur
    ''' </summary>
    ''' <param name="NomGroupe">Le nom du groupe</param>
    ''' <returns>La liste des membres du groupe qui sont de type Utilisateur</returns>
    ''' <remarks>Un cache statique est utilisé pour stocker les informations</remarks>
    Private Function obtenirListeMembreDeTypeUtilisateur(ByVal NomGroupe As String) As List(Of IdentityReference)
        Dim DonneesDansLeCache As Boolean = False

        If Not estCacheExpiree() Then
            ' Les caches ne sont pas expirées
            objLockCacheUtilisateurMembresDuGroupe.AcquireReaderLock(1000)

            Try
                DonneesDansLeCache = CacheUtilisateurMembresDuGroupe.ContainsKey(NomGroupe)
            Finally
                objLockCacheUtilisateurMembresDuGroupe.ReleaseReaderLock()
            End Try
        End If

        ' Si les données ne sont pas en cache, on va les chercher
        If DonneesDansLeCache = False Then
            chargerCache(NomGroupe)
        End If

        Dim resultat As List(Of IdentityReference) = Nothing
        objLockCacheUtilisateurMembresDuGroupe.AcquireReaderLock(1000)
        Try
            resultat = CacheUtilisateurMembresDuGroupe(NomGroupe)
        Finally
            objLockCacheUtilisateurMembresDuGroupe.ReleaseReaderLock()
        End Try

        Return resultat
    End Function

    ''' <summary>
    ''' Obtiens la liste des membres du groupe qui sont de type Groupe
    ''' </summary>
    ''' <param name="NomGroupe">Le nom du groupe</param>
    ''' <returns>La liste des membres du groupe qui sont de type Groupe</returns>
    ''' <remarks>Un cache statique est utilisé pour stocker les informations</remarks>
    Private Function obtenirListeMembreDeTypeGroupe(ByVal NomGroupe As String) As List(Of IdentityReference)
        Dim DonneesDansLeCache As Boolean = False

        If Not estCacheExpiree() Then
            ' Les caches ne sont pas expirées
            objLockCacheGroupeMembresDuGroupe.AcquireReaderLock(1000)

            Try
                DonneesDansLeCache = CacheGroupeMembresDuGroupe.ContainsKey(NomGroupe)
            Finally
                objLockCacheGroupeMembresDuGroupe.ReleaseReaderLock()
            End Try
        End If

        ' Si les données ne sont pas en cache, on va les chercher
        If DonneesDansLeCache = False Then
            chargerCache(NomGroupe)
        End If

        Dim resultat As List(Of IdentityReference) = Nothing
        objLockCacheGroupeMembresDuGroupe.AcquireReaderLock(1000)
        Try
            resultat = CacheGroupeMembresDuGroupe(NomGroupe)
        Finally
            objLockCacheGroupeMembresDuGroupe.ReleaseReaderLock()
        End Try

        Return resultat
    End Function

    Private Sub obtenirUtilisateurGroupe(ByVal deRoot As DirectoryEntry,
                                         ByVal LsNomGroupe As IList(Of String),
                                         ByVal LsNomGroupeFait As List(Of String),
                                         ByVal Recursif As Boolean,
                                         ByVal LsChamp As List(Of String),
                                         ByVal DtUtils As DataTable)

        ' Préparer une liste de groupes pour lesquels il faut obtenir les utilisateurs
        ' (ceux pour lesquels l'obtention n'a pas encore été faite)
        Dim criteres As New StringBuilder
        For Each NomGroupe As String In LsNomGroupe
            If LsNomGroupeFait.IndexOf(NomGroupe) = -1 Then
                criteres.AppendFormat("(sAMAccountName={0})", NomGroupe)
                LsNomGroupeFait.Add(NomGroupe)
            End If
        Next

        If criteres.Length > 0 Then
            ' Obtenir les distinguishedName et primaryGroupToken des groupes de la liste
            ' (nécessaire pour l'obtention des membres des groupes)
            Using dsGroupe As New DirectorySearcher(deRoot)
                dsGroupe.CacheResults = False
                dsGroupe.PageSize = 1000

                dsGroupe.Filter = String.Format("(&(objectCategory=group)(|{0}))", criteres.ToString())
                dsGroupe.PropertiesToLoad.Add("distinguishedName")
                dsGroupe.PropertiesToLoad.Add("primaryGroupToken")

                criteres = New StringBuilder
                Using collectionResultats As SearchResultCollection = dsGroupe.FindAll()
                    For Each Resultat As SearchResult In collectionResultats
                        criteres.AppendFormat("(memberOf={0})(primaryGroupID={1})",
                                               Resultat.Properties.Item("distinguishedName")(0).ToString(),
                                               Resultat.Properties.Item("primaryGroupToken")(0).ToString())
                    Next
                End Using
            End Using

            If criteres.Length > 0 Then
                ' Obtenir les membres des groupes de la liste
                Using dsMembre As New DirectorySearcher(deRoot)
                    dsMembre.CacheResults = False
                    dsMembre.PageSize = 1000

                    dsMembre.Filter = String.Format("(&(|(objectCategory=person)(objectCategory=group))(|{0}))", criteres.ToString())

                    dsMembre.PropertiesToLoad.Add("sAMAccountName")
                    dsMembre.PropertiesToLoad.Add("objectCategory")

                    For Each Champ As String In LsChamp
                        dsMembre.PropertiesToLoad.Add(Champ)
                    Next

                    Dim LsSousGroupe As New List(Of String)
                    Using collectionResultats As SearchResultCollection = dsMembre.FindAll()
                        For Each Resultat As SearchResult In collectionResultats
                            If Resultat.Properties("objectCategory")(0).ToString().StartsWith("CN=Person") Then
                                Dim CodeUtils As String = Resultat.Properties.Item("sAMAccountName")(0).ToString()

                                If DtUtils.Rows.Find(CodeUtils) Is Nothing Then
                                    Dim Dr As DataRow = DtUtils.NewRow()

                                    For Each Champ As String In LsChamp
                                        Dim sbValeur As New StringBuilder

                                        Select Case Champ.ToLower()
                                            Case "memberof"
                                                For Each Valeur As Object In Resultat.Properties(Champ)
                                                    Dim Parties() As String = Valeur.ToString().Split(","c)

                                                    If Parties(0).Length > 3 Then
                                                        If sbValeur.Length > 0 Then
                                                            sbValeur.Append(";")
                                                        End If

                                                        sbValeur.Append(Parties(0).Substring(3))
                                                    End If
                                                Next

                                            Case "objectsid"
                                                For Each Valeur As Object In Resultat.Properties(Champ)
                                                    sbValeur.Length = 0
                                                    sbValeur.Append(convertirSid(DirectCast(Valeur, Byte())))
                                                Next

                                            Case Else
                                                ' On passe ici pour sAMAccountName
                                                For Each Valeur As Object In Resultat.Properties(Champ)
                                                    sbValeur.Length = 0
                                                    sbValeur.Append(Valeur)
                                                Next
                                        End Select

                                        Dr(Champ) = sbValeur.ToString()
                                    Next

                                    DtUtils.Rows.Add(Dr)
                                End If

                            Else
                                LsSousGroupe.Add(Resultat.Properties.Item("sAMAccountName")(0).ToString())
                            End If
                        Next
                    End Using

                    ' Obtenir (en récursif) les membres des sous-groupes
                    If Recursif AndAlso LsSousGroupe.Count > 0 Then
                        obtenirUtilisateurGroupe(deRoot, LsSousGroupe, LsNomGroupeFait, Recursif, LsChamp, DtUtils)
                    End If
                End Using
            End If
        End If
    End Sub

    ''' <summary>
    ''' Cette fonction obtient les groupes d'utilisateurs direct via l'AD
    ''' </summary>
    ''' <param name="deRoot">Directory entry</param>
    ''' <param name="Usager">Compte de l'utilisateur</param>
    ''' <remarks></remarks>
    Private Sub obtenirGroupeDirect(ByVal deRoot As DirectoryEntry, ByVal Usager As String, ByVal LsGroupe As List(Of String))
        Using DsRecherche As New System.DirectoryServices.DirectorySearcher(deRoot)
            'On va chercher ici un objet de type user et dont le nom d'ouverture de session = <username>.
            DsRecherche.Filter = String.Concat("(&(objectClass=User) (sAMAccountName=", Usager, "))")

            'Ne récupère que la propritée MemberOf.
            DsRecherche.PropertiesToLoad.Add("MemberOf")
            'Et aussi primaryGroupID
            DsRecherche.PropertiesToLoad.Add("primaryGroupID")

            'Recherche et retourne la première entrée trouvé.
            Dim result As SearchResult = DsRecherche.FindOne
            If result Is Nothing Then
                Throw New TsCuUtilisateurInexistantException(Usager, deRoot.Path)
            End If

            'Parcours le contenu de la propritée MemberOf 
            For Each Valeur As Object In result.Properties("MemberOf")
                Dim Parties() As String = Valeur.ToString().Split(","c)
                Dim nomgroupe As String = Parties(0).Substring(3)
                If Not LsGroupe.Contains(nomgroupe) Then
                    LsGroupe.Add(nomgroupe)
                    ' Ajout des groupes de sécurité et des listes de distribution imbriquées dans les listes membres de
                    Dim nouveauxgroupestmp As IList(Of TsDtGroupe) = ObtenirGroupeMembreDe(nomgroupe, True)
                    For Each grp As TsDtGroupe In nouveauxgroupestmp
                        If Not LsGroupe.Contains(grp.NomGroupe) Then
                            LsGroupe.Add(grp.NomGroupe)
                        End If
                    Next
                End If
            Next

            Dim groupeprimaire As String = obtenirNomGroupePrimaire(deRoot, DsRecherche, result)

            ' Ajouter le groupe Utilisateurs Domaine
            If Not LsGroupe.Contains(groupeprimaire) Then
                LsGroupe.Add(groupeprimaire)
                ' Ajout des listes de distribution imbriquées dans les listes membres de
                Dim nouveauxgroupestmp As IList(Of TsDtGroupe) = ObtenirGroupeMembreDe(groupeprimaire, True)
                For Each grp As TsDtGroupe In nouveauxgroupestmp
                    If Not LsGroupe.Contains(grp.NomGroupe) Then
                        LsGroupe.Add(grp.NomGroupe)
                    End If
                Next
            End If
        End Using
    End Sub

    Private Function convertirSid(ByRef Octets As Byte()) As String
        Dim sbSid As New StringBuilder

        ' Convertir le SID en String
        For Each Octet As Byte In Octets
            If Octet < &H10 Then
                sbSid.AppendFormat("\0{0}", Hex(Octet))
            Else
                sbSid.AppendFormat("\{0}", Hex(Octet))
            End If
        Next

        Return sbSid.ToString()
    End Function

    ' http://blog.cjwdev.co.uk/2010/09/02/vb-net-get-group-name-from-primarygroupid-attribute-in-active-directory/
    Private Function obtenirNomGroupePrimaire(ByVal Domain As DirectoryEntry, ByVal SearcherObject As DirectorySearcher, ByVal User As SearchResult) As String
        Try
            Dim GroupSID As New Security.Principal.SecurityIdentifier(New Security.Principal.SecurityIdentifier(DirectCast(Domain.Properties("objectSid").Value, Byte()), 0).ToString & "-" & CStr(User.Properties("primaryGroupID")(0)))
            Dim GroupSIDString As New System.Text.StringBuilder
            Dim GroupSIDBytes(GroupSID.BinaryLength - 1) As Byte
            GroupSID.GetBinaryForm(GroupSIDBytes, 0)
            For i As Integer = 0 To GroupSIDBytes.Length - 1
                GroupSIDString.Append("\" & Hex(GroupSIDBytes(i)).PadLeft(2, "0"c))
            Next
            SearcherObject.Filter = "(objectSid=" & GroupSIDString.ToString & ")"
            Dim GroupSearchResult As SearchResult = SearcherObject.FindOne
            If Not GroupSearchResult Is Nothing Then
                Dim path As String = GroupSearchResult.Path
                Return path.Substring(path.IndexOf("=") + 1, (path.IndexOf(",") - path.IndexOf("=") - 1))
            Else
                Throw New ApplicationException("Failed to locate primary group – no results returned for the LDAP query " & SearcherObject.Filter)
            End If

        Catch ex As Exception
            Throw New ApplicationException("Error getting primary group: " & ex.Message.Trim)
        End Try
    End Function

    ''' <summary>
    ''' Déterminer si la cache est expirée, si oui, elle met la date d'expiration à jour
    ''' </summary>
    ''' <returns>True/False</returns>
    ''' <remarks></remarks>
    Private Function estCacheExpiree() As Boolean
        If DateExpirationCaches > DateTime.Now Then
            Return False

        Else
            ' Les caches sont expirées, on les vide et on met une nouvelle date d'expiration
            viderCaches()
            Dim dureeTTL As String = Nothing
            dureeTTL = XuCuConfiguration.ObtenirValeurSystemeOptionnelle("TS4", "TS4N213\TTL")
            If String.IsNullOrEmpty(dureeTTL) Then
                ' Par defaut 2 heures
                dureeTTL = "2"
            End If
            DateExpirationCaches = DateTime.Now.AddHours(CType(dureeTTL, Double))

            Return True
        End If
    End Function

    Private Sub viderCaches()
        viderCacheUtilisateurMembresDuGroupe()
        viderCacheGroupeMembresDuGroupe()
        viderCacheGroupesEtListesMembreDuGroupe()
        viderCacheUtilisateurGroupe()
    End Sub

    Private Sub viderCacheUtilisateurMembresDuGroupe()
        objLockCacheUtilisateurMembresDuGroupe.AcquireWriterLock(1000)
        Try
            CacheUtilisateurMembresDuGroupe.Clear()
        Finally
            objLockCacheUtilisateurMembresDuGroupe.ReleaseWriterLock()
        End Try
    End Sub

    Private Sub viderCacheGroupeMembresDuGroupe()
        objLockCacheGroupeMembresDuGroupe.AcquireWriterLock(1000)
        Try
            CacheGroupeMembresDuGroupe.Clear()
        Finally
            objLockCacheGroupeMembresDuGroupe.ReleaseWriterLock()
        End Try
    End Sub

    Private Sub viderCacheGroupesEtListesMembreDuGroupe()
        objLockCacheGroupesEtListesMembreDuGroupe.AcquireWriterLock(1000)
        Try
            CacheGroupesEtListesMembreDuGroupe.Clear()
        Finally
            objLockCacheGroupesEtListesMembreDuGroupe.ReleaseWriterLock()
        End Try
    End Sub

    Private Sub viderCacheUtilisateurGroupe()
        objLockCacheUtilisateurGroupe.AcquireWriterLock(1000)
        Try
            CacheUtilisateurGroupe.Clear()
        Finally
            objLockCacheUtilisateurGroupe.ReleaseWriterLock()
        End Try
    End Sub

#End Region

#Region "--- Propriétés privées ---"

    ''' <summary>
    ''' Cette propriété retourne le nom du serveur de l'active directory tel que paramétré dans le registre machine.
    ''' </summary>
    Private ReadOnly Property serveurActiveDirectory() As String
        Get
            Return XuCuPolitiqueConfig.ConfigDomaine.ValeurSysteme("General", "ServeurActiveDirectory")
        End Get
    End Property

#End Region

End Class

Friend Module FlagEnumExtensions

    <Extension>
    Public Function Contient(source As TsAadInfoUtilisateur, valeur As TsAadInfoUtilisateur) As Boolean
        Return (source And valeur) = valeur
    End Function

    <Extension>
    Public Function Contient(source As TsAadInfoGroupe, valeur As TsAadInfoGroupe) As Boolean
        Return (source And valeur) = valeur
    End Function

End Module