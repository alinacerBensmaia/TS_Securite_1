Imports System.Runtime.CompilerServices
Imports Rrq.InfrastructureCommune.Parametres
Imports System.Data.SqlClient

Friend Class TsCdAccesAD
    Private Const SQL_OBTENIR_GROUPE_ADLDS As String = "SELECT TOP 1 NM_GRO_SEC_LDS " &
                                                       "FROM [TS8].[DEMEXTS], " &
                                                            "[TS8].[GROSETS] " &
                                                       "WHERE DEMEXTS.CO_ETT_DEM_EXT_AD = 'TRA' " &
                                                       "AND   DEMEXTS.NO_DEM_EXT_AD = GROSETS.NO_DEM_EXT_AD " &
                                                       "AND   GROSETS.NM_GRO_SEC = @NM_GRO_SEC"
    Private ReadOnly _connectionString As String
    Public Sub New()
        _connectionString = XuCuPolitiqueConfig.ConfigDomaine.ObtenirValeurSystemeOptionnelle("TS4", "TS4\TS4N213\Connexion_ConformiteADLDS")
    End Sub

    <MethodImpl(MethodImplOptions.NoInlining)>
    Public Function ObtenirNomGroupeAdLds(nomGroupeAD As String) As String
        Using cnx As New SqlConnection(_connectionString)
            Using cmd As New SqlCommand()
                cmd.Connection = cnx
                cmd.CommandType = CommandType.Text
                cmd.CommandText = SQL_OBTENIR_GROUPE_ADLDS
                cmd.Parameters.Add(New SqlParameter("NM_GRO_SEC", nomGroupeAD))

                Using resultat As New DataTable()
                    Using data As New SqlDataAdapter(cmd)
                        If data.Fill(resultat) > 0 Then
                            Return resultat.Rows(0)("NM_GRO_SEC_LDS").ToString()
                        Else
                            Return String.Empty
                        End If
                    End Using
                End Using
            End Using
        End Using
    End Function

End Class
