<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRole
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
        Me.txtID = New System.Windows.Forms.TextBox
        Me.lbUserID = New System.Windows.Forms.Label
        Me.txtName = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtOwner = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtOrganisation = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtDate = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtCodeApproval = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.txtReview = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.txtFilter = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.txtType = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.txtDateApprove = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtDateExpiration = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.lstbRelationURo = New System.Windows.Forms.ListBox
        Me.lstbRelationRoRe = New System.Windows.Forms.ListBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.lstbRelationFils = New System.Windows.Forms.ListBox
        Me.lstbRelationParent = New System.Windows.Forms.ListBox
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtID
        '
        Me.txtID.Location = New System.Drawing.Point(33, 40)
        Me.txtID.Name = "txtID"
        Me.txtID.ReadOnly = True
        Me.txtID.Size = New System.Drawing.Size(269, 22)
        Me.txtID.TabIndex = 4
        Me.txtID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lbUserID
        '
        Me.lbUserID.AutoSize = True
        Me.lbUserID.Location = New System.Drawing.Point(30, 20)
        Me.lbUserID.Name = "lbUserID"
        Me.lbUserID.Size = New System.Drawing.Size(58, 17)
        Me.lbUserID.TabIndex = 3
        Me.lbUserID.Text = "Role ID:"
        '
        'txtName
        '
        Me.txtName.Location = New System.Drawing.Point(332, 39)
        Me.txtName.Name = "txtName"
        Me.txtName.ReadOnly = True
        Me.txtName.Size = New System.Drawing.Size(270, 22)
        Me.txtName.TabIndex = 6
        Me.txtName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(329, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(94, 17)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Nom du Rôle:"
        '
        'txtDescription
        '
        Me.txtDescription.Location = New System.Drawing.Point(33, 127)
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.ReadOnly = True
        Me.txtDescription.Size = New System.Drawing.Size(270, 110)
        Me.txtDescription.TabIndex = 8
        Me.txtDescription.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(30, 108)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(136, 17)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Description du Rôle:"
        '
        'txtOwner
        '
        Me.txtOwner.Location = New System.Drawing.Point(332, 303)
        Me.txtOwner.Multiline = True
        Me.txtOwner.Name = "txtOwner"
        Me.txtOwner.ReadOnly = True
        Me.txtOwner.Size = New System.Drawing.Size(270, 66)
        Me.txtOwner.TabIndex = 10
        Me.txtOwner.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(329, 284)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(83, 17)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Role owner:"
        '
        'txtOrganisation
        '
        Me.txtOrganisation.Location = New System.Drawing.Point(32, 303)
        Me.txtOrganisation.Multiline = True
        Me.txtOrganisation.Name = "txtOrganisation"
        Me.txtOrganisation.ReadOnly = True
        Me.txtOrganisation.Size = New System.Drawing.Size(270, 66)
        Me.txtOrganisation.TabIndex = 12
        Me.txtOrganisation.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(29, 284)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(126, 17)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "Role orgainisation:"
        '
        'txtDate
        '
        Me.txtDate.Location = New System.Drawing.Point(332, 127)
        Me.txtDate.Name = "txtDate"
        Me.txtDate.ReadOnly = True
        Me.txtDate.Size = New System.Drawing.Size(270, 22)
        Me.txtDate.TabIndex = 14
        Me.txtDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(329, 108)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(174, 17)
        Me.Label5.TabIndex = 13
        Me.Label5.Text = "Date de création du  Rôle:"
        '
        'txtCodeApproval
        '
        Me.txtCodeApproval.Location = New System.Drawing.Point(332, 84)
        Me.txtCodeApproval.Name = "txtCodeApproval"
        Me.txtCodeApproval.ReadOnly = True
        Me.txtCodeApproval.Size = New System.Drawing.Size(270, 22)
        Me.txtCodeApproval.TabIndex = 22
        Me.txtCodeApproval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(329, 65)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(189, 17)
        Me.Label8.TabIndex = 21
        Me.Label8.Text = "Code d'approbation du Rôle:"
        '
        'txtReview
        '
        Me.txtReview.Location = New System.Drawing.Point(32, 259)
        Me.txtReview.Name = "txtReview"
        Me.txtReview.ReadOnly = True
        Me.txtReview.Size = New System.Drawing.Size(270, 22)
        Me.txtReview.TabIndex = 20
        Me.txtReview.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(29, 240)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(103, 17)
        Me.Label9.TabIndex = 19
        Me.Label9.Text = "Role Reviewer:"
        '
        'txtFilter
        '
        Me.txtFilter.Location = New System.Drawing.Point(333, 259)
        Me.txtFilter.Name = "txtFilter"
        Me.txtFilter.ReadOnly = True
        Me.txtFilter.Size = New System.Drawing.Size(270, 22)
        Me.txtFilter.TabIndex = 18
        Me.txtFilter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(330, 240)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(96, 17)
        Me.Label10.TabIndex = 17
        Me.Label10.Text = "Filtre de Rôle:"
        '
        'txtType
        '
        Me.txtType.Location = New System.Drawing.Point(33, 85)
        Me.txtType.Name = "txtType"
        Me.txtType.ReadOnly = True
        Me.txtType.Size = New System.Drawing.Size(269, 22)
        Me.txtType.TabIndex = 16
        Me.txtType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(30, 65)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(97, 17)
        Me.Label11.TabIndex = 15
        Me.Label11.Text = "Type du Rôle:"
        '
        'txtDateApprove
        '
        Me.txtDateApprove.Location = New System.Drawing.Point(332, 171)
        Me.txtDateApprove.Name = "txtDateApprove"
        Me.txtDateApprove.ReadOnly = True
        Me.txtDateApprove.Size = New System.Drawing.Size(270, 22)
        Me.txtDateApprove.TabIndex = 24
        Me.txtDateApprove.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(329, 152)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(186, 17)
        Me.Label6.TabIndex = 23
        Me.Label6.Text = "Date d'approbation du Rôle:"
        '
        'txtDateExpiration
        '
        Me.txtDateExpiration.Location = New System.Drawing.Point(332, 215)
        Me.txtDateExpiration.Name = "txtDateExpiration"
        Me.txtDateExpiration.ReadOnly = True
        Me.txtDateExpiration.Size = New System.Drawing.Size(270, 22)
        Me.txtDateExpiration.TabIndex = 26
        Me.txtDateExpiration.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(329, 196)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(171, 17)
        Me.Label7.TabIndex = 25
        Me.Label7.Text = "Date d'expiration du Rôle:"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(6, 15)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(136, 17)
        Me.Label12.TabIndex = 27
        Me.Label12.Text = "Usager lié à ce Rôle"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(295, 15)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(158, 17)
        Me.Label13.TabIndex = 28
        Me.Label13.Text = "Ressource lié à ce Rôle"
        '
        'lstbRelationURo
        '
        Me.lstbRelationURo.FormattingEnabled = True
        Me.lstbRelationURo.ItemHeight = 16
        Me.lstbRelationURo.Location = New System.Drawing.Point(9, 32)
        Me.lstbRelationURo.Name = "lstbRelationURo"
        Me.lstbRelationURo.Size = New System.Drawing.Size(270, 84)
        Me.lstbRelationURo.TabIndex = 29
        '
        'lstbRelationRoRe
        '
        Me.lstbRelationRoRe.FormattingEnabled = True
        Me.lstbRelationRoRe.ItemHeight = 16
        Me.lstbRelationRoRe.Location = New System.Drawing.Point(298, 32)
        Me.lstbRelationRoRe.Name = "lstbRelationRoRe"
        Me.lstbRelationRoRe.Size = New System.Drawing.Size(270, 84)
        Me.lstbRelationRoRe.TabIndex = 30
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label15)
        Me.GroupBox1.Controls.Add(Me.Label14)
        Me.GroupBox1.Controls.Add(Me.lstbRelationFils)
        Me.GroupBox1.Controls.Add(Me.lstbRelationParent)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Controls.Add(Me.Label13)
        Me.GroupBox1.Controls.Add(Me.lstbRelationRoRe)
        Me.GroupBox1.Controls.Add(Me.lstbRelationURo)
        Me.GroupBox1.Location = New System.Drawing.Point(33, 371)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(579, 226)
        Me.GroupBox1.TabIndex = 31
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Relations"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(295, 118)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(130, 17)
        Me.Label15.TabIndex = 34
        Me.Label15.Text = "Rôle fils de ce Rôle"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(6, 118)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(154, 17)
        Me.Label14.TabIndex = 33
        Me.Label14.Text = "Rôle parent de ce Rôle"
        '
        'lstbRelationFils
        '
        Me.lstbRelationFils.FormattingEnabled = True
        Me.lstbRelationFils.ItemHeight = 16
        Me.lstbRelationFils.Location = New System.Drawing.Point(298, 135)
        Me.lstbRelationFils.Name = "lstbRelationFils"
        Me.lstbRelationFils.Size = New System.Drawing.Size(270, 84)
        Me.lstbRelationFils.TabIndex = 32
        '
        'lstbRelationParent
        '
        Me.lstbRelationParent.FormattingEnabled = True
        Me.lstbRelationParent.ItemHeight = 16
        Me.lstbRelationParent.Location = New System.Drawing.Point(9, 135)
        Me.lstbRelationParent.Name = "lstbRelationParent"
        Me.lstbRelationParent.Size = New System.Drawing.Size(270, 84)
        Me.lstbRelationParent.TabIndex = 31
        '
        'frmRole
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(612, 598)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.txtDateExpiration)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtDateApprove)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtCodeApproval)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.txtReview)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.txtFilter)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.txtType)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.txtDate)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtOrganisation)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtOwner)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtDescription)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtID)
        Me.Controls.Add(Me.lbUserID)
        Me.Name = "frmRole"
        Me.Text = "Description du Rôle"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtID As System.Windows.Forms.TextBox
    Friend WithEvents lbUserID As System.Windows.Forms.Label
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtDescription As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtOwner As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtOrganisation As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtDate As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtCodeApproval As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtReview As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtFilter As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtType As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtDateApprove As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtDateExpiration As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents lstbRelationURo As System.Windows.Forms.ListBox
    Friend WithEvents lstbRelationRoRe As System.Windows.Forms.ListBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents lstbRelationFils As System.Windows.Forms.ListBox
    Friend WithEvents lstbRelationParent As System.Windows.Forms.ListBox
End Class
