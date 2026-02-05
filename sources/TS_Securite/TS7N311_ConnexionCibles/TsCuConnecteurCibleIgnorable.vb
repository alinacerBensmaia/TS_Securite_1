''' <summary>
''' Cette classe permet d'ignorer par défaut toutes les méthodes du connecteur cible.
''' Il faut en hériter et overrider des méthode pour en faire quelque chose d'utile.
''' </summary>
''' <remarks>
''' On supporte quand même l'appel des méthodes du connecteur, dans ce cas on marque tous les éléments comme étant «à jour».
''' </remarks>
Public MustInherit Class TsCuConnecteurCibleIgnorable
    Inherits TsCuConnecteurCible

    ''' <summary>
    ''' Construit une instance du connecteur en spécifiant explicitement le système cible concerné.
    ''' </summary>
    ''' <param name="idCible">Identificateur du système cible (resName3 dans Sage).</param>
    Public Sub New(ByVal idCible As String)
        MyBase.New(idCible)
    End Sub

    <TsAtMethodeIgnorable(True)> _
    Public Overrides Function CreerRoles(ByVal ajouts As IEnumerable(Of TsCdConnxRole), Optional ByVal contnErreur As Boolean = False) As Boolean
        MarquerAJour(ajouts)
    End Function

    <TsAtMethodeIgnorable(True)> _
    Public Overrides Function CreerUsers(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxUser), Optional ByVal contnErreur As Boolean = False) As Boolean
        MarquerAJour(ajouts)
    End Function

    <TsAtMethodeIgnorable(True)> _
    Public Overrides Function CreerRessr(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxRessr), Optional ByVal contnErreur As Boolean = False) As Boolean
        MarquerAJour(ajouts)
    End Function

    <TsAtMethodeIgnorable(True)> _
    Public Overrides Function DetruireRoles(ByVal suppr As IEnumerable(Of TsCdConnxRole), Optional ByVal contnErreur As Boolean = False) As Boolean
        MarquerAJour(suppr)
    End Function

    <TsAtMethodeIgnorable(True)> _
    Public Overrides Function DetruireUsers(ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxUser), Optional ByVal contnErreur As Boolean = False) As Boolean
        MarquerAJour(suppr)
    End Function

    <TsAtMethodeIgnorable(True)> _
    Public Overrides Function DetruireRessr(ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxRessr), Optional ByVal contnErreur As Boolean = False) As Boolean
        MarquerAJour(suppr)
    End Function

    <TsAtMethodeIgnorable(True)> _
    Public Overrides Function AppliquerAttrbRessr(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxRessrAttrb), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxRessrAttrb), Optional ByVal contnErreur As Boolean = False) As Boolean
        MarquerAJour(ajouts)
        MarquerAJour(suppr)
    End Function

    <TsAtMethodeIgnorable(True)> _
    Public Overrides Function AppliquerAttrbRoles(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxRoleAttrb), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxRoleAttrb), Optional ByVal contnErreur As Boolean = False) As Boolean
        MarquerAJour(ajouts)
        MarquerAJour(suppr)
    End Function

    <TsAtMethodeIgnorable(True)> _
    Public Overrides Function AppliquerAttrbUsers(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxUserAttrb), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxUserAttrb), Optional ByVal contnErreur As Boolean = False) As Boolean
        MarquerAJour(ajouts)
        MarquerAJour(suppr)
    End Function

    <TsAtMethodeIgnorable(True)> _
    Public Overrides Function AppliquerLiensRoleRole(ByVal ajouts As IEnumerable(Of TsCdConnxRoleRole), ByVal suppr As IEnumerable(Of TsCdConnxRoleRole), Optional ByVal contnErreur As Boolean = False) As Boolean
        MarquerAJour(ajouts)
        MarquerAJour(suppr)
    End Function

    <TsAtMethodeIgnorable(True)> _
    Public Overrides Function AppliquerLiensRoleRessr(ByVal ajouts As IEnumerable(Of TsCdConnxRoleRessr), ByVal suppr As IEnumerable(Of TsCdConnxRoleRessr), Optional ByVal contnErreur As Boolean = False) As Boolean
        MarquerAJour(ajouts)
        MarquerAJour(suppr)
    End Function

    <TsAtMethodeIgnorable(True)> _
    Public Overrides Function AppliquerLiensUserRessr(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRessr), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRessr), Optional ByVal contnErreur As Boolean = False) As Boolean
        MarquerAJour(ajouts)
        MarquerAJour(suppr)
    End Function

    <TsAtMethodeIgnorable(True)> _
    Public Overrides Function AppliquerLiensUserRole(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRole), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRole), Optional ByVal contnErreur As Boolean = False) As Boolean
        MarquerAJour(ajouts)
        MarquerAJour(suppr)
    End Function

    ' Pas besoin de marquer les méthodes Verifier avec <TsAtMethodeIgnorable(True)> _
    Public Overrides Sub VerifierRoles(ByVal ajouts As IEnumerable(Of TsCdConnxRole), ByVal suppr As IEnumerable(Of TsCdConnxRole))
        MarquerAJour(ajouts)
        MarquerAJour(suppr)
    End Sub

    ' Pas besoin de marquer les méthodes Verifier avec <TsAtMethodeIgnorable(True)> _
    Public Overrides Sub VerifierUsers(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxUser), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxUser))
        MarquerAJour(ajouts)
        MarquerAJour(suppr)
    End Sub

    ' Pas besoin de marquer les méthodes Verifier avec <TsAtMethodeIgnorable(True)> _
    Public Overrides Sub VerifierRessr(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxRessr), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxRessr))
        MarquerAJour(ajouts)
        MarquerAJour(suppr)
    End Sub

    ' Pas besoin de marquer les méthodes Verifier avec <TsAtMethodeIgnorable(True)> _
    Public Overrides Sub VerifierAttrbRoles(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxRoleAttrb), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxRoleAttrb))
        MarquerAJour(ajouts)
        MarquerAJour(suppr)
    End Sub

    ' Pas besoin de marquer les méthodes Verifier avec <TsAtMethodeIgnorable(True)> _
    Public Overrides Sub VerifierAttrbUsers(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxUserAttrb), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxUserAttrb))
        MarquerAJour(ajouts)
        MarquerAJour(suppr)
    End Sub

    ' Pas besoin de marquer les méthodes Verifier avec <TsAtMethodeIgnorable(True)> _
    Public Overrides Sub VerifierAttrbRessr(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxRessrAttrb), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxRessrAttrb))
        MarquerAJour(ajouts)
        MarquerAJour(suppr)
    End Sub

    ' Pas besoin de marquer les méthodes Verifier avec <TsAtMethodeIgnorable(True)> _
    Public Overrides Sub VerifierLiensRoleRole(ByVal ajouts As IEnumerable(Of TsCdConnxRoleRole), ByVal suppr As IEnumerable(Of TsCdConnxRoleRole))
        MarquerAJour(ajouts)
        MarquerAJour(suppr)
    End Sub

    ' Pas besoin de marquer les méthodes Verifier avec <TsAtMethodeIgnorable(True)> _
    Public Overrides Sub VerifierLiensRoleRessr(ByVal ajouts As IEnumerable(Of TsCdConnxRoleRessr), ByVal suppr As IEnumerable(Of TsCdConnxRoleRessr))
        MarquerAJour(ajouts)
        MarquerAJour(suppr)
    End Sub

    ' Pas besoin de marquer les méthodes Verifier avec <TsAtMethodeIgnorable(True)> _
    Public Overrides Sub VerifierLiensUserRessr(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRessr), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRessr))
        MarquerAJour(ajouts)
        MarquerAJour(suppr)
    End Sub

    ' Pas besoin de marquer les méthodes Verifier avec <TsAtMethodeIgnorable(True)> _
    Public Overrides Sub VerifierLiensUserRole(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRole), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRole))
        MarquerAJour(ajouts)
        MarquerAJour(suppr)
    End Sub

End Class

