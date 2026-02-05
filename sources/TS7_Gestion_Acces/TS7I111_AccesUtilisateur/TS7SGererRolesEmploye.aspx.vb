Imports Rrq.Web.GabaritsPetitsSystemes.FonctionsCommunes.NiCdParametresMsg
Imports Rrq.Web.GabaritsPetitsSystemes.FonctionsCommunes.NiTypeMessage
Imports Rrq.Web.GabaritsPetitsSystemes.Controles
Imports Rrq.Web.GabaritsPetitsSystemes.ControlesBase
Imports Rrq.Web.GabaritsPetitsSystemes.Utilitaires
Imports Rrq.Web.ServicesCommunsPetitsSystemes.Utilitaires
Imports Rrq.Securite.GestionAcces
Imports Rrq.Web.AccesUtilisateurs.Utilitaires
Imports System.Collections.Generic
Imports System.Linq
Imports System.Security.Policy.Url
Imports TS7I112_RAAccesUtilisateur


Partial Class TS7SGererRolesEmploye
    Inherits Rrq.Web.GabaritsPetitsSystemes.Controles.NiCrPage

#Region " Code généré par le Concepteur Web Form "

    'Cet appel est requis par le Concepteur Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'REMARQUE : la déclaration d'espace réservé suivante est requise par le Concepteur Web Form.
    'Ne pas supprimer ou déplacer.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DataBinding
        If Not IsPostBack Then initialiserCombos()
    End Sub

    Private Sub Page_evnGererRetourFenDlg(ByVal strNomBoutonAppelant As String, ByVal strValeurBoutonRetour As String) Handles Me.evnGererRetourFenDlg
        If strValeurBoutonRetour = "Recommencer" Then
            InitialiserNouvelleTrx()
            IndAvertissementInterruption = enumAvertissementInterruption.Non
            Response.Redirect(Session("TS7I111AccesUtilisateur").ToString)
        ElseIf strValeurBoutonRetour = "NePasConserver" Then
            mobjTrx.IndAChoisiConserver = False
            mobjTrx.SupprimerComptesSupp
            AfficherComptesSupp()
            Session("ObjTrx") = Me.mobjTrx
        End If
    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN : cet appel de méthode est requis par le Concepteur Web Form
        'Ne le modifiez pas en utilisant l'éditeur de code.
        InitializeComponent()
    End Sub

