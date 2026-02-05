''' <summary>
''' Cette attribut permet à une méthode d'un connecteur d'indiquer qu'elle n'a pas à être appelée.
''' </summary>
''' <remarks>
''' Normalement, une méthode doit être appelé pour faire son travail, mais il se peut que pour certains
''' connecteur il n'y ait jamais de travail à effectuer.
''' Il ne faut pas confondre une méthode qui n'a pas à être appelée avec une méthode pas implémentée.
''' Une méthode qui n'est pas implémentée devrait lancer une exception lorsqu'on l'appelle,
''' alors qu'une méthode qui n'a pas à être appelée doit se terminer normalement si elle est appelée malgré tout.
''' (Pour être gentille avec son appelant, elle devrait indiquer que les éléments sont tous à jour dans la cible).
''' </remarks>
<AttributeUsage(AttributeTargets.Method, AllowMultiple:=False, Inherited:=False)> _
Public Class TsAtMethodeIgnorableAttribute
    Inherits Attribute

    Public Shared [Default] As New TsAtMethodeIgnorableAttribute(False)
    Public Shared MethodeIgnorable As New TsAtMethodeIgnorableAttribute(True)

    Private _ignorable As Boolean

    Public Sub New(ByVal ignorable As Boolean)
        _ignorable = ignorable
    End Sub

    Public ReadOnly Property Ignorable() As Boolean
        Get
            Return _ignorable
        End Get
    End Property

    Public Overrides Function IsDefaultAttribute() As Boolean
        Return Me.Equals(TsAtMethodeIgnorableAttribute.Default)
    End Function

    ' Pas besoin d'overrider Equals, Attribute fait la bonne chose par réflexion

End Class
