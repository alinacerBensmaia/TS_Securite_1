Imports Rrq.InfrastructureCommune.Parametres
Imports System.DirectoryServices
Imports System.Collections.Generic

Friend Class TsCuObtnrInfoADCARRA
    Implements TsIObtnrInfoAD

    Private _accesseurAD As TsCuAccesseurAD
    Private _relationTypeReqAttributAD As TsCuTypeRequeteVsAttributAD
    Private Const ADS_UF_ACCOUNTDISABLE As Integer = &H2


    Sub New()
        CombinerTypeRequeteVSAttributAD()
        _accesseurAD = New TsCuAccesseurAD(NomServeur, _relationTypeReqAttributAD)
    End Sub

#Region " Propriétés "
    Public ReadOnly Property Domaine As TsIadNomDomaine Implements TsIObtnrInfoAD.Domaine
        Get
            Return TsIadNomDomaine.TsDomaineCARRA
        End Get
    End Property

#End Region

#Region " Fonctions et méthodes publiques "

    Public Function ObtenirUtilisateur(ByVal strCodeUtilisateur As String) As TsCuUtilisateurAD Implements TsIObtnrInfoAD.ObtenirUtilisateur

        Dim dtAD As DataTable = Nothing
        Dim utilisateurAD As TsCuUtilisateurAD

        Try
            ' Exécuter la requête d'accès à l'active directory
            dtAD = _accesseurAD.RechercheActiveDirectory(TsIadTypeRequete.TsIadTrCodeUtilisateur, strCodeUtilisateur, pObjectCategory:=TsIadObjectCategory.TsIadOcPerson)
            ' Conserver les informations
            If dtAD.Rows.Count = 1 Then
                utilisateurAD = New TsCuUtilisateurAD(strCodeUtilisateur, _
                    CStr(IIf(IsDBNull(dtAD.Rows(0).Item("Sn")) = False, dtAD.Rows(0).Item("Sn"), String.Empty)), _
                    CStr(IIf(IsDBNull(dtAD.Rows(0).Item("GivenName")) = False, dtAD.Rows(0).Item("GivenName").ToString, String.Empty)), _
                    CStr(IIf(IsDBNull(dtAD.Rows(0).Item("DisplayName")) = False, dtAD.Rows(0).Item("DisplayName").ToString, String.Empty)), _
                    CStr(IIf(IsDBNull(dtAD.Rows(0).Item("Mail")) = False, dtAD.Rows(0).Item("Mail").ToString, String.Empty)), _
                    CStr(IIf(IsDBNull(dtAD.Rows(0).Item("initials")) = False, dtAD.Rows(0).Item("initials").ToString, String.Empty)), _
                    CStr(IIf(IsDBNull(dtAD.Rows(0).Item("personalTitle")) = False, dtAD.Rows(0).Item("personalTitle").ToString, String.Empty)), _
                    CStr(IIf(IsDBNull(dtAD.Rows(0).Item("department")) = False, dtAD.Rows(0).Item("department").ToString, String.Empty)), _
                    CStr(IIf(IsDBNull(dtAD.Rows(0).Item("employeeNumber")) = False, dtAD.Rows(0).Item("employeeNumber").ToString, String.Empty)), _
                    CBool(IIf(IsDBNull(dtAD.Rows(0).Item("userAccountControl")) = False, CBool(CType(dtAD.Rows(0).Item("userAccountControl"), Integer) And ADS_UF_ACCOUNTDISABLE), Nothing)), _
                    TsIadNomDomaine.TsDomaineCARRA, _
                    CStr(IIf(IsDBNull(dtAD.Rows(0).Item("MemberOf")) = False, dtAD.Rows(0).Item("MemberOf").ToString, String.Empty)))

            ElseIf dtAD.Rows.Count > 1 Then
                ' Plus d'un utilisateur retourné dans le DataTable
                Throw New TsCuRetourMultipleException
            Else
                ' Aucun utilisateur retourné dans le DataTable
                Throw New TsCuCodeUtilisateurInexistantException(strCodeUtilisateur)
            End If
        Finally
            ' Libérer les ressources utilisées
            If Not IsNothing(dtAD) Then
                dtAD.Dispose()
                dtAD = Nothing
            End If
        End Try

        Return utilisateurAD
    End Function


    Public Function ObtenirListeGroupes(ByVal Filtre As String) As SearchResultCollection Implements TsIObtnrInfoAD.ObtenirListeGroupes
        Return _accesseurAD.ObtenirListeGroupes(Filtre)
    End Function


    Public Function ObtenirListeUtilisateur(ByVal pTypeRequete As TsIadTypeRequete, ByVal pCritereRecherche As String, _
                                       ByVal pCritereRechercheSecondaire As String, _
                                       ByVal pCategorie As TsIadObjectCategory) As List(Of TsCuUtilisateurAD) Implements TsIObtnrInfoAD.ObtenirListeUtilisateur

        Dim listeUtilisateur As New List(Of TsCuUtilisateurAD)
        For Each row As DataRow In _accesseurAD.RechercheActiveDirectory(pTypeRequete, pCritereRecherche, pCritereRechercheSecondaire, TsIadObjectCategory.TsIadOcTous).Rows
            Dim utilisateur As TsCuUtilisateurAD
            Dim userAccountControl As Object = row.Item("userAccountControl")
            If userAccountControl Is System.DBNull.Value Then userAccountControl = Nothing


            utilisateur = New TsCuUtilisateurAD(row.Item("sAMAccountName").ToString, _
                                                     row.Item("Sn").ToString, _
                                                     row.Item("GivenName").ToString, _
                                                     row.Item("DisplayName").ToString, _
                                                     row.Item("Mail").ToString, _
                                                     row.Item("initials").ToString, _
                                                     row.Item("personalTitle").ToString, _
                                                     row.Item("department").ToString, _
                                                     row.Item("employeeNumber").ToString, _
                                                     CBool(CType(userAccountControl, Integer) And ADS_UF_ACCOUNTDISABLE), _
                                                     TsIadNomDomaine.TsDomaineCARRA, _
                                                     row.Item("memberof").ToString)

            listeUtilisateur.Add(utilisateur)
        Next

        Return listeUtilisateur


    End Function


    Public Function VerifierGroupeExiste(ByVal strGroupe As String) As Boolean Implements TsIObtnrInfoAD.VerifierGroupeExiste
        Return _accesseurAD.VerifierGroupeExiste(strGroupe)
    End Function

    Public Function ObtenirListeMembreGroupe(ByVal strGroupe As String, ByVal blnRechRecursive As Boolean) As List(Of TsCuUtilisateurAD) Implements TsIObtnrInfoAD.ObtenirListeMembreGroupe

        Dim listeUtilisateur As New List(Of TsCuUtilisateurAD)


        For Each row As DataRow In _accesseurAD.RechercheGroupeAD(strGroupe, blnRechRecursive).Rows
            Dim utilisateur As TsCuUtilisateurAD

            utilisateur = New TsCuUtilisateurAD(row.Item("sAMAccountName").ToString, _
                                                 row.Item("Sn").ToString, _
                                                 row.Item("GivenName").ToString, _
                                                 row.Item("DisplayName").ToString, _
                                                 row.Item("Mail").ToString, _
                                                 row.Item("initials").ToString, _
                                                 row.Item("personalTitle").ToString, _
                                                 row.Item("department").ToString, _
                                                 row.Item("employeeNumber").ToString, _
                                                 CBool(CType(row.Item("userAccountControl"), Integer) And ADS_UF_ACCOUNTDISABLE), _
                                                 TsIadNomDomaine.TsDomaineCARRA, _
                                                 row.Item("MemberOf").ToString)
            listeUtilisateur.Add(utilisateur)
        Next



        Return listeUtilisateur


    End Function


    Public Function RechercheGroupeAD(ByVal strGroupe As String, ByVal blnRechRecursive As Boolean) As DataTable Implements TsIObtnrInfoAD.RechercheGroupeAD

        Return _accesseurAD.RechercheGroupeAD(strGroupe, blnRechRecursive)

    End Function

    Public Function ChercheDansGroupes(ByVal strACID As String, ByVal strGroupeRecherche As String) As Boolean Implements TsIObtnrInfoAD.ChercheDansGroupes
        Return _accesseurAD.ChercheDansGroupes(strACID, strGroupeRecherche)
    End Function


    Public Function RechercheActiveDirectory(ByVal pTypeRequete As TsIadTypeRequete, ByVal strCritereRecherche As String, _
                                      Optional ByVal strCritereRechercheSecondaire As String = "", _
                                      Optional ByVal pObjectCategory As TsIadObjectCategory = TsIadObjectCategory.TsIadOcTous) As DataTable Implements TsIObtnrInfoAD.RechercheActiveDirectory

        Return _accesseurAD.RechercheActiveDirectory(pTypeRequete, strCritereRecherche, strCritereRechercheSecondaire, pObjectCategory)

    End Function

    Public Function ObtenirMembresGroupe(ByVal NomGroupe As String) As String() Implements TsIObtnrInfoAD.ObtenirMembresGroupe
        Return _accesseurAD.ObtenirMembresGroupe(NomGroupe)
    End Function
