
''' <summary>
''' Définition de la source du Rapport.
''' </summary>
''' <remarks></remarks>
Friend Class TsDtSourceRapport

#Region "--- Propriétés ---"

    Private mEmployes As New List(Of TsDtSourceEmploye)
    ''' <summary>
    ''' Liste des employés sélectionnés.
    ''' </summary>
    Public Property Employes() As List(Of TsDtSourceEmploye)
        Get
            Return mEmployes
        End Get
        Set(ByVal value As List(Of TsDtSourceEmploye))
            mEmployes = value
        End Set
    End Property

    Private mLstRessources As New List(Of TsDtSourceRessources)

    Public Property LstRessources As List(Of TsDtSourceRessources)
        Get
            Return mLstRessources
        End Get
        Set(value As List(Of TsDtSourceRessources))
            mLstRessources = value
        End Set
    End Property


    Private mdctUtilisateursParUA As Dictionary(Of String, Integer)

    Public Property dctUtilisateursParUA As Dictionary(Of String, Integer)
        Get
            Return mdctUtilisateursParUA
        End Get
        Set(value As Dictionary(Of String, Integer))
            mdctUtilisateursParUA = value
        End Set

    End Property


    Private mLstRoleUaDemander As List(Of String)
    ''' <summary>
    ''' Liste des unités administratives demandé par l'appelant.
    ''' </summary>
    Public Property LstUaDemander() As List(Of String)
        Get
            Return mLstRoleUaDemander
        End Get
        Set(ByVal value As List(Of String))
            mLstRoleUaDemander = value
        End Set
    End Property

    ''' <summary>
    ''' Date de production.
    ''' </summary>
    ''' <remarks></remarks>
    Public Property DateProduction As Date


#End Region

#Region "--- Constructeurs ---"

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <param name="pDate">Date de production.</param>
    ''' <param name="pLstUaDemander">Liste des unitées administrative demandées.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal pDate As Date, ByVal pLstUaDemander As List(Of String))
        mLstRoleUaDemander = pLstUaDemander

        DateProduction = pDate
    End Sub

#End Region

#Region "--- Méthodes ---"


    ''' <summary>
    ''' Permet d'obtenir tous les unités administratives de tout les employés.
    ''' </summary>
    ''' <returns>Liste d'unité administrative.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirTousNoUA() As List(Of Integer)
        Return (From e In Employes _
                Select e.NoUA _
                Distinct).ToList
    End Function

#End Region

End Class
