from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel
from dotenv import load_dotenv
import os

from langchain_groq import ChatGroq

from services.redaction_service import RedactionService
from services.generation_service import GenerationService
from services.destinataire_service import DestinataireService


load_dotenv()


# ==========================================================
# APPLICATION
# ==========================================================

app = FastAPI(
    title="Bureau d'Ordre Digital - AI Service"
)


# ==========================================================
# CORS
# ==========================================================

app.add_middleware(
    CORSMiddleware,
    allow_origins=["http://localhost:5173"],
    allow_methods=["*"],
    allow_headers=["*"],
)


# ==========================================================
# LLM
# ==========================================================
llm = ChatGroq(
    model="openai/gpt-oss-20b",
    api_key=os.getenv("GROQ_API_KEY"),
    temperature=0.3
)

# ==========================================================
# SERVICES IA
# ==========================================================

redaction_service = RedactionService(llm)

generation_service = GenerationService(llm)

destinataire_service = DestinataireService(llm)


# ==========================================================
# DTO
# ==========================================================

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
    libelle: str


class DestinataireRequest(BaseModel):
    texte: str
    servicesDisponibles: list[ServiceDestinataireDto]


# ==========================================================
# ROUTE REDACTION
# ==========================================================

@app.post("/assistant/redaction")
def redaction(payload: AmeliorerRequest):

    suggestion = redaction_service.ameliorer(
        payload.texte,
        payload.champ
    )

    return {
        "suggestion": suggestion
    }


# ==========================================================
# ROUTE GENERATION
# ==========================================================

@app.post("/assistant/generation")
def generation(payload: GenererRequest):

    resultat = generation_service.generer(
        payload.texteLibre,
        [t.model_dump() for t in payload.typesDisponibles]
    )

    return resultat


# ==========================================================
# ROUTE DESTINATAIRE
# ==========================================================

@app.post("/assistant/destinataire")
def destinataire(payload: DestinataireRequest):

    resultat = destinataire_service.analyser(
        payload.texte,
        [s.model_dump() for s in payload.servicesDisponibles]
    )

    return resultat


# ==========================================================
# HEALTH
# ==========================================================

@app.get("/health")
def health():

    return {
        "status": "ok"
    }