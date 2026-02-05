Imports TS7I132_CiGererRapports.TsCuConstantesRapport

''' <summary>
''' Cette classe contient la définition des styles du rapport.
''' </summary>
''' <remarks></remarks>
Public Class TsCaStylesRapport

    Private Const STYLE_LIGNE_DEFAUT As TsCuStyleBorder.LineStyleType = TsCuStyleBorder.LineStyleType.Dot


#Region "--- Style défaut ---"

    ''' <summary>
    ''' Retourne le style par défaut.
    ''' </summary>
    ''' <returns>Un Style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirParDefaut() As TsCuStyle
        Dim defaut As New TsCuStyle(ConstantesStyle.CELLULE_DEFAULT)

        With defaut
            With .Alignment
                .Vertical = TsCuStyleAlignment.VerticalType.Bottom
                .WrapText = True
            End With
            With .Font
                .FontName = "Arial"
                .Family = TsCuStyleFont.FamilyType.Swiss
                .Color = "#000000"
            End With
            With .Protection
                .Protected = True
            End With
        End With

        Return defaut
    End Function

#End Region

#Region "--- Style général ---"

    ''' <summary>
    ''' Retourne le style par défaut.
    ''' </summary>
    ''' <returns>Un Style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirHyperLien() As TsCuStyle
        Dim defaut As New TsCuStyle(ConstantesStyle.CELLULE_HYPERLIEN)

        With defaut
            With .Alignment
                .Vertical = TsCuStyleAlignment.VerticalType.Center
                .WrapText = True
            End With
            With .Font
                .FontName = "Arial"
                .Family = TsCuStyleFont.FamilyType.Swiss
                .Color = "#FF0000"
            End With
            With .Protection
                .Protected = True
            End With
        End With

        Return defaut
    End Function

#End Region

#Region "--- Styles en-tête ---"

    ''' <summary>
    ''' Permet d'obtenir le style de la cellule de l'en-tête du rapport.
    ''' </summary>
    ''' <returns>Un style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirCelluleEnteteRapport() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_ENTETE_RAPPORT)

        With style
            With .Alignment
                .Horizontal = TsCuStyleAlignment.HorizontalType.Center
                .Vertical = TsCuStyleAlignment.VerticalType.Bottom
            End With
        End With

        Return style
    End Function

    ''' <summary>
    ''' Permet d'obtenir le style des cellules de la ligne de l'en-tête du tableau.
    ''' </summary>
    ''' <returns>Un style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirCelluleEnteteTableau() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_ENTETE_TABLEAU)

        With style
            With .Alignment
                .Vertical = TsCuStyleAlignment.VerticalType.Bottom
                .Horizontal = TsCuStyleAlignment.HorizontalType.Center
            End With
            With .Font
                .FontName = "Arial"
                .Family = TsCuStyleFont.FamilyType.Swiss
                .Bold = True
                .Color = "#000000"
            End With
            With .Interior
                .Color = "#A5A5A5"
                .Pattern = TsCuStyleInterior.PatternType.Solid
            End With
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
        End With

        Return style
    End Function

#End Region

#Region "--- Styles sous en-tête ---"

    ''' <summary>
    ''' Permet d'obtenir le style des cellules de la ligne du sous en-tête du tableau.
    ''' </summary>
    ''' <returns>Un style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirCelluleSousEnteteTableau() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_SOUS_ENTETE_TABLEAU)
        With style
            With .Alignment
                .Vertical = TsCuStyleAlignment.VerticalType.Bottom
                .Horizontal = TsCuStyleAlignment.HorizontalType.Center
            End With
            With .Font
                .FontName = "Arial"
                .Family = TsCuStyleFont.FamilyType.Swiss
                .Bold = True
                .Color = "#000000"
            End With
            With .Interior
                .Color = "#D8D8D8"
                .Pattern = TsCuStyleInterior.PatternType.Solid
            End With
        End With
        Return style
    End Function

#End Region

