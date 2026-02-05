Imports Rrq.Web.GabaritsPetitsSystemes.FonctionsCommunes.NiCdParametresMsg
Imports Rrq.Web.GabaritsPetitsSystemes.FonctionsCommunes.NiTypeMessage
Imports Rrq.Web.GabaritsPetitsSystemes.Controles.NiCrPage
Imports Rrq.Web.GabaritsPetitsSystemes.ControlesBase
Imports Rrq.Web.GabaritsPetitsSystemes.Utilitaires
Imports Rrq.Securite.GestionAcces
Imports Rrq.Web.AccesUtilisateurs.Utilitaires
Imports System.Collections.Generic
Imports System.Linq

Partial Class TS7SCopierRoles
    Inherits Rrq.Web.GabaritsPetitsSystemes.Controles.NiCrPage

#Region " Code généré par le Concepteur Web Form "

    'Cet appel est requis par le Concepteur Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'REMARQUE : la déclaration d'espace réservé suivante est requise par le Concepteur Web Form.
    'Ne pas supprimer ou déplacer.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN : cet appel de méthode est requis par le Concepteur Web Form
        'Ne le modifiez pas en utilisant l'éditeur de code.
        InitializeComponent()
    End Sub

#End Region

    Protected mobjTrx As TS7I112_RAAccesUtilisateur.TSCdObjetTrx
    Protected mobjAffaire As TS7I112_RAAccesUtilisateur.TSCaAccesUtilisateur
    Public strCodeUtilisateur As String

    Public Enum enumEnvironnement
        UNITAIRE = 0
        INTEGRATION = 1
        PRODUCTION = 2
    End Enum

    Public Overrides ReadOnly Property GroupeADRequis() As String
        Get
            'Return "ROG_#ENVIRONNEMENT#_Gestion des acces demandeur"
            Return String.Concat(Rrq.InfrastructureCommune.Parametres.XuCuConfiguration.ValeurSysteme("TS7", "GroupesSecurite\GRP_ACCES_UTILISATEUR"), ";", _
                                 Rrq.InfrastructureCommune.Parametres.XuCuConfiguration.ValeurSysteme("TS7", "GroupesSecurite\GRP_RESPONSABLE_UA"), ";", _
                                 Rrq.InfrastructureCommune.Parametres.XuCuConfiguration.ValeurSysteme("TS7", "GroupesSecurite\GRP_PILOTAGE"))
        End Get
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim strExpressionTriInitial As String = Nothing
        Dim lstUnitAdminAcces As New List(Of String)
        If Not IsPostBack Then strExpressionTriInitial = "Nom ASC"

        grdListeRolesMetier.InitialiserControle("TS7I111_AccesUtilisateurCopierRoles", hypPage1, hypPage2, hypPage3, hypPage4, hypPage5, hypPage6, hypPage7, hypPage8, HyPrecedent, hySuivant, "")
        grdListeRolesTaches.InitialiserControle("TS7I111_AccesUtilisateurCopierRoles", hypPage1Taches, hypPage2Taches, hypPage3Taches, hypPage4Taches, hypPage5Taches, hypPage6Taches, hypPage7Taches, hypPage8Taches, HyPrecedentTaches, hySuivantTaches, "")

        IndAvertissementInterruption = enumAvertissementInterruption.ConditionnelAUnChangement

        mobjTrx = CType(ContexteApp.TrxCourante, TS7I112_RAAccesUtilisateur.TSCdObjetTrx)
        mobjAffaire = New TS7I112_RAAccesUtilisateur.TSCaAccesUtilisateur

        'Obtention des unités administratives selon les groupes d'acces.
        lstUnitAdminAcces = ObtenirUnitAdminADParAcces()


        'Remplir la liste des régles de cohérence au moins une fois.  Utilisée lors de la validation sur "Ajouter"
        mobjTrx.lstReglesCoherences = mobjAffaire.ObtenirRegleCoherence()


        If Not IsPostBack Then
            ForcerRafraichissement()
            If ContexteApp Is Nothing Then
                InitialiserNouvelleTrx()
            End If
           
            mobjTrx.IDRoleNonValideCoherence = String.Empty
            mobjTrx.RegleCoherenceEnErreur = String.Empty
            mobjTrx.IDRoleCauseErreur = String.Empty

            strCodeUtilisateur = Request.QueryString("CodeUtilisateur")
            If Not strCodeUtilisateur Is Nothing Then
                Dim objUtilisateurCopi As TsCdUtilisateur
                objUtilisateurCopi = New TsCdUtilisateur
                objUtilisateurCopi = TsCaServiceGestnAcces.ObtenirUtilisateur(strCodeUtilisateur)
                lblEmployeRecherche.Text = String.Concat("   Employé sélectionné: ", objUtilisateurCopi.Nom, " ", _
                                            objUtilisateurCopi.Prenom, " (", objUtilisateurCopi.ID, ")")
                mobjTrx.dtListeRoleAjout = TSCuGeneral.AssignationRoleDataTable(TsCaServiceGestnAcces.ObtenirAssignationsRole(objUtilisateurCopi.ID))

                'Retirer les rôles organisationnels
                For i As Integer = mobjTrx.dtListeRoleAjout.Rows.Count - 1 To 0 Step -1
                    If mobjTrx.dtListeRoleAjout.Rows(i).Item("Organisationnel").Equals("True") Then
                        mobjTrx.dtListeRoleAjout.Rows(i).Delete()
                    End If
                Next

                NiCuADO.AjouterDtColonne(mobjTrx.dtListeRoleAjout, "SELECT", System.Type.GetType("System.String"), 1, "N")

                'Ajouter une colonne qui contrôle la visibilité de la case à cocher
                NiCuADO.AjouterDtColonne(mobjTrx.dtListeRoleAjout, "IN_UA_DEM", System.Type.GetType("System.String"), 1, "N")

                'Obtenir tous les groupes par Acces pour le demandeur.

                'Parcourir les rôles et indiquer si le rôle fait parti de l'unité administrative du demandeur, si oui ce rôle sera sélectionnable
                For i As Integer = 0 To mobjTrx.dtListeRoleAjout.Rows.Count - 1
                    If mobjTrx.dtListeRoleAjout.Rows(i).Item("ListeUniteAdministrativeResponsable").ToString <> String.Empty Then

                        For Each UnitAdminDemandeur As String In lstUnitAdminAcces
                            If mobjTrx.dtListeRoleAjout.Rows(i).Item("ListeUniteAdministrativeResponsable").ToString.LastIndexOf(UnitAdminDemandeur) >= 0 Then
                                mobjTrx.dtListeRoleAjout.Rows(i).Item("IN_UA_DEM") = "O"
                            End If
                        Next
                        
                    End If
                Next

                mobjTrx.dtListeRoleAjout.AcceptChanges()

                If Not (mobjTrx.dtListeRoleAjout Is Nothing OrElse mobjTrx.dtListeRoleAjout.Rows.Count = 0) Then
                    Dim tabMetier As IEnumerable(Of DataRow)
                    tabMetier = mobjTrx.dtListeRoleAjout.Select("ID like 'REM_%'")
                    Dim tabTaches As IEnumerable(Of DataRow)
                    tabTaches = mobjTrx.dtListeRoleAjout.Select("ID like 'RET_%'")

                    If Not (tabMetier Is Nothing OrElse tabMetier.Count = 0) Then
                        grdListeRolesMetier.DataSource = tabMetier.CopyToDataTable
                        grdListeRolesMetier.DataBind()
                    End If

                    If Not (tabTaches Is Nothing OrElse tabTaches.Count = 0) Then
                        grdListeRolesTaches.DataSource = tabTaches.CopyToDataTable
                        grdListeRolesTaches.DataBind()
                    End If

                End If

                'Gérer les boutons et le libellé
                'Si  aucun rôle 
                '   Modifier le libellé du bouton précédent par Retour et
                '   rendre invisible Copier et ajouter
                '   rendre invisible Copier et remplacer
                '   modifier le libellé Rôle de -> pour  Aucun rôle pour 
                If mobjTrx.dtListeRoleAjout.Rows.Count = 0 Then
                    lblRoleUA.Text = String.Concat("Aucun rôle pour ", objUtilisateurCopi.Nom, " ", objUtilisateurCopi.Prenom)
                    cmdPrecedent.Text = "Retour"
                    cmdCopierAjouter.Visible = False
                    cmdCopierRemplacer.Visible = False
                    grdListeRolesMetier.Visible = False
                    grdListeRolesTaches.Visible = False
                Else
                    lblRoleUA.Text = String.Concat("Rôles de ", objUtilisateurCopi.Nom, " ", objUtilisateurCopi.Prenom)
                    grdListeRolesMetier.Visible = True
                    grdListeRolesTaches.Visible = True


                End If

            End If
        Else 'IsPostBack
            If Not (mobjTrx.dtListeRoleAjout Is Nothing OrElse mobjTrx.dtListeRoleAjout.Rows.Count = 0) Then

                Dim tabMetier As IEnumerable(Of DataRow)
                tabMetier = mobjTrx.dtListeRoleAjout.Select("ID like 'REM_%'")
                Dim tabTaches As IEnumerable(Of DataRow)
                tabTaches = mobjTrx.dtListeRoleAjout.Select("ID like 'RET_%'")
                Dim dtRoleTaches As New DataTable
                Dim dtRoleMetier As New DataTable

                If Not (tabMetier Is Nothing OrElse tabMetier.Count = 0) Then
                    dtRoleMetier = tabMetier.CopyToDataTable
                End If

                If Not (tabTaches Is Nothing OrElse tabTaches.Count = 0) Then
                    dtRoleTaches = tabTaches.CopyToDataTable
                End If

                grdListeRolesMetier.DataBind()
                grdListeRolesTaches.DataBind()

                NiCuRecupererCaseCocheGrille.RecupererSelection( _
               "chkSelection", _
               dtRoleMetier, _
               "ID", _
               "SELECT", "O", "N", _
               dtRoleMetier.DefaultView.Sort, _
               grdListeRolesMetier.CurrentPageIndex + 1, _
               grdListeRolesMetier.PageSize)

                NiCuRecupererCaseCocheGrille.RecupererSelection( _
               "chkSelection", _
               dtRoleTaches, _
               "ID", _
               "SELECT", "O", "N", _
               dtRoleTaches.DefaultView.Sort, _
               grdListeRolesTaches.CurrentPageIndex + 1, _
               grdListeRolesTaches.PageSize)

                'On remet les valeurs à jour dans mobjtrx.dtListeRoleAjout pour y avoir accès aux validations.
                For Each RoleMetier As DataRow In dtRoleMetier.Rows
                    Dim intIndex As Integer = 0
                    If NiCuADO.PointerDT(mobjTrx.dtListeRoleAjout, "ID", RoleMetier("ID").ToString, intIndex) = True Then
                        mobjTrx.dtListeRoleAjout.Rows(intIndex)("SELECT") = RoleMetier("SELECT")
                        mobjTrx.dtListeRoleAjout.AcceptChanges()
                    End If
                Next

                For Each RoleTache As DataRow In dtRoleTaches.Rows
                    Dim intIndex As Integer = 0
                    If NiCuADO.PointerDT(mobjTrx.dtListeRoleAjout, "ID", RoleTache("ID").ToString, intIndex) = True Then
                        mobjTrx.dtListeRoleAjout.Rows(intIndex)("SELECT") = RoleTache("SELECT")
                        mobjTrx.dtListeRoleAjout.AcceptChanges()
                    End If
                Next
                If Not (dtRoleMetier Is Nothing OrElse dtRoleMetier.Rows.Count = 0) Then
                    grdListeRolesMetier.DataSource = dtRoleMetier
                    grdListeRolesMetier.DataBind()
                    grdListeRolesMetier.Visible = True
                End If

                If Not (dtRoleTaches Is Nothing OrElse dtRoleTaches.Rows.Count = 0) Then
                    grdListeRolesTaches.DataSource = dtRoleTaches
                    grdListeRolesTaches.DataBind()
                    grdListeRolesTaches.Visible = True
                End If

            End If
            End If
    End Sub

    Private Sub cmdPrecedent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrecedent.Click
        Dim strCodetrans As String = Request.QueryString("codetrans")
        Response.Redirect(String.Concat(TSCuDomVal.PAGE_ROLE_EMPLOYE, "?codetrans=", strCodetrans, "&Prov=TS7SCopierRoles"))
    End Sub

    
    Public Function AfficherRole(ByVal strNomRole As String) As Boolean
        'si le role est retirer afficher barré
        Return True
    End Function

    Public Function AfficherDate(ByVal strdate As String) As String
        Return strdate
    End Function

    Public Function AfficherLienDetailRole(ByVal strNoLigne As String) As String
        Dim strRetour As String = "../TS7I111_AccesUtilisateur/"
        Return strRetour
    End Function


    Private Sub InitialiserNouvelleTrx()

        Dim strCodeUsager As String
        strCodeUsager = ContexteApp.UtilisateurCourant.CodeUtilisateur

       
    End Sub

    Public Function ObtenirInfoBulleDetail(ByVal strNomRole As String) As String
        Return String.Concat("Afficher le détail du rôle ", strNomRole)
    End Function

    Private Sub grdListeRoles_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdListeRolesMetier.ItemCommand, grdListeRolesTaches.ItemCommand
        Dim strDetailRole As String = e.CommandArgument.ToString
        Select Case e.CommandName
            Case "AfficherDetail"
                InitialiserFenComplDetailRole(strDetailRole)
        End Select
    End Sub

    Private Sub InitialiserFenComplDetailRole(ByVal strDetailRole As String)
        AfficherFenComplDetailRole("Retour", _
                               "Gérer les accès des utilisateurs", _
                               strDetailRole, _
                New TS7CdParametresFenComplDetailRole.TS7CdBouton() { _
                New TS7CdParametresFenComplDetailRole.TS7CdBouton("Retour", "Retour", True, True)})
    End Sub

    Public Sub AfficherFenComplDetailRole(ByVal strNomboutonAppelant As String, _
                                         ByVal strTitreFenMessage As String, _
                                         ByVal strDetailRole As String, _
                                         ByVal aBoutons() As TS7CdParametresFenComplDetailRole.TS7CdBouton)

        ' s'assure que le contrôle d'ouverture de dialogue est créé
        Me.EnsureChildControls()
        mobjCtrlOuvrir.URL = "../TS7I111_AccesUtilisateur/TS7FenComplDetailRole.aspx"
        mobjCtrlOuvrir.Hauteur = New Unit(350, UnitType.Pixel)
        mobjCtrlOuvrir.Largeur = New Unit(680, UnitType.Pixel)
        Dim oParam As New TS7CdParametresFenComplDetailRole

        oParam.NomBoutonAppelant = strNomboutonAppelant
        oParam.ValeurDetailRole = strDetailRole
        oParam.TitreBteMesage = strTitreFenMessage
        oParam.Bouton1 = New _
         TS7CdParametresFenComplDetailRole.TS7CdBouton(aBoutons(0).TexteBouton, aBoutons(0).Valeur, aBoutons(0).EstBoutonAnnulation, aBoutons(0).EstBoutonAnnulation)

        mobjCtrlOuvrir.OuvrirFenetreModaleAvecRetour(oParam)
    End Sub

    Private Sub hypPage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles hypPage1.Click, hypPage2.Click, hypPage3.Click, hypPage4.Click, hypPage5.Click, hypPage6.Click, hypPage7.Click, hypPage8.Click, HyPrecedent.Click, hySuivant.Click, hypPage1Taches.Click, hypPage2Taches.Click, hypPage3Taches.Click, hypPage4Taches.Click, hypPage5Taches.Click, hypPage6Taches.Click, hypPage7Taches.Click, hypPage8Taches.Click, HyPrecedentTaches.Click, hySuivantTaches.Click
        Dim lkBouton As LinkButton = CType(sender, LinkButton)

        If InStr("Taches", lkBouton.ID) > 0 Then
            grdListeRolesTaches.PageButtonClick(sender, e)
        Else
            grdListeRolesMetier.PageButtonClick(sender, e)
        End If

    End Sub

