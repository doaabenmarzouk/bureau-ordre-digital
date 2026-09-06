from fastapi import FastAPI, UploadFile, File, Form
from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel

from dotenv import load_dotenv

import os
import json

from langchain_groq import ChatGroq

from services.redaction_service import RedactionService
from services.generation_service import GenerationService
from services.destinataire_service import DestinataireService
from services.pdf_service import extraire_texte_pdf


# ==============================================================
# ENVIRONNEMENT
# ==============================================================

load_dotenv()


# ==============================================================
# APPLICATION
# ==============================================================

app = FastAPI(
    title="Bureau d'Ordre Digital - AI Service"
)


# ==============================================================
# CORS
# ==============================================================

app.add_middleware(
    CORSMiddleware,
    allow_origins=["http://localhost:5173"],
    allow_methods=["*"],
    allow_headers=["*"],
)


# ==============================================================
# LLM
# ==============================================================

llm = ChatGroq(
    model="openai/gpt-oss-20b",
    api_key=os.getenv("GROQ_API_KEY"),
    temperature=0.1
)


# ==============================================================
# SERVICES IA
# ==============================================================

redaction_service = RedactionService(llm)

generation_service = GenerationService(llm)

destinataire_service = DestinataireService(llm)


# ==============================================================
# DTO
# ==============================================================

class AmeliorerRequest(BaseModel):
    texte: str
    champ: str


class TypeCourrierDto(BaseModel):
    id: int
    libelle: str


class GenererRequest(BaseModel):
    texteLibre: str
    typesDisponibles: list[TypeCourrierDto]


class ServiceDestinataireDto(BaseModel):
    id: int
    nomOrganisme: str
    nomService: str
    ville: str = ""
    typeOrganisme: str = ""


# ==============================================================
# REDACTION
# ==============================================================

@app.post("/assistant/redaction")
def redaction(payload: AmeliorerRequest):

    suggestion = redaction_service.ameliorer(
        payload.texte,
        payload.champ
    )

    return {
        "suggestion": suggestion
    }


# ==============================================================
# GENERATION
# ==============================================================

@app.post("/assistant/generation")
def generation(payload: GenererRequest):

    resultat = generation_service.generer(
        payload.texteLibre,
        [
            t.model_dump()
            for t in payload.typesDisponibles
        ]
    )

    return resultat


# ==============================================================
# DESTINATAIRE
# ==============================================================

@app.post("/assistant/destinataire")
async def destinataire(
    texte: str = Form(""),
    servicesDisponibles: str = Form(...),
    fichier: UploadFile | None = File(None)
):

    # ----------------------------------------------------------
    # 1. Récupérer la liste des services
    # ----------------------------------------------------------

    try:

        services_list = json.loads(
            servicesDisponibles
        )

    except json.JSONDecodeError:

        return {
            "serviceDestinataireId": None,
            "confidence": 0,
            "reason": "La liste des services est invalide."
        }


    # ----------------------------------------------------------
    # 2. Texte saisi par le citoyen
    # ----------------------------------------------------------

    # Limite à 3000 caractères
    texte_final = (texte or "").strip()[:3000]


    # ----------------------------------------------------------
    # 3. PDF facultatif
    # ----------------------------------------------------------

    if fichier is not None:

        contenu = await fichier.read()

        # Vérifier la taille du fichier
        MAX_FILE_SIZE = 5 * 1024 * 1024

        if len(contenu) > MAX_FILE_SIZE:

            return {
                "serviceDestinataireId": None,
                "confidence": 0,
                "reason": "Le PDF dépasse la taille maximale de 5 Mo."
            }


        try:

            # Extraction limitée à 6000 caractères
            texte_pdf = extraire_texte_pdf(
                contenu,
                max_chars=6000
            )

        except Exception as e:

            print(
                f"Erreur extraction PDF : {e}"
            )

            return {
                "serviceDestinataireId": None,
                "confidence": 0,
                "reason": "Impossible de lire le PDF."
            }


        # ------------------------------------------------------
        # Ajouter le contenu du PDF
        # ------------------------------------------------------

        if texte_pdf:

            if texte_final:

                texte_final += "\n\n"

            texte_final += (
                "Contenu du document :\n"
                + texte_pdf
            )


    # ----------------------------------------------------------
    # 4. Vérifier qu'il y a quelque chose à analyser
    # ----------------------------------------------------------

    if not texte_final.strip():

        return {
            "serviceDestinataireId": None,
            "confidence": 0,
            "reason": "Aucun texte ou contenu PDF exploitable."
        }


    # ----------------------------------------------------------
    # 5. Logs
    # ----------------------------------------------------------

    print("==========================================")
    print("ASSISTANT DESTINATAIRE")
    print("==========================================")

    print(
        f"Nombre de services : {len(services_list)}"
    )

    print(
        f"Nombre de caractères envoyés au LLM : "
        f"{len(texte_final)}"
    )

    print(
        f"PDF reçu : {fichier is not None}"
    )

    print("==========================================")


    # ----------------------------------------------------------
    # 6. Appel IA
    # ----------------------------------------------------------

    resultat = destinataire_service.analyser(
        texte_final,
        services_list
    )


    # ----------------------------------------------------------
    # 7. Retour
    # ----------------------------------------------------------

    return resultat


# ==============================================================
# HEALTH CHECK
# ==============================================================

@app.get("/health")
def health():

    return {
        "status": "ok"
    }