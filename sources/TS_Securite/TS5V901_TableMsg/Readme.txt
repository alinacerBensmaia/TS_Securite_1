Pour créer un nouveau dépot de message
--------------------------------------

1. Créer un nouveau projet à partir du projet gabarit XY6V901_TableMsg.
2. Ajouter les messages désirés au fichier de ressources (format .RC)
   selon la procédure décrites ci-dessous.
3. Enregistrer le fichier de ressources à l'intérieur de votre structure de projet.
4. Compiler le fichier de ressources.
5. Inclure le fichier de ressource compilé (format .RES) à votre projet.
6. Créer le .DLL.
7. Aviser le responsable de la composante de gestion des messages XY5V051_GestionMsg
   pour que cette dernière fasse référence à votre nouveau dépôt de message.

	Particularités
	--------------
	1. Un dépôt de message par sous-système.

Pour modifier un dépôt de message
---------------------------------

1. Ouvrir le projet contenant le dépôt de message.
2. Ajouter, modifier ou détruire les messages au fichier de ressources (format .RC)
   selon la procédure décrites ci-dessous.
3. Enregistrer le fichier de ressources à l'intérieur de votre structure de projet.
4. Supprimer l'ancien fichier de ressource compilé (format .RES) de votre projet.
5. Compiler le nouveau fichier de ressources.
6. Inclure le nouveau fichier de ressource compilé (format .RES) à votre projet.
7. Remplacer le .DLL existant.
 
Pour ajouter, modifier ou détruire un message dans le fichier de ressources
---------------------------------------------------------------------------

Ajouter, modifier ou détruire les messages dans le fichier .RC associé au projet 
en suivant la structure de message décrite plus bas.

	Structure d'un message
	----------------------
	9999 "S Texte du message avec *"
		
	où:
		
	9999 est le numéro du message (les valeurs possibles vont de 0001 à 9999);
	S représente le code de sévérité du message (les valeurs possibles sont 
	  I pour Information, A pour Avertissement, E pour Erreur);
	'Texte du message avec *' est le texte du message à afficher et * représente une
				 variable.

	ex.: 0001 "I Message d'information. Variable 1 = *; Variable 2 = *"

	Particularités
	--------------

	1. Il n'y a pas de limite à la longueur du texte d'un message.
	2. On peut inscrire autant de variables que désirées dans un message.

Pour compiler un fichier de ressources
--------------------------------------

	La première fois
	----------------

	1. Copier le fichier Compiler.BAT dans un répertoire de votre disque C.
	2. Accéder à la clé "rcfile" de la la base de registres HKEY_CLASSES_ROOT.
	3. Créer une nouvelle clé sous la sous-clé Shell et nommer cette clé Compiler.
	4. Créer un sous-clé Command sous la clé Compiler.
	5. Assigner la valeur <Path>\COMPILER.BAT "%1" à la clé Command
	   où <Path> est le chemin pour accéder à Compiler.BAT.

1. À l'aide de l'Explorateur Windows, accéder au fichier ressources (format .RC)
   à compiler.
2. Avec le bouton droit de la souris, sélectionner la commande Compiler.
3. Vérifier le résultat de la session DOS.
4. Fermer la session DOS.
