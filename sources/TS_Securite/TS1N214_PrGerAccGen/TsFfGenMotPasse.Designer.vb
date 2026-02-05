<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TsFfGenMotPasse
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
        Me.Label1 = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel()
        Me.chkIncMinuscules = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrCheckBox()
        Me.chkIncChiffres = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrCheckBox()
        Me.chkIncMajuscules = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrCheckBox()
        Me.chkIncSymboles = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrCheckBox()
        Me.btnGenerer = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton()
        Me.numLongueur = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrNumericUpDown()
        CType(Me.numLongueur, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 18)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(139, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Longueur du mot de passe :"
        '
        'chkIncMinuscules
        '
        Me.chkIncMinuscules._EtatCaseCoche = CType(1, Short)
        Me.chkIncMinuscules.AutoSize = True
        Me.chkIncMinuscules.Checked = True
        Me.chkIncMinuscules.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkIncMinuscules.EstEnEvidence = False
        Me.chkIncMinuscules.Location = New System.Drawing.Point(10, 46)
        Me.chkIncMinuscules.Margin = New System.Windows.Forms.Padding(2)
        Me.chkIncMinuscules.Name = "chkIncMinuscules"
        Me.chkIncMinuscules.NomChampsDonnee = Nothing
        Me.chkIncMinuscules.NomSourceDonnee = Nothing
        Me.chkIncMinuscules.Size = New System.Drawing.Size(129, 17)
        Me.chkIncMinuscules.TabIndex = 2
        Me.chkIncMinuscules.Text = "Inclure les minuscules"
        Me.chkIncMinuscules.UseVisualStyleBackColor = True
        Me.chkIncMinuscules.ValeurIndetermine = Nothing
        Me.chkIncMinuscules.ValeurNonSelectionne = Nothing
        Me.chkIncMinuscules.ValeurSelectionne = Nothing
        '
        'chkIncChiffres
        '
        Me.chkIncChiffres._EtatCaseCoche = CType(1, Short)
        Me.chkIncChiffres.AutoSize = True
        Me.chkIncChiffres.Checked = True
        Me.chkIncChiffres.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkIncChiffres.EstEnEvidence = False
        Me.chkIncChiffres.Location = New System.Drawing.Point(140, 46)
        Me.chkIncChiffres.Margin = New System.Windows.Forms.Padding(2)
        Me.chkIncChiffres.Name = "chkIncChiffres"
        Me.chkIncChiffres.NomChampsDonnee = Nothing
        Me.chkIncChiffres.NomSourceDonnee = Nothing
        Me.chkIncChiffres.Size = New System.Drawing.Size(111, 17)
        Me.chkIncChiffres.TabIndex = 3
        Me.chkIncChiffres.Text = "Inclure les chiffres"
        Me.chkIncChiffres.UseVisualStyleBackColor = True
        Me.chkIncChiffres.ValeurIndetermine = Nothing
        Me.chkIncChiffres.ValeurNonSelectionne = Nothing
        Me.chkIncChiffres.ValeurSelectionne = Nothing
        '
        'chkIncMajuscules
        '
        Me.chkIncMajuscules._EtatCaseCoche = CType(0, Short)
        Me.chkIncMajuscules.AutoSize = True
        Me.chkIncMajuscules.EstEnEvidence = False
        Me.chkIncMajuscules.Location = New System.Drawing.Point(10, 69)
        Me.chkIncMajuscules.Margin = New System.Windows.Forms.Padding(2)
        Me.chkIncMajuscules.Name = "chkIncMajuscules"
        Me.chkIncMajuscules.NomChampsDonnee = Nothing
        Me.chkIncMajuscules.NomSourceDonnee = Nothing
        Me.chkIncMajuscules.Size = New System.Drawing.Size(129, 17)
        Me.chkIncMajuscules.TabIndex = 4
        Me.chkIncMajuscules.Text = "Inclure les majuscules"
        Me.chkIncMajuscules.UseVisualStyleBackColor = True
        Me.chkIncMajuscules.ValeurIndetermine = Nothing
        Me.chkIncMajuscules.ValeurNonSelectionne = Nothing
        Me.chkIncMajuscules.ValeurSelectionne = Nothing
        '
        'chkIncSymboles
        '
        Me.chkIncSymboles._EtatCaseCoche = CType(0, Short)
        Me.chkIncSymboles.AutoSize = True
        Me.chkIncSymboles.EstEnEvidence = False
        Me.chkIncSymboles.Location = New System.Drawing.Point(140, 69)
        Me.chkIncSymboles.Margin = New System.Windows.Forms.Padding(2)
        Me.chkIncSymboles.Name = "chkIncSymboles"
        Me.chkIncSymboles.NomChampsDonnee = Nothing
        Me.chkIncSymboles.NomSourceDonnee = Nothing
        Me.chkIncSymboles.Size = New System.Drawing.Size(120, 17)
        Me.chkIncSymboles.TabIndex = 5
        Me.chkIncSymboles.Text = "Inclure les symboles"
        Me.chkIncSymboles.UseVisualStyleBackColor = True
        Me.chkIncSymboles.ValeurIndetermine = Nothing
        Me.chkIncSymboles.ValeurNonSelectionne = Nothing
        Me.chkIncSymboles.ValeurSelectionne = Nothing
        '
        'btnGenerer
        '
        Me.btnGenerer.Location = New System.Drawing.Point(133, 97)
        Me.btnGenerer.Margin = New System.Windows.Forms.Padding(2)
        Me.btnGenerer.Name = "btnGenerer"
        Me.btnGenerer.Size = New System.Drawing.Size(125, 24)
        Me.btnGenerer.TabIndex = 6
        Me.btnGenerer.Text = "Générer mot de passe"
        Me.btnGenerer.UseVisualStyleBackColor = True
        '
        'numLongueur
        '
        Me.numLongueur.Location = New System.Drawing.Point(167, 18)
        Me.numLongueur.Margin = New System.Windows.Forms.Padding(2)
        Me.numLongueur.Maximum = New Decimal(New Integer() {15, 0, 0, 0})
        Me.numLongueur.Minimum = New Decimal(New Integer() {8, 0, 0, 0})
        Me.numLongueur.Name = "numLongueur"
        Me.numLongueur.NomChampsDonnee = Nothing
        Me.numLongueur.NomSourceDonnee = Nothing
        Me.numLongueur.Size = New System.Drawing.Size(42, 20)
        Me.numLongueur.TabIndex = 7
        Me.numLongueur.Value = New Decimal(New Integer() {8, 0, 0, 0})
        '
        'TsFfGenMotPasse
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(267, 129)
        Me.Controls.Add(Me.numLongueur)
        Me.Controls.Add(Me.btnGenerer)
        Me.Controls.Add(Me.chkIncSymboles)
        Me.Controls.Add(Me.chkIncMajuscules)
        Me.Controls.Add(Me.chkIncChiffres)
        Me.Controls.Add(Me.chkIncMinuscules)
        Me.Controls.Add(Me.Label1)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "TsFfGenMotPasse"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "Générateur de mot de passe"
        CType(Me.numLongueur, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel
    Friend WithEvents chkIncMinuscules As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrCheckBox
    Friend WithEvents chkIncChiffres As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrCheckBox
    Friend WithEvents chkIncMajuscules As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrCheckBox
    Friend WithEvents chkIncSymboles As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrCheckBox
    Friend WithEvents btnGenerer As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton
    Friend WithEvents numLongueur As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrNumericUpDown
End Class
