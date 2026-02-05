Imports System.Data
Imports Configuration = Rrq.InfrastructureCommune.Parametres.XuCuConfiguration

Public Class TsCuOutils

    Private Shared ReadOnly Property CheminFichierUniteAdministrative() As String
        Get
            Return Configuration.ValeurSysteme("TS7", "TS7N121\UniteAdministrative")
        End Get
    End Property

    ''' <summary>
    ''' Liste de toutes les unités administratives
    ''' </summary>
    ''' <remarks>Données en lecture seule.</remarks>
    Private Shared ReadOnly Property ListeDeToutesLesUnitesAdministratives() As List(Of String)
        Get
            Dim Liste As List(Of String) = New List(Of String)

            Dim regex As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex("^([0-9][0-9][0-9][0-9]).*$")
            Dim ToutesLesLignes As String() = IO.File.ReadAllLines(CheminFichierUniteAdministrative)
            For Each ligne As String In ToutesLesLignes
                Dim match As System.Text.RegularExpressions.Match = regex.Match(ligne)
                If match.Success Then
                    Liste.Add(match.Groups(1).Value)
                End If
            Next
            Return Liste
        End Get
    End Property

    ''' <summary>
    ''' Transforme les wildcard en liste d'unites administratives existantes
    ''' </summary>
    ''' <param name="uniteAdmin">Un identifiant d'unité administrative (peut contenir le caractère special *)</param>
    ''' <returns>Une liste d'unites adminstratives existantes</returns>
    ''' <remarks>Usage interne seulement</remarks>
    Public Shared Function TraiterUniteAdministrativeResponsable(ByVal uniteAdmin As String) As List(Of String)
        Dim Liste As List(Of String) = New List(Of String)

        If uniteAdmin.Contains("*") Then
            Dim regex As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex("^" & uniteAdmin.Replace("*", ".*") & "$")
            For Each uniteAdminCourante As String In ListeDeToutesLesUnitesAdministratives
                If regex.Match(uniteAdminCourante).Success Then
                    Liste.Add(uniteAdminCourante)
                End If
            Next
        Else
            Liste.Add(uniteAdmin)
        End If

        Return Liste
    End Function

    ''' <summary>
    ''' Fonction de traduction.
    ''' </summary>
    ''' <param name="utilisateurs"></param>
    ''' <returns></returns>
    ''' <remarks>Outils fait pour les petits systêmes web.</remarks>
    ''' listeColonnes()=        {"ID", "NomComplet",   "Prenom", "Nom", "Numéro unité administrative", "Fin prévue",           "Date de fin",      "Courriel",     "Ville"}
    ''' listeInformations()=    {u.ID, u.NomComplet,    u.Prenom, u.Nom, u.NoUniteAdmin,                u.FinPrevue.ToString,   u.DateFin.ToString, u.Courriel,     u.Ville}
    ''' Si vous changé quelque chose dans une des 2 listes, il faut le changé dans l'autre pour que la logique suive.
    Public Shared Function UtilisateurDataTable(ByVal utilisateurs As List(Of TsCdUtilisateur)) As DataTable
        Dim paramRetour As New DataTable()

        ' C'est une liste de noms des colonnes
        Dim listeColonnes() As String = {"ID", "NomComplet", "Prenom", "Nom", "Numéro unité administrative",
        "Fin prévue", "Date de fin", "Courriel", "Ville"}

        For Each c As String In listeColonnes
            Dim colonne As New DataColumn()
            colonne.ColumnName = c
            colonne.DataType = Type.GetType("System.String")
            paramRetour.Columns.Add(colonne)
        Next


        For Each u As TsCdUtilisateur In utilisateurs
            ' C'est une liste de données aussi longue et trié comme la liste des noms des colonnes. L'ordre est très important.
            Dim listeInformations() As String = {u.ID, u.NomComplet, u.Prenom, u.Nom,
            u.NoUniteAdmin, u.FinPrevue.ToString, u.DateFin.ToString, u.Courriel, u.Ville}

            Dim ligne As DataRow = paramRetour.NewRow()

            Dim MaxI As Integer = listeColonnes.Length - 1
            For i As Integer = 0 To MaxI
                ligne(listeColonnes(i)) = listeInformations(i)
            Next
            paramRetour.Rows.Add(ligne)
        Next

        Return paramRetour
    End Function

End Class
