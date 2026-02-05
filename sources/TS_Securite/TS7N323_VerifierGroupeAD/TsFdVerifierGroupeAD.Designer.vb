<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TsFdVerifierGroupeAD
    Inherits Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XzCrFormAutonome

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.XzCrListView1 = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrListView()
        Me.XzCrButtonEnregistrer = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton()
        Me.XzCrComboBoxFiltreUniteAdministrative = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XzCrComboBox()
        Me.XzCrLabel2 = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel()
        Me.XzCrTextboxRepertoireSauvegarde = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTextbox()
        Me.XzCrLabel1 = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel()
        Me.XzCrButtonComparer = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton()
        Me.XzCrGroupBoxDifferences = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrGroupBox()
        Me.XzCrLabel3 = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel()
        Me.XzCrButtonAfficherDifferences = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton()
        Me.XzCrLabelDifferences = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel()
        Me.XzCrGroupBoxDetail = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrGroupBox()
        Me.XzCrLabelDetails = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel()
        Me.XzCrTreeView1 = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTreeView()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.XzCrLabel5 = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel()
        Me.XzCrTxtAttributDeRecherche = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTextbox()
        Me.XzCrLabel4 = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel()
        Me.XzCrCboServeurActiveDirectory = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XzCrComboBox()
        Me.XzCrGroupBoxDifferences.SuspendLayout()
        Me.XzCrGroupBoxDetail.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'XzCrListView1
        '
        Me.XzCrListView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.XzCrListView1.CheckBoxes = True
        Me.XzCrListView1.Location = New System.Drawing.Point(15, 160)
        Me.XzCrListView1.MultiSelect = False
        Me.XzCrListView1.Name = "XzCrListView1"
        Me.XzCrListView1.Size = New System.Drawing.Size(537, 628)
        Me.XzCrListView1.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.XzCrListView1.TabIndex = 9
        Me.XzCrListView1.UseCompatibleStateImageBehavior = False
        Me.XzCrListView1.View = System.Windows.Forms.View.List
        '
        'XzCrButtonEnregistrer
        '
        Me.XzCrButtonEnregistrer.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.XzCrButtonEnregistrer.Location = New System.Drawing.Point(408, 128)
        Me.XzCrButtonEnregistrer.Name = "XzCrButtonEnregistrer"
        Me.XzCrButtonEnregistrer.Size = New System.Drawing.Size(144, 26)
        Me.XzCrButtonEnregistrer.TabIndex = 8
        Me.XzCrButtonEnregistrer.Text = "Enregistrer"
        Me.XzCrButtonEnregistrer.UseVisualStyleBackColor = True
        '
        'XzCrComboBoxFiltreUniteAdministrative
        '
        Me.XzCrComboBoxFiltreUniteAdministrative.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.XzCrComboBoxFiltreUniteAdministrative.FormattingEnabled = True
        Me.XzCrComboBoxFiltreUniteAdministrative.GererTabCommeEnter = False
        Me.XzCrComboBoxFiltreUniteAdministrative.Location = New System.Drawing.Point(230, 34)
        Me.XzCrComboBoxFiltreUniteAdministrative.Name = "XzCrComboBoxFiltreUniteAdministrative"
        Me.XzCrComboBoxFiltreUniteAdministrative.NomChampsDonnee = Nothing
        Me.XzCrComboBoxFiltreUniteAdministrative.NomSourceDonnee = Nothing
        Me.XzCrComboBoxFiltreUniteAdministrative.Size = New System.Drawing.Size(322, 23)
        Me.XzCrComboBoxFiltreUniteAdministrative.TabIndex = 3
        '
        'XzCrLabel2
        '
        Me.XzCrLabel2.AutoSize = True
        Me.XzCrLabel2.Location = New System.Drawing.Point(12, 42)
        Me.XzCrLabel2.Name = "XzCrLabel2"
        Me.XzCrLabel2.Size = New System.Drawing.Size(170, 15)
        Me.XzCrLabel2.TabIndex = 2
        Me.XzCrLabel2.Text = "Filtre sur l'unité administrative"
        '
        'XzCrTextboxRepertoireSauvegarde
        '
        Me.XzCrTextboxRepertoireSauvegarde.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.XzCrTextboxRepertoireSauvegarde.Location = New System.Drawing.Point(230, 7)
        Me.XzCrTextboxRepertoireSauvegarde.Name = "XzCrTextboxRepertoireSauvegarde"
        Me.XzCrTextboxRepertoireSauvegarde.NomChampsDonnee = Nothing
        Me.XzCrTextboxRepertoireSauvegarde.NomSourceDonnee = Nothing
        Me.XzCrTextboxRepertoireSauvegarde.Size = New System.Drawing.Size(322, 21)
        Me.XzCrTextboxRepertoireSauvegarde.TabIndex = 1
        '
        'XzCrLabel1
        '
        Me.XzCrLabel1.AutoSize = True
        Me.XzCrLabel1.Location = New System.Drawing.Point(12, 13)
        Me.XzCrLabel1.Name = "XzCrLabel1"
        Me.XzCrLabel1.Size = New System.Drawing.Size(150, 15)
        Me.XzCrLabel1.TabIndex = 0
        Me.XzCrLabel1.Text = "Repertoire de sauvegarde"
        '
        'XzCrButtonComparer
        '
        Me.XzCrButtonComparer.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.XzCrButtonComparer.Location = New System.Drawing.Point(377, 117)
        Me.XzCrButtonComparer.Name = "XzCrButtonComparer"
        Me.XzCrButtonComparer.Size = New System.Drawing.Size(186, 26)
        Me.XzCrButtonComparer.TabIndex = 1
        Me.XzCrButtonComparer.Text = "Comparer"
        Me.XzCrButtonComparer.UseVisualStyleBackColor = True
        '
        'XzCrGroupBoxDifferences
        '
        Me.XzCrGroupBoxDifferences.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.XzCrGroupBoxDifferences.Controls.Add(Me.XzCrLabel3)
        Me.XzCrGroupBoxDifferences.Controls.Add(Me.XzCrButtonAfficherDifferences)
        Me.XzCrGroupBoxDifferences.Controls.Add(Me.XzCrLabelDifferences)
        Me.XzCrGroupBoxDifferences.Location = New System.Drawing.Point(9, 149)
        Me.XzCrGroupBoxDifferences.Name = "XzCrGroupBoxDifferences"
        Me.XzCrGroupBoxDifferences.Size = New System.Drawing.Size(554, 639)
        Me.XzCrGroupBoxDifferences.TabIndex = 2
        Me.XzCrGroupBoxDifferences.TabStop = False
        Me.XzCrGroupBoxDifferences.Text = "Différences entre deux fichiers"
        '
        'XzCrLabel3
        '
        Me.XzCrLabel3.AutoSize = True
        Me.XzCrLabel3.Location = New System.Drawing.Point(9, 132)
        Me.XzCrLabel3.Name = "XzCrLabel3"
        Me.XzCrLabel3.Size = New System.Drawing.Size(52, 15)
        Me.XzCrLabel3.TabIndex = 1
        Me.XzCrLabel3.Text = "Détails :"
        '
        'XzCrButtonAfficherDifferences
        '
        Me.XzCrButtonAfficherDifferences.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.XzCrButtonAfficherDifferences.Location = New System.Drawing.Point(362, 607)
        Me.XzCrButtonAfficherDifferences.Name = "XzCrButtonAfficherDifferences"
        Me.XzCrButtonAfficherDifferences.Size = New System.Drawing.Size(186, 26)
        Me.XzCrButtonAfficherDifferences.TabIndex = 2
        Me.XzCrButtonAfficherDifferences.Text = "Agrandir le détail"
        Me.XzCrButtonAfficherDifferences.UseVisualStyleBackColor = True
        '
        'XzCrLabelDifferences
        '
        Me.XzCrLabelDifferences.Location = New System.Drawing.Point(6, 21)
        Me.XzCrLabelDifferences.Name = "XzCrLabelDifferences"
        Me.XzCrLabelDifferences.Size = New System.Drawing.Size(473, 126)
        Me.XzCrLabelDifferences.TabIndex = 0
        '
        'XzCrGroupBoxDetail
        '
        Me.XzCrGroupBoxDetail.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.XzCrGroupBoxDetail.Controls.Add(Me.XzCrLabelDetails)
        Me.XzCrGroupBoxDetail.Location = New System.Drawing.Point(9, 7)
        Me.XzCrGroupBoxDetail.Name = "XzCrGroupBoxDetail"
        Me.XzCrGroupBoxDetail.Size = New System.Drawing.Size(554, 101)
        Me.XzCrGroupBoxDetail.TabIndex = 0
        Me.XzCrGroupBoxDetail.TabStop = False
        Me.XzCrGroupBoxDetail.Text = "Détails sur le fichier sélectionné"
        '
        'XzCrLabelDetails
        '
        Me.XzCrLabelDetails.AutoSize = True
        Me.XzCrLabelDetails.Dock = System.Windows.Forms.DockStyle.Fill
        Me.XzCrLabelDetails.Location = New System.Drawing.Point(3, 17)
        Me.XzCrLabelDetails.Name = "XzCrLabelDetails"
        Me.XzCrLabelDetails.Size = New System.Drawing.Size(0, 15)
        Me.XzCrLabelDetails.TabIndex = 0
        '
        'XzCrTreeView1
        '
        Me.XzCrTreeView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.XzCrTreeView1.Location = New System.Drawing.Point(21, 299)
        Me.XzCrTreeView1.Name = "XzCrTreeView1"
        Me.XzCrTreeView1.SelectedNode = Nothing
        Me.XzCrTreeView1.Size = New System.Drawing.Size(536, 451)
        Me.XzCrTreeView1.TabIndex = 3
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.XzCrLabel5)
        Me.SplitContainer1.Panel1.Controls.Add(Me.XzCrTxtAttributDeRecherche)
        Me.SplitContainer1.Panel1.Controls.Add(Me.XzCrLabel4)
        Me.SplitContainer1.Panel1.Controls.Add(Me.XzCrCboServeurActiveDirectory)
        Me.SplitContainer1.Panel1.Controls.Add(Me.XzCrLabel1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.XzCrTextboxRepertoireSauvegarde)
        Me.SplitContainer1.Panel1.Controls.Add(Me.XzCrListView1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.XzCrLabel2)
        Me.SplitContainer1.Panel1.Controls.Add(Me.XzCrButtonEnregistrer)
        Me.SplitContainer1.Panel1.Controls.Add(Me.XzCrComboBoxFiltreUniteAdministrative)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.XzCrGroupBoxDetail)
        Me.SplitContainer1.Panel2.Controls.Add(Me.XzCrTreeView1)
        Me.SplitContainer1.Panel2.Controls.Add(Me.XzCrGroupBoxDifferences)
        Me.SplitContainer1.Panel2.Controls.Add(Me.XzCrButtonComparer)
        Me.SplitContainer1.Size = New System.Drawing.Size(1143, 800)
        Me.SplitContainer1.SplitterDistance = 564
        Me.SplitContainer1.TabIndex = 0
        '
        'XzCrLabel5
        '
        Me.XzCrLabel5.AutoSize = True
        Me.XzCrLabel5.Location = New System.Drawing.Point(12, 98)
        Me.XzCrLabel5.Name = "XzCrLabel5"
        Me.XzCrLabel5.Size = New System.Drawing.Size(216, 15)
        Me.XzCrLabel5.TabIndex = 6
        Me.XzCrLabel5.Text = "Attribut contenant l'unité administrative"
        '
        'XzCrTxtAttributDeRecherche
        '
        Me.XzCrTxtAttributDeRecherche.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.XzCrTxtAttributDeRecherche.Location = New System.Drawing.Point(230, 92)
        Me.XzCrTxtAttributDeRecherche.Name = "XzCrTxtAttributDeRecherche"
        Me.XzCrTxtAttributDeRecherche.NomChampsDonnee = Nothing
        Me.XzCrTxtAttributDeRecherche.NomSourceDonnee = Nothing
        Me.XzCrTxtAttributDeRecherche.Size = New System.Drawing.Size(322, 21)
        Me.XzCrTxtAttributDeRecherche.TabIndex = 7
        Me.XzCrTxtAttributDeRecherche.Text = "Department"
        '
        'XzCrLabel4
        '
        Me.XzCrLabel4.AutoSize = True
        Me.XzCrLabel4.Location = New System.Drawing.Point(12, 71)
        Me.XzCrLabel4.Name = "XzCrLabel4"
        Me.XzCrLabel4.Size = New System.Drawing.Size(133, 15)
        Me.XzCrLabel4.TabIndex = 4
        Me.XzCrLabel4.Text = "Serveur Active Directory"
        '
        'XzCrCboServeurActiveDirectory
        '
        Me.XzCrCboServeurActiveDirectory.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.XzCrCboServeurActiveDirectory.FormattingEnabled = True
        Me.XzCrCboServeurActiveDirectory.GererTabCommeEnter = False
        Me.XzCrCboServeurActiveDirectory.Items.AddRange(New Object() {"int.rrq.qc", "rq.retraitequebec.gouv.qc.ca", "intra.carra.gouv.qc.ca"})
        Me.XzCrCboServeurActiveDirectory.Location = New System.Drawing.Point(230, 63)
        Me.XzCrCboServeurActiveDirectory.Name = "XzCrCboServeurActiveDirectory"
        Me.XzCrCboServeurActiveDirectory.NomChampsDonnee = Nothing
        Me.XzCrCboServeurActiveDirectory.NomSourceDonnee = Nothing
        Me.XzCrCboServeurActiveDirectory.Size = New System.Drawing.Size(322, 23)
        Me.XzCrCboServeurActiveDirectory.TabIndex = 5
        '
        'TsFdVerifierGroupeAD
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1143, 800)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "TsFdVerifierGroupeAD"
        Me.Text = "Enregistrement et comparaison AD pour une unité administrative"
        Me.XzCrGroupBoxDifferences.ResumeLayout(False)
        Me.XzCrGroupBoxDifferences.PerformLayout()
        Me.XzCrGroupBoxDetail.ResumeLayout(False)
        Me.XzCrGroupBoxDetail.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents XzCrListView1 As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrListView
    Friend WithEvents XzCrButtonEnregistrer As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton
    Friend WithEvents XzCrComboBoxFiltreUniteAdministrative As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XzCrComboBox
    Friend WithEvents XzCrLabel2 As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel
    Friend WithEvents XzCrTextboxRepertoireSauvegarde As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTextbox
    Friend WithEvents XzCrLabel1 As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel
    Friend WithEvents XzCrButtonComparer As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton
    Friend WithEvents XzCrGroupBoxDifferences As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrGroupBox
    Friend WithEvents XzCrGroupBoxDetail As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrGroupBox
    Friend WithEvents XzCrLabelDetails As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel
    Friend WithEvents XzCrLabelDifferences As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel
    Friend WithEvents XzCrButtonAfficherDifferences As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton
    Friend WithEvents XzCrTreeView1 As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTreeView
    Friend WithEvents XzCrLabel3 As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents XzCrLabel5 As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel
    Friend WithEvents XzCrTxtAttributDeRecherche As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTextbox
    Friend WithEvents XzCrLabel4 As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel
    Friend WithEvents XzCrCboServeurActiveDirectory As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XzCrComboBox
End Class
