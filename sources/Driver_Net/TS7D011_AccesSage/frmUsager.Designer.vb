<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUsager
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
        Me.lbUserID = New System.Windows.Forms.Label
        Me.lbPersonID = New System.Windows.Forms.Label
        Me.lbUserName = New System.Windows.Forms.Label
        Me.lbOrganisation = New System.Windows.Forms.Label
        Me.lbOrganisationType = New System.Windows.Forms.Label
        Me.txtUserID = New System.Windows.Forms.TextBox
        Me.txtPersonID = New System.Windows.Forms.TextBox
        Me.txtUserName = New System.Windows.Forms.TextBox
        Me.txtOrganisazion = New System.Windows.Forms.TextBox
        Me.txtOrganizationType = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.lstbRole = New System.Windows.Forms.ListBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.lstbRessource = New System.Windows.Forms.ListBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lbUserID
        '
        Me.lbUserID.AutoSize = True
        Me.lbUserID.Location = New System.Drawing.Point(30, 20)
        Me.lbUserID.Name = "lbUserID"
        Me.lbUserID.Size = New System.Drawing.Size(59, 17)
        Me.lbUserID.TabIndex = 0
        Me.lbUserID.Text = "User ID:"
        '
        'lbPersonID
        '
        Me.lbPersonID.AutoSize = True
        Me.lbPersonID.Location = New System.Drawing.Point(30, 70)
        Me.lbPersonID.Name = "lbPersonID"
        Me.lbPersonID.Size = New System.Drawing.Size(74, 17)
        Me.lbPersonID.TabIndex = 1
        Me.lbPersonID.Text = "Person ID:"
        '
        'lbUserName
        '
        Me.lbUserName.AutoSize = True
        Me.lbUserName.Location = New System.Drawing.Point(30, 120)
        Me.lbUserName.Name = "lbUserName"
        Me.lbUserName.Size = New System.Drawing.Size(115, 17)
        Me.lbUserName.TabIndex = 1
        Me.lbUserName.Text = "Nom de l'usager:"
        '
        'lbOrganisation
        '
        Me.lbOrganisation.AutoSize = True
        Me.lbOrganisation.Location = New System.Drawing.Point(30, 170)
        Me.lbOrganisation.Name = "lbOrganisation"
        Me.lbOrganisation.Size = New System.Drawing.Size(101, 17)
        Me.lbOrganisation.TabIndex = 1
        Me.lbOrganisation.Text = "L'organisation:"
        '
        'lbOrganisationType
        '
        Me.lbOrganisationType.AutoSize = True
        Me.lbOrganisationType.Location = New System.Drawing.Point(30, 226)
        Me.lbOrganisationType.Name = "lbOrganisationType"
        Me.lbOrganisationType.Size = New System.Drawing.Size(152, 17)
        Me.lbOrganisationType.TabIndex = 1
        Me.lbOrganisationType.Text = "Type de l'organisation:"
        '
        'txtUserID
        '
        Me.txtUserID.Location = New System.Drawing.Point(33, 40)
        Me.txtUserID.Name = "txtUserID"
        Me.txtUserID.ReadOnly = True
        Me.txtUserID.Size = New System.Drawing.Size(270, 22)
        Me.txtUserID.TabIndex = 2
        Me.txtUserID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtPersonID
        '
        Me.txtPersonID.Location = New System.Drawing.Point(33, 90)
        Me.txtPersonID.Name = "txtPersonID"
        Me.txtPersonID.ReadOnly = True
        Me.txtPersonID.Size = New System.Drawing.Size(270, 22)
        Me.txtPersonID.TabIndex = 3
        Me.txtPersonID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtUserName
        '
        Me.txtUserName.Location = New System.Drawing.Point(33, 140)
        Me.txtUserName.Name = "txtUserName"
        Me.txtUserName.ReadOnly = True
        Me.txtUserName.Size = New System.Drawing.Size(270, 22)
        Me.txtUserName.TabIndex = 4
        Me.txtUserName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtOrganisazion
        '
        Me.txtOrganisazion.Location = New System.Drawing.Point(33, 190)
        Me.txtOrganisazion.Name = "txtOrganisazion"
        Me.txtOrganisazion.ReadOnly = True
        Me.txtOrganisazion.Size = New System.Drawing.Size(270, 22)
        Me.txtOrganisazion.TabIndex = 5
        Me.txtOrganisazion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtOrganizationType
        '
        Me.txtOrganizationType.Location = New System.Drawing.Point(33, 246)
        Me.txtOrganizationType.Name = "txtOrganizationType"
        Me.txtOrganizationType.ReadOnly = True
        Me.txtOrganizationType.Size = New System.Drawing.Size(270, 22)
        Me.txtOrganizationType.TabIndex = 6
        Me.txtOrganizationType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(138, 17)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Rôle lié à cet usager"
        '
        'lstbRole
        '
        Me.lstbRole.FormattingEnabled = True
        Me.lstbRole.ItemHeight = 16
        Me.lstbRole.Location = New System.Drawing.Point(10, 38)
        Me.lstbRole.Name = "lstbRole"
        Me.lstbRole.Size = New System.Drawing.Size(162, 196)
        Me.lstbRole.TabIndex = 9
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(175, 18)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(177, 17)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Ressource lié à cet usager"
        '
        'lstbRessource
        '
        Me.lstbRessource.FormattingEnabled = True
        Me.lstbRessource.ItemHeight = 16
        Me.lstbRessource.Location = New System.Drawing.Point(178, 38)
        Me.lstbRessource.Name = "lstbRessource"
        Me.lstbRessource.Size = New System.Drawing.Size(162, 196)
        Me.lstbRessource.TabIndex = 9
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.lstbRessource)
        Me.GroupBox1.Controls.Add(Me.lstbRole)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Location = New System.Drawing.Point(323, 20)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(357, 248)
        Me.GroupBox1.TabIndex = 10
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Relations"
        '
        'frmUsager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(692, 278)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.txtOrganizationType)
        Me.Controls.Add(Me.txtOrganisazion)
        Me.Controls.Add(Me.txtUserName)
        Me.Controls.Add(Me.txtPersonID)
        Me.Controls.Add(Me.txtUserID)
        Me.Controls.Add(Me.lbOrganisationType)
        Me.Controls.Add(Me.lbOrganisation)
        Me.Controls.Add(Me.lbUserName)
        Me.Controls.Add(Me.lbPersonID)
        Me.Controls.Add(Me.lbUserID)
        Me.Name = "frmUsager"
        Me.Text = "Description de l'Usager"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lbUserID As System.Windows.Forms.Label
    Friend WithEvents lbPersonID As System.Windows.Forms.Label
    Friend WithEvents lbUserName As System.Windows.Forms.Label
    Friend WithEvents lbOrganisation As System.Windows.Forms.Label
    Friend WithEvents lbOrganisationType As System.Windows.Forms.Label
    Friend WithEvents txtUserID As System.Windows.Forms.TextBox
    Friend WithEvents txtPersonID As System.Windows.Forms.TextBox
    Friend WithEvents txtUserName As System.Windows.Forms.TextBox
    Friend WithEvents txtOrganisazion As System.Windows.Forms.TextBox
    Friend WithEvents txtOrganizationType As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lstbRole As System.Windows.Forms.ListBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lstbRessource As System.Windows.Forms.ListBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
End Class
