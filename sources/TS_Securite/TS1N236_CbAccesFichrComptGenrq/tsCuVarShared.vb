Imports System.Collections.Generic
Imports System.Runtime.CompilerServices


''' <summary>
''' On y conserve le dataset contenant les données des codes d'accès génériques.
''' Cela évite de lire le fichier XML à chaque demande d'information.
''' On vérifie la date de modification avant de lire le fichier XML.
''' </summary>
''' <remarks>
''' Historique des modifications: 
''' --------------------------------------------------------------------------------
''' Date		Nom			Description
''' 
''' --------------------------------------------------------------------------------
''' 2007-11-26	T206500		Épurer plusieurs variables.
'''                         On y conserve seulement le dataset complet des codes
'''                         d'accès génériques ainsi que la date de modification.
'''                         Les autres variables ont été rapatriées dans les classes
'''                         où elles étaient utilisées.
''' 
''' 2008-01-14  T206500     Ajouter la clé d'encryption et le vecteur d'initalisation
''' 
''' 2010-01-13  T206500     Ajouter la variable Heure prévue de la prochaine vérification 
'''                         du fichier des codes accès.  cela évite d'accéder au serveur
'''                         FIC1 à chaque appel.  Performance!!!!
'''                         On effectue la vérification seulement aux 5 minutes.
''' </remarks>
Friend Class tsCuVarShared

    'Dataset qui contient tous les codes d'accès génériques
    Friend Shared dsCachedCodeAcces As DataSet

    'Dictionnaire qui contient tous les codes d'accès génériques
    Friend Shared dicCachedCodeAcces As Dictionary(Of String, DataRow)

    'Date de modification du dépot des codes d'accès génériques
    Friend Shared dtDernModifDepot As DateTime

    'Nom du fichier des codes d'accès génériques
    Friend Shared strDernNomFichCdAcc As String

    'Heure prévue de la prochaine vérification du changement de la date du dépôt des codes d'accès génériques
    ' --> On met la variable Nullable afin de s'assurer que la variable datetime soit mise à jour intégralement 
    ' --> lors de chaque comparaison de cette date.  
    Friend Shared dtProchVerifModifDepot As Nullable(Of DateTime)

End Class

Friend Module Extensions

    <Extension>
    Public Function GenererCle(source As DataRow) As String
        Return String.Concat(source("Cle").ToString().ToUpperInvariant(), source("Type").ToString.ToUpperInvariant())
    End Function

    <Extension>
    Public Sub AjouterLigneTS5From(source As DataTable, valeur As DataRow)
        Dim cleTS5 As DataRow = source.NewRow()
        cleTS5.Item("Cle") = valeur.Item("Cle").ToString()
        cleTS5.Item("Code") = valeur.Item("Code").ToString.ToUpper()
        source.Rows.Add(cleTS5)
    End Sub

    <Extension>
    Public Function Est(source As String, valeur As String) As Boolean
        Return String.Equals(source, valeur, StringComparison.InvariantCultureIgnoreCase)
    End Function

End Module