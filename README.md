## Test Technique Backend

## Contexte
 
Il s'agit de créer une **API RESTful** pour retourner les statistiques des joueurs de tennis à partir d'un fichier JSON.

Projets realisés:

- API REST avec **ASP.NET Core 8**
- Tests unitaires avec **xUnit** et **Moq**

---

## Fonctionnalités

- Retourner la liste des joueurs triés du meilleur au moins bon
- Retourner les détails d’un joueur par son ID
- Retourner les statistiques globales :<br> - Pays avec le meilleur ratio de victoires<br> - IMC moyen<br> - Médiane des tailles
- Déploiement en Cloud: En l'occurence Azure sur un App Service qui est un service Azure qui permet d'héberger des applications web et des API.

## Instructions
On peut publier l'API sur le App Service Azure soit via l'interface de ligne de commande Azure CLI soit à partir de visual studio:

I - Déploiement de l'API via l'interface Visual Studio:
-  Se connecter à Azure dans Visual Studio
	- En haut à droite de Visual Studio, clique sur ton profil utilisateur et choisis "Se connecter".
- Publier le projet
	- Clique droit sur ton projet (pas la solution entière) dans l'Explorateur de solutions => Publier.
	- Sur la fenêtre "Publier", clique sur Azure => App Service (Windows/Linux).
	- Clique sur Créer un nouvel App Service (ou sélectionne un existant si tu en as déjà un).
	- Une fois ton App Service sélectionné, le mode de publication : Framework-Dependent (par défaut, sauf besoin spécial) avec Configuration : Release et Cible : Framework .NET 8 (net8.0) 
	- Déployer: Clique sur Terminer puis sur Publier.
	- Une fois publié, tu verras dans Visual Studio l'URL d'accès du type : https://ton-app.azurewebsites.net

II - Déploiement de l'API via Azure CLI:
- Télécharger Azure CLI
- Se connecter à Azure
```
az login
```
- Créer un groupe de ressources (admettant eastus est la localisation du ressources group)
```
az group create --name myResourceGroup --location eastus
```

Créer un plan Azure App Service
```
az appservice plan create --name myAppServicePlan --resource-group myResourceGroup --sku B1 --is-linux
```

Créer l'application web
```
az webapp create --resource-group myResourceGroup --plan myAppServicePlan --name myApiApp --runtime "DOTNET|6.0"
```

Publier votre projet d'API .NET
```
dotnet publish -c Release -o ./publish
```

Déployer l'API sur Azure App Service
```
az webapp deploy --resource-group myResourceGroup --name myApiApp --src-path ./publish

```