#Region "--- Styles UA ---"

    ''' <summary>
    ''' Permet d'obtenir le style d'une cellule d'unité administrative.
    ''' </summary>
    ''' <returns>Une style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirCelluleUA() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_UA)

        With style
            With .Alignment
                .Vertical = TsCuStyleAlignment.VerticalType.Bottom
                .Horizontal = TsCuStyleAlignment.HorizontalType.Center
                .Rotate = 90
                .WrapText = True
            End With
            With .Font
                .FontName = "Arial"
                .Family = TsCuStyleFont.FamilyType.Swiss
                .Color = "#000000"
            End With
            With .Interior
                .Color = "#FFFFFF"
                .Pattern = TsCuStyleInterior.PatternType.Solid
            End With
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Bottom) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
        End With

        Return style
    End Function

#End Region

#Region "--- Styles en-tête coupé ---"

    ''' <summary>
    ''' Permet d'obtenir le style des cellules de l'en-tête coupé.
    ''' </summary>
    ''' <returns>Un style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirCelluleEnteteCoupe() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_ENTETE_COUPE)

        With style
            With .Alignment
                .Horizontal = TsCuStyleAlignment.HorizontalType.Center
                .Vertical = TsCuStyleAlignment.VerticalType.Bottom
                .WrapText = True
            End With
            With .Font
                .Bold = True
            End With
            With .Interior
                .Color = "#A5A5A5"
                .Pattern = TsCuStyleInterior.PatternType.Solid
            End With
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Bottom) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
        End With

        Return style
    End Function

    ''' <summary>
    ''' Permet d'obtenir le style des cellules des nom d'employé.
    ''' </summary>
    ''' <returns>Une style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirCelluleNom() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_NOM)
        With style
            .Alignment.ShrinkToFit = True
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Bottom) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
        End With
        Return style
    End Function

    ''' <summary>
    ''' Permet d'obtenir le style des cellules des nom d'employé sur la première ligne.
    ''' </summary>
    ''' <returns>Un style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirCelluleNomHaut() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_NOM_HAUT)
        With style
            .Alignment.ShrinkToFit = True
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Bottom) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
        End With
        Return style
    End Function

    ''' <summary>
    ''' Permet d'obtenir le style des cellules des nom d'employé sur la dernière ligne.
    ''' </summary>
    ''' <returns>Un style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirCelluleNomBas() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_NOM_BAS)
        With style
            .Alignment.ShrinkToFit = True
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Bottom) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
        End With
        Return style
    End Function

    ''' <summary>
    ''' Permet d'obtenir le style des cellules des numéros d'unités administratives.
    ''' </summary>
    ''' <returns>Un Style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirCelluleNoUA() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_NO_UA)
        With style
            .Parent = ConstantesStyle.CELLULE_NOM
            With .Alignment
                .Horizontal = TsCuStyleAlignment.HorizontalType.Center
                .Vertical = TsCuStyleAlignment.VerticalType.Center
            End With
        End With
        Return style
    End Function

    ''' <summary>
    ''' Permet d'obtenir le style des cellules des numéros d'unités administratives se trouvant sur la première ligne.
    ''' </summary>
    ''' <returns>Un Style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirCelluleNoUAHaut() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_NO_UA_HAUT)
        With style
            .Parent = ConstantesStyle.CELLULE_NOM_HAUT
            With .Alignment
                .Horizontal = TsCuStyleAlignment.HorizontalType.Center
                .Vertical = TsCuStyleAlignment.VerticalType.Center
            End With
        End With
        Return style
    End Function

    ''' <summary>
    ''' Permet d'obtenir le style des cellules des numéros d'unités administratives se trouvant sur la dernière ligne.
    ''' </summary>
    ''' <returns>Un Style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirCelluleNoUABas() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_NO_UA_BAS)
        With style
            .Parent = ConstantesStyle.CELLULE_NOM_BAS
            With .Alignment
                .Horizontal = TsCuStyleAlignment.HorizontalType.Center
                .Vertical = TsCuStyleAlignment.VerticalType.Center
            End With
        End With
        Return style
    End Function

#End Region

