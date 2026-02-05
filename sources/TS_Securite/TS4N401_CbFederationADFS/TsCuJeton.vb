Imports System.Security.Claims

Friend Class TsCuJeton


    Friend Shared Function ObtenirInfClaims(Claims As IEnumerable(Of Claim)) As String
        Dim retour As String = "Aucune identité"
        If Claims IsNot Nothing Then
            retour = String.Empty
            For Each claim As System.Security.Claims.Claim In Claims
                retour = String.Concat(retour, Environment.NewLine, FormaterClaim(claim))
            Next
        End If
        Return retour
    End Function
    Private Shared Function FormaterClaim(claim As System.Security.Claims.Claim) As String
        Dim retour As String = String.Empty
        retour = String.Concat(claim.Type, ":", claim.Value)
        Return retour
    End Function


End Class
