Imports Rrq.InfrastructureCommune.Parametres

Friend Class FabriqueFiltreConformite

    Public Shared Function Creer(conformiteActivee As Boolean) As IFiltreConformite
        Dim retour As New ListOfFiltreConformite()

        If conformiteActivee Then
            Dim valeur As String = XuCuPolitiqueConfig.ConfigDomaine.ObtenirValeurSystemeOptionnelle("TS4", "TS4\TS4N213\ConformiteActiveePourFiltres")
            If Not String.IsNullOrWhiteSpace(valeur) Then
                retour.AddRange(valeur.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            End If
        End If

        Return retour
    End Function

End Class

Public Interface IFiltreConformite
    Function Correspond(valeur As String) As Boolean
End Interface
