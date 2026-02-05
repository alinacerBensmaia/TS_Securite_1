Imports System.Collections.Generic

Friend Class Groupe
    Public Property DistinguishedName As String
    Public Property Members As New List(Of Groupe)
    Public Property MemberOf As New List(Of Groupe)
    Public Property distinguishedNamesOfMembers As New DistinctList()
    Public Property distinguishedNamesOfMemberOf As New DistinctList()


    Public Function GetMembersAsListOfStrings() As IEnumerable(Of String)
        Dim retour As New DistinctList()
        retour.Ajouter(DistinguishedName)

        For Each g As Groupe In Members
            retour.AjouterValeurs(g.GetMembersAsListOfStrings)
        Next
        Return retour
    End Function

    Public Function GetMemberOfAsListOfStrings() As IEnumerable(Of String)
        Dim retour As New DistinctList()
        retour.Ajouter(DistinguishedName)

        For Each g As Groupe In MemberOf
            retour.AjouterValeurs(g.GetMemberOfAsListOfStrings)
        Next
        Return retour
    End Function
End Class