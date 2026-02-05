Public Class TsFdTestServiceGestnAcces

#Region "Variables privées"

    Private memoire As New TsCdMemoireForm

#End Region

    Private Sub TsFdTestServiceGestnAcces_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        pnlPage1.Visible = True
        pnlPage2.Visible = False
        pnlPage3.Visible = False
        pnlPage4.Visible = False
        ChargerPage1()
    End Sub

#Region "Fonctions Page 1"

    Private Sub ChargerPage1()
        Text = "Assistant - Informations sur la configuration par défault (1 de 4)"

        txtPage1ConfigDefaut.Text = Rrq.InfrastructureCommune.Parametres.XuCuConfiguration.ValeurSysteme("TS7", "TS7N121\ConfigDefaut")
    End Sub

    Private Sub btnPage1Info_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPage1Info.Click

        Cursor = Cursors.WaitCursor
        Dim config As TsCdSageConfiguration = TsBaConfigSage.GetConfiguration(txtPage1ConfigDefaut.Text)
        lblPage1IDConfig.Text = config.ConfigurationID
        lblPage1NomConfig.Text = config.ConfigurationName
        lblPage1DateCreation.Text = config.CreateDate.ToString
        lblPage1NomRdb.Text = config.ResourceDatabaseName
        lblPage1Completer.Text = config.IsCompleted.ToString
        lblPage1Connecter.Text = config.IsLogged.ToString
        lblPage1LectureSeul.Text = config.IsReadOnly.ToString
        lblPage1DateModification.Text = config.ModifyDate.ToString
        lblPage1Operation.Text = config.Operation1
        lblPage1Proprietaire.Text = config.Owner1
        lblPage1ConfigParent.Text = config.ParentConfigName
        lblPage1IdRdb.Text = config.ResourceDatabaseID
        lblPage1IdUdb.Text = config.UserDatabaseID
        lblPage1NomUdb.Text = config.UserDatabaseName
        Cursor = Cursors.Arrow

    End Sub

    Private Sub btnPage1Suivant_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPage1Suivant.Click
        pnlPage1.Visible = False
        pnlPage2.Visible = True
        ChargerPage2()
    End Sub

#End Region

