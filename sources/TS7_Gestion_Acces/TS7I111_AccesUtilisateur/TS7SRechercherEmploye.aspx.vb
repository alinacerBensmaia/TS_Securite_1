Imports Rrq.Web.GabaritsPetitsSystemes.FonctionsCommunes.NiCdParametresMsg
Imports Rrq.Web.GabaritsPetitsSystemes.FonctionsCommunes.NiTypeMessage
Imports Rrq.Web.GabaritsPetitsSystemes.Controles.NiCrPage
Imports Rrq.Securite.GestionAcces
Imports Rrq.Web.AccesUtilisateurs.Utilitaires
Imports System.Collections.Generic
Imports Rrq.Web.GabaritsPetitsSystemes.Utilitaires

Partial Class TS7SRechercherEmploye
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

    Public strType As String = Nothing
    Public strCodetrans As String = Nothing
    Public strPage As String
    Public strNom As String
    Public strPrenom As String
    Public strProv As String
    Public strGuid As String
    Public strAncUA As String



    Public Overrides ReadOnly Property GroupeADRequis() As String
        Get
           'Changement du nom du groupe pour la gestion des roles. 2013-12-11
            'Return "GTS7#ENVIRONNEMENT#GestionAcces"
            ' Return "ROG_#ENVIRONNEMENT#_Gestion des acces demandeur"
            Return String.Concat(Rrq.InfrastructureCommune.Parametres.XuCuConfiguration.ValeurSysteme("TS7", "GroupesSecurite\GRP_ACCES_UTILISATEUR"), ";", _
                                 Rrq.InfrastructureCommune.Parametres.XuCuConfiguration.ValeurSysteme("TS7", "GroupesSecurite\GRP_RESPONSABLE_UA"), ";", _
                                 Rrq.InfrastructureCommune.Parametres.XuCuConfiguration.ValeurSysteme("TS7", "GroupesSecurite\GRP_PILOTAGE"))
        End Get
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim strExpTriInitial As String = "NomComplet"
        grdEmployes.InitialiserControle("TS7I111_AccesUtilisateur", hypPage1, hypPage2, hypPage3, hypPage4, hypPage5, hypPage6, hypPage7, hypPage8, HyPrecedent, hySuivant, strExpTriInitial)

        mobjAffaire = New TS7I112_RAAccesUtilisateur.TSCaAccesUtilisateur
        mobjTrx = CType(ContexteApp.TrxCourante, TS7I112_RAAccesUtilisateur.TSCdObjetTrx)


        IndAvertissementInterruption = enumAvertissementInterruption.Non
        'Prov=TS7SGererRolesEmploye  lblTitre
        strCodetrans = Request.QueryString("codetrans")
        strType = Request.QueryString("Type")
        strNom = Request.QueryString("Nom")
        strPrenom = Request.QueryString("Prenom")
        strPage = Request.QueryString("Page")
        strProv = Request.QueryString("Prov")
        strGuid = Request.QueryString("Guid")
        strAncUA = Request.QueryString("AncUniteAdm")

        Select Case strProv
            Case "TS7SGererRolesEmploye"
                lblTitre.Text = "Rechercher un employé afin de copier ses rôles"
                Critere.Visible = True
                pnlCmd.Visible = True
                pnlImportant.Visible = False

            Case "TS7SSupprimerEmploye"
                lblTitre.Text = "Rechercher un employé afin de supprimer ses rôles"
                Critere.Visible = True
                pnlCmd.Visible = True
                pnlImportant.Visible = False

            Case Else
                lblTitre.Text = "Important"
                cmdPrecedent1.Visible = False
                cmdPrecedent.Visible = False
        End Select

        If Not IsPostBack Then
            ForcerRafraichissement()
            InitialiserNouvelleTrx()
            Session("RolesSelectionnes") = Nothing
            mobjTrx.dtListeEmployes = Nothing
            If Not strNom Is Nothing Then txtNom.Text = strNom
            If Not strPrenom Is Nothing Then txtPrenom.Text = strPrenom

            If Not strNom Is Nothing OrElse Not strNom Is Nothing Then
                If txtCodeUtilisateur.Text.Equals(String.Empty) Then
                    mobjTrx.dtListeEmployes = TSCuGeneral.UtilisateurDataTable(TsCaServiceGestnAcces.RechercherUtilisateur(String.Concat(txtNom.Text, " ", txtPrenom.Text)))
                Else
                    mobjTrx.dtListeEmployes = TSCuGeneral.UtilisateurUniqueDataTable(TsCaServiceGestnAcces.ObtenirUtilisateur(txtCodeUtilisateur.Text.ToUpper))
                End If
            End If
            AssignerControleFocus(txtNom.ClientID)
        End If

        If Not mobjTrx.dtListeEmployes Is Nothing AndAlso
             mobjTrx.dtListeEmployes.Rows.Count > 0 Then
            grdEmployes.BinderGrille(mobjTrx.dtListeEmployes)
            pnlResultatRecherche.Visible = True
            pnlNavigation.Visible = True

        Else
            pnlResultatRecherche.Visible = False
            pnlNavigation.Visible = False
        End If

    End Sub

    Private Sub cmdRecommencer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRecommencer.Click
        IndAvertissementInterruption = enumAvertissementInterruption.Non
        Response.Redirect(Request.Url.ToString)

        'Exemple de lancement de fenêtre modale
        'Me.AfficherFenetreMessage("cmdRecommencer",
        '                          "RV10006A",
        '                          "<Titre page en cours>",
        '                          Avertissement,
        '        New NiCdBouton() {New NiCdBouton("Recommencer", "Recommencer", True, False),
        '                          New NiCdBouton("NePasRecommencer", "NePasRecommencer", False, True)})
    End Sub

    Private Sub InitialiserNouvelleTrx()

        If mobjTrx Is Nothing Then

            Dim strCodeUsager As String
            strCodeUsager = ContexteApp.UtilisateurCourant.CodeUtilisateur

            'On initialise une nouvelle transaction pour l'utilisateur courant
            mobjTrx = New TS7I112_RAAccesUtilisateur.TSCdObjetTrx(strCodeUsager)

            ContexteApp.TrxCourante = mobjTrx
        End If
    End Sub

    Private Sub hypPage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles hypPage1.Click, hypPage2.Click, hypPage3.Click, hypPage4.Click, hypPage5.Click, hypPage6.Click, hypPage7.Click, hypPage8.Click, HyPrecedent.Click, hySuivant.Click
        grdEmployes.PageButtonClick(sender, e)
    End Sub

    Protected Sub cmdRechercher_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRechercher.Click

        If txtCodeUtilisateur.Text.Length = 0 And txtNom.Text.Trim.Length = 0 And txtPrenom.Text.Trim.Length = 0 Then
            valErreur.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("TS70005E", False, "")
            valErreur.IsValid = False
            mobjTrx.dtListeEmployes = Nothing
            pnlResultatRecherche.Visible = False
            pnlNavigation.Visible = False
        End If

        If Page.IsValid Then
            Try
                If txtCodeUtilisateur.Text.Equals(String.Empty) Then
                    mobjTrx.dtListeEmployes = TSCuGeneral.UtilisateurDataTable(TsCaServiceGestnAcces.RechercherUtilisateur(String.Concat(txtNom.Text, " ", txtPrenom.Text)))
                Else
                    mobjTrx.dtListeEmployes = TSCuGeneral.UtilisateurUniqueDataTable(TsCaServiceGestnAcces.ObtenirUtilisateur(txtCodeUtilisateur.Text.ToUpper))
                End If

                If mobjTrx.dtListeEmployes.Rows.Count > 0 Then
                    grdEmployes.BinderGrille(mobjTrx.dtListeEmployes, 0)
                    pnlResultatRecherche.Visible = True
                    pnlNavigation.Visible = True
                    cmdRechercher.CssClass = "boutonnormal boutonADroite"
                    cmdPrecedent1.Visible = False
                Else
                    valErreur.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("TS70002E", False, "")
                    valErreur.IsValid = False
                    pnlResultatRecherche.Visible = False
                    pnlNavigation.Visible = False
                End If

            Catch ex As Exception
                valErreur.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("TS70002E", False, "")
                valErreur.IsValid = False
                mobjTrx.dtListeEmployes = Nothing
                pnlResultatRecherche.Visible = False
                pnlNavigation.Visible = False
            End Try
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

    Protected Sub cmdSelectionner_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSelectionner.Click
        Dim strCodUtilisateurSelect As String = String.Empty
        Dim strNomPrenom As String = String.Empty

        For i As Integer = 0 To grdEmployes.Items.Count - 1
            If CType(grdEmployes.Items(i).FindControl("chkEmployeSelect"), RadioButton).Checked Then
                strCodUtilisateurSelect = grdEmployes.Items(i).Cells(3).Text
                strNomPrenom = grdEmployes.Items(i).Cells(1).Text
                Exit For
            End If
        Next

        'Aucune sélection
        If strCodUtilisateurSelect.Equals(String.Empty) Then
            valErreur.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("NI10008E", False, "")
            valErreur.IsValid = False
        Else
            Select Case strPage
                Case "TS7SCopierRoles"
                    Response.Redirect(String.Concat(TSCuDomVal.PAGE_COPIER_ROLE, "?codetrans=", strCodetrans, "&CodeUtilisateur=" & strCodUtilisateurSelect, "&Type=", strType))

                Case "TS7SSupprimerEmploye"
                    Response.Redirect(String.Concat(TSCuDomVal.PAGE_SUPPRIMER_EMPLOYE, "?codetrans=", strCodetrans,
                            "&CodeUtilisateur=" & strCodUtilisateurSelect, "&NomPrenom=", strNomPrenom, "&Prov=TS7SRechercherEmploye"))

                Case Else

                    mobjTrx.blnUtilisateurSage = True

                    Response.Redirect(String.Concat(TSCuDomVal.PAGE_ROLE_EMPLOYE, "?codetrans=", strCodetrans,
                            "&CodeUtilisateur=" & strCodUtilisateurSelect, "&Type=", strType, "&Prov=TS7SRechercherEmploye"))
            End Select

        End If
    End Sub

    Protected Sub cmdPrecedent_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrecedent.Click,
                                                                                                        cmdPrecedent1.Click
        Dim strCodetrans As String = Request.QueryString("codetrans")
        Select Case strProv
            Case "TS7SGererRolesEmploye"
                Response.Redirect(String.Concat(TSCuDomVal.PAGE_ROLE_EMPLOYE, "?codetrans=", strCodetrans, "&Prov=TS7SRechercherEmployeAjout"))

            Case "TS7SSupprimerEmploye"
                Response.Redirect(String.Concat(TSCuDomVal.PAGE_SUPPRIMER_EMPLOYE, "?codetrans=", strCodetrans, "&Prov=TS7SRechercherEmploye"))

            Case Else

        End Select


    End Sub

End Class
