Friend Class TsCuAdldsCache
    Private Const FORMAT_ADLDS_UTIL_GUID As String = "ADLDS_UTIL_GUID|{0}"
    Private Const FORMAT_ADLDS_GRP_GUID As String = "ADLDS_GRP_GUID|{0}"

    Private _helperMemorisation As TsCaHelperMemorisation = Nothing

    Private ReadOnly Property HelperMemorisation As TsCaHelperMemorisation
        Get
            If _helperMemorisation Is Nothing Then
                _helperMemorisation = New TsCaHelperMemorisation()
            End If

            Return _helperMemorisation
        End Get
    End Property

    Private Function ConstruireCleCacheGuidUtilisateur(ByVal codeUtilisateur As String) As String
        Return String.Format(FORMAT_ADLDS_UTIL_GUID, codeUtilisateur)
    End Function

    Public Function ObtenirGuidUtilisateur(ByVal codeUtilisateur As String) As Guid?
        Dim cle As String = ConstruireCleCacheGuidUtilisateur(codeUtilisateur)
        Return DirectCast(HelperMemorisation.ObtenirObjetMemoire(cle), Guid?)
    End Function

    Public Sub MemoriserGuidUtilisateur(ByVal codeUtilisateur As String, ByVal guidUtilisteur As Guid)
        Dim cle As String = ConstruireCleCacheGuidUtilisateur(codeUtilisateur)
        HelperMemorisation.Memoriser(cle, guidUtilisteur)
    End Sub

    Private Function ConstruireCleCacheGuidGroupe(ByVal codeGroupe As String) As String
        Return String.Format(FORMAT_ADLDS_GRP_GUID, codeGroupe)
    End Function

    Public Function ObtenirGuidGroupe(ByVal codeGroupe As String) As Guid?
        Dim cle As String = ConstruireCleCacheGuidGroupe(codeGroupe)
        Return DirectCast(HelperMemorisation.ObtenirObjetMemoire(cle), Guid?)
    End Function

    Public Sub MemoriserGuidGroupe(ByVal codeGroupe As String, ByVal guidGroupe As Guid)
        Dim cle As String = ConstruireCleCacheGuidGroupe(codeGroupe)
        HelperMemorisation.Memoriser(cle, guidGroupe)
    End Sub
End Class
