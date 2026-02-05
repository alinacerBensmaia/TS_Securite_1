Imports Rrq.Web.GabaritsPetitsSystemes.FonctionsCommunes.NiCdParametresMsg
Imports Rrq.Web.GabaritsPetitsSystemes.FonctionsCommunes.NiTypeMessage
Imports Rrq.Web.GabaritsPetitsSystemes.Controles.NiCrPage
Imports Rrq.Web.AccesUtilisateurs.Utilitaires



Partial Class TS7SConfirmation
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

    Protected mobjtrx As TS7I112_RAAccesUtilisateur.TSCdObjetTrx
    Protected mobjAffaire As TS7I112_RAAccesUtilisateur.TSCaAccesUtilisateur

    Public Overrides ReadOnly Property GroupeADRequis() As String
        Get
            'Changement du nom du groupe pour la gestion des roles. 2013-12-11
            'Return "GTS7#ENVIRONNEMENT#GestionAcces"
            'Return "ROG_#ENVIRONNEMENT#_Gestion des acces demandeur"
            Return String.Concat(Rrq.InfrastructureCommune.Parametres.XuCuConfiguration.ValeurSysteme("TS7", "GroupesSecurite\GRP_ACCES_UTILISATEUR"), ";", _
                                 Rrq.InfrastructureCommune.Parametres.XuCuConfiguration.ValeurSysteme("TS7", "GroupesSecurite\GRP_RESPONSABLE_UA"), ";", _
                                 Rrq.InfrastructureCommune.Parametres.XuCuConfiguration.ValeurSysteme("TS7", "GroupesSecurite\GRP_PILOTAGE"))
        End Get
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim strAction As String = Request.QueryString("Action")
        mobjtrx = CType(ContexteApp.TrxCourante, TS7I112_RAAccesUtilisateur.TSCdObjetTrx)
        mobjAffaire = New TS7I112_RAAccesUtilisateur.TSCaAccesUtilisateur
        Dim strNomPrenom As String = String.Concat(mobjtrx.strNom, " ", mobjtrx.strPrenom)
        If Not IsPostBack Then
            lblInfosSuppl.Text = Me.ContexteApp.ObtenirMessageNonFormate("TS70035I", "")
            Select Case strAction
                Case "Ajout"
                    lblMessage.Text = Me.ContexteApp.ObtenirMessageNonFormate("TS70011I", strNomPrenom)
                    'Obtenir le no de l'unite admin de l'utilisateur avant
                    Dim strNoUA As String = String.Empty
                    strNoUA = Mid(mobjtrx.strUaPrinc, InStr(mobjtrx.strUaPrinc, "_") + 1, 4)

                    mobjAffaire.EnvoyerMessageConfirmation(ContexteApp, mobjtrx.strUaPrincModifie, strNomPrenom, String.Concat(ContexteApp.UtilisateurCourant.Nom, " ", ContexteApp.UtilisateurCourant.Prenom), mobjtrx.strDatEffective, mobjtrx.dtListeAssignationRoleConfirm)

                Case "Modification"
                    lblMessage.Text = Me.ContexteApp.ObtenirMessageNonFormate("TS70012I", strNomPrenom)

                    'Mis en commentaire T208704.  On envoie un courriel dans tous les cas maintenant.
                    ' Au demandeur, au gestionnaire de l'employé et au répondants en sécurité selon leur UA.

                    ' If mobjtrx.strTypeAcces Is Nothing OrElse mobjtrx.strTypeAcces.ToUpper = "MOD" Then
                    'Envoyer un courriel au gestionnaire
                    'mobjAffaire.EnvoyerMessageGestionnaire(ContexteApp, ContexteApp.UtilisateurCourant.NoUniteAdm, strNomPrenom, String.Concat(ContexteApp.UtilisateurCourant.Nom, " ", ContexteApp.UtilisateurCourant.Prenom), mobjtrx.strDatEffective, mobjtrx.dtListeAssignationRoleConfirm)
                    'Obtenir le no de l'unite admin de l'utilisateur avant
                    Dim strNoUA As String = String.Empty
                    strNoUA = Mid(mobjtrx.strUaPrinc, InStr(mobjtrx.strUaPrinc, "_") + 1, 4)

                    mobjAffaire.EnvoyerMessageConfirmation(ContexteApp, strNoUA, strNomPrenom, String.Concat(ContexteApp.UtilisateurCourant.Nom, " ", ContexteApp.UtilisateurCourant.Prenom), mobjtrx.strDatEffective, mobjtrx.dtListeAssignationRoleConfirm)
                    'End If

                    If Not mobjtrx.strTypeAcces Is Nothing Then
                        If mobjtrx.strTypeAcces.ToUpper = "CHG" And mobjtrx.blnUAModelise = True Then
                            'Vérifier si des dates d'expiration dépasse la date de plus de 14 jours.

                            Dim dtExpirationvalide As DateTime = CType(mobjtrx.strDatEffective, DateTime).AddDays(14)
                            Dim blnRoleValide As Boolean = True
                            For Each drRole As DataRow In mobjtrx.dtListeAssignationRoleConfirm.Rows
                                'Trouver au moins un role qui correspond aux critere pour l'envoi de courriel.
                                ' Doit être de type CHG et l'ancienne unité administrative doit être modelisée.
                                ' Le demandeur ne doit pas être responsable du role.
                                ' la date d'expiration du role dépasse la date effective + 14 jours.
                                ' le role doit avoir l'action "Modifier" et non "Ajouter"
                                If Not TSCuGeneral.boolResponsableRole(drRole.Item("ListeUniteAdministrativeResponsable").ToString(), mobjtrx.dtListeUADemandeur) Then

                                    If Not String.IsNullOrEmpty(drRole.Item("DateFin").ToString) Then
                                        If dtExpirationvalide < CType(drRole.Item("DateFin"), DateTime) Then

                                            If drRole("ActionAfficher").ToString() = "Modifier" Then
                                                blnRoleValide = False
                                            End If

                                        End If
                                    End If
                                End If

                            Next

                            If blnRoleValide = False Then
                                'au moins un role, on envoi le courriel.
                                mobjAffaire.EnvoyerMessageProlongationExpiration(ContexteApp, mobjtrx.AncienneUA, strNomPrenom, String.Concat(ContexteApp.UtilisateurCourant.Nom, " ", ContexteApp.UtilisateurCourant.Prenom), mobjtrx.strDatEffective, mobjtrx.dtListeAssignationRoleConfirm, mobjtrx.dtListeUADemandeur)
                            End If
                        End If
                    End If

                Case "AccesRepertoire"
                    lblMessage.Text = Me.ContexteApp.ObtenirMessageNonFormate("TS70014I")
            End Select

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

    Protected Sub cmdRetour_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRetour.Click
        Response.Redirect(String.Concat(TSCuDomVal.PAGE_ROLE_EMPLOYE, "?Codetrans=ts7role"))
    End Sub

End Class
