Imports System.ServiceModel
Imports Rrq.CS.ServicesCommuns.ScenarioTransactionnel
Imports Rrq.InfrastructureCommune.ScenarioTransactionnel
Imports TS1N215_INiveauSecrt1
Imports TS1N224 = TS1N224_CbCodAccGen

'''-----------------------------------------------------------------------------
''' Project		: TS1N215_CiCodAccGen
''' Class		: TsCaNiveauSecrt1
''' 	
'''-----------------------------------------------------------------------------
''' <summary>
''' Classe d'affaire.
''' </summary>
'''-----------------------------------------------------------------------------
<ServiceBehavior(ConcurrencyMode:=ConcurrencyMode.Single, InstanceContextMode:=InstanceContextMode.PerCall, AddressFilterMode:=AddressFilterMode.Any)> _
Public Class TsCaNiveauSecrt1
    Inherits ClassesBaseIntegration.XuCaBaseComposantV2
	Implements TsICompI

    <OperationBehavior(TransactionScopeRequired:=False, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)> _
	Private Function ObtenirListeCle(ByRef ChaineContexte as String) as Generic.IList(Of TS1N201_DtCdAccGenV1.TsDtCleSym) Implements TsICompI.ObtenirListeCle

        Dim listeCle As Generic.IList(Of TS1N201_DtCdAccGenV1.TsDtCleSym)
        listeCle = New Generic.List(Of TS1N201_DtCdAccGenV1.TsDtCleSym)()

        Try

            listeCle = TS1N224.TsCaCodAccGen.ObtenirListeCle(ChaineContexte)

        Catch ex As XuExcEErrFonctionnelle
            MyBase.CreerRetourErrFonctionnelle(ChaineContexte, ex)
        Catch ex As XuExcEErrValidation
            'Pour les applications Client/Serveur, on doit convertir l'exception XU en exception XZ.
            MyBase.CreerRetourErrValidation(ChaineContexte, New XZCuErrValdtException(ex))
        End Try
		
        Return listeCle
		
	End Function

	
	<OperationBehavior(TransactionScopeRequired:=False, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)> _
	Private Function AfficherMDP(ByRef chaineContexte as String, ByVal cle as TS1N201_DtCdAccGenV1.TsDtCleSym) as String Implements TsICompI.AfficherMDP

        Dim codeAccesGen As New TS1N224_CbCodAccGen.TsCaCodAccGen()
        Dim mdp As String = String.Empty

        Try

            mdp = codeAccesGen.AfficherMDP(chaineContexte, cle)

        Catch ex As XuExcEErrFonctionnelle
            MyBase.CreerRetourErrFonctionnelle(chaineContexte, ex)
        Catch ex As XuExcEErrValidation
            'Pour les applications Client/Serveur, on doit convertir l'exception XU en exception XZ.
            MyBase.CreerRetourErrValidation(chaineContexte, New XZCuErrValdtException(ex))
        End Try
		
        Return mdp
		
	End Function
	
End Class
