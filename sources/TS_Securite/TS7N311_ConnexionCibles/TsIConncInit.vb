''' <summary>
''' Interface pour un connecteur qui a de l'interation à faire avec un utilisateur à un moment jugé opportun par l'appelant.
''' </summary>
Public Interface TsIConncInteraction

    ''' <summary>
    ''' Permet à un connecteur de faire son initialisation (ou un reset) à un moment jugé opportun par l'appelant.
    ''' Cette initialisation ne devrait pas être longue, mais elle peut demander l'interaction d'un utilisateur.
    ''' </summary>
    ''' <returns><c>True</c> si l'initialisation a fonctionné, <c>False</c> autrement.</returns>
    ''' <remarks>
    ''' Puisqu'on s'attend à de l'interaction dans cette méthode, il vaut mieux éviter qu'il y ait des opérations longue.
    ''' S'il y a de l'initialisation qui peut être longue, il vaudrait mieux la faire dans les méthodes principales du connecteur.
    ''' Cette méthode peut être appelée plusieurs fois. Suite à un appel, le connecteur doit être dans un état équivalent
    ''' à celui qu'il serait si l'objet venait d'être créé et que l'initialisation était dans le constructeur.
    ''' Si ce n'est pas possible, cette méthode doit lancer une exception. 
    ''' </remarks>
    Function Initialiser() As Boolean

End Interface
