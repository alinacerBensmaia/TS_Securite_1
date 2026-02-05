
''' <summary>
''' Ce producteur permet de construire, la feuille de travail principale du rapport.
''' </summary>
''' <remarks></remarks>
Friend Class TsCaFeuillePrincipale
    Implements TsICompositeurFeuille

#Region "--- Variables ---"

    ''' <summary>Source de la feuille de travail.</summary>
    Private mSource As TsDtSourcFeuilPrinc

#End Region

#Region "--- Constructeur ---"

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <param name="pSource">Source pour la feuille de travail.</param>
    ''' <remarks></remarks>
    Public Sub New(pSource As TsDtSourceRapport)
        Dim contexte As String = ""

        mSource = DirectCast(pSource, TsDtSourcFeuilPrinc)
    End Sub

#End Region

#Region "--- Méthodes publiques ---"

    ''' <summary>
    ''' Permet de produire la feuille de travail.
    ''' </summary>
    ''' <returns>Une feuille de travail.</returns>
    ''' <remarks></remarks>
    Public Function ProduireFeuilleTravail() As TsCuWorksheet Implements TsICompositeurFeuille.ProduireFeuilleTravail
        Dim feuille As TsCuWorksheet
        Dim livreDeTravail As TsCuWorkbook

        feuille = New TsCuWorksheet(TsCuConstantesRapport.NOM_FEUILLE_1)

        AjouterLaTable(feuille)

        livreDeTravail = New TsCuWorkbook(feuille)

        Return feuille
    End Function

#End Region

#Region "--- Sous-méthodes ---"

    ''' <summary>
    ''' Permet d'ajouter la table du rapport à une feuille de travail.
    ''' </summary>
    ''' <param name="pFeuilleTravail">Un livre de travail Excel.</param>
    ''' <remarks></remarks>
    Private Sub AjouterLaTable(ByVal pFeuilleTravail As TsCuWorksheet)
        Dim table As New TsCuTable()

        AjouterColonnes(table)

        AjouterLignes(table)

        pFeuilleTravail.Table = table
    End Sub

    ''' <summary>
    ''' Permet d'ajouter les lignes d'une table.
    ''' </summary>
    ''' <param name="pTable">Une table d'une feuillle de travail.</param>
    ''' <remarks></remarks>
    Private Sub AjouterColonnes(ByVal pTable As TsCuTable)

        With pTable.Columns

            ' ---- Sous-entête coupée
            .Add(TsCaColonnesRapport.NomResources())
            .Add(TsCaColonnesRapport.NomFonctionnelResources())

            ' ---- Grille 
            AjouterColonnesGrille(pTable.Columns, mSource.Employes)


        End With

    End Sub

    ''' <summary>
    ''' Permet d'ajouter les lignes de la table du rapport.
    ''' </summary>
    ''' <param name="pTable">La table du rapport.</param>
    ''' <remarks></remarks>
    Private Sub AjouterLignes(ByVal pTable As TsCuTable)
        Dim concepteurLignes As New TsCaLignesRapport(mSource)

        With pTable.Rows
            '    ' ---- Entête Tableau
            .Add(concepteurLignes.ObtenirEnteteTableau())

            '    ' ---- Afficher les employés
            .Add(concepteurLignes.ObtenirEmployesSelectionnes(mSource))

            '' ---- Ressources
            If mSource.LstRessources.Count > 0 Then

                For Each Res In mSource.LstRessources

                    .Add(concepteurLignes.ObtenirRessourcesAssociees(Res, mSource))

                Next

            End If

            .Add(concepteurLignes.ObtenirTotaux())
        End With

    End Sub


#End Region

#Region "--- Fonctions privées ---"


    ''' <summary>
    ''' Fonction de service. Permet d'ajouter une colonne de la grille du rapport.
    ''' </summary>
    ''' <typeparam name="T">Type générique.</typeparam>
    ''' <param name="pLstColonnes">La liste des colonnes qui recevera les nouvelle colonnes.</param>
    ''' <param name="liste">Liste d'éléments qui donne le nombre de colonnes à ajouter.</param>
    ''' <remarks>Le type T n'a pas d'importance.</remarks>
    Private Sub AjouterColonnesGrille(Of T)(ByVal pLstColonnes As List(Of TsCuColumn), ByVal liste As IEnumerable(Of T))
        For Each c In liste
            pLstColonnes.Add(TsCaColonnesRapport.Grille())
        Next
    End Sub




#End Region


End Class
