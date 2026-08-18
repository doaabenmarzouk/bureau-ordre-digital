using BureauOrdreDigital.Api.Modules.Utilisateur.DTOs;
using BureauOrdreDigital.Api.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace BureauOrdreDigital.Api.Modules.Utilisateur.Services;

public class UtilisateurService : IUtilisateurService
{
    private readonly AppDbContext _context;

    public UtilisateurService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UtilisateurResponseDto> GetProfil(Guid id)
    {
        var utilisateur = await _context.Utilisateurs.FindAsync(id)
            ?? throw new InvalidOperationException("Utilisateur introuvable.");

        return new UtilisateurResponseDto
        {
            Id = utilisateur.Id,
            Nom = utilisateur.Nom,
            Prenom = utilisateur.Prenom,
            Email = utilisateur.Email,
            Telephone = utilisateur.Telephone,
            EmailVerifie = utilisateur.EmailVerifie
        };
    }

    public async Task<UtilisateurResponseDto> UpdateProfil(Guid id, ProfilUpdateDto dto)
    {
        var utilisateur = await _context.Utilisateurs.FindAsync(id)
            ?? throw new InvalidOperationException("Utilisateur introuvable.");

        bool emailPrisParAutre = await _context.Utilisateurs
            .AnyAsync(u => u.Email == dto.Email && u.Id != id);

        if (emailPrisParAutre)
        {
            throw new InvalidOperationException("Cet email est déjà utilisé par un autre compte.");
        }

        utilisateur.Nom = dto.Nom;
        utilisateur.Prenom = dto.Prenom;
        utilisateur.Email = dto.Email;
        utilisateur.Telephone = dto.Telephone;

        await _context.SaveChangesAsync();

        return new UtilisateurResponseDto
        {
            Id = utilisateur.Id,
            Nom = utilisateur.Nom,
            Prenom = utilisateur.Prenom,
            Email = utilisateur.Email,
            Telephone = utilisateur.Telephone,
            EmailVerifie = utilisateur.EmailVerifie
        };
    }
}