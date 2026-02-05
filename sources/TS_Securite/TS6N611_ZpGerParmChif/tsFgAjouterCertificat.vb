Public Class tsFgAjouterCertificat
    Inherits System.Windows.Forms.Form

#Region " Code généré par le Concepteur Windows Form "

    Public Sub New()
        MyBase.New()

        'Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        'Ajoutez une initialisation quelconque après l'appel InitializeComponent()

    End Sub

    'La méthode substituée Dispose du formulaire pour nettoyer la liste des composants.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Requis par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée en utilisant le Concepteur Windows Form.  
    'Ne la modifiez pas en utilisant l'éditeur de code.
    Friend WithEvents chkGenrrAutom As System.Windows.Forms.CheckBox
    Friend WithEvents chkActif As System.Windows.Forms.CheckBox
    Friend WithEvents txtCode As System.Windows.Forms.TextBox
    Friend WithEvents lblCode As System.Windows.Forms.Label
    Friend WithEvents cmdAnnuler As System.Windows.Forms.Button
    Friend WithEvents cmdAjouter As System.Windows.Forms.Button
    Friend WithEvents txtNomMagasin As System.Windows.Forms.TextBox
    Friend WithEvents lblNomMagasin As System.Windows.Forms.Label
    Friend WithEvents txtIdCertificat As System.Windows.Forms.TextBox
    Friend WithEvents lblIdCertificat As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.chkGenrrAutom = New System.Windows.Forms.CheckBox
        Me.cmdAnnuler = New System.Windows.Forms.Button
        Me.cmdAjouter = New System.Windows.Forms.Button
        Me.chkActif = New System.Windows.Forms.CheckBox
        Me.txtCode = New System.Windows.Forms.TextBox
        Me.lblCode = New System.Windows.Forms.Label
        Me.txtNomMagasin = New System.Windows.Forms.TextBox
        Me.lblNomMagasin = New System.Windows.Forms.Label
        Me.txtIdCertificat = New System.Windows.Forms.TextBox
        Me.lblIdCertificat = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'chkGenrrAutom
        '
        Me.chkGenrrAutom.Checked = True
        Me.chkGenrrAutom.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkGenrrAutom.Location = New System.Drawing.Point(112, 24)
        Me.chkGenrrAutom.Name = "chkGenrrAutom"
        Me.chkGenrrAutom.Size = New System.Drawing.Size(168, 24)
        Me.chkGenrrAutom.TabIndex = 21
        Me.chkGenrrAutom.Text = "Générer automatiquement"
        '
        'cmdAnnuler
        '
        Me.cmdAnnuler.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdAnnuler.Location = New System.Drawing.Point(272, 176)
        Me.cmdAnnuler.Name = "cmdAnnuler"
        Me.cmdAnnuler.TabIndex = 20
        Me.cmdAnnuler.Text = "Annuler"
        '
        'cmdAjouter
        '
        Me.cmdAjouter.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdAjouter.Location = New System.Drawing.Point(352, 176)
        Me.cmdAjouter.Name = "cmdAjouter"
        Me.cmdAjouter.TabIndex = 19
        Me.cmdAjouter.Text = "Ajouter"
        '
        'chkActif
        '
        Me.chkActif.Checked = True
        Me.chkActif.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkActif.Location = New System.Drawing.Point(8, 160)
        Me.chkActif.Name = "chkActif"
        Me.chkActif.TabIndex = 18
        Me.chkActif.Text = "Actif"
        '
        'txtCode
        '
        Me.txtCode.Enabled = False
        Me.txtCode.Location = New System.Drawing.Point(8, 24)
        Me.txtCode.Name = "txtCode"
        Me.txtCode.TabIndex = 13
        Me.txtCode.Text = ""
        '
        'lblCode
        '
        Me.lblCode.Location = New System.Drawing.Point(8, 8)
        Me.lblCode.Name = "lblCode"
        Me.lblCode.Size = New System.Drawing.Size(88, 16)
        Me.lblCode.TabIndex = 12
        Me.lblCode.Text = "Code"
        '
        'txtNomMagasin
        '
        Me.txtNomMagasin.Location = New System.Drawing.Point(8, 128)
        Me.txtNomMagasin.Multiline = True
        Me.txtNomMagasin.Name = "txtNomMagasin"
        Me.txtNomMagasin.Size = New System.Drawing.Size(416, 20)
        Me.txtNomMagasin.TabIndex = 27
        Me.txtNomMagasin.Text = ""
        '
        'lblNomMagasin
        '
        Me.lblNomMagasin.Location = New System.Drawing.Point(8, 112)
        Me.lblNomMagasin.Name = "lblNomMagasin"
        Me.lblNomMagasin.Size = New System.Drawing.Size(416, 16)
        Me.lblNomMagasin.TabIndex = 26
        Me.lblNomMagasin.Text = "Nom du magasin où le certiftcat est installé"
        '
        'txtIdCertificat
        '
        Me.txtIdCertificat.Location = New System.Drawing.Point(8, 72)
        Me.txtIdCertificat.Multiline = True
        Me.txtIdCertificat.Name = "txtIdCertificat"
        Me.txtIdCertificat.Size = New System.Drawing.Size(416, 32)
        Me.txtIdCertificat.TabIndex = 25
        Me.txtIdCertificat.Text = ""
        '
        'lblIdCertificat
        '
        Me.lblIdCertificat.Location = New System.Drawing.Point(8, 56)
        Me.lblIdCertificat.Name = "lblIdCertificat"
        Me.lblIdCertificat.Size = New System.Drawing.Size(416, 16)
        Me.lblIdCertificat.TabIndex = 24
        Me.lblIdCertificat.Text = "Id Certificat"
        '
        'tsFgAjouterCertificat
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(432, 205)
        Me.Controls.Add(Me.txtNomMagasin)
        Me.Controls.Add(Me.lblNomMagasin)
        Me.Controls.Add(Me.txtIdCertificat)
        Me.Controls.Add(Me.lblIdCertificat)
        Me.Controls.Add(Me.chkGenrrAutom)
        Me.Controls.Add(Me.cmdAnnuler)
        Me.Controls.Add(Me.cmdAjouter)
        Me.Controls.Add(Me.chkActif)
        Me.Controls.Add(Me.txtCode)
        Me.Controls.Add(Me.lblCode)
        Me.Name = "tsFgAjouterCertificat"
        Me.Text = "Ajouter un certificat"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private _CertificatRow As DataRow

    Public Property CertificatRow() As DataRow
        Get
            Return _CertificatRow
        End Get
        Set(ByVal Value As DataRow)
            _CertificatRow = Value
        End Set
    End Property

    Private Sub tsFgAjouterCleVecteur_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtCode.Text = CertificatRow.Item("Code").ToString
        txtIdCertificat.Text = CertificatRow.Item("IdCertificat").ToString
        txtNomMagasin.Text = CertificatRow.Item("NomMagasin").ToString
        chkActif.Checked = CType(CertificatRow.Item("Actif"), Boolean)
    End Sub

    Private Sub cmdAjouter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAjouter.Click
        CertificatRow.Item("Code") = txtCode.Text
        CertificatRow.Item("IdCertificat") = txtIdCertificat.Text
        CertificatRow.Item("NomMagasin") = txtNomMagasin.Text
        CertificatRow.Item("Actif") = chkActif.Checked
        Me.Close()
    End Sub

    Private Sub cmdAnnuler_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAnnuler.Click
        Me.Close()
    End Sub

    Private Sub chkGenrrAutom_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkGenrrAutom.CheckedChanged
        If chkGenrrAutom.Checked Then
            txtCode.Enabled = False
            txtCode.Text = ""
        Else
            txtCode.Enabled = True
            txtCode.Focus()
        End If
    End Sub

End Class
