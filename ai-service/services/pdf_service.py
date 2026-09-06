import pymupdf


def extraire_texte_pdf(
    contenu_bytes: bytes,
    max_chars: int = 6000
) -> str:
    """
    Extrait le texte d'un PDF fourni sous forme de bytes.

    Le texte extrait est limité à max_chars caractères
    afin d'éviter d'envoyer un document trop volumineux
    au modèle IA.
    """

    doc = pymupdf.open(
        stream=contenu_bytes,
        filetype="pdf"
    )

    texte = ""

    try:
        for page in doc:

            texte_page = page.get_text("text")

            if not texte_page:
                continue

            # Nombre de caractères restant à récupérer
            restant = max_chars - len(texte)

            if restant <= 0:
                break

            # Ajouter seulement la quantité nécessaire
            texte += texte_page[:restant]

            # Arrêter dès qu'on atteint la limite
            if len(texte) >= max_chars:
                break

    finally:
        doc.close()

    return texte.strip()