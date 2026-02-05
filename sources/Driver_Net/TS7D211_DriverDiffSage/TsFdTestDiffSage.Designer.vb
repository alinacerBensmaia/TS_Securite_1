<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TsFdTestDiffSage
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
        Me.txtParam1 = New System.Windows.Forms.TextBox
        Me.txtParam2 = New System.Windows.Forms.TextBox
        Me.lblParam1 = New System.Windows.Forms.Label
        Me.lblParam2 = New System.Windows.Forms.Label
        Me.btnLiensUReRec = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnValiderExistence = New System.Windows.Forms.Button
        Me.btnLienUReDir = New System.Windows.Forms.Button
        Me.btnLienURo = New System.Windows.Forms.Button
        Me.btnLienRoleRole = New System.Windows.Forms.Button
        Me.btnLienRoRe = New System.Windows.Forms.Button
        Me.btnDiffUser = New System.Windows.Forms.Button
        Me.btnDiffRole = New System.Windows.Forms.Button
        Me.btnDiffRessource = New System.Windows.Forms.Button
        Me.btnAttrbUser = New System.Windows.Forms.Button
        Me.btnAttrbRole = New System.Windows.Forms.Button
        Me.btnAttrbRessr = New System.Windows.Forms.Button
        Me.btnListeConfig = New System.Windows.Forms.Button
        Me.txtResultat = New System.Windows.Forms.TextBox
        Me.lblResultat = New System.Windows.Forms.Label
        Me.tltIndication = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnValiderIntegriter = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'txtParam1
        '
        Me.txtParam1.Location = New System.Drawing.Point(12, 264)
        Me.txtParam1.Name = "txtParam1"
        Me.txtParam1.Size = New System.Drawing.Size(212, 20)
        Me.txtParam1.TabIndex = 1
        '
        'txtParam2
        '
        Me.txtParam2.Location = New System.Drawing.Point(230, 264)
        Me.txtParam2.Name = "txtParam2"
        Me.txtParam2.Size = New System.Drawing.Size(212, 20)
        Me.txtParam2.TabIndex = 2
        '
        'lblParam1
        '
        Me.lblParam1.AutoSize = True
        Me.lblParam1.Location = New System.Drawing.Point(9, 248)
        Me.lblParam1.Name = "lblParam1"
        Me.lblParam1.Size = New System.Drawing.Size(64, 13)
        Me.lblParam1.TabIndex = 1
        Me.lblParam1.Text = "Paramètre 1"
        '
        'lblParam2
        '
        Me.lblParam2.AutoSize = True
        Me.lblParam2.Location = New System.Drawing.Point(227, 248)
        Me.lblParam2.Name = "lblParam2"
        Me.lblParam2.Size = New System.Drawing.Size(64, 13)
        Me.lblParam2.TabIndex = 1
        Me.lblParam2.Text = "Paramètre 2"
        '
        'btnLiensUReRec
        '
        Me.btnLiensUReRec.Location = New System.Drawing.Point(12, 435)
        Me.btnLiensUReRec.Name = "btnLiensUReRec"
        Me.btnLiensUReRec.Size = New System.Drawing.Size(212, 23)
        Me.btnLiensUReRec.TabIndex = 8
        Me.btnLiensUReRec.Text = "ObtnrDiffrUilisateurRessourceRecurcif()"
        Me.tltIndication.SetToolTip(Me.btnLiensUReRec, "Paramètre 1: Nom de la cible (Ex: Active Directoy)")
        Me.btnLiensUReRec.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(12, 377)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(212, 23)
        Me.btnNew.TabIndex = 3
        Me.btnNew.Text = "Créer l'objet interactif"
        Me.tltIndication.SetToolTip(Me.btnNew, "Paramètre 1: Nom de la vieille configuration" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Paramètre 2: Nom de la nouvelle con" & _
                "figuration")
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnValiderExistence
        '
        Me.btnValiderExistence.Location = New System.Drawing.Point(12, 319)
        Me.btnValiderExistence.Name = "btnValiderExistence"
        Me.btnValiderExistence.Size = New System.Drawing.Size(212, 23)
        Me.btnValiderExistence.TabIndex = 5
        Me.btnValiderExistence.Text = "ValiderExistenceConfig()"
        Me.tltIndication.SetToolTip(Me.btnValiderExistence, "Paramètre 1: Nom de la configuration")
        Me.btnValiderExistence.UseVisualStyleBackColor = True
        '
        'btnLienUReDir
        '
        Me.btnLienUReDir.Location = New System.Drawing.Point(12, 406)
        Me.btnLienUReDir.Name = "btnLienUReDir"
        Me.btnLienUReDir.Size = New System.Drawing.Size(212, 23)
        Me.btnLienUReDir.TabIndex = 7
        Me.btnLienUReDir.Text = "ObtnrDiffrUtilisateurRessourceDirect()"
        Me.tltIndication.SetToolTip(Me.btnLienUReDir, "Paramètre 1: Nom de la cible (Ex: Active Directoy)")
        Me.btnLienUReDir.UseVisualStyleBackColor = True
        '
        'btnLienURo
        '
        Me.btnLienURo.Location = New System.Drawing.Point(230, 290)
        Me.btnLienURo.Name = "btnLienURo"
        Me.btnLienURo.Size = New System.Drawing.Size(212, 23)
        Me.btnLienURo.TabIndex = 9
        Me.btnLienURo.Text = "ObtnrDiffrUtilisateurRole()"
        Me.tltIndication.SetToolTip(Me.btnLienURo, "Aucun paramètre requis pour cet appel")
        Me.btnLienURo.UseVisualStyleBackColor = True
        '
        'btnLienRoleRole
        '
        Me.btnLienRoleRole.Location = New System.Drawing.Point(230, 348)
        Me.btnLienRoleRole.Name = "btnLienRoleRole"
        Me.btnLienRoleRole.Size = New System.Drawing.Size(212, 23)
        Me.btnLienRoleRole.TabIndex = 11
        Me.btnLienRoleRole.Text = "ObtnrDiffrRoleRole()"
        Me.tltIndication.SetToolTip(Me.btnLienRoleRole, "Aucun paramètre requis pour cet appel")
        Me.btnLienRoleRole.UseVisualStyleBackColor = True
        '
        'btnLienRoRe
        '
        Me.btnLienRoRe.Location = New System.Drawing.Point(230, 319)
        Me.btnLienRoRe.Name = "btnLienRoRe"
        Me.btnLienRoRe.Size = New System.Drawing.Size(212, 23)
        Me.btnLienRoRe.TabIndex = 10
        Me.btnLienRoRe.Text = "ObtnrDiffrRoleRessource()"
        Me.tltIndication.SetToolTip(Me.btnLienRoRe, "Paramètre 1: Nom de la cible (Ex: Active Directoy)")
        Me.btnLienRoRe.UseVisualStyleBackColor = True
        '
        'btnDiffUser
        '
        Me.btnDiffUser.Location = New System.Drawing.Point(230, 377)
        Me.btnDiffUser.Name = "btnDiffUser"
        Me.btnDiffUser.Size = New System.Drawing.Size(212, 23)
        Me.btnDiffUser.TabIndex = 12
        Me.btnDiffUser.Text = "ObtnrDiffrUtilisateur()"
        Me.tltIndication.SetToolTip(Me.btnDiffUser, "Aucun paramètre requis pour cet appel")
        Me.btnDiffUser.UseVisualStyleBackColor = True
        '
        'btnDiffRole
        '
        Me.btnDiffRole.Location = New System.Drawing.Point(230, 406)
        Me.btnDiffRole.Name = "btnDiffRole"
        Me.btnDiffRole.Size = New System.Drawing.Size(212, 23)
        Me.btnDiffRole.TabIndex = 13
        Me.btnDiffRole.Text = "ObtnrDiffrRole()"
        Me.tltIndication.SetToolTip(Me.btnDiffRole, "Aucun paramètre requis pour cet appel")
        Me.btnDiffRole.UseVisualStyleBackColor = True
        '
        'btnDiffRessource
        '
        Me.btnDiffRessource.Location = New System.Drawing.Point(230, 435)
        Me.btnDiffRessource.Name = "btnDiffRessource"
        Me.btnDiffRessource.Size = New System.Drawing.Size(212, 23)
        Me.btnDiffRessource.TabIndex = 14
        Me.btnDiffRessource.Text = "ObtnrDiffrRessource()"
        Me.tltIndication.SetToolTip(Me.btnDiffRessource, "Paramètre 1: Nom de la cible (Ex: Active Directoy)")
        Me.btnDiffRessource.UseVisualStyleBackColor = True
        '
        'btnAttrbUser
        '
        Me.btnAttrbUser.Location = New System.Drawing.Point(448, 290)
        Me.btnAttrbUser.Name = "btnAttrbUser"
        Me.btnAttrbUser.Size = New System.Drawing.Size(212, 23)
        Me.btnAttrbUser.TabIndex = 15
        Me.btnAttrbUser.Text = "ObtnrDiffrAttrbUser()"
        Me.tltIndication.SetToolTip(Me.btnAttrbUser, "Aucun paramètre requis pour cet appel")
        Me.btnAttrbUser.UseVisualStyleBackColor = True
        '
        'btnAttrbRole
        '
        Me.btnAttrbRole.Location = New System.Drawing.Point(448, 319)
        Me.btnAttrbRole.Name = "btnAttrbRole"
        Me.btnAttrbRole.Size = New System.Drawing.Size(212, 23)
        Me.btnAttrbRole.TabIndex = 16
        Me.btnAttrbRole.Text = "ObtnrDiffrAttrbRole()"
        Me.tltIndication.SetToolTip(Me.btnAttrbRole, "Aucun paramètre requis pour cet appel")
        Me.btnAttrbRole.UseVisualStyleBackColor = True
        '
        'btnAttrbRessr
        '
        Me.btnAttrbRessr.Location = New System.Drawing.Point(448, 348)
        Me.btnAttrbRessr.Name = "btnAttrbRessr"
        Me.btnAttrbRessr.Size = New System.Drawing.Size(212, 23)
        Me.btnAttrbRessr.TabIndex = 17
        Me.btnAttrbRessr.Text = "ObtnrDiffrAttrbRessr()"
        Me.tltIndication.SetToolTip(Me.btnAttrbRessr, "Paramètre 1: Nom de la cible (Ex: Active Directoy)")
        Me.btnAttrbRessr.UseVisualStyleBackColor = True
        '
        'btnListeConfig
        '
        Me.btnListeConfig.Location = New System.Drawing.Point(12, 290)
        Me.btnListeConfig.Name = "btnListeConfig"
        Me.btnListeConfig.Size = New System.Drawing.Size(212, 23)
        Me.btnListeConfig.TabIndex = 4
        Me.btnListeConfig.Text = "ObtenirListeConfig()"
        Me.tltIndication.SetToolTip(Me.btnListeConfig, "Aucun paramètre requis pour cet appel")
        Me.btnListeConfig.UseVisualStyleBackColor = True
        '
        'txtResultat
        '
        Me.txtResultat.Location = New System.Drawing.Point(12, 24)
        Me.txtResultat.Multiline = True
        Me.txtResultat.Name = "txtResultat"
        Me.txtResultat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtResultat.Size = New System.Drawing.Size(648, 221)
        Me.txtResultat.TabIndex = 0
        '
        'lblResultat
        '
        Me.lblResultat.AutoSize = True
        Me.lblResultat.Location = New System.Drawing.Point(9, 8)
        Me.lblResultat.Name = "lblResultat"
        Me.lblResultat.Size = New System.Drawing.Size(46, 13)
        Me.lblResultat.TabIndex = 1
        Me.lblResultat.Text = "Résultat"
        '
        'tltIndication
        '
        Me.tltIndication.ToolTipTitle = "Paramètre(s):"
        '
        'btnValiderIntegriter
        '
        Me.btnValiderIntegriter.Location = New System.Drawing.Point(12, 348)
        Me.btnValiderIntegriter.Name = "btnValiderIntegriter"
        Me.btnValiderIntegriter.Size = New System.Drawing.Size(212, 23)
        Me.btnValiderIntegriter.TabIndex = 6
        Me.btnValiderIntegriter.Text = "ValiderIntegriteConfig()"
        Me.tltIndication.SetToolTip(Me.btnValiderIntegriter, "Paramètre 1: Nom de la configuration")
        Me.btnValiderIntegriter.UseVisualStyleBackColor = True
        '
        'TsFdTestDiffSage
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(670, 469)
        Me.Controls.Add(Me.txtResultat)
        Me.Controls.Add(Me.btnAttrbRole)
        Me.Controls.Add(Me.btnLienRoleRole)
        Me.Controls.Add(Me.btnAttrbUser)
        Me.Controls.Add(Me.btnLienURo)
        Me.Controls.Add(Me.btnDiffRessource)
        Me.Controls.Add(Me.btnLienUReDir)
        Me.Controls.Add(Me.btnDiffRole)
        Me.Controls.Add(Me.btnListeConfig)
        Me.Controls.Add(Me.btnDiffUser)
        Me.Controls.Add(Me.btnValiderIntegriter)
        Me.Controls.Add(Me.btnValiderExistence)
        Me.Controls.Add(Me.btnAttrbRessr)
        Me.Controls.Add(Me.btnLienRoRe)
        Me.Controls.Add(Me.btnNew)
        Me.Controls.Add(Me.btnLiensUReRec)
        Me.Controls.Add(Me.lblParam2)
        Me.Controls.Add(Me.lblResultat)
        Me.Controls.Add(Me.lblParam1)
        Me.Controls.Add(Me.txtParam2)
        Me.Controls.Add(Me.txtParam1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "TsFdTestDiffSage"
        Me.Text = "Driver TS7N211"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtParam1 As System.Windows.Forms.TextBox
    Friend WithEvents txtParam2 As System.Windows.Forms.TextBox
    Friend WithEvents lblParam1 As System.Windows.Forms.Label
    Friend WithEvents lblParam2 As System.Windows.Forms.Label
    Friend WithEvents btnLiensUReRec As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnValiderExistence As System.Windows.Forms.Button
    Friend WithEvents btnLienUReDir As System.Windows.Forms.Button
    Friend WithEvents btnLienURo As System.Windows.Forms.Button
    Friend WithEvents btnLienRoleRole As System.Windows.Forms.Button
    Friend WithEvents btnLienRoRe As System.Windows.Forms.Button
    Friend WithEvents btnDiffUser As System.Windows.Forms.Button
    Friend WithEvents btnDiffRole As System.Windows.Forms.Button
    Friend WithEvents btnDiffRessource As System.Windows.Forms.Button
    Friend WithEvents btnAttrbUser As System.Windows.Forms.Button
    Friend WithEvents btnAttrbRole As System.Windows.Forms.Button
    Friend WithEvents btnAttrbRessr As System.Windows.Forms.Button
    Friend WithEvents btnListeConfig As System.Windows.Forms.Button
    Friend WithEvents txtResultat As System.Windows.Forms.TextBox
    Friend WithEvents lblResultat As System.Windows.Forms.Label
    Friend WithEvents tltIndication As System.Windows.Forms.ToolTip
    Friend WithEvents btnValiderIntegriter As System.Windows.Forms.Button

End Class
