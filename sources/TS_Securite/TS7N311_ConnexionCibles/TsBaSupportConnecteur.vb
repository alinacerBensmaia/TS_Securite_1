''' <summary>
''' Cette classe permet d'ignorer par défaut toutes les méthodes du connecteur cible.
''' Il faut en hériter et overrider des méthode pour en faire quelque chose d'utile.
''' </summary>
Public Module TsBaSupportConnecteur

    ' Même avec les Generics de VB.NET 2008, il n'y a pas assez d'inférence de type pour faire qqc de plus général (sans alourdir la tâche pour l'appelant)
    'XXX pas si sûr, voir mon projet TestAttrib
    Public Delegate Function MethodeUser(ByVal ajouts As IEnumerable(Of TsCdConnxUser), ByVal contnErreur As Boolean) As Boolean
    Public Delegate Function MethodeRole(ByVal ajouts As IEnumerable(Of TsCdConnxRole), ByVal contnErreur As Boolean) As Boolean
    Public Delegate Function MethodeRessr(ByVal ajouts As IEnumerable(Of TsCdConnxRessr), ByVal contnErreur As Boolean) As Boolean

    Public Delegate Function MethodeUserAttrb(ByVal ajouts As IEnumerable(Of TsCdConnxUserAttrb), ByVal suppr As IEnumerable(Of TsCdConnxUserAttrb), ByVal contnErreur As Boolean) As Boolean
    Public Delegate Function MethodeRoleAttrb(ByVal ajouts As IEnumerable(Of TsCdConnxRoleAttrb), ByVal suppr As IEnumerable(Of TsCdConnxRoleAttrb), ByVal contnErreur As Boolean) As Boolean
    Public Delegate Function MethodeRessrAttrb(ByVal ajouts As IEnumerable(Of TsCdConnxRessrAttrb), ByVal suppr As IEnumerable(Of TsCdConnxRessrAttrb), ByVal contnErreur As Boolean) As Boolean

    Public Delegate Function MethodeLienRoleRole(ByVal ajouts As IEnumerable(Of TsCdConnxRoleRole), ByVal suppr As IEnumerable(Of TsCdConnxRoleRole), ByVal contnErreur As Boolean) As Boolean
    Public Delegate Function MethodeLienRoleResr(ByVal ajouts As IEnumerable(Of TsCdConnxRoleRessr), ByVal suppr As IEnumerable(Of TsCdConnxRoleRessr), ByVal contnErreur As Boolean) As Boolean
    Public Delegate Function MethodeLienUserRole(ByVal ajouts As IEnumerable(Of TsCdConnxUserRole), ByVal suppr As IEnumerable(Of TsCdConnxUserRole), ByVal contnErreur As Boolean) As Boolean
    Public Delegate Function MethodeLienUserResr(ByVal ajouts As IEnumerable(Of TsCdConnxUserRessr), ByVal suppr As IEnumerable(Of TsCdConnxUserRessr), ByVal contnErreur As Boolean) As Boolean


    Public Sub MarquerAJour(Of T As TsCdElementConnexion)(ByVal liste As IEnumerable(Of T), Optional ByVal valeur As TsECcCibleAJour = TsECcCibleAJour.AJour)
        For Each e As T In liste
            e.CibleAJour = valeur
        Next
    End Sub

    Public Function SupporteRole(ByVal connecteur As Object) As Boolean
        Return SupporteRole(connecteur.GetType())
    End Function

    Public Function SupporteRole(ByVal type As Type) As Boolean
        Dim supporte As Boolean = TsAtCibleRBACAttribute.Default.CibleRBAC
        Dim attrib As TsAtCibleRBACAttribute = DirectCast(Attribute.GetCustomAttribute(type, GetType(TsAtCibleRBACAttribute)), TsAtCibleRBACAttribute)
        If attrib IsNot Nothing Then
            supporte = attrib.CibleRBAC
        End If
        Return supporte
    End Function


    Public Function MethodeIgnorable(ByVal deleg As MethodeUser) As Boolean
        Return MethodeIgnorable(deleg.Method)
    End Function

    Public Function MethodeIgnorable(ByVal deleg As MethodeRole) As Boolean
        Return MethodeIgnorable(deleg.Method)
    End Function

    Public Function MethodeIgnorable(ByVal deleg As MethodeRessr) As Boolean
        Return MethodeIgnorable(deleg.Method)
    End Function


    Public Function MethodeIgnorable(ByVal deleg As MethodeUserAttrb) As Boolean
        Return MethodeIgnorable(deleg.Method)
    End Function

    Public Function MethodeIgnorable(ByVal deleg As MethodeRoleAttrb) As Boolean
        Return MethodeIgnorable(deleg.Method)
    End Function

    Public Function MethodeIgnorable(ByVal deleg As MethodeRessrAttrb) As Boolean
        Return MethodeIgnorable(deleg.Method)
    End Function


    Public Function MethodeIgnorable(ByVal deleg As MethodeLienRoleRole) As Boolean
        Return MethodeIgnorable(deleg.Method)
    End Function

    Public Function MethodeIgnorable(ByVal deleg As MethodeLienRoleResr) As Boolean
        Return MethodeIgnorable(deleg.Method)
    End Function

    Public Function MethodeIgnorable(ByVal deleg As MethodeLienUserRole) As Boolean
        Return MethodeIgnorable(deleg.Method)
    End Function

    Public Function MethodeIgnorable(ByVal deleg As MethodeLienUserResr) As Boolean
        Return MethodeIgnorable(deleg.Method)
    End Function

    Public Function MethodeIgnorable(ByVal m As System.Reflection.MethodInfo) As Boolean
        Dim ignorable As Boolean = TsAtMethodeIgnorableAttribute.Default.Ignorable
        Dim attrib As TsAtMethodeIgnorableAttribute = DirectCast(Attribute.GetCustomAttribute(m, GetType(TsAtMethodeIgnorableAttribute)), TsAtMethodeIgnorableAttribute)
        If attrib IsNot Nothing Then
            ignorable = attrib.Ignorable
        End If
        Return ignorable
    End Function

End Module