#Region "Fonctions Page 2"

    Private Sub ChargerPage2()
        Text = "Assistant - Choix du mode (2 de 4)"

        btnPage2Precedant.Enabled = True
        btnPage2Suivant.Enabled = False

        optPage2Creation.Checked = False
        optPage2Modification.Checked = False
        optPage2Destruction.Checked = False

        PreparerPage2Mode()
    End Sub

    Private Sub PreparerPage2Mode()
        lbxPage2Utilisateurs.Items.Clear()

        Dim lstComposant() As System.Windows.Forms.Control = _
                    {txtPage2Courriel, txtPage2Id, txtPage2Nom, txtPage2NomComplet, txtPage2Prenom, _
                     txtPage2UA, txtPage2Ville, dtpPage2Approuve, dtpPage2DateDeFin, chkPage2Approuve, chkPage2DateDeFin}

        For Each c As Control In lstComposant
            c.Enabled = False
            If TypeOf c Is TextBox Then
                DirectCast(c, TextBox).Text = ""
            End If
        Next

        dtpPage2Approuve.Visible = False
        dtpPage2DateDeFin.Visible = False

        chkPage2Approuve.Checked = False
        chkPage2DateDeFin.Checked = False
        dtpPage2DateDeFin.Value = DateAdd(DateInterval.Day, 1, Date.Now)
        dtpPage2Approuve.Value = Date.Now

        btnPage2Suivant.Enabled = False
    End Sub

    Private Sub ChargerPage2ModeCreation()
        PreparerPage2Mode()

        Dim lstComposant() As System.Windows.Forms.Control = _
                    {txtPage2Courriel, txtPage2Nom, txtPage2NomComplet, txtPage2Prenom, _
                     txtPage2UA, txtPage2Ville, chkPage2Approuve, chkPage2DateDeFin}
        For Each c As Control In lstComposant
            c.Enabled = True
        Next

        dtpPage2DateDeFin.Enabled = True
        btnPage2Suivant.Enabled = True
    End Sub

    Private Sub ChargerPage2ModeModification()
        PreparerPage2Mode()

        RemplirPage2ListeUtilisateur()

        dtpPage2DateDeFin.Enabled = True
    End Sub

    Private Sub ChargerPage2ModeDestruction()
        PreparerPage2Mode()

        RemplirPage2ListeUtilisateur()
    End Sub

    Private Sub RemplirPage2ListeUtilisateur()
        Cursor = Cursors.WaitCursor
        For Each u As TsCdUtilisateur In TsCaServiceGestnAcces.RechercherUtilisateur("")
            lbxPage2Utilisateurs.Items.Add(New UtilisateurListe(u))
        Next
        Cursor = Cursors.Arrow
    End Sub

    Private Sub btnPage2Precedant_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPage2Precedant.Click
        pnlPage1.Visible = True
        pnlPage2.Visible = False
        ChargerPage1()
    End Sub

    Private Sub optPage2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optPage2Creation.CheckedChanged, optPage2Modification.CheckedChanged, optPage2Destruction.CheckedChanged
        Select Case True
            Case optPage2Creation.Checked = True
                ChargerPage2ModeCreation()
            Case optPage2Modification.Checked = True
                ChargerPage2ModeModification()
            Case optPage2Destruction.Checked = True
                ChargerPage2ModeDestruction()
        End Select
    End Sub

    Private Sub chkPage2Approuve_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPage2Approuve.CheckedChanged
        If chkPage2Approuve.Checked = True Then
            dtpPage2Approuve.Visible = True
        Else
            dtpPage2Approuve.Visible = False
        End If
    End Sub

    Private Sub chkPage2DateDeFin_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPage2DateDeFin.CheckedChanged

        If chkPage2DateDeFin.Checked = True Then
            dtpPage2DateDeFin.Visible = True
        Else
            dtpPage2DateDeFin.Visible = False
        End If

    End Sub

    Private Sub lbxPage2Utilisateurs_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbxPage2Utilisateurs.SelectedIndexChanged
        If lbxPage2Utilisateurs.SelectedItem Is Nothing Then Exit Sub

        Dim utilisateur As UtilisateurListe = DirectCast(lbxPage2Utilisateurs.SelectedItem, UtilisateurListe)

        Dim lstChamp() As TextBox = {txtPage2Courriel, txtPage2Id, _
        txtPage2Nom, txtPage2NomComplet, txtPage2UA, _
        txtPage2Prenom, txtPage2Ville}

        Dim lstValeurs() As String = {utilisateur.Courriel, utilisateur.ID, utilisateur.Nom, utilisateur.NomComplet, _
        utilisateur.NoUniteAdmin, utilisateur.Prenom, utilisateur.Ville}

        For i As Integer = 0 To lstChamp.Length - 1
            lstChamp(i).Text = lstValeurs(i)
        Next

        If utilisateur.FinPrevue = True Then
            chkPage2DateDeFin.Checked = True
            dtpPage2DateDeFin.Value = utilisateur.DateFin
        Else
            chkPage2DateDeFin.Checked = False
        End If

        If utilisateur.ApprobationAccepter = True Then
            chkPage2Approuve.Checked = True
            dtpPage2Approuve.Value = utilisateur.DateApprobation
        Else
            chkPage2Approuve.Checked = False
        End If

        If optPage2Modification.Checked = True Then
            txtPage2UA.Enabled = True
            chkPage2DateDeFin.Enabled = True
            chkPage2Approuve.Enabled = True
            btnPage2Suivant.Enabled = True
        End If

        If optPage2Destruction.Checked = True Then
            btnPage2Suivant.Enabled = True
        End If

    End Sub

    Private Sub btnPage2Suivant_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPage2Suivant.Click
        Select Case True
            Case optPage2Creation.Checked
                memoire.mode = TsCdMemoireForm.TsMemModeGestion.Creation

                Dim utilisateur As New TsCdUtilisateur()
                utilisateur.Courriel = txtPage2Courriel.Text
                utilisateur.DateFin = dtpPage2DateDeFin.Value
                utilisateur.FinPrevue = chkPage2DateDeFin.Checked
                utilisateur.Nom = txtPage2Nom.Text
                utilisateur.NomComplet = txtPage2NomComplet.Text
                utilisateur.NoUniteAdmin = txtPage2UA.Text
                utilisateur.Prenom = txtPage2Prenom.Text
                utilisateur.Ville = txtPage2Ville.Text
                utilisateur.DateApprobation = dtpPage2Approuve.Value
                utilisateur.ApprobationAccepter = chkPage2Approuve.Checked

                Dim demandeCreation As New TsCdDemndCreationModif()
                demandeCreation.Utilisateur = utilisateur

                memoire.demandeCreationModification = demandeCreation

            Case optPage2Modification.Checked
                memoire.mode = TsCdMemoireForm.TsMemModeGestion.Modification
                Dim demandeModification As New TsCdDemndCreationModif(txtPage2Id.Text)

                If chkPage2DateDeFin.Checked = True Then
                    demandeModification.Utilisateur.FinPrevue = True
                    demandeModification.Utilisateur.DateFin = dtpPage2DateDeFin.Value
                Else
                    demandeModification.Utilisateur.FinPrevue = False
                    demandeModification.Utilisateur.DateFin = Nothing
                End If

                If chkPage2Approuve.Checked = True Then
                    demandeModification.Utilisateur.ApprobationAccepter = True
                    demandeModification.Utilisateur.DateApprobation = dtpPage2Approuve.Value
                Else
                    demandeModification.Utilisateur.ApprobationAccepter = False
                    demandeModification.Utilisateur.DateApprobation = Nothing
                End If

                demandeModification.Utilisateur.NoUniteAdmin = txtPage2UA.Text

                memoire.demandeCreationModification = demandeModification

            Case optPage2Destruction.Checked
                memoire.mode = TsCdMemoireForm.TsMemModeGestion.Suppression
                memoire.demandeSuppresion = New TsCdDemandeDestruction(txtPage2Id.Text)
        End Select

        pnlPage2.Visible = False
        If memoire.mode = TsCdMemoireForm.TsMemModeGestion.Suppression Then
            ChargerPage4()
            pnlPage4.Visible = True
        Else
            ChargerPage3()
            pnlPage3.Visible = True
        End If
    End Sub