#End Region

    Protected mobjTrx As TS7I112_RAAccesUtilisateur.TSCdObjetTrx
    Protected mobjAffaire As TS7I112_RAAccesUtilisateur.TSCaAccesUtilisateur
    Public blnContexte As Boolean = False

    Private Property IndFichierEstJoint As Boolean = False

    Public Enum enumEnvironnement
        UNITAIRE = 0
        INTEGRATION = 1
        PRODUCTION = 2
    End Enum

    Public Overrides ReadOnly Property GroupeADRequis() As String
        Get
            Return String.Concat(Rrq.InfrastructureCommune.Parametres.XuCuConfiguration.ValeurSysteme("TS7", "GroupesSecurite\GRP_ACCES_UTILISATEUR"), ";",
                                 Rrq.InfrastructureCommune.Parametres.XuCuConfiguration.ValeurSysteme("TS7", "GroupesSecurite\GRP_RESPONSABLE_UA"), ";",
                                 Rrq.InfrastructureCommune.Parametres.XuCuConfiguration.ValeurSysteme("TS7", "GroupesSecurite\GRP_PILOTAGE"))
        End Get
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim strExpressionTriInitial As String = Nothing
        If Not IsPostBack Then strExpressionTriInitial = "Nom"

        'Pour reinitialiser la cache de CA RCM
        'TsCaServiceGestnAcces.RafraichirBuffer()


        IndAvertissementInterruption = enumAvertissementInterruption.ConditionnelAUnChangement

        If IsPostBack Then
            mobjTrx = CType(ContexteApp.TrxCourante, TS7I112_RAAccesUtilisateur.TSCdObjetTrx)
            gererAssignationRoles()
        End If
        grdRolesMetier.InitialiserControle("TS7I111_AccesUtilisateurRoles", hypPage1, hypPage2, hypPage3, hypPage4, hypPage5, hypPage6, hypPage7, hypPage8, HyPrecedent, hySuivant, strExpressionTriInitial)
        grdRolesTache.InitialiserControle("TS7I111_AccesUtilisateurRoles", hypPageRole1, hypPageRole2, hypPageRole3, hypPageRole4, hypPageRole5, hypPageRole6, hypPageRole7, hypPageRole8, hypPageRole9, hypPageRole10, strExpressionTriInitial)

        If IsPostBack Then
            mobjTrx = CType(ContexteApp.TrxCourante, TS7I112_RAAccesUtilisateur.TSCdObjetTrx)
            gererAssignationRoles()
        End If

        mobjAffaire = New TS7I112_RAAccesUtilisateur.TSCaAccesUtilisateur

        Dim strCodetrans As String = Request.QueryString("codetrans")

        If Not IsPostBack Then

            If Request.QueryString("Prov") Is Nothing Then
                ForcerRafraichissement()
                InitialiserNouvelleTrx()

                Dim strUsers As String = Request.QueryString("CodeUtilisateur")
                If strUsers IsNot Nothing OrElse strUsers <> String.Empty Then
                    mobjTrx.strCodUtilisateurSelect = strUsers
                End If

                mobjTrx.blnAfficherValeurs = False
                mobjTrx.IDRoleCauseErreur = String.Empty
                mobjTrx.IDRoleNonValideCoherence = String.Empty
                mobjTrx.RegleCoherenceEnErreur = String.Empty

                'Vérifier si Nouvel employé est  sélectionner
                If strCodetrans Is Nothing Then
                    mobjTrx.blnCreation = False
                Else
                    mobjTrx.blnCreation = strCodetrans.Equals("ts7nouvel")
                End If

                'Listes des Unités administratives
                mobjTrx.dtListeUnitesAdmin = TSCuGeneral.UniteAdminDataTable(TsCaServiceGestnAcces.ObtenirListeUnitesAdmin())

                RemplirUADemandeur()

                'Type=ARR&Prenom=Pascal&Nom=Bouchard&Ville=Qu%C3%A9bec%20et%20les%20r%C3%A9gions&UniteAdm=4360&FinContrat=2009-12-31
                If Not mobjTrx.blnCreation Then

                    ObtenirQueryString()
                    If mobjTrx.strTypeAcces = "ARR" Then ObtenirLabelUAPrinc()
                    If mobjTrx.strTypeAcces = "CHG" Then GererCodeCHG(strCodetrans)
                    If mobjTrx.strTypeAcces = "MOD" Then GererCodeMOD(strCodetrans)
                    If mobjTrx.strTypeAcces Is Nothing Then GererSansCode()

                Else
                    AssignerControleFocus(txtNomEmploye.ClientID)
                End If
            Else 'Request.QueryString("Prov")

                mobjTrx = CType(ContexteApp.TrxCourante, TS7I112_RAAccesUtilisateur.TSCdObjetTrx)

                mobjTrx.blnAfficherValeurs = True

                AfficherFichier()

                'Obtenir l'information passée en paramètre à partir de la page TS7SRechercherEmploye.aspx
                Dim strUser As String = Request.QueryString("CodeUtilisateur")
                If strUser IsNot Nothing Then
                    mobjTrx.strCodUtilisateurSelect = Request.QueryString("CodeUtilisateur")
                End If

                gererAssignationRoles()

                If Not mobjTrx.blnCreation And (Request.QueryString("Prov").Equals("TS7SRechercherEmploye") Or Request.QueryString("Prov").Equals("TS7SRechercherEmployeAD")) Then
                    If mobjTrx.strTypeAcces = "CHG" Then GererCodeCHG(strCodetrans)
                    If mobjTrx.strTypeAcces = "MOD" Then GererCodeMOD(strCodetrans)
                    If mobjTrx.strTypeAcces Is Nothing Then GererSansCode()
                    If Not mobjTrx.blnCreation Then
                        'Conserver IDRole de l'unité princirale
                        Dim intNoligne As Integer = -1

                        'Listes des Unités administratives
                        mobjTrx.dtListeUnitesAdmin = TSCuGeneral.UniteAdminDataTable(TsCaServiceGestnAcces.ObtenirListeUnitesAdmin())
                        'A effacer
                        RemplirUADemandeur()


                        If mobjTrx.strUaPrincModifie Is Nothing Then 'Vérifier si changement UA principal
                            NiCuADO.PointerDT(mobjTrx.dtListeUnitesAdmin, "No", mobjTrx.objUtilisateur.NoUniteAdmin, intNoligne)
                        Else
                            NiCuADO.PointerDT(mobjTrx.dtListeUnitesAdmin, "No", mobjTrx.strUaPrincModifie, intNoligne)
                        End If

                        mobjTrx.strUaPrinc = mobjTrx.dtListeUnitesAdmin.Rows(intNoligne)("IDRole").ToString
                        mobjTrx.strUaPrincOpt = String.Concat(mobjTrx.dtListeUnitesAdmin.Rows(intNoligne)("No"), "-", mobjTrx.dtListeUnitesAdmin.Rows(intNoligne)("Abbreviation"))

                        'Possibilité de deux UA - Ligne inverse de Ua Principale
                        If Not mobjTrx.dtUAUtilisateur Is Nothing AndAlso
                            mobjTrx.dtUAUtilisateur.Rows.Count > 1 Then
                            GererStrUaAutre()
                        End If
                    End If
                End If
            End If
        End If
        'initialiser la liste des régles pour valider la cohérence entre les roles.
        mobjTrx.lstReglesCoherences = mobjAffaire.ObtenirRegleCoherence()

        'Assigne la date effective recu en querystring.

        Dim strDateEffective As String = Date.Now.ToShortDateString 'Request.QueryString("DateEff")
        If Not String.IsNullOrEmpty(strDateEffective) Then
            mobjTrx.blnDateEffectiveModifiable = False
        End If

        If strDateEffective IsNot Nothing Then
            Me.mobjTrx.strDatEffective = strDateEffective
        End If

        'XlCrDatEffective.ReadOnly = Not mobjTrx.blnDateEffectiveModifiable
        'XlCrDatEffective.BoutonVisible = mobjTrx.blnDateEffectiveModifiable
        'ChangerStyleDateEffective(XlCrDatEffective)


        Me.DataBind()

        If Not IsPostBack Then
            GererAffichage()
            mobjTrx.RegleCoherenceEnErreur = String.Empty
            mobjTrx.IDRoleNonValideCoherence = String.Empty

        End If

        BinderToutesGrilles()

        'Apres le postBack du nom se positionner sur le prénom
        If IsPostBack Then
            Dim strIDClientDuChamp As String = Me.Request.Form("__EVENTTARGET")
            If strIDClientDuChamp.IndexOf("txtNomEmploye") >= 0 Then
                strIDClientDuChamp = "txtPrenomEmploye"
                NiCuGeneral.PositionnerPageEnPostback(Me, strIDClientDuChamp)
            End If

        End If

        'Conserver URL de départ à utilisr dans le bouton Recommencer
        If Session("TS7I111AccesUtilisateur") Is Nothing Then
            Session("TS7I111AccesUtilisateur") = Request.Url.ToString
        End If
    End Sub


    Private Sub ObtenirQueryString()
        mobjTrx.strTypeAcces = Request.QueryString("Type")
        mobjTrx.strPrenom = Request.QueryString("Prenom")
        mobjTrx.strNom = Request.QueryString("Nom")
        mobjTrx.AncienneUA = Request.QueryString("AncUniteAdm")


        If Not mobjTrx.strTypeAcces Is Nothing Then
            Dim strTxtNomEmploye As String = EnleverAccent(mobjTrx.strNom)
            Dim strTxtPrenomEmploye As String = EnleverAccent(mobjTrx.strPrenom)

            mobjTrx.strCourriel = Me.ObtAdresseCourrielDisponible(String.Concat(strTxtPrenomEmploye.ToLower, ".", strTxtNomEmploye.ToLower, "@rrq.gouv.qc.ca"))


        End If
        'Convertir le Code de la ville par sa description
        Dim strVilleCode As String = Request.QueryString("Ville")
        If Not strVilleCode Is Nothing Then
            Dim pos As Integer = 0
            Dim dtVille As DataTable = TSCuDomVal.obtenirVille()
            NiCuADO.PointerDT(dtVille, "VA_ELE_DON", strVilleCode, pos)

            If dtVille.Rows.Count > 0 Then
                mobjTrx.strVille = dtVille.Rows(pos).Item("DS_ELE_DON").ToString
                mobjTrx.strCodeVille = dtVille.Rows(pos).Item("VA_ELE_DON").ToString
            End If
        End If

        'Correction SL : On change l'unité admin seulement si c'est un changement et si c'est pas la même que l'unité administrative.
        ' PB : J'ai ajouté aussi sur une arrivée pour corrigé un bug.
        If mobjTrx.strTypeAcces = "CHG" Or mobjTrx.strTypeAcces = "ARR" Then
            mobjTrx.strUaPrincModifie = Request.QueryString("UniteAdm")

        End If
        mobjTrx.AncienneUA = Request.QueryString("AncUniteAdm")

        mobjTrx.strFinContrat = Request.QueryString("FinContrat")
        't208509
        mobjTrx.strGuid = Request.QueryString("Guid")
        Dim strDateEffective As String = Request.QueryString("DateEff")

        mobjTrx.blnDateEffectiveModifiable = String.IsNullOrEmpty(Request.QueryString("DateEff"))

        If strDateEffective IsNot Nothing Then
            mobjTrx.strDatEffective = strDateEffective
        End If

        'XlCrDatEffective.ReadOnly = Not mobjTrx.blnDateEffectiveModifiable
        'XlCrDatEffective.BoutonVisible = mobjTrx.blnDateEffectiveModifiable
    End Sub

    Private Sub GererCodeCHG(ByVal strCodetrans As String)

        VerifierUAModelisee()

        mobjTrx.blnUtilisateurSage = False
        Dim strprov As String = Request.QueryString("Prov")

        If mobjTrx.strCodUtilisateurSelect Is Nothing Then
            'Aucun code utilisateur de sélectionné.
            RechercherUtilisateurAD()
        End If

        'Si un utilisateur a été sélectionné dans la recherche A l'AD, on doit vérfier si il n'existe toujours pas dans sage.
        If (Not mobjTrx.strCodUtilisateurSelect Is Nothing) And mobjTrx.blnUtilisateurSage = False Then

            Dim lstUtil As New List(Of TsCdUtilisateur)
            Dim monUtil As New TsCdUtilisateur

            Try
                monUtil = TsCaServiceGestnAcces.ObtenirUtilisateur(mobjTrx.strCodUtilisateurSelect)
                mobjTrx.blnUtilisateurSage = True
                lstUtil.Add(monUtil)
                mobjTrx.dtListeEmployes = TSCuGeneral.UtilisateurDataTable(lstUtil)
            Catch ex As Exception
                mobjTrx.blnUtilisateurSage = False
            End Try

        End If

        If mobjTrx.blnUtilisateurSage Then
            ObtenirUtilisateur(mobjTrx.strCodUtilisateurSelect)
            'Inscrire L'unité administrative
            If Not mobjTrx.strUaPrincModifie Is Nothing AndAlso
                    Not mobjTrx.objUtilisateur.NoUniteAdmin.Equals(mobjTrx.strUaPrincModifie) Then
                If Not (mobjTrx.dtListeAssignationRole Is Nothing OrElse mobjTrx.dtListeAssignationRole.Rows.Count = 0) Then
                    For i As Integer = 0 To mobjTrx.dtListeAssignationRole.Rows.Count - 1

                        If Not String.IsNullOrEmpty(mobjTrx.strDatEffective) Then
                            Dim dtDatEffective As DateTime = CType(mobjTrx.strDatEffective, DateTime)
                            mobjTrx.dtListeAssignationRole.Rows(i).Item("DateFin") = dtDatEffective.AddDays(14).ToShortDateString
                        End If
                    Next
                End If

            End If

            If mobjTrx.IndComptesSuppPresent Then
                AfficherMessageConservationComptes()
                mobjTrx.IndAChoisiConserver = True
            End If

            ObtenirLabelUAPrinc() 'UA princ

        Else
            ObtenirUtilisateurAD(mobjTrx.strCodUtilisateurSelect)

        End If

    End Sub

    Private Sub GererCodeMOD(ByVal strCodetrans As String)

        If mobjTrx.strCodUtilisateurSelect Is Nothing Then
            'RechercheUtilisateur avec Nom-Prénom 
            mobjTrx.dtListeEmployes = TSCuGeneral.UtilisateurDataTable(TsCaServiceGestnAcces.RechercherUtilisateur(String.Concat(mobjTrx.strNom, " ", mobjTrx.strPrenom)))
            If mobjTrx.dtListeEmployes.Rows.Count = 1 Then
                'Conserver ID le l'employé
                mobjTrx.strCodUtilisateurSelect = mobjTrx.dtListeEmployes.Rows(0).Item("ID").ToString
            Else
                'Plus d'un utilisateur Redirect vers TS7SRechercherEmploye.aspx
                Response.Redirect(String.Concat(TSCuDomVal.PAGE_RECHERCHER_EMPLOYE, "?codetrans=ts7role", "&Prenom=", mobjTrx.strPrenom, "&Nom=", mobjTrx.strNom, "&Type=", mobjTrx.strTypeAcces))

            End If
        End If



        ObtenirUtilisateur(mobjTrx.strCodUtilisateurSelect)

        'Conserver IDRole de l'unité principale
        Dim intNoligne As Integer = 0

        If mobjTrx.strUaPrincModifie Is Nothing Then 'Vérifier si changement UA principal
            NiCuADO.PointerDT(mobjTrx.dtListeUnitesAdmin, "No", mobjTrx.objUtilisateur.NoUniteAdmin, intNoligne)
        Else
            NiCuADO.PointerDT(mobjTrx.dtListeUnitesAdmin, "No", mobjTrx.strUaPrincModifie, intNoligne)
        End If

        mobjTrx.strUaPrinc = mobjTrx.dtListeUnitesAdmin.Rows(intNoligne)("IDRole").ToString
        mobjTrx.strUaPrincOpt = String.Concat(mobjTrx.dtListeUnitesAdmin.Rows(intNoligne)("No"), "-", mobjTrx.dtListeUnitesAdmin.Rows(intNoligne)("Abbreviation"))

    End Sub
    Private Sub ObtenirLabelUAPrinc()
        Dim intNoligne As Integer = -1

        If mobjTrx.strUaPrincModifie Is Nothing Then 'Vérifier si changement UA principal
            NiCuADO.PointerDT(mobjTrx.dtListeUnitesAdmin, "No", mobjTrx.objUtilisateur.NoUniteAdmin, intNoligne)
        Else
            NiCuADO.PointerDT(mobjTrx.dtListeUnitesAdmin, "No", mobjTrx.strUaPrincModifie, intNoligne)
        End If

        If intNoligne = -1 Then
            ddwUAPrincipal.Visible = True
        Else
            mobjTrx.strUaPrinc = mobjTrx.dtListeUnitesAdmin.Rows(intNoligne)("IDRole").ToString
            mobjTrx.strUaPrincOpt = String.Concat(mobjTrx.dtListeUnitesAdmin.Rows(intNoligne)("No"), "-", mobjTrx.dtListeUnitesAdmin.Rows(intNoligne)("Abbreviation"))

        End If


    End Sub

    Private Function ObtenirLabelUA(ByVal strNoUA As String) As String
        Dim intNoligne As Integer = 0
        NiCuADO.PointerDT(mobjTrx.dtListeUnitesAdmin, "No", strNoUA, intNoligne)
        Return String.Concat(mobjTrx.dtListeUnitesAdmin.Rows(intNoligne)("No"), "-", mobjTrx.dtListeUnitesAdmin.Rows(intNoligne)("Abbreviation"))

    End Function

    Private Sub GererSansCode()
        If Not mobjTrx.blnCreation Then 'Si en ajout -> rien faire
            If mobjTrx.strCodUtilisateurSelect Is Nothing Then
                IndAvertissementInterruption = enumAvertissementInterruption.Non
                Response.Redirect(String.Concat(TSCuDomVal.PAGE_RECHERCHER_EMPLOYE, "?codetrans=ts7role"))
            Else
                ObtenirUtilisateur(mobjTrx.strCodUtilisateurSelect)
            End If
        End If
    End Sub

    Private Sub ConserverValeurs()
        If mobjTrx.blnCreation Then
            mobjTrx.strPrenom = txtPrenomEmploye.Text.Trim
            mobjTrx.strNom = txtNomEmploye.Text.Trim
            mobjTrx.strVille = ddwVille.SelectedItem.Text.Trim
            mobjTrx.strCodeVille = ddwVille.SelectedValue.Trim
            mobjTrx.strFinContrat = XlCrDateFinContrat.Text.Trim
            mobjTrx.blnAfficherCourriel = True
        End If

        ConserverFichier()
        ConserverComptesSupp()

        mobjTrx.strDatEffective = Date.Now.ToShortDateString

        mobjTrx.strTexteLibre = txtBesoinSuppl.Text

    End Sub

    Private Sub ConserverFichier()
        If fileFichierJoint.PostedFile Is Nothing OrElse IndFichierEstJoint Then Exit Sub

        If fileFichierJoint.PostedFile.ContentLength = 0 Then
            IndFichierEstJoint = False
            mobjTrx.FichierPieceJointe = Nothing
            Exit Sub
        End If

        mobjTrx.ConserverFichier(fileFichierJoint.PostedFile.FileName, fileFichierJoint.FileBytes)
        IndFichierEstJoint = True

        fileFichierJoint.Visible = False
        txtFichierJoint.Text = mobjTrx.FichierPieceJointe.NomFichier
        txtFichierJoint.ToolTip = mobjTrx.FichierPieceJointe.NomFichier
        txtFichierJoint.Visible = True
        txtFichierJoint.Enabled = False
        btnFauxParcourir.Visible = True
        btnAnnulerFichier.Enabled = True
    End Sub

    Private Sub ConserverComptesSupp()
        'mobjTrx.IndADMCentral = cbADMCentral.Checked
        mobjTrx.IndADMDevelopeur = cbADMDevelopeur.Checked
        mobjTrx.IndADMPoste = cbADMPoste.Checked
        mobjTrx.IndADMServeur = cbADMServeur.Checked
        mobjTrx.IndEssaisAgent = cbEssaisAgent.Checked
        mobjTrx.IndEssaisCE = cbEssaisCE.Checked
        mobjTrx.IndSoutienProdAgent = cbSoutienProdAgent.Checked
        mobjTrx.IndSoutienProdCE = cbSoutienProdCE.Checked
    End Sub

    Private Sub AfficherValeurs()
        txtPrenomEmploye.Text = mobjTrx.strPrenom
        txtNomEmploye.Text = mobjTrx.strNom
        ddwVille.SelectedValue = mobjTrx.strCodeVille
        XlCrDateFinContrat.Text = mobjTrx.strFinContrat
        lblValeurVille.Text = mobjTrx.strVille
        lblValeurDateFinContrat.Text = mobjTrx.strFinContrat
        ddwUAPrincipal.SelectedValue = mobjTrx.strUaPrinc
        'XlCrDatEffective.Text = mobjTrx.strDatEffective
        lblBesoinSuppl.Text = mobjTrx.strTexteLibre
        AfficherComptesSupp()

        AfficherFichier()

        mobjTrx.blnAfficherValeurs = False
    End Sub

    Private Sub AfficherFichier()
        If fileFichierJoint.FileName <> "" OrElse mobjTrx.FichierPieceJointe Is Nothing OrElse String.IsNullOrEmpty(mobjTrx.FichierPieceJointe.NomFichier) Then
            fileFichierJoint.Visible = True
            txtFichierJoint.Visible = False
            btnAnnulerFichier.Enabled = False
            btnFauxParcourir.Visible = False
        Else
            fileFichierJoint.Visible = False
            txtFichierJoint.Text = mobjTrx.FichierPieceJointe.NomFichier
            txtFichierJoint.ToolTip = mobjTrx.FichierPieceJointe.NomFichier
            txtFichierJoint.Visible = True
            txtFichierJoint.Enabled = False
            btnFauxParcourir.Visible = True
            btnAnnulerFichier.Enabled = True
        End If

    End Sub

    Private Sub AfficherComptesSupp()
        'cbADMCentral.Checked = mobjTrx.IndADMCentral
        cbADMDevelopeur.Checked = mobjTrx.IndADMDevelopeur
        cbADMPoste.Checked = mobjTrx.IndADMPoste
        cbADMServeur.Checked = mobjTrx.IndADMServeur
        cbEssaisAgent.Checked = mobjTrx.IndEssaisAgent
        cbEssaisCE.Checked = mobjTrx.IndEssaisCE
        cbSoutienProdAgent.Checked = mobjTrx.IndSoutienProdAgent
        cbSoutienProdCE.Checked = mobjTrx.IndSoutienProdCE
    End Sub

    Private Sub GererStrUaAutre()
        Dim intNoligne As Integer = 0
        NiCuADO.PointerDT(mobjTrx.dtUAUtilisateur, "No", mobjTrx.objUtilisateur.NoUniteAdmin, intNoligne)
        If intNoligne = 0 Then
            mobjTrx.strUaAutre = mobjTrx.dtUAUtilisateur.Rows(1)("IDRole").ToString
            mobjTrx.strUaAutreOpt = String.Concat(mobjTrx.dtUAUtilisateur.Rows(1)("No").ToString, "-", mobjTrx.dtUAUtilisateur.Rows(1)("Abbreviation").ToString)
        Else
            mobjTrx.strUaAutre = mobjTrx.dtUAUtilisateur.Rows(0)("IDRole").ToString
            mobjTrx.strUaAutreOpt = String.Concat(mobjTrx.dtUAUtilisateur.Rows(0)("No").ToString, "-", mobjTrx.dtUAUtilisateur.Rows(1)("Abbreviation").ToString)
        End If
    End Sub



    Public Function AfficherLienDetailRole(ByVal strNoLigne As String) As String
        Dim strRetour As String = "../TS7I111_AccesUtilisateur/"
        Return strRetour
    End Function

    Private Sub InitialiserNouvelleTrx()
        Dim strCodeUsager As String
        strCodeUsager = ContexteApp.UtilisateurCourant.CodeUtilisateur

        'On initialise une nouvelle transaction pour l'utilisateur courant
        mobjTrx = New TS7I112_RAAccesUtilisateur.TSCdObjetTrx(strCodeUsager)
        ContexteApp.TrxCourante = mobjTrx
        mobjTrx.strUaAutre = String.Empty
        mobjTrx.blnApprobation = False
        mobjTrx.strDatApprobation = NiCuGeneral.ObtenirDateJour
        mobjTrx.strDatEffective = String.Empty
    End Sub


    Public Sub initialiserCombos()
        Dim dtUAAutr As DataTable = Nothing

        ''Listes des Unités administratives
        mobjTrx.dtListeUnitesAdmin = TSCuGeneral.UniteAdminDataTable(TsCaServiceGestnAcces.ObtenirListeUnitesAdmin())

        Dim drListeUnitesAdmin As DataRow 'Ajouter la ligne "---"
        drListeUnitesAdmin = mobjTrx.dtListeUnitesAdmin.NewRow
        drListeUnitesAdmin("No") = " "
        drListeUnitesAdmin("IDRole") = " "
        drListeUnitesAdmin("NoAbbreviation") = "---"
        mobjTrx.dtListeUnitesAdmin.Rows.InsertAt(drListeUnitesAdmin, 0)

        'Unités administratives principales
        With ddwUAPrincipal
            .DataValueField = "IDRole"
            .DataTextField = "NoAbbreviation"
            .DataSource = mobjTrx.dtListeUnitesAdmin
        End With
        ddwUAPrincipal.DataBind()

        'Ville
        Dim dtVille As DataTable = TSCuDomVal.obtenirVille()

        Dim drVille As DataRow
        drVille = dtVille.NewRow
        drVille("VA_ELE_DON") = " "
        drVille("DS_ELE_DON") = "---"
        dtVille.Rows.InsertAt(drVille, 0)

        With ddwVille 'Ville
            .DataValueField = "VA_ELE_DON"
            .DataTextField = "DS_ELE_DON"
            .DataSource = dtVille
        End With
        ddwVille.DataBind()

    End Sub


    Public Sub GererDisponibiliteAffichage()
        txtNomEmploye.Visible = mobjTrx.blnCreation
        lblValeurNomEmploye.Visible = Not mobjTrx.blnCreation
        txtPrenomEmploye.Visible = mobjTrx.blnCreation
        lblValeurPrenomEmploye.Visible = Not mobjTrx.blnCreation
        XlCrDateFinContrat.Visible = mobjTrx.blnCreation
        lblValeurDateFinContrat.Visible = Not mobjTrx.blnCreation
        ddwVille.Visible = mobjTrx.blnCreation
        lblValeurVille.Visible = Not mobjTrx.blnCreation
        ddwUAPrincipal.Visible = mobjTrx.blnCreation OrElse mobjTrx.strUaPrinc Is Nothing
        lblValeurUAPrinc.Visible = Not mobjTrx.blnCreation AndAlso mobjTrx.strUaPrinc IsNot Nothing
        NiCrNomEmploye.AfficherPuce = mobjTrx.blnCreation
        NiCrPrenomEmploye.AfficherPuce = mobjTrx.blnCreation
        NiCrVille.AfficherPuce = mobjTrx.blnCreation
        'En code ARR s'il y a un problème sur le courriel -> Afficher 
        'If mobjTrx.blnAfficherCourriel Then

        'End If
    End Sub



    Public Sub GererAffichage()
        GererDisponibiliteAffichage()

        If mobjTrx.blnCreation Then
            If mobjTrx.blnAfficherValeurs = True Then AfficherValeurs()

            If ddwUAPrincipal.SelectedIndex = 0 Then
                pnlGroupRole.Visible = False
            Else
                pnlGroupRole.Visible = True
            End If


            'XlCrDatEffective.Text = mobjTrx.strDatEffective

        Else
            'Avec Code ARR si courriel en double -> pour modifier afficher txtCourrielEmploye
            'If mobjTrx.blnAfficherCourriel And Not Request.QueryString("Prov") Is Nothing Then txtCourrielEmploye.Text = mobjTrx.strCourriel

            lblValeurNomEmploye.Text = mobjTrx.strNom
            lblValeurPrenomEmploye.Text = mobjTrx.strPrenom
            lblValeurDateFinContrat.Text = mobjTrx.strFinContrat
            lblValeurVille.Text = mobjTrx.strVille
            lblValeurUAPrinc.Text = mobjTrx.strUaPrincOpt
            AfficherComptesSupp()

            'XlCrDatEffective.Text = mobjTrx.strDatEffective

            If Not mobjTrx.strTexteLibre Is Nothing Then txtBesoinSuppl.Text = mobjTrx.strTexteLibre

            'Initialiser avec --- si pas de valeur
            If lblValeurNomEmploye.Text.Equals(String.Empty) Then lblValeurNomEmploye.Text = "---"
            If lblValeurPrenomEmploye.Text.Equals(String.Empty) Then lblValeurPrenomEmploye.Text = "---"
            If lblValeurDateFinContrat.Text.Equals(String.Empty) Then lblValeurDateFinContrat.Text = "---"
            If lblValeurVille.Text.Equals(String.Empty) Then lblValeurVille.Text = "---"
            If lblValeurUAPrinc.Text.Equals(String.Empty) Then lblValeurUAPrinc.Text = "---"
        End If

        If Not mobjTrx.dtListeAssignationRole Is Nothing AndAlso
            mobjTrx.dtListeAssignationRole.Rows.Count.Equals(0) Then
            grdRolesMetier.Visible = False
            grdRolesTache.Visible = False
        End If


        BinderToutesGrilles()


    End Sub

    Public Sub ObtenirUtilisateur(ByVal strCodeUtilisateur As String)

        'Obtenir TsCdUtilisateurs
        mobjTrx.objUtilisateur = TsCaServiceGestnAcces.ObtenirUtilisateur(mobjTrx.strCodUtilisateurSelect)
        mobjTrx.strUaPrinc = mobjTrx.objUtilisateur.NoUniteAdmin
        mobjTrx.strPrenom = mobjTrx.objUtilisateur.Prenom
        mobjTrx.strNom = mobjTrx.objUtilisateur.Nom
        mobjTrx.strCourriel = mobjTrx.objUtilisateur.Courriel
        mobjTrx.strVille = mobjTrx.objUtilisateur.Ville
        mobjTrx.strOrganisation = mobjTrx.objUtilisateur.Organisation
        mobjTrx.IndADMCentral = mobjTrx.objUtilisateur.ComptesSupplementaires.IndADMCentral
        mobjTrx.IndADMDevelopeur = mobjTrx.objUtilisateur.ComptesSupplementaires.IndADMDevelopeur
        mobjTrx.IndADMPoste = mobjTrx.objUtilisateur.ComptesSupplementaires.IndADMPoste
        mobjTrx.IndADMServeur = mobjTrx.objUtilisateur.ComptesSupplementaires.IndADMServeur
        mobjTrx.IndEssaisAgent = mobjTrx.objUtilisateur.ComptesSupplementaires.IndEssaisAgent
        mobjTrx.IndEssaisCE = mobjTrx.objUtilisateur.ComptesSupplementaires.IndEssaisCE
        mobjTrx.IndSoutienProdAgent = mobjTrx.objUtilisateur.ComptesSupplementaires.IndSoutienProdAgent
        mobjTrx.IndSoutienProdCE = mobjTrx.objUtilisateur.ComptesSupplementaires.IndSoutienProdCE

        If Not mobjTrx.objUtilisateur.DateFin.Equals(Nothing) Then _
            mobjTrx.strFinContrat = mobjTrx.objUtilisateur.DateFin.ToShortDateString

        'Obtenir les UA de l'utilisateur
        mobjTrx.dtUAUtilisateur = TSCuGeneral.UniteAdminDataTable(
                TsCaServiceGestnAcces.ObtenirUnitesAdmin(mobjTrx.objUtilisateur.ID))

        If mobjTrx.dtUAUtilisateur.Rows.Count > 1 Then
            Dim intPos As Integer = 0
            NiCuADO.PointerDT(mobjTrx.dtUAUtilisateur, "No", mobjTrx.objUtilisateur.NoUniteAdmin, intPos)
            Select Case intPos
                Case 0
                    mobjTrx.strUaAutre = mobjTrx.dtUAUtilisateur.Rows(1)("IDRole").ToString
                    mobjTrx.strUaAutreOpt = String.Concat(mobjTrx.dtUAUtilisateur.Rows(1)("No").ToString, "-", mobjTrx.dtUAUtilisateur.Rows(1)("Abbreviation").ToString)

                Case 1
                    mobjTrx.strUaAutre = mobjTrx.dtUAUtilisateur.Rows(0)("IDRole").ToString
                    mobjTrx.strUaAutreOpt = String.Concat(mobjTrx.dtUAUtilisateur.Rows(0)("No").ToString, "-", mobjTrx.dtUAUtilisateur.Rows(0)("Abbreviation").ToString)

            End Select
        Else
            mobjTrx.strUaAutre = String.Empty

        End If

        'Obtenir les Equipes de l'utilisateur
        mobjTrx.dtEquipUtilisateur = TSCuGeneral.EquipDataTable(
                            TsCaServiceGestnAcces.ObtenirEquipesUtilisateur(mobjTrx.objUtilisateur.ID))

        If mobjTrx.dtEquipUtilisateur.Rows.Count > 0 Then
            mobjTrx.dtEquipPrincUtilisateur = NiCuADO.Filtrerdt(mobjTrx.dtEquipUtilisateur, "NoUniteAdmin", mobjTrx.objUtilisateur.NoUniteAdmin)

            mobjTrx.intNoLigneUaAutre = 0 'Pas Equipe Autre
            If mobjTrx.dtUAUtilisateur.Rows.Count > 1 Then
                'possibilité de deux équipes
                Dim pos As Integer = 0
                NiCuADO.PointerDT(mobjTrx.dtUAUtilisateur, "No", mobjTrx.objUtilisateur.NoUniteAdmin, pos)

                If pos = 0 And mobjTrx.dtUAUtilisateur.Rows.Count > 1 Then mobjTrx.intNoLigneUaAutre = 1 'vérifier si UA principal est en position 0
                mobjTrx.dtEquipAutreUtilisateur = NiCuADO.Filtrerdt(mobjTrx.dtEquipUtilisateur, "NoUniteAdmin", mobjTrx.dtUAUtilisateur.Rows(mobjTrx.intNoLigneUaAutre)("No").ToString)
            End If
        End If

        'Obtenir les assignations de l'utilisateur
        mobjTrx.dtListeAssignationRole = TSCuGeneral.AssignationRoleDataTable(
                        TsCaServiceGestnAcces.ObtenirAssignationsRole(mobjTrx.objUtilisateur.ID))
        'Retirer les rôles organisationnels
        If Not (mobjTrx.dtListeAssignationRole Is Nothing OrElse mobjTrx.dtListeAssignationRole.Rows.Count = 0) Then
            For i As Integer = mobjTrx.dtListeAssignationRole.Rows.Count - 1 To 0 Step -1
                If mobjTrx.dtListeAssignationRole.Rows(i).Item("Organisationnel").Equals("True") Then
                    mobjTrx.dtListeAssignationRole.Rows(i).Delete()
                End If
            Next
        End If

        'Ajouter une colonne pour gérer le retrait des roles
        NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "strSupprimer", System.Type.GetType("System.String"), 5, "False", False)
        NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "strChangerContexte", System.Type.GetType("System.String"), 5, "False", False)
        'Gerer l'ajout des Rôles
        NiCuADO.AjouterDtColonne(mobjTrx.dtListeAssignationRole, "Action", System.Type.GetType("System.String"), 5, "O", False)
        mobjTrx.dtListeAssignationRole.AcceptChanges()

        'Approbation
        mobjTrx.blnApprobation = mobjTrx.objUtilisateur.ApprobationAccepter
        If mobjTrx.blnApprobation Then
            mobjTrx.strDatApprobation = mobjTrx.objUtilisateur.DateApprobation.ToShortDateString
        Else
            mobjTrx.strDatApprobation = NiCuGeneral.ObtenirDateJour
        End If



    End Sub

    Public Sub RechercherUtilisateurAD()
        Dim UtilisateurSage As TsCdUtilisateur = Nothing 'pour la vérification dans sage.

        If mobjTrx.strCodUtilisateurSelect Is Nothing Then

            'RechercheUtilisateur avec Nom-Prénom dans L'Active directory
            mobjTrx.dtListeEmployes = NiCuRechercheAD.RechercherUtilisateursDT(NiCuRechercheAD.enumColonneAD.NomEtPrenom,
                            mobjTrx.strNom & ";" & mobjTrx.strPrenom, , False)


            If mobjTrx.dtListeEmployes.Rows.Count = 1 Then
                'Conserver ID le l'employé
                mobjTrx.strCodUtilisateurSelect = mobjTrx.dtListeEmployes.Rows(0).Item("CodeUtilisateur").ToString


            Else

                'Plus d'un utilisateur Redirect vers TS7SRechercherEmployeAD.aspx
                Response.Redirect(String.Concat("TS7SRechercherEmployeAD.aspx", "?codetrans=ts7role", "&Prenom=", mobjTrx.strPrenom, "&Nom=", mobjTrx.strNom, "&Type=", mobjTrx.strTypeAcces, "&Guid=", mobjTrx.strGuid))

            End If
        Else
            'Prov=TS7SRechercherEmployeAD
            ObtenirUtilisateurAD(mobjTrx.strCodUtilisateurSelect)
        End If



    End Sub
    Public Sub ObtenirUtilisateurAD(ByVal CodeUtilisateur As String)


        mobjTrx.dtListeEmployes = NiCuRechercheAD.RechercherUtilisateursDT(NiCuRechercheAD.enumColonneAD.CodeUtilisateur, CodeUtilisateur, NiCuRechercheAD.enumColonneAD.CodeUtilisateur, False)


        mobjTrx.strCourriel = mobjTrx.dtListeEmployes.Rows(0).Item("AdresseCourriel").ToString
        mobjTrx.strNom = mobjTrx.dtListeEmployes.Rows(0).Item("Nom").ToString
        mobjTrx.strPrenom = mobjTrx.dtListeEmployes.Rows(0).Item("Prenom").ToString
        mobjTrx.strUaPrinc = mobjTrx.strUaPrincModifie 'mobjTrx.dtListeEmployes.Rows(0).Item("NoUniteAdm").ToString
        mobjTrx.strUaPrincOpt = mobjTrx.dtListeEmployes.Rows(0).Item("NoUniteAdm").ToString
        mobjTrx.objUtilisateur = New TsCdUtilisateur()

        mobjTrx.objUtilisateur.Courriel = mobjTrx.strCourriel
        mobjTrx.objUtilisateur.Nom = mobjTrx.strNom
        mobjTrx.objUtilisateur.Prenom = mobjTrx.strPrenom
        mobjTrx.objUtilisateur.NoUniteAdmin = mobjTrx.strUaPrinc
        mobjTrx.objUtilisateur.ID = mobjTrx.strCodUtilisateurSelect

        mobjTrx.objUtilisateur.Ville = mobjTrx.strVille

        'Mettre l'unité administrative dans la table
        Dim lstUAUtilisateur As New List(Of TsCdUniteAdministrative)
        Dim UAUtilisateur As TsCdUniteAdministrative = New TsCdUniteAdministrative
        UAUtilisateur.No = mobjTrx.strUaPrinc
        lstUAUtilisateur.Insert(0, UAUtilisateur)

        mobjTrx.dtUAUtilisateur = TSCuGeneral.UniteAdminDataTable(lstUAUtilisateur)

        If mobjTrx.dtUAUtilisateur.Rows.Count > 1 Then
            Dim intPos As Integer = 0
            NiCuADO.PointerDT(mobjTrx.dtUAUtilisateur, "No", mobjTrx.objUtilisateur.NoUniteAdmin, intPos)
            Select Case intPos
                Case 0
                    mobjTrx.strUaAutre = mobjTrx.dtUAUtilisateur.Rows(1)("IDRole").ToString
                    mobjTrx.strUaAutreOpt = String.Concat(mobjTrx.dtUAUtilisateur.Rows(1)("No").ToString, "-", mobjTrx.dtUAUtilisateur.Rows(1)("Abbreviation").ToString)

                Case 1
                    mobjTrx.strUaAutre = mobjTrx.dtUAUtilisateur.Rows(0)("IDRole").ToString
                    mobjTrx.strUaAutreOpt = String.Concat(mobjTrx.dtUAUtilisateur.Rows(0)("No").ToString, "-", mobjTrx.dtUAUtilisateur.Rows(0)("Abbreviation").ToString)

            End Select
        Else
            mobjTrx.strUaAutre = String.Empty
        End If

        ObtenirLabelUAPrinc()



    End Sub

    Public Function ObtenirInfoBulleDetail(ByVal strNomRole As String) As String
        Return String.Concat("Afficher le détail du rôle ", strNomRole)
    End Function

    Public Function ObtenirInfoBulleRetirer(ByVal strNomRole As String, ByVal pTexteBouton As String) As String
        If pTexteBouton = "Retirer" Then
            Return String.Concat("Retirer le rôle ", strNomRole, " à la date effective de la demande")
        Else
            Return String.Concat("Conserver le rôle ", strNomRole, " à la date effective de la demande")
        End If

    End Function

    Public Function ObtenirFontRetirer(ByVal strSupprimer As String) As Boolean
        Return CType(strSupprimer, Boolean)
    End Function

    Public Function AfficherRetirerSupprimer(ByVal strSupprimer As String) As String
        Dim strRetour As String = "Retirer"
        If strSupprimer.Equals("True") Then
            strRetour = "Conserver"
        End If

        Return strRetour
    End Function


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

    'Protected ReadOnly Property objUAPrincipalPlusMoins() As NI1I516_PlusMoinsInfos.NI1P516_PlusMoinsInfos
    '    Get
    '        Return CType(XLCrUAPrincipal.ControleASCX, NI1I516_PlusMoinsInfos.NI1P516_PlusMoinsInfos)
    '    End Get
    'End Property

    'Protected ReadOnly Property objUAAutrPlusMoins() As NI1I516_PlusMoinsInfos.NI1P516_PlusMoinsInfos
    '    Get
    '        Return CType(XLCrUAAutr.ControleASCX, NI1I516_PlusMoinsInfos.NI1P516_PlusMoinsInfos)
    '    End Get
    'End Property

    'Protected ReadOnly Property objRolesPlusMoins() As NI1I516_PlusMoinsInfos.NI1P516_PlusMoinsInfos
    '    Get
    '        Return CType(XLCrRole.ControleASCX, NI1I516_PlusMoinsInfos.NI1P516_PlusMoinsInfos)
    '    End Get
    'End Property

