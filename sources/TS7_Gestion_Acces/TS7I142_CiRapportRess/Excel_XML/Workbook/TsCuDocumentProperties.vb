
''' <summary>
''' Propriétés du document.
''' </summary>
''' <remarks>Ce ne sont pas d'information relier à "XML Spreadsheet Reference".</remarks>
Public Class TsCuDocumentProperties

#Region "--- Propriété ---"

    Private mAuthor As String
    ''' <summary>
    ''' L'auteur original du document.
    ''' </summary>
    Public Property Author() As String
        Get
            Return mAuthor
        End Get
        Set(ByVal value As String)
            mAuthor = value
        End Set
    End Property

    Private mLastAuthor As String
    ''' <summary>
    ''' Dernier auteur du document.
    ''' </summary>
    Public Property LastAuthor() As String
        Get
            Return mLastAuthor
        End Get
        Set(ByVal value As String)
            mLastAuthor = value
        End Set
    End Property

    Private mLastPrinted As String
    ''' <summary>
    ''' Dernière date à laquelle le document a été imprimer.
    ''' </summary>
    Public Property LastPrinted() As String
        Get
            Return mLastPrinted
        End Get
        Set(ByVal value As String)
            mLastPrinted = value
        End Set
    End Property

    Private mCreated As String
    ''' <summary>
    ''' Date à laquelle le document a été créé.
    ''' </summary>
    Public Property Created() As String
        Get
            Return mCreated
        End Get
        Set(ByVal value As String)
            mCreated = value
        End Set
    End Property

    Private mLastSaved As String
    ''' <summary>
    ''' Date à laquelle la dernière sauvegarde a été effectué.
    ''' </summary>
    Public Property LastSaved() As String
        Get
            Return mLastSaved
        End Get
        Set(ByVal value As String)
            mLastSaved = value
        End Set
    End Property

    Private mCompany As String
    ''' <summary>
    ''' Compagnie pour laquelle ce document à été produit.
    ''' </summary>
    Public Property Company() As String
        Get
            Return mCompany
        End Get
        Set(ByVal value As String)
            mCompany = value
        End Set
    End Property

    Private mVersion As String
    ''' <summary>
    ''' Version du document.
    ''' </summary>
    Public Property Version() As String
        Get
            Return mVersion
        End Get
        Set(ByVal value As String)
            mVersion = value
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
        Dim balise = "<DocumentProperties " & ObtenirSchema() & ">"

        balise &= ConstruireBalise("Autor", Author)
        balise &= ConstruireBalise("LastAuthor", LastAuthor)
        balise &= ConstruireBalise("LastPrinted", LastPrinted)
        balise &= ConstruireBalise("Created", Created)
        balise &= ConstruireBalise("LastSaved", LastSaved)
        balise &= ConstruireBalise("Company", Company)
        balise &= ConstruireBalise("Version", Version)

        balise &= "</DocumentProperties>"
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
        Dim schema As String = String.Format("xmlns={0}urn:schemas-microsoft-com:office:office{0}", q)

        Return schema
    End Function

    ''' <summary>
    ''' Permet de construire des balises formatées.
    ''' </summary>
    ''' <param name="pNomBalise">Le nom de la balise.</param>
    ''' <param name="pTexte">Le texte entre les balises.</param>
    ''' <returns>Texte formaté.</returns>
    ''' <remarks></remarks>
    Private Function ConstruireBalise(ByVal pNomBalise As String, ByVal pTexte As String) As String
        Return String.Format(If(String.IsNullOrEmpty(pTexte), "<{0}/>", "<{0}>{1}</{0}>"), pNomBalise, pTexte)
    End Function

#End Region

End Class
