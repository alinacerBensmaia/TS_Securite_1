Imports System.DirectoryServices
Imports Rrq.InfrastructureCommune.Parametres

'''-----------------------------------------------------------------------------
''' Project		: TS7N322_AccesAD
''' Class		: TsBaAccesAD
'''
'''-----------------------------------------------------------------------------
''' <summary>
''' Cette classe sert à utiliser les fonctions de bases d'un Active Directory.
''' </summary>
''' <remarks></remarks>
'''-----------------------------------------------------------------------------
Public Class TsBaAccesAD

#Region "Variables privées"
    Private Shared mDictioEntryDN As New Dictionary(Of String, String)
    Private Shared mObjetRacine As DirectoryEntry
#End Region

#Region "Fonctions Publics"

    ''' <summary>
    ''' Crée un nouveau groupe dans l'active directory.
    ''' </summary>
    ''' <param name="nomGroupe">Le nom du nouveau groupe.</param>
    ''' <exception cref="TsExcObjetDejaExistantAD">Le groupe existe déja dans l'AD.</exception>
    Public Shared Sub CreerGroupe(ByVal nomGroupe As String)
        Using objetRacine As DirectoryEntry = ObtenirObjetRacine()
            Dim adresse As String = XuCuConfiguration.ValeurSysteme("TS7", "TS7N322\AdresseGroupes")

            Using racineGroupe As DirectoryEntry = TrouverParDN(adresse, objetRacine)
                Using nouveaugroupe As DirectoryEntry = racineGroupe.Children.Add("CN=" + nomGroupe, "group")
                    nouveaugroupe.Properties("cn").Value = nomGroupe
                    nouveaugroupe.Properties("sAMAccountName").Value = nomGroupe

                    Try
                        nouveaugroupe.CommitChanges()
                    Catch ex As System.Runtime.InteropServices.COMException When ex.ErrorCode = TsExcObjetDejaExistantAD.CODE_HRESULT
                        Throw New TsExcObjetDejaExistantAD("L'objet est déjà existant dans l'AD.", ex)
                    End Try
                End Using

            End Using

        End Using
        
    End Sub

    ''' <summary>
    ''' Crée un nouveau groupe universel et de distribution dans l'active directory.
    ''' </summary>
    ''' <param name="nomGroupe">Le nom du nouveau groupe.</param>
    ''' <exception cref="TsExcObjetDejaExistantAD">Le groupe existe déja dans l'AD.</exception>
    Public Shared Sub CreerGroupeUniverselDistribution(ByVal nomGroupe As String, ByVal description As String)

        Using objetRacine As DirectoryEntry = ObtenirObjetRacine()
            Dim adresse As String = XuCuConfiguration.ValeurSysteme("TS7", "TS7N322\AdresseGroupes")
            Dim chemin As String = ""
            Try
                chemin = XuCuConfiguration.ValeurSysteme("TS7", "TS7N322\Chemin" & (nomGroupe.Split("_"c)(0)))
            Catch e As XuExcCClefExistePas
                'Lorsque la clé n'est pas trouvé nous créons sous l'adresse groupe simplement
                chemin = ""
            End Try

            Using racineGroupe As DirectoryEntry = TrouverParDN(chemin & adresse, objetRacine)
                Using nouveaugroupe As DirectoryEntry = racineGroupe.Children.Add("CN=" + nomGroupe, "group")

                    nouveaugroupe.Properties("cn").Value = nomGroupe
                    nouveaugroupe.Properties("sAMAccountName").Value = nomGroupe
                    nouveaugroupe.Properties("groupType").Value = 8 'Universal, Distribution
                    nouveaugroupe.Properties("description").Value = description

                    Try
                        nouveaugroupe.CommitChanges()
                    Catch ex As System.Runtime.InteropServices.COMException When ex.ErrorCode = TsExcObjetDejaExistantAD.CODE_HRESULT
                        Throw New TsExcObjetDejaExistantAD("L'objet est déjà existant dans l'AD.", ex)
                    End Try
                End Using
            End Using
        End Using
    End Sub

    ''' <summary>
    ''' Détruit un groupe dans l'active directory
    ''' </summary>
    ''' <param name="nomGroupe">Le nom du groupe.</param>
    ''' <remarks></remarks>
    ''' <exception cref="TsExcGroupeInexistant">Dans le cas où le groupe n'est pas présent dans l'AD.</exception>
    Public Shared Sub DetruireGroupe(ByVal nomGroupe As String)

        Using objetRacine As DirectoryEntry = ObtenirObjetRacine()
            Using groupe As DirectoryEntry = TrouverParSAMAccountName(nomGroupe, objetRacine)

                If groupe Is Nothing Then
                    Throw New TsExcGroupeInexistant("Le groupe est introuvable.")
                End If

                Dim em As IEnumerator = groupe.Properties("member").GetEnumerator()
                While em.MoveNext()
                    If em.Current Is Nothing Then
                        Continue While
                    End If
                    Using DE As DirectoryEntry = TrouverParDN(em.Current.ToString, objetRacine)
                        If (DE.SchemaClassName = "group") OrElse (DE.SchemaClassName = "user") Then
                            EnleverMembreGroupe(nomGroupe, DE.Properties("sAMAccountName").Value.ToString)
                        End If
                    End Using
                End While
                em = groupe.Properties("memberOf").GetEnumerator()
                While em.MoveNext()
                    If em.Current Is Nothing Then
                        Continue While
                    End If
                    Using DE As DirectoryEntry = TrouverParDN(em.Current.ToString, objetRacine)
                        If (DE.SchemaClassName = "group") OrElse (DE.SchemaClassName = "user") Then
                            EnleverMembreGroupe(DE.Properties("sAMAccountName").Value.ToString, nomGroupe)
                        End If
                    End Using
                End While


                Dim historique As DirectoryEntry
                Using DShearcher As New DirectoryServices.DirectorySearcher(objetRacine, "distinguishedName=" & ObtenirNomHistorique())
                    Dim directoryCible As SearchResult = DShearcher.FindOne
                    If directoryCible IsNot Nothing Then
                        historique = directoryCible.GetDirectoryEntry()
                        groupe.MoveTo(historique)
                    Else
                        Throw New ApplicationException("Impossible de trouver le noeud pour déplacer les groupes supprimés.")
                    End If
                End Using

                groupe.CommitChanges()

            End Using
        End Using

    End Sub

    ''' <summary>
    ''' Obtenir la description groupe dans l'active directory
    ''' </summary>
    ''' <param name="nomGroupe">Le nom du groupe.</param>
    ''' <remarks></remarks>
    ''' <exception cref="TsExcGroupeInexistant">Dans le cas où le groupe n'est pas présent dans l'AD.</exception>
    Public Shared Function ObtenirDescriptionGroupe(ByVal nomGroupe As String) As String

        Using objetRacine As DirectoryEntry = ObtenirObjetRacine()
            Using groupe As DirectoryEntry = TrouverParSAMAccountName(nomGroupe, objetRacine)

                If groupe Is Nothing Then
                    Throw New TsExcGroupeInexistant("Le groupe est introuvable.")
                Else
                    If groupe.Properties("description").Value Is Nothing Then
                        Return String.Empty
                    Else
                        Return groupe.Properties("description").Value.ToString()
                    End If
                End If
            End Using
        End Using

    End Function

    ''' <summary>
    ''' Obtenir la description groupe dans l'active directory
    ''' </summary>
    ''' <param name="nomGroupe">Le nom du groupe.</param>
    ''' <remarks></remarks>
    ''' <exception cref="TsExcGroupeInexistant">Dans le cas où le groupe n'est pas présent dans l'AD.</exception>
    Public Shared Sub EcrireDescriptionGroupe(ByVal nomGroupe As String, ByVal description As String)
        Using objetRacine As DirectoryEntry = ObtenirObjetRacine()
            Using groupe As DirectoryEntry = TrouverParSAMAccountName(nomGroupe, objetRacine)

                If groupe Is Nothing Then
                    Throw New TsExcGroupeInexistant("Le groupe est introuvable.")
                End If

                groupe.Properties("description").Value = description
                groupe.CommitChanges()
            End Using
        End Using
    End Sub

    ''' <summary>
    ''' Ajoute un utilisateur en tant que membre d'un autre groupe.
    ''' </summary>
    ''' <param name="nomMembre">Le nom sAMAccountName de l'utilisateur à ajouter.</param>
    ''' <param name="nomGroupe">Le nom sAMAccountName du groupe acceuillant le nouveau membre.</param>
    ''' <remarks></remarks>
    ''' <exception cref="TsExcGroupeInexistant">Dans le cas où le groupe n'est pas présent dans l'AD.</exception>
    ''' <exception cref="TsExcMembreInexistant">Dans le cas où le membre n'est pas présent dans l'AD.</exception>
    ''' <exception cref="TsExcLienDejaExistantAD">Dans le cas où le liens est déja existant dans l'AD.</exception>
    ''' <exception cref="TsExcObjetDejaExistantAD">L'objet existe déja dans le groupe.</exception>
    Public Shared Sub AjouterMembreGroupe(ByVal nomGroupe As String, ByVal nomMembre As String)
        If nomGroupe = nomMembre Then
            Throw New ApplicationException("Impossible d'ajouter un membre sur lui même.")
        End If

        Try
            Dim objetRacine As DirectoryEntry = ObtenirObjetRacine()

            Using membre As DirectoryEntry = TrouverParSAMAccountName(nomMembre, objetRacine)
                Using groupe As DirectoryEntry = TrouverParSAMAccountName(nomGroupe, objetRacine)

                    If membre Is Nothing Then
                        Throw New TsExcMembreInexistant("Le membre est introuvable.")
                    End If
                    If groupe Is Nothing Then
                        Throw New TsExcGroupeInexistant("Le groupe est introuvable.")
                    End If

                    groupe.Properties("member").Add(membre.Properties("distinguishedName").Value)

                    Try
                        groupe.CommitChanges()
                    Catch ex As System.Runtime.InteropServices.COMException When ex.ErrorCode = TsExcObjetDejaExistantAD.CODE_HRESULT
                        Throw New TsExcObjetDejaExistantAD("L'objet existe déja dans le groupe.", ex)
                    Catch ex As System.Runtime.InteropServices.COMException When ex.ErrorCode = TsExcLienDejaExistantAD.CODE_HRESULT
                        Throw New TsExcLienDejaExistantAD("Le lien est déja existant.", ex)
                    End Try
                End Using
            End Using

        Catch ex As DirectoryServicesCOMException When ex.ErrorCode = TsExcServeurRefuseOperation.CODE_HRESULT
            Throw New TsExcServeurRefuseOperation("L'association entre le membre et le groupe a été refuser par le serveur.", ex)
        End Try
    End Sub
    
    ''' <summary>
    ''' Retire un membre étant membre d'un autre groupe.
    ''' </summary>
    ''' <param name="nomMembre">Le nom sAMAccountName du membre à retirer.</param>
    ''' <param name="nomGroupe">Le nom sAMAccountName du groupe contenant le  membre.</param>
    ''' <remarks></remarks>
    ''' <exception cref="TsExcMembreInexistant">Dans le cas où le membre n'est pas présent dans l'AD.</exception>
    ''' <exception cref="TsExcGroupeInexistant">Dans le cas où le groupe n'est pas présent dans l'AD.</exception>
    ''' <exception cref="TsExcLienInexistantAD">Dans le cas où le lien n'existe pas dans l'AD.</exception>
    Public Shared Sub EnleverMembreGroupe(ByVal nomGroupe As String, ByVal nomMembre As String)
        Dim objetRacine As DirectoryEntry = ObtenirObjetRacine()

        Using membre As DirectoryEntry = TrouverParSAMAccountName(nomMembre, objetRacine)
            Using groupe As DirectoryEntry = TrouverParSAMAccountName(nomGroupe, objetRacine)

                If membre Is Nothing Then
                    Throw New TsExcMembreInexistant("Le membre est introuvable.")
                End If

                If groupe Is Nothing Then
                    Throw New TsExcGroupeInexistant("Le groupe est introuvable.")
                End If

                groupe.Properties("member").Remove(membre.Properties("distinguishedName").Value)
                Try
                    groupe.CommitChanges()
                Catch ex As System.Runtime.InteropServices.COMException When ex.ErrorCode = TsExcLienInexistantAD.CODE_HRESULT
                    Throw New TsExcLienInexistantAD("Le lien n'existe pas dans l'AD.", ex)
                Catch ex As DirectoryServicesCOMException When ex.ErrorCode = TsExcServeurRefuseOperation.CODE_HRESULT
                    Throw New TsExcServeurRefuseOperation("Le lien n'existe pas alors le serveur refuse d'opérer l'opération.", ex)
                End Try
            End Using
        End Using
           
    End Sub

    ''' <summary>
    ''' Retourne une liste des utilisateurs directement attachés au groupe.
    ''' </summary>
    ''' <param name="nomGroupe">Le nom sAMAccountName du groupe dont vous recherchez les utilisateur.</param>
    ''' <returns></returns>
    ''' <exception cref="TsExcGroupeInexistant">Dans le cas où le groupe n'est pas présent dans l'AD.</exception>
    Public Shared Function ObtenirUtilisateursGroupe(ByVal nomGroupe As String) As List(Of String)
        Dim paramRetour As New List(Of String)
        Using objetRacine As DirectoryEntry = ObtenirObjetRacine()

            Using groupe As DirectoryEntry = TrouverParSAMAccountName(nomGroupe, objetRacine)

                If groupe Is Nothing Then
                    Throw New TsExcGroupeInexistant("Le groupe est introuvable.")
                End If

                If groupe.SchemaClassName <> "group" Then
                    Throw New TsExcGroupeInexistant("Le groupe est introuvable.")
                End If

                Dim membres As Object = groupe.Properties("member").Value

                If membres Is Nothing Then
                    Return paramRetour
                End If

                Select Case True
                    Case (membres.GetType.BaseType.Name = "Array")
                        For Each m As Object In CType(membres, Array)
                            Using DE As DirectoryEntry = TrouverParDN(CType(m, String), objetRacine)
                                If (DE.SchemaClassName = "user") Then
                                    paramRetour.Add(CType(DE.Properties("sAMAccountName").Value, String))
                                End If
                            End Using
                        Next
                    Case Else
                        Using DE As DirectoryEntry = TrouverParDN(CType(membres, String), objetRacine)
                            If (DE.SchemaClassName = "user") Then
                                paramRetour.Add(CType(DE.Properties("sAMAccountName").Value, String))
                            End If
                        End Using
                End Select
            End Using
        End Using

        Return paramRetour
    End Function

    ''' <summary>
    ''' Retourne une liste des membres directement attachés au groupe.
    ''' </summary>
    ''' <param name="nomGroupe">Le nom sAMAccountName du groupe dont vous recherchez les utilisateur.</param>
    ''' <returns></returns>
    ''' <exception cref="TsExcGroupeInexistant">Dans le cas où le groupe n'est pas présent dans l'AD.</exception>
    Public Shared Function ObtenirMembreGroupe(ByVal nomGroupe As String) As List(Of String)
        Dim paramRetour As New List(Of String)

        Using objetRacine As DirectoryEntry = ObtenirObjetRacine()
            Using groupe As DirectoryEntry = TrouverParSAMAccountName(nomGroupe, objetRacine)

                If groupe Is Nothing Then
                    Throw New TsExcGroupeInexistant("Le groupe est introuvable.")
                End If
                If groupe.SchemaClassName <> "group" Then
                    Throw New TsExcGroupeInexistant("Le groupe est introuvable.")
                End If

                Dim membres As Object = groupe.Properties("member").Value

                If membres Is Nothing Then Return paramRetour

                Select Case True
                    Case (membres.GetType.BaseType.Name = "Array")
                        For Each m As Object In CType(membres, Array)
                            paramRetour.Add(CType(m, String))
                        Next
                    Case Else
                        paramRetour.Add(CType(membres, String))
                End Select
            End Using
        End Using

        Return paramRetour
    End Function

    ''' <summary>
    ''' Retourne une liste des groupes dont l'utilisateur est attaché.
    ''' </summary>
    ''' <param name="nomUtilisateur">Le nom sAMAccountName de l'utilisateur dont vous recherchez les groupes.</param>
    ''' <returns></returns>
    ''' <exception cref="TsExcUtilisateurInexistant">Dans le cas où l'utilisateur n'est pas présent dans l'AD.</exception>
    Public Shared Function ObtenirGroupesUtilisateur(ByVal nomUtilisateur As String) As List(Of String)
        Dim paramRetour As New List(Of String)
        Using objetRacine As DirectoryEntry = ObtenirObjetRacine()
            Using utilisateur As DirectoryEntry = TrouverParSAMAccountName(nomUtilisateur, objetRacine)

                If utilisateur.SchemaClassName <> "user" Then
                    Throw New TsExcUtilisateurInexistant("L'utilisateur est introuvable.")
                End If

                Dim membres As Object = utilisateur.Properties("memberOf").Value

                If membres Is Nothing Then Return paramRetour

                Select Case True
                    Case (membres.GetType.BaseType.Name = "Array")
                        For Each m As Object In CType(membres, Array)
                            Using DE As DirectoryEntry = TrouverParDN(CType(m, String), objetRacine)
                                If (DE.SchemaClassName = "group") Then
                                    paramRetour.Add(CType(DE.Properties("sAMAccountName").Value, String))
                                End If
                            End Using
                        Next
                    Case Else
                        Using DE As DirectoryEntry = TrouverParDN(CType(membres, String), objetRacine)
                            If (DE.SchemaClassName = "group") Then
                                paramRetour.Add(CType(DE.Properties("sAMAccountName").Value, String))
                            End If
                        End Using
                End Select
            End Using
        End Using

        Return paramRetour
    End Function

    ''' <summary>
    ''' Détermine si un utilisateur existe dans l'AD.
    ''' </summary>
    ''' <param name="nomUtilisateur">Le nom sAMAccountName de l'utilisateur.</param>
    Public Shared Function UtilisateurExiste(ByVal nomUtilisateur As String) As Boolean
        Using objetRacine As DirectoryEntry = ObtenirObjetRacine()
            Using utilisateur As DirectoryEntry = TrouverParSAMAccountName(nomUtilisateur, objetRacine)
                Select Case True
                    Case utilisateur Is Nothing
                        Return False
                    Case utilisateur.SchemaClassName = "user"
                        Return True
                    Case Else
                        Return False
                End Select
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Détermine si un groupe existe dans l'AD.
    ''' </summary>
    ''' <param name="nomGroupe">Le nom sAMAccountName du groupe.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GroupeExiste(ByVal nomGroupe As String) As Boolean
        Using objetRacine As DirectoryEntry = ObtenirObjetRacine()
            Using groupe As DirectoryEntry = TrouverParSAMAccountName(nomGroupe, objetRacine)

                Select Case True
                    Case groupe Is Nothing
                        Return False
                    Case groupe.SchemaClassName = "group"
                        'Cas special, si l'objet est dans la poubelle
                        If groupe.Parent.Properties("distinguishedName").Value.ToString.Equals(ObtenirNomHistorique(), StringComparison.InvariantCultureIgnoreCase) Then
                            Return False
                        End If
                        Return True
                    Case Else
                        Return False
                End Select
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Détermine si le groupe possède le membre.
    ''' </summary>
    ''' <param name="nomMembre">Le nom sAMAccountName du membre recherché.</param>
    ''' <param name="nomGroupe">Le nom sAMAccountName du groupe possédant le membre.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <exception cref="TsExcGroupeInexistant">Dans le cas où le groupe n'est pas présent dans l'AD.</exception>
    Public Shared Function EstMembreGroupe(ByVal nomMembre As String, ByVal nomGroupe As String) As Boolean
        Using chercheur As New DirectorySearcher(ObtenirObjetRacine)

            chercheur.Filter = "(sAMAccountName=" + nomMembre + ")"
            Using collectionResultat As SearchResultCollection = chercheur.FindAll()
                If collectionResultat.Count = 0 Then
                    Return False
                Else
                    Dim membres As ResultPropertyValueCollection = collectionResultat(0).Properties("memberOf")
                    Dim estPresent As Boolean = False
                    For Each m As Object In membres
                        If m.ToString().Contains("CN=" + nomGroupe + ",") Then
                            estPresent = True
                            Exit For
                        End If
                    Next
                    Return estPresent
                End If
            End Using
        End Using
    End Function

