using BureauOrdreDigital.Api.Modules.Courrier.DTOs;
using BureauOrdreDigital.Api.Modules.Courrier.Models;
using BureauOrdreDigital.Api.Shared.Data;
using BureauOrdreDigital.Api.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace BureauOrdreDigital.Api.Modules.Courrier.Services;

public class CourrierService : ICourrierService
{
    private readonly AppDbContext _context;
    private readonly IHistoriqueService _historiqueService;
    private readonly IEmailService _emailService;
    private readonly IStockageService _stockageService;

    private static readonly string[] FormatsAutorises =
    {
        "PDF",
        "JPEG",
        "JPG",
        "PNG",
        "DOCX",
        "XLSX"
    };

    private const long TailleMaxParFichier = 5 * 1024 * 1024;   // 5 Mo
    private const long TailleMaxTotale = 20 * 1024 * 1024;      // 20 Mo

    public CourrierService(
        AppDbContext context,
        IHistoriqueService historiqueService,
        IEmailService emailService,
        IStockageService stockageService)
    {
        _context = context;
        _historiqueService = historiqueService;
        _emailService = emailService;
        _stockageService = stockageService;
    }

    // ============================================================
    // 1. SOUMETTRE UN COURRIER
    // ============================================================

    public async Task<CourrierDetailDto> SoumettreCourrier(
        Guid utilisateurId,
        CourrierCreateDto dto,
        List<IFormFile>? fichiers)
    {
        // --------------------------------------------------------
        // UTILISATEUR
        // --------------------------------------------------------

        var utilisateur = await _context.Utilisateurs
            .FirstOrDefaultAsync(u => u.Id == utilisateurId)
            ?? throw new InvalidOperationException(
                "Utilisateur introuvable.");

        // --------------------------------------------------------
        // TYPE DE COURRIER
        // --------------------------------------------------------

        var typeCourrier = await _context.TypesCourrier
            .FirstOrDefaultAsync(t => t.Id == dto.TypeCourrierId)
            ?? throw new InvalidOperationException(
                "Type de courrier introuvable.");

        // --------------------------------------------------------
        // ORGANISME DESTINATAIRE
        // --------------------------------------------------------

        var organisme = await _context.Organismes
            .FirstOrDefaultAsync(
                o => o.Id == dto.OrganismeId && o.Actif)
            ?? throw new InvalidOperationException(
                "Organisme destinataire introuvable ou inactif.");

        // --------------------------------------------------------
        // STATUT INITIAL
        // --------------------------------------------------------

        var statutRecu = await _context.StatutsCourrier
            .FirstOrDefaultAsync(s => s.Code == "RECU")
            ?? throw new InvalidOperationException(
                "Statut RECU introuvable.");

        // --------------------------------------------------------
        // VALIDATION DES FICHIERS
        // --------------------------------------------------------

        if (fichiers is not null && fichiers.Count > 0)
        {
            ValiderFichiers(fichiers);
        }

        // --------------------------------------------------------
        // GENERATION DU NUMERO DE REFERENCE
        // --------------------------------------------------------

        var numReference =
            $"BOD-{DateTime.UtcNow.Year}-" +
            $"{Guid.NewGuid().ToString()[..8].ToUpper()}";

        // --------------------------------------------------------
        // CREATION DU COURRIER
        // --------------------------------------------------------

        var courrier = new Models.Courrier
        {
            UtilisateurId = utilisateurId,
            TypeCourrierId = dto.TypeCourrierId,

            // Nouveau système :
            // Le courrier est directement lié à l'organisme.
            OrganismeId = dto.OrganismeId,

            StatutId = statutRecu.Id,
            NumReference = numReference,
            Objet = dto.Objet,
            Description = dto.Description,
            DateSoumission = DateTime.UtcNow,
            DateDerniereMaj = DateTime.UtcNow
        };

        await _context.Courriers.AddAsync(courrier);

        await _context.SaveChangesAsync();

        // --------------------------------------------------------
        // HISTORIQUE
        // --------------------------------------------------------

        await _historiqueService.AjouterAction(
            courrier.Id,
            statutRecu.Id,
            "Création du courrier");

        // --------------------------------------------------------
        // PIECES JOINTES
        // --------------------------------------------------------

        var piecesJointesDto = new List<PieceJointeDto>();

        if (fichiers is not null && fichiers.Count > 0)
        {
            foreach (var fichier in fichiers)
            {
                var urlStockage =
                    await _stockageService.SauvegarderFichier(fichier);

                var extension = Path
                    .GetExtension(fichier.FileName)
                    .TrimStart('.')
                    .ToUpper();

                var pieceJointe = new PieceJointe
                {
                    CourrierId = courrier.Id,
                    NomFichier = fichier.FileName,
                    UrlStockage = urlStockage,
                    TailleOctets = fichier.Length,
                    Format = extension,
                    DateUpload = DateTime.UtcNow
                };

                await _context.PiecesJointes.AddAsync(pieceJointe);

                piecesJointesDto.Add(new PieceJointeDto
                {
                    Id = pieceJointe.Id,
                    NomFichier = pieceJointe.NomFichier,
                    UrlStockage = pieceJointe.UrlStockage,
                    TailleOctets = pieceJointe.TailleOctets,
                    Format = pieceJointe.Format,
                    DateUpload = pieceJointe.DateUpload
                });
            }

            await _context.SaveChangesAsync();
        }

        // --------------------------------------------------------
        // EMAIL D'ACCUSÉ DE RÉCEPTION
        // --------------------------------------------------------

        await _emailService.SendEmailAsync(
            utilisateur.Email,
            "Accusé de réception de votre courrier",
            $"""
            <h2>Accusé de réception</h2>

            <p>Bonjour,</p>

            <p>
                Votre courrier a bien été reçu
                par le Bureau d'Ordre Digital.
            </p>

            <p>
                <strong>Numéro de référence :</strong>
                {courrier.NumReference}
            </p>

            <p>
                <strong>Objet :</strong>
                {courrier.Objet}
            </p>

            <p>
                <strong>Organisme destinataire :</strong>
                {organisme.Nom}
            </p>

            <p>Merci.</p>
            """
        );

        // --------------------------------------------------------
        // REPONSE
        // --------------------------------------------------------

        return new CourrierDetailDto
        {
            Id = courrier.Id,
            NumReference = courrier.NumReference,
            Objet = courrier.Objet,
            Description = courrier.Description,
            TypeCourrier = typeCourrier.Libelle,

            // Nouveau :
            OrganismeName = organisme.Nom,

            Statut = statutRecu.Libelle,
            DateSoumission = courrier.DateSoumission,
            PiecesJointes = piecesJointesDto
        };
    }

