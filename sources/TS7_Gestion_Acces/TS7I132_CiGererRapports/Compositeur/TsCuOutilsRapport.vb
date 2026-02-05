''' <summary>
''' Classe qui concentre des fonction utile à la conception du rapport.
''' </summary>
''' <remarks></remarks>
Friend Class TsCuOutilsRapport

    ''' <summary>
    ''' Permet de calculer la largeur réel du tableau qui sera produit.
    ''' </summary>
    ''' <param name="pNbMetiersSelect">Le nombre de metiers sélectionnés.</param>
    ''' <param name="pNbTachesSelect">Le nombre de tâches sélectionnés.</param>
    ''' <param name="pNbMetiersAutre">Le nombre de metiers non-sélectionnés.</param>
    ''' <param name="pNbTachesAutre">Le nombre de tâches non-sélectionnés.</param>
    ''' <returns>La largeur du tableau.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirLargeurTableau(ByVal pNbMetiersSelect As Integer, _
                                                 ByVal pNbTachesSelect As Integer, _
                                                 ByVal pNbMetiersAutre As Integer, _
                                                 ByVal pNbTachesAutre As Integer) As Integer
        Dim largeurEnteteCoupee = 2
        Dim sectionSelect = ObtenirLargeurSectionUaSelect(pNbMetiersSelect, pNbTachesSelect)
        Dim sectionautre = ObtenirLargeurSectionUaAutre(pNbMetiersAutre, pNbTachesAutre)


        Dim resultat = largeurEnteteCoupee + sectionautre + sectionSelect

        If sectionautre > 0 And sectionSelect > 0 Then ' Colonne du séparateur
            resultat += 1
        End If

        Return resultat
    End Function

    ''' <summary>
    ''' Permet d'obtenir la largeur de la sction des sélectionnés.
    ''' </summary>
    ''' <param name="pNbMetiersSelect">Le nombre de metiers sélectionnés.</param>
    ''' <param name="pNbTachesSelect">Le nombre de tâches sélectionnés.</param>
    ''' <returns>La largeur de la section des sélectionnés.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirLargeurSectionUaSelect(ByVal pNbMetiersSelect As Integer, ByVal pNbTachesSelect As Integer) As Integer
        Dim nbMetiersSelect As Integer = pNbMetiersSelect
        Dim nbTachesSelect As Integer = pNbTachesSelect

        Dim resultat = nbMetiersSelect + nbTachesSelect + 1 ' La colonne séparatrice de départ.

        If nbMetiersSelect > 0 And nbTachesSelect > 0 Then ' Colonne du séparateur
            resultat += 1
        End If

        Return resultat
    End Function

    ''' <summary>
    ''' Permet d'obtenir la largeur de la sction des non-sélectionnés.
    ''' </summary>
    ''' <param name="pNbMetiersAutre">Le nombre de metiers non-sélectionnés.</param>
    ''' <param name="pNbTachesAutre">Le nombre de tâches non-sélectionnés.</param>
    ''' <returns>La largeur de la section des non-sélectionnés.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirLargeurSectionUaAutre(ByVal pNbMetiersAutre As Integer, ByVal pNbTachesAutre As Integer) As Integer
        Dim nbMetiersAutre As Integer = pNbMetiersAutre
        Dim nbTachesAutre As Integer = pNbTachesAutre

        Dim resultat = nbMetiersAutre + nbTachesAutre

        If nbMetiersAutre > 0 And nbTachesAutre > 0 Then ' Colonne du séparateur
            resultat += 1
        End If

        Return resultat
    End Function

    ''' <summary>
    ''' Calcule le nombre minumum de colone pour le rapport.
    ''' </summary>
    ''' <param name="NbreReelTableau">Nombre réel de colonne.</param>
    ''' <returns>Renvois le nombre minimum de canal.</returns>
    ''' <remarks></remarks>
    Public Shared Function CalculerNbreColonneMinimum(ByVal NbreReelTableau As ULong?) As ULong
        Dim cmptLegende1 As Integer = TsCuConstantesRapport.ConstanteGlobales.NB_COLONNE_LEGENDE_1
        Dim cmptLegende2 As Integer = TsCuConstantesRapport.ConstanteGlobales.NB_COLONNE_LEGENDE_2

        Dim minColonne As Integer

        minColonne = Math.Max(cmptLegende1, cmptLegende2)

        Return CULng(Math.Max(minColonne, CULng(NbreReelTableau)))

    End Function

End Class
