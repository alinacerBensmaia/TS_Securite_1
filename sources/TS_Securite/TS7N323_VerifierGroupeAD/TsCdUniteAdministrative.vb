Imports System.Xml.Serialization
Imports System.Collections.Generic

''' <summary>
''' Classe permettant de stocker une unité administrative et ses membres
''' </summary>
<XmlRoot("UniteAdministrative")> Public Class TsCdUniteAdministrative
    Public Sub New()
        _ListeUtilisateurs = New List(Of TsCdUtilisateur)
    End Sub

    ''' <summary>
    ''' Le numéro de l'unité administrative
    ''' </summary>
    ''' <returns></returns>
    Public Property NumeroUniteAdministrative() As String

    ''' <summary>
    ''' Date de sauvegarde
    ''' </summary>
    ''' <returns></returns>
    Public Property DateDeSauvegarde() As DateTime

    ''' <summary>
    ''' Liste d'utilisateurs
    ''' </summary>
    ''' <returns></returns>
    <XmlArrayItem("Utilisateur", GetType(TsCdUtilisateur))> Public ReadOnly Property ListeUtilisateurs() As List(Of TsCdUtilisateur)

    ''' <summary>
    ''' Nombre d'utilisateur
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property NombreUtilisateurs() As Integer
        Get
            Return Me.ListeUtilisateurs.Count
        End Get
    End Property

    ''' <summary>
    ''' Nombre de groupe
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property NombreGroupes() As Integer
        Get
            Return ListeUtilisateurs.Sum(Function(x) x.ListeGroupes.Count)
        End Get
    End Property

End Class