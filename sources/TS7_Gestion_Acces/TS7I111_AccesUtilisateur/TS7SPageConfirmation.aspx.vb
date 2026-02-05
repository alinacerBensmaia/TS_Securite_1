Imports Rrq.Web.GabaritsPetitsSystemes.FonctionsCommunes.NiCdParametresMsg
Imports Rrq.Web.GabaritsPetitsSystemes.FonctionsCommunes.NiTypeMessage
Imports Rrq.Web.GabaritsPetitsSystemes.Controles.NiCrPage
Imports Rrq.Web.GabaritsPetitsSystemes.Utilitaires
Imports Rrq.Web.AccesUtilisateurs.Utilitaires
Imports Rrq.Securite.GestionAcces
Imports System.Collections.Generic
Imports System.Linq
Imports Rrq.InfrastructureCommune.Parametres
Imports TS7I112_RAAccesUtilisateur

Partial Class TS7SPageConfirmation
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
    Private dtListeAssignationRoleConfirm As DataTable = Nothing


    Public Overrides ReadOnly Property GroupeADRequis() As String
        Get
            'Changement du nom du groupe pour la gestion des roles. 2013-12-11
            'Return "GTS7#ENVIRONNEMENT#GestionAcces"
            'Return "ROG_#ENVIRONNEMENT#_Gestion des acces demandeur"
            Return String.Concat(Rrq.InfrastructureCommune.Parametres.XuCuConfiguration.ValeurSysteme("TS7", "GroupesSecurite\GRP_ACCES_UTILISATEUR"), ";",
                                 Rrq.InfrastructureCommune.Parametres.XuCuConfiguration.ValeurSysteme("TS7", "GroupesSecurite\GRP_RESPONSABLE_UA"), ";",
                                 Rrq.InfrastructureCommune.Parametres.XuCuConfiguration.ValeurSysteme("TS7", "GroupesSecurite\GRP_PILOTAGE"))
        End Get
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        mobjTrx = CType(ContexteApp.TrxCourante, TS7I112_RAAccesUtilisateur.TSCdObjetTrx)
        mobjAffaire = New TS7I112_RAAccesUtilisateur.TSCaAccesUtilisateur

        If Not IsPostBack Then

            If Me.mobjTrx.strCourrielDemande.Length > 0 _
            AndAlso Me.mobjTrx.strCourrielDemande <> Me.mobjTrx.strCourriel Then

                InitialiserBoiteMessage(Me.pnlInfo, enumTypeMessage.Msg_Info)

                'valErreur.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("TS70021I")
                lblMessageInfo.Text = Me.ContexteApp.ObtenirMessageFormate("TS70021I", False)
                pnlInfo.Visible = True
                'valErreur.IsValid = False


            End If

            lblValeurNomEmploye.Text = mobjTrx.strNom
            lblValeurPrenomEmploye.Text = mobjTrx.strPrenom
            'lblValeurCourrielEmploye.Text = mobjTrx.strCourriel
            lblValeurVille.Text = mobjTrx.strVille
            lblValeurDateFinContrat.Text = mobjTrx.strFinContrat
            lblValeurUAPrinc.Text = mobjTrx.strUaPrincOpt
            lblPieceJointe.Text =
                If(mobjTrx.FichierPieceJointe Is Nothing OrElse String.IsNullOrEmpty(mobjTrx.FichierPieceJointe.NomFichier),
                    "Aucun",
                    mobjTrx.FichierPieceJointe.NomFichier)

            'lblValeurUAAutre.Text = mobjTrx.strUaAutreOpt

            If Not mobjTrx.strTypeAcces Is Nothing AndAlso mobjTrx.strTypeAcces.Equals("ARR") Then
                pnlRetrait.Visible = False
            Else
                'If Not (mobjTrx.strTypeAcces = "CHG" And mobjTrx.blnUAModelise = False) Then
                If Not (mobjTrx.strTypeAcces = "CHG" And mobjTrx.blnUtilisateurSage = False) Then
                    pnlRetrait.Visible = ObtenirZoneRetrait()
                End If
            End If

            initialiserABlanc()

            'Creer une copie afin de gérer l'affichage de l'action
            'T208704 : Aussi la liste des Unite administratives du role.
            'TRier le datatable selon l'action
            Dim dtAfficherRoles As DataTable = Nothing

            If Not (mobjTrx.dtListeAssignationRole Is Nothing OrElse mobjTrx.dtListeAssignationRole.Rows.Count = 0) Then
                dtListeAssignationRoleConfirm = mobjTrx.dtListeAssignationRole.Copy
                NiCuADO.AjouterDtColonne(dtListeAssignationRoleConfirm, "ActionAfficher", System.Type.GetType("System.String"), 10, "", True)
                dtListeAssignationRoleConfirm.AcceptChanges()
                dtAfficherRoles = ObtenirActionRole()
                'Conserver le datatable pour la page de confirmation
                mobjTrx.dtListeAssignationRoleConfirm = dtAfficherRoles


                'Séparer les grilles Taches et Metier
                Dim tabMetier As IEnumerable(Of DataRow)
                Dim tabTaches As IEnumerable(Of DataRow)

                tabMetier = dtAfficherRoles.Select("ID like 'REM_%'")
                tabTaches = dtAfficherRoles.Select("ID like 'RET_%'")

                If Not tabMetier Is Nothing AndAlso tabMetier.Count > 0 Then
                    tabMetier = tabMetier.OrderBy(Function(x) x("NomAAfficher"))
                    grdRolesMetier.DataSource = tabMetier.CopyToDataTable
                Else
                    grdRolesMetier.DataSource = Nothing
                End If

                If Not tabTaches Is Nothing AndAlso tabTaches.Count > 0 Then
                    tabTaches = tabTaches.OrderBy(Function(x) x("NomAAfficher"))
                    grdRolesTaches.DataSource = tabTaches.CopyToDataTable
                Else
                    grdRolesTaches.DataSource = Nothing
                End If

                'Enlever la colonne contexte si pas de contexte dans les roles de taches
                Dim drRolesContexte As IEnumerable(Of DataRow)

                drRolesContexte = dtAfficherRoles.Select("DomValcontexte <> ''")

                grdRolesTaches.Columns(2).Visible = Not drRolesContexte Is Nothing AndAlso drRolesContexte.Count > 0

                grdRolesMetier.DataBind()
                grdRolesTaches.DataBind()
            End If

            If Not dtAfficherRoles Is Nothing AndAlso dtAfficherRoles.Rows.Count > 0 Then
                lblRoles.Text = String.Concat("Rôles de ", mobjTrx.strNom, " ", mobjTrx.strPrenom)
                grdRolesMetier.Visible = True
                grdRolesTaches.Visible = True
            Else
                lblRoles.Text = String.Concat("Aucun rôles pour ", mobjTrx.strNom, " ", mobjTrx.strPrenom)
                grdRolesMetier.Visible = False
                grdRolesTaches.Visible = False

            End If
            'PB : Si un jour la date d'approbation doit être réaffichée, enlever les lignes suivantes en commentaire
            'If mobjTrx.blnApprobation Then
            'lblValeurApprobation.Text = String.Concat("La demande est approuvée le ", mobjTrx.strDatApprobation, ".")
            'Else
            'lblValeurApprobation.Text = "La demande n'a pas été approuvée."
            'End If

            If mobjTrx.IndComptesSuppModifie Then
                lblComptesSupplementaires.Text = mobjTrx.ObtenirTexteDifferencesComptesSupp(True)
            Else
                lblTitreComptesSupplementaires.Visible = False
            End If

            lblTexteLibre.Text = mobjTrx.strTexteLibre

            lblValeurDatEffective.Text = String.Concat("La date de saisie de la demande est: ", mobjTrx.strDatEffective)
        End If
    End Sub

    Private Sub cmdPrecedent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrecedent.Click
        Response.Redirect(String.Concat(TSCuDomVal.PAGE_ROLE_EMPLOYE, "?codetrans=ts7role", "&Prov=TS7SPageConfirmation"))
    End Sub

    Private Function ObtenirZoneRetrait() As Boolean
        Dim blnRetour As Boolean = False

        Dim pos As Integer
        lblValeurUA.Text = String.Empty
        For Each dr As DataRow In mobjTrx.dtUAUtilisateurCopie.Rows
            If dr("Action").Equals("S") Then
                If NiCuADO.PointerDT(mobjTrx.dtListeUnitesAdmin, "IDRole", dr("IDRole").ToString, pos) Then
                    lblValeurUA.Text &= String.Concat("<br />Unité administrative: ", mobjTrx.dtListeUnitesAdmin.Rows(pos).Item("Nom"))
                    lblValeurUA.Visible = True
                End If
            End If
        Next

        lblValeurEquip.Text = String.Empty
        For Each dr As DataRow In mobjTrx.dtEquipUtilisateurCopie.Rows
            If dr("Action").Equals("S") Then
                lblValeurEquip.Text = String.Concat(lblValeurEquip.Text, "<br />", dr("Nom"))
                lblValeurEquip.Visible = True
            End If
        Next
        If String.Concat(lblValeurUA.Text.Trim, lblValeurEquip.Text.Trim).Length > 0 Then blnRetour = True
        Return blnRetour
    End Function


    Private Sub initialiserABlanc()
        'Initialiser avec un blanc afin de gérer l'affichage
        If lblValeurNomEmploye.Text.Equals(String.Empty) Then lblValeurNomEmploye.Text = "---"
        If lblValeurPrenomEmploye.Text.Equals(String.Empty) Then lblValeurPrenomEmploye.Text = "---"
        'If lblValeurCourrielEmploye.Text.Equals(String.Empty) Then lblValeurCourrielEmploye.Text = "---"
        If lblValeurDateFinContrat.Text.Equals(String.Empty) Then lblValeurDateFinContrat.Text = "---"
        If lblValeurVille.Text.Equals(String.Empty) Then lblValeurVille.Text = "---"
        If lblValeurUAPrinc.Text.Equals(String.Empty) Then lblValeurUAPrinc.Text = "---"
        'If lblValeurUAAutre.Text.Equals(String.Empty) Then lblValeurUAAutre.Text = "---"
        If lblPieceJointe.Text.Equals(String.Empty) Then lblPieceJointe.Text = "---"
    End Sub

    Public Sub MettreAJourRolesContexte(ByVal pDr As DataRow, ByVal pDescription As String, ByVal pID As String, ByVal pNom As String,
                                        ByVal pContexte As String, ByVal pContexteOrigine As String, ByVal pAction As String,
                                        ByVal pstrSupprimer As String)

        Dim rRoleContexte As DataRow = dtListeAssignationRoleConfirm.NewRow()
        rRoleContexte("DateFin") = pDr("DateFin")
        rRoleContexte("Description") = pDescription
        rRoleContexte("FinPrevu") = pDr("FinPrevu")
        rRoleContexte("ID") = pID
        rRoleContexte("Nom") = pNom
        rRoleContexte("Particulier") = pDr("Particulier")
        rRoleContexte("Organisationnel") = pDr("Organisationnel")
        rRoleContexte("ListeUniteAdministrativeResponsable") = pDr("ListeUniteAdministrativeResponsable")
        rRoleContexte("Contexte") = Trim(pContexte)
        rRoleContexte("LienTachesMetiers") = pDr("LienTachesMetiers")
        rRoleContexte("DomValContexte") = pDr("DomValContexte")
        rRoleContexte("ContexteOrigine") = Trim(pContexteOrigine)
        rRoleContexte("NomAAfficher") = pDr("NomAAfficher")
        rRoleContexte("Action") = pAction
        rRoleContexte("strSupprimer") = pstrSupprimer
        dtListeAssignationRoleConfirm.Rows.Add(rRoleContexte)
        dtListeAssignationRoleConfirm.AcceptChanges()

    End Sub
    Public Sub GererRolesAvecContexte()
        For i As Integer = dtListeAssignationRoleConfirm.Rows.Count - 1 To 0 Step -1

            If InStr(dtListeAssignationRoleConfirm.Rows(i)("ID").ToString, "RET_C_") > 0 Then
                'changement de contexte
                If dtListeAssignationRoleConfirm.Rows(i)("strChangerContexte").ToString = "True" Then

                    'Si c'est un role retiré, on verifie s'il existait avant.
                    If dtListeAssignationRoleConfirm.Rows(i)("strSupprimer").ToString = "True" Then

                        If Trim(dtListeAssignationRoleConfirm.Rows(i)("ContexteOrigine").ToString) = String.Empty Then
                            'role retiré nouveau 
                            dtListeAssignationRoleConfirm.Rows(i).Delete()
                        Else
                            'role déja existant, rechercher le role avec contexte origine
                            Dim RoleContexte As New TsCdRole
                            RoleContexte = TSCuGeneral.RechercherRoleParID(dtListeAssignationRoleConfirm.Rows(i)("ID").ToString & "_" & Trim(dtListeAssignationRoleConfirm.Rows(i)("ContexteOrigine").ToString))
                            If Not RoleContexte Is Nothing Then
                                MettreAJourRolesContexte(dtListeAssignationRoleConfirm.Rows(i), RoleContexte.Description, RoleContexte.ID, RoleContexte.Nom,
                                        Trim(dtListeAssignationRoleConfirm.Rows(i)("ContexteOrigine").ToString()), Trim(dtListeAssignationRoleConfirm.Rows(i)("ContexteOrigine").ToString),
                                        dtListeAssignationRoleConfirm.Rows(i)("Action").ToString(), dtListeAssignationRoleConfirm.Rows(i)("strSupprimer").ToString)
                            End If

                        End If
                    Else
                        Dim RoleOrigine As New TsCdRole
                        Dim RoleNouveau As New TsCdRole

                        'Mettre l'ancien role (contexte original) en suppression
                        If Not String.IsNullOrEmpty(Trim(dtListeAssignationRoleConfirm.Rows(i)("ContexteOrigine").ToString)) Then
                            RoleOrigine = TSCuGeneral.RechercherRoleParID(dtListeAssignationRoleConfirm.Rows(i)("ID").ToString &
                                                                          "_" & Trim(dtListeAssignationRoleConfirm.Rows(i)("ContexteOrigine").ToString))
                            If Not RoleOrigine Is Nothing Then
                                MettreAJourRolesContexte(dtListeAssignationRoleConfirm.Rows(i), RoleOrigine.Description, RoleOrigine.ID, RoleOrigine.Nom,
                                                             Trim(dtListeAssignationRoleConfirm.Rows(i)("ContexteOrigine").ToString),
                                                             Trim(dtListeAssignationRoleConfirm.Rows(i)("ContexteOrigine").ToString), "O", "True")
                            End If

                        End If

                        If Not String.IsNullOrEmpty(Trim(dtListeAssignationRoleConfirm.Rows(i)("Contexte").ToString)) Then
                            RoleNouveau = TSCuGeneral.RechercherRoleParID(dtListeAssignationRoleConfirm.Rows(i)("ID").ToString &
                                                                          "_" & Trim(dtListeAssignationRoleConfirm.Rows(i)("Contexte").ToString))

                            If Not RoleNouveau Is Nothing Then
                                'Ajouter le role avec le nouveau contexte
                                MettreAJourRolesContexte(dtListeAssignationRoleConfirm.Rows(i), RoleNouveau.Description, RoleNouveau.ID, RoleNouveau.Nom,
                                                             Trim(dtListeAssignationRoleConfirm.Rows(i)("Contexte").ToString),
                                                             String.Empty, "A", "False")
                            End If

                        End If

                    End If
                Else
                    'Roles dont le contexte n'est pas modifié.  On va quand même chercher le bon role avec le contexte ou le contexte générique.
                    'On y met les valeurs du contexte générique du dtlisteassignationRoleConfirm.
                    If dtListeAssignationRoleConfirm.Rows(i)("Action").ToString = "A" Then
                        'Role ajouté sans contexte d'origine.  Recherche de l'ID avec le contexte choisi.
                        Dim RoleNouveau As New TsCdRole
                        RoleNouveau = TSCuGeneral.RechercherRoleParID(dtListeAssignationRoleConfirm.Rows(i)("ID").ToString &
                                                                      "_" & Trim(dtListeAssignationRoleConfirm.Rows(i)("Contexte").ToString))
                        If Not RoleNouveau Is Nothing Then
                            MettreAJourRolesContexte(dtListeAssignationRoleConfirm.Rows(i), RoleNouveau.Description, RoleNouveau.ID,
                                                            RoleNouveau.Nom, Trim(dtListeAssignationRoleConfirm.Rows(i)("Contexte").ToString),
                                                                Trim(dtListeAssignationRoleConfirm.Rows(i)("ContexteOrigine").ToString),
                                                               dtListeAssignationRoleConfirm.Rows(i)("Action").ToString, dtListeAssignationRoleConfirm.Rows(i)("strSupprimer").ToString)
                        End If

                    Else
                        Dim Role As New TsCdRole
                        Role = TSCuGeneral.RechercherRoleParID(dtListeAssignationRoleConfirm.Rows(i)("ID").ToString &
                                                               "_" & Trim(dtListeAssignationRoleConfirm.Rows(i)("ContexteOrigine").ToString))
                        If Not Role Is Nothing Then
                            MettreAJourRolesContexte(dtListeAssignationRoleConfirm.Rows(i), Role.Description, Role.ID,
                                                             Role.Nom, Trim(dtListeAssignationRoleConfirm.Rows(i)("Contexte").ToString),
                                                                 Trim(dtListeAssignationRoleConfirm.Rows(i)("ContexteOrigine").ToString),
                                                                 dtListeAssignationRoleConfirm.Rows(i)("Action").ToString, dtListeAssignationRoleConfirm.Rows(i)("strSupprimer").ToString)
                        End If

                    End If

                End If
                'Supprimer le Role générique
                dtListeAssignationRoleConfirm.Rows(i).Delete()
                dtListeAssignationRoleConfirm.AcceptChanges()

            End If

        Next

    End Sub
    Private Function ObtenirActionRole() As DataTable
        GererRolesAvecContexte()
        For i As Integer = dtListeAssignationRoleConfirm.Rows.Count - 1 To 0 Step -1


            'Role ajouter et supprimer
            If dtListeAssignationRoleConfirm.Rows(i).Item("strSupprimer").Equals("True") And
                dtListeAssignationRoleConfirm.Rows(i).Item("Action").Equals("A") Then
                dtListeAssignationRoleConfirm.Rows(i).Delete()
            ElseIf dtListeAssignationRoleConfirm.Rows(i).Item("strSupprimer").Equals("True") And
                dtListeAssignationRoleConfirm.Rows(i).Item("Action").Equals("O") Then

                dtListeAssignationRoleConfirm.Rows(i).Item("ActionAfficher") = "Supprimer"
            ElseIf dtListeAssignationRoleConfirm.Rows(i).Item("strSupprimer").Equals("False") And
                dtListeAssignationRoleConfirm.Rows(i).Item("Action").Equals("A") Then
                dtListeAssignationRoleConfirm.Rows(i).Item("ActionAfficher") = "Ajouter"
            ElseIf dtListeAssignationRoleConfirm.Rows(i).Item("strSupprimer").Equals("False") And
                dtListeAssignationRoleConfirm.Rows(i).Item("Action").Equals("O") And
                Not dtListeAssignationRoleConfirm.Rows(i).Item("DateFin").Equals(String.Empty) Then
                dtListeAssignationRoleConfirm.Rows(i).Item("ActionAfficher") = "Modifier"
            End If


        Next

        If dtListeAssignationRoleConfirm.Rows.Count = 0 Then
            Return dtListeAssignationRoleConfirm
        Else
            Return NiCuADO.TrierDT(dtListeAssignationRoleConfirm, "ActionAfficher asc")
        End If

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