#End Region


#Region "Fonctions Page 3"
    Private Sub ChargerPage3()
        Text = "Assistant - Changement de l'assignation des rôles (3 de 4)"

        dtpPage3FinAssignation.Visible = False
        chkPage3Assignation.Checked = False

        Cursor = Cursors.WaitCursor
        lbxPage3RolesDisponibles.Items.Clear()
        For Each r As TsCdRole In TsCaServiceGestnAcces.RechercherRole("")
            lbxPage3RolesDisponibles.Items.Add(New RolesListe(r))
        Next
        Cursor = Cursors.Arrow

        lbxPage3RoleAssigner.Items.Clear()
        If memoire.mode = TsCdMemoireForm.TsMemModeGestion.Modification Then
            For Each r As TsCdAssignationRole In memoire.demandeCreationModification.RolesOriginaux
                lbxPage3RoleAssigner.Items.Add(r.ID)
            Next
        End If

        MAJPage4Operations()

        btnPage3AjoutRole.Enabled = False
        btnPage3ModifierRole.Enabled = False
        btnPage3SupprimerRole.Enabled = False

        dtpPage3FinAssignation.Value = DateAdd(DateInterval.Day, 1, Date.Now)

    End Sub

    Private Sub MAJPage4Operations()
        If memoire.demandeCreationModification IsNot Nothing Then
            lsvPage3Operations.Items.Clear()
            For Each r As TsCdOperationRole In memoire.demandeCreationModification.OperationsRoles
                Dim sousColonne As ListViewItem = lsvPage3Operations.Items.Add(r.Operation.ToString)
                sousColonne.SubItems.Add(r.IdRole)
            Next
        End If
    End Sub

    Private Sub lbxPage3RolesDisponibles_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbxPage3RolesDisponibles.SelectedIndexChanged
        Dim role As RolesListe = DirectCast(lbxPage3RolesDisponibles.SelectedItem, RolesListe)
        txtPage3Description.Text = role.Description
        txtPage3Id.Text = role.ID
        txtPage3Nom.Text = role.Nom
        chkPage3RoleParticulier.Checked = role.Particulier

        chkPage3Assignation.Enabled = True
        btnPage3AjoutRole.Enabled = True
        btnPage3ModifierRole.Enabled = True
        btnPage3SupprimerRole.Enabled = True
    End Sub

    Private Sub chkPage3Assignation_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPage3Assignation.CheckedChanged
        If chkPage3Assignation.Checked = True Then
            dtpPage3FinAssignation.Visible = True
        Else
            dtpPage3FinAssignation.Visible = False
        End If
    End Sub

    Private Sub btnPage3AjoutRole_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPage3AjoutRole.Click
        Try
            If chkPage3Assignation.Checked = True Then
                memoire.demandeCreationModification.AjouterRole(txtPage3Id.Text, dtpPage3FinAssignation.Value)
            Else
                memoire.demandeCreationModification.AjouterRole(txtPage3Id.Text)
            End If
            MAJPage4Operations()

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Erreur")
        End Try


    End Sub

    Private Sub btnPage3ModifierRole_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPage3ModifierRole.Click
        Try
            If chkPage3Assignation.Checked = True Then
                memoire.demandeCreationModification.ModifierRole(txtPage3Id.Text, dtpPage3FinAssignation.Value)
            Else
                memoire.demandeCreationModification.ModifierRole(txtPage3Id.Text, False)
            End If

            MAJPage4Operations()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Erreur")
        End Try
    End Sub

    Private Sub btnPage3SupprimerRole_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPage3SupprimerRole.Click
        Try
            memoire.demandeCreationModification.RetirerRole(txtPage3Id.Text)
            MAJPage4Operations()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Erreur")
        End Try
    End Sub

    Private Sub btnPage3Precedant_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPage3Precedant.Click
        pnlPage3.Visible = False
        pnlPage2.Visible = True
    End Sub

    Private Sub btnPage3Suivant_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPage3Suivant.Click
        pnlPage3.Visible = False
        pnlPage4.Visible = True
        ChargerPage4()
    End Sub

