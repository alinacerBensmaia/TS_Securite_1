
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

    Private mUaSelectionnes As New List(Of String)
    ''' <summary>
    ''' Liste des rôles d'unité administrative sélectionnées.
    ''' </summary>
    Public Property RoleUaSelectionnes() As List(Of String)
        Get
            Return mUaSelectionnes
        End Get
        Set(ByVal value As List(Of String))
            mUaSelectionnes = value
        End Set
    End Property

    Private mContextes As New List(Of TsDtSourceContexteUA)
    ''' <summary>
    ''' Liste qui contient les Contextes des rôle d'unité administrative disponibles.
    ''' </summary>
    Public Property Contextes() As List(Of TsDtSourceContexteUA)
        Get
            Return mContextes
        End Get
        Set(ByVal value As List(Of TsDtSourceContexteUA))
            mContextes = value
        End Set
    End Property

    Private mRoleUaDisponibles As New List(Of TsDtSourceUa)
    ''' <summary>
    ''' Liste des rôles d'unité administrative disponibles.
    ''' </summary>
    Public Property RoleUaDisponibles() As List(Of TsDtSourceUa)
        Get
            Return mRoleUaDisponibles
        End Get
        Set(ByVal value As List(Of TsDtSourceUa))
            mRoleUaDisponibles = value
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
    ''' Permet d'obtenir les métiers sélectionnés.
    ''' </summary>
    ''' <returns>Une liste d'unité administrative filtrée.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirMetiersSelectionnes() As List(Of TsDtSourceUa)
        Dim uas = RoleUaDisponibles()
        Dim uaMetier = uas.Where(Function(ua) ua.Type = TsDtSourceUa.TypeRoleUA.Metier Or ua.Type = TsDtSourceUa.TypeRoleUA.REO)

        Return (From ua In uaMetier _
                Where mUaSelectionnes.Any(Function(s) s = ua.Nom) _
                ).ToList
    End Function

    ''' <summary>
    ''' Permet d'obtenir les métiers non sélectionnés.
    ''' </summary>
    ''' <returns>Une liste d'unité administrative filtrée.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirMetiersAutre() As List(Of TsDtSourceUa)
        Dim uas = RoleUaDisponibles()
        Dim uaMetier = uas.Where(Function(ua) ua.Type = TsDtSourceUa.TypeRoleUA.Metier Or ua.Type = TsDtSourceUa.TypeRoleUA.REO)

        Return (From ua In uaMetier _
                Where mUaSelectionnes.All(Function(s) s <> ua.Nom) _
                ).ToList
    End Function

    ''' <summary>
    ''' Permet d'obtenir les tâches sélectionnés.
    ''' </summary>
    ''' <returns>Une liste d'unité administrative filtrée.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirRolesTachesSelectionnes() As List(Of TsDtSourceUa)
        Dim uas = RoleUaDisponibles()
        Dim uaMetier = uas.Where(Function(ua) ua.Type = TsDtSourceUa.TypeRoleUA.Tache)

        Return (From ua In uaMetier _
                Where mUaSelectionnes.Any(Function(s) s = ua.Nom) _
                ).ToList
    End Function

    ''' <summary>
    ''' Permet d'obtenir les tâches non sélectionnés.
    ''' </summary>
    ''' <returns>Une liste d'unité administrative filtrée.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirRolesTachesAutre() As List(Of TsDtSourceUa)
        Dim uas = RoleUaDisponibles()
        Dim uaMetier = uas.Where(Function(ua) ua.Type = TsDtSourceUa.TypeRoleUA.Tache)

        Return (From ua In uaMetier _
                Where mUaSelectionnes.All(Function(s) s <> ua.Nom) _
                ).ToList
    End Function

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

    ''' <summary>
    ''' Permet de déterminé si au moins un employé a une association étrangère.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PossedeAssociationEtrangere() As Boolean
        Dim indicateur As Boolean = False

        For Each e In mEmployes
            For Each uaa In e.UniteAdmnsAssociees

                If mUaSelectionnes.Contains(uaa.Titre) Then

                    If uaa.ValeurAssocieeEtrangere = True Then
                        indicateur = True
                    End If

                End If

                If indicateur = True Then Exit For
            Next

            If indicateur = True Then Exit For
        Next

        Return indicateur
    End Function

#End Region

End Class
