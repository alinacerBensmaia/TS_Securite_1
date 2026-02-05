Imports System.Collections.Generic
Imports System.DirectoryServices
Imports Rrq.Securite

Friend MustInherit Class TsCuBaseIObtnrInfoAD
    Implements TsIObtnrInfoAD
    Protected _accesseurAD As TsCuAccesseurAD
    Protected _champsAD As TsCuTypeRequeteVsAttributAD


    Protected Overridable ReadOnly Property ChampCodeUtilisateur As String = "sAMAccountName"
    Protected Overridable ReadOnly Property ChampNom As String = "Sn"
    Protected Overridable ReadOnly Property ChampPrenom As String = "GivenName"
    Protected Overridable ReadOnly Property ChampNomComplet As String = "DisplayName"
    Protected Overridable ReadOnly Property ChampCourriel As String = "Mail"
    Protected Overridable ReadOnly Property ChampUniteAdministrative As String = "Department"
    Protected Overridable ReadOnly Property ChampFonction As String = "Title"
    Protected Overridable ReadOnly Property ChampMembreDe As String = "MemberOf"
    Protected Overridable ReadOnly Property ChampSID As String = "ObjectSid"
    Protected Overridable ReadOnly Property ChampSociete As String = "company"
    Protected Overridable ReadOnly Property ChampDescription As String = "description"
    Protected Overridable ReadOnly Property ChampNumeroEmploye As String = "employeeNumber"
    Protected Overridable ReadOnly Property ChampNumeroTelephone As String = "telephoneNumber"


    Protected Overridable ReadOnly Property ChampEstBacule As String = "EXTENSIONATTRIBUTE12"

    Protected Sub New()
        assignerCorrespondanceChampsAD()
        _accesseurAD = New TsCuAccesseurAD(NomServeur, _champsAD)
    End Sub


    ''' <summary>
    '''  Cette propriété retourne le domaine accédé par la l'intance de la classe.
    ''' </summary>
    Protected MustOverride ReadOnly Property Domaine As TsIadNomDomaine

    ''' <summary>
    '''  L'implementation de cette propriété doit retourner le nom du serveur AD à être accédé.
    ''' </summary>
    Protected MustOverride ReadOnly Property NomServeur As String Implements TsIObtnrInfoAD.ServeurActiveDirectory

    Public ReadOnly Property DomaineNT As String Implements TsIObtnrInfoAD.DomaineNT
        Get
            Return _accesseurAD.DomaineNT
        End Get
    End Property

    ''' <summary>
    '''  L'implementation de cette doit remplir la correspondance entre les type de requête et les attributs de l'AD.
    ''' </summary>
    Private Sub assignerCorrespondanceChampsAD()
        _champsAD = New TsCuTypeRequeteVsAttributAD
        _champsAD.AjouterCombinaison(TsIadTypeRequete.TsIadTrCodeUtilisateur, ChampCodeUtilisateur)
        _champsAD.AjouterCombinaison(TsIadTypeRequete.TsIadTrNom, ChampNom)
        _champsAD.AjouterCombinaison(TsIadTypeRequete.TsIadTrPrenom, ChampPrenom)
        _champsAD.AjouterCombinaison(TsIadTypeRequete.TsIadTrNomComplet, ChampNomComplet)
        _champsAD.AjouterCombinaison(TsIadTypeRequete.TsIadTrCourriel, ChampCourriel)
        _champsAD.AjouterCombinaison(TsIadTypeRequete.TsIadTrUniteAdmn, ChampUniteAdministrative)
        _champsAD.AjouterCombinaison(TsIadTypeRequete.TsIadTrFonction, ChampFonction)
        _champsAD.AjouterCombinaison(TsIadTypeRequete.TsIadTrMembreDe, ChampMembreDe)
        _champsAD.AjouterCombinaison(TsIadTypeRequete.TsIadTrSid, ChampSID)
        _champsAD.AjouterCombinaison(TsIadTypeRequete.TsIadTrSociete, ChampSociete)
        _champsAD.AjouterCombinaison(TsIadTypeRequete.TsIadTrDescription, ChampDescription)
        _champsAD.AjouterCombinaison(TsIadTypeRequete.TsIadTrNoEmploye, ChampNumeroEmploye)
        _champsAD.AjouterCombinaison(TsIadTypeRequete.TsIadTrNoTelephone, ChampNumeroTelephone)
    End Sub


    Public Overridable Function ChercheDansGroupes(strACID As String, strGroupeRecherche As String) As Boolean Implements TsIObtnrInfoAD.ChercheDansGroupes
        Return _accesseurAD.ChercheDansGroupes(strACID, strGroupeRecherche)
    End Function


    Public Overridable Function ObtenirListeGroupes(Filtre As String) As SearchResultCollection Implements TsIObtnrInfoAD.ObtenirListeGroupes
        Return _accesseurAD.ObtenirListeGroupes(Filtre)
    End Function



    Public Overridable Function ObtenirUtilisateur(codeUtilisateur As String) As TsCuUtilisateurAD Implements TsIObtnrInfoAD.ObtenirUtilisateur
        ' Exécuter la requête d'accès à l'active directory
        Using dt As DataTable = _accesseurAD.RechercheActiveDirectory(TsIadTypeRequete.TsIadTrCodeUtilisateur, codeUtilisateur, String.Empty, TsIadObjectCategory.TsIadOcPerson)
            ' Conserver les informations
            If dt.Rows.Count = 1 Then
                Dim utilisateur As TsCuUtilisateurAD = convertirRowEnUtilisateur(dt.Rows(0))
                Return utilisateur

            ElseIf dt.Rows.Count > 1 Then
                ' Plus d'un utilisateur retourné dans le DataTable
                Throw New TsCuRetourMultipleException
            Else
                ' Aucun utilisateur retourné dans le DataTable
                Throw New TsCuCodeUtilisateurInexistantException(codeUtilisateur)
            End If
        End Using
    End Function

    Public Overridable Function ObtenirListeUtilisateur(pTypeRequete As TsIadTypeRequete, pCritereRecherche As String, pCritereRechercheSecondaire As String, pCategorie As TsIadObjectCategory) As List(Of TsCuUtilisateurAD) Implements TsIObtnrInfoAD.ObtenirListeUtilisateur
        Dim resultat As New List(Of TsCuUtilisateurAD)

        Using dt As DataTable = _accesseurAD.RechercheActiveDirectory(pTypeRequete, pCritereRecherche, pCritereRechercheSecondaire, TsIadObjectCategory.TsIadOcPerson)
            For Each row As DataRow In dt.Rows
                Dim utilisateur As TsCuUtilisateurAD = convertirRowEnUtilisateur(row)
                resultat.Add(utilisateur)
            Next
        End Using

        Return resultat
    End Function

    Public Overridable Function ObtenirListeMembreGroupe(strGroupe As String, blnRechRecursive As Boolean) As List(Of TsCuUtilisateurAD) Implements TsIObtnrInfoAD.ObtenirListeMembreGroupe
        Dim resultat As New List(Of TsCuUtilisateurAD)

        Using dt As DataTable = _accesseurAD.RechercheGroupeAD(strGroupe, blnRechRecursive)
            For Each row As DataRow In dt.Rows
                Dim utilisateur As TsCuUtilisateurAD = convertirRowEnUtilisateur(row)
                resultat.Add(utilisateur)
            Next
        End Using

        Return resultat
    End Function

    Private Function convertirRowEnUtilisateur(row As DataRow) As TsCuUtilisateurAD
        Dim codeUtilisateur As String = row.sAMAccountName
        Dim utilisateur As New TsCuUtilisateurAD(codeUtilisateur,
                                                 row.EnChaine(ChampNom),
                                                 row.EnChaine(ChampPrenom),
                                                 row.EnChaine(ChampNomComplet),
                                                 row.EnChaine(ChampCourriel),
                                                 row.EnChaine(ChampUniteAdministrative),
                                                 row.EnChaine(ChampFonction),
                                                 row.EnChaine(ChampSociete),
                                                 row.EnChaine(ChampNumeroEmploye),
                                                 row.EstCompteDesactive(),
                                                 Domaine,
                                                 row.MemberOf,
                                                 GetNomPoste(codeUtilisateur),
                                                 row.EnChaine(ChampNumeroTelephone),
                                                 row.EstCompteBascule(),
                                                 row.EstCompteAdmin)
        Return utilisateur
    End Function

    Public Overridable Function RechercheGroupeAD(strGroupe As String, blnRechRecursive As Boolean) As DataTable Implements TsIObtnrInfoAD.RechercheGroupeAD
        Return _accesseurAD.RechercheGroupeAD(strGroupe, blnRechRecursive)
    End Function

    Public Overridable Function RechercheActiveDirectory(pTypeRequete As TsIadTypeRequete, strCritereRecherche As String, strCritereRechercheSecondaire As String, pObjectCategory As TsIadObjectCategory) As DataTable Implements TsIObtnrInfoAD.RechercheActiveDirectory
        Return _accesseurAD.RechercheActiveDirectory(pTypeRequete, strCritereRecherche, strCritereRechercheSecondaire, pObjectCategory)
    End Function

    Public Overridable Function ObtenirMembresGroupe(NomGroupe As String) As String() Implements TsIObtnrInfoAD.ObtenirMembresGroupe
        Return _accesseurAD.ObtenirMembresGroupe(NomGroupe)
    End Function


    Public Overridable Function VerifierGroupeExiste(NomGroupe As String) As Boolean Implements TsIObtnrInfoAD.VerifierGroupeExiste
        Return _accesseurAD.VerifierGroupeExiste(NomGroupe)
    End Function

    Protected Function GetNomPoste(codeUtilisateur As String) As String
        If codeUtilisateur.Equals(Environment.UserName, StringComparison.InvariantCultureIgnoreCase) Then
            Return Environment.MachineName()
        End If
        Return String.Empty
    End Function
End Class


Friend Module DataRowExtensions
    Private Const ADS_UF_ACCOUNTDISABLE As Integer = &H2

    <Runtime.CompilerServices.Extension>
    Public Function EnChaine(source As DataRow, champ As String) As String
        If Not IsDBNull(source.Item(champ)) Then
            Return source.Item(champ).ToString()
        End If
        Return String.Empty
    End Function

    <Runtime.CompilerServices.Extension>
    Public Function sAMAccountName(source As DataRow) As String
        Return source.EnChaine("sAMAccountName")
    End Function

    <Runtime.CompilerServices.Extension>
    Public Function MemberOf(source As DataRow) As String
        Return source.EnChaine("MemberOf")
    End Function

    <Runtime.CompilerServices.Extension>
    Public Function EstCompteDesactive(source As DataRow) As Boolean
        If Not IsDBNull(source.Item("userAccountControl")) Then
            Return CBool(CType(source.Item("userAccountControl"), Integer) And ADS_UF_ACCOUNTDISABLE)
        End If

        Return CBool(Nothing)
    End Function

    <Runtime.CompilerServices.Extension>
    Public Function EstCompteBascule(source As DataRow) As Boolean
        Dim valeur As String = source.EnChaine("EXTENSIONATTRIBUTE12")
        'si la valeur est "FALSE" le compte n'est pas basculé.
        Return String.IsNullOrEmpty(valeur)
    End Function

    <Runtime.CompilerServices.Extension>
    Public Function EstCompteAdmin(source As DataRow) As Boolean
        If Not IsDBNull(source.Item("estCompteAdmin")) Then
            Return DirectCast(source.Item("estCompteAdmin"), Boolean)
        End If
        Return False
    End Function

End Module