#End Region



    Protected Sub cmdEnregistrer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEnregistrer.Click

        'T208704 : il faut copier les changements de contexte dans la table des AssignationRole
        If Not (mobjTrx.dtListeAssignationRoleConfirm Is Nothing OrElse mobjTrx.dtListeAssignationRoleConfirm.Rows.Count = 0) Then
            mobjTrx.dtListeAssignationRole = mobjTrx.dtListeAssignationRoleConfirm.Copy
        End If

        DefinirNomPieceJointe()

        Select Case mobjTrx.strTypeAcces
            Case "ARR"
                GererActionsCreation()

            Case Else
                If (mobjTrx.strTypeAcces = "CHG" And mobjTrx.blnUtilisateurSage = False) Then
                    GererActionsCreation()
                Else
                    GererActionsModification()
                End If

        End Select

        If Page.IsValid Then
            IndAvertissementInterruption = enumAvertissementInterruption.Non

            If mobjTrx.blnCreation Then
                Response.Redirect(String.Concat(TSCuDomVal.PAGE_CONFIRMATION, "?codetrans=ts7nouvel", "&Action=Ajout"))
            ElseIf Not mobjTrx.strTypeAcces Is Nothing AndAlso mobjTrx.strTypeAcces.Equals("ARR") Then
                Response.Redirect(String.Concat(TSCuDomVal.PAGE_CONFIRMATION, "?codetrans=ts7role", "&Action=Ajout"))
            ElseIf (mobjTrx.strTypeAcces = "CHG" And mobjTrx.blnUtilisateurSage = False) Then
                Response.Redirect(String.Concat(TSCuDomVal.PAGE_CONFIRMATION, "?codetrans=ts7role", "&Action=Ajout"))

            Else
                Response.Redirect(String.Concat(TSCuDomVal.PAGE_CONFIRMATION, "?codetrans=ts7role", "&Action=Modification"))
            End If
        End If
    End Sub

    Private Sub DefinirNomPieceJointe()
        If mobjTrx.FichierPieceJointe Is Nothing Then Exit Sub

        Dim cheminDepotPieceJointe As String = XuCuConfiguration.ValeurSysteme("TS7", "TS7N121\CheminDepotPieceJointe")
        mobjTrx.FichierPieceJointe.DefinirNouveauNomFichier(cheminDepotPieceJointe, mobjTrx.strGuid)
    End Sub

    Public Sub GererActionsCreation()
        Dim creation As New TsCdDemndCreationModif()
        Dim objCreationUtilisateur As New TsCdUtilisateur
        Dim LigneInfo As String = ""
        Dim Separateur As String = "|"
        Dim lstRoles As String = Nothing
        Dim strNmRep As String = Nothing
        Dim strPermFic As String = Nothing
        Dim strPermRep As String = Nothing

        'Try
        'Inscrire Objet Utilisateur



        objCreationUtilisateur.ApprobationAccepter = mobjTrx.blnApprobation
        'objCreationUtilisateur.ApprobationAccepter = chkApprobation.Checked
        objCreationUtilisateur.Nom = mobjTrx.strNom
        objCreationUtilisateur.Prenom = mobjTrx.strPrenom
        objCreationUtilisateur.ID = mobjTrx.strCodUtilisateurSelect
        If mobjTrx.strTypeAcces = "ARR" Then
            objCreationUtilisateur.Courriel = mobjTrx.strCourriel.Replace(" ", "-")
        Else
            objCreationUtilisateur.Courriel = mobjTrx.strCourriel
        End If

        objCreationUtilisateur.Ville = mobjTrx.strVille
        If mobjTrx.blnUtilisateurSage = False And mobjTrx.strTypeAcces = "CHG" Then
            objCreationUtilisateur.ID = mobjTrx.strCodUtilisateurSelect
        End If

        If Not mobjTrx.strFinContrat Is Nothing AndAlso Not mobjTrx.strFinContrat.Trim.Equals(String.Empty) Then objCreationUtilisateur.DateFin = CType(mobjTrx.strFinContrat, Date)

        objCreationUtilisateur.NoUniteAdmin = mobjTrx.strUaPrincOpt.Substring(0,
                                            mobjTrx.strUaPrincOpt.IndexOf("-"))

        objCreationUtilisateur.NomComplet = mobjTrx.strNom & " " & mobjTrx.strPrenom & " (" & objCreationUtilisateur.NoUniteAdmin & ")"
        If mobjTrx.blnApprobation Then _
                objCreationUtilisateur.DateApprobation = CType(mobjTrx.strDatApprobation, Date)

        creation.Utilisateur = objCreationUtilisateur

        creation.AjouterRole(mobjTrx.strUaPrinc)


        If Not mobjTrx.strEquip1Princ.Trim.Equals(String.Empty) Then creation.AjouterRole(mobjTrx.strEquip1Princ)
        If Not mobjTrx.strEquip2Princ.Trim.Equals(String.Empty) Then creation.AjouterRole(mobjTrx.strEquip2Princ)

        If Not mobjTrx.strUaAutre.Trim.Equals(String.Empty) Then creation.AjouterRole(mobjTrx.strUaAutre)
        If Not mobjTrx.strEquip1Autre.Trim.Equals(String.Empty) Then creation.AjouterRole(mobjTrx.strEquip1Autre)
        If Not mobjTrx.strEquip2Autre.Trim.Equals(String.Empty) Then creation.AjouterRole(mobjTrx.strEquip2Autre)

        'Gerer Assignations
        If Not mobjTrx.dtListeAssignationRole Is Nothing Then
            For Each dr As DataRow In mobjTrx.dtListeAssignationRole.Rows
                If dr("Action").Equals("A") And dr("strSupprimer").Equals("False") And dr("DateFin").Equals(String.Empty) Then
                    creation.AjouterRole(dr("ID").ToString)
                ElseIf dr("Action").Equals("A") And dr("strSupprimer").Equals("False") And Not dr("DateFin").Equals(String.Empty) Then
                    creation.AjouterRole(dr("ID").ToString, CType(dr("DateFin"), Date))
                End If
                lstRoles = lstRoles & dr("ID").ToString & ", "
            Next
        End If

        'Pour envoyer un courriel aux administrateurs si Heat n'as pas eu le temps de créer le guid.
        creation.NomDemandeur = ContexteApp.UtilisateurCourant.Prenom & " " & ContexteApp.UtilisateurCourant.Nom

        InscrireMessageREOouRoleEnTrop(creation)

        'Ajouter la remarque si des roles sans REO et des REO sans roles.
        creation.TexteLibre = mobjTrx.strTexteLibre

        If mobjTrx.strValiderREO <> String.Empty Then
            creation.TexteLibre = creation.TexteLibre & vbCrLf & mobjTrx.strValiderREO
        End If

        creation.PieceJointe = mobjTrx.FichierPieceJointe
        creation.ModifComptesSupp = mobjTrx.ObtenirTexteDifferencesComptesSupp(False)

        't208509
        'Mis en commentaire SL + nouvelle ligne
        If String.IsNullOrEmpty(mobjTrx.strGuid) Then
            'If mobjTrx.strGuid Is Nothing ThenF
            TsCaServiceGestnAcces.DemanderCreation(creation, CType(mobjTrx.strDatEffective, Date))
        Else
            TsCaServiceGestnAcces.DemanderCreation(creation, CType(mobjTrx.strDatEffective, Date), mobjTrx.strGuid)
        End If


    End Sub

    Public Sub GererActionsModification()
        'Inscrire Objet Utilisateur
        Dim modification As New TsCdDemndCreationModif(mobjTrx.objUtilisateur.ID)

        'Try
        'Mettre à jour objUtilisateur
        'Vérifier si on a recoché l'approbation
        If mobjTrx.blnApprobation AndAlso
            modification.Utilisateur.ApprobationAccepter Then
            If Not modification.Utilisateur.DateApprobation.Equals(CType(mobjTrx.strDatApprobation, Date)) Then
                modification.Utilisateur.DateApprobation = CType(mobjTrx.strDatApprobation, Date) 'La date a été modifié
            End If
        ElseIf mobjTrx.blnApprobation AndAlso
             Not modification.Utilisateur.ApprobationAccepter Then
            modification.Utilisateur.ApprobationAccepter = True
            modification.Utilisateur.DateApprobation = CType(mobjTrx.strDatApprobation, Date) 'La date a été modifié
        Else
            modification.Utilisateur.ApprobationAccepter = False
        End If



        'Pour la fonction qui envoie le courriel si Heat ne trouve pas le guid.
        modification.NomDemandeur = ContexteApp.UtilisateurCourant.Prenom & " " & ContexteApp.UtilisateurCourant.Nom

        modification.PieceJointe = mobjTrx.FichierPieceJointe
        modification.Organisation = mobjTrx.strOrganisation

        If (mobjTrx.IndAChoisiConserver AndAlso Not mobjTrx.IndComptesSuppModifie) Then
            Dim typesComptes As String = mobjTrx.ObtenirTexteTypesComptesSupp()
            modification.ValidComptesSupp = ContexteApp.ObtenirMessageNonFormate("TS70044I", typesComptes)
        Else
            modification.ModifComptesSupp = mobjTrx.ObtenirTexteDifferencesComptesSupp(False)
        End If

        ''Select Case modification.Utilisateur.ApprobationAccepter
        ''    Case True
        ''        If mobjTrx.blnApprobation = False Then 'Nest plus approuvé
        ''            modification.Utilisateur.ApprobationAccepter = False
        ''        ElseIf Not modification.Utilisateur.DateApprobation.Equals(CType(mobjTrx.strDatApprobation, Date)) Then
        ''            modification.Utilisateur.DateApprobation = CType(mobjTrx.strDatApprobation, Date) 'La date a été modifié
        ''        End If
        ''    Case Else
        ''End Select

        'UA princ
        GererActionsModificationModifUA(modification)

        'Equipes
        GererActionsModificationEquip(modification)

        'Gerer Assignations
        If Not (mobjTrx.dtListeAssignationRole Is Nothing OrElse mobjTrx.dtListeAssignationRole.Rows.Count = 0) Then
            For Each dr As DataRow In mobjTrx.dtListeAssignationRole.Rows
                Dim dateFin As String = dr("DateFin").ToString()
                Dim action As String = dr("Action").ToString()
                Dim indSupprimer As Boolean = dr("strSupprimer").ToString = "True"
                Dim id As String = dr("ID").ToString

                Select Case action
                    Case "O"
                        If indSupprimer Then
                            modification.RetirerRole(id)
                        ElseIf Not indSupprimer And Not String.IsNullOrEmpty(dateFin) Then
                            modification.ModifierRole(id, Date.Parse(dateFin))
                        End If
                    Case "A"
                        If Not indSupprimer And Not String.IsNullOrEmpty(dateFin) Then
                            modification.AjouterRole(id, Date.Parse(dateFin))
                        ElseIf Not indSupprimer And String.IsNullOrEmpty(dateFin) Then
                            modification.AjouterRole(id)
                        End If
                End Select
            Next
        End If

        InscrireMessageREOouRoleEnTrop(modification)

        'Ajouter la remarque si des roles sans REO et des REO sans roles.
        modification.TexteLibre = mobjTrx.strTexteLibre

        If mobjTrx.strValiderREO <> String.Empty Then
            modification.TexteLibre = modification.TexteLibre & vbCrLf & mobjTrx.strValiderREO
        End If

        If mobjTrx.strGuid Is Nothing Then
            TsCaServiceGestnAcces.DemanderModification(modification, CType(mobjTrx.strDatEffective, Date))
        Else
            TsCaServiceGestnAcces.DemanderModification(modification, CType(mobjTrx.strDatEffective, Date), mobjTrx.strGuid)
        End If

        'Catch ex As Exception

        'End Try
    End Sub

    Public Sub GererActionsModificationModifUA(ByRef modification As TsCdDemndCreationModif)
        'Dim strUAPrincASupprimer As String = Nothing
        Dim strIDRoleUAPrincASupprimer As String = Nothing

        If Not mobjTrx.strUaPrincModifie Is Nothing AndAlso
            Not modification.Utilisateur.NoUniteAdmin.Equals(mobjTrx.strUaPrincModifie) Then
            'TsCdUtilisateur
            modification.Utilisateur.NoUniteAdmin = mobjTrx.strUaPrincModifie
            modification.Utilisateur.NomComplet = mobjTrx.strNom & " " & mobjTrx.strPrenom & " (" & mobjTrx.strUaPrincModifie & ")"
        End If

        For Each dr As DataRow In mobjTrx.dtUAUtilisateurCopie.Rows
            Select Case dr("Action").ToString
                Case "A"
                    modification.AjouterRole(dr("IDRole").ToString)
                Case "S"
                    If validerSupprimerRole(dr("IDRole").ToString, modification) Then _
                        modification.RetirerRole(dr("IDRole").ToString)
                Case "C"

            End Select
        Next
    End Sub

    Public Sub GererActionsModificationEquip(ByRef modification As TsCdDemndCreationModif)
        'Vérifier les Équipes
        For Each dr As DataRow In mobjTrx.dtEquipUtilisateurCopie.Rows
            Select Case dr("Action").ToString
                Case "A"
                    modification.AjouterRole(dr("IDRole").ToString.Trim) 'Ajouter Equip Ajoute
                Case "S"
                    If validerSupprimerRole(dr("IDRole").ToString.Trim, modification) Then _
                        modification.RetirerRole(dr("IDRole").ToString.Trim) 'Retirer Equip
                Case "C"

            End Select
        Next

    End Sub

    Private Function ObtenirIDRoleUA(ByVal strNoUA As String) As String
        Dim strRetour As String = Nothing
        Dim intNoligne As Integer = 0

        If Not strNoUA Is Nothing Then
            If Not NiCuADO.PointerDT(mobjTrx.dtListeUnitesAdmin, "No", mobjTrx.strUaPrincModifie, intNoligne) = False Then
                strRetour = mobjTrx.dtListeUnitesAdmin.Rows(intNoligne)("IDRole").ToString()
            End If
        End If
        Return strRetour

    End Function

    Public Function validerSupprimerRole(ByVal strIDRole As String, ByRef modification As TsCdDemndCreationModif) As Boolean
        Dim blnRetour As Boolean = False
        For i As Integer = 0 To modification.Roles.Count - 1 'Vérifier si role existe
            If modification.Roles.Item(i).ID.Equals(strIDRole) Then
                blnRetour = True
                Exit For
            End If
        Next
        Return blnRetour
    End Function

    ''' <summary>
    '''  Gestion des roles et de leur REO en trop.
    '''  Envoie un message aux gestionnaires via le champs "Remarque" qui est généré en XML.
    ''' </summary>
    ''' <remarks></remarks>

    Public Sub InscrireMessageREOouRoleEnTrop(ByVal pDemande As TsCdDemndCreationModif)


        mobjTrx.strValiderREO = String.Empty

        'La liste des roles peut ne pas contenir toutes informations nécessaires pour les requetes selon si c'est un ajout, arrivée ou une modification.
        'Standardiser les données reçues dans les datatables qui serviront pour les requetes.
        'Séparer les roles de type "REO" des autres types de roles.
        Dim dtUAUtil As New DataTable
        Dim dtRoleUtil As New DataTable
        Dim lstRoleEnErreur As IEnumerable(Of DataRow) = Nothing

        'Construire la datatable des UA de l'utilisateur.
        NiCuADO.AjouterDtColonne(dtUAUtil, "No", System.Type.GetType("System.String"), 10, True)
        NiCuADO.AjouterDtColonne(dtUAUtil, "Nom", System.Type.GetType("System.String"), 400, True)
        NiCuADO.AjouterDtColonne(dtUAUtil, "IDRole", System.Type.GetType("System.String"), 400, True)
        NiCuADO.AjouterDtColonne(dtUAUtil, "DateFin", System.Type.GetType("System.String"), 100, True)
        dtUAUtil.AcceptChanges()

        'Construire la table des roles
        NiCuADO.AjouterDtColonne(dtRoleUtil, "Nom", System.Type.GetType("System.String"), 400, True)
        NiCuADO.AjouterDtColonne(dtRoleUtil, "IDRole", System.Type.GetType("System.String"), 400, True)
        NiCuADO.AjouterDtColonne(dtRoleUtil, "ListeUAResponsable", System.Type.GetType("System.String"), 1000, True)
        NiCuADO.AjouterDtColonne(dtRoleUtil, "DateFin", System.Type.GetType("System.String"), 100, True)
        dtRoleUtil.AcceptChanges()

        'Remplir les valeurs dans les tables.
        For Each role As TsCdAssignationRole In pDemande.Roles
            If role.Type = "REO" Then
                'Les UA de l'utilisateur
                Dim dr As DataRow = dtUAUtil.NewRow
                If role.ListeUniteAdministrativeResponsable.Count = 0 Then
                    'Aller chercher les infos dans CA RCM.
                    Dim RoleSage As TsCdRole = TSCuGeneral.RechercherRoleParID(role.ID)
                    dr("No") = RoleSage.ListeUniteAdministrativeResponsable(0)
                Else
                    dr("No") = role.ListeUniteAdministrativeResponsable(0)
                End If

                dr("Nom") = role.Nom
                dr("IDRole") = role.ID
                dr("DateFin") = ""
                dtUAUtil.Rows.Add(dr)
                dtUAUtil.AcceptChanges()
            Else
                'Les roles
                Dim dr As DataRow = dtRoleUtil.NewRow
                If role.ListeUniteAdministrativeResponsable.Count = 0 Then
                    'Un role avec contexte n'a pas d'unité administrative responsable.
                    'Il faut aller chercher les infos dans le role générique.
                    If role.Type.ToUpper() = "RET_C_CONTEXTE" Then
                        'Obtenir le ID du role générique.
                        Dim IDRoleGenerique As String = Left(role.ID, InStrRev(role.ID, "_") - 1)

                        Dim RoleCARCM As TsCdRole = TSCuGeneral.RechercherRoleParID(IDRoleGenerique)
                        dr("ListeUAResponsable") = String.Join(",", RoleCARCM.ListeUniteAdministrativeResponsable.ToArray)
                    Else
                        'C'est un role ajouté normal.  On va chercher les infos directement sur le role
                        'dans CA RCM.
                        Dim RoleCARCM As TsCdRole = TSCuGeneral.RechercherRoleParID(role.ID)
                        dr("ListeUAResponsable") = String.Join(",", RoleCARCM.ListeUniteAdministrativeResponsable.ToArray)
                    End If
                Else

                    dr("ListeUAResponsable") = String.Join(",", role.ListeUniteAdministrativeResponsable.ToArray)
                End If

                dr("Nom") = role.Nom
                dr("IDRole") = role.ID
                If role.DateFin = Nothing Then
                    dr("DateFin") = String.Empty
                Else
                    dr("DateFin") = role.DateFin.ToString
                End If

                dtRoleUtil.Rows.Add(dr)
                dtRoleUtil.AcceptChanges()
            End If
        Next

        Dim strRequete As String = String.Empty
        Dim strRequeteUADemandeur As String = String.Empty

        'Construire la requete pour retrouver tous les roles qui ne font pas partie des unités
        ' administratives de l'utilisateur.
        For Each UA As DataRow In dtUAUtil.Rows
            If strRequete = String.Empty Then
                strRequete = " ListeUAResponsable not like '%" & UA("No").ToString & "%'"
            Else
                strRequete = strRequete & " and " & " ListeUAResponsable not like '%" & UA("No").ToString & "%'"
            End If

        Next

        'Enlever de la liste les roles dont le demandeur n'est pas responsable.
        strRequeteUADemandeur = String.Empty

        For Each drUADemandeur As DataRow In mobjTrx.dtListeUADemandeur.Rows
            If String.IsNullOrEmpty(strRequeteUADemandeur) Then
                strRequeteUADemandeur = " ListeUAResponsable like '%" & drUADemandeur("No").ToString & "%'"
            Else
                strRequeteUADemandeur = strRequeteUADemandeur & " or " & " ListeUAResponsable like '%" & drUADemandeur("No").ToString & "%'"
            End If

        Next

        If Not String.IsNullOrEmpty(strRequeteUADemandeur) Then
            strRequeteUADemandeur = " and ( " & strRequeteUADemandeur & " )"
        End If


        If Not String.IsNullOrEmpty(strRequete) Then
            strRequete = strRequete & strRequeteUADemandeur
            strRequete = strRequete & " and DateFin = ''"
            lstRoleEnErreur = dtRoleUtil.Select(strRequete)
        End If

        If lstRoleEnErreur.Count > 0 Then

            'Construire le message pour envoyer au xml selon le nombre de roles en erreur.
            If lstRoleEnErreur.Count = 1 Then
                'Message un seul
                mobjTrx.strValiderREO = Me.ContexteApp.ObtenirMessageBrut("TS70028I", False).Replace("{0}", "'" & lstRoleEnErreur(0)("Nom").ToString & "'")
            Else
                'Message plusieurs
                Dim strChaineRole As String = String.Empty
                For Each Role As DataRow In lstRoleEnErreur

                    If strChaineRole = String.Empty Then
                        strChaineRole = "'" & Role("Nom").ToString & "'"
                    Else
                        strChaineRole = strChaineRole & ", " & "'" & Role("Nom").ToString & "'"
                    End If
                Next
                If mobjTrx.strValiderREO <> String.Empty Then
                    mobjTrx.strValiderREO = mobjTrx.strValiderREO & vbCrLf & Me.ContexteApp.ObtenirMessageBrut("TS70029I", False).Replace("{0}", "des rôles " & strChaineRole)
                Else
                    mobjTrx.strValiderREO = Me.ContexteApp.ObtenirMessageBrut("TS70029I", False).Replace("{0}", "des rôles " & strChaineRole)
                End If
            End If
        End If

        '2 - Trouver quel REO n'est pas utilisé par les roles assignés
        '*************************************************************
        'Dim dtRolesRestants As DataTable = dtRoleUtil.Copy
        Dim dtRolesRestants As IEnumerable(Of DataRow) = Nothing
        Dim lstREOEnErreur As IEnumerable(Of DataRow) = Nothing
        Dim lstRolesAssocieUAPrinc As IEnumerable(Of DataRow) = Nothing

        'Enlever de la liste les roles dont le demandeur n'est pas responsable.
        strRequeteUADemandeur = String.Empty

        For Each drUADemandeur As DataRow In mobjTrx.dtListeUADemandeur.Rows
            If String.IsNullOrEmpty(strRequeteUADemandeur) Then
                strRequeteUADemandeur = " No like '" & drUADemandeur("No").ToString & "'"
            Else
                strRequeteUADemandeur = strRequeteUADemandeur & " or " & " No like '" & drUADemandeur("No").ToString & "'"
            End If

        Next

        If Not String.IsNullOrEmpty(strRequeteUADemandeur) Then
            strRequeteUADemandeur = " and ( " & strRequeteUADemandeur & " )"
        End If


        'Vérifier si au moins un role pour l'unité administrative principale de l'utilisateur
        Dim NoUAPrinc As String = mobjTrx.strUaPrinc.Replace("REO_", "")
        NoUAPrinc = Left(NoUAPrinc, InStr(NoUAPrinc, "_") - 1)

        'Si cette liste est vide, on ajoute l'UA principale a la liste des REO en trop.
        lstRolesAssocieUAPrinc = dtRoleUtil.Select("ListeUAResponsable like '%" & NoUAPrinc & "%'")

        'Enlever les roles de la liste de roles dont l'ua principale fait partie
        'Le but est d'éviter que la cas particulier suivant se produise :
        'L'utilisateur a 2 roles qui ont tous les 2 les mêmes Unités admin. responsables dont une est l'unité principale
        'et l'autre secondaire. On veut prioriser la conservation de l'unité principale.
        ' EX : 3420,3430.  
        'l'utilisateur a 3430 comme UA principale et 3420 comme unité secondaire. Dans ce cas, on aimerait que TS7 garde
        'le 3430 mais demande à supprimer le 3420.
        'C'est pourquoi on parcourt les roles qui ont l'unité principale et on verifie les autres par la suite.  Si aucun role
        'ne correspond à l'unité principale, on met l'ua principale dans les REO en trop.

        dtRolesRestants = dtRoleUtil.Select("ListeUAResponsable not like '%" & NoUAPrinc & "%'")

        If dtRolesRestants Is Nothing OrElse dtRolesRestants.Count = 0 Then
            'Apres avoir enlever tous les roles associé à l'UA principale, il ne reste plus de roles.
            'Garder dans la liste tous les autres REO de l'utilisateur qui ne sont pas supprimés.
            lstREOEnErreur = dtUAUtil.Select("No not like '" & NoUAPrinc & "'" & strRequeteUADemandeur)
        Else
            'Il reste des roles : Verifier avec les autres REO
            'dtRolesRestants = arrRolesRestants.CopyToDataTable
            Dim strRequeteRoleAssigne As String = String.Empty
            strRequete = String.Empty

            For Each drRole As DataRow In dtRolesRestants
                If strRequeteRoleAssigne = String.Empty Then
                    strRequeteRoleAssigne = drRole("ListeUAResponsable").ToString '.Replace(";", ",")
                Else
                    strRequeteRoleAssigne = strRequeteRoleAssigne & "," & drRole("ListeUAResponsable").ToString '.Replace(";", ",")
                End If
            Next




            If String.IsNullOrEmpty(strRequeteRoleAssigne) Then
                lstREOEnErreur = dtUAUtil.Select(" DateFin = '' and No not like'" & NoUAPrinc & "'" & strRequeteUADemandeur).ToList
            Else
                lstREOEnErreur = dtUAUtil.Select("DateFin = '' and No not in(" & strRequeteRoleAssigne & ") and No not like'" & NoUAPrinc & "'" & strRequeteUADemandeur).ToList
            End If


        End If

        'Si AUCUN role associé à l'unité principale, on l'ajoute à la liste des REOEnErreur si elle est dans uA dont le demandeur est responsable.
        If (lstRolesAssocieUAPrinc Is Nothing OrElse lstRolesAssocieUAPrinc.Count = 0) Then
            lstREOEnErreur = lstREOEnErreur.Union(dtUAUtil.Select("No like '" & NoUAPrinc & "' " & strRequeteUADemandeur))
        End If


        If Not (lstREOEnErreur Is Nothing) AndAlso lstREOEnErreur.Count > 0 Then
            'Message
            If lstREOEnErreur.Count = 1 Then
                If mobjTrx.strValiderREO = String.Empty Then
                    mobjTrx.strValiderREO = Me.ContexteApp.ObtenirMessageBrut("TS70030I", False).Replace("{0}", "'" & lstREOEnErreur(0)("Nom").ToString & "'")
                Else
                    mobjTrx.strValiderREO = mobjTrx.strValiderREO & vbCrLf & Me.ContexteApp.ObtenirMessageBrut("TS70030I", False).Replace("{0}", "'" & lstREOEnErreur(0)("Nom").ToString & "'")
                End If

            Else
                Dim strChaineReo As String = String.Empty

                For Each drreo As DataRow In lstREOEnErreur
                    If strChaineReo = String.Empty Then
                        strChaineReo = "'" & drreo("Nom").ToString & "'"
                    Else
                        strChaineReo = strChaineReo & "," & " '" & drreo("Nom").ToString & "'"
                    End If
                Next
                If mobjTrx.strValiderREO = String.Empty Then
                    mobjTrx.strValiderREO = Me.ContexteApp.ObtenirMessageBrut("TS70031I", False).Replace("{0}", strChaineReo)
                Else
                    mobjTrx.strValiderREO = mobjTrx.strValiderREO & vbCrLf & Me.ContexteApp.ObtenirMessageBrut("TS70031I", False).Replace("{0}", strChaineReo)
                End If

            End If
        End If


    End Sub



End Class
