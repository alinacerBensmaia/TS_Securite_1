# Référence .md
https://help.github.com/en/github/writing-on-github/basic-writing-and-formatting-syntax
https://guides.github.com/features/mastering-markdown/

# Pour consulter le .md
Markdown viewer ++ pour NotePad++

Installer un module complémentaire dans chrome (Markdown viewer)

# Titre du projet

TS4N401_CbFederationADFS -- Gestion d'un fédération ADFS + Http module de sécurité des entêtes HTTP et cookies

## Pour débuter

Il a de la configuration à ajuster pour mettre en place une fédération ADFS.

1- Faire créer une application dans ADFS

2- Dans la configuration de ts4.config section TS4N401 ajouter une entrée pour l'application
	Valeurs à titre d'exemple
```xml	
		<NomDuPoolDansIIS>
			<add cle="ClientId" valeur="L'id d'application dans ADFS" />
			<add cle="Authority" valeur="https://sts1.retraitequebec.gouv.qc.ca/adfs"/>
			<add cle="RedirectUri" valeur="/Interactif3270/3270/Pages/Contexte.aspx" />
			<add cle="RegExExlusion" valeur="Pages/BigIp.aspx|Pages/Deconnexion.aspx" />
			<add cle="ExpirationCookies" valeur="120" />
		</NomDuPoolDansIIS>
```
3- Ajuster le web.config
### Configuration du web.config
#### Pour activer le module OWIN
```xml
  <appSettings>
	<add key="owin:AppStartup" value="TS4N401_CbFederationADFS.TsCuGestId"/>
  </appSettings>
```
#### Pour activer le module HTTP
```xml
  <system.webServer>
    <modules>
	  <add name="TS4N401_CbFederationADFS" type="TS4N401_CbFederationADFS.TsCuModuleHttp, TS4N401_CbFederationADFS" />
    </modules>
    <httpErrors errorMode="Detailed" />
  </system.webServer>
```

## Deploiement

Graduer le tout via le STCM et l'outil de graduation de fichiers de configuration.
Il faut faire un fichier de configuration par environnement
Il faut faire créer une application ADFS par environnement

## Auteur

Sylvain Lachance (Développement initial)