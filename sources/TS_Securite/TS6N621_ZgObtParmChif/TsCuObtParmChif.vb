Imports System.Collections.Generic
Imports System.IO
Imports System.Text.RegularExpressions
Imports Rrq.InfrastructureCommune.Parametres

''' --------------------------------------------------------------------------------
''' Project:	TS6N621_ZgObtParmChif
''' Class:	tsCuObtParmChif
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
''' 2005-04-06	t209376		Création initiale
''' 2005-07-15	t209428		Ajustement pour le SQAG
''' 
''' </pre></para>
''' </remarks>
''' --------------------------------------------------------------------------------
Public Class tsCuObtParmChif
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
    ' 	Variable de type string dans laquelle est retournée le code de chiffrement 
    ''     sélectionné.  Cette valeur est nécessaire lors de la demande d'obtention du 
    ''     code pour le déchiffrement. 
    ' 	Reference Type: <see cref="String" />	(System.String)
    '' </param>
    '' <param name="bytCle">
    ' 	Variable de type tableau de bytes dans laquelle est retournée la clé de 
    ''     chiffrement sélectionnée. 
    '' 	Reference Type: <see cref="Byte" />	(System.Byte)
    ' </param>
    '' <param name="bytVecteur">
    '' 	Variable de type tableau de bytes dans laquelle est retournée le vecteur 
    '     de chiffrement sélectionné. 
    '' 	Reference Type: <see cref="Byte" />	(System.Byte)
    ' </param>
    '' 
    ' 	Cette exception est lancée si...
    '' </exception>
    '' <returns><see cref="Boolean" />	(System.Boolean)</returns>
    ' <remarks><para><pre>
    '' Historique des modifications: 
    '' 
    '' --------------------------------------------------------------------------------
    ' Date		Nom			Description
    '' 
    ' --------------------------------------------------------------------------------
    '' 2005-02-21	t209376		Création initiale
    '' 
    '' </pre></para>
    '' </remarks>
    '' --------------------------------------------------------------------------------
    Public Function ObtenirParamChiffrement(objType As objTypeCle, ByRef strCode As String,
                                            ByRef bytCle As Byte(), ByRef bytVecteur As Byte()) As Boolean
        Try
            Dim objDsCleVecteurLoc As New TS6N628_DtParmChif.tsDsObtParmChif
            Dim tabLignes As DataRow()
            Dim objRnd As New Random(Environment.TickCount)
            Dim intRnd As Integer
            Dim strCheminFichierChiffrement As String
            Dim strChaine As String
            Dim strType As String = String.Empty

            strCheminFichierChiffrement = XuCuConfiguration.ValeurSysteme("TS6", "TS6\TS6N621\CheminFichierChiffrement")

            Try
                ChiffrementDechiffrementPermis = True
                objDsCleVecteurLoc = TS6N628_DtParmChif.TsCuDataSetSerializer.Deserialize(Of TS6N628_DtParmChif.tsDsObtParmChif)(DechiffrerFichier(strCheminFichierChiffrement, GetType(TS6N628_DtParmChif.tsDsObtParmChif).Name))
                ChiffrementDechiffrementPermis = False
            Catch exFNF As FileNotFoundException
                Throw New Exception("Vérifier l'emplacement et la présence du fichier de clés/vecteurs.")
            End Try

            Select Case objType
                Case objTypeCle.Interne
                    strType = "Interne"
                Case objTypeCle.SQAG
                    strType = "SQAG"
                Case objTypeCle.Externe
                    strType = "Externe"
            End Select

            Dim liste As New List(Of DataRow)
            liste.AddRange(objDsCleVecteurLoc.CleVecteur.Select)

            If String.IsNullOrEmpty(strCode) Then
                ' Si on ne nous a pas fourni de code, on va chercher de façon aléatoire dans toutes les clés numériques ( génériques )
                tabLignes = liste.FindAll(Function(x) x.Item("Actif").ToString.ToUpper.Equals("TRUE") AndAlso
                                                      x.Item("Type").ToString.Equals(strType) AndAlso
                                                      Regex.IsMatch(x.Item("Code").ToString, "^[0-9]*$")).ToArray

            Else
                Dim code As String = strCode

                ' Si on nous a fourni un code, on recherche cette donnée en particulier
                tabLignes = liste.FindAll(Function(x) x.Item("Actif").ToString.ToUpper.Equals("TRUE") AndAlso
                                                      x.Item("Type").ToString.Equals(strType) AndAlso
                                                      String.Equals(x.Item("Code").ToString, code, StringComparison.CurrentCultureIgnoreCase)).ToArray
            End If


            If tabLignes.Length > 0 Then
                intRnd = objRnd.Next(0, tabLignes.Length)

                strCode = tabLignes(intRnd).Item("Code").ToString
                strChaine = tabLignes(intRnd).Item("Cle").ToString
                bytCle = StringToByteArray(strChaine.Split(" "c))
                strChaine = tabLignes(intRnd).Item("Vecteur").ToString
                bytVecteur = StringToByteArray(strChaine.Split(" "c))
                Return True
            Else
                ObtenirParamChiffrement = False
                Throw New Exception("Il n'y a aucun code de disponible pour le chiffrement.  " &
                                    "Vérifier également l'emplacement et la présence du fichier de clés/vecteurs.")
            End If
        Catch ex As Exception
            ObtenirParamChiffrement = False
            Throw
        End Try
    End Function

    '' --------------------------------------------------------------------------------
    '' Class.Method:	tsCuObtParmChif.ObtenirParamDechiffrement
    '' <summary>
    '' 
    '' </summary>
    '' <param name="strCode">
    '' 	Variable de type string indiquant le code de déchiffrement désiré. 
    '' 	Value Type: <see cref="Int32" />	(System.Int32)
    '' </param>
    ' <param name="bytCle">
    '' 	Variable de type tableau de bytes dans laquelle est retournée la clé de déchiffrement désirée. 
    '' 	Reference Type: <see cref="Byte" />	(System.Byte)
    '' </param>
    '' <param name="bytVecteur">
    '' 	Variable de type tableau de bytes dans laquelle est retournée le vecteur de déchiffrement désiré. 
    '' 	Reference Type: <see cref="Byte" />	(System.Byte)
    '' </param>
    '' 
    ' 	Cette exception est lancée si...
    '' </exception>
    '' <returns><see cref="Boolean" />	(System.Boolean)</returns>
    '' <remarks><para><pre>
    ' Historique des modifications: 
    '' 
    '' --------------------------------------------------------------------------------
    '' Date		Nom			Description
    '' 
    '' --------------------------------------------------------------------------------
    '' 2005-02-21	t209376		Création initiale
    '' 
    '' </pre></para>
    '' </remarks>
    '' --------------------------------------------------------------------------------
    Public Function ObtenirParamDechiffrement(objType As objTypeCle, strCode As String,
                                              ByRef bytCle As Byte(), ByRef bytVecteur As Byte()) As Boolean
        Try
            Dim objDsCleVecteurLoc As New TS6N628_DtParmChif.tsDsObtParmChif
            Dim tabLignes As DataRow()
            Dim strCheminFichierChiffrement As String
            Dim strChaine As String
            Dim strType As String = String.Empty

            strCheminFichierChiffrement = XuCuConfiguration.ValeurSysteme("TS6", "TS6\TS6N621\CheminFichierChiffrement")

            Try
                ChiffrementDechiffrementPermis = True
                objDsCleVecteurLoc = TS6N628_DtParmChif.TsCuDataSetSerializer.Deserialize(Of TS6N628_DtParmChif.tsDsObtParmChif)(DechiffrerFichier(strCheminFichierChiffrement, GetType(TS6N628_DtParmChif.tsDsObtParmChif).Name))
                ChiffrementDechiffrementPermis = False
            Catch exFNF As FileNotFoundException
                Throw New Exception("Vérifier l'emplacement et la présence du fichier de clés/vecteurs.")
            End Try

            Select Case objType
                Case objTypeCle.Interne
                    strType = "Interne"
                Case objTypeCle.SQAG
                    strType = "SQAG"
                Case objTypeCle.Externe
                    strType = "Externe"
            End Select

            tabLignes = objDsCleVecteurLoc.CleVecteur.Select("Code='" & strCode &
                                                             "' and Type='" & strType & "'")

            If tabLignes.Length > 0 Then
                strChaine = tabLignes(0).Item("Cle").ToString
                bytCle = StringToByteArray(strChaine.Split(" "c))
                strChaine = tabLignes(0).Item("Vecteur").ToString
                bytVecteur = StringToByteArray(strChaine.Split(" "c))
                ObtenirParamDechiffrement = True
            Else
                Throw New Exception("Il n'y a aucun code de disponible pour le chiffrement.  " &
                                    "Vérifier également l'emplacement et la présence du fichier de clés/vecteurs.")
                ObtenirParamDechiffrement = False
            End If
        Catch ex As Exception
            ObtenirParamDechiffrement = False
            Throw
        End Try
    End Function

End Class