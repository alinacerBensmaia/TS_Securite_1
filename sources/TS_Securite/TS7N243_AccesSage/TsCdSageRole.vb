Imports Rrq.Securite.GestionAcces.TsCdConstanteNomChampNorm

Public Class TsCdSageRole
    ' Faire du ménage dans cette classe. Voir les commentaires dans TsCdConstanteNomChampNorm
    ' Ici on peut conserver des noms spécifiques à Sage car c'est "AccesSage"

    <TsAtNomChampGen(ROLE.NAME)> _
    Public Name As String
    <TsAtNomChampGen(ROLE.DESCRIPTION)> _
    Public Description As String
    <TsAtNomChampGen(ROLE.ORGANIZATION)> _
    Public Organization As String
    <TsAtNomChampGen(ROLE.OWNER)> _
    Public Owner As String
    <TsAtNomChampGen(ROLE.CREATE_DATE)> _
    Public CreateDate As Date
    <TsAtNomChampGen(ROLE.APPROVE_CODE)> _
    Public ApprovalStatus As String
    <TsAtNomChampGen(ROLE.APPROVED_DATE)> _
    Public ApproveDate As Date
    <TsAtNomChampGen(ROLE.EXPIRATION_DATE)> _
    Public ExpirationDate As Date
    <TsAtNomChampGen(ROLE.ORGANIZATION2)> _
    Public Organization2 As String
    <TsAtNomChampGen(ROLE.ORGANIZATION3)> _
    Public Organization3 As String
    <TsAtNomChampGen(ROLE.TYPE)> _
    Public Type As String
    <TsAtNomChampGen(ROLE.REVIEWER)> _
    Public Reviewer As String
    <TsAtNomChampGen(ROLE.RULE)> _
    Public Rule As String

    Public Overrides Function ToString() As String
        Return Name
    End Function
End Class
