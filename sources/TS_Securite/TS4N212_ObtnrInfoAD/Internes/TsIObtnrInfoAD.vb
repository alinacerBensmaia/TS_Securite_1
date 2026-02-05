Imports System.Collections.Generic
Imports System.DirectoryServices

Friend Interface TsIObtnrInfoAD

    ReadOnly Property ServeurActiveDirectory As String
    ReadOnly Property DomaineNT As String

    ''' --------------------------------------------------------------------------------
    ''' <summary>
    '''     Cette procédure obtient un utilisateur unique dans l'active directory.
    ''' </summary>
    ''' <remarks><para><pre>
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    Function ObtenirUtilisateur(ByVal strCodeUtilisateur As String) As TsCuUtilisateurAD


    ''' --------------------------------------------------------------------------------
    ''' <summary>
    '''     Cette méthode retourne une liste d'utilisateurs récupérés dans l'active directory.
    ''' </summary>
    ''' <param name="pTypeRequete">
    ''' 	Le champ sur lequel s'effectue la requête à l'active directory. 
    ''' </param>
    ''' <param name="pCritereRecherche">
    ''' 	Le critère de recherche à l'active directory. 
    ''' </param>
    ''' <param name="pCritereRechercheSecondaire">
    ''' 	Le critère de recherche secondaire à l'active directory utilisé lors d'une recherche nom et prénom. 
    ''' </param>
    ''' <param name="pCategorie">
    ''' 	Catégorie d'objets (personne, groupe ou les deux) à utilisé lors de la recherche. 
    ''' </param>
    ''' <returns>Retourne une liste d'utilisateurs sous format List(Of TsCuUtilisateur)</returns>
    ''' --------------------------------------------------------------------------------
    Function ObtenirListeUtilisateur(ByVal pTypeRequete As TsIadTypeRequete,
                                     ByVal pCritereRecherche As String,
                                     ByVal pCritereRechercheSecondaire As String,
                                     ByVal pCategorie As TsIadObjectCategory) As List(Of TsCuUtilisateurAD)

    ''' --------------------------------------------------------------------------------
    ''' <summary>
    '''     Effectue une recherche dans l'AD en utilisant les paramètres reçus.
    ''' </summary>
    ''' <param name="pTypeRequete">
    ''' 	Indicateur du champs sur lequel le recherche doit être effectuée. 
    ''' </param>
    ''' <param name="strCritereRecherche">
    ''' 	Valeur recherchée, peut être une valeur spécifique ou un masque de recherche (ex: T20*). 
    ''' </param>
    ''' <param name="strCritereRechercheSecondaire">
    ''' 	Le critère de recherche secondaire à l'active directory utilisé lors d'une recherche nom et prénom. 
    ''' </param>
    ''' <returns><see cref="Data.DataTable" />	(System.Data.DataTable)</returns>
    ''' <remarks><para><pre>
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    Function RechercheActiveDirectory(ByVal pTypeRequete As TsIadTypeRequete,
                                      ByVal strCritereRecherche As String,
                                      ByVal strCritereRechercheSecondaire As String,
                                      ByVal pObjectCategory As TsIadObjectCategory) As DataTable


    ''' <summary>
    ''' Cette méthode retourne la liste des membres du groupe passé en paramètre.
    ''' </summary>
    ''' <param name="strGroupe">Représente le groupe pour lequel on désire avoir la liste des membres.</param>
    ''' <param name="blnRechRecursive"></param>
    ''' <returns>Une DataTable avec les utilisateur membre du groupe. Les noms de colonne du DataTable sont les nom des attributs de l'AD.</returns>
    Function RechercheGroupeAD(strGroupe As String, blnRechRecursive As Boolean) As DataTable


    ''' <summary>
    ''' Cette méthode retourne une liste de groupe contenu dans l'AD selon un filtre.
    ''' </summary>
    ''' <param name="Filtre">Filtre a appliquer pour la rechercher dans l'AD.</param>
    ''' <returns>Une collection contenant groupes trouvés.</returns>
    Function ObtenirListeGroupes(Filtre As String) As SearchResultCollection

    ''' <summary>
    ''' Fonction récursive de recherche de groupe.  Il recherche dans toute la cascade de groupes que l'utilisateur peut avoir.
    ''' </summary>
    ''' <param name="strACID">Nom du groupe dans lequel on recherche présentement le groupe désiré.</param>
    ''' <param name="strGroupeRecherche">Nom du groupe que l'on recherche.</param>
    ''' <returns></returns>
    Function ChercheDansGroupes(strACID As String, strGroupeRecherche As String) As Boolean

    Function ObtenirMembresGroupe(NomGroupe As String) As String()

    ''' <summary>
    ''' Cette méthode retourne la liste des membres du groupe passé en paramètre.
    ''' </summary>
    ''' <param name="strGroupe">Représente le groupe pour lequel on désire avoir la liste des membres.</param>
    ''' <param name="blnRechRecursive"></param>
    ''' <returns>Une liste d'utilisateur.</returns>
    Function ObtenirListeMembreGroupe(strGroupe As String, blnRechRecursive As Boolean) As List(Of TsCuUtilisateurAD)

    ''' <summary>
    ''' Cette méthode vérifie si un groupe existe dans l'AD.
    ''' </summary>
    ''' <param name="NomGroupe">Nom du groupe a vérifier</param>
    ''' <returns></returns>
    Function VerifierGroupeExiste(NomGroupe As String) As Boolean
End Interface
