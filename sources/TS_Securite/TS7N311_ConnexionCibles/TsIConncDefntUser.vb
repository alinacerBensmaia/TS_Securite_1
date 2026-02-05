''' <summary>
''' Interface pour les connecteurs permettant de gérer les utilisateurs sur un système cible.
''' </summary>
Public Interface TsIConncMajDefntUser
    Inherits IDisposable

    ' Méthodes qui permettent d'obtenir l'état d'un système cible (dans une interface de lecture...)
    ' ObtenirUtilisateurs
    ' ObtenirLiensUserRole
    ' ObtenirLiensUserRessr
    ' ObtenirAttrbUser
    ' ObtenirDiffAttrbUser


    ' Méthodes à ajouter pour les attributs...
    ' AjouterAttrbUser
    ' EnleverAttrbUser
    ' RemplacerAttrbUser

    ' Méthodes qui permettent de faire des changements dans un système cible

    ''' <summary>
    ''' Effectue des créations d'utilisateur dans le système cible.
    ''' </summary>
    ''' <param name="ajouts">Utilisateurs à créer.</param>
    ''' <param name="contnErreur">Indique si le traitement peut continuer en cas d'erreur.</param>
    ''' <returns>True si tous les changements demandés ont réussi, False autrement.</returns>
    ''' <remarks>
    ''' <para>
    ''' Cette méthode ne permet que de créer des utilisateurs, elle ne permet pas
    ''' de les associer à des rôles ni à des ressrouces.
    ''' </para>
    ''' <para>
    ''' Si le paramètre <paramref name="contnErreur"/> a la valeur <c>True</c>, le connecteur
    ''' est libre d'ignorer les erreurs qui pourraient se produire lors du traitement d'un utilisateur.
    ''' Demander d'ajouter des utilisateurs déjà existants 
    ''' ne doit pas être considéré comme une erreur par le connecteur.
    ''' Si le connecteur reçoit un élément pour lequel la propriété <see cref="TsCdConnxRole.CibleAJour" />
    ''' a une valeur supérieure ou égale à <see cref="TsECcCibleAJour.ExtractionAJour"/>,
    ''' il n'a pas à mettre à jour la cible et ça ne l'empêche pas de retourner <c>True</c>.
    ''' Lorsqu'un connecteur traite un élément correctement, il doit les marquer en affectant à
    ''' la propriété <see cref="TsCdConnxRole.CibleAJour" /> la valeur
    ''' <see cref="TsECcCibleAJour.MajDemande"/> ou <see cref="TsECcCibleAJour.AJour"/>.
    ''' </para>
    ''' </remarks>
    Function CreerUsers(ByVal ajouts As IEnumerable(Of TsCdConnxUser), Optional ByVal contnErreur As Boolean = False) As Boolean

    ''' <summary>
    ''' Effectue des destructions d'utilisateur dans le système cible.
    ''' </summary>
    ''' <param name="suppr">Utilisateurs à détruire.</param>
    ''' <param name="contnErreur">Indique si le traitement peut continuer en cas d'erreur.</param>
    ''' <returns>True si tous les changements demandés ont réussi, False autrement.</returns>
    ''' <remarks>
    ''' <para>
    ''' Cette méthode ne permet que d'effacer des utilisateurs, elle ne permet pas
    ''' de les disssocier des rôles ou des ressrouces.
    ''' </para>
    ''' <para>
    ''' Si le paramètre <paramref name="contnErreur"/> a la valeur <c>True</c>, le connecteur
    ''' est libre d'ignorer les erreurs qui pourraient se produire lors du traitement d'un utilisateur.
    ''' Demander d'enlever des utilisateur inexistants
    ''' ne doit pas être considéré comme une erreur par le connecteur.
    ''' Si le connecteur reçoit un élément pour lequel la propriété <see cref="TsCdConnxRole.CibleAJour" />
    ''' a une valeur supérieure ou égale à <see cref="TsECcCibleAJour.ExtractionAJour"/>,
    ''' il n'a pas à mettre à jour la cible et ça ne l'empêche pas de retourner <c>True</c>.
    ''' Lorsqu'un connecteur traite un élément correctement, il doit les marquer en affectant à
    ''' la propriété <see cref="TsCdConnxRole.CibleAJour" /> la valeur
    ''' <see cref="TsECcCibleAJour.MajDemande"/> ou <see cref="TsECcCibleAJour.AJour"/>.
    ''' </para>
    ''' </remarks>
    Function DetruireUsers(ByVal suppr As IEnumerable(Of TsCdConnxUser), Optional ByVal contnErreur As Boolean = False) As Boolean

    ''' <summary>
    ''' Effectue des modifications des attributs des utilisateurs dans le système cible.
    ''' </summary>
    ''' <param name="ajouts">Les attributs des utilisateurs à ajouter.</param>
    ''' <param name="suppr">Les attributs des utilisateurs à effacer.</param>
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
    Function AppliquerAttrbUsers(ByVal ajouts As IEnumerable(Of TsCdConnxUserAttrb), ByVal suppr As IEnumerable(Of TsCdConnxUserAttrb), Optional ByVal contnErreur As Boolean = False) As Boolean

    ''' <summary>
    ''' Effectue des créations et des destructions de liens entre les utilisateurs et les rôles dans le système cible.
    ''' </summary>
    ''' <param name="ajouts">Liens utilisateur-rôle à créer.</param>
    ''' <param name="suppr">Liens utilisateur-rôle à détruire.</param>
    ''' <param name="contnErreur">Indique si le traitement peut continuer en cas d'erreur.</param>
    ''' <returns>True si tous les changements demandés ont réussi, False autrement.</returns>
    ''' <remarks>
    ''' <para>
    ''' Si le paramètre <paramref name="contnErreur"/> a la valeur <c>True</c>, le connecteur
    ''' est libre d'ignorer les erreurs qui pourraient se produire lors du traitement d'un lien.
    ''' Demander d'ajouter des liens déjà existants ou d'enlever des liens inexistants
    ''' ne doit pas être considéré comme une erreur par le connecteur.
    ''' Si le connecteur reçoit un élément pour lequel la propriété <see cref="TsCdConnxUserRole.CibleAJour" />
    ''' a une valeur supérieure ou égale à <see cref="TsECcCibleAJour.ExtractionAJour"/>,
    ''' il n'a pas à mettre à jour la cible et ça ne l'empêche pas de retourner <c>True</c>.
    ''' Lorsqu'un connecteur traite un élément correctement, il doit les marquer en affectant à
    ''' la propriété <see cref="TsCdConnxUserRole.CibleAJour" /> la valeur
    ''' <see cref="TsECcCibleAJour.MajDemande"/> ou <see cref="TsECcCibleAJour.AJour"/>.
    ''' </para>
    ''' </remarks>
    Function AppliquerLiensUserRole(ByVal ajouts As IEnumerable(Of TsCdConnxUserRole), ByVal suppr As IEnumerable(Of TsCdConnxUserRole), Optional ByVal contnErreur As Boolean = False) As Boolean

    ''' <summary>
    ''' Effectue des créations et des destructions de liens entre les utilisateurs et les ressources dans le système cible.
    ''' </summary>
    ''' <param name="ajouts">Liens utilisateur-ressource à créer.</param>
    ''' <param name="suppr">Liens utilisateur-ressource à détruire.</param>
    ''' <param name="contnErreur">Indique si le traitement peut continuer en cas d'erreur.</param>
    ''' <returns>True si tous les changements demandés ont réussi, False autrement.</returns>
    ''' <remarks>
    ''' <para>
    ''' Si le paramètre <paramref name="contnErreur"/> a la valeur <c>True</c>, le connecteur
    ''' est libre d'ignorer les erreurs qui pourraient se produire lors du traitement d'un lien.
    ''' Demander d'ajouter des liens déjà existants ou d'enlever des liens inexistants
    ''' ne doit pas être considéré comme une erreur par le connecteur.
    ''' Si le connecteur reçoit un élément pour lequel la propriété <see cref="TsCdConnxUserRessr.CibleAJour" />
    ''' a une valeur supérieure ou égale à <see cref="TsECcCibleAJour.ExtractionAJour"/>,
    ''' il n'a pas à mettre à jour la cible et ça ne l'empêche pas de retourner <c>True</c>.
    ''' Lorsqu'un connecteur traite un élément correctement, il doit les marquer en affectant à
    ''' la propriété <see cref="TsCdConnxUserRessr.CibleAJour" /> la valeur
    ''' <see cref="TsECcCibleAJour.MajDemande"/> ou <see cref="TsECcCibleAJour.AJour"/>.
    ''' </para>
    ''' </remarks>
    Function AppliquerLiensUserRessr(ByVal ajouts As IEnumerable(Of TsCdConnxUserRessr), ByVal suppr As IEnumerable(Of TsCdConnxUserRessr), Optional ByVal contnErreur As Boolean = False) As Boolean


    ' Méthodes qui permettent de vérifier si des changements sont à faire dans un système cible

    ''' <summary>
    ''' Vérifie dans le système cible les utilisateurs qui sont déjà existants ou inexistants.
    ''' </summary>
    ''' <param name="ajouts">Utilisateurs dont l'existance est à vérifier dans le système cible.</param>
    ''' <param name="suppr">Utilisateurs dont l'inexistance est à vérifier dans le système cible.</param>
    ''' <remarks>
    ''' Cette méthode doit vérifier l'état du système cible et
    ''' marquer les utilisateurs déjà existants du paramètre <paramref name="ajouts" />
    ''' ainsi que les utilisateurs déjà inexistants du paramètre <paramref name="suppr" />
    ''' en affectant à <see cref="TsCdConnxRole.CibleAJour"/> une des valeurs
    ''' <see cref="TsECcCibleAJour.ExtractionPasAJour" />, <see cref="TsECcCibleAJour.ExtractionAJour" />,
    ''' <see cref="TsECcCibleAJour.PasAJour" /> ou <see cref="TsECcCibleAJour.AJour" /> selon le cas.
    ''' </remarks>
    Sub VerifierUsers(ByVal ajouts As IEnumerable(Of TsCdConnxUser), ByVal suppr As IEnumerable(Of TsCdConnxUser))

    ''' <summary>
    ''' Vérifie dans le système cible les attributs des utilisateurs qui sont déjà existants ou inexistants.
    ''' </summary>
    ''' <param name="ajouts">Utilisateurs dont l'existance est à vérifier dans le système cible.</param>
    ''' <param name="suppr">Utilisateurs dont l'inexistance est à vérifier dans le système cible.</param>
    ''' <remarks>
    ''' Cette méthode doit vérifier l'état du système cible et
    ''' marquer les attributs des utilisateurs déjà existants du paramètre <paramref name="ajouts" />
    ''' ainsi que les attributs utilisateurs déjà inexistants du paramètre <paramref name="suppr" />
    ''' en affectant à <see cref="TsCdConnxRole.CibleAJour"/> une des valeurs
    ''' <see cref="TsECcCibleAJour.ExtractionPasAJour" />, <see cref="TsECcCibleAJour.ExtractionAJour" />,
    ''' <see cref="TsECcCibleAJour.PasAJour" /> ou <see cref="TsECcCibleAJour.AJour" /> selon le cas.
    ''' </remarks>
    Sub VerifierAttrbUsers(ByVal ajouts As IEnumerable(Of TsCdConnxUserAttrb), ByVal suppr As IEnumerable(Of TsCdConnxUserAttrb))

    ''' <summary>
    ''' Vérifie dans le système cible les liens entre les utilisateurs et les rôles qui sont déjà existants ou inexistants.
    ''' </summary>
    ''' <param name="ajouts">Liens dont l'existance est à vérifier dans le système cible.</param>
    ''' <param name="suppr">Liens dont l'inexistance est à vérifier dans le système cible.</param>
    ''' <remarks>
    ''' <para>
    ''' Cette méthode doit vérifier l'état du système cible et
    ''' marquer les liens déjà existants du paramètre <paramref name="ajouts" />
    ''' ainsi que les liens déjà inexistants du paramètre <paramref name="suppr" />
    ''' Cette méthode doit vérifier l'état du système cible et
    ''' marquer les liens déjà existants du paramètre <paramref name="ajouts" />
    ''' ainsi que les liens déjà inexistants du paramètre <paramref name="suppr" />
    ''' en affectant à <see cref="TsCdConnxUserRole.CibleAJour"/> une des valeurs
    ''' <see cref="TsECcCibleAJour.ExtractionPasAJour" />, <see cref="TsECcCibleAJour.ExtractionAJour" />,
    ''' <see cref="TsECcCibleAJour.PasAJour" /> ou <see cref="TsECcCibleAJour.AJour" /> selon le cas.
    ''' Lorsqu'un connecteur traite un élément correctement, il doit les marquer en affectant à
    ''' la propriété <see cref="TsCdConnxUserRole.CibleAJour" /> la valeur
    ''' <see cref="TsECcCibleAJour.MajDemande"/> ou <see cref="TsECcCibleAJour.AJour"/>.
    ''' </para>
    ''' </remarks>
    Sub VerifierLiensUserRole(ByVal ajouts As IEnumerable(Of TsCdConnxUserRole), ByVal suppr As IEnumerable(Of TsCdConnxUserRole))

    ''' <summary>
    ''' Vérifie dans le système cible les liens entre les utilisateurs et les ressources qui sont déjà existants ou inexistants.
    ''' </summary>
    ''' <param name="ajouts">Liens dont l'existance est à vérifier dans le système cible.</param>
    ''' <param name="suppr">Liens dont l'inexistance est à vérifier dans le système cible.</param>
    ''' <remarks>
    ''' Cette méthode doit vérifier l'état du système cible et
    ''' marquer les liens déjà existants du paramètre <paramref name="ajouts" />
    ''' ainsi que les liens déjà inexistants du paramètre <paramref name="suppr" />
    ''' Cette méthode doit vérifier l'état du système cible et
    ''' marquer les liens déjà existants du paramètre <paramref name="ajouts" />
    ''' ainsi que les liens déjà inexistants du paramètre <paramref name="suppr" />
    ''' en affectant à <see cref="TsCdConnxUserRessr.CibleAJour"/> une des valeurs
    ''' <see cref="TsECcCibleAJour.ExtractionPasAJour" />, <see cref="TsECcCibleAJour.ExtractionAJour" />,
    ''' <see cref="TsECcCibleAJour.PasAJour" /> ou <see cref="TsECcCibleAJour.AJour" /> selon le cas.
    ''' </remarks>
    Sub VerifierLiensUserRessr(ByVal ajouts As IEnumerable(Of TsCdConnxUserRessr), ByVal suppr As IEnumerable(Of TsCdConnxUserRessr))

End Interface
