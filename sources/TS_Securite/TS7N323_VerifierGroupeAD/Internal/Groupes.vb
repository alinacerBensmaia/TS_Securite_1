Imports System.Collections.Generic
Imports System.DirectoryServices

Friend Class Groupes
    Inherits Dictionary(Of String, Groupe)

    Public Sub New(searchResults As SearchResultCollection)
        For Each groupe As SearchResult In searchResults
            Dim dn As String = groupe.distinguishedName()

            If Not contient(dn) Then
                Dim g As New Groupe()
                g.DistinguishedName = dn
                g.distinguishedNamesOfMembers.AjouterValeurs(groupe.member)
                g.distinguishedNamesOfMemberOf.AjouterValeurs(groupe.memberOf)

                ajouter(g)
            End If
        Next

        populateObjectLists()
    End Sub

    Private Function contient(clef As String) As Boolean
        Return ContainsKey(clef.ToUpper())
    End Function

    Private Function obtenirGroupe(clef As String) As Groupe
        Return Item(clef.ToUpper())
    End Function

    Private Sub ajouter(g As Groupe)
        Add(g.DistinguishedName.ToUpper(), g)
    End Sub

    Private Sub populateObjectLists()
        For Each groupe As Groupe In Values
            For Each dn As String In groupe.distinguishedNamesOfMembers
                If contient(dn) Then 'si c'est un groupe
                    groupe.Members.Add(obtenirGroupe(dn))
                End If
            Next
            For Each dn As String In groupe.distinguishedNamesOfMemberOf
                If contient(dn) Then 'si c'est un groupe
                    groupe.MemberOf.Add(obtenirGroupe(dn))
                End If
            Next
        Next
    End Sub
End Class