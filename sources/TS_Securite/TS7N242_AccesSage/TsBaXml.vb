Imports System.Xml.Serialization
Imports System.Xml
Imports System.Text
Imports System.IO

' Fonctions communes pour la sérialisation et le formatage Xml
Friend Module TsBaXml

    Public Function Deserialize(Of T)(ByVal s As String) As T
        Dim serial As New XmlSerializer(GetType(T))
        Return DirectCast(serial.Deserialize(New StringReader(s)), T)
    End Function

    Public Function Serialize(Of T)(ByVal o As T) As String
        Dim serial As New XmlSerializer(GetType(T))
        Dim sb As New StringBuilder
        serial.Serialize(New StringWriter(sb), o)
        Return sb.ToString()
    End Function

    Public Function Formater(ByVal xml As String) As String
        Dim doc As New XmlDocument()
        doc.LoadXml(xml)
        Dim sb As New StringBuilder
        Dim w As New XmlTextWriter(New StringWriter(sb))
        w.Formatting = Formatting.Indented
        doc.WriteTo(w)
        Return sb.ToString()
    End Function

End Module
