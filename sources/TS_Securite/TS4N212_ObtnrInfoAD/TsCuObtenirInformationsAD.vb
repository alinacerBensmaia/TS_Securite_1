Imports System.DirectoryServices
Imports System.Collections.Generic
Imports Rrq.InfrastructureCommune.Parametres
Imports System.Text

''' <summary>
'''   Cette classe utilitaire récupère et retourne des informations sur un ou des utilisateurs selon le contenu de l'active directory.
''' </summary>
Public Class TsCuObtenirInformationsAD

    ''' <summary>Cette propriété retourne le nom du serveur de l'active directory tel que paramétré dans le registre machine.</summary>
    Private Shared ReadOnly Property ServeurActiveDirectory() As String
        Get
            Return XuCuPolitiqueConfig.ConfigDomaine.ValeurSysteme("General", "ServeurActiveDirectory")
        End Get
    End Property


#Region "Désuèt - Publique Utilisé"

    ''' <summary>
    '''   Cet méthode retourne les informations sur des membres de l'active directory dans une datatable, selon des paramètres de recherche
    '''   spécifiés en paramètres.
    ''' </summary>
    ''' <param name="TypeRequete">Le champ sur lequel s'effectue la requête à l'active directory.</param>
    ''' <param name="CritereRecherche">Le critère de recherche à l'active directory.</param>
    ''' <param name="CritereRechercheSecondaire">Le critère de recherche secondaire à l'active directory utilisé lors d'une recherche nom et prénom.</param>
    ''' <param name="Categorie">Filtre sur ObjectCategory</param>
    ''' <returns>Table qui contient les informations sur les membres.</returns>
    <Obsolete(Desuet.UTILISEZ_212_OBTENIR_UTILISATEURS, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Shared Function ObtenirInformationsMembres(ByVal TypeRequete As TsIadTypeRequete, ByVal CritereRecherche As String, ByVal CritereRechercheSecondaire As String, ByVal Categorie As TsIadObjectCategory) As DataTable
        ''
        ''XF3N131_PrModeliserAut, XF3N171_PrConsoleGrad
        ''  Utilisé pour obtenir une liste d'utilisateur et consommer le "DisplayName" et "sAMAccountName"
        ''
        Dim SearchResults As SearchResultCollection = RechercherToutAD(TypeRequete, Categorie, CritereRecherche, CritereRechercheSecondaire)
        Dim Datatable As DataTable = ConvertirEnDatatable(SearchResults)

        Return Datatable
    End Function

    ''' <summary>
    '''   Retourne une classe de données qui contient les informations sur un utilisateur de l'AD.
    ''' </summary>
    ''' <param name="CodeUtilisateur">Code de l'utilisateur dont on veut obtenir les informations.</param>
    ''' <returns>une classe de données qui contient les informations sur un utilisateur de l'AD.</returns>
    <Obsolete(Desuet.UTILISEZ_212_OBTENIR_UTILISATEUR, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Shared Function ObtenirInformationsUtilisateur(ByVal CodeUtilisateur As String) As TsCuUtilisateurAD
        ''
        ''TS7N121_ServiceGestnAcces, XE4N402_CbAccesParametres, XF3N171_PrConsoleGrad
        ''  Ils utilisent seulement l'objet pour obtenir NomComplet
        ''
        Return tsCuObtnrInfoAD.ObtenirUtilisateur(CodeUtilisateur)
    End Function

    ''' <summary>
    '''  Cette méthode vérifie si un groupe existe dans l'AD.
    ''' </summary>
    ''' <param name="NomGroupe">Nom du groupe a vérifier</param>
    ''' <returns>True si le groupe existe et False si ce n'est pas le cas.</returns>
    <Obsolete(Desuet.UTILISEZ_COMPOSANT_311, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Shared Function VerifierGroupeExiste(ByVal NomGroupe As String) As Boolean
        ''
        ''XF3N131_PrModeliserAut
        ''  Valide l'existance d'un ROG
        ''
        Return New TsCuObtnrInfoADRQ().VerifierGroupeExiste(NomGroupe)
    End Function

#End Region

#Region "Désuèt - Publique Non-Utilisé"

    ''' <summary>
    '''   Permet de détermier si un utilisateur est membre d'un groupe de l'active directory.
    ''' </summary>
    ''' <param name="CodeUtilisateur">Code de l'utilisateur dont on veut vérifier si il est membre d'un groupe.</param>
    ''' <param name="NomGroupe">Nom du groupe dont on désire vérifier l'appartenance</param>
    ''' <returns>True si il est membre du groupe et False si ce n'est pas le cas.</returns>
    <Obsolete(Desuet.MEMBRE_NON_UTILISE, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Shared Function EstMembreDe(ByVal CodeUtilisateur As String, ByVal NomGroupe As String) As Boolean
        Dim EstMembre As Boolean

        EstMembre = EstMembreDe(CodeUtilisateur, NomGroupe, True)

        Return EstMembre
    End Function

    ''' <summary>
    '''   Permet de détermier si un utilisateur est membre d'un groupe de l'active directory.
    ''' </summary>
    ''' <param name="CodeUtilisateur">Code de l'utilisateur dont on veut vérifier si il est membre d'un groupe.</param>
    ''' <param name="NomGroupe">Nom du groupe dont on désire vérifier l'appartenance</param>
    ''' <param name="EstRecursif">
    '''     Si le paramètre est True (par défaut), la recherche s'effectue de manière récursive
    '''     à l'intérieur de tous les groupes dont est membre l'utilisateur et rallonge le temps d'exécution.
    '''     Si le paramètre est False, la recherche ne s'effectue qu'au premier niveau des groupes dont
    '''     l'utilisateur est membre ce qui est de loin plus rapide.
    ''' </param>
    ''' <returns>True si il est membre du groupe et False si ce n'est pas le cas.</returns>
    <Obsolete(Desuet.MEMBRE_NON_UTILISE, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Shared Function EstMembreDe(ByVal CodeUtilisateur As String, ByVal NomGroupe As String, ByVal EstRecursif As Boolean) As Boolean
        Return RechercherGroupeUtilisateur(CodeUtilisateur, NomGroupe, EstRecursif)
    End Function

    ''' <summary>
    '''   Récupère le code utilisateur dans l'AD qui correspond au Sid reçu en paramètre, 
    '''   sans modifier les valeurs des propriétés de l'instance en cours. 
    ''' </summary>
    ''' <param name="SID">Tableau de Bytes qui contient le Sid d'un compte dans l'AD.</param>
    ''' <returns>Le code utilisateur qui correspond au Sid.</returns>
    <Obsolete(Desuet.MEMBRE_NON_UTILISE, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Shared Function ObtenirCodeUtilisateur(ByRef SID As Byte()) As String
        Dim Table As Data.DataTable
        Dim CleAD As String
        Dim CodeUtilisateur As String = String.Empty

        CleAD = ConvertirSid(SID)

        Table = ObtenirInformationsMembres(TsIadTypeRequete.TsIadTrSid, CleAD, String.Empty, TsIadObjectCategory.TsIadOcPerson)

        If Table.Rows.Count = 0 Then
            CodeUtilisateur = "N/A"
        Else
            If IsDBNull(Table.Rows(0).Item("sAMAccountName")) = False Then
                CodeUtilisateur = Table.Rows(0).Item("sAMAccountName").ToString
            End If
        End If

        Return CodeUtilisateur
    End Function

    ''' <summary>
    '''   Permet d'obtenir les informations sur les groupes qui font partie du groupe passé en paramètre.
    ''' </summary>
    ''' <param name="NomGroupe">Nom du groupe dont on veut obtenir les sous groupes.</param>
    ''' <returns>Une table qui contient les informations sur les groupes d'un groupe.</returns>
    <Obsolete(Desuet.MEMBRE_NON_UTILISE, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Shared Function ObtenirInformationsGroupesGroupe(ByVal NomGroupe As String) As DataTable
        Dim Membres As DataSet
        Dim Vue As DataView

        If NomGroupe <> String.Empty Then
            Membres = ObtenirInformationsMembresGroupe(NomGroupe)
        Else
            Throw New TsCuParametreAbsentException("NomGroupe")
        End If

        Vue = Membres.Tables("Groupes").DefaultView
        Vue.Sort = "sAMAccountName"

        Return Vue.ToTable()
    End Function

    ''' <summary>
    '''   Permet d'obtenir les informations sur les groupes qui font partie du groupe passé en paramètre.
    ''' </summary>
    ''' <param name="NomGroupe">Nom du groupe dont on veut obtenir les sous groupes.</param>
    ''' <param name="EstRecursif">
    '''   Si EstRecursif est a "True", on va obtenir les groupes 
    '''   qui sont dans les sous-groupes du groupe passé en paramètre.
    ''' </param>
    ''' <returns>Une table qui contient les informations sur les groupes d'un groupe.</returns>
    <Obsolete(Desuet.MEMBRE_NON_UTILISE, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Shared Function ObtenirInformationsGroupesGroupe(ByVal NomGroupe As String, ByVal EstRecursif As Boolean) As DataTable
        Dim TableGroupes As DataTable
        Dim Vue As DataView

        TableGroupes = ObtenirInformationsGroupesSousGroupe(NomGroupe, EstRecursif)

        Vue = TableGroupes.DefaultView
        Vue.Sort = "sAMAccountName"

        TableGroupes = Vue.ToTable(True)

        Return TableGroupes
    End Function

    ''' <summary>
    '''   Cet méthode retourne les informations sur des membres de l'active directory dans une datatable, selon des paramètres de recherche
    '''   spécifiés en paramètres.
    ''' </summary>
    ''' <param name="TypeRequete">Le champ sur lequel s'effectue la requête à l'active directory.</param>
    ''' <param name="CritereRecherche">Le critère de recherche à l'active directory.</param>
    ''' <returns>Table qui contient les informations sur les membres.</returns>
    <Obsolete(Desuet.MEMBRE_NON_UTILISE, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Shared Function ObtenirInformationsMembres(ByVal TypeRequete As TsIadTypeRequete, ByVal CritereRecherche As String) As DataTable
        Dim Table As DataTable

        Table = ObtenirInformationsMembres(TypeRequete, CritereRecherche, String.Empty, TsIadObjectCategory.TsIadOcTous)

        Return Table
    End Function

    ''' <summary>
    '''   Cet méthode retourne les informations sur des membres de l'active directory dans une datatable, selon des paramètres de recherche
    '''   spécifiés en paramètres.
    ''' </summary>
    ''' <param name="TypeRequete">Le champ sur lequel s'effectue la requête à l'active directory.</param>
    ''' <param name="CritereRecherche">Le critère de recherche à l'active directory.</param>
    ''' <param name="CritereRechercheSecondaire">Le critère de recherche secondaire à l'active directory utilisé lors d'une recherche nom et prénom.</param>
    ''' <returns>Table qui contient les informations sur les membres.</returns>
    <Obsolete(Desuet.MEMBRE_NON_UTILISE, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Shared Function ObtenirInformationsMembres(ByVal TypeRequete As TsIadTypeRequete, ByVal CritereRecherche As String, ByVal CritereRechercheSecondaire As String) As DataTable
        Dim Table As DataTable

        Table = ObtenirInformationsMembres(TypeRequete, CritereRecherche, CritereRechercheSecondaire, TsIadObjectCategory.TsIadOcTous)

        Return Table
    End Function

    ''' <summary>
    '''   Permet d'obtenir les informations sur les utilisateurs qui font partie du groupe passé en paramètre.
    ''' </summary>
    ''' <param name="NomGroupe">Nom du groupe dont on veut obtenir les membres.</param>
    ''' <returns>Une table qui contient les informations sur les utilisateurs d'un groupe.</returns>
    <Obsolete(Desuet.MEMBRE_NON_UTILISE, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Shared Function ObtenirInformationsUtilisateursGroupe(ByVal NomGroupe As String) As DataTable
        Dim Membres As New DataSet
        Dim Vue As DataView

        If NomGroupe <> String.Empty Then
            Membres = ObtenirInformationsMembresGroupe(NomGroupe)
        Else
            Throw New TsCuParametreAbsentException("NomGroupe")
        End If

        Vue = Membres.Tables("Utilisateurs").DefaultView
        Vue.Sort = "sAMAccountName"

        Return Vue.ToTable()
    End Function

    ''' <summary>
    '''   Permet d'obtenir les informations sur les utilisateurs qui font partie du groupe passé en paramètre.
    ''' </summary>
    ''' <param name="NomGroupe">Nom du groupe dont on veut obtenir les membres.</param>
    ''' <param name="EstRecursif">
    '''   Si EstRecursif est a "True", on va obtenir les utilisateurs 
    '''   qui sont dans les sous-groupe du groupe passé en paramètre.
    ''' </param>
    ''' <returns>Une table qui contient les informations sur les utilisateurs d'un groupe.</returns>
    <Obsolete(Desuet.MEMBRE_NON_UTILISE, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Shared Function ObtenirInformationsUtilisateursGroupe(ByVal NomGroupe As String, ByVal EstRecursif As Boolean) As DataTable
        Dim TableUtilisateurs As DataTable
        Dim Vue As DataView

        TableUtilisateurs = ObtenirInformationsUtilisateursSousGroupe(NomGroupe, EstRecursif)

        Vue = TableUtilisateurs.DefaultView
        Vue.Sort = "sAMAccountName"

        TableUtilisateurs = Vue.ToTable(True)

        Return TableUtilisateurs
    End Function

    ''' <summary>
    '''   Cette méthode permet d'obtenir une liste de groupe de l'AD débutant par le filtre
    '''   passé en paramètre.
    ''' </summary>
    ''' <param name="Filtre">Filtre a appliquer pour la recherche.</param>
    ''' <returns>Retourne la liste des groupe correspondant au filtre.</returns>
    <Obsolete(Desuet.MEMBRE_NON_UTILISE, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Shared Function ObtenirListeGroupesDebutePar(ByVal Filtre As String) As List(Of String)
        Dim Resultats As SearchResultCollection
        Dim ListeGroupe As New List(Of String)
        Dim ProprieteAccountName As Object

        If Filtre <> String.Empty Then
            Resultats = RechercherListeGroupesAD(Filtre & "*", "sAMAccountName")

            If Resultats IsNot Nothing Then
                For Each Groupe As SearchResult In Resultats
                    ProprieteAccountName = ObtenirValeurPropriete(Groupe.Properties, "sAMAccountName")

                    If ProprieteAccountName IsNot Nothing AndAlso ProprieteAccountName.ToString <> String.Empty Then
                        ListeGroupe.Add(ProprieteAccountName.ToString)
                    End If
                Next
            End If
        Else
            Throw New TsCuParametreAbsentException("Filtre")
        End If

        Return ListeGroupe
    End Function

    ''' <summary>
    '''   Permet d'obtenir la liste des groupes d'un utilisateur.
    ''' </summary>
    ''' <param name="CodeUtilisateur">Code de l'utilisateur dont on veut obtenir les groupe.</param>
    ''' <returns>La liste des groupes d'un utilisateur.</returns>
    <Obsolete(Desuet.MEMBRE_NON_UTILISE, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Shared Function ObtenirListeGroupesUtilisateur(ByVal CodeUtilisateur As String) As List(Of String)
        Dim ListeGroupes As New List(Of String)
        Dim Utilisateur As SearchResult

        If CodeUtilisateur IsNot Nothing AndAlso CodeUtilisateur <> String.Empty Then
            Utilisateur = RechercherUtilisateurAD(CodeUtilisateur, "sAMAccountName", "MemberOf")
        Else
            Throw New TsCuParametreAbsentException("CodeUtilisateur")
        End If

        If Utilisateur IsNot Nothing Then
            ListeGroupes = ObtenirValeurProprieteMemberOf(Utilisateur.Properties)
        Else
            Throw New TsCuCodeUtilisateurInexistantException(CodeUtilisateur)
        End If

        Return ListeGroupes
    End Function

    ''' <summary>
    '''   Permet d'obtenir la liste des groupes d'un utilisateur.
    ''' </summary>
    ''' <param name="EstRecursif">Si la recherche est récursive, alors on doit obtenir les groupes qui font partie des groupe dont l'utilisateur est membre.</param>
    ''' <param name="CodeUtilisateur">Code de l'utilisateur dont on veut obtenir les groupe.</param>
    ''' <returns>La liste des groupes d'un utilisateur.</returns>
    <Obsolete(Desuet.MEMBRE_NON_UTILISE, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Shared Function ObtenirListeGroupesUtilisateur(ByVal CodeUtilisateur As String, ByVal EstRecursif As Boolean) As List(Of String)
        Dim ListeGroupes As List(Of String)
        Dim ListeGroupesRetour As New List(Of String)

        ListeGroupes = ObtenirListeGroupesUtilisateur(CodeUtilisateur)

        If ListeGroupes IsNot Nothing AndAlso ListeGroupes.Count > 0 Then
            ListeGroupesRetour.AddRange(ListeGroupes)
        End If

        If EstRecursif = True Then
            If ListeGroupes IsNot Nothing Then
                For Each Groupe As String In ListeGroupes
                    AjouterListeSousGroupe(Groupe, ListeGroupesRetour)
                Next
            End If
        End If

        ListeGroupesRetour.Sort()

        Return ListeGroupesRetour
    End Function

#End Region

#Region "Méthodes privées"

    ''' <summary>
    '''   Ajouter les sous-groupe d'un groupe dans une liste.
    ''' </summary>
    ''' <param name="NomGroupe">Nom du groupe dont on veut obtenir les sous-groupe.</param>
    ''' <param name="ListeTotal">Liste dans lequel on va ajouter les sous-groupe.</param>
    Private Shared Sub AjouterListeSousGroupe(ByVal NomGroupe As String, ByRef ListeTotal As List(Of String))
        Dim GroupeSID As String
        Dim Resultats As SearchResultCollection
        Dim ProprieteAccountName As Object

        GroupeSID = ObtenirSIDGroupeAD(NomGroupe)

        If GroupeSID <> String.Empty Then
            Resultats = RechercherToutAD(GroupeSID.ToString, "sAMAccountName")

            If Resultats IsNot Nothing Then
                For Each Resultat As SearchResult In Resultats
                    ProprieteAccountName = ObtenirValeurPropriete(Resultat.Properties, "sAMAccountName")

                    If ProprieteAccountName IsNot Nothing AndAlso ProprieteAccountName.ToString <> String.Empty Then
                        If ListeTotal.Contains(ProprieteAccountName.ToString) = False Then
                            ListeTotal.Add(ProprieteAccountName.ToString)
                        End If
                    End If
                Next
            End If
        End If
    End Sub

    ''' <summary>
    '''   Convertis les résultats de la recherche en datatable.
    ''' </summary>
    ''' <param name="SearchResults">Les résultats de la recherche.</param>
    ''' <returns>Une table qui contient les résultats de la recherche.</returns>
    Private Shared Function ConvertirEnDatatable(ByVal SearchResults As SearchResultCollection) As DataTable
        Dim Datatable As DataTable

        Datatable = ObtenirStructureTableMembre(String.Empty)

        For Each Result As SearchResult In SearchResults
            Datatable.Rows.Add(ConvertirSearchResultDatarow(Result, Datatable))
        Next

        Return Datatable
    End Function

    ''' <summary>
    '''   Charger les propriétés qui seront retournés lors de la recherche.
    ''' </summary>
    ''' <param name="Searcher">Objet pour rechercher dans l'AD.</param>
    Private Shared Sub ChargerProprietesAD(ByRef Searcher As DirectorySearcher)
        ChargerProprietesAD(Searcher, "sAMAccountName", "Sn", "GivenName", "DisplayName", "Mail", "Department", "Title", "MemberOf", "ObjectSid", "company", "description", "objectClass")
    End Sub

    ''' <summary>
    '''   Charger les propriétés que l'on veut retourner.
    ''' </summary>
    ''' <param name="Searcher">Objet pour rechercher dans l'AD.</param>
    ''' <param name="ListeProprietes">Liste des propriétés que l'on veut retourner.</param>
    Private Shared Sub ChargerProprietesAD(ByRef Searcher As DirectorySearcher, ByVal ParamArray ListeProprietes() As String)
        If Searcher IsNot Nothing Then
            If ListeProprietes IsNot Nothing Then
                For Each NomPropriete As String In ListeProprietes
                    Searcher.PropertiesToLoad.Add(NomPropriete)
                Next
            End If
        End If
    End Sub

    ''' <summary>
    '''   Convertir un tableau de bytes en string.
    ''' </summary>
    ''' <param name="Bytes">Tableau de bytes a convertir.</param>
    ''' <returns>Tableau de bytes converti en chaîne de caractères.</returns>
    Private Shared Function ConvertirOctetString(ByVal Bytes As Byte()) As String
        Dim TexteBytes As New StringBuilder
        Dim i As Integer = 0

        If Bytes IsNot Nothing Then
            While i < Bytes.Length
                TexteBytes.AppendFormat("\{0}", Bytes(i).ToString("X2"))
                i += 1
            End While
        End If

        Return TexteBytes.ToString
    End Function

    ''' <summary>
    '''   Convertir un objet searchResult en Datarow.
    ''' </summary>
    ''' <param name="Result">Objet a convertir.</param>
    ''' <param name="Table">Table dont on veut créer une nouvelle ligne.</param>
    ''' <returns>Une ligne de la table passé en paramètre.</returns>
    Private Shared Function ConvertirSearchResultDatarow(ByVal Result As SearchResult, ByVal Table As DataTable) As DataRow
        Dim Ligne As DataRow

        Ligne = Table.NewRow

        Ligne.Item("sAMAccountName") = ObtenirValeurPropriete(Result.Properties, "sAMAccountName")
        Ligne.Item("Sn") = ObtenirValeurPropriete(Result.Properties, "Sn")
        Ligne.Item("GivenName") = ObtenirValeurPropriete(Result.Properties, "GivenName")
        Ligne.Item("DisplayName") = ObtenirValeurPropriete(Result.Properties, "DisplayName")
        Ligne.Item("Mail") = ObtenirValeurPropriete(Result.Properties, "Mail")
        Ligne.Item("Department") = ObtenirValeurPropriete(Result.Properties, "Department")
        Ligne.Item("Title") = ObtenirValeurPropriete(Result.Properties, "Title")
        Ligne.Item("MemberOf") = ConvertirTableauEnChaineCaracteres(ObtenirValeurProprieteMemberOf(Result.Properties))
        Ligne.Item("ObjectSid") = ConvertirSid(CType(ObtenirValeurPropriete(Result.Properties, "ObjectSid"), Byte()))
        Ligne.Item("company") = ObtenirValeurPropriete(Result.Properties, "company")
        Ligne.Item("description") = ObtenirValeurPropriete(Result.Properties, "description")
        Ligne.Item("objectClass") = ObtenirValeurProprieteObjectClass(Result.Properties)

        Return Ligne
    End Function

    ''' <summary>
    '''     Cette fonction convertie le Sid en chaine de caractère.
    ''' </summary>
    ''' <param name="Sid">
    ''' 	Tableau de Bytes qui contient le Sid d'un compte dans l'AD. 
    ''' </param>
    ''' <returns>Le Sid en chaine de caractère.</returns>
    Private Shared Function ConvertirSid(ByRef Sid As Byte()) As String
        Dim Cle As String = String.Empty
        Dim Octet As String

        If Sid IsNot Nothing Then
            ' Convertir le SID en string
            For Cpt As Integer = LBound(Sid) To UBound(Sid)
                If Sid(Cpt) < &H10 Then
                    Octet = "\0" & Hex(Sid(Cpt))
                Else
                    Octet = "\" & Hex(Sid(Cpt))
                End If
                Cle = Cle & Octet
            Next Cpt
        End If

        Return Cle
    End Function

    ''' <summary>
    '''   Converti une liste en chaîne de caractères, les élément du tableau sont séparés par ;.
    ''' </summary>
    ''' <param name="Liste">Liste a convertir.</param>
    ''' <returns>Une chaîne de caractères dont les élément sont séparés par ;</returns>
    Private Shared Function ConvertirTableauEnChaineCaracteres(ByVal Liste As List(Of String)) As String
        Dim TableauConvertit As String = String.Empty

        If Liste IsNot Nothing Then
            For Each ElementTableau As String In Liste
                TableauConvertit &= ElementTableau

                If Liste.IndexOf(ElementTableau) < Liste.Count - 1 Then
                    TableauConvertit &= ";"
                End If
            Next
        End If

        Return TableauConvertit
    End Function

    ''' <summary>
    '''   Compose le filtre la requete LDAP avec les paramètres.
    ''' </summary>
    ''' <param name="TypeRequete">Le champ sur lequel s'effectue la requête à l'active directory.</param>
    ''' <param name="Categorie">Filtre sur ObjectCategory</param>
    ''' <param name="CritereRecherche">Le critère de recherche à l'active directory.</param>
    ''' <param name="CritereSecondaire">Le critère de recherche secondaire à l'active directory utilisé lors d'une recherche nom et prénom.</param>
    ''' <returns></returns>
    Private Shared Function CreerRequeteLDAP(ByVal TypeRequete As TsIadTypeRequete, ByVal Categorie As TsIadObjectCategory, ByVal CritereRecherche As String, ByVal CritereSecondaire As String) As String
        Dim query As String = "{0}"
        If Categorie = TsIadObjectCategory.TsIadOcPerson Then
            query = "(&(objectCategory=person){0})"
        ElseIf Categorie = TsIadObjectCategory.TsIadOcGroup Then
            query = "(&(objectCategory=group){0})"
        End If

        Dim specificCriteria As String
        If TypeRequete = TsIadTypeRequete.TsIadTrNomEtPrenom Then
            specificCriteria = String.Format("(&(Sn={0})(GivenName={1}))", CritereRecherche, CritereSecondaire)
        Else
            specificCriteria = String.Format("({0}={1})", ObtenirNomCritereRecherche(TypeRequete), CritereRecherche)
        End If

        Return String.Format(query, specificCriteria)
    End Function

    ''' <summary>
    '''   Obtient les informations contenu dans l'AD sur les membres d'un groupe.
    ''' </summary>
    ''' <param name="NomGroupe">Nom du groupe dont on veut les informations sur ses membres situé dans l'AD </param>
    ''' <returns>Un dataset contenant une ligne avec les informations sur les membres.</returns>
    Private Shared Function ObtenirInformationsMembresGroupe(ByVal NomGroupe As String) As DataSet
        Dim TableGroupes As DataTable
        Dim TableUtilisateurs As DataTable
        Dim Row As DataRow
        Dim ListeMembres As SearchResultCollection
        Dim DatasetRetour As DataSet
        Dim TypeMembre As String
        Dim DateDebut As Date = Now

        TableGroupes = ObtenirStructureTableMembre("Groupes")
        TableUtilisateurs = ObtenirStructureTableMembre("Utilisateurs")

        ListeMembres = RechercherMembresGroupeAD(NomGroupe)

        For Each Membre As SearchResult In ListeMembres
            TypeMembre = ObtenirValeurProprieteObjectClass(Membre.Properties).ToString

            If TypeMembre = "user" Then
                Row = ConvertirSearchResultDatarow(Membre, TableUtilisateurs)
                TableUtilisateurs.Rows.Add(Row)

            ElseIf TypeMembre = "group" Then
                Row = ConvertirSearchResultDatarow(Membre, TableGroupes)
                TableGroupes.Rows.Add(Row)
            End If
        Next

        DatasetRetour = New DataSet
        DatasetRetour.Tables.Add(TableUtilisateurs)
        DatasetRetour.Tables.Add(TableGroupes)

        Return DatasetRetour
    End Function

    ''' <summary>
    '''    Permet d'obtenir les informations sur les utilisateurs qui font partie du groupe passé en paramètre.
    ''' </summary>
    ''' <param name="NomGroupe">Nom du groupe dont on veut obtenir les informations sur les utilisateurs.</param>
    ''' <param name="EstRecursif">
    '''    Si la recherche est récursive, on doit obtenir les utilisateurs des groupes qui sont membre
    '''    du groupe passé en paramètre.
    ''' </param>
    ''' <returns>Une table qui contient les informations sur les utilisateurs d'un groupe.</returns>
    <Obsolete>
    Private Shared Function ObtenirInformationsUtilisateursSousGroupe(ByVal NomGroupe As String, ByVal EstRecursif As Boolean) As DataTable
        Dim Utilisateurs As DataTable
        Dim Membres As New DataSet

        If NomGroupe <> String.Empty Then
            Membres = ObtenirInformationsMembresGroupe(NomGroupe)
        Else
            Throw New TsCuParametreAbsentException("NomGroupe")
        End If

        Utilisateurs = Membres.Tables("Utilisateurs")

        If EstRecursif Then
            For Each Row As DataRow In Membres.Tables("Groupes").Rows
                Dim UtilisateursTemp As DataTable

                UtilisateursTemp = ObtenirInformationsUtilisateursGroupe(Row.Item("sAMAccountName").ToString, True)

                Utilisateurs.Merge(UtilisateursTemp)
            Next
        End If

        Return Utilisateurs
    End Function

    ''' <summary>
    '''    Permet d'obtenir les informations sur les groupes qui font partie du groupe passé en paramètre.
    ''' </summary>
    ''' <param name="NomGroupe">Nom du groupe dont on veut obtenir les informations sur les sous groupes.</param>
    ''' <param name="EstRecursif">
    '''    Si la recherche est récursive, on doit obtenir les groupes des groupes qui sont membre
    '''    du groupe passé en paramètre.
    ''' </param>
    ''' <returns>Une table qui contient les informations sur les groupes d'un groupe.</returns>
    <Obsolete>
    Private Shared Function ObtenirInformationsGroupesSousGroupe(ByVal NomGroupe As String, ByVal EstRecursif As Boolean) As DataTable
        Dim Groupes As DataTable
        Dim Membres As New DataSet

        If NomGroupe <> String.Empty Then
            Membres = ObtenirInformationsMembresGroupe(NomGroupe)
        Else
            Throw New TsCuParametreAbsentException("NomGroupe")
        End If

        Groupes = Membres.Tables("Groupes").Copy()

        If EstRecursif Then
            For Each Row As DataRow In Membres.Tables("Groupes").Rows
                Dim GroupesTemp As DataTable

                GroupesTemp = ObtenirInformationsGroupesGroupe(Row.Item("sAMAccountName").ToString(), True)

                Groupes.Merge(GroupesTemp)
            Next
        End If

        Return Groupes
    End Function

    ''' <summary>
    '''   Obtien la propriété de l'AD qui correspond a l'énumération.
    ''' </summary>
    ''' <param name="TypeRequete">Énumération qui correspond a une propriété de l'AD.</param>
    ''' <returns>nom de la propriété.</returns>
    Private Shared Function ObtenirNomCritereRecherche(ByVal TypeRequete As TsIadTypeRequete) As String
        Dim DateDebut As Date = Now

        Select Case TypeRequete
            Case TsIadTypeRequete.TsIadTrCodeUtilisateur
                Return "sAMAccountName"

            Case TsIadTypeRequete.TsIadTrCourriel
                Return "Mail"

            Case TsIadTypeRequete.TsIadTrDescription
                Return "Description"

            Case TsIadTypeRequete.TsIadTrFonction
                Return "Title"

            Case TsIadTypeRequete.TsIadTrMembreDe
                Return "MemberOf"

            Case TsIadTypeRequete.TsIadTrNom
                Return "Sn"

            Case TsIadTypeRequete.TsIadTrNomComplet
                Return "DisplayName"

            Case TsIadTypeRequete.TsIadTrPrenom
                Return "GivenName"

            Case TsIadTypeRequete.TsIadTrSid
                Return "ObjectSid"

            Case TsIadTypeRequete.TsIadTrSociete
                Return "company"

            Case TsIadTypeRequete.TsIadTrUniteAdmn
                Return "Department"

            Case TsIadTypeRequete.TsIadTrNoEmploye
                Return "employeeNumber"

            Case Else
                Return String.Empty
        End Select
    End Function

    ''' <summary>
    '''   Obtient le sid correspond a un groupe de l'AD.
    ''' </summary>
    ''' <param name="NomGroupe">Nom du groupe dont on veut obtenir le SID.</param>
    ''' <returns>Le sid correspondant a un groupe de l'AD.</returns>
    Private Shared Function ObtenirSIDGroupeAD(ByVal NomGroupe As String) As String
        Dim Groupe As DirectoryEntry = Nothing
        Dim GroupeSID As New StringBuilder
        Dim DateDebut As Date = Now
        Dim Resultat As SearchResult

        Resultat = RechercherGroupeAD(NomGroupe)

        If Resultat IsNot Nothing Then
            Groupe = Resultat.GetDirectoryEntry
        End If

        If Groupe IsNot Nothing Then
            GroupeSID.Append("(|")

            Groupe.RefreshCache(New String() {"tokenGroups"})

            For Each sid As Byte() In Groupe.Properties("tokenGroups")
                GroupeSID.AppendFormat("(objectSid={0})", ConvertirOctetString(sid))
            Next

            GroupeSID.Append(")")

            If GroupeSID.ToString = "(|)" Then
                Return String.Empty
            Else
                Return GroupeSID.ToString
            End If
        End If

        Return String.Empty
    End Function

    ''' <summary>
    '''   Construit la structure de la datatable qui est prête à recevoir des entree de l'AD.
    ''' </summary>
    ''' <param name="NomTable"></param>
    ''' <returns>Un datatable qui est prête à recevoir des entree de l'AD.</returns>
    Private Shared Function ObtenirStructureTableMembre(ByVal NomTable As String) As DataTable
        Dim Table As DataTable
        Dim TypeString As System.Type
        Dim DateDebut As Date = Now

        Table = New DataTable(NomTable)

        TypeString = System.Type.GetType("System.String")

        Table.Columns.Add("sAMAccountName", TypeString)
        Table.Columns.Add("Sn", TypeString)
        Table.Columns.Add("GivenName", TypeString)
        Table.Columns.Add("DisplayName", TypeString)
        Table.Columns.Add("Mail", TypeString)
        Table.Columns.Add("Department", TypeString)
        Table.Columns.Add("Title", TypeString)
        Table.Columns.Add("MemberOf", TypeString)
        Table.Columns.Add("ObjectSid", TypeString)
        Table.Columns.Add("company", TypeString)
        Table.Columns.Add("description", TypeString)
        Table.Columns.Add("objectClass", TypeString)

        Return Table
    End Function

    ''' <summary>
    '''   Obtient la valeur d'une propriété d'un objet searchresult.
    ''' </summary>
    ''' <param name="Propriete">Propriété d'un objet searchresult.</param>
    ''' <param name="NomPropriete">Nom de la propriété.</param>
    ''' <returns>La valeur d'une propriété d'un objet searchresult.</returns>
    Private Shared Function ObtenirValeurPropriete(ByVal Propriete As ResultPropertyCollection, ByVal NomPropriete As String) As Object
        Return ObtenirValeurPropriete(Propriete, NomPropriete, 0)
    End Function

    ''' <summary>
    '''   Obtient la valeur d'une propriété d'un objet searchresult.
    ''' </summary>
    ''' <param name="Propriete">Propriété d'un objet searchresult.</param>
    ''' <param name="NomPropriete">Nom de la propriété.</param>
    ''' <param name="IndexValeur">Index du tableau si il y a plusieur valeurs dans la propriété.</param>
    ''' <returns>La valeur d'une propriété d'un objet searchresult.</returns>
    Private Shared Function ObtenirValeurPropriete(ByVal Propriete As ResultPropertyCollection, ByVal NomPropriete As String, ByVal IndexValeur As Integer) As Object
        Dim ValeurPropriete As Object = Nothing

        If Propriete IsNot Nothing AndAlso Propriete(NomPropriete) IsNot Nothing Then
            If Propriete(NomPropriete).Count >= IndexValeur + 1 Then
                ValeurPropriete = Propriete(NomPropriete).Item(IndexValeur)
            End If
        End If

        Return ValeurPropriete
    End Function

    ''' <summary>
    '''   Obtient la valeur de la propriété MemberOf d'un SearchResult.
    ''' </summary>
    ''' <param name="Propriete">Propriété d'un objet searchresult.</param>
    ''' <returns>La valeur de la propriété MemberOf d'un SearchResult.</returns>
    Private Shared Function ObtenirValeurProprieteMemberOf(ByVal Propriete As ResultPropertyCollection) As List(Of String)
        Dim ListeMembre As List(Of String)
        Dim ProprieteMemberOf As ResultPropertyValueCollection
        Dim Groupe As String

        ListeMembre = New List(Of String)

        ProprieteMemberOf = Propriete("MemberOf")

        If ProprieteMemberOf IsNot Nothing Then
            If ProprieteMemberOf.Count > 0 Then
                For i As Integer = 0 To ProprieteMemberOf.Count - 1
                    Groupe = ProprieteMemberOf.Item(i).ToString
                    ListeMembre.Add(Groupe.Split(","c)(0).Substring(3))
                Next
            End If
        End If

        ListeMembre.Sort()

        Return ListeMembre
    End Function

    ''' <summary>
    '''   Obtient la valeur de la propriété MemberOf d'un SearchResult.
    ''' </summary>
    ''' <param name="Propriete">Propriété d'un objet searchresult.</param>
    ''' <param name="EnMajuscule">Est-ce que tout les éléments doivent être en majuscule.</param>
    ''' <returns>La valeur de la propriété MemberOf d'un SearchResult.</returns>
    Private Shared Function ObtenirValeurProprieteMemberOf(ByVal Propriete As ResultPropertyCollection, ByVal EnMajuscule As Boolean) As List(Of String)
        Dim ListeMembre As List(Of String)
        Dim ProprieteMemberOf As ResultPropertyValueCollection
        Dim Groupe As String

        ListeMembre = New List(Of String)

        ProprieteMemberOf = Propriete("MemberOf")

        If ProprieteMemberOf IsNot Nothing Then
            If ProprieteMemberOf.Count > 0 Then
                For i As Integer = 0 To ProprieteMemberOf.Count - 1
                    If EnMajuscule = True Then
                        Groupe = ProprieteMemberOf.Item(i).ToString.ToUpper
                    Else
                        Groupe = ProprieteMemberOf.Item(i).ToString
                    End If

                    ListeMembre.Add(Groupe.Split(","c)(0).Substring(3))
                Next
            End If
        End If

        ListeMembre.Sort()

        Return ListeMembre
    End Function

    ''' <summary>
    '''   Obtient la valeur de la propriété ObjectClass d'un SearchResult.
    ''' </summary>
    ''' <param name="Propriete">Propriété d'un objet searchresult.</param>
    ''' <returns>La valeur de la propriété ObjectClass d'un SearchResult.</returns>
    Private Shared Function ObtenirValeurProprieteObjectClass(ByVal Propriete As ResultPropertyCollection) As Object
        Dim ValeurPropriete As Object = Nothing

        If Propriete IsNot Nothing AndAlso Propriete("ObjectClass") IsNot Nothing Then
            If Propriete("ObjectClass").Count = 4 Then
                ValeurPropriete = Propriete("ObjectClass").Item(3)
            ElseIf Propriete("ObjectClass").Count = 2 Then
                ValeurPropriete = Propriete("ObjectClass").Item(1)
            End If
        End If

        Return ValeurPropriete
    End Function

    ''' <summary>
    '''   Cette méthode retourne une liste de groupe contenu dans l'AD selon un filtre.
    ''' </summary>
    ''' <param name="FiltreNomGroupe">Filtre a appliquer pour la rechercher dans l'AD.</param>
    ''' <param name="ListeProprietesRetour"></param>
    ''' <returns>Une collection contenant groupes trouvés.</returns>
    Private Shared Function RechercherListeGroupesAD(ByVal FiltreNomGroupe As String, ByVal ParamArray ListeProprietesRetour() As String) As SearchResultCollection
        Dim Requete As String

        Requete = String.Format("(&(objectCategory=group)(sAMAccountName={0}))", FiltreNomGroupe)

        Return RechercherToutAD(Requete, ListeProprietesRetour)
    End Function

    ''' <summary>
    '''   Rechercher les membres d'un groupe de l'AD.
    ''' </summary>
    ''' <param name="NomGroupe">Nom du groupe recherché.</param>
    ''' <returns>Les membres d'un groupe de l'AD.</returns>
    Private Shared Function RechercherMembresGroupeAD(ByVal NomGroupe As String) As SearchResultCollection
        Return RechercherMembresGroupeAD(NomGroupe, Nothing)
    End Function

    ''' <summary>
    '''   Rechercher les membres d'un groupe de l'AD.
    ''' </summary>
    ''' <param name="NomGroupe">Nom du groupe recherché.</param>
    ''' <param name="ListeProprietesRetour">Liste des propriétés retournées par la recherche.</param>
    ''' <returns>Les membres d'un groupe de l'AD.</returns>
    Private Shared Function RechercherMembresGroupeAD(ByVal NomGroupe As String, ByVal ParamArray ListeProprietesRetour() As String) As SearchResultCollection
        Dim Requete As String
        Dim Groupe As SearchResult = RechercherGroupeAD(NomGroupe, "distinguishedName")

        Requete = String.Format("(&(|(objectCategory=person)(objectCategory=group))(memberof={0}))", ObtenirValeurPropriete(Groupe.Properties, "distinguishedName"))

        Return RechercherToutAD(Requete, ListeProprietesRetour)
    End Function

    ''' <summary>
    '''   Rechercher les groupes d'un utilisateur de l'AD.
    ''' </summary>
    ''' <param name="CodeUtilisateur">Utilisateur dont on veut obtenir les groupes.</param>
    ''' <param name="NomGroupe">Nom du groupe</param>
    ''' <param name="EstRecursif">Si EstRecursif est True on doit obtenir les sous-groupes contenu dans les groupe de l'utilisateur.</param>
    ''' <returns>Les groupes d'un utilisateur de l'AD.</returns>
    Private Shared Function RechercherGroupeUtilisateur(ByVal CodeUtilisateur As String, ByVal NomGroupe As String, ByVal EstRecursif As Boolean) As Boolean
        Dim ListeGroupes As List(Of String)

        ListeGroupes = ObtenirListeGroupesUtilisateurMajuscule(CodeUtilisateur)

        If ListeGroupes.Contains(NomGroupe.ToUpper) Then
            Return True
        End If

        If EstRecursif = True Then
            If ListeGroupes IsNot Nothing Then
                For Each Groupe As String In ListeGroupes
                    If RechercherDansSousGroupe(Groupe, NomGroupe) = True Then
                        Return True
                    End If
                Next
            End If
        End If

        Return False
    End Function

    ''' <summary>
    '''   Recherche un groupe dans les membres d'un groupe.
    ''' </summary>
    ''' <param name="NomGroupeListe">Groupe dans lequel on va rechercher un groupe.</param>
    ''' <param name="NomGroupeRechercher">Nom du groupe recherché</param>
    ''' <returns>True si le groupe est trouvé et False si ce n'est pas le cas.</returns>
    Private Shared Function RechercherDansSousGroupe(ByVal NomGroupeListe As String, ByVal NomGroupeRechercher As String) As Boolean
        Dim GroupeSID As String
        Dim Resultats As SearchResultCollection
        Dim ProprieteAccountName As String

        GroupeSID = ObtenirSIDGroupeAD(NomGroupeListe)

        If GroupeSID <> String.Empty Then
            Resultats = RechercherToutAD(GroupeSID.ToString, "sAMAccountName")

            If Resultats IsNot Nothing Then
                For Each Resultat As SearchResult In Resultats
                    ProprieteAccountName = ObtenirValeurPropriete(Resultat.Properties, "sAMAccountName").ToString.ToUpper

                    If ProprieteAccountName IsNot Nothing AndAlso ProprieteAccountName <> String.Empty Then
                        If ProprieteAccountName = NomGroupeRechercher.ToUpper Then
                            Return True
                        End If
                    End If
                Next
            End If
        End If

        Return False
    End Function

    ''' <summary>
    '''   Obtenir les groupes d'un utilisateur en majuscule.
    ''' </summary>
    ''' <param name="CodeUtilisateur">Code utilisateur dont on veut obtenir les groupes.</param>
    ''' <returns>Les groupes d'un utilisateur en majuscule.</returns>
    Private Shared Function ObtenirListeGroupesUtilisateurMajuscule(ByVal CodeUtilisateur As String) As List(Of String)
        Dim ListeGroupes As New List(Of String)
        Dim Utilisateur As SearchResult

        If CodeUtilisateur IsNot Nothing AndAlso CodeUtilisateur <> String.Empty Then
            Utilisateur = RechercherUtilisateurAD(CodeUtilisateur, "sAMAccountName", "MemberOf")
        Else
            Throw New TsCuParametreAbsentException("CodeUtilisateur")
        End If

        If Utilisateur IsNot Nothing Then
            ListeGroupes = ObtenirValeurProprieteMemberOf(Utilisateur.Properties, True)
        Else
            Throw New TsCuCodeUtilisateurInexistantException(CodeUtilisateur)
        End If

        Return ListeGroupes
    End Function

    ''' <summary>
    '''   Recherche un groupe de l'AD.
    ''' </summary>
    ''' <param name="NomGroupe">Nom du groupe recherché.</param>
    ''' <returns>Un objet résultat de l'AD.</returns>
    Private Shared Function RechercherGroupeAD(ByVal NomGroupe As String) As SearchResult
        Return RechercherGroupeAD(NomGroupe, Nothing)
    End Function

    ''' <summary>
    '''   Recherche un groupe de l'AD.
    ''' </summary>
    ''' <param name="NomGroupe">Nom du groupe recherché.</param>
    ''' <param name="ListeProprietesRetour">Liste des propriétés retournées par la recherche.</param>
    ''' <returns>Un objet résultat de l'AD.</returns>
    Private Shared Function RechercherGroupeAD(ByVal NomGroupe As String, ByVal ParamArray ListeProprietesRetour() As String) As SearchResult
        Dim Requete As String

        Requete = String.Format("(&(objectCategory=group)(sAMAccountName={0}))", NomGroupe)

        Return RechercherPremiereOccurenceAD(Requete, ListeProprietesRetour)
    End Function

    ''' <summary>
    '''   Retourne la première occurence trouvé correspondant au filtre. 
    ''' </summary>
    ''' <param name="Filtre">Filtre de recherche.</param>
    ''' <param name="ListeProprietesRetour">Liste des propriétés retournées par la recherche.</param>
    ''' <returns>Un objet résultat de l'AD.</returns>
    Private Shared Function RechercherPremiereOccurenceAD(ByVal Filtre As String, ByVal ParamArray ListeProprietesRetour() As String) As SearchResult
        Dim Resultat As SearchResult = Nothing

        Using DirectoryEntry As New DirectoryEntry()
            DirectoryEntry.AuthenticationType = AuthenticationTypes.Secure
            DirectoryEntry.Path = String.Format("LDAP://{0}", ServeurActiveDirectory())

            Using DirectorySearcher As New DirectorySearcher(DirectoryEntry)
                If ListeProprietesRetour Is Nothing Then
                    ChargerProprietesAD(DirectorySearcher)
                Else
                    ChargerProprietesAD(DirectorySearcher, ListeProprietesRetour)
                End If

                DirectorySearcher.Filter = Filtre
                DirectorySearcher.CacheResults = False

                Resultat = DirectorySearcher.FindOne()

                Return Resultat
            End Using
        End Using
    End Function

    ''' <summary>
    '''   Recherche toutes les entrées de l'AD qui correspondent au filtre en paramètre.
    ''' </summary> 
    ''' <param name="Filtre">Filtre de recherche.</param>
    ''' <param name="ListeProprietesRetour">Liste des propriétés retournées par la recherche.</param>
    ''' <returns>Une collection d'objet résultat de l'AD.</returns>
    Private Shared Function RechercherToutAD(ByVal Filtre As String, ByVal ParamArray ListeProprietesRetour() As String) As SearchResultCollection
        Dim Resultats As SearchResultCollection = Nothing

        Using DirectoryEntry As New DirectoryEntry()
            DirectoryEntry.AuthenticationType = AuthenticationTypes.Secure
            DirectoryEntry.Path = String.Format("LDAP://{0}", ServeurActiveDirectory())

            Using DirectorySearcher As New DirectorySearcher(DirectoryEntry)
                If ListeProprietesRetour Is Nothing Then
                    ChargerProprietesAD(DirectorySearcher)
                Else
                    ChargerProprietesAD(DirectorySearcher, ListeProprietesRetour)
                End If

                DirectorySearcher.Filter = Filtre
                DirectorySearcher.CacheResults = False
                DirectorySearcher.PageSize = 1000

                Resultats = DirectorySearcher.FindAll()

                Return Resultats
            End Using
        End Using
    End Function

    ''' <summary>
    '''   Recherche toutes les entrées de l'AD qui correspondent au filtre en paramètre.
    ''' </summary> 
    ''' <param name="Filtre">Filtre de recherche.</param>
    ''' <returns>Une collection d'objet résultat de l'AD.</returns>
    Private Shared Function RechercherToutAD(ByVal Filtre As String) As SearchResultCollection
        Return RechercherToutAD(Filtre, Nothing)
    End Function

    ''' <summary>
    '''   Permet de rechercher dans l'active directory sur un champ.
    ''' </summary>
    ''' <param name="TypeRequete">Le champs sur lequel on veut rechercher.</param>
    ''' <param name="Categorie">Tous, Utilisateur ou un Groupe</param>
    ''' <param name="CritereRecherche">Valeur a rechercher.</param>
    ''' <param name="CritereRechercheSecondaire">Seulement utilisé lors de la recherche sur le nom et prénom. Correspond au prénom.</param>
    ''' <returns>Une collection d'objet résultat de l'AD.</returns>
    Private Shared Function RechercherToutAD(ByVal TypeRequete As TsIadTypeRequete, ByVal Categorie As TsIadObjectCategory, ByVal CritereRecherche As String, ByVal CritereRechercheSecondaire As String) As SearchResultCollection
        Dim RequeteLDAP As String

        RequeteLDAP = CreerRequeteLDAP(TypeRequete, Categorie, CritereRecherche, CritereRechercheSecondaire)

        Return RechercherToutAD(RequeteLDAP)
    End Function

    ''' <summary>
    '''   Permet de rechercher un utilisateur dans l'active directory.
    ''' </summary>
    ''' <param name="CodeUtilisateur">Code utilisateur a rechercher.</param>
    ''' <param name="ListeProprietesRetour">Liste des propriétés retournées par la recherche.</param>
    ''' <returns>Un objet résultat de l'AD avec les propriétés demandés.</returns>
    Private Shared Function RechercherUtilisateurAD(ByVal CodeUtilisateur As String, ByVal ParamArray ListeProprietesRetour() As String) As SearchResult
        Dim Requete As String

        Requete = String.Format("(&(objectCategory=person)(sAMAccountName={0}))", CodeUtilisateur)

        Return RechercherPremiereOccurenceAD(Requete, ListeProprietesRetour)
    End Function

#End Region

End Class
