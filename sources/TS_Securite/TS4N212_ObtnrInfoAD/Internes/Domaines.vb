Imports System.Collections.Generic

Friend Class Domaines
    Public Shared Invalide As New Domaines()
    Public Shared Rq As New Domaines(TsIadNomDomaine.TsDomaineRQ)

    Public Shared ReadOnly All As New List(Of Domaines)({Rq})

    Public Shared Function Parse(valeur As String) As Domaines
        For Each item As Domaines In All
            If item.DomaineNT = valeur Then Return item
        Next
        Return Invalide
    End Function

    Public Shared Function Parse(valeur As TsIadNomDomaine) As Domaines
        For Each item As Domaines In All
            If item.EnumValue = valeur Then Return item
        Next
        Return Invalide
    End Function


#Region " Instance content "

    Public ReadOnly Property EnumValue As TsIadNomDomaine
    Public ReadOnly Property DomaineNT As String = String.Empty
    Public ReadOnly Property ServerActiveDirectory As String = String.Empty

    Private Sub New()
    End Sub

    Private Sub New(enumValue As TsIadNomDomaine)
        Me.EnumValue = enumValue

        Dim accesseur As TsIObtnrInfoAD = New TsCuObtnrInfoADRQ()
        DomaineNT = accesseur.DomaineNT
        ServerActiveDirectory = accesseur.ServeurActiveDirectory
    End Sub


#End Region

End Class
