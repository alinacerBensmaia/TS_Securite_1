Public Interface TsIObtnrCompteGenerique

    ''' <summary>
    ''' Obtenir le code accès et le mot de passe de la clé symbolique voulue.
    ''' La clé symbolique est recherchée dans le champ 'Cle'.   Depuis la normalisation
    ''' des clés symboliques, on doit utiliser cette méthode.
    ''' </summary>
    ''' <param name="strCle">
    ''' 	Nom de la clé symbolique.
    ''' 	Value Type: string
    ''' </param>
    ''' <param name="strRaison">
    ''' 	La raison de la demande.  Elle est obligatoire.  On la retrouve dans l'EventLog
    ''' 	Value Type: string
    ''' </param>
    ''' <param name="strCompte">
    ''' 	Le code d'accès correspondant à la clé symbolique demandée.  Si l'usager a les 
    '''     droits nécessaires, le code d'accès y est inscrit.
    ''' 	Value Type: string
    ''' </param>
    ''' <param name="strMDP">
    ''' 	Le mot de passe du code d'accès correspondant à la clé symbolique demandée.  
    '''     Si l'usager a les droits nécessaires, le mot de passe y est inscrit.
    ''' 	Value Type: string
    ''' </param> 
    Sub ObtenirCodeAccesMotDePasse(ByVal strCle As String, ByVal strRaison As String, ByRef strCompte As String, ByRef strMDP As String)



    ''' <summary>
    ''' Obtenir le code accès et le mot de passe de la clé symbolique voulue.
    ''' La clé symbolique est recherchée dans le champ 'Cle'.   Depuis la normalisation
    ''' des clés symboliques, on doit utiliser cette méthode.
    ''' </summary>
    ''' <param name="strCle">
    ''' 	Nom de la clé symbolique.
    ''' 	Value Type: string
    ''' </param>
    ''' <param name="strRaison">
    ''' 	La raison de la demande.  Elle est obligatoire.  On la retrouve dans l'EventLog
    ''' 	Value Type: string
    ''' </param>
    ''' <param name="strCompte">
    ''' 	Le code d'accès correspondant à la clé symbolique demandée.  Si l'usager a les 
    '''     droits nécessaires, le code d'accès y est inscrit.
    ''' 	Value Type: string
    ''' </param>
    ''' <param name="strMDP">
    ''' 	Le mot de passe du code d'accès correspondant à la clé symbolique demandée.  
    '''     Si l'usager a les droits nécessaires, le mot de passe y est inscrit.
    ''' 	Value Type: string
    ''' </param> 
    Sub ObtenirCodeAccesMotDePasseLibraire(ByVal strCle As String, ByVal strRaison As String, ByRef strCompte As String, ByRef strMDP As String)



End Interface




''' <summary>
''' Identifie les Ressources de securite.
''' </summary>
<Flags()>
Public Enum TsRessourceCleSymbolique


    ''' <summary>
    ''' Fichier sur le réseau
    ''' </summary>
    FICHIER

    ''' <summary>
    ''' CIWCF - Appelle un composant d'intégration de la Logique d'affaire.
    ''' </summary>
    CIWCF

    ''' <summary>
    ''' CSWCF - Appelle un composant de service.
    ''' </summary>
    CSWCF


End Enum



