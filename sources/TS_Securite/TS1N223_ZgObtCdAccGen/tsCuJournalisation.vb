Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports Rrq.InfrastructureCommune.UtilitairesCommuns
Imports Rrq.InfrastructureCommune.UtilitairesCommuns.XuGeJournalEvenement
Imports Rrq.InfrastructureCommune.UtilitairesCommuns.XuGeTypeEvenement
Imports GestEvent = Rrq.InfrastructureCommune.UtilitairesCommuns.XuCuGestionEvent

<EditorBrowsable(EditorBrowsableState.Never)>
Friend Class tsCuJournalisation

    Public Enum TypeEvenement
        Erreur = 0
        Avertissement = 1
        Information = 2
    End Enum


#Region "*-----  Méthodes publiques   -----*"

    Public Overloads Sub EcrireJournal(ByVal strCle As String, ByVal strCompte As String, ByVal strUtilisateur As String, ByVal strPriorite As String,
                                            ByVal strCommentaire As String, ByVal intTypeMessage As TypeEvenement)

        Dim typeEvenement As XuGeTypeEvenement = intTypeMessage.Convert()
        GestEvent.AjouterEvenmSpecifique(XuGeJeSecuriteRRQ, typeEvenement, 102, strCle, strCompte, strUtilisateur, strPriorite, strCommentaire)
    End Sub

    Public Overloads Sub EcrireJournal(ByVal strCle As String, ByVal strCompte As String, ByVal strEnvrn As String, ByVal strUtilisateur As String,
                                        ByVal strPriorite As String, ByVal strCommentaire As String, ByVal intTypeMessage As TypeEvenement)

        Dim typeEvenement As XuGeTypeEvenement = intTypeMessage.Convert()
        GestEvent.AjouterEvenmSpecifique(XuGeJeSecuriteRRQ, typeEvenement, 103, strCle, strCompte, strEnvrn, strUtilisateur, strPriorite, strCommentaire)
    End Sub

    Public Overloads Sub EcrireJournal(ByVal strType As String, ByVal strCle As String, ByVal strProfil As String)
        GestEvent.AjouterEvenmSpecifique(XuGeJeSecuriteRRQ, XuGeTeSucces, 104, strType, strCle, strProfil)
    End Sub

    'Pour les ID de message 105 et 106
    Public Overloads Sub EcrireJournal(ByVal strType As String, ByVal strCle As String, ByVal intIDMesg As Integer)
        GestEvent.AjouterEvenmSpecifique(XuGeJeSecuriteRRQ, XuGeTeSucces, intIDMesg, strType, strCle)
    End Sub

    Public Overloads Sub EcrireJournal(ByVal strType As String, ByVal strCle As String, ByVal strUtilisateur As String, ByVal strPriorite As String)
        GestEvent.AjouterEvenmSpecifique(XuGeJeSecuriteRRQ, XuGeTeErreur, 107, strType, strCle, strUtilisateur, strPriorite)
    End Sub

    Public Overloads Sub EcrireJournal(ByVal objException As Exception)
        GestEvent.AjouterEvenmSpecifique(XuGeJeSecuriteRRQ, XuGeTeErreur, 400, objException.Source, objException.Message, objException.StackTrace.Replace(Environment.NewLine, " ; "))
    End Sub

#End Region

End Class

Friend Module JournalisationExtensions

    <Extension>
    Public Function Convert(source As tsCuJournalisation.TypeEvenement) As XuGeTypeEvenement
        ' Convertir le type de message propriétaire en type de message EventLogEntry
        Select Case source
            Case tsCuJournalisation.TypeEvenement.Avertissement
                Return XuGeTeAvertissement
            Case tsCuJournalisation.TypeEvenement.Erreur
                Return XuGeTeErreur
            Case tsCuJournalisation.TypeEvenement.Information
                Return XuGeTeInformation
        End Select

        Dim valeurParDefaut As XuGeTypeEvenement
        Return valeurParDefaut
    End Function

End Module