#End Region

#Region "Fonctions de services"

    ''' <summary>
    ''' Fonction qui rechercher le Distinguished Name par le SAMAccountName.
    ''' </summary>
    ''' <param name="nomEntry">Le SAMAccountName dont on veut connaitre le Distinguished Name.</param>
    ''' <returns>Le Distinguished Name de l'Entry'.</returns>
    ''' <remarks></remarks>
    Private Shared Function ObtenirEntryDN(ByVal nomEntry As String) As String
        Dim paramRetour As String
        If mDictioEntryDN.ContainsKey(nomEntry) = True Then
            paramRetour = mDictioEntryDN.Item(nomEntry)
        Else
            Using objetRacine As DirectoryEntry = ObtenirObjetRacine()
                Using entry As DirectoryEntry = TrouverParSAMAccountName(nomEntry, objetRacine)
                    If entry Is Nothing Then
                        Return ""
                    End If
                    Dim entryDN As String = entry.Properties("distinguishedName").Value.ToString
                    mDictioEntryDN.Add(nomEntry, entryDN)
                    paramRetour = entryDN
                End Using

            End Using
        End If

        Return paramRetour
    End Function


    ''' <summary>
    ''' Fonction de service. Permet de trouver un DirectoryEntry par sa propriété sAMAccountName.
    ''' </summary>
    ''' <param name="nomEntry">Le sAMAccountName du DirectoryEntry recherché.</param>
    ''' <param name="DETete">L'objet DirectoryEntry dans lequel sera effectué la recherche.</param>
    ''' <returns>Retourne nothing si aucun DirectoryEntry ne correspond au sAMAccountName demandé.</returns>
    Private Shared Function TrouverParSAMAccountName(ByVal nomEntry As String, ByVal DETete As DirectoryEntry) As DirectoryEntry
        Dim paramRetour As DirectoryEntry = Nothing
        Using DShearcher As New DirectoryServices.DirectorySearcher(DETete, "sAMAccountName=" + nomEntry)

            Dim directoryCible As SearchResult = DShearcher.FindOne
            If directoryCible IsNot Nothing Then
                paramRetour = directoryCible.GetDirectoryEntry()
            End If
        End Using
        Return paramRetour
    End Function

    ''' <summary>
    ''' Fonction de service. Permet de trouver un DirectoryEntry par sa propriété distinguishedName.
    ''' </summary>
    ''' <param name="nomDN">Le distinguishedName du DirectoryEntry recherché.</param>
    ''' <param name="DETete">L'objet DirectoryEntry dans lequel sera effectué la recherche.</param>
    ''' <returns>Retourne Nothing si aucun DirectoryEntry ne correspond au distinguishedName demandé.</returns>
    Private Shared Function TrouverParDN(ByVal nomDN As String, ByVal DETete As DirectoryEntry) As DirectoryEntry
        Dim paramRetour As DirectoryEntry = Nothing
        Using DShearcher As New DirectoryServices.DirectorySearcher(DETete, "distinguishedName=" + nomDN)
            paramRetour = DShearcher.FindOne.GetDirectoryEntry()
        End Using

        Return paramRetour
    End Function


    ''' <summary>
    ''' Fonction de service. Cette fonction définit quel AD sera utilisé, basé sur les données du TS7.config.
    ''' </summary>
    Private Shared Function ObtenirObjetRacine() As DirectoryEntry
        If mObjetRacine Is Nothing Then
            Dim modeAD As String = XuCuConfiguration.ValeurSysteme("TS7", "TS7N322\ConnecterSurAD")

            If String.Compare(modeAD, "True", True) = 0 Then
                mObjetRacine = New DirectoryEntry()
            Else
                Dim adresse As String = XuCuConfiguration.ValeurSysteme("TS7", "TS7N322\AdresseADAM")
                mObjetRacine = New DirectoryEntry(adresse)
            End If
        End If
        Return mObjetRacine
    End Function

    Private Shared Function ObtenirNomHistorique() As String
        Return XuCuConfiguration.ValeurSysteme("TS7", "TS7N322\AdresseHistoriques")
    End Function

#End Region

End Class
