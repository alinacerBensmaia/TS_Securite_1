Public Class TsDtCodeUsageMotPasse

    Private _codeUsager As String
    Private _motPasse As String

    Public Property CodeUsager As String
        Get
            Return _codeUsager
        End Get
        Set(value As String)
            _codeUsager = value
        End Set
    End Property

    Public Property MotPasse As String
        Get
            Return _motPasse
        End Get
        Set(value As String)
            _motPasse = value
        End Set
    End Property

End Class
