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
    private readonly IServiceDestinataireService _serviceDestinataireService;
    private readonly AppDbContext _context;

    public AssistantController(
        IAssistantIaService assistantIaService,
        IServiceDestinataireService serviceDestinataireService,
        AppDbContext context)
    {
        _assistantIaService = assistantIaService;
        _serviceDestinataireService = serviceDestinataireService;
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

    {   Console.WriteLine("===== DESTINATAIRE =====");
    Console.WriteLine($"Texte : {request.Texte}");
    Console.WriteLine($"Fichier : {request.Fichier?.FileName}");
    Console.WriteLine($"Taille : {request.Fichier?.Length}");
    Console.WriteLine($"ContentType : {request.Fichier?.ContentType}");
        // ------------------------------------------------------
        // 1. Récupérer les services actifs depuis la DB
        // ------------------------------------------------------

        var services =
            await _serviceDestinataireService.RechercherServices(
                ville: null,
                type: null,
                motCle: null);

        if (services.Count == 0)
        {
            return BadRequest(
                new
                {
                    message = "Aucun service destinataire disponible."
                });
        }

        // ------------------------------------------------------
        // 2. Préparer les données envoyées à l'IA
        // ------------------------------------------------------

        var servicesDisponibles = services
            .Select(s => new AiServiceDto
            {
                Id = s.Id,
                NomOrganisme = s.NomOrganisme,
                NomService = s.NomService,
                Ville = s.Ville,
                TypeOrganisme = s.TypeOrganisme
            })
            .ToList();

        // ------------------------------------------------------
        // 3. Demander à l'IA de choisir
        // ------------------------------------------------------

        var resultat =
            await _assistantIaService.SuggererDestinataireAsync(
                request,
                servicesDisponibles);

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
        //
        // L'IA choisit l'ID.
        // .NET vérifie simplement que cet ID existe.
        // ------------------------------------------------------

        if (resultat.ServiceDestinataireId.HasValue)
        {
            var idExiste = servicesDisponibles.Any(
                s => s.Id == resultat.ServiceDestinataireId.Value);

            if (!idExiste)
            {
                resultat.ServiceDestinataireId = null;
                resultat.Confidence = 0;
                resultat.Reason =
                    "L'IA a proposé un service qui n'existe pas " +
                    "dans la liste des services disponibles.";
            }
        }

        return Ok(resultat);
    }
}