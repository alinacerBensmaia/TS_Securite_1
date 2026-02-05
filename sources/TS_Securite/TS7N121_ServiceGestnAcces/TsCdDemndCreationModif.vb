Imports Rrq.Securite.GestionAcces
Imports System.Xml.Serialization

''' <summary>
''' Cette classe permet de Créer ou Modifier un utilisateur dans sage.
''' Le constructeur choisie détermine le mode création ou modification.
''' </summary>
<XmlType()>
Public Class TsCdDemndCreationModif

#Region "Enum"

    Enum TsDCMode
        Creation
        Modification
    End Enum

#End Region

#Region "Private Vars"

    Private mIDUtilisateur As String
    Private mUtilisateur As TsCdUtilisateur
    Private mTexteLibre As String

    Private mUtilisateurOriginal As TsCdUtilisateur
    Private mRolesOriginaux As List(Of TsCdAssignationRole)

    Private mOperationsRoles As List(Of TsCdOperationRole)

    Private mMode As TsDCMode

    Private mPieceJointe As TsCuFichierPieceJointe

    Friend mLectureSeule As Boolean


#End Region

#Region "Propriétés"

    ''' <summary>
    ''' L'identifiant de l'utilisateur.
    ''' </summary>
    ''' <remarks>En lecture seule.</remarks>
    Public Property IDUtilisateur() As String
        Get
            Return mIDUtilisateur
        End Get
        Set(ByVal value As String)
            '! Il faut conserver le Set pour supporter la désérialisation XML
            If mLectureSeule = True Then
                Throw New TsExcErreurGeneral("L'élément est lecture seul. Vous ne pouvez l'accéder en écriture")
            Else
                mIDUtilisateur = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Texte libre avec la demande pour signaler un problème ou demander de l'aide.
    ''' </summary>
    Public Property TexteLibre() As String
        Get
            Return mTexteLibre
        End Get
        Set(ByVal value As String)
            mTexteLibre = value
        End Set
    End Property

    ''' <summary>
    ''' Les rôles originaux en lecture seule.
    ''' </summary>
    ''' <remarks>En lecture seule.</remarks>
    Public ReadOnly Property RolesOriginaux() As List(Of TsCdAssignationRole)
        Get
            If mLectureSeule Then
                Return New List(Of TsCdAssignationRole)(mRolesOriginaux)
            Else
                Return mRolesOriginaux
            End If
        End Get
    End Property

    ''' <summary>
    ''' L'utilisateur original de Sage , totalement barré.
    ''' </summary>
    ''' <remarks>Totalement barré.</remarks>
    Public Property UtilisateurOriginal() As TsCdUtilisateur
        Get
            Return mUtilisateurOriginal
        End Get
        Set(ByVal value As TsCdUtilisateur)
            ' Il faut conserver le Set pour supporter la désérialisation XML
            If mLectureSeule = True Then
                Throw New TsExcErreurGeneral("L'élément est lecture seul. Vous ne pouvez l'accéder en écriture")
            Else
                mUtilisateurOriginal = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' L'utilisateur partiellement barré.
    ''' </summary>
    ''' <remarks>Partiellement barré.</remarks>
    Public Property Utilisateur() As TsCdUtilisateur
        Get
            Return mUtilisateur
        End Get
        Set(ByVal value As TsCdUtilisateur)
            ' Il faut conserver le Set pour supporter la désérialisation XML
            If mLectureSeule = True Then
                Throw New TsExcErreurGeneral("L'élément est lecture seul. Vous ne pouvez l'accéder en écriture")
            Else
                mUtilisateur = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Les opérations sur les rôle en lecture seule.
    ''' </summary>
    ''' <remarks>En lecture seule.</remarks>
    Public ReadOnly Property OperationsRoles() As List(Of TsCdOperationRole)
        Get
            If mLectureSeule Then
                Return New List(Of TsCdOperationRole)(mOperationsRoles)
            Else
                Return mOperationsRoles
            End If
        End Get
    End Property

    ''' <summary>
    ''' Retourne une liste d'assignation de rôles de la demande courante.
    ''' </summary>
    ''' <remarks></remarks>
    <XmlIgnore()>
    Public ReadOnly Property Roles() As List(Of TsCdAssignationRole)
        Get
            Dim lesRoles As New List(Of TsCdAssignationRole)(RolesOriginaux)
            For Each op As TsCdOperationRole In OperationsRoles
                Select Case op.Operation
                    Case TsCdOperationRole.TsSgaOperation.Ajout
                        If ChercherRole(op.IdRole, lesRoles) Is Nothing Then
                            lesRoles.Add(TsCdAssignationRole.ConvertirAssignation(op))
                        Else
                            Throw New TsExcErreurGeneral("Incohérence des rôles lors d'une opération d'ajout.")
                        End If
                    Case TsCdOperationRole.TsSgaOperation.Modification
                        Dim r As TsCdAssignationRole = ChercherRole(op.IdRole, lesRoles)
                        If r IsNot Nothing Then
                            lesRoles.Remove(r)
                            lesRoles.Add(TsCdAssignationRole.ConvertirAssignation(op))
                        Else
                            Throw New TsExcErreurGeneral("Incohérence des rôles lors d'une opération de modification.")
                        End If
                    Case TsCdOperationRole.TsSgaOperation.Suppression
                        Dim r As TsCdAssignationRole = ChercherRole(op.IdRole, lesRoles)
                        If r IsNot Nothing Then
                            lesRoles.Remove(r)
                        Else
                            Throw New TsExcErreurGeneral("Incohérence des rôles lors d'une opération de suppression.")
                        End If
                    Case Else
                        Throw New TsExcErreurGeneral("Type d'opération sur les rôles inconnue.")
                End Select
            Next
            Return lesRoles
        End Get
    End Property

    ''' <summary>
    ''' Le mode de l'objet.
    ''' </summary>
    ''' <remarks>En lecture seule.</remarks>
    Property Mode() As TsDCMode
        Get
            Return mMode
        End Get
        Set(ByVal value As TsDCMode)
            If mLectureSeule = True Then
                Throw New TsExcErreurGeneral("L'élément est lecture seul. Vous ne pouvez l'accéder en écriture")
            Else
                mMode = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Nom du demandeur de la demande de création/modification.
    ''' </summary>
    ''' <remarks></remarks>
    Public Property NomDemandeur As String

    Public Property PieceJointe As Rrq.Securite.GestionAcces.TsCuFichierPieceJointe
        Get
            Return mPieceJointe
        End Get
        Set(ByVal value As Rrq.Securite.GestionAcces.TsCuFichierPieceJointe)
            'If mLectureSeule = True Then
            '    Throw New TsExcErreurGeneral("L'élément est lecture seul. Vous ne pouvez l'accéder en écriture")
            'Else
            mPieceJointe = value
            'End If
        End Set
    End Property

    Public Property Organisation As String

    Public Property ModifComptesSupp As String
    Public Property ValidComptesSupp As String

#End Region

#Region "Constructeurs"

    ''' <summary>
    ''' Constructeur pour le mode création de nouvel utilisateur.
    ''' </summary>
    Public Sub New()
        mUtilisateur = Nothing
        mRolesOriginaux = New List(Of TsCdAssignationRole)
        mUtilisateurOriginal = Nothing
        mOperationsRoles = New List(Of TsCdOperationRole)
        mLectureSeule = False

        mMode = TsDCMode.Creation
    End Sub

    ''' <summary>
    ''' Constructeur pour le mode modification d'utilisateur. 
    ''' </summary>
    ''' <remarks>Il doit être déja existent dans sage, sinon une exception est générée.</remarks>
    Public Sub New(ByVal pIDUtilisateur As String)
        '! Créer les objets de base de l'instance.
        mOperationsRoles = New List(Of TsCdOperationRole)

        '! Assignation d'information d'entrée.
        mIDUtilisateur = pIDUtilisateur

        '! Définition des variables locaux.
        Dim utilisateurSage As TsCdSageUser

        '! Collect des informations de Sage.
        utilisateurSage = TsCuAccesSage.ObtenirUtilisateur(IDUtilisateur)

        '! L'usager existe dans Sage? Non, Lancé Exception. 
        If utilisateurSage Is Nothing Then Throw New TsExcUtilisateurInexistant()

        '! Enregistrer les inforamtions d'origines de l'usager en modification
        mUtilisateurOriginal = TsCdUtilisateur.TraductionUtilisateur(utilisateurSage)
        mUtilisateur = TsCdUtilisateur.TraductionUtilisateur(utilisateurSage)
        mUtilisateur.protection = TsCdUtilisateur.NiveauProtection.Partielle

        mRolesOriginaux = New List(Of TsCdAssignationRole)
        Dim ListeSageRole As TsCdSageRoleCollection = TsCuAccesSage.ObtenirRelationURo(pIDUtilisateur)
        For Each element As TsCdSageRole In ListeSageRole
            Dim tmpRole As TsCdAssignationRole = TsCdAssignationRole.TraductionAssignationRole(element)
            tmpRole.lectureSeule = True
            mRolesOriginaux.Add(tmpRole)
        Next

        mLectureSeule = True

        mMode = TsDCMode.Modification
    End Sub

