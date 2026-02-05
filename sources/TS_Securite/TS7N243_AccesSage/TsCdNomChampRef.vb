Imports Rrq.Securite.GestionAcces.TsCdConstanteNomChampNorm
''' <summary>
''' Classe de donnée Fix. 
''' Elle possède la définition des champs de Sage et sont affilier aux "Noms des champs normalisées".
''' </summary>
''' <remarks></remarks>
Public Class TsCdNomChampRef
    Private Shared _mapChampRef As Dictionary(Of String, TsCdChampSage) = Nothing

    ''' <summary>
    ''' Avoir le dictionnaire qui mappe des noms de champs normalisés vers les noms Sage
    ''' </summary>
    ''' <returns>Un dictionnaire.</returns>
    Public Shared ReadOnly Property MapChampRef() As Dictionary(Of String, TsCdChampSage)
        Get
            If _mapChampRef Is Nothing Then
                _mapChampRef = ConstruireDictioRef()
            End If
            Return _mapChampRef
        End Get
    End Property

    ''' <summary>
    ''' Fonction de services. Sert à construire le dictionnaire qui mappe des noms de référence vers les noms Sage
    ''' </summary>
    ''' <returns>Un dictionnaire de référence</returns>
    Private Shared Function ConstruireDictioRef() As Dictionary(Of String, TsCdChampSage)
        Dim paramRetour As New Dictionary(Of String, TsCdChampSage)

        With paramRetour

            '! UTILISATEUR
            .Add(USER.PERSON_ID, New TsCdChampSage(-1, "PersonID"))
            .Add(USER.NAME, New TsCdChampSage(-1, "UserName"))
            .Add(USER.ORGANIZATION, New TsCdChampSage(-1, "Organization"))
            .Add(USER.ORGANIZATION_TYPE, New TsCdChampSage(-1, "OrganizationType"))
            .Add(USER.VILLE, New TsCdChampSage(1, "Ville"))
            .Add(USER.COURRIEL, New TsCdChampSage(2, "Mail"))
            .Add(USER.DATE_FIN, New TsCdChampSage(3, "Date de fin"))
            .Add(USER.PRENOM, New TsCdChampSage(4, "Prénom"))
            .Add(USER.NOM, New TsCdChampSage(5, "Nom"))
            .Add(USER.DATE_APPROBATION, New TsCdChampSage(6, "Date approbation"))
            .Add(USER.CN, New TsCdChampSage(7, "CN"))
            .Add(USER.NOM_UNITE, New TsCdChampSage(8, "Nom unité"))
            .Add(USER.GESTIONNAIRE, New TsCdChampSage(9, "Gestionnaire"))
            .Add(USER.CHAMP_9, New TsCdChampSage(9, "Field 9"))
            .Add(USER.TITRE, New TsCdChampSage(10, "Titre"))
            .Add(USER.CHAMP_10, New TsCdChampSage(10, "Field 10"))
            .Add(USER.CHAMP_11, New TsCdChampSage(11, "Field 11"))
            .Add(USER.CHAMP_12, New TsCdChampSage(12, "Field 12"))

            '! RÔLE
            .Add(ROLE.ID, New TsCdChampSage(-1, "ID"))
            .Add(ROLE.NAME, New TsCdChampSage(-1, "Name"))
            .Add(ROLE.DESCRIPTION, New TsCdChampSage(-1, "Description"))
            .Add(ROLE.ORGANIZATION, New TsCdChampSage(-1, "Organization"))
            .Add(ROLE.OWNER, New TsCdChampSage(-1, "Owner"))
            .Add(ROLE.TYPE, New TsCdChampSage(-1, "Type"))
            .Add(ROLE.CREATE_DATE, New TsCdChampSage(-1, "CreateDate"))
            .Add(ROLE.EXPIRATION_DATE, New TsCdChampSage(-1, "ExpirationDate"))
            .Add(ROLE.REVIEWER, New TsCdChampSage(-1, "Reviewer"))
            .Add(ROLE.APPROVE_CODE, New TsCdChampSage(-1, "ApprovalStatus"))
            .Add(ROLE.APPROVED_DATE, New TsCdChampSage(-1, "ApprovalDate"))
            .Add(ROLE.ORGANIZATION2, New TsCdChampSage(-1, "Organization2"))
            .Add(ROLE.ORGANIZATION3, New TsCdChampSage(-1, "Organization3"))
            .Add(ROLE.RULE, New TsCdChampSage(-1, "Rule"))

            '! RESSOURCE
            .Add(RESSOURCE.RESNAME_1, New TsCdChampSage(-1, "ResName1"))
            .Add(RESSOURCE.RESNAME_2, New TsCdChampSage(-1, "ResName2"))
            .Add(RESSOURCE.RESNAME_3, New TsCdChampSage(-1, "ResName3"))
            .Add(RESSOURCE.DESCRIPTION, New TsCdChampSage(1, "Nom fonctionnel ou Description"))
            .Add(RESSOURCE.CN, New TsCdChampSage(2, "CN"))
            .Add(RESSOURCE.DETAILS, New TsCdChampSage(3, "Détails"))
            .Add(RESSOURCE.DETENTEUR, New TsCdChampSage(4, "Détenteur"))
            .Add(RESSOURCE.DATE_CREATION, New TsCdChampSage(5, "Date création"))
            .Add(RESSOURCE.DERN_MODIF, New TsCdChampSage(6, "Dernière modification"))
        End With

        Return paramRetour
    End Function

End Class

''' <summary>
''' Classe de constantes. Possède les nom de champ normalisé pour chaque entité.
''' </summary>
''' <remarks>
''' !!!IMPORTANT!!!
''' Cette Classe devra être placé dans un autre module plus global,
''' au cas où il y aurait d'autre sources de différences.
''' AUTRE REMARQUE: Toutes les constantes doivent avoir des noms uniques.
''' </remarks>
Public Class TsCdConstanteNomChampNorm
    'TODO: Réviser les noms normalisés: il faudrait se décoler de sage et donner des noms (normalisés) plus logiques
    ' Vérifier avec Lucinda les noms qui sont vraiment significatifs
    ' ATTENTION: Ça va demander de tout recompiler

    ''' <summary>
    ''' Noms des champs de l'entité Utilisateur.
    ''' </summary>
    Public Class USER
        Public Const ID As String = "User_UserID"
        Public Const PERSON_ID As String = "User_PersonID"
        Public Const NAME As String = "User_Name"
        Public Const ORGANIZATION As String = "User_Organization"
        Public Const ORGANIZATION_TYPE As String = "User_OrganizationType"
        Public Const COURRIEL As String = "User_Courriel"
        Public Const DATE_FIN As String = "User_DateFin"
        Public Const VILLE As String = "User_Ville"
        Public Const PRENOM As String = "User_Prenom"
        Public Const NOM As String = "User_Nom"
        Public Const DATE_APPROBATION As String = "User_DateApprobation"
        Public Const CN As String = "User_CN"
        Public Const NOM_UNITE As String = "User_nomUnite"
        Public Const GESTIONNAIRE As String = "User_Gestionnaire"
        Public Const TITRE As String = "User_Titre"

        Public Const CHAMP_9 As String = "Champ 9"
        Public Const CHAMP_10 As String = "Champ 10 pas utilisé"
        Public Const CHAMP_11 As String = "Champ 11 pas utilisé"
        Public Const CHAMP_12 As String = "Champ 12 pas utilisé"
    End Class

    ''' <summary>
    ''' Noms des champs de l'entité Rôle.
    ''' </summary>
    Public Class ROLE
        'TODO: Réviser les noms normalisés: il faudrait se décoler de sage et donner des noms (normalisés) plus logiques
        ' Ex: Id devrait être le REM_... et Nom le nom «friendly» et pas de «Rule»
        ' Vérifier avec Lucinda les noms qui sont vraiment significatifs
        ' ATTENTION: Ça va demander de tout recompiler

        Public Const ID As String = "Role_ID"
        Public Const NAME As String = "Role_Name"
        Public Const DESCRIPTION As String = "Role_Description"
        Public Const TYPE As String = "Role_Type"
        Public Const RULE As String = "Role_Rule"

        Public Const ORGANIZATION As String = "Role_Organization"
        Public Const OWNER As String = "Role_Owner"
        Public Const CREATE_DATE As String = "Role_CreateDate"
        Public Const APPROVE_CODE As String = "Role_ApproveCode"
        Public Const APPROVED_DATE As String = "Role_ApproveDate"
        Public Const EXPIRATION_DATE As String = "Role_ExpirationDate"
        Public Const ORGANIZATION2 As String = "Role_Organization2"
        Public Const ORGANIZATION3 As String = "Role_Organization3"
        Public Const REVIEWER As String = "Role_Reviewer"

    End Class

    ''' <summary>
    ''' Noms des champs de l'entité Ressource.
    ''' </summary>
    Public Class RESSOURCE
        'TODO: Réviser les noms normalisés: il faudrait se décoler de sage et donner des noms (normalisés) plus logiques
        ' Vérifier avec Lucinda les noms qui sont vraiment significatifs
        ' ATTENTION: Ça va demander de tout recompiler

        Public Const RESNAME_1 As String = "Res_ResName1"
        Public Const RESNAME_2 As String = "Res_ResName2"
        Public Const RESNAME_3 As String = "Res_ResName3"
        Public Const DESCRIPTION As String = "Res_Description"
        Public Const DERN_MODIF As String = "Res_DernModif"
        Public Const CN As String = "Res_Cn"
        Public Const DETAILS As String = "Res_Details"
        Public Const DETENTEUR As String = "Res_Detenteur"
        Public Const DATE_CREATION As String = "Res_DateCreation"

        Public Const RESNAME_4 As String = "Res_ResName4"
        Public Const FIELD_VALUE_1 As String = "Res_FieldValue1"
        Public Const FIELD_VALUE_2 As String = "Res_FieldValue2"
        Public Const FIELD_VALUE_3 As String = "Res_FieldValue3"
        Public Const FIELD_VALUE_4 As String = "Res_FieldValue4"
        Public Const FIELD_VALUE_5 As String = "Res_FieldValue5"
        Public Const FIELD_VALUE_6 As String = "Res_FieldValue6"
    End Class

End Class
