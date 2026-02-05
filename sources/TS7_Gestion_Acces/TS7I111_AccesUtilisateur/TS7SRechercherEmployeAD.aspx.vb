Imports Rrq.Web.GabaritsPetitsSystemes.FonctionsCommunes.NiCdParametresMsg
Imports Rrq.Web.GabaritsPetitsSystemes.FonctionsCommunes.NiTypeMessage
Imports Rrq.Web.GabaritsPetitsSystemes.Utilitaires
Imports Rrq.Web.GabaritsPetitsSystemes.Controles
Imports Rrq.Web.AccesUtilisateurs.Utilitaires
Imports Rrq.Securite.GestionAcces


Public Class TS7SRechercherEmployeAD
    Inherits Rrq.Web.GabaritsPetitsSystemes.Controles.NiCrPage

    Const PAGE_ACCUEIL As String = "../TS7I111_AccesUtilisateur/TS7SRechercherEmploye.aspx"
    Protected mobjTrx As TS7I112_RAAccesUtilisateur.TSCdObjetTrx
    Protected mobjAffaire As TS7I112_RAAccesUtilisateur.TSCaAccesUtilisateur

#Region " Code généré par le Concepteur Web Form "

    'Cet appel est requis par le Concepteur Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents XlCrAffchAscxPartage1 As Rrq.Web.ServicesCommunsPetitsSystemes.Utilitaires.XlCrAffchAscxPartage
    Protected WithEvents XlCrAffchAscxPartage2 As Rrq.Web.ServicesCommunsPetitsSystemes.Utilitaires.XlCrAffchAscxPartage
    Protected WithEvents lblTitre As System.Web.UI.WebControls.Label
    Protected WithEvents XlCrAffchAscxPartage3 As Rrq.Web.ServicesCommunsPetitsSystemes.Utilitaires.XlCrAffchAscxPartage
    Protected WithEvents ctrlNI1P514_RechercheUtilAD As Rrq.Web.ServicesCommunsPetitsSystemes.Utilitaires.XlCrAffchAscxPartage

    'REMARQUE : la déclaration d'espace réservé suivante est requise par le Concepteur Web Form.
    'Ne pas supprimer ou déplacer.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN : cet appel de méthode est requis par le Concepteur Web Form
        'Ne le modifiez pas en utilisant l'éditeur de code.
        InitializeComponent()
    End Sub

#End Region

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
        mobjTrx = CType(ContexteApp.TrxCourante, TS7I112_RAAccesUtilisateur.TSCdObjetTrx)
        mobjAffaire = New TS7I112_RAAccesUtilisateur.TSCaAccesUtilisateur
        'grdEmployes.InitialiserControle("TS7I111_AccesUtilisateur", hypPage1, hypPage2, hypPage3, hypPage4, hypPage5, hypPage6, hypPage7, hypPage8, HyPrecedent, hySuivant, strExpTriInitial)
        objctrlUtilisateur.InitialiseControl(Me, , , NI1I514_RechercheUtilAD.NI1P514_RechercheUtilAD.enumBoutonSelection.BoutonSelectionner, False, True, False)
        objctrlUtilisateur.LibelleNote = "Inscrire les paramètres nécessaires à l'identification de l'employé, puis faire Rechercher."

        objctrlUtilisateur.Nom = Request.QueryString("Nom")
        objctrlUtilisateur.Prenom = Request.QueryString("Prenom")
        ' CType(objctrlUtilisateur.Controls(0).FindControl("txtNom"), TextBox).Text = Request.QueryString("Nom")
        'CType(objctrlUtilisateur.Controls(0).FindControl("txtPrenom"), TextBox).Text = Request.QueryString("Prenom")
        objctrlUtilisateur.blnRechercheAuto = True


        objctrlUtilisateur.LibelleUtilisateurAAjouter = "Sélectionner l'employé désiré dans la liste ci-dessous."


        AddHandler objctrlUtilisateur.SelectionnerUtilisateur, AddressOf UtilisateurSelectionne
        AddHandler objctrlUtilisateur.Annuler, AddressOf AucuneSelection

    End Sub


    Public Sub UtilisateurSelectionne(ByVal objNiCdUtilisateur As NiCdUtilisateurAD)
        objctrlUtilisateur.Visible = False
        Dim UtilisateurSage As TsCdUtilisateur = Nothing

        'Exception : on fait une dernière vérification dans sage si l'utilisateur existe
        Try
            UtilisateurSage = TsCaServiceGestnAcces.ObtenirUtilisateur(objNiCdUtilisateur.CodeUtilisateur.ToUpper)
        Catch ex As Exception

        End Try

        If UtilisateurSage Is Nothing Then
            mobjTrx.blnUtilisateurSage = False
            mobjTrx.strCodUtilisateurSelect = objNiCdUtilisateur.CodeUtilisateur
            Response.Redirect(String.Concat(TSCuDomVal.PAGE_ROLE_EMPLOYE, "?codetrans=TS7Role", _
                                "&CodeUtilisateur=" & objNiCdUtilisateur.CodeUtilisateur, "&Type=", mobjTrx.strTypeAcces, "&Prov=TS7SRechercherEmployeAD"))

        Else
            mobjTrx.blnUtilisateurSage = True
            mobjTrx.strCodUtilisateurSelect = UtilisateurSage.ID 'dtTemp.Rows(0).Item("CodeUtilisateur").ToString()
            Response.Redirect(String.Concat(TSCuDomVal.PAGE_ROLE_EMPLOYE, "?codetrans=", Request.QueryString("codetrans").ToString(), _
                            "&CodeUtilisateur=" & mobjTrx.strCodUtilisateurSelect, "&Type=", Request.QueryString("Type").ToString(), "&Prov=TS7SRechercherEmploye"))
        End If
       
    End Sub

    Private Sub AucuneSelection()

    End Sub

    Protected ReadOnly Property objctrlUtilisateur() As NI1I514_RechercheUtilAD.NI1P514_RechercheUtilAD
        Get
            Return CType(ctrlNI1P514_RechercheUtilAD.ControleASCX, NI1I514_RechercheUtilAD.NI1P514_RechercheUtilAD)
        End Get
    End Property

    'Private Sub cmdRetour_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Response.Redirect(PAGE_ACCUEIL)
    'End Sub

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

End Class
