
Partial Class TS7FenComplDetailRole
    Inherits System.Web.UI.Page
    Private mobjParametres As TS7CdParametresFenComplDetailRole
    Private mstrTitre As String = "Fenêtre de messsage"


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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Placez ici le code utilisateur pour initialiser la page
        'Gerer l'indicateur de modifiation
        Dim strValeur As String = String.Empty
        Try
            cmdBouton2.Attributes.Add("onClick", _
            String.Concat(strValeur, "AffecterChangementContenu('", "TS7", "');"))
        Catch ex As Exception
            cmdBouton2.Attributes.Add("onClick", _
            String.Concat(strValeur, "AffecterChangementContenu('", "TS7", "');"))
        End Try

        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        Response.Expires = -1

    End Sub

    Private Sub ctlGestionDialogue_Ouverture(ByVal sender As Object, ByVal e As Rrq.Web.ServicesCommunsPetitsSystemes.ScenarioTransactionnel.XlCdParametreAppel) Handles ctlGestionDialogue.Ouverture

        ' Est-ce que le parametre d'appel présent ?
        If (e.ParametreAppel Is Nothing) Then
            Throw New ApplicationException("Le paramètre d'appel est obligatoire pour afficher un message. " & _
                                           "Une instance valide de la classe ""NiCdMessage"" doit être" & _
                                           "fourni comme paramètre d'appel pour afficher une message.")
        End If

        ' Est-ce un paramètre valide, c'est-à-dire une instance de la classe XlCdMessage ?
        If Not (TypeOf (e.ParametreAppel) Is TS7CdParametresFenComplDetailRole) Then
            Throw New ApplicationException("Une instance valide de la classe ""TS7CdParametresFenComplDetailRole."" doit " & _
                                           "être fourni comme paramètre d'appel pour afficher une message.")
        End If

        ' Récupère le pramètre dans son bon type
        mobjParametres = CType(e.ParametreAppel, TS7CdParametresFenComplDetailRole)

        ' Extrait le texte
        Dim strTexte As String = ""
        'message = Me.ContexteApp.ObtenirMessageFormate(mobjParametres.NoMessage, False, mobjParametres.VariablesRemplacements)
        strTexte = mobjParametres.ValeurDetailRole

        ' Affiche le titre de la fenêtre
        mstrTitre = mobjParametres.TitreBteMesage
        Session("mstrTitre") = mstrTitre    'voir html...

        ' Affiche le message
        Me.txtSaisie.Text = strTexte

        'Boutons/
        ' On initialise les boutons
        If (mobjParametres.Bouton1 Is Nothing) Then
            Me.cmdBouton1.Visible = False
        Else
            'Me.cmdBouton1.ImageUrl = String.Concat(path, "boutons/", mobjParametres.Bouton1.TexteBouton)
            Me.cmdBouton1.Text = mobjParametres.Bouton1.TexteBouton
            Me.cmdBouton1.Visible = Not mobjParametres.Bouton1.Invisible
            If mobjParametres.Bouton1.EstBoutonDefaut Then cmdBouton1.TabIndex = 0
        End If

        If (mobjParametres.Bouton2 Is Nothing) Then
            Me.cmdBouton2.Visible = False
        Else
            'Me.cmdBouton2.ImageUrl = String.Concat(path, "boutons/", mobjParametres.Bouton2.TexteBouton)
            Me.cmdBouton2.Text = mobjParametres.Bouton2.TexteBouton
            Me.cmdBouton2.Visible = Not mobjParametres.Bouton2.Invisible
            If mobjParametres.Bouton2.EstBoutonDefaut Then cmdBouton2.TabIndex = 1
        End If


        'RegisterStartupScript("MettreFocus", String.Concat("<SCRIPT language='JavaScript'>SetFocus('", txtSaisie.ID, "');</script>"))
        'If mobjParametres.Bouton2.EstBoutonDefaut Then cmdBouton2.TabIndex = 1
    End Sub

    Private Sub cmdBouton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBouton1.Click, cmdBouton2.Click
        ' Sender correspond à un bouton cliqué
        Dim boutonClique As System.Web.UI.WebControls.Button = CType(sender, System.Web.UI.WebControls.Button)


        ' On récupère le bouton de la bte msg
        Dim oInfoBteMessage As TS7CdParametresFenComplDetailRole = CType(ctlGestionDialogue.ObtenirParametreAppel, TS7CdParametresFenComplDetailRole)
        Dim oBoutonBteMessg As TS7CdParametresFenComplDetailRole.TS7CdBouton = oInfoBteMessage.ObtenirBouton(CType(boutonClique.ID.Substring(boutonClique.ID.Length - 1, 1), Short) - 1S)

        '' Est-ce que le bouton ayant l'étiquette est inexistant ?
        If (oBoutonBteMessg Is Nothing) Then
            Throw New ApplicationException(String.Format("Le bouton de la boîte de message " & _
                                           "correspondant à l'étiquette ""{0}"" " & _
                                           "n'a pas été trouvé.", boutonClique.Text))
        End If

        ' Est-ce un bouton d'annulation ?
        If (oBoutonBteMessg.EstBoutonAnnulation) Then
            ' On ferme le dialogue avec annulation
            'ctlGestionDialogue.FermerFenetreAvecAnnulation()

            Dim aRetour(3) As String
            aRetour(0) = oInfoBteMessage.NomBoutonAppelant
            aRetour(1) = oBoutonBteMessg.Valeur
            aRetour(2) = txtSaisie.Text
            aRetour(3) = CType(oInfoBteMessage.NoEtapeCommentaire, String)
            ctlGestionDialogue.FermerFenetre(CType(aRetour, Object))
        Else
            ' On ferme le dialogue en retournant la valeur du bouton cliqué comme paramètre de retour 
            Dim aRetour(3) As String
            aRetour(0) = oInfoBteMessage.NomBoutonAppelant
            aRetour(1) = oBoutonBteMessg.Valeur
            aRetour(2) = txtSaisie.Text
            aRetour(3) = CType(oInfoBteMessage.NoEtapeCommentaire, String)

            ctlGestionDialogue.FermerFenetre(CType(aRetour, Object))
        End If
    End Sub



End Class
