
Imports System.Collections.Generic

Friend Class Proprietes
    Inherits List(Of Propriete)

    Public Sub New()
        Add(New Propriete("sAMAccountName", GetType(String)))
        Add(New Propriete("Sn", GetType(String)))
        Add(New Propriete("GivenName", GetType(String)))
        Add(New Propriete("DisplayName", GetType(String)))
        Add(New Propriete("Mail", GetType(String)))
        Add(New Propriete("Department", GetType(String)))
        Add(New Propriete("Title", GetType(String)))
        Add(New Propriete("MemberOf", GetType(String), True))
        Add(New Propriete("ObjectSid", GetType(String), True))
        Add(New Propriete("company", GetType(String)))
        Add(New Propriete("description", GetType(String)))
        Add(New Propriete("employeeNumber", GetType(String)))
        Add(New Propriete("initials", GetType(String)))
        Add(New Propriete("personalTitle", GetType(String)))
        Add(New Propriete("EXTENSIONATTRIBUTE12", GetType(String)))
        Add(New Propriete("EXTENSIONATTRIBUTE13", "estCompteAdmin", GetType(Boolean)))
        Add(New Propriete("userAccountControl", GetType(Integer)))
        Add(New Propriete("objectClass", GetType(String)))
        Add(New Propriete("telephoneNumber", GetType(String)))
    End Sub

End Class

Friend Class Propriete
    Public ReadOnly Property NomAd As String
    Public ReadOnly Property NomSql As String
    Public ReadOnly Property TypeSql As Type
    Public ReadOnly Property Exceptionnel As Boolean

    Friend Sub New(nom As String, typeSql As Type)
        Me.New(nom, nom, typeSql, False)
    End Sub
    Friend Sub New(nom As String, typeSql As Type, exceptionnel As Boolean)
        Me.New(nom, nom, typeSql, exceptionnel)
    End Sub
    Friend Sub New(nomAd As String, nomSql As String, typeSql As Type)
        Me.New(nomAd, nomSql, typeSql, False)
    End Sub
    Friend Sub New(nomAd As String, nomSql As String, typeSql As Type, exceptionnel As Boolean)
        _NomAd = nomAd
        _NomSql = nomSql
        _TypeSql = typeSql
        _Exceptionnel = exceptionnel
    End Sub

    Public Function EstUserAccountControl() As Boolean
        Return estPropriete("userAccountControl")
    End Function

    Public Function EstEstCompteAdmin() As Boolean
        Return estPropriete("EXTENSIONATTRIBUTE13")
    End Function

    Public Function EstMemberOf() As Boolean
        Return estPropriete("MemberOf")
    End Function

    Public Function EstObjectSid() As Boolean
        Return estPropriete("ObjectSid")
    End Function

    Private Function estPropriete(valeur As String) As Boolean
        Return String.Equals(NomAd, valeur, StringComparison.InvariantCultureIgnoreCase)
    End Function
End Class