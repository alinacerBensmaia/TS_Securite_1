Imports Rrq.Web.GabaritsPetitsSystemes.Utilitaires
Imports Rrq.Securite.GestionAcces
Imports Rrq.Web.AccesUtilisateurs.Utilitaires

Public Class TSCdObjetTrx
    Inherits Rrq.Web.GabaritsPetitsSystemes.Controles.NICaContexteTrx


    Private mdtListeAssignationRole As DataTable
    Private mdtListeAssignationRoleConfirm As DataTable
    Private mdtListeEmployes As DataTable
    Private mdtUAUtilisateur As DataTable
    Private mdtUAUtilisateurCopie As DataTable
    Private mdtEquipUtilisateur As DataTable
    Private mdtEquipUtilisateurCopie As DataTable
    Private mdtEquipPrincUtilisateur As DataTable
    Private mdtEquipAutreUtilisateur As DataTable
    Private mdtListeUnitesAdmin As DataTable
    Private mdtListeRoleAjout As DataTable
    Private mdtListeRoleUaPrinc As DataTable
    Private mdtListeRoleUaAutr As DataTable
    Private mdtGroupesTravail As DataTable
    Private mdtListeMetier As DataTable
    Private mstrCodUtilisateurSelect As String
    Private mstrNom As String
    Private mstrPrenom As String
    Private mstrCourriel As String
    Private mstrCourrielDemande As String
    Private mstrVille As String
    Private mstrOrganisation As String
    Private mIndADMServeur As Boolean
    Private mIndAChoisiConserver As Boolean
    Private mIndADMPoste As Boolean
    Private mIndADMDevelopeur As Boolean
    Private mIndADMCentral As Boolean
    Private mIndEssaisAgent As Boolean
    Private mIndEssaisCE As Boolean
    Private mIndSoutienProdAgent As Boolean
    Private mIndSoutienProdCE As Boolean
    Private mstrCodeVille As String
    Private mstrUniteAdm As String
    Private mstrFinContrat As String
    Private mstrUaPrinc As String
    Private mstrUaPrincModifie As String
    Private mstrUaAutre As String
    Private mstrUaPrincOpt As String
    Private mstrUaAutreOpt As String
    Private mstrTypeAcces As String
    Private mstrEquip1Princ As String
    Private mstrEquip2Princ As String
    Private mstrEquip1Autre As String
    Private mstrEquip2Autre As String
    Private mstrEquip1PrincAff As String
    Private mstrEquip2PrincAff As String
    Private mstrEquip1AutreAff As String
    Private mstrEquip2AutreAff As String

    Private mstrDatEffective As String
    Private mstrTexteLibre As String
    Private mstrDatApprobation As String
    Private mstrPageRedirection As String
    Private mstrGuid As String
    Private mintNoLigneUaAutre As Integer
    Private mblnCreation As Boolean
    Private mblnApprobation As Boolean
    Private mblnAfficherValeurs As Boolean
    Private mblnAfficherCourriel As Boolean
    Private mstrAncienneUA As String
    Private mblnUAModelisee As Boolean
    Private mblnUtilisateurSage As Boolean
    'Private mstrCorrespondanceErreur As String 'Si des groupes ajoutés n'ont pas leur correspondance Metier/Taches, cette variable
    '' contient la correspondance en erreur pour surligner les suggestions"
    'Private mstrTypeCorrespondanceErreur As String 'Pour la meme validation plus haut, il fournit l'information si le role ajouté
    ''qui n'a pas de correspondance est de type "Metier" ou Taches.
    'Private mstrCorrespondanceIDRoleErreur As String 'contient l'id du role qui n'a pas de correspondance
    Private mstrValiderREO As String 'Contient tous les messages pour les roles sans REO et les REO sans roles.  Ce message
    'sera affiché seulement dans le xml généré à la fin.
    Private mdtListeAutresUA As DataTable 'Contient la liste de toutes les autres UA dont le demandeur ne fait pas partie.  Utilisé dans
    'la page des rapports.

    Private mlstReglesCoherences As New List(Of TsCdRegleCoherence)
    Private mIDRoleNonValideCoherence As String
    Private mRegleCoherenceEnErreur As String
    Private mIDRoleCauseErreur As String

    Private mFichierPieceJointe As TsCuFichierPieceJointe

    Private mblnDateEffectiveModifiable As Boolean


    Private mdtListeUADemandeur As DataTable

    Private mobjUtilisateur As TsCdUtilisateur
    Public Property IDRoleCauseErreur As String
        Get
            Return mIDRoleCauseErreur
        End Get
        Set(value As String)
            mIDRoleCauseErreur = value
        End Set
    End Property
    Public Property lstReglesCoherences As List(Of TsCdRegleCoherence)
        Get
            Return mlstReglesCoherences
        End Get
        Set(value As List(Of TsCdRegleCoherence))
            mlstReglesCoherences = value
        End Set
    End Property

    Public Property IDRoleNonValideCoherence As String
        Get
            Return mIDRoleNonValideCoherence
        End Get
        Set(value As String)
            mIDRoleNonValideCoherence = value
        End Set
    End Property

    Public Property RegleCoherenceEnErreur As String
        Get
            Return mRegleCoherenceEnErreur
        End Get
        Set(value As String)
            mRegleCoherenceEnErreur = value
        End Set
    End Property

    Public Property FichierPieceJointe As TsCuFichierPieceJointe
        Get
            Return mFichierPieceJointe
        End Get
        Set(value As TsCuFichierPieceJointe)
            mFichierPieceJointe = value
        End Set
    End Property

    Public Sub New(ByVal strCodeUsager As String)
        MyBase.New("TS7", "TS7I111_AccesUtilisateur")
    End Sub

    Public Overrides Sub InitialiserValeurDefaut()

        strUaAutre = String.Empty
        strEquip1Princ = String.Empty
        strEquip2Princ = String.Empty
        strEquip1Autre = String.Empty
        strEquip2Autre = String.Empty
        strEquip1AutreAff = String.Empty
        strEquip2AutreAff = String.Empty

        strDatApprobation = NiCuGeneral.ObtenirDateJour
        blnDateEffectiveModifiable = True

    End Sub

