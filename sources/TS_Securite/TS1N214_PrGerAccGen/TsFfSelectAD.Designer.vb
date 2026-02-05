<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class TsFfSelectAD
    Inherits System.Windows.Forms.Form

    'La méthode substituée Dispose du formulaire pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    Friend WithEvents txtProfil As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTextbox
    Friend WithEvents lstProfil As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrListbox
    Friend WithEvents cmdSelectionner As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton
    Friend WithEvents cmdAnnuler As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton
    Friend WithEvents cmdRafraichir As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton
    Friend WithEvents tlTip As System.Windows.Forms.ToolTip

    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TsFfSelectAD))
        Me.txtProfil = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTextbox()
        Me.lstProfil = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrListbox()
        Me.cmdSelectionner = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton()
        Me.cmdAnnuler = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton()
        Me.cmdRafraichir = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton()
        Me.tlTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.SuspendLayout()
        '
        'txtProfil
        '
        Me.txtProfil.Location = New System.Drawing.Point(6, 6)
        Me.txtProfil.Name = "txtProfil"
        Me.txtProfil.Size = New System.Drawing.Size(269, 22)
        Me.txtProfil.TabIndex = 0
        '
        'lstProfil
        '
        Me.lstProfil.ItemHeight = 16
        Me.lstProfil.Location = New System.Drawing.Point(6, 33)
        Me.lstProfil.Name = "lstProfil"
        Me.lstProfil.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstProfil.Size = New System.Drawing.Size(269, 292)
        Me.lstProfil.TabIndex = 1
        '
        'cmdSelectionner
        '
        Me.cmdSelectionner.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdSelectionner.Location = New System.Drawing.Point(72, 346)
        Me.cmdSelectionner.Name = "cmdSelectionner"
        Me.cmdSelectionner.Size = New System.Drawing.Size(102, 31)
        Me.cmdSelectionner.TabIndex = 3
        Me.cmdSelectionner.Text = "Sélectionner"
        '
        'cmdAnnuler
        '
        Me.cmdAnnuler.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdAnnuler.Location = New System.Drawing.Point(182, 346)
        Me.cmdAnnuler.Name = "cmdAnnuler"
        Me.cmdAnnuler.Size = New System.Drawing.Size(90, 31)
        Me.cmdAnnuler.TabIndex = 4
        Me.cmdAnnuler.Text = "Annuler"
        '
        'cmdRafraichir
        '
        Me.cmdRafraichir.Image = CType(resources.GetObject("cmdRafraichir.Image"), System.Drawing.Image)
        Me.cmdRafraichir.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.cmdRafraichir.Location = New System.Drawing.Point(6, 346)
        Me.cmdRafraichir.Name = "cmdRafraichir"
        Me.cmdRafraichir.Size = New System.Drawing.Size(32, 31)
        Me.cmdRafraichir.TabIndex = 2
        Me.tlTip.SetToolTip(Me.cmdRafraichir, "Rafraîchir")
        '
        'TsFfSelectAD
        '
        Me.AcceptButton = Me.cmdSelectionner
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.CancelButton = Me.cmdAnnuler
        Me.ClientSize = New System.Drawing.Size(283, 388)
        Me.Controls.Add(Me.cmdRafraichir)
        Me.Controls.Add(Me.cmdAnnuler)
        Me.Controls.Add(Me.cmdSelectionner)
        Me.Controls.Add(Me.lstProfil)
        Me.Controls.Add(Me.txtProfil)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "TsFfSelectAD"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Liste des profils"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
End Class
