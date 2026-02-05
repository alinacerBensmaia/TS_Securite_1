Imports Rrq.Securite
Imports Rrq.InfrastructureCommune.Parametres
Imports System.Runtime.CompilerServices

Public Class TsCaHelperMemorisation

    Private memorisateur As TsIHelperMemrsSecurite
    Private typeMemorisation As Integer = ParseEnum(Of TsTypeMemorisation)(XuCuPolitiqueConfig.ConfigDomaine.ValeurSysteme("TS4", "TS4\TS4N331\TypeMemorisation"))
    Private memorisationRecente As List(Of String)

    Public Sub New()

        Select Case typeMemorisation
            Case TsTypeMemorisation.Runtime
                memorisateur = CreerMemorisateurRunTime()
            Case TsTypeMemorisation.Aucun
                memorisateur = Nothing
            Case Else
                ' Garde-fou au cas où un autre accesseur soit ajouté ou que la propriété contienne du garbage 
                Throw New TsCuTypeMemorisationInconnuException()
        End Select

        memorisationRecente = New List(Of String)
    End Sub

    Function ObtenirObjetMemoire(ByVal pCleObjetMemoire As String) As Object
        If memorisateur IsNot Nothing Then
            Return memorisateur.ObtenirObjetMemoire(pCleObjetMemoire)
        End If
        Return Nothing
    End Function

    Sub Memoriser(ByVal pCleObjetMemoire As String, ByVal pObjetSecurite As Object)
        If memorisateur IsNot Nothing Then
            memorisateur.Memoriser(pCleObjetMemoire, pObjetSecurite)
        End If
        memorisationRecente.Add(pCleObjetMemoire)
    End Sub

    Sub Dememoriser(ByVal pCleObjetMemoire As String)
        If memorisateur IsNot Nothing Then
            memorisateur.Dememoriser(pCleObjetMemoire)
        End If
    End Sub

    Sub DememoriserTout()
        If memorisateur IsNot Nothing Then
            memorisateur.DememoriserTout()
        End If
    End Sub

    Function EstMemorisationRecente(ByVal pCleObjetMemoire As String) As Boolean
        Dim estMemRecente As Boolean = False

        If memorisationRecente.Contains(pCleObjetMemoire) Then
            estMemRecente = True
        End If

        Return estMemRecente

    End Function


#Region "--- Privées ---"
    <MethodImpl(MethodImplOptions.NoInlining)> _
    Private Function CreerMemorisateurRunTime() As TsIHelperMemrsSecurite
        Return New TsCuHelperMemrsRuntime
    End Function


    ' Idéalement, on déclarerait T As [Enum], mais "'Enum' ne peut pas être utilisé en tant que contrainte de type."
    Private Function ParseEnum(Of T)(ByVal valeur As String) As T
        If GetType(T).BaseType IsNot GetType([Enum]) Then
            Throw New FormatException("T doit être une énumération")
        End If

        ' On est gentil et on ignore la casse dans le parsing
        Return DirectCast([Enum].Parse(GetType(T), valeur, True), T)
    End Function

#End Region

End Class
