Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Configuration


Public Class TsCuErreur
    '------------------------------------------------------------------------------ 
    ' Fichiers de messages de l'observateur d'événements 
    '------------------------------------------------------------------------------ 
    '------------------------------------------------------------------------------
    ' ID 0
    ' 
    ' Message personnalisé
    '
    ' Paramètres
    '   1 - Texte libre (taggé)
    '------------------------------------------------------------------------------
    '------------------------------------------------------------------------------
    ' ID 1
    ' 
    ' Message normalisé
    '
    ' Paramètres
    '   1 - Message d'erreur (non taggé)
    '   2 - Emplacement de l'erreur (non taggé)
    '   3 - Pile d'appels (non taggée)
    '   4 - Informations du contexte (non taggées)
    '------------------------------------------------------------------------------
#Region " Constantes et énumérations "
    Const SOURCE As String = "SecurityRRQ"
    Const SOURCEALTERNATIVE As String = "Application"

    Public Enum TypeEcriture
        FichierLog = 1
        FichierLogAvecJournalEvenm = 2
        FichierTexte = 3
        FichierTexteAvecJournalEvenm = 4
        JournalEvenm = 5
    End Enum

    Public Enum TypeEvenement ' Extrait de <WinNT.h>
        Succes = &H0
        Erreur = &H1
        Avertissement = &H2
        Information = &H4
        SuccesAudit = &H8
        EchecAudit = &H10
    End Enum
#End Region

#Region "Attributs"



#End Region

