Imports System.Collections.Generic
Imports TS6N631_ZpTrtParmChif.TsCuParamsChiffrement

Public Interface IGestFichierChif

    Property Environnement As TsCuParamsChiffrement.Envrn
    Property ListeDependances As List(Of IGestFichierChif)

    ''' <summary>
    ''' Obtenir le nom du fichier selon le type de fichier recherché et l'environnement courant
    ''' </summary>
    ''' <param name="typeFichier"></param>
    ''' <returns></returns>
    Function ObtenirNomFichierServeurInterne(typeFichier As TypeFichier) As String

    ''' <summary>
    ''' Obtenir le nom du fichier selon le type de fichier recherché et l'environnement courant
    ''' </summary>
    ''' <param name="typeFichier"></param>
    ''' <returns></returns>
    Function ObtenirNomFichierServeurExtranet(typeFichier As TypeFichier) As String

    ''' <summary>
    ''' Obtenir le nom du fichier selon le type de fichier recherché et l'environnement courant
    ''' </summary>
    ''' <param name="typeFichier"></param>
    ''' <returns></returns>
    Function ObtenirNomFichierServeurInforoute(typeFichier As TypeFichier) As String

    ''' <summary>
    ''' Permet d'ajouter une dépendance entre deux environnments, par exemple entre Unit et Intg
    ''' Quand on met à jour Unit, il faut aussi faire la modif en Intg, et dans tous les autres environnements dépendants.
    ''' </summary>
    ''' <param name="dependance"></param>
    Sub AjouterDependance(dependance As IGestFichierChif)

    ''' <summary>
    ''' Lire le fichier source selon les paramètres saisis dans la fenêtre principale
    ''' </summary>
    ''' <returns></returns>
    Function ObtenirSource(type As TypeCle, typeFichier As TypeFichier) As DataRow()

    ''' <summary>
    ''' Ajouter une entrée dans le fichier courant
    ''' </summary>
    ''' <param name="recordAAjouter"></param>
    ''' <returns></returns>
    Function Ajouter(ByVal recordAAjouter As DataRow, type As TypeCle, typeFichier As TypeFichier) As Boolean

    ''' <summary>
    ''' Copie d'une clé d'un environnement à un autre.
    ''' </summary>
    ''' <param name="recordAAjouter"></param>
    ''' <returns></returns>
    Function Copier(ByVal recordAAjouter As DataRow, type As TypeCle, typeFichier As TypeFichier) As Boolean

    ''' <summary>
    ''' Obtenir une clé vide selon bon format de fichier courant
    ''' </summary>
    ''' <returns></returns>
    Function ObtenirNouvlCleVecteur(typeFichier As TypeFichier) As DataRow

    ''' <summary>
    ''' Écriture des modifications apportées sur disque.
    ''' </summary>
    ''' <param name="majInterneCommun"></param>
    ''' <param name="majExterneCommun"></param>
    ''' <returns></returns>
    Function MettreAJourSource(majInterneCommun As Boolean, majExterneCommun As Boolean, typeFichier As TypeFichier) As Boolean

    ''' <summary>
    ''' Valide si un code équivalent à celui passé en paramètre existe déjà
    ''' </summary>
    ''' <param name="code"></param>
    ''' <param name="typeCle"></param>
    ''' <param name="typeFichier"></param>
    ''' <returns></returns>
    Function CodeExiste(code As String, typeCle As TypeCle, typeFichier As TypeFichier) As Boolean

End Interface
