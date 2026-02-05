''' <summary>
''' Classe qui contient des fonctions communes.
''' </summary>
''' <remarks></remarks>
Friend Class TsCuOutilsExcel

    ''' <summary>
    ''' Permet de construire un attribut d'une balise.
    ''' </summary>
    ''' <param name="pNomAttribut">Le nom de l'attribut.</param>
    ''' <param name="pValeur">La valeur à associé à l'attribut.</param>
    ''' <returns>Un attribut XML.</returns>
    ''' <remarks></remarks>
    Public Shared Function ConstruireAttribut(ByVal pNomAttribut As String, ByVal pValeur As String) As String
        Dim q As String = """"
        Dim attribut As String = ""

        attribut &= String.Format(" {0}={2}{1}{2}", pNomAttribut, ObtenirValeurConvertie(pValeur), q)

        Return attribut
    End Function

    ''' <summary>
    ''' Permet de construire un attribut d'une balise.
    ''' </summary>
    ''' <param name="pNomAttribut">Le nom de l'attribut.</param>
    ''' <param name="pValeur">La valeur à associé à l'attribut.</param>
    ''' <returns>Un attribut XML.</returns>
    ''' <remarks></remarks>
    Public Shared Function ConstruireAttribut(Of T)(ByVal pNomAttribut As String, ByVal pValeur As T) As String
        Dim q As String = """"
        Dim attribut As String = ""

        attribut &= String.Format(" {0}={2}{1}{2}", pNomAttribut, ObtenirValeurConvertie(pValeur), q)

        Return attribut
    End Function

    ''' <summary>
    ''' Permet de construire un attribut optionnel d'une balise.
    ''' </summary>
    ''' <param name="pNomAttribut">Le nom de l'attribut.</param>
    ''' <param name="pValeur">La valeur à associé à l'attribut.</param>
    ''' <returns>Un attribut XML.</returns>
    ''' <remarks></remarks>
    Public Shared Function ConstruireAttributOptionnel(ByVal pNomAttribut As String, ByVal pValeur As String) As String
        Dim q As String = """"
        Dim attribut As String = ""

        If String.IsNullOrEmpty(pValeur) = False Then
            attribut &= String.Format(" {0}={2}{1}{2}", pNomAttribut, ObtenirValeurConvertie(pValeur), q)
        End If

        Return attribut
    End Function

    ''' <summary>
    ''' Permet de construire un attribut optionnel d'une balise.
    ''' </summary>
    ''' <param name="pNomAttribut">Le nom de l'attribut.</param>
    ''' <param name="pValeur">La valeur à associé à l'attribut.</param>
    ''' <returns>Un attribut XML.</returns>
    ''' <remarks></remarks>
    Public Shared Function ConstruireAttributOptionnel(Of T As Structure)(ByVal pNomAttribut As String, ByVal pValeur As Nullable(Of T)) As String
        Dim q As String = """"
        Dim attribut As String = ""

        If pValeur.HasValue = True Then
            attribut &= String.Format(" {0}={2}{1}{2}", pNomAttribut, ObtenirValeurConvertie(pValeur), q)
        End If

        Return attribut
    End Function

    ''' <summary>
    ''' Format certain type connue en donnée Excel XML.
    ''' </summary>
    ''' <param name="pValeur">La valeur à évaluer.</param>
    ''' <returns>La valeur convertie.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirValeurConvertie(ByVal pValeur As Object) As String

        If TypeOf pValeur Is Boolean Then
            Return If(DirectCast(pValeur, Boolean) = True, "1", "0")
        End If

        If TypeOf pValeur Is Boolean? Then
            Dim valeur = DirectCast(pValeur, Boolean?)
            If valeur.HasValue Then
                Return If(valeur.Value, "1", "0")
            End If
        End If

        If TypeOf pValeur Is Double Then
            Return DirectCast(pValeur, Double).ToString.Replace(","c, "."c)
        End If

        If TypeOf pValeur Is Double? Then
            Dim valeur = DirectCast(pValeur, Double?)
            If valeur.HasValue Then
                Return valeur.ToString.Replace(","c, "."c)
            End If
        End If

        Return pValeur.ToString
    End Function

End Class
