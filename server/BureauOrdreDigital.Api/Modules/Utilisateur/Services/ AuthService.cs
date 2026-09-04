using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BureauOrdreDigital.Api.Modules.Utilisateur.DTOs;
using BureauOrdreDigital.Api.Modules.Utilisateur.Models;
using BureauOrdreDigital.Api.Shared.Data;
using BureauOrdreDigital.Api.Shared.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BureauOrdreDigital.Api.Modules.Utilisateur.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
  
    private readonly IConfiguration _configuration;

    private readonly IEmailService _emailService;

public AuthService(AppDbContext context, IEmailService emailService, IConfiguration configuration)
{
    _context = context;
    _emailService = emailService;
    _configuration = configuration;
}

    public async Task<Models.Utilisateur> Register(RegisterDto dto)
    {
        bool emailExiste = await _context.Utilisateurs
            .AnyAsync(u => u.Email == dto.Email);

        if (emailExiste)
        {
            throw new InvalidOperationException("Cet email est déjà utilisé.");
        }

        var utilisateur = new Models.Utilisateur
        {
            Nom = dto.Nom,
            Prenom = dto.Prenom,
            Email = dto.Email,
            Telephone = dto.Telephone,
            MotDePasseHash = BCrypt.Net.BCrypt.HashPassword(dto.MotDePasse),
            TokenVerification = Guid.NewGuid().ToString(),
            DateCreation = DateTime.UtcNow
        };

        _context.Utilisateurs.Add(utilisateur);
        await _context.SaveChangesAsync();
        var lien =
    $"http://localhost:5029/api/auth/verify-email?token={utilisateur.TokenVerification}";
var corps = ConstruireTemplateVerification(lien);

await _emailService.SendEmailAsync(utilisateur.Email, "Vérifiez votre adresse email", corps);

        return utilisateur;
    }

    public async Task<LoginResponseDto> Login(LoginDto dto)
    {
        var utilisateur = await _context.Utilisateurs
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (utilisateur is null)
        {
            throw new InvalidOperationException("Email ou mot de passe incorrect.");
        }

        bool motDePasseValide = BCrypt.Net.BCrypt.Verify(dto.MotDePasse, utilisateur.MotDePasseHash);

        if (!motDePasseValide)
        {
            throw new InvalidOperationException("Email ou mot de passe incorrect.");
        }

        if (!utilisateur.EmailVerifie)
        {
            throw new InvalidOperationException(
                "Veuillez vérifier votre adresse email avant de vous connecter."
            );
        }

        var token = GenererToken(utilisateur);

        return new LoginResponseDto
{
               Id = utilisateur.Id,
               Nom = utilisateur.Nom,
               Prenom = utilisateur.Prenom,
               Email = utilisateur.Email,
               Telephone = utilisateur.Telephone,
               Token = token
        };
    }

    public async Task VerifyEmail(string token)
    {
        var utilisateur = await _context.Utilisateurs
            .FirstOrDefaultAsync(u => u.TokenVerification == token);

        if (utilisateur is null)
        {
            throw new InvalidOperationException("Token invalide.");
        }

        utilisateur.EmailVerifie = true;
        utilisateur.TokenVerification = null;

        await _context.SaveChangesAsync();
    }
    public async Task ForgotPassword(ForgotPasswordDto dto)
{
    var utilisateur = await _context.Utilisateurs
        .FirstOrDefaultAsync(u => u.Email == dto.Email);

    if (utilisateur is null)
    {
        return;
    }

    var token = Guid.NewGuid().ToString("N");

    utilisateur.TokenResetPassword = token;
    utilisateur.TokenResetExpiration = DateTime.UtcNow.AddMinutes(30);

    await _context.SaveChangesAsync();

    var lien =
        $"http://localhost:5173/reset-password?token={token}";

    var corps = ConstruireTemplateResetPassword(lien);

    await _emailService.SendEmailAsync(
        utilisateur.Email,
        "Réinitialisation de votre mot de passe",
        corps
    );
}
public async Task ResetPassword(ResetPasswordDto dto)
{
    if (dto.NouveauMotDePasse != dto.ConfirmationMotDePasse)
    {
        throw new InvalidOperationException(
            "Les mots de passe ne correspondent pas."
        );
    }

    var utilisateur = await _context.Utilisateurs
        .FirstOrDefaultAsync(
            u => u.TokenResetPassword == dto.Token
        );

    if (utilisateur is null)
    {
        throw new InvalidOperationException(
            "Lien de réinitialisation invalide."
        );
    }

    if (utilisateur.TokenResetExpiration < DateTime.UtcNow)
    {
        throw new InvalidOperationException(
            "Le lien de réinitialisation a expiré."
        );
    }

    utilisateur.MotDePasseHash =
        BCrypt.Net.BCrypt.HashPassword(dto.NouveauMotDePasse);

    utilisateur.TokenResetPassword = null;
    utilisateur.TokenResetExpiration = null;

    await _context.SaveChangesAsync();
}
    private string GenererToken(Models.Utilisateur utilisateur)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, utilisateur.Id.ToString()),
            new(ClaimTypes.Email, utilisateur.Email)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expirationMinutes = double.Parse(_configuration["Jwt:ExpirationMinutes"]!);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
  private static string ConstruireTemplateVerification(string lien)
{
    return $$"""
    <div style="font-family: Arial, sans-serif; background-color: #f4f6f8; padding: 40px 20px;">
        <div style="max-width: 560px; margin: 0 auto; background-color: #ffffff; border-radius: 12px; padding: 40px; text-align: center; border: 1px solid #e5e7eb;">

            <h2 style="color: #91a91c; margin-bottom: 20px;">
                Bureau d'Ordre Digital
            </h2>

            <h3 style="color: #111827; font-size: 22px; margin-bottom: 15px;">
                Vérification de votre adresse email
            </h3>

            <p style="color: #4b5563; font-size: 15px; line-height: 1.7;">
                Bienvenue sur le Bureau d'Ordre Digital.
                Pour activer votre compte, veuillez confirmer votre adresse email.
            </p>

            <a href="{{lien}}"
               style="display: inline-block;
                      margin: 25px 0;
                      padding: 14px 30px;
                      background-color: #1e3a8a;
                      color: #ffffff;
                      text-decoration: none;
                      border-radius: 7px;
                      font-weight: bold;
                      font-size: 14px;">
                Vérifier mon email
            </a>

            <p style="color: #6b7280; font-size: 13px; line-height: 1.6; margin-top: 25px;">
                Ce lien est personnel et vous permet d'activer votre compte.
            </p>

            <p style="color: #9ca3af; font-size: 12px; line-height: 1.6; margin-top: 25px;">
                Si vous n'êtes pas à l'origine de cette demande, vous pouvez ignorer cet email.
            </p>

            <hr style="border: 0; border-top: 1px solid #e5e7eb; margin: 30px 0;">

            <p style="color: #9ca3af; font-size: 11px;">
                © 2026 Bureau d'Ordre Digital
            </p>

        </div>
    </div>
    """;
}
private static string ConstruireTemplateResetPassword(string lien)
{
    return $$"""
    <div style="
        font-family: Arial, sans-serif;
        background-color: #f4f6f8;
        padding: 40px 20px;
    ">

        <div style="
            max-width: 560px;
            margin: 0 auto;
            background-color: #ffffff;
            border-radius: 12px;
            padding: 40px;
            text-align: center;
            border: 1px solid #e5e7eb;
        ">

            <h2 style="
                color: #91a91c;
                margin-bottom: 20px;
            ">
                Bureau d'Ordre Digital
            </h2>

            <h3 style="
                color: #111827;
                font-size: 22px;
                margin-bottom: 15px;
            ">
                Réinitialisation du mot de passe
            </h3>

            <p style="
                color: #4b5563;
                font-size: 15px;
                line-height: 1.7;
            ">
                Vous avez demandé la réinitialisation de votre mot de passe.
            </p>

            <p style="
                color: #4b5563;
                font-size: 15px;
                line-height: 1.7;
            ">
                Cliquez sur le bouton ci-dessous pour créer un nouveau mot de passe.
            </p>

            <a href="{{lien}}"
               style="
                   display: inline-block;
                   margin: 25px 0;
                   padding: 14px 30px;
                   background-color: #1e3a8a;
                   color: #ffffff;
                   text-decoration: none;
                   border-radius: 7px;
                   font-weight: bold;
                   font-size: 14px;
               ">
                Réinitialiser mon mot de passe
            </a>

            <p style="
                color: #6b7280;
                font-size: 13px;
                line-height: 1.6;
            ">
                Ce lien est valable pendant 30 minutes.
            </p>

            <p style="
                color: #9ca3af;
                font-size: 12px;
                line-height: 1.6;
            ">
                Si vous n'avez pas demandé cette réinitialisation,
                vous pouvez simplement ignorer cet email.
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

    </div>
    """;
}
}