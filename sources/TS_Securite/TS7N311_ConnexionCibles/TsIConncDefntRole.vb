''' <summary>
''' Interface pour un connecteur permettant de gérer les rôles sur un système cible.
''' </summary>
Public Interface TsIConncMajDefntRole
    Inherits IDisposable

    ' Méthodes qui permettent d'obtenir l'état d'un système cible (dans une interface de lecture...)
    ' ObtenirRoles
    ' ObtenirLiensRoleRole
    ' ObtenirLiensRoleRessr
    ' ObtenirDiffAttrbRole



    ' Méthodes qui permettent de faire des changements dans un système cible

    ''' <summary>
    ''' Effectue des créations de rôles dans le système cible.
    ''' </summary>
    ''' <param name="ajouts">Rôles à créer.</param>
    ''' <param name="contnErreur">Indique si le traitement peut continuer en cas d'erreur.</param>
    ''' <returns>True si tous les changements demandés ont réussi, False autrement.</returns>
    ''' <remarks>
    ''' <para>
    ''' Cette méthode ne permet que de créer des rôles, elle ne permet pas
    ''' de les associer à des utilisateurs ni à des ressrouces.
    ''' Si on avait d'autres systèmes cibles que l'AD qui supportent la notion de rôle, cette méthode
    ''' pourrait également associer le rôle dans le système cible au groupe AD correspondant.
    ''' </para>
    ''' <para>
    ''' Si le paramètre <paramref name="contnErreur"/> a la valeur <c>True</c>, le connecteur
    ''' est libre d'ignorer les erreurs qui pourraient se produire lors du traitement d'un rôle.
    ''' Demander d'ajouter des rôles déjà existants 
    ''' ne doit pas être considéré comme une erreur par le connecteur.
    ''' Si le connecteur reçoit un élément pour lequel la propriété <see cref="TsCdConnxRole.CibleAJour" />
    ''' a une valeur supérieure ou égale à <see cref="TsECcCibleAJour.ExtractionAJour"/>,
    ''' il n'a pas à mettre à jour la cible et ça ne l'empêche pas de retourner <c>True</c>.
    ''' Lorsqu'un connecteur traite un élément correctement, il doit les marquer en affectant à
    ''' la propriété <see cref="TsCdConnxRole.CibleAJour" /> la valeur
    ''' <see cref="TsECcCibleAJour.MajDemande"/> ou <see cref="TsECcCibleAJour.AJour"/>.
    ''' </para>
    ''' </remarks>
    Function CreerRoles(ByVal ajouts As IEnumerable(Of TsCdConnxRole), Optional ByVal contnErreur As Boolean = False) As Boolean

    ''' <summary>
    ''' Effectue des destructions de rôle dans le système cible.
    ''' </summary>
    ''' <param name="suppr">Rôles à détruire.</param>
    ''' <param name="contnErreur">Indique si le traitement peut continuer en cas d'erreur.</param>
    ''' <returns>True si tous les changements demandés ont réussi, False autrement.</returns>
    ''' <remarks>
    ''' <para>
    ''' Cette méthode ne permet que d'effacer des rôles, elle ne permet pas
    ''' de les disssocier des utilisateurs ou des ressrouces.
    ''' Si on avait d'autres systèmes cibles que l'AD qui supportent la notion de rôle, cette méthode
    ''' pourrait également associer le rôle dans le système cible au groupe AD correspondant.
    ''' </para>
    ''' <para>
    ''' Si le paramètre <paramref name="contnErreur"/> a la valeur <c>True</c>, le connecteur
    ''' est libre d'ignorer les erreurs qui pourraient se produire lors du traitement d'un rôle.
    ''' Demander d'enlever des rôles inexistants
    ''' ne doit pas être considéré comme une erreur par le connecteur.
    ''' Si le connecteur reçoit un élément pour lequel la propriété <see cref="TsCdConnxRole.CibleAJour" />
    ''' a une valeur supérieure ou égale à <see cref="TsECcCibleAJour.ExtractionAJour"/>,
    ''' il n'a pas à mettre à jour la cible et ça ne l'empêche pas de retourner <c>True</c>.
    ''' Lorsqu'un connecteur traite un élément correctement, il doit les marquer en affectant à
    ''' la propriété <see cref="TsCdConnxRole.CibleAJour" /> la valeur
    ''' <see cref="TsECcCibleAJour.MajDemande"/> ou <see cref="TsECcCibleAJour.AJour"/>.
    ''' </para>
    ''' </remarks>
    Function DetruireRoles(ByVal suppr As IEnumerable(Of TsCdConnxRole), Optional ByVal contnErreur As Boolean = False) As Boolean

    ''' <summary>
    ''' Effectue des modifications des attributs des rôles dans le système cible.
    ''' </summary>
    ''' <param name="ajouts">Les attributs des rôles à ajouter.</param>
    ''' <param name="suppr">Les attributs des rôles à effacer.</param>
    ''' <param name="contnErreur">Indique si le traitement peut continuer en cas d'erreur.</param>
    ''' <returns>True si tous les changements demandés ont réussi, False autrement.</returns>
    ''' <remarks>
    ''' <para>
    ''' Si le paramètre <paramref name="contnErreur"/> a la valeur <c>True</c>, le connecteur
    ''' est libre d'ignorer les erreurs qui pourraient se produire lors du traitement d'un attribut.
    ''' Si le connecteur reçoit un élément pour lequel la propriété <see cref="TsCdConnxRoleRole.CibleAJour" />
    ''' a une valeur supérieure ou égale à <see cref="TsECcCibleAJour.ExtractionAJour"/>,
    ''' il n'a pas à mettre à jour la cible et ça ne l'empêche pas de retourner <c>True</c>.
    ''' Lorsqu'un connecteur traite un élément correctement, il doit les marquer en affectant à
    ''' la propriété <see cref="TsCdConnxRoleRole.CibleAJour" /> la valeur
    ''' <see cref="TsECcCibleAJour.MajDemande"/> ou <see cref="TsECcCibleAJour.AJour"/>.
    ''' </para>
    ''' </remarks>
    Function AppliquerAttrbRoles(ByVal ajouts As IEnumerable(Of TsCdConnxRoleAttrb), ByVal suppr As IEnumerable(Of TsCdConnxRoleAttrb), Optional ByVal contnErreur As Boolean = False) As Boolean

    ''' <summary>
    ''' Effectue des créations et des destructions de liens d'héritage entre les rôles dans le système cible.
    ''' </summary>
    ''' <param name="ajouts">Liens d'héritage à créer.</param>
    ''' <param name="suppr">Liens d'héritage à détruire.</param>
    ''' <param name="contnErreur">Indique si le traitement peut continuer en cas d'erreur.</param>
    ''' <returns>True si tous les changements demandés ont réussi, False autrement.</returns>
    ''' <remarks>
    ''' <para>
    ''' Si le paramètre <paramref name="contnErreur"/> a la valeur <c>True</c>, le connecteur
    ''' est libre d'ignorer les erreurs qui pourraient se produire lors du traitement d'un lien.
    ''' Demander d'ajouter des liens déjà existants ou d'enlever des liens inexistants
    ''' ne doit pas être considéré comme une erreur par le connecteur.
    ''' Si le connecteur reçoit un élément pour lequel la propriété <see cref="TsCdConnxRoleRole.CibleAJour" />
    ''' a une valeur supérieure ou égale à <see cref="TsECcCibleAJour.ExtractionAJour"/>,
    ''' il n'a pas à mettre à jour la cible et ça ne l'empêche pas de retourner <c>True</c>.
    ''' Lorsqu'un connecteur traite un élément correctement, il doit les marquer en affectant à
    ''' la propriété <see cref="TsCdConnxRoleRole.CibleAJour" /> la valeur
    ''' <see cref="TsECcCibleAJour.MajDemande"/> ou <see cref="TsECcCibleAJour.AJour"/>.
    ''' </para>
    ''' </remarks>
    Function AppliquerLiensRoleRole(ByVal ajouts As IEnumerable(Of TsCdConnxRoleRole), ByVal suppr As IEnumerable(Of TsCdConnxRoleRole), Optional ByVal contnErreur As Boolean = False) As Boolean

    ''' <summary>
    ''' Effectue des créations et des destructions de liens entre les rôles et les ressources dans le système cible.
    ''' </summary>
    ''' <param name="ajouts">Liens rôle-ressource à créer.</param>
    ''' <param name="suppr">Liens rôle-ressource à détruire.</param>
    ''' <param name="contnErreur">Indique si le traitement peut continuer en cas d'erreur.</param>
    ''' <returns>True si tous les changements demandés ont réussi, False autrement.</returns>
    ''' <remarks>
    ''' <para>
    ''' Si le paramètre <paramref name="contnErreur"/> a la valeur <c>True</c>, le connecteur
    ''' est libre d'ignorer les erreurs qui pourraient se produire lors du traitement d'un lien.
    ''' Demander d'ajouter des liens déjà existants ou d'enlever des liens inexistants
    ''' ne doit pas être considéré comme une erreur par le connecteur.
    ''' Si le connecteur reçoit un élément pour lequel la propriété <see cref="TsCdConnxRoleRessr.CibleAJour" />
    ''' a une valeur supérieure ou égale à <see cref="TsECcCibleAJour.ExtractionAJour"/>,
    ''' il n'a pas à mettre à jour la cible et ça ne l'empêche pas de retourner <c>True</c>.
    ''' Lorsqu'un connecteur traite un élément correctement, il doit les marquer en affectant à
    ''' la propriété <see cref="TsCdConnxRoleRessr.CibleAJour" /> la valeur
    ''' <see cref="TsECcCibleAJour.MajDemande"/> ou <see cref="TsECcCibleAJour.AJour"/>.
    ''' </para>
    ''' </remarks>
    Function AppliquerLiensRoleRessr(ByVal ajouts As IEnumerable(Of TsCdConnxRoleRessr), ByVal suppr As IEnumerable(Of TsCdConnxRoleRessr), Optional ByVal contnErreur As Boolean = False) As Boolean


    ' Méthodes qui permettent de vérifier si des changements sont à faire dans le système cible


    ''' <summary>
    ''' Vérifie dans le système cible les rôles qui sont déjà existants ou inexistants.
    ''' </summary>
    ''' <param name="ajouts">Roles dont l'existance est à vérifier dans le système cible.</param>
    ''' <param name="suppr">Roles dont l'inexistance est à vérifier dans le système cible.</param>
    ''' <remarks>
    ''' Cette méthode doit vérifier l'état du système cible et
    ''' marquer les rôles déjà existants du paramètre <paramref name="ajouts" />
    ''' ainsi que les rôles déjà inexistants du paramètre <paramref name="suppr" />
    ''' en affectant à <see cref="TsCdConnxRole.CibleAJour"/> une des valeurs
    ''' <see cref="TsECcCibleAJour.ExtractionPasAJour" />, <see cref="TsECcCibleAJour.ExtractionAJour" />,
    ''' <see cref="TsECcCibleAJour.PasAJour" /> ou <see cref="TsECcCibleAJour.AJour" /> selon le cas.
    ''' </remarks>
    Sub VerifierRoles(ByVal ajouts As IEnumerable(Of TsCdConnxRole), ByVal suppr As IEnumerable(Of TsCdConnxRole))

    ''' <summary>
    ''' Vérifie dans le système cible les attributs des rôles qui sont déjà existants ou inexistants.
    ''' </summary>
    ''' <param name="ajouts">Rôles dont l'existance est à vérifier dans le système cible.</param>
    ''' <param name="suppr">Rôles dont l'inexistance est à vérifier dans le système cible.</param>
    ''' <remarks>
    ''' Cette méthode doit vérifier l'état du système cible et
    ''' marquer les attributs des rôles déjà existants du paramètre <paramref name="ajouts" />
    ''' ainsi que les attributs rôles déjà inexistants du paramètre <paramref name="suppr" />
    ''' en affectant à <see cref="TsCdConnxRole.CibleAJour"/> une des valeurs
    ''' <see cref="TsECcCibleAJour.ExtractionPasAJour" />, <see cref="TsECcCibleAJour.ExtractionAJour" />,
    ''' <see cref="TsECcCibleAJour.PasAJour" /> ou <see cref="TsECcCibleAJour.AJour" /> selon le cas.
    ''' </remarks>
    Sub VerifierAttrbRoles(ByVal ajouts As IEnumerable(Of TsCdConnxRoleAttrb), ByVal suppr As IEnumerable(Of TsCdConnxRoleAttrb))

    ''' <summary>
    ''' Vérifie dans le système cible les liens d'héritage entre les rôles qui sont déjà existants ou inexistants.
    ''' </summary>
    ''' <param name="ajouts">Liens dont l'existance est à vérifier dans le système cible.</param>
    ''' <param name="suppr">Liens dont l'inexistance est à vérifier dans le système cible.</param>
    ''' <remarks>
    ''' Cette méthode doit vérifier l'état du système cible et
    ''' marquer les liens déjà existants du paramètre <paramref name="ajouts" />
    ''' ainsi que les liens déjà inexistants du paramètre <paramref name="suppr" />
    ''' en affectant à <see cref="TsCdConnxRoleRole.CibleAJour"/> une des valeurs
    ''' <see cref="TsECcCibleAJour.ExtractionPasAJour" />, <see cref="TsECcCibleAJour.ExtractionAJour" />,
    ''' <see cref="TsECcCibleAJour.PasAJour" /> ou <see cref="TsECcCibleAJour.AJour" /> selon le cas.
    ''' </remarks>
    Sub VerifierLiensRoleRole(ByVal ajouts As IEnumerable(Of TsCdConnxRoleRole), ByVal suppr As IEnumerable(Of TsCdConnxRoleRole))

    ''' <summary>
    ''' Vérifie dans le système cible les liens entre les rôles et les ressources qui sont déjà existants ou inexistants.
    ''' </summary>
    ''' <param name="ajouts">Liens dont l'existance est à vérifier dans le système cible.</param>
    ''' <param name="suppr">Liens dont l'inexistance est à vérifier dans le système cible.</param>
    ''' <remarks>
    ''' Cette méthode doit vérifier l'état du système cible et
    ''' marquer les liens déjà existants du paramètre <paramref name="ajouts" />
    ''' ainsi que les liens déjà inexistants du paramètre <paramref name="suppr" />
    ''' en affectant à <see cref="TsCdConnxRoleRessr.CibleAJour"/> une des valeurs
    ''' <see cref="TsECcCibleAJour.ExtractionPasAJour" />, <see cref="TsECcCibleAJour.ExtractionAJour" />,
    ''' <see cref="TsECcCibleAJour.PasAJour" /> ou <see cref="TsECcCibleAJour.AJour" /> selon le cas.
    ''' </remarks>
    Sub VerifierLiensRoleRessr(ByVal ajouts As IEnumerable(Of TsCdConnxRoleRessr), ByVal suppr As IEnumerable(Of TsCdConnxRoleRessr))

End Interface
