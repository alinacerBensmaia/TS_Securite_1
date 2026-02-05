<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TsFdFonctionsIndependantes
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
        Me.btnLienUserRole = New System.Windows.Forms.Button
        Me.btnEquipeAdmin = New System.Windows.Forms.Button
        Me.btnEquipesUtilsateur = New System.Windows.Forms.Button
        Me.btnListeUA = New System.Windows.Forms.Button
        Me.btnRoleEquipe = New System.Windows.Forms.Button
        Me.btnRolesUA = New System.Windows.Forms.Button
        Me.btnUniteAdministratives = New System.Windows.Forms.Button
        Me.btnUser = New System.Windows.Forms.Button
        Me.btnRoles = New System.Windows.Forms.Button
        Me.btnUsers = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtParam1 = New System.Windows.Forms.TextBox
        Me.txtResultat = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.tltIndication = New System.Windows.Forms.ToolTip(Me.components)
        Me.Button1 = New System.Windows.Forms.Button
        Me.btnRafraichirBuffer = New System.Windows.Forms.Button
        Me.btnDemandes = New System.Windows.Forms.Button
        Me.btnErreurs = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'btnLienUserRole
        '
        Me.btnLienUserRole.Location = New System.Drawing.Point(16, 309)
        Me.btnLienUserRole.Margin = New System.Windows.Forms.Padding(4)
        Me.btnLienUserRole.Name = "btnLienUserRole"
        Me.btnLienUserRole.Size = New System.Drawing.Size(200, 28)
        Me.btnLienUserRole.TabIndex = 0
        Me.btnLienUserRole.Text = "ObtenirAssignationsRole()"
        Me.tltIndication.SetToolTip(Me.btnLienUserRole, "Paramètre 1:  L'identifiant de l'utilisateur")
        Me.btnLienUserRole.UseVisualStyleBackColor = True
        '
        'btnEquipeAdmin
        '
        Me.btnEquipeAdmin.Location = New System.Drawing.Point(16, 345)
        Me.btnEquipeAdmin.Margin = New System.Windows.Forms.Padding(4)
        Me.btnEquipeAdmin.Name = "btnEquipeAdmin"
        Me.btnEquipeAdmin.Size = New System.Drawing.Size(200, 28)
        Me.btnEquipeAdmin.TabIndex = 0
        Me.btnEquipeAdmin.Text = "ObtenirEquipesUniteAdmin()"
        Me.tltIndication.SetToolTip(Me.btnEquipeAdmin, "Paramètre 1: L'identifiant de l'unité administratif" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10))
        Me.btnEquipeAdmin.UseVisualStyleBackColor = True
        '
        'btnEquipesUtilsateur
        '
        Me.btnEquipesUtilsateur.Location = New System.Drawing.Point(16, 380)
        Me.btnEquipesUtilsateur.Margin = New System.Windows.Forms.Padding(4)
        Me.btnEquipesUtilsateur.Name = "btnEquipesUtilsateur"
        Me.btnEquipesUtilsateur.Size = New System.Drawing.Size(200, 28)
        Me.btnEquipesUtilsateur.TabIndex = 0
        Me.btnEquipesUtilsateur.Text = "ObtenirEquipesUtilisateur()"
        Me.tltIndication.SetToolTip(Me.btnEquipesUtilsateur, "Paramètre 1: L'identifiant de l'utilisateur")
        Me.btnEquipesUtilsateur.UseVisualStyleBackColor = True
        '
        'btnListeUA
        '
        Me.btnListeUA.Location = New System.Drawing.Point(16, 416)
        Me.btnListeUA.Margin = New System.Windows.Forms.Padding(4)
        Me.btnListeUA.Name = "btnListeUA"
        Me.btnListeUA.Size = New System.Drawing.Size(200, 28)
        Me.btnListeUA.TabIndex = 0
        Me.btnListeUA.Text = "ObtenirListeUnitesAdmin()"
        Me.tltIndication.SetToolTip(Me.btnListeUA, "Aucun paramètre nécessaire")
        Me.btnListeUA.UseVisualStyleBackColor = True
        '
        'btnRoleEquipe
        '
        Me.btnRoleEquipe.Location = New System.Drawing.Point(224, 309)
        Me.btnRoleEquipe.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRoleEquipe.Name = "btnRoleEquipe"
        Me.btnRoleEquipe.Size = New System.Drawing.Size(200, 28)
        Me.btnRoleEquipe.TabIndex = 0
        Me.btnRoleEquipe.Text = "ObtenirRoleEquipe()"
        Me.tltIndication.SetToolTip(Me.btnRoleEquipe, "Paramètre 1: L'identifiant de rôle équipe")
        Me.btnRoleEquipe.UseVisualStyleBackColor = True
        '
        'btnRolesUA
        '
        Me.btnRolesUA.Location = New System.Drawing.Point(224, 345)
        Me.btnRolesUA.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRolesUA.Name = "btnRolesUA"
        Me.btnRolesUA.Size = New System.Drawing.Size(200, 28)
        Me.btnRolesUA.TabIndex = 0
        Me.btnRolesUA.Text = "ObtenirRolesUniteAdmin()"
        Me.tltIndication.SetToolTip(Me.btnRolesUA, "Paramètre 1: L'indentifiant de l'unité administratif")
        Me.btnRolesUA.UseVisualStyleBackColor = True
        '
        'btnUniteAdministratives
        '
        Me.btnUniteAdministratives.Location = New System.Drawing.Point(224, 380)
        Me.btnUniteAdministratives.Margin = New System.Windows.Forms.Padding(4)
        Me.btnUniteAdministratives.Name = "btnUniteAdministratives"
        Me.btnUniteAdministratives.Size = New System.Drawing.Size(200, 28)
        Me.btnUniteAdministratives.TabIndex = 0
        Me.btnUniteAdministratives.Text = "ObtenirUnitesAdmin()"
        Me.tltIndication.SetToolTip(Me.btnUniteAdministratives, "Paramètre 1: L'identifiant de l'utilisateur")
        Me.btnUniteAdministratives.UseVisualStyleBackColor = True
        '
        'btnUser
        '
        Me.btnUser.Location = New System.Drawing.Point(224, 416)
        Me.btnUser.Margin = New System.Windows.Forms.Padding(4)
        Me.btnUser.Name = "btnUser"
        Me.btnUser.Size = New System.Drawing.Size(200, 28)
        Me.btnUser.TabIndex = 0
        Me.btnUser.Text = "ObtenirUtilisateur()"
        Me.tltIndication.SetToolTip(Me.btnUser, "Paramètre 1: L'identifiant de l'utilisateur")
        Me.btnUser.UseVisualStyleBackColor = True
        '
        'btnRoles
        '
        Me.btnRoles.Location = New System.Drawing.Point(432, 309)
        Me.btnRoles.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRoles.Name = "btnRoles"
        Me.btnRoles.Size = New System.Drawing.Size(200, 28)
        Me.btnRoles.TabIndex = 0
        Me.btnRoles.Text = "RechercherRole()"
        Me.tltIndication.SetToolTip(Me.btnRoles, "Paramètre 1: Nom partiel ou complet d'un rôle")
        Me.btnRoles.UseVisualStyleBackColor = True
        '
        'btnUsers
        '
        Me.btnUsers.Location = New System.Drawing.Point(432, 345)
        Me.btnUsers.Margin = New System.Windows.Forms.Padding(4)
        Me.btnUsers.Name = "btnUsers"
        Me.btnUsers.Size = New System.Drawing.Size(200, 28)
        Me.btnUsers.TabIndex = 0
        Me.btnUsers.Text = "RechercherUtilisateur()"
        Me.tltIndication.SetToolTip(Me.btnUsers, "Paramètre 1: Nom partiel ou complet d'un utilisateur")
        Me.btnUsers.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 236)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(86, 17)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Paramètre 1"
        '
        'txtParam1
        '
        Me.txtParam1.Location = New System.Drawing.Point(16, 256)
        Me.txtParam1.Margin = New System.Windows.Forms.Padding(4)
        Me.txtParam1.Name = "txtParam1"
        Me.txtParam1.Size = New System.Drawing.Size(205, 22)
        Me.txtParam1.TabIndex = 2
        '
        'txtResultat
        '
        Me.txtResultat.Location = New System.Drawing.Point(16, 30)
        Me.txtResultat.Margin = New System.Windows.Forms.Padding(4)
        Me.txtResultat.Multiline = True
        Me.txtResultat.Name = "txtResultat"
        Me.txtResultat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtResultat.Size = New System.Drawing.Size(689, 202)
        Me.txtResultat.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 10)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 17)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Résultat"
        '
        'tltIndication
        '
        Me.tltIndication.ToolTipTitle = "Paramètre(s):"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(471, 381)
        Me.Button1.Margin = New System.Windows.Forms.Padding(4)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(200, 28)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "TestPerf()"
        Me.tltIndication.SetToolTip(Me.Button1, "Paramètre 1: Nom partiel ou complet d'un utilisateur")
        Me.Button1.UseVisualStyleBackColor = True
        '
        'btnRafraichirBuffer
        '
        Me.btnRafraichirBuffer.Location = New System.Drawing.Point(432, 416)
        Me.btnRafraichirBuffer.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRafraichirBuffer.Name = "btnRafraichirBuffer"
        Me.btnRafraichirBuffer.Size = New System.Drawing.Size(200, 28)
        Me.btnRafraichirBuffer.TabIndex = 0
        Me.btnRafraichirBuffer.Text = "Vider le cache sage"
        Me.btnRafraichirBuffer.UseVisualStyleBackColor = True
        '
        'btnDemandes
        '
        Me.btnDemandes.Location = New System.Drawing.Point(471, 252)
        Me.btnDemandes.Margin = New System.Windows.Forms.Padding(4)
        Me.btnDemandes.Name = "btnDemandes"
        Me.btnDemandes.Size = New System.Drawing.Size(200, 28)
        Me.btnDemandes.TabIndex = 0
        Me.btnDemandes.Text = "Passer en mode demandes"
        Me.btnDemandes.UseVisualStyleBackColor = True
        '
        'btnErreurs
        '
        Me.btnErreurs.Location = New System.Drawing.Point(263, 252)
        Me.btnErreurs.Margin = New System.Windows.Forms.Padding(4)
        Me.btnErreurs.Name = "btnErreurs"
        Me.btnErreurs.Size = New System.Drawing.Size(200, 28)
        Me.btnErreurs.TabIndex = 0
        Me.btnErreurs.Text = "Passer en mode test erreus"
        Me.btnErreurs.UseVisualStyleBackColor = True
        '
        'TsFdFonctionsIndependantes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(723, 459)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.txtResultat)
        Me.Controls.Add(Me.txtParam1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnRoles)
        Me.Controls.Add(Me.btnUser)
        Me.Controls.Add(Me.btnRolesUA)
        Me.Controls.Add(Me.btnRoleEquipe)
        Me.Controls.Add(Me.btnErreurs)
        Me.Controls.Add(Me.btnDemandes)
        Me.Controls.Add(Me.btnRafraichirBuffer)
        Me.Controls.Add(Me.btnUsers)
        Me.Controls.Add(Me.btnUniteAdministratives)
        Me.Controls.Add(Me.btnEquipesUtilsateur)
        Me.Controls.Add(Me.btnListeUA)
        Me.Controls.Add(Me.btnEquipeAdmin)
        Me.Controls.Add(Me.btnLienUserRole)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "TsFdFonctionsIndependantes"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "(TS7D121) Driver pour tester les fonctions indépendantes"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnLienUserRole As System.Windows.Forms.Button
    Friend WithEvents btnEquipeAdmin As System.Windows.Forms.Button
    Friend WithEvents btnEquipesUtilsateur As System.Windows.Forms.Button
    Friend WithEvents btnListeUA As System.Windows.Forms.Button
    Friend WithEvents btnRoleEquipe As System.Windows.Forms.Button
    Friend WithEvents btnRolesUA As System.Windows.Forms.Button
    Friend WithEvents btnUniteAdministratives As System.Windows.Forms.Button
    Friend WithEvents btnUser As System.Windows.Forms.Button
    Friend WithEvents btnRoles As System.Windows.Forms.Button
    Friend WithEvents btnUsers As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtParam1 As System.Windows.Forms.TextBox
    Friend WithEvents txtResultat As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents tltIndication As System.Windows.Forms.ToolTip
    Friend WithEvents btnRafraichirBuffer As System.Windows.Forms.Button
    Friend WithEvents btnDemandes As System.Windows.Forms.Button
    Friend WithEvents btnErreurs As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
End Class