#End Region

#Region "Méthodes"

    ''' <summary> 
    ''' Assigné un rôle à la demande de Création/Modification avec une date de fin.
    ''' </summary>
    ''' <remarks>Si le role est inexistant une exception sera produite.</remarks>
    Public Sub AjouterRole(ByVal idRole As String, ByVal DateFin As Date)
        If ChercherRole(idRole, Roles) Is Nothing Then
            ' On serait peut-être mieux avec une fonction qui converti directement de SageRole à Operation Role
            Dim sageRole As TsCdSageRole = TsCuAccesSage.ObtenirRole(idRole)
            If sageRole Is Nothing Then Throw New TsExcErreurGeneral("Rôle inexistant dans sage.")
            Dim op As TsCdOperationRole = TsCdOperationRole.ConversionAssignation(TsCdAssignationRole.TraductionAssignationRole(sageRole))
            op.lectureSeule = False
            op.Operation = TsCdOperationRole.TsSgaOperation.Ajout
            op.FinPrevue = True
            op.DateFin = DateFin
            op.lectureSeule = True
            mOperationsRoles.Add(op)
        Else
            Throw New TsExcErreurGeneral("Le rôle est déja assigneé à cette utilisateur.")
        End If
    End Sub

    ''' <summary>
    ''' Assigné un rôle à la demande de Création/Modification sans date de fin.
    ''' </summary>
    ''' <remarks>Si le role est inexistant une exception sera produite.</remarks>
    Public Sub AjouterRole(ByVal idRole As String)
        If ChercherRole(idRole, Roles) Is Nothing Then
            '! On serait peut-être mieux avec une fonction qui converti directement de SageRole à Operation Role
            Dim sageRole As TsCdSageRole = TsCuAccesSage.ObtenirRole(idRole)
            If sageRole Is Nothing Then Throw New TsExcErreurGeneral("Rôle inexistant dans sage.")
            Dim op As TsCdOperationRole = TsCdOperationRole.ConversionAssignation(TsCdAssignationRole.TraductionAssignationRole(sageRole))
            op.lectureSeule = False
            op.Operation = TsCdOperationRole.TsSgaOperation.Ajout
            op.FinPrevue = False
            op.lectureSeule = True
            mOperationsRoles.Add(op)
        Else
            Throw New TsExcErreurGeneral("Le rôle est déja assigné à cette utilisateur.")
        End If
    End Sub

    ''' <summary>
    ''' Modifie l'assigantion d'un rôle déja assigné. Lui affect une date de fin.
    ''' </summary>
    Public Sub ModifierRole(ByVal idRole As String, ByVal DateFin As Date)
        If ChercherRole(idRole, Roles) IsNot Nothing Then

            ' On serait peut-être mieux avec une fonction qui converti directement de SageRole à Operation Role
            Dim sageRole As TsCdSageRole = TsCuAccesSage.ObtenirRole(idRole)
            Dim op As TsCdOperationRole = TsCdOperationRole.ConversionAssignation(TsCdAssignationRole.TraductionAssignationRole(sageRole))
            op.lectureSeule = False
            op.Operation = TsCdOperationRole.TsSgaOperation.Modification
            op.FinPrevue = True
            op.DateFin = DateFin
            op.lectureSeule = True
            mOperationsRoles.Add(op)
        Else
            Throw New TsExcErreurGeneral("Modification d'un role non-assigné impossible.")
        End If
    End Sub

    ''' <summary>
    ''' Modifie l'assigantion d'un rôle déja assigné. Enlève une date de fin. 
    ''' </summary>
    ''' <param name="FinPrevue">Si la valeur n'est pas False la méthode lancera un exception.</param>
    Public Sub ModifierRole(ByVal idRole As String, ByVal finPrevue As Boolean)
        If finPrevue = True Then
            ' Il faudrait spécifier qu'il faut passer une date quand il y a une fin de prévue
            Throw New TsExcErreurDemandeModification()
        End If
        If ChercherRole(idRole, Roles) IsNot Nothing Then
            ' On serait peut-être mieux avec une fonction qui converti directement de SageRole à Operation Role
            Dim sageRole As TsCdSageRole = TsCuAccesSage.ObtenirRole(idRole)
            Dim op As TsCdOperationRole = TsCdOperationRole.ConversionAssignation(TsCdAssignationRole.TraductionAssignationRole(sageRole))
            op.lectureSeule = False
            op.Operation = TsCdOperationRole.TsSgaOperation.Modification
            op.FinPrevue = False
            op.lectureSeule = True
            mOperationsRoles.Add(op)
        Else
            Throw New TsExcErreurGeneral("Modification d'un role non-assigné impossible.")
        End If
    End Sub

    ''' <summary>
    ''' Retire un rôle déja assigné.
    ''' </summary>
    Public Sub RetirerRole(ByVal idRole As String)
        If ChercherRole(idRole, Roles) IsNot Nothing Then
            ' On serait peut-être mieux avec une fonction qui converti directement de SageRole à Operation Role
            Dim op As TsCdOperationRole = TsCdOperationRole.ConversionAssignation(TsCdAssignationRole.TraductionAssignationRole(TsCuAccesSage.ObtenirRole(idRole)))
            op.lectureSeule = False
            op.Operation = TsCdOperationRole.TsSgaOperation.Suppression
            op.lectureSeule = True
            mOperationsRoles.Add(op)
        Else
            ' Est-ce qu'on a une exception plus spécifique pour ça?
            Throw New TsExcErreurGeneral("Suppression d'un role non-assigné impossible.")
        End If
    End Sub

    ''' <summary>
    ''' Cette fucntion va simplifier la liste d'opération en enlevant les doublons et les opérations inutiles.
    ''' </summary>
    ''' <remarks>Elle est utiliser dans la sérialisation.</remarks>
    Friend Sub SimplifierOperations()
        Dim nouvelUtilisateur As Boolean
        If mIDUtilisateur = "" Then
            nouvelUtilisateur = True
        Else
            nouvelUtilisateur = False
        End If

        Dim dictionnaire As New Dictionary(Of String, TsCdOperationRole)

        For Each op As TsCdOperationRole In mOperationsRoles
            Dim tmpOpRole As TsCdOperationRole
            Dim tmpLectureSeule As Boolean
            Select Case nouvelUtilisateur '! Action à prendre si celui-çi est un nouvel utilisateur ou pas.
                Case True
                    Select Case op.Operation '! Action à prendre dépendant sur le type d'opération.
                        Case TsCdOperationRole.TsSgaOperation.Ajout
                            ajouterDictio(dictionnaire, op.Clone)
                        Case TsCdOperationRole.TsSgaOperation.Modification
                            tmpOpRole = op.Clone
                            tmpLectureSeule = tmpOpRole.lectureSeule
                            tmpOpRole.lectureSeule = False
                            tmpOpRole.Operation = TsCdOperationRole.TsSgaOperation.Ajout
                            tmpOpRole.lectureSeule = tmpLectureSeule
                            ajouterDictio(dictionnaire, tmpOpRole)
                        Case TsCdOperationRole.TsSgaOperation.Suppression
                            dictionnaire.Remove(op.IdRole)
                    End Select
                Case False
                    Select Case op.Operation '! Action à prendre dépendant sur le type d'opération.
                        Case TsCdOperationRole.TsSgaOperation.Ajout
                            If estRoleOriginal(op.IdRole) Then
                                tmpOpRole = op.Clone
                                tmpLectureSeule = tmpOpRole.lectureSeule
                                tmpOpRole.lectureSeule = False
                                tmpOpRole.Operation = TsCdOperationRole.TsSgaOperation.Modification
                                tmpOpRole.lectureSeule = tmpLectureSeule
                                ajouterDictio(dictionnaire, tmpOpRole)
                            Else
                                ajouterDictio(dictionnaire, op)
                            End If
                        Case TsCdOperationRole.TsSgaOperation.Modification
                            If estRoleOriginal(op.IdRole) Then
                                ajouterDictio(dictionnaire, op)
                            Else
                                tmpOpRole = op.Clone
                                tmpLectureSeule = tmpOpRole.lectureSeule
                                tmpOpRole.lectureSeule = False
                                tmpOpRole.Operation = TsCdOperationRole.TsSgaOperation.Ajout
                                tmpOpRole.lectureSeule = tmpLectureSeule
                                ajouterDictio(dictionnaire, tmpOpRole)
                            End If
                        Case TsCdOperationRole.TsSgaOperation.Suppression
                            If estRoleOriginal(op.IdRole) Then
                                ajouterDictio(dictionnaire, op.Clone)
                            Else
                                dictionnaire.Remove(op.IdRole)
                            End If
                    End Select
            End Select
        Next

        mOperationsRoles = New List(Of TsCdOperationRole)(dictionnaire.Values)
    End Sub
