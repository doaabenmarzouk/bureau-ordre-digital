from typing import List, Dict, Any

from langchain_core.prompts import ChatPromptTemplate
from langchain_core.output_parsers import JsonOutputParser


class GenerationService:

    def __init__(self, llm):
        self.llm = llm
        self.parser = JsonOutputParser()

        self.prompt = ChatPromptTemplate.from_messages([
            (
                "system",
                "Tu es un assistant spécialisé dans la rédaction de courriers "
                "administratifs. À partir du texte libre fourni par le citoyen, "
                "prépare un courrier administratif clair, précis et "
                "professionnel.\n\n"
                "Ta mission est de proposer :\n"
                "1. Le type de courrier le plus approprié parmi les types "
                "disponibles.\n"
                "2. Un objet court et précis.\n"
                "3. Une description administrative claire et fidèle à la "
                "demande.\n\n"
                "Règles :\n"
                "- Ne change jamais le sens de la demande du citoyen.\n"
                "- N'invente aucune information.\n"
                "- N'ajoute aucun fait qui n'est pas présent dans la demande.\n"
                "- Le type doit obligatoirement être choisi parmi la liste "
                "fournie ci-dessous, en te basant sur son ID réel.\n"
                "- L'ID doit toujours être retourné comme une valeur "
                "numérique JSON (sans guillemets autour), quelle que soit "
                "sa valeur.\n"
                "- L'objet doit être court et explicite.\n"
                "- La description doit être professionnelle mais rester "
                "fidèle aux informations fournies.\n"
                "- Le format de sortie doit être un JSON valide avec "
                "exactement 3 clés : typeCourrierId (nombre), objet "
                "(texte), description (texte)."
            ),
            (
                "user",
                "Types disponibles :\n{types_disponibles}\n\n"
                "Texte de l'utilisateur :\n{texte_libre}"
            )
        ])

        self.chain = self.prompt | self.llm | self.parser

    def generer(
        self,
        texte_libre: str,
        types_disponibles: List[Dict[str, Any]]
    ) -> Dict[str, Any]:

        types_text = "\n".join(
            f"- ID: {type_['id']} | Libellé: {type_['libelle']}"
            for type_ in types_disponibles
        )

        resultat = self.chain.invoke({
            "texte_libre": texte_libre,
            "types_disponibles": types_text
        })

        ids_valides = {str(t["id"]) for t in types_disponibles}
        type_id_brut = str(resultat.get("typeCourrierId", ""))

        if type_id_brut in ids_valides:
            type_id = int(type_id_brut)
        else:
            type_id = None

        return {
            "typeCourrierId": type_id,
            "objet": resultat.get("objet", ""),
            "description": resultat.get("description", "")
        }