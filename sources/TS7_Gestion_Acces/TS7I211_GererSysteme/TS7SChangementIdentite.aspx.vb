Imports Rrq.Web.GabaritsPetitsSystemes.FonctionsCommunes.NiCdParametresMsg
Imports Rrq.Web.GabaritsPetitsSystemes.FonctionsCommunes.NiTypeMessage
Imports Rrq.Web.GabaritsPetitsSystemes.Utilitaires
Imports Rrq.Web.GabaritsPetitsSystemes.Controles
Imports Rrq.Web.ReservationSalles.Utilitaires

Public Class TS7SChangementIdentite
    Inherits Rrq.Web.GabaritsPetitsSystemes.Controles.NiCrPage

    Const PAGE_ACCUEIL As String = "../TS7I111_AccesUtilisateur/TS7SRechercherEmploye.aspx"

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


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        objctrlUtilisateur.InitialiseControl(Me, , , NI1I514_RechercheUtilAD.NI1P514_RechercheUtilAD.enumBoutonSelection.BoutonSelectionner, Not IsPostBack, True, False)
        objctrlUtilisateur.LibelleNote = "Inscrire les paramètres nécessaires à l'identification de l'employé, puis faire Rechercher."
        objctrlUtilisateur.ChangementIdentite = True

        objctrlUtilisateur.LibelleUtilisateurAAjouter = "Sélectionner l'employé désiré dans la liste ci-dessous."

        AddHandler objctrlUtilisateur.SelectionnerUtilisateur, AddressOf UtilisateurSelectionne
        AddHandler objctrlUtilisateur.Annuler, AddressOf AucuneSelection

    End Sub


    Public Sub UtilisateurSelectionne(ByVal objNiCdUtilisateur As NiCdUtilisateurAD)
        objctrlUtilisateur.Visible = False

        If Me.ChangerIdentite(objNiCdUtilisateur.CodeUtilisateur) Then
            Response.Redirect(PAGE_ACCUEIL)
        End If
        'objctrlUtilisateur.Visible = False

        'If Me.ChangerIdentite(objNiCdUtilisateur.CodeUtilisateur) Then
        '    Dim strCleVarSession As String = String.Empty
        '    If Not ContexteApp.CodeUniteOrganique Is Nothing _
        '        AndAlso ContexteApp.CodeUniteOrganique.StartsWith("I") Then

        '        strCleVarSession = "NI1Menu_" &
        '                               ContexteApp.CodeSysteme & "_" &
        '                                 ContexteApp.CodeUniteOrganique & "_" &
        '                                  ContexteApp.UtilisateurCourant.CodeUtilisateur

        '    Else
        '        strCleVarSession = "NI1Menu_" &
        '                               ContexteApp.CodeSysteme & "_" &
        '                               ContexteApp.UtilisateurCourant.CodeUtilisateur

        '    End If

        '    Dim strPageAccueil As String = NI1I511_MenuGaucheGabarit.NI1P511_MenuGaucheGabarit.ObtenirMenu(Me, ContexteApp).LienPageAccueil

        '    Response.Redirect(strPageAccueil)
        'End If
    End Sub

    Private Sub AucuneSelection()

    End Sub

    Protected ReadOnly Property objctrlUtilisateur() As NI1I514_RechercheUtilAD.NI1P514_RechercheUtilAD
        Get
            Return CType(ctrlNI1P514_RechercheUtilAD.ControleASCX, NI1I514_RechercheUtilAD.NI1P514_RechercheUtilAD)
        End Get
    End Property

    Private Sub cmdRetour_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Response.Redirect(PAGE_ACCUEIL)
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

End Class
