using System.Security.Claims;
using BureauOrdreDigital.Api.Modules.Courrier.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BureauOrdreDigital.Api.Modules.Courrier.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SuiviController : ControllerBase
{
    private readonly ICourrierService _courrierService;

    public SuiviController(ICourrierService courrierService)
    {
        _courrierService = courrierService;
    }

    private Guid GetUtilisateurIdDepuisToken()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new InvalidOperationException("Utilisateur non authentifié.");

        return Guid.Parse(idClaim);
    }

    [HttpGet]
    public async Task<IActionResult> GetMesCourriers()
    {
        try
        {
            var utilisateurId = GetUtilisateurIdDepuisToken();
            var courriers = await _courrierService.GetMesCourriers(utilisateurId);

            return Ok(courriers);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCourrierById(Guid id)
    {
        try
        {
            var utilisateurId = GetUtilisateurIdDepuisToken();
            var courrier = await _courrierService.GetCourrierById(utilisateurId, id);

            return Ok(courrier);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("rechercher")]
    public async Task<IActionResult> RechercherCourriers([FromQuery] string motCle)
    {
        try
        {
            var utilisateurId = GetUtilisateurIdDepuisToken();
            var courriers = await _courrierService.RechercherCourriers(utilisateurId, motCle);

            return Ok(courriers);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}