#Region "--- Styles sous En-tête coupé ---"

    ''' <summary>
    ''' Permet d'obtenir le style des cellules du sous en-tête coupé.
    ''' </summary>
    ''' <returns>Un Style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirCelluleSousEnteteCoupe() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_SOUS_ENTETE_COUPE)

        With style
            With .Font
                .Bold = True
            End With
            With .Interior
                .Color = ConstantesCouleur.GRIS_PALE
                .Pattern = TsCuStyleInterior.PatternType.Solid
            End With
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Bottom) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
        End With

        Return style
    End Function

    ''' <summary>
    ''' Permet d'obtenir le style des cellules des contextes situés à gauche.
    ''' </summary>
    ''' <returns>Un Style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirCelluleContexteGauche() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_CONTEXTE_GAUCHE)
        With style
            With .Interior
                .Color = ConstantesCouleur.BLANC
                .Pattern = TsCuStyleInterior.PatternType.Solid
            End With
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Bottom) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
        End With
        Return style
    End Function

    ''' <summary>
    ''' Permet d'obtenir le style des cellules des contextes situés à droite.
    ''' </summary>
    ''' <returns>Un Style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirCelluleContexteDroite() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_CONTEXTE_DROITE)
        With style
            .Parent = ConstantesStyle.CELLULE_CONTEXTE_GAUCHE
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Bottom) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
        End With
        Return style
    End Function

    ''' <summary>
    ''' Permet d'obtenir le style des cellules de totaux.
    ''' </summary>
    ''' <returns>Un Style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirCelluleTotal() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_TOTAL)
        With style
            With .Alignment
                .Horizontal = TsCuStyleAlignment.HorizontalType.Center
            End With
            With .Font
                .Bold = True
            End With
            With .Interior
                .Color = "#FFFFFF"
                .Pattern = TsCuStyleInterior.PatternType.Solid
            End With
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Bottom) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
        End With
        Return style
    End Function

    ''' <summary>
    ''' Permet d'obtenir le style des cellules des totaux à gauche.
    ''' </summary>
    ''' <returns>Un Style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirCelluleTotalGauche() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_TOTAL_GAUCHE)
        With style
            .Parent = ConstantesStyle.CELLULE_TOTAL
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Bottom) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
        End With
        Return style
    End Function

    ''' <summary>
    ''' Permet d'obtenir le style des cellules des sous totaux.
    ''' </summary>
    ''' <returns>Un Style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirCelluleSousTotal() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_SOUS_TOTAL)
        With style
            With .Alignment
                .Horizontal = TsCuStyleAlignment.HorizontalType.Center
            End With
            With .Interior
                .Color = "#FFFFFF"
                .Pattern = TsCuStyleInterior.PatternType.Solid
            End With
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Bottom) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
        End With
        Return style
    End Function

    ''' <summary>
    ''' Permet d'obtenir le style des cellules des sous totaux situés à gauche.
    ''' </summary>
    ''' <returns>Un Style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirCelluleSousTotalGauche() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_SOUS_TOTAL_GAUCHE)
        With style
            .Parent = ConstantesStyle.CELLULE_SOUS_TOTAL
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Bottom) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
        End With
        Return style
    End Function

    ''' <summary>
    ''' Permet d'obtenir le style des cellules des sous totaux barré.
    ''' </summary>
    ''' <returns>Un Style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirCelluleSousTotalBarre() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_SOUS_TOTAL_BARRE)
        With style
            With .Interior
                .PatternColor = ConstantesCouleur.NOIR
                .Pattern = TsCuStyleInterior.PatternType.ThinDiagStripe
            End With
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Bottom) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
        End With
        Return style
    End Function

    ''' <summary>
    ''' Permet d'obtenir le style des cellules des sous totaux barré situés à gauche.
    ''' </summary>
    ''' <returns>Un Style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirCelluleSousTotalBarreGauche() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_SOUS_TOTAL_BARRE_GAUCHE)
        With style
            .Parent = ConstantesStyle.CELLULE_SOUS_TOTAL_BARRE
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Bottom) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
        End With
        Return style
    End Function

#End Region

#Region "--- Styles grille ---"

    ''' <summary>
    ''' Permet d'obtenir le style d'une cellule de la grille de saisie.
    ''' </summary>
    ''' <returns>Un Style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirCelluleGrille() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_GRILLE)

        With style
            With .Alignment
                .Horizontal = TsCuStyleAlignment.HorizontalType.Center
                .Vertical = TsCuStyleAlignment.VerticalType.Center
            End With
            With .Interior
                .Color = "#FFFFFF"
                .Pattern = TsCuStyleInterior.PatternType.Solid
            End With
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Bottom) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})

            With .Protection
                .Protected = False
            End With
        End With

        Return style
    End Function

    ''' <summary>
    ''' Permet d'obtenir le style d'une cellule de la grille de saisie.
    ''' </summary>
    ''' <returns>Un Style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirCelluleGrilleMarque() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_GRILLE_MARQUE)

        With style
            .Parent = ConstantesStyle.CELLULE_GRILLE
            With .Interior
                .Color = "#FF5533"
                .Pattern = TsCuStyleInterior.PatternType.Solid
            End With
        End With

        Return style
    End Function