#End Region

#Region "Fonctions de services"

    ''' <summary>
    ''' Recherche le rôle correspondant à l'identifiant dans la liste d'entrée.
    ''' </summary>
    Private Shared Function ChercherRole(ByVal id As String, ByVal liste As List(Of TsCdAssignationRole)) As TsCdAssignationRole
        For Each r As TsCdAssignationRole In liste
            If r.ID = id Then
                Return r
            End If
        Next
        Return Nothing
    End Function

    ''' <summary>
    ''' Ajoute l'opération dans le dictionnaire, s'assure de ne pas d'ajouté un role déja existant.
    ''' </summary>
    Private Shared Sub ajouterDictio(ByRef dictio As Dictionary(Of String, TsCdOperationRole), ByVal opRole As TsCdOperationRole)
        If dictio.ContainsKey(opRole.IdRole) = True Then
            dictio.Remove(opRole.IdRole)
            dictio.Add(opRole.IdRole, opRole)
        Else
            dictio.Add(opRole.IdRole, opRole)
        End If
    End Sub

    ''' <summary>
    ''' Vérifie si le role est un rôle original de l'utilisateur.
    ''' </summary>
    ''' <param name="idRole"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function estRoleOriginal(ByVal idRole As String) As Boolean
        For Each role As TsCdAssignationRole In mRolesOriginaux
            If idRole = role.ID Then Return True
        Next

        Return False
    End Function

#End Region


End Class