# 🏛️ Bureau d'Ordre Digital

Application web de gestion digitalisée des courriers citoyens permettant la **soumission, le traitement et le suivi des correspondances administratives en ligne**.

Projet réalisé dans le cadre d'un stage chez **NeoLedge MEA**, sous la supervision de **Ali BEN YAHIA**.

---

## 📌 Contexte

Le projet s'inscrit dans une démarche de **digitalisation des services publics** afin de simplifier les démarches administratives des citoyens, améliorer la traçabilité des courriers et faciliter leur traitement par les services concernés.

Une fonctionnalité d'**Intelligence Artificielle** est également intégrée afin d'assister le citoyen dans la préparation de son courrier.

---

## ✨ Fonctionnalités

### 👤 Gestion des utilisateurs

- Inscription avec vérification de l'adresse email
- Authentification sécurisée
- Gestion du profil utilisateur
- Gestion de session
- Déconnexion

### 📄 Gestion des courriers

- Soumission de courriers en ligne
- Sélection du type de courrier
- Sélection du service destinataire
- Saisie de l'objet et de la description
- Ajout de pièces jointes
- Validation des informations
- Génération automatique d'un accusé de réception

### 🔎 Suivi des courriers

- Consultation des courriers soumis
- Recherche par mots-clés
- Consultation du détail d'un courrier
- Historique des modifications
- Chronologie des statuts
- Notifications par email

### 🤖 Assistant Citoyen IA

L'Assistant Citoyen aide l'utilisateur à préparer son courrier administratif à partir de différentes sources :

- 💬 Description libre de son problème
- 📄 Document PDF
- 📄 + 💬 Document PDF accompagné d'une description

L'IA analyse les informations fournies et peut proposer :

