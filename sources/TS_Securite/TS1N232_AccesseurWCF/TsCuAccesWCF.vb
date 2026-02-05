Imports System.Runtime.CompilerServices
Imports TS1N233_IAccesseurWCF

Public Class TsCuAccesWCF
    Implements TsIObtnrCompteGenerique

    Public Sub ObtenirCodeAccesMotDePasse(strCle As String, strRaison As String, ByRef strCompte As String, ByRef strMDP As String) Implements TsIObtnrCompteGenerique.ObtenirCodeAccesMotDePasse
        Using cleSymbolique As New TsCuProxyAccesseurWCF()
            'Il est impossible de passer le compte du domaine ZE vers la logique d'affaire par le contexte de l'appel WCF.
            'Pour cette raison, nous devons passer le code usager du domaine ZE en paramètre. Afin de limiter l'utilisation de cette méthode nous devons:
            '- S'assurer qu'elle est appelée à partir d'un serveur de presentation web (ZWEPRE ou SWEPRE)
            '- S'assurer que l'utilisateur courrant appartient vraiment au domaine ZE 
            If ((Environment.MachineName.StartsWith("ZWEPRE") Or Environment.MachineName.StartsWith("SWEPRE")) And Security.Principal.WindowsIdentity.GetCurrent.Name.StartsWith("ZERRQ")) Then
                cleSymbolique.ObtenirCodeAccesMotDePasse(strCle, strRaison, Security.Principal.WindowsIdentity.GetCurrent.Name, strCompte, strMDP)
            Else
                cleSymbolique.ObtenirCodeAccesMotDePasse(strCle, strRaison, strCompte, strMDP)
            End If
        End Using
    End Sub

    Public Sub ObtenirCodeAccesMotDePasseLibraire(strCle As String, strRaison As String, ByRef strCompte As String, ByRef strMDP As String) Implements TsIObtnrCompteGenerique.ObtenirCodeAccesMotDePasseLibraire
        Using cleSymbolique As New TsCuProxyAccesseurWCF()
            cleSymbolique.ObtenirCodeAccesMotDePasseLibraire(strCle, strRaison, strCompte, strMDP)
        End Using
    End Sub

End Class