#Region " Fonctions et Méthodes "
    Private Function ConstruireMessage(ByVal strMessage As String, ByVal strEmplacement As String, _
                                        ByVal strPileAppels As String, ByVal strInfoCompl As String) As String
        Dim strMesgErr As String = "<LOGRRQDESC Nom=""Message d'erreur"">" & vbCrLf
        Dim strEmplace As String = "<LOGRRQDESC Nom=""Emplacement de l'erreur"">" & vbCrLf
        Dim strPileApp As String = "<LOGRRQDESC Nom=""Pile d'appels"">" & vbCrLf
        Dim strInfComp As String = "<LOGRRQDESC Nom=""Informations du contexte"">" & vbCrLf
        Dim strFin As String = vbCrLf & "</LOGRRQDESC>" & vbCrLf & vbCrLf

        ConstruireMessage = strMesgErr & strMessage & strFin & strEmplace & strEmplacement & strFin & _
                            strPileApp & strPileAppels & strFin & strInfComp & strInfoCompl & strFin
    End Function

    Public Overloads Sub EcrireJournal(ByVal strMessage As String, ByVal intTypeMessage As TypeEvenement)
        Dim EntreeJournal As New EventLog
        Dim intEntryType As EventLogEntryType
        Dim strMesg As String = String.Empty

        ' Dans un message, il ne peut y avoir plus de 32766 caractères
        If strMessage.Length > 32766 Then
            strMessage = strMessage.Substring(0, 32766)
        End If

        ' Convertir le type de message propriétaire en type de message EventLogEntry
        Select Case intTypeMessage
            Case TypeEvenement.Avertissement
                intEntryType = EventLogEntryType.Warning
            Case TypeEvenement.EchecAudit
                intEntryType = EventLogEntryType.FailureAudit
            Case TypeEvenement.Erreur
                intEntryType = EventLogEntryType.Error
            Case TypeEvenement.Information
                intEntryType = EventLogEntryType.Information
            Case TypeEvenement.Succes
                intEntryType = EventLogEntryType.Information
            Case TypeEvenement.SuccesAudit
                intEntryType = EventLogEntryType.SuccessAudit
        End Select

        If (EventLog.SourceExists(SOURCE)) Then
            Try
                EventLog.WriteEntry(SOURCE, strMessage, intEntryType, 0, 1)
            Catch ex As Exception
                EventLog.WriteEntry(SOURCEALTERNATIVE, strMessage, intEntryType, 0, 1)
            End Try
        Else
            EventLog.WriteEntry(SOURCEALTERNATIVE, strMessage, intEntryType, 0, 1)
        End If
    End Sub

    Public Overloads Sub EcrireJournal(ByVal strMessage As String, ByVal strEmplacement As String, _
                                        ByVal strPileAppels As String, ByVal strInfoCompl As String, _
                                        ByVal intTypeMessage As TypeEvenement)
        Dim EntreeJournal As New EventLog
        Dim intEntryType As EventLogEntryType
        Dim strMesg As String

        ' Construction du message
        strMesg = ConstruireMessage(strMessage, strEmplacement, strPileAppels, strInfoCompl)

        ' Dans un message, il ne peut y avoir plus de 32766 caractères
        If strMesg.Length > 32766 Then
            strMesg = strMesg.Substring(0, 32766)
        End If

        ' Convertir le type de message propriétaire en type de message EventLogEntry
        Select Case intTypeMessage
            Case TypeEvenement.Avertissement
                intEntryType = EventLogEntryType.Warning
            Case TypeEvenement.EchecAudit
                intEntryType = EventLogEntryType.FailureAudit
            Case TypeEvenement.Erreur
                intEntryType = EventLogEntryType.Error
            Case TypeEvenement.Information
                intEntryType = EventLogEntryType.Information
            Case TypeEvenement.Succes
                intEntryType = EventLogEntryType.Information
            Case TypeEvenement.SuccesAudit
                intEntryType = EventLogEntryType.SuccessAudit
        End Select

        If (EventLog.SourceExists(SOURCE)) Then
            Try
                EventLog.WriteEntry(SOURCE, strMesg, intEntryType, 1, 1)
            Catch
                EventLog.WriteEntry(SOURCEALTERNATIVE, strMesg, intEntryType, 1, 1)
            End Try
        Else
            EventLog.WriteEntry(SOURCEALTERNATIVE, strMesg, intEntryType, 1, 1)
        End If
    End Sub

    Public Overloads Sub GererErreur(ByVal objErreur As Exception, ByVal typEcriture As TypeEcriture)

        Dim stkErreur As StackTrace
        Dim strSectionMessage As String
        Dim strSectionEmplacement As String
        Dim strSectionPileAppels As String
        Dim strSectionInfoCompl As String
        Dim intCtr As Integer

        ' Initialiser les chaînes de message à vide
        strSectionMessage = ""
        strSectionEmplacement = ""
        strSectionPileAppels = ""
        strSectionInfoCompl = ""

        stkErreur = New StackTrace(objErreur, True)

        strSectionMessage &= objErreur.GetType.ToString & " : " & objErreur.Message

        While intCtr <= stkErreur.FrameCount - 1
            If Trim(stkErreur.GetFrame(intCtr).GetFileName) <> "" Then
                strSectionEmplacement &= stkErreur.GetFrame(intCtr).GetFileName
                strSectionEmplacement &= " à la ligne " & stkErreur.GetFrame(intCtr).GetFileLineNumber
                strSectionEmplacement &= " dans la méthode " & stkErreur.GetFrame(intCtr).GetMethod.Name & vbCrLf
            End If
            intCtr += 1
        End While

        strSectionPileAppels &= stkErreur.ToString

        strSectionInfoCompl &= "Nom de la machine : " & System.Environment.MachineName & vbCrLf & vbCrLf
        strSectionInfoCompl &= "Système d'exploitation : " & ObtenirVersionOS() & " (" & System.Environment.OSVersion.Version.ToString & ")" & vbCrLf & vbCrLf
        strSectionInfoCompl &= "Service pack : " & ObtenirServicePack() & vbCrLf & vbCrLf
        strSectionInfoCompl &= "Nom de l'utilisateur : " & System.Security.Principal.WindowsIdentity.GetCurrent.Name & vbCrLf & vbCrLf
        strSectionInfoCompl &= "Environnement : " & UCase(ConfigurationManager.AppSettings.Item("XW_Environnement")) & vbCrLf & vbCrLf

        'On inscrit le message d'erreur dans le journal
        EcrireJournal(strSectionMessage, strSectionEmplacement, strSectionPileAppels, strSectionInfoCompl, TypeEvenement.Erreur)
    End Sub

    Public Overloads Sub GererErreur(ByVal objErreur As Exception, ByVal typEcriture As TypeEcriture, ByVal strFichier As String)

        Dim stkErreur As StackTrace        
        Dim FSOStrm As IO.FileStream
        Dim FSOWriter As IO.StreamWriter
        Dim strSectionMessage As String
        Dim strSectionEmplacement As String
        Dim strSectionPileAppels As String
        Dim strSectionInfoCompl As String
        Dim strMessg As String
        Dim intCtr As Integer

        ' Initialiser les chaînes de message à vide
        strSectionMessage = ""
        strSectionEmplacement = ""
        strSectionPileAppels = ""
        strSectionInfoCompl = ""

        stkErreur = New StackTrace(objErreur, True)

        strSectionMessage &= objErreur.GetType.ToString & " : " & objErreur.Message

        While intCtr <= stkErreur.FrameCount - 1
            If Trim(stkErreur.GetFrame(intCtr).GetFileName) <> "" Then
                strSectionEmplacement &= stkErreur.GetFrame(intCtr).GetFileName
                strSectionEmplacement &= " à la ligne " & stkErreur.GetFrame(intCtr).GetFileLineNumber
                strSectionEmplacement &= " dans la méthode " & stkErreur.GetFrame(intCtr).GetMethod.Name & vbCrLf
            End If
            intCtr += 1
        End While

        strSectionPileAppels &= stkErreur.ToString

        strSectionInfoCompl &= "Nom de la machine : " & System.Environment.MachineName & vbCrLf & vbCrLf
        strSectionInfoCompl &= "Système d'exploitation : " & ObtenirVersionOS() & " (" & System.Environment.OSVersion.Version.ToString & ")" & vbCrLf & vbCrLf
        strSectionInfoCompl &= "Service pack : " & ObtenirServicePack() & vbCrLf & vbCrLf
        strSectionInfoCompl &= "Nom de l'utilisateur : " & System.Security.Principal.WindowsIdentity.GetCurrent.Name & vbCrLf & vbCrLf
        strSectionInfoCompl &= "Environnement : " & UCase(ConfigurationManager.AppSettings.Item("XW_Environnement")) & vbCrLf & vbCrLf

        ' Construction du message
        strMessg = ConstruireMessage(strSectionMessage, strSectionEmplacement, strSectionPileAppels, strSectionInfoCompl)

        Select Case typEcriture
            Case TypeEcriture.FichierLog, TypeEcriture.FichierTexte 'On inscrit le message d'erreur dans un fichier log ou texte
                FSOStrm = File.Open(strFichier, IO.FileMode.Create, IO.FileAccess.Write)
                FSOWriter = New IO.StreamWriter(FSOStrm)
                FSOWriter.WriteLine(strMessg)
                FSOWriter.Close()
            Case TypeEcriture.FichierLogAvecJournalEvenm, TypeEcriture.FichierTexteAvecJournalEvenm 'On inscrit le message d'erreur dans un fichier log ou texte et dans le journal
                FSOStrm = File.Open(strFichier, IO.FileMode.Create, IO.FileAccess.Write)
                FSOWriter = New IO.StreamWriter(FSOStrm)
                FSOWriter.WriteLine(strMessg)
                FSOWriter.Close()

                EcrireJournal(strSectionMessage, strSectionEmplacement, strSectionPileAppels, strSectionInfoCompl, TypeEvenement.Erreur)
            Case TypeEcriture.JournalEvenm 'On inscrit le message d'erreur dans le journal
                EcrireJournal(strSectionMessage, strSectionEmplacement, strSectionPileAppels, strSectionInfoCompl, TypeEvenement.Erreur)
        End Select

        FSOWriter = Nothing
        FSOStrm = Nothing        
    End Sub

    Private Function ObtenirVersionOS() As String
        Dim versionOS As String = String.Empty
        Select Case Environment.OSVersion.Platform
            Case PlatformID.Win32S
                ObtenirVersionOS = "Windows 3.1"
            Case PlatformID.Win32Windows
                Select Case Environment.OSVersion.Version.Minor
                    Case 0
                        versionOS = "Windows 95"
                    Case 10
                        versionOS = "Windows 98"
                    Case 90
                        versionOS = "Windows ME"
                    Case Else
                        versionOS = "Inconnu"
                End Select
            Case PlatformID.Win32NT
                Select Case Environment.OSVersion.Version.Major
                    Case 3
                        versionOS = "Windows NT 3.51"
                    Case 4
                        versionOS = "Windows NT 4.0"
                    Case 5
                        Select Case _
                            Environment.OSVersion.Version.Minor
                            Case 0
                                versionOS = "Windows 2000"
                            Case 1
                                versionOS = "Windows XP"
                            Case 2
                                versionOS = "Windows 2003"
                        End Select
                    Case Else
                        versionOS = "Inconnu"
                End Select
            Case PlatformID.WinCE
                versionOS = "Windows CE"
        End Select

        Return versionOS

    End Function
#End Region
End Class

Module modErreur
    Private Structure OSVERSIONINFO
        Dim dwOSVersionInfoSize As Integer
        Dim dwMajorVersion As Integer
        Dim dwMinorVersion As Integer
        Dim dwBuildNumber As Integer
        Dim dwPlatformId As Integer
        <VBFixedString(128), MarshalAs(UnmanagedType.ByValTStr, SizeConst:=128)> Dim szCSDVersion As String
    End Structure

    Private Declare Function GetVersionExA Lib "kernel32" (ByRef lpVersionInformation As OSVERSIONINFO) As Short

    Public Function ObtenirServicePack() As String
        Dim osinfo As OSVERSIONINFO = Nothing
        Dim retvalue As Short

        osinfo.dwOSVersionInfoSize = 148
        retvalue = GetVersionExA(osinfo)

        If Len(osinfo.szCSDVersion) = 0 Then
            ObtenirServicePack = ("Aucun service pack d'installé")
        Else
            ObtenirServicePack = (CStr(osinfo.szCSDVersion))
        End If
    End Function
End Module
