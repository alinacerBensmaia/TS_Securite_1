Imports TS1N224 = Rrq.Securite
Imports Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ
Imports System.Collections.Generic
Imports System.Globalization

'''-----------------------------------------------------------------------------
''' Project		: TS1N214 Gestion des clés symbolique
''' Class		: TsFfRech
''' 
'''-----------------------------------------------------------------------------
''' <summary>
''' Classe de recherche des clés symbolique
''' ''' </summary>
''' <remarks></remarks>
''' <history>
'''  Demande  -    Date    -       Auteur        - Description de la modification
''' --------- - ---------- - ------------------- - ------------------------------
''' [      ]  - 2013-08-21   Patrick Thibault    - Création initiale (Importation de l'écran de TS1N214 et modification)
''' </history>
Friend Class TsFfRech

#Region " Constructeurs"
    Friend Sub New(ByRef pFenetreGestion As TsFdGerAccGen)
        MyBase.New()

        'Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        'Ajoutez une initialisation quelconque après l'appel InitializeComponent()
        mFenetreGestion = pFenetreGestion
    End Sub
#End Region

#Region " Énumérations "
    Private Enum TsPgagTypeRecherche
        TsPgagTrType = 0
        TsPgagTrCle = 1
        TsPgagTrEnvironnement = 2
        TsPgagTrCode = 3
        TsPgagTrProfil = 4
    End Enum
#End Region

#Region " Variables privées "
    Private mFenetreGestion As TsFdGerAccGen
#End Region

#Region "Propriétés"
    ''' <summary>
    ''' Propriété pour obtenir une référence sur l'écran de gestion des clé symbolique
    ''' </summary>
    ''' <value>Instance de TsFdGerAccGen</value>
    ''' <returns>Instance de TsFdGerAccGen</returns>
    ''' <remarks></remarks>
    Private Property FenetreGestion() As TsFdGerAccGen
        Get
            Return mFenetreGestion
        End Get
        Set(ByVal value As TsFdGerAccGen)
            mFenetreGestion = value
        End Set
    End Property
#End Region

#Region " Méthodes privées "
    ''' <summary>
    '''     Méthode utilisée pour initialiser la liste dans la forme.
    ''' </summary>
    ''' --------------------------------------------------------------------------------
    Private Sub InitListe()

        ' ------------------------------------------------------------------------------
        ' Mettre la liste en mode initialisation
        ' ------------------------------------------------------------------------------
        grdResultat.BeginInit()


        '-------------------------------------------------------------------------------
        ' Associer un gabarit de donnée vide pour ajouter les colonnes.
        ' ------------------------------------------------------------------------------
        grdResultat.DataSource = TsCuRecherche.ObtenirGrilleRechercheVide

        ' ------------------------------------------------------------------------------
        ' Définiton des propriétés de la liste.
        ' ------------------------------------------------------------------------------
        With grdResultat
            ' Enlever les lignes séparatrices de la liste
            .GridLineColor = Color.DarkGray

            ' Paramètre de la colonne de sélection "RowSelectorPane"
            .RowSelectorPane.Visible = True
            .RowSelectorPane.AllowRowResize = False

            ' Permetre la sélection d'une seule ligne à la fois
            .SelectionMode = SelectionMode.One

            ' Changer la couleur de la sélection lorque la ligne n'a pas le focus
            '*** Pris en charge par XZ5N909 .InactiveSelectionBackColor = lstEntPri.SelectionBackColor
            '*** Pris en charge par XZ5N909 .InactiveSelectionForeColor = lstEntPri.SelectionForeColor

            ' Interdire la navigation par cellule
            .AllowCellNavigation = False

            ' Forcer la barre de défilement vertical
            '.ScrollBars = Xceed.Grid.GridScrollBars.ForcedVertical

            ' Enlever le séparateur de colonne
            '*** Pris en charge par XZ5N909 .FixedColumnSplitter.Visible = False

            ' Mettre la grille en lecture seul 
            .ReadOnly = True

        End With

        ' ------------------------------------------------------------------------------
        ' Propriété du gestionnaire d'entêtes de colonne.
        ' ------------------------------------------------------------------------------
        ' Interdire la modification de la postion et de la largeure des colonnes
        With cmrResultat
            .AllowColumnResize = True
            .AllowColumnReorder = False

            ' Interdire le "drag-and-drop"
            .AllowDrop = False

            ' Permettre le tri
            .AllowSort = True

            ' Ajuster l'alignement à gauche des entêtes de colonne
            .HorizontalAlignment = Xceed.Grid.HorizontalAlignment.Left
        End With

        ' ------------------------------------------------------------------------------
        ' Propriétés du entête et pied de grille.
        ' ------------------------------------------------------------------------------        
        txtrResulNouveau.Visible = False
        txtrResulNouveau.CanBeSelected = True
        txtrResulNouveau.Text = "Création en cours ..."

        ' ------------------------------------------------------------------------------
        ' Propriétés du modèle de lignes ("DataRowTemplate").
        ' ------------------------------------------------------------------------------
        With drtResultat
            ' Déactiver le concept de ligne courante pour gerer le changement de la ligne
            ' courante par le double clique sur la liste
            .CanBeCurrent = False

            ' Ajuster l'alignement à gauche des cellules de la liste
            .HorizontalAlignment = Xceed.Grid.HorizontalAlignment.Left
        End With


        'On définit les colonnes de la liste.
        DefinirColonnes()


        ' ------------------------------------------------------------------------------
        ' Définiton des Handlers.
        ' ------------------------------------------------------------------------------
        ' Attacher un Handler sur l'événement DoubleClick des DataCell
        Dim cell As Xceed.Grid.DataCell
        For Each cell In grdResultat.DataRowTemplate.Cells
            AddHandler cell.DoubleClick, AddressOf Me.grdResultat_DoubleClick
        Next cell

        ' ------------------------------------------------------------------------------
        ' Terminer le mode initialisation
        ' ------------------------------------------------------------------------------
        grdResultat.EndInit()

    End Sub

    ''' <summary>
    ''' Méthode pour définir les colonnes de la liste
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DefinirColonnes()

        ' ------------------------------------------------------------------------------
        ' Définiton des colonnes

        ' *** Définiton de la colonne Type ***
        With grdResultat.Columns("Type")
            .Width = 188      ' Largeur
            .Title = "Type"   ' Titre
            .VisibleIndex = 0 ' Index d'affichage

            ' Utilisation du visualisateur RRQ                        
            'On veut afficher la description du type
            .CellViewerManager = New Grilles.Visualisateurs.XzCrListeCombinee(TsCuRecherche.RemplirDataViewType, "CodeType", "%Desc%")
        End With


        ' *** Définiton de la colonne Cle ***
        With grdResultat.Columns("Cle")

            ' Lageur
            .Width = 81
            ' Titre
            .Title = "Clé"
            ' Index d'affichage
            .VisibleIndex = 1

            ' Utilisation du visualisateur RRQ
            .CellViewerManager = New Grilles.Visualisateurs.XzCrTexte
        End With


        With grdResultat.Columns("Env")

            ' Lageur
            .Width = 121
            ' Titre
            .Title = "Environnement"
            ' Index d'affichage
            .VisibleIndex = 2

            ' Utilisation du visualisateur RRQ
            ' On veut afficher la description de l'environnement.
            .CellViewerManager = New Grilles.Visualisateurs.XzCrListeCombinee(TsCuRecherche.RemplirDataViewEnvironnement, "CodeEnv", "%Desc%")
        End With


        With grdResultat.Columns("Code")
            ' Lageur
            .Width = 81
            ' Titre
            .Title = "Code"
            ' Index d'affichage
            .VisibleIndex = 3

            ' Modification du CellViewerManager            
            .CellViewerManager = New Grilles.Visualisateurs.XzCrTexte
        End With

        ' *** Définiton de la colonne Profil ***

        With grdResultat.Columns("Profil")
            ' Lageur
            .Width = 139
            ' Titre
            .Title = "Profil"
            ' Index d'affichage
            .VisibleIndex = 5

            ' Utilisation du visualisateur RRQ
            .CellViewerManager = New Grilles.Visualisateurs.XzCrTexte
        End With
    End Sub

    

    

    ''' <summary>
    ''' Permet de lier un table de donnée à la liste.
    ''' Si cette table est nulle ou vide on lie le gabarit à la liste.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RemplirGrille()
        Dim tblDonnee As DataTable = Nothing
        Dim codeEnv As String = String.Empty
        Dim codeType As String = String.Empty

        If cboType.SelectedValue IsNot Nothing Then
            codeType = cboType.SelectedValue.ToString
        End If

        If cboEnv.SelectedValue IsNot Nothing Then
            codeEnv = cboEnv.SelectedValue.ToString
        End If

        tblDonnee = TsCuRecherche.ObtenirCleRecherche(codeType, _
                                                      codeEnv, _
                                                      txtProfil.Text, _
                                                      txtCle.Text, _
                                                      txtCode.Text)

        If (tblDonnee Is Nothing OrElse _
            tblDonnee.Rows.Count = 0) Then
            'Aucune donnée donc on affiche le gabarit
            grdResultat.DataSource = TsCuRecherche.ObtenirGrilleRechercheVide
        Else
            'On affiche les donneés de recherche.
            grdResultat.DataSource = tblDonnee
        End If

    End Sub

    Private Sub cmdRechercher_Click(ByVal sender As System.Object, _
                                    ByVal e As System.EventArgs) Handles cmdRechercher.Click
        RemplirGrille()
    End Sub

    Private Sub cmdFermer_Click(ByVal sender As System.Object, _
                                ByVal e As System.EventArgs) Handles cmdFermer.Click
        Me.Hide()
        Me.Dispose()
    End Sub

    Private Sub cmdProfil_Click(ByVal sender As System.Object, _
                                ByVal e As System.EventArgs) Handles cmdProfil.Click
        Dim objSelectAD As New TsFfSelectAD()

        If objSelectAD.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            txtProfil.Text = String.Empty
            For Each item As String In objSelectAD.lstProfil.SelectedItems
                If txtProfil.Text.Length > 0 Then
                    txtProfil.Text &= ", "
                End If
                txtProfil.Text &= item
            Next
        End If

    End Sub

    Private Sub grdResultat_DoubleClick(ByVal sender As System.Object, _
                                        ByVal e As System.EventArgs) Handles grdResultat.DoubleClick

        Dim cle As String = String.Empty
        Dim enrgsCourant As Xceed.Grid.CellRow = DirectCast(sender, Xceed.Grid.Cell).ParentRow
        Dim tsDtCleSymTrouve As TS1N201_DtCdAccGenV1.TsDtCleSym = Nothing

        cle = enrgsCourant.Cells("Cle").Value.ToString

        tsDtCleSymTrouve = TsCuPrGerAccGen.ObtenirCle(cle)

        If tsDtCleSymTrouve IsNot Nothing Then
            FenetreGestion.ChoisirCle(tsDtCleSymTrouve)
        End If
        cmdFermer.PerformClick()
    End Sub

    ''' <summary>
    ''' Chargement de la fenetre de recherche
    ''' </summary>
    ''' <param name="sender">sender</param>
    ''' <param name="e">e</param>
    ''' <remarks></remarks>
    Private Sub TsFfRech_Load(ByVal sender As System.Object, _
                              ByVal e As System.EventArgs) Handles MyBase.Load
        TsCuPrGerAccGen.RemplirCboTypeCle(cboType, True)
        TsCuPrGerAccGen.RemplirCboEnvironnement(cboEnv)
        InitListe()
        cboType.Select()
    End Sub

#End Region

End Class
