Imports System.Runtime.CompilerServices

Friend Module Extensions

    <Extension>
    Public Function EnChaineSansBlancs(source As Object) As String
        Dim retour As String = String.Empty

        If source IsNot Nothing Then
            retour = source.ToString().Trim()
        End If

        Return retour
    End Function

    ''' <summary>
    ''' Obtenir la lettre de l'environnement à partir du code d'environnement
    ''' </summary>
    ''' <param name="source">Code d'environnement</param>
    ''' <returns>Retourne la lettre de l'environnement</returns>
    ''' <remarks></remarks>
    <Extension>
    Public Function LettreEnvironnement(ByVal source As String) As String
        'retourne 'Tous' pour tous ce qui n'est pas valide
        Dim e As Environnements = Environnements.ParseCode(source)
        'retourne '' pour 'Tous'
        Return e.Lettre
    End Function

    <Extension>
    Public Function Est(source As String, valeur As String) As Boolean
        Return source.Equals(valeur, StringComparison.InvariantCultureIgnoreCase)
    End Function

    <Extension>
    Public Function TrimArray(ByVal source() As Byte, ByVal truncate As Boolean) As Byte()
        Dim enum1 As IEnumerator = source.GetEnumerator()
        Dim i As Integer = 0
        Dim lastData As Integer = 0

        While enum1.MoveNext()
            If Not enum1.Current.ToString().Equals("0") Then
                lastData = i
            End If
            i += 1
        End While
        If Not truncate Then
            lastData = ((lastData \ 16 + 1) * 16) - 1
        End If
        Dim returnedArray(lastData) As Byte
        Dim j As Integer
        For j = 0 To lastData
            returnedArray(j) = source(j)
        Next j

        Return returnedArray
    End Function

End Module
