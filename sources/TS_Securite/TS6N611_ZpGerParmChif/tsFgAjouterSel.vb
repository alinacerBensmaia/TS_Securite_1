Public Class tsFgAjouterSel
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
    Friend WithEvents cmdAnnuler As System.Windows.Forms.Button
    Friend WithEvents cmdAjouter As System.Windows.Forms.Button
    Friend WithEvents chkActif As System.Windows.Forms.CheckBox
    Friend WithEvents txtCode As System.Windows.Forms.TextBox
    Friend WithEvents lblCode As System.Windows.Forms.Label
    Friend WithEvents txtSel As System.Windows.Forms.TextBox
    Friend WithEvents lblSel As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.chkGenrrAutom = New System.Windows.Forms.CheckBox
        Me.cmdAnnuler = New System.Windows.Forms.Button
        Me.cmdAjouter = New System.Windows.Forms.Button
        Me.chkActif = New System.Windows.Forms.CheckBox
        Me.txtSel = New System.Windows.Forms.TextBox
        Me.lblSel = New System.Windows.Forms.Label
        Me.txtCode = New System.Windows.Forms.TextBox
        Me.lblCode = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'chkGenrrAutom
        '
        Me.chkGenrrAutom.Checked = True
        Me.chkGenrrAutom.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkGenrrAutom.Location = New System.Drawing.Point(112, 24)
        Me.chkGenrrAutom.Name = "chkGenrrAutom"
        Me.chkGenrrAutom.Size = New System.Drawing.Size(168, 24)
        Me.chkGenrrAutom.TabIndex = 31
        Me.chkGenrrAutom.Text = "Générer automatiquement"
        '
        'cmdAnnuler
        '
        Me.cmdAnnuler.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdAnnuler.Location = New System.Drawing.Point(272, 128)
        Me.cmdAnnuler.Name = "cmdAnnuler"
        Me.cmdAnnuler.TabIndex = 30
        Me.cmdAnnuler.Text = "Annuler"
        '
        'cmdAjouter
        '
        Me.cmdAjouter.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdAjouter.Location = New System.Drawing.Point(352, 128)
        Me.cmdAjouter.Name = "cmdAjouter"
        Me.cmdAjouter.TabIndex = 29
        Me.cmdAjouter.Text = "Ajouter"
        '
        'chkActif
        '
        Me.chkActif.Checked = True
        Me.chkActif.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkActif.Location = New System.Drawing.Point(8, 112)
        Me.chkActif.Name = "chkActif"
        Me.chkActif.TabIndex = 28
        Me.chkActif.Text = "Actif"
        '
        'txtSel
        '
        Me.txtSel.Location = New System.Drawing.Point(8, 72)
        Me.txtSel.Multiline = True
        Me.txtSel.Name = "txtSel"
        Me.txtSel.Size = New System.Drawing.Size(416, 32)
        Me.txtSel.TabIndex = 25
        Me.txtSel.Text = ""
        '
        'lblSel
        '
        Me.lblSel.Location = New System.Drawing.Point(8, 56)
        Me.lblSel.Name = "lblSel"
        Me.lblSel.Size = New System.Drawing.Size(416, 16)
        Me.lblSel.TabIndex = 24
        Me.lblSel.Text = "Sel"
        '
        'txtCode
        '
        Me.txtCode.Enabled = False
        Me.txtCode.Location = New System.Drawing.Point(8, 24)
        Me.txtCode.Name = "txtCode"
        Me.txtCode.TabIndex = 23
        Me.txtCode.Text = ""
        '
        'lblCode
        '
        Me.lblCode.Location = New System.Drawing.Point(8, 8)
        Me.lblCode.Name = "lblCode"
        Me.lblCode.Size = New System.Drawing.Size(88, 16)
        Me.lblCode.TabIndex = 22
        Me.lblCode.Text = "Code"
        '
        'tsFgAjouterSel
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(432, 157)
        Me.Controls.Add(Me.chkGenrrAutom)
        Me.Controls.Add(Me.cmdAnnuler)
        Me.Controls.Add(Me.cmdAjouter)
        Me.Controls.Add(Me.chkActif)
        Me.Controls.Add(Me.txtSel)
        Me.Controls.Add(Me.lblSel)
        Me.Controls.Add(Me.txtCode)
        Me.Controls.Add(Me.lblCode)
        Me.Name = "tsFgAjouterSel"
        Me.Text = "tsFgAjouterSel"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private _SelRow As DataRow

    Public Property SelRow() As DataRow
        Get
            Return _SelRow
        End Get
        Set(ByVal Value As DataRow)
            _SelRow = Value
        End Set
    End Property

    Private Sub tsFgAjouterCleVecteur_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtCode.Text = SelRow.Item("Code").ToString
        txtSel.Text = SelRow.Item("Sel").ToString
        chkActif.Checked = CType(SelRow.Item("Actif"), Boolean)
    End Sub

    Private Sub cmdAjouter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAjouter.Click
        SelRow.Item("Code") = txtCode.Text
        SelRow.Item("Sel") = txtSel.Text
        SelRow.Item("Actif") = chkActif.Checked
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
