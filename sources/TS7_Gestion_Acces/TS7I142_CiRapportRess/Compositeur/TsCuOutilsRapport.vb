''' <summary>
''' Classe qui concentre des fonction utile à la conception du rapport.
''' </summary>
''' <remarks></remarks>
Friend Class TsCuOutilsRapport

    ''' <summary>
    ''' Permet de calculer la largeur réel du tableau qui sera produit.
    ''' </summary>
    ''' <returns>La largeur du tableau.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirLargeurTableau(ByVal pSource As TsDtSourceRapport) As Long
        Dim largeurEnteteCoupee As Integer = 2

        Dim NbreUtilisateur As Long = pSource.Employes.Count

        Dim resultat As Long = largeurEnteteCoupee + NbreUtilisateur

        Return resultat
    End Function




End Class
