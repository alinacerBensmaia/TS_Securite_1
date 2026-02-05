Imports System.Collections.Generic
Imports Rrq.Web.AccesUtilisateurs.Utilitaires
Imports Rrq.Web.ServicesCommunsPetitsSystemes.Utilitaires
Imports Rrq.Securite.GestionAcces
Imports System.Linq
Imports Rrq.Web.GabaritsPetitsSystemes.ControlesBase
Imports Rrq.Web.GabaritsPetitsSystemes.Utilitaires
Imports System.Xml

Partial Public Class rap_ParUnitAdm
    Inherits Rrq.Web.GabaritsPetitsSystemes.Controles.NiCrPage



    Protected mobjTrx As TS7I112_RAAccesUtilisateur.TSCdObjetTrx
    Protected mobjAffaire As TS7I112_RAAccesUtilisateur.TSCaAccesUtilisateur


    Public Enum enumEnvironnement
        UNITAIRE = 0
        INTEGRATION = 1
        PRODUCTION = 2
    End Enum
    Public lstUARapport As New List(Of Integer)

    Public Overrides ReadOnly Property GroupeADRequis() As String
        Get
            'Changement du nom du groupe pour la gestion des roles. 2013-12-11
            'Return "GTS7#ENVIRONNEMENT#GestionAcces"
            'Return "ROG_#ENVIRONNEMENT#_Gestion des acces consultation"
            Return String.Concat(Rrq.InfrastructureCommune.Parametres.XuCuConfiguration.ValeurSysteme("TS7", "GroupesSecurite\GRP_ACCES_RAPPORTS"), ";",
                                 Rrq.InfrastructureCommune.Parametres.XuCuConfiguration.ValeurSysteme("TS7", "GroupesSecurite\GRP_RESPONSABLE_UA"), ";",
                                 Rrq.InfrastructureCommune.Parametres.XuCuConfiguration.ValeurSysteme("TS7", "GroupesSecurite\GRP_PILOTAGE"))
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim ConditionFormatting As String = Request.QueryString("ConditionFormatting")
        Dim uniteSelectionnee As String = Request.QueryString("uniteSelectionnee")

        If ConditionFormatting = "MultipleUnite" Then
            ValErreur.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("TS70042E", False, uniteSelectionnee)
            ValErreur.IsValid = False
        ElseIf ConditionFormatting = "UniteSeule" Then
            ValErreur.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("TS70043E", False, uniteSelectionnee)
            ValErreur.IsValid = False
        End If

        If Not IsPostBack Then
            ForcerRafraichissement()

            InitialiserNouvelleTrx()

        End If

        IndAvertissementInterruption = enumAvertissementInterruption.ConditionnelAUnChangement

        mobjTrx = CType(ContexteApp.TrxCourante, TS7I112_RAAccesUtilisateur.TSCdObjetTrx)
        mobjAffaire = New TS7I112_RAAccesUtilisateur.TSCaAccesUtilisateur

        Dim ComposantAffichage As Rrq.Web.ServicesCommunsPetitsSystemes.Utilitaires.XlCrAffchAscxPartage = CType(Me.FindControl("XLCrUAAutres"), Rrq.Web.ServicesCommunsPetitsSystemes.Utilitaires.XlCrAffchAscxPartage)
        Dim PlusMoins As New NI1I516_PlusMoinsInfos.NI1P516_PlusMoinsInfos
        PlusMoins = CType(ComposantAffichage.ControleASCX, NI1I516_PlusMoinsInfos.NI1P516_PlusMoinsInfos)


        If IsPostBack Then

            If Not (mobjTrx.dtListeUADemandeur Is Nothing OrElse mobjTrx.dtListeUADemandeur.Rows.Count = 0) Then

                NiCuRecupererCaseCocheGrille.RecupererSelection(
                  "chkSelection",
                  mobjTrx.dtListeUADemandeur,
                  "IDRole",
                  "SELECT", "O", "N",
                  mobjTrx.dtListeUADemandeur.DefaultView.Sort,
                  grdUADemandeur.CurrentPageIndex + 1,
                  grdUADemandeur.PageSize)
            End If

            Dim blnActionPlusMoins As Boolean = True
            For i As Integer = 0 To HttpContext.Current.Request.Form().AllKeys.Count - 1
                'Chercher les occurence d'une case à cochée.  Sinon c'est que le demandeur a fait "Plus""Moins"
                If InStr(HttpContext.Current.Request.Form().Keys(i).ToString, "chkSelectionAutresUA") > 0 Then
                    blnActionPlusMoins = False
                End If

            Next

            If Not blnActionPlusMoins Then
                'Si le demandeur a cliquer sur "+" ou "-", on n'écrase pas les valeurs déja sélectionnées.
                NiCuRecupererCaseCocheGrille.RecupererSelection(
                  "chkSelectionAutresUA",
                  mobjTrx.dtListeAutresUA,
                  "IDRole",
                  "SELECT", "O", "N",
                  mobjTrx.dtListeAutresUA.DefaultView.Sort,
                  grdUAAutres.CurrentPageIndex + 1,
                  grdUAAutres.PageSize)
            End If



        Else
            mobjTrx.dtListeUnitesAdmin = TSCuGeneral.UniteAdminDataTable(TsCaServiceGestnAcces.ObtenirListeUnitesAdmin())
            NiCuADO.AjouterDtColonne(mobjTrx.dtListeUnitesAdmin, "SELECT", System.Type.GetType("System.String"), 1, "N")
            'Mettre la liste des UA du demandeur par ordre de numéro.
            Dim dtTrie As IEnumerable(Of DataRow)

            If Not (mobjTrx.dtListeUnitesAdmin Is Nothing OrElse mobjTrx.dtListeUnitesAdmin.Rows.Count = 0) Then
                dtTrie = mobjTrx.dtListeUnitesAdmin.Select("No <> ''").ToList
                dtTrie.OrderBy(Function(x) CInt(x("No")))

                mobjTrx.dtListeUnitesAdmin = dtTrie.CopyToDataTable

            End If

            SeparerUA()


        End If

        Dim strTitrePanneau As String = AfficherTitreAutresUA()

        'Déterminer si le panneau est ouvert ou non.
        '- si pas d'unité de demandeur = Ouvert, sinon, fermé.
        If Not Page.IsPostBack Then
            If mobjTrx.dtListeUADemandeur Is Nothing OrElse mobjTrx.dtListeUADemandeur.Rows.Count = 0 Then
                'Ouvert
                GererPlusMoins(PlusMoins, pnlListeRoles, True, strTitrePanneau)
            Else
                'Fermé
                GererPlusMoins(PlusMoins, pnlListeRoles, False, strTitrePanneau)
            End If
        Else
            'selon ce qui était affiché.
            GererPlusMoins(PlusMoins, pnlListeRoles, False, strTitrePanneau)
        End If

        grdUADemandeur.DataSource = mobjTrx.dtListeUADemandeur
        grdUAAutres.DataSource = mobjTrx.dtListeAutresUA
        grdUADemandeur.DataBind()
        grdUAAutres.DataBind()


    End Sub

    Private Sub InitialiserNouvelleTrx()

        Dim strCodeUsager As String
        strCodeUsager = ContexteApp.UtilisateurCourant.CodeUtilisateur

        'On initialise une nouvelle transaction pour l'utilisateur courant
        mobjTrx = New TS7I112_RAAccesUtilisateur.TSCdObjetTrx(strCodeUsager)

        ContexteApp.TrxCourante = mobjTrx

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


        strRecherche = "ROR_" & ObtenirEnvironnementStr(System.Web.HttpContext.Current) & "_TS7_GestionAcces"

        'Pour tout le tableau des groupes.  Rechercher les groupes "GestionsAcces" pour obtenir les unité administratives.
        For Each groupe As String In Groupes
            position = InStr(groupe, strRecherche)
            If position <> 0 Then
                lstUnitAdmin.Add(groupe.Replace(strRecherche, ""))
            End If
        Next
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
    Public Sub SeparerUA()

        If Not (mobjTrx.dtListeUnitesAdmin Is Nothing OrElse mobjTrx.dtListeUnitesAdmin.Rows.Count = 0) Then
            Dim lstUnitAdmin As List(Of String)


            'Recherche des autres unité administrative reliées aux groupes de sécurité de l'utilisateur.
            lstUnitAdmin = ObtenirUnitAdminADParAcces()
            Dim strRequete As String = String.Empty

            If Not lstUnitAdmin Is Nothing Then
                For Each UA As String In lstUnitAdmin
                    If IsNumeric(UA) Then
                        If strRequete = String.Empty Then
                            strRequete = UA
                        Else
                            strRequete = strRequete & ", " & UA
                        End If
                    End If

                Next
            End If

            Dim lstUADemandeur As IEnumerable(Of DataRow)
            Dim lstAutresUA As IEnumerable(Of DataRow)

            If strRequete = String.Empty Then
                'Le demandeur n'a pas d'unité admn, on lui fait afficher tout dans la liste de toutes les UA.
                lstUADemandeur = Nothing
                lstAutresUA = mobjTrx.dtListeUnitesAdmin.Select("IDRole <> ''")

                If Not (lstUADemandeur Is Nothing OrElse lstUADemandeur.Count = 0) Then
                    mobjTrx.dtListeUADemandeur = lstUADemandeur.CopyToDataTable
                End If

                If Not (lstAutresUA Is Nothing OrElse lstAutresUA.Count = 0) Then
                    mobjTrx.dtListeAutresUA = lstAutresUA.CopyToDataTable
                End If
            Else

                'Sépare les unités administratives du demandeur des autres unités administratives.
                lstUADemandeur = mobjTrx.dtListeUnitesAdmin.Select("No in(" & strRequete & ")")
                lstAutresUA = mobjTrx.dtListeUnitesAdmin.Select("No not in(" & strRequete & ")")

                If Not (lstUADemandeur Is Nothing OrElse lstUADemandeur.Count = 0) Then
                    mobjTrx.dtListeUADemandeur = lstUADemandeur.CopyToDataTable
                End If

                If Not (lstAutresUA Is Nothing OrElse lstAutresUA.Count = 0) Then
                    mobjTrx.dtListeAutresUA = lstAutresUA.CopyToDataTable
                End If
            End If
        End If


    End Sub

    Public Sub cmdAfficher_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAfficher.Click

        ValidationsUnitaire()

        If Page.IsValid Then
            'Pour faire afficher le rapport, on le redirrige vers la page afficherRapport.
            'Raison : Si un message d'erreur était affiché dans la page, le fait de généré le rapport envoyait
            'la réponse de la page à excel mais on voyait encore le message d'erreur.
            'Pour régler le cas, on laisse la page sans message d'erreur s'afficher et on redirrige vers la page 
            'd'affichage de rapport après.
            Dim meta As New HtmlMeta()
            meta.HttpEquiv = "Refresh"
            'meta.Content = "1;url=AfficherRapport.aspx?UA=1000,1010"


            Dim strUA As String = String.Empty
            strUA = String.Join(",", lstUARapport.Select(Function(x) x.ToString()).ToArray())

            meta.Content = "1;url=AfficherRapport.aspx?UA=" & strUA
            Me.Controls.Add(meta)

        End If

    End Sub

    Public Sub ValidationsUnitaire()

        Dim lstUADemandeurSelect As IEnumerable(Of DataRow) = Nothing
        Dim lstUAAutresSelect As IEnumerable(Of DataRow) = Nothing

        lstUARapport.Clear()

        'Les unités administratives du demandeur
        If Not (mobjTrx.dtListeUADemandeur Is Nothing OrElse mobjTrx.dtListeUADemandeur.Rows.Count = 0) Then
            lstUADemandeurSelect = mobjTrx.dtListeUADemandeur.Select("SELECT LIKE 'O'")
        End If

        'Les autres
        If Not (mobjTrx.dtListeAutresUA Is Nothing OrElse mobjTrx.dtListeAutresUA.Rows.Count = 0) Then
            lstUAAutresSelect = mobjTrx.dtListeAutresUA.Select("SELECT LIKE 'O'")
        End If

        'Mettre les no des UA sélectionnées dans la liste de UA à envoyer au rapport
        If Not (lstUAAutresSelect Is Nothing OrElse lstUAAutresSelect.Count = 0) Then
            For Each drUA As DataRow In lstUAAutresSelect
                lstUARapport.Add(CInt(drUA("No")))
            Next
        End If

        If Not (lstUADemandeurSelect Is Nothing OrElse lstUADemandeurSelect.Count = 0) Then
            For Each drUA As DataRow In lstUADemandeurSelect
                lstUARapport.Add(CInt(drUA("No")))
            Next
        End If

        'Validations du nombre minimum et maximum de UA sélectionnées.
        If lstUARapport Is Nothing OrElse lstUARapport.Count = 0 OrElse lstUARapport.Count > 5 Then
            'Message
            'TS70034E
            ValErreur.ErrorMessage = Me.ContexteApp.ObtenirMessageFormate("TS70034E", False, "")
            ValErreur.IsValid = False
        Else
            ValErreur.IsValid = True
        End If



    End Sub

    Public Function AfficherTitreAutresUA() As String
        Dim strTitre As String = String.Empty

        If mobjTrx.dtListeUADemandeur Is Nothing OrElse mobjTrx.dtListeUADemandeur.Rows.Count = 0 Then
            strTitre = "Les unités administratives"
        Else
            strTitre = "Les autres unités administratives"
        End If

        Return strTitre

    End Function

    Private Sub GererPlusMoins(ByRef objUAPrincipalPlusMoins As NI1I516_PlusMoinsInfos.NI1P516_PlusMoinsInfos, ByRef pnlListeRoles As Panel, ByVal blnPanneauOuvert As Boolean, ByVal pstrTitre As String)
        If Not IsPostBack Then

            If blnPanneauOuvert Then
                objUAPrincipalPlusMoins.InitialiserControleSousTitre(pnlListeRoles,
                                pstrTitre,
                                NI1I516_PlusMoinsInfos.NI1P516_PlusMoinsInfos.enumEclatement.Eclater)
            Else
                objUAPrincipalPlusMoins.InitialiserControleSousTitre(pnlListeRoles,
                                pstrTitre,
                                NI1I516_PlusMoinsInfos.NI1P516_PlusMoinsInfos.enumEclatement.Reduire)
            End If


        Else
            'Initialisation du contrôle plus moins pour afficher la liste des tables
            objUAPrincipalPlusMoins.InitialiserControleSousTitre(pnlListeRoles,
            pstrTitre)


        End If

        grdUAAutres.DataSource = mobjTrx.dtListeUnitesAdmin
        grdUAAutres.DataBind()

    End Sub


End Class

