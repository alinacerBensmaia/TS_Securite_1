Friend Class TsCuAccesServiceAnnuaire
    Implements IDisposable

    Private _serviceAnnuaire As TsIServiceAnnuaire = Nothing
    Private _membreGroupeCache As TsCuMembreGroupeCache = Nothing

    Private ReadOnly Property ServiceAnnuaire As TsIServiceAnnuaire
        Get

            If _serviceAnnuaire Is Nothing Then
                _serviceAnnuaire = TsCuFabriqueServiceAnnuaire.CreerInstance()
            End If

            Return _serviceAnnuaire
        End Get
    End Property

    Private ReadOnly Property MembreGroupeCache As TsCuMembreGroupeCache
        Get

            If _membreGroupeCache Is Nothing Then
                _membreGroupeCache = New TsCuMembreGroupeCache()
            End If

            Return _membreGroupeCache
        End Get
    End Property

    Public Function EstMembreDe(ByVal codeUtilisateur As String, ByVal codesGroupes As IList(Of String)) As IDictionary(Of String, Boolean)
        Dim membreGroupes As IDictionary(Of String, Boolean) = Nothing
        Dim estMembre As Boolean = False

        If codeUtilisateur IsNot Nothing AndAlso codesGroupes IsNot Nothing Then

            membreGroupes = New Dictionary(Of String, Boolean)

            ' pour eviter les doublons dans le dictionnaire
            codesGroupes = codesGroupes.Distinct().ToList()

            Dim lastIndex = codesGroupes.Count - 1

            For index As Integer = 0 To lastIndex
                ' aller chercher l'item en cache
                Dim estMembreGroupeCache As Boolean? = MembreGroupeCache.ObtenirEstMembreGroupe(codeUtilisateur, codesGroupes(index))
                If estMembreGroupeCache IsNot Nothing Then
                    estMembre = estMembreGroupeCache.Value
                Else
                    ' s'il n'existe pas, aller le chercher dans le service d'annuaire 
                    ' et le mettre ensuite en cache s'il est autorisé
                    estMembre = ServiceAnnuaire.EstMembreDe(codeUtilisateur, codesGroupes(index))
                    If estMembre Then
                        MembreGroupeCache.MemoriserEstMembreGroupe(codeUtilisateur, codesGroupes(index), estMembre)
                    End If
                End If

                membreGroupes.Add(codesGroupes(index), estMembre)
            Next
        End If

        Return membreGroupes
    End Function

    Public Sub Dispose() _
        Implements IDisposable.Dispose

        If _serviceAnnuaire IsNot Nothing Then
            ServiceAnnuaire.Dispose()
        End If
    End Sub
End Class

