Imports System.Xml.Serialization

''' <summary>
''' Classe de données. 
''' Sert à enrgistrer les informations sur un utilisateur à effacer.
''' </summary>
''' <remarks></remarks>
<XmlType()>
Public Class TsCdDemandeDestruction

#Region "Private var"

    Private mIDUtilisateur As String
    Friend LectureSeule As Boolean

#End Region

#Region "Property"

    ''' <summary>
    ''' L'identifiant de l'utilisateur. Lecture seule.
    ''' </summary>
    ''' <remarks>Lecture seule.</remarks>
    Public Property IDUtilisateur() As String
        Get
            Return mIDUtilisateur
        End Get
        Set(ByVal value As String)
            If LectureSeule = True Then Throw New TsExcErreurGeneral("L'élément est en lecture seule, Vous ne pouvez la modifier.")
            mIDUtilisateur = value
        End Set
    End Property

    ''' <summary>
    ''' Nom du demandeur de la demande de destruction.
    ''' </summary>
    ''' <remarks></remarks>
    Public Property NomDemandeur As String

#End Region

#Region "Constructeurs"

    ''' <summary>
    ''' Constructeur. 
    ''' Entrée un identifiant d'utilisateur pour créer un objet valide.
    ''' </summary>
    Public Sub New(ByVal idUtilisateur As String)
        ' On serait peut-être mieux avec une fonction qui converti directement de SageRole à Operation Role
        Dim sageUtilisateur As TsCdSageUser = TsCuAccesSage.ObtenirUtilisateur(idUtilisateur)
        If sageUtilisateur Is Nothing Then Throw New TsExcErreurGeneral("Utilisateur inexistant dans sage.")
        mIDUtilisateur = idUtilisateur
        LectureSeule = True
    End Sub

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        LectureSeule = False
        mIDUtilisateur = ""
    End Sub

#End Region

End Class
