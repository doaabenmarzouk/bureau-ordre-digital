import pymupdf


def extraire_texte_pdf(contenu_bytes: bytes) -> str:
    """
    Extrait le texte brut d'un PDF fourni sous forme de bytes
    (contenu déjà en mémoire, pas un chemin de fichier).
    """

    doc = pymupdf.open(stream=contenu_bytes, filetype="pdf")

    texte = ""
    for page in doc:
        texte += page.get_text()

    doc.close()

    return texte.strip()
 