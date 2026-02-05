''' <summary>
''' Classe de Données. Une unité Administrative.
''' Referme les informations d'une unité administrative.
''' </summary>
''' <remarks>
''' Cette classe possède des niveaux d'écritures.
''' Si les données de cette objet proviennent du serveur Sage, ils seront totalements barrées.
''' </remarks>
Public Class TsCdUniteAdministrative

#Region "CONSTANTE"

#End Region

#Region "Private Vars"

    Private mNo As String
    Private mIDRole As String
    Private mNom As String
    Private mAbbreviation As String

    Friend lectureSeule As Boolean

#End Region

#Region "Property"

    ''' <summary>
    ''' Le numéro de l'unité administrative. Lecture seule.
    ''' </summary>
    ''' <remarks>Donnée en lecture seule.</remarks>
    Public Property No() As String
        Get
            Return mNo
        End Get
        Set(ByVal nouvelValeur As String)
            If lectureSeule = True Then
                Throw New TsExcErreurGeneral("L'élément est en Lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mNo = nouvelValeur
            End If
        End Set
    End Property

    ''' <summary>
    ''' L'indentifiant du rôle. Lecture seule.
    ''' </summary>
    ''' <remarks>Donnée en lecture seule.</remarks>
    Public Property IDRole() As String
        Get
            Return mIDRole
        End Get
        Set(ByVal nouvelValeur As String)
            If lectureSeule = True Then
                Throw New TsExcErreurGeneral("L'élément est en Lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mIDRole = nouvelValeur
            End If
        End Set
    End Property

    ''' <summary>
    ''' Le nom de l'unité administrative. Lecture seule.
    ''' </summary>
    ''' <remarks>Donnée en lecture seule.</remarks>
    Public Property Nom() As String
        Get
            Return mNom
        End Get
        Set(ByVal nouvelValeur As String)
            If lectureSeule = True Then
                Throw New TsExcErreurGeneral("L'élément est en Lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mNom = nouvelValeur
            End If
        End Set
    End Property

    ''' <summary>
    ''' L'abbréviation de l'unité administrative. Lecture seule.
    ''' </summary>
    ''' <remarks>Donnée en lecture seule.</remarks>
    Public Property Abbreviation() As String
        Get
            Return mAbbreviation
        End Get
        Set(ByVal nouvelValeur As String)
            If lectureSeule = True Then
                Throw New TsExcErreurGeneral("L'élément est en Lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mAbbreviation = nouvelValeur
            End If
        End Set
    End Property

#End Region

#Region "Constructeur"

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    Public Sub New()
        lectureSeule = False
    End Sub

#End Region

#Region "Méthodes"

    ''' <summary>
    ''' Retourne une version string des informations de l'instance.
    ''' </summary>
    Public Overrides Function ToString() As String
        Dim ParamRetour As String

        ParamRetour = "-----------------" + vbCrLf
        ParamRetour = ParamRetour + "Numéro de l'Unité Administrative: " & No & vbCrLf
        ParamRetour = ParamRetour + "Nom de l'Unité Administrative: " & Nom & vbCrLf
        ParamRetour = ParamRetour + "Abbréviation de l'Unité Administrative: " & Abbreviation & vbCrLf
        ParamRetour = ParamRetour + "-----------------" + vbCrLf

        Return ParamRetour
    End Function

#End Region

#Region "Fonctions de services"

    ''' <summary>
    ''' Transforme les informations d'un unité administrative du module sage en unité administrative Module web.
    ''' </summary>
    Friend Shared Function TraductionUnitAdmin(ByVal sageRole As TsCdSageRole) As TsCdUniteAdministrative
        If sageRole.Type <> TsCuAccesSage.TYPEROLE_REO Then Throw New TsExcErreurGeneral("Le role n'est pas une unité administrative.")

        Dim ParamRetour As TsCdUniteAdministrative = New TsCdUniteAdministrative()
        Dim listeSeparateur() As Char = {"_"c}

        ParamRetour.mNom = sageRole.Rule
        ParamRetour.mNo = sageRole.Name.Split(listeSeparateur)(1)
        ParamRetour.mIDRole = sageRole.Name
        ParamRetour.mAbbreviation = sageRole.Name.Split(listeSeparateur)(2)

        ParamRetour.lectureSeule = True

        Return ParamRetour
    End Function

#End Region

End Class
