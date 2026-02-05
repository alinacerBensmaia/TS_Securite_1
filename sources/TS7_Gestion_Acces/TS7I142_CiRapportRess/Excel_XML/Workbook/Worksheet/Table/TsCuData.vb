

''' <summary>
''' Contient les informations de la cellule à afficher.
''' </summary>
''' <remarks></remarks>
Public Class TsCuData

#Region "--- Énumérations ---"

    ''' <summary>
    ''' Définition des types de data.
    ''' </summary>
    ''' <remarks></remarks>
    Enum TypeType
        Number
        DateTime
        [Boolean]
        [String]
        [Error]
    End Enum

#End Region

#Region "--- Propriétés ---"

    'ss:Type
    Private mType As TypeType
    ''' <summary>
    ''' Le type de Data.
    ''' </summary>
    Public ReadOnly Property Type() As TypeType
        Get
            Return mType
        End Get
    End Property

    Private mContenue As String = ""
    ''' <summary>
    ''' Le contenue de la cellule.
    ''' </summary>
    Public Property Contenue() As String
        Get
            Return mContenue
        End Get
        Set(ByVal value As String)
            mContenue = value
        End Set
    End Property

    'x:Ticked
    Private mTicked As Boolean?
    ''' <summary>
    ''' Si à vrai, il rajoute au data un ' lors du chargement et l'enlève lors de l'enregistrement.
    ''' Fonctionne en conjontion avec le Type String.
    ''' </summary>
    Public Property Ticked() As Boolean?
        Get
            Return mTicked
        End Get
        Set(ByVal value As Boolean?)
            mTicked = value
        End Set
    End Property

#End Region

#Region "--- Constructeurs ---"

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <param name="pType">Le type de data.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal pType As TypeType)
        mType = pType
    End Sub

#End Region

#Region "--- Méthodes ---"

    ''' <summary>
    ''' Permet d'obtenir la version XML valide SpreadSheet.
    ''' </summary>
    ''' <returns>Un XML.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirXML() As String
        Dim balise As String = ""
        balise &= "<Data"
        balise &= TsCuOutilsExcel.ConstruireAttribut("ss:Type", Type)
        balise &= TsCuOutilsExcel.ConstruireAttributOptionnel("x:Ticked", Ticked)
        If String.IsNullOrEmpty(mContenue) Then
            balise &= "/>"
        Else
            balise &= ">"
        End If

        balise &= mContenue

        If String.IsNullOrEmpty(mContenue) = False Then
            balise &= "</Data>"
        End If
        Return balise
    End Function

#End Region

End Class