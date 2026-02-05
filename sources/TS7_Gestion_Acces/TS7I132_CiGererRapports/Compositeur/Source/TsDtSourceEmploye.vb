''' <summary>
''' Définition d'un employé de la source du rapport.
''' </summary>
''' <remarks></remarks>
Friend Class TsDtSourceEmploye

#Region "--- Propriétés ---"

    Private mNom As String
    ''' <summary>
    ''' Nom de l'employé.
    ''' </summary>
    Public Property Nom() As String
        Get
            Return mNom
        End Get
        Set(ByVal value As String)
            mNom = value
        End Set
    End Property

    Private mNoUA As Integer
    ''' <summary>
    ''' Le numéro d'unité administratif dont fait partie l'employé.
    ''' </summary>
    Public Property NoUA() As Integer
        Get
            Return mNoUA
        End Get
        Set(ByVal value As Integer)
            mNoUA = value
        End Set
    End Property

    Private mUniteAdmnsAssociees As New List(Of TsDtSourceUaAssociee)
    ''' <summary>
    ''' Les Unités administratives que l'employé possède.
    ''' </summary>
    Public Property UniteAdmnsAssociees() As List(Of TsDtSourceUaAssociee)
        Get
            Return mUniteAdmnsAssociees
        End Get
        Set(ByVal value As List(Of TsDtSourceUaAssociee))
            mUniteAdmnsAssociees = value
        End Set
    End Property

#End Region

#Region "--- Méthodes ---"

    ''' <summary>
    ''' Permet d'obtenir la valeur associée entre l'employé et le titre de l'unité administrative.
    ''' </summary>
    ''' <param name="pTitre">Titre de l'unité administrative.</param>
    ''' <returns>La valeur associée.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirValeurAssociee(ByVal pTitre As String) As String
        Return (From ua In UniteAdmnsAssociees _
                Where ua.Titre = pTitre _
                Select ua.ValeurAssociee).FirstOrDefault
    End Function

    ''' <summary>
    ''' Permet de connaître si l'association est étrangère à l'unité administrative dont fait parti l'employé.
    ''' </summary>
    ''' <param name="pTitre">Titre de l'association.</param>
    ''' <returns>Vrai si l'association est étrangère.</returns>
    ''' <remarks></remarks>
    Public Function EstValeurEtrangere(ByVal pTitre As String) As Boolean
        Return UniteAdmnsAssociees.Any(Function(ua) ua.Titre = pTitre And ua.ValeurAssocieeEtrangere = True)
    End Function

#End Region

End Class
