
''' <summary>
''' Ce producteur permet de construire, la feuille de travail secondaire du rapport.
''' </summary>
''' <remarks></remarks>
Friend Class TsCaFeuilleSecondaire
    Implements TsICompositeurFeuille

#Region "--- Variables ---"

    ''' <summary>Source de la feuille de travail.</summary>
    Private mSource As TsDtSourceRapport


#End Region

#Region "--- Constructeur ---"

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <param name="pSource">Source pour la feuille de travail.</param>
    ''' <remarks></remarks>
    Public Sub New(pSource As TsDtSourceRapport)
        Dim contexte As String = ""

        mSource = pSource
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
        Dim formatConditionne As TsCaFormatConditionneRapport
        Dim livreDeTravail As TsCuWorkbook

        feuille = New TsCuWorksheet(TsCuConstantesRapport.NOM_FEUILLE_2)
        formatConditionne = New TsCaFormatConditionneRapport(mSource)

        AjouterZonesDeNom(feuille)
        AjouterLaTable(feuille)

        livreDeTravail = New TsCuWorkbook(feuille)

        AjouterOptions(feuille)

        Return feuille
    End Function

#End Region

#Region "--- Sous-méthodes ---"

    ''' <summary>
    ''' Permet d'obtenir l'en-tête du rapport.
    ''' </summary>
    ''' <param name="pLstNoUA">La liste des numéro d'unité administrative du rapport.</param>
    ''' <remarks></remarks>
    Private Function ObtenirEnteteRapport(ByVal pLstNoUA As List(Of String)) As String
        Dim msgEntete As String
        If pLstNoUA.Count = 1 Then
            msgEntete = "Assignation des rôles aux employés de l'unité "
        Else
            msgEntete = "Assignation des rôles aux employés des unités "
        End If
        For i = 0 To pLstNoUA.Count - 1
            If i = pLstNoUA.Count - 1 Then
                msgEntete &= String.Format("{0} ", pLstNoUA(i))
            ElseIf i = pLstNoUA.Count - 2 Then
                msgEntete &= String.Format("{0} et ", pLstNoUA(i))
            Else
                msgEntete &= String.Format("{0}, ", pLstNoUA(i))
            End If
        Next
        msgEntete &= "au " & mSource.DateProduction.ToString("yyyy-MM-dd")

        Return msgEntete
    End Function

    ''' <summary>
    ''' Permet d'ajouté les option de la feuille, dont ceux de l'impression.
    ''' </summary>
    ''' <param name="pPage">La page, dont les options seront attribués.</param>
    ''' <remarks></remarks>
    Private Sub AjouterOptions(ByVal pPage As TsCuWorksheet)
        With pPage
            .WorksheetOptions = New TsCuWorkSheetOptions()
            With .WorksheetOptions
                .PageSetup = New TsCuPageSetup()
                With .PageSetup
                    .Footer = New TsCuFooter(0.2)
                    .Footer.Data = "&amp;CPage &amp;P de &amp;N"

                    .Header = New TsCuHeader(0.2)
                    .Header.Data = "&amp;C" & ObtenirEnteteRapport(mSource.LstUaDemander)

                    .Layout = New TsCuLayout()
                    .Layout.Orientation = TsCuLayout.OrientationType.Landscape

                    .PageMargins = New TsCuPageMargins()
                    With .PageMargins
                        .Bottom = 0.4
                        .Left = 0.2
                        .Right = 0.2
                        .Top = 0.4
                    End With
                End With

                .Print = New TsCuPrint()
                With .Print
                    Dim legende As Integer = 4

                    .LeftToRight = True
                    .PaperSizeIndex = 5
                    .FitHeight = CInt(Math.Ceiling((mSource.Employes.Count + mSource.Contextes.Count + legende) / 15))
                    .FitWidth = CInt(Math.Ceiling(mSource.RoleUaDisponibles.Count / 25))
                End With

                .FitToPage = True
                .ActivePane = 0
                .FreezePanes = True
                .SlipVertical = 3
                .SplitHorizontal = 4
                .LeftColumnRightPane = 3
                .TopRowBottomPane = 4
            End With
        End With
    End Sub

    ''' <summary>
    ''' Permet d'ajouté des Zones nommées.
    ''' </summary>
    ''' <param name="pFeuilleTravail">Un livre de travail Excel.</param>
    ''' <remarks></remarks>
    Private Sub AjouterZonesDeNom(ByVal pFeuilleTravail As TsCuWorksheet)
        Dim zoneDeNomImpresion As New TsCuNamedRange()
        Dim zoneDeNomBarrer As New TsCuNamedRange()

        With zoneDeNomImpresion
            .Name = TsCuConstantesRapport.NAMED_PRINT_AREA
            .RefersTo = String.Format("='{2}'!R1C1:R{0}C{1}", ObtenirFinZoneBas(), ObtenirFinZoneGauche(), TsCuConstantesRapport.NOM_FEUILLE_2)
        End With

        With zoneDeNomBarrer
            .Name = TsCuConstantesRapport.NAMED_PRINT_TITILES
            .RefersTo = String.Format("='{0}'!C1:C2,'{0}'!R1:R4", TsCuConstantesRapport.NOM_FEUILLE_2)
        End With

        pFeuilleTravail.Names.Add(zoneDeNomImpresion)
        pFeuilleTravail.Names.Add(zoneDeNomBarrer)
    End Sub

    ''' <summary>
    ''' Permet d'ajouter la table du rapport à une feuille de travail.
    ''' </summary>
    ''' <param name="pFeuilleTravail">Un livre de travail Excel.</param>
    ''' <remarks></remarks>
    Private Sub AjouterLaTable(ByVal pFeuilleTravail As TsCuWorksheet)
        Dim table As New TsCuTable()

        With table
            .ExpandedColumnCount = CULng(ObtenirFinZoneGauche())
            .ExpandedRowCount = CULng(ObtenirFinZoneBas())
            .FullColumns = True
            .FullRows = True
            .DefaultColumnWidth = 60
        End With

        AjouterColonnes(table)

        AjouterLignes(table)

        'T208704
        'Réajuster le ExpandedColumnCount de la table en fonction du texte des légendes
        'Raison : Excel ne permet pas que le texte des légendes dépasse le nombre de colonne de la table.
        With table
            .ExpandedColumnCount = TsCuOutilsRapport.CalculerNbreColonneMinimum(table.ExpandedColumnCount)
        End With

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
            .Add(TsCaColonnesRapport.NomEmployes())
            .Add(TsCaColonnesRapport.UA())
            .Add(TsCaColonnesRapport.Separateur())

            ' ---- Grille - Métiers sélectionnés
            AjouterColonnesGrille(pTable.Columns, mSource.ObtenirMetiersSelectionnes)

            ' ---- Séparateur - Sélectionnés
            AjouterColonneSeparateur(pTable.Columns, mSource.ObtenirMetiersSelectionnes, mSource.ObtenirRolesTachesSelectionnes)

            ' ---- Grille - Tâches sélectionnés
            AjouterColonnesGrille(pTable.Columns, mSource.ObtenirRolesTachesSelectionnes)

            ' ---- Séparateur - Sélectionnés/Autres
            Dim colonneSelectionnes As New List(Of TsDtSourceUa)(mSource.ObtenirMetiersSelectionnes())
            colonneSelectionnes.AddRange(mSource.ObtenirRolesTachesSelectionnes())

            Dim colonneAutre As New List(Of TsDtSourceUa)(mSource.ObtenirMetiersAutre())
            colonneAutre.AddRange(mSource.ObtenirRolesTachesAutre())
            AjouterColonneSeparateur(pTable.Columns, colonneSelectionnes, colonneAutre)

            ' ---- Grille - Metiers autres
            AjouterColonnesGrille(pTable.Columns, mSource.ObtenirMetiersAutre)

            ' ---- Séparateur - Autres
            AjouterColonneSeparateur(pTable.Columns, mSource.ObtenirMetiersAutre, mSource.ObtenirRolesTachesAutre)

            ' ---- Grille - Tâches autres
            AjouterColonnesGrille(pTable.Columns, mSource.ObtenirRolesTachesAutre)

            pTable.Columns.Add(New TsCuColumn() With {.Width = 1})

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
            ' ---- Entête Tableau
            .Add(concepteurLignes.ObtenirEnteteTableau())
            .Add(concepteurLignes.ObtenirSousEnteteTableau())

            ' ---- Unités Administatives
            .Add(concepteurLignes.ObtenirUA())
            If mSource.PossedeAssociationEtrangere() = True Then
                With .Last.Cells.First
                    .Data = New TsCuData(TsCuData.TypeType.String) With {.Contenue = "**** Au moins un rôle (en surligné) octroyé en dehors de ses unités responsables"}
                    .StyleID = TsCuConstantesRapport.ConstantesStyle.CELLULE_HYPERLIEN
                End With
            End If

            ' ---- Entête coupée
            .Add(concepteurLignes.ObtenirEnteteCoupe())
            With .Last.Cells.First
                .Data = New TsCuData(TsCuData.TypeType.String) With {.Contenue = "Employés d'autres unités que celles sélectionnées"}
            End With

            ' ---- Employés
            If mSource.Employes.Count > 0 Then
                Dim premier As Boolean = True
                Dim dernier As String = mSource.Employes.Last.Nom
                For Each e In mSource.Employes
                    Select Case True
                        Case premier = True
                            premier = False
                            .Add(concepteurLignes.ObtenirEmployeHaut(e))
                        Case dernier = e.Nom
                            .Add(concepteurLignes.ObtenirEmployeBas(e))
                        Case Else
                            .Add(concepteurLignes.ObtenirEmploye(e))
                    End Select
                Next
            End If

            .Add(concepteurLignes.ObtenirTotaux())


            For i = 0 To mSource.Contextes.Count - 1
                .Add(concepteurLignes.ObtenirContextes(mSource.Contextes(i), i + 1))
            Next

            .Add(concepteurLignes.ObtenirFinRapport())
            .Add(concepteurLignes.Espacement())
            If mSource.Contextes.Count <> 0 Then .Add(concepteurLignes.ObtenirLegende1())
        End With

    End Sub

