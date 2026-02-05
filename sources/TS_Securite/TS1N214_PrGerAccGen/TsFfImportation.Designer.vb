<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TsFfImportation
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
        Me.btnAnnuler = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton()
        Me.lblImportation = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel()
        Me.chkAD = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrCheckBox()
        Me.chkADLDS = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrCheckBox()
        Me.SuspendLayout()
        '
        'btnAnnuler
        '
        Me.btnAnnuler.Location = New System.Drawing.Point(98, 140)
        Me.btnAnnuler.Margin = New System.Windows.Forms.Padding(2)
        Me.btnAnnuler.Name = "btnAnnuler"
        Me.btnAnnuler.Size = New System.Drawing.Size(82, 24)
        Me.btnAnnuler.TabIndex = 0
        Me.btnAnnuler.Text = "Annuler"
        Me.btnAnnuler.UseVisualStyleBackColor = True
        '
        'lblImportation
        '
        Me.lblImportation.AutoSize = True
        Me.lblImportation.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblImportation.Location = New System.Drawing.Point(65, 20)
        Me.lblImportation.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblImportation.Name = "lblImportation"
        Me.lblImportation.Size = New System.Drawing.Size(177, 17)
        Me.lblImportation.TabIndex = 1
        Me.lblImportation.Text = "Importation en cours ..."
        '
        'chkAD
        '
        Me.chkAD._EtatCaseCoche = CType(0, Short)
        Me.chkAD.AutoSize = True
        Me.chkAD.EstEnEvidence = False
        Me.chkAD.Location = New System.Drawing.Point(80, 49)
        Me.chkAD.Margin = New System.Windows.Forms.Padding(2)
        Me.chkAD.Name = "chkAD"
        Me.chkAD.NomChampsDonnee = Nothing
        Me.chkAD.NomSourceDonnee = Nothing
        Me.chkAD.Size = New System.Drawing.Size(128, 17)
        Me.chkAD.TabIndex = 2
        Me.chkAD.Text = "Créer les comptes AD"
        Me.chkAD.UseVisualStyleBackColor = True
        Me.chkAD.ValeurIndetermine = Nothing
        Me.chkAD.ValeurNonSelectionne = Nothing
        Me.chkAD.ValeurSelectionne = Nothing
        
        '
        'chkADLDS
        '
        Me.chkADLDS._EtatCaseCoche = CType(0, Short)
        Me.chkADLDS.AutoSize = True
        Me.chkADLDS.EstEnEvidence = False
        Me.chkADLDS.Location = New System.Drawing.Point(80, 105)
        Me.chkADLDS.Margin = New System.Windows.Forms.Padding(2)
        Me.chkADLDS.Name = "chkADLDS"
        Me.chkADLDS.NomChampsDonnee = Nothing
        Me.chkADLDS.NomSourceDonnee = Nothing
        Me.chkADLDS.Size = New System.Drawing.Size(154, 17)
        Me.chkADLDS.TabIndex = 4
        Me.chkADLDS.Text = "Créer les comptes AD/LDS"
        Me.chkADLDS.UseVisualStyleBackColor = True
        Me.chkADLDS.ValeurIndetermine = Nothing
        Me.chkADLDS.ValeurNonSelectionne = Nothing
        Me.chkADLDS.ValeurSelectionne = Nothing
        '
        'TsFfImportation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(277, 185)
        Me.Controls.Add(Me.chkADLDS)
        Me.Controls.Add(Me.chkAD)
        Me.Controls.Add(Me.lblImportation)
        Me.Controls.Add(Me.btnAnnuler)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "TsFfImportation"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Importation"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnAnnuler As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton
    Friend WithEvents lblImportation As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel
    Friend WithEvents chkAD As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrCheckBox
    Friend WithEvents chkADLDS As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrCheckBox
End Class
