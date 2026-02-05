Friend Class TsCuMembreGroupeCache

    Private Const FORMAT_MEMBRE_UTIL_GRP As String = "MEMBRE_UTIL_GRP_{0}_{1}"

    Private _helperMemorisation As TsCaHelperMemorisation = Nothing

    Private ReadOnly Property HelperMemorisation As TsCaHelperMemorisation
        Get
            If _helperMemorisation Is Nothing Then
                _helperMemorisation = New TsCaHelperMemorisation()
            End If

            Return _helperMemorisation
        End Get
    End Property

    Private Function ConstruireCleCacheEstMembreGroupe(ByVal codeUtilisateur As String, ByVal codeGroupe As String) As String
        Return String.Format(FORMAT_MEMBRE_UTIL_GRP, codeUtilisateur, codeGroupe)
    End Function

    Public Function ObtenirEstMembreGroupe(ByVal codeUtilisateur As String, ByVal codeGroupe As String) As Boolean?
        Dim estMembreGroupe As Boolean?

        Dim cle As String = ConstruireCleCacheEstMembreGroupe(codeUtilisateur, codeGroupe)
        Dim objetMemoire As Object = HelperMemorisation.ObtenirObjetMemoire(cle)
        If objetMemoire IsNot Nothing Then
            estMembreGroupe = Convert.ToBoolean(objetMemoire)
        End If

        Return estMembreGroupe
    End Function

    Public Sub MemoriserEstMembreGroupe(ByVal codeUtilisateur As String, ByVal codeGroupe As String, ByVal cacheItem As Boolean)
        Dim cle As String = ConstruireCleCacheEstMembreGroupe(codeUtilisateur, codeGroupe)
        HelperMemorisation.Memoriser(cle, cacheItem)
    End Sub

End Class
