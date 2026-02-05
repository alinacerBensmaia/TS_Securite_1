Public Class TsCuFichierPieceJointe
    Public Property NomFichier As String
    Public Property NouveauNomFichier As String

    Public Property CheminDepot As String
    Public Property Contenu As Byte()

    Public Sub New()

    End Sub

    Public Sub New(pNomFichier As String, pContenu As Byte())
        NomFichier = pNomFichier
        Contenu = pContenu
    End Sub

    Public ReadOnly Property Extension As String
        Get
            If String.IsNullOrEmpty(NomFichier) Then Return String.Empty

            Dim justeFichier As String = If(NomFichier.Contains("\"), NomFichier.Split("\"c).Last, NomFichier)
            If Not justeFichier.Contains(".") Then Return String.Empty

            Dim splits As String() = justeFichier.Split("."c)
            Return String.Concat(".", splits.Last)
        End Get
    End Property

    Public Sub ObtenirFichier(pNomFichier As String, pContenu As Byte())
        If pNomFichier = NomFichier Then Exit Sub

        If String.IsNullOrEmpty(pNomFichier) Then
            NomFichier = String.Empty
            Contenu = Nothing
        Else
            NomFichier = pNomFichier
            Contenu = pContenu
        End If
    End Sub

    'Public Sub DefinirNouveauNomFichier(pCheminDepot As String)
    '    CheminDepot = pCheminDepot

    '    Dim now As Date = Date.Now
    '    Dim timestamp As String = TsCuCommuns.ObtenirTimeStamp()
    '    NouveauNomFichier = String.Concat(CheminDepot, "\PJ_", timestamp, Extension)
    'End Sub

    Public Sub DefinirNouveauNomFichier(pCheminDepot As String, GUID As String)
        CheminDepot = pCheminDepot

        Dim now As Date = Date.Now
        Dim timestamp As String = TsCuCommuns.ObtenirTimeStamp()
        NouveauNomFichier = String.Concat(CheminDepot, "\PJ_", GUID, timestamp, Extension)
    End Sub
End Class