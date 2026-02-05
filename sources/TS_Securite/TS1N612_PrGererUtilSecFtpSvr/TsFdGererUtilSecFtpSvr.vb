Imports Microsoft
Imports System.Net.Sockets
Imports Rrq.InfrastructureCommune.Parametres
Imports System.Collections.Generic
Imports TS1N621_INiveauSecrt1
Imports Rrq.InfrastructureCommune.ScenarioTransactionnel

Public Class TsFdGererUtilSecFtpSvr
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
    Friend WithEvents tlbGererUtils As System.Windows.Forms.ToolBar
    Friend WithEvents imlGererUtils As System.Windows.Forms.ImageList
    Friend WithEvents cmdNouveau As System.Windows.Forms.ToolBarButton
    Friend WithEvents cmdEnregistrer As System.Windows.Forms.ToolBarButton
    Friend WithEvents cmdAnnuler As System.Windows.Forms.ToolBarButton
    Friend WithEvents cmdDeverrouiller As System.Windows.Forms.ToolBarButton
    Friend WithEvents cmdSupprimer As System.Windows.Forms.ToolBarButton
    Friend WithEvents tbSeparateur1 As System.Windows.Forms.ToolBarButton
    Friend WithEvents tbSeparateur2 As System.Windows.Forms.ToolBarButton
    Friend WithEvents tbSeparateur3 As System.Windows.Forms.ToolBarButton
    Friend WithEvents tbSeparateur4 As System.Windows.Forms.ToolBarButton
    Friend WithEvents tbSeparateur5 As System.Windows.Forms.ToolBarButton
    Friend WithEvents lblPoint1 As System.Windows.Forms.Label
    Friend WithEvents lblPoint2 As System.Windows.Forms.Label
    Friend WithEvents lblPoint3 As System.Windows.Forms.Label
    Friend WithEvents optPartn As System.Windows.Forms.RadioButton
    Friend WithEvents optEmplRRQ As System.Windows.Forms.RadioButton
    Friend WithEvents txtIP4 As System.Windows.Forms.TextBox
    Friend WithEvents txtIP3 As System.Windows.Forms.TextBox
    Friend WithEvents txtIP2 As System.Windows.Forms.TextBox
    Friend WithEvents txtIP1 As System.Windows.Forms.TextBox
    Friend WithEvents txtAbrevClient As System.Windows.Forms.TextBox
    Friend WithEvents lblIP As System.Windows.Forms.Label
    Friend WithEvents chkClearFTP As System.Windows.Forms.CheckBox
    Friend WithEvents lblAbrevClient As System.Windows.Forms.Label
    Friend WithEvents pnlAjout As System.Windows.Forms.Panel
    Friend WithEvents pnlSelect As System.Windows.Forms.Panel
    Friend WithEvents lblCodeUtil As System.Windows.Forms.Label
    Friend WithEvents lvwCodeUtil As System.Windows.Forms.ListView
    Friend WithEvents CodeUtil As System.Windows.Forms.ColumnHeader
    Friend WithEvents chkEnvUnit As System.Windows.Forms.CheckBox
    Friend WithEvents chkEnvProd As System.Windows.Forms.CheckBox
    Friend WithEvents chkEnvAccp As System.Windows.Forms.CheckBox
    Friend WithEvents chkEnvIntg As System.Windows.Forms.CheckBox
    Friend WithEvents chkCompteAvecFtp As System.Windows.Forms.CheckBox
    Friend WithEvents optPartnIndiv As System.Windows.Forms.RadioButton
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TsFdGererUtilSecFtpSvr))
        Me.tlbGererUtils = New System.Windows.Forms.ToolBar()
        Me.cmdNouveau = New System.Windows.Forms.ToolBarButton()
        Me.cmdEnregistrer = New System.Windows.Forms.ToolBarButton()
        Me.cmdAnnuler = New System.Windows.Forms.ToolBarButton()
        Me.tbSeparateur1 = New System.Windows.Forms.ToolBarButton()
        Me.tbSeparateur2 = New System.Windows.Forms.ToolBarButton()
        Me.tbSeparateur3 = New System.Windows.Forms.ToolBarButton()
        Me.tbSeparateur4 = New System.Windows.Forms.ToolBarButton()
        Me.tbSeparateur5 = New System.Windows.Forms.ToolBarButton()
        Me.cmdDeverrouiller = New System.Windows.Forms.ToolBarButton()
        Me.cmdSupprimer = New System.Windows.Forms.ToolBarButton()
        Me.imlGererUtils = New System.Windows.Forms.ImageList(Me.components)
        Me.lblPoint1 = New System.Windows.Forms.Label()
        Me.lblPoint2 = New System.Windows.Forms.Label()
        Me.lblPoint3 = New System.Windows.Forms.Label()
        Me.pnlAjout = New System.Windows.Forms.Panel()
        Me.optPartnIndiv = New System.Windows.Forms.RadioButton()
        Me.optPartn = New System.Windows.Forms.RadioButton()
        Me.optEmplRRQ = New System.Windows.Forms.RadioButton()
        Me.txtIP4 = New System.Windows.Forms.TextBox()
        Me.txtIP3 = New System.Windows.Forms.TextBox()
        Me.txtIP2 = New System.Windows.Forms.TextBox()
        Me.txtIP1 = New System.Windows.Forms.TextBox()
        Me.txtAbrevClient = New System.Windows.Forms.TextBox()
        Me.lblIP = New System.Windows.Forms.Label()
        Me.chkClearFTP = New System.Windows.Forms.CheckBox()
        Me.lblAbrevClient = New System.Windows.Forms.Label()
        Me.pnlSelect = New System.Windows.Forms.Panel()
        Me.lvwCodeUtil = New System.Windows.Forms.ListView()
        Me.CodeUtil = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lblCodeUtil = New System.Windows.Forms.Label()
        Me.chkEnvUnit = New System.Windows.Forms.CheckBox()
        Me.chkEnvIntg = New System.Windows.Forms.CheckBox()
        Me.chkEnvAccp = New System.Windows.Forms.CheckBox()
        Me.chkEnvProd = New System.Windows.Forms.CheckBox()
        Me.chkCompteAvecFtp = New System.Windows.Forms.CheckBox()
        Me.pnlAjout.SuspendLayout()
        Me.pnlSelect.SuspendLayout()
        Me.SuspendLayout()
        '
        'tlbGererUtils
        '
        Me.tlbGererUtils.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.cmdNouveau, Me.cmdEnregistrer, Me.cmdAnnuler, Me.tbSeparateur1, Me.tbSeparateur2, Me.tbSeparateur3, Me.tbSeparateur4, Me.tbSeparateur5, Me.cmdDeverrouiller, Me.cmdSupprimer})
        Me.tlbGererUtils.DropDownArrows = True
        Me.tlbGererUtils.ImageList = Me.imlGererUtils
        Me.tlbGererUtils.Location = New System.Drawing.Point(0, 0)
        Me.tlbGererUtils.Name = "tlbGererUtils"
        Me.tlbGererUtils.ShowToolTips = True
        Me.tlbGererUtils.Size = New System.Drawing.Size(389, 28)
        Me.tlbGererUtils.TabIndex = 5
        '
        'cmdNouveau
        '
        Me.cmdNouveau.ImageIndex = 0
        Me.cmdNouveau.Name = "cmdNouveau"
        Me.cmdNouveau.ToolTipText = "Nouveau"
        '
        'cmdEnregistrer
        '
        Me.cmdEnregistrer.ImageIndex = 1
        Me.cmdEnregistrer.Name = "cmdEnregistrer"
        Me.cmdEnregistrer.ToolTipText = "Enregistrer"
        '
        'cmdAnnuler
        '
        Me.cmdAnnuler.ImageIndex = 2
        Me.cmdAnnuler.Name = "cmdAnnuler"
        Me.cmdAnnuler.ToolTipText = "Annuler"
        '
        'tbSeparateur1
        '
        Me.tbSeparateur1.Name = "tbSeparateur1"
        Me.tbSeparateur1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'tbSeparateur2
        '
        Me.tbSeparateur2.Name = "tbSeparateur2"
        Me.tbSeparateur2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'tbSeparateur3
        '
        Me.tbSeparateur3.Name = "tbSeparateur3"
        Me.tbSeparateur3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'tbSeparateur4
        '
        Me.tbSeparateur4.Name = "tbSeparateur4"
        Me.tbSeparateur4.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'tbSeparateur5
        '
        Me.tbSeparateur5.Name = "tbSeparateur5"
        Me.tbSeparateur5.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'cmdDeverrouiller
        '
        Me.cmdDeverrouiller.ImageIndex = 4
        Me.cmdDeverrouiller.Name = "cmdDeverrouiller"
        Me.cmdDeverrouiller.ToolTipText = "Déverrouiller"
        '
        'cmdSupprimer
        '
        Me.cmdSupprimer.Enabled = False
        Me.cmdSupprimer.ImageIndex = 3
        Me.cmdSupprimer.Name = "cmdSupprimer"
        Me.cmdSupprimer.ToolTipText = "Supprimer"
        '
        'imlGererUtils
        '
        Me.imlGererUtils.ImageStream = CType(resources.GetObject("imlGererUtils.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imlGererUtils.TransparentColor = System.Drawing.Color.Transparent
        Me.imlGererUtils.Images.SetKeyName(0, "")
        Me.imlGererUtils.Images.SetKeyName(1, "")
        Me.imlGererUtils.Images.SetKeyName(2, "")
        Me.imlGererUtils.Images.SetKeyName(3, "")
        Me.imlGererUtils.Images.SetKeyName(4, "")
        '
        'lblPoint1
        '
        Me.lblPoint1.Location = New System.Drawing.Point(150, 219)
        Me.lblPoint1.Name = "lblPoint1"
        Me.lblPoint1.Size = New System.Drawing.Size(9, 19)
        Me.lblPoint1.TabIndex = 15
        Me.lblPoint1.Text = "."
        '
        'lblPoint2
        '
        Me.lblPoint2.Location = New System.Drawing.Point(206, 219)
        Me.lblPoint2.Name = "lblPoint2"
        Me.lblPoint2.Size = New System.Drawing.Size(10, 19)
        Me.lblPoint2.TabIndex = 16
        Me.lblPoint2.Text = "."
        '
        'lblPoint3
        '
        Me.lblPoint3.Location = New System.Drawing.Point(265, 219)
        Me.lblPoint3.Name = "lblPoint3"
        Me.lblPoint3.Size = New System.Drawing.Size(10, 19)
        Me.lblPoint3.TabIndex = 17
        Me.lblPoint3.Text = "."
        '
        'pnlAjout
        '
        Me.pnlAjout.Controls.Add(Me.chkEnvProd)
        Me.pnlAjout.Controls.Add(Me.chkEnvAccp)
        Me.pnlAjout.Controls.Add(Me.chkEnvIntg)
        Me.pnlAjout.Controls.Add(Me.chkEnvUnit)
        Me.pnlAjout.Controls.Add(Me.chkCompteAvecFtp)
        Me.pnlAjout.Controls.Add(Me.optPartnIndiv)
        Me.pnlAjout.Controls.Add(Me.lblPoint3)
        Me.pnlAjout.Controls.Add(Me.optPartn)
        Me.pnlAjout.Controls.Add(Me.lblPoint2)
        Me.pnlAjout.Controls.Add(Me.lblPoint1)
        Me.pnlAjout.Controls.Add(Me.optEmplRRQ)
        Me.pnlAjout.Controls.Add(Me.txtIP4)
        Me.pnlAjout.Controls.Add(Me.txtIP3)
        Me.pnlAjout.Controls.Add(Me.txtIP2)
        Me.pnlAjout.Controls.Add(Me.txtIP1)
        Me.pnlAjout.Controls.Add(Me.txtAbrevClient)
        Me.pnlAjout.Controls.Add(Me.lblIP)
        Me.pnlAjout.Controls.Add(Me.chkClearFTP)
        Me.pnlAjout.Controls.Add(Me.lblAbrevClient)
        Me.pnlAjout.Location = New System.Drawing.Point(12, 34)
        Me.pnlAjout.Name = "pnlAjout"
        Me.pnlAjout.Size = New System.Drawing.Size(355, 259)
        Me.pnlAjout.TabIndex = 20
        '
        'optPartnIndiv
        '
        Me.optPartnIndiv.Location = New System.Drawing.Point(10, 37)
        Me.optPartnIndiv.Name = "optPartnIndiv"
        Me.optPartnIndiv.Size = New System.Drawing.Size(211, 18)
        Me.optPartnIndiv.TabIndex = 18
        Me.optPartnIndiv.Text = "Clientèle externe (Internaute)"
        '
        'chkEnvIntg
        '
        Me.chkCompteAvecFtp.AutoSize = True
        Me.chkCompteAvecFtp.Location = New System.Drawing.Point(205, 37)
        Me.chkCompteAvecFtp.Name = "chkAvecCompteFtp"
        Me.chkCompteAvecFtp.Size = New System.Drawing.Size(97, 21)
        Me.chkCompteAvecFtp.TabIndex = 29
        Me.chkCompteAvecFtp.Text = "Avec Compte FTP"
        Me.chkCompteAvecFtp.UseVisualStyleBackColor = True
        '
        'optPartn
        '
        Me.optPartn.Location = New System.Drawing.Point(10, 65)
        Me.optPartn.Name = "optPartn"
        Me.optPartn.Size = New System.Drawing.Size(153, 18)
        Me.optPartn.TabIndex = 19
        Me.optPartn.Text = "Partenaire système"
        '
        'optEmplRRQ
        '
        Me.optEmplRRQ.Checked = True
        Me.optEmplRRQ.Location = New System.Drawing.Point(10, 9)
        Me.optEmplRRQ.Name = "optEmplRRQ"
        Me.optEmplRRQ.Size = New System.Drawing.Size(230, 19)
        Me.optEmplRRQ.TabIndex = 17
        Me.optEmplRRQ.TabStop = True
        Me.optEmplRRQ.Text = "Clientèle interne (Employé RRQ)"
        '
        'txtIP4
        '
        Me.txtIP4.Location = New System.Drawing.Point(279, 216)
        Me.txtIP4.MaxLength = 3
        Me.txtIP4.Name = "txtIP4"
        Me.txtIP4.Size = New System.Drawing.Size(39, 22)
        Me.txtIP4.TabIndex = 27
        '
        'txtIP3
        '
        Me.txtIP3.Location = New System.Drawing.Point(222, 216)
        Me.txtIP3.MaxLength = 3
        Me.txtIP3.Name = "txtIP3"
        Me.txtIP3.Size = New System.Drawing.Size(38, 22)
        Me.txtIP3.TabIndex = 26
        '
        'txtIP2
        '
        Me.txtIP2.Location = New System.Drawing.Point(164, 216)
        Me.txtIP2.MaxLength = 3
        Me.txtIP2.Name = "txtIP2"
        Me.txtIP2.Size = New System.Drawing.Size(39, 22)
        Me.txtIP2.TabIndex = 25
        '
        'txtIP1
        '
        Me.txtIP1.Location = New System.Drawing.Point(107, 216)
        Me.txtIP1.MaxLength = 3
        Me.txtIP1.Name = "txtIP1"
        Me.txtIP1.Size = New System.Drawing.Size(38, 22)
        Me.txtIP1.TabIndex = 24
        '
        'txtAbrevClient
        '
        Me.txtAbrevClient.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAbrevClient.Location = New System.Drawing.Point(145, 151)
        Me.txtAbrevClient.Name = "txtAbrevClient"
        Me.txtAbrevClient.Size = New System.Drawing.Size(173, 22)
        Me.txtAbrevClient.TabIndex = 20
        '
        'lblIP
        '
        Me.lblIP.Location = New System.Drawing.Point(20, 216)
        Me.lblIP.Name = "lblIP"
        Me.lblIP.Size = New System.Drawing.Size(87, 18)
        Me.lblIP.TabIndex = 23
        Me.lblIP.Text = "Adresse IP :"
        '
        'chkClearFTP
        '
        Me.chkClearFTP.Location = New System.Drawing.Point(20, 188)
        Me.chkClearFTP.Name = "chkClearFTP"
        Me.chkClearFTP.Size = New System.Drawing.Size(307, 19)
        Me.chkClearFTP.TabIndex = 21
        Me.chkClearFTP.Text = "Permettre les connexions FTP non-sécurisées"
        '
        'lblAbrevClient
        '
        Me.lblAbrevClient.Location = New System.Drawing.Point(11, 151)
        Me.lblAbrevClient.Name = "lblAbrevClient"
        Me.lblAbrevClient.Size = New System.Drawing.Size(134, 19)
        Me.lblAbrevClient.TabIndex = 22
        Me.lblAbrevClient.Text = "Abréviation du client :"
        '
        'pnlSelect
        '
        Me.pnlSelect.Controls.Add(Me.lvwCodeUtil)
        Me.pnlSelect.Controls.Add(Me.lblCodeUtil)
        Me.pnlSelect.Location = New System.Drawing.Point(9, 34)
        Me.pnlSelect.Name = "pnlSelect"
        Me.pnlSelect.Size = New System.Drawing.Size(371, 273)
        Me.pnlSelect.TabIndex = 21
        '
        'lvwCodeUtil
        '
        Me.lvwCodeUtil.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.CodeUtil})
        Me.lvwCodeUtil.FullRowSelect = True
        Me.lvwCodeUtil.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lvwCodeUtil.HideSelection = False
        Me.lvwCodeUtil.Location = New System.Drawing.Point(11, 29)
        Me.lvwCodeUtil.MultiSelect = False
        Me.lvwCodeUtil.Name = "lvwCodeUtil"
        Me.lvwCodeUtil.Size = New System.Drawing.Size(348, 231)
        Me.lvwCodeUtil.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwCodeUtil.TabIndex = 25
        Me.lvwCodeUtil.UseCompatibleStateImageBehavior = False
        Me.lvwCodeUtil.View = System.Windows.Forms.View.Details
        '
        'CodeUtil
        '
        Me.CodeUtil.Text = "Code utilisateur"
        Me.CodeUtil.Width = 260
        '
        'lblCodeUtil
        '
        Me.lblCodeUtil.Location = New System.Drawing.Point(10, 9)
        Me.lblCodeUtil.Name = "lblCodeUtil"
        Me.lblCodeUtil.Size = New System.Drawing.Size(134, 19)
        Me.lblCodeUtil.TabIndex = 23
        Me.lblCodeUtil.Text = "Code de l'utilisateur"
        '
        'chkEnvUnit
        '
        Me.chkEnvUnit.AutoSize = True
        Me.chkEnvUnit.Location = New System.Drawing.Point(29, 94)
        Me.chkEnvUnit.Name = "chkEnvUnit"
        Me.chkEnvUnit.Size = New System.Drawing.Size(79, 21)
        Me.chkEnvUnit.TabIndex = 28
        Me.chkEnvUnit.Text = "Unitaire"
        Me.chkEnvUnit.UseVisualStyleBackColor = True
        '
        'chkEnvIntg
        '
        Me.chkEnvIntg.AutoSize = True
        Me.chkEnvIntg.Location = New System.Drawing.Point(164, 93)
        Me.chkEnvIntg.Name = "chkEnvIntg"
        Me.chkEnvIntg.Size = New System.Drawing.Size(97, 21)
        Me.chkEnvIntg.TabIndex = 29
        Me.chkEnvIntg.Text = "Intégration"
        Me.chkEnvIntg.UseVisualStyleBackColor = True
        '
        'chkEnvAccp
        '
        Me.chkEnvAccp.AutoSize = True
        Me.chkEnvAccp.Location = New System.Drawing.Point(29, 122)
        Me.chkEnvAccp.Name = "chkEnvAccp"
        Me.chkEnvAccp.Size = New System.Drawing.Size(104, 21)
        Me.chkEnvAccp.TabIndex = 30
        Me.chkEnvAccp.Text = "Acceptation"
        Me.chkEnvAccp.UseVisualStyleBackColor = True
        '
        'chkEnvProd
        '
        Me.chkEnvProd.AutoSize = True
        Me.chkEnvProd.Location = New System.Drawing.Point(164, 122)
        Me.chkEnvProd.Name = "chkEnvProd"
        Me.chkEnvProd.Size = New System.Drawing.Size(98, 21)
        Me.chkEnvProd.TabIndex = 31
        Me.chkEnvProd.Text = "Production"
        Me.chkEnvProd.UseVisualStyleBackColor = True
        '
        'TsFdGererUtilSecFtpSvr
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(389, 315)
        Me.Controls.Add(Me.pnlSelect)
        Me.Controls.Add(Me.pnlAjout)
        Me.Controls.Add(Me.tlbGererUtils)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "TsFdGererUtilSecFtpSvr"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Gestion des utilisateurs de Secure FTP Server"
        Me.pnlAjout.ResumeLayout(False)
        Me.pnlAjout.PerformLayout()
        Me.pnlSelect.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

