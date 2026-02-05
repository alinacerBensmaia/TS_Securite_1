Imports Rrq.CS.ServicesCommuns.ScenarioTransactionnel
Imports Rrq.CS.ServicesCommuns.UtilitairesCommuns
Imports CTRL = Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ
Imports RIS = Rrq.InfrastructureCommune.ScenarioTransactionnel
Imports System.Collections.Generic
Imports System.Text
Imports System.Globalization
Imports TS1N214_PrGerAccGen.TsCuConversionsTypes
Imports TS1N215_INiveauSecrt2
Imports TS1N201_DtCdAccGenV1
Imports System.Security.Principal
Imports TS1N214_PrGerAccGen.XzCrComboBoxExtensions

''' <summary>
''' Énumération des modes de l'écran
''' </summary>
''' <remarks></remarks>
Friend Enum TsPgagModeEcran As Integer
    TsPgagMeConsultation = 0
    TsPgagMeAjout = 1
    TsPgagMeModification = 2
End Enum

'''-----------------------------------------------------------------------------
''' Project		: $safeprojectname$
''' Class		: TsFdGerAccGen
'''
'''-----------------------------------------------------------------------------
''' <summary>
''' Fenêtre principale de l'application.
''' </summary>
''' <remarks></remarks>
'''-----------------------------------------------------------------------------
Friend Class TsFdGerAccGen
    Implements XzIBesoinValid 'Implementer XzIBesoinValid pour la validation du formulaire
    Implements XzIBindingForm 'Utiliser le XzBinding automatique des contrôles.

#Region "--- Variables ---"

    ''' --------------------------------------------------------------------------------
    ''' <summary>
    ''' Membre privé à chaque formulaire qui contient une référence à toutes 
    ''' les sources de données (Datatable, etc) nécessaire pour le XzBinding
    ''' automatique.
    ''' </summary>
    ''' --------------------------------------------------------------------------------
    Private mColSourceDonnee As Hashtable

    Private securite As New TsCuSecuriteApplicative()

    Private systeme As String
    Private soussysteme As String

    ''' --------------------------------------------------------------------------------
    ''' <summary>
    ''' Déclaration de l'événément à déclencher pour initialiser le XzBinding.
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    Private Event XzBindingInit(ByVal e As XzBindingEventArg) Implements XzIBindingForm.XzBindingInit

    Private WithEvents mMessageEclair As New XzCuMessageEclair
    Private mModeEcran As TsPgagModeEcran            'Gérer le mode de l'écran
    Private mClePrec As New TsCuCle()                'Clé précédente
    Private mIndicBloqueEvenements As Boolean = True 'Indicateur pour bloquer les événements
    Private mIndicMdpNouveau As Boolean
    Private mDtbConnection As New DataTable()

    Private mDictCharge As New Dictionary(Of String, Boolean)()
    Private mSystemes As DataTable
    Private mListeCleAffiche As IList(Of TS1N201_DtCdAccGenV1.TsDtCleSym)
    Private Panel1OriginalHeight As Integer
    Private Panel2OriginalTop As Integer
    Private mGrowth As Integer
    Private mLigneChoisi As Xceed.Grid.DataRow
    Private mRowToUnselect As Xceed.Grid.DataRow
    Private mGroupeCle As TsCuGroupe
    Private listCheckboxEnv As New List(Of CTRL.XZCrCheckBox)()
    Private mVerrouActif As TsDtVerrou = Nothing
    Private mVerrouForce As Boolean = False
    Private mAccesNiveau1 As Boolean? = Nothing
    Private mAccesNiveau2 As Boolean? = Nothing
    Private mIndCreationAd As Boolean
    Private mIndCreationAdLds As Boolean


#End Region

