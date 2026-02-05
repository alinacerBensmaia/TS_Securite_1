Imports System.Runtime.CompilerServices

Namespace Internal

    Friend Module StringExtensions

        ''' <summary>
        ''' Détermine si cette chaîne et une chaîne spécifié ont la même valeur, ignorant la casse.
        ''' </summary>
        ''' <param name="source">Cette chaîne.</param>
        ''' <param name="valeur">La chaîne à comparer.</param>
        ''' <returns>true si la valeur du paramètre valeur est la même que cette chaîne, sinon false.</returns>
        <Extension>
        Public Function Est(source As String, valeur As String) As Boolean
            Return source.Equals(valeur, StringComparison.InvariantCultureIgnoreCase)
        End Function

        ''' <summary>
        ''' Détermine si le début de cette instance de chaîne correspond à la chaîne spécifiée, ignorant la casse.
        ''' </summary>
        ''' <param name="source">Cette chaîne.</param>
        ''' <param name="valeur">La chaîne à comparer.</param>
        ''' <returns>true si cette instance commence par valeur, sinon false.</returns>
        <Extension>
        Public Function CommencePar(source As String, valeur As String) As Boolean
            Return source.StartsWith(valeur, StringComparison.InvariantCultureIgnoreCase)
        End Function

        ''' <summary>
        ''' Obtient la lettre d'environnement d'un groupe de sécurité applicative.
        ''' </summary>
        ''' <param name="source">Le nom du groupe.</param>
        ''' <returns>La lettre représentant le code d'environnement du groupe.</returns>
        ''' <remarks>
        ''' La lettre d'environnement doit être à l'index 4 de la chaine de caractères.
        ''' </remarks>
        <Extension>
        Public Function CodeEnvironnement(source As String) As String
            Return source.Substring(4, 1)
        End Function

        ''' <summary>
        ''' Obtient le mot utilisé dans le dépôt ADLDS correspondant au code d'environnement permis dans un nom de groupe de sécurité applicative.
        ''' </summary>
        ''' <param name="source">Le lettre correspondant à un code d'environnement.</param>
        ''' <returns>Le mot correspondant au code d'environnement.</returns>
        ''' <exception cref="ArgumentException">Lorsque le code d'environnement n'est pas valide.</exception>
        <Extension>
        Public Function ToMotEnvironnement(source As String) As String
            If source.Length <> 1 Then Throw New ArgumentException("Le code doit avoir un seul caractère.")
            If Not {"D", "U", "I", "A", "B", "S", "Q", "P"}.Contient(source) Then Throw New ArgumentException("Le code n'est pas valide.")

            If source.Est("D") Then Return "Communs"
            If source.Est("U") Then Return "UNIT"
            If source.Est("I") Then Return "INTG"
            If source.Est("A") Then Return "ACCP"
            If source.Est("B") Then Return "FORA"
            If source.Est("S") Then Return "SIML"
            If source.Est("Q") Then Return "FORP"
            If source.Est("P") Then Return "PROD"

            Throw New ApplicationException("Ne devrait jamais ce rendre ici.")
        End Function

        ''' <summary>
        ''' Retourne une valeur qui indique si la chaîne spécifiée apparaît dans cette liste de chaîne.
        ''' </summary>
        ''' <param name="source">Cette liste de chaîne.</param>
        ''' <param name="valeur">La chaîne à rechercher.</param>
        ''' <returns>true si le paramètre valeur apparaît dans cette liste, sinon false.</returns>
        <Extension>
        Public Function Contient(source() As String, valeur As String) As Boolean
            For Each v As String In source
                If v.Est(valeur) Then Return True
            Next
            Return False
        End Function
    End Module

End Namespace