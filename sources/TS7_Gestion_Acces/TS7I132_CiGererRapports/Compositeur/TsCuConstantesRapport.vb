''' <summary>
''' Contient les constantes relier à la production du rapport.
''' </summary>
''' <remarks></remarks>
Friend Class TsCuConstantesRapport

    ''' <summary>Nom de la zone d'impression.</summary>
    Public Const NAMED_PRINT_AREA As String = "Print_Area"

    ''' <summary>Nom de la zone des colonnes et ligne barré pour l'impression.</summary>
    Public Const NAMED_PRINT_TITILES As String = "Print_Titles"

    ''' <summary>Première ligne ou le tableau commence.</summary>
    Public Const PREMIERE_LIGNE As Integer = 5

    ''' <summary>Première colonne ou le tableau commence.</summary>
    Public Const PREMIERE_COLONNE As Integer = 3

    ''' <summary>Identifiant de la feuille principale.</summary>
    Public Const ID_FEUILLE_1 As String = "Rapport"

    ''' <summary>Nom de la feuille principale.</summary>
    Public Const NOM_FEUILLE_1 As String = "Unité(s) sélectionnée(s)"

    ''' <summary>Identifiant de la feuille secondaire.</summary>
    Public Const ID_FEUILLE_2 As String = "Autre"

    ''' <summary>Nom de la feuille principale.</summary>
    Public Const NOM_FEUILLE_2 As String = "Autre(s) unité(s)"

    ''' <summary>
    ''' Classe des constantes contenant les styles disponibles dans le rapport.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ConstantesStyle

        Public Const CELLULE_DEFAULT As String = "Default"

        Public Const CELLULE_HYPERLIEN As String = "Hyperlien"

        Public Const CELLULE_ENTETE_TABLEAU As String = "CelluleEnteteTableau"
        Public Const CELLULE_SOUS_ENTETE_TABLEAU As String = "CelluleSousEnteteTableau"
        Public Const CELLULE_SOUS_ENTETE_COUPE As String = "CelluleSousEnteteCoupe"
        Public Const CELLULE_ENTETE_RAPPORT As String = "CelluleEnteteRapport"
        Public Const CELLULE_ENTETE_COUPE As String = "CelluleEnteteCoupee"

        Public Const CELLULE_SEPARATEUR_FONCE As String = "CelluleSeparateurFonce"
        Public Const CELLULE_SEPARATEUR_PALE As String = "CelluleSeparateurPale"

        Public Const CELLULE_VIDE_GAUCHE As String = "CelluleVideGauche"
        Public Const CELLULE_VIDE_DROITE As String = "CelluleVideDroite"
        Public Const CELLULE_VIDE_HAUT As String = "CelluleVideHaut"
        Public Const CELLULE_VIDE_COIN_HAUT_GAUCHE As String = "CelluleVideCoinHautGauche"
        Public Const CELLULE_VIDE_COIN_HAUT_DROIT As String = "CelluleVideCoinHautDroit"
        Public Const CELLULE_VIDE_BAS As String = "CelluleVideBas"

        Public Const CELLULE_UA As String = "CelluleUA"

        Public Const CELLULE_NOM As String = "CelluleNom"
        Public Const CELLULE_NOM_HAUT As String = "CelluleNomHaut"
        Public Const CELLULE_NOM_BAS As String = "CelluleNomBas"

        Public Const CELLULE_NO_UA As String = "CelluleNoUA"
        Public Const CELLULE_NO_UA_HAUT As String = "CelluleNoUAHaut"
        Public Const CELLULE_NO_UA_BAS As String = "CelluleNoUABas"

        Public Const CELLULE_GRILLE As String = "CelluleGrille"
        Public Const CELLULE_GRILLE_MARQUE As String = "CelluleGrilleMarqué"

        Public Const CELLULE_TOTAL As String = "CelluleTotal"
        Public Const CELLULE_TOTAL_GAUCHE As String = "CelluleTotalGauche"

        Public Const CELLULE_CONTEXTE_GAUCHE As String = "CelluleContexteGauche"
        Public Const CELLULE_CONTEXTE_DROITE As String = "CelluleContexteDroite"

        Public Const CELLULE_SOUS_TOTAL As String = "CelluleSousTotal"
        Public Const CELLULE_SOUS_TOTAL_GAUCHE As String = "CelluleSousTotalGauche"
        Public Const CELLULE_SOUS_TOTAL_BARRE As String = "CelluleSousTotalBarree"
        Public Const CELLULE_SOUS_TOTAL_BARRE_GAUCHE As String = "CelluleSousTotalBarreeGauche"

        Public Const CELLULE_LEGENDE_BARREE As String = "CelluleLegendeBarree"
        Public Const CELLULE_LEGENDE_JAUNE As String = "CelluleLegendeJaune"
        Public Const CELLULE_LEGENDE_TEXTE As String = "CelluleLegendeTexte"

    End Class

    ''' <summary>
    ''' Classe contenant les constantes les valeur RGB de couleurs.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ConstantesCouleur
        Public Const NOIR As String = "#000000"
        Public Const BLANC As String = "#FFFFFF"
        Public Const GRIS_PALE As String = "#D8D8D8"
        Public Const GRIS_FONCE As String = "#A5A5A5"
        Public Const JAUNE As String = "#FFFF00"
    End Class

    ''' <summary>
    ''' Constantes globales.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ConstanteGlobales
        Public Const NB_COLONNE_LEGENDE_1 As Integer = 15
        Public Const NB_COLONNE_LEGENDE_2 As Integer = 15
    End Class

End Class
