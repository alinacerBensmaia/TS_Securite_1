

''' <summary>
''' Permet de définir les paramètres de couleur du fond d'une cellule d'un Style.
''' </summary>
''' <remarks></remarks>
Public Class TsCuStyleInterior

#Region "--- Énumérations ---"

    ''' <summary>
    ''' Définition du type de Pattern disponible.
    ''' </summary>
    Enum PatternType
        None
        Solid
        Gray75
        Gray50
        Gray25
        Gray125
        Gray0625
        HorzStripe
        VertStripe
        ReverseDiagStripe
        DiagStripe
        DiagCross
        ThickDiagCross
        ThinHorzStripe
        ThinVertStripe
        ThinReverseDiagStripe
        ThinDiagStripe
        ThinHorzCross
        ThinDiagCross
    End Enum

#End Region

#Region "--- Propiétés ---"

    'ss:Color
    Private mColor As String = Nothing
    ''' <summary>
    ''' Définit la couleur du font de la cellule. 
    ''' Code de couleur RVB: "#rrvvbb".
    ''' La valeur "Automatic" est accepté.
    ''' </summary>
    Public Property Color() As String
        Get
            Return mColor
        End Get
        Set(ByVal value As String)
            mColor = value
        End Set
    End Property

    'ss:Pattern
    Private mPattern As PatternType?
    ''' <summary>
    ''' Définit comment Color et le ColorPattern se mélange.
    ''' </summary>
    Public Property Pattern() As PatternType?
        Get
            Return mPattern
        End Get
        Set(ByVal value As PatternType?)
            mPattern = value
        End Set
    End Property

    'ss:PatternColor
    Private mPatternColor As String = Nothing
    ''' <summary>
    ''' Spéficie une couleur secondaire. 
    ''' Code de couleur RVB: "#rrvvbb".
    ''' </summary>
    ''' <remarks>Attribut spécifique à Excel.</remarks>
    Public Property PatternColor() As String
        Get
            Return mPatternColor
        End Get
        Set(ByVal value As String)
            mPatternColor = value
        End Set
    End Property

#End Region

#Region "--- Méthodes ---"

    ''' <summary>
    ''' Permet d'obtenir la version XML valide SpreadSheet de l'intérieur.
    ''' </summary>
    ''' <returns>Un XML.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirXML() As String
        Dim balise As String = ""
        balise &= "<Interior"
        balise &= TsCuOutilsExcel.ConstruireAttributOptionnel("ss:Color", Color)
        balise &= TsCuOutilsExcel.ConstruireAttributOptionnel("ss:Pattern", Pattern)
        balise &= TsCuOutilsExcel.ConstruireAttributOptionnel("ss:PatternColor", PatternColor)
        balise &= "/>"

        Return balise
    End Function

#End Region

End Class