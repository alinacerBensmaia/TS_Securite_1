Imports System.Collections.Generic
Imports System.Linq

Public Class FiltreConformite
    Implements IFiltreConformite
    Private Const CHAR_ETOILE As String = "*"
    Private Const CHAR_REMPLACEMENT As String = "?"
    Private Const ABSENT As Integer = -1

    Private ReadOnly _regle As String
    Private ReadOnly _typeRegle As Types
    Private ReadOnly _etoileIndex As Integer
    Private ReadOnly _qMarkIndexes As List(Of Integer)

    Public Enum Types
        Plain
        Wildcard
        Caracter
        WildcardAndCaracter
    End Enum

    Public Sub New(regle As String)
        If String.IsNullOrWhiteSpace(regle) Then Throw New ArgumentException("La règle ne peut pas être vide.")
        _regle = regle.Trim

        _etoileIndex = _regle.IndexOf(CHAR_ETOILE)
        _qMarkIndexes = obtenirLesIndexDeRemplacementDeCaracteres(_regle)

        If _etoileIndex <> _regle.LastIndexOf(CHAR_ETOILE) Then Throw New ArgumentException(String.Format("La règle ne peut contenir plus d'un '{0}'.", CHAR_ETOILE))
        If _regle = CHAR_ETOILE Then Throw New ArgumentException(String.Format("La règle ne peut contenir seulement '{0}'.", CHAR_ETOILE))
        If regle.Length = _qMarkIndexes.Count Then Throw New ArgumentException(String.Format("La règle ne peut contenir seulement des '{0}'.", CHAR_REMPLACEMENT))

        _typeRegle = Types.Plain
        If _etoileIndex <> ABSENT AndAlso _qMarkIndexes.Count > 0 Then
            _typeRegle = Types.WildcardAndCaracter
        Else
            If _etoileIndex <> ABSENT Then _typeRegle = Types.Wildcard
            If _qMarkIndexes.Count > 0 Then _typeRegle = Types.Caracter
        End If
    End Sub

    Public ReadOnly Property Type As Types
        Get
            Return _typeRegle
        End Get
    End Property

    Public Function Correspond(valeur As String) As Boolean Implements IFiltreConformite.Correspond
        If String.IsNullOrEmpty(valeur) Then Return False

        Select Case _typeRegle
            Case Types.Plain
                Return _regle.Equals(valeur, StringComparison.InvariantCultureIgnoreCase)

            Case Types.Caracter
                If _regle.Length <> valeur.Length Then Return False
                Return estEquivalentJusqua(valeur.Length, valeur)

            Case Types.Wildcard, Types.WildcardAndCaracter
                Dim prefixValid As Boolean = estEquivalentJusqua(_etoileIndex, valeur)
                Dim suffixValid As Boolean = estEquivalentDeLafin(_regle.Length - _etoileIndex, valeur)
                Return prefixValid AndAlso suffixValid
        End Select

        Return False
    End Function

    Private Function obtenirLesIndexDeRemplacementDeCaracteres(regle As IEnumerable(Of Char)) As List(Of Integer)
        Dim reponse As New List(Of Integer)()
        For index As Integer = 0 To regle.Count - 1
            If regle(index) = CHAR_REMPLACEMENT Then reponse.Add(index)
        Next
        Return reponse
    End Function

    Private Function estEquivalentJusqua(length As Integer, valeur As String) As Boolean
        If length > valeur.Length Then Return False

        For index As Integer = 0 To length - 1
            If _qMarkIndexes.Contains(index) Then Continue For
            If Not String.Equals(_regle(index), valeur(index), StringComparison.InvariantCultureIgnoreCase) Then Return False
        Next
        Return True
    End Function

    Private Function estEquivalentDeLafin(length As Integer, valeur As String) As Boolean
        If length - 1 > valeur.Length Then Return False

        Dim v() As Char = valeur.Reverse().ToArray()
        Dim r() As Char = _regle.Reverse().ToArray()
        Dim q As List(Of Integer) = obtenirLesIndexDeRemplacementDeCaracteres(r)

        For index As Integer = 0 To length - 2
            If q.Contains(index) Then Continue For
            If Not String.Equals(r(index), v(index), StringComparison.InvariantCultureIgnoreCase) Then Return False
        Next
        Return True
    End Function

End Class