Imports TS7I132_CiGererRapports.TsCuConstantesRapport

''' <summary>
''' Cette classe contient les styles du rapport.
''' </summary>
''' <remarks></remarks>
Public Class TsCaStylesRapport

    'TODO FAIRE COMMENTAIRE
#Region "--- Style Défaut ---"

    ''' <summary>
    ''' Retourne le style par défaut.
    ''' </summary>
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

#Region "--- Style Entête ---"

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
    ''' Permet d'obtenir le style des cellules de la ligne de l'entête du tableau.
    ''' </summary>
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
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
        End With
        Return style
    End Function

    Public Shared Function ObtenirCelluleEnteteTableauGauche() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_ENTETE_TABLEAU_GAUCHE)
        With style
            .Parent = ConstantesStyle.CELLULE_ENTETE_TABLEAU
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
        End With
        Return style
    End Function

    'TODO EFFACER
    'Public Shared Function ObtenirCelluleEnteteTableauDroite() As TsCuStyle

    '    Dim style As New TsCuStyle(ConstantesStyle.CELLULE_ENTETE_TABLEAU_DROITE)

    '    With style
    '        .Parent = ConstantesStyle.CELLULE_ENTETE_TABLEAU
    '        .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
    '        .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
    '    End With

    '    Return style
    'End Function

#End Region

#Region "--- Style Sous-Entête ---"

    Public Shared Function ObtenirCelluleSousEnteteTableau() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_SOUS_ENTETE_TABLEAU)
        With style
            With .Alignment
                .Vertical = TsCuStyleAlignment.VerticalType.Bottom
                .Horizontal = TsCuStyleAlignment.HorizontalType.Center
                .WrapText = True
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

    Public Shared Function ObtenirCelluleSousEnteteTableauGauche() As TsCuStyle

        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_SOUS_ENTETE_TABLEAU_GAUCHE)

        With style
            .Parent = ConstantesStyle.CELLULE_SOUS_ENTETE_TABLEAU
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
        End With

        Return style
    End Function

    'TODO EFFACER
    'Public Shared Function ObtenirCelluleSousEnteteTableauDroite() As TsCuStyle

    '    Dim style As New TsCuStyle(ConstantesStyle.CELLULE_SOUS_ENTETE_TABLEAU_DROITE)

    '    With style
    '        .Parent = ConstantesStyle.CELLULE_SOUS_ENTETE_TABLEAU
    '        .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
    '    End With

    '    Return style
    'End Function

#End Region

#Region "--- Style UA ---"

    Public Shared Function ObtenirCelluleUA() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_UA)
        With style
            With .Alignment
                .Vertical = TsCuStyleAlignment.VerticalType.Bottom
                .Horizontal = TsCuStyleAlignment.HorizontalType.Center
                .Rotate = 90
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
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Bottom) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
        End With
        Return style
    End Function

    'TODO EFFACER
    'Public Shared Function ObtenirCelluleUAGauche() As TsCuStyle
    '    Dim style As New TsCuStyle(ConstantesStyle.CELLULE_UA_GAUCHE)
    '    With style
    '        .Parent = ConstantesStyle.CELLULE_UA
    '        .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
    '        .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Bottom) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
    '        .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
    '    End With
    '    Return style
    'End Function

    'TODO EFFACER
    'Public Shared Function ObtenirCelluleUADroite() As TsCuStyle
    '    Dim style As New TsCuStyle(ConstantesStyle.CELLULE_UA_DROITE)
    '    With style
    '        .Parent = ConstantesStyle.CELLULE_UA
    '        .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
    '        .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Bottom) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
    '        .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
    '    End With
    '    Return style
    'End Function

#End Region

#Region "--- Style Entête Coupée ---"

    Public Shared Function ObtenirCelluleEnteteCoupee() As TsCuStyle

        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_ENTETE_COUPEE)
        With style
            With .Alignment
                .Horizontal = TsCuStyleAlignment.HorizontalType.Center
                .Vertical = TsCuStyleAlignment.VerticalType.Bottom
            End With
            With .Font
                .Bold = True
            End With
            With .Interior
                .Color = "#A5A5A5"
                .Pattern = TsCuStyleInterior.PatternType.Solid
            End With
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Bottom) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
        End With

        Return style
    End Function

    Public Shared Function ObtenirCelluleNom() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_NOM)
        With style
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Bottom) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
        End With
        Return style
    End Function

    Public Shared Function ObtenirCelluleNomHaut() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_NOM_HAUT)
        With style
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Bottom) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
        End With
        Return style
    End Function

    Public Shared Function ObtenirCelluleNomBas() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_NOM_BAS)
        With style
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Bottom) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
        End With
        Return style
    End Function

    Public Shared Function ObtenirCelluleNoUA() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_NO_UA)
        With style
            .Parent = ConstantesStyle.CELLULE_NOM
            With .Alignment
                .Horizontal = TsCuStyleAlignment.HorizontalType.Center
            End With
        End With
        Return style
    End Function

    Public Shared Function ObtenirCelluleNoUAHaut() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_NO_UA_HAUT)
        With style
            .Parent = ConstantesStyle.CELLULE_NOM_HAUT
            With .Alignment
                .Horizontal = TsCuStyleAlignment.HorizontalType.Center
            End With
        End With
        Return style
    End Function

    Public Shared Function ObtenirCelluleNoUABas() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_NO_UA_BAS)
        With style
            .Parent = ConstantesStyle.CELLULE_NOM_BAS
            With .Alignment
                .Horizontal = TsCuStyleAlignment.HorizontalType.Center
            End With
        End With
        Return style
    End Function

#End Region

#Region "--- Style Sous-Entête Coupée ---"

    Public Shared Function ObtenirCelluleSousEnteteCoupee() As TsCuStyle

        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_SOUS_ENTETE_COUPEE)
        With style
            With .Font
                .Bold = True
            End With
            With .Interior
                .Color = ConstantesCouleur.BLANC
                .Pattern = TsCuStyleInterior.PatternType.Solid
            End With
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Bottom) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
        End With

        Return style
    End Function

    Public Shared Function ObtenirCelluleContexteGauche() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_CONTEXTE_GAUCHE)
        With style
            With .Interior
                .Color = ConstantesCouleur.BLANC
                .Pattern = TsCuStyleInterior.PatternType.Solid
            End With
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Bottom) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
        End With
        Return style
    End Function

    Public Shared Function ObtenirCelluleContexteDroite() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_CONTEXTE_DROITE)
        With style
            .Parent = ConstantesStyle.CELLULE_CONTEXTE_GAUCHE
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Bottom) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
        End With
        Return style
    End Function

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
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Bottom) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
        End With
        Return style
    End Function

    Public Shared Function ObtenirCelluleTotalGauche() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_TOTAL_GAUCHE)
        With style
            .Parent = ConstantesStyle.CELLULE_TOTAL
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Bottom) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
        End With
        Return style
    End Function

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
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Bottom) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
        End With
        Return style
    End Function

    Public Shared Function ObtenirCelluleSousTotalGauche() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_SOUS_TOTAL_GAUCHE)
        With style
            .Parent = ConstantesStyle.CELLULE_SOUS_TOTAL
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Bottom) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
        End With
        Return style
    End Function

    Public Shared Function ObtenirCelluleSousTotalBarre() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_SOUS_TOTAL_BARRE)
        With style
            With .Interior
                .PatternColor = ConstantesCouleur.NOIR
                .Pattern = TsCuStyleInterior.PatternType.ThinDiagStripe
            End With
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Bottom) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
        End With
        Return style
    End Function

    Public Shared Function ObtenirCelluleSousTotalBarreGauche() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_SOUS_TOTAL_BARRE_GAUCHE)
        With style
            .Parent = ConstantesStyle.CELLULE_SOUS_TOTAL_BARRE
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Bottom) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
        End With
        Return style
    End Function

#End Region

#Region "--- Style Grille ---"

    ''' <summary>
    ''' Permet d'obtenir le style d'une séparateur foncé.
    ''' </summary>
    Public Shared Function ObtenirCelluleGrille() As TsCuStyle

        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_GRILLE)
        With style
            With .Alignment
                .Horizontal = TsCuStyleAlignment.HorizontalType.Center
            End With
            With .Interior
                .Color = "#FFFFFF"
                .Pattern = TsCuStyleInterior.PatternType.Solid
            End With
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Bottom) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = TsCuStyleBorder.LineStyleType.Dot, .Weight = 1})

            With .Protection
                .Protected = False
            End With
        End With

        Return style
    End Function

#End Region

#Region "--- Style Légende ---"

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

#End Region

#Region "--- Style Séparateur ---"

    ''' <summary>
    ''' Permet d'obtenir le style d'une séparateur foncé.
    ''' </summary>
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

    Public Shared Function ObtenirCelluleVideGauche() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_VIDE_GAUCHE)
        With style
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Left) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
        End With
        Return style
    End Function

    Public Shared Function ObtenirCelluleVideDroite() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_VIDE_DROITE)
        With style
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Right) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
        End With
        Return style
    End Function

    Public Shared Function ObtenirCelluleVideHaut() As TsCuStyle
        Dim style As New TsCuStyle(ConstantesStyle.CELLULE_VIDE_HAUT)
        With style
            .Borders.Add(New TsCuStyleBorder(TsCuStyleBorder.PositionType.Top) With {.LineStyle = TsCuStyleBorder.LineStyleType.Continuous, .Weight = 1})
        End With
        Return style
    End Function

#End Region

End Class
