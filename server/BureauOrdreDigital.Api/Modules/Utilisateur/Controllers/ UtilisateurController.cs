using System.Security.Claims;
using BureauOrdreDigital.Api.Modules.Utilisateur.DTOs;
using BureauOrdreDigital.Api.Modules.Utilisateur.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BureauOrdreDigital.Api.Modules.Utilisateur.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UtilisateurController : ControllerBase
{
    private readonly IUtilisateurService _utilisateurService;

    public UtilisateurController(IUtilisateurService utilisateurService)
    {
        _utilisateurService = utilisateurService;
    }

    private Guid GetUtilisateurIdDepuisToken()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new InvalidOperationException("Utilisateur non authentifié.");

        return Guid.Parse(idClaim);
    }

    [HttpGet("moi")]
    public async Task<IActionResult> GetProfil()
    {
        try
        {
            var id = GetUtilisateurIdDepuisToken();
            var profil = await _utilisateurService.GetProfil(id);
            return Ok(profil);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPut("moi")]
    public async Task<IActionResult> UpdateProfil(ProfilUpdateDto dto)
    {
        try
        {
            var id = GetUtilisateurIdDepuisToken();
            var profil = await _utilisateurService.UpdateProfil(id, dto);
            return Ok(profil);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}