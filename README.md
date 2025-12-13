# PortfolioBlogMVC

## 📝 Description
**PortfolioBlogMVC** est une application web développée en **ASP.NET Core MVC** combinant un **portfolio personnel** avec un **système de blog**.  
Elle permet à un utilisateur de présenter ses **projets**, **articles** et **éléments de portfolio**, tout en offrant des fonctionnalités avancées de **gestion de contenu** : catégories, tags et commentaires.

---

## 🎯 Objectif du Projet
L’objectif principal est de créer une **plateforme personnelle** pour les développeurs ou professionnels souhaitant :
- Mettre en valeur leurs projets.
- Partager des articles de blog.
- Interagir avec les visiteurs via un système de commentaires.
- Gérer facilement le contenu grâce à un tableau de bord administrateur et un système d’authentification intégré.

---

## ⚙️ Fonctionnalités
- **Gestion des articles** : Création, édition, suppression et affichage d’articles de blog.  
- **Catégories d’articles** : Organisation des articles par catégories.  
- **Éléments de portfolio** : Présentation de projets ou réalisations.  
- **Tags** : Étiquetage des articles pour une meilleure recherche.  
- **Commentaires** : Possibilité de commenter les articles.  
- **Authentification** : Gestion des utilisateurs et rôles via **ASP.NET Core Identity**.  
- **Interface utilisateur** : Vues Razor modernes et responsive basées sur **Bootstrap**.

---

## 🧱 Architecture
L’application suit le modèle **MVC (Modèle-Vue-Contrôleur)** d’ASP.NET Core.

### Structure des Dossiers
- `Controllers/` : Gère les requêtes HTTP (ex. `ArticleController`, `HomeController`).  
- `Models/` : Définit les entités de données (ex. `Article`, `ApplicationUser`, `Commentaire`).  
- `Views/` : Contient les vues Razor pour le rendu HTML.  
- `Data/` : Gestion des données avec **Entity Framework Core** (migrations incluses).  
- `Areas/Identity/` : Pages d’authentification et gestion des utilisateurs.  
- `Components/` : Composants de vue réutilisables (ex. `CategorieMenuViewComponent`).  
- `wwwroot/` : Ressources statiques (CSS, JavaScript, images).

---

## 🗄️ Base de Données
- Utilise **Entity Framework Core** avec **SQL Server**.  
- Les migrations permettent d’initialiser et de mettre à jour le schéma.  
- Contexte principal : `ApplicationDbContext`.

---

## 🧰 Technologies Utilisées
| Type | Technologies |
|------|---------------|
| Langages | C#, JavaScript |
| Frameworks | ASP.NET Core MVC, Entity Framework Core |
| Front-End | Bootstrap, jQuery |
| Base de données | SQL Server |

---

## 🚀 Installation et Configuration

### Prérequis
- [`.NET 8.0 SDK`](https://dotnet.microsoft.com/)  
- **SQL Server** ou une base de données compatible.

### Étapes d’installation
1. **Cloner le dépôt :**
