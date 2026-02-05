Imports System.Runtime.CompilerServices
Imports System.Security.Principal
Imports System.Text.RegularExpressions
Imports Rrq.InfrastructureCommune.Parametres
Imports Rrq.InfrastructureCommune.UtilitairesCommuns
Imports Rrq.Securite.ParametreChiffrement

Public Class TsCuObtnrParmtV2

    Private paramChiffr As TsIAccesseur
    Private blnChiffrementDechiffrementPermis As Boolean

    Private ressourceParamChiffr As Integer = ParseEnum(Of TsRessourceParamChiffr)(XuCuPolitiqueConfig.ConfigDomaine.ValeurSysteme("TS6", "TS6\TS6N621\RessourceParamChiffr"))
    Public Sub New()

        XuCuChargementAssembly.CreerHandlerAssemblyResolve()

        Dim accesseur As TsIAccesseur

        Select Case ressourceParamChiffr
            Case TsRessourceParamChiffr.FICHIER
                If EstAppelWCFouWEBAPI() Then
                    If EstPoolTS() Then
                        accesseur = CreerAccesseurFichier()
                    Else
                        accesseur = CreerAccesseurService()
                    End If
                Else
                    accesseur = CreerAccesseurFichier()
                End If


            Case TsRessourceParamChiffr.CIWCF
                If EstAppelWCFouWEBAPI() Then
                    accesseur = CreerAccesseurService()
                Else
                    accesseur = CreerAccesseurCIWCF()
                End If

            Case Else
                ' Garde-fou au cas où un autre accesseur soit ajouté ou que la propriété contienne du garbage 
                Throw New ApplicationException("La ressource de fichier de clé symbolique n'est pas valide")
        End Select

        paramChiffr = accesseur
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
    '' Class.Method:    tsCuObtParmChif.ChiffrerFichier
    '' <summary>
    '' 
    '' </summary>
    '' <param name="strNomFichier">
    ''  Variable de type string indiquant le nom du fichier à chiffrer.  On doit mettre le chemin complet et non seulement le nom du fichier. 
    ''  Value Type: <see cref="String" />       (System.String)
    '' </param>
    ''  Cette exception est lancée si...
    '' </exception>
    '' <returns><see cref="Security.Cryptography.CryptoStream" />       (System.Security.Cryptography.CryptoStream)</returns>
    '' <remarks><para><pre>
    '' Historique des modifications: 
    '' 
    '' --------------------------------------------------------------------------------
    '' Date             Nom                     Description
    '' 
    ' --------------------------------------------------------------------------------
    '' 2005-02-22       t209376         Création initiale
    '' 
    ' </pre></para>
    '' </remarks>
    '' --------------------------------------------------------------------------------
    Public Sub ChiffrerFichier(strNomFichier As String, strNomTypeDataset As String, strContenuFichier As String)

        strNomFichier = strNomFichier.Replace("\TS6\Chiffrement", XuCuPolitiqueConfig.ConfigDomaine.ValeurSysteme("TS6", "TS6\TS6N621\RepertoireNouvelleSolution"))

        paramChiffr.InscrireFichierChiffre(strNomFichier, strNomTypeDataset, strContenuFichier, blnChiffrementDechiffrementPermis)

    End Sub

    '' --------------------------------------------------------------------------------
    '' Class.Method:    tsCuObtParmChif.DechiffrerFichier
    '' <summary>
    '' 
    '' </summary>
    '' <param name="strNomFichier">
    ''  Variable de type string indiquant le nom du fichier à déchiffrer.  On doit mettre le chemin complet et non seulement le nom du fichier. 
    ''  Value Type: <see cref="String" />       (System.String)
    '' </param>
    '' 
    '   Cette exception est lancée si...
    '' </exception>
    '' <returns><see cref="Security.Cryptography.CryptoStream" />       (System.Security.Cryptography.CryptoStream)</returns>
    '' <remarks><para><pre>
    ' Historique des modifications: 
    '' 
    '' --------------------------------------------------------------------------------
    '' Date             Nom                     Description
    '' 
    '' --------------------------------------------------------------------------------
    '' 2005-02-22       t209376         Création initiale
    '' 
    '' </pre></para>
    '' </remarks>
    '' --------------------------------------------------------------------------------
    Public Function DechiffrerFichier(strNomFichier As String, NomTypeDataSet As String) As String

        strNomFichier = strNomFichier.Replace("\TS6\Chiffrement", XuCuPolitiqueConfig.ConfigDomaine.ValeurSysteme("TS6", "TS6\TS6N621\RepertoireNouvelleSolution"))
        Return paramChiffr.ObtenirFichierChiffre(strNomFichier, NomTypeDataSet, blnChiffrementDechiffrementPermis)

    End Function

    '' --------------------------------------------------------------------------------
    '' Class.Method:    tsCuObtParmChif.StringToByteArray
    '' <summary>
    '' 
    '' </summary>
    '' <param name="tabChaine">
    ''  Tableau de string pour qu'ils soit convertis en tableau de byte. 
    '   Value Type: <see cref="String" />       (System.String)
    '' </param>
    '' 
    ''  Cette exception est lancée si...
    ' </exception>
    '' <returns><see cref="Byte" />     (System.Byte)</returns>
    '' <remarks><para><pre>
    '' Historique des modifications: 
    ' 
    '' --------------------------------------------------------------------------------
    ' Date              Nom                     Description
    '' 
    '' --------------------------------------------------------------------------------
    '' 2005-02-22       t209376         Création initiale
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

    ' Idéalement, on déclarerait T As [Enum], mais "'Enum' ne peut pas être utilisé en tant que contrainte de type."
    Private Function ParseEnum(Of T)(valeur As String) As T
        If GetType(T).BaseType IsNot GetType([Enum]) Then
            Throw New FormatException("T doit être une énumération")
        End If

        ' On est gentil et on ignore la casse dans le parsing
        Return DirectCast([Enum].Parse(GetType(T), valeur, True), T)
    End Function

    Private Function EstPoolTS() As Boolean
        'Le nom de l'usager est composé comme tel:  Domaine\CodeUtilisateur 
        Dim strDomCodeUtil As String() = WindowsIdentity.GetCurrent.Name.Split("\".ToCharArray())
        Dim regexComptePool As New Regex("ZAP[IABSP]WTS")
        If regexComptePool.IsMatch(strDomCodeUtil(1)) Then
            Return True
        End If

        Return False
    End Function

    Private Function EstAppelWCFouWEBAPI() As Boolean
        'Le nom de l'usager est composé comme tel:  Domaine\CodeUtilisateur 
        Dim strDomCodeUtil As String() = WindowsIdentity.GetCurrent.Name.Split("\".ToCharArray())
        Dim regexComptePool As New Regex("ZAP[IABSP]W|SYS(INT|ACC|FOA|SIM|PRD)APAPI")
        If regexComptePool.IsMatch(strDomCodeUtil(1)) Then
            Return True
        End If

        Return False
    End Function

    <MethodImpl(MethodImplOptions.NoInlining)>
    Private Function CreerAccesseurFichier() As TsIAccesseur
        Return New TsCuAccesseurFichier
    End Function

    <MethodImpl(MethodImplOptions.NoInlining)>
    Private Function CreerAccesseurService() As TsIAccesseur
        Return New TsCuAccesseurCS
    End Function

    <MethodImpl(MethodImplOptions.NoInlining)>
    Private Function CreerAccesseurCIWCF() As TsIAccesseur
        Return New TsCuAccesseurCI
    End Function
End Class