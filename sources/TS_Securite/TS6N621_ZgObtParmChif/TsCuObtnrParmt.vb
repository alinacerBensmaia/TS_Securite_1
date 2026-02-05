Imports System.IO
Imports System.Security.Cryptography
Imports Rrq.InfrastructureCommune.Parametres
Imports Rrq.InfrastructureCommune.UtilitairesCommuns

Public MustInherit Class TsCuObtnrParmt
    Private Cle() As Byte = {47, 139, 1, 46, 248, 163, 248, 134, 48, 93, 99, 57, 241, 160, 148, 141, 17, 186, 151, 23, 58, 110, 141, 152, 249, 253, 173, 95, 90, 24, 106, 161}
    Private Vecteur() As Byte = {38, 55, 72, 244, 225, 125, 22, 45, 231, 213, 251, 49, 129, 90, 241, 178}

    Private blnChiffrementDechiffrementPermis As Boolean

    Public Sub New()
        XuCuChargementAssembly.CreerHandlerAssemblyResolve()
    End Sub

    Public Enum objTypeCle
        Interne = 0
        SQAG = 1
        Externe = 2
    End Enum

    <System.ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never)>
    Public Property ChiffrementDechiffrementPermis() As Boolean
        Get
            Return blnChiffrementDechiffrementPermis
        End Get
        Set(Value As Boolean)
            blnChiffrementDechiffrementPermis = Value
        End Set
    End Property

    ''--------------------------------------------------------------------------------
    '' Class.Method:	tsCuObtParmChif.ChiffrerFichier
    '' <summary>
    '' 
    '' </summary>
    '' <param name="strNomFichier">
    '' 	Variable de type string indiquant le nom du fichier à chiffrer.  On doit mettre le chemin complet et non seulement le nom du fichier. 
    '' 	Value Type: <see cref="String" />	(System.String)
    '' </param>
    '' 	Cette exception est lancée si...
    '' </exception>
    '' <returns><see cref="Security.Cryptography.CryptoStream" />	(System.Security.Cryptography.CryptoStream)</returns>
    '' <remarks><para><pre>
    '' Historique des modifications: 
    '' 
    '' --------------------------------------------------------------------------------
    '' Date		Nom			Description
    '' 
    ' --------------------------------------------------------------------------------
    '' 2005-02-22	t209376		Création initiale
    '' 
    ' </pre></para>
    '' </remarks>
    '' --------------------------------------------------------------------------------
    Public Sub ChiffrerFichier(strNomFichier As String, contenuFichier As DataSet)
        'Chiffrer les fichier avec la nouvelle version et avec l'ancienne version 

        'Nouvelle version
        Dim nouvelleSolution As New TsCuObtnrParmtV2()
        nouvelleSolution.ChiffrementDechiffrementPermis = blnChiffrementDechiffrementPermis

        nouvelleSolution.ChiffrerFichier(strNomFichier, contenuFichier.GetType.Name, TS6N628_DtParmChif.TsCuDataSetSerializer.Serialize(contenuFichier))

        'Ancienne version
        Dim objChiffreur As FileStream = New FileStream(strNomFichier, FileMode.Create)
        Dim objRijndael As New RijndaelManaged

        If blnChiffrementDechiffrementPermis Then
            Dim objChiffre As ICryptoTransform = objRijndael.CreateEncryptor(Cle, Vecteur)
            Dim objChifStrm As New CryptoStream(objChiffreur, objChiffre, CryptoStreamMode.Write)
            contenuFichier.WriteXml(objChifStrm)
            objChifStrm.Close()
        End If

    End Sub

    '' --------------------------------------------------------------------------------
    '' Class.Method:	tsCuObtParmChif.DechiffrerFichier
    '' <summary>
    '' 
    '' </summary>
    '' <param name="strNomFichier">
    '' 	Variable de type string indiquant le nom du fichier à déchiffrer.  On doit mettre le chemin complet et non seulement le nom du fichier. 
    '' 	Value Type: <see cref="String" />	(System.String)
    '' </param>
    '' 
    ' 	Cette exception est lancée si...
    '' </exception>
    '' <returns><see cref="Security.Cryptography.CryptoStream" />	(System.Security.Cryptography.CryptoStream)</returns>
    '' <remarks><para><pre>
    ' Historique des modifications: 
    '' 
    '' --------------------------------------------------------------------------------
    '' Date		Nom			Description
    '' 
    '' --------------------------------------------------------------------------------
    '' 2005-02-22	t209376		Création initiale
    '' 
    '' </pre></para>
    '' </remarks>
    '' --------------------------------------------------------------------------------


    Public Function DechiffrerFichier(strNomFichier As String, nomTypeDataset As String) As String

        Dim UtiliserNouvelleSolution As Boolean = XuCuPolitiqueConfig.ConfigDomaine.ObtenirValeurSystemeOptionnelle("TS6", "TS6\TS6N621\UtiliserNouvelleSolution", String.Empty) = "O"

        If UtiliserNouvelleSolution Then
            Dim nouvelleSolution As New TsCuObtnrParmtV2()
            nouvelleSolution.ChiffrementDechiffrementPermis = blnChiffrementDechiffrementPermis
            Return nouvelleSolution.DechiffrerFichier(strNomFichier, nomTypeDataset)
        Else

            Dim dsContenu As DataSet = New TS6N628_DtParmChif.TsDsObtnrParmtCertf

            Select Case nomTypeDataset
                Case GetType(TS6N628_DtParmChif.TsDsObtnrParmtSel).Name
                    dsContenu = New TS6N628_DtParmChif.TsDsObtnrParmtSel
                Case GetType(TS6N628_DtParmChif.tsDsObtParmChif).Name
                    dsContenu = New TS6N628_DtParmChif.tsDsObtParmChif
            End Select

            Dim objDechiffreur As FileStream = New FileStream(strNomFichier, FileMode.Open, FileAccess.Read, FileShare.Read)
            Dim objRijndael As New RijndaelManaged

            If blnChiffrementDechiffrementPermis Then
                Dim objDechiffre As ICryptoTransform = objRijndael.CreateDecryptor(Cle, Vecteur)
                Dim objDechifStrm As New CryptoStream(objDechiffreur, objDechiffre, CryptoStreamMode.Read)
                dsContenu.ReadXml(objDechifStrm)
                dsContenu.AcceptChanges()
                objDechifStrm.Close()
                Return TS6N628_DtParmChif.TsCuDataSetSerializer.Serialize(dsContenu)
            End If

        End If
        Return Nothing

    End Function



    '' --------------------------------------------------------------------------------
    '' Class.Method:	tsCuObtParmChif.StringToByteArray
    '' <summary>
    '' 
    '' </summary>
    '' <param name="tabChaine">
    '' 	Tableau de string pour qu'ils soit convertis en tableau de byte. 
    ' 	Value Type: <see cref="String" />	(System.String)
    '' </param>
    '' 
    '' 	Cette exception est lancée si...
    ' </exception>
    '' <returns><see cref="Byte" />	(System.Byte)</returns>
    '' <remarks><para><pre>
    '' Historique des modifications: 
    ' 
    '' --------------------------------------------------------------------------------
    ' Date		Nom			Description
    '' 
    '' --------------------------------------------------------------------------------
    '' 2005-02-22	t209376		Création initiale
    '' 
    '' </pre></para>
    '' </remarks>
    '' --------------------------------------------------------------------------------
    Protected Function StringToByteArray(tabChaine As String()) As Byte()
        Dim bytBuffer(tabChaine.Length - 1) As Byte

        For intCtr As Integer = 0 To tabChaine.Length - 1
            bytBuffer(intCtr) = Convert.ToByte(tabChaine(intCtr))
        Next

        Return bytBuffer
    End Function
End Class
