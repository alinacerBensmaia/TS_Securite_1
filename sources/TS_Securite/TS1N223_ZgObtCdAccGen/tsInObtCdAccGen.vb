<Obsolete("Interface désuète, utilisez la classe 'tsCuObtCdAccGen' directement.", False)>
Public Interface tsInObtCdAccGen
    Sub ObtenirCodeAccesMotDePasse(ByVal strCle As String, ByVal strRaison As String, ByRef strCompte As String, ByRef strMDP As String)

    'Sub ObtenirCodeAccesGenerique(ByVal strCle As String, ByVal strRaison As String, ByRef strCompte As String, ByRef strMDP As String)
    'Sub ObtenirCodeAccesGenerique(ByVal strCle As String, ByVal strEnvrn As String, ByVal strRaison As String, ByRef strCompte As String, ByRef strMDP As String)
    'Sub ObtenirCodeAccesMotDePasseAvecVerif(ByVal strCle As String, ByVal strRaison As String, ByVal strCodeVerif As String, ByRef strCompte As String, ByRef strMDP As String)
    'Function ObtenirClesAccessible(ByVal strTri As String) As DataRow()
    'Function ObtenirListeRecherchee(ByVal strClause As String) As DataRow()
    'Function ObtenirListeCleTS5() As DataTable
End Interface
