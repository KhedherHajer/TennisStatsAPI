## Test Technique Backend

## Contexte
 
Il s'agit de cr�er une **API RESTful** pour retourner les statistiques des joueurs de tennis � partir d'un fichier JSON.

Projets realis�s:

- API REST avec **ASP.NET Core 8**
- Tests unitaires avec **xUnit** et **Moq**

---

## Fonctionnalit�s

- Retourner la liste des joueurs tri�s du meilleur au moins bon
- Retourner les d�tails d�un joueur par son ID
- Retourner les statistiques globales :<br> - Pays avec le meilleur ratio de victoires<br> - IMC moyen<br> - M�diane des tailles
- D�ploiement en Cloud: En l'occurence Azure sur un App Service qui est un service Azure qui permet d'h�berger des applications web et des API.

## Instructions
On peut publier l'API sur le App Service Azure soit via l'interface de ligne de commande Azure CLI soit � partir de visual studio:

I - D�ploiement de l'API via l'interface Visual Studio:
-  Se connecter � Azure dans Visual Studio
	- En haut � droite de Visual Studio, clique sur ton profil utilisateur et choisis "Se connecter".
- Publier le projet
	- Clique droit sur ton projet (pas la solution enti�re) dans l'Explorateur de solutions => Publier.
	- Sur la fen�tre "Publier", clique sur Azure => App Service (Windows/Linux).
	- Clique sur Cr�er un nouvel App Service (ou s�lectionne un existant si tu en as d�j� un).
	- Une fois ton App Service s�lectionn�, le mode de publication : Framework-Dependent (par d�faut, sauf besoin sp�cial) avec Configuration : Release et Cible : Framework .NET 8 (net8.0) 
	- D�ployer: Clique sur Terminer puis sur Publier.
	- Une fois publi�, tu verras dans Visual Studio l'URL d'acc�s du type : https://ton-app.azurewebsites.net

II - D�ploiement de l'API via Azure CLI:
- T�l�charger Azure CLI
- Se connecter � Azure
```
az login
```
- Cr�er un groupe de ressources (admettant eastus est la localisation du ressources group)
```
az group create --name myResourceGroup --location eastus
```

Cr�er un plan Azure App Service
```
az appservice plan create --name myAppServicePlan --resource-group myResourceGroup --sku B1 --is-linux
```

Cr�er l'application web
```
az webapp create --resource-group myResourceGroup --plan myAppServicePlan --name myApiApp --runtime "DOTNET|6.0"
```

Publier votre projet d'API .NET
```
dotnet publish -c Release -o ./publish
```

D�ployer l'API sur Azure App Service
```
az webapp deploy --resource-group myResourceGroup --name myApiApp --src-path ./publish

```

