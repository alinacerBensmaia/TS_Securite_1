Imports TS7I132_CiGererRapports.TsCuConstantesRapport

''' <summary>
''' Cette classe contient les cellules définit pour le rapport.
''' </summary>
''' <remarks></remarks>
Friend Class TsCaCellulesRapport

    'TODO FAIRE COMENTAIRE

#Region "--- Entête ---"

    ''' <summary>
    ''' Permet d'obtenir la cellule de l'entête du rapport.
    ''' </summary>
    Public Shared Function ObtenirEntete(ByVal pLargeurTable As ULong, ByVal pValeur As String) As TsCuCell
        Dim cellule As New TsCuCell() 'TODO VOIR POUR INSERER LES CARACTÉRISITUQE xmlns
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

#Region "--- Entête Tableau ---"

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

    'TODO EFFACER
    'Public Shared Function ObtenirEnteteSeparateur() As TsCuCell
    '    Dim cellule As New TsCuCell()
    '    With cellule
    '        .StyleID = ConstantesStyle.CELLULE_ENTETE_TABLEAU
    '        .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
    '    End With
    '    Return cellule
    'End Function

    'Public Shared Function ObtenirEnteteUaAutre(ByVal pLargeurSection As ULong) As TsCuCell
    '    Dim cellule As New TsCuCell()
    '    With cellule
    '        .MergeAcross = CULng(pLargeurSection - 1)
    '        .StyleID = ConstantesStyle.CELLULE_ENTETE_TABLEAU
    '        If pLargeurSection < 2 Then
    '            .Data = New TsCuData(TsCuData.TypeType.String) With {.Contenue = "Aut"}
    '        Else
    '            .Data = New TsCuData(TsCuData.TypeType.String) With {.Contenue = "Autre UA"}
    '        End If
    '        .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
    '    End With
    '    Return cellule
    'End Function

#End Region

#Region "--- Sous-Entête Tableau---"

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

    'TODO EFFACER
    'Public Shared Function ObtenirSousEnteteMetierGauche(ByVal pLargeurSection As ULong) As TsCuCell
    '    Dim cellule As New TsCuCell()
    '    With cellule
    '        .MergeAcross = CULng(pLargeurSection - 1)
    '        .StyleID = ConstantesStyle.CELLULE_SOUS_ENTETE_TABLEAU_GAUCHE
    '        If pLargeurSection < 2 Then
    '            .Data = New TsCuData(TsCuData.TypeType.String) With {.Contenue = "Mét"}
    '        Else
    '            .Data = New TsCuData(TsCuData.TypeType.String) With {.Contenue = "Métier"}
    '        End If
    '        .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
    '    End With
    '    Return cellule
    'End Function

    'Public Shared Function ObtenirSousEnteteTacheGauche(ByVal pLargeurSection As ULong) As TsCuCell
    '    Dim cellule As New TsCuCell()
    '    With cellule
    '        .MergeAcross = CULng(pLargeurSection - 1)
    '        .StyleID = ConstantesStyle.CELLULE_SOUS_ENTETE_TABLEAU
    '        If pLargeurSection < 2 Then
    '            .Data = New TsCuData(TsCuData.TypeType.String) With {.Contenue = "Tâe"}
    '        Else
    '            .Data = New TsCuData(TsCuData.TypeType.String) With {.Contenue = "Tâche"}
    '        End If
    '        .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
    '    End With
    '    Return cellule
    'End Function

    'Public Shared Function ObtenirSousEnteteMetierDroite(ByVal pLargeurSection As ULong) As TsCuCell
    '    Dim cellule As New TsCuCell()
    '    With cellule
    '        .MergeAcross = CULng(pLargeurSection - 1)
    '        .StyleID = ConstantesStyle.CELLULE_SOUS_ENTETE_TABLEAU
    '        If pLargeurSection < 2 Then
    '            .Data = New TsCuData(TsCuData.TypeType.String) With {.Contenue = "Mét"}
    '        Else
    '            .Data = New TsCuData(TsCuData.TypeType.String) With {.Contenue = "Métier"}
    '        End If
    '        .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
    '    End With
    '    Return cellule
    'End Function

    'Public Shared Function ObtenirSousEnteteTacheDroite(ByVal pLargeurSection As ULong) As TsCuCell
    '    Dim cellule As New TsCuCell()
    '    With cellule
    '        .MergeAcross = CULng(pLargeurSection - 1)
    '        .StyleID = ConstantesStyle.CELLULE_SOUS_ENTETE_TABLEAU_DROITE
    '        If pLargeurSection < 2 Then
    '            .Data = New TsCuData(TsCuData.TypeType.String) With {.Contenue = "Tâe"}
    '        Else
    '            .Data = New TsCuData(TsCuData.TypeType.String) With {.Contenue = "Tâche"}
    '        End If
    '        .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
    '    End With
    '    Return cellule
    'End Function

#End Region

#Region "--- UA ---"

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

    'TODO EFFACER
    'Public Shared Function ObtenirUAGauche(ByVal pValeur As String) As TsCuCell
    '    Dim cellule As New TsCuCell()
    '    With cellule
    '        .StyleID = ConstantesStyle.CELLULE_UA_GAUCHE
    '        .Data = New TsCuData(TsCuData.TypeType.String)
    '        With .Data
    '            .Contenue = pValeur
    '        End With
    '        .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
    '    End With
    '    Return cellule
    'End Function
    'TODO EFFACER
    'Public Shared Function ObtenirUADroite(ByVal pValeur As String) As TsCuCell
    '    Dim cellule As New TsCuCell()
    '    With cellule
    '        .StyleID = ConstantesStyle.CELLULE_UA_DROITE
    '        .Data = New TsCuData(TsCuData.TypeType.String)
    '        With .Data
    '            .Contenue = pValeur
    '        End With
    '        .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
    '    End With
    '    Return cellule
    'End Function

#End Region

#Region "--- Entête-Coupée ---"

    Public Shared Function ObtenirEnteteCouper(ByVal pValeur As String) As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .StyleID = ConstantesStyle.CELLULE_ENTETE_COUPEE
            .Data = New TsCuData(TsCuData.TypeType.String) With {.Contenue = pValeur}
            .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function

    Public Shared Function ObtenirEmploye(ByVal pValeur As String) As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .StyleID = ConstantesStyle.CELLULE_NOM
            .Data = New TsCuData(TsCuData.TypeType.String) With {.Contenue = pValeur}
            .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function

    Public Shared Function ObtenirEmployeHaut(ByVal pValeur As String) As TsCuCell
        Dim cellule As TsCuCell = ObtenirEmploye(pValeur)
        cellule.StyleID = ConstantesStyle.CELLULE_NOM_HAUT
        Return cellule
    End Function

    Public Shared Function ObtenirEmployeBas(ByVal pValeur As String) As TsCuCell
        Dim cellule As TsCuCell = ObtenirEmploye(pValeur)
        cellule.StyleID = ConstantesStyle.CELLULE_NOM_BAS
        Return cellule
    End Function

    Public Shared Function ObtenirNoUA(ByVal pValeur As String) As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .StyleID = ConstantesStyle.CELLULE_NO_UA
            .Data = New TsCuData(TsCuData.TypeType.Number) With {.Contenue = pValeur}
            .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function

    Public Shared Function ObtenirNoUAHaut(ByVal pValeur As String) As TsCuCell
        Dim cellule As TsCuCell = ObtenirNoUA(pValeur)
        cellule.StyleID = ConstantesStyle.CELLULE_NO_UA_HAUT
        Return cellule
    End Function

    Public Shared Function ObtenirNoUABas(ByVal pValeur As String) As TsCuCell
        Dim cellule As TsCuCell = ObtenirNoUA(pValeur)
        cellule.StyleID = ConstantesStyle.CELLULE_NO_UA_BAS
        Return cellule
    End Function

#End Region

#Region "--- Grille ---"

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

#Region "--- Sous-Entête Coupée ---"

    Public Shared Function ObtenirSousEnteteCoupee() As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .StyleID = ConstantesStyle.CELLULE_SOUS_ENTETE_COUPEE
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

    Public Shared Function ObtenirContexteGauche(ByVal pValeur As String) As TsCuCell
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

    Public Shared Function ObtenirContexteDroite(ByVal pValeur As String) As TsCuCell
        Dim cellule = ObtenirContexteGauche(pValeur)
        With cellule
            .StyleID = ConstantesStyle.CELLULE_CONTEXTE_DROITE
        End With
        Return cellule
    End Function

#End Region

#Region "--- Totaux ---"

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

    'TODO EFFACER
    'Public Shared Function ObtenirTotalGauche(ByVal pValeur As String) As TsCuCell
    '    Dim cellule = ObtenirTotal(pValeur)
    '    With cellule
    '        .StyleID = ConstantesStyle.CELLULE_TOTAL_GAUCHE
    '    End With
    '    Return cellule
    'End Function

    Public Shared Function ObtenirSousTotal(ByVal pStyle As String, ByVal pFormule As String) As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .StyleID = pStyle
            .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
            .Formula = pFormule
        End With
        Return cellule
    End Function

    'TODO EFFACER
    'Public Shared Function ObtenirSousTotalGauche(ByVal pValeur As String) As TsCuCell
    '    Dim cellule = ObtenirSousTotal(pValeur)
    '    With cellule
    '        .StyleID = ConstantesStyle.CELLULE_SOUS_TOTAL_GAUCHE
    '    End With
    '    Return cellule
    'End Function

    'Public Shared Function ObtenirSousTotalBarre() As TsCuCell
    '    Dim cellule = ObtenirSousTotal("")
    '    With cellule
    '        .StyleID = ConstantesStyle.CELLULE_SOUS_TOTAL_BARRE
    '    End With
    '    Return cellule
    'End Function

    'Public Shared Function ObtenirSousTotalBarreGauche() As TsCuCell
    '    Dim cellule = ObtenirSousTotalBarre()
    '    With cellule
    '        .StyleID = ConstantesStyle.CELLULE_SOUS_TOTAL_BARRE_GAUCHE
    '    End With
    '    Return cellule
    'End Function

#End Region

#Region "--- Légendes ---"

    Public Shared Function ObtenirLegendeBarre() As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .StyleID = ConstantesStyle.CELLULE_LEGENDE_BARREE
            .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function

    Public Shared Function ObtenirLegendeJaune() As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .StyleID = ConstantesStyle.CELLULE_LEGENDE_JAUNE
            .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function

    Public Shared Function ObtenirLegendeTexte(ByVal pValeur As String) As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .MergeAcross = 10
            .Data = New TsCuData(TsCuData.TypeType.String) With {.Contenue = pValeur}
            .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function

#End Region

#Region "--- Séparateurs ---"

    ''' <summary>
    ''' Permet d'obtenir la cellule de l'entête du tableau affichant le "UA sélectionnées".
    ''' </summary>
    Public Shared Function ObtenirSeparateurFonce() As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .StyleID = ConstantesStyle.CELLULE_SEPARATEUR_FONCE
            .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function

    Public Shared Function ObtenirSeparateurPale() As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .StyleID = ConstantesStyle.CELLULE_SEPARATEUR_PALE
            .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function

    Public Shared Function ObtenirVide() As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function

    Public Shared Function ObtenirVideGauche() As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .StyleID = ConstantesStyle.CELLULE_VIDE_GAUCHE
            .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function

    Public Shared Function ObtenirVideDroite() As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .StyleID = ConstantesStyle.CELLULE_VIDE_DROITE
            .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function

    Public Shared Function ObtenirVideHaut() As TsCuCell
        Dim cellule As New TsCuCell()
        With cellule
            .StyleID = ConstantesStyle.CELLULE_VIDE_HAUT
            .NamedCells.Add(New TsCuNamedCell(NAMED_PRINT_AREA))
        End With
        Return cellule
    End Function

#End Region

End Class
