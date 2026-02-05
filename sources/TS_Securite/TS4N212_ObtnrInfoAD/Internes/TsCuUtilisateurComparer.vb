
Imports System.Collections.Generic

Public Class TsCuUtilisateurADComparer
    Implements IEqualityComparer(Of TsCuUtilisateurAD)

    Public Function Equals(x As TsCuUtilisateurAD, y As TsCuUtilisateurAD) As Boolean Implements IEqualityComparer(Of TsCuUtilisateurAD).Equals
        If (Object.ReferenceEquals(x, y)) Then
            Return True
        End If

        Return x.CodeUtilisateur.ToUpper.Trim = y.CodeUtilisateur.ToUpper.Trim

    End Function

    Public Function GetHashCode(obj As TsCuUtilisateurAD) As Integer Implements IEqualityComparer(Of TsCuUtilisateurAD).GetHashCode
        Return obj.CodeUtilisateur.GetHashCode Xor obj.NomComplet.GetHashCode
    End Function
End Class