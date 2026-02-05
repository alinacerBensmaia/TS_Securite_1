Imports LireConfig = Rrq.InfrastructureCommune.Parametres.XuCuConfiguration

''' <summary>
''' ce module cert à faire des fichiers textes de journalisations.
''' </summary>
''' <remarks>Le chemin de dépot des fichiers de journalisations sont paramètrés dans le fichier de configuration TS7.config</remarks>
Public Module TsBaJournalisation

#Region "Variables Privées"

    Private dateDemarer As Date = Nothing
    Private journalisationActiver As Boolean = False

    Private fichierJounaliser As IO.StreamWriter = Nothing

#End Region

#Region "Méthodes"

    ''' <summary>
    ''' Fonction permettant d'initialiser la création du journal.
    ''' </summary>
    Public Sub DemarerJournalisation()
        If journalisationActiver = False Then

            journalisationActiver = True
            dateDemarer = Date.Now()

            Dim dossierDepot As String = LireConfig.ValeurSysteme("TS7", "TS7\TS7N221\Depot_Journalisation")
            Dim nomFichier As String = dossierDepot + "\" + dateDemarer.ToString("TS7N221_PrepareCible yyyy_MM_dd hh;mm") + ".txt"

            If My.Computer.FileSystem.DirectoryExists(dossierDepot) = False Then
                My.Computer.FileSystem.CreateDirectory(dossierDepot)
            End If

            fichierJounaliser = New IO.StreamWriter(nomFichier, True)
            fichierJounaliser.AutoFlush = True

            EcrireEntree("Création du journal.")
        End If
    End Sub

    ''' <summary>
    ''' Fonction permettant d'arrêter la journalisation du fichier actif.
    ''' </summary>
    Public Sub TerminerJournalisation()
        EcrireEntree("Demande de fermeture du journal.")
        fichierJounaliser.Close()
        fichierJounaliser.Dispose()
        fichierJounaliser = Nothing

        dateDemarer = Nothing

        journalisationActiver = False

    End Sub

    ''' <summary>
    ''' Fonction permmettant d'écrire une ligne dans le journal.
    ''' </summary>
    ''' <param name="entree">L'entrée d'information qui sera ajouter dans le journal.</param>
    ''' <remarks>Si le journal n'a pas été initialiser avant, cette fonction le fera.</remarks>
    Public Sub EcrireEntree(ByVal entree As String)
        If journalisationActiver = False Then
            DemarerJournalisation()
        End If

        Dim ligneAEcrire As String = String.Format("<{0}> {1}", New String() {Date.Now.ToString("yyyy-MM-dd hh:mm:ss"), entree})
        fichierJounaliser.WriteLine(ligneAEcrire)
    End Sub

#End Region

End Module
