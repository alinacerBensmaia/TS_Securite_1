Imports CTRL = Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ
Imports RIS = Rrq.InfrastructureCommune.ScenarioTransactionnel
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Text
Imports TS1N214_PrGerAccGen.TsCuConversionsTypes
Imports TS1N215_INiveauSecrt2
Imports Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ.Grilles
Imports System.Runtime.CompilerServices
Imports TS1N201_DtCdAccGenV1

''' <summary>
''' Classe utilitaire de la couche présentation de la gestion des clé symbolique
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class TsCuPrGerAccGen

    Private Sub New()
        'Classe de méthodes statique non instantiable
    End Sub

    ''' <summary>
    ''' Remplir combo TypeCle
    ''' </summary>
    ''' <remarks></remarks>
    Friend Shared Sub RemplirCboTypeCle(ByRef pCboType As CTRL.XzCrComboBox,
                                        ByVal pIndicTous As Boolean)
        Dim dtbTypeCle As DataTable = Nothing

        dtbTypeCle = RemplirTableTypeCle(pIndicTous)
        pCboType.DataSource = dtbTypeCle
        pCboType.ValueMember = "CodeType"
        pCboType.DisplayMember = "Desc"
    End Sub

    ''' <summary>
    ''' Remplir combo d'environnements avec l'élément "Tous"
    ''' </summary>
    ''' <remarks></remarks>
    Friend Shared Sub RemplirCboEnvironnement(ByRef pCboEnv As CTRL.XzCrComboBox)
        pCboEnv.DataSource = TableEnv.Copy()
        pCboEnv.ValueMember = "CodeEnv"
        pCboEnv.DisplayMember = "Desc"
    End Sub

    ''' <summary>
    ''' Remplir combo d'environnements
    ''' </summary>
    ''' <remarks></remarks>
    Friend Shared Sub RemplirCboEnvironnementUnique(ByRef pCboEnv As CTRL.XzCrComboBox)
        pCboEnv.DataSource = TableEnvUnique
        pCboEnv.ValueMember = "CodeEnv"
        pCboEnv.DisplayMember = "Desc"
    End Sub
    Friend Shared Function RemplirTableTypeCle(ByVal pIndicTous As Boolean) As DataTable
        Dim dtb As New DataTable

        dtb.Locale = CultureInfo.InvariantCulture
        dtb.Columns.Add("CodeType")
        dtb.Columns.Add("Desc")

        If pIndicTous Then
            dtb.Rows.Add(My.Resources.EnvDescTous, My.Resources.EnvDescTous)
        End If
        dtb.Rows.Add(My.Resources.TypeCleCodeDom, My.Resources.TypeCleDescDom)   'Domaine
        dtb.Rows.Add(My.Resources.TypeCleCodeHors, My.Resources.TypeCleDescHors) 'Hors Domaine
        dtb.Rows.Add(My.Resources.TypeCleCodeInforte, My.Resources.TypeCleDescInforte) 'Inforoute
        dtb.Rows.Add(My.Resources.TypeCleCodeInforteVerif, My.Resources.TypeCleDescInforteVerif) 'Inforoute avec vérification

        Return dtb
    End Function

    Friend Shared Function EstTypeCleValide(ByVal pTypeCle As String) As Boolean

        Select Case pTypeCle
            Case My.Resources.TypeCleCodeDom, My.Resources.TypeCleCodeHors, My.Resources.TypeCleCodeInforte, My.Resources.TypeCleCodeInforteVerif
                Return True
        End Select

        Return False

    End Function

    Private Shared mTableEnv As DataTable
    ''' <summary>
    ''' Contient la liste des environnements avec en plus un élément "Tous"
    ''' </summary>
    Friend Shared ReadOnly Property TableEnv() As DataTable
        Get
            Return genererTableEnvironnements(mTableEnv, False)
        End Get
    End Property


    Private Shared mTableEnvUnique As DataTable
    ''' <summary>
    ''' Contient la liste des environnements
    ''' </summary>
    Friend Shared ReadOnly Property TableEnvUnique() As DataTable
        Get
            Return genererTableEnvironnements(mTableEnvUnique, True)
        End Get
    End Property

    Private Shared Function genererTableEnvironnements(ByRef table As DataTable, unique As Boolean) As DataTable
        If table Is Nothing Then
            table = New DataTable()
            With table
                .Locale = CultureInfo.InvariantCulture
                .Columns.Add("CodeEnv")
                .Columns.Add("Desc")
                .Columns.Add("TypeEnv")

                If Not unique Then
                    .Rows.Add(My.Resources.EnvCodeTous, My.Resources.EnvDescTous, String.Empty)
                End If
                .Rows.Add(My.Resources.EnvCodeEssa, My.Resources.EnvDescEssa, "E")
                .Rows.Add(My.Resources.EnvCodeUnit, My.Resources.EnvDescUnit, "U")
                .Rows.Add(My.Resources.EnvCodeIntg, My.Resources.EnvDescIntg, "I")
                .Rows.Add(My.Resources.EnvCodeAccp, My.Resources.EnvDescAccp, "A")
                .Rows.Add(My.Resources.EnvCodeForA, My.Resources.EnvDescForA, "B")
                .Rows.Add(My.Resources.EnvCodeForP, My.Resources.EnvDescForP, "Q")
                .Rows.Add(My.Resources.EnvCodeProd, My.Resources.EnvDescProd, "P")
                .Rows.Add(My.Resources.EnvCodeSiml, My.Resources.EnvDescSimu, "S")
            End With
        End If

        Return table
    End Function

    Friend Shared Function CreerNomCles(ByVal nmCle As String,
                                  ByVal ssCles As DataTable) As String
        Dim strEnv As String = String.Empty
        Dim strCle As String
        For Each cle As DataRow In ssCles.Rows
            If strEnv.Length > 0 Then
                strEnv &= ", "
            Else
                strEnv &= "["
            End If
            strCle = DirectCast(cle(My.Resources.NomCle), String)
            strEnv &= strCle.Substring(strCle.Length - 1, 1).ToUpper
        Next
        strEnv &= "]"

        Return nmCle.Substring(0, nmCle.Length - 1) & strEnv
    End Function

    ''' <summary>
    ''' Obtenir la touche pour le systeme et le sous-systeme 
    ''' </summary>
    ''' <param name="pValue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function ObtenirTouche(ByVal pValue As String) As String
        If pValue.Length > 0 Then
            pValue = pValue.Replace("NumPad", String.Empty)
            pValue = pValue.Replace("D", String.Empty)
        End If

        Return pValue
    End Function

    ''' <summary>
    ''' Obtenir le code d'environnement (ex: Unit, Accp, Intg, etc.)
    ''' </summary>
    ''' <param name="pTypeEnv">Lettre du type d'environnement</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function ObtenirCodeEnv(ByVal pTypeEnv As String) As String
        Dim codeEnv As String = String.Empty
        Dim row As DataRow = Nothing

        For Each row In TableEnv.Rows
            If row("TypeEnv").ToString = pTypeEnv Then
                codeEnv = row("CodeEnv").ToString
                Exit For
            End If
        Next

        Return codeEnv
    End Function

    Friend Shared Function ObtenirVerrouEdition() As TsDtVerrou
        Dim CaAffaire As TS1N215_INiveauSecrt2.TsICompI
        Dim resultat As TsDtVerrou
        Dim contexte As Object = Nothing

        Using objAppel As New RIS.XuCuAppelerCompI(Of TS1N215_INiveauSecrt2.TsICompI)
            Dim chaineContexte As String = objAppel.PreparerAppel(contexte, ObtenirCodeEnv())

            CaAffaire = objAppel.CreerComposantIntegration(chaineContexte)
            resultat = CaAffaire.ObtenirVerrouEdition(chaineContexte)
            objAppel.AnalyserRetour(chaineContexte, Nothing)
        End Using

        Return resultat
    End Function

    Friend Shared Sub RelacherVerrouEdition()
        Dim CaAffaire As TS1N215_INiveauSecrt2.TsICompI
        Dim contexte As Object = Nothing

        Using objAppel As New RIS.XuCuAppelerCompI(Of TS1N215_INiveauSecrt2.TsICompI)
            Dim chaineContexte As String = objAppel.PreparerAppel(contexte, ObtenirCodeEnv())

            CaAffaire = objAppel.CreerComposantIntegration(chaineContexte)
            CaAffaire.RelacherVerrouEdition(chaineContexte)
            objAppel.AnalyserRetour(chaineContexte, Nothing)
        End Using
    End Sub

    Friend Shared Function ObtenirEtatFichierExportation() As TS1N201_DtCdAccGenV1.TsDtEtaFicExp
        Dim CaAffaire As TS1N215_INiveauSecrt9.TsICompI
        Dim resultat As TS1N201_DtCdAccGenV1.TsDtEtaFicExp
        Dim contexte As Object = Nothing

        Using objAppel As New RIS.XuCuAppelerCompI(Of TS1N215_INiveauSecrt9.TsICompI)
            Dim chaineContexte As String = objAppel.PreparerAppel(contexte, ObtenirCodeEnv())

            CaAffaire = objAppel.CreerComposantIntegration(chaineContexte)
            resultat = CaAffaire.ObtenirEtatFichierExportation(chaineContexte)
            objAppel.AnalyserRetour(chaineContexte, Nothing)
        End Using

        Return resultat
    End Function

    Friend Shared Function ObtenirCles(ByVal pCleParent As String) As IList(Of TS1N201_DtCdAccGenV1.TsDtCleSym)
        Dim CaAffaire As TS1N215_INiveauSecrt9.TsICompI
        Dim resultat As IList(Of TS1N201_DtCdAccGenV1.TsDtCleSym)
        Dim contexte As Object = Nothing

        Using objAppel As New RIS.XuCuAppelerCompI(Of TS1N215_INiveauSecrt9.TsICompI)
            Dim chaineContexte As String = objAppel.PreparerAppel(contexte, ObtenirCodeEnv())

            CaAffaire = objAppel.CreerComposantIntegration(chaineContexte)
            resultat = CaAffaire.ObtenirCles(chaineContexte, pCleParent)
            objAppel.AnalyserRetour(chaineContexte, Nothing)
        End Using

        Return resultat
    End Function

    Friend Shared Function ObtenirListeCles() As IList(Of TS1N201_DtCdAccGenV1.TsDtCleSym)
        Dim CaAffaire As TS1N215_INiveauSecrt9.TsICompI
        Dim resultat As IList(Of TS1N201_DtCdAccGenV1.TsDtCleSym)
        Dim chaineContexte As String = String.Empty
        Dim contexte As Object = Nothing

        Using objAppel As New RIS.XuCuAppelerCompI(Of TS1N215_INiveauSecrt9.TsICompI)

            chaineContexte = objAppel.PreparerAppel(contexte, TsCuPrGerAccGen.ObtenirCodeEnv())

            CaAffaire = objAppel.CreerComposantIntegration(chaineContexte)
            resultat = CaAffaire.ObtenirListeCle(chaineContexte)
            objAppel.AnalyserRetour(chaineContexte, Nothing)
        End Using

        Return resultat
    End Function

    Friend Shared Function ObtenirCle(ByVal pCle As String) As TS1N201_DtCdAccGenV1.TsDtCleSym
        Dim CaAffaire As TS1N215_INiveauSecrt9.TsICompI
        Dim resultat As TS1N201_DtCdAccGenV1.TsDtCleSym
        Dim contexte As Object = Nothing

        Using objAppel As New RIS.XuCuAppelerCompI(Of TS1N215_INiveauSecrt9.TsICompI)
            Dim chaineContexte As String = objAppel.PreparerAppel(contexte, ObtenirCodeEnv())

            CaAffaire = objAppel.CreerComposantIntegration(chaineContexte)
            resultat = CaAffaire.ObtenirCle(chaineContexte, pCle)
            objAppel.AnalyserRetour(chaineContexte, Nothing)
        End Using

        Return resultat
    End Function


    Friend Shared Function ValiderGroupes(ByVal pGroupe As String) As Boolean
        If Not String.IsNullOrEmpty(GetString(pGroupe)) Then
            Dim securite As New TsCuSecuriteApplicative()
            Dim groupes() As String = pGroupe.Split({","c}, StringSplitOptions.RemoveEmptyEntries)

            Return securite.GroupesExistent(groupes)
        End If

        Return False
    End Function

    Friend Shared Function SauvegardeCle(ByVal pCle As TS1N201_DtCdAccGenV1.TsDtCleSym,
                                         ByVal pIndicMdpNouveau As Boolean,
                                         ByVal pIndicMaj As Boolean) As Boolean
        Dim CaAffaire As TS1N215_INiveauSecrt2.TsICompI
        Dim resultat As Boolean = True
        Dim contexte As Object = Nothing

        Using objAppel As New RIS.XuCuAppelerCompI(Of TS1N215_INiveauSecrt2.TsICompI)
            Dim chaineContexte As String = objAppel.PreparerAppel(contexte, ObtenirCodeEnv())

            CaAffaire = objAppel.CreerComposantIntegration(chaineContexte)
            resultat = CaAffaire.EnregistrerCle(chaineContexte, pCle, pIndicMdpNouveau, pIndicMaj)
            objAppel.AnalyserRetour(chaineContexte, Nothing)
        End Using

        Return resultat
    End Function

    Friend Shared Function ObtenirIndicateursCreationCompte() As TS1N201_DtCdAccGenV1.TsDtIndCreCpt
        Dim CaAffaire As TS1N215_INiveauSecrt9.TsICompI
        Dim resultat As TS1N201_DtCdAccGenV1.TsDtIndCreCpt
        Dim contexte As Object = Nothing

        Using objAppel As New RIS.XuCuAppelerCompI(Of TS1N215_INiveauSecrt9.TsICompI)
            Dim chaineContexte As String = objAppel.PreparerAppel(contexte, ObtenirCodeEnv())

            CaAffaire = objAppel.CreerComposantIntegration(chaineContexte)
            resultat = CaAffaire.ObtenirIndicateursCreationCompte(chaineContexte)
            objAppel.AnalyserRetour(chaineContexte, Nothing)
        End Using

        Return resultat
    End Function

    ''' <summary>
    ''' Afficher les erreurs de validation
    ''' </summary>
    ''' <param name="pRetValidation">TsCuRetValidation</param>
    ''' <remarks></remarks>
    Friend Shared Sub AfficherErreurValidation(ByVal pRetValidation As TsCuRetValidation)
        Dim erreurs As New StringBuilder

        For Each Err As String In pRetValidation.ListeErreur
            erreurs.Append("- ")
            erreurs.AppendLine(Err)
        Next

        MessageBox.Show(erreurs.ToString, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
        If pRetValidation.ControlInvalide IsNot Nothing Then
            If TypeOf pRetValidation.ControlInvalide Is TextBox Then
                DirectCast(pRetValidation.ControlInvalide, TextBox).Focus()
            ElseIf TypeOf pRetValidation.ControlInvalide Is ComboBox Then
                DirectCast(pRetValidation.ControlInvalide, ComboBox).Focus()
            ElseIf TypeOf pRetValidation.ControlInvalide Is Button Then
                DirectCast(pRetValidation.ControlInvalide, Button).Focus()
            End If
        End If
    End Sub

    Friend Shared Function SupprimerCle(ByVal CleADetruire As TS1N201_DtCdAccGenV1.TsDtCleSym) As Boolean
        Dim CaAffaire As TS1N215_INiveauSecrt2.TsICompI
        Dim resultat As Boolean = True
        Dim contexte As Object = Nothing

        Using objAppel As New RIS.XuCuAppelerCompI(Of TS1N215_INiveauSecrt2.TsICompI)
            Dim chaineContexte As String = objAppel.PreparerAppel(contexte, ObtenirCodeEnv())

            CaAffaire = objAppel.CreerComposantIntegration(chaineContexte)
            resultat = CaAffaire.DetruireCle(chaineContexte, CleADetruire)
            objAppel.AnalyserRetour(chaineContexte, Nothing)
        End Using

        Return resultat
    End Function

    Friend Shared Function ObtenirPassword(ByVal Cle As TS1N201_DtCdAccGenV1.TsDtCleSym) As String
        Dim CaAffaire As TS1N215_INiveauSecrt1.TsICompI
        Dim resultat As String = String.Empty
        Dim contexte As Object = Nothing

        Using objAppel As New RIS.XuCuAppelerCompI(Of TS1N215_INiveauSecrt1.TsICompI)
            Dim chaineContexte As String = objAppel.PreparerAppel(contexte, ObtenirCodeEnv())

            CaAffaire = objAppel.CreerComposantIntegration(chaineContexte)
            resultat = CaAffaire.AfficherMDP(chaineContexte, Cle)
            objAppel.AnalyserRetour(chaineContexte, Nothing)
        End Using

        Return resultat
    End Function

    Friend Shared Sub ImporterCles()
        Dim CaAffaire As TS1N215_INiveauSecrt2.TsICompI
        Dim chaineContexte As String = String.Empty
        Dim contexte As Object = Nothing

        Using objAppel As New RIS.XuCuAppelerCompI(Of TS1N215_INiveauSecrt2.TsICompI)
            chaineContexte = objAppel.PreparerAppel(contexte, ObtenirCodeEnv())

            CaAffaire = objAppel.CreerComposantIntegration(chaineContexte)
            CaAffaire.ImporterCles(chaineContexte)
            objAppel.AnalyserRetour(chaineContexte, Nothing)
        End Using
    End Sub

    Friend Shared Sub ExporterCles()
        Dim CaAffaire As TS1N215_INiveauSecrt2.TsICompI
        Dim chaineContexte As String = String.Empty
        Dim contexte As Object = Nothing

        Using objAppel As New RIS.XuCuAppelerCompI(Of TS1N215_INiveauSecrt2.TsICompI)
            chaineContexte = objAppel.PreparerAppel(contexte, ObtenirCodeEnv())

            CaAffaire = objAppel.CreerComposantIntegration(chaineContexte)
            CaAffaire.ExporterCles(chaineContexte)
            objAppel.AnalyserRetour(chaineContexte, Nothing)
        End Using
    End Sub

    Friend Shared Function ObtenirCodeEnv() As RIS.XuCaCreerContexte.XuCCEnvrn
        Dim result As RIS.XuCaCreerContexte.XuCCEnvrn = RIS.XuCaCreerContexte.XuCCEnvrn.UNIT
        Select Case Rrq.InfrastructureCommune.ScenarioTransactionnel.XuCaContexte.EnvrnPFI(String.Empty)
            Case "U"
                result = RIS.XuCaCreerContexte.XuCCEnvrn.UNIT
            Case "I"
                result = RIS.XuCaCreerContexte.XuCCEnvrn.INTG
            Case "A"
                result = RIS.XuCaCreerContexte.XuCCEnvrn.ACCP
            Case "B"
                result = RIS.XuCaCreerContexte.XuCCEnvrn.FORA
            Case "Q"
                result = RIS.XuCaCreerContexte.XuCCEnvrn.FORP
            Case "S"
                result = RIS.XuCaCreerContexte.XuCCEnvrn.SIML
            Case "P"
                result = RIS.XuCaCreerContexte.XuCCEnvrn.PROD
            Case Else
                result = RIS.XuCaCreerContexte.XuCCEnvrn.PROD
        End Select
        Return result
    End Function

    Friend Shared Function ObtenirTypesCles() As List(Of TypeCle)
        Dim cles As String() = Config.TypesConnexion

        Dim values As New List(Of TypeCle)()
        For Each cle As String In cles
            Dim valeurs As String() = Config.ObtenirValeurs(cle)

            Dim code As String = cle.Split("\"c).Last()
            Dim affichage As String = valeurs(0)
            Dim corpsDeCle As String = valeurs(1)

            values.Add(New TypeCle(code, affichage, corpsDeCle))
        Next

        Return values
    End Function

End Class

Friend Class TypeCle
    Public ReadOnly Property Code As String
    Public ReadOnly Property Affichage As String
    Public ReadOnly Property Corps As String

    Friend Sub New(code As String, affichage As String, corps As String)
        Me.Code = code
        Me.Affichage = affichage
        Me.Corps = corps
    End Sub

    Public Overrides Function ToString() As String
        Return String.Format("Code:{0, Affichage:{1}, Corps:{2}", Code, Affichage, Corps)
    End Function
End Class


Friend Module Ext

    <Extension>
    Public Function SansBlanc(source As String) As String
        If source Is Nothing Then Return String.Empty
        Return source.ToString().Trim()
    End Function

    <Extension>
    Public Function SelectionEstUneClefOuGroupeEnvironnements(source As XzCrArborescence, ByRef nomCle As String) As Boolean
        If source.SelectedRows.Count <> 1 Then Return False

        Dim row As Xceed.Grid.DataRow = DirectCast(source.SelectedRows(0), Xceed.Grid.DataRow)
        If row.Cells.Count < 2 Then Return False

        Dim nomClef As String = row.Cells(1).Value.ToString()
        If nomClef.Length > 4 AndAlso Not nomClef.Equals(My.Resources.SousSystemeVide) Then
            nomCle = nomClef
            Return True
        End If

        Return False
    End Function

    <Extension>
    Public Function ExtraireNumeroSequence(source As String, template As TsCuCle) As Integer
        Dim sequence As Integer = source.ExtraireDernierChiffreSequence()
        Return template.AjusterSequence(sequence)
    End Function

    <Extension>
    Private Function ExtraireDernierChiffreSequence(ByVal source As String) As Integer
        Dim numeric As Integer = source.ExtraireDernierChiffre()

        'si la valeur n'est pas valide ou 0, on retourne 1
        If numeric < 1 Then Return 1

        'on retourne la valeur trouvé
        Return numeric
    End Function

    <Extension>
    Private Function ExtraireDernierChiffre(ByVal source As String) As Integer
        Dim pattern As String = "[0-9]+"

        Dim numericString As String = "-1"
        Dim m As RegularExpressions.Match = RegularExpressions.Regex.Match(source, pattern, RegularExpressions.RegexOptions.Compiled)
        While m.Success
            numericString = m.Value
            m = m.NextMatch
        End While

        Dim numeric As Integer
        'si la valeur n'est pas valide ou 0, on retourne 1
        If Not Integer.TryParse(numericString, numeric) Then Return -1
        'on retourne la valeur trouvé
        Return numeric
    End Function

    <Extension>
    Private Function AjusterSequence(source As TsCuCle, sequence As Integer) As Integer
        'connexion avec fin numérique
        If Not Char.IsNumber(source.Connection.Last) Then Return sequence

        'retirer le numéral de la fin de la connexion, du début de la séquence
        Dim numeral As Integer = source.Connection.ExtraireDernierChiffre()
        Return Integer.Parse(sequence.ToString().Substring(numeral.ToString().Length))
    End Function

End Module

