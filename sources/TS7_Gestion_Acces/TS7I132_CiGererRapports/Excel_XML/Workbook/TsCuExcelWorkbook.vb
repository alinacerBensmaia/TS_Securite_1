
''' <summary>
''' Informations du Workbook spécifique à Excel.
''' </summary>
''' <remarks>Ce ne sont pas d'information relier à "XML Spreadsheet Reference".</remarks>
Public Class TsCuExcelWorkbook

#Region "--- Propriétés ---"

    Private mWindowHeight As Integer
    ''' <summary>
    ''' Hauteur de la fenêtre.
    ''' </summary>
    Public Property WindowHeight() As Integer
        Get
            Return mWindowHeight
        End Get
        Set(ByVal value As Integer)
            mWindowHeight = value
        End Set
    End Property

    Private mWindowWidth As Integer
    ''' <summary>
    ''' Largeur de la fenêtre.
    ''' </summary>
    Public Property WindowWidth() As Integer
        Get
            Return mWindowWidth
        End Get
        Set(ByVal value As Integer)
            mWindowWidth = value
        End Set
    End Property

    Private mWindowTopX As Integer
    ''' <summary>
    ''' Position en X de la fenêtre.
    ''' </summary>
    Public Property WindowTopX() As Integer
        Get
            Return mWindowTopX
        End Get
        Set(ByVal value As Integer)
            mWindowTopX = value
        End Set
    End Property

    Private mWindowTopY As Integer
    ''' <summary>
    ''' Position en Y de la fenêtre.
    ''' </summary>
    Public Property WindowTopY() As Integer
        Get
            Return mWindowTopY
        End Get
        Set(ByVal value As Integer)
            mWindowTopY = value
        End Set
    End Property

    Private mIteration As Integer?
    ''' <summary>
    ''' Nombre d'itération de la fenêtre.
    ''' </summary>
    Public Property Iteration() As Integer?
        Get
            Return mIteration
        End Get
        Set(ByVal value As Integer?)
            mIteration = value
        End Set
    End Property

    Private mProtectStructure As Boolean
    ''' <summary>
    ''' Structure protégé?
    ''' </summary>
    Public Property ProtectStructure() As Boolean
        Get
            Return mProtectStructure
        End Get
        Set(ByVal value As Boolean)
            mProtectStructure = value
        End Set
    End Property

    Private mProtectWindows As Boolean
    ''' <summary>
    ''' Fenêtre protégé?
    ''' </summary>
    Public Property ProtectWindows() As Boolean
        Get
            Return mProtectWindows
        End Get
        Set(ByVal value As Boolean)
            mProtectWindows = value
        End Set
    End Property

#End Region

#Region "--- Méthodes ---"

    ''' <summary>
    ''' Permet d'obtenir la valeur de l'objet en xml.
    ''' </summary>
    ''' <returns>Les xml de l'objet.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirXML() As String
        Dim balise = "<ExcelWorkbook " & ObtenirSchema() & ">"

        balise &= ConstructeurBalise("WindowHeight", WindowHeight)
        balise &= ConstructeurBalise("WindowWidth", WindowWidth)
        balise &= ConstructeurBalise("WindowTopX", WindowTopX)
        balise &= ConstructeurBalise("WindowTopY", WindowTopY)
        balise &= ConstructeurBalise("Iteration", Iteration)
        balise &= ConstructeurBalise("ProtectStructure", ProtectStructure)
        balise &= ConstructeurBalise("ProtectWindows", ProtectWindows)

        balise &= "</ExcelWorkbook>"
        Return balise
    End Function

#End Region

#Region "--- Fonctions privées ---"

    ''' <summary>
    ''' Permet d'obtenir les schémas relier à la balise. 
    ''' </summary>
    ''' <returns>Les schéma en version texte.</returns>
    ''' <remarks></remarks>
    Private Function ObtenirSchema() As String
        Dim q As String = """"
        Dim schema As String = String.Format("xmlns={0}urn:schemas-microsoft-com:office:excel{0}", q)

        Return schema
    End Function

    ''' <summary>
    ''' Permet de construire des balises formatées.
    ''' </summary>
    ''' <param name="pNomBalise">Le nom de la balise.</param>
    ''' <param name="pTexte">Le texte entre les balises.</param>
    ''' <returns>Texte formaté.</returns>
    ''' <remarks></remarks>
    Private Function ConstructeurBalise(ByVal pNomBalise As String, ByVal pTexte As String) As String
        Return String.Format(If(String.IsNullOrEmpty(pTexte), "<{0}/>", "<{0}>{1}</{0}>"), pNomBalise, pTexte)
    End Function

    ''' <summary>
    ''' Permet de construire des balises formatées.
    ''' </summary>
    ''' <param name="pNomBalise">Le nom de la balise.</param>
    ''' <param name="pTexte">Le texte entre les balises.</param>
    ''' <returns>Texte formaté.</returns>
    ''' <remarks></remarks>
    Private Function ConstructeurBalise(ByVal pNomBalise As String, ByVal pTexte As Nullable(Of Integer)) As String
        Return If(pTexte.HasValue, ConstructeurBalise(pNomBalise, pTexte.ToString), ConstructeurBalise(pNomBalise, ""))
    End Function

    ''' <summary>
    ''' Permet de construire des balises formatées.
    ''' </summary>
    ''' <param name="pNomBalise">Le nom de la balise.</param>
    ''' <param name="pChiffre">Le chiffre entre les balises.</param>
    ''' <returns>Texte formaté.</returns>
    ''' <remarks></remarks>
    Private Function ConstructeurBalise(ByVal pNomBalise As String, ByVal pChiffre As Integer) As String
        Return ConstructeurBalise(pNomBalise, pChiffre.ToString)
    End Function

    ''' <summary>
    ''' Permet de construire des balises formatées.
    ''' </summary>
    ''' <param name="pNomBalise">Le nom de la balise.</param>
    ''' <param name="pBooleen">Le booléen entre les balises.</param>
    ''' <returns>Texte formaté.</returns>
    ''' <remarks></remarks>
    Private Function ConstructeurBalise(ByVal pNomBalise As String, ByVal pBooleen As Boolean) As String
        Return ConstructeurBalise(pNomBalise, pBooleen.ToString)
    End Function

#End Region

End Class
