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
    private readonly EmailService _emailService;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, EmailService emailService, IConfiguration configuration)
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

        _emailService.EnvoyerEmailVerification(utilisateur.Email, utilisateur.TokenVerification!);

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
}