using BureauOrdreDigital.Api.Modules.Utilisateur.DTOs;
using BureauOrdreDigital.Api.Modules.Utilisateur.Services;
using Microsoft.AspNetCore.Mvc;

namespace BureauOrdreDigital.Api.Modules.Utilisateur.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        try
        {
            await _authService.Register(dto);

            return Ok(new
            {
                message = "Compte créé. Vérifiez votre email pour l'activer."
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpGet("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromQuery] string token)
    {
        try
        {
            await _authService.VerifyEmail(token);

            return Ok(new
            {
                message = "Email vérifié avec succès. Vous pouvez maintenant vous connecter."
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        try
        {
            var utilisateur = await _authService.Login(dto);

            return Ok(new
            {
                utilisateur.Id,
                utilisateur.Nom,
                utilisateur.Prenom,
                utilisateur.Email
            });
        }
        catch (InvalidOperationException ex)
        {
            return Unauthorized(new
            {
                message = ex.Message
            });
        }
    }
}