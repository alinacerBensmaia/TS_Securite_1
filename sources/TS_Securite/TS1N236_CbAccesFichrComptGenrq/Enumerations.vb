Imports System.Collections.Generic
Imports System.Runtime.CompilerServices
Imports Rrq.InfrastructureCommune.Parametres
Imports Rrq.Securite

Public Class Zones
    Implements IEnumerable(Of Zone)
    Private ReadOnly _zones As IEnumerable(Of Zone)

    Friend Sub New(zones As IEnumerable(Of Zone))
        _zones = zones
    End Sub

    Public Function Contient(valeur As Zone) As Boolean
        Return _zones.Contains(valeur)
    End Function

    Public Function ToStringList() As String
        Return String.Format("'{0}'", String.Join("','", _zones))
    End Function

    Public Function GetEnumerator() As IEnumerator(Of Zone) Implements IEnumerable(Of Zone).GetEnumerator
        Return _zones.GetEnumerator()
    End Function

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return _zones.GetEnumerator()
    End Function
End Class

Public Class Zone
    Public Shared Domaine As New Zone("D", "Domaine")
    Public Shared HorsDomaine As New Zone("H", "Hors domaine")
    Public Shared Inforoute As New Zone("I", "Inforoute")
    Public Shared InforouteAvecVerification As New Zone("IV", "Inforoute avec vérification")
    Public Shared All As New Zones({Domaine, HorsDomaine, Inforoute, InforouteAvecVerification})

    Public ReadOnly Property Code As String
    Public ReadOnly Property Description As String

    Public Sub New(code As String, description As String)
        _Code = code
        _Description = description
    End Sub

    Public Shared Function Parse(code As String) As Zone
        code = code.Replace("'", String.Empty)
        For Each z As Zone In All
            If z.Code.Est(code) Then Return z
        Next
        Throw New ArgumentException(String.Format("Le code de zone '{0}' est invalide.", code), "code")
    End Function

    Public Shared Function ParseFromDescription(description As String) As Zone
        For Each z As Zone In All
            If z.Description.Est(description) Then Return z
        Next
        Throw New ArgumentException(String.Format("La description de zone '{0}' est invalide.", description), "code")
    End Function


    Public Shared Function GetCurrents() As Zones
        Dim valeurConfig As String = XuCuPolitiqueConfig.ConfigDomaine.ValeurSysteme("TS1", "TS1\TS1N236\Zone")
        If String.IsNullOrWhiteSpace(valeurConfig) Then Throw New ArgumentException("La zone spécifiée dans le fichier de configuration est invalide.")

        Dim zones() As String = valeurConfig.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
        Dim retour As New List(Of Zone)
        For Each code As String In zones
            retour.Add(Zone.Parse(code))
        Next
        Return New Zones(retour)
    End Function

    Public Overrides Function ToString() As String
        Return String.Format("{0}", _Code)
    End Function

    Public Function Est(valeur As Zone) As Boolean
        Return _Code.Est(valeur.Code)
    End Function

    Public Function Est(valeurs As IEnumerable(Of Zone)) As Boolean
        For Each valeur As Zone In valeurs
            If Est(valeur) Then Return True
        Next
        Return False
    End Function

End Class

Public Module EnumExtensions

    <Extension>
    Public Function Ou(source As Zone, valeur As Zone) As IEnumerable(Of Zone)
        Return {source, valeur}
    End Function

    <Extension>
    Public Function Ou(source As IEnumerable(Of Zone), valeur As Zone) As IEnumerable(Of Zone)
        Dim retour As New List(Of Zone)(source)
        retour.Add(valeur)
        Return retour
    End Function

End Module