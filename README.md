# PortfolioBlogMVC - Guide de démarrage

Ce guide décrit les étapes pour préparer l'environnement, résoudre les erreurs courantes (LocalDB / certificat / dotnet-ef) et exécuter les migrations pour `PortfolioBlogMVC`.

## Prérequis
1. Windows (développement local).  
2. .NET SDK 8\.0.x installé (`dotnet --version`).  
3. Visual Studio / JetBrains Rider (optionnel).  
4. SQL Server Express LocalDB ou une instance SQL Server (ex: `.\SQLEXPRESS`).  

## Étapes d'installation rapides

1\. Mettre à jour/installer `dotnet-ef` (PowerShell)  
- Mettre à jour si déjà installé :  
  `dotnet tool update --global dotnet-ef --version 8.0.20`  
- Sinon installer :  
  `dotnet tool install --global dotnet-ef --version 8.0.20`

2\. Mettre à jour les packages NuGet (si nécessaire)  
- Exemple :  
  `dotnet add PortfolioBlogMVC package Microsoft.EntityFrameworkCore.Design --version 8.0.20`  
  `dotnet add PortfolioBlogMVC package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.20`  
  `dotnet add PortfolioBlogMVC package Microsoft.EntityFrameworkCore.Tools --version 8.0.20`

3\. Installer / vérifier LocalDB (si vous utilisez `(localdb)\mssqllocaldb`)  
- Vérifier les instances : `sqllocaldb info`  
- Créer et démarrer l'instance par défaut si nécessaire :  
  `sqllocaldb create MSSQLLocalDB`  
  `sqllocaldb start MSSQLLocalDB`  
- Si `sqllocaldb` n'est pas reconnu : installez SQL Server Express (LocalDB) depuis le site Microsoft ou via Chocolatey :  
  `choco install sql-server-express -y`  
- Après installation, ouvrez un nouveau terminal.

4\. Alternative : utiliser `.\SQLEXPRESS`  
- Modifier la chaîne de connexion dans `PortfolioBlogMVC/appsettings.json` pour pointer vers `.\SQLEXPRESS` si vous préférez cette instance.

## Chaîne de connexion & problème de certificat
- Pour le développement, si vous obtenez l'erreur SSL "autorité qui n'est pas approuvée", ajoutez dans la chaîne de connexion : `TrustServerCertificate=True` (ou `Encrypt=False`).  
- Exemple de clé à vérifier dans `PortfolioBlogMVC/appsettings.json` :  
  `DefaultConnection` doit contenir `TrustServerCertificate=True` ou `Encrypt=False` si vous ne gérez pas de certificat approuvé.

Sécurité : en production, installez un certificat émis par une CA approuvée ou ajoutez l'AC racine dans les Trusted Root Certification Authorities, puis supprimez `TrustServerCertificate=True`.

## Migrations et mise à jour de la base
1\. Créer une migration (si nécessaire) :  
`dotnet ef migrations add InitialCreate --project PortfolioBlogMVC --startup-project PortfolioBlogMVC`

2\. Appliquer la migration / créer la base :  
`dotnet ef database update --project PortfolioBlogMVC --startup-project PortfolioBlogMVC`

Remarque : ces commandes n'ont pas besoin que l'application tourne.

## Résolution rapide des erreurs rencontrées
- `\'sqllocaldb' n’est pas reconnu` : installer LocalDB et relancer le terminal.  
- Erreur "Unable to locate a Local Database Runtime installation" : installer SQL Server Express LocalDB.  
- Erreur SSL "autorité qui n’est pas approuvée" : pour dev, ajouter `TrustServerCertificate=True` ; pour prod, installer un certificat valide.

## Vérifications utiles
- `dotnet --info` pour vérifier le SDK.  
- `dotnet-ef --version` pour vérifier la version des outils.  
- `sqllocaldb info` pour lister les instances LocalDB.  
- Ouvrir le service SQL Server dans les services Windows pour vérifier l'état de l'instance `MSSQL$SQLEXPRESS` ou similaire.