#End Region


#Region " Fonctions et méthodes privés "

    Private Sub CombinerTypeRequeteVSAttributAD() Implements TsIObtnrInfoAD.CombinerTypeRequeteVSAttributAD
        _relationTypeReqAttributAD = New TsCuTypeRequeteVsAttributAD
        _relationTypeReqAttributAD.AjouterCombinaison(TsIadTypeRequete.TsIadTrCodeUtilisateur, "sAMAccountName")
        _relationTypeReqAttributAD.AjouterCombinaison(TsIadTypeRequete.TsIadTrNom, "Sn")
        _relationTypeReqAttributAD.AjouterCombinaison(TsIadTypeRequete.TsIadTrPrenom, "GivenName")
        _relationTypeReqAttributAD.AjouterCombinaison(TsIadTypeRequete.TsIadTrNomComplet, "DisplayName")
        _relationTypeReqAttributAD.AjouterCombinaison(TsIadTypeRequete.TsIadTrCourriel, "Mail")
        _relationTypeReqAttributAD.AjouterCombinaison(TsIadTypeRequete.TsIadTrUniteAdmn, "initials")
        _relationTypeReqAttributAD.AjouterCombinaison(TsIadTypeRequete.TsIadTrFonction, "personalTitle")
        _relationTypeReqAttributAD.AjouterCombinaison(TsIadTypeRequete.TsIadTrMembreDe, "MemberOf")
        _relationTypeReqAttributAD.AjouterCombinaison(TsIadTypeRequete.TsIadTrSid, "ObjectSid")
        _relationTypeReqAttributAD.AjouterCombinaison(TsIadTypeRequete.TsIadTrSociete, "Department")
        _relationTypeReqAttributAD.AjouterCombinaison(TsIadTypeRequete.TsIadTrDescription, "description")
        _relationTypeReqAttributAD.AjouterCombinaison(TsIadTypeRequete.TsIadTrNoEmploye, "employeeNumber")
    End Sub

    Private ReadOnly Property NomServeur() As String Implements TsIObtnrInfoAD.NomServeur
        Get

            Return XuCuPolitiqueConfig.ConfigDomaine.ValeurSysteme("TS4", "TS4\ServeurActiveDirectoryCARRA")

        End Get
    End Property

#End Region

End Class
