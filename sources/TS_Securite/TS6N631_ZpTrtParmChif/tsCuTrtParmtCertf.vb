Imports System.Security.Cryptography
Imports TS6N621_ZgObtParmChif

Public Class tsCuTrtParmtCertf
    Inherits tsCuTrtParmt

    Private objDs As New TS6N628_DtParmChif.TsDsObtnrParmtCertf
    Private objObtnrParmtCertf As New TS6N621_ZgObtParmChif.TsCuObtnrParmtCertf

    Protected Overrides ReadOnly Property DsParmt() As System.Data.DataSet
        Get
            Return objDs
        End Get
    End Property

    Protected Overrides ReadOnly Property DtParmt() As System.Data.DataTable
        Get
            Return CType(objDs, TS6N628_DtParmChif.TsDsObtnrParmtCertf).Certificat
        End Get
    End Property

    Protected Overrides ReadOnly Property objObtnrParmt() As TS6N621_ZgObtParmChif.TsCuObtnrParmt
        Get
            Return objObtnrParmtCertf
        End Get
    End Property

    Public Overrides Function CopieCle(cleACopier As DataRow) As DataRow

        Dim dtParmtC As TS6N628_DtParmChif.TsDsObtnrParmtCertf.CertificatDataTable = CType(DtParmt, TS6N628_DtParmChif.TsDsObtnrParmtCertf.CertificatDataTable)
        Dim CertificatRow As TS6N628_DtParmChif.TsDsObtnrParmtCertf.CertificatRow = dtParmtC.NewCertificatRow

        ' Initiliser les champs du record Sel
        CertificatRow.Code = cleACopier("Code").ToString
        CertificatRow.IdCertificat = cleACopier("IdCertificat").ToString
        CertificatRow.NomMagasin = cleACopier("NomMagasin").ToString
        CertificatRow.Actif = Boolean.Parse(cleACopier("Actif").ToString)

        Return CertificatRow

    End Function

    Public Function ObtenirNouvlCertificat() As DataRow
        Dim objDsParmtCertf As New TS6N628_DtParmChif.TsDsObtnrParmtCertf

        Dim dtParmtC As TS6N628_DtParmChif.TsDsObtnrParmtCertf.CertificatDataTable = CType(DtParmt, TS6N628_DtParmChif.TsDsObtnrParmtCertf.CertificatDataTable)
        Dim CertificatRow As TS6N628_DtParmChif.TsDsObtnrParmtCertf.CertificatRow = dtParmtC.NewCertificatRow

        ' Initiliser les champs du record Sel
        CertificatRow.Code = ""
        CertificatRow.IdCertificat = ""
        CertificatRow.NomMagasin = ""
        CertificatRow.Actif = True

        Return CertificatRow
    End Function

    Public Overrides Function Ajouter(type As TsCuParamsChiffrement.TypeCle, ByVal recordAAjouter As DataRow, nomFichierServeurExtranet As String, nomFichierServeurInterne As String, nomFichierServeurInforoute As String) As Boolean

        Dim oCertificatRow As TS6N628_DtParmChif.TsDsObtnrParmtCertf.CertificatRow = CType(recordAAjouter, TS6N628_DtParmChif.TsDsObtnrParmtCertf.CertificatRow)
        If oCertificatRow.IdCertificat.Length = 0 OrElse oCertificatRow.NomMagasin.Length = 0 Then
            Throw New TsCuExceptionErreurValdt("L'id du certificat et le nom du magasin sont obligatoires.")
        End If

        Return MyBase.Ajouter(type, recordAAjouter, nomFichierServeurExtranet, nomFichierServeurInterne, nomFichierServeurInforoute, False, True)
    End Function

    Public Overrides Sub ObtenirValeurs(ByVal objObtnrParmt As TS6N621_ZgObtParmChif.TsCuObtnrParmt, ByVal nomFichierServeurInterne As String)
        objDs = TS6N628_DtParmChif.TsCuDataSetSerializer.Deserialize(Of TS6N628_DtParmChif.TsDsObtnrParmtCertf)(objObtnrParmt.DechiffrerFichier(nomFichierServeurInterne, GetType(TS6N628_DtParmChif.TsDsObtnrParmtCertf).Name))
        objDs.AcceptChanges()
    End Sub
End Class
