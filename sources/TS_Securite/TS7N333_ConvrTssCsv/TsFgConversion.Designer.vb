<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TsFgConversion
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
        Me.cmdConvertir = New System.Windows.Forms.Button
        Me.cmdAppliquer = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'cmdConvertir
        '
        Me.cmdConvertir.Location = New System.Drawing.Point(12, 12)
        Me.cmdConvertir.Name = "cmdConvertir"
        Me.cmdConvertir.Size = New System.Drawing.Size(269, 23)
        Me.cmdConvertir.TabIndex = 0
        Me.cmdConvertir.Text = "Convertir une extraction TSS en CSV"
        Me.cmdConvertir.UseVisualStyleBackColor = True
        '
        'cmdAppliquer
        '
        Me.cmdAppliquer.Location = New System.Drawing.Point(12, 41)
        Me.cmdAppliquer.Name = "cmdAppliquer"
        Me.cmdAppliquer.Size = New System.Drawing.Size(269, 23)
        Me.cmdAppliquer.TabIndex = 0
        Me.cmdAppliquer.Text = "Appliquer un fichier de commande TSS sur du CSV"
        Me.cmdAppliquer.UseVisualStyleBackColor = True
        '
        'TsFgConversion
        '
        Me.AcceptButton = Me.cmdConvertir
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(291, 78)
        Me.Controls.Add(Me.cmdAppliquer)
        Me.Controls.Add(Me.cmdConvertir)
        Me.Name = "TsFgConversion"
        Me.Text = "Conversion TSS-CSV"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cmdConvertir As System.Windows.Forms.Button
    Friend WithEvents cmdAppliquer As System.Windows.Forms.Button

End Class
