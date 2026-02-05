Friend Class Environnements
    Public Shared Tous As New Environnements("Tous", String.Empty, String.Empty)
    Public Shared Essais As New Environnements("ESSA", "E", "Essais")
    Public Shared Unitaire As New Environnements("UNIT", "U", "Unitaire")
    Public Shared Integration As New Environnements("INTG", "I", "Intégration")
    Public Shared Acceptation As New Environnements("ACCP", "A", "Acceptation")
    Public Shared FormationAcceptation As New Environnements("FORA", "B", "Formation Acceptation")
    Public Shared FormationProduction As New Environnements("FORP", "Q", "Formation Production")
    Public Shared Production As New Environnements("PROD", "P", "Production")
    Public Shared Simulation As New Environnements("SIML", "S", "Simulation")
    Private Shared All As List(Of Environnements)

    Shared Sub New()
        'tous sauf 'Tous'
        All = New List(Of Environnements)({Unitaire, Integration, Acceptation, FormationAcceptation, FormationProduction, Production, Simulation, Essais})
    End Sub

    Public Shared Function ParseCode(valeur As String) As Environnements
        For Each e As Environnements In All
            If e.Code.Equals(valeur, StringComparison.InvariantCultureIgnoreCase) Then Return e
        Next
        Return Tous
    End Function

    Public Shared Function ParseLettre(valeur As String) As Environnements
        For Each e As Environnements In All
            If e.Lettre.Equals(valeur, StringComparison.InvariantCultureIgnoreCase) Then Return e
        Next
        Return Tous
    End Function


    Public ReadOnly Property Code As String
    Public ReadOnly Property Lettre As String
    Public ReadOnly Property Description As String

    Private Sub New(code As String, lettre As String, description As String)
        Me.Code = code
        Me.Lettre = lettre
        Me.Description = description
    End Sub

    Public Function Est(valeur As Environnements) As Boolean
        Return Code.Equals(valeur.Code, StringComparison.InvariantCultureIgnoreCase)
    End Function

End Class