    // ============================================================
    // VALIDATION DES FICHIERS
    // ============================================================

    private void ValiderFichiers(List<IFormFile> fichiers)
    {
        long tailleTotale = 0;

        foreach (var fichier in fichiers)
        {
            var extension = Path
                .GetExtension(fichier.FileName)
                .TrimStart('.')
                .ToUpper();

            // ----------------------------------------------------
            // FORMAT
            // ----------------------------------------------------

            if (!FormatsAutorises.Contains(extension))
            {
                throw new InvalidOperationException(
                    $"Format non autorisé : {extension}. " +
                    "Formats acceptés : PDF, JPEG, PNG, DOCX, XLSX.");
            }

            // ----------------------------------------------------
            // TAILLE INDIVIDUELLE
            // ----------------------------------------------------

            if (fichier.Length > TailleMaxParFichier)
            {
                throw new InvalidOperationException(
                    $"Le fichier {fichier.FileName} " +
                    "dépasse la taille maximale de 5 Mo.");
            }

            // ----------------------------------------------------
            // TAILLE TOTALE
            // ----------------------------------------------------

            tailleTotale += fichier.Length;
        }

        if (tailleTotale > TailleMaxTotale)
        {
            throw new InvalidOperationException(
                "La taille totale des pièces jointes dépasse 20 Mo.");
        }
    }

    // ============================================================
    // 2. GET MES COURRIERS
    // ============================================================

    public async Task<List<CourrierListItemDto>> GetMesCourriers(
        Guid utilisateurId)
    {
        // --------------------------------------------------------
        // VERIFICATION UTILISATEUR
        // --------------------------------------------------------

        var utilisateurExiste =
            await _context.Utilisateurs
                .AnyAsync(u => u.Id == utilisateurId);

        if (!utilisateurExiste)
        {
            throw new InvalidOperationException(
                "Utilisateur introuvable.");
        }

        // --------------------------------------------------------
        // LISTE DES COURRIERS
        // --------------------------------------------------------

        return await _context.Courriers
            .Where(c => c.UtilisateurId == utilisateurId)

            .Include(c => c.StatutCourrier)

            .OrderByDescending(c => c.DateSoumission)

            .Select(c => new CourrierListItemDto
            {
                Id = c.Id,

                NumReference = c.NumReference,

                Objet = c.Objet,

                Statut = c.StatutCourrier!.Libelle,

                DateSoumission = c.DateSoumission,

                // Nouveau :
                OrganismeName = c.Organisme!.Nom
            })

            .ToListAsync();
    }

