# Bureau d'Ordre Digital

Application de gestion digitalisée des courriers citoyens, permettant la soumission et le suivi des correspondances avec l'administration en ligne.

Projet réalisé dans le cadre d'un stage chez **NeoLedge MEA**, sous la supervision d'Ali BEN YAHIA.


## Contexte

Le développement de cette application s'inscrit dans le cadre de la modernisation et de la digitalisation des services publics, visant à améliorer l'efficacité administrative et la transparence pour les citoyens.

## Fonctionnalités

- **Gestion des utilisateurs** — inscription avec vérification email, authentification, gestion du profil
- **Soumission de courrier** — formulaire multi-étapes (service destinataire, type, objet, description, pièces jointes), génération automatique d'un accusé de réception
- **Suivi de courrier** — historique, recherche par mots-clés, détail avec chronologie des statuts, notifications par email
- **Classification automatique** *(module IA)* — suggestion du type de courrier à partir de la description, via un microservice dédié

## Stack technique

| Couche | Technologie |
|---|---|
| Frontend | Vue.js |
| Backend | ASP.NET Core (.NET 10) |
| Base de données | SQL Server |
| Microservice IA | Python, FastAPI |
| Conteneurisation | Docker |
| Gestion de versions | Git / GitHub |

## Architecture

Le projet suit une **architecture modulaire (modular monolith)** : chaque fonctionnalité est isolée dans son propre module (Controllers, Services, Models), avec un socle commun partagé (accès aux données, middlewares). Le module IA est externalisé en microservice Python indépendant, appelé en REST par le backend .NET.

Cette organisation permet d'ajouter facilement de nouvelles fonctionnalités sans impacter les modules existants — exigence explicite du cahier des charges (section 3.4, Architecture modulaire).

```
BureauOrdreDigital.Api/
├── Modules/
│   ├── Courrier/
│   ├── Utilisateur/
│   ├── Suivi/
│   └── IA/                  (client HTTP vers le microservice)
├── Shared/
│   ├── Data/
│   └── Middlewares/
└── Program.cs

services-ia/
└── classification/           (FastAPI)
```

## Prérequis

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [Entity Framework Core CLI](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) (`dotnet tool install --global dotnet-ef`)

## Installation

### Base de données (SQL Server via Docker)

```bash
docker pull mcr.microsoft.com/mssql/server:2022-latest
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=VotreMotDePasse123!" \
  -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
```

Exécuter ensuite le script `bureau_ordre_schema.sql` pour créer les tables.

### Backend

```bash
cd BureauOrdreDigital.Api
dotnet restore
dotnet run
```

### Frontend

```bash
cd bureau-ordre-frontend
npm install
npm run dev
```

## Diagrammes

Les diagrammes de conception (cas d'utilisation, séquences, classes, architecture) sont disponibles au format PlantUML dans `/docs/bureau_ordre_diagrammes.puml`.

## Statut du projet

🚧 En cours de développement — Phase 1 (analyse et conception) complétée.

## Auteur

**Doaa Ben Marzouk** — ENICarthage, stagiaire NeoLedge MEA
[GitHub](https://github.com/douabenmarzouk) · [LinkedIn](https://www.linkedin.com/in/doua-ben-marzouk)