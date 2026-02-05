<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TsFdTestErreurGerer
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
        Me.btnUtilisateurManquant = New System.Windows.Forms.Button
        Me.btnDestrcutionSansUser = New System.Windows.Forms.Button
        Me.btnModUserInexistant = New System.Windows.Forms.Button
        Me.btnAjoutRoleInexistant = New System.Windows.Forms.Button
        Me.btnMauvaiseModif = New System.Windows.Forms.Button
        Me.btnLectureSeule = New System.Windows.Forms.Button
        Me.btnTropFichiers = New System.Windows.Forms.Button
        Me.btnErreurBD = New System.Windows.Forms.Button
        Me.btnPrecedant = New System.Windows.Forms.Button
        Me.txtResultat = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'btnUtilisateurManquant
        '
        Me.btnUtilisateurManquant.Location = New System.Drawing.Point(12, 251)
        Me.btnUtilisateurManquant.Name = "btnUtilisateurManquant"
        Me.btnUtilisateurManquant.Size = New System.Drawing.Size(150, 23)
        Me.btnUtilisateurManquant.TabIndex = 0
        Me.btnUtilisateurManquant.Text = "Création sans utilisateur"
        Me.btnUtilisateurManquant.UseVisualStyleBackColor = True
        '
        'btnDestrcutionSansUser
        '
        Me.btnDestrcutionSansUser.Location = New System.Drawing.Point(12, 280)
        Me.btnDestrcutionSansUser.Name = "btnDestrcutionSansUser"
        Me.btnDestrcutionSansUser.Size = New System.Drawing.Size(150, 23)
        Me.btnDestrcutionSansUser.TabIndex = 0
        Me.btnDestrcutionSansUser.Text = "Destruction user inexistant"
        Me.btnDestrcutionSansUser.UseVisualStyleBackColor = True
        '
        'btnModUserInexistant
        '
        Me.btnModUserInexistant.Location = New System.Drawing.Point(12, 309)
        Me.btnModUserInexistant.Name = "btnModUserInexistant"
        Me.btnModUserInexistant.Size = New System.Drawing.Size(150, 23)
        Me.btnModUserInexistant.TabIndex = 0
        Me.btnModUserInexistant.Text = "Modification user inexistant"
        Me.btnModUserInexistant.UseVisualStyleBackColor = True
        '
        'btnAjoutRoleInexistant
        '
        Me.btnAjoutRoleInexistant.Location = New System.Drawing.Point(12, 338)
        Me.btnAjoutRoleInexistant.Name = "btnAjoutRoleInexistant"
        Me.btnAjoutRoleInexistant.Size = New System.Drawing.Size(150, 23)
        Me.btnAjoutRoleInexistant.TabIndex = 0
        Me.btnAjoutRoleInexistant.Text = "Ajout d'un rôle inexistant"
        Me.btnAjoutRoleInexistant.UseVisualStyleBackColor = True
        '
        'btnMauvaiseModif
        '
        Me.btnMauvaiseModif.Location = New System.Drawing.Point(168, 251)
        Me.btnMauvaiseModif.Name = "btnMauvaiseModif"
        Me.btnMauvaiseModif.Size = New System.Drawing.Size(150, 23)
        Me.btnMauvaiseModif.TabIndex = 0
        Me.btnMauvaiseModif.Text = "Mauvaise modification"
        Me.btnMauvaiseModif.UseVisualStyleBackColor = True
        '
        'btnLectureSeule
        '
        Me.btnLectureSeule.Location = New System.Drawing.Point(168, 280)
        Me.btnLectureSeule.Name = "btnLectureSeule"
        Me.btnLectureSeule.Size = New System.Drawing.Size(150, 23)
        Me.btnLectureSeule.TabIndex = 0
        Me.btnLectureSeule.Text = "Modifier en lecture seule"
        Me.btnLectureSeule.UseVisualStyleBackColor = True
        '
        'btnTropFichiers
        '
        Me.btnTropFichiers.Location = New System.Drawing.Point(168, 309)
        Me.btnTropFichiers.Name = "btnTropFichiers"
        Me.btnTropFichiers.Size = New System.Drawing.Size(150, 23)
        Me.btnTropFichiers.TabIndex = 0
        Me.btnTropFichiers.Text = "Trop de fichiers heat"
        Me.btnTropFichiers.UseVisualStyleBackColor = True
        '
        'btnErreurBD
        '
        Me.btnErreurBD.Location = New System.Drawing.Point(168, 338)
        Me.btnErreurBD.Name = "btnErreurBD"
        Me.btnErreurBD.Size = New System.Drawing.Size(150, 23)
        Me.btnErreurBD.TabIndex = 0
        Me.btnErreurBD.Text = "Erreur dans la BD Heat"
        Me.btnErreurBD.UseVisualStyleBackColor = True
        '
        'btnPrecedant
        '
        Me.btnPrecedant.Font = New System.Drawing.Font("Arial", 9.0!)
        Me.btnPrecedant.Location = New System.Drawing.Point(12, 195)
        Me.btnPrecedant.Name = "btnPrecedant"
        Me.btnPrecedant.Size = New System.Drawing.Size(111, 30)
        Me.btnPrecedant.TabIndex = 18
        Me.btnPrecedant.Text = "< Précédant"
        Me.btnPrecedant.UseVisualStyleBackColor = True
        '
        'txtResultat
        '
        Me.txtResultat.Location = New System.Drawing.Point(12, 24)
        Me.txtResultat.Multiline = True
        Me.txtResultat.Name = "txtResultat"
        Me.txtResultat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtResultat.Size = New System.Drawing.Size(518, 165)
        Me.txtResultat.TabIndex = 20
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 8)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(46, 13)
        Me.Label2.TabIndex = 19
        Me.Label2.Text = "Résultat"
        '
        'TsFdTestErreurGerer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(542, 373)
        Me.Controls.Add(Me.txtResultat)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnPrecedant)
        Me.Controls.Add(Me.btnErreurBD)
        Me.Controls.Add(Me.btnTropFichiers)
        Me.Controls.Add(Me.btnAjoutRoleInexistant)
        Me.Controls.Add(Me.btnLectureSeule)
        Me.Controls.Add(Me.btnModUserInexistant)
        Me.Controls.Add(Me.btnMauvaiseModif)
        Me.Controls.Add(Me.btnDestrcutionSansUser)
        Me.Controls.Add(Me.btnUtilisateurManquant)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "TsFdTestErreurGerer"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "TsFdTestErreurGerer"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnUtilisateurManquant As System.Windows.Forms.Button
    Friend WithEvents btnDestrcutionSansUser As System.Windows.Forms.Button
    Friend WithEvents btnModUserInexistant As System.Windows.Forms.Button
    Friend WithEvents btnAjoutRoleInexistant As System.Windows.Forms.Button
    Friend WithEvents btnMauvaiseModif As System.Windows.Forms.Button
    Friend WithEvents btnLectureSeule As System.Windows.Forms.Button
    Friend WithEvents btnTropFichiers As System.Windows.Forms.Button
    Friend WithEvents btnErreurBD As System.Windows.Forms.Button
    Friend WithEvents btnPrecedant As System.Windows.Forms.Button
    Friend WithEvents txtResultat As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
End Class