    // ============================================================
    // 3. GET COURRIER BY ID
    // ============================================================

    public async Task<CourrierDetailDto> GetCourrierById(
        Guid utilisateurId,
        Guid courrierId)
    {
        // --------------------------------------------------------
        // RECUPERATION DU COURRIER
        // --------------------------------------------------------

        var courrier = await _context.Courriers
            .Include(c => c.TypeCourrier)

            .Include(c => c.Organisme)

            .Include(c => c.StatutCourrier)

            .FirstOrDefaultAsync(
                c =>
                    c.Id == courrierId &&
                    c.UtilisateurId == utilisateurId)

            ?? throw new InvalidOperationException(
                "Courrier introuvable.");

        // --------------------------------------------------------
        // HISTORIQUE
        // --------------------------------------------------------

        var historique = await _context.HistoriqueActions
            .Where(h => h.CourrierId == courrier.Id)

            .Include(h => h.StatutCourrier)

            .OrderByDescending(h => h.DateAction)

            .Select(h => new HistoriqueActionDto
            {
                Id = h.Id,

                Description = h.Description,

                DateAction = h.DateAction,

                Statut = h.StatutCourrier!.Libelle
            })

            .ToListAsync();

        // --------------------------------------------------------
        // PIECES JOINTES
        // --------------------------------------------------------

        var piecesJointes = await _context.PiecesJointes
            .Where(p => p.CourrierId == courrier.Id)

            .Select(p => new PieceJointeDto
            {
                Id = p.Id,

                NomFichier = p.NomFichier,

                UrlStockage = p.UrlStockage,

                TailleOctets = p.TailleOctets,

                Format = p.Format,

                DateUpload = p.DateUpload
            })

            .ToListAsync();

        // --------------------------------------------------------
        // REPONSE
        // --------------------------------------------------------

        return new CourrierDetailDto
        {
            Id = courrier.Id,

            NumReference = courrier.NumReference,

            Objet = courrier.Objet,

            Description = courrier.Description,

            TypeCourrier = courrier.TypeCourrier!.Libelle,

            // Nouveau :
            OrganismeName = courrier.Organisme!.Nom,

            Statut = courrier.StatutCourrier!.Libelle,

            DateSoumission = courrier.DateSoumission,

            Historique = historique,

            PiecesJointes = piecesJointes
        };
    }

    // ============================================================
    // 4. RECHERCHER LES COURRIERS
    // ============================================================

    public async Task<List<CourrierListItemDto>> RechercherCourriers(
        Guid utilisateurId,
        string motCle)
    {
        // --------------------------------------------------------
        // VERIFICATION UTILISATEUR
        // --------------------------------------------------------

        var utilisateurExiste =
            await _context.Utilisateurs
                .AnyAsync(u => u.Id == utilisateurId);

        if (!utilisateurExiste)
        {
            throw new InvalidOperationException(
                "Utilisateur introuvable.");
        }

        // --------------------------------------------------------
        // NETTOYAGE DU MOT-CLE
        // --------------------------------------------------------

        motCle = motCle.Trim();

        // --------------------------------------------------------
        // SI MOT-CLE VIDE
        // --------------------------------------------------------

        if (string.IsNullOrWhiteSpace(motCle))
        {
            return await GetMesCourriers(utilisateurId);
        }

        // --------------------------------------------------------
        // RECHERCHE
        // --------------------------------------------------------

        return await _context.Courriers
            .Where(c =>
                c.UtilisateurId == utilisateurId &&
                c.Objet.Contains(motCle))

            .Include(c => c.StatutCourrier)

            .OrderByDescending(c => c.DateSoumission)

            .Select(c => new CourrierListItemDto
            {
                Id = c.Id,

                NumReference = c.NumReference,

                Objet = c.Objet,

                Statut = c.StatutCourrier!.Libelle,

                DateSoumission = c.DateSoumission,

                // Nouveau :
                OrganismeName = c.Organisme!.Nom
            })

            .ToListAsync();
    }
}