#End Region

#Region "Fonctions Page 4"

    Private Sub ChargerPage4()
        Text = "Assistant - Choisir le mode d'envois (4 de 4)"

        btnPage4Envois.Enabled = False
        optPage4Creation.Checked = False
        optPage4Modification.Checked = False
        optPage4Suppression.Checked = False

        txtPage4Guid.Text = ""

        dtpPage4DateVigueur.Value = DateAdd(DateInterval.Day, 1, Date.Now)

    End Sub

    Private Sub optPage4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optPage4Creation.CheckedChanged, optPage4Suppression.CheckedChanged, optPage4Modification.CheckedChanged
        btnPage4Envois.Enabled = True
    End Sub

    Private Sub btnPage4Precedant_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPage4Precedant.Click
        pnlPage4.Visible = False
        If memoire.mode = TsCdMemoireForm.TsMemModeGestion.Suppression Then
            pnlPage2.Visible = True
            ChargerPage2()
        Else
            pnlPage3.Visible = True
            ChargerPage3()
        End If

    End Sub

    Private Sub btnPage4Envois_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPage4Envois.Click
        Try
            Select Case True
                Case optPage4Creation.Checked
                    memoire.demandeCreationModification.TexteLibre = TxtPage4Commentaire.Text
                    If txtPage4Guid.Text <> "" Then
                        TsCaServiceGestnAcces.DemanderCreation(memoire.demandeCreationModification, dtpPage4DateVigueur.Value, txtPage4Guid.Text)
                    Else
                        TsCaServiceGestnAcces.DemanderCreation(memoire.demandeCreationModification, dtpPage4DateVigueur.Value)
                    End If
                Case optPage4Modification.Checked
                    memoire.demandeCreationModification.TexteLibre = TxtPage4Commentaire.Text
                    If txtPage4Guid.Text <> "" Then
                        TsCaServiceGestnAcces.DemanderModification(memoire.demandeCreationModification, dtpPage4DateVigueur.Value, txtPage4Guid.Text)
                    Else
                        TsCaServiceGestnAcces.DemanderModification(memoire.demandeCreationModification, dtpPage4DateVigueur.Value)
                    End If
                Case optPage4Suppression.Checked
                    If txtPage4Guid.Text <> "" Then
                        TsCaServiceGestnAcces.DemanderDestruction(memoire.demandeSuppresion, dtpPage4DateVigueur.Value, txtPage4Guid.Text)
                    Else
                        TsCaServiceGestnAcces.DemanderDestruction(memoire.demandeSuppresion, dtpPage4DateVigueur.Value)
                    End If
            End Select
        Catch ex As ApplicationException
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Erreur")
        End Try
    End Sub

    Private Sub btnPage1Precedant_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPage1Precedant.Click
        TsFdFonctionsIndependantes.Show()
        Me.Close()
    End Sub
