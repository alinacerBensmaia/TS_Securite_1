Imports System.Xml.Serialization
Imports System.Collections.Generic

''' <summary>
''' Classe permettant de stocker un utilisateur et ses groupes
''' </summary>
<XmlRoot("Utilisateur")> Public Class TsCdUtilisateur

    Public Sub New()
        _ListeGroupes = New List(Of String)
    End Sub

    ''' <summary>
    ''' Code de l'utilisateur
    ''' </summary>
    ''' <returns></returns>
    <XmlAttribute()> Public Property CodeUtilisateur() As String

    ''' <summary>
    ''' Nom complet de l'utilisateur
    ''' </summary>
    ''' <returns></returns>
    <XmlAttribute()> Public Property NomComplet() As String

    ''' <summary>
    ''' Liste de groupes
    ''' </summary>
    ''' <returns></returns>
    <XmlArrayItem("Groupe", GetType(String))> Public ReadOnly Property ListeGroupes() As List(Of String)

End Class
