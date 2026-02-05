''' <summary>
''' Classe de données. Une équipe de travail.
''' </summary>
''' <remarks>
''' Cette classe possède des niveaux d'écritures.
''' Si les données de cette objet proviennent du serveur Sage, ils seront totalements barrées.
''' </remarks>
Public Class TsCdEquipe

#Region "CONSTANTE"

#End Region

#Region "Private Vars"

    Private mIDRole As String
    Private mNom As String
    Private mNoUniteAdmin As String

    Friend LectureSeule As Boolean

#End Region

#Region "Property"


    ''' <summary>
    ''' L'identifiant du rôle. Lecture seule.
    ''' </summary>
    ''' <remarks>Données en lecture seule.</remarks>
    Public Property IDRole() As String
        Get
            Return mIDRole
        End Get
        Set(ByVal nouvelValeur As String)
            If LectureSeule = True Then
                Throw New TsExcErreurGeneral("L'élément est en Lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mIDRole = nouvelValeur
            End If
        End Set
    End Property

    ''' <summary>
    ''' Le nom de l'équpie. Lecture seule.
    ''' </summary>
    ''' <remarks>Données en lecture seule.</remarks>
    Public Property Nom() As String
        Get
            Return mNom
        End Get
        Set(ByVal nouvelValeur As String)
            If LectureSeule = True Then
                Throw New TsExcErreurGeneral("L'élément est en Lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mNom = nouvelValeur
            End If
        End Set
    End Property

    ''' <summary>
    ''' L'unité administrative à lequel est lier cette équipe.
    ''' </summary>
    ''' <remarks>Données en lecture seule.</remarks>
    Public Property NoUniteAdmin() As String
        Get
            Return mNoUniteAdmin
        End Get
        Set(ByVal value As String)
            If LectureSeule = True Then
                Throw New TsExcErreurGeneral("L'élément est en Lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mNoUniteAdmin = value
            End If
        End Set
    End Property



#End Region

#Region "Constructeur"

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        LectureSeule = False
    End Sub

#End Region

#Region "Méthodes"

    ''' <summary>
    ''' Retourne une version string des informations de l'instance.
    ''' </summary>
    Public Overrides Function ToString() As String
        Dim _sRet As String

        _sRet = "-----------------" + vbCrLf
        _sRet = _sRet + "Numéro de l'équipe: " & IDRole & vbCrLf
        _sRet = _sRet + "Nom de l'équipe: " & Nom & vbCrLf
        _sRet = _sRet + "Nom de l'Unité Administrative qui le regroupe: " & Nom & vbCrLf
        _sRet = _sRet + "-----------------" + vbCrLf

        Return _sRet
    End Function

#End Region

#Region "Fonctions de services"

    ''' <summary>
    ''' Transforme les informations d'une équipe du module sage en équipe du module web.
    ''' </summary>
    Protected Friend Shared Function TraductionEquipe(ByVal sageRole As TsCdSageRole) As TsCdEquipe
        If sageRole.Type <> TsCuAccesSage.TYPEROLE_REO_E Then Throw New TsExcErreurGeneral("Le role n'est pas une équipe.")

        Dim paramRetour As TsCdEquipe = New TsCdEquipe()

        paramRetour.Nom = sageRole.Rule
        paramRetour.IDRole = sageRole.Name

        '! --------------------------
        '! Appeler sage pour définir quel unité administrative

        Dim lstRoles As TsCdSageRoleCollection = TsCuAccesSage.ObtenirRelationRoRoEnfant(paramRetour.IDRole)
        If lstRoles.Roles.Count <> 1 Then
            Throw New TsExcErreurGeneral("La configuration dans sage est incohérente. Impossible d'avoir plus d'une unité administrative affectée l'équpie à la fois.")
        End If
        Dim listeSeparateur() As Char = {"_"c}
        paramRetour.NoUniteAdmin = lstRoles.Roles(0).Name.Split(listeSeparateur)(1)

        '! Fin de la recherche d'unité administrative
        '! --------------------------

        paramRetour.LectureSeule = True

        Return paramRetour
    End Function

#End Region

End Class
