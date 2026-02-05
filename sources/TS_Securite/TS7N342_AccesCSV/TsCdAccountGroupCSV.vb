Imports System.IO
Imports system.Text

''' <summary>
''' Cette classe permet de manipuler un couple de fichier CSV (accounts et groups)
''' </summary>
''' <remarks>
''' Conçu pour fonctionner avec le connecteur agtdos-complete-sample de IDM Suite (Hitachi ID).
''' Seul le fichier des groupes est modiable pour le moment.
''' </remarks>
'''-----------------------------------------------------------------------------
Public Class TsCdAccountGroupCSV
    Implements IDisposable

    ' Tiré de agtdos-accounts.csv et agtdos-groups.csv
    Private Const HEAD_ACCOUNTS_DEFAULT As String = """longid"",""shortid"",""fullname"",""password"",""pexpired"",""aexpired"",""locked"",""disabled"",""distinction"",""century"""
    Private Const HEAD_GROUPS_DEFAULT As String = """longid"",""shortid"",""desc"",""members"""

    ' Expressions régulières qui décrivent le format des fichiers
    ' (il faudrait peut-être détecté les escape-sequence, mais je ne les connais pas...)
    Private Const RX_ACCOUNTS As String = "^""([^""]*)"","    ' Pas besoin de décrire le reste de la ligne...
    Private Const RX_GROUPS As String = "^""([^""]*)"",""([^""]*)"",""([^""]*)"",""([^""]*)"""

    ' Format pour l'écriture
    Private Const FMT_GROUPS As String = """{0}"",""{1}"",""{2}"",""{3}"""
    Private Const FMT_ACCOUNTS As String = """{0}"",""{0}"",""{0}"",""pwd"",""0"",""0"",""0"",""0"",""tata"",""toto"""

    ' Index des groupes intéressants dans les expressions régulières
    Private Const IDX_ACCOUNTS_ID As Integer = 1
    Private Const IDX_GROUPS_ID As Integer = 1
    Private Const IDX_GROUPS_MEMBERS As Integer = 4

    ' Chemin des fichiers Accounts et Groups
    Private _cheminAccounts As String
    Private _cheminGroups As String

    ' Copie indexée en mémoire des fichiers Accounts et Groups
    Private _utilisateurs As New Dictionary(Of String, TsCdUtilisateurCSV)
    Private _groupes As New Dictionary(Of String, TsCdGroupeCSV)

    ' Nécessaire pour l'écriture
    Private _headerGroups As String
    Private _headerAccounts As String
    Private _groupsDirty As Boolean = False
    Private _accountsDirty As Boolean = False

    Private _disposedValue As Boolean = False        ' Pour détecter les appels redondants


    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <param name="cheminAccounts">Chemin du fichier contenant les comptes.</param>
    ''' <param name="cheminGroups">Chemin du fichier contenant les groupes.</param>
    Public Sub New(ByVal cheminAccounts As String, ByVal cheminGroups As String, Optional ByVal skipRead As Boolean = False)
        _cheminAccounts = cheminAccounts
        _cheminGroups = cheminGroups
        If Not skipRead Then
            LireCSV()
        End If
    End Sub

    Private Sub LireCSV()
        Dim rx As New Regex(RX_ACCOUNTS)
        ' On lit d'abord le fichier des comptes
        Dim reader = New StreamReader(New FileStream(_cheminAccounts, FileMode.Open, FileAccess.Read, FileShare.None), System.Text.Encoding.ASCII)
        ' La première ligne est la description des champs, on n'en aura pas besoin...
        _headerAccounts = reader.ReadLine()
        Dim ligne = reader.ReadLine()
        Do While ligne IsNot Nothing
            Dim match = rx.Match(ligne)
            Dim id = match.Groups(IDX_ACCOUNTS_ID).Value
            _utilisateurs(id) = New TsCdUtilisateurCSV(id)
            ligne = reader.ReadLine()
        Loop
        reader.Close()

        rx = New Regex(RX_GROUPS)
        ' On lit ensuite le fichier des groupes
        reader = New StreamReader(New FileStream(_cheminGroups, FileMode.Open, FileAccess.Read, FileShare.None), System.Text.Encoding.ASCII)
        ' La première ligne est la description des champs, il faut la conserver pour réécrire le fichier plus tard
        _headerGroups = reader.ReadLine()
        ligne = reader.ReadLine()
        Do While ligne IsNot Nothing
            Dim match = rx.Match(ligne)
            Dim id = match.Groups(IDX_GROUPS_ID).Value
            Dim gr = New TsCdGroupeCSV(id)

            ' Les membres sont séparés par des espaces
            Dim membres = match.Groups(IDX_GROUPS_MEMBERS).Value.Split(" "c)
            For Each m In membres
                gr.Membres.Add(m)
                _utilisateurs(m).Groupes.Add(id)
            Next
            _groupes(id) = gr

            ligne = reader.ReadLine()
        Loop
        reader.Close()
    End Sub


    ''' <summary>
    ''' Ajoute un groupe à un utilisateur.
    ''' </summary>
    ''' <param name="groupe">Le groupe à ajouter.</param>
    ''' <param name="utilisateur">L'utilisateur à qui sera ajouté le groupe.</param>
    ''' <returns>True si l'ajout est fait ou si l'utilisateur avait déjà le groupe, False sinon.</returns>
    ''' <remarks>
    ''' Si le groupe ou l'utilisateur n'existe pas, on retourne False.
    ''' </remarks>
    Public Function AjouterGroupeUtilisateur(ByVal groupe As String, ByVal utilisateur As String) As Boolean
        Dim user As TsCdUtilisateurCSV = Nothing
        If Not _utilisateurs.TryGetValue(utilisateur, user) Then
            ' Utilisateur inexistant
            Return False
        End If

        Dim gr As TsCdGroupeCSV = Nothing
        If Not _groupes.TryGetValue(groupe, gr) Then
            ' Groupe introuvable
            Return False
        End If

        ' Si un élément est déjà existant, ce n'est pas une erreur de l'ajouter à nouveau dans un HashSet
        user.Groupes.Add(gr.Id)
        gr.Membres.Add(user.Id)
        _groupsDirty = True

        Return True
    End Function

    ''' <summary>
    ''' Enlève un groupe à un utilisateur.
    ''' </summary>
    ''' <param name="groupe">Le groupe à ajouter.</param>
    ''' <param name="utilisateur">L'utilisateur à qui sera enlevé le groupe.</param>
    ''' <returns>True si le groupe est enlevé ou si l'utilisateur ne l'avait déjà pas, False sinon.</returns>
    ''' <remarks>
    ''' Si le groupe ou l'utilisateur n'existe pas, on retourne False.
    ''' </remarks>
    Public Function EnleverGroupeUtilisateur(ByVal groupe As String, ByVal utilisateur As String) As Boolean
        Dim user As TsCdUtilisateurCSV = Nothing
        If Not _utilisateurs.TryGetValue(utilisateur, user) Then
            ' Utilisateur inexistant
            Return False
        End If

        Dim gr As TsCdGroupeCSV = Nothing
        If Not _groupes.TryGetValue(groupe, gr) Then
            ' Groupe introuvable
            Return False
        End If

        ' Si un élément est déjà existant, ce n'est pas une erreur de l'ajouter à nouveau dans un HashSet
        user.Groupes.Remove(gr.Id)
        gr.Membres.Remove(user.Id)
        _groupsDirty = True

        Return True
    End Function

    Public Function UtilisateurMembre(ByVal uid As String, ByVal gid As String) As Boolean
        ' On assume que _utilisateurs et _groupes sont intègres
        Return _utilisateurs.ContainsKey(uid) AndAlso _utilisateurs(uid).Groupes.Contains(gid)
    End Function

    Public Sub AjouterUtilisateur(ByVal uid As String)
        If Not _utilisateurs.ContainsKey(uid) Then
            _utilisateurs.Add(uid, New TsCdUtilisateurCSV(uid))
            _accountsDirty = True
        End If
    End Sub

    Public Sub AjouterGroupe(ByVal gid As String)
        If Not _groupes.ContainsKey(gid) Then
            _groupes.Add(gid, New TsCdGroupeCSV(gid))
            _groupsDirty = True
        End If
    End Sub


    Private Sub EcrireGroupsCSV()
        ' On lit ensuite le fichier des groupes
        Dim writer = New StreamWriter(New FileStream(_cheminGroups, FileMode.Create, FileAccess.Write, FileShare.None), System.Text.Encoding.ASCII)
        ' La première ligne est la description des champs...
        writer.WriteLine(If(_headerGroups IsNot Nothing, _headerGroups, HEAD_ACCOUNTS_DEFAULT))
        For Each gr In _groupes.Values
            ' On copie les membres du groupe dans un tableau pour pouvoir utiliser la méthode String.Join
            Dim membres(gr.Membres.Count - 1) As String
            gr.Membres.CopyTo(membres)
            writer.WriteLine(String.Format(FMT_GROUPS, gr.Id, gr.Id, gr.Id, String.Join(" ", membres)))
        Next
        writer.Close()
    End Sub

    Private Sub EcrireAccountsCSV()
        ' On lit ensuite le fichier des groupes
        Dim writer = New StreamWriter(New FileStream(_cheminAccounts, FileMode.Create, FileAccess.Write, FileShare.None), System.Text.Encoding.ASCII)
        ' La première ligne est la description des champs...
        writer.WriteLine(If(_headerAccounts IsNot Nothing, _headerAccounts, HEAD_ACCOUNTS_DEFAULT))
        For Each acc In _utilisateurs.Values
            writer.WriteLine(String.Format(FMT_ACCOUNTS, acc.Id))
        Next
        writer.Close()
    End Sub

    Public Sub Flush()
        If _GroupsDirty Then
            EcrireGroupsCSV()
            _GroupsDirty = False
        End If
        If _AccountsDirty Then
            EcrireAccountsCSV()
        End If
    End Sub

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me._disposedValue Then
            If disposing Then
                ' ressources managées...
            End If

            Flush()
        End If
        Me._disposedValue = True
    End Sub

#Region " IDisposable Support "
    ' Ce code a été ajouté par Visual Basic pour permettre l'implémentation correcte du modèle pouvant être supprimé.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Ne modifiez pas ce code. Ajoutez du code de nettoyage dans Dispose(ByVal disposing As Boolean) ci-dessus.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
