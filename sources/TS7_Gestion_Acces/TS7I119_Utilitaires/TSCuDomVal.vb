Imports Rrq.Web.GabaritsPetitsSystemes.Utilitaires
Imports Config = Rrq.InfrastructureCommune.Parametres.XuCuConfiguration


Public Class TSCuDomVal
    Public Const PAGE_AJOUTER_ROLE As String = "../TS7I111_AccesUtilisateur/TS7SAjouterRoles.aspx"
    Public Const PAGE_COPIER_ROLE As String = "../TS7I111_AccesUtilisateur/TS7SCopierRoles.aspx"
    Public Const PAGE_RECHERCHER_EMPLOYE As String = "../TS7I111_AccesUtilisateur/TS7SRechercherEmploye.aspx"
    Public Const PAGE_ROLE_EMPLOYE As String = "../TS7I111_AccesUtilisateur/TS7SGererRolesEmploye.aspx"
    Public Const PAGE_SUPPRIMER_EMPLOYE As String = "../TS7I111_AccesUtilisateur/TS7SSupprimerEmploye.aspx"
    Public Const PAGE_CONFIRMATION_VALEURS As String = "../TS7I111_AccesUtilisateur/TS7SPageConfirmation.aspx"
    Public Const PAGE_CONFIRMATION As String = "../TS7I111_AccesUtilisateur/TS7SConfirmation.aspx"
    'Public Const CHEMIN_UA_MOUV_PERSONNEL As String = "\\S20NET01\IntranetProdBDRRQ$\ib\MouvPerso\t_uniteadm.txt"
    'Public Const CHEMIN_GROUPES_TRAVAIL_MOUV_PERSONNEL As String = "\\S20NET01\IntranetProdBDRRQ$\ib\MouvPerso\t_groupes.txt"
    
    Public Shared Function obtenirVille() As DataTable
        Dim dtRetour As DataTable = Nothing
        Dim objConn As IDbConnection = Nothing
        Dim strRequete As String = "" & _
             "SELECT RTRIM(CO_SYS_SER_NI) AS CO_SYS_SER," & _
            "       RTRIM(NM_ELE_DON_NI) AS NM_ELE_DON," & _
            "       RTRIM(VA_ELE_DON_NI) AS VA_ELE_DON," & _
            "       RTRIM(DS_ELE_DON_NI) AS DS_ELE_DON," & _
            "       IN_VAL_ACT_NI as IN_VAL_ACT," & _
            "       NO_ORD_PRE_NI as NO_ORD_PRE" & _
            "   FROM NI9.DOMVANI " & _
            "   WHERE CO_SYS_SER_NI = 'TS7' AND  " & _
            "       NM_ELE_DON_NI = 'Ville'  AND " & _
            "       IN_VAL_ACT_NI = 'O' " & _
            "       ORDER BY NO_ORD_PRE"

        objConn = NiCuADO.ObtenirConnexion("TS7", "", "\CNN\SQLSERVERNI1\Type")
        dtRetour = NiCuADO.ExecuterRequeteSelect(strRequete, objConn)
        Return dtRetour
    End Function

    Public Shared Function obtenirAdresseCourriel() As String
        'Obtenir l'adresse courriel de la sécurité
        Dim dtRetour As DataTable = Nothing
        Dim strRetour As String = Nothing
        Dim objConn As IDbConnection = Nothing
        Dim strRequete As String = "" & _
             "SELECT RTRIM(CO_SYS_SER_NI) AS CO_SYS_SER," & _
            "       RTRIM(NM_ELE_DON_NI) AS NM_ELE_DON," & _
            "       RTRIM(VA_ELE_DON_NI) AS VA_ELE_DON," & _
            "       RTRIM(DS_ELE_DON_NI) AS DS_ELE_DON," & _
            "       IN_VAL_ACT_NI as IN_VAL_ACT," & _
            "       NO_ORD_PRE_NI as NO_ORD_PRE" & _
            "   FROM NI9.DOMVANI " & _
            "   WHERE CO_SYS_SER_NI = 'TS7' AND  " & _
            "       NM_ELE_DON_NI = 'Courriel_Securite'  AND " & _
            "       IN_VAL_ACT_NI = 'O' " & _
            "       ORDER BY NO_ORD_PRE"

        objConn = NiCuADO.ObtenirConnexion("TS7", "", "\CNN\SQLSERVERNI1\Type")
        dtRetour = NiCuADO.ExecuterRequeteSelect(strRequete, objConn)
        If dtRetour.Rows.Count > 0 Then
            strRetour = dtRetour.Rows(0).Item("DS_ELE_DON").ToString
        End If

        Return strRetour
    End Function

    Public Shared Function ObtenirCheminHeat() As String
        Dim strRetour As String = Nothing

        strRetour = Config.ValeurSysteme("TS7", "TS7\TS7N111\CheminDepotHeat")

        Return strRetour
    End Function
    Public Shared Function ObtenirAssignationHeat() As String
        Dim strRetour As String = Nothing

        strRetour = Config.ValeurSysteme("TS7", "TS7\TS7N111\Assignation")

        Return strRetour
    End Function

    Public Shared Function ObtenirAdrCourrielGestionAcces() As String
        Dim strRetour As String = Nothing

        strRetour = Config.ValeurSysteme("TS7", "TS7\TS7N111\AdrCourrielGestionAcces")

        Return strRetour
    End Function

    Public Shared Function ObtenirCheminAdrCourrielGestionnaires() As String
        Dim strRetour As String = Nothing

        strRetour = Config.ValeurSysteme("TS7", "TS7\TS7N111\CheminAdrCourrielGestionnaires")

        Return strRetour
    End Function
    Public Shared Function ObtenirCheminAdrCourrielSupportTechnique() As String
        Dim strRetour As String = Nothing

        strRetour = Config.ValeurSysteme("TS7", "TS7\TS7N111\AdresseSupportTechnique")

        Return strRetour
    End Function
    Public Shared Function ObtenirCheminAdrCourrielRepondantsSecurite() As String
        Dim strRetour As String = Nothing

        strRetour = Config.ValeurSysteme("TS7", "TS7\TS7N111\CheminAdrCourrielRepondants")

        Return strRetour
    End Function

    Public Shared Function ObtenirFichierUniteAdministratives() As String
        Dim strRetour As String = Nothing

        strRetour = Config.ValeurSysteme("TS7", "TS7\TS7N111\UniteAdministrative")

        Return strRetour
    End Function

    Public Shared Function ObtenirFichierMetiers() As String
        Dim strRetour As String = Nothing

        strRetour = Config.ValeurSysteme("TS7", "TS7\TS7N111\CheminFichierMetiers")

        Return strRetour
    End Function

    Public Shared Function ObtenirFichierReglesCoherence() As String
        Dim strRetour As String = Nothing

        strRetour = Config.ValeurSysteme("TS7", "TS7\TS7N111\CheminFichierCoherence")

        Return strRetour
    End Function

End Class
