Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Text

Public Class TsCuDataSetSerializer
    ' Une méthode pour sérialiser un objet DataSet en une chaîne Base64
    'Public Shared Function Serialize(dataSet As DataSet) As String
    '    ' Vérifier si l'objet DataSet est valide
    '    If dataSet Is Nothing Then
    '        Throw New ArgumentNullException("dataSet")
    '    End If

    '    ' Créer un MemoryStream pour stocker les octets de l'objet DataSet
    '    Using stream As New MemoryStream()
    '        ' Créer un BinaryFormatter pour sérialiser l'objet DataSet en octets
    '        Dim formatter As New BinaryFormatter()
    '        ' Sérialiser l'objet DataSet dans le MemoryStream
    '        formatter.Serialize(stream, dataSet)
    '        ' Obtenir le tableau d'octets du MemoryStream
    '        Dim bytes As Byte() = stream.ToArray()
    '        ' Convertir le tableau d'octets en une chaîne Base64
    '        Dim base64 As String = Convert.ToBase64String(bytes)
    '        ' Retourner la chaîne Base64
    '        Return base64
    '    End Using
    'End Function

    '' Une méthode pour désérialiser une chaîne Base64 en un objet DataSet
    'Public Shared Function Deserialize(base64 As String) As DataSet
    '    ' Vérifier si la chaîne Base64 est valide
    '    If base64 Is Nothing OrElse base64 = "" Then
    '        Throw New ArgumentNullException("base64")
    '    End If

    '    ' Convertir la chaîne Base64 en un tableau d'octets
    '    Dim bytes As Byte() = Convert.FromBase64String(base64)
    '    ' Créer un MemoryStream à partir du tableau d'octets
    '    Using stream As New MemoryStream(bytes)
    '        ' Créer un BinaryFormatter pour désérialiser le tableau d'octets en un objet DataSet
    '        Dim formatter As New BinaryFormatter()
    '        ' Désérialiser le MemoryStream en un objet DataSet
    '        Dim dataSet As DataSet = CType(formatter.Deserialize(stream), DataSet)
    '        ' Retourner l'objet DataSet
    '        Return dataSet
    '    End Using
    'End Function



    Public Shared Function Serialize(value As DataSet) As String
        Return value.GetXml()
    End Function

    Public Shared Function Deserialize(Of T As DataSet)(value As String) As T
        Dim ds As DataSet = CType(Activator.CreateInstance(GetType(T)), T)
        ds.ReadXml(New StringReader(value))
        Return CType(ds, T)
    End Function


End Class
