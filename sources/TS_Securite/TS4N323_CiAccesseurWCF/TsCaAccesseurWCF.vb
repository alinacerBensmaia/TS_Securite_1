Imports System.ServiceModel
Imports Rrq.InfrastructureCommune.ScenarioTransactionnel
Imports TS4N323_IAccesseurWCF
Imports Rrq.Securite.Applicative

'''-----------------------------------------------------------------------------
''' Project		: TS4N323_CiAccesseurWCF
''' Class		: TsCaAccesseurWCF
''' 	
'''-----------------------------------------------------------------------------
''' <summary>
''' Classe d'affaire.
''' </summary>
'''-----------------------------------------------------------------------------
<ServiceBehavior(ConcurrencyMode:=ConcurrencyMode.Single, InstanceContextMode:=InstanceContextMode.PerCall, AddressFilterMode:=AddressFilterMode.Any)> _
Public Class TsCaAccesseurWCF
    Implements TsIAccesseurWCF

    <OperationBehavior(TransactionScopeRequired:=False, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)> _
    Private Function EstMembreGroupe(ByVal NomGroupe As String, ByVal CodeUsager As String) As Boolean Implements TsIAccesseurWCF.EstMembreGroupe

        Return (New TsCaVerfrSecrtApplicative).EstMembreGroupe(NomGroupe, CodeUsager)

    End Function

    <OperationBehavior(TransactionScopeRequired:=False, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)>
    Private Function EstMembreGroupeV2(ByVal CodeUsager As String, ByVal NomGroupes As Generic.IList(Of String)) As Generic.IDictionary(Of String, Boolean) Implements TsIAccesseurWCF.EstMembreGroupeV2

        Return (New TsCaVerfrSecrtApplicative).EstMembreGroupeV2(CodeUsager, NomGroupes)

    End Function

    <OperationBehavior(TransactionScopeRequired:=False, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)> _
    Private Function ObtenirUtilisateurGroupe(ByVal NomGroupe As Generic.IList(Of String), ByVal Recursif As Boolean) As Generic.IList(Of Rrq.Securite.Applicative.TsDtUtilisateur) Implements TsIAccesseurWCF.ObtenirUtilisateurGroupe

        Return (New TsCaVerfrSecrtApplicative).ObtenirUtilisateurGroupe(NomGroupe, Recursif)

    End Function


    <OperationBehavior(TransactionScopeRequired:=False, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)> _
    Private Function ObtenirGroupeUtilisateur(ByVal CodeUsager As String) As Generic.IList(Of Rrq.Securite.Applicative.TsDtGroupe) Implements TsIAccesseurWCF.ObtenirGroupeUtilisateur

        Return (New TsCaVerfrSecrtApplicative).ObtenirGroupeUtilisateur(CodeUsager)

    End Function


    <OperationBehavior(TransactionScopeRequired:=False, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)> _
    Private Function ObtenirGroupeMembreDe(ByVal NomGroupe As String, ByVal Recursif As Boolean) As Generic.IList(Of Rrq.Securite.Applicative.TsDtGroupe) Implements TsIAccesseurWCF.ObtenirGroupeMembreDe

        Return (New TsCaVerfrSecrtApplicative).ObtenirGroupeMembreDe(NomGroupe, Recursif)

    End Function


    <OperationBehavior(TransactionScopeRequired:=False, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)> _
    Private Function ObtenirGroupe(ByVal NomGroupe As String) As Rrq.Securite.Applicative.TsDtGroupe Implements TsIAccesseurWCF.ObtenirGroupe

        Return (New TsCaVerfrSecrtApplicative).ObtenirGroupe(NomGroupe)

    End Function


    <OperationBehavior(TransactionScopeRequired:=False, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)> _
    Private Function RechercherGroupes(ByVal Filtre As String) As Generic.IList(Of String) Implements TsIAccesseurWCF.RechercherGroupes

        Return (New TsCaVerfrSecrtApplicative).RechercherGroupes(Filtre)

    End Function

End Class
