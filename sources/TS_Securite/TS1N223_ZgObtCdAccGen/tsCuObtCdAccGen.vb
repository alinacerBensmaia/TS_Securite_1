
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Security.Principal
Imports System.Text.RegularExpressions
Imports Rrq.InfrastructureCommune.Parametres
Imports Rrq.Securite.CleSymbolique

Public Class tsCuObtCdAccGen

    Private cleSymbolique As TsIObtnrCompteGenerique
    Private ressourceCleSymbolique As Integer = ParseEnum(Of TsRessourceCleSymbolique)(XuCuPolitiqueConfig.ConfigDomaine.ValeurSysteme("TS1", "TS1\TS1N223\RessourceCleSymbolique"))
    Private Shared Environnement As String

    Public Sub New()

        InfrastructureCommune.UtilitairesCommuns.XuCuChargementAssembly.CreerHandlerAssemblyResolve()
        ' Les filtres sont imbriqués de la façon suivante:
        ' securiteApplicative -> mémorisation -> acces

        ' Dans tous les cas, on instancie d'abord l'accesseur
        Dim accesseur As TsIObtnrCompteGenerique = Nothing
        Select Case ressourceCleSymbolique
            Case TsRessourceCleSymbolique.FICHIER
                If EstAppelWCFouWEBAPI() Then
                    'Si on est déjà sur le pool TS, on peut lire le ficheir de clé symbolique.
                    'Sinon, on fait un appel de service pour changer de pool.
                    If EstPoolTS() Then
                        accesseur = CreerAccesseurFichier()
                    Else
                        accesseur = CreerAccesseurService()
                    End If
                Else
                    accesseur = CreerAccesseurFichier()
                End If

            Case TsRessourceCleSymbolique.CIWCF
                'Si la l'appel est pour la création des pools, 
                'il faut dans ce cas lire le fichier et ne pas faire un appel WCF, même si le config indique WCFCI
                If AppelPourCreationPoolWXCF() Then
                    accesseur = CreerAccesseurFichier()
                Else
                    'Si l'appel n'est pas pour la création d'un pool,
                    'Cela veut dire qu'on est sur la présetation ou sur le LOT.
                    'Sur le LOT, ont peut déjà être sur un pool WCF, dans ce cas il faut faire un appel de service.
                    'Sinon, on est sur la présentation et il faut faire un appel à un CI
                    If EstAppelWCFouWEBAPI() Then
                        accesseur = CreerAccesseurService()
                    Else
                        accesseur = CreerAccesseurCIWCF()
                    End If
                End If

            Case Else
                ' Garde-fou au cas où un autre accesseur soit ajouté ou que la propriété contienne du garbage 
                Throw New ApplicationException("Le ressource de fichier de clé symbolique n'est pas valide")
        End Select

        cleSymbolique = accesseur
    End Sub

    Public Sub ObtenirCodeAccesMotDePasse(ByVal strCle As String, ByVal strRaison As String, ByRef strCompte As String, ByRef strMDP As String)

        If Not EstEnvrUnitiare() AndAlso (AppelPourCreationPoolWXCF() Or AppelPourChangementBigIP()) AndAlso Rrq.Securite.tsCuObtnrInfoAD.ObtenirUtilisateur.EstCompteAdmin Then
            cleSymbolique.ObtenirCodeAccesMotDePasseLibraire(strCle, strRaison, strCompte, strMDP)
        Else
            cleSymbolique.ObtenirCodeAccesMotDePasse(strCle, strRaison, strCompte, strMDP)
        End If


    End Sub



