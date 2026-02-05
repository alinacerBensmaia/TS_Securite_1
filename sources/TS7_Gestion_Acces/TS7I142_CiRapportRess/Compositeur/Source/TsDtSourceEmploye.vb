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



    Private mlstRessourcesAssociees As New List(Of TsDtSourceRessources)
    Public Property lstRessourcesAssociees As List(Of TsDtSourceRessources)
        Get
            Return mlstRessourcesAssociees
        End Get
        Set(value As List(Of TsDtSourceRessources))
            mlstRessourcesAssociees = value
        End Set
    End Property


#End Region


End Class
