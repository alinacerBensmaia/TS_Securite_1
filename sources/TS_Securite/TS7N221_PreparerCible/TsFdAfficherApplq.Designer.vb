<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TsFdAfficherApplq
    Inherits System.Windows.Forms.Form

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
        Me.components = New System.ComponentModel.Container
        Me.btnAnnuler = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton
        Me.viewChangement = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrListView
        Me.clnSystemCible = New System.Windows.Forms.ColumnHeader
        Me.clnOperation = New System.Windows.Forms.ColumnHeader
        Me.clnType = New System.Windows.Forms.ColumnHeader
        Me.clnOperande1 = New System.Windows.Forms.ColumnHeader
        Me.clnOperande2 = New System.Windows.Forms.ColumnHeader
        Me.clnErreur = New System.Windows.Forms.ColumnHeader
        Me.btnAppliqer = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton
        Me.ttpDetailErreur = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XzCrToolTip(Me.components)
        Me.txtErreur = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTextbox
        Me.lblNbAjour = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel
        Me.lblNbPasAjour = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel
        Me.lblTotal = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel
        Me.txtNbAjour = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTextbox
        Me.txtNbPasAjour = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTextbox
        Me.txtTotal = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTextbox
        Me.XzCrCheckBox1 = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrCheckBox
        Me.clnAjour = New System.Windows.Forms.ColumnHeader
        Me.SuspendLayout()
        '
        'btnAnnuler
        '
        Me.btnAnnuler.Font = New System.Drawing.Font("Arial", 9.0!)
        Me.btnAnnuler.Location = New System.Drawing.Point(713, 384)
        Me.btnAnnuler.Name = "btnAnnuler"
        Me.btnAnnuler.Size = New System.Drawing.Size(111, 30)
        Me.btnAnnuler.TabIndex = 2
        Me.btnAnnuler.Text = "Annuler"
        Me.btnAnnuler.UseVisualStyleBackColor = True
        '
        'viewChangement
        '
        Me.viewChangement.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.clnSystemCible, Me.clnOperation, Me.clnType, Me.clnOperande1, Me.clnOperande2, Me.clnAjour, Me.clnErreur})
        Me.viewChangement.Location = New System.Drawing.Point(12, 12)
        Me.viewChangement.Name = "viewChangement"
        Me.viewChangement.Size = New System.Drawing.Size(812, 346)
        Me.viewChangement.TabIndex = 3
        Me.viewChangement.UseCompatibleStateImageBehavior = False
        Me.viewChangement.View = System.Windows.Forms.View.Details
        '
        'clnSystemCible
        '
        Me.clnSystemCible.Text = "Système cible"
        Me.clnSystemCible.Width = 150
        '
        'clnOperation
        '
        Me.clnOperation.Text = "Opération"
        Me.clnOperation.Width = 65
        '
        'clnType
        '
        Me.clnType.Text = "Type de changement"
        Me.clnType.Width = 113
        '
        'clnOperande1
        '
        Me.clnOperande1.Text = "Opérande 1"
        Me.clnOperande1.Width = 200
        '
        'clnOperande2
        '
        Me.clnOperande2.Text = "Opérande 2"
        Me.clnOperande2.Width = 200
        '
        'clnErreur
        '
        Me.clnErreur.DisplayIndex = 5
        Me.clnErreur.Text = ""
        Me.clnErreur.Width = 40
        '
        'btnAppliqer
        '
        Me.btnAppliqer.Font = New System.Drawing.Font("Arial", 9.0!)
        Me.btnAppliqer.Location = New System.Drawing.Point(596, 386)
        Me.btnAppliqer.Name = "btnAppliqer"
        Me.btnAppliqer.Size = New System.Drawing.Size(111, 30)
        Me.btnAppliqer.TabIndex = 1
        Me.btnAppliqer.Text = "Appliquer"
        Me.btnAppliqer.UseVisualStyleBackColor = True
        '
        'ttpDetailErreur
        '
        Me.ttpDetailErreur.AutomaticDelay = 500
        Me.ttpDetailErreur.AutoPopDelay = 30000
        Me.ttpDetailErreur.InitialDelay = 500
        Me.ttpDetailErreur.ReshowDelay = 100
        '
        'txtErreur
        '
        Me.txtErreur.Location = New System.Drawing.Point(12, 358)
        Me.txtErreur.Name = "txtErreur"
        Me.txtErreur.NomChampsDonnee = Nothing
        Me.txtErreur.NomSourceDonnee = Nothing
        Me.txtErreur.Size = New System.Drawing.Size(812, 22)
        Me.txtErreur.TabIndex = 4
        Me.txtErreur.Verrouiller = True
        '
        'lblNbAjour
        '
        Me.lblNbAjour.AutoSize = True
        Me.lblNbAjour.Location = New System.Drawing.Point(12, 386)
        Me.lblNbAjour.Name = "lblNbAjour"
        Me.lblNbAjour.Size = New System.Drawing.Size(74, 17)
        Me.lblNbAjour.TabIndex = 5
        Me.lblNbAjour.Text = "Nb à jour :"
        '
        'lblNbPasAjour
        '
        Me.lblNbPasAjour.AutoSize = True
        Me.lblNbPasAjour.Location = New System.Drawing.Point(12, 408)
        Me.lblNbPasAjour.Name = "lblNbPasAjour"
        Me.lblNbPasAjour.Size = New System.Drawing.Size(101, 17)
        Me.lblNbPasAjour.TabIndex = 5
        Me.lblNbPasAjour.Text = "Nb pas à jour :"
        '
        'lblTotal
        '
        Me.lblTotal.AutoSize = True
        Me.lblTotal.Location = New System.Drawing.Point(177, 408)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(48, 17)
        Me.lblTotal.TabIndex = 5
        Me.lblTotal.Text = "Total :"
        '
        'txtNbAjour
        '
        Me.txtNbAjour.Location = New System.Drawing.Point(94, 384)
        Me.txtNbAjour.Name = "txtNbAjour"
        Me.txtNbAjour.NomChampsDonnee = Nothing
        Me.txtNbAjour.NomSourceDonnee = Nothing
        Me.txtNbAjour.Size = New System.Drawing.Size(61, 22)
        Me.txtNbAjour.TabIndex = 6
        Me.txtNbAjour.Verrouiller = True
        '
        'txtNbPasAjour
        '
        Me.txtNbPasAjour.Location = New System.Drawing.Point(94, 405)
        Me.txtNbPasAjour.Name = "txtNbPasAjour"
        Me.txtNbPasAjour.NomChampsDonnee = Nothing
        Me.txtNbPasAjour.NomSourceDonnee = Nothing
        Me.txtNbPasAjour.Size = New System.Drawing.Size(61, 22)
        Me.txtNbPasAjour.TabIndex = 6
        Me.txtNbPasAjour.Verrouiller = True
        '
        'txtTotal
        '
        Me.txtTotal.Location = New System.Drawing.Point(220, 405)
        Me.txtTotal.Name = "txtTotal"
        Me.txtTotal.NomChampsDonnee = Nothing
        Me.txtTotal.NomSourceDonnee = Nothing
        Me.txtTotal.Size = New System.Drawing.Size(61, 22)
        Me.txtTotal.TabIndex = 6
        Me.txtTotal.Verrouiller = True
        '
        'XzCrCheckBox1
        '
        Me.XzCrCheckBox1._EtatCaseCoche = CType(0, Short)
        Me.XzCrCheckBox1.AutoSize = True
        Me.XzCrCheckBox1.EstEnEvidence = False
        Me.XzCrCheckBox1.Location = New System.Drawing.Point(333, 391)
        Me.XzCrCheckBox1.Name = "XzCrCheckBox1"
        Me.XzCrCheckBox1.NomChampsDonnee = Nothing
        Me.XzCrCheckBox1.NomSourceDonnee = Nothing
        Me.XzCrCheckBox1.Size = New System.Drawing.Size(267, 21)
        Me.XzCrCheckBox1.TabIndex = 7
        Me.XzCrCheckBox1.Text = "Afficher seulement les éléments à jour"
        Me.XzCrCheckBox1.UseVisualStyleBackColor = True
        Me.XzCrCheckBox1.ValeurIndetermine = Nothing
        Me.XzCrCheckBox1.ValeurNonSelectionne = Nothing
        Me.XzCrCheckBox1.ValeurSelectionne = Nothing
        '
        'clnAjour
        '
        Me.clnAjour.Text = "A jour ?"
        Me.clnAjour.Width = 65
        '
        'TsFdAfficherApplq
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(836, 430)
        Me.Controls.Add(Me.XzCrCheckBox1)
        Me.Controls.Add(Me.txtTotal)
        Me.Controls.Add(Me.txtNbPasAjour)
        Me.Controls.Add(Me.txtNbAjour)
        Me.Controls.Add(Me.lblTotal)
        Me.Controls.Add(Me.lblNbPasAjour)
        Me.Controls.Add(Me.lblNbAjour)
        Me.Controls.Add(Me.txtErreur)
        Me.Controls.Add(Me.viewChangement)
        Me.Controls.Add(Me.btnAppliqer)
        Me.Controls.Add(Me.btnAnnuler)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "TsFdAfficherApplq"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Changements"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnAnnuler As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton
    Friend WithEvents viewChangement As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrListView
    Friend WithEvents clnOperation As System.Windows.Forms.ColumnHeader
    Friend WithEvents clnType As System.Windows.Forms.ColumnHeader
    Friend WithEvents clnOperande1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents clnOperande2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnAppliqer As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton
    Friend WithEvents clnSystemCible As System.Windows.Forms.ColumnHeader
    Friend WithEvents clnErreur As System.Windows.Forms.ColumnHeader
    Friend WithEvents ttpDetailErreur As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XzCrToolTip
    Friend WithEvents txtErreur As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTextbox
    Friend WithEvents lblNbAjour As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel
    Friend WithEvents lblNbPasAjour As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel
    Friend WithEvents lblTotal As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel
    Friend WithEvents txtNbAjour As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTextbox
    Friend WithEvents txtNbPasAjour As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTextbox
    Friend WithEvents txtTotal As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTextbox
    Friend WithEvents XzCrCheckBox1 As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrCheckBox
    Friend WithEvents clnAjour As System.Windows.Forms.ColumnHeader
End Class
