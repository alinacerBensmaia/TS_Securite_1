Imports TS7I132_CiGererRapports.TsCuConstantesRapport

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
            .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
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
    Public Shared Function ObtenirEnteteTableau(ByVal pStyle As String, ByVal pLargeurSection As ULong, ByVal pValLongue As String, ByVal pValCourte As String) As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .MergeAcross = CULng(pLargeurSection - 1)
            .StyleID = pStyle
            If pLargeurSection < 4 Then
                .Data = New TsCuData(TsCuData.TypeType.String) With {.Contenue = pValCourte}
            Else
                .Data = New TsCuData(TsCuData.TypeType.String) With {.Contenue = pValLongue}
            End If
            .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function

#End Region

#Region "--- Sous en-tête tableau---"

    ''' <summary>
    ''' Permet d'obtenir une cellule de sous en-tête.
    ''' </summary>
    ''' <param name="pStyle">Le style qui sera appliqué à la cellule.</param>
    ''' <param name="pLargeurSection">La largeur de la section du sous en-tête.</param>
    ''' <param name="pValLongue">Valeur longue qui sera affiché dans la cellule si la cellule est assez grande.</param>
    ''' <param name="pValCourte">Valeur courte qui sera affiché dans la cellule si la cellule est trop petite.</param>
    ''' <returns>Une cellule.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirSousEntete(ByVal pStyle As String, ByVal pLargeurSection As ULong, ByVal pValLongue As String, ByVal pValCourte As String) As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .MergeAcross = CULng(pLargeurSection - 1)
            .StyleID = pStyle
            If pLargeurSection < 2 Then
                .Data = New TsCuData(TsCuData.TypeType.String) With {.Contenue = pValCourte}
            Else
                .Data = New TsCuData(TsCuData.TypeType.String) With {.Contenue = pValLongue}
            End If
            .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function

#End Region

#Region "--- UA ---"

    ''' <summary>
    ''' Permet d'obtenir une cellule affichant le titre d'une Unité Administrative.
    ''' </summary>
    ''' <param name="pStyle">Le style qui sera appliqué à la cellule.</param>
    ''' <param name="pValeur">La valeur qui sera afficher à la cellule.</param>
    ''' <returns>Une cellule.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirUA(ByVal pStyle As String, ByVal pValeur As String) As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .StyleID = pStyle
            .Data = New TsCuData(TsCuData.TypeType.String)
            With .Data
                .Contenue = pValeur
            End With
            .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function

#End Region

#Region "--- En-tête coupé ---"

    ''' <summary>
    ''' Permet d'obtenir une cellule de l'en-tête coupé.
    ''' </summary>
    ''' <param name="pValeur">La valeur de l'en-tête.</param>
    ''' <returns>Une cellule.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirEnteteCoupe(ByVal pValeur As String) As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .StyleID = ConstantesStyle.CELLULE_ENTETE_COUPE
            .Data = New TsCuData(TsCuData.TypeType.String) With {.Contenue = pValeur}
            .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function

    ''' <summary>
    ''' Permet d'obtenir une cellule du nom de l'employé dans le sous en-tête coupé.
    ''' </summary>
    ''' <param name="pValeur">La valeur à afficher dans la cellule.</param>
    ''' <param name="pStyle">Le style qui sera appliqué à la cellule.</param>
    ''' <returns>Une cellule.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirEmploye(ByVal pStyle As String, ByVal pValeur As String) As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .StyleID = pStyle
            .Data = New TsCuData(TsCuData.TypeType.String) With {.Contenue = pValeur}
            .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function

    ''' <summary>
    ''' Permet d'obtenir une cellule du numéro d'Unité Administrative d'un employé dans l'en-tête coupé.
    ''' </summary>
    ''' <param name="pValeur">La valeur qui sera affiché.</param>
    ''' <returns>Une cellule.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirNoUA(ByVal pStyle As String, ByVal pValeur As String) As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .StyleID = pStyle
            .Data = New TsCuData(TsCuData.TypeType.Number) With {.Contenue = pValeur}
            .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function

