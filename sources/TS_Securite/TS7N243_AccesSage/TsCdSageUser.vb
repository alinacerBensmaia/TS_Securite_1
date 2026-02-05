Imports Rrq.Securite.GestionAcces.TsCdConstanteNomChampNorm

Public Class TsCdSageUser
    <TsAtNomChampGen(USER.PERSON_ID)> _
    Public PersonID As String

    <TsAtNomChampGen(USER.NAME)> _
    Public UserName As String

    <TsAtNomChampGen(USER.ORGANIZATION)> _
    Public Organization As String

    <TsAtNomChampGen(USER.ORGANIZATION_TYPE)> _
    Public OrganizationType As String

    <TsAtNomChampGen(USER.VILLE)> _
    Public Ville As String

    <TsAtNomChampGen(USER.COURRIEL)> _
    Public Courriel As String

    <TsAtNomChampGen(USER.DATE_FIN)> _
    Public DateFin As Date

    <TsAtNomChampGen(USER.PRENOM)> _
    Public Prenom As String

    <TsAtNomChampGen(USER.NOM)> _
    Public Nom As String

    <TsAtNomChampGen(USER.DATE_APPROBATION)> _
    Public DateApprobation As Date

    <TsAtNomChampGen(USER.CN)> _
    Public CN As String

    <TsAtNomChampGen(USER.NOM_UNITE)>
    Public NomUnite As String

    <TsAtNomChampGen(USER.CHAMP_9)>
    Public Champ9 As String

    Public Overrides Function ToString() As String
        Return UserName
    End Function
End Class
