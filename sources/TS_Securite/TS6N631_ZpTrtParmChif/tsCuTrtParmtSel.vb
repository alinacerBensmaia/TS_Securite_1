Imports System.Security.Cryptography
Imports TS6N621_ZgObtParmChif

Public Class tsCuTrtParmtSel
    Inherits tsCuTrtParmt

    Private objDs As New TS6N628_DtParmChif.TsDsObtnrParmtSel
    Private objObtnrParmtSel As New TS6N621_ZgObtParmChif.TsCuObtnrParmtSel

    Protected Overrides ReadOnly Property DsParmt() As System.Data.DataSet
        Get
            Return objDs
        End Get
    End Property

    Protected Overrides ReadOnly Property DtParmt() As System.Data.DataTable
        Get
            Return CType(objDs, TS6N628_DtParmChif.TsDsObtnrParmtSel).Sel
        End Get
    End Property

    Protected Overrides ReadOnly Property objObtnrParmt() As TS6N621_ZgObtParmChif.TsCuObtnrParmt
        Get
            Return objObtnrParmtSel
        End Get
    End Property

    Public Overrides Function CopieCle(cleACopier As DataRow) As DataRow

        Dim dtParmtS As TS6N628_DtParmChif.TsDsObtnrParmtSel.SelDataTable = CType(DtParmt, TS6N628_DtParmChif.TsDsObtnrParmtSel.SelDataTable)
        Dim SelRow As TS6N628_DtParmChif.TsDsObtnrParmtSel.SelRow = dtParmtS.NewSelRow

        ' Initiliser les champs du record Sel
        SelRow.Code = cleACopier("Code").ToString()
        SelRow.Sel = cleACopier("Sel").ToString()
        SelRow.Actif = Boolean.Parse(cleACopier("Actif").ToString)

        Return SelRow

    End Function


    Public Function ObtenirNouveauSel() As DataRow
        Dim objDsParmtSel As New TS6N628_DtParmChif.TsDsObtnrParmtSel

        Dim dtParmtS As TS6N628_DtParmChif.TsDsObtnrParmtSel.SelDataTable = CType(DtParmt, TS6N628_DtParmChif.TsDsObtnrParmtSel.SelDataTable)
        Dim SelRow As TS6N628_DtParmChif.TsDsObtnrParmtSel.SelRow = dtParmtS.NewSelRow

        ' Initiliser les champs du record Sel
        SelRow.Code = ""
        SelRow.Sel = ""
        SelRow.Actif = True

        Return SelRow
    End Function

    Public Overrides Function Ajouter(type As TsCuParamsChiffrement.TypeCle, ByVal recordAAjouter As DataRow, nomFichierServeurExtranet As String, nomFichierServeurInterne As String, nomFichierServeurInforoute As String) As Boolean

        Dim oSelRow As TS6N628_DtParmChif.TsDsObtnrParmtSel.SelRow = CType(recordAAjouter, TS6N628_DtParmChif.TsDsObtnrParmtSel.SelRow)
        If oSelRow.Sel.Length = 0 Then
            Throw New TsCuExceptionErreurValdt("Le sel est obligatoire.")
        End If

        Return MyBase.Ajouter(type, recordAAjouter, nomFichierServeurExtranet, nomFichierServeurInterne, nomFichierServeurInforoute, False, True)
    End Function

    Public Overrides Sub ObtenirValeurs(ByVal objObtnrParmt As TS6N621_ZgObtParmChif.TsCuObtnrParmt, ByVal nomFichierServeurInterne As String)
        objDs = TS6N628_DtParmChif.TsCuDataSetSerializer.Deserialize(Of TS6N628_DtParmChif.TsDsObtnrParmtSel)(objObtnrParmt.DechiffrerFichier(nomFichierServeurInterne, GetType(TS6N628_DtParmChif.TsDsObtnrParmtSel).Name))
        objDs.AcceptChanges()
    End Sub
End Class
