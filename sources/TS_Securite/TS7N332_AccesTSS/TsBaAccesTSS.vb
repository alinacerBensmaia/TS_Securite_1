Imports System.IO
Imports system.Text
Imports System.Linq

'''-----------------------------------------------------------------------------
''' Project		: TS7N332_AccesTSS
''' Class		: TsBaAccesTSS
'''
'''-----------------------------------------------------------------------------
''' <summary>
''' Cette classe sert de classe de service pour utiliser le fichier d'extraction TSS.
''' </summary>
''' <remarks></remarks>
'''-----------------------------------------------------------------------------
Public Class TsBaAccesTSS

#Region "Énumérations"

    ''' <summary>
    ''' Type possible tiré d'un fichier d'extraction TSS.
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum TypeTSS
        Inconnu
        Utilisateur
        Profile
    End Enum

#End Region

#Region "Constante"

    Private Const COMMANDE_AJOUT As String = "TSS ADDTO({0}) PROFILE({1}) BEFORE(PXXALL)"
    Private Const COMMANDE_RETRAIT As String = "TSS REMOVE({0}) PROFILE({1})"

#End Region

#Region "Variables privées"

    Private fichierExtraction As StreamReader
    Private dictioUtilisateurRessource As Dictionary(Of String, HashSet(Of String))
    Private fichierCommandes As StreamWriter

#End Region

#Region "Constructeurs"

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <param name="adresseFichierEntree">L'adresse du fichier où les inforamtions de Top Secret se situe.</param>
    ''' <param name="adresseFichierSortie">L'adresse ainsi que le nom du fichier où vous désirez que les commandes soient écrites.</param>
    ''' <remarks>Le fichier d'entrée peu ne pas exister, alors tous les ajouts, supressions et vérifications seront positifs.</remarks>
    Public Sub New(ByVal adresseFichierEntree As String, ByVal adresseFichierSortie As String)
        fichierCommandes = New StreamWriter(adresseFichierSortie, False)
        fichierCommandes.AutoFlush = True

        fichierExtraction = LireFichier(adresseFichierEntree)
        ConstruireDictioUtilstr()
        If fichierExtraction IsNot Nothing Then
            fichierExtraction.Dispose()
        End If
    End Sub

#End Region