```text
Type de courrier
Service destinataire
Objet
Description
Résumé du document

Le citoyen conserve le contrôle de sa demande et peut modifier les suggestions de l'IA avant de soumettre le courrier.

Exemple
Citoyen :

"J'ai reçu ce document et je ne sais pas à quel service
je dois m'adresser."

                ↓

          🤖 Assistant IA

                ↓

Type de courrier :
Demande d'information

Service destinataire :
Service concerné

Objet :
Demande d'informations concernant le dossier

Description :
Générée à partir des informations fournies.

🧠 Fonctionnement de l'Assistant IA

L'Assistant Citoyen suit le processus suivant :
              👤 Citoyen
                  │
        ┌─────────┴─────────┐
        │                   │
        ▼                   ▼
   💬 Description       📄 Document PDF
        │                   │
        └─────────┬─────────┘
                  │
                  ▼
           Microservice IA
               FastAPI
                  │
                  ▼
          Extraction du texte
                  │
                  ▼
             Analyse IA
                  │
        ┌─────────┼─────────┐
        ▼         ▼         ▼
      Type     Service     Objet
    courrier  destinataire   │
        │         │         │
        └─────────┼─────────┘
                  ▼
             Description
                  │
                  ▼
          🤖 Suggestions IA
                  │
                  ▼
           👤 Validation
                  │
                  ▼
            ✏️ Modification
                  │
                  ▼
             📤 Soumission

L'IA fournit des recommandations et ne soumet pas automatiquement le courrier.

Le citoyen peut vérifier et modifier les informations proposées avant l'envoi.

🏗️ Architecture

Le backend suit une architecture Modular Monolith.

Chaque fonctionnalité principale est organisée dans un module indépendant avec ses propres :

Controllers
Services
Models
DTOs

Le module IA est externalisé sous la forme d'un microservice Python basé sur FastAPI.

Le backend ASP.NET Core communique avec le microservice IA via une API REST.
                         ┌──────────────────────┐
                         │      Citoyen         │
                         │       Vue.js         │
                         └──────────┬───────────┘
                                    │
                                    │ REST API
                                    ▼
                  ┌────────────────────────────────┐
                  │        ASP.NET Core .NET 10    │
                  │                                │
                  │       Modular Monolith         │
                  │                                │
                  │  ┌────────────┐ ┌────────────┐│
                  │  │ Utilisateur│ │  Courrier  ││
                  │  └────────────┘ └────────────┘│
                  │                                │
                  │  ┌────────────┐ ┌────────────┐│
                  │  │    Suivi   │ │     IA     ││
                  │  └────────────┘ └──────┬─────┘│
                  └────────────────────────┼──────┘
                                           │
                                           │ REST API
                                           ▼
                              ┌──────────────────────┐
                              │   Microservice IA    │
                              │       FastAPI        │
                              │                      │
                              │ PDF + Description    │
                              │          ↓           │
                              │       Analyse        │
                              │          ↓           │
                              │     Suggestions      │
                              └──────────┬───────────┘
                                         │
                                         ▼
                              ┌──────────────────────┐
                              │      LLM / IA        │
                              └──────────────────────┘


🛠️ Stack technique
Domaine	Technologie
Frontend	Vue.js
Backend	ASP.NET Core .NET 10
Base de données	SQL Server
ORM	Entity Framework Core
Authentification	JWT
Microservice IA	Python + FastAPI
Analyse PDF	PyMuPDF
OCR	Tesseract OCR
IA / LLM	OpenAI API
Orchestration IA	LangChain
Vector Database	ChromaDB
Embeddings	OpenAI Embeddings
Communication	REST API
Conteneurisation	Docker
Gestion de versions	Git / GitHub
Documentation	PlantUML
📁 Structure du projet
BureauOrdreDigital/
│
├── BureauOrdreDigital.Api/
│   │
│   ├── Modules/
│   │   │
│   │   ├── Utilisateur/
│   │   │   ├── Controllers/
│   │   │   ├── Services/
│   │   │   ├── Models/
│   │   │   └── DTOs/
│   │   │
│   │   ├── Courrier/
│   │   │   ├── Controllers/
│   │   │   ├── Services/
│   │   │   ├── Models/
│   │   │   └── DTOs/
│   │   │
│   │   ├── Suivi/
│   │   │   ├── Controllers/
│   │   │   ├── Services/
│   │   │   ├── Models/
│   │   │   └── DTOs/
│   │   │
│   │   └── IA/
│   │       ├── Services/
│   │       ├── DTOs/
│   │       └── Clients/
│   │
│   ├── Shared/
│   │   ├── Data/
│   │   ├── Services/
│   │   └── Middlewares/
│   │
│   └── Program.cs
│
├── client/
│   ├── src/
│   ├── package.json
│   └── ...
│
├── services-ia/
│   │
│   └── classification/
│       ├── app/
│       ├── services/
│       ├── models/
│       ├── requirements.txt
│       └── main.py
│
├── docs/
│   └── bureau_ordre_diagrammes.puml
│
├── .gitignore
└── README.md
🔐 Sécurité

Le projet intègre plusieurs mécanismes de sécurité :

Authentification par JWT
Hashage sécurisé des mots de passe
Vérification de l'adresse email
Protection des endpoints nécessitant une authentification
Gestion de la déconnexion
Révocation des tokens
Validation des données reçues
Validation des fichiers uploadés
Limitation de la taille des pièces jointes
Restriction des extensions de fichiers autorisées
Protection des informations sensibles
Utilisation des User Secrets et variables d'environnement

Les clés API, mots de passe et autres informations sensibles ne doivent jamais être enregistrés directement dans le dépôt Git.

📎 Gestion des pièces jointes

Les formats autorisés sont :

PDF
JPEG
JPG
PNG
DOCX
XLSX

Limites :

Taille maximale par fichier : 5 MB
Taille maximale totale : 20 MB
📋 Prérequis

Avant de lancer le projet, installer :

.NET 10 SDK
Node.js 20+
Python 3.11+
Docker Desktop
Git

Installer également Entity Framework Core CLI :

dotnet tool install --global dotnet-ef
🗄️ Base de données — SQL Server

Le projet utilise SQL Server via Docker.

Télécharger l'image :

docker pull mcr.microsoft.com/mssql/server:2022-latest

Créer le conteneur :

docker run \
  -e "ACCEPT_EULA=Y" \
  -e "MSSQL_SA_PASSWORD=VotreMotDePasse123!" \
  -p 1433:1433 \
  --name sqlserver \
  -d mcr.microsoft.com/mssql/server:2022-latest

Vérifier que le conteneur fonctionne :

docker ps

Puis créer la base de données à partir du script SQL du projet.

⚙️ Installation du Backend

Se placer dans le dossier backend :

cd BureauOrdreDigital.Api

Restaurer les dépendances :

dotnet restore

Configurer les variables sensibles avec User Secrets ou des variables d'environnement.

Lancer le backend :

dotnet run
💻 Installation du Frontend

Se placer dans le dossier frontend :

cd client

Installer les dépendances :

npm install

Lancer le serveur de développement :

npm run dev
🤖 Installation du microservice IA

Se placer dans le dossier du microservice :

cd services-ia/classification

Créer un environnement virtuel :

python -m venv venv

Sous Windows :

venv\Scripts\activate

Installer les dépendances :

pip install -r requirements.txt

Lancer FastAPI :

uvicorn main:app --reload
🔌 Communication Backend / IA

Le backend .NET joue le rôle d'intermédiaire entre le frontend et le microservice IA.

Le microservice peut recevoir :

PDF : optionnel
Description du problème : optionnelle

Le citoyen peut donc :

PDF
ou
Description
ou
PDF + Description

Le service IA analyse les informations et retourne les suggestions au backend.

Vue.js
   │
   │ POST /api/assistant/analyze
   ▼
ASP.NET Core
   │
   │ HTTP Request
   ▼
FastAPI
   │
   ├── PDF
   └── Description
          │
          ▼
       LLM / IA
          │
          ▼
      Analyse
          │
          ▼
     Suggestions
          │
          ▼
ASP.NET Core
          │
          ▼
       Vue.js
🧠 Évolution de l'IA

Le module IA est conçu pour évoluer progressivement.

Analyse de texte
Description du citoyen
        ↓
      LLM
        ↓
Type + Service + Objet + Description
Analyse de PDF
PDF
 ↓
Extraction du texte
 ↓
Analyse IA
 ↓
Suggestions
Analyse combinée
PDF + Description
       ↓
Analyse combinée
       ↓
      LLM
       ↓
Suggestions
RAG

Une base documentaire pourra être utilisée pour améliorer les recommandations concernant les services administratifs.

Documents administratifs
          ↓
       Embeddings
          ↓
        ChromaDB
          ↓
Recherche sémantique
          ↓
          LLM
          ↓
Réponse / recommandation
📊 Exemple de résultat IA

L'Assistant peut retourner une réponse structurée :

{
  "typeCourrier": "Demande d'information",
  "serviceDestinataire": "Service administratif",
  "objet": "Demande d'informations concernant mon dossier",
  "description": "Je souhaite obtenir des informations concernant l'état de mon dossier et connaître les démarches nécessaires.",
  "resume": "La demande concerne le suivi d'un dossier administratif.",
  "confiance": 0.91
}

Les informations proposées sont ensuite affichées dans le formulaire de courrier afin que le citoyen puisse les vérifier et les modifier.

📐 Diagrammes

Les diagrammes de conception sont disponibles dans :

/docs/bureau_ordre_diagrammes.puml

Les diagrammes comprennent notamment :

Diagramme de cas d'utilisation
Diagrammes de séquence
Diagramme de classes
Diagramme d'architecture
Workflow de l'Assistant IA


## Auteur

**Doaa Ben Marzouk** — ENICarthage, stagiaire NeoLedge MEA
[GitHub](https://github.com/douabenmarzouk) · [LinkedIn](https://www.linkedin.com/in/doua-ben-marzouk)