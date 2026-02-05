''' <summary>
''' Fournit un mécanisme pour interagir avec le dépôt de sécurité applicative ADLDS.
''' </summary>
Public Interface IDepotSecurite

    ''' <summary>
    ''' Ajoute un utilisateur ou un groupe en tant que membre d'un groupe.
    ''' </summary>
    ''' <param name="nomGroupe">Le 'sAMAccountName' du groupe à modifier.</param>
    ''' <param name="nomMembre">Le 'sAMAccountName' de l'utilisateur ou du groupe à ajouter en tant que membre.</param>
    ''' <exception cref="ApplicationException">Lorsque l'on essais d'ajouter un membre sur lui même.</exception>
    ''' <exception cref="TsExcMembreInexistant">Lorsqu'un utilisateur ou un groupe à ajouter en tant que membre n'est pas présent.</exception>
    ''' <exception cref="TsExcGroupeInexistant">Lorsqu'un groupe à modifier n'existe pas.</exception>
    ''' <exception cref="TsExcObjetDejaExistantADLDS">Lorque le membre existe déja dans le groupe.</exception>
    ''' <exception cref="TsExcLienDejaExistantADLDS">Lorsque le liens est existe déjà.</exception>
    ''' <exception cref="TsExcServeurRefuseOperation">Lorsque l'association entre le membre et le groupe est refusé par le serveur.</exception>
    ''' <remarks>
    ''' L'équivalent du 'sAMAccountName' de l'AD pour les groupes dans ADLDS est 'name'.
    ''' L'équivalent du 'sAMAccountName' de l'AD pour les utilisateurs dans ADLDS est 'userPrincipalName'.
    ''' </remarks>
    Sub AjouterMembreGroupe(ByVal nomGroupe As String, ByVal nomMembre As String)

    ''' <summary>
    ''' Crée un nouveau groupe de sécurité préventive de type ROA dans ADLDS.
    ''' </summary>
    ''' <param name="nomGroupe">Le 'sAMAccountName' du groupe à créer.</param>
    ''' <param name="description">La description du groupe à créer.</param>
    ''' <exception cref="ArgumentException">Lorsque le nom du groupe n'est pas un ROA.</exception>
    ''' <exception cref="TsExcObjetDejaExistantADLDS">Lorsque le groupe existe déjà.</exception>
    ''' <remarks>
    ''' L'équivalent du 'sAMAccountName' de l'AD pour les groupes dans ADLDS est 'name'.
    ''' </remarks>
    Sub CreerGroupeSecuriteApplicativeROA(nomGroupe As String, description As String)

    ''' <summary>
    ''' Crée un nouveau groupe de sécurité préventive de type ROG dans ADLDS.
    ''' </summary>
    ''' <param name="nomGroupe">Le 'sAMAccountName' du groupe à créer.</param>
    ''' <param name="description">La description du groupe à créer.</param>
    ''' <exception cref="ArgumentException">Lorsque le nom du groupe n'est pas un ROG.</exception>
    ''' <exception cref="TsExcObjetDejaExistantADLDS">Lorsque le groupe existe déjà.</exception>
    ''' <remarks>
    ''' L'équivalent du 'sAMAccountName' de l'AD pour les groupes dans ADLDS est 'name'.
    ''' </remarks>
    Sub CreerGroupeSecuriteApplicativeROG(nomGroupe As String, description As String)

    ''' <summary>
    ''' Modifie la description d'un groupe.
    ''' </summary>
    ''' <param name="nomGroupe">Le 'sAMAccountName' du groupe à modifier.</param>
    ''' <param name="description">La nouvelle description.</param>
    ''' <exception cref="TsExcGroupeInexistant">Lorsque le groupe n'existe pas.</exception>
    ''' <remarks>
    ''' L'équivalent du 'sAMAccountName' de l'AD pour les groupes dans ADLDS est 'name'.
    ''' </remarks>
    Sub ModifierDescriptionGroupe(ByVal nomGroupe As String, ByVal description As String)

    ''' <summary>
    ''' Obtient la description d'un groupe.
    ''' </summary>
    ''' <param name="nomGroupe">Le 'sAMAccountName' du groupe.</param>
    ''' <returns>La description du groupe spécifié.</returns>
    ''' <exception cref="TsExcGroupeInexistant">Lorsque le groupe n'existe pas.</exception>
    ''' <remarks>
    ''' L'équivalent du 'sAMAccountName' de l'AD pour les groupes dans ADLDS est 'name'.
    ''' </remarks>
    Function ObtenirDescriptionGroupe(ByVal nomGroupe As String) As String

End Interface