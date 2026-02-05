Imports System.Collections.Generic
Imports System.Text
Imports TS1N214_PrGerAccGen.TsCuConversionsTypes

''' <summary>
''' Classe utilitaire pour la gestion d'un groupe de clés
''' </summary>
''' <remarks></remarks>
Friend Class TsCuGroupe
    Private mListeCle As Dictionary(Of String, TsCuCle)

#Region "Énumérations"
    Friend Enum TsPgagTypeDict As Integer
        TsPgagTdDictCode = 0
        TsPgagTdDictMdp = 1
    End Enum
#End Region

#Region "Propriétés"
    Friend ReadOnly Property ListeCle() As Dictionary(Of String, TsCuCle)
        Get
            Return mListeCle
        End Get
    End Property
#End Region

    Friend Sub New()
        mListeCle = New Dictionary(Of String, TsCuCle)
    End Sub

    ''' <summary>
    ''' Effacer les éléments des 3 dictionnaires de la classe
    ''' </summary>
    ''' <remarks></remarks>
    Friend Sub Clear()
        ListeCle.Clear()
    End Sub

    Friend Function ObtenirListeCodes(ByVal pTypeDict As TsPgagTypeDict) As String
        Dim item As KeyValuePair(Of String, TsCuCle)
        Dim sb As New StringBuilder

        If ListeCle IsNot Nothing Then
            For Each item In ListeCle
                If item.Value IsNot Nothing Then
                    If Not String.IsNullOrEmpty(getstring(item.Key)) Then
                        sb.Append(item.Key)
                    End If
                    sb.Append(":<<")

                    Select Case pTypeDict
                        Case TsPgagTypeDict.TsPgagTdDictCode
                            If item.Value.CodeUsagerAd IsNot Nothing AndAlso
                               Not String.IsNullOrEmpty(GetString(item.Value.CodeUsagerAd)) Then
                                sb.Append(item.Value.CodeUsagerAd)
                            End If
                        Case TsPgagTypeDict.TsPgagTdDictMdp
                            If item.Value.Mdp IsNot Nothing AndAlso
                               Not String.IsNullOrEmpty(GetString(item.Value.Mdp)) Then
                                sb.Append(item.Value.Mdp)
                            End If
                    End Select

                    sb.Append(">>")
                End If
            Next
        End If

        Return sb.ToString
    End Function

    Friend Function ObtenirListeProfils() As IList(Of TS1N201_DtCdAccGenV1.TsDtGroAd)
        Dim result As List(Of TS1N201_DtCdAccGenV1.TsDtGroAd) = New List(Of TS1N201_DtCdAccGenV1.TsDtGroAd)
        Dim item As KeyValuePair(Of String, TsCuCle)
        Dim group As TS1N201_DtCdAccGenV1.TsDtGroAd = Nothing

        For Each item In ListeCle
            If item.Value IsNot Nothing Then
                group = New TS1N201_DtCdAccGenV1.TsDtGroAd
                group.NmGroActDirTs = item.Key & ":<<"
                If Not String.IsNullOrEmpty(GetString(item.Value.GroupeAd)) Then
                    group.NmGroActDirTs = group.NmGroActDirTs & item.Value.GroupeAd
                End If
                group.NmGroActDirTs = group.NmGroActDirTs & ">>"
                result.Add(group)
            End If
        Next
        Return result
    End Function

    ''' <summary>
    ''' Valider que le mot de passe est fourni
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function ValiderMotPasse() As Boolean
        Dim count As Integer = 0
        Dim item As KeyValuePair(Of String, TsCuCle)

        For Each item In ListeCle
            If item.Value IsNot Nothing AndAlso
               String.IsNullOrEmpty(GetString(item.Value.Mdp)) Then
                count += 1
            End If
        Next

        Return count = 0
    End Function



    ''' <summary>
    ''' Valider la longueur du mot de passe  
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function ValiderLonguerMotPasse(ByVal longueur As Integer) As Boolean
        Dim count As Integer = 0
        Dim item As KeyValuePair(Of String, TsCuCle)

        For Each item In ListeCle
            If item.Value IsNot Nothing AndAlso
               item.Value.Mdp.Length < longueur Then
                count += 1
            End If
        Next

        Return count = 0
    End Function
End Class
