using System.Security.Claims;
using BureauOrdreDigital.Api.Modules.Courrier.DTOs;
using BureauOrdreDigital.Api.Modules.Courrier.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BureauOrdreDigital.Api.Modules.Courrier.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CourrierController : ControllerBase
{
    private readonly ICourrierService _courrierService;

    public CourrierController(ICourrierService courrierService)
    {
        _courrierService = courrierService;
    }

    private Guid GetUtilisateurIdDepuisToken()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new InvalidOperationException("Utilisateur non authentifié.");

        return Guid.Parse(idClaim);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> SoumettreCourrier(
        [FromForm] CourrierCreateDto dto,
        [FromForm] List<IFormFile>? fichiers)
    {
        try
        {
            var utilisateurId = GetUtilisateurIdDepuisToken();

            var courrier = await _courrierService.SoumettreCourrier(
                utilisateurId,
                dto,
                fichiers
            );

            return Ok(courrier);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}