#Region "Objets du gabarit"

    Protected ReadOnly Property objBandeauPIVGabarit() As NI1I512_BandeauPIVGabarit.NI1P512_BandeauPIVGabarit
        Get
            Return CType(XlCrAffchAscxPartage1.ControleASCX, NI1I512_BandeauPIVGabarit.NI1P512_BandeauPIVGabarit)
        End Get
    End Property

    Protected ReadOnly Property objMenuGaucheGabarit() As NI1I511_MenuGaucheGabarit.NI1P511_MenuGaucheGabarit
        Get
            Return CType(XlCrAffchAscxPartage2.ControleASCX, NI1I511_MenuGaucheGabarit.NI1P511_MenuGaucheGabarit)
        End Get
    End Property

    Protected ReadOnly Property objBasPageGabarit() As NI1I513_BasPageGabarit.NI1P513_BasPageGabarit
        Get
            Return CType(XlCrAffchAscxPartage3.ControleASCX, NI1I513_BasPageGabarit.NI1P513_BasPageGabarit)
        End Get
    End Property
#End Region

    Protected Sub cmdCopierAjouter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCopierAjouter.Click

        'Réinitaliser les validations de cohérence
        mobjTrx.IDRoleNonValideCoherence = String.Empty
        mobjTrx.RegleCoherenceEnErreur = String.Empty
        mobjTrx.IDRoleCauseErreur = String.Empty

        valErreurMsg.ErrorMessage = String.Empty
        valErreurMsg.IsValid = True

        validationUnitaire()

        'Dim blnValiderCoherence As Boolean = True

        'Mis en commentaire SL 2015-02-02
        'If Page.IsValid Then
        '    blnValiderCoherence = ValiderCoherenceRole()
        'End If

        'If blnValiderCoherence Then

        'Mis en commentaire SL 2015-02-02
        'ValiderCoherenceCopierRoleAvecListeRegle()

        'If String.IsNullOrEmpty(mobjTrx.IDRoleNonValideCoherence) Then


        If Page.IsValid Then
            IndAvertissementInterruption = enumAvertissementInterruption.Oui
            If mobjTrx.dtListeAssignationRole Is Nothing OrElse mobjTrx.dtListeAssignationRole.Rows.Count = 0 Then
                mobjTrx.dtListeAssignationRole = New DataTable()
                NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "DateFin", System.Type.GetType("System.date"), 10, "", True)
                NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "Description", System.Type.GetType("System.String"), 256, "", True)
                NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "FinPrevu", System.Type.GetType("System.String"), 5, "False", False)
                NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "ID", System.Type.GetType("System.String"), 256, "", True)
                NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "Nom", System.Type.GetType("System.String"), 256, "", True)
                NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "Particulier", System.Type.GetType("System.String"), 5, "False", False)
                NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "Organisationnel", System.Type.GetType("System.String"), 5, "False", False)
                NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "StrSupprimer", System.Type.GetType("System.String"), 5, "False", False)
                NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "Action", System.Type.GetType("System.String"), 5, "", True)
                NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "ListeUniteAdministrativeResponsable", System.Type.GetType("System.String"), 4000, "", True)
                NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "Contexte", System.Type.GetType("System.String"), 256, "False", True)
                NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "LienTachesMetiers", System.Type.GetType("System.String"), 256, "False", True)
                NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "DomValContexte", System.Type.GetType("System.String"), 4000, "False", True)
                NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "ContexteOrigine", System.Type.GetType("System.String"), 256, "False", True)
                NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "StrChangerContexte", System.Type.GetType("System.String"), 5, "False", False)
                NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "NomAAfficher", System.Type.GetType("System.String"), 256, "", True)

            End If


            For Each dr As DataRow In mobjTrx.dtListeRoleAjout.Rows
                If dr.Item("SELECT").Equals("O") Then
                    'Vérifier s'il existe déjà dans l'assignation des rôles
                    Dim intNoLigne As Integer = 0
                    If NiCuADO.PointerDT(mobjTrx.dtListeAssignationRole, "ID", dr("ID").ToString, intNoLigne) = False Then
                        'If intNoLigne.Equals(-1) Then
                        Dim drAjout As DataRow
                        drAjout = mobjTrx.dtListeAssignationRole.NewRow
                        'dim dtEssai as DataTable = NiCuADO.

                        Dim RoleGenerique As New TsCdRole
                        RoleGenerique.ID = dr("ID").ToString()

                        If dr("ID").ToString.Substring(0, 4).ToUpper = "RET_C" And dr("Contexte").ToString <> String.Empty Then
                            Dim strIDRoleGenerique As String = Left(dr("ID").ToString, InStrRev(dr("ID").ToString, "_") - 1)

                            RoleGenerique = TSCuGeneral.RechercherRoleParID(strIDRoleGenerique)
                        End If

                        drAjout("ID") = RoleGenerique.ID
                        drAjout("Description") = dr("Description")
                        drAjout("Nom") = dr("Nom")
                        drAjout("Particulier") = dr("Particulier")
                        drAjout("Organisationnel") = dr("Organisationnel")
                        drAjout("Action") = "A"
                        drAjout("ListeUniteAdministrativeResponsable") = dr("ListeUniteAdministrativeResponsable")
                        drAjout("Contexte") = String.Empty
                        drAjout("LienTachesMetiers") = dr("LienTachesMetiers")
                        drAjout("DomValContexte") = dr("DomValContexte")
                        drAjout("ContexteOrigine") = String.Empty
                        drAjout("strChangerContexte") = "False"
                        drAjout("NomAAfficher") = dr("NomAAfficher")

                        mobjTrx.dtListeAssignationRole.Rows.Add(drAjout)
                        mobjTrx.blnApprobation = False 'Ajout ->approbation = false
                    Else
                        'S'il existe il ne doit pas être supprimer
                        mobjTrx.dtListeAssignationRole.Rows(intNoLigne).Item("strSupprimer") = "False"
                        mobjTrx.dtListeAssignationRole.Rows(intNoLigne).Item("strChangerContexte") = "False"
                    End If
                End If

            Next
            'Si approuvé Vérifier s'il y a eu ajout dans la liste
            If mobjTrx.blnApprobation Then mobjTrx.blnApprobation = _
                        Not mobjAffaire.ValiderSiListeAjout(mobjTrx.dtListeAssignationRole)
            'mobjTrx.blnApprobation = False
            Dim strCodetrans As String = Request.QueryString("codetrans")
            Response.Redirect(String.Concat(TSCuDomVal.PAGE_ROLE_EMPLOYE, "?codetrans=", strCodetrans, "&Prov=TS7SAjouterRoles"))
        End If
        'Mis enc ommentaire SL 2015-02-02
        'End If
        'End If

    End Sub

    Private Function validationUnitaire() As Boolean


        'Un role doit être sélectionné

        If Page.IsValid Then
            valErreurMsg.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("TS70009e", False, "Rôle")
            valErreurMsg.IsValid = False
            For Each dr As DataRow In mobjTrx.dtListeRoleAjout.Rows
                If dr.Item("SELECT").Equals("O") Then
                    valErreurMsg.IsValid = True
                    Exit For
                End If
            Next
        End If


        Return Page.IsValid
    End Function

    'Protected Sub cmdRechercherEmploye_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRechercherEmploye.Click
    '    Dim strCodetrans As String = Request.QueryString("codetrans")
    '    Response.Redirect(String.Concat(TSCuDomVal.PAGE_RECHERCHER_EMPLOYE, "?codetrans=ts7role", "&Prov=TS7SCopierRoles&Page=TS7SCopierRoles"))

    'End Sub


    Private Sub cmdCopierRemplacer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCopierRemplacer.Click
        'Réinitaliser les validations de cohérence
        mobjTrx.IDRoleNonValideCoherence = String.Empty
        mobjTrx.RegleCoherenceEnErreur = String.Empty
        mobjTrx.IDRoleCauseErreur = String.Empty

        valErreurMsg.ErrorMessage = String.Empty
        valErreurMsg.IsValid = True

        validationUnitaire()
        Dim blnValiderCoherence As Boolean = True

        If Page.IsValid Then
            '    blnValiderCoherence = ValiderCoherenceRole()
            'End If

            'If blnValiderCoherence Then

            ValiderCoherenceCopierRoleAvecListeRegle()

            If String.IsNullOrEmpty(mobjTrx.IDRoleNonValideCoherence) Then


                If Page.IsValid Then
                    IndAvertissementInterruption = enumAvertissementInterruption.Oui
                    If mobjTrx.dtListeAssignationRole Is Nothing OrElse mobjTrx.dtListeAssignationRole.Rows.Count = 0 Then
                        mobjTrx.dtListeAssignationRole = New DataTable()
                        NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "DateFin", System.Type.GetType("System.date"), 10, "", True)
                        NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "Description", System.Type.GetType("System.String"), 256, "", True)
                        NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "FinPrevu", System.Type.GetType("System.String"), 5, "False", False)
                        NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "ID", System.Type.GetType("System.String"), 256, "", True)
                        NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "Nom", System.Type.GetType("System.String"), 256, "", True)
                        NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "Particulier", System.Type.GetType("System.String"), 5, "False", False)
                        NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "Organisationnel", System.Type.GetType("System.String"), 5, "False", False)
                        NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "StrSupprimer", System.Type.GetType("System.String"), 5, "False", False)
                        NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "Action", System.Type.GetType("System.String"), 5, "", True)
                        NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "ListeUniteAdministrativeResponsable", System.Type.GetType("System.String"), 4000, "", True)
                        NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "Contexte", System.Type.GetType("System.String"), 256, "False", True)
                        NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "LienTachesMetiers", System.Type.GetType("System.String"), 256, "False", True)
                        NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "DomValContexte", System.Type.GetType("System.String"), 4000, "False", True)
                        NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "ContexteOrigine", System.Type.GetType("System.String"), 256, "False", True)
                        NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "strChangerContexte", System.Type.GetType("System.String"), 5, "False", False)
                        NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "NomAAfficher", System.Type.GetType("System.String"), 256, "", True)

                    Else
                        'Inscrire comme supprimer tous les rôles existant
                        For Each dr As DataRow In mobjTrx.dtListeAssignationRole.Rows
                            dr.Item("strSupprimer") = "True"
                        Next
                    End If

                    'Ajouter les rôles sélectionnés

                    For Each dr As DataRow In mobjTrx.dtListeRoleAjout.Rows
                        If dr.Item("SELECT").Equals("O") Then
                            'Vérifier s'il existe déjà dans l'assignation des rôles
                            Dim intNoLigne As Integer = 0
                            If NiCuADO.PointerDT(mobjTrx.dtListeAssignationRole, "ID", dr("ID").ToString, intNoLigne) = False Then
                                'If intNoLigne.Equals(-1) Then
                                Dim drAjout As DataRow
                                drAjout = mobjTrx.dtListeAssignationRole.NewRow
                                drAjout("ID") = dr("ID")
                                drAjout("Description") = dr("Description")
                                drAjout("Nom") = dr("Nom")
                                drAjout("Particulier") = dr("Particulier")
                                drAjout("Organisationnel") = dr("Organisationnel")
                                drAjout("Action") = "A"
                                drAjout("ListeUniteAdministrativeResponsable") = dr("ListeUniteAdministrativeResponsable")
                                drAjout("Contexte") = Trim(dr("Contexte").ToString)
                                drAjout("LienTachesMetiers") = dr("LienTachesMetiers")
                                drAjout("DomValContexte") = dr("DomValContexte")
                                drAjout("ContexteOrigine") = Trim(dr("ContexteOrigine").ToString)
                                drAjout("strChangerContexte") = dr("strChangerContexte")
                                drAjout("NomAAfficher") = dr("NomAAfficher")

                                mobjTrx.dtListeAssignationRole.Rows.Add(drAjout)
                                mobjTrx.blnApprobation = False 'Ajout ->approbation = false
                            Else
                                'S'il existe enlever le code supprimer
                                mobjTrx.dtListeAssignationRole.Rows(intNoLigne).Item("strSupprimer") = "False"
                                mobjTrx.dtListeAssignationRole.Rows(intNoLigne).Item("strChangerContexte") = "False"
                            End If
                        End If

                    Next
                    'Si approuvé Vérifier s'il y a eu ajout dans la liste
                    If mobjTrx.blnApprobation Then mobjTrx.blnApprobation = _
                                Not mobjAffaire.ValiderSiListeAjout(mobjTrx.dtListeAssignationRole)
                    'mobjTrx.blnApprobation = False
                    Dim strCodetrans As String = Request.QueryString("codetrans")
                    Response.Redirect(String.Concat(TSCuDomVal.PAGE_ROLE_EMPLOYE, "?codetrans=", strCodetrans, "&Prov=TS7SAjouterRoles"))
                End If
            End If
        End If
    End Sub
    Public Function ObtenirUnitAdminADParAcces() As List(Of String)
        Dim lstUnitAdmin As New List(Of String)
        Dim strRecherche As String = String.Empty
        Dim position As Integer = 0

        Dim Groupes() As String = ContexteApp.UtilisateurCourant.GroupesMembreDe()

        'strRecherche = "ROR_" & ObtenirEnvironnementStr(System.Web.HttpContext.Current) & "_TS7_GestionAcces"
        strRecherche = "ROX_" & ObtenirEnvironnementStr(System.Web.HttpContext.Current) & "_NavigSAW_TS7_GestionAcces"
        'Pour tout le tableau des groupes.  Rechercher les groupes "GestionsAcces" pour obtenir les unité administratives.

        For Each groupe As String In Groupes
            'on recherche seulement celles qui ont une UA dans le nom du groupe.
            If Not groupe.ToUpper = strRecherche.ToUpper Then
                position = InStr(groupe, strRecherche)
                If position <> 0 Then
                    lstUnitAdmin.Add(groupe.Replace(strRecherche, ""))
                End If
            End If

        Next

        Return lstUnitAdmin

    End Function
    Public Shared Function ObtenirEnvironnement(ByRef objHttpContext As System.Web.HttpContext) As enumEnvironnement

        Dim strServerName As String = objHttpContext.Request.ServerVariables("SERVER_NAME").ToLower
        If strServerName = "localhost" OrElse strServerName.Substring(0, 1).ToLower = "w" Then
            Return enumEnvironnement.UNITAIRE
        ElseIf strServerName.IndexOf("intg") <> -1 Then
            Return enumEnvironnement.INTEGRATION
        Else
            Return enumEnvironnement.PRODUCTION
        End If
    End Function
    Public Shared Function ObtenirEnvironnementStr(ByRef objHttpContext As System.Web.HttpContext) As String
        Select Case ObtenirEnvironnement(objHttpContext)
            Case enumEnvironnement.UNITAIRE
                Return "U"
            Case enumEnvironnement.INTEGRATION
                Return "I"
            Case enumEnvironnement.PRODUCTION
                Return "P"
        End Select
        Return Nothing
    End Function

    Public Function AfficherSuggestion(ByVal IDRole As String, ByVal NomGrille As String, ByVal strChaineAComparer As String, ByVal pPosition As String) As String
        If mobjTrx.IDRoleNonValideCoherence = String.Empty Then
            'Aucune erreur trouvée dans les validations
            Return String.Empty
        Else
            Return TSCuGeneral.AfficherSuggestionCoherence(IDRole, NomGrille, strChaineAComparer, pPosition, _
                                                           mobjTrx.IDRoleNonValideCoherence, mobjTrx.RegleCoherenceEnErreur, mobjTrx.lstReglesCoherences)


        End If
        'Return String.Empty

       
    End Function
    Public Sub ValiderCoherenceCopierRoleAvecListeRegle()

        Dim paramMessage() As String

        Dim dtRolesAssigne As IEnumerable(Of DataRow) = mobjTrx.dtListeAssignationRole.Select("strSupprimer = 'False'")
        Dim strRolesAssigne As String = String.Join("','", dtRolesAssigne.Select(Function(x) x("ID")).ToList)
        'Il peut y avoir des doublons dans la table car on peut avoir un role assigné dans GererRole, mais choisir le même role dans 
        'AjouterRole.
        Dim dtRolesAjout As IEnumerable(Of DataRow) = mobjTrx.dtListeRoleAjout.Select("SELECT = 'O' and ID not in('" & strRolesAssigne & "')")
        Dim dtRolesSelectionne As IEnumerable(Of DataRow) = dtRolesAssigne.Union(dtRolesAjout)

        Dim dtTableRoles As New DataTable
        dtTableRoles = dtRolesSelectionne.CopyToDataTable

        Dim lstErreur As List(Of String) = mobjAffaire.ValiderReglesCoherence(dtTableRoles, mobjTrx.lstReglesCoherences)
        If Not (lstErreur Is Nothing OrElse lstErreur.Count = 0) Then
            mobjTrx.RegleCoherenceEnErreur = lstErreur(1)
            mobjTrx.IDRoleNonValideCoherence = lstErreur(0)
            mobjTrx.IDRoleCauseErreur = lstErreur(2)
        End If

        If Not String.IsNullOrEmpty(mobjTrx.IDRoleNonValideCoherence) Then
            If mobjTrx.IDRoleCauseErreur = String.Empty Then
                paramMessage = {DirectCast(TSCuGeneral.RechercherRoleParID(mobjTrx.IDRoleNonValideCoherence), TsCdRole).Nom}
            Else
                paramMessage = {DirectCast(TSCuGeneral.RechercherRoleParID(mobjTrx.IDRoleNonValideCoherence), TsCdRole).Nom, _
                                            mobjTrx.IDRoleCauseErreur}
            End If
            Select Case mobjTrx.RegleCoherenceEnErreur
                Case "I"
                    valErreurMsg.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("TS70037E", False, paramMessage)
                    valErreurMsg.IsValid = False
                    ReafficherPageListeCopierRole()
                Case "O"
                    valErreurMsg.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("TS70038E", False, paramMessage)
                    valErreurMsg.IsValid = False
                    ReafficherPageListeCopierRole()

                Case "CM"
                    valErreurMsg.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("TS70040E", False, paramMessage)
                    valErreurMsg.IsValid = False
                    ReafficherPageListeCopierRole()

                Case "CU"
                    valErreurMsg.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("TS70039E", False, paramMessage)
                    valErreurMsg.IsValid = False
                    ReafficherPageListeCopierRole()


            End Select
        End If

    End Sub

    Public Sub ReafficherPageListeCopierRole()
        'Refaire afficher les grilles pour afficher les suggestions.
        If Not (mobjTrx.dtListeRoleAjout Is Nothing OrElse mobjTrx.dtListeRoleAjout.Rows.Count = 0) Then

            Dim tabMetier As IEnumerable(Of DataRow)
            tabMetier = mobjTrx.dtListeRoleAjout.Select("ID like 'REM_%'")
            Dim tabTaches As IEnumerable(Of DataRow)
            tabTaches = mobjTrx.dtListeRoleAjout.Select("ID like 'RET_%'")
            Dim dtRoleTaches As New DataTable
            Dim dtRoleMetier As New DataTable

            If Not (tabMetier Is Nothing OrElse tabMetier.Count = 0) Then
                dtRoleMetier = tabMetier.CopyToDataTable
            End If

            If Not (tabTaches Is Nothing OrElse tabTaches.Count = 0) Then
                dtRoleTaches = tabTaches.CopyToDataTable
            End If

            grdListeRolesMetier.BinderGrille(dtRoleMetier, 0)
            grdListeRolesTaches.BinderGrille(dtRoleTaches, 0)


        End If
    End Sub

End Class
