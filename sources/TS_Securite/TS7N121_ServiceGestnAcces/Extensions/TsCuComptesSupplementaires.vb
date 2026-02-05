Imports System.Runtime.CompilerServices

Public Module TsCuComptesSupplementaires

    <Extension>
    Public Function ConvertirEnComptesSupplementaires(pUtilisateurSage As TsCdSageUser) As TsCdComptesSupplementaires
        Dim valeursStr As String() = pUtilisateurSage.Champ9.Split(";"c)
        Dim valeursBool As New List(Of Boolean)

        For Each valeur As String In valeursStr
            valeursBool.Add(valeur.ToUpper = "TRUE")
        Next

        Dim comptes As New TsCdComptesSupplementaires
        For i As Integer = 0 To valeursBool.Count - 1
            Select Case i
                Case 0
                    comptes.IndADMServeur = valeursBool(i)
                Case 1
                    comptes.IndADMPoste = valeursBool(i)
                Case 2
                    comptes.IndADMDevelopeur = valeursBool(i)
                Case 3
                    comptes.IndADMCentral = valeursBool(i)
                Case 4
                    comptes.IndEssaisAgent = valeursBool(i)
                Case 5
                    comptes.IndEssaisCE = valeursBool(i)
                Case 6
                    comptes.IndSoutienProdAgent = valeursBool(i)
                Case 7
                    comptes.IndSoutienProdCE = valeursBool(i)
            End Select
        Next

        Return comptes
    End Function

End Module
