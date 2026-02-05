Imports Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ
Imports Rrq.InfrastructureCommune.Parametres
Imports System.Collections.Generic
Imports TS6N631_ZpTrtParmChif
Imports TS6N631_ZpTrtParmChif.TsCuParamsChiffrement

Public Class tsFfGerParmChif
    Inherits System.Windows.Forms.Form

#Region " Code généré par le Concepteur Windows Form "

    Public Sub New()
        MyBase.New()

        Rrq.InfrastructureCommune.UtilitairesCommuns.XuCuChargementAssembly.CreerHandlerAssemblyResolve()

        'Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        'Ajoutez une initialisation quelconque après l'appel InitializeComponent()

    End Sub

    'La méthode substituée Dispose du formulaire pour nettoyer la liste des composants.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Requis par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée en utilisant le Concepteur Windows Form.  
    'Ne la modifiez pas en utilisant l'éditeur de code.
    Friend WithEvents lblEnvrn As System.Windows.Forms.Label
    Friend WithEvents cboEnvrn As System.Windows.Forms.ComboBox
    Friend WithEvents cmdActualiseGrille As System.Windows.Forms.Button
    Friend WithEvents cmdAjoutCleVecteur As System.Windows.Forms.Button
    Friend WithEvents cmdFermer As System.Windows.Forms.Button
    Friend WithEvents lblType As System.Windows.Forms.Label
    Friend WithEvents cboType As System.Windows.Forms.ComboBox
    Friend WithEvents tabParmChiff As System.Windows.Forms.TabControl
    Friend WithEvents tabCleVecteur As System.Windows.Forms.TabPage
    Friend WithEvents tabIdCertificat As System.Windows.Forms.TabPage
    Friend WithEvents tabSel As System.Windows.Forms.TabPage
    Friend WithEvents cmdAjoutCertificat As System.Windows.Forms.Button
    Friend WithEvents cmdAjoutSel As System.Windows.Forms.Button
    Friend WithEvents grdClesVecteurs As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.Grilles.XzCrGrille
    Friend WithEvents drtCleVect As Xceed.Grid.DataRow
    Friend WithEvents cmrCleVect As Xceed.Grid.ColumnManagerRow
    Friend WithEvents DataRow1 As Xceed.Grid.DataRow
    Friend WithEvents ColumnManagerRow1 As Xceed.Grid.ColumnManagerRow
    Friend WithEvents grdIdCertificats As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.Grilles.XzCrGrille
    Friend WithEvents drtIdCertificats As Xceed.Grid.DataRow
    Friend WithEvents cmrIdCertificats As Xceed.Grid.ColumnManagerRow
    Friend WithEvents grdSels As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.Grilles.XzCrGrille
    Friend WithEvents dtrSels As Xceed.Grid.DataRow
    Friend WithEvents cmrSels As Xceed.Grid.ColumnManagerRow
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.lblEnvrn = New System.Windows.Forms.Label()
        Me.cboEnvrn = New System.Windows.Forms.ComboBox()
        Me.cmdActualiseGrille = New System.Windows.Forms.Button()
        Me.cmdAjoutCleVecteur = New System.Windows.Forms.Button()
        Me.cmdFermer = New System.Windows.Forms.Button()
        Me.cboType = New System.Windows.Forms.ComboBox()
        Me.lblType = New System.Windows.Forms.Label()
        Me.tabParmChiff = New System.Windows.Forms.TabControl()
        Me.tabCleVecteur = New System.Windows.Forms.TabPage()
        Me.grdClesVecteurs = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.Grilles.XzCrGrille()
        Me.drtCleVect = New Xceed.Grid.DataRow()
        Me.cmrCleVect = New Xceed.Grid.ColumnManagerRow()
        Me.tabIdCertificat = New System.Windows.Forms.TabPage()
        Me.grdIdCertificats = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.Grilles.XzCrGrille()
        Me.drtIdCertificats = New Xceed.Grid.DataRow()
        Me.cmrIdCertificats = New Xceed.Grid.ColumnManagerRow()
        Me.cmdAjoutCertificat = New System.Windows.Forms.Button()
        Me.tabSel = New System.Windows.Forms.TabPage()
        Me.grdSels = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.Grilles.XzCrGrille()
        Me.dtrSels = New Xceed.Grid.DataRow()
        Me.cmrSels = New Xceed.Grid.ColumnManagerRow()
        Me.cmdAjoutSel = New System.Windows.Forms.Button()
        Me.DataRow1 = New Xceed.Grid.DataRow()
        Me.ColumnManagerRow1 = New Xceed.Grid.ColumnManagerRow()
        Me.tabParmChiff.SuspendLayout()
        Me.tabCleVecteur.SuspendLayout()
        CType(Me.grdClesVecteurs, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.drtCleVect, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmrCleVect, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabIdCertificat.SuspendLayout()
        CType(Me.grdIdCertificats, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.drtIdCertificats, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmrIdCertificats, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabSel.SuspendLayout()
        CType(Me.grdSels, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtrSels, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmrSels, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataRow1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ColumnManagerRow1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblEnvrn
        '
        Me.lblEnvrn.Location = New System.Drawing.Point(8, 16)
        Me.lblEnvrn.Name = "lblEnvrn"
        Me.lblEnvrn.Size = New System.Drawing.Size(88, 16)
        Me.lblEnvrn.TabIndex = 0
        Me.lblEnvrn.Text = "Environnement :"
        '
        'cboEnvrn
        '
        Me.cboEnvrn.Location = New System.Drawing.Point(96, 13)
        Me.cboEnvrn.Name = "cboEnvrn"
        Me.cboEnvrn.Size = New System.Drawing.Size(136, 21)
        Me.cboEnvrn.TabIndex = 0
        '
        'cmdActualiseGrille
        '
        Me.cmdActualiseGrille.Location = New System.Drawing.Point(376, 12)
        Me.cmdActualiseGrille.Name = "cmdActualiseGrille"
        Me.cmdActualiseGrille.Size = New System.Drawing.Size(104, 23)
        Me.cmdActualiseGrille.TabIndex = 1
        Me.cmdActualiseGrille.Text = "Actualiser la grille"
        Me.cmdActualiseGrille.Visible = False
        '
        'cmdAjoutCleVecteur
        '
        Me.cmdAjoutCleVecteur.Location = New System.Drawing.Point(8, 232)
        Me.cmdAjoutCleVecteur.Name = "cmdAjoutCleVecteur"
        Me.cmdAjoutCleVecteur.Size = New System.Drawing.Size(103, 23)
        Me.cmdAjoutCleVecteur.TabIndex = 3
        Me.cmdAjoutCleVecteur.Text = "Ajouter Code"
        Me.cmdAjoutCleVecteur.Visible = False
        '
        'cmdFermer
        '
        Me.cmdFermer.Location = New System.Drawing.Point(440, 336)
        Me.cmdFermer.Name = "cmdFermer"
        Me.cmdFermer.Size = New System.Drawing.Size(75, 23)
        Me.cmdFermer.TabIndex = 4
        Me.cmdFermer.Text = "Fermer"
        '
        'cboType
        '
        Me.cboType.Location = New System.Drawing.Point(280, 13)
        Me.cboType.Name = "cboType"
        Me.cboType.Size = New System.Drawing.Size(88, 21)
        Me.cboType.TabIndex = 6
        '
        'lblType
        '
        Me.lblType.Location = New System.Drawing.Point(240, 16)
        Me.lblType.Name = "lblType"
        Me.lblType.Size = New System.Drawing.Size(40, 16)
        Me.lblType.TabIndex = 5
        Me.lblType.Text = "Type :"
        '
        'tabParmChiff
        '
        Me.tabParmChiff.Controls.Add(Me.tabCleVecteur)
        Me.tabParmChiff.Controls.Add(Me.tabIdCertificat)
        Me.tabParmChiff.Controls.Add(Me.tabSel)
        Me.tabParmChiff.Location = New System.Drawing.Point(8, 40)
        Me.tabParmChiff.Name = "tabParmChiff"
        Me.tabParmChiff.SelectedIndex = 0
        Me.tabParmChiff.Size = New System.Drawing.Size(504, 288)
        Me.tabParmChiff.TabIndex = 7
        '
        'tabCleVecteur
        '
        Me.tabCleVecteur.Controls.Add(Me.grdClesVecteurs)
        Me.tabCleVecteur.Controls.Add(Me.cmdAjoutCleVecteur)
        Me.tabCleVecteur.Location = New System.Drawing.Point(4, 22)
        Me.tabCleVecteur.Name = "tabCleVecteur"
        Me.tabCleVecteur.Size = New System.Drawing.Size(496, 262)
        Me.tabCleVecteur.TabIndex = 0
        Me.tabCleVecteur.Text = "Clés & vecteurs"
        '
        'grdClesVecteurs
        '
        Me.grdClesVecteurs.ColonneTampon = Nothing
        Me.grdClesVecteurs.DataRowTemplate = Me.drtCleVect
        '
        '
        '
        Me.grdClesVecteurs.FixedColumnSplitter.Visible = False
        Me.grdClesVecteurs.FixedHeaderRows.Add(Me.cmrCleVect)
        Me.grdClesVecteurs.Location = New System.Drawing.Point(8, 8)
        Me.grdClesVecteurs.Name = "grdClesVecteurs"
        '
        '
        '
        Me.grdClesVecteurs.RowSelectorPane.AllowRowResize = False
        Me.grdClesVecteurs.RowSelectorPane.Visible = True
        Me.grdClesVecteurs.Size = New System.Drawing.Size(480, 216)
        Me.grdClesVecteurs.TabIndex = 0
        '
        'tabIdCertificat
        '
        Me.tabIdCertificat.Controls.Add(Me.grdIdCertificats)
        Me.tabIdCertificat.Controls.Add(Me.cmdAjoutCertificat)
        Me.tabIdCertificat.Location = New System.Drawing.Point(4, 22)
        Me.tabIdCertificat.Name = "tabIdCertificat"
        Me.tabIdCertificat.Size = New System.Drawing.Size(496, 262)
        Me.tabIdCertificat.TabIndex = 1
        Me.tabIdCertificat.Text = "Id certificat"
        '
        'grdIdCertificats
        '
        Me.grdIdCertificats.ColonneTampon = Nothing
        Me.grdIdCertificats.DataRowTemplate = Me.drtIdCertificats
        '
        '
        '
        Me.grdIdCertificats.FixedColumnSplitter.Visible = False
        Me.grdIdCertificats.FixedHeaderRows.Add(Me.cmrIdCertificats)
        Me.grdIdCertificats.Location = New System.Drawing.Point(8, 8)
        Me.grdIdCertificats.Name = "grdIdCertificats"
        '
        '
        '
        Me.grdIdCertificats.RowSelectorPane.AllowRowResize = False
        Me.grdIdCertificats.RowSelectorPane.Visible = True
        Me.grdIdCertificats.Size = New System.Drawing.Size(480, 216)
        Me.grdIdCertificats.TabIndex = 5
        '
        'cmdAjoutCertificat
        '
        Me.cmdAjoutCertificat.Enabled = False
        Me.cmdAjoutCertificat.Location = New System.Drawing.Point(8, 232)
        Me.cmdAjoutCertificat.Name = "cmdAjoutCertificat"
        Me.cmdAjoutCertificat.Size = New System.Drawing.Size(112, 23)
        Me.cmdAjoutCertificat.TabIndex = 4
        Me.cmdAjoutCertificat.Text = "Ajouter un certificat"
        '
        'tabSel
        '
        Me.tabSel.Controls.Add(Me.grdSels)
        Me.tabSel.Controls.Add(Me.cmdAjoutSel)
        Me.tabSel.Location = New System.Drawing.Point(4, 22)
        Me.tabSel.Name = "tabSel"
        Me.tabSel.Size = New System.Drawing.Size(496, 262)
        Me.tabSel.TabIndex = 2
        Me.tabSel.Text = "Sels"
        '
        'grdSels
        '
        Me.grdSels.ColonneTampon = Nothing
        Me.grdSels.DataRowTemplate = Me.dtrSels
        '
        '
        '
        Me.grdSels.FixedColumnSplitter.Visible = False
        Me.grdSels.FixedHeaderRows.Add(Me.cmrSels)
        Me.grdSels.Location = New System.Drawing.Point(8, 8)
        Me.grdSels.Name = "grdSels"
        '
        '
        '
        Me.grdSels.RowSelectorPane.AllowRowResize = False
        Me.grdSels.RowSelectorPane.Visible = True
        Me.grdSels.Size = New System.Drawing.Size(480, 216)
        Me.grdSels.TabIndex = 6
        '
        'cmdAjoutSel
        '
        Me.cmdAjoutSel.Enabled = False
        Me.cmdAjoutSel.Location = New System.Drawing.Point(8, 232)
        Me.cmdAjoutSel.Name = "cmdAjoutSel"
        Me.cmdAjoutSel.Size = New System.Drawing.Size(112, 23)
        Me.cmdAjoutSel.TabIndex = 5
        Me.cmdAjoutSel.Text = "Ajouter un sel"
        '
        'tsFfGerParmChif
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(520, 365)
        Me.Controls.Add(Me.tabParmChiff)
        Me.Controls.Add(Me.cboType)
        Me.Controls.Add(Me.lblType)
        Me.Controls.Add(Me.cmdFermer)
        Me.Controls.Add(Me.cmdActualiseGrille)
        Me.Controls.Add(Me.cboEnvrn)
        Me.Controls.Add(Me.lblEnvrn)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "tsFfGerParmChif"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Gestion des paramètres de chiffrement"
        Me.tabParmChiff.ResumeLayout(False)
        Me.tabCleVecteur.ResumeLayout(False)
        CType(Me.grdClesVecteurs, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.drtCleVect, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmrCleVect, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabIdCertificat.ResumeLayout(False)
        CType(Me.grdIdCertificats, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.drtIdCertificats, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmrIdCertificats, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabSel.ResumeLayout(False)
        CType(Me.grdSels, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtrSels, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmrSels, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataRow1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ColumnManagerRow1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Variables et propriétés privées"

    Private bSourceChangee As Boolean = False
    Private objSourceGrille As DataRow()

    Private dictEnv As Dictionary(Of String, IGestFichierChif)

    Private ReadOnly Property GestEnvCourant As IGestFichierChif
        Get
            Return dictEnv(cboEnvrn.SelectedItem.ToString())
        End Get
    End Property



#End Region

#Region "Méthodes publiques"

    ''' <summary>
    ''' Ajout d'instances de classes de gestion des environnements, chaque instance connait l'emplacement de ses fichiers et le contenu de ceux-ci
    ''' </summary>
    ''' <param name="env"></param>
    ''' <param name="gestionnaireEnvr"></param>
    Public Sub AjouterGestionnaireEnvironnement(env As String, gestionnaireEnvr As IGestFichierChif)

        If dictEnv Is Nothing Then
            dictEnv = New Dictionary(Of String, IGestFichierChif)
        End If

        'On vérifie si déjà dans le dictionnaire, car on aurait pu injecter des Mocks avant d'ouvrir le formulaire
        If Not dictEnv.ContainsKey(env) Then
            dictEnv.Add(env, gestionnaireEnvr)
        End If

    End Sub

#End Region

#Region "Évènements générals"

    Private Sub tsFfGerParmChif_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim gestFichierChif As TsCuGestFichierChif = Nothing

        'Création des objets pour les différents environnements possibles
        For Each unEnv As String In System.Enum.GetNames(GetType(TsCuParamsChiffrement.Envrn))
            gestFichierChif = New TsCuGestFichierChif(System.Enum.Parse(GetType(TsCuParamsChiffrement.Envrn), unEnv))
            AjouterGestionnaireEnvironnement(unEnv, gestFichierChif)
        Next

        gestFichierChif = dictEnv.Item(TsCuParamsChiffrement.Envrn.Unitaire.ToString)

        Dim environnment As String = XuCuPolitiqueConfig.ConfigDomaine.ValeurSysteme("General", "General\Environnement")
        Select Case environnment
            Case "PROD"
                'Ajout des dépendances entre Unitaires et les autres environnements d'essais seulement quand on est en production.
                gestFichierChif.AjouterDependance(dictEnv.Item(TsCuParamsChiffrement.Envrn.Intégration.ToString))
                gestFichierChif.AjouterDependance(dictEnv.Item(TsCuParamsChiffrement.Envrn.Acceptation.ToString))
                gestFichierChif.AjouterDependance(dictEnv.Item(TsCuParamsChiffrement.Envrn.Formation_Acceptation.ToString))
            Case "INTG"
                'Ajout des dépendances entre Unitaires et intégration quand on est en intégration
                gestFichierChif.AjouterDependance(dictEnv.Item(TsCuParamsChiffrement.Envrn.Intégration.ToString))
        End Select

        RemplirListe()
        InitGrilles()
        cmdActualiseGrille.Enabled = False
        cmdAjoutCleVecteur.Enabled = False

    End Sub

    Private Sub cboEnvrn_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboEnvrn.SelectedIndexChanged
        cmdActualiseGrille.Enabled = True
        ActualiserGrille()
    End Sub

    Private Sub cboType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboType.SelectedIndexChanged
        cmdActualiseGrille.Enabled = True
        ActualiserGrille()
    End Sub

    Private Sub tabParmChiff_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabParmChiff.SelectedIndexChanged
        ActualiserGrille()
    End Sub

    Private Sub cmdActualiseGrille_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdActualiseGrille.Click
        If Not ActualiserGrille() Then
            MessageBox.Show("Vous devez sélectionner un environnement et un type avant d'actualiser la grille.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub cmdFermer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFermer.Click
        If XuCuPolitiqueConfig.ConfigDomaine.ValeurSysteme("General", "General\Environnement").Equals("PROD") Then
            If bSourceChangee Then
                MessageBox.Show("Des fichiers de chiffrement ont été mis à jour." & vbCrLf & "Ne pas oublier de demander des graduations Web pour que ceux-ci soient déployés.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        End If
        End
    End Sub

#End Region

#Region "Évènements de la grille et du bouton d'ajout d'un Cle/Vecteur"

    Private Sub cmdAjoutCleVecteur_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAjoutCleVecteur.Click
        AjoutCleValeur()
    End Sub

    Private Sub AjoutCleValeur()
        Try
            Dim blnResultat As Boolean

            Dim frmACV As New tsFgAjouterCleVecteur
            frmACV.CleVecteurRow = GestEnvCourant.ObtenirNouvlCleVecteur(ObtenirTypeFichier)
            frmACV.CleVecteurAllRows = objSourceGrille
            frmACV.GestFichierChif = GestEnvCourant
            frmACV.TypeCle = ObtenirTypeCle()
            frmACV.TypeFichier = ObtenirTypeFichier()
            frmACV.Envi = cboEnvrn.SelectedItem

            If frmACV.ShowDialog(Me) = DialogResult.OK Then
                blnResultat = GestEnvCourant.Ajouter(frmACV.CleVecteurRow, ObtenirTypeCle, ObtenirTypeFichier)

                If blnResultat Then RemplirGrille()
                bSourceChangee = True
                ActualiserGrille()

            End If
        Catch Ex As TS6N631_ZpTrtParmChif.TsCuExceptionErreurValdt
            MessageBox.Show(Ex.Message)
        Catch exErr As TS6N631_ZpTrtParmChif.TsCuExceptionErreur
            MessageBox.Show(exErr.Message)
        End Try

        ActualiserGrille()

    End Sub


    Private Sub grdClesVecteurs_ValueChanging(ByVal sender As Object, ByVal e As Xceed.Grid.ValueChangingEventArgs)

        Dim cellule As Xceed.Grid.Cell = DirectCast(sender, Xceed.Grid.Cell)
        Dim donnees() As DataRow = Nothing
        'Obtenir les données les données
        donnees = grdClesVecteurs.DataSource
        If (Not (donnees Is Nothing) AndAlso donnees.Length > 0) Then
            'Assigner la nouvelle valeur dans la cellule de la rangée éditée
            donnees(grdClesVecteurs.DataRows.IndexOf(cellule.ParentRow))("Actif") = e.NewValue
            'On indique que l'édition est terminée
            cellule.ParentRow.EndEdit()
        End If

        'On mets à jour la source de données.
        bSourceChangee = GestEnvCourant.MettreAJourSource(True, True, ObtenirTypeFichier)

    End Sub


#End Region

#Region "Évènements de la grille et du bouton d'ajout d'un Certificat"

    Private Sub cmdAjoutCertificat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAjoutCertificat.Click
        Try
            Dim blnResultat As Boolean

            Dim frmAC As New tsFgAjouterCertificat
            frmAC.CertificatRow = GestEnvCourant.ObtenirNouvlCleVecteur(ObtenirTypeFichier)

            If frmAC.ShowDialog(Me) = DialogResult.OK Then
                blnResultat = GestEnvCourant.Ajouter(frmAC.CertificatRow, ObtenirTypeCle, ObtenirTypeFichier)

                If blnResultat Then RemplirGrille()
                ActualiserGrille()
            End If
        Catch Ex As TS6N631_ZpTrtParmChif.TsCuExceptionErreurValdt
            MessageBox.Show(Ex.Message)
        Catch exErr As TS6N631_ZpTrtParmChif.TsCuExceptionErreur
            MessageBox.Show(exErr.Message)
        End Try

        ActualiserGrille()

    End Sub

    Private Sub grdIdCertificats_ValueChanging(ByVal sender As Object, ByVal e As Xceed.Grid.ValueChangingEventArgs)

        Dim cellule As Xceed.Grid.Cell = DirectCast(sender, Xceed.Grid.Cell)
        Dim donnees() As DataRow = Nothing

        'Obtenir les données les données
        donnees = grdIdCertificats.DataSource

        If (Not (donnees Is Nothing) AndAlso donnees.Length > 0) Then

            'Assigner la nouvelle valeur dans la cellule de la rangée éditée
            donnees(grdIdCertificats.DataRows.IndexOf(cellule.ParentRow))("Actif") = e.NewValue

            'On indique que l'édition est terminée
            cellule.ParentRow.EndEdit()

        End If

        'On mets à jour la source de données.
        bSourceChangee = GestEnvCourant.MettreAJourSource(False, True, ObtenirTypeFichier)

    End Sub

#End Region

#Region "Évènements de la grille et du bouton d'ajout d'un Sel"

    Private Sub cmdAjoutSel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAjoutSel.Click
        Try
            Dim blnResultat As Boolean

            Dim frmAS As New tsFgAjouterSel
            frmAS.SelRow = GestEnvCourant.ObtenirNouvlCleVecteur(ObtenirTypeFichier)

            If frmAS.ShowDialog(Me) = DialogResult.OK Then
                blnResultat = GestEnvCourant.Ajouter(frmAS.SelRow, ObtenirTypeCle, ObtenirTypeFichier)
                If blnResultat Then RemplirGrille()
                ActualiserGrille()
            End If

        Catch Ex As TS6N631_ZpTrtParmChif.TsCuExceptionErreurValdt
            MessageBox.Show(Ex.Message)
        Catch exErr As TS6N631_ZpTrtParmChif.TsCuExceptionErreur
            MessageBox.Show(exErr.Message)
        End Try

        ActualiserGrille()

    End Sub

    Private Sub grdSels_ValueChanging(ByVal sender As Object, ByVal e As Xceed.Grid.ValueChangingEventArgs)


        Dim cellule As Xceed.Grid.Cell = DirectCast(sender, Xceed.Grid.Cell)
        Dim donnees() As DataRow = Nothing

        'Obtenir les données
        donnees = grdSels.DataSource

        If (Not (donnees Is Nothing) AndAlso donnees.Length > 0) Then

            'Assigner la nouvelle valeur dans la cellule de la rangée éditée
            donnees(grdSels.DataRows.IndexOf(cellule.ParentRow))("Actif") = e.NewValue

            'On indique que l'édition est terminée
            cellule.ParentRow.EndEdit()

        End If

        'On mets à jour la source de données.
        bSourceChangee = GestEnvCourant.MettreAJourSource(False, True, ObtenirTypeFichier)

    End Sub


#End Region

#Region "Fonctions et méthodes privées"

    ''' <summary>
    ''' Basé sur le choix dans le drop down de type de code, retourne la bonne valeur d'enum
    ''' </summary>
    ''' <returns></returns>
    Private Function ObtenirTypeCle() As TypeCle
        Return System.Enum.Parse(GetType(TsCuParamsChiffrement.TypeCle), cboType.SelectedItem.ToString)
    End Function

    ''' <summary>
    ''' Basé sur le choix dans le drop down de type de code, retourne la bonne valeur d'enum
    ''' </summary>
    ''' <returns></returns>
    Private Function ObtenirTypeFichier() As TypeFichier
        Select Case tabParmChiff.SelectedTab.Name
            Case tabCleVecteur.Name
                Return TypeFichier.Chiffrement
            Case tabIdCertificat.Name
                Return TypeFichier.Certificat
            Case tabSel.Name
                Return TypeFichier.Sel
        End Select
    End Function

    ''' --------------------------------------------------------------------------------
    ''' Class.Method:	tsFfGerParmChif.RemplirGrille
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <exception cref="Rrq.CS.ServicesCommuns.ScenarioTransactionnel.XZCuRrqException">
    ''' 	Cette exception est lancée si...
    ''' </exception>
    ''' <remarks><para><pre>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2005-02-22	t209376		Création initiale
    ''' 
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    Private Sub RemplirGrille()

        objSourceGrille = GestEnvCourant.ObtenirSource(ObtenirTypeCle, ObtenirTypeFichier)

        Select Case tabParmChiff.SelectedTab.Name
            Case tabCleVecteur.Name
                SetDataBindingGrille(grdClesVecteurs, objSourceGrille)
            Case tabIdCertificat.Name
                SetDataBindingGrille(grdIdCertificats, objSourceGrille)
            Case tabSel.Name
                SetDataBindingGrille(grdSels, objSourceGrille)
        End Select
    End Sub


    ''' --------------------------------------------------------------------------------
    ''' Class.Method:	tsFfGerParmChif.RemplirListe
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <exception cref="Rrq.CS.ServicesCommuns.ScenarioTransactionnel.XZCuRrqException">
    ''' 	Cette exception est lancée si...
    ''' </exception>
    ''' <remarks><para><pre>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2005-02-22	t209376		Création initiale
    ''' 
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    Private Sub RemplirListe()

        Dim environnment As String = XuCuPolitiqueConfig.ConfigDomaine.ValeurSysteme("General", "General\Environnement")
        Select Case (environnment)
            Case "UNIT"
                cboEnvrn.Items.Add(System.Enum.GetName(GetType(TsCuParamsChiffrement.Envrn), TsCuParamsChiffrement.Envrn.Unitaire))
            Case "INTG"
                cboEnvrn.Items.Add(System.Enum.GetName(GetType(TsCuParamsChiffrement.Envrn), TsCuParamsChiffrement.Envrn.Unitaire))
                cboEnvrn.Items.Add(System.Enum.GetName(GetType(TsCuParamsChiffrement.Envrn), TsCuParamsChiffrement.Envrn.Intégration))
            Case Else
                For Each unEnv As String In System.Enum.GetNames(GetType(TsCuParamsChiffrement.Envrn))
                    cboEnvrn.Items.Add(unEnv)
                Next
        End Select

        For Each unType As String In System.Enum.GetNames(GetType(TsCuParamsChiffrement.TypeCle))
            cboType.Items.Add(unType)
        Next

    End Sub

    Private Function ActualiserGrille() As Boolean

        If cboEnvrn.SelectedIndex = -1 Or cboType.SelectedIndex = -1 Then
            Return False
        Else
            cmdAjoutCleVecteur.Visible = False
            cmdAjoutCleVecteur.Enabled = False

            Select Case GestEnvCourant.Environnement
                Case TsCuParamsChiffrement.Envrn.Unitaire, TsCuParamsChiffrement.Envrn.Production, TsCuParamsChiffrement.Envrn.Simulation
                    cmdAjoutCleVecteur.Visible = True
                    cmdAjoutCleVecteur.Enabled = True
            End Select

            RemplirGrille()

            cmdAjoutCertificat.Enabled = (ObtenirTypeCle() = TypeCle.SQAG)
            cmdAjoutSel.Enabled = (ObtenirTypeCle() = TypeCle.SQAG)

            Return True

        End If

    End Function


    ''' <summary>
    ''' Initialise les grilles de la vue.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitGrilles()

        InitGrilleClesVecteurs()
        InitGrilleIdCertificats()
        InitGrilleSels()

    End Sub

#Region " InitGrilleClesVecteurs "

    ''' <summary>
    ''' Initialise la grille Clés et Vecteurs
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitGrilleClesVecteurs()

        ' ------------------------------------------------------------------------------
        ' Mettre la grille en mode initialisation
        ' ------------------------------------------------------------------------------
        grdClesVecteurs.BeginInit()

        ' ------------------------------------------------------------------------------
        ' Effectuer le databinding
        ' ------------------------------------------------------------------------------
        grdClesVecteurs.DataSource = ObtenirGabaritClesVecteurs()

        ' ------------------------------------------------------------------------------
        ' Définiton des propriétés de la grille.
        ' ------------------------------------------------------------------------------
        With grdClesVecteurs
            ' Permetre la sélection d'une seule ligne à la fois
            .SelectionMode = SelectionMode.None

            ' Permettre l'édition des cellules sur le click au lieu du double-click
            '*** Pris en charge par XZ5N909 .SingleClickEdit = True

            ' Permettre la navigation par cellule
            '*** Pris en charge par XZ5N909 .AllowCellNavigation = True

            ' Forcer la barre de défilement vertical
            .ScrollBars = Xceed.Grid.GridScrollBars.ForcedVertical

            ' Mettre la grille en maj. 
            '*** Pris en charge par XZ5N909 .ReadOnly = False

            ' Ne pas permettre de modifier la hauteur des lignes et le "Drag and Drop"
            .RowSelectorPane.AllowRowResize = False
            .RowSelectorPane.AllowDrop = False

        End With

        ' ------------------------------------------------------------------------------
        ' Propriété du gestionnaire d'entêtes de colonne.
        ' ------------------------------------------------------------------------------
        With cmrCleVect
            ' Ne pas permettre de modifier la largeur des colonnes et le "Drag and Drop"
            .AllowColumnResize = True
            .AllowColumnReorder = False
            .AllowDrop = False

            ' Interdire le tri
            .AllowSort = False

            ' Ajuster l'alignement des entêtes de colonne
            .HorizontalAlignment = Xceed.Grid.HorizontalAlignment.Left

        End With

        ' ------------------------------------------------------------------------------
        ' Propriétés du modèle de lignes ("DataRowTemplate").
        ' ------------------------------------------------------------------------------
        With drtCleVect
            ' Activer le concept de ligne courante
            .CanBeCurrent = True

            ' Ajuster l'alignement à gauche des cellules de la grille
            .HorizontalAlignment = Xceed.Grid.HorizontalAlignment.Left
        End With

        ' ------------------------------------------------------------------------------
        ' Définiton des colonnes
        ' ------------------------------------------------------------------------------

        ' *** Définiton de la colonne Code ***
        With grdClesVecteurs.Columns("Code")
            ' Lageur
            .Width = 155
            ' Titre
            .Title = "Code"
            ' Index d'affichage
            .VisibleIndex = 0

            ' Verouiller la colonne Code car elle affichera des images seulement.
            .ReadOnly = True

            ' Modification du CellViewerManager et CellEditorManager                        
            .CellViewerManager = New Grilles.Visualisateurs.XzCrTexte
        End With

        ' *** Définiton de la colonne Cle ***
        With grdClesVecteurs.Columns("Cle")
            ' Lageur
            .Width = 120
            ' Titre
            .Title = "Clé"
            ' Index d'affichage
            .VisibleIndex = 1

            ' Verouiller la colonne Cle car elle affichera des images seulement.
            .ReadOnly = True

            ' Modification du CellViewerManager et CellEditorManager
            .CellViewerManager = New Grilles.Visualisateurs.XzCrTexte

        End With

        ' *** Définiton de la colonne Vecteur ***
        With grdClesVecteurs.Columns("Vecteur")
            ' Lageur
            .Width = 120
            ' Titre
            .Title = "Vecteur"
            ' Index d'affichage
            .VisibleIndex = 2

            ' Verouiller la colonne Cle car elle affichera des images seulement.
            .ReadOnly = True

            ' Modification du CellViewerManager et CellEditorManager
            .CellViewerManager = New Grilles.Visualisateurs.XzCrTexte

        End With

        ' *** Définiton de la colonne Actif ***
        With grdClesVecteurs.Columns("Actif")
            ' Lageur
            .Width = 48
            ' Titre
            .Title = "Actif"
            ' Index d'affichage
            .VisibleIndex = 3

            ' Modification du CellEditorManager
            .CellEditorManager = New Grilles.Editeurs.XzCrCaseACocher(True, False)
            .CellViewerManager = New Grilles.Visualisateurs.XzCrCaseACocher(True, False)

        End With


        '' Masquer les colonnes non-désirées.
        grdClesVecteurs.Columns("Table").Visible = False
        grdClesVecteurs.Columns("Type").Visible = False
        grdClesVecteurs.Columns("RowState").Visible = False
        grdClesVecteurs.Columns("ID").Visible = False
        grdClesVecteurs.Columns("HasErrors").Visible = False
        grdClesVecteurs.Columns("RowError").Visible = False

        ' ------------------------------------------------------------------------------
        ' Définiton des Handlers.
        ' ------------------------------------------------------------------------------
        Dim cell As Xceed.Grid.DataCell
        ' Attacher un Handler sur les événements EnteringEdit, EditEntered, LeavingEdit, ValueChanging,
        ' ValueChanged, EditLeft, SiblingValueChanged et ValidationError des DataCell de la grille
        For Each cell In grdClesVecteurs.DataRowTemplate.Cells

            AddHandler cell.ValueChanging, New Xceed.Grid.ValueChangingEventHandler(AddressOf Me.grdClesVecteurs_ValueChanging)

        Next cell

        ' ------------------------------------------------------------------------------
        ' Terminer le mode initialisation
        ' ------------------------------------------------------------------------------
        grdClesVecteurs.EndInit()

        ' ------------------------------------------------------------------------------
        ' Verouiller les colonnes
        ' IMPORTANT : Il est important d'effectuer le vérouillage après le endinit
        ' ------------------------------------------------------------------------------
        grdClesVecteurs.VerrouillerColonne("Code") = True
        grdClesVecteurs.VerrouillerColonne("Cle") = True
        grdClesVecteurs.VerrouillerColonne("Vecteur") = True
        grdClesVecteurs.VerrouillerColonne("Actif") = False

    End Sub

#End Region

#Region " InitGrilleIdCertificats "

    ''' <summary>
    ''' Initialise la grille Id Certificats
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitGrilleIdCertificats()

        ' ------------------------------------------------------------------------------
        ' Mettre la grille en mode initialisation
        ' ------------------------------------------------------------------------------
        grdIdCertificats.BeginInit()

        ' ------------------------------------------------------------------------------
        ' Effectuer le databinding
        ' ------------------------------------------------------------------------------
        grdIdCertificats.DataSource = ObtenirGabaritCertificats()

        ' ------------------------------------------------------------------------------
        ' Définiton des propriétés de la grille.
        ' ------------------------------------------------------------------------------
        With grdIdCertificats
            ' Permetre la sélection d'une seule ligne à la fois
            .SelectionMode = SelectionMode.None

            ' Permettre l'édition des cellules sur le click au lieu du double-click
            '*** Pris en charge par XZ5N909 .SingleClickEdit = True

            ' Permettre la navigation par cellule
            '*** Pris en charge par XZ5N909 .AllowCellNavigation = True

            ' Forcer la barre de défilement vertical
            .ScrollBars = Xceed.Grid.GridScrollBars.ForcedVertical

            ' Mettre la grille en maj. 
            '*** Pris en charge par XZ5N909 .ReadOnly = False

            ' Ne pas permettre de modifier la hauteur des lignes et le "Drag and Drop"
            .RowSelectorPane.AllowRowResize = False
            .RowSelectorPane.AllowDrop = False

        End With

        ' ------------------------------------------------------------------------------
        ' Propriété du gestionnaire d'entêtes de colonne.
        ' ------------------------------------------------------------------------------
        With cmrIdCertificats
            ' Ne pas permettre de modifier la largeur des colonnes et le "Drag and Drop"
            .AllowColumnResize = True
            .AllowColumnReorder = False
            .AllowDrop = False

            ' Interdire le tri
            .AllowSort = False

            ' Ajuster l'alignement des entêtes de colonne
            .HorizontalAlignment = Xceed.Grid.HorizontalAlignment.Left

        End With

        ' ------------------------------------------------------------------------------
        ' Propriétés du modèle de lignes ("DataRowTemplate").
        ' ------------------------------------------------------------------------------
        With drtIdCertificats
            ' Activer le concept de ligne courante
            .CanBeCurrent = True

            ' Ajuster l'alignement à gauche des cellules de la grille
            .HorizontalAlignment = Xceed.Grid.HorizontalAlignment.Left
        End With

        ' ------------------------------------------------------------------------------
        ' Définiton des colonnes
        ' ------------------------------------------------------------------------------

        ' *** Définiton de la colonne Code ***
        With grdIdCertificats.Columns("Code")
            ' Lageur
            .Width = 155
            ' Titre
            .Title = "Code"
            ' Index d'affichage
            .VisibleIndex = 0

            ' Verouiller la colonne Code car elle affichera des images seulement.
            .ReadOnly = True

            ' Modification du CellViewerManager et CellEditorManager                        
            .CellViewerManager = New Grilles.Visualisateurs.XzCrTexte
        End With

        ' *** Définiton de la colonne IdCertificat ***
        With grdIdCertificats.Columns("IdCertificat")
            ' Lageur
            .Width = 120
            ' Titre
            .Title = "Id du certificat"
            ' Index d'affichage
            .VisibleIndex = 1

            ' Verouiller la colonne Cle car elle affichera des images seulement.
            .ReadOnly = True

            ' Modification du CellViewerManager et CellEditorManager
            .CellViewerManager = New Grilles.Visualisateurs.XzCrTexte

        End With

        ' *** Définiton de la colonne NomMagasin ***
        With grdIdCertificats.Columns("NomMagasin")
            ' Lageur
            .Width = 120
            ' Titre
            .Title = "Nom du magasin"
            ' Index d'affichage
            .VisibleIndex = 2

            ' Verouiller la colonne Cle car elle affichera des images seulement.
            .ReadOnly = True

            ' Modification du CellViewerManager et CellEditorManager
            .CellViewerManager = New Grilles.Visualisateurs.XzCrTexte

        End With

        ' *** Définiton de la colonne Actif ***
        With grdIdCertificats.Columns("Actif")
            ' Lageur
            .Width = 48
            ' Titre
            .Title = "Actif"
            ' Index d'affichage
            .VisibleIndex = 3

            ' Modification du CellEditorManager
            .CellEditorManager = New Grilles.Editeurs.XzCrCaseACocher(True, False)
            .CellViewerManager = New Grilles.Visualisateurs.XzCrCaseACocher(True, False)

        End With


        '' Masquer les colonnes non-désirées.
        grdIdCertificats.Columns("Table").Visible = False
        grdIdCertificats.Columns("Type").Visible = False
        grdIdCertificats.Columns("RowState").Visible = False
        grdIdCertificats.Columns("ID").Visible = False
        grdIdCertificats.Columns("HasErrors").Visible = False
        grdIdCertificats.Columns("RowError").Visible = False

        ' ------------------------------------------------------------------------------
        ' Définiton des Handlers.
        ' ------------------------------------------------------------------------------
        Dim cell As Xceed.Grid.DataCell
        ' Attacher un Handler sur les événements EnteringEdit, EditEntered, LeavingEdit, ValueChanging,
        ' ValueChanged, EditLeft, SiblingValueChanged et ValidationError des DataCell de la grille
        For Each cell In grdIdCertificats.DataRowTemplate.Cells

            AddHandler cell.ValueChanging, New Xceed.Grid.ValueChangingEventHandler(AddressOf Me.grdIdCertificats_ValueChanging)

        Next cell

        ' ------------------------------------------------------------------------------
        ' Terminer le mode initialisation
        ' ------------------------------------------------------------------------------
        grdIdCertificats.EndInit()

        ' ------------------------------------------------------------------------------
        ' Verouiller les colonnes
        ' IMPORTANT : Il est important d'effectuer le vérouillage après le endinit
        ' ------------------------------------------------------------------------------
        grdIdCertificats.VerrouillerColonne("Code") = True
        grdIdCertificats.VerrouillerColonne("IdCertificat") = True
        grdIdCertificats.VerrouillerColonne("NomMagasin") = True
        grdIdCertificats.VerrouillerColonne("Actif") = False

    End Sub

#End Region

#Region " InitGrilleSels "

    ''' <summary>
    ''' Initialise la grille Sels
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitGrilleSels()

        ' ------------------------------------------------------------------------------
        ' Mettre la grille en mode initialisation
        ' ------------------------------------------------------------------------------
        grdSels.BeginInit()

        ' ------------------------------------------------------------------------------
        ' Effectuer le databinding
        ' ------------------------------------------------------------------------------
        grdSels.DataSource = ObtenirGabaritSels()

        ' ------------------------------------------------------------------------------
        ' Définiton des propriétés de la grille.
        ' ------------------------------------------------------------------------------
        With grdSels
            ' Permetre la sélection d'une seule ligne à la fois
            .SelectionMode = SelectionMode.None

            ' Permettre l'édition des cellules sur le click au lieu du double-click
            '*** Pris en charge par XZ5N909 .SingleClickEdit = True

            ' Permettre la navigation par cellule
            '*** Pris en charge par XZ5N909 .AllowCellNavigation = True

            ' Forcer la barre de défilement vertical
            .ScrollBars = Xceed.Grid.GridScrollBars.ForcedVertical

            ' Mettre la grille en maj. 
            '*** Pris en charge par XZ5N909 .ReadOnly = False

            ' Ne pas permettre de modifier la hauteur des lignes et le "Drag and Drop"
            .RowSelectorPane.AllowRowResize = False
            .RowSelectorPane.AllowDrop = False

        End With

        ' ------------------------------------------------------------------------------
        ' Propriété du gestionnaire d'entêtes de colonne.
        ' ------------------------------------------------------------------------------
        With cmrSels
            ' Ne pas permettre de modifier la largeur des colonnes et le "Drag and Drop"
            .AllowColumnResize = True
            .AllowColumnReorder = False
            .AllowDrop = False

            ' Interdire le tri
            .AllowSort = False

            ' Ajuster l'alignement des entêtes de colonne
            .HorizontalAlignment = Xceed.Grid.HorizontalAlignment.Left

        End With

        ' ------------------------------------------------------------------------------
        ' Propriétés du modèle de lignes ("DataRowTemplate").
        ' ------------------------------------------------------------------------------
        With dtrSels
            ' Activer le concept de ligne courante
            .CanBeCurrent = True

            ' Ajuster l'alignement à gauche des cellules de la grille
            .HorizontalAlignment = Xceed.Grid.HorizontalAlignment.Left
        End With

        ' ------------------------------------------------------------------------------
        ' Définiton des colonnes
        ' ------------------------------------------------------------------------------

        ' *** Définiton de la colonne Code ***
        With grdSels.Columns("Code")
            ' Lageur
            .Width = 155
            ' Titre
            .Title = "Code"
            ' Index d'affichage
            .VisibleIndex = 0

            ' Verouiller la colonne Code car elle affichera des images seulement.
            .ReadOnly = True

            ' Modification du CellViewerManager et CellEditorManager                        
            .CellViewerManager = New Grilles.Visualisateurs.XzCrTexte
        End With

        ' *** Définiton de la colonne Sel ***
        With grdSels.Columns("Sel")
            ' Lageur
            .Width = 120
            ' Titre
            .Title = "Sel"
            ' Index d'affichage
            .VisibleIndex = 1

            ' Verouiller la colonne Cle car elle affichera des images seulement.
            .ReadOnly = True

            ' Modification du CellViewerManager et CellEditorManager
            .CellViewerManager = New Grilles.Visualisateurs.XzCrTexte

        End With


        ' *** Définiton de la colonne Actif ***
        With grdSels.Columns("Actif")
            ' Lageur
            .Width = 168
            ' Titre
            .Title = "Actif"
            ' Index d'affichage
            .VisibleIndex = 3

            ' Modification du CellEditorManager
            .CellEditorManager = New Grilles.Editeurs.XzCrCaseACocher(True, False)
            .CellViewerManager = New Grilles.Visualisateurs.XzCrCaseACocher(True, False)

        End With


        '' Masquer les colonnes non-désirées.
        grdSels.Columns("Table").Visible = False
        grdSels.Columns("Type").Visible = False
        grdSels.Columns("RowState").Visible = False
        grdSels.Columns("ID").Visible = False
        grdSels.Columns("HasErrors").Visible = False
        grdSels.Columns("RowError").Visible = False

        ' ------------------------------------------------------------------------------
        ' Définiton des Handlers.
        ' ------------------------------------------------------------------------------
        Dim cell As Xceed.Grid.DataCell
        ' Attacher un Handler sur les événements EnteringEdit, EditEntered, LeavingEdit, ValueChanging,
        ' ValueChanged, EditLeft, SiblingValueChanged et ValidationError des DataCell de la grille
        For Each cell In grdSels.DataRowTemplate.Cells

            AddHandler cell.ValueChanging, New Xceed.Grid.ValueChangingEventHandler(AddressOf Me.grdSels_ValueChanging)

        Next cell

        ' ------------------------------------------------------------------------------
        ' Terminer le mode initialisation
        ' ------------------------------------------------------------------------------
        grdSels.EndInit()

        ' ------------------------------------------------------------------------------
        ' Verouiller les colonnes
        ' IMPORTANT : Il est important d'effectuer le vérouillage après le endinit
        ' ------------------------------------------------------------------------------
        grdSels.VerrouillerColonne("Code") = True
        grdSels.VerrouillerColonne("Sel") = True
        grdSels.VerrouillerColonne("Actif") = False


    End Sub

#End Region

    ''' <summary>
    ''' Assigne les données à la grille et créer les colonnes
    ''' appropriées même si le datatable est vide.
    ''' </summary>
    ''' <param name="grille">la grille</param>
    ''' <param name="lignesDonnees">data table contenant les données</param>
    ''' <remarks></remarks>
    Private Sub SetDataBindingGrille(ByVal grille As Grilles.XzCrGrille, ByVal lignesDonnees() As DataRow)
        grille.BeginInit()
        If (Not (lignesDonnees Is Nothing) AndAlso _
            lignesDonnees.Length > 0) Then
            'S'il y a des données on assigne la table à la grille
            grille.DataSource = lignesDonnees
        Else
            'vu qu'il n'y a pas de données on assigne le gabrit
            'afin d'y afficher les colonnes appropriées.
            Select Case (grille.Name)
                Case "grdClesVecteurs"
                    grille.DataSource = ObtenirGabaritClesVecteurs()
                Case "grdIdCertificats"
                    grille.DataSource = ObtenirGabaritCertificats()
                Case "grdSels"
                    grille.DataSource = ObtenirGabaritSels()
            End Select
        End If
        grille.EndInit()
    End Sub

#Region " Gabarits pour grilles "

    ''' <summary>
    ''' Gabarit pour la grille Clé-Vecteur.
    ''' </summary>
    ''' <returns>retourne une table vide contenant seulement le nom des colonnes</returns>
    ''' <remarks></remarks>
    Private Function ObtenirGabaritClesVecteurs() As DataTable

        Dim tblGabarit As DataTable

        tblGabarit = New DataTable()
        tblGabarit.Columns.Add("Code")
        tblGabarit.Columns.Add("Cle")
        tblGabarit.Columns.Add("Vecteur")
        tblGabarit.Columns.Add("Actif")
        tblGabarit.Columns.Add("Table")
        tblGabarit.Columns.Add("Type")
        tblGabarit.Columns.Add("RowState")
        tblGabarit.Columns.Add("ID")
        tblGabarit.Columns.Add("HasErrors")
        tblGabarit.Columns.Add("RowError")

        Return tblGabarit

    End Function


    ''' <summary>
    ''' Gabarit pour la grille Id Certificat.
    ''' </summary>
    ''' <returns>retourne une table vide contenant seulement le nom des colonnes</returns>
    ''' <remarks></remarks>
    Private Function ObtenirGabaritCertificats() As DataTable

        Dim tblGabarit As DataTable

        tblGabarit = New DataTable()
        tblGabarit.Columns.Add("Code")
        tblGabarit.Columns.Add("IdCertificat")
        tblGabarit.Columns.Add("NomMagasin")
        tblGabarit.Columns.Add("Actif")
        tblGabarit.Columns.Add("Table")
        tblGabarit.Columns.Add("Type")
        tblGabarit.Columns.Add("RowState")
        tblGabarit.Columns.Add("ID")
        tblGabarit.Columns.Add("HasErrors")
        tblGabarit.Columns.Add("RowError")


        Return tblGabarit

    End Function

    ''' <summary>
    ''' Gabarit pour la grille Sels
    ''' </summary>
    ''' <returns>retourne une table vide contenant seulement le nom des colonnes</returns>
    ''' <remarks></remarks>
    Private Function ObtenirGabaritSels() As DataTable

        Dim tblGabarit As DataTable

        tblGabarit = New DataTable()
        tblGabarit.Columns.Add("Code")
        tblGabarit.Columns.Add("Sel")
        tblGabarit.Columns.Add("Actif")
        tblGabarit.Columns.Add("Table")
        tblGabarit.Columns.Add("Type")
        tblGabarit.Columns.Add("RowState")
        tblGabarit.Columns.Add("ID")
        tblGabarit.Columns.Add("HasErrors")
        tblGabarit.Columns.Add("RowError")


        Return tblGabarit

    End Function


#End Region

#End Region

End Class
