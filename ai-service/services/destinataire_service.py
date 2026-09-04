from typing import List, Dict, Any

from langchain_core.prompts import ChatPromptTemplate
from langchain_core.output_parsers import JsonOutputParser


class DestinataireService:

    def __init__(self, llm):
        self.llm = llm
        self.parser = JsonOutputParser()

        self.prompt = ChatPromptTemplate.from_messages([
            (
                "system",
                """
Tu es un assistant administratif chargé d'orienter les citoyens
vers le service administratif approprié.

Ta mission est de déterminer le service destinataire le plus pertinent
à partir du texte fourni.

Le texte peut provenir :
- d'une description saisie par le citoyen ;
- du contenu extrait d'un PDF.

Analyse les mots-clés et surtout le contexte global de la demande.

Règles :
- Ne crée jamais un service qui n'existe pas dans la liste.
- Utilise uniquement les services disponibles.
- Si plusieurs services sont possibles, choisis le plus pertinent.
- Si les informations sont insuffisantes, donne une confiance faible.
- Ne fais aucune action à la place du citoyen.
- Ne soumets aucun courrier.
- Retourne uniquement un JSON valide.

Format exact :

{{
    "serviceDestinataireId": "ID",
    "confidence": 0,
    "reason": "Explication courte du choix"
}}
"""
            ),
            (
                "user",
                """
Services disponibles :

{services}

Texte de la demande :

{texte}
"""
            )
        ])

        self.chain = self.prompt | self.llm | self.parser

    def analyser(
        self,
        texte: str,
        services_disponibles: List[Dict[str, Any]]
    ) -> Dict[str, Any]:

        services_text = "\n".join(
            f"- ID: {service['id']} | Libellé: {service['libelle']}"
            for service in services_disponibles
        )

        resultat = self.chain.invoke({
            "services": services_text,
            "texte": texte
        })

        # Vérification que l'ID retourné existe
        ids_valides = {
            str(service["id"])
            for service in services_disponibles
        }

        service_id = str(
            resultat.get("serviceDestinataireId")
        )

        if service_id not in ids_valides:
            resultat["serviceDestinataireId"] = None
            resultat["confidence"] = 0
            resultat["reason"] = (
                "Le service retourné par l'IA "
                "n'existe pas dans les services disponibles."
            )

        return resultat