''' <summary>
''' Classe Attribut.
''' Définit les nom des champs sur un standard normalisé.
''' </summary>
''' <remarks>
''' !!!IMPORTANT!!!
''' Cette Classe devra être placé dans un autre module plus global,
''' au cas où il y aurait d'autre sources de différences.
''' </remarks>
<AttributeUsage(AttributeTargets.Field, AllowMultiple:=False, Inherited:=True)> _
Public Class TsAtNomChampGen
    Inherits Attribute

    Public Shared [Default] As New TsAtNomChampGen("")
    Public Shared MethodeIgnorable As New TsAtNomChampGen("")

    Private _nomChamp As String

    Public Sub New(ByVal nomChamp As String)
        _nomChamp = nomChamp
    End Sub

    Public ReadOnly Property NomChamp() As String
        Get
            Return _nomChamp
        End Get
    End Property

    Public Overrides Function IsDefaultAttribute() As Boolean
        Return Me.Equals(TsAtNomChampGen.Default)
    End Function
End Class
