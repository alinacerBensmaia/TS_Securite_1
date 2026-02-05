Imports System.Runtime.CompilerServices
Imports System.Security.Principal
Imports System.Text.RegularExpressions
Imports System.Transactions
Imports Rrq.InfrastructureCommune.Parametres
Imports Rrq.InfrastructureCommune.ScenarioTransactionnel
Imports Rrq.InfrastructureCommune.ScenarioTransactionnel.XU5N160_AppelAosExtern
Imports Rrq.TS.AccesseurServiceAOSV1
Imports TS1N234_AccesseurService

Public Class TsCuAccesService
    Implements TsIObtnrCompteGenerique

    <MethodImpl(MethodImplOptions.NoInlining)>
    Public Sub ObtenirCodeAccesMotDePasse(strCle As String, strRaison As String, ByRef strCompte As String, ByRef strMDP As String) Implements TsIObtnrCompteGenerique.ObtenirCodeAccesMotDePasse

        Dim contexte As String
        'En ce moment, on lit l'environnement cible. Cela veut dire que même si on est sur le navigateur de support de production
        'l'environnement qui sera lu c'est celui qui a été choisi par l'utilisateur. On doit traiter le serveur le navigateur de support comme la prod
        'pour qu'il n'essaie pas de lire la phase qui n'existe pas sur lui.
        If XuCuConfiguration.ValeurSysteme("General", "General\Environnement").Equals("PROD") Or XuCuConfiguration.ObtenirValeurSystemeOptionnelle("TS1", "TS1N234\AppelServiceCommePROD", "False").Equals("True") Then
            contexte = XuCaCreerContexte.CreerContexte(New XuDtContexte() With {.CellDonnees = "001", .CellParametre = "001"})
        Else
            contexte = XuCaCreerContexte.CreerContexte(New XuDtContexte() With {.PhaseCentral = "1", .CellDonnees = "001", .CellParametre = "001" })
        End If
        'Comme nous n'avons pas de contexte et nous voulons faire un appel à un service TS pour que la lecture du fichier de cle symgolique
        'se fasse sur le pool TS, nous devons donner un nom de composnat d'integration fictif. Par contre, il faut qu'il soit d'un système différent de TS
        'afin de provoquer le changement de pool.
        XuCaContexte.AssignerValeur(contexte, "CompsIntg", "XU5N152_CiLocalisateurDeService")

        XuCaContexte.AssignerValeur(contexte, "IdInstTrans", Guid.NewGuid().ToString)

        If EstAppelWEBAPI() Then
            XuCuApplicationHost.WCFHost = True
            contexte = ObtenirContexteAppelExterne(contexte)
        End If

        AppelServiceAOSTS(contexte, strCle, strRaison, strCompte, strMDP)

    End Sub

    Public Sub ObtenirCodeAccesMotDePasseLibraire(strCle As String, strRaison As String, ByRef strCompte As String, ByRef strMDP As String) Implements TsIObtnrCompteGenerique.ObtenirCodeAccesMotDePasseLibraire
        Throw New ApplicationException("On devrait jamais utiliser l'obtention de mot de passe libraire en WCF.")
    End Sub

    <MethodImpl(MethodImplOptions.NoInlining)>
    Public Sub AppelServiceAOSTS(ByRef ChaineContexte As String, strCle As String, strRaison As String, ByRef strCompte As String, ByRef strMDP As String)

        Using objAppelSerAOS As New XuCuAppelerAOS(Of TsIAccesseurServiceAOS)

            'Obtenir la référence à l'inscription d'une demande de traitement
            Dim obternirMotPasse As TsIAccesseurServiceAOS = objAppelSerAOS.CreerComposantService(ChaineContexte)

            'Ajouter notre demande
            obternirMotPasse.ObtenirCodeAccesMotDePasse(ChaineContexte, strCle, strRaison, strCompte, strMDP)

            'Vérifier le retour
            objAppelSerAOS.AnalyserRetour(ChaineContexte)

        End Using
    End Sub

    <MethodImpl(MethodImplOptions.NoInlining)>
    Private Function ObtenirContexteAppelExterne(ByRef ChaineContexte As String) As String

        Return XuCuContextAosExtern.PreparerAppel(ChaineContexte, "XU5N152_CiLocalisateurDeService")

    End Function


    Private Function EstAppelWEBAPI() As Boolean
        'Le nom de l'usager est composé comme tel:  Domaine\CodeUtilisateur 
        Dim strDomCodeUtil As String() = WindowsIdentity.GetCurrent.Name.Split("\".ToCharArray())
        Dim regexComptePool As Regex = New Regex("SYS(INT|ACC|FOA|SIM|PRD)APAPI")
        If regexComptePool.IsMatch(strDomCodeUtil(1)) Then
            Return True
        End If

        Return False
    End Function
End Class
