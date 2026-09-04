from langchain_core.prompts import ChatPromptTemplate


class RedactionService:

    def __init__(self, llm):
        self.llm = llm

        self.prompt = ChatPromptTemplate.from_messages([
            (
                "system",
                "Tu es un assistant spécialisé dans la correction et "
                "la reformulation de textes administratifs.\n\n"

                "OBJECTIF :\n"
                "Améliorer uniquement la forme du texte fourni par le citoyen, "
                "sans jamais ajouter ou modifier son contenu.\n\n"

                "RÈGLES STRICTES :\n"
                "- Corriger uniquement l'orthographe et la grammaire.\n"
                "- Améliorer la formulation et la clarté.\n"
                "- Utiliser un vocabulaire professionnel et administratif.\n"
                "- Conserver exactement les informations du texte original.\n"
                "- Ne jamais inventer une information.\n"
                "- Ne jamais ajouter un détail absent du texte original.\n"
                "- Ne jamais faire de supposition ou de déduction.\n"
                "- Ne jamais ajouter de contexte juridique ou administratif.\n"
                "- Ne jamais ajouter de normes, règlements ou procédures.\n"
                "- Ne jamais ajouter de justification ou de motif.\n"
                "- Ne jamais modifier les faits.\n"
                "- Ne jamais modifier la demande du citoyen.\n"
                "- Ne jamais supprimer une information importante.\n"
                "- Si le texte est court, rester très proche du texte original.\n"
                "- Pour une description, améliorer uniquement la rédaction "
                "sans ajouter de contenu.\n"
                "- Pour un objet, utiliser maximum 15 mots et conserver "
                "uniquement les informations originales.\n\n"

                "IMPORTANT :\n"
                "La réponse doit contenir uniquement des informations "
                "présentes dans le texte original. Une réponse simple "
                "et fidèle est préférable à une réponse plus élaborée "
                "qui ajoute des informations.\n\n"

                "Retourne uniquement le texte final, sans explication, "
                "sans commentaire et sans guillemets."
            ),
            (
                "user",
                "Champ à améliorer : {champ}\n\n"
                "Texte original : {texte}"
            )
        ])

        self.chain = self.prompt | self.llm

    def ameliorer(self, texte: str, champ: str) -> str:

        instruction_champ = (
            "objet (maximum 15 mots, sans ajouter d'information)"
            if champ == "objet"
            else "description (claire et professionnelle, "
                 "sans ajouter d'information)"
        )

        resultat = self.chain.invoke({
            "texte": texte,
            "champ": instruction_champ
        })

        return resultat.content.strip()
    