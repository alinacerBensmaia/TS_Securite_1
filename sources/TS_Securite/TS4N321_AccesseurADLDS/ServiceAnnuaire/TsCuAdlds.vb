Imports System.DirectoryServices.Protocols
Imports Rrq.InfrastructureCommune.Parametres

Friend Class TsCuAdlds
    Implements TsIServiceAnnuaire, IDisposable

    Private Const FORMAT_RECHERCHE_GUID As String = "<GUID={0}>"
    Private Const NB_RESULTATS_DE_RECHERCHE_PAR_PAGE As Integer = 1000

    Private _adresseAdlds As String = Nothing
    Private _dnUtilisateurs As String = Nothing
    Private _dnRacine As String = Nothing
    Private _adldsCache As TsCuAdldsCache = Nothing
    Private _connexionLdap As LdapConnection = Nothing

    Private ReadOnly Property AdresseAdlds As String
        Get

            If _adresseAdlds Is Nothing Then
                _adresseAdlds = XuCuPolitiqueConfig.ConfigDomaine.ValeurSysteme("TS4", "TS4\TS4N321\ServeurADLDS")
            End If

            Return _adresseAdlds
        End Get
    End Property

    Private ReadOnly Property DnUtilisateurs As String
        Get

            If _dnUtilisateurs Is Nothing Then
                _dnUtilisateurs = XuCuPolitiqueConfig.ConfigDomaine.ValeurSysteme("TS4", "TS4\TS4N321\RepertoireUtilisateurs")
            End If

            Return _dnUtilisateurs
        End Get
    End Property

    Private ReadOnly Property DnRacine As String
        Get

            If _dnRacine Is Nothing Then
                _dnRacine = XuCuPolitiqueConfig.ConfigDomaine.ValeurSysteme("TS4", "TS4\TS4N321\RepertoireRacine")
            End If

            Return _dnRacine
        End Get
    End Property

    Private ReadOnly Property CacheAdlds As TsCuAdldsCache
        Get

            If _adldsCache Is Nothing Then
                _adldsCache = New TsCuAdldsCache()
            End If

            Return _adldsCache
        End Get
    End Property

    Private ReadOnly Property ConnexionLdap As LdapConnection
        Get

            If _connexionLdap Is Nothing Then
                _connexionLdap = New LdapConnection(New LdapDirectoryIdentifier(AdresseAdlds))
                _connexionLdap.AutoBind = True
            End If

            Return _connexionLdap
        End Get
    End Property

    Public Function EstMembreDe(ByVal codeUtilisateur As String, ByVal codeGroupe As String) As Boolean _
        Implements TsIServiceAnnuaire.EstMembreDe

        Dim guidUtilisateur As Guid
        Dim guidGroupe As Guid

        Try
            guidUtilisateur = ObtenirGuidUtilisateur(codeUtilisateur)
            guidGroupe = ObtenirGuidGroupe(codeGroupe)
        Catch ex As TsCuUtilisateurInexistantException
            Return False
        Catch ex As TsCuGroupeSecuriteInexistantException
            Return False
        End Try


        Dim estMembre As Boolean = False
        Dim baseRecherche As String = String.Format(FORMAT_RECHERCHE_GUID, guidGroupe.ToString())
        Dim filtreRecherche As String = String.Format("(member:1.2.840.113556.1.4.1941:={0})", String.Format(FORMAT_RECHERCHE_GUID, guidUtilisateur.ToString()))
        Dim attributs As String() = New String() {}
        Dim resultat As SearchResultEntry = RechercherUn(baseRecherche, filtreRecherche, SearchScope.Base, attributs)
        estMembre = resultat IsNot Nothing

        Return estMembre
    End Function

    Private Function ObtenirGuidUtilisateur(ByVal codeUtilisateur As String) As Guid
        Dim guidUtilisateur As Guid? = CacheAdlds.ObtenirGuidUtilisateur(codeUtilisateur)

        If guidUtilisateur Is Nothing Then
            guidUtilisateur = ObtenirGuidUtilisateurAdlds(codeUtilisateur)
            CacheAdlds.MemoriserGuidUtilisateur(codeUtilisateur, guidUtilisateur.Value)
        End If

        Return guidUtilisateur.Value
    End Function

    Private Function ObtenirGuidUtilisateurAdlds(ByVal codeUtilisateur As String) As Guid

        Dim guidUtilisateur As Guid? = Nothing
        Dim baseRecherche As String = DnUtilisateurs
        Dim filtreRecherche As String = String.Format("(&(objectClass=user)(userPrincipalName={0}))", codeUtilisateur)
        Dim attributs As String() = New String() {"objectGUID"}
        Dim resultat As SearchResultEntry = RechercherUn(baseRecherche, filtreRecherche, SearchScope.Subtree, attributs)

        If resultat IsNot Nothing Then
            guidUtilisateur = ConvertirAttributEnGuid(resultat, "objectGUID")
        Else
            Throw New TsCuUtilisateurInexistantException(codeUtilisateur)
        End If

        Return guidUtilisateur.Value
    End Function

    Private Function ObtenirGuidGroupe(ByVal codeGroupe As String) As Guid
        Dim guidGroupe As Guid? = CacheAdlds.ObtenirGuidGroupe(codeGroupe)

        If guidGroupe Is Nothing Then
            guidGroupe = ObtenirGuidGroupeAdlds(codeGroupe)
            CacheAdlds.MemoriserGuidGroupe(codeGroupe, guidGroupe.Value)
        End If

        Return guidGroupe.Value
    End Function

    Private Function ObtenirGuidGroupeAdlds(ByVal codeGroupe As String) As Guid

        Dim guidGroupe As Guid? = Nothing
        Dim baseRecherche As String = DnRacine
        Dim filtreRecherche As String = String.Format("(&(objectClass=group)(Name={0}))", codeGroupe)
        Dim attributs As String() = New String() {"objectGUID"}
        Dim resultat As SearchResultEntry = RechercherUn(baseRecherche, filtreRecherche, SearchScope.Subtree, attributs)

        If resultat IsNot Nothing Then
            guidGroupe = ConvertirAttributEnGuid(resultat, "objectGUID")
        Else
            Throw New TsCuGroupeSecuriteInexistantException(codeGroupe)
        End If

        Return guidGroupe.Value
    End Function

    Private Function ConvertirAttributEnGuid(ByVal resultat As SearchResultEntry, ByVal attributGuid As String) As Guid
        Dim guidAttribut As Guid? = Nothing
        Dim guidByteArray As Byte() = TryCast(resultat.Attributes(attributGuid).GetValues(GetType(Byte())).FirstOrDefault(), Byte())

        If guidByteArray IsNot Nothing Then
            guidAttribut = New Guid(guidByteArray)
        End If

        Return guidAttribut.Value
    End Function

    Private Function RechercherUn(ByVal dnBaseRecherche As String, ByVal filtreRecherche As String, ByVal searchScope As SearchScope, ByVal attributs As String()) As SearchResultEntry

        Dim resultats As IList(Of SearchResultEntry) = RechercherPlusieurs(dnBaseRecherche, filtreRecherche, searchScope, attributs, 1)

        If resultats IsNot Nothing AndAlso resultats.Count > 0 Then
            Return resultats(0)
        End If

        Return Nothing
    End Function

    Private Function RechercherPlusieurs(ByVal dnBaseRecherche As String, ByVal filtreRecherche As String, ByVal searchScope As SearchScope, ByVal attributs As String(), ByVal taillePage As Integer?) As IList(Of SearchResultEntry)

        Dim resultats As IList(Of SearchResultEntry) = Nothing

        If attributs Is Nothing Then
            attributs = New String() {}
        End If

        If Not taillePage.HasValue Then
            taillePage = NB_RESULTATS_DE_RECHERCHE_PAR_PAGE
        End If

        Dim searchRequest As SearchRequest = New SearchRequest(dnBaseRecherche, filtreRecherche, searchScope, attributs)
        searchRequest.SizeLimit = taillePage.Value

        Dim pageResultRequest As PageResultRequestControl = New PageResultRequestControl(taillePage.Value)
        searchRequest.Controls.Add(pageResultRequest)

        Dim searchOptions As SearchOptionsControl = New SearchOptionsControl(SearchOption.DomainScope)
        searchRequest.Controls.Add(searchOptions)

        resultats = New List(Of SearchResultEntry)()

        ' boucle servant à gérer la pagination des résultats de recherche
        While True
            Dim searchResponse As SearchResponse = TryCast(ConnexionLdap.SendRequest(searchRequest), SearchResponse)

            If searchResponse IsNot Nothing AndAlso searchResponse.ResultCode = ResultCode.Success AndAlso searchResponse.Entries.Count > 0 Then
                Dim pageResultResponse As PageResultResponseControl = TryCast(searchResponse.Controls.FirstOrDefault(Function(ctrl) TypeOf ctrl Is PageResultResponseControl), PageResultResponseControl)

                ' parcourir touts les résultats de la page et les ajouter dans la liste de résultats
                For i As Integer = 0 To searchResponse.Entries.Count - 1
                    resultats.Add(searchResponse.Entries(i))
                Next

                ' aller chercher le cookie pour la pagination
                If pageResultResponse IsNot Nothing Then
                    pageResultRequest.Cookie = pageResultResponse.Cookie
                End If

                ' si la taille du cookie est à zéro, cela veut dire que  
                ' nous avons reçu tous les résultats de recherche
                If pageResultRequest.Cookie.Length = 0 Then
                    Exit While
                End If

            Else
                Exit While
            End If
        End While

        Return resultats
    End Function

    Public Sub Dispose() _
        Implements IDisposable.Dispose

        If _connexionLdap IsNot Nothing Then
            ConnexionLdap.Dispose()
        End If
    End Sub
End Class
