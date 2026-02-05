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
            Return String.Concat(Rrq.InfrastructureCommune.Parametres.XuCuConfiguration.ValeurSysteme("TS7", "GroupesSecurite\GRP_ACCES_RAPPORTS"), ";", _
                                 Rrq.InfrastructureCommune.Parametres.XuCuConfiguration.ValeurSysteme("TS7", "GroupesSecurite\GRP_RESPONSABLE_UA"), ";", _
                                 Rrq.InfrastructureCommune.Parametres.XuCuConfiguration.ValeurSysteme("TS7", "GroupesSecurite\GRP_PILOTAGE"))
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            ForcerRafraichissement()

            InitialiserNouvelleTrx()

        End If

        IndAvertissementInterruption = enumAvertissementInterruption.ConditionnelAUnChangement
        
        mobjTrx = CType(ContexteApp.TrxCourante, TS7I112_RAAccesUtilisateur.TSCdObjetTrx)
        mobjAffaire = New TS7I112_RAAccesUtilisateur.TSCaAccesUtilisateur



        If IsPostBack Then


            NiCuRecupererCaseCocheGrille.RecupererSelection(
                  "chkSelectionAutresUA",
                  mobjTrx.dtListeUnitesAdmin,
                  "No",
                  "SELECT", "O", "N",
                  mobjTrx.dtListeUnitesAdmin.DefaultView.Sort,
                  grdUAAutres.CurrentPageIndex + 1,
                  grdUAAutres.PageSize)




        Else
            mobjTrx.dtListeUnitesAdmin = TSCuGeneral.UniteAdminDataTable(TS7I142_CiRapportRess.TsCaGererRapports.ObtenirListeUnitesAdmin) 'TSCuGeneral.UniteAdminDataTable(TsCaServiceGestnAcces.ObtenirListeUnitesAdmin())
            NiCuADO.AjouterDtColonne(mobjTrx.dtListeUnitesAdmin, "SELECT", System.Type.GetType("System.String"), 1, "N")


        End If


        grdUAAutres.DataSource = mobjTrx.dtListeUnitesAdmin

        grdUAAutres.DataBind()


    End Sub

    Private Sub InitialiserNouvelleTrx()

        Dim strCodeUsager As String
        strCodeUsager = ContexteApp.UtilisateurCourant.CodeUtilisateur

        'On initialise une nouvelle transaction pour l'utilisateur courant
        mobjTrx = New TS7I112_RAAccesUtilisateur.TSCdObjetTrx(strCodeUsager)

        ContexteApp.TrxCourante = mobjTrx

    End Sub

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
        'Dim lstUAAutresSelect As IEnumerable(Of DataRow) = Nothing

        lstUARapport.Clear()

        'Les unités administratives du demandeur
        If Not (mobjTrx.dtListeUnitesAdmin Is Nothing OrElse mobjTrx.dtListeUnitesAdmin.Rows.Count = 0) Then
            lstUADemandeurSelect = mobjTrx.dtListeUnitesAdmin.Select("SELECT LIKE 'O'")
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

        grdUAAutres.DataSource = mobjTrx.dtListeUnitesAdmin
        grdUAAutres.DataBind()

    End Sub
    

End Class

