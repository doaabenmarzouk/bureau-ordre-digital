using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using BureauOrdreDigital.Api.Modules.Assistant.DTOs;
using BureauOrdreDigital.Api.Modules.Assistant.Services;
using BureauOrdreDigital.Api.Modules.Courrier.Services;
using BureauOrdreDigital.Api.Shared.Data;

namespace BureauOrdreDigital.Api.Modules.Assistant.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AssistantController : ControllerBase
{
    private readonly IAssistantIaService _assistantIaService;
    private readonly IOrganismeService _organismeService;
    private readonly AppDbContext _context;

    public AssistantController(
        IAssistantIaService assistantIaService,
        IOrganismeService organismeService,
        AppDbContext context)
    {
        _assistantIaService = assistantIaService;
        _organismeService = organismeService;
        _context = context;
    }

    // ==========================================================
    // REDACTION
    // ==========================================================

    [HttpPost("redaction")]
    public async Task<IActionResult> Redaction(
        [FromBody] RedactionRequestDto request)
    {
        var resultat =
            await _assistantIaService.AmeliorerRedactionAsync(request);

        if (resultat is null)
        {
            return StatusCode(
                502,
                new
                {
                    message = "Le service IA n'a pas répondu correctement."
                });
        }

        return Ok(resultat);
    }

    // ==========================================================
    // GENERATION
    // ==========================================================

    [HttpPost("generation")]
    public async Task<IActionResult> Generation(
        [FromBody] GenerationRequestDto request)
    {
        var typesDisponibles = await _context.TypesCourrier
            .Select(t => new AiTypeCourrierDto
            {
                Id = t.Id,
                Libelle = t.Libelle
            })
            .ToListAsync();

        if (typesDisponibles.Count == 0)
        {
            return BadRequest(
                new
                {
                    message = "Aucun type de courrier disponible."
                });
        }

        var resultat =
            await _assistantIaService.GenererAsync(
                request,
                typesDisponibles);

        if (resultat is null)
        {
            return StatusCode(
                502,
                new
                {
                    message = "Le service IA n'a pas répondu correctement."
                });
        }

        return Ok(resultat);
    }

    // ==========================================================
    // DESTINATAIRE
    // ==========================================================

    [HttpPost("destinataire")]
    public async Task<IActionResult> Destinataire(
        [FromForm] DestinataireRequestDto request)
    {
        Console.WriteLine("===== DESTINATAIRE =====");
        Console.WriteLine($"Texte : {request.Texte}");
        Console.WriteLine($"Fichier : {request.Fichier?.FileName}");
        Console.WriteLine($"Taille : {request.Fichier?.Length}");
        Console.WriteLine($"ContentType : {request.Fichier?.ContentType}");

        // ------------------------------------------------------
        // 1. Récupérer les organismes actifs
        // ------------------------------------------------------

        var organismes =
            await _organismeService.RechercherOrganismes(
                ville: null,
                type: null,
                motCle: null);

        if (organismes.Count == 0)
        {
            return BadRequest(
                new
                {
                    message = "Aucun organisme destinataire disponible."
                });
        }

        // ------------------------------------------------------
        // 2. Préparer les organismes pour l'IA
        // ------------------------------------------------------

        var organismesDisponibles = organismes
            .Select(o => new AiOrganismeDto
            {
                Id = o.Id,
                Nom = o.Nom,
                NomAbrege = o.NomAbrege,
                Ville = o.Ville,
                Type = o.Type
            })
            .ToList();

        // ------------------------------------------------------
        // 3. Demander à l'IA de choisir l'organisme
        // ------------------------------------------------------

        var resultat =
            await _assistantIaService.SuggererDestinataireAsync(
                request,
                organismesDisponibles);

        if (resultat is null)
        {
            return StatusCode(
                502,
                new
                {
                    message = "Le service IA n'a pas répondu correctement."
                });
        }

        // ------------------------------------------------------
        // 4. Vérification de sécurité
        // ------------------------------------------------------

        if (resultat.OrganismeId.HasValue)
        {
            var idExiste = organismesDisponibles.Any(
                o => o.Id == resultat.OrganismeId.Value);

            if (!idExiste)
            {
                resultat.OrganismeId = null;
                resultat.Confidence = 0;
                resultat.Reason =
                    "L'IA a proposé un organisme qui n'existe pas " +
                    "dans la liste des organismes disponibles.";
            }
        }

        return Ok(resultat);
    }
}