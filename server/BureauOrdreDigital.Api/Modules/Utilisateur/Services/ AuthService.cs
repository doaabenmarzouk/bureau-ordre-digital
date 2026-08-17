using BureauOrdreDigital.Api.Modules.Utilisateur.DTOs;
using BureauOrdreDigital.Api.Modules.Utilisateur.Models;
using BureauOrdreDigital.Api.Shared.Data;
using BureauOrdreDigital.Api.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace BureauOrdreDigital.Api.Modules.Utilisateur.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly EmailService _emailService;

    public AuthService(AppDbContext context, EmailService emailService)
    {
        _context = context;
        _emailService = emailService;
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

    public async Task<Models.Utilisateur> Login(LoginDto dto)
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

        return utilisateur;
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
}