#Region " Constantes, énumérations et variables globales "

    Private Enum TypeAction
        Ajout = 1
        Liste = 2
        Chargement = 3
    End Enum

    Private blnEnvUnit As Boolean
    Private blnEnvIntg As Boolean
    Private blnEnvAccp As Boolean
    Private blnEnvProd As Boolean
    Private strPrefixeUsager As String

#End Region

#Region " Méthodes associées au formulaire "
    Private Sub chkClearFTP_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkClearFTP.CheckedChanged
        If chkClearFTP.Checked Then
            lblIP.Enabled = True
            txtIP1.Enabled = True
            txtIP2.Enabled = True
            txtIP3.Enabled = True
            txtIP4.Enabled = True
            lblPoint1.Enabled = True
            lblPoint2.Enabled = True
            lblPoint3.Enabled = True
        Else
            lblIP.Enabled = False
            txtIP1.Enabled = False
            txtIP2.Enabled = False
            txtIP3.Enabled = False
            txtIP4.Enabled = False
            lblPoint1.Enabled = False
            lblPoint2.Enabled = False
            lblPoint3.Enabled = False
        End If
    End Sub

    Private Sub chkEnvUnit_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEnvUnit.CheckedChanged
        blnEnvUnit = chkEnvUnit.Checked
    End Sub

    Private Sub chkEnvIntg_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEnvIntg.CheckedChanged
        blnEnvIntg = chkEnvIntg.Checked
    End Sub

    Private Sub chkCompteFtp_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCompteAvecFtp.CheckedChanged
        blnEnvIntg = chkCompteAvecFtp.Checked
    End Sub

    Private Sub chkEnvAccp_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEnvAccp.CheckedChanged
        blnEnvAccp = chkEnvAccp.Checked
    End Sub

    Private Sub chkEnvProd_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEnvProd.CheckedChanged
        blnEnvProd = chkEnvProd.Checked
    End Sub

    Private Sub optEmplRRQ_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optEmplRRQ.CheckedChanged
        If optEmplRRQ.Checked Then
            chkClearFTP.Enabled = False
            chkCompteAvecFtp.Enabled = False
            strPrefixeUsager = "I"
        End If
    End Sub

    Private Sub optPartn_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optPartn.CheckedChanged
        If optPartn.Checked Then
            chkClearFTP.Enabled = True
            chkCompteAvecFtp.Enabled = False
            strPrefixeUsager = "Z"
        End If
    End Sub

    Private Sub optPartnIndiv_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optPartnIndiv.CheckedChanged
        If optPartnIndiv.Checked Then
            chkClearFTP.Enabled = False
            chkCompteAvecFtp.Enabled = True
            strPrefixeUsager = "E"
        End If
    End Sub

    Private Sub TsFdGererUtilSecFtpSvr_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'On initialise l'interface
        RemplirListe()

    End Sub

    Private Sub backgroundListe_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)

        Dim CaAffaire As TsICompI
        Dim resultat As List(Of String)
        Dim contexte As Object = Nothing

        Using objAppel As New XuCuAppelerCompI(Of TsICompI)
            Dim chaineContexte As String = objAppel.PreparerAppel(contexte, ObtenirCodeEnv())

            CaAffaire = objAppel.CreerComposantIntegration(chaineContexte)
            resultat = CaAffaire.ObtenirListeComptes(chaineContexte)
            objAppel.AnalyserRetour(chaineContexte, Nothing)
        End Using

        e.Result = resultat

    End Sub

    Private Sub backgroundListe_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)

        If (e.Error IsNot Nothing) Then
            MessageBox.Show(e.Error.ToString(), "Erreur", MessageBoxButtons.OK)
        Else

            Dim resultat As IList(Of String) = DirectCast(e.Result, IList(Of String))

            lvwCodeUtil.Items.Clear()

            For Each site As String In resultat
                lvwCodeUtil.Items.Add(site)
            Next

        End If

        InitialiserVisuel(TypeAction.Liste)

    End Sub

    Private Sub tlbGererUtils_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbGererUtils.ButtonClick
        Select Case tlbGererUtils.Buttons.IndexOf(e.Button)
            Case 0  'Nouveau
                InitialiserVisuel(TypeAction.Ajout)
            Case 1  'Enregistrer
                If txtAbrevClient.Text <> "" AndAlso (blnEnvAccp OrElse blnEnvIntg OrElse blnEnvProd OrElse blnEnvUnit) Then
                    If Enregistrer() Then
                        RemplirListe()
                    End If
                Else
                    MessageBox.Show("Vous devez saisir l'abréviation du client à créer et sélectionner au moins un environnement", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Case 2  'Annuler
                txtAbrevClient.Text = ""
                InitialiserVisuel(TypeAction.Liste)
            Case 8  'Déverrouiller
                If lvwCodeUtil.SelectedItems.Count = 0 Then
                    Return
                End If

                Deverrouiller()
                InitialiserVisuel(TypeAction.Liste)
            Case 9  'Supprimer
                If lvwCodeUtil.SelectedItems.Count = 0 Then
                    Return
                End If

                If MessageBox.Show("Êtes-vous sûr de vouloir supprimer l'utilisateur sélectionné ?", "Suppression", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                    Me.Cursor = Cursors.WaitCursor
                    If Supprimer() Then
                        RemplirListe()
                    End If
                    Me.Cursor = Cursors.Default
                End If
        End Select
    End Sub

    Private Sub txtIP1_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtIP1.KeyPress
        If Not IsNumeric(e.KeyChar) Then e.Handled = True
    End Sub

    Private Sub txtIP1_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtIP1.LostFocus
        txtIP1.TextAlign = HorizontalAlignment.Center
    End Sub

    Private Sub txtIP1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtIP1.TextChanged
        If txtIP1.Text.Length = 3 Then txtIP2.Focus()
    End Sub

    Private Sub txtIP2_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtIP2.KeyPress
        If Not IsNumeric(e.KeyChar) Then e.Handled = True
    End Sub

    Private Sub txtIP2_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtIP2.LostFocus
        txtIP2.TextAlign = HorizontalAlignment.Center
    End Sub

    Private Sub txtIP2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtIP2.TextChanged
        If txtIP2.Text.Length = 3 Then txtIP3.Focus()
    End Sub

    Private Sub txtIP3_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtIP3.KeyPress
        If Not IsNumeric(e.KeyChar) Then e.Handled = True
    End Sub

    Private Sub txtIP3_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtIP3.LostFocus
        txtIP3.TextAlign = HorizontalAlignment.Center
    End Sub

    Private Sub txtIP3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtIP3.TextChanged
        If txtIP3.Text.Length = 3 Then txtIP4.Focus()
    End Sub

    Private Sub txtIP4_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtIP4.KeyPress
        If Not IsNumeric(e.KeyChar) Then e.Handled = True
    End Sub

    Private Sub txtIP4_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtIP4.LostFocus
        txtIP4.TextAlign = HorizontalAlignment.Center
    End Sub

    Private Sub txtIP4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtIP4.TextChanged
        If txtIP4.Text.Length = 3 Then txtAbrevClient.Focus()
    End Sub
#End Region

#Region " Fonctions et procédures "

    ''' <summary>
    ''' Demande le déverrouillage du compte sélectionné
    ''' </summary>
    Private Sub Deverrouiller()

        Try

            Dim CaAffaire As TsICompI
            Dim contexte As Object = Nothing

            Using objAppel As New XuCuAppelerCompI(Of TsICompI)()
                Dim chaineContexte As String = objAppel.PreparerAppel(contexte, ObtenirCodeEnv())

                CaAffaire = objAppel.CreerComposantIntegration(chaineContexte)
                CaAffaire.DeverrouillerCompte(chaineContexte, lvwCodeUtil.SelectedItems.Item(0).Text)
                objAppel.AnalyserRetour(chaineContexte, Nothing)
            End Using

        Catch ex As Exception
            MessageBox.Show(ex.ToString, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    ''' <summary>
    ''' Effectue la création des comptes FTP spéficiés dans le formulaire, à la fin du traitement affiche une fenêtre résumé
    ''' </summary>
    ''' <returns>Indicateur de succès</returns>
    Private Function Enregistrer() As Boolean
        Dim listeCreation As List(Of TsDtInfoCleSymbolique) = Nothing

        Try
            'Debugger.Launch()
            Dim donneesCreation As New TsDtCreationComptes()

            With donneesCreation
                .InConFtpNonSec = chkClearFTP.Checked
                .InCreCompteFtp = chkCompteAvecFtp.Checked
                .InCreAcp = chkEnvAccp.Checked
                .InCreInt = chkEnvIntg.Checked
                .InCrePrd = chkEnvProd.Checked
                .InCreUni = chkEnvUnit.Checked
                .VlAbrCli = txtAbrevClient.Text
                .VlIpRac = txtIP1.Text & "." & txtIP2.Text & "." & txtIP3.Text & "." & txtIP4.Text
                .VlPfxUsg = strPrefixeUsager
            End With

            Dim CaAffaire As TsICompI
            Dim contexte As Object = Nothing

            Using objAppel As New XuCuAppelerCompI(Of TsICompI)()
                Dim chaineContexte As String = objAppel.PreparerAppel(contexte, ObtenirCodeEnv())

                CaAffaire = objAppel.CreerComposantIntegration(chaineContexte)
                listeCreation = CaAffaire.CreerCompteFtp(chaineContexte, donneesCreation)
                objAppel.AnalyserRetour(chaineContexte, Nothing)
            End Using

            Enregistrer = True

        Catch exFonc As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.XZCuErrValdtException
            MessageBox.Show(exFonc.MsgErreur.NumMessage, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Enregistrer = False
        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString(), "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Enregistrer = False
        End Try

        ' Si des comptes ont été créés, afficher la fenêtre de sommaire
        If listeCreation IsNot Nothing AndAlso listeCreation.Count > 0 Then
            Dim fenetreSommaire As New TsFfSommaireCreation(listeCreation)
            fenetreSommaire.ShowDialog()
        End If
    End Function

    ''' <summary>
    ''' Prépare la fenêtre pour afficher les champs pertinents, et dans l'état approprié, selon l'action qui en cours par l'utilisateur
    ''' Actions possible : Ajout, Liste et Chargement
    ''' </summary>
    ''' <param name="typAction">Type d'action en cours</param>
    Private Sub InitialiserVisuel(ByVal typAction As TypeAction)

        Select Case typAction

            Case TypeAction.Chargement 'Action = CHARGEMENT
                cmdNouveau.Enabled = False
                cmdEnregistrer.Enabled = False
                cmdAnnuler.Enabled = False
                cmdDeverrouiller.Enabled = False
                cmdSupprimer.Enabled = False

                pnlAjout.Visible = False
                pnlSelect.Visible = True

                optEmplRRQ.Checked = False
                optPartnIndiv.Checked = False
                optPartn.Checked = False

                chkCompteAvecFtp.Checked = False
                chkCompteAvecFtp.Enabled = False

                chkClearFTP.Checked = False
                chkClearFTP.Enabled = False
                txtIP1.Text = ""
                txtIP2.Text = ""
                txtIP3.Text = ""
                txtIP4.Text = ""

            Case TypeAction.Liste 'Action = LISTE
                cmdNouveau.Enabled = True
                cmdEnregistrer.Enabled = False
                cmdAnnuler.Enabled = False
                cmdDeverrouiller.Enabled = True
                cmdSupprimer.Enabled = True

                pnlAjout.Visible = False
                pnlSelect.Visible = True

                optEmplRRQ.Checked = False
                optPartnIndiv.Checked = False
                optPartn.Checked = False

                chkCompteAvecFtp.Checked = False
                chkCompteAvecFtp.Enabled = False

                chkClearFTP.Checked = False
                chkClearFTP.Enabled = False
                txtIP1.Text = ""
                txtIP2.Text = ""
                txtIP3.Text = ""
                txtIP4.Text = ""

            Case TypeAction.Ajout
                'Action = AJOUT
                If XuCuConfiguration.ValeurSysteme("General", "Environnement") = "PROD" Then
                    chkEnvUnit.Checked = False
                    chkEnvIntg.Checked = False
                    chkEnvAccp.Checked = False
                    chkEnvProd.Checked = True
                    chkEnvProd.Enabled = True
                Else
                    chkEnvUnit.Checked = True
                    chkEnvIntg.Checked = True
                    chkEnvAccp.Checked = True
                    chkEnvProd.Checked = False
                    chkEnvProd.Enabled = False
                End If

                cmdNouveau.Enabled = False
                cmdEnregistrer.Enabled = True
                cmdAnnuler.Enabled = True
                cmdDeverrouiller.Enabled = False
                cmdSupprimer.Enabled = False

                pnlAjout.Visible = True
                pnlSelect.Visible = False

                optEmplRRQ.Checked = True
                optPartnIndiv.Checked = False
                optPartn.Checked = False

                chkCompteAvecFtp.Checked = False
                chkCompteAvecFtp.Enabled = False

                chkClearFTP.Checked = False
                chkClearFTP.Enabled = False
                txtIP1.Text = ""
                txtIP2.Text = ""
                txtIP3.Text = ""
                txtIP4.Text = ""

                txtIP1.Enabled = False
                txtIP2.Enabled = False
                txtIP3.Enabled = False
                txtIP4.Enabled = False
                lblPoint1.Enabled = False
                lblPoint2.Enabled = False
                lblPoint3.Enabled = False

                txtAbrevClient.Focus()

        End Select
    End Sub

    ''' <summary>
    ''' Lance le chargement de la liste des comptes de façon asynchrone pour que la fenêtre demeure active durant le traitement
    ''' Durant le chargement, tout est désactivé et la liste affiche un message de chargement en cours
    ''' </summary>
    Private Sub RemplirListe()
        'Debugger.Launch()
        InitialiserVisuel(TypeAction.Chargement)
        lvwCodeUtil.Items.Clear()
        lvwCodeUtil.Items.Add("Chargement en cours ...")

        Dim worker As New System.ComponentModel.BackgroundWorker()
        AddHandler worker.DoWork, AddressOf backgroundListe_DoWork
        AddHandler worker.RunWorkerCompleted, AddressOf backgroundListe_RunWorkerCompleted
        worker.RunWorkerAsync()

    End Sub

    ''' <summary>
    ''' Supprime le compte FTP sélectionné, supprime également sa clé symbolique si elle existe
    ''' </summary>
    ''' <returns>Indicateur de succès de l'opération</returns>
    Private Function Supprimer() As Boolean
        'Debugger.Launch()
        ' Obtenir le compte sélectionné, ne pas effectuer l'opération si il n'y a aucun compte sélectionné
        If lvwCodeUtil.SelectedItems.Count = 0 Then Return False
        Dim strCompte As String = lvwCodeUtil.SelectedItems.Item(0).Text

        ' Supprimer le compte FTP
        Try

            Dim CaAffaire As TsICompI
            Dim contexte As Object = Nothing

            Using objAppel As New XuCuAppelerCompI(Of TsICompI)()
                Dim chaineContexte As String = objAppel.PreparerAppel(contexte, ObtenirCodeEnv())

                CaAffaire = objAppel.CreerComposantIntegration(chaineContexte)
                CaAffaire.SupprimerCompte(chaineContexte, strCompte)
                objAppel.AnalyserRetour(chaineContexte, Nothing)
            End Using

            Supprimer = True
        Catch ex As Exception
            MessageBox.Show(ex.ToString(), "Erreur Supprimer", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Supprimer = False
        End Try

        ' Supprimer la clé symbolique (il se peut qu'elle n'existe pas)
        Try

            Dim supprCle As TS1N201_DtCdAccGenV1.TsDtCleSym = New TS1N201_DtCdAccGenV1.TsDtCleSym
            supprCle.CoIdnCleSymTs = "EE1" + strCompte
            supprCle.CoEnvCleSymTs = ObtenirCodeEnv(strCompte.Substring(1, 1))
            supprCle.CoTypDepCleTs = "AUT"
            Dim CaAffaire As TS1N215_INiveauSecrt2.TsICompI
            Dim contexte As Object = Nothing

            Using objAppel As New XuCuAppelerCompI(Of TS1N215_INiveauSecrt2.TsICompI)
                Dim chaineContexte As String = objAppel.PreparerAppel(contexte, TsFdGererUtilSecFtpSvr.ObtenirCodeEnv())

                CaAffaire = objAppel.CreerComposantIntegration(chaineContexte)
                CaAffaire.DetruireCle(chaineContexte, supprCle)
                objAppel.AnalyserRetour(chaineContexte, Nothing)
            End Using

        Catch ex As Exception
            MessageBox.Show("Erreur lors de la suppression de la clé symbolique, le compte FTP a toutefois été supprimé :" + Environment.NewLine + _
                   ex.ToString(), "Suppression Clé Symbolique", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Function

    ''' <summary>
    ''' Obtenir le code d'énumération pour l'environnement d'exécution
    ''' </summary>
    ''' <returns>Code de l'environnement d'exécution</returns>
    Friend Shared Function ObtenirCodeEnv() As XuCaCreerContexte.XuCCEnvrn
        Dim result As XuCaCreerContexte.XuCCEnvrn
        Select Case Rrq.InfrastructureCommune.ScenarioTransactionnel.XuCaContexte.EnvrnPFI(String.Empty)
            Case "U"
                result = XuCaCreerContexte.XuCCEnvrn.UNIT
            Case "I"
                result = XuCaCreerContexte.XuCCEnvrn.INTG
            Case "A"
                result = XuCaCreerContexte.XuCCEnvrn.ACCP
            Case "B"
                result = XuCaCreerContexte.XuCCEnvrn.FORA
            Case "Q"
                result = XuCaCreerContexte.XuCCEnvrn.FORP
            Case "S"
                result = XuCaCreerContexte.XuCCEnvrn.SIML
            Case "P"
                result = XuCaCreerContexte.XuCCEnvrn.PROD
            Case Else
                result = XuCaCreerContexte.XuCCEnvrn.PROD
        End Select
        Return result
    End Function

    ''' <summary>
    ''' Obtenir le code d'énumération pour l'environnement d'exécution
    ''' </summary>
    ''' <returns>Code de l'environnement d'exécution</returns>
    Friend Shared Function ObtenirCodeEnv(ByVal lettreEnv As String) As String
        Select Case lettreEnv
            Case "U"
                Return "UNIT"
            Case "I"
                Return "INTG"
            Case "A"
                Return "ACCP"
            Case "B"
                Return "FORA"
            Case "Q"
                Return "FORP"
            Case "S"
                Return "SIML"
        End Select

        Return "PROD"
    End Function

#End Region

End Class
