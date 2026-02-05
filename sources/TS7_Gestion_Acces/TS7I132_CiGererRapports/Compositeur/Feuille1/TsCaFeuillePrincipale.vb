
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
        Dim formatConditionne As TsCaFormatConditionneRapport
        Dim livreDeTravail As TsCuWorkbook

        feuille = New TsCuWorksheet(TsCuConstantesRapport.NOM_FEUILLE_1)
        formatConditionne = New TsCaFormatConditionneRapport(mSource)

        formatConditionne.AjouterFormatConditionne(feuille)
        AjouterCondtionLegende(feuille.ConditionnalFormattings)

        AjouterZonesDeNom(feuille)
        AjouterLaTable(feuille)

        livreDeTravail = New TsCuWorkbook(feuille)

        AjouterOptions(feuille)

        Return feuille
    End Function

#End Region

#Region "--- Sous-méthodes ---"

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
            If mSource.PresenceFeuille2 = True Then
                With .Last.Cells.First
                    .HRef = String.Format("#'{0}'!A1", TsCuConstantesRapport.NOM_FEUILLE_2)
                    .Data = New TsCuData(TsCuData.TypeType.String) With {.Contenue = "Voir, dans l'autre onglet, les assignations de ces rôles aux comptes d'employé d'unités autres que celles sélectionnées"}
                    .StyleID = TsCuConstantesRapport.ConstantesStyle.CELLULE_HYPERLIEN
                End With
            End If

            ' ---- Entête coupée
            .Add(concepteurLignes.ObtenirEnteteCoupe())

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

                .Add(concepteurLignes.ObtenirTotaux())
            End If


            For i = 0 To mSource.Contextes.Count - 1
                .Add(concepteurLignes.ObtenirContextes(mSource.Contextes(i), i + 1))
            Next

            .Add(concepteurLignes.ObtenirFinRapport())
            .Add(concepteurLignes.Espacement())

            If mSource.Contextes.Count <> 0 Then .Add(concepteurLignes.ObtenirLegende1())
            If mSource.Employes.Count > 0 Then .Add(concepteurLignes.ObtenirLegende2())
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
            .RefersTo = String.Format("='{2}'!R1C1:R{0}C{1}", ObtenirFinZoneBas(), ObtenirFinZoneGauche(), TsCuConstantesRapport.NOM_FEUILLE_1)
        End With

        With zoneDeNomBarrer
            .Name = TsCuConstantesRapport.NAMED_PRINT_TITILES
            .RefersTo = String.Format("='{0}'!C1:C2,'{0}'!R1:R4", TsCuConstantesRapport.NOM_FEUILLE_1)
        End With

        pFeuilleTravail.Names.Add(zoneDeNomImpresion)
        pFeuilleTravail.Names.Add(zoneDeNomBarrer)
    End Sub

    ''' <summary>
    ''' Permet d'ajouter la condtion de la ligne des totaux.
    ''' </summary>
    ''' <param name="lstConditions">La liste des formats conditionnés qui recevera la condition.</param>
    ''' <remarks></remarks>
    Private Sub AjouterCondtionLegende(ByVal lstConditions As List(Of TsCuConditionalFormatting))
        Dim premiereLigne = TsCuConstantesRapport.PREMIERE_LIGNE
        Dim derniereLigne = premiereLigne + mSource.Employes.Count - 1
        Dim premiereColonne = TsCuConstantesRapport.PREMIERE_COLONNE
        Dim derniereColonne = premiereColonne + ObtenirLargeurTableau() - 3
        Dim nombreColonne = derniereColonne - premiereColonne
        Dim nombreLigne = mSource.Employes.Count
        Dim zone1 As String
        Dim zone2 As String
        Dim condition1x As String = ""
        Dim condition2x As String = ""

        Dim decalementVertical = 4 + mSource.Contextes.Count + If(mSource.Contextes.Count > 0, 1, 0)


        zone1 = String.Format("R{0}C{1}:R{0}C{1}", derniereLigne + decalementVertical, premiereColonne + 1, derniereColonne)
        zone2 = String.Format("R{0}C{1}:R{0}C{1}", derniereLigne + decalementVertical, premiereColonne + 2, derniereColonne)

        For y = 0 To nombreColonne
            If condition1x <> "" Then condition1x &= ","
            If condition2x <> "" Then condition2x &= ","
            condition1x &= String.Format("(COUNTBLANK(R[-{3}]C[{0}]:R[-{1}]C[{0}]) = {2}-R[-{4}]C[{0}])", y, nombreLigne + decalementVertical - 1, nombreLigne, decalementVertical, decalementVertical - 1)
            condition2x &= String.Format("(COUNTBLANK(R[-{3}]C[{0}]:R[-{1}]C[{0}]) = {2}-R[-{4}]C[{0}])", y - 1, nombreLigne + decalementVertical - 1, nombreLigne, decalementVertical, decalementVertical - 1)
        Next

        lstConditions.Add(New TsCuConditionalFormatting(zone1, New TsCuCondition(String.Format("AND({0})", condition1x), New TsCuFormat("color:white;mso-background-source:auto"))))
        lstConditions.Add(New TsCuConditionalFormatting(zone2, New TsCuCondition(String.Format("AND({0})", condition2x), New TsCuFormat("color:white;mso-background-source:auto"))))
    End Sub

#End Region

#Region "--- Fonctions privées ---"

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
    ''' Fonction de service. Permet d'obtenir la largeur du tableau du rapport.
    ''' </summary>
    ''' <returns>La largeur du tableau.</returns>
    ''' <remarks></remarks>
    Private Function ObtenirLargeurTableau() As ULong
        Return CULng(TS7I132_CiGererRapports.TsCuOutilsRapport.ObtenirLargeurTableau( _
                                                            mSource.ObtenirMetiersSelectionnes.Count, _
                                                            mSource.ObtenirRolesTachesSelectionnes.Count, _
                                                            mSource.ObtenirMetiersAutre.Count, _
                                                            mSource.ObtenirRolesTachesAutre.Count) _
                    )
    End Function

#End Region


End Class
