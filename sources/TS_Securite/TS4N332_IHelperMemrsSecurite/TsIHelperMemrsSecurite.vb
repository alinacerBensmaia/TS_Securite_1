Public Interface TsIHelperMemrsSecurite

    Function ObtenirObjetMemoire(ByVal pCleObjetMemoire As String) As Object

    Sub Memoriser(ByVal pCleObjetMemoire As String, ByVal pObjetSecurite As Object)

    Sub Dememoriser(ByVal pCleObjetMemoire As String)

    Sub DememoriserTout()

End Interface

''' <summary>
''' Identifie les types de mémorisation.
''' </summary>
<Flags()> _
Public Enum TsTypeMemorisation

    ''' <summary>
    ''' Aucun.
    ''' </summary>
    Aucun

    ''' <summary>
    ''' RunTime Cache.
    ''' </summary>
    Runtime

End Enum