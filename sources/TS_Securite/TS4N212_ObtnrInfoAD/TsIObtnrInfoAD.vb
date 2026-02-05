Imports System.Collections.Generic
Imports System.DirectoryServices

Friend Interface TsIObtnrInfoAD

    ''' --------------------------------------------------------------------------------
    ''' <summary>
    '''     Cette propriété retourne le domaine accédé par la l'intance de la classe.
    ''' </summary>
    ''' <value></value>
    ''' <remarks><para><pre>
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    ReadOnly Property Domaine() As TsIadNomDomaine


    ''' --------------------------------------------------------------------------------
    ''' <summary>
    '''     Cette procédure obtient un utilisateur unique dans l'active directory.
    ''' </summary>
    ''' <remarks><para><pre>
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    Function ObtenirUtilisateur(ByVal strCodeUtilisateur As String) As TsCuUtilisateurAD


    ''' <summary>
    '''   Cette méthode retourne une liste de groupe contenu dans l'AD selon un filtre.
    ''' </summary>
    ''' <param name="Filtre">Filtre a appliquer pour la rechercher dans l'AD.</param>
    ''' <returns>Une collection contenant groupes trouvés.</returns>
    Function ObtenirListeGroupes(ByVal Filtre As String) As SearchResultCollection


    ''' --------------------------------------------------------------------------------
    ''' <summary>
    '''     Cette méthode retourne une liste d'utilisateurs récupérés dans l'active directory.
    ''' </summary>
    ''' <param name="pTypeRequete">
    ''' 	Le champ sur lequel s'effectue la requête à l'active directory. 
    ''' 	Value Type: see cref="Securite.tsCuObtnrInfoAD.XzIadTypeRequete" />	(Rrq.Securite.tsCuObtnrInfoAD.XzIadTypeRequete)
    ''' </param>
    ''' <param name="pCritereRecherche">
    ''' 	Le critère de recherche à l'active directory. 
    ''' 	Value Type: <see cref="String" />	(System.String)
    ''' </param>
    ''' <param name="pCritereRechercheSecondaire">
    ''' 	Le critère de recherche secondaire à l'active directory utilisé lors d'une recherche nom et prénom. 
    '''     Value Type: <see cref="String" />	(System.String)
    ''' </param>
    ''' <param name="pCategorie">
    ''' 	Catégorie d'objets (personne, groupe ou les deux) à utilisé lors de la recherche. 
    '''     Value Type: <see cref="Securite.tsCuObtnrInfoAD.TsIadObjectCategory" />	(RRQ.Securite.tsCuObtnrInfoAD.TsIadObjectCategory)
    ''' </param>
    ''' <returns>Retourne une liste d'utilisateurs sous format List(Of TsCuUtilisateur)</returns>
    ''' --------------------------------------------------------------------------------
    Function ObtenirListeUtilisateur(ByVal pTypeRequete As TsIadTypeRequete, ByVal pCritereRecherche As String, _
                                       ByVal pCritereRechercheSecondaire As String, _
                                       ByVal pCategorie As TsIadObjectCategory) As List(Of TsCuUtilisateurAD)


    ''' <summary>
    '''  Cette méthode vérifie si un groupe existe dans l'AD.
    ''' </summary>
    ''' <param name="NomGroupe">Nom du groupe a vérifier</param>
    ''' <returns></returns>
    Function VerifierGroupeExiste(ByVal NomGroupe As String) As Boolean


    ''' --------------------------------------------------------------------------------
    ''' <summary>
    '''     Cette méthode retourne la liste des membres du groupe passé en paramètre.
    ''' </summary>
    ''' <param name="strGroupe">
    ''' 	Représente le groupe pour lequel on désire avoir la liste des membres. 
    ''' 	Value Type: <see cref="String" />	(System.String)
    ''' </param>
    ''' <returns>Une liste d'utilisateur. List(of TsCuUtilisateurAD)</returns>
    ''' <remarks>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    Function ObtenirListeMembreGroupe(ByVal strGroupe As String, ByVal blnRechRecursive As Boolean) As List(Of TsCuUtilisateurAD)


    ''' --------------------------------------------------------------------------------
    ''' <summary>
    '''     Cette méthode retourne la liste des membres du groupe passé en paramètre.
    ''' </summary>
    ''' <param name="strGroupe">
    ''' 	Représente le groupe pour lequel on désire avoir la liste des membres. 
    ''' 	Value Type: <see cref="String" />	(System.String)
    ''' </param>
    ''' <returns>Une DataTable avec les utilisateur membre du groupe. Les noms de 
    ''' colonne du DataTable sont les nom des attributs de l'AD </returns>
    ''' <remarks>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    Function RechercheGroupeAD(ByVal strGroupe As String, ByVal blnRechRecursive As Boolean) As DataTable

    ''' --------------------------------------------------------------------------------
    ''' <summary>
    '''     Fonction récursive de recherche de groupe.  Il recherche dans toute la
    '''     cascade de groupes que l'utilisateur peut avoir.
    ''' </summary>
    ''' <param name="strGroupeRecherche">
    ''' 	Nom du groupe que l'on recherche. 
    ''' 	Value Type: <see cref="String" />	(System.String)
    ''' </param>
    ''' <param name="strACID">
    ''' 	Nom du groupe dans lequel on recherche présentement le groupe désiré. 
    ''' 	Value Type: <see cref="String" />	(System.String)
    ''' </param>
    ''' <returns><see cref="Boolean" />	(System.Boolean)</returns>
    ''' <remarks><para><pre>
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------

    Function ChercheDansGroupes(ByVal strACID As String, ByVal strGroupeRecherche As String) As Boolean


    ''' --------------------------------------------------------------------------------
    ''' <summary>
    '''     Effectue une recherche dans l'AD en utilisant les paramètres reçus.
    ''' </summary>
    ''' <param name="pTypeRequete">
    ''' 	Indicateur du champs sur lequel le recherche doit être effectuée. 
    ''' 	Value Type: <see cref="Securite.tsCuObtnrInfoAD.TsIadTypeRequete" />	(Rrq.Securite.tsCuObtnrInfoAD.TsIadTypeRequete)
    ''' </param>
    ''' <param name="strCritereRecherche">
    ''' 	Valeur recherchée, peut être une valeur spécifique ou un masque de recherche (ex: T20*). 
    ''' 	Value Type: <see cref="String" />	(System.String)
    ''' </param>
    ''' <param name="strCritereRechercheSecondaire">
    ''' 	Le critère de recherche secondaire à l'active directory utilisé lors d'une recherche nom et prénom. 
    '''     Value Type: <see cref="String" />	(System.String)
    ''' </param>
    ''' <returns><see cref="Data.DataTable" />	(System.Data.DataTable)</returns>
    ''' <remarks><para><pre>
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    Function RechercheActiveDirectory(ByVal pTypeRequete As TsIadTypeRequete, ByVal strCritereRecherche As String, _
                                              Optional ByVal strCritereRechercheSecondaire As String = "", _
                                              Optional ByVal pObjectCategory As TsIadObjectCategory = TsIadObjectCategory.TsIadOcTous) As DataTable



    Function ObtenirMembresGroupe(ByVal NomGroupe As String) As String()


    ''' <summary>
    '''  L'implementation de cette propriété doit retourner le nom du serveur AD à être accédé.
    ''' </summary>
    ''' <returns></returns>
    ReadOnly Property NomServeur() As String


    ''' <summary>
    '''  L'implementation de cette doit remplir la correspondance entre
    ''' les type de requête et les attributs de l'AD.
    ''' </summary>
    ''' <returns></returns>
    Sub CombinerTypeRequeteVSAttributAD()


End Interface
