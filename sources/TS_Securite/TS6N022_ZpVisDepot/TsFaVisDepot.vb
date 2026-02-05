Imports System.Threading

Public Class TsFaVisDepot
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
    Friend WithEvents tmrRafraichir As System.Timers.Timer
    Friend WithEvents tvwDepot As System.Windows.Forms.TreeView
    Friend WithEvents mscActivite As AxMSChart20Lib.AxMSChart
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents colActif As System.Windows.Forms.ColumnHeader
    Friend WithEvents colInactif As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ltvMoyenne As System.Windows.Forms.ListView
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(TsFaVisDepot))
        Dim ListViewItem1 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"", "0", "0"}, -1)
        Me.tmrRafraichir = New System.Timers.Timer
        Me.tvwDepot = New System.Windows.Forms.TreeView
        Me.mscActivite = New AxMSChart20Lib.AxMSChart
        Me.Label1 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.ltvMoyenne = New System.Windows.Forms.ListView
        Me.colActif = New System.Windows.Forms.ColumnHeader
        Me.colInactif = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        CType(Me.tmrRafraichir, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.mscActivite, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'tmrRafraichir
        '
        Me.tmrRafraichir.Enabled = True
        Me.tmrRafraichir.Interval = 1
        Me.tmrRafraichir.SynchronizingObject = Me
        '
        'tvwDepot
        '
        Me.tvwDepot.ImageIndex = -1
        Me.tvwDepot.Location = New System.Drawing.Point(0, 0)
        Me.tvwDepot.Name = "tvwDepot"
        Me.tvwDepot.SelectedImageIndex = -1
        Me.tvwDepot.Size = New System.Drawing.Size(600, 376)
        Me.tvwDepot.TabIndex = 1
        '
        'mscActivite
        '
        Me.mscActivite.DataSource = Nothing
        Me.mscActivite.Location = New System.Drawing.Point(8, 400)
        Me.mscActivite.Name = "mscActivite"
        Me.mscActivite.OcxState = CType(resources.GetObject("mscActivite.OcxState"), System.Windows.Forms.AxHost.State)
        Me.mscActivite.Size = New System.Drawing.Size(472, 96)
        Me.mscActivite.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 384)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(584, 16)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Nombre de jetons dans le dépot sur 60 secondes"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.ltvMoyenne)
        Me.GroupBox1.Location = New System.Drawing.Point(488, 395)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(104, 102)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Moy. par sec."
        '
        'ltvMoyenne
        '
        Me.ltvMoyenne.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ltvMoyenne.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.colActif, Me.colInactif})
        Me.ltvMoyenne.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.ltvMoyenne.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem1})
        Me.ltvMoyenne.Location = New System.Drawing.Point(8, 17)
        Me.ltvMoyenne.Name = "ltvMoyenne"
        Me.ltvMoyenne.Size = New System.Drawing.Size(88, 77)
        Me.ltvMoyenne.TabIndex = 0
        Me.ltvMoyenne.View = System.Windows.Forms.View.Details
        '
        'colActif
        '
        Me.colActif.Text = "Actif"
        Me.colActif.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colActif.Width = 44
        '
        'colInactif
        '
        Me.colInactif.Text = "Inactif"
        Me.colInactif.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colInactif.Width = 44
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Width = 0
        '
        'TsFaVisDepot
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(600, 503)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.mscActivite)
        Me.Controls.Add(Me.tvwDepot)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "TsFaVisDepot"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Contenu du dépot de jetons de sécurité"
        CType(Me.tmrRafraichir, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.mscActivite, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Const C_SHRD_PROP_GROUP As String = "DepotJetons"
    Public Const C_SHRD_PROP As String = "Jetons"

    Private m_objDepot As TS6N021_ZpGestionJeton.TsCuGestionJeton
    Private m_dsDepot As New DataSet("Depot")
    Private m_arrActv(59, 1) As Integer
    Private m_CptActif As Integer
    Private m_CptInactif As Integer
    Private m_CptSec As Integer
    Private m_objNoeuxActif As TreeNode
    Private m_objNoeuxInactif As TreeNode

    Private Sub tmrRafraichir_Elapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs) Handles tmrRafraichir.Elapsed
        tmrRafraichir.Enabled = False
        If tmrRafraichir.Interval <> 1000 Then tmrRafraichir.Interval = 1000

        Try
            m_objDepot.ObtenirDepot(m_dsDepot, C_SHRD_PROP_GROUP, C_SHRD_PROP)

            If m_dsDepot.Tables.Contains("Depot") Then
                Dim drActif() As DataRow = m_dsDepot.Tables("Depot").Select("Actif = true")
                Dim drInactif() As DataRow = m_dsDepot.Tables("Depot").Select("Actif = false")
                

                m_objNoeuxActif.Text = "Actif (" + drActif.Length.ToString + " jeton(s))"
                m_objNoeuxInactif.Text = "Inactif (" + drInactif.Length.ToString + " jeton(s))"

                SupprimerJetonsAbsent(drActif, m_objNoeuxActif)
                SupprimerJetonsAbsent(drInactif, m_objNoeuxInactif)
                AjouterNouveauxJetons(drActif, m_objNoeuxActif)
                AjouterNouveauxJetons(drInactif, m_objNoeuxInactif)

                For iSec As Integer = 58 To 0 Step -1
                    For iIndex As Integer = 0 To 1
                        m_arrActv(iSec + 1, iIndex) = m_arrActv(iSec, iIndex)
                    Next
                Next

                AfficherStats(drActif.Length, drInactif.Length)
            Else
                m_objNoeuxActif.Text = "Actif (0 jeton)"
                m_objNoeuxInactif.Text = "Inactif (0 jeton)"
            End If

            tvwDepot.ExpandAll()
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

        tmrRafraichir.Enabled = True
    End Sub

    Private Sub AfficherStats(ByVal iNbActif As Integer, ByVal iNbInactif As Integer)
        m_arrActv(0, 0) = iNbInactif
        m_arrActv(0, 1) = iNbActif

        m_CptActif += iNbActif
        m_CptInactif += iNbInactif
        m_CptSec += 1

        mscActivite.ChartData = m_arrActv
        mscActivite.Column = 1
        mscActivite.ColumnLabel = "Inactif"
        mscActivite.Column = 2
        mscActivite.ColumnLabel = "Actif"

        ltvMoyenne.Items(0).SubItems(1).Text = (m_CptActif / m_CptSec).ToString("N")
        ltvMoyenne.Items(0).SubItems(2).Text = (m_CptInactif / m_CptSec).ToString("N")
    End Sub

    Private Sub SupprimerJetonsAbsent(ByRef drRows() As DataRow, ByRef tnNoeuxParent As TreeNode)
        Dim tnNode As TreeNode
        Dim drRow As DataRow
        Dim sJeton As String
        Dim blnJetonTrouvee As Boolean
        Dim iNode As Integer

        iNode = 0
        Do While iNode < tnNoeuxParent.Nodes.Count
            tnNode = tnNoeuxParent.Nodes(iNode)
            blnJetonTrouvee = False
            For Each drRow In drRows
                sJeton = "Clé : " + drRow.Item("Cle") + " -- Usager : " + drRow.Item("Usager") + ", " + _
                    "Composant : " + drRow.Item("Composant")

                If sJeton = tnNode.Text Then
                    blnJetonTrouvee = True
                    Exit For
                End If
            Next

            If Not blnJetonTrouvee Then tnNoeuxParent.Nodes.RemoveAt(iNode) _
            Else iNode += 1
        Loop
    End Sub

    Private Sub AjouterNouveauxJetons(ByRef drRows() As DataRow, ByRef tnNoeuxParent As TreeNode)
        Dim tnNode As TreeNode
        Dim drRow As DataRow
        Dim sJeton As String
        Dim blnJetonTrouvee As Boolean

        For Each drRow In drRows
            blnJetonTrouvee = False
            sJeton = "Clé : " + drRow.Item("Cle") + " -- Usager : " + drRow.Item("Usager") + ", " + _
                                    "Composant : " + drRow.Item("Composant")

            For Each tnNode In tnNoeuxParent.Nodes
                If sJeton = tnNode.Text Then
                    blnJetonTrouvee = True
                    Exit For
                End If
            Next

            If Not blnJetonTrouvee Then tnNoeuxParent.Nodes.Add(sJeton)
        Next
    End Sub

    Private Sub TsFaVisDepot_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_objDepot = CreateObject("TS6N021_ZpGestionJeton.TsCuGestionJeton")

        m_CptActif = 0
        m_CptInactif = 0
        m_CptSec = 0
        mscActivite.ChartData = m_arrActv
        mscActivite.Column = 1
        mscActivite.ColumnLabel = "Inactif"
        mscActivite.Column = 2
        mscActivite.ColumnLabel = "Actif"

        tvwDepot.Nodes.Clear()
        m_objNoeuxActif = tvwDepot.Nodes.Add("Actif (0 jeton)")
        m_objNoeuxInactif = tvwDepot.Nodes.Add("Inactif (0 jeton)")
    End Sub
End Class