#Region "Gérer les rôles associés à un employé"
    Public Property blnDateEffectiveModifiable As Boolean
        Get
            Return mblnDateEffectiveModifiable
        End Get
        Set(value As Boolean)
            mblnDateEffectiveModifiable = value
        End Set
    End Property

    Public Property strValiderREO() As String
        Get
            Return mstrValiderREO
        End Get
        Set(ByVal value As String)
            mstrValiderREO = value
        End Set
    End Property

    Public Property objUtilisateur() As TsCdUtilisateur
        Get
            Return mobjUtilisateur
        End Get
        Set(ByVal Value As TsCdUtilisateur)
            mobjUtilisateur = Value
        End Set
    End Property

    Public Property blnUtilisateurSage() As Boolean
        Get
            Return mblnUtilisateurSage
        End Get
        Set(ByVal Value As Boolean)
            mblnUtilisateurSage = Value
        End Set
    End Property

    Public Property AncienneUA() As String
        Get
            Return mstrAncienneUA
        End Get
        Set(ByVal Value As String)
            mstrAncienneUA = Value
        End Set
    End Property
    Public Property blnUAModelise() As Boolean
        Get
            Return mblnUAModelisee
        End Get
        Set(ByVal Value As Boolean)
            mblnUAModelisee = Value
        End Set
    End Property

    Public Property dtListeAssignationRole() As DataTable
        Get
            Return mdtListeAssignationRole
        End Get
        Set(ByVal Value As DataTable)
            mdtListeAssignationRole = Value
        End Set
    End Property
    Public Property dtListeUADemandeur() As DataTable
        Get
            Return mdtListeUADemandeur
        End Get
        Set(ByVal Value As DataTable)
            mdtListeUADemandeur = Value
        End Set
    End Property

    Public Property dtListeAssignationRoleConfirm() As DataTable
        Get
            Return mdtListeAssignationRoleConfirm
        End Get
        Set(ByVal Value As DataTable)
            mdtListeAssignationRoleConfirm = Value
        End Set
    End Property

    Public Property dtListeAutresUA() As DataTable
        Get
            Return mdtListeAutresUA
        End Get
        Set(ByVal Value As DataTable)
            mdtListeAutresUA = Value
        End Set
    End Property

    Public Property dtListeEmployes() As DataTable
        Get
            Return mdtListeEmployes
        End Get
        Set(ByVal Value As DataTable)
            mdtListeEmployes = Value
        End Set
    End Property

    Public Property dtUAUtilisateur() As DataTable
        Get
            Return mdtUAUtilisateur
        End Get
        Set(ByVal Value As DataTable)
            mdtUAUtilisateur = Value
        End Set
    End Property

    Public Property dtUAUtilisateurCopie() As DataTable
        Get
            Return mdtUAUtilisateurCopie
        End Get
        Set(ByVal Value As DataTable)
            mdtUAUtilisateurCopie = Value
        End Set
    End Property


    Public Property dtEquipUtilisateur() As DataTable
        Get
            Return mdtEquipUtilisateur
        End Get
        Set(ByVal Value As DataTable)
            mdtEquipUtilisateur = Value
        End Set
    End Property

    Public Property dtEquipUtilisateurCopie() As DataTable
        Get
            Return mdtEquipUtilisateurCopie
        End Get
        Set(ByVal Value As DataTable)
            mdtEquipUtilisateurCopie = Value
        End Set
    End Property



    Public Property dtEquipPrincUtilisateur() As DataTable
        Get
            Return mdtEquipPrincUtilisateur
        End Get
        Set(ByVal Value As DataTable)
            mdtEquipPrincUtilisateur = Value
        End Set
    End Property

    Public Property dtEquipAutreUtilisateur() As DataTable
        Get
            Return mdtEquipAutreUtilisateur
        End Get
        Set(ByVal Value As DataTable)
            mdtEquipAutreUtilisateur = Value
        End Set
    End Property


    Public Property dtListeUnitesAdmin() As DataTable
        Get
            Return mdtListeUnitesAdmin
        End Get
        Set(ByVal Value As DataTable)
            mdtListeUnitesAdmin = Value
        End Set
    End Property

    Public Property dtListeRoleAjout() As DataTable
        Get
            Return mdtListeRoleAjout
        End Get
        Set(ByVal Value As DataTable)
            mdtListeRoleAjout = Value
        End Set
    End Property

    Public Property dtListeRoleUaPrinc() As DataTable
        Get
            Return mdtListeRoleUaPrinc
        End Get
        Set(ByVal Value As DataTable)
            mdtListeRoleUaPrinc = Value
        End Set
    End Property

    Public Property dtListeRoleUaAutr() As DataTable
        Get
            Return mdtListeRoleUaAutr
        End Get
        Set(ByVal Value As DataTable)
            mdtListeRoleUaAutr = Value
        End Set
    End Property

    Public Property dtGroupesTravail() As DataTable
        Get
            Return mdtGroupesTravail
        End Get
        Set(ByVal Value As DataTable)
            mdtGroupesTravail = Value
        End Set
    End Property

    Public Property dtListeMetier() As DataTable
        Get
            Return mdtListeMetier
        End Get
        Set(ByVal Value As DataTable)
            mdtListeMetier = Value
        End Set
    End Property

    Public Property strCodUtilisateurSelect() As String
        Get
            Return mstrCodUtilisateurSelect
        End Get
        Set(ByVal Value As String)
            mstrCodUtilisateurSelect = Value
        End Set
    End Property

    Public Property strNom() As String
        Get
            Return mstrNom
        End Get
        Set(ByVal Value As String)
            mstrNom = Value
        End Set
    End Property

    Public Property strPrenom() As String
        Get
            Return mstrPrenom
        End Get
        Set(ByVal Value As String)
            mstrPrenom = Value
        End Set
    End Property

    Public Property strCourriel() As String
        Get
            Return mstrCourriel
        End Get
        Set(ByVal Value As String)
            mstrCourriel = Value
        End Set
    End Property

    Public Property strCourrielDemande() As String
        Get
            Return mstrCourrielDemande
        End Get
        Set(ByVal Value As String)
            mstrCourrielDemande = Value
        End Set
    End Property

    Public Property strVille() As String
        Get
            Return mstrVille
        End Get
        Set(ByVal Value As String)
            mstrVille = Value
        End Set
    End Property

    Public Property strOrganisation() As String
        Get
            Return mstrOrganisation
        End Get
        Set(ByVal Value As String)
            mstrOrganisation = Value
        End Set
    End Property

    Public Property IndAChoisiConserver As Boolean
        Get
            Return mIndAChoisiConserver
        End Get
        Set(ByVal Value As Boolean)
            mIndAChoisiConserver = Value
        End Set
    End Property

    Public Property IndADMServeur As Boolean
        Get
            Return mIndADMServeur
        End Get
        Set(ByVal Value As Boolean)
            mIndADMServeur = Value
        End Set
    End Property

    Public Property IndADMPoste As Boolean
        Get
            Return mIndADMPoste
        End Get
        Set(ByVal Value As Boolean)
            mIndADMPoste = Value
        End Set
    End Property

    Public Property IndADMDevelopeur As Boolean
        Get
            Return mIndADMDevelopeur
        End Get
        Set(ByVal Value As Boolean)
            mIndADMDevelopeur = Value
        End Set
    End Property

    Public Property IndADMCentral As Boolean
        Get
            Return mIndADMCentral
        End Get
        Set(ByVal Value As Boolean)
            mIndADMCentral = Value
        End Set
    End Property

    Public Property IndEssaisAgent As Boolean
        Get
            Return mIndEssaisAgent
        End Get
        Set(ByVal Value As Boolean)
            mIndEssaisAgent = Value
        End Set
    End Property

    Public Property IndEssaisCE As Boolean
        Get
            Return mIndEssaisCE
        End Get
        Set(ByVal Value As Boolean)
            mIndEssaisCE = Value
        End Set
    End Property

    Public Property IndSoutienProdAgent As Boolean
        Get
            Return mIndSoutienProdAgent
        End Get
        Set(ByVal Value As Boolean)
            mIndSoutienProdAgent = Value
        End Set
    End Property

    Public Property IndSoutienProdCE As Boolean
        Get
            Return mIndSoutienProdCE
        End Get
        Set(ByVal Value As Boolean)
            mIndSoutienProdCE = Value
        End Set
    End Property

    Public Property strCodeVille() As String
        Get
            Return mstrCodeVille
        End Get
        Set(ByVal Value As String)
            mstrCodeVille = Value
        End Set
    End Property

    Public Property strUniteAdm() As String
        Get
            Return mstrUniteAdm
        End Get
        Set(ByVal Value As String)
            mstrUniteAdm = Value
        End Set
    End Property


    Public Property strFinContrat() As String
        Get
            Return mstrFinContrat
        End Get
        Set(ByVal Value As String)
            mstrFinContrat = Value
        End Set
    End Property



    Public Property strUaPrinc() As String
        Get
            Return mstrUaPrinc
        End Get
        Set(ByVal Value As String)
            mstrUaPrinc = Value
        End Set
    End Property

    Public Property strUaPrincModifie() As String
        Get
            Return mstrUaPrincModifie
        End Get
        Set(ByVal Value As String)
            mstrUaPrincModifie = Value
        End Set
    End Property


    Public Property strUaAutre() As String
        Get
            Return mstrUaAutre
        End Get
        Set(ByVal Value As String)
            mstrUaAutre = Value
        End Set
    End Property



    Public Property strUaPrincOpt() As String
        Get
            Return mstrUaPrincOpt
        End Get
        Set(ByVal Value As String)
            mstrUaPrincOpt = Value
        End Set
    End Property


    Public Property strUaAutreOpt() As String
        Get
            Return mstrUaAutreOpt
        End Get
        Set(ByVal Value As String)
            mstrUaAutreOpt = Value
        End Set
    End Property

    Public Property strTypeAcces() As String
        Get
            Return mstrTypeAcces
        End Get
        Set(ByVal Value As String)
            mstrTypeAcces = Value
        End Set
    End Property

    Public Property strEquip1Princ() As String
        Get
            Return mstrEquip1Princ
        End Get
        Set(ByVal Value As String)
            mstrEquip1Princ = Value
        End Set
    End Property

    Public Property strEquip2Princ() As String
        Get
            Return mstrEquip2Princ
        End Get
        Set(ByVal Value As String)
            mstrEquip2Princ = Value
        End Set
    End Property


    Public Property strEquip1PrincAff() As String
        Get
            Return mstrEquip1PrincAff
        End Get
        Set(ByVal Value As String)
            mstrEquip1PrincAff = Value
        End Set
    End Property

    Public Property strEquip2PrincAff() As String
        Get
            Return mstrEquip2PrincAff
        End Get
        Set(ByVal Value As String)
            mstrEquip2PrincAff = Value
        End Set
    End Property



    Public Property strEquip1Autre() As String
        Get
            Return mstrEquip1Autre
        End Get
        Set(ByVal Value As String)
            mstrEquip1Autre = Value
        End Set
    End Property

    Public Property strEquip2Autre() As String
        Get
            Return mstrEquip2Autre
        End Get
        Set(ByVal Value As String)
            mstrEquip2Autre = Value
        End Set
    End Property


    Public Property strEquip1AutreAff() As String
        Get
            Return mstrEquip1AutreAff
        End Get
        Set(ByVal Value As String)
            mstrEquip1AutreAff = Value
        End Set
    End Property

    Public Property strEquip2AutreAff() As String
        Get
            Return mstrEquip2AutreAff
        End Get
        Set(ByVal Value As String)
            mstrEquip2AutreAff = Value
        End Set
    End Property



    Public Property strDatEffective() As String
        Get
            Return mstrDatEffective
        End Get
        Set(ByVal Value As String)
            mstrDatEffective = Value
        End Set
    End Property

    Public Property strTexteLibre() As String
        Get
            Return mstrTexteLibre
        End Get
        Set(ByVal Value As String)
            mstrTexteLibre = Value
        End Set
    End Property

    Public Property strDatApprobation() As String
        Get
            Return mstrDatApprobation
        End Get
        Set(ByVal Value As String)
            mstrDatApprobation = Value
        End Set
    End Property

    Public Property strPageRedirection() As String
        Get
            Return mstrPageRedirection
        End Get
        Set(ByVal Value As String)
            mstrPageRedirection = Value
        End Set
    End Property

    Public Property intNoLigneUaAutre() As Integer
        Get
            Return mintNoLigneUaAutre
        End Get
        Set(ByVal Value As Integer)
            mintNoLigneUaAutre = Value
        End Set
    End Property

    Public Property blnCreation() As Boolean
        Get
            Return mblnCreation
        End Get
        Set(ByVal Value As Boolean)
            mblnCreation = Value
        End Set
    End Property


    Public Property blnApprobation() As Boolean
        Get
            Return mblnApprobation
        End Get
        Set(ByVal Value As Boolean)
            mblnApprobation = Value
        End Set
    End Property


    Public Property blnAfficherValeurs() As Boolean
        Get
            Return mblnAfficherValeurs
        End Get
        Set(ByVal Value As Boolean)
            mblnAfficherValeurs = Value
        End Set
    End Property

    Public Property blnAfficherCourriel() As Boolean
        Get
            Return mblnAfficherCourriel
        End Get
        Set(ByVal Value As Boolean)
            mblnAfficherCourriel = Value
        End Set
    End Property

    Public Property strGuid() As String
        Get
            Return mstrGuid
        End Get
        Set(ByVal Value As String)
            mstrGuid = Value
        End Set
    End Property

    Public Sub ConserverFichier(pNomFichier As String, pContenu As Byte())
        If FichierPieceJointe Is Nothing Then
            FichierPieceJointe = New TsCuFichierPieceJointe(pNomFichier, pContenu)
        Else
            FichierPieceJointe.ObtenirFichier(pNomFichier, pContenu)
        End If
    End Sub

#End Region



End Class
