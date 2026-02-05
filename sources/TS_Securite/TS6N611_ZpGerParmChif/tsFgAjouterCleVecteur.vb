Imports System.Collections.Generic
Imports TS6N631_ZpTrtParmChif
Imports TS6N631_ZpTrtParmChif.TsCuParamsChiffrement

Public Class tsFgAjouterCleVecteur
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
    Friend WithEvents cmdAnnuler As System.Windows.Forms.Button
    Friend WithEvents cmdAjouter As System.Windows.Forms.Button
    Friend WithEvents chkActif As System.Windows.Forms.CheckBox
    Friend WithEvents txtVecteur As System.Windows.Forms.TextBox
    Friend WithEvents lblVecteur As System.Windows.Forms.Label
    Friend WithEvents txtCle As System.Windows.Forms.TextBox
    Friend WithEvents lblCle As System.Windows.Forms.Label
    Friend WithEvents txtCode As System.Windows.Forms.TextBox
    Friend WithEvents lblNote1 As System.Windows.Forms.Label
    Friend WithEvents lblNote2 As System.Windows.Forms.Label
    Friend WithEvents lblCode As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.cmdAnnuler = New System.Windows.Forms.Button()
        Me.cmdAjouter = New System.Windows.Forms.Button()
        Me.chkActif = New System.Windows.Forms.CheckBox()
        Me.txtVecteur = New System.Windows.Forms.TextBox()
        Me.lblVecteur = New System.Windows.Forms.Label()
        Me.txtCle = New System.Windows.Forms.TextBox()
        Me.lblCle = New System.Windows.Forms.Label()
        Me.txtCode = New System.Windows.Forms.TextBox()
        Me.lblCode = New System.Windows.Forms.Label()
        Me.lblNote1 = New System.Windows.Forms.Label()
        Me.lblNote2 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'cmdAnnuler
        '
        Me.cmdAnnuler.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdAnnuler.Location = New System.Drawing.Point(311, 223)
        Me.cmdAnnuler.Name = "cmdAnnuler"
        Me.cmdAnnuler.Size = New System.Drawing.Size(75, 23)
        Me.cmdAnnuler.TabIndex = 30
        Me.cmdAnnuler.Text = "Annuler"
        '
        'cmdAjouter
        '
        Me.cmdAjouter.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdAjouter.Location = New System.Drawing.Point(391, 223)
        Me.cmdAjouter.Name = "cmdAjouter"
        Me.cmdAjouter.Size = New System.Drawing.Size(75, 23)
        Me.cmdAjouter.TabIndex = 29
        Me.cmdAjouter.Text = "Ajouter"
        '
        'chkActif
        '
        Me.chkActif.Checked = True
        Me.chkActif.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkActif.Location = New System.Drawing.Point(8, 219)
        Me.chkActif.Name = "chkActif"
        Me.chkActif.Size = New System.Drawing.Size(104, 24)
        Me.chkActif.TabIndex = 28
        Me.chkActif.Text = "Actif"
        '
        'txtVecteur
        '
        Me.txtVecteur.Location = New System.Drawing.Point(8, 187)
        Me.txtVecteur.Name = "txtVecteur"
        Me.txtVecteur.Size = New System.Drawing.Size(416, 20)
        Me.txtVecteur.TabIndex = 27
        '
        'lblVecteur
        '
        Me.lblVecteur.Location = New System.Drawing.Point(8, 170)
        Me.lblVecteur.Name = "lblVecteur"
        Me.lblVecteur.Size = New System.Drawing.Size(416, 16)
        Me.lblVecteur.TabIndex = 26
        Me.lblVecteur.Text = "Vecteur d'initialisation"
        '
        'txtCle
        '
        Me.txtCle.Location = New System.Drawing.Point(8, 115)
        Me.txtCle.Multiline = True
        Me.txtCle.Name = "txtCle"
        Me.txtCle.Size = New System.Drawing.Size(416, 48)
        Me.txtCle.TabIndex = 25
        '
        'lblCle
        '
        Me.lblCle.Location = New System.Drawing.Point(8, 98)
        Me.lblCle.Name = "lblCle"
        Me.lblCle.Size = New System.Drawing.Size(416, 16)
        Me.lblCle.TabIndex = 24
        Me.lblCle.Text = "Clé"
        '
        'txtCode
        '
        Me.txtCode.Location = New System.Drawing.Point(8, 33)
        Me.txtCode.Name = "txtCode"
        Me.txtCode.Size = New System.Drawing.Size(100, 20)
        Me.txtCode.TabIndex = 23
        '
        'lblCode
        '
        Me.lblCode.Location = New System.Drawing.Point(8, 15)
        Me.lblCode.Name = "lblCode"
        Me.lblCode.Size = New System.Drawing.Size(88, 16)
        Me.lblCode.TabIndex = 22
        Me.lblCode.Text = "Code"
        '
        'lblNote1
        '
        Me.lblNote1.Location = New System.Drawing.Point(114, 15)
        Me.lblNote1.Name = "lblNote1"
        Me.lblNote1.Size = New System.Drawing.Size(348, 49)
        Me.lblNote1.TabIndex = 31
        Me.lblNote1.Text = "Note - Un code numérique est considéré comme générique et sera attribué aléatoire" & _
    "ment lors des demandes de chiffrement sans code spécifique reçu en paramètre"
        '
        'lblNote2
        '
        Me.lblNote2.Location = New System.Drawing.Point(114, 66)
        Me.lblNote2.Name = "lblNote2"
        Me.lblNote2.Size = New System.Drawing.Size(348, 46)
        Me.lblNote2.TabIndex = 32
        Me.lblNote2.Text = "Note - Un code alphanumérique est considéré comme spécifique et ne sera utilisé q" & _
    "ue s'il est explicitement demandé lors des demandes de chiffrement"
        '
        'tsFgAjouterCleVecteur
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(474, 253)
        Me.Controls.Add(Me.lblNote2)
        Me.Controls.Add(Me.lblNote1)
        Me.Controls.Add(Me.cmdAnnuler)
        Me.Controls.Add(Me.cmdAjouter)
        Me.Controls.Add(Me.chkActif)
        Me.Controls.Add(Me.txtVecteur)
        Me.Controls.Add(Me.lblVecteur)
        Me.Controls.Add(Me.txtCle)
        Me.Controls.Add(Me.lblCle)
        Me.Controls.Add(Me.txtCode)
        Me.Controls.Add(Me.lblCode)
        Me.Name = "tsFgAjouterCleVecteur"
        Me.Text = "Ajouter une Clé et un Vecteur d'intialisation"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Public Property CleVecteurRow As DataRow
    Public Property CleVecteurAllRows As DataRow()
    Public Property GestFichierChif As IGestFichierChif
    Public Property TypeCle As TypeCle
    Public Property TypeFichier As TypeFichier
    Public Property Envi As String

    Private Sub tsFgAjouterCleVecteur_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        txtCode.Text = CleVecteurRow.Item("Code").ToString
        txtCle.Text = CleVecteurRow.Item("Cle").ToString
        txtVecteur.Text = CleVecteurRow.Item("Vecteur").ToString
        chkActif.Checked = CType(CleVecteurRow.Item("Actif"), Boolean)

    End Sub

    Private Sub cmdAjouter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAjouter.Click
        If ValiderCle() Then
            CleVecteurRow.Item("Code") = txtCode.Text
            CleVecteurRow.Item("Cle") = txtCle.Text
            CleVecteurRow.Item("Vecteur") = txtVecteur.Text
            CleVecteurRow.Item("Actif") = chkActif.Checked
            Me.Close()
        Else
            Me.DialogResult = Windows.Forms.DialogResult.None
        End If
    End Sub

    Private Sub cmdAnnuler_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAnnuler.Click
        Me.Close()
    End Sub

    Private Function ValiderCle() As Boolean

        Dim estValide As Boolean = True
        Dim message As String = "Il est interdit de créer un Code de clé en double {0}"

        Dim liste As New List(Of DataRow)
        liste.AddRange(CleVecteurAllRows)

        If String.IsNullOrEmpty(txtCode.Text) Then
            MessageBox.Show("Le Code de clé est obligatoire", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If

        If GestFichierChif.CodeExiste(txtCode.Text, TypeCle, TypeFichier) Then
            estValide = False
            message = String.Format(message, "")
        Else
            For Each unGestFichier As IGestFichierChif In GestFichierChif.ListeDependances
                If unGestFichier.CodeExiste(txtCode.Text, TypeCle, TypeFichier) Then
                    estValide = False
                    message = String.Format(message, Chr(13) & Chr(10) & "Le code existe déjà dans l'environnement " & unGestFichier.Environnement.ToString)
                    Exit For
                End If
            Next
        End If

        'Valider que le code n'existe pas déjà dans la liste actuelle
        If Not estValide Then
            MessageBox.Show(message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If

        Return True

    End Function

End Class