#End Region

#Region "--- Fonctions privées ---"

    ''' <summary>
    ''' Fonction de service. Permet d'obtenir la dernière colonne du rapport.
    ''' </summary>
    Private Function ObtenirFinZoneGauche() As Integer
        Return TsCuOutilsRapport.ObtenirLargeurTableau(mSource.ObtenirMetiersSelectionnes.Count, _
                                                       mSource.ObtenirRolesTachesSelectionnes.Count, _
                                                       mSource.ObtenirMetiersAutre.Count, _
                                                       mSource.ObtenirRolesTachesAutre.Count) + 1
    End Function

    ''' <summary>
    ''' Fonction de service. Permet d'obtenir la dernière ligne du rapport.
    ''' </summary>
    Private Function ObtenirFinZoneBas() As Integer
        Dim tailleEnteteRapport = 9
        Return tailleEnteteRapport + mSource.Employes.Count + mSource.Contextes.Count
    End Function

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

    ''' <summary>
    ''' Fonction de service. Permet d'ajouter une colonne séparateur.
    ''' </summary>
    ''' <typeparam name="T">Type de la liste 1.</typeparam>
    ''' <typeparam name="S">Type de la liste 2.</typeparam>
    ''' <param name="pLstColonnes">La liste des colonnes qui recevera les nouvelle colonnes.</param>
    ''' <param name="liste1">Liste d'éléments 1.</param>
    ''' <param name="liste2">Liste d'éléments 2.</param>
    ''' <remarks>Les types T et S n'ont pas d'importances.</remarks>
    Private Sub AjouterColonneSeparateur(Of T, S)(ByVal pLstColonnes As List(Of TsCuColumn), ByVal liste1 As IEnumerable(Of T), ByVal liste2 As IEnumerable(Of S))
        If liste1.Count > 0 And liste2.Count > 0 Then
            pLstColonnes.Add(TsCaColonnesRapport.Separateur())
        End If
    End Sub

#End Region

End Class
