Imports System.IO

''' <summary>
''' Classes de méthodes statique pour manipuler des fichiers de clés symbolique
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class TsCuFichier

    Private Sub New()
        'Classe non instanciable de méthodes statique (pour la performance)
    End Sub

    ''' <summary>
    ''' Remplir dataset de la zone d'échange applicative (ZEA)
    ''' </summary>
    ''' <param name="pDsTrtCodeAcces">Dataset des codes d'accès</param>
    Friend Shared Function RemplirDsZea(ByVal pDsTrtCodeAcces As DataSet) As DataSet
        Dim listeSystemes As String() = Config.SystemesExportationZEA

        Dim listeZEA As IEnumerable(Of DataRow) = From enrgZea In pDsTrtCodeAcces.Tables(0).Rows.OfType(Of DataRow)()
                                                  Where listeSystemes.Any(Function(x) enrgZea.Item("Cle").ToString().StartsWith(x, StringComparison.OrdinalIgnoreCase))

        Dim dsZea As DataSet = pDsTrtCodeAcces.Clone()
        For Each enrg As DataRow In listeZEA
            dsZea.Tables(0).ImportRow(enrg)
        Next
        dsZea.AcceptChanges()

        Return dsZea
    End Function

    ''' <summary>
    ''' Prendre en backup le fichier des clés exportés
    ''' </summary>
    ''' <param name="PathSrc">Répertoire et fichier</param>
    ''' <remarks></remarks>
    Friend Shared Sub BackupFile(ByVal PathSrc As String)
        Dim fichierCible As String = String.Format("{0}.bak", PathSrc)

        Dim i As Integer = 1
        While File.Exists(fichierCible)
            fichierCible = String.Format("{0}.bak({1})", PathSrc, i)
            i += 1
        End While

        If File.Exists(PathSrc) Then File.Copy(PathSrc, fichierCible)
    End Sub
End Class
