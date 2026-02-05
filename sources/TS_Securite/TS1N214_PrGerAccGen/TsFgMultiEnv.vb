Imports Rrq.CS.ServicesCommuns.ScenarioTransactionnel
Imports System.Collections.Generic
Imports System.Globalization
Imports TS1N214_PrGerAccGen.TsCuConversionsTypes

'''-----------------------------------------------------------------------------
''' Project		: $safeprojectname$
''' Class		: TsFgMultiEnv
'''
'''-----------------------------------------------------------------------------
''' <summary>
''' Formulaire de dialogue permettant à l'utilisateur de définir pour chaque environnement:
'''  1- Code (Usager AD)
'''  2- Mot de passe
'''  3- Profil (Groupe AD)
''' </summary>
''' <remarks></remarks>
'''-----------------------------------------------------------------------------
Friend Class TsFgMultiEnv
    Private objControlInvalide As Object
    Private msgErreur As String
    Private mEnvErreur As String
    Private mEnvPrecedent As String = String.Empty
    Private mIndicBloqueEvenements As Boolean = True
    Private mDtEnv As DataTable
    Private mIndNiveau1 As Boolean

    Private Property IndicBloqueEvenements() As Boolean
        Get
            Return mIndicBloqueEvenements
        End Get
        Set(ByVal value As Boolean)
            mIndicBloqueEvenements = value
        End Set
    End Property

    ''' <summary>
    ''' Data table environnement
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DtEnv() As DataTable
        Get
            Return mDtEnv
        End Get
        Set(ByVal value As DataTable)
            mDtEnv = value
        End Set
    End Property

    ''' <summary>
    ''' Indicateur d'accessibilité de niveau 1
    ''' </summary>
    ''' <param name="indNiveau1"></param>
    Public Sub New(ByVal indNiveau1 As Boolean)
        ' Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        mDtEnv = New DataTable
        mIndNiveau1 = indNiveau1
        txtMdp.Verrouiller = Not mIndNiveau1
        cmdMontrerPassword.Enabled = mIndNiveau1
    End Sub

    Private Sub TsFgMultiEnv_Load(ByVal sender As Object,
                                  ByVal e As System.EventArgs) Handles Me.Load
        IndicBloqueEvenements = True
        RemplirCboEnv()
        cboEnv.SelectedIndex = 0
        If cboEnv.SelectedValue IsNot Nothing Then
            mEnvPrecedent = cboEnv.SelectedValue.ToString
        End If
        AfficherEnv()
        IndicBloqueEvenements = False
    End Sub

    ''' <summary>
    ''' Remplir la liste de séléction de l'environnement
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RemplirCboEnv()
        Dim cle As TsCuCle

        DtEnv.Locale = CultureInfo.InvariantCulture
        DtEnv.Columns.Add("CodeEnv")
        DtEnv.Columns.Add("DescEnv")

        For Each cle In TsFdGerAccGen.GroupeCle.ListeCle.Values
            If cle IsNot Nothing Then
                DtEnv.Rows.Add(cle.CodeEnv, cle.DescEnv)
            End If
        Next

        cboEnv.DataSource = DtEnv
        cboEnv.ValueMember = "CodeEnv"
        cboEnv.DisplayMember = "DescEnv"

    End Sub

    Private Sub cboEnv_SelectedIndexChanged(ByVal sender As System.Object, _
                                            ByVal e As System.EventArgs) Handles cboEnv.SelectedIndexChanged
        If Not IndicBloqueEvenements Then
            IndicBloqueEvenements = True
            MajEnvironnement(mEnvPrecedent)
            AfficherEnv()
            IndicBloqueEvenements = False
        End If
    End Sub

    Private Sub MajEnvironnement(ByVal pEnv As String)
        Dim cle As TsCuCle

        If TsFdGerAccGen.GroupeCle.ListeCle.ContainsKey(pEnv) Then
            cle = TsFdGerAccGen.GroupeCle.ListeCle(pEnv)
            cle.CodeUsagerAd = txtCode.Text
            cle.Mdp = txtMdp.Text
            cle.GroupeAd = txtProfil.Text
        End If
    End Sub

    Private Sub AfficherEnv()
        Dim env As String = cboEnv.SelectedValue.ToString
        Dim cle As TsCuCle

        If TsFdGerAccGen.GroupeCle.ListeCle.ContainsKey(env) Then
            cle = TsFdGerAccGen.GroupeCle.ListeCle(env)

            txtCode.Text = cle.CodeUsagerAd
            txtMdp.Text = cle.Mdp
            txtProfil.Text = cle.GroupeAd
        Else
            txtCode.Text = String.Empty
            txtMdp.Text = String.Empty
            txtProfil.Text = String.Empty
        End If
        mEnvPrecedent = env
    End Sub

    Private Sub cmdProfil_Click(ByVal sender As System.Object,
                                ByVal e As System.EventArgs) Handles cmdProfil.Click
        Using objSelectAD As New TsFfSelectAD()

            objSelectAD.ProfilSelect = txtProfil.Text.Split(","c).Select(Function(x) x.Trim()).ToList()
            If objSelectAD.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                txtProfil.Text = String.Empty
                For Each item As String In objSelectAD.lstProfil.SelectedItems
                    If txtProfil.Text.Length > 0 Then
                        txtProfil.Text &= ", "
                    End If
                    txtProfil.Text &= item
                Next
            End If
        End Using
    End Sub

    Private Sub cmdOk_Click(ByVal sender As System.Object, _
                            ByVal e As System.EventArgs) Handles cmdOk.Click
        Dim IndicBloque As Boolean = IndicBloqueEvenements

        IndicBloqueEvenements = True
        If cboEnv.SelectedValue IsNot Nothing Then
            MajEnvironnement(cboEnv.SelectedValue.ToString)
        End If

        If Valider() Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
        Else
            cboEnv.SelectedValue = mEnvErreur
            AfficherEnv()
            MessageBox.Show(msgErreur, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
            If TypeOf objControlInvalide Is TextBox Then
                DirectCast(objControlInvalide, TextBox).Focus()
            End If
            Me.DialogResult = Windows.Forms.DialogResult.None
        End If
        IndicBloqueEvenements = IndicBloque
    End Sub

    Private Function Valider() As Boolean
        Dim cle As TsCuCle

        For Each cle In TsFdGerAccGen.GroupeCle.ListeCle.Values

            'S07e Un code, mot de passe et profil sont requis pour chaque environnement
            If String.IsNullOrEmpty(GetString(cle.CodeUsagerAd)) AndAlso
               String.IsNullOrEmpty(GetString(cle.Mdp)) AndAlso
               String.IsNullOrEmpty(GetString(cle.GroupeAd)) Then

                objControlInvalide = txtCode
                msgErreur = My.Resources.TS12107E
                mEnvErreur = cle.CodeEnv
                Return False
            End If

            'S13e Le code de la clé est obligatoire pour tous les environnements
            If String.IsNullOrEmpty(GetString(cle.CodeUsagerAd)) Then
                objControlInvalide = txtCode
                msgErreur = My.Resources.TS12113E
                mEnvErreur = cle.CodeEnv
                Return False
            End If

            'S14e Le mot de passe de la clé est obligatoire pour tous les environnements
            If String.IsNullOrEmpty(GetString(cle.Mdp)) Then
                objControlInvalide = txtMdp
                msgErreur = My.Resources.TS12114E
                mEnvErreur = cle.CodeEnv
                Return False
            End If

            'S05e Il faut choisir un profil qui existe dans l'AD (Groupe AD)
            If Not TsCuPrGerAccGen.ValiderGroupes(cle.GroupeAd) Then
                objControlInvalide = txtProfil
                msgErreur = My.Resources.TS12105E
                mEnvErreur = cle.CodeEnv
                Return False
            End If

            'S15e Au moins un profil doit être associé à la clé pour tous les environnements
            If String.IsNullOrEmpty(GetString(cle.GroupeAd)) Then
                objControlInvalide = txtProfil
                msgErreur = My.Resources.TS12115E
                mEnvErreur = cle.CodeEnv
                Return False
            Else
                If cle.CodeEnv = "PROD" AndAlso _
                   (cle.GroupeAd.Length < 4 OrElse cle.GroupeAd.Substring(3, 1) <> "P") Then
                    objControlInvalide = txtProfil
                    msgErreur = String.Format(My.Resources.TS12106E, My.Resources.EnvDescProd)
                    mEnvErreur = cle.CodeEnv
                    Return False
                End If

                If cle.CodeEnv = "SIML" AndAlso _
                   (cle.GroupeAd.Length < 4 OrElse cle.GroupeAd.Substring(3, 1) <> "S") Then
                    objControlInvalide = txtProfil
                    msgErreur = String.Format(My.Resources.TS12106E, My.Resources.EnvDescSimu)
                    mEnvErreur = cle.CodeEnv
                    Return False
                End If
            End If
        Next

        Return True
    End Function

    Private Sub cmdMontrerPassword_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdMontrerPassword.Click
        txtMdp.UseSystemPasswordChar = Not txtMdp.UseSystemPasswordChar
    End Sub

    Private Sub cmdGenererMdp_Click(sender As System.Object, e As System.EventArgs) Handles cmdGenererMdp.Click


        Dim motDePasse As String = TsFfGenMotPasse.AfficherGenererMotPasse(Me, TsFdGerAccGen.txtMdp.MaxLength)

        If Not String.IsNullOrEmpty(motDePasse) Then
            txtMdp.Text = motDePasse
        End If

    End Sub
End Class