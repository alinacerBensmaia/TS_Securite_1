<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TsFdDifferences
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
        Me.XzCrTreeView1 = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTreeView
        Me.SuspendLayout()
        '
        'XzCrTreeView1
        '
        Me.XzCrTreeView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.XzCrTreeView1.Location = New System.Drawing.Point(0, 0)
        Me.XzCrTreeView1.Name = "XzCrTreeView1"
        Me.XzCrTreeView1.SelectedNode = Nothing
        Me.XzCrTreeView1.Size = New System.Drawing.Size(807, 620)
        Me.XzCrTreeView1.TabIndex = 0
        '
        'TsFdDifferences
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(807, 620)
        Me.Controls.Add(Me.XzCrTreeView1)
        Me.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "TsFdDifferences"
        Me.Text = "Liste des différences"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents XzCrTreeView1 As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTreeView
End Class