#Region "--- Propriétés ---"



    ''' <summary>
    ''' Indique si l'utilisateur a les accès de niveau 1
    ''' </summary>
    ''' <remarks>Il n'y a pas de groupe AD pour Unitaire, donc on autorise tout</remarks>
    Private ReadOnly Property AccesNiveau1 As Boolean
        Get
            If mAccesNiveau1 Is Nothing Then
                Dim envPFI As String = RIS.XuCaContexte.EnvrnPFI(String.Empty)
                Dim roi As String = String.Format("ROI_{0}_TS1N215_INiveauSecrt1", envPFI)

                mAccesNiveau1 = (envPFI = "U") OrElse securite.EstMembreROI(roi, WindowsIdentity.GetCurrent())
            End If
            Return mAccesNiveau1.GetValueOrDefault()
            'Return (False)
        End Get
    End Property

    ''' <summary>
    ''' Indique si l'utilisateur a les accès de niveau 2
    ''' </summary>
    ''' <remarks>Il n'y a pas de groupe AD pour Unitaire, donc on autorise tout</remarks>
    Private ReadOnly Property AccesNiveau2 As Boolean
        Get
            If mAccesNiveau2 Is Nothing Then
                Dim envPFI As String = RIS.XuCaContexte.EnvrnPFI(String.Empty)
                Dim roiNiveau1 As String = String.Format("ROI_{0}_TS1N215_INiveauSecrt1", envPFI)
                Dim roiNiveau2 As String = String.Format("ROI_{0}_TS1N215_INiveauSecrt2", envPFI)

                mAccesNiveau2 = (envPFI = "U") OrElse (securite.EstMembreROI(roiNiveau1, WindowsIdentity.GetCurrent()) OrElse securite.EstMembreROI(roiNiveau2, WindowsIdentity.GetCurrent()))
            End If
            Return mAccesNiveau2.GetValueOrDefault()
            'Return False
        End Get
    End Property

    ''' <summary>
    ''' Permet d'obtenir l'état du verrou d'édition
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public ReadOnly Property VerrouActif As TsDtVerrou
        Get
            If mVerrouActif Is Nothing Then
                If AccesNiveau2 Then
                    ' Effectue la demande pour un verrou d'édition, si celui-ci est obtenu avec succès, l'édition est verrouillée sur l'utilisateur courant
                    mVerrouActif = TsCuPrGerAccGen.ObtenirVerrouEdition()
                Else
                    mVerrouActif = New TsDtVerrou()
                    mVerrouActif.InVerObt = False
                End If
            End If

            Return mVerrouActif
        End Get
    End Property

    Friend ReadOnly Property GroupeCle() As TsCuGroupe
        Get
            If mGroupeCle Is Nothing Then mGroupeCle = New TsCuGroupe()
            Return mGroupeCle
        End Get
    End Property

    Friend ReadOnly Property DtbConnection() As DataTable
        Get
            Return mDtbConnection
        End Get
    End Property

    Private Property ModeEcran() As TsPgagModeEcran
        Get
            Return mModeEcran
        End Get
        Set(ByVal value As TsPgagModeEcran)
            mModeEcran = value
        End Set
    End Property

    Private Property ListeCleAffiche() As IList(Of TS1N201_DtCdAccGenV1.TsDtCleSym)
        Get
            If mListeCleAffiche Is Nothing Then
                mListeCleAffiche = New List(Of TS1N201_DtCdAccGenV1.TsDtCleSym)()
            End If

            Return mListeCleAffiche
        End Get
        Set(ByVal value As IList(Of TS1N201_DtCdAccGenV1.TsDtCleSym))
            mListeCleAffiche = value
        End Set
    End Property

    ''' <summary>
    ''' Indicateur de verrou pour éviter que deux événements entrent en conflit
    ''' </summary>
    Private Property IndicBloqueEvenements() As Boolean
        Get
            Return mIndicBloqueEvenements
        End Get
        Set(ByVal value As Boolean)
            mIndicBloqueEvenements = value
        End Set
    End Property
#End Region

#Region "--- Constructeurs ---"

    Public Sub New()

        ' Cet appel est requis par le Concepteur Windows Form.
        IndicBloqueEvenements = True
        InitializeComponent()

        ModeEcran = TsPgagModeEcran.TsPgagMeConsultation
        mSystemes = New DataTable
        mSystemes.Locale = CultureInfo.InvariantCulture
        mSystemes.Columns.Add(New DataColumn(My.Resources.NomSysteme))
        mSystemes.Columns.Add(New DataColumn(My.Resources.TagSousSysteme, GetType(DataTable)))

    End Sub
#End Region

#Region "--- Privées ---"

    Private Sub TsFdGerAccGen_Load(ByVal sender As Object,
                                   ByVal e As System.EventArgs) Handles Me.Load

        PreparerControle()
        mDictCharge.Add(My.Resources.Root, False)
        ChargerSystemes()
        IndicBloqueEvenements = False
        RafraichirEtatFichierExportation()
        mnuForcerEdition.Visible = AccesNiveau2 AndAlso Not VerrouActif.InVerObt

        Dim indCreationCompte As TS1N201_DtCdAccGenV1.TsDtIndCreCpt = TsCuPrGerAccGen.ObtenirIndicateursCreationCompte()
        mIndCreationAd = indCreationCompte.InCreCptAdTs
        mIndCreationAdLds = indCreationCompte.InCreCptLdsTs
 
        If AccesNiveau2 AndAlso Not VerrouActif.InVerObt Then
            MessageBox.Show(String.Format("La console de gestion des clés symboliques est actuellement en cours d'utilisation par {0}. La console sera ouverte en mode consultation seulement.", VerrouActif.CoUtlPro),
                            "TS1N214", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Me_Closing(ByVal sender As Object,
                           ByVal e As System.ComponentModel.CancelEventArgs) Handles Me.Closing
        e.Cancel = VerifierChangement("quitter")

        If Not e.Cancel AndAlso mVerrouActif.InVerObt Then
            TsCuPrGerAccGen.RelacherVerrouEdition()
        End If
    End Sub

    Private Sub CreerListeCheckboxEnv()
        Dim cb As CTRL.XZCrCheckBox
        Dim top As Integer = cboEnv.Top
        Dim row As DataRow

        For Each row In TsCuPrGerAccGen.TableEnv.Rows
            cb = New CTRL.XZCrCheckBox
            cb.Top = top
            cb.Left = cboEnv.Left
            cb.Width = cboEnv.Width
            cb.Text = row("Desc").ToString
            cb.ValeurSelectionne = row("CodeEnv").ToString
            AddHandler cb.CheckedChanged, AddressOf chkEnv_CheckedChanged
            Controls.Add(cb)
            listCheckboxEnv.Add(cb)
            Panel1.Controls.Add(cb)
            top = top + 20
        Next
        mGrowth = top - cboEnv.Top - 20
    End Sub

    Private Sub RemplirTableConnection()
        DtbConnection.Columns.Add("Value")
        DtbConnection.Columns.Add("Desc1") 'valeur affiché à l'écran
        DtbConnection.Columns.Add("Desc2") 'utilisé pour générer la clé

        Dim cnx As List(Of TypeCle) = TsCuPrGerAccGen.ObtenirTypesCles()
        For Each t As TypeCle In cnx
            DtbConnection.Rows.Add(t.Code, t.Affichage, t.Corps)
        Next
    End Sub

    ''' <summary>
    ''' Remplir combo connections
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RemplirCboConnection()
        IndicBloqueEvenements = True
        RemplirTableConnection()
        cboConn.DataSource = DtbConnection
        cboConn.ValueMember = "Value"
        cboConn.DisplayMember = "Desc1"
        cboConn.SelectedValue = "AUT"
    End Sub

    Private Function ObtenirDesc2Connection(ByVal pType As String) As String
        Dim desc2 As String = String.Empty
        Dim row As DataRow = Nothing

        For Each row In DtbConnection.Rows
            If row("Value").ToString() = pType Then
                desc2 = row("Desc2").ToString()
                Exit For
            End If
        Next

        Return desc2
    End Function

    ''' <summary>
    ''' Preparer les controles pour le chargement de l'écran
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PreparerControle()
        IndicBloqueEvenements = True
        TsCuPrGerAccGen.RemplirCboEnvironnement(cboEnv)
        CreerListeCheckboxEnv()
        TsCuPrGerAccGen.RemplirCboTypeCle(cboType, False)
        RemplirCboConnection()

        Panel1OriginalHeight = Panel1.Height
        Panel2OriginalTop = Panel2.Top

        tlbCmdRechercher.Visible = True
        mnuRechercher.Visible = True
        mnuTopImporter.Visible = Config.AfficherMenuImporter
        tlbCmdImporter.Visible = Config.AfficherMenuImporter

        EnableCmdModifier(False, False)
        EnableCmdAnnuler(False)
        EnableCmdMDP(False)
    End Sub

    Private Sub RemplirGrilleArbreCle()
        Dim indicBloque As Boolean = IndicBloqueEvenements

        IndicBloqueEvenements = True
        If Not mDictCharge(My.Resources.Root) Then
            treCle.BeginInit()

            AddHandler treCle.SelectedRowsChanged, AddressOf SelectedRowsChanged
            mDictCharge(My.Resources.Root) = True

            treCle.SynchronizeDetailGrids = False

            treCle.DetailGridTemplates.Add(CreerSouSysTre())
            treCle.DataSource = mSystemes
            treCle.UpdateDetailGrids()

            ' Partie "Tables"
            With treCle
                .Title = My.Resources.LblSysteme

                ' Couleur 
                .BackColor = SystemColors.Window

                ' Coupure de mot
                .ClipPartialLine = False
                .WordWrap = True
                .Trimming = StringTrimming.EllipsisCharacter
            End With

            With cmrCle
                .AllowDrop = False
                .AllowSort = False ' Interdire le tri

                ' Ajuster l'alignement des entêtes de colonne
                .HorizontalAlignment = Xceed.Grid.HorizontalAlignment.Left
            End With

            With drtCle
                For Each cell As Xceed.Grid.Cell In .Cells
                    cell.CanBeCurrent = False
                Next

                ' Ajuster l'alignement à gauche des cellules de la grille
                .HorizontalAlignment = Xceed.Grid.HorizontalAlignment.Left

                'Couleur
                .BackColor = SystemColors.Window

                .Height = 20
            End With

            Dim SysIconColumn As New Xceed.Grid.Column(My.Resources.IconSys, GetType(Bitmap))
            With SysIconColumn
                .Visible = True
                .VisibleIndex = 1
                .Width = imgListe.ImageSize.Width + 1
                .CellViewerManager.ImagePadding = New Xceed.UI.Margins(0)
            End With

            treCle.Columns.Add(SysIconColumn)

            Dim sysCol As Xceed.Grid.Column = treCle.Columns(My.Resources.NomSysteme)
            With sysCol
                .Visible = True
                .VisibleIndex = 2
                .Width = 140
            End With

            treCle.EndInit()
        End If

        treCle.UpdateDetailGrids()

        For Each row As Xceed.Grid.DataRow In treCle.DataRows
            row.Cells(My.Resources.IconSys).Value = imgListe.Images(My.Resources.DossierFerme)
            Dim detailGrid As Xceed.Grid.DetailGrid = row.DetailGrids(0)
            Dim NouvNode As Xceed.Grid.DataRow
            NouvNode = detailGrid.DataRows.AddNew
            NouvNode.Cells(My.Resources.NomSousSysteme).Value = My.Resources.LblChargerCours
            NouvNode.EndEdit()
            AddHandler NouvNode.IsSelectedChanged, AddressOf Cle_IsSelectedChanged
            row.EndEdit()
            AddHandler row.IsSelectedChanged, AddressOf Cle_IsSelectedChanged
        Next
        IndicBloqueEvenements = indicBloque
    End Sub

    Private Sub CleSouSys_CollapsedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim grid As Xceed.Grid.DetailGrid = DirectCast(sender, Xceed.Grid.DetailGrid)

        If grid.Collapsed Then
            grid.ParentDataRow.Cells(My.Resources.IconSys).Value = imgListe.Images(My.Resources.DossierFerme)
            treCle.SelectedRows.Clear()
        Else
            grid.ParentDataRow.Cells(My.Resources.IconSys).Value = imgListe.Images(My.Resources.DossierOuvert)
            Dim table As DataTable = DirectCast(grid.ParentDataRow.Cells(My.Resources.TagSousSysteme).Value, DataTable)
            grid.DataRows.Clear()
            For Each row As DataRow In table.Rows
                Dim NouvNode As Xceed.Grid.DataRow
                NouvNode = grid.DataRows.AddNew
                NouvNode.BeginEdit()
                If String.IsNullOrEmpty(GetString(row(My.Resources.NomSousSysteme))) Then
                    NouvNode.Cells(My.Resources.NomSousSysteme).Value = My.Resources.SousSystemeVide
                Else
                    NouvNode.Cells(My.Resources.NomSousSysteme).Value = GetString(row(My.Resources.NomSousSysteme))
                End If
                NouvNode.Cells(My.Resources.IconSousSys).Value = imgListe.Images(My.Resources.DossierFerme)
                NouvNode.Cells(My.Resources.TagCle).Value = row(My.Resources.TagCle)
                NouvNode.EndEdit()
                AddHandler NouvNode.IsSelectedChanged, AddressOf Cle_IsSelectedChanged
            Next
        End If
    End Sub

    Private Sub Cle_CollapsedChanged(ByVal sender As Object,
                                     ByVal e As EventArgs)
        Dim grid As Xceed.Grid.DetailGrid = DirectCast(sender, Xceed.Grid.DetailGrid)

        If grid.Collapsed Then
            grid.ParentDataRow.Cells(My.Resources.IconSousSys).Value = imgListe.Images(My.Resources.DossierFerme)
        Else
            grid.ParentDataRow.Cells(My.Resources.IconSousSys).Value = imgListe.Images(My.Resources.DossierOuvert)
            Dim table As DataTable = DirectCast(grid.ParentDataRow.Cells(My.Resources.TagCle).Value, DataTable)
            grid.DataRows.Clear()
            For Each row As DataRow In table.Rows
                Dim NouvNode As Xceed.Grid.DataRow
                NouvNode = grid.DataRows.AddNew
                NouvNode.BeginEdit()
                Dim ssTable As DataTable = DirectCast(row(My.Resources.TagCleEnv), DataTable)
                If ssTable.Rows.Count > 1 Then
                    NouvNode.Cells(My.Resources.NomCle).Value = TsCuPrGerAccGen.CreerNomCles(row(My.Resources.NomCle).ToString, ssTable)
                    NouvNode.Cells(My.Resources.IconCles).Value = imgListe.Images(My.Resources.ColCles)
                    If grid.DetailGridTemplates.Count = 0 Then
                        grid.DetailGridTemplates.Add(CreerCleEnvTre())
                        grid.UpdateDetailGrids()
                    End If
                Else
                    NouvNode.Cells(My.Resources.NomCle).Value = row(My.Resources.NomCle)
                    NouvNode.Cells(My.Resources.IconCles).Value = imgListe.Images(My.Resources.ColCle)
                End If
                NouvNode.Cells(My.Resources.TagCleEnv).Value = row(My.Resources.TagCleEnv)
                NouvNode.EndEdit()
                AddHandler NouvNode.IsSelectedChanged, AddressOf Cle_IsSelectedChanged

            Next
        End If

    End Sub

    Private Sub CleEnv_CollapsedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim grid As Xceed.Grid.DetailGrid = DirectCast(sender, Xceed.Grid.DetailGrid)

        If grid.Collapsed Then
            grid.DataRows.Clear()
        Else
            Dim table As DataTable = DirectCast(grid.ParentDataRow.Cells(My.Resources.TagCleEnv).Value, DataTable)
            If table.Rows.Count > 1 Then
                grid.DataRows.Clear()
                For Each row As DataRow In table.Rows
                    Dim NouvNode As Xceed.Grid.DataRow
                    NouvNode = grid.DataRows.AddNew
                    NouvNode.BeginEdit()
                    NouvNode.Cells(My.Resources.NomCle).Value = row(My.Resources.NomCle)
                    NouvNode.Cells(My.Resources.IconCle).Value = imgListe.Images(My.Resources.ColCle)
                    NouvNode.EndEdit()
                    AddHandler NouvNode.IsSelectedChanged, AddressOf Cle_IsSelectedChanged
                Next
            End If
        End If
    End Sub

    Private Sub SelectedRowsChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Not IndicBloqueEvenements Then
            IndicBloqueEvenements = True
            If mLigneChoisi IsNot Nothing Then
                RemoveHandler mLigneChoisi.IsSelectedChanged, AddressOf Cle_IsSelectedChanged

                Select Case True
                    Case mRowToUnselect IsNot Nothing
                        RemoveHandler mRowToUnselect.IsSelectedChanged, AddressOf Cle_IsSelectedChanged
                        mLigneChoisi.IsSelected = True
                        mRowToUnselect.IsSelected = False
                        If mRowToUnselect IsNot Nothing Then AddHandler mRowToUnselect.IsSelectedChanged, AddressOf Cle_IsSelectedChanged
                    Case treCle.SelectedRows.Count > 0
                        treCle.SelectedRows(0).IsSelected = False
                        mLigneChoisi.IsSelected = True
                End Select
                AddHandler mLigneChoisi.IsSelectedChanged, AddressOf Cle_IsSelectedChanged
                mLigneChoisi = Nothing
                mRowToUnselect = Nothing
            End If
            IndicBloqueEvenements = False
        End If
    End Sub

    Private Sub Cle_IsSelectedChanged(ByVal sender As Object,
                                      ByVal e As EventArgs)
        Dim row As Xceed.Grid.DataRow = DirectCast(sender, Xceed.Grid.DataRow)

        If Not IndicBloqueEvenements Then
            IndicBloqueEvenements = True
            If row.IsSelected Then

                AbandonModif()
                EnableCmdAnnuler(False)
                If row.Cells(My.Resources.NomCle) IsNot Nothing Then
                    If row.Cells.Count > 2 AndAlso
                       row.Cells(My.Resources.TagCleEnv) IsNot Nothing AndAlso
                       DirectCast(row.Cells(My.Resources.TagCleEnv).Value, DataTable).Rows.Count > 1 Then
                        AfficherCleGroupe(row.Cells(My.Resources.NomCle).Value.ToString)
                        EnableCmdModifier(False, True)
                    Else
                        'Seulement dans le cas de suppression qui élimine toutes les clés sauf la dernières sous une clé combiné avec environnement...
                        If row.Cells(My.Resources.NomCle).Value.ToString.IndexOf("[") >= 0 Then
                            row.Cells(My.Resources.NomCle).Value = DirectCast(row.Cells(My.Resources.TagCleEnv).Value, DataTable).Rows(0)(My.Resources.NomCle).ToString
                        End If
                        'Clé
                        AfficherCle(row.Cells(My.Resources.NomCle).Value.ToString)
                        EnableCmdModifier(True, True)
                    End If
                    Panel1.Visible = True
                    Panel2.Visible = True
                Else
                    Panel1.Visible = False
                    Panel2.Visible = False
                    ListeCleAffiche.Clear()
                    EnableCmdModifier(False, False)
                End If

                If mLigneChoisi IsNot Nothing Then
                    mRowToUnselect = row
                End If
            Else
                If Not VerifierChangement("Naviguer") Then
                    'Hide all 
                    AbandonModif()
                    mLigneChoisi = Nothing
                    Panel1.Visible = False
                    Panel2.Visible = False
                    ListeCleAffiche.Clear()
                    EnableCmdModifier(False, False)
                    EnableCmdMDP(False)
                Else
                    mLigneChoisi = row
                End If
            End If
            IndicBloqueEvenements = False
        End If
    End Sub

    Private Sub EnableCmdAnnuler(ByVal value As Boolean)
        Dim envPFI As String = Rrq.InfrastructureCommune.ScenarioTransactionnel.XuCaContexte.EnvrnPFI(String.Empty)
        If envPFI <> "U" Then
            tlbCmdImporter.Visible = False
        End If

        If (VerrouActif.InVerObt OrElse mVerrouForce) Then
            tlbCmdAnnuler.Enabled = value
            mnuAnnuler.Enabled = value
            tlbCmdEnregistrer.Enabled = value
            mnuEnregistrer.Enabled = value
            tlbCmdNouveau.Enabled = Not value
            mnuNouveau.Enabled = Not value
            mnuImporterFichier.Enabled = Not value
        Else
            tlbCmdNouveau.Enabled = False
            mnuNouveau.Enabled = False
            tlbCmdAnnuler.Enabled = False
            mnuAnnuler.Enabled = False
            tlbCmdEnregistrer.Enabled = False
            mnuEnregistrer.Enabled = False
            tlbCmdImporter.Enabled = False
            tlbCmdExporter.Enabled = False
            tlbCmdModifier.Enabled = False
            mnuModifier.Enabled = False
            tlbCmdSupprimer.Enabled = False
            mnuSupprimer.Enabled = False
            mnuImporterFichier.Enabled = False
        End If
    End Sub

    ''' <summary>
    ''' Gérer l'événement des cases à cocher de l'environnement
    ''' </summary>
    ''' <param name="sender">Sender, objet qui a crée l'événement</param>
    ''' <param name="e">EventArgs, arguments de l'événements</param>
    ''' <remarks></remarks>
    Private Sub chkEnv_CheckedChanged(ByVal sender As Object,
                                      ByVal e As EventArgs)
        Dim cb As CTRL.XZCrCheckBox = Nothing
        Dim chkRecu As CTRL.XZCrCheckBox = Nothing
        Dim indicExiste As Boolean
        Dim cle As TsCuCle = Nothing
        Dim env As String = String.Empty

        If Not IndicBloqueEvenements Then
            IndicBloqueEvenements = True

            chkRecu = DirectCast(sender, CTRL.XZCrCheckBox)

            For Each cb In listCheckboxEnv
                If cb.ValeurSelectionne IsNot Nothing Then
                    env = cb.ValeurSelectionne.ToString

                    If chkRecu.ValeurSelectionne IsNot Nothing AndAlso
                       chkRecu.ValeurSelectionne.ToString.Equals(My.Resources.EnvCodeTous) AndAlso
                       Not env.Equals(My.Resources.EnvCodeEssa) AndAlso
                       Not env.Equals(My.Resources.EnvCodeTous) Then
                        cb.Checked = chkRecu.Checked
                    End If

                    If Not env.Equals(My.Resources.EnvCodeTous) Then
                        indicExiste = GroupeCle.ListeCle.ContainsKey(env)
                        If cb.Checked AndAlso
                           cb.Enabled Then  'Cas ajout d'un environnement
                            If Not indicExiste Then
                                cle = New TsCuCle
                                cle.CodeEnv = env
                                cle.DescEnv = cb.Text
                                GroupeCle.ListeCle.Add(env, cle)
                            End If
                        Else                'Cas retrait d'un environnement
                            If indicExiste Then
                                GroupeCle.ListeCle.Remove(env)
                            End If
                        End If
                    End If
                End If
            Next

            IndicBloqueEvenements = False
        End If

        GererControlEnv()
    End Sub

    ''' <summary>
    ''' Gerer les controls selon le nombre d'environnements choisi
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GererControlEnv()
        Dim count As Integer = 0
        Dim cb As CTRL.XZCrCheckBox

        If Not ModeEcran = TsPgagModeEcran.TsPgagMeConsultation Then
            If Not cboEnv.Visible Then
                For Each cb In listCheckboxEnv
                    If cb.Checked AndAlso
                       cb.ValeurSelectionne IsNot Nothing AndAlso
                       Not cb.ValeurSelectionne.ToString.Equals(My.Resources.EnvCodeTous) AndAlso
                       cb.Enabled Then
                        count += 1
                    End If
                Next
            End If

            If count > 1 Then
                txtCode.Verrouiller = True
                txtCode.Text = String.Empty
                txtMdp.Verrouiller = True
                txtMdp.Text = String.Empty
                txtProfil.Verrouiller = True
                txtProfil.Text = String.Empty
                cmdProfil.Enabled = False
                cmdEditMultiple.Visible = True
                cmdMontrerPassword.Enabled = False
                cmdGenererMdp.Enabled = False
            Else
                txtCode.Verrouiller = False
                txtMdp.Verrouiller = Not AccesNiveau1
                txtProfil.Verrouiller = False
                cmdProfil.Enabled = True
                cmdEditMultiple.Visible = False
                cmdGenererMdp.Enabled = True
                EnableCmdMDP(True)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Obtenir le nombre d'événement coché
    ''' </summary>
    ''' <returns>Obtenir le nombre d'événement coché</returns>
    ''' <remarks></remarks>
    Private Function ObtenirNbEnv() As Integer
        Dim cb As CTRL.XZCrCheckBox
        Dim count As Integer = 0

        For Each cb In listCheckboxEnv
            If cb.Checked AndAlso
               cb.ValeurSelectionne IsNot Nothing AndAlso
               Not cb.ValeurSelectionne.ToString.Equals(My.Resources.EnvCodeTous) Then
                count += 1
            End If
        Next

        Return count
    End Function

    Private Sub EnableCmdModifier(ByVal pIndicModifier As Boolean,
                                  ByVal pIndicSupprimer As Boolean)
        If VerrouActif.InVerObt OrElse mVerrouForce Then
            tlbCmdModifier.Enabled = pIndicModifier
            mnuModifier.Enabled = pIndicModifier
            tlbCmdSupprimer.Enabled = pIndicSupprimer
            mnuSupprimer.Enabled = pIndicSupprimer
        Else
            tlbCmdModifier.Enabled = False
            mnuModifier.Enabled = False
            tlbCmdSupprimer.Enabled = False
            mnuSupprimer.Enabled = False
        End If
    End Sub

    Private Sub EnableCmdMDP(ByVal pValue As Boolean)
        If ListeCleAffiche IsNot Nothing AndAlso AccesNiveau1 Then
            cmdMontrerPassword.Enabled = pValue
        Else
            cmdMontrerPassword.Enabled = False
        End If
    End Sub

    ''' <summary>
    ''' Place la panneau de saisie de clé dans un état où on peut sélectionner un seul ou plusieurs environnements
    ''' </summary>
    ''' <param name="multiple">Indicateur de sélection de plusieurs environnement</param>
    ''' <param name="tousEnvironnement"></param>
    Private Sub FormatCle(ByVal multiple As Boolean, ByVal tousEnvironnement As Boolean)
        If multiple Then
            cboEnv.Visible = False
            Dim ctl As CTRL.XZCrCheckBox

            For Each ctl In listCheckboxEnv
                ctl.Visible = True
            Next
            lblEnvrn.Text = "Environnements"
            Panel1.Height = Panel1OriginalHeight + mGrowth
            Panel2.Top = Panel2OriginalTop + mGrowth
        Else
            If tousEnvironnement Then
                cboEnv.DataSource = TsCuPrGerAccGen.TableEnv
            Else
                cboEnv.DataSource = TsCuPrGerAccGen.TableEnvUnique
            End If

            cboEnv.Visible = True

            Dim ctl As CTRL.XZCrCheckBox
            For Each ctl In listCheckboxEnv
                ctl.Visible = False
            Next
            Panel1.Height = Panel1OriginalHeight
            Panel2.Top = Panel2OriginalTop
            lblEnvrn.Text = "Environnement"
        End If
    End Sub

    Private Sub GererVisibiliteGroupe(ByVal pIndicGroupe As Boolean)
        If ModeEcran = TsPgagModeEcran.TsPgagMeAjout Then pIndicGroupe = False
        cmdMontrerPassword.Visible = Not pIndicGroupe
        cmdGenererMdp.Visible = Not pIndicGroupe
        cmdEditMultiple.Visible = pIndicGroupe AndAlso ModeEcran = TsPgagModeEcran.TsPgagMeAjout
        lblCode.Visible = Not pIndicGroupe
        txtCode.Visible = Not pIndicGroupe
        lblMdp.Visible = Not pIndicGroupe
        txtMdp.Visible = Not pIndicGroupe

        lblProfil.Visible = Not pIndicGroupe
        txtProfil.Visible = Not pIndicGroupe
        cmdProfil.Visible = Not pIndicGroupe
        lblDesc.Visible = Not pIndicGroupe
        txtDesc.Visible = Not pIndicGroupe
        lblComm.Visible = Not pIndicGroupe
        txtComm.Visible = Not pIndicGroupe
        lblCdVerif.Visible = Not pIndicGroupe
        txtCdVerif.Visible = Not pIndicGroupe

        lblCreerCompte.Visible = ModeEcran = TsPgagModeEcran.TsPgagMeAjout
        chkCompteAD.Visible = ModeEcran = TsPgagModeEcran.TsPgagMeAjout
        chkCompteADLDS.Visible = ModeEcran = TsPgagModeEcran.TsPgagMeAjout   
    End Sub

    Private Sub AfficherCleGroupe(ByVal cleName As String)
        Dim indicGroupe As Boolean = True
        Dim cle As TS1N201_DtCdAccGenV1.TsDtCleSym

        IndicBloqueEvenements = True
        FormatCle(True, True)
        txtCle.Verrouiller = True
        txtCle.Text = cleName.Split("["c)(0)
        ListeCleAffiche = TsCuPrGerAccGen.ObtenirCles(txtCle.Text)

        txtSysteme.Verrouiller = True
        txtSysteme.Text = ListeCleAffiche(0).CoSysCleSymTs
        txtSousSysteme.Verrouiller = True
        txtSousSysteme.Text = ListeCleAffiche(0).CoSouCleSymTs

        txtCode.Text = ListeCleAffiche(0).CoUtlGenCleTs
        txtCode.Verrouiller = True

        GererVisibiliteGroupe(indicGroupe)

        EnableCmdMDP(ListeCleAffiche.Count = 1)

        cboType.Verrouiller = True

        cboType.SelectedValue = ListeCleAffiche(0).CoTypCleSymTs
        For Each cle In ListeCleAffiche
            If cboType.SelectedValue IsNot Nothing AndAlso
               cboType.SelectedValue.ToString <> cle.CoTypCleSymTs Then
                cboType.SelectedIndex = -1
                Exit For
            End If
        Next

        cboConn.Verrouiller = True
        txtCle.Verrouiller = True

        cboConn.SelectedValue = ListeCleAffiche(0).CoTypDepCleTs

        Dim ctl As CTRL.XZCrCheckBox
        For Each ctl In listCheckboxEnv
            ctl.Verrouiller = True
            ctl.Checked = False
            For Each cle In ListeCleAffiche
                If ctl.ValeurSelectionne.ToString = cle.CoEnvCleSymTs Then
                    ctl.Checked = True
                    Exit For
                End If
            Next
        Next

        IndicBloqueEvenements = False
    End Sub

    Private Sub AfficherCle(ByVal cleName As String)
        IndicBloqueEvenements = True
        GererVisibiliteGroupe(False)
        GererControl()
        FormatCle(False, True)

        Dim Cle As TS1N201_DtCdAccGenV1.TsDtCleSym = TsCuPrGerAccGen.ObtenirCle(cleName)
        ListeCleAffiche = New List(Of TS1N201_DtCdAccGenV1.TsDtCleSym)
        ListeCleAffiche.Add(Cle)

        If Cle IsNot Nothing Then
            txtCle.Text = GetString(Cle.CoIdnCleSymTs)
            txtCode.Text = GetString(Cle.CoUtlGenCleTs)
            txtSysteme.Text = GetString(Cle.CoSysCleSymTs)
            txtSousSysteme.Text = GetString(Cle.CoSouCleSymTs)
            txtCdVerif.Text = GetString(Cle.VlVerCleSymTs)

            If Cle.LsGroAd IsNot Nothing AndAlso
               Cle.LsGroAd.Count > 0 Then
                txtProfil.Text = (Cle.LsGroAd.Select(Function(g As TS1N201_DtCdAccGenV1.TsDtGroAd) g.NmGroActDirTs)).Aggregate(Function(current As String, snext As String) current & ", " & snext)
            Else
                txtProfil.Text = String.Empty
            End If


            txtComm.Text = GetString(Cle.CmCleSymTs)
            txtComm.Verrouiller = True

            txtDesc.Text = GetString(Cle.DsCleSymTs)
            txtDesc.Verrouiller = True

            txtMdp.Text = GetString(Cle.VlMotPasCleTs)
            txtMdp.UseSystemPasswordChar = True
            txtMdp.Verrouiller = True
            EnableCmdMDP(True)

            cboEnv.SelectedValue = GetString(Cle.CoEnvCleSymTs)
            cboEnv.Verrouiller = True
            cboType.SelectedValue = GetString(Cle.CoTypCleSymTs)
            cboType.Verrouiller = True

            cboConn.SelectedValue = GetString(Cle.CoTypDepCleTs)

            cboConn.Verrouiller = True
            txtCle.Verrouiller = True
        End If
        IndicBloqueEvenements = False
    End Sub

    Private Sub ChargerSystemes()
        Dim indicBloque As Boolean = IndicBloqueEvenements

        IndicBloqueEvenements = True
        mMessageEclair.Action = My.Resources.CmdChargerSysteme
        mMessageEclair.Titre = Me.Text
        mMessageEclair.Message = My.Resources.LblChargerSysteme
        mMessageEclair.LaurgeurFenetre = 380
        mMessageEclair.Demarrer(Me)
        mMessageEclair_ExecuterAction()

        mMessageEclair.Dispose()
        IndicBloqueEvenements = indicBloque
    End Sub

    Private Function CreerSouSysTre() As Xceed.Grid.DetailGrid
        Dim detailGrid As New Xceed.Grid.DetailGrid

        detailGrid.BeginInit()
        detailGrid.Tag = My.Resources.TagSousSysteme

        detailGrid.Collapsed = True

        ' Set the margins of the DetailGrid
        detailGrid.TopMargin.Height = 0
        detailGrid.BottomMargin.Height = 0

        ' Subscribe to the CollapsedChanged event.
        AddHandler detailGrid.CollapsedChanged, AddressOf CleSouSys_CollapsedChanged

        Dim SouSysIconColumn As New Xceed.Grid.Column(My.Resources.IconSousSys, GetType(Bitmap))
        Dim SouSysNameColumn As New Xceed.Grid.Column(My.Resources.NomSousSysteme, GetType(String))
        Dim dataColumn As New Xceed.Grid.Column(My.Resources.TagCle, GetType(DataTable))

        SouSysNameColumn.Width = 170
        SouSysIconColumn.Width = imgListe.Images(My.Resources.DossierFerme).Width
        SouSysIconColumn.CellViewerManager.ImagePadding = New Xceed.UI.Margins(0)
        dataColumn.Visible = False

        detailGrid.Columns.Add(SouSysIconColumn)
        detailGrid.Columns.Add(SouSysNameColumn)
        detailGrid.Columns.Add(dataColumn)

        ' Prevent navigation to individual cells.
        detailGrid.AllowCellNavigation = False
        If detailGrid.DetailGridTemplates.Count = 0 Then
            detailGrid.DetailGridTemplates.Add(CreerCleTre())
            detailGrid.EndInit()
        End If

        Return detailGrid
    End Function

    Private Function CreerCleTre() As Xceed.Grid.DetailGrid
        Dim detailGrid As New Xceed.Grid.DetailGrid

        detailGrid.BeginInit()
        detailGrid.Tag = My.Resources.TagCle

        detailGrid.Collapsed = True

        ' Set the margins of the DetailGrid
        detailGrid.TopMargin.Height = 0
        detailGrid.BottomMargin.Height = 0

        ' Subscribe to the CollapsedChanged event.
        AddHandler detailGrid.CollapsedChanged, AddressOf Cle_CollapsedChanged

        Dim ClesIconColumn As New Xceed.Grid.Column(My.Resources.IconCles, GetType(Bitmap))
        Dim CleNameColumn As New Xceed.Grid.Column(My.Resources.NomCle, GetType(String))
        Dim dataColumn As New Xceed.Grid.Column(My.Resources.TagCleEnv, GetType(DataTable))

        CleNameColumn.Width = 170
        ClesIconColumn.Width = imgListe.Images(My.Resources.DossierFerme).Width
        ClesIconColumn.CellViewerManager.ImagePadding = New Xceed.UI.Margins(0)

        detailGrid.Columns.Add(ClesIconColumn)
        detailGrid.Columns.Add(CleNameColumn)
        detailGrid.Columns.Add(dataColumn)

        ' Prevent navigation to individual cells.
        detailGrid.AllowCellNavigation = False
        detailGrid.EndInit()

        Return detailGrid
    End Function

    Private Function CreerCleEnvTre() As Xceed.Grid.DetailGrid
        Dim detailGrid As New Xceed.Grid.DetailGrid

        detailGrid.BeginInit()
        detailGrid.Tag = My.Resources.TagCleEnv

        detailGrid.Collapsed = True

        ' Set the margins of the DetailGrid
        detailGrid.TopMargin.Height = 0
        detailGrid.BottomMargin.Height = 0

        ' Subscribe to the CollapsedChanged event.
        AddHandler detailGrid.CollapsedChanged, AddressOf CleEnv_CollapsedChanged

        Dim ClesIconColumn As New Xceed.Grid.Column(My.Resources.IconCle, GetType(Bitmap))
        Dim CleNameColumn As New Xceed.Grid.Column(My.Resources.NomCle, GetType(String))

        CleNameColumn.Width = 170
        ClesIconColumn.Width = imgListe.Images(My.Resources.DossierFerme).Width
        ClesIconColumn.CellViewerManager.ImagePadding = New Xceed.UI.Margins(0)

        detailGrid.Columns.Add(ClesIconColumn)
        detailGrid.Columns.Add(CleNameColumn)

        ' Prevent navigation to individual cells.
        detailGrid.AllowCellNavigation = False
        detailGrid.EndInit()

        Return detailGrid
    End Function

    Private Sub mMessageEclair_ExecuterAction() Handles mMessageEclair.ExecuterAction
        'ne rien faire si l'action ne correspond pas
        If mMessageEclair.Action <> My.Resources.CmdChargerSysteme Then Return

        Dim listeCleGrille As IList(Of TS1N201_DtCdAccGenV1.TsDtCleSym)
        listeCleGrille = TsCuPrGerAccGen.ObtenirListeCles()
        treCle.Clear()
        mSystemes.Clear()
        mDictCharge.Clear()
        mDictCharge.Add(My.Resources.Root, False)

        Dim cle As TS1N201_DtCdAccGenV1.TsDtCleSym
        Using arbre As New TsCuArbre
            For Each cle In From c In listeCleGrille Order By c.CoSysCleSymTs, c.CoSouCleSymTs, c.CoIdnCleSymTs, c.CoEnvCleSymTs
                If mSystemes.Rows.Count = 0 OrElse mSystemes.Rows(mSystemes.Rows.Count - 1)(My.Resources.NomSysteme).ToString <> cle.CoSysCleSymTs Then
                    Dim r As DataRow = mSystemes.NewRow
                    r(My.Resources.NomSysteme) = cle.CoSysCleSymTs
                    arbre.CreerArbreSousSys(Nothing, cle)
                    r(My.Resources.TagSousSysteme) = arbre.DtArbreCleSousSys
                    mSystemes.Rows.Add(r)
                Else
                    Dim r As DataRow = mSystemes.Rows(mSystemes.Rows.Count - 1)
                    arbre.CreerArbreSousSys(r, cle)
                    r(My.Resources.TagSousSysteme) = arbre.DtArbreCleSousSys
                End If
            Next
            RemplirGrilleArbreCle()
        End Using
    End Sub

    Private Sub mnuNouveau_Click(ByVal sender As System.Object,
                                 ByVal e As System.EventArgs) Handles mnuNouveau.Click
        AjouterCle()
    End Sub

    Private Sub AnnulerModifierCle()
        AbandonModif()
        GererControl()
        If treCle.SelectedRows.Count > 0 Then
            Cle_IsSelectedChanged(treCle.SelectedRows(0), Nothing)
        End If
        mIndicMdpNouveau = False
    End Sub

    Private Sub AjouterCle()
        Dim nomCle As String = String.Empty
        Dim cleNouv As New TsCuCle()

        ModeEcran = TsPgagModeEcran.TsPgagMeAjout
        EnableCmdAnnuler(True)
        mClePrec = New TsCuCle()

        If treCle.SelectedRows.Count > 0 Then
            mLigneChoisi = DirectCast(treCle.SelectedRows(0), Xceed.Grid.DataRow)
        Else
            mLigneChoisi = Nothing
        End If
        FormatCle(True, True)
        Panel1.Visible = True
        Panel2.Visible = True
        txtCle.Text = String.Empty
        txtCle.Verrouiller = True

        If treCle.SelectedRows.Count = 1 Then
            Dim row As Xceed.Grid.DataRow = DirectCast(treCle.SelectedRows(0), Xceed.Grid.DataRow)
            Dim up As Boolean = True
            While up
                If row.Cells(My.Resources.NomSysteme) IsNot Nothing Then
                    cleNouv.Systeme = GetString(row.Cells(My.Resources.NomSysteme).Value)
                    up = False
                    GererVisibiliteGroupe(False)
                ElseIf row.Cells(My.Resources.NomSousSysteme) IsNot Nothing Then
                    cleNouv.SousSysteme = GetString(row.Cells(My.Resources.NomSousSysteme).Value)
                    If cleNouv.SousSysteme = My.Resources.SousSystemeVide Then
                        cleNouv.SousSysteme = String.Empty
                    End If
                    cleNouv.Systeme = GetString(row.ParentGrid.ParentDataRow.Cells(My.Resources.NomSysteme).Value)
                    up = False
                    GererVisibiliteGroupe(False)
                ElseIf row.Cells(My.Resources.IconCles) IsNot Nothing Then
                    nomCle = GetString(row.Cells(My.Resources.NomCle).Value)
                    mClePrec.Systeme = GetString(ListeCleAffiche(0).CoSysCleSymTs)
                    mClePrec.SousSysteme = GetString(ListeCleAffiche(0).CoSouCleSymTs)
                    mClePrec.Connection = GetString(ListeCleAffiche(0).CoTypDepCleTs)

                    row = row.ParentGrid.ParentDataRow
                    GererVisibiliteGroupe(True)
                Else
                    If row.ParentGrid IsNot Nothing AndAlso row.ParentGrid.ParentDataRow IsNot Nothing Then
                        row = row.ParentGrid.ParentDataRow
                    Else
                        up = False
                    End If
                End If
            End While
        End If

        'Les champs suivant doivent être vérouillé pour que AfficherNom ne s'exécute pas plusieurs fois inutilement
        txtSysteme.Text = cleNouv.Systeme
        txtSousSysteme.Text = cleNouv.SousSysteme

        cboType.SelectedItem = My.Resources.TypeCleDescDom
        cboType.Verrouiller = nomCle.Length > 0
        If mClePrec.Connection Is Nothing OrElse
           String.IsNullOrEmpty(GetString(mClePrec.Connection)) Then
            cboConn.SelectedValue = "AUT"
            FormatCle(False, False)
            cboEnv.Verrouiller = False
        Else
            cboConn.SelectedValue = GetString(ListeCleAffiche(0).CoTypDepCleTs)
        End If
        cboConn.Verrouiller = nomCle.Length > 0

        EnableCmdMDP(False)
        txtMdp.UseSystemPasswordChar = True

        Dim cb As CTRL.XZCrCheckBox

        For Each cb In listCheckboxEnv
            If nomCle.Length > 0 AndAlso
               cb.ValeurSelectionne IsNot Nothing AndAlso
               (cb.ValeurSelectionne.ToString().Equals(My.Resources.EnvCodeTous) OrElse CheckEnv(nomCle, cb.ValeurSelectionne.ToString())) Then
                cb.Checked = Not cb.ValeurSelectionne.ToString().Equals(My.Resources.EnvCodeTous)
                cb.Enabled = False
            Else
                cb.Checked = False
                cb.Enabled = True
            End If
        Next
        If nomCle.Length > 0 AndAlso cboConn.ValeurSelectionnee() = "AUT" Then
            Dim nomCleDecompose() As String = nomCle.Split("["c)
            If nomCleDecompose.Length > 1 Then
                txtCle.Text = nomCleDecompose(0)
            Else
                txtCle.Text = nomCle.Substring(0, nomCle.Length - 1)
            End If
        Else
            AfficherNom()
        End If
        GererControl()

        chkCompteAD.Checked = chkCompteAD.Enabled
        chkCompteADLDS.Checked = chkCompteADLDS.Enabled AndAlso chkCompteAD.Enabled    
        Dim blnVerrouillerCle As Boolean = True
        If Not cboConn.Verrouiller Then
            If cboConn.ValeurSelectionnee() = "AUT" Then
                blnVerrouillerCle = False
            End If
        End If
        txtCle.Verrouiller = blnVerrouillerCle

        GroupeCle.Clear()
    End Sub

    ''' <summary>
    ''' Gérer les controles
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GererControl()
        Dim indicBloque As Boolean = IndicBloqueEvenements
        Dim indicVerrou As Boolean = ModeEcran = TsPgagModeEcran.TsPgagMeConsultation

        IndicBloqueEvenements = True

        txtCle.Verrouiller = True
        txtCode.Verrouiller = indicVerrou
        txtCdVerif.Verrouiller = indicVerrou
        txtMdp.Verrouiller = indicVerrou OrElse Not AccesNiveau1
        txtDesc.Verrouiller = indicVerrou
        txtComm.Verrouiller = indicVerrou

        txtSysteme.Verrouiller = indicVerrou OrElse (ModeEcran = TsPgagModeEcran.TsPgagMeAjout AndAlso ObtenirNbEnv() > 0)
        txtSousSysteme.Verrouiller = indicVerrou OrElse (ModeEcran = TsPgagModeEcran.TsPgagMeAjout AndAlso ObtenirNbEnv() > 0)

        txtProfil.Verrouiller = indicVerrou
        cmdProfil.Enabled = Not indicVerrou
        cmdGenererMdp.Enabled = Not indicVerrou
        tlbCmdSupprimer.Enabled = Not ModeEcran = TsPgagModeEcran.TsPgagMeAjout
        tlbCmdRechercher.Enabled = ModeEcran = TsPgagModeEcran.TsPgagMeConsultation
        tlbCmdModifier.Enabled = ModeEcran = TsPgagModeEcran.TsPgagMeConsultation
        tlbCmdActualiser.Enabled = ModeEcran = TsPgagModeEcran.TsPgagMeConsultation
        tlbCmdExporter.Enabled = (ModeEcran = TsPgagModeEcran.TsPgagMeConsultation) AndAlso (VerrouActif.InVerObt OrElse mVerrouForce)
        mnuSupprimer.Verrouiller = ModeEcran = TsPgagModeEcran.TsPgagMeAjout
        mnuRechercher.Verrouiller = Not ModeEcran = TsPgagModeEcran.TsPgagMeConsultation
        mnuModifier.Verrouiller = Not ModeEcran = TsPgagModeEcran.TsPgagMeConsultation
        mnuActualiser.Verrouiller = Not ModeEcran = TsPgagModeEcran.TsPgagMeConsultation

        EnableCmdMDP(Not indicVerrou)

        Select Case ModeEcran
            Case TsPgagModeEcran.TsPgagMeAjout
                'Affecter les champs qui doivent être vide
                txtCode.Text = String.Empty
                txtMdp.Text = String.Empty
                txtCdVerif.Text = String.Empty
                txtProfil.Text = String.Empty
                txtDesc.Text = String.Empty
                txtComm.Text = String.Empty
                chkCompteAD.Enabled = mIndCreationAd
                chkCompteADLDS.Enabled = mIndCreationAdLds AndAlso mIndCreationAd
            Case TsPgagModeEcran.TsPgagMeModification
                txtSysteme.Verrouiller = True
                txtSousSysteme.Verrouiller = True
                cboEnv.Verrouiller = True
            Case TsPgagModeEcran.TsPgagMeConsultation
        End Select
        IndicBloqueEvenements = indicBloque
    End Sub



    Private Function CheckEnv(ByVal nomCle As String,
                              ByVal pCodeEnv As String) As Boolean
        Dim match As RegularExpressions.Match = RegularExpressions.Regex.Match(nomCle, "\[(.+)\]", RegularExpressions.RegexOptions.Compiled)
        Dim envs As String()
        Dim env As String

        If match.Success Then
            envs = match.Groups(1).Value.Split(","c)
            Dim envs2(envs.Length - 1) As String
            Dim i As Integer = 0

            For Each env In envs
                envs2(i) = TsCuPrGerAccGen.ObtenirCodeEnv(GetString(env))
                i += 1
            Next
            envs = envs2
        Else
            envs = New String() {ListeCleAffiche(0).CoEnvCleSymTs}
        End If

        For Each env In envs
            If env = pCodeEnv Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Sub ModifierCle()
        If treCle.SelectedRows.Count = 1 Then
            EnableCmdAnnuler(True)
            EnableCmdModifier(False, False)
            ModeEcran = TsPgagModeEcran.TsPgagMeModification

            GererControl()
        End If
    End Sub

    Private Sub RechercheCle()
        Dim rech As New TsFfRech(Me)

        rech.ShowDialog(Me)
    End Sub

    Private Function SauvegarderCle() As Boolean
        Dim nouvCle As TS1N201_DtCdAccGenV1.TsDtCleSym = New TS1N201_DtCdAccGenV1.TsDtCleSym
        Dim countChecked As Integer = 0
        Dim retValidation As TsCuRetValidation
        Dim cb As CTRL.XZCrCheckBox

        retValidation = ValiderCle()
        If retValidation.ListeErreur.Count = 0 Then
            If ModeEcran = TsPgagModeEcran.TsPgagMeAjout Then nouvCle.CoIdnCleSymTs = txtCle.Text

            If cboEnv.Visible = True Then
                If cboEnv.SelectedValue IsNot Nothing Then
                    nouvCle.CoEnvCleSymTs = cboEnv.SelectedValue.ToString()
                End If
            Else
                nouvCle.InAjtEnv = True
                For Each cb In listCheckboxEnv
                    If cb.Checked AndAlso
                       cb.ValeurSelectionne IsNot Nothing AndAlso
                       Not cb.ValeurSelectionne.ToString.Equals(My.Resources.EnvCodeTous) AndAlso
                       cb.Enabled Then
                        If nouvCle.CoEnvCleSymTs IsNot Nothing AndAlso
                           Not String.IsNullOrEmpty(GetString(nouvCle.CoEnvCleSymTs)) Then
                            nouvCle.CoEnvCleSymTs &= ";"
                        End If
                        nouvCle.CoEnvCleSymTs &= cb.ValeurSelectionne.ToString()
                        countChecked += 1
                    End If
                Next
            End If

            nouvCle.CoSysCleSymTs = txtSysteme.Text     'Systeme
            nouvCle.CoSouCleSymTs = txtSousSysteme.Text 'Sous-Systeme
            nouvCle.CmCleSymTs = txtComm.Text           'Commentaire
            nouvCle.DsCleSymTs = txtDesc.Text           'Description
            nouvCle.VlVerCleSymTs = txtCdVerif.Text     'Code verification

            nouvCle.CoTypCleSymTs = cboType.ValeurSelectionnee() 'Type Cle
            nouvCle.CoTypDepCleTs = cboConn.ValeurSelectionnee() 'Type Connection

            If countChecked <= 1 Then 'Cas un environnement coché, countChecked = 0 si aucun coché
                nouvCle.CoUtlGenCleTs = txtCode.Text
                nouvCle.VlMotPasCleTs = txtMdp.Text
                nouvCle.LsGroAd = (From item In txtProfil.Text.Split(","c).AsEnumerable
                                   Select New TS1N201_DtCdAccGenV1.TsDtGroAd With {.NmGroActDirTs = GetString(item)}).ToList()
            Else                     'Cas plusieurs environnements cochés
                nouvCle.CoUtlGenCleTs = GroupeCle.ObtenirListeCodes(TsCuGroupe.TsPgagTypeDict.TsPgagTdDictCode)
                nouvCle.VlMotPasCleTs = GroupeCle.ObtenirListeCodes(TsCuGroupe.TsPgagTypeDict.TsPgagTdDictMdp)
                nouvCle.LsGroAd = GroupeCle.ObtenirListeProfils
            End If

            If ModeEcran = TsPgagModeEcran.TsPgagMeModification Then
                nouvCle.CoIdnCleSymTs = txtCle.Text
            End If

            ' On laisse le service décider si les comptes AD et TSS doivent être créés
            nouvCle.StIndCreCpt = New TsDtIndCreCpt()
            nouvCle.StIndCreCpt.InCreCptAdTs = chkCompteAD.Checked
            nouvCle.StIndCreCpt.InCreCptLdsTs = chkCompteADLDS.Checked AndAlso chkCompteAD.Checked


            Try
                If TsCuPrGerAccGen.SauvegardeCle(nouvCle,
                                                 mIndicMdpNouveau,
                                                 ModeEcran = TsPgagModeEcran.TsPgagMeModification) Then
                    AnnulerModifierCle()
                    If nouvCle.CoIdnCleSymTs IsNot Nothing AndAlso
                       Not String.IsNullOrEmpty(GetString(nouvCle.CoIdnCleSymTs)) Then
                        DecocherEnvironnements()
                        ChargerSystemes()

                        GroupeCle.Clear()
                        If treCle.SelectedRows.Count > 0 Then
                            Cle_IsSelectedChanged(treCle.SelectedRows(0), Nothing)
                        End If
                    End If
                    RafraichirEtatFichierExportation()
                End If
                mIndicMdpNouveau = False
            Catch ex As XZCuErrValdtException
                AfficheMessage().AfficherErrValidation(ex)
                txtCle.Focus()
            End Try
        Else
            TsCuPrGerAccGen.AfficherErreurValidation(retValidation)
        End If

        Return retValidation.ListeErreur.Count = 0
    End Function

    ''' <summary>
    ''' Après une sauvegarde avec succès on décoche les environnements
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DecocherEnvironnements()
        Dim indicBloque As Boolean = IndicBloqueEvenements

        IndicBloqueEvenements = True
        Dim cb As CTRL.XZCrCheckBox

        For Each cb In listCheckboxEnv
            cb.Checked = False
        Next
        IndicBloqueEvenements = indicBloque
    End Sub

    ''' <summary>
    ''' Valider la clé
    ''' </summary>
    ''' <returns>Liste des champs incorrect</returns>
    ''' <remarks></remarks>
    Private Function ValiderCle() As TsCuRetValidation
        Dim ret As New TsCuRetValidation

        '01E Valider que le système est fourni
        If GetString(txtSysteme.Text) = String.Empty Then
            ret.ControlInvalide = txtSysteme
            ret.ListeErreur.Add(My.Resources.TS12101E)
        End If

        '02E Valider que le type de la clé est choisi
        If cboType.SelectedValue Is Nothing Then
            ret.ControlInvalide = cboType
            ret.ListeErreur.Add(My.Resources.TS12102E)
        End If

        Dim countChecked As Integer = 0
        Dim ProductionChecked As Boolean = False
        Dim SimulationChecked As Boolean = False

        If cboEnv.Visible Then
            '03E Au moins un environnement doit être choisi
            If cboEnv.SelectedValue Is Nothing Then
                ret.ControlInvalide = cboEnv
                ret.ListeErreur.Add(My.Resources.TS12103E)
            Else
                If cboEnv.SelectedValue.ToString() = "PROD" Then
                    ProductionChecked = True
                ElseIf cboEnv.SelectedValue.ToString() = "SIML" Then
                    SimulationChecked = True
                End If
                countChecked = 1
            End If
        Else
            Dim EssaisChecked As Boolean = False

            Dim cb As CTRL.XZCrCheckBox
            For Each cb In listCheckboxEnv
                If cb.Checked AndAlso
                   cb.ValeurSelectionne IsNot Nothing AndAlso
                   Not cb.Text.Equals(My.Resources.EnvCodeTous) AndAlso
                   cb.Enabled Then
                    Select Case cb.Text
                        Case My.Resources.EnvDescEssa
                            EssaisChecked = True
                        Case My.Resources.EnvDescProd
                            ProductionChecked = True
                        Case My.Resources.EnvDescSimu
                            SimulationChecked = True
                    End Select
                    countChecked += 1
                End If
            Next

            If countChecked = 0 Then
                '03E Au moins un environnement doit être choisi
                ret.ControlInvalide = listCheckboxEnv(0)
                ret.ListeErreur.Add(My.Resources.TS12103E)
            ElseIf countChecked > 1 AndAlso
                   Not GroupeCle.ValiderMotPasse Then
                '07E Un code, mot de passe et profil sont requis pour chaque environnement
                ret.ControlInvalide = cmdEditMultiple
                ret.ListeErreur.Add(My.Resources.TS12107E)
            End If

            '08E Lorsque l'environnement "Essais" est choisi, le seul autre environnement accepté est celui de "Production"
            If EssaisChecked Then
                Dim indicEssaisValide As Boolean = True
                For Each cb In listCheckboxEnv
                    If cb.Checked AndAlso
                       Not (cb.Text = My.Resources.EnvDescEssa OrElse cb.Text = My.Resources.EnvDescProd) Then
                        indicEssaisValide = False
                        Exit For
                    End If
                Next
                If Not indicEssaisValide Then
                    ret.ControlInvalide = listCheckboxEnv(0)
                    ret.ListeErreur.Add(My.Resources.TS12108E)
                End If
            End If
        End If

        ' Ne pas valider si il y a plusieurs environnements de sélectionné
        If countChecked = 1 Then
            '05E Il faut choisir un profil qui existe dans l'AD (Groupe AD)
            If Not TsCuPrGerAccGen.ValiderGroupes(txtProfil.Text) Then
                ret.ListeErreur.Add(My.Resources.TS12105E)
            End If

            '06E Simulation : Pour une clé en {0} utilisez un profil approprié
            If SimulationChecked AndAlso (txtProfil.Text.Length < 4 OrElse txtProfil.Text.Substring(3, 1) <> "S") Then
                ret.ControlInvalide = txtProfil
                ret.ListeErreur.Add(String.Format(My.Resources.TS12106E, My.Resources.EnvDescSimu))
            End If

            '06E Production : Pour une clé en {0} utilisez un profil approprié
            If ProductionChecked AndAlso (txtProfil.Text.Length < 4 OrElse txtProfil.Text.Substring(3, 1) <> "P") Then
                ret.ControlInvalide = txtProfil
                ret.ListeErreur.Add(String.Format(My.Resources.TS12106E, My.Resources.EnvDescProd))
            End If
        End If

        '09E Le code de la clé est obligatoire (Valider code si modifiable)
        If Not txtCode.Verrouiller AndAlso
           String.IsNullOrEmpty(GetString(txtCode.Text)) Then
            ret.ControlInvalide = txtCode
            ret.ListeErreur.Add(My.Resources.TS12109E)
        End If

        '10E Le mot de passe de la clé est obligatoire (Valider mot de passe si modifiable)
        If Not txtMdp.Verrouiller AndAlso
           String.IsNullOrEmpty(GetString(txtMdp.Text)) Then
            ret.ControlInvalide = txtMdp
            ret.ListeErreur.Add(My.Resources.TS12110E)
        End If

        '11E La description de la clé est obligatoire
        If String.IsNullOrEmpty(GetString(txtDesc.Text)) Then
            ret.ControlInvalide = txtDesc
            ret.ListeErreur.Add(My.Resources.TS12111E)
        End If

        '12E Le code de vérification est obligatoire pour une clé de type inforoute avec vérification
        If GetString(cboType.SelectedValue) = My.Resources.TypeCleCodeInforteVerif AndAlso
           txtCdVerif.Text = String.Empty Then
            ret.ControlInvalide = txtCdVerif
            ret.ListeErreur.Add(My.Resources.TS12112E)
        End If

        '16E Le nombre de caractères du mot de passe doit respecter la quantité de caractères choisi.
        If cboConn.SelectedValue.ToString.StartsWith("WC") Then
            If cboEnv.Visible Or countChecked = 1 Then
                If txtMdp.Text.Length < txtMdp.MaxLength Then
                    ret.ControlInvalide = txtMdp
                    ret.ListeErreur.Add(String.Format(My.Resources.TS12116E, txtMdp.MaxLength))
                End If
            Else
                If countChecked > 1 AndAlso
                   Not GroupeCle.ValiderLonguerMotPasse(txtMdp.MaxLength) Then
                    '07E Un code, mot de passe et profil sont requis pour chaque environnement
                    ret.ControlInvalide = cmdEditMultiple
                    ret.ListeErreur.Add(String.Format(My.Resources.TS12116E, txtMdp.MaxLength))
                End If
            End If
        End If

        Return ret
    End Function

    Private Sub SupprimerCle()
        If treCle.SelectedRows.Count = 1 AndAlso AfficheMessage.ConfirmerSuppression(PreparerMessageSuppression()) = XzCuAfficherMessage.XxGmConfirmerSuppression.XzGmCsSupprimer Then
            Dim Cle As TsDtCleSym = New TsDtCleSym
            Cle.CoIdnCleSymTs = txtCle.Text
            Cle.CoSysCleSymTs = txtSysteme.Text
            Cle.CoSouCleSymTs = txtSousSysteme.Text
            Cle.CoTypCleSymTs = cboType.ValeurSelectionnee()
            Cle.CoTypDepCleTs = cboConn.ValeurSelectionnee()

            If cboEnv.Visible = True Then
                Cle.CoEnvCleSymTs = cboEnv.ValeurSelectionnee()
            Else
                Dim cb As CTRL.XZCrCheckBox
                For Each cb In listCheckboxEnv
                    If cb.Checked Then
                        If Cle.CoEnvCleSymTs IsNot Nothing AndAlso Not String.IsNullOrEmpty(GetString(Cle.CoEnvCleSymTs)) Then
                            Cle.CoEnvCleSymTs &= ";"
                        End If
                        Cle.CoEnvCleSymTs &= GetString(cb.ValeurSelectionne)
                    End If
                Next
            End If

            If TsCuPrGerAccGen.SupprimerCle(Cle) Then
                ChargerSystemes()
                ChoisirCle(Cle)
                If treCle.SelectedRows.Count > 0 Then
                    Cle_IsSelectedChanged(treCle.SelectedRows(0), Nothing)
                End If
                RafraichirEtatFichierExportation()
            Else
                AfficheMessage.AfficherMsg("Il y a une erreur lors de la suppression de la clé")
            End If
        End If
    End Sub

    Private Function PreparerMessageSuppression() As String
        If cboEnv.Visible Then
            Return txtCle.Text
        Else
            Return txtCle.Text & " pour tous les environnements"
        End If
    End Function

    Private Sub tlbPrincipal_ButtonClick(ByVal sender As System.Object, ByVal e As ToolBarButtonClickEventArgs) Handles tlbPrincipal.ButtonClick

        Using ChangerCurseur(Cursors.WaitCursor)
            Select Case e.Button.Name
                Case "tlbCmdNouveau"
                    AjouterCle()
                Case "tlbCmdOuvrir"
                Case "tlbCmdModifier"
                    ModifierCle()
                Case "tlbCmdEnregistrer"
                    SauvegarderCle()
                Case "tlbCmdAnnuler"
                    AnnulerModifierCle()
                Case "tlbCmdSupprimer"
                    SupprimerCle()
                Case "tlbCmdCouper"
                    Couper()
                Case "tlbCmdCopier"
                    Copier()
                Case "tlbCmdColler"
                    Coller()
                Case "tlbCmdRechercher"
                    RechercheCle()
                Case "tlbCmdActualiser"
                    ChargerSystemes()
                Case "tlbCmdImporter"
                    If MessageBox.Show(My.Resources.ConfirmImportMsg, "Confirmer l'importation des clés", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                        TsCuPrGerAccGen.ImporterCles()
                    End If
                Case "tlbCmdExporter"
                    If MessageBox.Show(My.Resources.ConfirmExportMsg, "Confirmer l'exportation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                        TsCuPrGerAccGen.ExporterCles()
                        RafraichirEtatFichierExportation()
                    End If
                Case Else
            End Select
        End Using

    End Sub

    Private Sub btnAvertissement_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAvertissement.Click
        If MessageBox.Show(My.Resources.ConfirmExportMsg, "Confirmer l'exportation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            TsCuPrGerAccGen.ExporterCles()
            RafraichirEtatFichierExportation()
        End If
    End Sub
    Private Sub mnuModifier_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuModifier.Click
        ModifierCle()
    End Sub

    Private Sub mnuAnnuler_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAnnuler.Click
        AnnulerModifierCle()
    End Sub

    Private Sub mnuFermer_Click(ByVal sender As System.Object,
                                ByVal e As System.EventArgs) Handles mnuFermer.Click
        If Not VerifierChangement("quitter") Then
            Me.Dispose()
        End If
    End Sub

    Private Function VerifierChangement(ByRef pNmAction As String) As Boolean
        Dim indicCancel As Boolean = False

        If Not ModeEcran = TsPgagModeEcran.TsPgagMeConsultation Then
            Select Case AfficheMessage.ConfirmerAbandon("de la clé", pNmAction)
                Case XzCuAfficherMessage.XzGmConfirmerAbandon.XzGmCaNePasEnregistrer
                    IndicBloqueEvenements = True
                Case XzCuAfficherMessage.XzGmConfirmerAbandon.XzGmCaEnregistrer
                    indicCancel = Not SauvegarderCle()
                Case XzCuAfficherMessage.XzGmConfirmerAbandon.XzGmCaAnnuler
                    indicCancel = True
            End Select
        End If
        Return indicCancel
    End Function

    Private Sub cmdProfil_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdProfil.Click
        Dim objSelectAD As New TsFfSelectAD()

        IndicBloqueEvenements = True
        objSelectAD.ProfilSelect = txtProfil.Text.Split(","c).Select(Function(x) x.Trim()).ToList
        If objSelectAD.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            txtProfil.Text = String.Empty
            For Each item As String In objSelectAD.lstProfil.SelectedItems
                If txtProfil.Text.Length > 0 Then
                    txtProfil.Text &= ", "
                End If
                txtProfil.Text &= item
            Next
        End If
        IndicBloqueEvenements = False
    End Sub

    Private Sub cmdMontrerPassword_Click(ByVal sender As System.Object,
                                         ByVal e As System.EventArgs) Handles cmdMontrerPassword.Click
        IndicBloqueEvenements = True
        If txtMdp.UseSystemPasswordChar Then
            If ListeCleAffiche IsNot Nothing AndAlso
               ListeCleAffiche.Count >= 1 AndAlso
               Not ModeEcran = TsPgagModeEcran.TsPgagMeAjout AndAlso
               Not mIndicMdpNouveau Then
                txtMdp.Text = TsCuPrGerAccGen.ObtenirPassword(ListeCleAffiche(0))
            End If
            txtMdp.UseSystemPasswordChar = False
        Else
            If ListeCleAffiche IsNot Nothing AndAlso
               ListeCleAffiche.Count >= 1 AndAlso
               Not ModeEcran = TsPgagModeEcran.TsPgagMeAjout AndAlso
               Not mIndicMdpNouveau Then
                txtMdp.Text = ListeCleAffiche(0).VlMotPasCleTs
            End If
            txtMdp.UseSystemPasswordChar = True
        End If
        IndicBloqueEvenements = False
    End Sub

    Private Sub mnuEnregistrer_Click(ByVal sender As System.Object,
                                     ByVal e As System.EventArgs) Handles mnuEnregistrer.Click
        SauvegarderCle()
    End Sub

    Private Sub mnuSupprimer_Click(ByVal sender As System.Object,
                                   ByVal e As System.EventArgs) Handles mnuSupprimer.Click
        SupprimerCle()
    End Sub

    Private Sub mnuActualiser_Click(ByVal sender As System.Object,
                                    ByVal e As System.EventArgs) Handles mnuActualiser.Click
        ChargerSystemes()
    End Sub

    Private Sub mnuCouper_Click(ByVal sender As System.Object,
                                ByVal e As System.EventArgs) Handles mnuCouper.Click
        Couper()
    End Sub

    Private Sub mnuCopier_Click(ByVal sender As System.Object,
                                ByVal e As System.EventArgs) Handles mnuCopier.Click
        Copier()
    End Sub

    Private Sub mnuColler_Click(ByVal sender As System.Object,
                                ByVal e As System.EventArgs) Handles mnuColler.Click
        Coller()
    End Sub

    Private m_SystemePrecedent As String

    Private Sub txtSysteme_TextChanged(ByVal sender As System.Object,
                                       ByVal e As System.EventArgs) Handles txtSysteme.TextChanged
        If Not IndicBloqueEvenements Then
            IndicBloqueEvenements = True

            If Not txtSysteme.Verrouiller Then
                If txtCle.Verrouiller Then
                    AfficherNom()
                Else
                    Dim strText As String = If(String.IsNullOrEmpty(txtCle.Text), String.Empty, txtCle.Text)
                    If Not String.IsNullOrEmpty(m_SystemePrecedent) AndAlso strText.Length >= m_SystemePrecedent.Length Then
                        strText = strText.Substring(m_SystemePrecedent.Length)
                    End If
                    txtCle.Text = txtSysteme.Text + strText
                End If
            End If

            IndicBloqueEvenements = False
        End If

        m_SystemePrecedent = txtSysteme.Text
    End Sub

    Private Sub GererKeyDown(ByVal sender As Object,
                             ByVal e As System.Windows.Forms.KeyEventArgs) _
            Handles txtSysteme.KeyDown, txtSousSysteme.KeyDown
        Dim regex As RegularExpressions.Regex = Nothing
        Dim txt As CTRL.XZCrTextbox = DirectCast(sender, CTRL.XZCrTextbox)

        Select Case txt.Name
            Case "txtSysteme"
                regex = New RegularExpressions.Regex("^[A-Z]?$", RegularExpressions.RegexOptions.Compiled)
            Case "txtSousSysteme"
                regex = New RegularExpressions.Regex("^[A-Z0-9]?$", RegularExpressions.RegexOptions.Compiled)
        End Select

        Dim value As String

        value = TsCuPrGerAccGen.ObtenirTouche(e.KeyCode.ToString)

        If regex IsNot Nothing AndAlso
           Not e.KeyCode = Keys.Back AndAlso
           Not e.KeyCode = Keys.Left AndAlso
           Not e.KeyCode = Keys.Right AndAlso
           Not e.KeyCode = Keys.Delete AndAlso
           Not e.KeyCode = Keys.End AndAlso
           Not e.KeyCode = Keys.Home AndAlso
           Not regex.Match(value).Success Then
            e.SuppressKeyPress = True
        End If
    End Sub

    Private m_SousSystemePrecedent As String = String.Empty

    Private Sub txtSousSysteme_TextChanged(ByVal sender As System.Object,
                                           ByVal e As System.EventArgs) Handles txtSousSysteme.TextChanged
        If Not IndicBloqueEvenements Then
            IndicBloqueEvenements = True

            If Not txtSousSysteme.Verrouiller Then
                If txtCle.Verrouiller Then
                    AfficherNom()
                Else
                    Dim strText As String = txtCle.Text
                    If strText.Length >= m_SystemePrecedent.Length + m_SousSystemePrecedent.Length Then
                        strText = strText.Substring(0, m_SystemePrecedent.Length) + txtSousSysteme.Text + strText.Substring(m_SystemePrecedent.Length + m_SousSystemePrecedent.Length)
                    Else
                        strText += txtSousSysteme.Text
                    End If
                    txtCle.Text = strText
                End If
            End If

            IndicBloqueEvenements = False
        End If

        m_SousSystemePrecedent = txtSousSysteme.Text
    End Sub

    Private Sub cboConn_SelectedIndexChanged(ByVal sender As System.Object,
                                             ByVal e As System.EventArgs) Handles cboConn.SelectedIndexChanged
        If Not IndicBloqueEvenements Then
            IndicBloqueEvenements = True
            Dim blnVerrouillerCle As Boolean = True
            If Not cboConn.Verrouiller Then
                AfficherNom()
                If cboConn.ValeurSelectionnee() = "AUT" Then
                    blnVerrouillerCle = False
                    cboEnv.Verrouiller = False
                End If
            End If
            txtCle.Verrouiller = blnVerrouillerCle
            FormatCle(blnVerrouillerCle, False)
            GererControlEnv()
            IndicBloqueEvenements = False
            If cboConn.ValeurSelectionnee() = "WAPW" Or cboConn.ValeurSelectionnee() = "WAPC" Then
                GererChoixCleWebAPI()
            Else
                GererAutretypeCle()
            End If
        End If
    End Sub

    Private Sub GererChoixCleWebAPI()
        If txtSysteme.Enabled = True Then
            systeme = txtSysteme.Text
            soussysteme = txtSousSysteme.Text
            txtSysteme.Text = "XU"
            txtSysteme.Enabled = False
            txtSousSysteme.Text = "7"
            txtSousSysteme.Enabled = False
        End If
    End Sub

    Private Sub GererAutretypeCle()
        'Si le code système est desactivé, cela veux dire que le choix précedent était une clé WebAPI
        'On active le champs système et sous-système et on recupère les anciennes valeurs
        If txtSysteme.Enabled = False Then
            txtSysteme.Text = systeme
            txtSousSysteme.Text = soussysteme
            txtSysteme.Enabled = True
            txtSousSysteme.Enabled = True
        End If
    End Sub

    Private Sub AfficherNom()
        If Not ModeEcran = TsPgagModeEcran.TsPgagMeConsultation Then
            Dim cleEcran As New TsCuCle
            cleEcran.Systeme = txtSysteme.Text
            cleEcran.SousSysteme = txtSousSysteme.Text

            If cboConn IsNot Nothing AndAlso cboConn.SelectedValue IsNot Nothing Then
                cleEcran.Connection = ObtenirDesc2Connection(cboConn.SelectedValue.ToString)
            Else
                cleEcran.Connection = String.Empty
            End If

            cleEcran.Sequence = mClePrec.Sequence

            If String.IsNullOrEmpty(GetString(mClePrec.Systeme)) OrElse
               cleEcran.Sequence = 0 Then
                ObtenirNouveauNom(cleEcran)
            End If

            txtCle.Text = cleEcran.ToString
        End If
    End Sub

    ''' <summary>
    ''' Obtenir le nom de clé avec le numéro de séquence suivant selon le noeud choisi dans l'arbre des clés
    ''' </summary>
    ''' <param name="pTsCuCle">Une cle</param>
    ''' <remarks></remarks>
    Private Sub ObtenirNouveauNom(ByVal pTsCuCle As TsCuCle)
        Dim name As String = pTsCuCle.ToString

        '-----------------------------------------------------------------------
        'Dans l'abre des clés (grille), on est sur un groupe d'environnements ou une cle et
        'On veut garder le meme numero de sequence pour pouvoir créer une nouvelle clé d'un autre environnement
        '-----------------------------------------------------------------------
        Dim nomCle As String = String.Empty
        If treCle.SelectionEstUneClefOuGroupeEnvironnements(nomCle) Then
            If String.IsNullOrEmpty(pTsCuCle.Systeme) Then
                pTsCuCle.Systeme = nomCle.Substring(0, 2)
            End If
            pTsCuCle.Sequence = nomCle.ExtraireNumeroSequence(pTsCuCle)

            Return
        End If

        '-----------------------------------------------------------------------
        'Dans l'arbre des clès (grille), on est sur un systeme ou un sous-systeme
        '-----------------------------------------------------------------------
        'L'utilisateur a le type de connection "Autre" à l'écran
        If String.IsNullOrEmpty(pTsCuCle.Connection.SansBlanc()) Then
            pTsCuCle.Sequence = 1
            Return
        End If

        'L'utilisateur a choisi un type de connection dans l'écran
        Dim listeCleGrille As IList(Of TsDtCleSym) = TsCuPrGerAccGen.ObtenirListeCles()

        'Obtenir les clés qui débute par le même systeme, sous-systeme et connection pour determiner la prochaine sequence a utiliser
        Dim req As IEnumerable(Of TsDtCleSym) = From cle In listeCleGrille
                                                Where cle.CoIdnCleSymTs.StartsWith(name) AndAlso
                                                    cle.CoSysCleSymTs = pTsCuCle.Systeme AndAlso
                                                    cle.CoSouCleSymTs.SansBlanc = pTsCuCle.SousSysteme
                                                Order By cle.CoIdnCleSymTs.ExtraireNumeroSequence(pTsCuCle) Descending

        If req.Count = 0 Then
            'il n'y pas de clé avec un nom similaire (systeme, sous-systeme, connection)
            pTsCuCle.Sequence = 1

        ElseIf req.Count > 0 Then
            'Obtenir la prochaine sequence pour une cle similaire (systeme, sous-systeme, connection)
            Dim cle As TsDtCleSym = req(0)
            pTsCuCle.Sequence = cle.CoIdnCleSymTs.ExtraireNumeroSequence(pTsCuCle) + 1
        End If
    End Sub

    ''' <summary>
    ''' Choisir la ligne de la clé dans la grille
    ''' </summary>
    ''' <param name="cle"></param>
    ''' <remarks></remarks>
    Friend Sub ChoisirCle(ByVal cle As TS1N201_DtCdAccGenV1.TsDtCleSym)
        Dim detailGridSys As Xceed.Grid.DetailGrid = Nothing
        Dim detailGridSou As Xceed.Grid.DetailGrid = Nothing

        For Each row As Xceed.Grid.DataRow In treCle.DataRows
            If GetString(row.Cells(My.Resources.NomSysteme).Value) = cle.CoSysCleSymTs Then
                detailGridSys = row.DetailGrids(0)
                detailGridSys.Expand()
                Exit For
            End If
        Next

        If detailGridSys IsNot Nothing Then
            If String.IsNullOrEmpty(GetString(cle.CoSouCleSymTs)) Then
                detailGridSou = detailGridSys.DataRows(0).DetailGrids(0)
                If detailGridSou IsNot Nothing Then detailGridSou.Expand()
            Else
                For Each row As Xceed.Grid.DataRow In detailGridSys.DataRows
                    If GetString(row.Cells(My.Resources.NomSousSysteme).Value) = cle.CoSouCleSymTs Then
                        detailGridSou = row.DetailGrids(0)
                        detailGridSou.Expand()
                        Exit For
                    End If
                Next
            End If
        End If

        If detailGridSou IsNot Nothing Then
            For Each row As Xceed.Grid.DataRow In detailGridSou.DataRows
                Dim nmCompare As String = GetString(row.Cells(My.Resources.NomCle).Value)
                If nmCompare.StartsWith(cle.CoIdnCleSymTs) Then
                    treCle.SelectedRows.Clear()
                    treCle.SelectedRows.Add(row)
                ElseIf nmCompare.StartsWith(cle.CoIdnCleSymTs.Substring(0, cle.CoIdnCleSymTs.Length - 1)) Then
                    treCle.SelectedRows.Clear()
                    treCle.SelectedRows.Add(row)
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Abandonner la modification, passe en mode consultation
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub AbandonModif()
        ModeEcran = TsPgagModeEcran.TsPgagMeConsultation
    End Sub

    Private Sub cmdEditMultiple_Click(ByVal sender As System.Object,
                                      ByVal e As System.EventArgs) Handles cmdEditMultiple.Click
        Dim multiEnv As New TsFgMultiEnv(AccesNiveau1)
        multiEnv.ShowDialog(Me)
    End Sub

    ''' <summary>
    ''' Survient lorsque le menu "Importer des clés à partir d'un fichier" est cliqué
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub mnuImporterFichier_Click(sender As System.Object, e As System.EventArgs) Handles mnuImporterFichier.Click

        ofdOuvrirFichier.CheckFileExists = True
        ofdOuvrirFichier.CheckPathExists = True
        ofdOuvrirFichier.AddExtension = True
        ofdOuvrirFichier.DefaultExt = "csv"
        ofdOuvrirFichier.Filter = "Fichier csv|*.csv"
        ofdOuvrirFichier.FileName = String.Empty

        If ofdOuvrirFichier.ShowDialog() = Windows.Forms.DialogResult.OK Then

            ImporterFichier(ofdOuvrirFichier.FileName.Replace(ofdOuvrirFichier.SafeFileName, String.Empty), ofdOuvrirFichier.SafeFileName)

        End If

    End Sub

    ''' <summary>
    ''' Importe les clés définies dans un fichier de type CSV
    ''' </summary>
    ''' <param name="repertoire">Répertoire qui héberge le fichier à importer</param>
    ''' <param name="nomFichier">Nom du fichier à importer, sans le répertoire</param>
    Private Sub ImporterFichier(ByVal repertoire As String, ByVal nomFichier As String)

        Dim importation As New TsFfImportation(repertoire, nomFichier, DtbConnection)
        importation.ShowDialog()

        ChargerSystemes()
        RafraichirEtatFichierExportation()

    End Sub

    ''' <summary>
    ''' Rafraichit les informations affichées à l'écran concernant l'état du fichier d'exportation
    ''' </summary>
    Private Sub RafraichirEtatFichierExportation()

        Dim etat As TS1N201_DtCdAccGenV1.TsDtEtaFicExp = TsCuPrGerAccGen.ObtenirEtatFichierExportation()

        btnAvertissement.Visible = Not etat.InFicJou

        If etat.InFicJou Then
            lblAvertissement.ForeColor = Color.Green
            lblAvertissement.Text = String.Format("Le fichier d'exportation des clés symbolique est à jour en date du {0}", etat.DtDerFicExp)
        Else
            lblAvertissement.ForeColor = Color.Red
            lblAvertissement.Text = String.Format("Le fichier d'exportation des clés symbolique en date du {0} n'est pas à jour", etat.DtDerFicExp)
        End If

    End Sub

#End Region

#Region "--- Substitutions ---"

    '''-----------------------------------------------------------------------------
    ''' <summary>
    ''' Traitement declenché lorsque l'utilisateur demande de l'aide.
    ''' </summary>
    ''' <remarks></remarks>
    '''-----------------------------------------------------------------------------
    Protected Overrides Sub TraiterAide()

        XzCaAfficherAide.AfficherAide("")

    End Sub

#End Region

#Region "--- Implémentations de l'interface  ""XzIBindingForm""  ---"

    ''' --------------------------------------------------------------------------------
    ''' <summary>
    ''' Propriété qui doit contenir une référence à toutes les sources de données nécessaires
    ''' pour le XzBinding automatique.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    ''' --------------------------------------------------------------------------------
    Public ReadOnly Property XzBindingSources() As System.Collections.Hashtable Implements XzIBindingForm.XzBindingSources
        Get
            If mColSourceDonnee Is Nothing Then
                '$RRQ-SUGGESTION : mettre en paramètre du constructeur la nombre
                'de sources de données, ceci est plus optimale...
                'mColSourceDonnee = New Hashtable(2)
                mColSourceDonnee = New Hashtable
            End If
            Return mColSourceDonnee
        End Get
    End Property

#End Region

#Region "--- Implémentation de l'interface  ""XzIBesoinValid""  ---"

    ''' --------------------------------------------------------------------------------
    ''' <summary>
    ''' Spécifie si les contrôles du formulaire requiert par défaut d'être validé.
    ''' </summary>
    ''' <value></value>
    ''' <remarks>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    Public ReadOnly Property NecessiteValidation() As Boolean Implements XzIBesoinValid.NecessiteValidation
        Get
            '$RRQ_ACTION : Doit spécifier si les contrôles du formulaire requiert par défaut de la validation

            '$RRQ_EXEMPLE_DEBUT :
            'Return True
            '$RRQ_EXEMPLE_FIN
        End Get
    End Property

#End Region

    ''' <summary>
    ''' Lors d'un changement du mot de passe on doit afficher la valeur saisie et nom celle qui est encodé en mémoire
    ''' </summary>
    ''' <param name="sender">sender</param>
    ''' <param name="e">e</param>
    Private Sub txtMdp_TextChanged(ByVal sender As Object,
                                   ByVal e As System.EventArgs) Handles txtMdp.TextChanged
        If Not IndicBloqueEvenements Then
            IndicBloqueEvenements = True
            mIndicMdpNouveau = True
            IndicBloqueEvenements = False
        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub txtCle_TextChanged(ByVal sender As Object,
                                   ByVal e As System.EventArgs) Handles txtCle.LostFocus
        If txtCle.Verrouiller = False AndAlso Not IndicBloqueEvenements Then
            IndicBloqueEvenements = True
            Dim strTexte As String = txtCle.Text
            Dim intSystemLong As Integer = txtSysteme.Text.Length
            If intSystemLong > 0 AndAlso (strTexte.Length < intSystemLong OrElse strTexte.Substring(0, intSystemLong) <> txtSysteme.Text) Then
                strTexte = txtSysteme.Text + strTexte
            End If
            Dim intSousSystemLong As Integer = txtSousSysteme.Text.Length
            If intSousSystemLong > 0 AndAlso (strTexte.Length < (intSystemLong + intSousSystemLong) OrElse
                                              strTexte.Substring(intSystemLong, intSousSystemLong) <> txtSousSysteme.Text) Then
                strTexte = strTexte.Insert(intSystemLong, txtSousSysteme.Text)
            End If
            If strTexte <> txtCle.Text Then
                txtCle.Text = strTexte
            End If
            IndicBloqueEvenements = False
        End If
    End Sub

    Private Sub mnuRechercher_Click(ByVal sender As System.Object,
                                    ByVal e As System.EventArgs) Handles mnuRechercher.Click
        RechercheCle()
    End Sub

    ''' <summary>
    ''' Clic du bouton générer mot de passe
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub cmdGenererMdp_Click(sender As System.Object, e As System.EventArgs) Handles cmdGenererMdp.Click

        Dim motDePasse As String = TsFfGenMotPasse.AfficherGenererMotPasse(Me, 15)

        If Not String.IsNullOrEmpty(motDePasse) Then
            txtMdp.Text = motDePasse
        End If

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub mnuForcerEdition_Click(sender As System.Object, e As System.EventArgs) Handles mnuForcerEdition.Click

        If MessageBox.Show("Désirez-vous passer en mode édition malgré le fait qu'un autre utilisateur utilise déjà la console de gestion des clés symboliques ?" + Environment.NewLine + Environment.NewLine +
                           "Cette opération ne bloquera pas les autres utilisateurs qui utilisent la console.", "Déverrouillage", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            mVerrouForce = True
            EnableCmdAnnuler(False)
            GererControl()
            mnuForcerEdition.Visible = False
        End If

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub chkCompteAD_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkCompteAD.CheckedChanged

        ' Si AD n'est pas activé, on ne devrait pas pouvoir créer de compte AD/LDS
        If chkCompteAD.Checked Then
            chkCompteADLDS.Enabled = mIndCreationAdLds AndAlso mIndCreationAd
        Else
            chkCompteADLDS.Checked = False
            chkCompteADLDS.Enabled = False
        End If

    End Sub

End Class