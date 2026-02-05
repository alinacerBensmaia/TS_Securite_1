Imports System.DirectoryServices
Imports System.Runtime.InteropServices
Imports Rrq.InfrastructureCommune.Parametres
Imports Rrq.Securite.GestionAcces.Internal


Public Class TsBaAccesADLDS
    Implements IDepotSecurite
    Private Const PREFIX_GROUPE_ROA As String = "ROA_"
    Private Const EMPLACEMENT_ENVIRONNEMENT As String = "OU={0},OU=Autorisations applicatives,O=org"
    Private Const PREFIX_GROUPE_ROG As String = "ROG_"
    Private Const EMPLACEMENT_REGROUPEMENT As String = "OU=Regroupements applicatifs,O=org"

    Private ReadOnly _adlds As ActiveDirectoryLightweightDirectoryServices


    Public Sub New()
        Dim serveur As String = XuCuPolitiqueConfig.ConfigDomaine.ValeurSysteme("TS4", "TS4\TS4N321\ServeurADLDS")
        Dim racine As String = XuCuPolitiqueConfig.ConfigDomaine.ValeurSysteme("TS4", "TS4\TS4N321\RepertoireRacine")

        _adlds = New ActiveDirectoryLightweightDirectoryServices(serveur, racine)
    End Sub


    Public Sub AjouterMembreGroupe(nomGroupe As String, nomMembre As String) Implements IDepotSecurite.AjouterMembreGroupe
        If nomGroupe.Est(nomMembre) Then Throw New ApplicationException("Impossible d'ajouter un membre sur lui même.")

        Try
            Using _adlds.Connexion
                'le membre peut être un 'user'
                Dim membre As DirectoryEntry = _adlds.ObtenirUtilisateur(nomMembre)
                If membre Is Nothing Then
                    'mais il peut aussi être un 'group'
                    membre = _adlds.ObtenirGroupe(nomMembre)
                    If membre Is Nothing Then Throw New TsExcMembreInexistant("Le membre est introuvable.")
                End If

                Using membre
                    Using groupe As DirectoryEntry = _adlds.ObtenirGroupe(nomGroupe)
                        If groupe Is Nothing Then Throw New TsExcGroupeInexistant("Le groupe est introuvable.")
                        If groupe.Contient(membre) Then Throw New TsExcObjetDejaExistantADLDS("L'objet existe déja dans le groupe.")

                        groupe.AjouterMembre(membre)
                        groupe.CommitChanges()
                    End Using
                End Using
            End Using

        Catch ex As COMException When ex.ErrorCode = TsExcObjetDejaExistantADLDS.CODE_HRESULT
            Throw New TsExcObjetDejaExistantADLDS("L'objet existe déja dans le groupe.", ex)
        Catch ex As COMException When ex.ErrorCode = TsExcLienDejaExistantADLDS.CODE_HRESULT
            Throw New TsExcLienDejaExistantADLDS("Le lien est déja existant.", ex)
        Catch ex As DirectoryServicesCOMException When ex.ErrorCode = TsExcServeurRefuseOperation.CODE_HRESULT
            Throw New TsExcServeurRefuseOperation("L'association entre le membre et le groupe a été refuser par le serveur.", ex)
        End Try
    End Sub

    Public Sub CreerGroupeSecuriteApplicativeROA(nomGroupe As String, description As String) Implements IDepotSecurite.CreerGroupeSecuriteApplicativeROA
        If Not nomGroupe.CommencePar(PREFIX_GROUPE_ROA) Then Throw New ArgumentException("Le nom du groupe n'est pas valide", "nomGroupe")

        Using _adlds.Connexion
            If _adlds.GroupeExiste(nomGroupe) Then Throw New TsExcObjetDejaExistantADLDS("L'objet existe déjà dans ADLDS.")

            Dim emplacement As String = String.Format(EMPLACEMENT_ENVIRONNEMENT, nomGroupe.CodeEnvironnement.ToMotEnvironnement())
            _adlds.CreerGroupe(nomGroupe, description, emplacement)
        End Using
    End Sub

    Public Sub CreerGroupeSecuriteApplicativeROG(nomGroupe As String, description As String) Implements IDepotSecurite.CreerGroupeSecuriteApplicativeROG
        If Not nomGroupe.CommencePar(PREFIX_GROUPE_ROG) Then Throw New ArgumentException("Le nom du groupe n'est pas valide", "nomGroupe")

        Using _adlds.Connexion
            If _adlds.GroupeExiste(nomGroupe) Then Throw New TsExcObjetDejaExistantADLDS("L'objet existe déjà dans ADLDS.")

            Dim emplacement As String = EMPLACEMENT_REGROUPEMENT
            _adlds.CreerGroupe(nomGroupe, description, emplacement)
        End Using
    End Sub

    Public Sub ModifierDescriptionGroupe(nomGroupe As String, description As String) Implements IDepotSecurite.ModifierDescriptionGroupe
        Using _adlds.Connexion
            Using groupe As DirectoryEntry = _adlds.ObtenirGroupe(nomGroupe)
                If groupe Is Nothing Then Throw New TsExcGroupeInexistant("Le groupe est introuvable.")

                groupe.AssignerDescription(description)
                groupe.CommitChanges()
            End Using
        End Using
    End Sub

    Public Function ObtenirDescriptionGroupe(nomGroupe As String) As String Implements IDepotSecurite.ObtenirDescriptionGroupe
        Using _adlds.Connexion
            Using groupe As DirectoryEntry = _adlds.ObtenirGroupe(nomGroupe)
                If groupe Is Nothing Then Throw New TsExcGroupeInexistant("Le groupe est introuvable.")

                Return groupe.ObtenirDescription
            End Using
        End Using
    End Function

End Class