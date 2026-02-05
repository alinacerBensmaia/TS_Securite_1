Imports TS7I132_CiGererRapports.TsCuOutilsExcel

''' <summary>
''' Permet de prédédinir un style qui pourra être appliqué à une cellule,
'''  une colonne ou une ligne.
''' </summary>
''' <remarks></remarks>
Public Class TsCuStyle

#Region "--- Propriétés ---"

    'ss:ID
    Private mID As String
    ''' <summary>
    ''' Identifiant du Style.
    ''' </summary>
    Public ReadOnly Property ID() As String
        Get
            Return mID
        End Get
    End Property

    'ss:Name
    Private mName As String = Nothing
    ''' <summary>
    ''' Nom du Style.
    ''' </summary>
    Public Property Name() As String
        Get
            Return mName
        End Get
        Set(ByVal value As String)
            mName = value
        End Set
    End Property

    'ss:Parent
    Private mParent As String = Nothing
    ''' <summary>
    ''' Nom du Style.
    ''' </summary>
    Public Property Parent() As String
        Get
            Return mParent
        End Get
        Set(ByVal value As String)
            mParent = value
        End Set
    End Property

    Private mAlignment As TsCuStyleAlignment
    ''' <summary>
    ''' Définit l'alignement du texte du Style.
    ''' </summary>
    Public Property Alignment() As TsCuStyleAlignment
        Get
            If mAlignment Is Nothing Then mAlignment = New TsCuStyleAlignment
            Return mAlignment
        End Get
        Set(ByVal value As TsCuStyleAlignment)
            mAlignment = value
        End Set
    End Property

    Private mBorders As List(Of TsCuStyleBorder)
    ''' <summary>
    ''' Définit les bordures du style.
    ''' </summary>
    Public Property Borders() As List(Of TsCuStyleBorder)
        Get
            If mBorders Is Nothing Then mBorders = New List(Of TsCuStyleBorder)
            Return mBorders
        End Get
        Set(ByVal value As List(Of TsCuStyleBorder))
            mBorders = value
        End Set
    End Property

    Private mFont As TsCuStyleFont
    ''' <summary>
    ''' Définit le format du texte du Style.
    ''' </summary>
    Public Property Font() As TsCuStyleFont
        Get
            If mFont Is Nothing Then mFont = New TsCuStyleFont
            Return mFont
        End Get
        Set(ByVal value As TsCuStyleFont)
            mFont = value
        End Set
    End Property

    Private mInterior As TsCuStyleInterior
    ''' <summary>
    ''' Définit le fond des cellules du Style.
    ''' </summary>
    Public Property Interior() As TsCuStyleInterior
        Get
            If mInterior Is Nothing Then mInterior = New TsCuStyleInterior
            Return mInterior
        End Get
        Set(ByVal value As TsCuStyleInterior)
            mInterior = value
        End Set
    End Property

    Private mNumberFormat As TsCuStyleNumberFormat
    ''' <summary>
    ''' Définit le format des cellules du Style.
    ''' </summary>
    Public Property NumberFormat() As TsCuStyleNumberFormat
        Get
            If mNumberFormat Is Nothing Then mNumberFormat = New TsCuStyleNumberFormat
            Return mNumberFormat
        End Get
        Set(ByVal value As TsCuStyleNumberFormat)
            mNumberFormat = value
        End Set
    End Property

    Private mProtection As TsCuStyleProtection
    ''' <summary>
    ''' Définit le niveau de protection des cellules du Style.
    ''' </summary>
    Public Property Protection() As TsCuStyleProtection
        Get
            If mProtection Is Nothing Then mProtection = New TsCuStyleProtection
            Return mProtection
        End Get
        Set(ByVal value As TsCuStyleProtection)
            mProtection = value
        End Set
    End Property

#End Region

#Region "--- Constructeurs ---"

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <param name="pID">L'identifiant du Style.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal pID As String)
        mID = pID
    End Sub

#End Region

#Region "--- Méthodes ---"

    ''' <summary>
    ''' Permet d'obtenir la version XML valide SpreadSheet du Style.
    ''' </summary>
    ''' <returns>Un XML.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirXML() As String
        Dim balise As String = ""

        balise &= "<Style"
        balise &= ConstruireAttribut("ss:ID", ID)
        balise &= ConstruireAttributOptionnel("ss:Name", Name)
        balise &= ConstruireAttributOptionnel("ss:Parent", Parent)
        balise &= ">"

        If mBorders IsNot Nothing Then
            If Borders.Count = 0 Then
                balise &= "<Borders/>"
            Else
                balise &= "<Borders>"
                For Each b In Borders
                    balise &= b.ObtenirXML()
                Next
                balise &= "</Borders>"
            End If
        End If

        If mAlignment IsNot Nothing Then balise &= Alignment.ObtenirXML()
        If mFont IsNot Nothing Then balise &= Font.ObtenirXML()
        If mInterior IsNot Nothing Then balise &= Interior.ObtenirXML()
        If mNumberFormat IsNot Nothing Then balise &= NumberFormat.ObtenirXML()
        If mProtection IsNot Nothing Then balise &= Protection.ObtenirXML()

        balise &= "</Style>"

        Return balise
    End Function

#End Region

End Class

