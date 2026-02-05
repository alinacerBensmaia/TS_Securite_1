Imports System.Text
Imports System.Text.RegularExpressions
Imports Rrq.InfrastructureCommune.ScenarioTransactionnel
Imports TS1N201_DtCdAccGenV1

Public NotInheritable Class TsCuGroupe
    Private Sub New()
        'Classe de méthodes statique non instanciable
    End Sub

    Friend Shared Sub GererGroupe(ByRef pChaineContexte As String, ByVal pCle As TsDtCleSym, ByVal pAccesDonnee As TsCdCodAccGen)
        'Rien a faire si il n'y a pas de groupe
        If pCle.LsGroAd.Count = 0 Then Return

        pAccesDonnee.LibererGroupe(pChaineContexte, pCle)
        For Each groupe As TsDtGroAd In pCle.LsGroAd
            If Not pAccesDonnee.ExisteGroupe(pChaineContexte, groupe) Then
                pAccesDonnee.InsertGroupe(pChaineContexte, groupe)
            End If
            pAccesDonnee.LierGroupe(pChaineContexte, pCle, groupe)
        Next
    End Sub

    Friend Shared Function GererMultiple(ByVal str As String, ByVal env As String) As String
        Dim match As Match = Regex.Match(str, env & ":<<([^>>]+)>>", RegexOptions.Compiled)
        If match.Success Then
            Return match.Groups(1).Value
        Else
            match = Regex.Match(str, ":<<([^>>]+)>>", RegexOptions.Compiled)
            If match.Success Then
                Throw New XuExcEErrValidation("Erreur lors du traitement des information par environnement, essayez de nouveau.")
            End If
            Return str
        End If
    End Function

    Friend Shared Function GererMultipleGroupe(ByVal pLstGroupe As IList(Of TsDtGroAd), ByVal pEnv As String) As IList(Of TsDtGroAd)
        Dim match As Match
        Dim found As Boolean = False
        Dim result As IList(Of TsDtGroAd) = Nothing
        Dim groupe As TsDtGroAd = Nothing

        For Each groupe In pLstGroupe
            match = Regex.Match(groupe.NmGroActDirTs, pEnv & ":<<([^>>]+)>>", RegexOptions.Compiled)
            If match.Success Then
                result = (From item In match.Groups(1).Value.Split(","c).AsEnumerable Select New TsDtGroAd With {.NmGroActDirTs = item.Trim}).ToList
                found = True
                Exit For
            End If
        Next

        If Not found Then
            result = pLstGroupe
        End If
        Return result
    End Function
End Class
