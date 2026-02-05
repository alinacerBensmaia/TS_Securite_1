Imports System.Collections.Generic
Imports System.DirectoryServices
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Xml.Serialization

Friend Module Extensions

    <Extension>
    Public Function Deserialize(nomFichier As String) As TsCdUniteAdministrative
        Using fs As New FileStream(nomFichier, FileMode.Open)
            Dim xs As XmlSerializer = New XmlSerializer(GetType(TsCdUniteAdministrative))

            Dim retour As TsCdUniteAdministrative = DirectCast(xs.Deserialize(fs), TsCdUniteAdministrative)
            Return retour
        End Using
    End Function

    <Extension>
    Public Sub Serialize(source As TsCdUniteAdministrative, ByVal nomFichier As String)
        Using fs As New FileStream(nomFichier, FileMode.Create)
            Dim xs As XmlSerializer = New XmlSerializer(GetType(TsCdUniteAdministrative))
            xs.Serialize(fs, source)
        End Using
    End Sub


#Region " DirectorySearcher.PropertiesToLoad.Add() "

    <Extension>
    Private Function addPropertiesToLoad(source As DirectorySearcher, propertyName As String) As DirectorySearcher
        source.PropertiesToLoad.Add(propertyName)
        Return source
    End Function

    <Extension>
    Public Function ReturnDistinguishedName(source As DirectorySearcher) As DirectorySearcher
        Return source.addPropertiesToLoad("distinguishedName")
    End Function
    <Extension>
    Public Function ReturnSAMAccountName(source As DirectorySearcher) As DirectorySearcher
        Return source.addPropertiesToLoad("sAMAccountName")
    End Function
    <Extension>
    Public Function ReturnDisplayName(source As DirectorySearcher) As DirectorySearcher
        Return source.addPropertiesToLoad("DisplayName")
    End Function
    <Extension>
    Public Function ReturnMemberOf(source As DirectorySearcher) As DirectorySearcher
        Return source.addPropertiesToLoad("memberOf")
    End Function
    <Extension>
    Public Function ReturnMember(source As DirectorySearcher) As DirectorySearcher
        Return source.addPropertiesToLoad("member")
    End Function

#End Region

#Region " SearchResult.Properties()"

    <Extension>
    Private Function stringProperty(source As SearchResult, propertyName As String) As String
        Dim values As ResultPropertyValueCollection = source.Properties(propertyName)
        If values Is Nothing Then Return String.Empty

        Dim value As Object = values.Item(0)
        If value Is Nothing Then Return String.Empty

        Return value.ToString()
    End Function

    <Extension>
    Public Function sAMAccountName(source As SearchResult) As String
        Return source.stringProperty("sAMAccountName")
    End Function

    <Extension>
    Public Function DisplayName(source As SearchResult) As String
        Return source.stringProperty("DisplayName")
    End Function

    <Extension>
    Public Function distinguishedName(source As SearchResult) As String
        Return source.stringProperty("distinguishedName")
    End Function

    <Extension>
    Public Iterator Function memberOf(source As SearchResult) As IEnumerable(Of String)
        For Each m As Object In source.Properties("memberOf")
            Yield m.ToString()
        Next
    End Function

    <Extension>
    Public Iterator Function member(source As SearchResult) As IEnumerable(Of String)
        If source.Properties("member").Count > 0 Then
            For Each m As Object In source.Properties("member")
                Yield m.ToString()
            Next

        Else
            'si le nombre d'éléments retourné est trop grand, les valeurs sont retourné sur plusieurs propriétés...(ex. "member;range=0-1500", "member;range=1501-*")
            Dim premierRange = (From s In source.Properties.PropertyNames Where s.ToString().StartsWith("member;range=0", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault()
            If Not premierRange Is Nothing Then
                Using de As DirectoryEntry = source.GetDirectoryEntry()
                    Dim tousLesMembres As ArrayList = rangeExpansion(de, "member")

                    For Each m As Object In tousLesMembres
                        Yield m.ToString()
                    Next
                End Using
            End If
        End If
    End Function

    Private Function rangeExpansion(de As DirectoryEntry, nomPropriete As String) As ArrayList
        Dim retour As New ArrayList(5000)
        Dim idx As Integer = 0

        'index à base zero, donc -1
        Dim ecart As Integer = de.Properties(nomPropriete).Count - 1
        Dim gabarit As String = String.Concat(nomPropriete, ";range={0}-{1}")
        Dim nomProprieteAvecEcart As String = String.Format(gabarit, idx, ecart)

        Using ds As DirectorySearcher = New DirectorySearcher(de, String.Format("({0}=*)", nomPropriete), New String() {nomProprieteAvecEcart}, SearchScope.Base)
            Dim lastSearch As Boolean = False
            Dim sr As SearchResult = Nothing

            While (True)
                If Not lastSearch Then
                    ds.PropertiesToLoad.Clear()
                    ds.PropertiesToLoad.Add(nomProprieteAvecEcart)
                    sr = ds.FindOne()
                End If

                If Not sr Is Nothing Then
                    If sr.Properties.Contains(nomProprieteAvecEcart) Then
                        For Each dn As Object In sr.Properties(nomProprieteAvecEcart)
                            retour.Add(dn)
                            idx += 1
                        Next
                        '//our exit condition
                        If (lastSearch) Then Exit While

                        nomProprieteAvecEcart = String.Format(gabarit, idx, (idx + ecart))

                    Else
                        'si le dernier 'range' complet n'est pas présent, il en reste peut-être un plus petit
                        lastSearch = True
                        nomProprieteAvecEcart = String.Format(gabarit, idx, "*")
                    End If
                Else
                    Exit While
                End If
            End While
        End Using

        Return retour
    End Function

#End Region

End Module