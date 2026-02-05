Imports System.Security.Cryptography
Imports TS6N621_ZgObtParmChif

Public Class tsCuTrtParmChif
    Inherits tsCuTrtParmt

    Private objDs As New TS6N628_DtParmChif.tsDsObtParmChif
    Private objObtnrParmtChiff As New TS6N621_ZgObtParmChif.tsCuObtParmChif

    Protected Overrides ReadOnly Property DsParmt() As System.Data.DataSet
        Get
            Return objDs
        End Get
    End Property

    Protected Overrides ReadOnly Property DtParmt() As System.Data.DataTable
        Get
            Return CType(objDs, TS6N628_DtParmChif.tsDsObtParmChif).CleVecteur
        End Get
    End Property

    Protected Overrides ReadOnly Property objObtnrParmt() As TsCuObtnrParmt
        Get
            Return objObtnrParmtChiff
        End Get
    End Property

    Public Overrides Function CopieCle(cleACopier As DataRow) As DataRow

        Dim dtParmtCV As TS6N628_DtParmChif.tsDsObtParmChif.CleVecteurDataTable = CType(DtParmt, TS6N628_DtParmChif.tsDsObtParmChif.CleVecteurDataTable)
        Dim CleVecteurRow As TS6N628_DtParmChif.tsDsObtParmChif.CleVecteurRow = dtParmtCV.NewCleVecteurRow

        CleVecteurRow.Code = cleACopier("Code").ToString
        CleVecteurRow.Cle = cleACopier("Cle").ToString
        CleVecteurRow.Vecteur = cleACopier("Vecteur").ToString
        CleVecteurRow.Actif = Boolean.Parse(cleACopier("Actif").ToString)

        Return CleVecteurRow

    End Function


    Public Function ObtenirNouvlCleVecteur() As DataRow
        Dim oRijndael As New RijndaelManaged
        Dim strKey As String = String.Empty
        Dim strIV As String = String.Empty
        Dim objDsParmtChiff As New TS6N628_DtParmChif.tsDsObtParmChif

        Dim dtParmtCV As TS6N628_DtParmChif.tsDsObtParmChif.CleVecteurDataTable = CType(DtParmt, TS6N628_DtParmChif.tsDsObtParmChif.CleVecteurDataTable)
        Dim CleVecteurRow As TS6N628_DtParmChif.tsDsObtParmChif.CleVecteurRow = dtParmtCV.NewCleVecteurRow

        ' Convertir la clé et le vecteur d'initialisation en string 
        ' (chaque byte est converti en string et séparé par un espace)
        For Each oItem As Byte In oRijndael.Key
            strKey &= oItem.ToString & " "
        Next
        For Each oItem As Byte In oRijndael.IV
            strIV &= oItem.ToString & " "
        Next

        ' Ajouter la clé et le vecteur dans le record à ajouter et initialiser les autres champs
        CleVecteurRow.Code = ""
        CleVecteurRow.Cle = Trim(strKey)
        CleVecteurRow.Vecteur = Trim(strIV)
        CleVecteurRow.Actif = True

        Return CleVecteurRow
    End Function

    Public Overrides Function Ajouter(type As TsCuParamsChiffrement.TypeCle, ByVal recordAAjouter As DataRow, nomFichierServeurExtranet As String, nomFichierServeurInterne As String, nomFichierServeurInforoute As String) As Boolean

        Dim oCleVecteurRow As TS6N628_DtParmChif.tsDsObtParmChif.CleVecteurRow = CType(recordAAjouter, TS6N628_DtParmChif.tsDsObtParmChif.CleVecteurRow)
        If oCleVecteurRow.Cle.Length = 0 OrElse oCleVecteurRow.Vecteur.Length = 0 Then
            Throw New TsCuExceptionErreurValdt("La clé de chiffrement et le vecteur d'initialisation sont obligatoires.")
        End If
        Return MyBase.Ajouter(type, recordAAjouter, nomFichierServeurExtranet, nomFichierServeurInterne, nomFichierServeurInforoute, True, True)
    End Function

    Public Overrides Sub ObtenirValeurs(ByVal objObtnrParmt As TS6N621_ZgObtParmChif.TsCuObtnrParmt, ByVal nomFichierServeurInterne As String)
        objDs = TS6N628_DtParmChif.TsCuDataSetSerializer.Deserialize(Of TS6N628_DtParmChif.tsDsObtParmChif)(objObtnrParmt.DechiffrerFichier(nomFichierServeurInterne, GetType(TS6N628_DtParmChif.tsDsObtParmChif).Name))
        objDs.AcceptChanges()
    End Sub
End Class
