<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TsFfRech
    Inherits Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrForm

    'La méthode substituée Dispose du formulaire pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    Friend WithEvents cmdFermer As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton
    Friend WithEvents grpTypeRech As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrGroupBox
    Friend WithEvents txtCle As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTextbox
    Friend WithEvents txtCode As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTextbox
    Friend WithEvents imgListe As System.Windows.Forms.ImageList
    Friend WithEvents GroupByRow1 As Xceed.Grid.GroupByRow
    Friend WithEvents ColumnManagerRow1 As Xceed.Grid.ColumnManagerRow
    Friend WithEvents grdResultat As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.Grilles.XzCrListe
    Friend WithEvents drtResultat As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.Grilles.XzCrDataRow
    Friend WithEvents cmrResultat As Xceed.Grid.ColumnManagerRow
    Friend WithEvents txtrResulNouveau As Xceed.Grid.TextRow

    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TsFfRech))
        Me.cmdFermer = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton()
        Me.grpTypeRech = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrGroupBox()
        Me.txtProfil = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTextbox()
        Me.cboType = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XzCrComboBox()
        Me.cboEnv = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XzCrComboBox()
        Me.XzCrLabel5 = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel()
        Me.XzCrLabel4 = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel()
        Me.XzCrLabel3 = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel()
        Me.XzCrLabel2 = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel()
        Me.XzCrLabel1 = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel()
        Me.cmdRechercher = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton()
        Me.cmdProfil = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton()
        Me.imgListe = New System.Windows.Forms.ImageList(Me.components)
        Me.txtCode = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTextbox()
        Me.txtCle = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTextbox()
        Me.GroupByRow1 = New Xceed.Grid.GroupByRow()
        Me.ColumnManagerRow1 = New Xceed.Grid.ColumnManagerRow()
        Me.grdResultat = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.Grilles.XzCrListe()
        Me.drtResultat = New Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.Grilles.XzCrDataRow()
        Me.cmrResultat = New Xceed.Grid.ColumnManagerRow()
        Me.txtrResulNouveau = New Xceed.Grid.TextRow()
        Me.grpTypeRech.SuspendLayout()
        CType(Me.ColumnManagerRow1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.grdResultat, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.drtResultat, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmrResultat, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdFermer
        '
        Me.cmdFermer.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdFermer.Location = New System.Drawing.Point(656, 304)
        Me.cmdFermer.Name = "cmdFermer"
        Me.cmdFermer.Size = New System.Drawing.Size(80, 24)
        Me.cmdFermer.TabIndex = 8
        Me.cmdFermer.Text = "Fermer"
        '
        'grpTypeRech
        '
        Me.grpTypeRech.Controls.Add(Me.txtProfil)
        Me.grpTypeRech.Controls.Add(Me.cboType)
        Me.grpTypeRech.Controls.Add(Me.cboEnv)
        Me.grpTypeRech.Controls.Add(Me.XzCrLabel5)
        Me.grpTypeRech.Controls.Add(Me.XzCrLabel4)
        Me.grpTypeRech.Controls.Add(Me.XzCrLabel3)
        Me.grpTypeRech.Controls.Add(Me.XzCrLabel2)
        Me.grpTypeRech.Controls.Add(Me.XzCrLabel1)
        Me.grpTypeRech.Controls.Add(Me.cmdRechercher)
        Me.grpTypeRech.Controls.Add(Me.cmdProfil)
        Me.grpTypeRech.Controls.Add(Me.txtCode)
        Me.grpTypeRech.Controls.Add(Me.txtCle)
        Me.grpTypeRech.Location = New System.Drawing.Point(8, 8)
        Me.grpTypeRech.Name = "grpTypeRech"
        Me.grpTypeRech.Size = New System.Drawing.Size(728, 128)
        Me.grpTypeRech.TabIndex = 32
        Me.grpTypeRech.TabStop = False
        Me.grpTypeRech.Text = "Type de recherche"
        '
        'txtProfil
        '
        Me.txtProfil.Location = New System.Drawing.Point(117, 93)
        Me.txtProfil.Name = "txtProfil"
        Me.txtProfil.NomChampsDonnee = Nothing
        Me.txtProfil.NomSourceDonnee = Nothing
        Me.txtProfil.Size = New System.Drawing.Size(196, 20)
        Me.txtProfil.TabIndex = 2
        '
        'cboType
        '
        Me.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboType.FormattingEnabled = True
        Me.cboType.GererTabCommeEnter = False
        Me.cboType.Location = New System.Drawing.Point(117, 18)
        Me.cboType.Name = "cboType"
        Me.cboType.NomChampsDonnee = Nothing
        Me.cboType.NomSourceDonnee = Nothing
        Me.cboType.Size = New System.Drawing.Size(227, 21)
        Me.cboType.TabIndex = 0
        '
        'cboEnv
        '
        Me.cboEnv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboEnv.FormattingEnabled = True
        Me.cboEnv.GererTabCommeEnter = False
        Me.cboEnv.Location = New System.Drawing.Point(117, 54)
        Me.cboEnv.Name = "cboEnv"
        Me.cboEnv.NomChampsDonnee = Nothing
        Me.cboEnv.NomSourceDonnee = Nothing
        Me.cboEnv.Size = New System.Drawing.Size(227, 21)
        Me.cboEnv.TabIndex = 1
        '
        'XzCrLabel5
        '
        Me.XzCrLabel5.AutoSize = True
        Me.XzCrLabel5.Location = New System.Drawing.Point(373, 56)
        Me.XzCrLabel5.Name = "XzCrLabel5"
        Me.XzCrLabel5.Size = New System.Drawing.Size(69, 13)
        Me.XzCrLabel5.TabIndex = 16
        Me.XzCrLabel5.Text = "Code Usager"
        '
        'XzCrLabel4
        '
        Me.XzCrLabel4.AutoSize = True
        Me.XzCrLabel4.Location = New System.Drawing.Point(373, 21)
        Me.XzCrLabel4.Name = "XzCrLabel4"
        Me.XzCrLabel4.Size = New System.Drawing.Size(22, 13)
        Me.XzCrLabel4.TabIndex = 15
        Me.XzCrLabel4.Text = "Clé"
        '
        'XzCrLabel3
        '
        Me.XzCrLabel3.AutoSize = True
        Me.XzCrLabel3.Location = New System.Drawing.Point(13, 93)
        Me.XzCrLabel3.Name = "XzCrLabel3"
        Me.XzCrLabel3.Size = New System.Drawing.Size(30, 13)
        Me.XzCrLabel3.TabIndex = 14
        Me.XzCrLabel3.Text = "Profil"
        '
        'XzCrLabel2
        '
        Me.XzCrLabel2.AutoSize = True
        Me.XzCrLabel2.Location = New System.Drawing.Point(13, 54)
        Me.XzCrLabel2.Name = "XzCrLabel2"
        Me.XzCrLabel2.Size = New System.Drawing.Size(78, 13)
        Me.XzCrLabel2.TabIndex = 13
        Me.XzCrLabel2.Text = "Environnement"
        '
        'XzCrLabel1
        '
        Me.XzCrLabel1.AutoSize = True
        Me.XzCrLabel1.Location = New System.Drawing.Point(13, 21)
        Me.XzCrLabel1.Name = "XzCrLabel1"
        Me.XzCrLabel1.Size = New System.Drawing.Size(31, 13)
        Me.XzCrLabel1.TabIndex = 12
        Me.XzCrLabel1.Text = "Type"
        '
        'cmdRechercher
        '
        Me.cmdRechercher.Location = New System.Drawing.Point(624, 93)
        Me.cmdRechercher.Name = "cmdRechercher"
        Me.cmdRechercher.Size = New System.Drawing.Size(80, 24)
        Me.cmdRechercher.TabIndex = 6
        Me.cmdRechercher.Text = "Rechercher"
        '
        'cmdProfil
        '
        Me.cmdProfil.ImageIndex = 0
        Me.cmdProfil.ImageList = Me.imgListe
        Me.cmdProfil.Location = New System.Drawing.Point(318, 89)
        Me.cmdProfil.Name = "cmdProfil"
        Me.cmdProfil.Size = New System.Drawing.Size(26, 26)
        Me.cmdProfil.TabIndex = 3
        '
        'imgListe
        '
        Me.imgListe.ImageStream = CType(resources.GetObject("imgListe.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgListe.TransparentColor = System.Drawing.Color.Transparent
        Me.imgListe.Images.SetKeyName(0, "")
        '
        'txtCode
        '
        Me.txtCode.Location = New System.Drawing.Point(488, 56)
        Me.txtCode.Name = "txtCode"
        Me.txtCode.NomChampsDonnee = Nothing
        Me.txtCode.NomSourceDonnee = Nothing
        Me.txtCode.Size = New System.Drawing.Size(216, 20)
        Me.txtCode.TabIndex = 5
        '
        'txtCle
        '
        Me.txtCle.Location = New System.Drawing.Point(488, 21)
        Me.txtCle.Name = "txtCle"
        Me.txtCle.NomChampsDonnee = Nothing
        Me.txtCle.NomSourceDonnee = Nothing
        Me.txtCle.Size = New System.Drawing.Size(216, 20)
        Me.txtCle.TabIndex = 4
        '
        'grdResultat
        '
        Me.grdResultat.ColonneTampon = Nothing
        Me.grdResultat.DataRowTemplate = Me.drtResultat
        '
        '
        '
        Me.grdResultat.FixedColumnSplitter.Visible = False
        Me.grdResultat.FixedHeaderRows.Add(Me.cmrResultat)
        Me.grdResultat.FooterRows.Add(Me.txtrResulNouveau)
        Me.grdResultat.Location = New System.Drawing.Point(8, 144)
        Me.grdResultat.Name = "grdResultat"
        '
        '
        '
        Me.grdResultat.RowSelectorPane.AllowRowResize = False
        Me.grdResultat.RowSelectorPane.Visible = True
        Me.grdResultat.Size = New System.Drawing.Size(728, 152)
        Me.grdResultat.TabIndex = 7
        Me.grdResultat.UIStyle = Xceed.UI.UIStyle.WindowsClassic
        Me.grdResultat.Verrouiller = True
        '
        'drtResultat
        '
        Me.drtResultat.TypeQuadrillage = Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.Grilles.XzCgTypeQuadrillage.XzCgTqComplet
        '
        'TsFfRech
        '
        Me.AcceptButton = Me.cmdRechercher
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.cmdFermer
        Me.ClientSize = New System.Drawing.Size(889, 386)
        Me.Controls.Add(Me.grdResultat)
        Me.Controls.Add(Me.grpTypeRech)
        Me.Controls.Add(Me.cmdFermer)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "TsFfRech"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Recherche clé symbolique"
        Me.grpTypeRech.ResumeLayout(False)
        Me.grpTypeRech.PerformLayout()
        CType(Me.ColumnManagerRow1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.grdResultat, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.drtResultat, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmrResultat, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents XzCrLabel4 As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel
    Friend WithEvents XzCrLabel3 As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel
    Friend WithEvents XzCrLabel2 As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel
    Friend WithEvents XzCrLabel1 As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel
    Friend WithEvents XzCrLabel5 As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrLabel
    Friend WithEvents cboEnv As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XzCrComboBox
    Friend WithEvents cboType As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XzCrComboBox
    Friend WithEvents txtProfil As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrTextbox
    Friend WithEvents cmdProfil As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton
    Friend WithEvents cmdRechercher As Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.XZCrButton
End Class