#Region "Méthodes"

    ''' <summary>
    ''' Cette méthode va écrire une commande, dans le fichier de commandes, 
    ''' demandant de faire un ajout d'un profile à un utilisateur dans TSS.
    ''' </summary>
    ''' <param name="profile">Le profile à ajouter.</param>
    ''' <param name="utilisateur">L'utilisateur à qui lui sera ajouté le profile.</param>
    ''' <returns>Si l'ajout est possible = True, sinon = False</returns>
    ''' <remarks>
    ''' Dans le cas où des informations sont manquantes pour définir si l'on peut ajouter le profile,
    ''' la méthode va faire une commande d'ajout quand même. Dans le doute on fait la demande.
    ''' </remarks>
    Public Function AjouterProfileUtilisateur(ByVal profile As String, ByVal utilisateur As String) As Boolean
        Dim tousOK As Boolean

        If dictioUtilisateurRessource.ContainsKey(utilisateur) = True Then
            If dictioUtilisateurRessource(utilisateur).Contains(profile) = True Then
                tousOK = False
            Else
                tousOK = True
            End If
        Else
            tousOK = True
        End If

        If tousOK = True Then
            EcrireAjout(profile, utilisateur)
        End If

        Return tousOK
    End Function

    ''' <summary>
    ''' Cette méthode va écrire une commande, dans le fichier de commandes, 
    ''' demandant de faire un retrait d'un profile à un utilisateur dans TSS.
    ''' </summary>
    ''' <param name="profile">Le profile à ajouter.</param>
    ''' <param name="utilisateur">L'utilisateur à qui lui sera retiré le profile.</param>
    ''' <returns>Si le retrais est possible = True, sinon = False</returns>
    ''' <remarks>
    ''' Dans le cas où des informations sont manquantes pour définir si l'on peut retirer le profile,
    ''' la méthode va faire une commande de retrait quand même. Dans le doute on fait la demande.
    ''' </remarks>
    Public Function EnleverProfileUtilisateur(ByVal profile As String, ByVal utilisateur As String) As Boolean
        Dim tousOK As Boolean

        If dictioUtilisateurRessource.ContainsKey(utilisateur) = True Then
            If dictioUtilisateurRessource(utilisateur).Contains(profile) = True Then
                tousOK = True
            Else
                tousOK = False
            End If
        Else
            tousOK = True
        End If

        If tousOK = True Then
            EcrireRetrait(profile, utilisateur)
        End If

        Return tousOK
    End Function

    ''' <summary>
    ''' Cette méthode vérifie si le profile est déja associé à l'utilisateur.
    ''' </summary>
    ''' <param name="profile">Le profile recherché qui est associé à l'utilisateur.</param>
    ''' <param name="utilisateur">L'utilisateur où l'on vérifie si le profile y est associé.</param>
    ''' <returns>True, si le profile est associé, sinon False.</returns>
    ''' <remarks>
    ''' Dans le cas où des informations sont manquantes pour définir l'association, 
    ''' la méthode revoira un réponse négative, False.
    ''' </remarks>
    Public Function ProfileDejaAssocier(ByVal profile As String, ByVal utilisateur As String) As Boolean
        If dictioUtilisateurRessource.ContainsKey(utilisateur) = False Then
            Return False
        End If
        Return dictioUtilisateurRessource(utilisateur).Contains(profile)
    End Function

    ''' <summary>
    ''' Cette méthode vérifie si le profile est déja désassocié à l'utilisateur.
    ''' </summary>
    ''' <param name="profile">Le profile recherché qui est associé à l'utilisateur.</param>
    ''' <param name="utilisateur">L'utilisateur où l'on vérifie si le profile y est associé.</param>
    ''' <returns>True, si le profile est associé, sinon False.</returns>
    ''' <remarks>
    ''' Dans le cas où des informations sont manquantes pour définir l'association, 
    ''' la méthode revoira un réponse négative, False.
    ''' Pour quoi ne pas utiliser <see cref="ProfileDejaAssocier"/>, 
    ''' parce que en cas de manque d'information le travail doit être fait.
    ''' </remarks>
    Public Function ProfileDejaDesassocier(ByVal profile As String, ByVal utilisateur As String) As Boolean
        If dictioUtilisateurRessource.ContainsKey(utilisateur) = False Then
            Return False
        End If
        Return Not (dictioUtilisateurRessource(utilisateur).Contains(profile))
    End Function

    ''' <summary>
    ''' Méhtode qui renvois les utilisateurs du fichier de lecture.
    ''' </summary>
    ''' <returns>Liste d'utilisateur.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirUtilisateurs() As IEnumerable(Of String)
        Return dictioUtilisateurRessource.Keys
    End Function

    ''' <summary>
    ''' Méthode qui renvois les groupes associé à un utilisateur.
    ''' </summary>
    ''' <param name="uid">l'identifiant de l'utilisateur.</param>
    ''' <returns>Liste de groupes.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirGroupes(ByVal uid As String) As IEnumerable(Of String)
        Return dictioUtilisateurRessource(uid)
    End Function

    ''' <summary>
    ''' Cette méthode relache toutes les ressources qu'elle possède.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Dispose()
        fichierCommandes.Dispose()
    End Sub

#End Region

#Region "Fonctions de services"

    ''' <summary>
    ''' Fonction de service. Initialise un fichier de lecture.
    ''' </summary>
    ''' <param name="adresseFichier">L'adresse et le nom du fichier où sera lu le fichier d'extraction TSS.</param>
    ''' <returns>Renvois un objet StreamReader initialisé.</returns>
    ''' <remarks></remarks>
    ''' <exception cref="FileNotFoundException">Le fichier n'a pas été trouvé à l'adresse spécifiée.</exception>
    Private Function LireFichier(ByVal adresseFichier As String) As StreamReader
        If adresseFichier = Nothing Then
            Return Nothing
        End If

        Return New StreamReader(adresseFichier)
    End Function

    ''' <summary>
    ''' Fonction de service. Écrit dans le fichier de commande, la ligne de commande d'ajout d'un profile à l'utilisateur.
    ''' </summary>
    ''' <param name="profile">Le profile à associer à l'utilisateur.</param>
    ''' <param name="utilisateur">L'utilisateur à qui sera associé le profile.</param>
    ''' <remarks></remarks>
    Private Sub EcrireAjout(ByVal profile As String, ByVal utilisateur As String)
        fichierCommandes.WriteLine(String.Format(COMMANDE_AJOUT, New String() {utilisateur, profile}))
    End Sub

    ''' <summary>
    ''' Fonction de service. Écrit dans le fichier de commande la ligne de commande de retrait de profile à un utilisateur.
    ''' </summary>
    ''' <param name="profile">Le profile à retirer de l'utilisateur.</param>
    ''' <param name="utilisateur">L'utilisateur à qui sera retiré le profile.</param>
    ''' <remarks></remarks>
    Private Sub EcrireRetrait(ByVal profile As String, ByVal utilisateur As String)
        fichierCommandes.WriteLine(String.Format(COMMANDE_RETRAIT, New String() {utilisateur, profile}))
    End Sub

    ''' <summary>
    ''' Fonction d'initialisation. Cette fonction va contruire le dictionnaire des utilisateurs.
    ''' La fonctionne est dépendante de <see  cref="fichierExtraction" /> qui doit être préalablement initialisé.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ConstruireDictioUtilstr()
        dictioUtilisateurRessource = New Dictionary(Of String, HashSet(Of String))
        Dim bloc As String

        If fichierExtraction Is Nothing Then
            Exit Sub
        End If

        '! Lire les blocs
        While Not fichierExtraction.EndOfStream
            bloc = ObtenirProchainBloc(fichierExtraction)
            Select Case ObtenirType(bloc)
                Case TypeTSS.Utilisateur
                    AjouterUtilisateurDictio(bloc)
            End Select
        End While
    End Sub

    ''' <summary>
    ''' Fonction de service. Sous-fonction de <see cref="ConstruireDictioUtilstr" />.
    ''' Fait l'ajout d'un utilisateur au dictionnaire <see cref="dictioUtilisateurRessource" />
    ''' à partir d'un bloc d'inforamtions venant du fichier d'extraction TSS.
    ''' </summary>
    ''' <param name="bloc">Un bloc d'information d'un utilisateur.</param>
    ''' <remarks></remarks>
    Private Sub AjouterUtilisateurDictio(ByVal bloc As String)
        Dim key As String
        Dim ensembleValeurs As New HashSet(Of String)()

        key = ObtenirAccessorID(bloc)

        Dim regexProfile As New Regex("PROFILES *= *([a-zA-Z0-9_]+) *([a-zA-Z0-9_]+)? *([a-zA-Z0-9_]+)? *([a-zA-Z0-9_]+)?")

        For Each trouver As Match In regexProfile.Matches(bloc)
            For i As Integer = 1 To trouver.Groups.Count - 1
                Dim valeur As String = trouver.Groups.Item(i).Value
                If valeur <> "" Then
                    ensembleValeurs.Add(valeur)
                End If
            Next
        Next

        dictioUtilisateurRessource.Add(key, ensembleValeurs)
    End Sub

    ''' <summary>
    ''' Fonction de service. À partir d'un bloc d'informations tirer du fichier d'extraction TSS, 
    ''' la fonction va trouver le "ACCESSOR ID" du bloc.
    ''' </summary>
    ''' <param name="bloc">Bloc d'information tirer du fichier d'Extraction TSS.</param>
    ''' <returns>Le "ACCESSOR ID" du bloc d'informations.</returns>
    ''' <remarks></remarks>
    Private Function ObtenirAccessorID(ByVal bloc As String) As String
        Dim regexValeur As New Regex("ACCESSORID *= *([a-zA-Z0-9_]+)")
        Dim collectionResultat As RegularExpressions.MatchCollection = regexValeur.Matches(bloc)
        If collectionResultat.Count = 0 Then
            Return Nothing
        End If

        Return collectionResultat.Item(0).Groups.Item(1).Value
    End Function

    ''' <summary>
    ''' Fonction de service. À partir d'un bloc d'informations tirer du fichier d'extraction TSS, 
    ''' la fonction va trouver le Type du bloc.
    ''' </summary>
    ''' <param name="bloc">Bloc d'information tirer du fichier d'Extraction TSS.</param>
    ''' <returns>Le type du bloc d'information.</returns>
    ''' <remarks></remarks>
    Private Function ObtenirType(ByVal bloc As String) As TypeTSS
        Dim regexType As New Regex("TYPE *= *([a-zA-Z0-9_]+)")
        Dim collectionResultat As RegularExpressions.MatchCollection = regexType.Matches(bloc)
        If collectionResultat.Count = 0 Then
            Return TypeTSS.Inconnu
        End If
        Dim resultat As String = collectionResultat.Item(0).Groups.Item(1).Value
        Select Case True
            Case resultat = "USER"
                Return TypeTSS.Utilisateur
            Case resultat = "PROFILE"
                Return TypeTSS.Profile
        End Select

        Return TypeTSS.Inconnu
    End Function

    ''' <summary>
    ''' Fonction de service. Découpe un fichier d'extraction TSS en bloc d'information.
    ''' </summary>
    ''' <param name="fichier">Le fichier dans lequel la découpe des blocs se fait.</param>
    ''' <returns>Un bloc d'informations en string.</returns>
    ''' <remarks></remarks>
    Private Function ObtenirProchainBloc(ByVal fichier As StreamReader) As String
        Dim regexBloc As New Regex("^ *$")
        Dim texte As New StringBuilder()
        Dim ligneCourante As String = fichier.ReadLine
        While Not regexBloc.IsMatch(ligneCourante) And Not fichier.EndOfStream
            texte.AppendLine(ligneCourante)
            ligneCourante = fichier.ReadLine
        End While

        Return texte.ToString
    End Function

#End Region

End Class
