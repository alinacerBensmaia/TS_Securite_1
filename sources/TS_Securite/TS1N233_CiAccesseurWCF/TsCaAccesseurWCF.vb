Imports System.ServiceModel
Imports Rrq.Securite.CleSymbolique
Imports TS1N233_IAccesseurWCF

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

    <OperationBehavior(TransactionScopeRequired:=False, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)>
    Public Sub ObtenirCodeAccesMotDePasse(strCle As String, strRaison As String, ByRef strCompte As String, ByRef strMDP As String) Implements TsIAccesseurWCF.ObtenirCodeAccesMotDePasse
        Dim cleSymbolique As New tsCuObtCdAccGen()
        cleSymbolique.AssemblyCaller = System.Reflection.Assembly.GetAssembly(Me.GetType)
        cleSymbolique.ObtenirCodeAccesMotDePasse(strCle, strRaison, strCompte, strMDP)
    End Sub

    <OperationBehavior(TransactionScopeRequired:=False, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)>
    Public Sub ObtenirCodeAccesMotDePasse(strCle As String, strRaison As String, strUsagerZE As String, ByRef strCompte As String, ByRef strMDP As String) Implements TsIAccesseurWCF.ObtenirCodeAccesMotDePasse
        Dim cleSymbolique As New tsCuObtCdAccGen()
        cleSymbolique.AssemblyCaller = System.Reflection.Assembly.GetAssembly(Me.GetType)
        cleSymbolique.UserDomaineZE = strUsagerZE
        cleSymbolique.ObtenirCodeAccesMotDePasse(strCle, strRaison, strCompte, strMDP)
    End Sub


    <OperationBehavior(TransactionScopeRequired:=False, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)>
    Public Sub ObtenirCodeAccesMotDePasseLibraire(strCle As String, strRaison As String, ByRef strCompte As String, ByRef strMDP As String) Implements TsIAccesseurWCF.ObtenirCodeAccesMotDePasseLibraire
        Dim cleSymbolique As New tsCuObtCdAccGen()
        cleSymbolique.AssemblyCaller = System.Reflection.Assembly.GetAssembly(Me.GetType)
        cleSymbolique.ObtenirCodeAccesMotDePasseLibraire(strCle, strRaison, strCompte, strMDP)
    End Sub

End Class
