Imports Rrq.Securite.GestionAcces

''' <summary>
''' Classe de Données. Un utilisateur.
''' Referme les informations sur l'utilisateur
''' </summary>
''' <remarks>
''' Cette classe possède des niveaux d'écritures.
''' Dépendament de sa situation, il peut etre libre de modification, partiellement barré ou totalement barré.
''' </remarks>
Public Class TsCdUtilisateur
#Region "Enums"
    Friend Enum NiveauProtection
        Aucune
        Partielle
        Totale
    End Enum
#End Region

#Region "Private Vars"

    Private mID As String
    Private mCodeUtilisateur As String

    Private mPrenom As String
    Private mNom As String
    Private mNomComplet As String
    Private mCourriel As String
    Private mNoUniteAdmin As String
    Private mDateFin As Date
    Private mVille As String
    Private mFinPrevue As Boolean
    Private mDateApprobation As Date
    Private mApprobationAccepter As Boolean
    Private mOrganisation As String
    Private mComptesSupplementaires As TsCdComptesSupplementaires

    Friend protection As NiveauProtection

#End Region

#Region "Property"

    ''' <summary>
    ''' L'identifiant de l'utilisateur.
    ''' </summary>
    Public Property ID() As String
        Get
            Return mID
        End Get
        Set(ByVal value As String)
            If protection = NiveauProtection.Totale Or protection = NiveauProtection.Partielle Then
                Throw New TsExcErreurGeneral("L'élément est en Lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mID = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Le prenom de l'utilisateur.
    ''' </summary>
    Public Property Prenom() As String
        Get
            Return mPrenom
        End Get
        Set(ByVal value As String)
            If protection = NiveauProtection.Totale Or protection = NiveauProtection.Partielle Then
                Throw New TsExcErreurGeneral("L'élément est en Lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mPrenom = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Le nom de l'utilisateur.
    ''' </summary>
    Public Property Nom() As String
        Get
            Return mNom
        End Get
        Set(ByVal value As String)
            If protection = NiveauProtection.Totale Or protection = NiveauProtection.Partielle Then
                Throw New TsExcErreurGeneral("L'élément est en Lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mNom = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Le nom complet de l'utilisateur.
    ''' </summary>
    Public Property NomComplet() As String
        Get
            Return mNomComplet
        End Get
        Set(ByVal value As String)
            If protection = NiveauProtection.Totale Then
                Throw New TsExcErreurGeneral("L'élément est en Lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mNomComplet = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Le courriel de l'utilisateur.
    ''' </summary>
    Public Property Courriel() As String
        Get
            Return mCourriel
        End Get
        Set(ByVal value As String)
            If protection = NiveauProtection.Totale Or protection = NiveauProtection.Partielle Then
                Throw New TsExcErreurGeneral("L'élément est en Lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mCourriel = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Le numéro de l'unité Administrative de l'utilisateur.
    ''' </summary>
    Public Property NoUniteAdmin() As String
        Get
            Return mNoUniteAdmin
        End Get
        Set(ByVal value As String)
            If protection = NiveauProtection.Totale Then
                Throw New TsExcErreurGeneral("L'élément est en Lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mNoUniteAdmin = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' La date de fin où l'utilisateur devient désactivé.
    ''' </summary>
    Public Property DateFin() As Date
        Get
            Return mDateFin
        End Get
        Set(ByVal value As Date)
            If protection = NiveauProtection.Totale Then
                Throw New TsExcErreurGeneral("L'élément est en Lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                If value = Nothing Then
                    mDateFin = value
                    mFinPrevue = False
                Else
                    mDateFin = value
                    mFinPrevue = True
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' La ville de l'utilisateur.
    ''' </summary>
    Public Property Ville() As String
        Get
            Return mVille
        End Get
        Set(ByVal value As String)
            If protection = NiveauProtection.Totale Or protection = NiveauProtection.Partielle Then
                Throw New TsExcErreurGeneral("L'élément est en Lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mVille = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' L'utilisateur a-t'il une date désactivation ?
    ''' </summary>
    Public Property FinPrevue() As Boolean
        Get
            Return mFinPrevue
        End Get
        Set(ByVal value As Boolean)
            If protection = NiveauProtection.Totale Then
                Throw New TsExcErreurGeneral("L'élément est en Lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mFinPrevue = value
                If value = False Then
                    mDateFin = Nothing
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' L'utilisateur a-t'il une date désactivation ?
    ''' </summary>
    Public Property ApprobationAccepter() As Boolean
        Get
            Return mApprobationAccepter
        End Get
        Set(ByVal value As Boolean)
            If protection = NiveauProtection.Totale Then
                Throw New TsExcErreurGeneral("L'élément est en Lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mApprobationAccepter = value
                If value = False Then
                    mDateApprobation = Nothing
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' La date où l'utilisateur est approuvé.
    ''' </summary>
    Public Property DateApprobation() As Date
        Get
            Return mDateApprobation
        End Get
        Set(ByVal value As Date)
            If protection = NiveauProtection.Totale Then
                Throw New TsExcErreurGeneral("L'élément est en Lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                If value = Nothing Then
                    mDateApprobation = value
                    mApprobationAccepter = False
                Else
                    mDateApprobation = value
                    mApprobationAccepter = True
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' L'organisation de l'utilisateur.
    ''' </summary>
    Public Property Organisation() As String
        Get
            Return mOrganisation
        End Get
        Set(ByVal value As String)
            If protection = NiveauProtection.Totale Or protection = NiveauProtection.Partielle Then
                Throw New TsExcErreurGeneral("L'élément est en Lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mOrganisation = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Les comptes supplementaires de l'utilisateur.
    ''' </summary>
    Public Property ComptesSupplementaires() As TsCdComptesSupplementaires
        Get
            Return mComptesSupplementaires
        End Get
        Set(ByVal value As TsCdComptesSupplementaires)
            If protection = NiveauProtection.Totale Or protection = NiveauProtection.Partielle Then
                Throw New TsExcErreurGeneral("L'élément est en Lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mComptesSupplementaires = value
            End If
        End Set
    End Property

#End Region

#Region "Constructeur"

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    Public Sub New()
        protection = NiveauProtection.Aucune
    End Sub

#End Region

#Region "Méthodes"

    ''' <summary>
    ''' Écrit une verstion string des données contenus dans l'instance.
    ''' </summary>
    Public Overrides Function ToString() As String
        Dim ParamRetour As String

        ParamRetour = "-----------------" + vbCrLf
        ParamRetour = ParamRetour + "Id de l'utilisateur: " & ID & vbCrLf
        ParamRetour = ParamRetour + "Nom complet de l'utilisateur: " & NomComplet & vbCrLf
        ParamRetour = ParamRetour + "Nom de famille de l'utilisateur: " & Nom & vbCrLf
        ParamRetour = ParamRetour + "Prenom de l'utilisateur: " & Prenom & vbCrLf
        ParamRetour = ParamRetour + "Courriel de l'utilisateur: " & Courriel & vbCrLf
        ParamRetour = ParamRetour + "Numéro de l'utilisateur administrateur: " & NoUniteAdmin & vbCrLf
        ParamRetour = ParamRetour + "Ville de l'utilisateur: " & Ville & vbCrLf
        ParamRetour = ParamRetour + "Date de fin du compte de l'utilisateur: " & DateFin & vbCrLf
        ParamRetour = ParamRetour + "Date de fin du compte de l'utilisateur: " & DateFin & vbCrLf
        ParamRetour = ParamRetour + "-----------------" + vbCrLf

        Return ParamRetour
    End Function

#End Region

#Region "Fonctions de services"

    ''' <summary>
    ''' Transformes les informations d'un utilisateur du module sage en utilisatuer module du module web. 
    ''' </summary>
    Friend Shared Function TraductionUtilisateur(ByVal sageUtilisateur As TsCdSageUser) As TsCdUtilisateur
        Dim paramRetour As TsCdUtilisateur = New TsCdUtilisateur()

        paramRetour.ID = sageUtilisateur.PersonID
        paramRetour.NoUniteAdmin = sageUtilisateur.OrganizationType

        paramRetour.Nom = sageUtilisateur.Nom
        paramRetour.Prenom = sageUtilisateur.Prenom
        paramRetour.NomComplet = sageUtilisateur.UserName
        paramRetour.Courriel = sageUtilisateur.Courriel
        paramRetour.Ville = sageUtilisateur.Ville
        paramRetour.Organisation = sageUtilisateur.Organization
        paramRetour.ComptesSupplementaires = sageUtilisateur.ConvertirEnComptesSupplementaires

        paramRetour.DateFin = sageUtilisateur.DateFin
        If sageUtilisateur.DateFin = Nothing Then
            paramRetour.FinPrevue = False
        Else
            paramRetour.FinPrevue = True
        End If

        paramRetour.DateApprobation = sageUtilisateur.DateApprobation
        If sageUtilisateur.DateApprobation = Nothing Then
            paramRetour.ApprobationAccepter = False
        Else
            paramRetour.ApprobationAccepter = True
        End If
        paramRetour.protection = NiveauProtection.Totale

        Return paramRetour
    End Function

#End Region

#Region "Operateurs"
    ''' <summary>
    ''' Comparateur d'objets. Compare deux objets de même nature, s'ils sont pareils.
    ''' </summary>
    Public Shared Operator =(ByVal premierUtilisateur As TsCdUtilisateur, ByVal deuxiemeUtilisateur As TsCdUtilisateur) As Boolean

        If premierUtilisateur.Courriel <> deuxiemeUtilisateur.Courriel Then Return False
        If premierUtilisateur.DateFin <> deuxiemeUtilisateur.DateFin Then Return False
        If premierUtilisateur.FinPrevue <> deuxiemeUtilisateur.FinPrevue Then Return False
        If premierUtilisateur.ID <> deuxiemeUtilisateur.ID Then Return False
        If premierUtilisateur.Nom <> deuxiemeUtilisateur.Nom Then Return False
        If premierUtilisateur.NoUniteAdmin <> deuxiemeUtilisateur.NoUniteAdmin Then Return False
        If premierUtilisateur.Prenom <> deuxiemeUtilisateur.Prenom Then Return False
        If premierUtilisateur.Ville <> deuxiemeUtilisateur.Ville Then Return False
        If premierUtilisateur.DateApprobation <> deuxiemeUtilisateur.DateApprobation Then Return False
        If premierUtilisateur.ApprobationAccepter <> deuxiemeUtilisateur.ApprobationAccepter Then Return False

        Return True
    End Operator

    ''' <summary>
    ''' Comparateur d'objets. Compare deux objets de même nature s'ils sont différents.
    ''' </summary>
    Public Shared Operator <>(ByVal premierUtilisateur As TsCdUtilisateur, ByVal deuxiemeUtilisateur As TsCdUtilisateur) As Boolean
        Return Not (premierUtilisateur = deuxiemeUtilisateur)
    End Operator
#End Region

End Class
