
Public Class TsCdRegleCoherence

    Private mIDRole As String
    Private mLstObligatoire As List(Of String)
    Private mLstIncoherent As List(Of String)
    Private mLstChoixMultiples As List(Of String)
    Private mLstChoixUnique As List(Of String)


    Public Property IDRole As String
        Get
            Return mIDRole
        End Get
        Set(value As String)
            mIDRole = value
        End Set
    End Property

    Public Property LstObligatoire As List(Of String)
        Get
            Return mLstObligatoire
        End Get
        Set(value As List(Of String))
            mLstObligatoire = value
        End Set
    End Property

    Public Property LstIncoherent As List(Of String)
        Get
            Return mLstIncoherent
        End Get
        Set(value As List(Of String))
            mLstIncoherent = value
        End Set
    End Property

    Public Property LstChoixMultiples As List(Of String)
        Get
            Return mLstChoixMultiples
        End Get
        Set(value As List(Of String))
            mLstChoixMultiples = value
        End Set
    End Property

    Public Property LstChoixUnique As List(Of String)
        Get
            Return mLstChoixUnique
        End Get
        Set(value As List(Of String))
            mLstChoixUnique = value
        End Set
    End Property

    Public Sub New()

        LstObligatoire = New List(Of String)
        LstIncoherent = New List(Of String)
        LstChoixMultiples = New List(Of String)
        LstChoixUnique = New List(Of String)
    End Sub


End Class
