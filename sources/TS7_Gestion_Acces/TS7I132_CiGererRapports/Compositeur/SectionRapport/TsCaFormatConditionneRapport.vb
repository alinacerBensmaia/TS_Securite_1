
''' <summary>
''' Cette classe contient la définition des formats de cellules conditionnées.
''' </summary>
''' <remarks></remarks>
Friend Class TsCaFormatConditionneRapport

#Region "--- Variables ---"

    ''' <summary>La source d'information pour réalisé les formats conditionnés.</summary>
    Private mSource As TsDtSourceRapport

#End Region

#Region "--- Constructeurs ---"

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <param name="pSource">La source du rapport.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal pSource As TsDtSourceRapport)
        mSource = pSource
    End Sub

#End Region

#Region "--- Méthodes ---"

    ''' <summary>
    ''' Permet de remplir la page de formats conditonnés.
    ''' </summary>
    ''' <param name="pPage">Une page de travail.</param>
    ''' <remarks></remarks>
    Public Sub AjouterFormatConditionne(ByVal pPage As TsCuWorksheet)
        With pPage
            AjouterCondtionGrille(.ConditionnalFormattings)
            AjouterCondtionTotaux(.ConditionnalFormattings)
        End With
    End Sub

#End Region

#Region "--- Fonctions privées ---"

    ''' <summary>
    ''' Permet d'ajouter la condition de la grille de saisie.
    ''' </summary>
    ''' <param name="lstConditions">La liste des formats conditionnés qui recevera la condition.</param>
    ''' <remarks></remarks>
    Private Sub AjouterCondtionGrille(ByVal lstConditions As List(Of TsCuConditionalFormatting))
        Dim premiereLigne = TsCuConstantesRapport.PREMIERE_LIGNE
        Dim derniereLigne = premiereLigne + mSource.Employes.Count - 1
        Dim premiereColonne = TsCuConstantesRapport.PREMIERE_COLONNE
        Dim derniereColonne = premiereColonne + ObtenirLargeurTableau() - 3

        If derniereColonne < premiereColonne Then derniereColonne = premiereColonne

        Dim zone As String = String.Format("R{0}C{1}:R{2}C{3}", premiereLigne, premiereColonne, derniereLigne, derniereColonne)

        Dim condition1x As String = ""
        For i = 0 To mSource.Contextes.Count - 1
            If condition1x <> "" Then condition1x &= ","
            condition1x &= String.Format("AND(R{0}C{1}=R[0]C[0],R{0}C[0]&lt;1)", derniereLigne + 2 + i, premiereColonne - 1)
        Next
        Dim condition1 As String
        If condition1x = "" Then
            condition1 = "FALSE" 'Si aucune condition acceptable metre la valeur neutre du OU.
        Else
            condition1 = String.Format("OR({0})", condition1x)
        End If

        Dim condition2x As String = "NOT(R[0]C[0]=&quot;X&quot;)," & _
                                    "NOT(R[0]C[0]=&quot;&quot;)"
        For i = 0 To mSource.Contextes.Count - 1
            condition2x &= String.Format(",NOT(R{0}C{1}=R[0]C[0])", derniereLigne + 2 + i, premiereColonne - 1)
        Next
        Dim condition2 As String
        If condition2x = "" Then
            condition2 = "FALSE" 'Si aucune condition acceptable metre la valeur neutre du OU.
        Else
            condition2 = String.Format("AND({0})", condition2x)
        End If

        Dim condition3x As String = ""
        For i = 0 To mSource.Contextes.Count - 1
            If condition3x <> "" Then condition3x &= ","
            condition3x &= String.Format("NOT(R{0}C[0]=&quot;&quot;)", derniereLigne + 2 + i)
        Next
        Dim condition3 As String
        If condition3x = "" Then
            condition3 = "FALSE" 'Si aucune condition acceptable metre la valeur neutre du OU.
        Else
            condition3 = String.Format("AND(R[0]C[0]=&quot;X&quot;,OR({0}))", condition3x)
        End If

        Dim conditions As String = String.Format("OR({0},{1},{2})", condition1, condition2, condition3)

        lstConditions.Add(New TsCuConditionalFormatting(zone, New TsCuCondition(conditions, New TsCuFormat("Background:yellow"))))
    End Sub

    ''' <summary>
    ''' Permet d'ajouter la condtion de la ligne des totaux.
    ''' </summary>
    ''' <param name="lstConditions">La liste des formats conditionnés qui recevera la condition.</param>
    ''' <remarks></remarks>
    Private Sub AjouterCondtionTotaux(ByVal lstConditions As List(Of TsCuConditionalFormatting))
        Dim premiereLigne = TsCuConstantesRapport.PREMIERE_LIGNE
        Dim derniereLigne = premiereLigne + mSource.Employes.Count - 1
        Dim premiereColonne = TsCuConstantesRapport.PREMIERE_COLONNE
        Dim derniereColonne = premiereColonne + ObtenirLargeurTableau() - 3

        If derniereColonne < premiereColonne Then derniereColonne = premiereColonne

        Dim zone As String = String.Format("R{0}C{1}:R{0}C{2}", derniereLigne + 1, premiereColonne, derniereColonne)

        Dim condition1x As String = ""
        For i = 1 To mSource.Employes.Count
            If condition1x <> "" Then condition1x &= "+"
            condition1x &= String.Format("IF(R[-{0}]C[0]=&quot;&quot;,1,0)", i)
        Next
        If condition1x = "" Then condition1x = "0"
        Dim conditions As String = String.Format("NOT({0}-R[0]C[0]=({1}))", mSource.Employes.Count, condition1x)

        lstConditions.Add(New TsCuConditionalFormatting(zone, New TsCuCondition(conditions, New TsCuFormat("Background:yellow"))))
    End Sub

    ''' <summary>
    ''' Fonction de service. Permet d'obtenir la largeur du tableau du rapport.
    ''' </summary>
    ''' <returns>La largeur du tableau.</returns>
    ''' <remarks></remarks>
    Private Function ObtenirLargeurTableau() As ULong
        Return CULng(TS7I132_CiGererRapports.TsCuOutilsRapport.ObtenirLargeurTableau( _
                                                            mSource.ObtenirMetiersSelectionnes.Count, _
                                                            mSource.ObtenirRolesTachesSelectionnes.Count, _
                                                            mSource.ObtenirMetiersAutre.Count, _
                                                            mSource.ObtenirRolesTachesAutre.Count) _
                    )
    End Function

#End Region

End Class
