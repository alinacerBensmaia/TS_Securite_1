''' <summary>
''' Cette classe contient la définition des colonnes.
''' </summary>
''' <remarks></remarks>
Friend Class TsCaColonnesRapport

    ''' <summary>
    ''' Permet d'obtenir un colonne d'espacement.
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
    Public Shared Function NomEmployes() As TsCuColumn
        Dim colonne As New TsCuColumn()
        With colonne
            .AutoFitWidth = False
            .Width = 127.5
        End With
        Return colonne
    End Function

    ''' <summary>
    ''' Permet d'obtenir la colonne qui affiche les Unité Administrative d'un employé.
    ''' </summary>
    Public Shared Function UA() As TsCuColumn
        Dim colonne As New TsCuColumn()
        With colonne
            .AutoFitWidth = False
            .Width = 34.5
        End With
        Return colonne
    End Function

    ''' <summary>
    ''' Permet d'obtenir une colonne qui fait parti de la grille de saisie.
    ''' </summary>
    Public Shared Function Grille() As TsCuColumn
        Dim colonne As New TsCuColumn()
        With colonne
            .AutoFitWidth = False
            .Width = 24.75
        End With
        Return colonne
    End Function

    ''' <summary>
    ''' Permet d'obtenir une colonne qui sépare deux section de la grille de saisie.
    ''' </summary>
    Public Shared Function Separateur() As TsCuColumn
        Dim colonne As New TsCuColumn()
        With colonne
            .AutoFitWidth = False
            .Width = 6
        End With
        Return colonne
    End Function


End Class
