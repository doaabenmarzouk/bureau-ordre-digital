using System.IdentityModel.Tokens.Jwt;
using BureauOrdreDigital.Api.Modules.Utilisateur.DTOs;
using BureauOrdreDigital.Api.Modules.Utilisateur.Services;
using BureauOrdreDigital.Api.Shared.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BureauOrdreDigital.Api.Modules.Utilisateur.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly AppDbContext _context ;
    
    public AuthController(IAuthService authService , AppDbContext context)
    {
        _authService = authService;
        _context = context;
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

        return Content("""
        <!DOCTYPE html>
        <html lang="fr">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>Bureau d'Ordre Digital - Email vérifié</title>
        </head>

        <body style="
            margin: 0;
            font-family: Arial, sans-serif;
            background-color: #f4f6f8;
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
        ">

            <div style="
                width: 90%;
                max-width: 500px;
                background-color: white;
                padding: 45px;
                border-radius: 16px;
                text-align: center;
                box-shadow: 0 10px 35px rgba(0,0,0,0.08);
            ">

                <div style="
                    width: 75px;
                    height: 75px;
                    margin: 0 auto 25px;
                    border-radius: 50%;
                    background-color: #dcfce7;
                    color: #16a34a;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    font-size: 40px;
                    font-weight: bold;
                ">
                    ✓
                </div>

                <h1 style="
                    color: #1e3a8a;
                    margin-bottom: 10px;
                    font-size: 26px;
                ">
                    Bureau d'Ordre Digital
                </h1>

                <h2 style="
                    color: #111827;
                    font-size: 21px;
                    margin-bottom: 15px;
                ">
                    Email vérifié avec succès
                </h2>

                <p style="
                    color: #6b7280;
                    font-size: 15px;
                    line-height: 1.7;
                ">
                    Votre adresse email a été vérifiée avec succès.
                    Votre compte est maintenant activé.
                </p>

                <div style="
                    margin-top: 30px;
                    padding: 15px;
                    background-color: #f0fdf4;
                    border-radius: 8px;
                    color: #166534;
                    font-size: 14px;
                ">
                    Vous pouvez maintenant vous connecter
                    à votre compte.
                </div>

                <p style="
                    color: #9ca3af;
                    font-size: 12px;
                    margin-top: 30px;
                ">
                    Vous pouvez fermer cette page.
                </p>

                <hr style="
                    border: 0;
                    border-top: 1px solid #e5e7eb;
                    margin: 30px 0;
                ">

                <p style="
                    color: #9ca3af;
                    font-size: 11px;
                ">
                    © 2026 Bureau d'Ordre Digital
                </p>

            </div>

        </body>
        </html>
        """, "text/html");
    }
    catch (InvalidOperationException)
    {
        return Content("""
        <!DOCTYPE html>
        <html lang="fr">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>Bureau d'Ordre Digital - Erreur</title>
        </head>

        <body style="
            margin: 0;
            font-family: Arial, sans-serif;
            background-color: #f4f6f8;
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
        ">

            <div style="
                width: 90%;
                max-width: 500px;
                background-color: white;
                padding: 45px;
                border-radius: 16px;
                text-align: center;
                box-shadow: 0 10px 35px rgba(0,0,0,0.08);
            ">

                <div style="
                    width: 75px;
                    height: 75px;
                    margin: 0 auto 25px;
                    border-radius: 50%;
                    background-color: #fee2e2;
                    color: #dc2626;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    font-size: 35px;
                    font-weight: bold;
                ">
                    !
                </div>

                <h1 style="color: #bcdb26;">
                    Bureau d'Ordre Digital
                </h1>

                <h2 style="color: #dc2626;">
                    Vérification impossible
                </h2>

                <p style="
                    color: #6b7280;
                    line-height: 1.7;
                ">
                    Le lien de vérification est invalide
                    ou a déjà été utilisé.
                </p>

            </div>

        </body>
        </html>
        """, "text/html");
    }
}

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        try
        {
            var reponse = await _authService.Login(dto);

            return Ok(reponse);
        }
        catch (InvalidOperationException ex)
        {
            return Unauthorized(new
            {
                message = ex.Message
            });
        }
    }
    [HttpPost("logout")]
    [Authorize]
public async Task<IActionResult> Logout()
{
       var jti = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
       var exp = User.FindFirst(JwtRegisteredClaimNames.Exp)?.Value;
       if (string.IsNullOrEmpty(jti) || string.IsNullOrEmpty(exp))
    {
        return BadRequest(new { message = "Token invalide" });
    }

        _context.RevokedTokens.Add(new RevokedToken
    {
        TokenId = jti,
        ExpirationDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(exp)).DateTime
    });
    await _context.SaveChangesAsync();

    return Ok(new { message = "Déconnecté avec succès" });
}
}