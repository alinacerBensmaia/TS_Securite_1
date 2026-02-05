<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TsFgMultiEnv
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                If mDtEnv IsNot Nothing Then mDtEnv.Dispose()
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TsFgMultiEnv))
        Me.txtProfil = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTextbox()
        Me.txtCode = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTextbox()
        Me.cmdProfil = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton()
        Me.imgListe = New System.Windows.Forms.ImageList(Me.components)
        Me.lblProfil = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel()
        Me.lblMdp = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel()
        Me.lblCode = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel()
        Me.Label1 = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel()
        Me.cboEnv = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XzCrComboBox()
        Me.cmdAnnuler = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton()
        Me.cmdOk = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton()
        Me.cmdMontrerPassword = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton()
        Me.txtMdp = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTextbox()
        Me.cmdGenererMdp = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton()
        Me.SuspendLayout()
        '
        'txtProfil
        '
        Me.txtProfil.Location = New System.Drawing.Point(140, 91)
        Me.txtProfil.MaxLength = 32
        Me.txtProfil.Name = "txtProfil"
        Me.txtProfil.NomChampsDonnee = Nothing
        Me.txtProfil.NomSourceDonnee = Nothing
        Me.txtProfil.Size = New System.Drawing.Size(216, 25)
        Me.txtProfil.TabIndex = 5
        '
        'txtCode
        '
        Me.txtCode.Location = New System.Drawing.Point(140, 34)
        Me.txtCode.MaxLength = 50
        Me.txtCode.Name = "txtCode"
        Me.txtCode.NomChampsDonnee = Nothing
        Me.txtCode.NomSourceDonnee = Nothing
        Me.txtCode.Size = New System.Drawing.Size(216, 25)
        Me.txtCode.TabIndex = 2
        '
        'cmdProfil
        '
        Me.cmdProfil.ImageIndex = 8
        Me.cmdProfil.ImageList = Me.imgListe
        Me.cmdProfil.Location = New System.Drawing.Point(362, 91)
        Me.cmdProfil.Name = "cmdProfil"
        Me.cmdProfil.Size = New System.Drawing.Size(26, 26)
        Me.cmdProfil.TabIndex = 6
        '
        'imgListe
        '
        Me.imgListe.ImageStream = CType(resources.GetObject("imgListe.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgListe.TransparentColor = System.Drawing.Color.Transparent
        Me.imgListe.Images.SetKeyName(0, "")
        Me.imgListe.Images.SetKeyName(1, "")
        Me.imgListe.Images.SetKeyName(2, "")
        Me.imgListe.Images.SetKeyName(3, "")
        Me.imgListe.Images.SetKeyName(4, "")
        Me.imgListe.Images.SetKeyName(5, "")
        Me.imgListe.Images.SetKeyName(6, "")
        Me.imgListe.Images.SetKeyName(7, "")
        Me.imgListe.Images.SetKeyName(8, "")
        Me.imgListe.Images.SetKeyName(9, "")
        Me.imgListe.Images.SetKeyName(10, "")
        Me.imgListe.Images.SetKeyName(11, "")
        Me.imgListe.Images.SetKeyName(12, "")
        Me.imgListe.Images.SetKeyName(13, "xyDossier fermé (arborescence).ico")
        Me.imgListe.Images.SetKeyName(14, "xyDossier ouvert (arborescence).ico")
        Me.imgListe.Images.SetKeyName(15, "Cle")
        Me.imgListe.Images.SetKeyName(16, "Cles")
        Me.imgListe.Images.SetKeyName(17, "xyDeverrouiller")
        Me.imgListe.Images.SetKeyName(18, "xyImporter.ico")
        Me.imgListe.Images.SetKeyName(19, "xyExporter.ico")
        '
        'lblProfil
        '
        Me.lblProfil.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProfil.Location = New System.Drawing.Point(12, 91)
        Me.lblProfil.Name = "lblProfil"
        Me.lblProfil.Size = New System.Drawing.Size(100, 16)
        Me.lblProfil.TabIndex = 82
        Me.lblProfil.Text = "Profil"
        '
        'lblMdp
        '
        Me.lblMdp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMdp.Location = New System.Drawing.Point(12, 61)
        Me.lblMdp.Name = "lblMdp"
        Me.lblMdp.Size = New System.Drawing.Size(122, 22)
        Me.lblMdp.TabIndex = 81
        Me.lblMdp.Text = "Mot de passe"
        '
        'lblCode
        '
        Me.lblCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCode.Location = New System.Drawing.Point(12, 34)
        Me.lblCode.Name = "lblCode"
        Me.lblCode.Size = New System.Drawing.Size(100, 16)
        Me.lblCode.TabIndex = 80
        Me.lblCode.Text = "Code"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 5)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(122, 21)
        Me.Label1.TabIndex = 83
        Me.Label1.Text = "Environnement"
        '
        'cboEnv
        '
        Me.cboEnv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboEnv.FormattingEnabled = True
        Me.cboEnv.GererTabCommeEnter = False
        Me.cboEnv.Location = New System.Drawing.Point(140, 5)
        Me.cboEnv.Name = "cboEnv"
        Me.cboEnv.NomChampsDonnee = Nothing
        Me.cboEnv.NomSourceDonnee = Nothing
        Me.cboEnv.Size = New System.Drawing.Size(248, 25)
        Me.cboEnv.TabIndex = 1
        '
        'cmdAnnuler
        '
        Me.cmdAnnuler.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdAnnuler.Location = New System.Drawing.Point(313, 123)
        Me.cmdAnnuler.Name = "cmdAnnuler"
        Me.cmdAnnuler.Size = New System.Drawing.Size(75, 27)
        Me.cmdAnnuler.TabIndex = 8
        Me.cmdAnnuler.Text = "Annuler"
        '
        'cmdOk
        '
        Me.cmdOk.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdOk.Location = New System.Drawing.Point(231, 123)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.Size = New System.Drawing.Size(75, 27)
        Me.cmdOk.TabIndex = 7
        Me.cmdOk.Text = "Ok"
        '
        'cmdMontrerPassword
        '
        Me.cmdMontrerPassword.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdMontrerPassword.ImageKey = "xyDeverrouiller"
        Me.cmdMontrerPassword.ImageList = Me.imgListe
        Me.cmdMontrerPassword.Location = New System.Drawing.Point(362, 61)
        Me.cmdMontrerPassword.Name = "cmdMontrerPassword"
        Me.cmdMontrerPassword.Size = New System.Drawing.Size(26, 26)
        Me.cmdMontrerPassword.TabIndex = 4
        '
        'txtMdp
        '
        Me.txtMdp.Location = New System.Drawing.Point(140, 61)
        Me.txtMdp.MaxLength = 62
        Me.txtMdp.Name = "txtMdp"
        Me.txtMdp.NomChampsDonnee = Nothing
        Me.txtMdp.NomSourceDonnee = Nothing
        Me.txtMdp.Size = New System.Drawing.Size(216, 25)
        Me.txtMdp.TabIndex = 3
        Me.txtMdp.UseSystemPasswordChar = True
        '
        'cmdGenererMdp
        '
        Me.cmdGenererMdp.ImageIndex = 15
        Me.cmdGenererMdp.ImageList = Me.imgListe
        Me.cmdGenererMdp.Location = New System.Drawing.Point(362, 34)
        Me.cmdGenererMdp.Name = "cmdGenererMdp"
        Me.cmdGenererMdp.Size = New System.Drawing.Size(26, 25)
        Me.cmdGenererMdp.TabIndex = 84
        Me.cmdGenererMdp.UseVisualStyleBackColor = True
        '
        'TsFgMultiEnv
        '
        Me.AcceptButton = Me.cmdOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.cmdAnnuler
        Me.ClientSize = New System.Drawing.Size(393, 152)
        Me.Controls.Add(Me.cmdGenererMdp)
        Me.Controls.Add(Me.txtMdp)
        Me.Controls.Add(Me.cmdMontrerPassword)
        Me.Controls.Add(Me.cmdAnnuler)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.cboEnv)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtProfil)
        Me.Controls.Add(Me.txtCode)
        Me.Controls.Add(Me.cmdProfil)
        Me.Controls.Add(Me.lblProfil)
        Me.Controls.Add(Me.lblMdp)
        Me.Controls.Add(Me.lblCode)
        Me.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "TsFgMultiEnv"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Valeur par environnement"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtProfil As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTextbox
    Friend WithEvents txtCode As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTextbox
    Friend WithEvents cmdProfil As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton
    Friend WithEvents lblProfil As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel
    Friend WithEvents lblMdp As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel
    Friend WithEvents lblCode As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel
    Friend WithEvents Label1 As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel
    Friend WithEvents cboEnv As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XzCrComboBox
    Friend WithEvents imgListe As System.Windows.Forms.ImageList
    Friend WithEvents cmdAnnuler As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton
    Friend WithEvents cmdOk As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton
    Friend WithEvents cmdMontrerPassword As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton
    Friend WithEvents txtMdp As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTextbox
    Friend WithEvents cmdGenererMdp As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton
End Class