#End Region

#Region "--- Grille ---"

    ''' <summary>
    ''' Permet d'obtenir une cellule de la grille modifiable.
    ''' </summary>
    ''' <param name="pValeur">La valeur qui sera affiché.</param>
    ''' <returns>Une cellule.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirGrille(ByVal pValeur As String) As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .StyleID = ConstantesStyle.CELLULE_GRILLE
            .Data = New TsCuData(TsCuData.TypeType.String)
            With .Data
                .Contenue = pValeur
            End With
            .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function

#End Region

#Region "--- Sous en-tête coupé ---"

    ''' <summary>
    ''' Permeet d'obtenir la cellule de l'en-tête coupé.
    ''' </summary>
    ''' <returns>Une cellule.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirSousEnteteCoupe() As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .StyleID = ConstantesStyle.CELLULE_SOUS_ENTETE_COUPE
            .MergeAcross = 1
            .Data = New TsCuData(TsCuData.TypeType.String)
            With .Data
                .Contenue = "Nombre d'assignations"
            End With
            .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function

#End Region

#Region "--- Case Contexte ---"

    ''' <summary>
    ''' Permet d'obtenir une cellule de contexte d'unité administrative.
    ''' </summary>
    ''' <param name="pStyle">Le style qui sera appliqué à la cellule.</param>
    ''' <param name="pValeur">La valeur qui sera affiché.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirContexte(ByVal pStyle As String, ByVal pValeur As String) As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .StyleID = ConstantesStyle.CELLULE_CONTEXTE_GAUCHE
            .Data = New TsCuData(TsCuData.TypeType.String)
            With .Data
                .Contenue = pValeur
            End With
            .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function

#End Region

#Region "--- Totaux ---"

    ''' <summary>
    ''' Permet d'obtenir une cellule de la ligne des totaux.
    ''' </summary>
    ''' <param name="pStyle">Le style qui sera appliqué à la cellule.</param>
    ''' <param name="pformule">La formule qui sera appliqué à la cellule.</param>
    ''' <returns>Une cellule.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirTotal(ByVal pStyle As String, ByVal pformule As String) As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .StyleID = pStyle
            .Data = New TsCuData(TsCuData.TypeType.Number)
            .Formula = pformule
            .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function

    ''' <summary>
    ''' Permet d'obtenir une cellule du sous total d'une des colonnes.
    ''' </summary>
    ''' <param name="pStyle">Le style qui sera appliqué à la cellule.</param>
    ''' <param name="pformule">La formule qui sera appliqué à la cellule.</param>
    ''' <returns>Une cellule.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirSousTotal(ByVal pStyle As String, ByVal pFormule As String) As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .StyleID = pStyle
            .Formula = pFormule
            .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function

#End Region

#Region "--- Légendes ---"

    ''' <summary>
    ''' Permet d'obetnir la case de la légende avec un style.
    ''' </summary>
    ''' <param name="pStyle">Le style qui sera appliqué à la cellule.</param>
    ''' <returns>Une cellule.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirLegende(ByVal pStyle As String) As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .StyleID = pStyle
            .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function

    ''' <summary>
    ''' Permet d'obtenir la case de la légende ayant du texte.
    ''' </summary>
    ''' <param name="pValeur">La valeur qui sera affiché dans la cellule.</param>
    ''' <returns>Une cellule.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirLegendeTexte(ByVal pValeur As String) As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .StyleID = ConstantesStyle.CELLULE_LEGENDE_TEXTE
            .MergeAcross = 10
            .Data = New TsCuData(TsCuData.TypeType.String) With {.Contenue = pValeur}
            .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
        End With
        Return cellule
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
            .StyleID = ConstantesStyle.CELLULE_SEPARATEUR_FONCE
            .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
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
            .StyleID = ConstantesStyle.CELLULE_SEPARATEUR_PALE
            .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
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
            .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function

#End Region

End Class
