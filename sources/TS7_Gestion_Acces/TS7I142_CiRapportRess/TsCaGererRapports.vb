
Imports Rrq.Securite.GestionAcces

''' <summary>
''' Classe point d'entrée. Donne accès au fonctionnalité de production de rapport.
''' </summary>
''' <remarks></remarks>
Public Class TsCaGererRapports

    ''' <summary>
    ''' Premet de produire un rapport Excel en fichier XML.
    ''' </summary>
    ''' <param name="pLstUaSlect">Liste des unités administratives sélectionnées.</param>
    ''' <returns>Un fichier XML en texte.</returns>
    ''' <remarks></remarks>
    Public Function ProduireRapportExcel(ByVal pLstUaSlect As List(Of Integer)) As String
        Dim uad As New TsCdObtenirDonnee()
        Dim dctSource As New Dictionary(Of String, TsDtSourceRapport)

        With dctSource
            Dim sourceFeuille1 As TsDtSourcFeuilPrinc


            sourceFeuille1 = uad.ObtenirSourcePrincipale(pLstUaSlect.ConvertAll(Function(ua) ua.ToString))


            If sourceFeuille1 IsNot Nothing Then
                dctSource.Add(TsCuConstantesRapport.ID_FEUILLE_1, sourceFeuille1)
                sourceFeuille1.PresenceFeuille2 = False '(sourceFeuille2 IsNot Nothing)
            End If


        End With

        Dim rapport As New TsCaComposerRapport(dctSource)

        Return rapport.ObtenirRapport()
    End Function

    Public Shared Function ObtenirListeUnitesAdmin() As List(Of String)
        Dim ParamRetour As List(Of String) = New List(Of String)
        Dim lstUA As New List(Of String)
        For Each c As TsCdSageUser In TsCuConfiguration.Utilisateurs
            lstUA.Add(c.OrganizationType)
        Next
        If Not (lstUA Is Nothing OrElse lstUA.Count = 0) Then
            Dim LstDistinct = From UA In lstUA
                              Select UA Distinct Order By UA

            ParamRetour = LstDistinct.ToList

        End If

        Return ParamRetour
    End Function

End Class
