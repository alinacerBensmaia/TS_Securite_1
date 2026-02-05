Imports System.Collections.Generic
Imports System.DirectoryServices
Imports Rrq.InfrastructureCommune.Parametres

Friend Class Chercheur
    Private ReadOnly _serveurActiveDirectory As String
    Private ReadOnly _attributUniteAdministrative As String


    Public Sub New(serveurActiveDirectory As String, attributUniteAdministrative As String)
        _serveurActiveDirectory = serveurActiveDirectory
        _attributUniteAdministrative = attributUniteAdministrative
    End Sub

    Private Function obtenirRoot() As DirectoryEntry
        Dim modeAD As String = XuCuConfiguration.ValeurSysteme("TS7", "TS7N323\ConnecterSurAD")

        If String.Compare(modeAD, "True", True) = 0 Then
            Return New DirectoryEntry(_serveurActiveDirectory)
        Else
            Dim adresse As String = XuCuConfiguration.ValeurSysteme("TS7", "TS7N323\AdresseADAM")
            Return New DirectoryEntry(adresse)
        End If
    End Function

    Public Function ObtenirUniteAdministrative(ByVal numeroUniteAdministrative As String) As TsCdUniteAdministrative
        Dim retour As New TsCdUniteAdministrative()
        retour.NumeroUniteAdministrative = numeroUniteAdministrative
        retour.DateDeSauvegarde = DateTime.Now
        retour.ListeUtilisateurs.AddRange(obtenirMembresUniteAdministrative(numeroUniteAdministrative))

        Return retour
    End Function


    Private Function obtenirMembresUniteAdministrative(numeroUniteAdministrative As String) As List(Of TsCdUtilisateur)
        Dim retour = New List(Of TsCdUtilisateur)

        Using root As DirectoryEntry = obtenirRoot()
            Dim tousLesGroupes As Groupes = obtenirTousLesGroupes(root)

            Using chercheur As New DirectorySearcher(root)
                chercheur.SearchScope = SearchScope.Subtree
                chercheur.Filter = String.Format("(&(objectCategory=person)({0}={1}))", _attributUniteAdministrative, numeroUniteAdministrative)
                chercheur.ReturnDistinguishedName().ReturnSAMAccountName().ReturnDisplayName().ReturnMemberOf()
                chercheur.PageSize = 1000
                chercheur.SizeLimit = 0

                Using searchResults As SearchResultCollection = chercheur.FindAll()
                    For Each employe As SearchResult In searchResults
                        Dim utilisateur As TsCdUtilisateur = New TsCdUtilisateur()
                        utilisateur.CodeUtilisateur = employe.sAMAccountName()
                        utilisateur.NomComplet = employe.DisplayName()

                        Dim list As New DistinctList()
                        For Each unGroupe As Groupe In tousLesGroupes.Values
                            If unGroupe.distinguishedNamesOfMembers.Contient(employe.distinguishedName) Then
                                list.AjouterValeurs(unGroupe.GetMemberOfAsListOfStrings)
                            End If
                        Next
                        list.Sort()
                        utilisateur.ListeGroupes.AddRange(list)

                        retour.Add(utilisateur)
                    Next
                End Using
            End Using
        End Using

        Return retour
    End Function

    Private Function obtenirTousLesGroupes(root As DirectoryEntry) As Groupes
        Using chercheur As New DirectorySearcher(root)
            chercheur.SearchScope = SearchScope.Subtree
            chercheur.Filter = "(&(objectClass=group))"
            chercheur.ReturnDistinguishedName().ReturnMember().ReturnMemberOf()
            chercheur.PageSize = 1000
            chercheur.SizeLimit = 0

            Using collectionResultats As SearchResultCollection = chercheur.FindAll()
                Return New Groupes(collectionResultats)
            End Using
        End Using
    End Function

End Class