Imports System.ServiceModel
Imports Rrq.CS.ServicesCommuns.ScenarioTransactionnel
Imports Rrq.InfrastructureCommune.ScenarioTransactionnel
Imports TS1N215_INiveauSecrt2
Imports TS1N224_CbCodAccGen
Imports Rrq.InfrastructureCommune.Parametres
Imports System.IO

'''-----------------------------------------------------------------------------
''' Project		: TS1N215_CiCodAccGen
''' Class		: TsCaNiveauSecrt2
''' 	
'''-----------------------------------------------------------------------------
''' <summary>
''' Classe d'affaire.
''' </summary>
'''-----------------------------------------------------------------------------
<ServiceBehavior(ConcurrencyMode:=ConcurrencyMode.Single, InstanceContextMode:=InstanceContextMode.PerCall, AddressFilterMode:=AddressFilterMode.Any)> _
Public Class TsCaNiveauSecrt2
    Inherits ClassesBaseIntegration.XuCaBaseComposantV2
    Implements TsICompI

    <OperationBehavior(TransactionScopeRequired:=False, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)> _
    Private Function EnregistrerCle(ByRef ChaineContexte As String, ByVal CleSymbolique As TS1N201_DtCdAccGenV1.TsDtCleSym, ByVal IndicMdpNouveau As Boolean, ByVal IndicMaj As Boolean) As Boolean Implements TsICompI.EnregistrerCle

        Dim result As Boolean = False
        Dim CodeAccesGen As TS1N224_CbCodAccGen.TsCaCodAccGen = New TS1N224_CbCodAccGen.TsCaCodAccGen

        Try

            result = CodeAccesGen.EnregistrerCle(ChaineContexte, CleSymbolique, IndicMdpNouveau, IndicMaj)

        Catch ex As XuExcEErrFonctionnelle
            MyBase.CreerRetourErrFonctionnelle(ChaineContexte, ex)
        Catch ex As XuExcEErrValidation
            'Pour les applications Client/Serveur, on doit convertir l'exception XU en exception XZ.
            MyBase.CreerRetourErrValidation(ChaineContexte, New XZCuErrValdtException(ex))
        End Try

        Return result

    End Function


    <OperationBehavior(TransactionScopeRequired:=False, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)> _
    Private Function DetruireCle(ByRef ChaineContexte As String, ByVal CleSymbolique As TS1N201_DtCdAccGenV1.TsDtCleSym) As Boolean Implements TsICompI.DetruireCle

        Dim result As Boolean = False

        Try

            result = TsCaCodAccGen.DetruireCle(ChaineContexte, CleSymbolique)

        Catch ex As XuExcEErrFonctionnelle
            MyBase.CreerRetourErrFonctionnelle(ChaineContexte, ex)
        Catch ex As XuExcEErrValidation
            'Pour les applications Client/Serveur, on doit convertir l'exception XU en exception XZ.
            MyBase.CreerRetourErrValidation(ChaineContexte, New XZCuErrValdtException(ex))
        End Try

        Return result

    End Function


    <OperationBehavior(TransactionScopeRequired:=False, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)> _
    Private Sub ImporterCles(ByRef ChaineContexte As String) Implements TsICompI.ImporterCles

        Dim CodeAccesGen As TS1N224_CbCodAccGen.TsCaCodAccGen = New TS1N224_CbCodAccGen.TsCaCodAccGen

        Try

            CodeAccesGen.ImporterCles(ChaineContexte)

        Catch ex As XuExcEErrFonctionnelle
            MyBase.CreerRetourErrFonctionnelle(ChaineContexte, ex)
        Catch ex As XuExcEErrValidation
            'Pour les applications Client/Serveur, on doit convertir l'exception XU en exception XZ.
            MyBase.CreerRetourErrValidation(ChaineContexte, New XZCuErrValdtException(ex))
        End Try

    End Sub


    <OperationBehavior(TransactionScopeRequired:=False, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)> _
    Private Sub ExporterCles(ByRef ChaineContexte As String) Implements TsICompI.ExporterCles

        Dim CodeAccesGen As TS1N224_CbCodAccGen.TsCaCodAccGen = New TS1N224_CbCodAccGen.TsCaCodAccGen

        Try

            CodeAccesGen.ExporterCles(ChaineContexte)

        Catch ex As XuExcEErrFonctionnelle
            MyBase.CreerRetourErrFonctionnelle(ChaineContexte, ex)
        Catch ex As XuExcEErrValidation
            'Pour les applications Client/Serveur, on doit convertir l'exception XU en exception XZ.
            MyBase.CreerRetourErrValidation(ChaineContexte, New XZCuErrValdtException(ex))
        End Try

    End Sub


    <OperationBehavior(TransactionScopeRequired:=False, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)> _
    Private Function ObtenirVerrouEdition(ByRef ChaineContexte As String) As TsDtVerrou Implements TsICompI.ObtenirVerrouEdition

        Dim verrouRetour As New TsDtVerrou()
        verrouRetour.InVerObt = False

        Try

            Dim utilisateur As String = XuCaContexte.CodeUsagerEssai(ChaineContexte)
            Dim fichierVerrou As String = XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N224\FichierVerrouEdition")

            ' Lire le verrou
            If File.Exists(fichierVerrou) Then
                Dim verrou As String = File.ReadAllText(fichierVerrou)

                If Not String.IsNullOrEmpty(fichierVerrou) Then
                    Dim informationsVerrou As String() = verrou.Split(","c)

                    If informationsVerrou.Length >= 1 Then
                        verrouRetour.CoUtlPro = informationsVerrou(0)
                    End If

                    If informationsVerrou.Length >= 2 AndAlso informationsVerrou(0) <> utilisateur Then
                        Dim dateVerrouActif As DateTime = DateTime.Parse(informationsVerrou(1))

                        If dateVerrouActif > DateTime.Now.AddHours(-4) Then
                            ' Un verrou est toujours actif, refuser le mode édition
                            Return verrouRetour
                        End If
                    End If
                End If
            End If

            ' Créer un nouveau verrou ou le mettre à jour
            File.WriteAllText(fichierVerrou, utilisateur + "," + DateTime.Now.ToString())
            verrouRetour.InVerObt = True

        Catch ex As XuExcEErrFonctionnelle
            MyBase.CreerRetourErrFonctionnelle(ChaineContexte, ex)
        Catch ex As XuExcEErrValidation
            'Pour les applications Client/Serveur, on doit convertir l'exception XU en exception XZ.
            MyBase.CreerRetourErrValidation(ChaineContexte, New XZCuErrValdtException(ex))
        End Try

        Return verrouRetour

    End Function


    <OperationBehavior(TransactionScopeRequired:=False, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)> _
    Private Sub RelacherVerrouEdition(ByRef ChaineContexte As String) Implements TsICompI.RelacherVerrouEdition

        Try

            Dim fichierVerrou As String = XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N224\FichierVerrouEdition")

            If File.Exists(fichierVerrou) Then
                File.Delete(fichierVerrou)
            End If

        Catch ex As XuExcEErrFonctionnelle
            MyBase.CreerRetourErrFonctionnelle(ChaineContexte, ex)
        Catch ex As XuExcEErrValidation
            'Pour les applications Client/Serveur, on doit convertir l'exception XU en exception XZ.
            MyBase.CreerRetourErrValidation(ChaineContexte, New XZCuErrValdtException(ex))
        End Try

    End Sub


End Class
