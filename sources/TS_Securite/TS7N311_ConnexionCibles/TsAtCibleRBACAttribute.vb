''' <summary>
''' Indique si le système cible visée par le connecteur supporte la notion de rôles.
''' </summary>
''' <remarks>
''' Si un connecteur ne supporte pas la notion de rôle, l'appelant doit faire le travail pour aller
''' chercher toutes les associations directes et indirectes de l'utilisateurs aux ressources.
''' </remarks>
<AttributeUsage(AttributeTargets.Class, AllowMultiple:=False, Inherited:=True)> _
Public Class TsAtCibleRBACAttribute
    Inherits Attribute

    Public Shared [Default] As New TsAtCibleRBACAttribute(True)
    Public Shared CibleNonRBAC As New TsAtCibleRBACAttribute(False)

    Private _cibleRBAC As Boolean

    Public Sub New(ByVal cibleRBAC As Boolean)
        _cibleRBAC = cibleRBAC
    End Sub

    Public ReadOnly Property CibleRBAC() As Boolean
        Get
            Return _cibleRBAC
        End Get
    End Property

    Public Overrides Function IsDefaultAttribute() As Boolean
        Return Me.Equals(TsAtCibleRBACAttribute.Default)
    End Function

    ' Pas besoin d'overrider Equals, Attribute fait la bonne chose par réflexion

End Class
