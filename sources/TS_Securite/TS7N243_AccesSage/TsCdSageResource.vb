Imports Rrq.Securite.GestionAcces.TsCdConstanteNomChampNorm

Public Class TsCdSageResource
    <TsAtNomChampGen(RESSOURCE.RESNAME_1)> _
    Public ResName1 As String
    <TsAtNomChampGen(RESSOURCE.RESNAME_2)> _
    Public ResName2 As String
    <TsAtNomChampGen(RESSOURCE.RESNAME_2)> _
    Public ResName3 As String
    <TsAtNomChampGen(RESSOURCE.DESCRIPTION)> _
    Public NomFonctionnelOuDescription As String
    <TsAtNomChampGen(RESSOURCE.DERN_MODIF)> _
    Public DerniereModification As String
    <TsAtNomChampGen(RESSOURCE.CN)> _
    Public CN As String
    <TsAtNomChampGen(RESSOURCE.DETAILS)> _
    Public Details As String
    <TsAtNomChampGen(RESSOURCE.DETENTEUR)> _
    Public Detenteur As String
    <TsAtNomChampGen(RESSOURCE.DATE_CREATION)> _
    Public DateCreation As Date

    Public Overrides Function ToString() As String
        Return ResName1
    End Function
End Class
