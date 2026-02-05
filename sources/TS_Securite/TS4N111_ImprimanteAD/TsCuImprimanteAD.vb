Imports System.Collections.Generic
Imports System.DirectoryServices
Imports Rrq.InfrastructureCommune.Parametres
Imports Rrq.InfrastructureCommune.ScenarioTransactionnel


''' <summary>
''' Cette classe utilitaire récupère et stocke des informations sur le contenu de l'active directory.
''' </summary>
Public Class TsCuImprimanteAD
    Private ReadOnly _domaines As New List(Of String)

    Public Sub New()
        'ordre est important pour la recherche multidomaine... rq, rrq, carra
        ajouterDomaine("General", "ServeurActiveDirectory")
        ajouterDomaine("TS4", "TS4\ServeurActiveDirectory")
        ajouterDomaine("TS4", "TS4\ServeurActiveDirectoryCARRA")
    End Sub


    ''' <summary>
    ''' Obtenir à l'AD l'adresse de l'imprimante
    ''' </summary>
    ''' <param name="nomImprimante">Le nom du "PrintShareName" de l'Imprimante (ex, "I1570")</param>
    ''' <returns>Retourne l'adresse de l'imprimante qui est composée dui nom de serveur de l'imprimante et de son nom </returns>

    Public Function ObtenirAdresseImprimante(ByVal nomImprimante As String) As String
        nomImprimante = nomImprimante.Trim()

        Dim adresseImprimante As String = String.Empty
        For Each domaine As String In _domaines
            adresseImprimante = obtenirAdresseImprimanteDomaine(domaine, nomImprimante)
            If Not String.IsNullOrEmpty(adresseImprimante) Then Return adresseImprimante
        Next

        Dim gabarit As String = "L'imprimante '{0}' n'a pas été trouvé dans le(s) domaine(s) : {1}"
        Throw New XuExcEErrFatale(String.Format(gabarit, nomImprimante, String.Join(", ", _domaines)))
    End Function


    Private Sub ajouterDomaine(codeSysteme As String, clef As String)
        Dim domaine As String = XuCuPolitiqueConfig.ConfigDomaine.ObtenirValeurSystemeOptionnelle(codeSysteme, clef)

        'si la valeur n'est pas définit, ne pas ajouter une valeur vide
        If Not String.IsNullOrWhiteSpace(domaine) Then _domaines.Add(domaine)
    End Sub

    Private Function obtenirAdresseImprimanteDomaine(domaine As String, nomImprimante As String) As String
        Using deRoot As New DirectoryEntry()
            deRoot.AuthenticationType = AuthenticationTypes.Secure
            deRoot.Path = String.Format("LDAP://{0}", domaine)

            Using dseSearcher As New DirectorySearcher(deRoot)
                dseSearcher.Filter = String.Format("(&(objectClass=printQueue)(printShareName={0}))", nomImprimante)
                dseSearcher.PropertiesToLoad.Add("printShareName")
                dseSearcher.PropertiesToLoad.Add("uNCName")

                Dim dseResult As SearchResult = dseSearcher.FindOne
                If dseResult Is Nothing Then Return String.Empty
                If dseResult.Properties.Item("uNCName") Is Nothing Then Return String.Empty
                If dseResult.Properties.Item("uNCName")(0) Is Nothing Then Return String.Empty

                Return dseResult.Properties.Item("uNCName")(0).ToString()
            End Using
        End Using
    End Function

End Class