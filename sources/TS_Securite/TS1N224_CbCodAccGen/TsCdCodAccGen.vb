Imports System.Text
Imports TS1N201_DtCdAccGenV1
Imports Rrq.InfrastructureCommune.ScenarioTransactionnel.ClassesBaseService
Imports Rrq.InfrastructureCommune.ScenarioTransactionnel

'''-----------------------------------------------------------------------------
''' Project		: $safeprojectname$
''' Class		: TsCdCodAccGen
'''
'''-----------------------------------------------------------------------------
''' <summary>
''' Classe permettant de stocker les paramètres utilisés par l'application. Elle
''' contient des propriétés partagées.
''' </summary>
''' <remarks></remarks>
'''-----------------------------------------------------------------------------
Friend Class TsCdCodAccGen
    Inherits XuCdAccesDonnees

#Region "--- Constantes ---"
    Private Const SQL_OBTENIR_LISTE_CLE As String =
        "SELECT [CO_IDN_CLE_SYM_TS] " &
        ",[CO_TYP_CLE_SYM_TS]" &
        ",[CO_ENV_CLE_SYM_TS]" &
        ",[CO_TYP_DEP_CLE_TS]" &
        ",[CO_SYS_CLE_SYM_TS]" &
        ",[CO_SOU_CLE_SYM_TS]" &
        ",[CO_UTL_GEN_CLE_TS]" &
        ",[VL_MOT_PAS_CLE_TS]" &
        ",[DS_CLE_SYM_TS]" &
        ",[CM_CLE_SYM_TS]" &
        ",[VL_VER_CLE_SYM_TS]" &
        " FROM [TS1].[CLSYMTS]"

    Private Const SQL_OBTENIR_CLE As String =
        "SELECT cle.[CO_IDN_CLE_SYM_TS] " &
        ",cle.[CO_TYP_CLE_SYM_TS] " &
        ",cle.[CO_ENV_CLE_SYM_TS] " &
        ",cle.[CO_TYP_DEP_CLE_TS] " &
        ",cle.[CO_SYS_CLE_SYM_TS] " &
        ",cle.[CO_SOU_CLE_SYM_TS] " &
        ",cle.[CO_UTL_GEN_CLE_TS] " &
        ",cle.[VL_MOT_PAS_CLE_TS] " &
        ",cle.[DS_CLE_SYM_TS] " &
        ",cle.[CM_CLE_SYM_TS] " &
        ",cle.[VL_VER_CLE_SYM_TS] " &
        ",gro.[NM_GRO_ACT_DIR_TS]  " &
        " FROM [TS1].[CLSYMTS] cle " &
        " Left outer join [TS1].[GCLSYTS] clegro on cle.CO_IDN_CLE_SYM_TS = clegro.CO_IDN_CLE_SYM_TS  " &
        " Left outer join [TS1].[GACDITS] gro on clegro.NM_GRO_ACT_DIR_TS = gro.NM_GRO_ACT_DIR_TS " &
        " WHERE cle.CO_IDN_CLE_SYM_TS = @CO_IDN_CLE_SYM_TS"

    Private Const SQL_OBTENIR_GROUPE As String =
        "SELECT gro.[NM_GRO_ACT_DIR_TS] FROM [TS1].[GACDITS] gro WHERE gro.NM_GRO_ACT_DIR_TS = @NM_GRO_ACT_DIR_TS "

    Private Const SQL_INSERT_GROUPE As String =
        "INSERT INTO [TS1].[GACDITS] ([NM_GRO_ACT_DIR_TS]) VALUES (@NM_GRO_ACT_DIR_TS) "

    Private Const SQL_INSERT_LIEN_GROUPE_CLE As String =
        "INSERT INTO [TS1].[GCLSYTS]" &
        " ([CO_IDN_CLE_SYM_TS], [NM_GRO_ACT_DIR_TS])" &
        " VALUES " &
        "(@CO_IDN_CLE_SYM_TS, @NM_GRO_ACT_DIR_TS) "

    Private Const SQL_LIBERER_LIEN_GROUPE_CLE As String =
        "DELETE FROM [TS1].[GCLSYTS] WHERE [CO_IDN_CLE_SYM_TS] =@CO_IDN_CLE_SYM_TS "

    Private Const SQL_DELETE_CLE As String =
        "DELETE FROM [TS1].[CLSYMTS] WHERE [CO_IDN_CLE_SYM_TS] =@CO_IDN_CLE_SYM_TS "

    Private Const SQL_OBTENIR_CLES As String =
        "SELECT cle.[CO_IDN_CLE_SYM_TS] " &
        ",cle.[CO_TYP_CLE_SYM_TS] " &
        ",cle.[CO_ENV_CLE_SYM_TS] " &
        ",cle.[CO_TYP_DEP_CLE_TS] " &
        ",cle.[CO_SYS_CLE_SYM_TS] " &
        ",cle.[CO_SOU_CLE_SYM_TS] " &
        ",cle.[CO_UTL_GEN_CLE_TS] " &
        ",cle.[VL_MOT_PAS_CLE_TS] " &
        ",cle.[DS_CLE_SYM_TS] " &
        ",cle.[CM_CLE_SYM_TS] " &
        ",cle.[VL_VER_CLE_SYM_TS] " &
        " FROM [TS1].[CLSYMTS] cle " &
        " WHERE cle.CO_IDN_CLE_SYM_TS like @CO_IDN_CLE_SYM_TS"

    Private Const SQL_OBTENIR_GROUPES As String =
        "SELECT cle.[CO_IDN_CLE_SYM_TS], gro.[NM_GRO_ACT_DIR_TS]  " &
        " FROM [TS1].[CLSYMTS] cle " &
        " inner join [TS1].[GCLSYTS] clegro on cle.CO_IDN_CLE_SYM_TS = clegro.CO_IDN_CLE_SYM_TS  " &
        " Left outer join [TS1].[GACDITS] gro on clegro.NM_GRO_ACT_DIR_TS = gro.NM_GRO_ACT_DIR_TS " &
        " WHERE cle.CO_IDN_CLE_SYM_TS = @CO_IDN_CLE_SYM_TS"

    Private Const SQL_INSERT_CLE As String =
        "INSERT INTO [TS1].[CLSYMTS] ( " &
        "[CO_IDN_CLE_SYM_TS]" &
        ",[CO_TYP_CLE_SYM_TS]" &
        ",[CO_ENV_CLE_SYM_TS]" &
        ",[CO_TYP_DEP_CLE_TS]" &
        ",[CO_SYS_CLE_SYM_TS]" &
        ",[CO_SOU_CLE_SYM_TS]" &
        ",[CO_UTL_GEN_CLE_TS]" &
        ",[VL_MOT_PAS_CLE_TS]" &
        ",[DS_CLE_SYM_TS]" &
        ",[CM_CLE_SYM_TS]" &
        ",[VL_VER_CLE_SYM_TS]" &
        ") VALUES (" &
        "@CO_IDN_CLE_SYM_TS" &
        ",@CO_TYP_CLE_SYM_TS" &
        ",@CO_ENV_CLE_SYM_TS" &
        ",@CO_TYP_DEP_CLE_TS" &
        ",@CO_SYS_CLE_SYM_TS" &
        ",@CO_SOU_CLE_SYM_TS" &
        ",@CO_UTL_GEN_CLE_TS" &
        ",@VL_MOT_PAS_CLE_TS" &
        ",@DS_CLE_SYM_TS" &
        ",@CM_CLE_SYM_TS" &
        ",@VL_VER_CLE_SYM_TS" &
        ")"

    Private Const SQL_UPDATE_CLE As String =
        "UPDATE [TS1].[CLSYMTS] SET " &
        "[CO_TYP_CLE_SYM_TS] = @CO_TYP_CLE_SYM_TS" &
        ",[CO_ENV_CLE_SYM_TS] = @CO_ENV_CLE_SYM_TS" &
        ",[CO_TYP_DEP_CLE_TS] = @CO_TYP_DEP_CLE_TS" &
        ",[CO_SYS_CLE_SYM_TS] = @CO_SYS_CLE_SYM_TS" &
        ",[CO_SOU_CLE_SYM_TS] = @CO_SOU_CLE_SYM_TS" &
        ",[CO_UTL_GEN_CLE_TS] = @CO_UTL_GEN_CLE_TS" &
        ",[VL_MOT_PAS_CLE_TS] = @VL_MOT_PAS_CLE_TS" &
        ",[DS_CLE_SYM_TS] = @DS_CLE_SYM_TS" &
        ",[CM_CLE_SYM_TS] = @CM_CLE_SYM_TS" &
        ",[VL_VER_CLE_SYM_TS] = @VL_VER_CLE_SYM_TS" &
        " WHERE " &
        "[CO_IDN_CLE_SYM_TS] = @CO_IDN_CLE_SYM_TS"

    Private Const SQL_INSERT_JOURNAL As String =
        "INSERT INTO [TS1].[HIACSTS] " &
        "([CO_TYP_ACT_CLE_TS], [CO_ENV_ACT_CLE_TS], [CO_IDN_CLE_SYM_TS], [DH_ACT_CLE_SYM_TS], [CO_UTL_ACT_CLE_TS])" &
        " OUTPUT INSERTED.[NO_SEQ_ACT_CLE_TS] VALUES " &
        "(@CO_TYP_ACT_CLE_TS, @CO_ENV_ACT_CLE_TS, @CO_IDN_CLE_SYM_TS, @DH_ACT_CLE_SYM_TS, @CO_UTL_ACT_CLE_TS)"

    Private Const SQL_INSERT_JOURNAL_MOD As String =
        "INSERT INTO [TS1].[MOCLSTS] " &
        "([NO_SEQ_ACT_CLE_TS], [NM_COD_ELM_ACT_TS], [VL_AVN_ACT_CLE_TS], [VL_APR_ACT_CLE_TS])" &
        " VALUES " &
        "(@NO_SEQ_ACT_CLE_TS, @NM_COD_ELM_ACT_TS, @VL_AVN_ACT_CLE_TS, @VL_APR_ACT_CLE_TS)"

    Private Const SQL_DERNIERE_MODIFICATION As String = "SELECT MAX(DH_ACT_CLE_SYM_TS) FROM [TS1].[HIACSTS] WHERE CO_TYP_ACT_CLE_TS <> 'CON'"

#End Region

#Region "--- Constructeurs ---"
    ''' <summary>
    ''' Surcharge du construteur de la base pour passer la chaine de connexion à la BD
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        MyBase.New("SQL_Securite")
    End Sub

#End Region

#Region "--- Publiques ---"

    ''' <summary>
    ''' Obtient la date de dernière modification sur les clés symboliques
    ''' </summary>
    ''' <param name="ChaineContexte"></param>
    ''' <returns>Date de dernière modification</returns>
    Friend Function ObtenirDateDerniereModification(ByRef ChaineContexte As String) As DateTime
        'Création des paramètres pour la requête
        Dim Criteres As New List(Of XuDtCritere)

        'Obtenir la liste
        Dim donnees As DataTable = MyBase.ExecuterLecture(ChaineContexte, SQL_DERNIERE_MODIFICATION, Criteres)
        If donnees.Rows.Count > 0 Then
            Return DirectCast(donnees(0)(0), DateTime)
        End If

        Return DateTime.MinValue
    End Function

    ''' <summary>
    ''' Obtient une clé avec ses sous-structures
    ''' </summary>
    ''' <param name="ChaineContexte"></param>
    ''' <param name="pCoIdnCleSymTs"></param>
    ''' <returns></returns>
    ''' <remarks>PLusieurs requêtes Linq sont utilisées au lieu d'une très grosse pour facilité l'entretien</remarks>
    Friend Function ObtenirCle(ByRef ChaineContexte As String, ByVal pCoIdnCleSymTs As String) As TsDtCleSym
        Dim Cle As New TsDtCleSym

        'ON va chercher un datatable qui contient tous les élément
        Dim dtCle As DataTable = SelectCle(ChaineContexte, pCoIdnCleSymTs)

        If dtCle.Rows.Count > 0 Then
            Cle = (From cl In dtCle.AsEnumerable Take 1 Select New TsDtCleSym With {
                .CoIdnCleSymTs = cl.Field(Of String)("CO_IDN_CLE_SYM_TS"),
                .CoTypCleSymTs = cl.Field(Of String)("CO_TYP_CLE_SYM_TS"),
                .CoEnvCleSymTs = cl.Field(Of String)("CO_ENV_CLE_SYM_TS"),
                .CoTypDepCleTs = cl.Field(Of String)("CO_TYP_DEP_CLE_TS"),
                .CoSysCleSymTs = cl.Field(Of String)("CO_SYS_CLE_SYM_TS"),
                .CoSouCleSymTs = cl.Field(Of String)("CO_SOU_CLE_SYM_TS"),
                .CoUtlGenCleTs = cl.Field(Of String)("CO_UTL_GEN_CLE_TS"),
                .VlMotPasCleTs = cl.Field(Of String)("VL_MOT_PAS_CLE_TS"),
                .DsCleSymTs = cl.Field(Of String)("DS_CLE_SYM_TS"),
                .CmCleSymTs = cl.Field(Of String)("CM_CLE_SYM_TS"),
                .VlVerCleSymTs = cl.Field(Of String)("VL_VER_CLE_SYM_TS")
            }).FirstOrDefault

            Cle.LsGroAd = (From cl In dtCle.AsEnumerable Distinct Select New TsDtGroAd With {
                .NmGroActDirTs = cl.Field(Of String)("NM_GRO_ACT_DIR_TS")
            }).ToList

        Else
            'C'est le service qui décidera comment informer son appelant que l'élément est inexistant
            Cle = Nothing
        End If

        Return Cle
    End Function

    ''' <summary>
    ''' Obtient une clé avec ses sous-structures
    ''' </summary>
    ''' <param name="pChaineContexte"></param>
    ''' <param name="pNmCleParent"></param>
    ''' <returns></returns>
    ''' <remarks>PLusieurs requêtes Linq sont utilisées au lieu d'une très grosse pour facilité l'entretien</remarks>
    Friend Function ObtenirListeClesParent(ByRef pChaineContexte As String, ByVal pNmCleParent As String) As IList(Of TsDtCleSym)
        Dim cles As New List(Of TsDtCleSym)

        'ON va chercher un datatable qui contient tous les élément
        Dim dtCle As DataTable = SelectCles(pChaineContexte, pNmCleParent)
        If dtCle.Rows.Count > 0 Then
            cles = (From cl In dtCle.AsEnumerable Select New TsDtCleSym With {
                .CoIdnCleSymTs = cl.Field(Of String)("CO_IDN_CLE_SYM_TS"),
                .CoTypCleSymTs = cl.Field(Of String)("CO_TYP_CLE_SYM_TS"),
                .CoEnvCleSymTs = cl.Field(Of String)("CO_ENV_CLE_SYM_TS"),
                .CoTypDepCleTs = cl.Field(Of String)("CO_TYP_DEP_CLE_TS").EnChaineSansBlancs(),
                .CoSysCleSymTs = cl.Field(Of String)("CO_SYS_CLE_SYM_TS"),
                .CoSouCleSymTs = cl.Field(Of String)("CO_SOU_CLE_SYM_TS"),
                .CoUtlGenCleTs = cl.Field(Of String)("CO_UTL_GEN_CLE_TS"),
                .VlMotPasCleTs = cl.Field(Of String)("VL_MOT_PAS_CLE_TS"),
                .DsCleSymTs = cl.Field(Of String)("DS_CLE_SYM_TS"),
                .CmCleSymTs = cl.Field(Of String)("CM_CLE_SYM_TS"),
                .VlVerCleSymTs = cl.Field(Of String)("VL_VER_CLE_SYM_TS")
            }).ToList
        End If

        Return cles
    End Function

    ''' <summary>
    ''' Obtient la liste des clés sans info complémentaire
    ''' </summary>
    ''' <param name="pChaineContexte"></param>
    ''' <returns></returns>
    Friend Function ObtenirListeCles(ByRef pChaineContexte As String) As IList(Of TsDtCleSym)
        Dim result As New List(Of TsDtCleSym)

        Dim dtListCles As DataTable = SelectListeCles(pChaineContexte)

        If dtListCles.Rows.Count > 0 Then
            result = (From cles In dtListCles.AsEnumerable Select New TsDtCleSym With {
                .CoIdnCleSymTs = cles.Field(Of String)("CO_IDN_CLE_SYM_TS"),
                .CoTypCleSymTs = cles.Field(Of String)("CO_TYP_CLE_SYM_TS"),
                .CoEnvCleSymTs = cles.Field(Of String)("CO_ENV_CLE_SYM_TS"),
                .CoTypDepCleTs = cles.Field(Of String)("CO_TYP_DEP_CLE_TS"),
                .CoSysCleSymTs = cles.Field(Of String)("CO_SYS_CLE_SYM_TS"),
                .CoSouCleSymTs = cles.Field(Of String)("CO_SOU_CLE_SYM_TS"),
                .CoUtlGenCleTs = cles.Field(Of String)("CO_UTL_GEN_CLE_TS"),
                .DsCleSymTs = cles.Field(Of String)("DS_CLE_SYM_TS"),
                .CmCleSymTs = cles.Field(Of String)("CM_CLE_SYM_TS")
            }).ToList
        End If

        Return result
    End Function

    ''' <summary>
    ''' Obtient la liste des clés sans info complémentaire
    ''' </summary>
    ''' <param name="pChaineContexte"></param>
    ''' <returns></returns>
    Friend Function ObtenirCleRecherche(ByRef pChaineContexte As String,
                                        ByVal pCoTypCle As String,
                                        ByVal pCoTypEnv As String,
                                        ByVal pGroupeAd As String,
                                        ByVal pIdCle As String,
                                        ByVal pUsagerAd As String) As DataTable

        Return SelectCleRecherche(pChaineContexte, pCoTypCle, pCoTypEnv, pGroupeAd, pIdCle, pUsagerAd)
    End Function

    Friend Function InsertCle(ByRef pChaineContexte As String, ByRef cle As TsDtCleSym) As Boolean
        Dim listeParam As New List(Of XuDtCritere)

        With cle
            listeParam.Add(New XuDtCritere("CLSYMTS", "CO_IDN_CLE_SYM_TS", .CoIdnCleSymTs))
            listeParam.Add(New XuDtCritere("CLSYMTS", "CO_TYP_CLE_SYM_TS", .CoTypCleSymTs))
            listeParam.Add(New XuDtCritere("CLSYMTS", "CO_ENV_CLE_SYM_TS", .CoEnvCleSymTs))
            listeParam.Add(New XuDtCritere("CLSYMTS", "CO_TYP_DEP_CLE_TS", .CoTypDepCleTs))
            listeParam.Add(New XuDtCritere("CLSYMTS", "CO_SYS_CLE_SYM_TS", .CoSysCleSymTs))
            listeParam.Add(New XuDtCritere("CLSYMTS", "CO_SOU_CLE_SYM_TS", .CoSouCleSymTs))
            listeParam.Add(New XuDtCritere("CLSYMTS", "CO_UTL_GEN_CLE_TS", .CoUtlGenCleTs))
            listeParam.Add(New XuDtCritere("CLSYMTS", "VL_MOT_PAS_CLE_TS", .VlMotPasCleTs))
            listeParam.Add(New XuDtCritere("CLSYMTS", "DS_CLE_SYM_TS", .DsCleSymTs))
            listeParam.Add(New XuDtCritere("CLSYMTS", "CM_CLE_SYM_TS", .CmCleSymTs))
            listeParam.Add(New XuDtCritere("CLSYMTS", "VL_VER_CLE_SYM_TS", .VlVerCleSymTs))
        End With

        Try
            If MyBase.ExecuterMAJ(pChaineContexte, SQL_INSERT_CLE, listeParam) <> 1 Then
                Return False
            End If

        Catch e As SqlClient.SqlException When e.Number = 2627
            Throw New XuExcEErrValidation("Une clé existe déjà, rafraichissez votre liste et essayez de nouveau.")
        End Try

        Return True
    End Function

    Friend Function ModifierCle(ByRef pChaineContexte As String, ByRef cle As TsDtCleSym) As Boolean
        Dim listeParam As New List(Of XuDtCritere)

        With cle
            listeParam.Add(New XuDtCritere("CLSYMTS", "CO_IDN_CLE_SYM_TS", .CoIdnCleSymTs))
            listeParam.Add(New XuDtCritere("CLSYMTS", "CO_TYP_CLE_SYM_TS", .CoTypCleSymTs))
            listeParam.Add(New XuDtCritere("CLSYMTS", "CO_ENV_CLE_SYM_TS", .CoEnvCleSymTs))
            listeParam.Add(New XuDtCritere("CLSYMTS", "CO_TYP_DEP_CLE_TS", .CoTypDepCleTs))
            listeParam.Add(New XuDtCritere("CLSYMTS", "CO_SYS_CLE_SYM_TS", .CoSysCleSymTs))
            listeParam.Add(New XuDtCritere("CLSYMTS", "CO_SOU_CLE_SYM_TS", .CoSouCleSymTs))
            listeParam.Add(New XuDtCritere("CLSYMTS", "CO_UTL_GEN_CLE_TS", .CoUtlGenCleTs))
            listeParam.Add(New XuDtCritere("CLSYMTS", "VL_MOT_PAS_CLE_TS", .VlMotPasCleTs))
            listeParam.Add(New XuDtCritere("CLSYMTS", "DS_CLE_SYM_TS", .DsCleSymTs))
            listeParam.Add(New XuDtCritere("CLSYMTS", "CM_CLE_SYM_TS", .CmCleSymTs))
            listeParam.Add(New XuDtCritere("CLSYMTS", "VL_VER_CLE_SYM_TS", .VlVerCleSymTs))
        End With

        If MyBase.ExecuterMAJ(pChaineContexte, SQL_UPDATE_CLE, listeParam) <> 1 Then
            Return False
        End If

        Return True
    End Function

    Friend Function DetruireCle(ByRef pChaineContexte As String, ByVal cle As TsDtCleSym) As Boolean
        Dim listeParam As New List(Of XuDtCritere)
        listeParam.Add(New XuDtCritere("CLSYMTS", "CO_IDN_CLE_SYM_TS", cle.CoIdnCleSymTs))

        If MyBase.ExecuterMAJ(pChaineContexte, SQL_DELETE_CLE, listeParam) <> 1 Then
            Return False
        End If

        Return True
    End Function

    Friend Function ExisteGroupe(ByRef pChaineContexte As String, ByVal groupe As TsDtGroAd) As Boolean
        Dim listeParam As New List(Of XuDtCritere)
        listeParam.Add(New XuDtCritere("GACDITS", "NM_GRO_ACT_DIR_TS", groupe.NmGroActDirTs))

        Return MyBase.ExecuterLecture(pChaineContexte, SQL_OBTENIR_GROUPE, listeParam).Rows.Count > 0
    End Function

    Friend Function InsertGroupe(ByRef pChaineContexte As String, ByVal groupe As TsDtGroAd) As Boolean
        Dim listeParam As New List(Of XuDtCritere)
        listeParam.Add(New XuDtCritere("GACDITS", "NM_GRO_ACT_DIR_TS", groupe.NmGroActDirTs))

        Return MyBase.ExecuterMAJ(pChaineContexte, SQL_INSERT_GROUPE, listeParam) = 1
    End Function

    Friend Function LierGroupe(ByRef pChaineContexte As String, ByVal pCle As TsDtCleSym, ByVal pGroupe As TsDtGroAd) As Boolean
        Dim listeParam As New List(Of XuDtCritere)
        listeParam.Add(New XuDtCritere("GCLSYTS", "CO_IDN_CLE_SYM_TS", pCle.CoIdnCleSymTs))
        listeParam.Add(New XuDtCritere("GCLSYTS", "NM_GRO_ACT_DIR_TS", pGroupe.NmGroActDirTs))

        Return MyBase.ExecuterMAJ(pChaineContexte, SQL_INSERT_LIEN_GROUPE_CLE, listeParam) = 1
    End Function

    Friend Sub LibererGroupe(ByRef pChaineContexte As String, ByVal pCle As TsDtCleSym)
        Dim listeParam As New List(Of XuDtCritere)
        listeParam.Add(New XuDtCritere("GCLSYTS", "CO_IDN_CLE_SYM_TS", pCle.CoIdnCleSymTs))

        MyBase.ExecuterMAJ(pChaineContexte, SQL_LIBERER_LIEN_GROUPE_CLE, listeParam)
    End Sub

    ''' <summary>
    ''' Effectue la journalisation d'une action effectuée sur la banque des codes d'accès
    ''' </summary>
    ''' <param name="pChaineContexte">Chaine du contexte pour obtention du code utilisateur</param>
    ''' <param name="pAction">Code d'action (sur 3 caractères)</param>
    ''' <param name="pCleSymboliqueAv">Clé symbolique associée à l'action avec les valeurs avant modification (si applicable)</param>
    ''' <param name="pCleSymboliqueAp">Clé symbolique associée à l'action avec les valeurs après modification (si applicable)</param>
    Friend Sub Journaliser(ByRef pChaineContexte As String, ByVal pAction As String, ByVal pCleSymboliqueAv As TsDtCleSym, ByVal pCleSymboliqueAp As TsDtCleSym)
        Dim listeParam As New List(Of XuDtCritere)

        listeParam.Add(New XuDtCritere("HIACSTS", "CO_TYP_ACT_CLE_TS", pAction))

        If pCleSymboliqueAp IsNot Nothing Then
            listeParam.Add(New XuDtCritere("HIACSTS", "CO_IDN_CLE_SYM_TS", pCleSymboliqueAp.CoIdnCleSymTs))
            listeParam.Add(New XuDtCritere("HIACSTS", "CO_ENV_ACT_CLE_TS", pCleSymboliqueAp.CoEnvCleSymTs))

        ElseIf pCleSymboliqueAv IsNot Nothing Then
            listeParam.Add(New XuDtCritere("HIACSTS", "CO_IDN_CLE_SYM_TS", pCleSymboliqueAv.CoIdnCleSymTs))
            listeParam.Add(New XuDtCritere("HIACSTS", "CO_ENV_ACT_CLE_TS", pCleSymboliqueAv.CoEnvCleSymTs))

        Else
            listeParam.Add(New XuDtCritere("HIACSTS", "CO_IDN_CLE_SYM_TS", String.Empty))
            listeParam.Add(New XuDtCritere("HIACSTS", "CO_ENV_ACT_CLE_TS", String.Empty))
        End If

        listeParam.Add(New XuDtCritere("HIACSTS", "DH_ACT_CLE_SYM_TS", DateTime.Now))
        listeParam.Add(New XuDtCritere("HIACSTS", "CO_UTL_ACT_CLE_TS", XuCaContexte.CodeUsagerEssai(pChaineContexte)))

        Dim donneeInsere As DataTable = MyBase.ExecuterMAJAvecRetour(pChaineContexte, SQL_INSERT_JOURNAL, listeParam)
        Dim idCle As Integer = Convert.ToInt32(donneeInsere.Rows(0)("NO_SEQ_ACT_CLE_TS"))

        If pCleSymboliqueAv IsNot Nothing AndAlso pCleSymboliqueAp IsNot Nothing Then
            ' Code
            If pCleSymboliqueAv.CoUtlGenCleTs <> pCleSymboliqueAp.CoUtlGenCleTs Then
                JournaliserModification(pChaineContexte, idCle, "Code", pCleSymboliqueAv.CoUtlGenCleTs, pCleSymboliqueAp.CoUtlGenCleTs)
            End If

            ' Mot de passe
            If pCleSymboliqueAv.VlMotPasCleTs <> pCleSymboliqueAp.VlMotPasCleTs Then
                JournaliserModification(pChaineContexte, idCle, "MotDePasse", pCleSymboliqueAv.VlMotPasCleTs, pCleSymboliqueAp.VlMotPasCleTs)
            End If

            ' Profil
            Dim groupesAvant As String = ObtenirGroupesAdText(pCleSymboliqueAv.LsGroAd)
            Dim groupesApres As String = ObtenirGroupesAdText(pCleSymboliqueAp.LsGroAd)
            If groupesAvant <> groupesApres Then
                JournaliserModification(pChaineContexte, idCle, "Profil", groupesAvant, groupesApres)
            End If

            ' Description
            If pCleSymboliqueAv.DsCleSymTs <> pCleSymboliqueAp.DsCleSymTs Then
                JournaliserModification(pChaineContexte, idCle, "Description", pCleSymboliqueAv.DsCleSymTs, pCleSymboliqueAp.DsCleSymTs)
            End If

            ' Commentaire
            If pCleSymboliqueAv.CmCleSymTs <> pCleSymboliqueAp.CmCleSymTs Then
                JournaliserModification(pChaineContexte, idCle, "Commentaire", pCleSymboliqueAv.CmCleSymTs, pCleSymboliqueAp.CmCleSymTs)
            End If

            ' Code de vérification
            If pCleSymboliqueAv.VlVerCleSymTs <> pCleSymboliqueAp.VlVerCleSymTs Then
                JournaliserModification(pChaineContexte, idCle, "CodeVérification", pCleSymboliqueAv.VlVerCleSymTs, pCleSymboliqueAp.VlVerCleSymTs)
            End If

        End If
    End Sub

    ''' <summary>
    ''' Effectue la journalisation d'une modification effectuée sur une action donnée à la banque des codes d'accès
    ''' </summary>
    ''' <param name="pChaineContexte">Chaine du contexte pour obtention du code utilisateur</param>
    ''' <param name="pId">Identifiant de l'action</param>
    ''' <param name="pNomValeur">Nom de la valeur modifiée</param>
    ''' <param name="pValeurAvant">Valeur avant la modification</param>
    ''' <param name="pValeurApres">Valeur après la modification</param>
    Friend Sub JournaliserModification(ByRef pChaineContexte As String, ByVal pId As Integer, ByVal pNomValeur As String, ByVal pValeurAvant As String, ByVal pValeurApres As String)
        Dim listeParam As New List(Of XuDtCritere)

        listeParam.Add(New XuDtCritere("MOCLSTS", "NO_SEQ_ACT_CLE_TS", pId))
        listeParam.Add(New XuDtCritere("MOCLSTS", "NM_COD_ELM_ACT_TS", pNomValeur))
        listeParam.Add(New XuDtCritere("MOCLSTS", "VL_AVN_ACT_CLE_TS", pValeurAvant))
        listeParam.Add(New XuDtCritere("MOCLSTS", "VL_APR_ACT_CLE_TS", pValeurApres))

        MyBase.ExecuterMAJ(pChaineContexte, SQL_INSERT_JOURNAL_MOD, listeParam)
    End Sub

#End Region

#Region "--- Privées ---"

    ''' <summary>
    ''' Permet de concaténer tous les noms de groups d'AD contenus dans une même liste, et séparés par des ';'
    ''' </summary>
    ''' <param name="pListeGroupe">Liste des groupes d'AD</param>
    ''' <returns>Chaine de groupes concaténés</returns>
    Private Function ObtenirGroupesAdText(ByVal pListeGroupe As IList(Of TsDtGroAd)) As String
        Dim sb As New StringBuilder()

        For Each groupe As TsDtGroAd In pListeGroupe
            If sb.Length > 0 Then
                sb.Append(";")
            End If
            sb.Append(groupe.NmGroActDirTs)
        Next

        Dim chaineRetour As String = sb.ToString()
        If chaineRetour.Length > 250 Then
            chaineRetour = chaineRetour.Substring(0, 250)
        End If

        Return chaineRetour
    End Function

    Private Function SelectCle(ByRef ChaineContexte As String, ByVal pCoIdnCleSymTs As String) As DataTable
        'Création des paramètres pour la requête
        Dim Criteres As New List(Of XuDtCritere)
        Criteres.Add(New XuDtCritere("CLSYMTS", "CO_IDN_CLE_SYM_TS", pCoIdnCleSymTs))

        'Obtenir l'entité principal
        Return MyBase.ExecuterLecture(ChaineContexte, SQL_OBTENIR_CLE, Criteres)
    End Function

    Private Function SelectCles(ByRef ChaineContexte As String, ByVal nmCleParent As String) As DataTable
        'Création des paramètres pour la requête
        Dim Criteres As New List(Of XuDtCritere)
        Criteres.Add(New XuDtCritere("CLSYMTS", "CO_IDN_CLE_SYM_TS", nmCleParent & "_"))

        'Obtenir l'entité principal
        Return MyBase.ExecuterLecture(ChaineContexte, SQL_OBTENIR_CLES, Criteres)
    End Function

    Private Function SelectCleRecherche(ByRef pChaineContexte As String,
                                        ByVal pCoTypCle As String,
                                        ByVal pCoTypEnv As String,
                                        ByVal pGroupeAd As String,
                                        ByVal pIdCle As String,
                                        ByVal pUsagerAd As String) As DataTable
        'Création des paramètres pour la requête
        Dim Criteres As New List(Of XuDtCritere)

        If Not String.IsNullOrEmpty(pIdCle) Then
            Criteres.Add(New XuDtCritere("CLSYMTS", "CO_IDN_CLE_SYM_TS", pIdCle & "%"))
        End If

        If Not String.IsNullOrEmpty(pCoTypCle) AndAlso pCoTypCle <> Environnements.Tous.Code Then
            Criteres.Add(New XuDtCritere("CLSYMTS", "CO_TYP_CLE_SYM_TS", pCoTypCle))
        End If

        If Not String.IsNullOrEmpty(pCoTypEnv) AndAlso pCoTypEnv.ToUpper <> Environnements.Tous.Code.ToUpper Then
            Criteres.Add(New XuDtCritere("CLSYMTS", "CO_ENV_CLE_SYM_TS", pCoTypEnv))
        End If

        If Not String.IsNullOrEmpty(pGroupeAd) Then
            Criteres.Add(New XuDtCritere("GCLSYTS", "NM_GRO_ACT_DIR_TS", pGroupeAd & "%"))
        End If

        If Not String.IsNullOrEmpty(pUsagerAd) Then
            Criteres.Add(New XuDtCritere("CLSYMTS", "CO_UTL_GEN_CLE_TS", pUsagerAd & "%"))
        End If

        'Obtenir l'entité principal
        Return MyBase.ExecuterLecture(pChaineContexte,
                                      ObtenirSQLCleRecherche(pCoTypCle, pCoTypEnv, pGroupeAd, pIdCle, pUsagerAd),
                                      Criteres)
    End Function

    ''' <summary>
    ''' Obtenir la requete SQL pour lancer la recherche
    ''' </summary>
    ''' <param name="pCoTypCle">Code Type Cle</param>
    ''' <param name="pCoTypEnv">Code Type Environnement</param>
    ''' <param name="pGroupeAd">Profil (Groupe AD)</param>
    ''' <param name="pIdCle">Identifiant de la clé</param>
    ''' <param name="pUsagerAd">Code (Usager AD)</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function ObtenirSQLCleRecherche(ByVal pCoTypCle As String,
                                                   ByVal pCoTypEnv As String,
                                                   ByVal pGroupeAd As String,
                                                   ByVal pIdCle As String,
                                                   ByVal pUsagerAd As String) As String
        Dim sql As New StringBuilder()
        Dim nbCritere As Integer = 0

        sql.AppendLine("SELECT CL.CO_IDN_CLE_SYM_TS as Cle,")
        sql.AppendLine("CL.CO_TYP_CLE_SYM_TS as Type,")
        sql.AppendLine("CL.CO_ENV_CLE_SYM_TS as Env,")
        sql.AppendLine("CL.CO_UTL_GEN_CLE_TS as Code,")
        sql.AppendLine("GC.NM_GRO_ACT_DIR_TS as Profil")

        sql.AppendLine("FROM TS1.CLSYMTS as CL INNER JOIN TS1.GCLSYTS as GC")
        sql.AppendLine("ON CL.CO_IDN_CLE_SYM_TS = GC.CO_IDN_CLE_SYM_TS")
        If Not String.IsNullOrEmpty(pCoTypCle) AndAlso pCoTypCle <> Environnements.Tous.Code OrElse
           Not String.IsNullOrEmpty(pCoTypEnv) AndAlso pCoTypEnv.ToUpper <> Environnements.Tous.Code.ToUpper OrElse
           Not String.IsNullOrEmpty(pGroupeAd) OrElse
           Not String.IsNullOrEmpty(pIdCle) OrElse
           Not String.IsNullOrEmpty(pUsagerAd) Then
            sql.Append("WHERE ")

            If Not String.IsNullOrEmpty(pIdCle) Then
                nbCritere += 1
                sql.AppendLine("CL.CO_IDN_CLE_SYM_TS like @CO_IDN_CLE_SYM_TS")
            End If

            If Not String.IsNullOrEmpty(pCoTypCle) AndAlso pCoTypCle <> Environnements.Tous.Code Then
                If nbCritere > 0 Then sql.Append("AND ")
                nbCritere += 1
                sql.AppendLine("CL.CO_TYP_CLE_SYM_TS = @CO_TYP_CLE_SYM_TS")
            End If

            If Not String.IsNullOrEmpty(pCoTypEnv) AndAlso pCoTypEnv.ToUpper <> Environnements.Tous.Code.ToUpper Then
                If nbCritere > 0 Then sql.Append("AND ")
                nbCritere += 1
                sql.AppendLine("CL.CO_ENV_CLE_SYM_TS = @CO_ENV_CLE_SYM_TS")
            End If

            If Not String.IsNullOrEmpty(pGroupeAd) Then
                If nbCritere > 0 Then sql.Append("AND ")
                nbCritere += 1
                sql.AppendLine("GC.NM_GRO_ACT_DIR_TS like @NM_GRO_ACT_DIR_TS")
            End If

            If Not String.IsNullOrEmpty(pUsagerAd) Then
                If nbCritere > 0 Then sql.Append("AND ")
                nbCritere += 1
                sql.AppendLine("CL.CO_UTL_GEN_CLE_TS like @CO_UTL_GEN_CLE_TS")
            End If

        End If
        Return sql.ToString
    End Function

    Private Function SelectGroupes(ByRef ChaineContexte As String, ByVal nmCle As String) As DataTable
        'Création des paramètres pour la requête
        Dim Criteres As New List(Of XuDtCritere)
        Criteres.Add(New XuDtCritere("CLSYMTS", "CO_IDN_CLE_SYM_TS", nmCle))

        'Obtenir l'entité principal
        Return MyBase.ExecuterLecture(ChaineContexte, SQL_OBTENIR_GROUPES, Criteres)
    End Function

    Private Function SelectListeCles(ByRef ChaineContexte As String) As DataTable
        'Obtenir la liste
        Return MyBase.ExecuterLecture(ChaineContexte, SQL_OBTENIR_LISTE_CLE, New List(Of XuDtCritere)())
    End Function

#End Region

End Class
