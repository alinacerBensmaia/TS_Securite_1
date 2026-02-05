Imports System.IO
Imports Rrq.InfrastructureCommune.Parametres

''' --------------------------------------------------------------------------------
''' Project:	TS6N621_ZgObtParmChif
''' Class:	TsCuObtnrParmtCertf
''' <summary>
''' 
''' </summary>
''' <remarks><para><pre>
''' Historique des modifications: 
''' 
''' --------------------------------------------------------------------------------
''' Date		Nom			Description
''' 
''' --------------------------------------------------------------------------------
''' 2005-07-15	T209428		Création initiale
''' 
''' </pre></para>
''' </remarks>
''' --------------------------------------------------------------------------------
Public Class TsCuObtnrParmtCertf
    Inherits TsCuObtnrParmt

    '' --------------------------------------------------------------------------------
    '' Class.Method:	tsCuObtParmChif.ObtenirParamChiffrement
    '' <summary>
    '' 
    '' </summary>
    ' <param name="objType">
    '' 	Variable de type objTypeCle dans laquelle on reçoit le type de clé demandée.
    '' 	Reference Type: <see cref="objTypeCle" />	(objTypeCle)
    '' </param>
    '' <param name="strCode">
    ' 	Variable de type string dans laquelle est reçu et retournée le code de  
    ''     chiffrement sélectionné.  Cette valeur est nécessaire lors de la demande 
    ''     d'obtention du code pour le déchiffrement. 
    '' 	Reference Type: <see cref="String" />	(System.String)
    '' </param>
    '' <param name="strIdCertificat">
    ' 	Variable de type string laquelle est retournée le Id du certificat de 
    ''     chiffrement sélectionné. 
    '' 	Reference Type: <see cref="String" />	(System.String)
    ' </param>
    '' 
    '' 	Cette exception est lancée si...
    '' </exception>
    '' <returns><see cref="Boolean" />	(System.Boolean)</returns>
    '' <remarks><para><pre>
    ' Historique des modifications: 
    '' 
    '' --------------------------------------------------------------------------------
    '' Date		Nom			Description
    ' 
    '' --------------------------------------------------------------------------------
    ' 2005-07-15	T209428		Création initiale
    '' 
    '' </pre></para>
    '' </remarks>
    '' --------------------------------------------------------------------------------
    Public Function ObtenirParamCertficat(objType As objTypeCle, ByRef strCode As String,
                                          ByRef strIdCertificat As String, ByRef strNomMagasin As String) As Boolean
        Try
            Dim objDsCertfLoc As New TS6N628_DtParmChif.TsDsObtnrParmtCertf
            Dim tabLignes As DataRow()
            Dim objRnd As New Random(Environment.TickCount)
            Dim intRnd As Integer
            Dim strCheminFichierChiffrement As String
            Dim strType As String = String.Empty

            strCheminFichierChiffrement = XuCuConfiguration.ValeurSysteme("TS6", "TS6\TS6N621\CheminFichierCertificat")

            Try
                ChiffrementDechiffrementPermis = True
                objDsCertfLoc = TS6N628_DtParmChif.TsCuDataSetSerializer.Deserialize(Of TS6N628_DtParmChif.TsDsObtnrParmtCertf)(DechiffrerFichier(strCheminFichierChiffrement, GetType(TS6N628_DtParmChif.TsDsObtnrParmtCertf).Name))
                ChiffrementDechiffrementPermis = False
            Catch exFNF As FileNotFoundException
                Throw New Exception("Vérifier l'emplacement et la présence du fichier des ID de certificats.")
            End Try

            Select Case objType
                Case objTypeCle.Interne
                    strType = "Interne"
                Case objTypeCle.SQAG
                    strType = "SQAG"
                Case objTypeCle.Externe
                    strType = "Externe"
            End Select

            If strCode.Length > 0 Then
                ' Si on nous a fourni un code, on recherche cette donnée en particulier
                tabLignes = objDsCertfLoc.Certificat.Select("Actif=true and Type='" & strType &
                                                            "' and Code='" & strCode & "'")
            Else
                ' Si on ne nous a pas fourni un code, on recherche toutes les données actives 
                ' et pour un type de données
                tabLignes = objDsCertfLoc.Certificat.Select("Actif=true and Type='" & strType & "'")
            End If

            If tabLignes.Length > 0 Then
                intRnd = objRnd.Next(0, tabLignes.Length)

                strCode = tabLignes(intRnd).Item("Code").ToString
                strIdCertificat = tabLignes(intRnd).Item("IdCertificat").ToString
                strNomMagasin = tabLignes(intRnd).Item("NomMagasin").ToString
                Return True
            Else
                ObtenirParamCertficat = False
                Throw New Exception("Il n'y a aucun code de disponible pour le chiffrement.  " &
                                    "Vérifier également l'emplacement et la présence du fichier des ID de certificats.")
            End If
        Catch ex As Exception
            ObtenirParamCertficat = False
            Throw
        End Try
    End Function
End Class