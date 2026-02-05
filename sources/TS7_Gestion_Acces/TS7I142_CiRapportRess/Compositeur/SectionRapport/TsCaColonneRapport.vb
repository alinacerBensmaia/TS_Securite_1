''' <summary>
''' Cette classe contient la définition des colonnes.
''' </summary>
''' <remarks></remarks>
Friend Class TsCaColonnesRapport

    ''' <summary>
    ''' Permet d'obtenir une colonne d'espacement.
    ''' </summary>
    Public Shared Function Espacement() As TsCuColumn
        Dim colonne As New TsCuColumn()
        With colonne
            .AutoFitWidth = False
            .Width = 24.75
        End With
        Return colonne
    End Function

    ''' <summary>
    ''' Permet d'obtenir une colonne prédéfinit pour les employés.
    ''' </summary>
    Public Shared Function NomResources() As TsCuColumn
        Dim colonne As New TsCuColumn()
        With colonne
            '.AutoFitWidth = False
            .AutoFitWidth = True
            '.Width = 127.5
            .Width = 200
        End With
        Return colonne
    End Function

    Public Shared Function NomFonctionnelResources() As TsCuColumn
        Dim colonne As New TsCuColumn()
        With colonne
            '.AutoFitWidth = False
            .AutoFitWidth = True
            '.Width = 127.5
            .Width = 200
        End With
        Return colonne
    End Function



    ''' <summary>
    ''' Permet d'obtenir une colonne qui fait partie de la grille de saisie.
    ''' </summary>
    Public Shared Function Grille() As TsCuColumn
        Dim colonne As New TsCuColumn()
        With colonne
            .AutoFitWidth = False
            .Width = 30
        End With
        Return colonne
    End Function

End Class
