Imports System.Collections.Generic
Imports System.Security.Principal

Namespace AccesSecurite

    Friend Interface IDepotSecurite

        Function EstMembreROI(nomGroupe As String, userToken As WindowsIdentity) As Boolean

        Function ObtenirTousLesProfils() As IList(Of String)

        Function GroupesExistent(nomGroupes() As String) As Boolean

    End Interface

End Namespace