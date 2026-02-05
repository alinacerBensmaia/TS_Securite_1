

''' <summary>
''' Cette classe contient la définiton des lignes du rapport.
''' </summary>
''' <remarks></remarks>
Friend Class TsCaLignesRapport

#Region "--- Variables ---"
    ' Définition des variable utilisé par toutes les méthodes.

    ''' <summary>La largeur totale du tableau en nombre de cellules.</summary>
    Private mLargeurTableau As ULong

    ''' <summary>La largeur totale de la section des sélectionnées en nombre de cellules.</summary>
    Private mLargeurUaSelect As ULong

    ''' <summary>La largeur totale de la section autres en nombre de cellules.</summary>
    Private mLargeurUaAutre As ULong

    ''' <summary>Le nombre d'employés dans le rapport.</summary>
    Private mNbEmployes As Integer

    'le nombre de ressources
    Private mNbRessources As Long

    ''' <summary>Le nombre de contexte de tâche dansle rapport.</summary>
    Private mNbContextes As Integer

    ''' <summary>Date de production du rapport.</summary>
    Private mDate As Date

    Private mdctUtilisateursParUA As Dictionary(Of String, Integer)

#End Region

#Region "--- Constructeurs ---"

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <param name="pSource">La source du rapport.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal pSource As TsDtSourceRapport)

        mLargeurTableau = CULng(TsCuOutilsRapport.ObtenirLargeurTableau(pSource))

        mNbEmployes = pSource.Employes.Count
        mNbRessources = pSource.LstRessources.Count

        mDate = pSource.DateProduction

        mdctUtilisateursParUA = pSource.dctUtilisateursParUA
    End Sub

#End Region

#Region "--- Méthodes ---"


    ''' <summary>
    ''' Permet d'obtenir l'en-tête du tableau.
    ''' </summary>
    ''' <returns>Une ligne.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirEnteteTableau() As TsCuRow
        Dim ligne As New TsCuRow()

        With ligne
            .Cells.Add(TsCaCellulesRapport.ObtenirVide(TsCuConstantesRapport.ConstantesStyle.CELLULE_VIDE_COIN_HAUT_GAUCHE))
            .Cells.Add(TsCaCellulesRapport.ObtenirVide(TsCuConstantesRapport.ConstantesStyle.CELLULE_VIDE_COIN_HAUT_DROIT))

            Dim style As String = TsCuConstantesRapport.ConstantesStyle.CELLULE_ENTETE_TABLEAU

            For Each UtParUA As KeyValuePair(Of String, Integer) In mdctUtilisateursParUA
                .Cells.Add(TsCaCellulesRapport.ObtenirEnteteTableau(style, Convert.ToInt32(UtParUA.Value), "Utilisateurs UA " & UtParUA.Key, UtParUA.Key))

            Next

        End With

        Return ligne
    End Function


    Public Function ObtenirEmployesSelectionnes(ByVal pSource As TsDtSourcFeuilPrinc) As TsCuRow
        Dim ligne As New TsCuRow()

        With ligne
            .Height = 200
            .Cells.Add(TsCaCellulesRapport.ObtenirVide(TsCuConstantesRapport.ConstantesStyle.CELLULE_VIDE_GAUCHE))
            .Cells.Add(TsCaCellulesRapport.ObtenirVide(TsCuConstantesRapport.ConstantesStyle.CELLULE_VIDE_DROITE))


            Dim style As String = TsCuConstantesRapport.ConstantesStyle.CELLULE_UA
            For Each empl In pSource.Employes
                .Cells.Add(TsCaCellulesRapport.ObtenirEmployesSelectionnes(style, empl.Nom))
            Next

        End With

        Return ligne
    End Function



    Public Function ObtenirRessourcesAssociees(ByVal pRessource As TsDtSourceRessources, ByVal pSource As TsDtSourcFeuilPrinc) As TsCuRow
        Dim ligne As New TsCuRow()

        With ligne
            '.AutoFitHeight = True
            .Cells.Add(TsCaCellulesRapport.ObtenirRessource(TsCuConstantesRapport.ConstantesStyle.CELLULE_NOM_HAUT, pRessource.Nom))
            .Cells.Add(TsCaCellulesRapport.ObtenirRessourceFonctionnel(TsCuConstantesRapport.ConstantesStyle.CELLULE_NOM_HAUT, pRessource.NomFonctionnel))

            RemplirCorrespondance(ligne, pRessource, pSource)


        End With

        Return ligne
    End Function


#End Region

#Region "--- Fonctions privées ---"


    ''' <summary>
    ''' Permet d'enrichir de la partie commune de chacune de type des lignes ressource.
    ''' </summary>
    ''' <param name="pLigne">La ligne à enrichir.</param>
    ''' <param name="pRessource">La ressource de la ligne.</param>
    ''' <remarks></remarks>
    Private Sub RemplirCorrespondance(ByVal pLigne As TsCuRow, ByVal pRessource As TsDtSourceRessources, ByVal pSource As TsDtSourcFeuilPrinc)
        With pLigne


            ' .AutoFitHeight = False

            ''pour chaque employé, si la ressource recherchée est associé à l'employé, on inscrit "X" sinon laisser vide.
            For Each empl As TsDtSourceEmploye In pSource.Employes
                .Cells.Add(TsCaCellulesRapport.ObtenirGrilleCorrespondance(pRessource.Nom, empl.lstRessourcesAssociees))
            Next


            .Cells.Add(TsCaCellulesRapport.ObtenirVide(TsCuConstantesRapport.ConstantesStyle.CELLULE_VIDE_GAUCHE))
        End With
    End Sub

    Public Function ObtenirTotaux() As TsCuRow
        Dim ligne As New TsCuRow()
        Dim style As String = TsCuConstantesRapport.ConstantesStyle.CELLULE_TOTAL
        With ligne

            .Cells.Add(TsCaCellulesRapport.ObtenirEnteteNbAssignations(style, 1, "Nombre d'assignations"))
            .Cells.Add(TsCaCellulesRapport.ObtenirVide(TsCuConstantesRapport.ConstantesStyle.CELLULE_VIDE_COIN_HAUT_DROIT))



            For i = 1 To mNbEmployes
                Dim formule = ConstruireFormuleTotaux()
                .Cells.Add(TsCaCellulesRapport.ObtenirTotal(style, formule))
            Next

        End With

        Return ligne
    End Function

    Private Function ConstruireFormuleTotaux() As String
        Dim formule As String = "="


        For i = 1 To mNbRessources
            If formule <> "=" Then formule &= "+"
            formule &= String.Format("IF(UPPER(R[-{0}]C)=&quot; X &quot;,1,0)", i)
        Next

        Return formule
    End Function

#End Region

End Class
