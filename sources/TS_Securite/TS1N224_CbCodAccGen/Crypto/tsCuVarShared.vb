''' --------------------------------------------------------------------------------
''' Project:	TS1N224_ZgObtCdAccGen
''' Class:	    Rrq.Securite.tsCuVarShared
''' <summary>
''' 
''' On y conserve le dataset contenant les données des codes d'accès génériques.
''' Cela évite de lire le fichier XML à chaque demande d'information.
''' On vérifie la date de modification avant de lire le fichier XML.
''' 
''' </summary>
''' <remarks>
''' Historique des modifications: 
''' 
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
'''     
''' </remarks>
''' --------------------------------------------------------------------------------
Friend Class TsCuVarShared

    'Dataset qui contient tous les codes d'accès génériques
    Friend Shared dsCachedCodeAcces As DataSet

    'Date de modification du dépot des codes d'accès génériques
    Friend Shared dtDernModifDepot As DateTime

    'Nom du fichier des codes d'accès génériques
    Friend Shared strDernNomFichCdAcc As String

    'Heure prévue de la prochaine vérification du changement de la date du dépôt des codes d'accès génériques
    ' --> On met la variable Nullable afin de s'assurer que la variable datetime soit mise à jour intégralement 
    ' --> lors de chaque comparaison de cette date.  
    Friend Shared dtProchVerifModifDepot As Nullable(Of DateTime)

    'Clé d'encryption
    Friend Shared strK_Fichr As String = "175, 9, 217, 205, 171, 255, 240, 107, 210, 82, 94, 242, 92, 213, 130, 118, 227, 155, 114, 118, 142, 215, 168, 244, 12, 135, 4, 32, 234, 117, 53, 164"

    'Vecteur d'initialisation
    Friend Shared strV_Fichr As String = "42, 235, 220, 217, 248, 82, 28, 111, 251, 163, 79, 126, 139, 163, 196, 187"


End Class
