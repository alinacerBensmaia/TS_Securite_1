Imports TS7I132_CiGererRapports.TsCuConstantesRapport

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
            Dim sourceFeuille2 As TsDtSourceRapport

            sourceFeuille1 = uad.ObtenirSourcePrincipale(pLstUaSlect.ConvertAll(Function(ua) ua.ToString))
            sourceFeuille2 = uad.ObtenirSourceSecondaire(pLstUaSlect.ConvertAll(Function(ua) ua.ToString))

            If sourceFeuille1 IsNot Nothing Then
                dctSource.Add(ID_FEUILLE_1, sourceFeuille1)
                sourceFeuille1.PresenceFeuille2 = (sourceFeuille2 IsNot Nothing)
            End If

            If sourceFeuille2 IsNot Nothing Then
                dctSource.Add(ID_FEUILLE_2, sourceFeuille2)
            End If

        End With

        Dim rapport As New TsCaComposerRapport(dctSource)

        Return rapport.ObtenirRapport()
    End Function

End Class
