Imports System.Text

''' <summary>
''' Classe utilitaire pour obtenir le prochain nom de cle
''' </summary>
''' <remarks></remarks>
Public Class TsCuCle
    Private mSysteme As String = String.Empty
    Private mSousSysteme As String = String.Empty
    Private mConnection As String = String.Empty
    Private mSequence As Integer
    Private mGroupeAd As String = String.Empty
    Private mMdp As String = String.Empty
    Private mCodeUsagerAd As String = String.Empty
    Private mCodeEnv As String
    Private mDescEnv As String
    
    ''' <summary>
    ''' Description Environnement
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DescEnv() As String
        Get
            Return mDescEnv
        End Get
        Set(ByVal value As String)
            mDescEnv = value
        End Set
    End Property


    ''' <summary>
    ''' Code Environnement
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CodeEnv() As String
        Get
            Return mCodeEnv
        End Get
        Set(ByVal value As String)
            mCodeEnv = value
        End Set
    End Property

    ''' <summary>
    ''' Code Usager Ad 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Code de l'écran</remarks>
    Public Property CodeUsagerAd() As String
        Get
            Return mCodeUsagerAd
        End Get
        Set(ByVal value As String)
            mCodeUsagerAd = value
        End Set
    End Property

    ''' <summary>
    ''' Mot de passe
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Mdp() As String
        Get
            Return mMdp
        End Get
        Set(ByVal value As String)
            mMdp = value
        End Set
    End Property

    ''' <summary>
    ''' Groupe Ad
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Profil de l'écran</remarks>
    Public Property GroupeAd() As String
        Get
            Return mGroupeAd
        End Get
        Set(ByVal value As String)
            mGroupeAd = value
        End Set
    End Property

    ''' <summary>
    ''' Séquence 
    ''' </summary>
    ''' <value>Numéro de séquence pour des clés qui ont le même système et sous-système</value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Sequence() As Integer
        Get
            Return mSequence
        End Get
        Set(ByVal value As Integer)
            mSequence = value
        End Set
    End Property

    ''' <summary>
    ''' Connection
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Connection dans la clé</remarks>
    Public Property Connection() As String
        Get
            Return mConnection
        End Get
        Set(ByVal value As String)
            mConnection = value
        End Set
    End Property

    ''' <summary>
    ''' Sous-Système
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SousSysteme() As String
        Get
            Return mSousSysteme
        End Get
        Set(ByVal value As String)
            mSousSysteme = value
        End Set
    End Property

    ''' <summary>
    ''' Système
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Systeme() As String
        Get
            Return mSysteme
        End Get
        Set(ByVal value As String)
            mSysteme = value
        End Set
    End Property

    ''' <summary>
    ''' Obtenir l'identifiant de la clé
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function ToString() As String
        Dim sb As New stringbuilder

        sb.Append(Systeme)
        sb.Append(SousSysteme)
        sb.Append(Connection)
        If Sequence > 0 Then sb.Append(Sequence.ToString("D2"))

        Return sb.ToString
    End Function
End Class