#Region "--- Privées ---"

    Private Function AppelPourCreationPoolWXCF() As Boolean
        Dim assemblyCourant As Assembly = Assembly.GetExecutingAssembly()
        Dim pileAppel As StackTrace = New StackTrace()
        Dim methodes() As StackFrame = pileAppel.GetFrames
        For Each methode As StackFrame In methodes
            If methode.GetMethod.DeclaringType IsNot Nothing AndAlso methode.GetMethod.DeclaringType.Assembly.GetName.Name.ToUpper.Trim.Equals("XU7N043_GERERSITESIISWCF") Then
                Return True
            End If
        Next
        Return False
    End Function

    <MethodImpl(MethodImplOptions.NoInlining)>
    Private Function CreerAccesseurFichier() As TsIObtnrCompteGenerique
        Return New TsCuAccesFichier
    End Function

    <MethodImpl(MethodImplOptions.NoInlining)>
    Private Function CreerAccesseurService() As TsIObtnrCompteGenerique
        Return New TsCuAccesService
    End Function

    <MethodImpl(MethodImplOptions.NoInlining)>
    Private Function CreerAccesseurCIWCF() As TsIObtnrCompteGenerique
        Return New TsCuAccesWCF()
    End Function

    Private Function EstEnvrUnitiare() As Boolean
        If Environnement Is Nothing Then
            Environnement = XuCuPolitiqueConfig.ConfigDomaine.ValeurSysteme("General", "Environnement")
        End If

        Return Environnement.Equals("UNIT")
    End Function

    '<MethodImpl(MethodImplOptions.NoInlining)>
    'Private Function EstComopteAdmin() As Boolean
    '    Try
    '        Return Rrq.Securite.tsCuObtnrInfoAD.ObtenirUtilisateur.EstCompteAdmin
    '    Catch ex As System.Runtime.InteropServices.COMException
    '        'Si on attrapé l'exception, c'est parce que le serveur n'a probablement pas accès à l'AD. Alors on fait une vérification selon le nom du compte
    '        If Environment.UserName.ToUpper.EndsWith("AS") Then
    '            Return True
    '        End If
    '        Return False
    '    End Try

    'End Function


    ' Idéalement, on déclarerait T As [Enum], mais "'Enum' ne peut pas être utilisé en tant que contrainte de type."
    Private Function ParseEnum(Of T)(ByVal valeur As String) As T
        If GetType(T).BaseType IsNot GetType([Enum]) Then
            Throw New FormatException("T doit être une énumération")
        End If

        ' On est gentil et on ignore la casse dans le parsing
        Return DirectCast([Enum].Parse(GetType(T), valeur, True), T)
    End Function

    Private Function EstAppelWCFouWEBAPI() As Boolean
        'Le nom de l'usager est composé comme tel:  Domaine\CodeUtilisateur 
        Dim strDomCodeUtil As String() = WindowsIdentity.GetCurrent.Name.Split("\".ToCharArray())
        Dim regexComptePool As Regex = New Regex("ZAP[IABSP]W|SYS(INT|ACC|FOA|SIM|PRD)APAPI")
        If regexComptePool.IsMatch(strDomCodeUtil(1)) Then
            Return True
        End If

        Return False
    End Function

    Private Function EstPoolTS() As Boolean
        'Le nom de l'usager est composé comme tel:  Domaine\CodeUtilisateur 
        Dim strDomCodeUtil As String() = WindowsIdentity.GetCurrent.Name.Split("\".ToCharArray())
        Dim regexComptePool As Regex = New Regex("ZAP[IABSP]WTS")
        If regexComptePool.IsMatch(strDomCodeUtil(1)) Then
            Return True
        End If

        Return False
    End Function

    Private Function AppelPourChangementBigIP() As Boolean
        Dim assemblyCourant As Assembly = Assembly.GetExecutingAssembly()
        Dim pileAppel As StackTrace = New StackTrace()
        Dim methodes() As StackFrame = pileAppel.GetFrames
        For Each methode As StackFrame In methodes
            If methode.GetMethod.DeclaringType IsNot Nothing AndAlso methode.GetMethod.DeclaringType.Assembly.GetName.Name.ToUpper.Trim.Equals("AYTN111_GESTIONBIGIPCMD") Then
                Return True
            End If
        Next
        Return False
    End Function


#End Region

End Class
