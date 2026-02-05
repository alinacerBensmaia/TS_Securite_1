Public Class TsFfSelectAD
    Public Sub New()
        MyBase.New()

        'Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        'Ajoutez une initialisation quelconque après l'appel InitializeComponent()
        mProfileSelect = New Generic.List(Of String)()
        mTousProfils = New Generic.List(Of String)()
    End Sub



#Region " Variables privées "
    Private mProfileSelect As Generic.IList(Of String)
    Private mTousProfils As Generic.IList(Of String)
    Private mOldIndex As Integer = -1
#End Region

#Region " Propriétés "
    Public Property ProfilSelect() As Generic.IList(Of String)
        Get
            Return mProfileSelect
        End Get
        Set(ByVal value As Generic.IList(Of String))
            mProfileSelect = value
        End Set
    End Property

    Public Property TousProfils() As Generic.IList(Of String)
        Get
            Return mTousProfils
        End Get
        Set(ByVal value As Generic.IList(Of String))
            mTousProfils = value
        End Set
    End Property

    Private Property OldIndex() As Integer
        Get
            Return mOldIndex
        End Get
        Set(ByVal value As Integer)
            mOldIndex = value
        End Set
    End Property
#End Region

#Region " Méthodes privées "
    Private Sub ChargerListe()

        lstProfil.Items.Clear()
        TousProfils.Clear()

        ' Rechercher les profils dans l'AD
        Dim securite As New TsCuSecuriteApplicative()
        TousProfils = securite.ObtenirTousLesProfils().OrderBy(Function(x) x).ToList()

        Dim item As String
        For Each item In TousProfils
            lstProfil.Items.Add(item)
        Next

        Dim pro As String
        For Each pro In ProfilSelect
            Dim strToCompare As String = pro
            item = TousProfils.FirstOrDefault(Function(x) String.Compare(x, strToCompare, Globalization.CultureInfo.CurrentCulture, Globalization.CompareOptions.IgnoreCase) = 0)
            Dim index As Integer = TousProfils.IndexOf(item)
            lstProfil.SelectedIndices.Add(index)
        Next
    End Sub
#End Region

#Region " Méthodes du formulaire "
    Private Sub TsFfSelectAD_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lstProfil.SelectedIndex = -1
        txtProfil.Text = ""
        txtProfil.Focus()

        ChargerListe()
    End Sub

    Private Sub cmdSelectionner_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSelectionner.Click
        Me.Close()
    End Sub

    Private Sub cmdRafraichir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRafraichir.Click
        ChargerListe()
    End Sub

    Private Sub cmdAnnuler_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAnnuler.Click
        Me.Close()
    End Sub

    Private Sub lstProfil_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstProfil.DoubleClick
        cmdSelectionner.PerformClick()
    End Sub

    Private Sub txtProfil_TextChanged(ByVal sender As System.Object,
                                      ByVal e As System.EventArgs) Handles txtProfil.TextChanged
        Dim item As String = TousProfils.FirstOrDefault(Function(x) String.Compare(x, 0, txtProfil.Text, 0, txtProfil.Text.Length, Globalization.CultureInfo.CurrentCulture, Globalization.CompareOptions.IgnoreCase) = 0)
        Dim index As Integer = TousProfils.IndexOf(item)


        If OldIndex > -1 Then
            lstProfil.SelectedIndices.Remove(OldIndex)
        End If
        OldIndex = index
        If index > -1 Then
            lstProfil.SelectedIndices.Add(index)
        End If
    End Sub
#End Region
End Class