#End Region

    Protected Sub txtNomEmploye_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNomEmploye.TextChanged
        Dim strTxtNomEmploye As String = EnleverAccent(txtNomEmploye.Text)
        Dim strTxtPrenomEmploye As String = EnleverAccent(txtPrenomEmploye.Text)
        NiCuGeneral.PositionnerPageEnPostback(Me)
    End Sub

    Protected Sub txtPrenomEmploye_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPrenomEmploye.TextChanged
        Dim strTxtNomEmploye As String = EnleverAccent(txtNomEmploye.Text)
        Dim strTxtPrenomEmploye As String = EnleverAccent(txtPrenomEmploye.Text)
    End Sub

    Private Function EnleverAccent(ByVal strTexte As String) As String
        'Enlever les accent pour adresse courriel
        Dim StrChaineAvecAccent As String = "àáâãäçèéêëìíîïñòóôõöùúûüýÀÁÂÃÄÇÈÉÊËÌÍÎÏÑÒÓÔÕÖÙÚÛÜÝ"
        Dim StrChaineSansAccent As String = "aaaaaceeeeiiiinooooouuuuyAAAAACEEEEIIIINOOOOOUUUUY"
        For i As Integer = 0 To StrChaineAvecAccent.Length - 1
            strTexte = strTexte.Replace(StrChaineAvecAccent.Substring(i, 1), StrChaineSansAccent.Substring(i, 1))
        Next
        Return strTexte
    End Function
    Public Sub cmdAjouterUA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAjouterUA.Click
        ConserverValeurs()
        Session("ObjTrx") = Me.mobjTrx

        Dim strCodetrans As String = Request.QueryString("codetrans")
        Response.Redirect(String.Concat(TSCuDomVal.PAGE_AJOUTER_ROLE, "?codetrans=", strCodetrans))
    End Sub

    Public Sub cmdAjouterEmploye_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAjouterEmploye.Click
        ConserverValeurs()
        Session("ObjTrx") = Me.mobjTrx
        Response.Redirect(String.Concat(TSCuDomVal.PAGE_RECHERCHER_EMPLOYE, "?codetrans=ts7role", "&Prov=TS7SGererRolesEmploye&Page=TS7SCopierRoles"))

    End Sub



    Protected Sub ddwUAPrincipal_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddwUAPrincipal.SelectedIndexChanged
        mobjTrx.strUaPrinc = ddwUAPrincipal.SelectedValue
        mobjTrx.strUaPrincOpt = ddwUAPrincipal.SelectedItem.Text



        mobjTrx.dtEquipPrincUtilisateur = Nothing

    End Sub

    Private Sub grdRoles_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdRolesMetier.ItemCommand, grdRolesTache.ItemCommand
        Dim strCodeRole As String = e.CommandArgument.ToString
        Dim strDetailRole As String = e.CommandArgument.ToString
        Dim intindiceLigne As Integer = -1

        ConserverFichier()
        ConserverComptesSupp()

        Select Case e.CommandName
            Case "AfficherDetail"
                InitialiserFenComplDetailRole(strDetailRole)

            Case "RetirerRole"
                NiCuADO.PointerDT(mobjTrx.dtListeAssignationRole, "ID", strCodeRole, intindiceLigne)
                If mobjTrx.dtListeAssignationRole.Rows(intindiceLigne).Item("strSupprimer").Equals("True") Then
                    mobjTrx.dtListeAssignationRole.Rows(intindiceLigne).Item("strSupprimer") = "False"
                    ValErreur.ErrorMessage = ""
                    ValErreur.IsValid = True
                Else
                    mobjTrx.dtListeAssignationRole.Rows(intindiceLigne).Item("strSupprimer") = "True"
                    If mobjTrx.dtListeAssignationRole.Rows(intindiceLigne).Item("datefin").ToString <> String.Empty Then
                        mobjTrx.dtListeAssignationRole.Rows(intindiceLigne).Item("datefin") = String.Empty
                        ValErreur.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("TS70033I", False)
                        ValErreur.IsValid = False

                    End If

                End If

                BinderToutesGrilles()
        End Select
    End Sub

    Public Sub grdRolesTache_Databinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdRolesTache.DataBinding
        blnContexte = False
        If Not mobjTrx.dtListeAssignationRole Is Nothing Then
            For i As Integer = 0 To mobjTrx.dtListeAssignationRole.Rows.Count - 1
                If mobjTrx.dtListeAssignationRole.Rows(i)("DomValContexte").ToString() <> String.Empty Then
                    blnContexte = True
                End If

            Next
        End If

        If blnContexte Then
            grdRolesTache.Columns(2).Visible = True
        Else
            grdRolesTache.Columns(2).Visible = False
        End If
    End Sub

    Public Sub grdRolesTache_ItemDataBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles grdRolesTache.ItemDataBound

        If blnContexte Then
            grdRolesTache.Columns(2).Visible = True
            'on a au moins un contexte, on remplit la liste de contexte avec le chamsp "DomValContexte"
            ' Sinon, on affiche le label

            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim dr As DataRowView = CType(e.Item.DataItem, DataRowView)

                Dim lblContexte As Label = CType(e.Item.Controls(2).FindControl("lblContexte"), Label)
                Dim lstContexte As DropDownList = CType(e.Item.Controls(2).FindControl("lstContexte"), DropDownList)

                If String.IsNullOrEmpty(dr("DomValContexte").ToString) Then
                    lblContexte.Visible = True
                    lstContexte.Visible = False
                Else
                    lblContexte.Visible = False
                    lstContexte.Visible = True
                    Dim lstDomVal As New List(Of String)
                    lstDomVal = dr("DomValContexte").ToString.Split(CChar(";")).ToList

                    'Enlever les lettres entre () dans les noms des domaines de valeurs
                    For i As Integer = 0 To lstDomVal.Count - 1
                        lstDomVal(i) = lstDomVal(i).Replace(Mid(lstDomVal(i), InStr(lstDomVal(i), "(")), "")
                    Next

                    'T208704 : Correction pour Guylaine.  Si seulement un choix, on ne fais pas afficher
                    ' le "Choisir".
                    If lstDomVal.Count > 1 Then
                        lstDomVal.Insert(0, "Choisir")
                    End If

                    lstContexte.DataSource = lstDomVal
                    lstContexte.DataBind()

                    If String.IsNullOrEmpty(Trim(dr("Contexte").ToString())) Then
                        lstContexte.SelectedIndex = 0
                    Else

                        'Rechercher la valeur du contexte dans la liste et le sélectionner
                        For i As Integer = 0 To lstContexte.Items.Count - 1
                            If Trim(lstContexte.Items(i).Text) = Trim(dr("Contexte").ToString()) Then
                                lstContexte.SelectedIndex = i
                            End If
                        Next
                    End If

                End If
                'Mettre la liste des contextes inactive si l'utilisateur n'est pas responsable du role.
                lstContexte.Enabled = ActiverListeContextes(dr)

            End If

        Else
            grdRolesTache.Columns(2).Visible = False
        End If

    End Sub
    Public Sub grdRolesTache_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles grdRolesTache.ItemCreated, grdRolesMetier.ItemCreated

        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dr As DataRowView = CType(e.Item.DataItem, DataRowView)

            'L'utilisateur n'est pas responsable de l'unité adm dont le role appartient.
            If Not mobjTrx Is Nothing AndAlso
                Not TSCuGeneral.boolResponsableRole(dr("ListeUniteAdministrativeResponsable").ToString, mobjTrx.dtListeUADemandeur) Then

                'enlever le lien "Retirer"
                Dim LiensSupprimerMetier As LinkButton = Nothing
                LiensSupprimerMetier = CType(e.Item.FindControl("LnkRetirer"), LinkButton)
                If Not LiensSupprimerMetier Is Nothing Then
                    LiensSupprimerMetier.Visible = False
                End If

                'Désactiver le calendrier
                Dim txtDateExp As New XlCrDate
                txtDateExp = CType(e.Item.FindControl("txtDatExpRole"), XlCrDate)
                If Not txtDateExp Is Nothing Then
                    txtDateExp.ReadOnly = True
                    txtDateExp.BoutonVisible = False
                    txtDateExp.Enabled = False
                End If

            End If

            'ROLES AVEC CONTEXTES - seulement les roles de taches.
            Dim grd As NiCuGrillePageTrie = CType(sender, NiCuGrillePageTrie)

            If blnContexte And grd.ID = "grdRolesTache" Then
                grdRolesTache.Columns(2).Visible = True
                'on a au moins un contexte, on remplit la liste de contexte avec le chamsp "DomValContexte"
                ' Sinon, on affiche le label
                Dim lblContexte As Label = CType(e.Item.Controls(2).FindControl("lblContexte"), Label)
                Dim lstContexte As DropDownList = CType(e.Item.Controls(2).FindControl("lstContexte"), DropDownList)

                If String.IsNullOrEmpty(dr("DomValContexte").ToString) Then
                    lblContexte.Visible = True
                    lstContexte.Visible = False
                Else
                    lblContexte.Visible = False
                    lstContexte.Visible = True
                    Dim lstDomVal As New List(Of String)
                    lstDomVal = dr("DomValContexte").ToString.Split(CChar(";")).ToList

                    'Enlever les lettres entre () dans les noms des domaines de valeurs
                    For i As Integer = 0 To lstDomVal.Count - 1
                        lstDomVal(i) = lstDomVal(i).Replace(Mid(lstDomVal(i), InStr(lstDomVal(i), "(")), "")
                    Next

                    'Si seulement un choix, on ne fais pas afficher le "Choisir".
                    If lstDomVal.Count > 1 Then
                        lstDomVal.Insert(0, "Choisir")
                    End If

                    lstContexte.DataSource = lstDomVal
                    lstContexte.DataBind()

                    If String.IsNullOrEmpty(Trim(dr("Contexte").ToString())) Then
                        lstContexte.SelectedIndex = 0
                    Else
                        'Rechercher la valeur du contexte dans la liste et le sélectionner
                        For i As Integer = 0 To lstContexte.Items.Count - 1
                            If Trim(lstContexte.Items(i).Text) = Trim(dr("Contexte").ToString()) Then
                                lstContexte.SelectedIndex = i
                            End If
                        Next
                    End If
                End If

                'Mettre la liste des contextes inactive si l'utilisateur n'est pas responsable du role.
                lstContexte.Enabled = TSCuGeneral.boolResponsableRole(dr("ListeUniteAdministrativeResponsable").ToString, mobjTrx.dtListeUADemandeur)
                'ActiverListeContextes(dr)

            End If
        End If
    End Sub


    Private Sub hypPage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles hypPage1.Click, hypPage2.Click, hypPage3.Click, hypPage4.Click, hypPage5.Click, hypPage6.Click, hypPage7.Click, hypPage8.Click, HyPrecedent.Click, hySuivant.Click
        grdRolesMetier.PageButtonClick(sender, e)
    End Sub

    Protected Sub cmdSuivant_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSuivant.Click

        'Réinitaliser les validations de cohérence
        mobjTrx.IDRoleCauseErreur = String.Empty
        mobjTrx.IDRoleNonValideCoherence = String.Empty
        mobjTrx.RegleCoherenceEnErreur = String.Empty

        ValErreur.ErrorMessage = String.Empty
        ValErreur.IsValid = True

        ValidationUnitaire()

        ConserverValeurs()


        If Page.IsValid Then

            'Enregistrement d'un nouvel utilisateur identique au code ARR
            If mobjTrx.blnCreation Then
                mobjTrx.strTypeAcces = "ARR"
            End If

            'S'il y a des assignation dans la grille gérer les dates
            If grdRolesMetier.Items.Count > 0 Or grdRolesTache.Items.Count > 0 Then
                gererAssignationRoles()
            End If


            'Si pas de changement dans les roles, il y doit y avoir une remarque.  
            If Not (mobjTrx.strTypeAcces = "ARR" Or (mobjTrx.strTypeAcces = "CHG" And mobjTrx.blnUtilisateurSage = False)) Then
                If Not GererChangement() Then
                    If mobjTrx.strTexteLibre.Equals(String.Empty) Then
                        ValErreur.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("TS70017I", False)
                        ValErreur.IsValid = False
                    End If
                End If
            End If

            ' Cette validation n'a lieu uniquement si il n'existe aucun fichier.
            If (mobjTrx.FichierPieceJointe Is Nothing OrElse String.IsNullOrEmpty(mobjTrx.FichierPieceJointe.NomFichier)) Then
                'Si tous les roles sont supprimés et pas de remarque, afficher un message.
                If mobjTrx.dtListeAssignationRole Is Nothing _
            OrElse mobjTrx.dtListeAssignationRole.Rows.Count = 0 _
            OrElse mobjTrx.dtListeAssignationRole.Select("strSupprimer = 'False'").Length = 0 Then
                    If mobjTrx.strTexteLibre.Equals(String.Empty) Then
                        ValErreur.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("TS70022E", False)
                        ValErreur.IsValid = False
                    End If
                Else
                    'Si tous les roles RESTANTS ont une date d'expiration et pas de remarque, afficher un message
                    If mobjTrx.dtListeAssignationRole.Select("datefin = '' and strSupprimer = 'False'").Length = 0 Then
                        If mobjTrx.strTexteLibre.Equals(String.Empty) Then
                            ValErreur.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("TS70036E", False)
                            ValErreur.IsValid = False
                        End If
                    End If
                End If
            End If

            If Page.IsValid Then

                Session("ObjTrx") = Me.mobjTrx

                If mobjTrx.blnCreation Then
                    Response.Redirect(String.Concat(TSCuDomVal.PAGE_CONFIRMATION_VALEURS, "?codetrans=ts7nouvel", "&Action=Ajout"))
                ElseIf Not mobjTrx.strTypeAcces Is Nothing AndAlso mobjTrx.strTypeAcces.Equals("ARR") Then
                    Response.Redirect(String.Concat(TSCuDomVal.PAGE_CONFIRMATION_VALEURS, "?codetrans=ts7role", "&Action=Ajout"))
                Else
                    Response.Redirect(String.Concat(TSCuDomVal.PAGE_CONFIRMATION_VALEURS, "?codetrans=ts7role", "&Action=Modification"))
                End If
            End If
        End If
    End Sub

    Private Function GererChangement() As Boolean
        Dim blnRetour As Boolean = False
        Dim pos As Integer

        'Gérer les équipes
        'Code   S pour suppression
        '       C pour conserver
        '       A pour Ajouter
        mobjTrx.dtUAUtilisateurCopie = mobjTrx.dtUAUtilisateur.Copy
        NiCuADO.AjouterDtColonne(mobjTrx.dtUAUtilisateurCopie, "Action", System.Type.GetType("System.String"), 5, "S", False)
        mobjTrx.dtUAUtilisateurCopie.AcceptChanges()
        If NiCuADO.PointerDT(mobjTrx.dtUAUtilisateurCopie, "IDRole", mobjTrx.strUaPrinc, pos) = True Then
            mobjTrx.dtUAUtilisateurCopie.Rows(pos).Item("Action") = "C" 'Conserver
        Else
            Dim drAjout As DataRow
            drAjout = mobjTrx.dtUAUtilisateurCopie.NewRow
            drAjout("Action") = "A"
            drAjout("IDRole") = mobjTrx.strUaPrinc
            mobjTrx.dtUAUtilisateurCopie.Rows.Add(drAjout)


        End If

        If Not mobjTrx.strUaAutre Is Nothing AndAlso Not (mobjTrx.strUaAutre.Equals("---") Or mobjTrx.strUaAutre.Trim.Equals(String.Empty)) Then
            If NiCuADO.PointerDT(mobjTrx.dtUAUtilisateurCopie, "IDRole", mobjTrx.strUaAutre, pos) = True Then
                mobjTrx.dtUAUtilisateurCopie.Rows(pos).Item("Action") = "C" 'Conserver
            Else
                Dim drAjout As DataRow
                drAjout = mobjTrx.dtUAUtilisateurCopie.NewRow
                drAjout("Action") = "A"
                drAjout("IDRole") = mobjTrx.strUaAutre
                mobjTrx.dtUAUtilisateurCopie.Rows.Add(drAjout)
            End If
        End If

        'Gérer les équipes
        mobjTrx.dtEquipUtilisateurCopie = mobjTrx.dtEquipUtilisateur.Copy
        NiCuADO.AjouterDtColonne(mobjTrx.dtEquipUtilisateurCopie, "Action", System.Type.GetType("System.String"), 5, "S", False)
        mobjTrx.dtEquipUtilisateurCopie.AcceptChanges()
        If Not mobjTrx.strEquip1Princ.Trim.Equals(String.Empty) Then GererEquip(mobjTrx.strEquip1Princ)
        If Not mobjTrx.strEquip2Princ.Trim.Equals(String.Empty) Then GererEquip(mobjTrx.strEquip2Princ)
        If Not mobjTrx.strEquip1Autre.Trim.Equals(String.Empty) Then GererEquip(mobjTrx.strEquip1Autre)
        If Not mobjTrx.strEquip2Autre.Trim.Equals(String.Empty) Then GererEquip(mobjTrx.strEquip2Autre)

        'Modification du UA princ de l'employé
        If Not mobjTrx.strUaPrincModifie Is Nothing AndAlso
                Not mobjTrx.objUtilisateur.NoUniteAdmin.Trim.Equals(mobjTrx.strUaPrincModifie.Trim) Then
            blnRetour = True
        End If

        'Vérifier s'il y a eu modif
        If blnRetour = False Then
            For Each dr As DataRow In mobjTrx.dtUAUtilisateurCopie.Rows
                If Not dr("Action").Equals("C") Then
                    blnRetour = True
                    Exit For
                End If
            Next
        End If

        If blnRetour = False Then
            For Each dr As DataRow In mobjTrx.dtEquipUtilisateurCopie.Rows
                If Not dr("Action").Equals("C") Then
                    blnRetour = True
                    Exit For
                End If
            Next
        End If


        If blnRetour = False Then
            'Gerer Assignations
            For Each dr As DataRow In mobjTrx.dtListeAssignationRole.Rows
                If dr("Action").Equals("T") Then
                    blnRetour = True
                    Exit For

                ElseIf dr("Action").Equals("O") And dr("strSupprimer").Equals("True") Then
                    blnRetour = True
                    Exit For
                ElseIf dr("Action").Equals("O") And dr("strSupprimer").Equals("False") And Not dr("DateFin").Equals(String.Empty) Then
                    blnRetour = True
                    Exit For
                ElseIf dr("Action").Equals("A") And dr("strSupprimer").Equals("False") And Not dr("DateFin").Equals(String.Empty) Then
                    blnRetour = True
                    Exit For
                ElseIf dr("Action").Equals("A") And dr("strSupprimer").Equals("False") And dr("DateFin").Equals(String.Empty) Then
                    blnRetour = True
                    Exit For
                ElseIf Trim(dr("ContexteOrigine").ToString.ToUpper) <> Trim(dr("Contexte").ToString.ToUpper) Then
                    blnRetour = True
                    Exit For
                End If
            Next
        End If

        ConserverFichier()
        ConserverComptesSupp()

        If Not (mobjTrx.FichierPieceJointe Is Nothing OrElse String.IsNullOrEmpty(mobjTrx.FichierPieceJointe.NomFichier)) Then blnRetour = True

        If mobjTrx.IndComptesSuppModifie Then blnRetour = True

        If Not mobjTrx.objUtilisateur.ApprobationAccepter.Equals(mobjTrx.blnApprobation) Then blnRetour = True

        If Not mobjTrx.strTexteLibre.Equals(String.Empty) Then blnRetour = True

        Return blnRetour
    End Function

    Private Sub GererEquip(ByVal strIDRole As String)
        Dim pos As Integer
        If NiCuADO.PointerDT(mobjTrx.dtEquipUtilisateurCopie, "IDRole", strIDRole, pos) = True Then
            mobjTrx.dtEquipUtilisateurCopie.Rows(pos).Item("Action") = "C" 'Conserver
        Else
            Dim drAjout As DataRow
            drAjout = mobjTrx.dtEquipUtilisateurCopie.NewRow
            drAjout("Action") = "A"
            drAjout("IDRole") = strIDRole
            mobjTrx.dtEquipUtilisateurCopie.Rows.Add(drAjout)
        End If
    End Sub

    Public Function ValidationUnitaire() As Boolean
        valDateValidMetier.ErrorMessage = Nothing
        valNomRequis.ErrorMessage = Nothing
        valPrenomRequis.ErrorMessage = Nothing
        valVilleRequis.ErrorMessage = Nothing
        ValDatFinContrat.ErrorMessage = Nothing
        valUAPrincipal.ErrorMessage = Nothing
        csValidateRolesTaches.ErrorMessage = Nothing
        ValErreur.ErrorMessage = Nothing

        If mobjTrx.blnCreation Then
            'nom
            If txtNomEmploye.Text.Length = 0 Then
                valNomRequis.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("TS70027E", False, "Nom")
                valNomRequis.IsValid = False
            End If
            'prénom
            If txtPrenomEmploye.Text.Length = 0 Then
                valPrenomRequis.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("TS70027E", False, "Prénom")
                valPrenomRequis.IsValid = False
            End If

            'Ville
            If mobjTrx.blnCreation And ddwVille.SelectedIndex = 0 Then
                valVilleRequis.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("TS70027E", False, "Ville")
                valVilleRequis.IsValid = False
            End If

            'Date de fin de contrat - vide -> OK
            'Date de fin de contrat - valider le format 
            'Date de fin de contrat > date du jour
            If XlCrDateFinContrat.Text.Trim.Equals(String.Empty) Then

            ElseIf Not NiCuGeneral.EstDateValide(XlCrDateFinContrat.Text) Then
                ValDatFinContrat.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("NI10009E", False, "Fin contrat")
                ValDatFinContrat.IsValid = False
            ElseIf DateDiff("d", CType(XlCrDateFinContrat.Text, Date), Today) >= 0 Then
                ValDatFinContrat.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("TS70010E", False)
                ValDatFinContrat.IsValid = False
            End If
        End If

        'courriel
        If mobjTrx.blnCreation OrElse
            (Not mobjTrx.strTypeAcces Is Nothing AndAlso mobjTrx.strTypeAcces.Equals("ARR")) Then

            Dim intPosNom As Integer
            Dim intPosPrenom As Integer
            Dim intPosFin As Integer
            Dim intPosPoint As Integer
            Dim strNomEmploye As String
            Dim strPrenomEmploye As String
            If mobjTrx.blnCreation Then
                strNomEmploye = EnleverAccent(txtNomEmploye.Text)
                strPrenomEmploye = EnleverAccent(txtPrenomEmploye.Text)
            Else
                strNomEmploye = EnleverAccent(lblValeurNomEmploye.Text)
                strPrenomEmploye = EnleverAccent(lblValeurPrenomEmploye.Text)
            End If

            'Adresse -> Prenom.nom "@rrq,gouv.qc.ca"
            If (intPosPrenom > intPosNom) Or (intPosPoint > intPosNom) Or (intPosFin = -1) Or
                (intPosNom = -1) Or (intPosPrenom = -1) Then

            End If
        End If

        'Ua principal
        If mobjTrx.blnCreation And ddwUAPrincipal.SelectedIndex = 0 Then
            valUAPrincipal.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("TS70027E", False, "Unité administrative principale")
            valUAPrincipal.IsValid = False
        End If

        'valDatEffectiv
        'Date effective de la demande  -> Obligatoire ("TS70027E")
        'Date effective de la demande - valider le format ("NI10009E") 
        'Date effective de la demande > date du jour (TS70020E)
        'If XlCrDatEffective.Text.Trim.Equals(String.Empty) Then
        '    valDatEffectiv.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("TS70027E", False, "Date effective de la demande")
        '    valDatEffectiv.IsValid = False
        'ElseIf Not NiCuGeneral.EstDateValide(XlCrDatEffective.Text.Trim) Then
        '    valDatEffectiv.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("NI10009E", False, "Date effective de la demande")
        '    valDatEffectiv.IsValid = False
        'ElseIf DateDiff("d", CType(XlCrDatEffective.Text.Trim, Date), Today) > 0 Then
        '    valDatEffectiv.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("TS70020E", False)
        '    valDatEffectiv.IsValid = False
        'End If


        'Vérifier le format des dates
        'If valDatEffectiv.IsValid Then
        'Dim blnDatePlusPetite As Boolean = False
        For i As Integer = 0 To grdRolesMetier.Items.Count - 1
            Dim dateExpRole As XlCrDate = CType(grdRolesMetier.Items(i).FindControl("txtDatExpRole"), XlCrDate)
            If Not dateExpRole Is Nothing AndAlso dateExpRole.Text.Length > 0 Then
                If Not NiCuGeneral.EstDateValide(dateExpRole.Text) Then
                    valDateValidMetier.ErrorMessage &= Me.ContexteApp.ObtenirMessageFormate("NI10009E", False, grdRolesMetier.Items(i).Cells(0).Text) & "<br />"
                    valDateValidMetier.IsValid = False

                    'ElseIf DateDiff("d", CType(dateExpRole.Text, Date), CType(XlCrDatEffective.Text.Trim, Date)) >= 0 Then
                    '    blnDatePlusPetite = True
                End If
            End If
        Next

        'If blnDatePlusPetite Then
        '    valDateValidMetier.ErrorMessage &= Me.ContexteApp.ObtenirMessageFormate("TS70015E", False, "") & "<br />"
        '    valDateValidMetier.IsValid = False
        'End If
        'blnDatePlusPetite = False
        For i As Integer = 0 To grdRolesTache.Items.Count - 1
            Dim dateExpRole As XlCrDate = CType(grdRolesTache.Items(i).FindControl("txtDatExpRole"), XlCrDate)
            If Not dateExpRole Is Nothing AndAlso dateExpRole.Text.Length > 0 Then
                If Not NiCuGeneral.EstDateValide(dateExpRole.Text) Then
                    csValidateRolesTaches.ErrorMessage &= Me.ContexteApp.ObtenirMessageFormate("NI10009E", False, grdRolesTache.Items(i).Cells(0).Text) & "<br />"
                    csValidateRolesTaches.IsValid = False

                    'ElseIf DateDiff("d", CType(dateExpRole.Text, Date), CType(XlCrDatEffective.Text.Trim, Date)) >= 0 Then
                    '    blnDatePlusPetite = True
                End If
            End If
        Next
        'If blnDatePlusPetite Then
        '    csValidateRolesTaches.ErrorMessage &= Me.ContexteApp.ObtenirMessageFormate("TS70015E", False, "") & "<br />"
        '    csValidateRolesTaches.IsValid = False
        'End If


        'End If

        If Page.IsValid Then
            If Not (mobjTrx.dtListeAssignationRole Is Nothing OrElse mobjTrx.dtListeAssignationRole.Rows.Count = 0) Then
                'Valider si tous les roles avec contexte ont un contexte
                For i As Integer = 0 To mobjTrx.dtListeAssignationRole.Rows.Count - 1
                    If Not (String.IsNullOrEmpty(mobjTrx.dtListeAssignationRole.Rows(i)("DomValContexte").ToString()) Or
                            mobjTrx.dtListeAssignationRole.Rows(i)("strSupprimer").ToString = "True") Then
                        If Trim(mobjTrx.dtListeAssignationRole.Rows(i)("Contexte").ToString) = String.Empty Then
                            csValidateRolesTaches.ErrorMessage &= Me.ContexteApp.ObtenirMessageFormate("TS70027E", False, "contexte du rôle " & mobjTrx.dtListeAssignationRole.Rows(i)("Nom").ToString())
                            csValidateRolesTaches.IsValid = False
                        End If

                    End If

                Next
            End If

        End If

        If Page.IsValid Then
            'Les deux Unités administratives doivent être différentes
            If mobjTrx.strUaPrinc.Equals(mobjTrx.strUaAutre) Then
                ValErreur.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("TS70003E", False)
                ValErreur.IsValid = False
            End If
        End If

        'Vérifier si le courriel existe déjà dans AD
        ' Exemple d'une recherche par code utilisateur
        If Page.IsValid Then

            mobjTrx.strCourrielDemande = String.Empty

            If mobjTrx.blnCreation Or
                 (Not mobjTrx.strTypeAcces Is Nothing AndAlso mobjTrx.strTypeAcces.Equals("ARR")) Then
                Dim strNouvellesAdr As String = ObtAdresseCourrielDisponible(mobjTrx.strCourriel)

                If strNouvellesAdr <> mobjTrx.strCourriel Then

                    'Conservation du courriel d'origine pour la page suivante.
                    mobjTrx.strCourriel = strNouvellesAdr

                    'Si code ARR Autoriser la modification -> affivher le texbox
                    If Not mobjTrx.strTypeAcces Is Nothing AndAlso mobjTrx.strTypeAcces.Equals("ARR") Then
                        mobjTrx.blnAfficherCourriel = True
                    End If
                End If
            End If
        End If

        'Valider les cohérences Metier/Taches
        If Page.IsValid Then
            csValidateRolesTaches.ErrorMessage = Nothing
            Dim blnCorrespondanceValide As Boolean = True
            Dim strNomEnErreur As String = String.Empty

            Dim dtRolesAssigne As IEnumerable(Of DataRow) = Nothing

            'Regles de cohérences
            If mobjTrx.dtListeAssignationRole IsNot Nothing Then
                dtRolesAssigne = mobjTrx.dtListeAssignationRole.Select("strSupprimer = 'False'")
            End If

            Dim dtTableRoles As New DataTable

            If dtRolesAssigne IsNot Nothing AndAlso dtRolesAssigne.Count > 0 Then
                dtTableRoles = dtRolesAssigne.CopyToDataTable

                Dim lstErreur As List(Of String) = mobjAffaire.ValiderReglesCoherence(dtTableRoles, mobjTrx.lstReglesCoherences)
                If Not (lstErreur Is Nothing OrElse lstErreur.Count = 0) Then
                    mobjTrx.RegleCoherenceEnErreur = lstErreur(1)
                    mobjTrx.IDRoleNonValideCoherence = lstErreur(0)
                    mobjTrx.IDRoleCauseErreur = lstErreur(2)
                End If
                Dim paramMessage() As String
                If Not String.IsNullOrEmpty(mobjTrx.IDRoleNonValideCoherence) Then
                    If mobjTrx.IDRoleCauseErreur = String.Empty Then
                        paramMessage = {DirectCast(TSCuGeneral.RechercherRoleParID(mobjTrx.IDRoleNonValideCoherence), TsCdRole).Nom}
                    Else
                        paramMessage = {DirectCast(TSCuGeneral.RechercherRoleParID(mobjTrx.IDRoleNonValideCoherence), TsCdRole).Nom,
                                                    mobjTrx.IDRoleCauseErreur}
                    End If

                    Select Case mobjTrx.RegleCoherenceEnErreur
                        Case "I"
                            ValErreur.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("TS70037E", False, paramMessage)
                            ValErreur.IsValid = False
                            BinderToutesGrilles()
                        Case "O"
                            ValErreur.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("TS70038E", False, paramMessage)
                            ValErreur.IsValid = False
                            BinderToutesGrilles()

                        Case "CM"
                            ValErreur.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("TS70040E", False, paramMessage)
                            ValErreur.IsValid = False
                            BinderToutesGrilles()

                        Case "CU"
                            ValErreur.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("TS70039E", False, paramMessage)
                            ValErreur.IsValid = False
                            BinderToutesGrilles()


                    End Select
                End If
            End If
        End If


        Return Page.IsValid
    End Function

    Private Function ObtAdresseCourrielDisponible(ByVal _strCourriel As String,
                                                  ByVal _intIncrementation As Int32) As String

        Dim strCourrielCour As String = _strCourriel
        If _intIncrementation > 0 Then
            'Insertion d'un nombre pour que l'adresse soit unique.
            _strCourriel = _strCourriel.Replace("@", Convert.ToString(_intIncrementation) & "@")
        End If

        Dim tblInfos As DataTable = NiCuRechercheAD.RechercherUtilisateursDTFiltreUsagerUniques(NiCuRechercheAD.enumColonneAD.AdresseCourriel, _strCourriel, , False, False, False)

        If tblInfos.Rows.Count > 0 Then
            'L'adresse n'est pas disponible.
            'On incrémente et revalide que cette nouvelle adresse est valide.
            Return ObtAdresseCourrielDisponible(strCourrielCour, _intIncrementation + 1)
        End If

        _strCourriel = _strCourriel.Replace("'", "")

        Return _strCourriel
    End Function

    Private Function ObtAdresseCourrielDisponible(ByVal _strCourriel As String) As String
        Return ObtAdresseCourrielDisponible(_strCourriel, 0)
    End Function

    Private Sub gererAssignationRoles()
        Dim pos As Integer = -1
        Dim DateValide As DateTime = Nothing


        If Not String.IsNullOrEmpty(mobjTrx.strDatEffective) Then

            DateValide = CType(mobjTrx.strDatEffective, DateTime).AddDays(14)

            'LES ROLES METIERS
            For i As Integer = 0 To grdRolesMetier.Items.Count - 1



                NiCuADO.PointerDT(mobjTrx.dtListeAssignationRole, "ID", grdRolesMetier.Items(i).Cells(0).Text, pos)

                'Vérifier si on doit lui assigner la nouvelle date d'expiration en fonction de la date effective.
                ' Doit être de type CHG et avoir une une ancienne unité administrative modelisée.
                ' La date d'expiration doit être plus petite que la date effective + 14 jours.
                ' le demandeur ne doit pas être responsable du role.
                Dim dateExpRole As XlCrDate = CType(grdRolesMetier.Items(i).FindControl("txtDatExpRole"), XlCrDate)
                If dateExpRole Is Nothing Then Continue For

                If Not mobjTrx.strTypeAcces Is Nothing Then
                    Dim blnAssignerDate As Boolean = False
                    If mobjTrx.blnUAModelise = True And mobjTrx.strTypeAcces.ToUpper = "CHG" Then

                        If Not (TSCuGeneral.boolResponsableRole(mobjTrx.dtListeAssignationRole.Rows(pos)("ListeUniteAdministrativeResponsable").ToString, mobjTrx.dtListeUADemandeur)) Then
                            'N'est pas responsable du role
                            If String.IsNullOrEmpty(mobjTrx.dtListeAssignationRole.Rows(pos)("DateFin").ToString) Then
                                blnAssignerDate = True

                            End If
                        End If

                        'la fonction "gererAssignationRoles" est appellé aussi dans le page_load.
                        ' A ce niveau, la date effective n'est pas validée mais l'objet mobjtrx.strdateeffective contient 
                        ' l'ancienne valeur.  Il ne faut pas assigner les nouvelles dates dans ce cas.
                        'If mobjTrx.strDatEffective <> XlCrDatEffective.Text Then
                        '    blnAssignerDate = False
                        'End If

                        If blnAssignerDate Then
                            mobjTrx.dtListeAssignationRole.Rows(pos)("DateFin") = DateValide.ToShortDateString
                            dateExpRole.Text = DateValide.ToShortDateString
                        Else
                            mobjTrx.dtListeAssignationRole.Rows(pos)("DateFin") = dateExpRole.Text
                        End If
                    Else
                        'pas de type CHG, on laisse les dates normalement.
                        mobjTrx.dtListeAssignationRole.Rows(pos)("DateFin") = dateExpRole.Text
                    End If
                Else
                    'pas d'acces, on laisse les dates.
                    mobjTrx.dtListeAssignationRole.Rows(pos)("DateFin") = dateExpRole.Text
                End If


            Next

            'LES ROLES DE TACHES
            For i As Integer = 0 To grdRolesTache.Items.Count - 1

                NiCuADO.PointerDT(mobjTrx.dtListeAssignationRole, "ID", grdRolesTache.Items(i).Cells(0).Text, pos)

                'Garder le contexte s'il y en a un
                If Not String.IsNullOrEmpty(mobjTrx.dtListeAssignationRole.Rows(pos)("DomValContexte").ToString) Then
                    Dim lstDomVal As DropDownList = CType(grdRolesTache.Items(i).Controls(2).FindControl("lstContexte"), DropDownList)
                    If Request.Form(lstDomVal.UniqueID) <> Nothing Then
                        If Request.Form(lstDomVal.UniqueID) <> "Choisir" Then
                            mobjTrx.dtListeAssignationRole.Rows(pos)("Contexte") = Request.Form(lstDomVal.UniqueID)
                            If Trim(mobjTrx.dtListeAssignationRole.Rows(pos)("Contexte").ToString) <> Trim(mobjTrx.dtListeAssignationRole.Rows(pos)("ContexteOrigine").ToString) Then
                                mobjTrx.dtListeAssignationRole.Rows(pos)("Action") = "O"
                                mobjTrx.dtListeAssignationRole.Rows(pos)("strChangerContexte") = "True"
                            Else
                                mobjTrx.dtListeAssignationRole.Rows(pos)("strChangerContexte") = "False"
                            End If
                        Else
                            mobjTrx.dtListeAssignationRole.Rows(pos)("Contexte") = String.Empty
                        End If

                    End If
                End If

                'Vérifier si on doit lui assigner la nouvelle date d'expiration en fonction de la date effective.
                ' Doit être de type CHG et avoir une ancienne unité administrative modelisée.
                ' La date d'expiration doit être plus petite que la date effective + 14 jours.
                ' le demandeur ne doit pas être responsable du role.
                Dim dateExpRole As XlCrDate = CType(grdRolesTache.Items(i).FindControl("txtDatExpRole"), XlCrDate)
                If dateExpRole Is Nothing Then Continue For

                If Not mobjTrx.strTypeAcces Is Nothing Then
                    Dim blnAssignerDate As Boolean = False
                    If mobjTrx.blnUAModelise = True And mobjTrx.strTypeAcces.ToUpper = "CHG" Then

                        Dim strListeUniteAdministrativeResponsable As String = String.Empty
                        strListeUniteAdministrativeResponsable = mobjTrx.dtListeAssignationRole.Rows(pos)("ListeUniteAdministrativeResponsable").ToString()


                        If Not (TSCuGeneral.boolResponsableRole(strListeUniteAdministrativeResponsable, mobjTrx.dtListeUADemandeur)) Then
                            'N'est pas responsable du role
                            If String.IsNullOrEmpty(mobjTrx.dtListeAssignationRole.Rows(pos)("DateFin").ToString) Then
                                blnAssignerDate = True
                            End If
                        End If

                        'la fonction "gererAssignationRoles" est appellé aussi dans le page_load.
                        ' A ce niveau, la date effective n'est pas validée mais l'objet mobjtrx.strdateeffective contient 
                        ' l'ancienne valeur.  Il ne faut pas assigner les nouvelles dates dans ce cas.
                        'If mobjTrx.strDatEffective <> XlCrDatEffective.Text Then
                        '    blnAssignerDate = False
                        'End If

                        If blnAssignerDate Then
                            mobjTrx.dtListeAssignationRole.Rows(pos)("DateFin") = DateValide.ToShortDateString
                            dateExpRole.Text = DateValide.ToShortDateString
                        Else
                            mobjTrx.dtListeAssignationRole.Rows(pos)("DateFin") = dateExpRole.Text
                        End If
                    Else
                        'pas de type CHG, on laisse les dates normalement.
                        mobjTrx.dtListeAssignationRole.Rows(pos)("DateFin") = dateExpRole.Text
                    End If
                Else
                    'pas d'acces, on laisse les dates.
                    mobjTrx.dtListeAssignationRole.Rows(pos)("DateFin") = dateExpRole.Text
                End If


            Next
        Else
            For i As Integer = 0 To grdRolesMetier.Items.Count - 1
                NiCuADO.PointerDT(mobjTrx.dtListeAssignationRole, "ID", grdRolesMetier.Items(i).Cells(0).Text, pos)
                Dim dateExpRole As XlCrDate = CType(grdRolesMetier.Items(i).FindControl("txtDatExpRole"), XlCrDate)
                If Not dateExpRole Is Nothing Then
                    mobjTrx.dtListeAssignationRole.Rows(pos)("DateFin") = dateExpRole.Text
                End If
            Next

            For i As Integer = 0 To grdRolesTache.Items.Count - 1
                NiCuADO.PointerDT(mobjTrx.dtListeAssignationRole, "ID", grdRolesTache.Items(i).Cells(0).Text, pos)
                Dim dateExpRole As XlCrDate = CType(grdRolesTache.Items(i).FindControl("txtDatExpRole"), XlCrDate)
                If Not dateExpRole Is Nothing Then
                    mobjTrx.dtListeAssignationRole.Rows(pos)("DateFin") = dateExpRole.Text
                End If


                'Garder le contexte s'il y en a un
                If Not String.IsNullOrEmpty(mobjTrx.dtListeAssignationRole.Rows(pos)("DomValContexte").ToString) Then
                    Dim lstDomVal As DropDownList = CType(grdRolesTache.Items(i).Controls(2).FindControl("lstContexte"), DropDownList)
                    If Request.Form(lstDomVal.UniqueID) <> Nothing Then
                        If Request.Form(lstDomVal.UniqueID) <> "Choisir" Then
                            mobjTrx.dtListeAssignationRole.Rows(pos)("Contexte") = Request.Form(lstDomVal.UniqueID)
                            mobjTrx.dtListeAssignationRole.AcceptChanges()
                        Else
                            mobjTrx.dtListeAssignationRole.Rows(pos)("Contexte") = String.Empty
                            mobjTrx.dtListeAssignationRole.AcceptChanges()
                        End If

                    End If
                End If


            Next

        End If



    End Sub


    Public Function ObtenirParticulier(ByVal blnParticulier As Boolean) As Boolean
        Return blnParticulier
    End Function

    Private Sub InitialiserFenComplDetailRole(ByVal strDetailRole As String)
        'Si c'est un lien, on affiche la page dans un autre onglet
        If strDetailRole.Substring(0, 4).ToUpper = "HTTP" Then
            Response.Write("<script>")
            Response.Write("window.open('" & strDetailRole & "','_blank')")
            Response.Write("</script>")
        Else

            'sinon, afficher le texte de description.
            AfficherFenComplDetailRole("Retour",
                                   "Gérer les accès des utilisateurs",
                                   strDetailRole,
                    New TS7CdParametresFenComplDetailRole.TS7CdBouton() {
                    New TS7CdParametresFenComplDetailRole.TS7CdBouton("Retour", "Retour", True, True)})
        End If

    End Sub


    Public Sub AfficherFenComplDetailRole(ByVal strNomboutonAppelant As String,
                                         ByVal strTitreFenMessage As String,
                                         ByVal strDetailRole As String,
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

    Protected Sub cmRecommencer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmRecommencer.Click
        If IndAvertissementInterruption = enumAvertissementInterruption.Oui Then
            Me.AfficherFenetreMessage("cmdRecommencer",
                                      "NI10012A",
                                      "Recommencer l'opération",
                                      Rrq.Web.GabaritsPetitsSystemes.FonctionsCommunes.NiTypeMessage.Avertissement,
                    New NiCdBouton() {New NiCdBouton("Ne pas recommencer", "NePasRecommencer", True, False),
                                      New NiCdBouton("Recommencer", "Recommencer", False, False)})
        End If
    End Sub

    Public Sub btnAnnulerFichier_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnnulerFichier.Click
        mobjTrx.FichierPieceJointe = Nothing
        fileFichierJoint.Visible = True
        txtFichierJoint.Visible = False
        btnAnnulerFichier.Enabled = False
        btnFauxParcourir.Visible = False
    End Sub

    ''' <summary>
    ''' Verifier si l'ancienne unite admin fait partie des ua modelisée.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub VerifierUAModelisee()

        mobjTrx.blnUAModelise = False

        If Not String.IsNullOrEmpty(mobjTrx.AncienneUA) Then

            'Obtenir la liste de toutes les UA dans Sage (CA RCM)
            Dim lstUA As List(Of Rrq.Securite.GestionAcces.TsCdUniteAdministrative) = TsCaServiceGestnAcces.ObtenirListeUnitesAdmin()

            For Each UA As Rrq.Securite.GestionAcces.TsCdUniteAdministrative In lstUA
                If UA.No = mobjTrx.AncienneUA Then
                    mobjTrx.blnUAModelise = True
                End If
            Next

        End If

    End Sub
    Public Sub RemplirUADemandeur()


        Dim lstUnitAdmin As List(Of String)
        Dim lstUnitAdminTS As New List(Of Rrq.Securite.GestionAcces.TsCdUniteAdministrative)


        'Obtient toutes les unités administratives disponibles.
        Dim dtToutesUnitAdmin As DataTable = mobjAffaire.ObtenirUniteAdmin()

        'Recherche des autres unité administrative reliées aux groupes de sécurité de l'utilisateur.
        lstUnitAdmin = ObtenirUnitAdminADParAcces()

        'Pour toutes les unités trouvées par accès, on parcourt le datatable de toutes les unités administratives pour remplir les données
        'manquantes pour créer des TSCDUniteAdministrative.
        For Each UnitAdmin As String In lstUnitAdmin
            For Each drToutesUnitAdmin As DataRow In dtToutesUnitAdmin.Rows
                If CStr(drToutesUnitAdmin("No")) = UnitAdmin Then
                    Dim UnitAdminAcces As New Rrq.Securite.GestionAcces.TsCdUniteAdministrative()
                    UnitAdminAcces.No = CStr(drToutesUnitAdmin("No"))
                    UnitAdminAcces.Nom = CStr(drToutesUnitAdmin("Nom"))
                    UnitAdminAcces.Abbreviation = drToutesUnitAdmin("Abbreviation").ToString()
                    UnitAdminAcces.IDRole = drToutesUnitAdmin("IDRole").ToString


                    lstUnitAdminTS.Add(UnitAdminAcces)
                    UnitAdminAcces = Nothing
                End If
            Next
        Next


        mobjTrx.dtListeUADemandeur = TSCuGeneral.UniteAdminDataTable(lstUnitAdminTS)

    End Sub

    Public Sub BinderToutesGrilles()
        If (Not mobjTrx.dtListeAssignationRole Is Nothing) AndAlso mobjTrx.dtListeAssignationRole.Rows.Count > 0 Then
            Dim tabMetier() As DataRow
            tabMetier = mobjTrx.dtListeAssignationRole.Select("ID like 'REM_%'")
            Dim tabTaches() As DataRow
            tabTaches = mobjTrx.dtListeAssignationRole.Select("ID like 'RET_%'")




            If (tabMetier Is Nothing OrElse tabMetier.Count = 0) And (tabTaches Is Nothing OrElse tabTaches.Count = 0) Then
                plcResultatVide.Visible = True
                lblResultatVide.Visible = True
            Else
                If Not (tabMetier Is Nothing OrElse tabMetier.Count = 0) Then
                    grdRolesMetier.BinderGrille(tabMetier.CopyToDataTable)
                End If
                If Not (tabTaches Is Nothing OrElse tabTaches.Count = 0) Then
                    grdRolesTache.BinderGrille(tabTaches.CopyToDataTable)
                    plcResultatVide.Visible = False
                    lblResultatVide.Visible = False
                End If

            End If

        Else
            plcResultatVide.Visible = True
            lblResultatVide.Visible = True
        End If

    End Sub
    Public Sub ChangerStyleDateEffective(ByRef sender As XlCrDate)

        If sender.ReadOnly Then
            'Fond gris, écriture grise estompée
            sender.CssClass = "dateDisabled date"
        Else
            'Fond blanc, écriture noire.
            sender.CssClass = "dateEnabled date"
        End If
    End Sub


    ''' <summary>
    ''' Survol de tous les groupes de sécurité de l'utilisateur afin d'obtenir la liste
    ''' de toutes les unités administratives dont il a accès.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ObtenirUnitAdminADParAcces() As List(Of String)
        Dim lstUnitAdmin As New List(Of String)
        Dim strRecherche As String = String.Empty
        Dim position As Integer = 0


        Dim Groupes() As String = ContexteApp.UtilisateurCourant.GroupesMembreDe()


        'strRecherche = "ROG_" & ObtenirEnvironnementStr(System.Web.HttpContext.Current) & "_TS7_GestionAcces"
        strRecherche = Rrq.InfrastructureCommune.Parametres.XuCuConfiguration.ValeurSysteme("TS7", "GroupesSecurite\GRP_RESPONSABLE_UA").Replace("#UA#", "")
        If Not String.IsNullOrEmpty(strRecherche) Then
            strRecherche = strRecherche.Replace("#ENVIRONNEMENT#", ObtenirEnvironnementStr(System.Web.HttpContext.Current))
            'Pour tout le tableau des groupes.  Rechercher les groupes "GestionsAcces" pour obtenir les unité administratives.
            For Each groupe As String In Groupes
                position = InStr(groupe, strRecherche)
                If position <> 0 Then
                    lstUnitAdmin.Add(groupe.Replace(strRecherche, ""))
                End If
            Next
        End If

        Return lstUnitAdmin

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

    Public Function AfficherSuggestion(ByVal IDRole As String, ByVal NomGrille As String, ByVal texteLienTachesMetiers As String, ByVal position As String) As String
        If mobjTrx.IDRoleNonValideCoherence = String.Empty Then
            'Aucune erreur trouvée dans les validations
            Return String.Empty
        Else
            If Not String.IsNullOrEmpty(TSCuGeneral.AfficherSuggestionCoherence(IDRole, NomGrille, texteLienTachesMetiers, position,
                                                           mobjTrx.IDRoleNonValideCoherence, mobjTrx.RegleCoherenceEnErreur, mobjTrx.lstReglesCoherences)) Then
                Return "color:red;font-weight:bold;"

            End If
        End If

        Return String.Empty
    End Function
    Public Function ActiverListeContextes(ByVal dr As DataRowView) As Boolean

        Dim lstUARole As List(Of String) = dr("ListeUniteAdministrativeResponsable").ToString.Split(CChar(";")).ToList

        If mobjTrx.dtListeUADemandeur IsNot Nothing Then
            For Each drUAResponsable As DataRow In mobjTrx.dtListeUADemandeur.Rows
                For Each drRole As String In lstUARole
                    If drRole = drUAResponsable("No").ToString Then
                        Return True
                    End If
                Next
            Next
        End If
        Return False

    End Function

    Private Sub AfficherMessageConservationComptes()
        Me.AfficherFenetreMessage("Conserver",
                                  "TS70042A",
                                  "Comptes supplémentaires",
                                  Avertissement,
                New NiCdBouton() {New NiCdBouton("Conserver", "Conserver", True, True),
                                  New NiCdBouton("Ne pas conserver", "NePasConserver", False, False)}, , 140)

    End Sub

End Class
