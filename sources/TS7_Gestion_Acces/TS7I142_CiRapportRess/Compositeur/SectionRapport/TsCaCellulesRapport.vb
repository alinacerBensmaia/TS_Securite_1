

''' <summary>
''' Cette classe contient la définition des cellules du rapport.
''' </summary>
''' <remarks></remarks>
Friend Class TsCaCellulesRapport

#Region "--- Entête ---"

    ''' <summary>
    ''' Permet d'obtenir la cellule de l'entête du rapport.
    ''' </summary>
    ''' <param name="pLargeurTable">La largeur réelle de la table.</param>
    ''' <param name="pValeur">Valeur qui sera présenté dans la cellule.</param>
    ''' <returns>Une cellule.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirEnteteRapport(ByVal pLargeurTable As ULong, ByVal pValeur As String) As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .StyleID = TsCuConstantesRapport.ConstantesStyle.CELLULE_ENTETE_RAPPORT
            .MergeAcross = CULng(pLargeurTable - 1)
            .Index = 2
            .Data = New TsCuData(TsCuData.TypeType.String)
            With .Data
                .Contenue = pValeur
            End With
            .NamedCells.Add(New TsCuNamedCell(TsCuConstantesRapport.NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function

#End Region

#Region "--- En-tête tableau ---"

    ''' <summary>
    ''' Permet d'obtenir une cellule de l'entête du tableau.
    ''' </summary>
    ''' <param name="pStyle">Style qui sera appliqué à la cellule d'entête de tableau</param>
    ''' <param name="pLargeurSection">La largeur de la section de l'entête.</param>
    ''' <param name="pValLongue">Valeur longue qui sera affiché dans la cellule si la cellule est assez grande.</param>
    ''' <param name="pValCourte">Valeur courte qui sera affiché dans la cellule si la cellule est trop petite.</param>
    ''' <returns>Une cellule.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirEnteteTableau(ByVal pStyle As String, ByVal pLargeurSection As Integer, ByVal pValLongue As String, ByVal pValCourte As String) As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .MergeAcross = CULng(pLargeurSection - 1)

            .StyleID = pStyle
            If pLargeurSection < 4 Then
                .Data = New TsCuData(TsCuData.TypeType.String) With {.Contenue = pValCourte}
            Else
                .Data = New TsCuData(TsCuData.TypeType.String) With {.Contenue = pValLongue}
            End If
            .NamedCells.Add(New TsCuNamedCell(TsCuConstantesRapport.NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function

    Public Shared Function ObtenirEnteteNbAssignations(ByVal pStyle As String, ByVal pLargeurSection As Integer, ByVal pValLongue As String) As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            '.MergeAcross = CULng(pLargeurSection - 1)

            .StyleID = pStyle

            .Data = New TsCuData(TsCuData.TypeType.String) With {.Contenue = "Nombre d'assignations"}


        End With
        Return cellule
    End Function

#End Region



    ''' <summary>
    ''' Permet d'obtenir une cellule affichant le titre d'une Unité Administrative.
    ''' </summary>
    ''' <param name="pStyle">Le style qui sera appliqué à la cellule.</param>
    ''' <param name="pValeur">La valeur qui sera afficher à la cellule.</param>
    ''' <returns>Une cellule.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirEmployesSelectionnes(ByVal pStyle As String, ByVal pValeur As String) As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .StyleID = pStyle
            .Data = New TsCuData(TsCuData.TypeType.String)
            With .Data
                .Contenue = pValeur
            End With
            .NamedCells.Add(New TsCuNamedCell(TsCuConstantesRapport.NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function

    ''' <summary>
    ''' Permet d'obtenir une cellule du nom de la ressource.
    ''' </summary>
    ''' <param name="pValeur">La valeur à afficher dans la cellule.</param>
    ''' <param name="pStyle">Le style qui sera appliqué à la cellule.</param>
    ''' <returns>Une cellule.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirRessource(ByVal pStyle As String, ByVal pValeur As String) As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .StyleID = pStyle
            .Data = New TsCuData(TsCuData.TypeType.String) With {.Contenue = pValeur}
            .NamedCells.Add(New TsCuNamedCell(TsCuConstantesRapport.NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function


    Public Shared Function ObtenirRessourceFonctionnel(ByVal pStyle As String, ByVal pValeur As String) As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .StyleID = pStyle

            .Data = New TsCuData(TsCuData.TypeType.String) With {.Contenue = pValeur}
            .NamedCells.Add(New TsCuNamedCell(TsCuConstantesRapport.NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function

#Region "--- Grille ---"


    Public Shared Function ObtenirGrilleCorrespondance(ByVal pNomRessource As String, ByVal pLstRessources As List(Of TsDtSourceRessources)) As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .StyleID = TsCuConstantesRapport.ConstantesStyle.CELLULE_GRILLE

            If TrouverCorrespondance(pNomRessource, pLstRessources) Then
                .Data = New TsCuData(TsCuData.TypeType.String) With {.Contenue = " X "}
            End If

        End With
        Return cellule
    End Function

    Public Shared Function TrouverCorrespondance(ByVal pNomRessource As String, plstRessources As List(Of TsDtSourceRessources)) As Boolean
        Dim resultat As IEnumerable(Of TsDtSourceRessources) = plstRessources.Where(Function(Res) Res.Nom.ToUpper() = pNomRessource.ToUpper())

        If Not (resultat Is Nothing OrElse resultat.Count = 0) Then
            Return True
        End If

        Return False
    End Function

#End Region



#Region "--- Séparateurs ---"

    ''' <summary>
    ''' Permet d'obtenir un séparateur foncé dans le tableau.
    ''' </summary>
    ''' <returns>Une cellule.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirSeparateurFonce() As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .StyleID = TsCuConstantesRapport.ConstantesStyle.CELLULE_SEPARATEUR_FONCE
            .NamedCells.Add(New TsCuNamedCell(TsCuConstantesRapport.NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function

    ''' <summary>
    ''' Permet d'obtenir un séparateur pâle dans le tableau.
    ''' </summary>
    ''' <returns>Une cellule.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirSeparateurPale() As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .StyleID = TsCuConstantesRapport.ConstantesStyle.CELLULE_SEPARATEUR_PALE
            .NamedCells.Add(New TsCuNamedCell(TsCuConstantesRapport.NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function

    ''' <summary>
    ''' Permet d'obtenir une cellule complétement vide sans style.
    ''' </summary>
    ''' <returns>Une cellule.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirVide(Optional ByVal pStyle As String = "") As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            If String.IsNullOrEmpty(pStyle) = False Then .StyleID = pStyle
            .NamedCells.Add(New TsCuNamedCell(TsCuConstantesRapport.NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function

    Public Shared Function ObtenirTotal(ByVal pStyle As String, ByVal pformule As String) As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .StyleID = pStyle
            .Data = New TsCuData(TsCuData.TypeType.Number)
            .Formula = pformule
            '.NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function

#End Region

End Class
