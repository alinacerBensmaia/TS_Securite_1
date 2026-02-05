Imports System.IO
Imports Rrq.Web.GabaritsPetitsSystemes.FonctionsCommunes.NiCdParametresMsg
Imports Rrq.Web.GabaritsPetitsSystemes.FonctionsCommunes.NiTypeMessage
Imports Rrq.Web.GabaritsPetitsSystemes.Controles.NiCrPage
Imports Rrq.Web.GabaritsPetitsSystemes.ControlesBase
Imports Rrq.Web.GabaritsPetitsSystemes.Utilitaires
Imports Rrq.Securite.GestionAcces
Imports Rrq.Web.AccesUtilisateurs.Utilitaires
Imports System.Collections.Generic
Imports Rrq.Web.ServicesCommunsPetitsSystemes.Utilitaires
Imports System.Linq



Partial Class TS7SAjouterRoles
    Inherits Rrq.Web.GabaritsPetitsSystemes.Controles.NiCrPage
    Protected mobjTrx As TS7I112_RAAccesUtilisateur.TSCdObjetTrx
    Protected mobjAffaire As TS7I112_RAAccesUtilisateur.TSCaAccesUtilisateur
    Public intIndexPanneauOuvert As Integer = 0




#Region " Code généré par le Concepteur Web Form "

    'Cet appel est requis par le Concepteur Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'REMARQUE : la déclaration d'espace réservé suivante est requise par le Concepteur Web Form.
    'Ne pas supprimer ou déplacer.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_evnGererRetourFenDlg(ByVal strNomBoutonAppelant As String, ByVal strValeurBoutonRetour As String) Handles Me.evnGererRetourFenDlg
        If strValeurBoutonRetour = "Recommencer" Then
            InitialiserNouvelleTrx()
            IndAvertissementInterruption = enumAvertissementInterruption.Non
            Response.Redirect(Request.Url.ToString)
        End If
    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN : cet appel de méthode est requis par le Concepteur Web Form
        'Ne le modifiez pas en utilisant l'éditeur de code.
        InitializeComponent()
    End Sub

#End Region

#Region "--- Énumération ---"

    Public Enum enumEnvironnement
        UNITAIRE = 0
        INTEGRATION = 1
        PRODUCTION = 2
    End Enum

