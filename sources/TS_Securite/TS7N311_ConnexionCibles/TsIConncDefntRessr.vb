''' <summary>
''' Interface pour un connecteur permettant de gérer les ressources sur un système cible.
''' </summary>
Public Interface TsIConncMajDefntRessr
    Inherits IDisposable
    ' Méthodes qui permettent d'obtenir l'état d'un système cible (dans une interface de lecture...)
    ' ObtenirRessr
    ' ObtenirLiensRoleRole
    ' ObtenirLiensRoleRessr
    ' ObtenirDiffAttrbRessr


    ' Méthodes qui permettent de faire des changements dans un système cible

    ''' <summary>
    ''' Effectue des créations de ressource dans le système cible.
    ''' </summary>
    ''' <param name="ajouts">Ressources à créer.</param>
    ''' <param name="contnErreur">Indique si le traitement peut continuer en cas d'erreur.</param>
    ''' <returns>True si tous les changements demandés ont réussi, False autrement.</returns>
    ''' <remarks>
    ''' <para>
    ''' Cette méthode ne permet que de créer des ressource, elle ne permet pas
    ''' de les associer à des utilisateurs ni à des rôles.
    ''' </para>
    ''' <para>
    ''' Si le paramètre <paramref name="contnErreur"/> a la valeur <c>True</c>, le connecteur
    ''' est libre d'ignorer les erreurs qui pourraient se produire lors du traitement d'une ressource.
    ''' Demander d'ajouter des ressources déjà existants 
    ''' ne doit pas être considéré comme une erreur par le connecteur.
    ''' Si le connecteur reçoit un élément pour lequel la propriété <see cref="TsCdConnxRole.CibleAJour" />
    ''' a une valeur supérieure ou égale à <see cref="TsECcCibleAJour.ExtractionAJour"/>,
    ''' il n'a pas à mettre à jour la cible et ça ne l'empêche pas de retourner <c>True</c>.
    ''' Lorsqu'un connecteur traite un élément correctement, il doit les marquer en affectant à
    ''' la propriété <see cref="TsCdConnxRole.CibleAJour" /> la valeur
    ''' <see cref="TsECcCibleAJour.MajDemande"/> ou <see cref="TsECcCibleAJour.AJour"/>.
    ''' </para>
    ''' </remarks>
    Function CreerRessr(ByVal ajouts As IEnumerable(Of TsCdConnxRessr), Optional ByVal contnErreur As Boolean = False) As Boolean

    ''' <summary>
    ''' Effectue des destructions de ressource dans le système cible.
    ''' </summary>
    ''' <param name="suppr">Ressources à détruire.</param>
    ''' <param name="contnErreur">Indique si le traitement peut continuer en cas d'erreur.</param>
    ''' <returns>True si tous les changements demandés ont réussi, False autrement.</returns>
    ''' <remarks>
    ''' <para>
    ''' Cette méthode ne permet que d'effacer des ressources, elle ne permet pas
    ''' de les disssocier des utilisateurs ou des rôles.
    ''' </para>
    ''' <para>
    ''' Si le paramètre <paramref name="contnErreur"/> a la valeur <c>True</c>, le connecteur
    ''' est libre d'ignorer les erreurs qui pourraient se produire lors du traitement d'une ressource.
    ''' Demander d'enlever des ressources inexistants
    ''' ne doit pas être considéré comme une erreur par le connecteur.
    ''' Si le connecteur reçoit un élément pour lequel la propriété <see cref="TsCdConnxRole.CibleAJour" />
    ''' a une valeur supérieure ou égale à <see cref="TsECcCibleAJour.ExtractionAJour"/>,
    ''' il n'a pas à mettre à jour la cible et ça ne l'empêche pas de retourner <c>True</c>.
    ''' Lorsqu'un connecteur traite un élément correctement, il doit les marquer en affectant à
    ''' la propriété <see cref="TsCdConnxRole.CibleAJour" /> la valeur
    ''' <see cref="TsECcCibleAJour.MajDemande"/> ou <see cref="TsECcCibleAJour.AJour"/>.
    ''' </para>
    ''' </remarks>
    Function DetruireRessr(ByVal suppr As IEnumerable(Of TsCdConnxRessr), Optional ByVal contnErreur As Boolean = False) As Boolean

    ''' <summary>
    ''' Effectue des modifications des attributs des ressources dans le système cible.
    ''' </summary>
    ''' <param name="ajouts">Les attributs des ressources à ajouter.</param>
    ''' <param name="suppr">Les attributs des ressources à effacer.</param>
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
    Function AppliquerAttrbRessr(ByVal ajouts As IEnumerable(Of TsCdConnxRessrAttrb), ByVal suppr As IEnumerable(Of TsCdConnxRessrAttrb), Optional ByVal contnErreur As Boolean = False) As Boolean

    ' Méthodes qui permettent de vérifier si des changements sont à faire dans le système cible


    ''' <summary>
    ''' Vérifie dans le système cible les ressources qui sont déjà existants ou inexistants.
    ''' </summary>
    ''' <param name="ajouts">Ressources dont l'existance est à vérifier dans le système cible.</param>
    ''' <param name="suppr">Ressources dont l'inexistance est à vérifier dans le système cible.</param>
    ''' <remarks>
    ''' Cette méthode doit vérifier l'état du système cible et
    ''' marquer les ressources déjà existants du paramètre <paramref name="ajouts" />
    ''' ainsi que les ressources déjà inexistants du paramètre <paramref name="suppr" />
    ''' en affectant à <see cref="TsCdConnxRole.CibleAJour"/> une des valeurs
    ''' <see cref="TsECcCibleAJour.ExtractionPasAJour" />, <see cref="TsECcCibleAJour.ExtractionAJour" />,
    ''' <see cref="TsECcCibleAJour.PasAJour" /> ou <see cref="TsECcCibleAJour.AJour" /> selon le cas.
    ''' </remarks>
    Sub VerifierRessr(ByVal ajouts As IEnumerable(Of TsCdConnxRessr), ByVal suppr As IEnumerable(Of TsCdConnxRessr))

    ''' <summary>
    ''' Vérifie dans le système cible les attributs des ressources qui sont déjà existants ou inexistants.
    ''' </summary>
    ''' <param name="ajouts">Ressources dont l'existance est à vérifier dans le système cible.</param>
    ''' <param name="suppr">Ressources dont l'inexistance est à vérifier dans le système cible.</param>
    ''' <remarks>
    ''' Cette méthode doit vérifier l'état du système cible et
    ''' marquer les attributs des ressources déjà existants du paramètre <paramref name="ajouts" />
    ''' ainsi que les attributs ressources déjà inexistants du paramètre <paramref name="suppr" />
    ''' en affectant à <see cref="TsCdConnxRole.CibleAJour"/> une des valeurs
    ''' <see cref="TsECcCibleAJour.ExtractionPasAJour" />, <see cref="TsECcCibleAJour.ExtractionAJour" />,
    ''' <see cref="TsECcCibleAJour.PasAJour" /> ou <see cref="TsECcCibleAJour.AJour" /> selon le cas.
    ''' </remarks>
    Sub VerifierAttrbRessr(ByVal ajouts As IEnumerable(Of TsCdConnxRessrAttrb), ByVal suppr As IEnumerable(Of TsCdConnxRessrAttrb))

End Interface






