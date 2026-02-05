<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
        Me.btnAction1 = New System.Windows.Forms.Button
        Me.blstUsager = New System.Windows.Forms.ListBox
        Me.blstRole = New System.Windows.Forms.ListBox
        Me.blstRessource = New System.Windows.Forms.ListBox
        Me.btnAction2 = New System.Windows.Forms.Button
        Me.btnAction3 = New System.Windows.Forms.Button
        Me.btnXmlReader1 = New System.Windows.Forms.Button
        Me.btnXMLReader2 = New System.Windows.Forms.Button
        Me.txtXML1 = New System.Windows.Forms.TextBox
        Me.btnXLMReader3 = New System.Windows.Forms.Button
        Me.btnRelationURo = New System.Windows.Forms.Button
        Me.btnRelationRoRe = New System.Windows.Forms.Button
        Me.btnRelationURe = New System.Windows.Forms.Button
        Me.btnRelationRoRo = New System.Windows.Forms.Button
        Me.btnGenerique = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'btnAction1
        '
        Me.btnAction1.Location = New System.Drawing.Point(12, 377)
        Me.btnAction1.Name = "btnAction1"
        Me.btnAction1.Size = New System.Drawing.Size(250, 23)
        Me.btnAction1.TabIndex = 3
        Me.btnAction1.Text = "Lire Usager"
        Me.btnAction1.UseVisualStyleBackColor = True
        '
        'blstUsager
        '
        Me.blstUsager.FormattingEnabled = True
        Me.blstUsager.ItemHeight = 16
        Me.blstUsager.Location = New System.Drawing.Point(12, 400)
        Me.blstUsager.Name = "blstUsager"
        Me.blstUsager.Size = New System.Drawing.Size(250, 340)
        Me.blstUsager.TabIndex = 4
        '
        'blstRole
        '
        Me.blstRole.FormattingEnabled = True
        Me.blstRole.ItemHeight = 16
        Me.blstRole.Location = New System.Drawing.Point(292, 400)
        Me.blstRole.Name = "blstRole"
        Me.blstRole.Size = New System.Drawing.Size(250, 340)
        Me.blstRole.TabIndex = 4
        '
        'blstRessource
        '
        Me.blstRessource.FormattingEnabled = True
        Me.blstRessource.ItemHeight = 16
        Me.blstRessource.Location = New System.Drawing.Point(568, 400)
        Me.blstRessource.Name = "blstRessource"
        Me.blstRessource.Size = New System.Drawing.Size(250, 340)
        Me.blstRessource.TabIndex = 4
        '
        'btnAction2
        '
        Me.btnAction2.Location = New System.Drawing.Point(292, 377)
        Me.btnAction2.Name = "btnAction2"
        Me.btnAction2.Size = New System.Drawing.Size(250, 23)
        Me.btnAction2.TabIndex = 3
        Me.btnAction2.Text = "Lire Role"
        Me.btnAction2.UseVisualStyleBackColor = True
        '
        'btnAction3
        '
        Me.btnAction3.Location = New System.Drawing.Point(568, 377)
        Me.btnAction3.Name = "btnAction3"
        Me.btnAction3.Size = New System.Drawing.Size(250, 23)
        Me.btnAction3.TabIndex = 3
        Me.btnAction3.Text = "Lire Ressource"
        Me.btnAction3.UseVisualStyleBackColor = True
        '
        'btnXmlReader1
        '
        Me.btnXmlReader1.Location = New System.Drawing.Point(655, 318)
        Me.btnXmlReader1.Name = "btnXmlReader1"
        Me.btnXmlReader1.Size = New System.Drawing.Size(163, 23)
        Me.btnXmlReader1.TabIndex = 5
        Me.btnXmlReader1.Text = "Lire XML Ressource"
        Me.btnXmlReader1.UseVisualStyleBackColor = True
        '
        'btnXMLReader2
        '
        Me.btnXMLReader2.Location = New System.Drawing.Point(333, 307)
        Me.btnXMLReader2.Name = "btnXMLReader2"
        Me.btnXMLReader2.Size = New System.Drawing.Size(170, 23)
        Me.btnXMLReader2.TabIndex = 6
        Me.btnXMLReader2.Text = "Lire XML Role"
        Me.btnXMLReader2.UseVisualStyleBackColor = True
        '
        'txtXML1
        '
        Me.txtXML1.Location = New System.Drawing.Point(12, 12)
        Me.txtXML1.Multiline = True
        Me.txtXML1.Name = "txtXML1"
        Me.txtXML1.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtXML1.Size = New System.Drawing.Size(806, 295)
        Me.txtXML1.TabIndex = 7
        '
        'btnXLMReader3
        '
        Me.btnXLMReader3.Location = New System.Drawing.Point(12, 318)
        Me.btnXLMReader3.Name = "btnXLMReader3"
        Me.btnXLMReader3.Size = New System.Drawing.Size(170, 23)
        Me.btnXLMReader3.TabIndex = 6
        Me.btnXLMReader3.Text = "Lire XML Usager"
        Me.btnXLMReader3.UseVisualStyleBackColor = True
        '
        'btnRelationURo
        '
        Me.btnRelationURo.Location = New System.Drawing.Point(182, 307)
        Me.btnRelationURo.Name = "btnRelationURo"
        Me.btnRelationURo.Size = New System.Drawing.Size(150, 23)
        Me.btnRelationURo.TabIndex = 8
        Me.btnRelationURo.Text = "XML Relation URo"
        Me.btnRelationURo.UseVisualStyleBackColor = True
        '
        'btnRelationRoRe
        '
        Me.btnRelationRoRe.Location = New System.Drawing.Point(504, 307)
        Me.btnRelationRoRe.Name = "btnRelationRoRe"
        Me.btnRelationRoRe.Size = New System.Drawing.Size(150, 23)
        Me.btnRelationRoRe.TabIndex = 8
        Me.btnRelationRoRe.Text = "XML Relation RoRe"
        Me.btnRelationRoRe.UseVisualStyleBackColor = True
        '
        'btnRelationURe
        '
        Me.btnRelationURe.Location = New System.Drawing.Point(182, 331)
        Me.btnRelationURe.Name = "btnRelationURe"
        Me.btnRelationURe.Size = New System.Drawing.Size(150, 23)
        Me.btnRelationURe.TabIndex = 9
        Me.btnRelationURe.Text = "XML Relation URe"
        Me.btnRelationURe.UseVisualStyleBackColor = True
        '
        'btnRelationRoRo
        '
        Me.btnRelationRoRo.Location = New System.Drawing.Point(345, 330)
        Me.btnRelationRoRo.Name = "btnRelationRoRo"
        Me.btnRelationRoRo.Size = New System.Drawing.Size(149, 23)
        Me.btnRelationRoRo.TabIndex = 11
        Me.btnRelationRoRo.Text = "XML Relation RoRo"
        Me.btnRelationRoRo.UseVisualStyleBackColor = True
        '
        'btnGenerique
        '
        Me.btnGenerique.Location = New System.Drawing.Point(504, 331)
        Me.btnGenerique.Name = "btnGenerique"
        Me.btnGenerique.Size = New System.Drawing.Size(150, 23)
        Me.btnGenerique.TabIndex = 12
        Me.btnGenerique.Text = "Teste Class"
        Me.btnGenerique.UseVisualStyleBackColor = True
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(822, 742)
        Me.Controls.Add(Me.btnGenerique)
        Me.Controls.Add(Me.btnRelationRoRo)
        Me.Controls.Add(Me.btnRelationURe)
        Me.Controls.Add(Me.btnRelationRoRe)
        Me.Controls.Add(Me.btnRelationURo)
        Me.Controls.Add(Me.txtXML1)
        Me.Controls.Add(Me.btnXLMReader3)
        Me.Controls.Add(Me.btnXMLReader2)
        Me.Controls.Add(Me.btnXmlReader1)
        Me.Controls.Add(Me.blstRessource)
        Me.Controls.Add(Me.blstRole)
        Me.Controls.Add(Me.blstUsager)
        Me.Controls.Add(Me.btnAction3)
        Me.Controls.Add(Me.btnAction2)
        Me.Controls.Add(Me.btnAction1)
        Me.Name = "frmMain"
        Me.Text = "Moniteur"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnAction1 As System.Windows.Forms.Button
    Friend WithEvents blstUsager As System.Windows.Forms.ListBox
    Friend WithEvents blstRole As System.Windows.Forms.ListBox
    Friend WithEvents blstRessource As System.Windows.Forms.ListBox
    Friend WithEvents btnAction2 As System.Windows.Forms.Button
    Friend WithEvents btnAction3 As System.Windows.Forms.Button
    Friend WithEvents btnXmlReader1 As System.Windows.Forms.Button
    Friend WithEvents btnXMLReader2 As System.Windows.Forms.Button
    Friend WithEvents txtXML1 As System.Windows.Forms.TextBox
    Friend WithEvents btnXLMReader3 As System.Windows.Forms.Button
    Friend WithEvents btnRelationURo As System.Windows.Forms.Button
    Friend WithEvents btnRelationRoRe As System.Windows.Forms.Button
    Friend WithEvents btnRelationURe As System.Windows.Forms.Button
    Friend WithEvents btnRelationRoRo As System.Windows.Forms.Button
    Friend WithEvents btnGenerique As System.Windows.Forms.Button

End Class