#End Region

    Public Overrides ReadOnly Property GroupeADRequis() As String
        Get
            'Return "ROG_#ENVIRONNEMENT#_Gestion des acces demandeur"
            Return String.Concat(Rrq.InfrastructureCommune.Parametres.XuCuConfiguration.ValeurSysteme("TS7", "GroupesSecurite\GRP_ACCES_UTILISATEUR"), ";", _
                                 Rrq.InfrastructureCommune.Parametres.XuCuConfiguration.ValeurSysteme("TS7", "GroupesSecurite\GRP_RESPONSABLE_UA"), ";", _
                                 Rrq.InfrastructureCommune.Parametres.XuCuConfiguration.ValeurSysteme("TS7", "GroupesSecurite\GRP_PILOTAGE"))
        End Get
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load



        If Not IsPostBack Then
            ForcerRafraichissement()
            If ContexteApp Is Nothing Then
                InitialiserNouvelleTrx()
            End If
            Session("RolesSelectionnes") = Nothing
           
            
        End If

        IndAvertissementInterruption = enumAvertissementInterruption.ConditionnelAUnChangement
        mobjTrx = CType(ContexteApp.TrxCourante, TS7I112_RAAccesUtilisateur.TSCdObjetTrx)
        mobjAffaire = New TS7I112_RAAccesUtilisateur.TSCaAccesUtilisateur



        'Mettre la liste des UA du demandeur par ordre de numéro.
        Dim dtTrie As IEnumerable(Of DataRow)

        If Not (mobjTrx.dtListeUADemandeur Is Nothing OrElse mobjTrx.dtListeUADemandeur.Rows.Count = 0) Then
            dtTrie = mobjTrx.dtListeUADemandeur.Select("No <> ''").ToList
            dtTrie.OrderBy(Function(x) CInt(x("No")))

            mobjTrx.dtListeUnitesAdmin = dtTrie.CopyToDataTable
        Else
            mobjTrx.dtListeUnitesAdmin = mobjTrx.dtListeUADemandeur
        End If

        GestionAffichagePremierOuvert()

        If Page.IsPostBack Then

            GererRetourCaseGrille()
        Else
            mobjTrx.IDRoleNonValideCoherence = String.Empty
            mobjTrx.RegleCoherenceEnErreur = String.Empty
            mobjTrx.IDRoleCauseErreur = String.Empty

            'Remplir la liste des régles de cohérence au moins une fois.  Utilisée lors de la validation sur "Ajouter"
            mobjTrx.lstReglesCoherences = mobjAffaire.ObtenirRegleCoherence()


        End If
        rptgrdListeRoles.DataSource = mobjTrx.dtListeUnitesAdmin 'lstUnitAdmin
        rptgrdListeRoles.DataBind()

        

    End Sub

    Public Sub GestionAffichagePremierOuvert()
        'Utilisateur = celui dont on modifie les acces
        'Demandeur = celui qui modifie les acces du demandeur.
        'Seules les unités adm dont le demandeur est responsable sont affichées.
        'La premiere unité administrative présentée ouverte dans cette page est choisie selon les critères suivants :
        '1-L'unité adm principale de l'utilisateur est présente dans la liste des unités admn responsable du demandeur.
        '2-Sinon, on prend l'unité adm principale du demandeur si elle se trouve dans la liste des unités adm responsable du demandeur.
        '3-Sinon, n'importe laquelle des ua responsables du demandeur. La première.
        If Not (mobjTrx.dtListeUnitesAdmin Is Nothing OrElse mobjTrx.dtListeUnitesAdmin.Rows.Count = 0) Then

            Dim intNoLigne As Integer = 0
            If NiCuADO.PointerDT(mobjTrx.dtListeUnitesAdmin, "IDRole", mobjTrx.strUaPrinc, intNoLigne) = True Then
                intIndexPanneauOuvert = intNoLigne
            Else
                If NiCuADO.PointerDT(mobjTrx.dtListeUnitesAdmin, "No", ContexteApp.UtilisateurCourant.NoUniteAdm, intNoLigne) = True Then
                    intIndexPanneauOuvert = intNoLigne
                End If

            End If
        End If




    End Sub
    Protected Sub GererRetourCaseGrille()
        Dim strSelection As String = String.Empty
        Dim lstSelection As New List(Of String)
        Dim evenement As Object = Request.Form("__EVENTTARGET")
        Dim strSelectionASupprimer As String = String.Empty
        'Dans le cas des roles communs à toutes les listes.  Si on en supprime un, il se rajoute tout de suite car il y a au moins un doublon dans le request.form.  Alors,
        'il faut l'empecher de l'ajouter si le role est déja dans la liste de sélection (avait déja été coché) pour ne pas que ses doublons s'ajoute.

        Dim blnAjoutNonValide As Boolean = False
        

        'ajouter les cases sélectionnées.
        For Each key As String In Request.Form.AllKeys
            If InStr(key, "chkSelection") > 0 Then
                If Not (strSelectionASupprimer = key.Replace("chkSelection", "") And blnAjoutNonValide = True) Then
                    'lstSelection.Remove(key.Replace("chkSelection", ""))
                    lstSelection.Add(key.Replace("chkSelection", ""))
                End If

            End If

        Next


        Session("RolesSelectionnes") = lstSelection


    End Sub


    Public Sub rptgrdListeRoles_databound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles rptgrdListeRoles.ItemDataBound

        Dim strExpressionTriInitial As String = Nothing
        Dim ComposantAffichage As Rrq.Web.ServicesCommunsPetitsSystemes.Utilitaires.XlCrAffchAscxPartage = CType(e.Item.FindControl("XLCrUAPrincipal"), Rrq.Web.ServicesCommunsPetitsSystemes.Utilitaires.XlCrAffchAscxPartage)
        Dim PlusMoins As NI1I516_PlusMoinsInfos.NI1P516_PlusMoinsInfos = CType(ComposantAffichage.ControleASCX, NI1I516_PlusMoinsInfos.NI1P516_PlusMoinsInfos)
        Dim pnlListeRoles As Panel = CType(e.Item.FindControl("pnlListeRoles"), Panel)
        Dim blnAfficherRolesMetier As Boolean = True
        Dim blnAfficherRolesTaches As Boolean = True
        Dim dtRoles As New DataTable()
        Dim dr As DataRowView = CType(e.Item.DataItem, DataRowView)
        Dim pnlPaging As Panel = CType(e.Item.FindControl("pnlPaging"), Panel)
        Dim pnlPagingTaches As Panel = CType(e.Item.FindControl("pnlPagingTaches"), Panel)


        Dim hypPage1 As LinkButton = CType(e.Item.FindControl("hypPage1"), LinkButton)
        Dim hypPage2 As LinkButton = CType(e.Item.FindControl("hypPage2"), LinkButton)
        Dim hypPage3 As LinkButton = CType(e.Item.FindControl("hypPage3"), LinkButton)
        Dim hypPage4 As LinkButton = CType(e.Item.FindControl("hypPage4"), LinkButton)
        Dim hypPage5 As LinkButton = CType(e.Item.FindControl("hypPage5"), LinkButton)
        Dim hypPage6 As LinkButton = CType(e.Item.FindControl("hypPage6"), LinkButton)
        Dim hypPage7 As LinkButton = CType(e.Item.FindControl("hypPage7"), LinkButton)
        Dim hypPage8 As LinkButton = CType(e.Item.FindControl("hypPage8"), LinkButton)
        Dim HyPrecedent As LinkButton = CType(e.Item.FindControl("HyPrecedent"), LinkButton)
        Dim hySuivant As LinkButton = CType(e.Item.FindControl("hySuivant"), LinkButton)


        Dim hypPage1Taches As LinkButton = CType(e.Item.FindControl("hypPage1Taches"), LinkButton)
        Dim hypPage2Taches As LinkButton = CType(e.Item.FindControl("hypPage2Taches"), LinkButton)
        Dim hypPage3Taches As LinkButton = CType(e.Item.FindControl("hypPage3Taches"), LinkButton)
        Dim hypPage4Taches As LinkButton = CType(e.Item.FindControl("hypPage4Taches"), LinkButton)
        Dim hypPage5Taches As LinkButton = CType(e.Item.FindControl("hypPage5Taches"), LinkButton)
        Dim hypPage6Taches As LinkButton = CType(e.Item.FindControl("hypPage6Taches"), LinkButton)
        Dim hypPage7Taches As LinkButton = CType(e.Item.FindControl("hypPage7Taches"), LinkButton)
        Dim hypPage8Taches As LinkButton = CType(e.Item.FindControl("hypPage8Taches"), LinkButton)
        Dim HyPrecedentTaches As LinkButton = CType(e.Item.FindControl("HyPrecedentTaches"), LinkButton)
        Dim hySuivantTaches As LinkButton = CType(e.Item.FindControl("hySuivantTaches"), LinkButton)
        Dim hypPage11Taches As LinkButton = CType(e.Item.FindControl("hypPage11Taches"), LinkButton)
        Dim hypPage12Taches As LinkButton = CType(e.Item.FindControl("hypPage12Taches"), LinkButton)

        Dim grdListeRolesMetier As NiCuGrillePageTrie = CType(e.Item.FindControl("grdListeRolesMetier"), NiCuGrillePageTrie)
        Dim grdListeRolesTaches As NiCuGrillePageTrie = CType(e.Item.FindControl("grdListeRolesTaches"), NiCuGrillePageTrie)


        Dim intCurPageIndexMetier As Integer = 0
        Dim intCurPageIndexTaches As Integer = 0



        If Not IsPostBack Then
            strExpressionTriInitial = "Nom ASC"
        End If

        'Load des roles.
        dtRoles = ObtenirRolesParUniteAdmin(dr.Item("No").ToString())

        grdListeRolesMetier.InitialiserControle("TS7I111_AccesUtilisateurAjouterRoles", hypPage1, hypPage2, hypPage3, hypPage4, hypPage5, hypPage6, hypPage7, hypPage8, HyPrecedent, hySuivant, strExpressionTriInitial)
        grdListeRolesTaches.InitialiserControle("TS7I111_AccesUtilisateurAjouterRoles", hypPage1Taches, hypPage2Taches, hypPage3Taches, hypPage4Taches, hypPage5Taches, hypPage6Taches, hypPage7Taches, hypPage8Taches, HyPrecedentTaches, hySuivantTaches, strExpressionTriInitial)

        MAJdtRole(dtRoles)

        Dim tabMetier() As DataRow = Nothing

        Dim tabTaches() As DataRow = Nothing
        tabMetier = dtRoles.Select("ID like 'REM_%'")
        tabTaches = dtRoles.Select("ID like 'RET_%'")

        Dim dtRoleMetier As New DataTable
        If Not (tabMetier Is Nothing OrElse tabMetier.Count = 0) Then
            dtRoleMetier = tabMetier.CopyToDataTable
        Else
            For Each column As DataColumn In dtRoles.Columns
                Dim newColumn As New DataColumn With {.ColumnName = column.ColumnName,
                                                      .DataType = column.DataType,
                                                      .DefaultValue = column.DefaultValue,
                                                      .Expression = column.Expression}
                dtRoleMetier.Columns.Add(newColumn)
            Next
        End If

        Dim dtRoleTaches As New DataTable
        If Not (tabTaches Is Nothing OrElse tabTaches.Count = 0) Then
            dtRoleTaches = tabTaches.CopyToDataTable
        Else
            For Each column As DataColumn In dtRoles.Columns
                Dim newColumn As New DataColumn With {.ColumnName = column.ColumnName,
                                                      .DataType = column.DataType,
                                                      .DefaultValue = column.DefaultValue,
                                                      .Expression = column.Expression}
                dtRoleTaches.Columns.Add(newColumn)
            Next
        End If


        If Page.IsPostBack Then

            grdListeRolesMetier.BinderGrille(dtRoleMetier, grdListeRolesMetier.CurrentPageIndex)
            grdListeRolesTaches.BinderGrille(dtRoleTaches, grdListeRolesTaches.CurrentPageIndex)

            If Not String.IsNullOrEmpty(CStr(Session(grdListeRolesMetier.ClientID))) Then
                intCurPageIndexMetier = CInt(Session(grdListeRolesMetier.ClientID))
            End If

            NiCuRecupererCaseCocheGrille.RecupererSelection(
                "chkSelection",
               dtRoleMetier,
               "ID",
               "SELECT", "O", "N",
               dtRoleMetier.DefaultView.Sort,
               intCurPageIndexMetier + 1,
               grdListeRolesMetier.PageSize)

            If Not String.IsNullOrEmpty(CStr(Session(grdListeRolesTaches.ClientID))) Then
                intCurPageIndexTaches = CInt(Session(grdListeRolesTaches.ClientID))
            End If

            NiCuRecupererCaseCocheGrille.RecupererSelection(
                "chkSelection",
               dtRoleTaches,
               "ID",
               "SELECT", "O", "N",
               dtRoleTaches.DefaultView.Sort,
               intCurPageIndexTaches + 1,
               grdListeRolesMetier.PageSize)

        Else
            grdListeRolesMetier.BinderGrille(dtRoleMetier, grdListeRolesMetier.CurrentPageIndex)
            grdListeRolesTaches.BinderGrille(dtRoleTaches, grdListeRolesTaches.CurrentPageIndex)

        End If

            Dim blnTrouve As Boolean = False

        'Afficher le titre du panel.
        Dim strTitre As String = String.Concat("Rôles de l'unité administrative ", String.Concat(dr.Item("No"), "-", dr.Item("Abbreviation")))

        GererPlusMoins(PlusMoins, pnlListeRoles, e.Item.ItemIndex, strTitre)
        'Verifie si doublon, si non on ajoute la ligne.
        For Each drRole As DataRow In dtRoles.Rows
            blnTrouve = False

            If mobjTrx.dtListeRoleAjout Is Nothing Then
                mobjTrx.dtListeRoleAjout = New DataTable()
                NiCuADO.AjouterDtColonne(mobjTrx.dtListeRoleAjout, "DateFin", System.Type.GetType("System.date"), 10, "", True)
                NiCuADO.AjouterDtColonne(mobjTrx.dtListeRoleAjout, "Description", System.Type.GetType("System.String"), 256, "", True)
                NiCuADO.AjouterDtColonne(mobjTrx.dtListeRoleAjout, "FinPrevu", System.Type.GetType("System.String"), 5, "False", False)
                NiCuADO.AjouterDtColonne(mobjTrx.dtListeRoleAjout, "ID", System.Type.GetType("System.String"), 256, "", True)
                NiCuADO.AjouterDtColonne(mobjTrx.dtListeRoleAjout, "Nom", System.Type.GetType("System.String"), 256, "", True)
                NiCuADO.AjouterDtColonne(mobjTrx.dtListeRoleAjout, "Particulier", System.Type.GetType("System.String"), 5, "False", False)
                NiCuADO.AjouterDtColonne(mobjTrx.dtListeRoleAjout, "Organisationnel", System.Type.GetType("System.String"), 5, "False", False)
                NiCuADO.AjouterDtColonne(mobjTrx.dtListeRoleAjout, "SELECT", System.Type.GetType("System.String"), 1, "False", False)
                NiCuADO.AjouterDtColonne(mobjTrx.dtListeRoleAjout, "ListeUniteAdministrativeResponsable", System.Type.GetType("System.String"), 4000, "False", True)
                NiCuADO.AjouterDtColonne(mobjTrx.dtListeRoleAjout, "Contexte", System.Type.GetType("System.String"), 256, "False", True)
                NiCuADO.AjouterDtColonne(mobjTrx.dtListeRoleAjout, "LienTachesMetiers", System.Type.GetType("System.String"), 256, "False", True)
                NiCuADO.AjouterDtColonne(mobjTrx.dtListeRoleAjout, "DomValContexte", System.Type.GetType("System.String"), 4000, "False", True)
                NiCuADO.AjouterDtColonne(mobjTrx.dtListeRoleAjout, "ContexteOrigine", System.Type.GetType("System.String"), 256, "False", True)
                NiCuADO.AjouterDtColonne(mobjTrx.dtListeRoleAjout, "NomAAfficher", System.Type.GetType("System.String"), 256, "", True)

            End If
            For Each drRoleAjout As DataRow In mobjTrx.dtListeRoleAjout.Rows
                If drRole("ID").ToString = drRoleAjout("ID").ToString Then
                    blnTrouve = True
                    drRoleAjout("SELECT") = drRole("SELECT")
                    drRoleAjout("ListeUniteAdministrativeResponsable") = drRole("ListeUniteAdministrativeResponsable")
                    drRoleAjout.AcceptChanges()

                End If
            Next
            'Pas trouvé, on l'ajoute
            If blnTrouve = False Then
                Dim drAjout As DataRow
                drAjout = mobjTrx.dtListeRoleAjout.NewRow
                drAjout("ID") = drRole("ID")
                drAjout("Description") = drRole("Description")
                drAjout("Nom") = drRole("Nom")
                drAjout("Particulier") = drRole("Particulier")
                drAjout("Organisationnel") = drRole("Organisationnel")
                drAjout("SELECT") = drRole("SELECT")
                drAjout("ListeUniteAdministrativeResponsable") = drRole("ListeUniteAdministrativeResponsable")
                drAjout("Contexte") = String.Empty
                drAjout("LienTachesMetiers") = drRole("LienTachesMetiers")
                drAjout("DomValContexte") = drRole("DomValContexte")
                drAjout("ContexteOrigine") = Trim(drRole("ContexteOrigine").ToString)
                drAjout("NomAAfficher") = drRole("NomAAfficher").ToString()



                mobjTrx.dtListeRoleAjout.Rows.Add(drAjout)

            End If
        Next

        ReafficherPageListeRole(CType(sender, Repeater), e)



    End Sub

    Private Sub InitialiserNouvelleTrx()

        Dim strCodeUsager As String
        strCodeUsager = ContexteApp.UtilisateurCourant.CodeUtilisateur

        'On initialise une nouvelle transaction pour l'utilisateur courant
        mobjTrx = New TS7I112_RAAccesUtilisateur.TSCdObjetTrx(strCodeUsager)

        ContexteApp.TrxCourante = mobjTrx

    End Sub

    Private Sub cmdAjouter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAjouter.Click
        'Réinitaliser les validations de cohérence
        mobjTrx.IDRoleNonValideCoherence = String.Empty
        mobjTrx.RegleCoherenceEnErreur = String.Empty
        mobjTrx.IDRoleCauseErreur = String.Empty

        valErreurMsg.ErrorMessage = String.Empty
        valErreurMsg.IsValid = True

        Dim blnCorrespondanceValide As Boolean = True

        validationUnitaire()

        If Page.IsValid Then

            Dim dtRolesAssigne As IEnumerable(Of DataRow)
            Dim dtRolesAjout As IEnumerable(Of DataRow)
            Dim dtRolesSelectionne As IEnumerable(Of DataRow)
            Dim strRolesAssigne As String = String.Empty

            'validations cohérences.
            If Not (mobjTrx.dtListeAssignationRole Is Nothing OrElse mobjTrx.dtListeAssignationRole.Rows.Count = 0) Then
                dtRolesAssigne = mobjTrx.dtListeAssignationRole.Select("strSupprimer = 'False'")
                strRolesAssigne = String.Join("','", dtRolesAssigne.Select(Function(x) x("ID")).ToList)
            End If
            If Not (mobjTrx.dtListeRoleAjout Is Nothing OrElse mobjTrx.dtListeRoleAjout.Rows.Count = 0) Then
                If String.IsNullOrEmpty(strRolesAssigne) Then
                    dtRolesAjout = mobjTrx.dtListeRoleAjout.Select("SELECT = 'O'")
                Else
                    'Il peut y avoir des doublons dans la table car on peut avoir un role assigné dans GererRole, mais choisir le même role dans 
                    'AjouterRole.
                    dtRolesAjout = mobjTrx.dtListeRoleAjout.Select("SELECT = 'O' and ID not in('" & strRolesAssigne & "')")
                End If
                
            End If
            If Not (dtRolesAssigne Is Nothing OrElse dtRolesAssigne.Count = 0) And _
                Not (dtRolesAjout Is Nothing OrElse dtRolesAjout.Count = 0) Then
                dtRolesSelectionne = dtRolesAssigne.Union(dtRolesAjout)
            Else
                'Au moins une datatable est vide, on met les valeurs de l'autre datatable dans dtRoleSelectionne
                If Not (dtRolesAssigne Is Nothing OrElse dtRolesAssigne.Count = 0) Then
                    dtRolesSelectionne = dtRolesAssigne
                End If
                If Not (dtRolesAjout Is Nothing OrElse dtRolesAjout.Count = 0) Then
                    dtRolesSelectionne = dtRolesAjout
                End If
            End If


            Dim dtTableRoles As New DataTable
            dtTableRoles = dtRolesSelectionne.CopyToDataTable

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
                    paramMessage = {DirectCast(TSCuGeneral.RechercherRoleParID(mobjTrx.IDRoleNonValideCoherence), TsCdRole).Nom, _
                                               mobjTrx.IDRoleCauseErreur}
                End If


                Select Case mobjTrx.RegleCoherenceEnErreur
                    Case "I"

                        valErreurMsg.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("TS70037E", False, paramMessage)
                        valErreurMsg.IsValid = False
                        ReafficherPageListeRole(rptgrdListeRoles, Nothing)
                    Case "O"
                        valErreurMsg.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("TS70038E", False, paramMessage)
                        valErreurMsg.IsValid = False
                        ReafficherPageListeRole(rptgrdListeRoles, Nothing)
                    Case "CM"
                        valErreurMsg.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("TS70040E", False, paramMessage)
                        valErreurMsg.IsValid = False
                        ReafficherPageListeRole(rptgrdListeRoles, Nothing)
                    Case "CU"
                        valErreurMsg.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("TS70039E", False, paramMessage)
                        valErreurMsg.IsValid = False
                        ReafficherPageListeRole(rptgrdListeRoles, Nothing)

                End Select



            Else


                If Page.IsValid Then
                    IndAvertissementInterruption = enumAvertissementInterruption.Oui
                    If mobjTrx.dtListeAssignationRole Is Nothing Then
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
                            If NiCuADO.PointerDT(mobjTrx.dtListeAssignationRole, "ID", CType(dr("ID"), String), intNoLigne) = False Then

                                Dim drAjout As DataRow
                                drAjout = mobjTrx.dtListeAssignationRole.NewRow

                                drAjout("ID") = dr("ID")
                                drAjout("Description") = dr("Description")
                                drAjout("Nom") = dr("Nom")
                                drAjout("Particulier") = dr("Particulier")
                                drAjout("Organisationnel") = dr("Organisationnel")
                                drAjout("FinPrevu") = "False"
                                drAjout("Action") = "A"
                                drAjout("ListeUniteAdministrativeResponsable") = dr("ListeUniteAdministrativeResponsable")
                                drAjout("Contexte") = String.Empty
                                drAjout("LienTachesMetiers") = dr("LienTachesMetiers")
                                drAjout("DomValContexte") = dr("DomValContexte")
                                drAjout("ContexteOrigine") = String.Empty
                                drAjout("strChangerContexte") = "False"
                                drAjout("NomAAfficher") = dr("NomAAfficher")


                                mobjTrx.dtListeAssignationRole.Rows.Add(drAjout)
                                mobjTrx.blnApprobation = False 'il y a eu ajout -> approbation = false
                            Else
                                mobjTrx.dtListeAssignationRole.Rows(intNoLigne).Item("strSupprimer") = "False"
                                mobjTrx.dtListeAssignationRole.Rows(intNoLigne).Item("strChangerContexte") = "False"
                            End If
                        End If

                    Next
                    'Si approuvé Vérifier s'il y a eu ajout dans la liste
                    If mobjTrx.blnApprobation Then mobjTrx.blnApprobation = _
                               Not mobjAffaire.ValiderSiListeAjout(mobjTrx.dtListeAssignationRole)

                    Dim strCodetrans As String = Request.QueryString("codetrans")
                    Response.Redirect(String.Concat(TSCuDomVal.PAGE_ROLE_EMPLOYE, "?codetrans=", strCodetrans, "&Prov=TS7SAjouterRoles"))
                End If
            End If
        End If
            ' End If
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

   

    Public Function ObtenirInfoBulleDetail(ByVal strNomRole As String) As String
        Return String.Concat("Afficher le détail du rôle ", strNomRole)
    End Function


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
        oParam.Bouton1 = New  _
         TS7CdParametresFenComplDetailRole.TS7CdBouton(aBoutons(0).TexteBouton, aBoutons(0).Valeur, aBoutons(0).EstBoutonAnnulation, aBoutons(0).EstBoutonAnnulation)

        mobjCtrlOuvrir.OuvrirFenetreModaleAvecRetour(oParam)
    End Sub

    Protected Sub cmdPrecedent_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrecedent.Click
        Dim strURL As String = Session("TS7I111AccesUtilisateur").ToString
        Dim intPos As Integer = strURL.IndexOf("Codetrans")
        If intPos < 0 Then
            Response.Redirect(String.Concat(Session("TS7I111AccesUtilisateur"), "?Codetrans=ts7role&Prov=TS7SAjouterRoles"))
        Else
            Response.Redirect(String.Concat(Session("TS7I111AccesUtilisateur"), "&Prov=TS7SAjouterRoles"))
        End If
    End Sub

    Private Function validationUnitaire() As Boolean
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
    

    Private Function ObtenirRolesParUniteAdmin(ByVal pstrUnitAdmin As String) As DataTable


        Dim strCritereRechercher As String = Nothing

        Dim rolesUA As List(Of TsCdRole) = TsCaServiceGestnAcces.ObtenirRolesUniteAdmin(pstrUnitAdmin)
        Dim dtRoles As DataTable = TSCuGeneral.RoleDataTable(rolesUA)
        NiCuADO.AjouterDtColonne(dtRoles, "SELECT", System.Type.GetType("System.String"), 1, "N")

        If rolesUA.Count > 0 Then
            'Retirer les rôles organisationnels
            For i As Integer = dtRoles.Rows.Count - 1 To 0 Step -1
                If dtRoles.Rows(i).Item("Organisationnel").Equals("True") Then
                    dtRoles.Rows(i).Delete()
                End If
            Next

            'T208704 :Retirer les roles que l'utilisateur a déja
            'En commentaire pour l'instant.  Cette modification sera peut-etre en entretien mais pas dans
            ' le cadre du projet 309
            'For i As Integer = 0 To mobjTrx.dtListeAssignationRole.Rows.Count - 1
            '    Dim blnTrouve As Boolean = False
            '    Dim intNoLigne As Integer = Nothing

            '    blnTrouve = NiCuADO.PointerDT(dtRoles, "ID", mobjTrx.dtListeAssignationRole.Rows(i)("ID").ToString, intNoLigne)
            '    If blnTrouve Then
            '        dtRoles.Rows(intNoLigne).Delete()
            '        dtRoles.AcceptChanges()
            '    End If
            'Next
        End If

        dtRoles.AcceptChanges()
        Return dtRoles
    End Function

    Private Sub GererPlusMoins(ByRef objUAPrincipalPlusMoins As NI1I516_PlusMoinsInfos.NI1P516_PlusMoinsInfos, ByRef pnlListeRoles As Panel, ByVal pintIndex As Integer, ByVal pstrTitre As String)
        If Not IsPostBack Then

            ' If pintIndex = 0 Then
            If pintIndex = intIndexPanneauOuvert Then
                objUAPrincipalPlusMoins.InitialiserControleSousTitre(pnlListeRoles, _
                                pstrTitre, _
                                NI1I516_PlusMoinsInfos.NI1P516_PlusMoinsInfos.enumEclatement.Eclater)
            Else
                objUAPrincipalPlusMoins.InitialiserControleSousTitre(pnlListeRoles, _
                                pstrTitre, _
                                NI1I516_PlusMoinsInfos.NI1P516_PlusMoinsInfos.enumEclatement.Reduire)
            End If


        Else
            'Initialisation du contrôle plus moins pour afficher la liste des tables
            objUAPrincipalPlusMoins.InitialiserControleSousTitre(pnlListeRoles, _
            pstrTitre)


        End If

        'intIndexPanneauOuvert = 0

    End Sub

    '    Protected Sub GererPagination(ByVal sender As Object, ByVal e As System.EventArgs)
    'Protected Sub GererPagination(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs)
    Protected Sub GererPagination(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs)
        Dim dpRoles As LinkButton = CType(sender, LinkButton)
        Dim pnlPaging As Panel = CType(dpRoles.Parent, Panel)
        Dim pnlListeRoles As Panel = CType(pnlPaging.Parent, Panel)
        Dim monrepeateritem As RepeaterItem = CType(pnlListeRoles.Parent, RepeaterItem)
        Dim grdLstRoles As NiCuGrillePageTrie
        Dim blnMetier As Boolean

        If InStr(dpRoles.ID, "Taches") > 0 Then
            'Prendre la grille des roles taches
            grdLstRoles = CType(monrepeateritem.FindControl("grdListeRolesTaches"), NiCuGrillePageTrie)
            blnMetier = False
        Else
            'Prendre la grille des roles métier
            grdLstRoles = CType(monrepeateritem.FindControl("grdListeRolesMetier"), NiCuGrillePageTrie)
            blnMetier = True
        End If

        Select Case dpRoles.CommandArgument
            Case "debut"
                grdLstRoles.CurrentPageIndex = 0
            Case "prev"
                grdLstRoles.CurrentPageIndex = grdLstRoles.CurrentPageIndex - 1
            Case "0"
                grdLstRoles.CurrentPageIndex = 0
            Case "Page1"
                grdLstRoles.CurrentPageIndex = 0
            Case "Page2", "1"
                grdLstRoles.CurrentPageIndex = 1
            Case "Page3", "2"
                grdLstRoles.CurrentPageIndex = 2
            Case "Page4", "3"
                grdLstRoles.CurrentPageIndex = 3
            Case "Page5", "4"
                grdLstRoles.CurrentPageIndex = 4
            Case "Page6", "5"
                grdLstRoles.CurrentPageIndex = 5
            Case "Page7", "6"
                grdLstRoles.CurrentPageIndex = 6
            Case "Page8", "7"
                grdLstRoles.CurrentPageIndex = 7
            Case "next"
                grdLstRoles.CurrentPageIndex = grdLstRoles.CurrentPageIndex + 1
            Case "fin"
                grdLstRoles.CurrentPageIndex = CInt(grdLstRoles.Items.Count / grdLstRoles.PageSize)
        End Select

        Session(grdLstRoles.ClientID) = grdLstRoles.CurrentPageIndex

        grdLstRoles.PageButtonClick(sender, e)
        grdLstRoles.DataBind()



    End Sub
    Protected Sub grdListeRoles_ItemCommand(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
        Dim strDetailRole As String = e.CommandArgument.ToString
        Select Case e.CommandName
            Case "AfficherDetail"
                InitialiserFenComplDetailRole(strDetailRole)
        End Select

    End Sub

    Protected Sub ReafficherPageListeRole(ByRef rptAffichePage As Repeater, ByVal e As System.EventArgs)

        For Each repitem As RepeaterItem In rptAffichePage.Items
            Dim ListeRolesMetier As NiCuGrillePageTrie = CType(repitem.FindControl("grdListeRolesMetier"), NiCuGrillePageTrie)
            Dim ListeRolesTaches As NiCuGrillePageTrie = CType(repitem.FindControl("grdListeRolesTaches"), NiCuGrillePageTrie)
            'obtenir le current index en session

            Select Case CInt(Session(ListeRolesMetier.ClientID))
                Case 0
                    Dim hypPage1 As LinkButton = CType(repitem.FindControl("hypPage1"), LinkButton)
                    ListeRolesMetier.PageButtonClick(hypPage1, e)
                    ListeRolesMetier.DataBind()
                Case 1
                    Dim hypPage2 As LinkButton = CType(repitem.FindControl("hypPage2"), LinkButton)
                    ListeRolesMetier.PageButtonClick(hypPage2, e)
                    ListeRolesMetier.DataBind()
                Case 2
                    Dim hypPage3 As LinkButton = CType(repitem.FindControl("hypPage3"), LinkButton)
                    ListeRolesMetier.PageButtonClick(hypPage3, e)
                    ListeRolesMetier.DataBind()
                Case 3
                    Dim hypPage4 As LinkButton = CType(repitem.FindControl("hypPage4"), LinkButton)
                    ListeRolesMetier.PageButtonClick(hypPage4, e)
                    ListeRolesMetier.DataBind()
                Case 4
                    Dim hypPage5 As LinkButton = CType(repitem.FindControl("hypPage5"), LinkButton)
                    ListeRolesMetier.PageButtonClick(hypPage5, e)
                    ListeRolesMetier.DataBind()
                Case 5
                    Dim hypPage6 As LinkButton = CType(repitem.FindControl("hypPage6"), LinkButton)
                    ListeRolesMetier.PageButtonClick(hypPage6, e)
                    ListeRolesMetier.DataBind()
                Case 6
                    Dim hypPage7 As LinkButton = CType(repitem.FindControl("hypPage7"), LinkButton)
                    ListeRolesMetier.PageButtonClick(hypPage7, e)
                    ListeRolesMetier.DataBind()
                Case 7
                    Dim hypPage8 As LinkButton = CType(repitem.FindControl("hypPage8"), LinkButton)
                    ListeRolesMetier.PageButtonClick(hypPage8, e)
                    ListeRolesMetier.DataBind()
                Case 8
                    Dim hypPage9 As LinkButton = CType(repitem.FindControl("hypPage9"), LinkButton)
                    ListeRolesMetier.PageButtonClick(hypPage9, e)
                    ListeRolesMetier.DataBind()

            End Select

            Select Case CInt(Session(ListeRolesTaches.ClientID))
                Case 0
                    Dim hypPage1 As LinkButton = CType(repitem.FindControl("hypPage1Taches"), LinkButton)
                    ListeRolesTaches.PageButtonClick(hypPage1, e)
                    ListeRolesTaches.DataBind()
                Case 1
                    Dim hypPage2 As LinkButton = CType(repitem.FindControl("hypPage2Taches"), LinkButton)
                    ListeRolesTaches.PageButtonClick(hypPage2, e)
                    ListeRolesTaches.DataBind()
                Case 2
                    Dim hypPage3 As LinkButton = CType(repitem.FindControl("hypPage3Taches"), LinkButton)
                    ListeRolesTaches.PageButtonClick(hypPage3, e)
                    ListeRolesTaches.DataBind()
                Case 3
                    Dim hypPage4 As LinkButton = CType(repitem.FindControl("hypPage4Taches"), LinkButton)
                    ListeRolesTaches.PageButtonClick(hypPage4, e)
                    ListeRolesTaches.DataBind()
                Case 4
                    Dim hypPage5 As LinkButton = CType(repitem.FindControl("hypPage5Taches"), LinkButton)
                    ListeRolesTaches.PageButtonClick(hypPage5, e)
                    ListeRolesTaches.DataBind()
                Case 5
                    Dim hypPage6 As LinkButton = CType(repitem.FindControl("hypPage6Taches"), LinkButton)
                    ListeRolesTaches.PageButtonClick(hypPage6, e)
                    ListeRolesTaches.DataBind()
                Case 6
                    Dim hypPage7 As LinkButton = CType(repitem.FindControl("hypPage7Taches"), LinkButton)
                    ListeRolesTaches.PageButtonClick(hypPage7, e)
                    ListeRolesTaches.DataBind()
                Case 7
                    Dim hypPage8 As LinkButton = CType(repitem.FindControl("hypPage8Taches"), LinkButton)
                    ListeRolesTaches.PageButtonClick(hypPage8, e)
                    ListeRolesTaches.DataBind()
                Case 8
                    'Dim hypPage9 As LinkButton = CType(repitem.FindControl("hypPage9"), LinkButton)
                    'ListeRolesTaches.PageButtonClick(hypPage9, e)
                    'ListeRolesTaches.DataBind()

            End Select

        Next
    End Sub

    Protected Sub rptgrdListeRoles_itemCommand(ByVal sender As Object, ByVal e As RepeaterCommandEventArgs) Handles rptgrdListeRoles.ItemCommand
        Dim rptAffiche As Repeater = CType(sender, Repeater)

        'Pagination
        If e.CommandName = "Resultat" Then
            Dim lnkPage As LinkButton = CType(e.CommandSource, LinkButton)
            GererPagination(lnkPage, e)

        Else 'PlusMoins
            'pour afficher le panneau, on refait un postback.  Il faut remettre les listes de roles sur les bonnes pages.
            ReafficherPageListeRole(rptAffiche, e)
        End If


    End Sub


    Protected Sub MAJdtRole(ByRef dtRole As DataTable)
        Dim ListeRole As New List(Of String)
        If Not Session("RolesSelectionnes") Is Nothing Then
            ListeRole = CType(Session("RolesSelectionnes"), List(Of String))
        End If

        'Supprimer tous les roles sélectionnées avant
        For Each drRole As DataRow In dtRole.Rows

            drRole("SELECT") = "N"
            dtRole.AcceptChanges()

        Next

        For Each Role As String In ListeRole

            For Each drRole As DataRow In dtRole.Rows
                If Role = drRole("ID").ToString() Then
                    drRole("SELECT") = "O"
                    dtRole.AcceptChanges()
                End If
            Next
        Next
    End Sub
#Region "Fonctions utilisé par le fichier aspx"
    Public Function AfficherSuggestion(ByVal IDRole As String, ByVal NomGrille As String, ByVal strChaineAComparer As String, ByVal pPosition As String) As String


        If mobjTrx.IDRoleNonValideCoherence = String.Empty Then
            'Aucune erreur trouvée dans les validations
            Return String.Empty
        Else
            Return TSCuGeneral.AfficherSuggestionCoherence(IDRole, NomGrille, strChaineAComparer, pPosition, _
                                                           mobjTrx.IDRoleNonValideCoherence, mobjTrx.RegleCoherenceEnErreur, mobjTrx.lstReglesCoherences)


        End If
        Return String.Empty
    End Function

    Public Function CocherCase(ByVal pstrSelectionRoleAjout As String, ByVal pIDRole As String) As Boolean

        'Coche la case du role si ; 
        '"SELECT" = "O" pour les roles nouvellement ajoutés.
        'Fait partie des roles déja assignés qui ne sont pas retirés.
        If CBool(pstrSelectionRoleAjout = "O") Then
            Return True
        End If

        If Not (mobjTrx.dtListeAssignationRole Is Nothing OrElse mobjTrx.dtListeAssignationRole.Rows.Count = 0) Then
            If CBool(mobjTrx.dtListeAssignationRole.Select("ID like '" & pIDRole & "' and strSupprimer = 'False'").Count > 0) Then
                Return True
            End If
        End If

        Return False
    End Function
    ''' <summary>
    ''' Précocher les roles déja assignés qui ne sont pas retirés.
    ''' </summary>
    ''' <param name="pIDRole"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DesactiverCaseCoche(ByVal pIDRole As String) As Boolean
        'True = case coche enabled
        'False = case coche disabled
        If Not (mobjTrx.dtListeAssignationRole Is Nothing OrElse mobjTrx.dtListeAssignationRole.Rows.Count = 0) Then
            Return Not CBool(mobjTrx.dtListeAssignationRole.Select("ID like '" & pIDRole & "' and strSupprimer = 'False'").Count > 0)
        End If

        Return True
    End Function
#End Region

End Class
