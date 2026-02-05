''' <summary>
''' Interface pour une source de différence.
''' </summary>
''' <remarks></remarks>
Public Interface TsISourceDiff

    ''' <summary>
    ''' Effectue une différence entre les liens direct utilisateur/ressource à partir des informations tirées de la source de différence.
    ''' </summary>
    ''' <param name="cible">Il faut distinguer l'origine des ressources pour ne traiter que ceux là.</param>
    ''' <param name="lstAjouter">Liste de retour. Les ajouts de la différence seront ajouté à cette liste.</param>
    ''' <param name="lstSupprimer">Liste de retour. Les suppressions de la différence seront ajouté à cette liste.</param>
    ''' <remarks></remarks>
    Sub ObtnrDiffrUserRessrDirect(ByVal cible As String, _
        ByRef lstAjouter As List(Of TsCdConnxUserRessr), ByRef lstSupprimer As List(Of TsCdConnxUserRessr))

    ''' <summary>
    ''' Effectue une différence entre les liens direct utilisateur/ressource  ou les liens indirect par l'intermédiare d'un ou plusieurs rôles.
    ''' Les informations sont tirées de la source de différence.
    ''' </summary>
    ''' <param name="cible">Il faut distinguer l'origine des ressources pour ne traiter que ceux là.</param>
    ''' <param name="lstAjouter">Liste de retour. Les ajouts de la différence seront ajouté à cette liste.</param>
    ''' <param name="lstSupprimer">Liste de retour. Les suppressions de la différence seront ajouté à cette liste.</param>
    ''' <remarks>
    ''' <para>
    ''' Cette méthode renvois les listes des différences entre les liens utilisateur/ressource et
    ''' liens utilisateur/rôle/.../rôle/ressources. 
    ''' </para>
    ''' <para>
    ''' Cette méthode est prévu pour les système cibble qui ne supporte pas le concept de rôles.
    ''' </para>
    ''' </remarks>
    Sub ObtnrDiffrUserRessrRecurcif(ByVal cible As String, _
        ByRef lstAjouter As List(Of TsCdConnxUserRessr), ByRef lstSupprimer As List(Of TsCdConnxUserRessr))

    ''' <summary>
    ''' Effectue une différence entre les liens utilisateur/rôle à partir des informations tirées de la source de différence.
    ''' </summary>
    ''' <param name="lstAjouter">Liste de retour. Les ajouts de la différence seront ajouté à cette liste.</param>
    ''' <param name="lstSupprimer">Liste de retour. Les suppressions de la différence seront ajouté à cette liste.</param>
    ''' <remarks></remarks>
    Sub ObtnrDiffrUserRole( _
        ByRef lstAjouter As List(Of TsCdConnxUserRole), ByRef lstSupprimer As List(Of TsCdConnxUserRole))

    ''' <summary>
    ''' Effectue une différence entre les liens rôle/rôle à partir des informations tirées de la source de différence.
    ''' </summary>
    ''' <param name="lstAjouter">Liste de retour. Les ajouts de la différence seront ajouté à cette liste.</param>
    ''' <param name="lstSupprimer">Liste de retour. Les suppressions de la différence seront ajouté à cette liste.</param>
    ''' <remarks></remarks>
    Sub ObtnrDiffrRoleRole( _
        ByRef lstAjouter As List(Of TsCdConnxRoleRole), ByRef lstSupprimer As List(Of TsCdConnxRoleRole))

    ''' <summary>
    ''' Effectue une différence entre les liens rôle/ressource à partir des informations tirées de la source de différence.
    ''' </summary>
    ''' <param name="cible">Il faut distinguer l'origine des ressources pour ne traiter que ceux là.</param>
    ''' <param name="lstAjouter">Liste de retour. Les ajouts de la différence seront ajouté à cette liste.</param>
    ''' <param name="lstSupprimer">Liste de retour. Les suppressions de la différence seront ajouté à cette liste.</param>
    ''' <remarks></remarks>
    Sub ObtnrDiffrRoleRessr(ByVal cible As String, _
        ByRef lstAjouter As List(Of TsCdConnxRoleRessr), ByRef lstSupprimer As List(Of TsCdConnxRoleRessr))

    ''' <summary>
    ''' Effectue une différence entre les utilisateurs à partir des informations tirées de la source de différence.
    ''' </summary>
    ''' <param name="lstAjouter">Liste de retour. Les ajouts de la différence seront ajouté à cette liste.</param>
    ''' <param name="lstSupprimer">Liste de retour. Les suppressions de la différence seront ajouté à cette liste.</param>
    ''' <remarks></remarks>
    Sub ObtnrDiffrUser( _
        ByRef lstAjouter As List(Of TsCdConnxUser), ByRef lstSupprimer As List(Of TsCdConnxUser))

    ''' <summary>
    ''' Effectue une différence entre les rôles à partir des informations tirées de la source de différence.
    ''' </summary>
    ''' <param name="lstAjouter">Liste de retour. Les ajouts de la différence seront ajouté à cette liste.</param>
    ''' <param name="lstSupprimer">Liste de retour. Les suppressions de la différence seront ajouté à cette liste.</param>
    ''' <remarks></remarks>
    Sub ObtnrDiffrRole( _
        ByRef lstAjouter As List(Of TsCdConnxRole), ByRef lstSupprimer As List(Of TsCdConnxRole))

    ''' <summary>
    ''' Effectue une différence entre les ressources à partir des informations tirées de la source de différence.
    ''' </summary>
    ''' <param name="cible">Il faut distinguer l'origine des ressources pour ne traiter que ceux là.</param>
    ''' <param name="lstAjouter">Liste de retour. Les ajouts de la différence seront ajouté à cette liste.</param>
    ''' <param name="lstSupprimer">Liste de retour. Les suppressions de la différence seront ajouté à cette liste.</param>
    ''' <remarks></remarks>
    Sub ObtnrDiffrRessource(ByVal cible As String, _
        ByRef lstAjouter As List(Of TsCdConnxRessr), ByRef lstSupprimer As List(Of TsCdConnxRessr))

    ''' <summary>
    ''' Effectue une différence entre les attributs des utilisateurs à partir des informations tirées de la source de différence.
    ''' </summary>
    ''' <param name="lstAjouter">Liste de retour. Les ajouts de la différence seront ajouté à cette liste.</param>
    ''' <param name="lstSupprimer">Liste de retour. Les suppressions de la différence seront ajouté à cette liste.</param>
    ''' <remarks></remarks>
    Sub ObtnrDiffrAttrbUser( _
        ByRef lstAjouter As List(Of TsCdConnxUserAttrb), ByRef lstSupprimer As List(Of TsCdConnxUserAttrb))

    ''' <summary>
    ''' Effectue une différence entre les attributs des rôles à partir des informations tirées de la source de différence.
    ''' </summary>
    ''' <param name="lstAjouter">Liste de retour. Les ajouts de la différence seront ajouté à cette liste.</param>
    ''' <param name="lstSupprimer">Liste de retour. Les suppressions de la différence seront ajouté à cette liste.</param>
    ''' <remarks></remarks>
    Sub ObtnrDiffrAttrbRole( _
        ByRef lstAjouter As List(Of TsCdConnxRoleAttrb), ByRef lstSupprimer As List(Of TsCdConnxRoleAttrb))

    ''' <summary>
    ''' Effectue une différence entre les attributs des ressources à partir des informations tirées de la source de différence.
    ''' </summary>
    ''' <param name="cible">Il faut distinguer l'origine des ressources pour ne traiter que ceux là.</param>
    ''' <param name="lstAjouter">Liste de retour. Les ajouts de la différence seront ajouté à cette liste.</param>
    ''' <param name="lstSupprimer">Liste de retour. Les suppressions de la différence seront ajouté à cette liste.</param>
    ''' <remarks></remarks>
    Sub ObtnrDiffrAttrbRessr(ByVal cible As String, _
        ByRef lstAjouter As List(Of TsCdConnxRessrAttrb), ByRef lstSupprimer As List(Of TsCdConnxRessrAttrb))

End Interface
