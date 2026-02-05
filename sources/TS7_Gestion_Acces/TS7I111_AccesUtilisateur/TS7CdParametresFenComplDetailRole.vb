<Serializable()> _
Public Class TS7CdParametresFenComplDetailRole

#Region "--- Variables membres ---"

    Private mTitreBteMesage As String
    Private mNomBoutonAppelant As String
    Private mintNoEtapeCommentaire As Integer
    Private mVariablesRemplacements() As String
    Private mValeurDetailRole As String
    Private mBouton1 As TS7CdBouton
    Private mBouton2 As TS7CdBouton

#End Region

#Region "--- constructeur ---"

    Public Sub New()

    End Sub

#End Region

#Region "--- Propriétés publiques ---"

    Public Property TitreBteMesage() As String
        Get
            Return mTitreBteMesage
        End Get
        Set(ByVal Value As String)
            mTitreBteMesage = Value
        End Set
    End Property

    Public Property NomBoutonAppelant() As String
        Get
            Return mNomBoutonAppelant
        End Get
        Set(ByVal Value As String)
            mNomBoutonAppelant = Value
        End Set
    End Property

    Public Property ValeurDetailRole() As String
        Get
            Return mValeurDetailRole
        End Get
        Set(ByVal Value As String)
            mValeurDetailRole = Value
        End Set
    End Property

    Public Property NoEtapeCommentaire() As Integer
        Get
            Return mintNoEtapeCommentaire
        End Get
        Set(ByVal Value As Integer)
            mintNoEtapeCommentaire = Value
        End Set
    End Property

    Public Property Bouton1() As TS7CdBouton
        Get
            Return mBouton1
        End Get
        Set(ByVal Value As TS7CdBouton)
            mBouton1 = Value
        End Set
    End Property


    Public Property Bouton2() As TS7CdBouton
        Get
            Return mBouton2
        End Get
        Set(ByVal Value As TS7CdBouton)
            mBouton2 = Value
        End Set
    End Property

#End Region

#Region "--- Méthodes publiques ---"

    Public Function ObtenirBouton(ByVal pindex As Short) As TS7CdBouton

        ' Est-ce que le bouton1 existe et que son étiquette correspond à pEtiquette
        If Not (Bouton1 Is Nothing) AndAlso _
               (pindex = 0) Then

            Return Bouton1

        ElseIf Not (Bouton2 Is Nothing) AndAlso _
               (pindex = 1) Then

            Return Bouton2

        End If
        Return Nothing
    End Function


#End Region

    <Serializable()> _
    Public Class TS7CdBouton

#Region "--- Variables membres ---"

        Private mTexteBouton As String
        Private mValeur As String
        Private mInvisible As Boolean
        Private mEstBoutonAnnulation As Boolean
        Private mEstBoutonDefaut As Boolean

#End Region

#Region "--- constructeur ---"

        Public Sub New()

        End Sub

        Public Sub New(ByVal pTexteBouton As String, _
                       ByVal pValeur As String, _
                       Optional ByVal pEstBoutonAnnulation As Boolean = False, _
                       Optional ByVal pEstBoutonDefaut As Boolean = False)

            TexteBouton = pTexteBouton
            Valeur = pValeur
            EstBoutonAnnulation = pEstBoutonAnnulation
            EstBoutonDefaut = pEstBoutonDefaut
        End Sub

#End Region

#Region "--- Propriétés publiques ---"

        Public Property TexteBouton() As String
            Get
                Return mTexteBouton
            End Get
            Set(ByVal Value As String)
                mTexteBouton = Value
            End Set
        End Property

        Public Property Valeur() As String
            Get
                Return mValeur
            End Get
            Set(ByVal Value As String)
                mValeur = Value
            End Set
        End Property

        Public Property Invisible() As Boolean
            Get
                Return mInvisible
            End Get
            Set(ByVal Value As Boolean)
                mInvisible = Value
            End Set
        End Property

        Public Property EstBoutonAnnulation() As Boolean
            Get
                Return mEstBoutonAnnulation
            End Get
            Set(ByVal Value As Boolean)
                mEstBoutonAnnulation = Value
            End Set
        End Property

        Public Property EstBoutonDefaut() As Boolean
            Get
                Return mEstBoutonDefaut
            End Get
            Set(ByVal Value As Boolean)
                mEstBoutonDefaut = Value
            End Set
        End Property

#End Region

    End Class
End Class

