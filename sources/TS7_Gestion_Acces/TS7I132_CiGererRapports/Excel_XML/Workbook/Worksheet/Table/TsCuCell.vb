Imports TS7I132_CiGererRapports.TsCuOutilsExcel

''' <summary>
''' Permet de définir une cellule dans un ligne.
''' </summary>
''' <remarks></remarks>
Public Class TsCuCell

#Region "--- Propriétés ---"

    'c:PasteFormula
    Private mPasteFormula As String = Nothing
    ''' <summary>
    ''' Formule copier provenant d'une autre cellule.
    ''' </summary>
    Public Property PasteFormula() As String
        Get
            Return mPasteFormula
        End Get
        Set(ByVal value As String)
            mPasteFormula = value
        End Set
    End Property

    'ss:ArrayRange
    Private mArrayRange As String = Nothing
    ''' <summary>
    ''' Indique quel est la zone de cellules sur laquelle la formule influence.
    ''' </summary>
    Public Property ArrayRange() As String
        Get
            Return mArrayRange
        End Get
        Set(ByVal value As String)
            mArrayRange = value
        End Set
    End Property

    'ss:Formula
    Private mFormula As String = Nothing
    ''' <summary>
    ''' La formule entreposée dans la cellule.
    ''' </summary>
    Public Property Formula() As String
        Get
            Return mFormula
        End Get
        Set(ByVal value As String)
            mFormula = value
        End Set
    End Property

    'ss:HRef
    Private mHRef As String = Nothing
    ''' <summary>
    ''' Lien hypertexte relié à la cellule.
    ''' </summary>
    Public Property HRef() As String
        Get
            Return mHRef
        End Get
        Set(ByVal value As String)
            mHRef = value
        End Set
    End Property

    'ss:Index
    Private mIndex As ULong?
    ''' <summary>
    ''' Indique la position de la cellule dans la ligne.
    ''' </summary>
    Public Property Index() As ULong?
        Get
            Return mIndex
        End Get
        Set(ByVal value As ULong?)
            mIndex = value
        End Set
    End Property

    'ss:MergeAcross
    Private mMergeAcross As ULong?
    ''' <summary>
    ''' Spécifie sur combien de cellules la cellule s'étend sur la ligne.
    ''' </summary>
    Public Property MergeAcross() As ULong?
        Get
            Return mMergeAcross
        End Get
        Set(ByVal value As ULong?)
            mMergeAcross = value
        End Set
    End Property

    'ss:MergeDown
    Private mMergeDown As ULong?
    ''' <summary>
    ''' Spécifie sur combien de cellules la cellule s'étend sur la colonne.
    ''' </summary>
    Public Property MergeDown() As ULong?
        Get
            Return mMergeDown
        End Get
        Set(ByVal value As ULong?)
            mMergeDown = value
        End Set
    End Property

    'ss:StyleID
    Private mStyleID As String = Nothing
    ''' <summary>
    ''' Indique quel style sera attribué à la cellule.
    ''' </summary>
    Public Property StyleID() As String
        Get
            Return mStyleID
        End Get
        Set(ByVal value As String)
            mStyleID = value
        End Set
    End Property

    'x:HRefScreenTip
    Private mHRefScreenTip As String = Nothing
    ''' <summary>
    ''' Texte à affichier en survole de la cellule.
    ''' </summary>
    Public Property HRefScreenTip() As String
        Get
            Return mHRefScreenTip
        End Get
        Set(ByVal value As String)
            mHRefScreenTip = value
        End Set
    End Property

    'ss:Data
    Private mData As TsCuData
    ''' <summary>
    ''' Le contenu de la cellule.
    ''' </summary>
    Public Property Data() As TsCuData
        Get
            Return mData
        End Get
        Set(ByVal value As TsCuData)
            mData = value
        End Set
    End Property

    'ss:NamedCell
    Private mNamedCells As New List(Of TsCuNamedCell)
    ''' <summary>
    ''' Indique si cette cellule fait partie du NamedRange de l'utilisateur.
    ''' </summary>
    Public Property NamedCells() As List(Of TsCuNamedCell)
        Get
            Return mNamedCells
        End Get
        Set(ByVal value As List(Of TsCuNamedCell))
            mNamedCells = value
        End Set
    End Property

#End Region

#Region "--- Méthodes ---"

    ''' <summary>
    ''' Permet d'obtenir la version XML valide SpreadSheet.
    ''' </summary>
    ''' <returns>Un XML.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirXML() As String
        Dim balise As String = ""
        balise &= "<Cell"
        balise &= ConstruireAttributOptionnel("c:PasteFormula", PasteFormula)
        balise &= ConstruireAttributOptionnel("ss:ArrayRange", ArrayRange)
        balise &= ConstruireAttributOptionnel("ss:Formula", Formula)
        balise &= ConstruireAttributOptionnel("ss:HRef", HRef)
        balise &= ConstruireAttributOptionnel("ss:Index", Index)
        balise &= ConstruireAttributOptionnel("ss:MergeAcross", MergeAcross)
        balise &= ConstruireAttributOptionnel("ss:MergeDown", MergeDown)
        balise &= ConstruireAttributOptionnel("ss:StyleID", StyleID)
        balise &= ConstruireAttributOptionnel("x:HRefScreenTip", HRefScreenTip)
        If NamedCells.Count = 0 And Data Is Nothing Then
            balise &= "/>"
        Else
            balise &= ">"
        End If

        If Data IsNot Nothing Then
            balise &= Data.ObtenirXML()
        End If

        For Each nc In NamedCells
            balise &= nc.ObtenirXML()
        Next

        If NamedCells.Count <> 0 Or Data IsNot Nothing Then
            balise &= "</Cell>"
        End If
        Return balise
    End Function

#End Region

End Class