#End Region

#Region "Class Privé"

    ''' <summary>
    ''' Classe Privée. Sert à afficher l'objet TsCdUtilisateur dans une liste combo.
    ''' </summary>
    ''' <remarks></remarks>
    Private Class UtilisateurListe
        Inherits TsCdUtilisateur

        ''' <summary>
        ''' Constructeur de transition.
        ''' </summary>
        Public Sub New(ByVal utilisateur As TsCdUtilisateur)
            Me.Courriel = utilisateur.Courriel
            Me.DateFin = utilisateur.DateFin
            Me.FinPrevue = utilisateur.FinPrevue
            Me.ID = utilisateur.ID
            Me.Nom = utilisateur.Nom
            Me.NomComplet = utilisateur.NomComplet
            Me.Prenom = utilisateur.Prenom
            Me.Ville = utilisateur.Ville
            Me.NoUniteAdmin = utilisateur.NoUniteAdmin
            Me.DateApprobation = utilisateur.DateApprobation
            Me.ApprobationAccepter = utilisateur.ApprobationAccepter
        End Sub

        ''' <summary>
        ''' Fonction redéfinie. Affichage pour liste combinée. 
        ''' </summary>
        Public Overrides Function toString() As String
            If Me.NomComplet = "" Then
                Return Prenom + " " + Nom
            Else
                Return NomComplet
            End If
        End Function
    End Class

    ''' <summary>
    ''' Classe Privée. Sert à afficher l'objet TsCdRole dans une liste combo.
    ''' </summary>
    ''' <remarks></remarks>
    Private Class RolesListe
        Inherits TsCdRole

        ''' <summary>
        ''' Constructeur de transition.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New(ByVal role As TsCdRole)
            Me.Description = role.Description
            Me.ID = role.ID
            Me.Nom = role.Nom
        End Sub

        ''' <summary>
        ''' Fonction redéfinie. Affichage pour liste combinée. 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function toString() As String
            Return ID
        End Function
    End Class

    Private Class TsCdMemoireForm
        Enum TsMemModeGestion
            Aucun
            Creation
            Modification
            Suppression
        End Enum

        Public mode As TsMemModeGestion
        Public demandeSuppresion As TsCdDemandeDestruction
        Public demandeCreationModification As TsCdDemndCreationModif

    End Class

#End Region

End Class

