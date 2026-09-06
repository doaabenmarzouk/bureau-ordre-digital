from typing import List, Dict, Any

from langchain_core.prompts import ChatPromptTemplate
from langchain_core.output_parsers import JsonOutputParser


class DestinataireService:

    def __init__(self, llm):

        self.llm = llm

        # ======================================================
        # PARSER JSON
        # ======================================================

        self.parser = JsonOutputParser()

        # ======================================================
        # PROMPT
        # ======================================================

        self.prompt = ChatPromptTemplate.from_messages([

            (
                "system",
                """
Tu es un système intelligent d'orientation
des courriers administratifs.

Ta mission principale est de choisir le service
destinataire le plus pertinent pour une demande citoyenne.

Tu dois prendre une décision uniquement parmi les services
qui te sont fournis.

==================================================
RÈGLE PRINCIPALE
==================================================

Tu dois choisir UN service parmi la liste
"Services disponibles".

Tu dois retourner EXACTEMENT son ID.

L'ID retourné doit être celui du service que tu
as réellement sélectionné.

Tu ne dois jamais inventer un ID.

Tu ne dois jamais inventer un service.

Tu ne dois jamais inventer un organisme.

==================================================
COMMENT CHOISIR
==================================================

Analyse la demande du citoyen en fonction de :

1. Sujet principal de la demande
2. Objet administratif
3. Mots-clés
4. Type de démarche
5. Organisme concerné
6. Service concerné
7. Ville
8. Contexte général du courrier

Compare ensuite la demande avec TOUS les services
disponibles.

Choisis celui qui correspond le mieux
au besoin administratif du citoyen.

==================================================
COHÉRENCE OBLIGATOIRE
==================================================

Avant de répondre, vérifie :

- que l'ID existe dans la liste ;
- que le service correspondant à cet ID est bien
  celui que tu as choisi ;
- que ton explication correspond à ce service ;
- que ton choix est cohérent avec la demande.

IMPORTANT :

Si tu choisis :

ID 1
Service État Civil
Commune de Bizerte

alors tu dois obligatoirement retourner :

"serviceDestinataireId": 1

Tu ne dois jamais retourner un autre ID.

==================================================
CAS SANS CORRESPONDANCE
==================================================

Si aucun service disponible ne correspond
réellement à la demande :

"serviceDestinataireId": null

Ne choisis pas un service simplement parce
qu'il est vaguement proche.

==================================================
CONFIANCE
==================================================

confidence est un nombre entre 0 et 1.

Exemples :

0.95 = très forte confiance
0.80 = forte confiance
0.60 = confiance moyenne
0.30 = faible confiance
0.00 = aucune correspondance

==================================================
REASON
==================================================

reason doit expliquer brièvement pourquoi
le service choisi correspond à la demande.

==================================================
FORMAT OBLIGATOIRE
==================================================

Retourne UNIQUEMENT un JSON valide.

Exemple :

{{
    "serviceDestinataireId": 1,
    "confidence": 0.98,
    "reason": "La demande concerne un acte de naissance, qui relève du Service État Civil."
}}

Si aucune correspondance :

{{
    "serviceDestinataireId": null,
    "confidence": 0.0,
    "reason": "Aucun service disponible ne correspond suffisamment à la demande."
}}
"""
            ),

            (
                "user",
                """
SERVICES DISPONIBLES :

{services}

==================================================

DEMANDE DU CITOYEN :

{texte}

==================================================

Analyse la demande.

Compare-la avec TOUS les services disponibles.

Choisis le service destinataire le plus pertinent.

Retourne son ID EXACT parmi les IDs fournis.

Si aucun service ne correspond réellement,
retourne null.

Retourne uniquement le JSON.
"""
            )
        ])

        # ======================================================
        # CHAIN
        # ======================================================

        self.chain = (
            self.prompt
            | self.llm
            | self.parser
        )

    # ==========================================================
    # ANALYSER
    # ==========================================================

    def analyser(
        self,
        texte: str,
        services_disponibles: List[Dict[str, Any]]
    ) -> Dict[str, Any]:

        # ======================================================
        # 1. VÉRIFIER LE TEXTE
        # ======================================================

        if not texte or not texte.strip():

            return {
                "serviceDestinataireId": None,
                "confidence": 0.0,
                "reason": "Aucun texte exploitable n'a été fourni."
            }

        # ======================================================
        # 2. VÉRIFIER LES SERVICES
        # ======================================================

        if not services_disponibles:

            return {
                "serviceDestinataireId": None,
                "confidence": 0.0,
                "reason": "Aucun service destinataire disponible."
            }

        # ======================================================
        # 3. PRÉPARER LES SERVICES POUR L'IA
        # ======================================================

        services_lignes = []

        for service in services_disponibles:

            service_id = service.get("id")
            nom_organisme = service.get("nomOrganisme")
            nom_service = service.get("nomService")
            ville = service.get("ville")
            type_organisme = service.get("typeOrganisme")

            services_lignes.append(
                f"- ID: {service_id} | "
                f"Organisme: {nom_organisme} | "
                f"Service: {nom_service} | "
                f"Ville: {ville} | "
                f"Type: {type_organisme}"
            )

        services_text = "\n".join(services_lignes)

        # ======================================================
        # 4. APPEL IA
        # ======================================================

        try:

            resultat = self.chain.invoke({
                "services": services_text,
                "texte": texte.strip()
            })

        except Exception as e:

            return {
                "serviceDestinataireId": None,
                "confidence": 0.0,
                "reason": f"Erreur IA : {str(e)}"
            }

        # ======================================================
        # 5. VÉRIFIER LE TYPE DE RÉSULTAT
        # ======================================================

        if not isinstance(resultat, dict):

            return {
                "serviceDestinataireId": None,
                "confidence": 0.0,
                "reason": "Réponse IA invalide."
            }

        # ======================================================
        # 6. RÉCUPÉRER L'ID
        # ======================================================

        service_id_raw = resultat.get(
            "serviceDestinataireId"
        )

        # ======================================================
        # 7. CAS NULL
        # ======================================================

        if service_id_raw is None:

            confidence_raw = resultat.get(
                "confidence",
                0.0
            )

            try:
                confidence = float(confidence_raw)
            except (TypeError, ValueError):
                confidence = 0.0

            confidence = max(
                0.0,
                min(1.0, confidence)
            )

            reason = resultat.get(
                "reason",
                "Aucun service correspondant."
            )

            if not isinstance(reason, str):
                reason = str(reason)

            return {
                "serviceDestinataireId": None,
                "confidence": confidence,
                "reason": reason.strip()
            }

        # ======================================================
        # 8. CONVERTIR L'ID
        # ======================================================

        try:

            service_id = int(service_id_raw)

        except (TypeError, ValueError):

            return {
                "serviceDestinataireId": None,
                "confidence": 0.0,
                "reason": "L'IA a retourné un ID invalide."
            }

        # ======================================================
        # 9. VÉRIFIER QUE L'ID EXISTE
        # ======================================================

        service_selectionne = None

        for service in services_disponibles:

            try:

                current_id = int(
                    service.get("id")
                )

            except (TypeError, ValueError):

                continue

            if current_id == service_id:

                service_selectionne = service
                break

        # ======================================================
        # 10. ID INEXISTANT
        # ======================================================

        if service_selectionne is None:

            return {
                "serviceDestinataireId": None,
                "confidence": 0.0,
                "reason": (
                    "L'IA a retourné un ID qui "
                    "n'existe pas parmi les services disponibles."
                )
            }

        # ======================================================
        # 11. RÉCUPÉRER LA CONFIDENCE
        # ======================================================

        confidence_raw = resultat.get(
            "confidence",
            0.0
        )

        try:

            confidence = float(
                confidence_raw
            )

        except (TypeError, ValueError):

            confidence = 0.0

        # Limiter entre 0 et 1

        confidence = max(
            0.0,
            min(1.0, confidence)
        )

        # ======================================================
        # 12. RÉCUPÉRER LA JUSTIFICATION
        # ======================================================

        reason = resultat.get(
            "reason",
            "Aucune justification fournie."
        )

        if not isinstance(reason, str):

            reason = str(reason)

        reason = reason.strip()

        # ======================================================
        # 13. RETOUR FINAL
        # ======================================================

        return {
            "serviceDestinataireId": service_id,
            "confidence": confidence,
            "reason": reason
        }