#End Region

#Region "--- Styles légende ---"

    ''' <summary>
    ''' Permet d'obtenir le style de la cellule de la légende barré.
    ''' </summary>
    ''' <returns>Un Style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirCelluleLegendeBarree() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_LEGENDE_BARREE)
        With style
            With .Interior
                .Pattern = TsCuStyleInterior.PatternType.ThinDiagStripe
                .PatternColor = ConstantesCouleur.NOIR
            End With
        End With
        Return style
    End Function

    ''' <summary>
    ''' Permet d'obtenir le style de la cellule de la légende barré.
    ''' </summary>
    ''' <returns>Un Style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirCelluleLegendeJaune() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_LEGENDE_JAUNE)
        With style
            With .Interior
                .Color = ConstantesCouleur.JAUNE
                .Pattern = TsCuStyleInterior.PatternType.Solid
            End With
        End With
        Return style
    End Function

    ''' <summary>
    ''' Permet d'obtenir le style de la cellule de la légende barré.
    ''' </summary>
    ''' <returns>Un Style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirCelluleLegendeTexte() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_LEGENDE_TEXTE)
        With style
            .Alignment.ShrinkToFit = True
        End With
        Return style
    End Function

#End Region

#Region "--- Styles séparateur ---"

    ''' <summary>
    ''' Permet d'obtenir le style d'un séparateur foncé.
    ''' </summary>
    ''' <returns>Un Style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirCelluleSeparateurFonce() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_SEPARATEUR_FONCE)
        With style
            With .Interior
                .Color = ConstantesCouleur.GRIS_FONCE
                .Pattern = TsCuStyleInterior.PatternType.Solid
            End With
        End With
        Return style
    End Function

    ''' <summary>
    ''' Permet d'obtenir le style d'une séparateur pâle.
    ''' </summary>
    ''' <returns>Un Style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirCelluleSeparateurPale() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_SEPARATEUR_PALE)
        With style
            With .Interior
                .Color = ConstantesCouleur.GRIS_PALE
                .Pattern = TsCuStyleInterior.PatternType.Solid
            End With
        End With
        Return style
    End Function

    ''' <summary>
    ''' Permet d'obtenir le style d'une cellule vide avec bordure à gauche.
    ''' </summary>
    ''' <returns>Un Style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirCelluleVideGauche() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_VIDE_GAUCHE)
        With style
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
        End With
        Return style
    End Function

    ''' <summary>
    ''' Permet d'obtenir le style d'une cellule vide avec bordure à droite.
    ''' </summary>
    ''' <returns>Un Style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirCelluleVideDroite() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_VIDE_DROITE)
        With style
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
        End With
        Return style
    End Function

    ''' <summary>
    ''' Permet d'obtenir le style d'une cellule vide avec bordure en haut.
    ''' </summary>
    ''' <returns>Un Style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirCelluleVideHaut() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_VIDE_HAUT)
        With style
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
        End With
        Return style
    End Function

    ''' <summary>
    ''' Permet d'obtenir le style d'une cellule vide avec bordure en haut et à gauche.
    ''' </summary>
    ''' <returns>Un Style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirCelluleVideCoinHautGauche() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_VIDE_COIN_HAUT_GAUCHE)
        With style
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
        End With
        Return style
    End Function

    ''' <summary>
    ''' Permet d'obtenir le style d'une cellule vide avec bordure en haut et à droite.
    ''' </summary>
    ''' <returns>Un Style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirCelluleVideCoinHautDroit() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_VIDE_COIN_HAUT_DROIT)
        With style
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
        End With
        Return style
    End Function

    ''' <summary>
    ''' Permet d'obtenir le style d'une cellule vide avec bordure en bas.
    ''' </summary>
    ''' <returns>Un Style.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirCelluleVideBas() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_VIDE_BAS)
        With style
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Bottom) With {.LineStyle = STYLE_LIGNE_DEFAUT, .Weight = 1})
        End With
        Return style
    End Function

#End Region

End Class
