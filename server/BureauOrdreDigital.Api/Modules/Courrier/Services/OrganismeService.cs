using BureauOrdreDigital.Api.Modules.Courrier.DTOs;
using BureauOrdreDigital.Api.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace BureauOrdreDigital.Api.Modules.Courrier.Services;

public class OrganismeService : IOrganismeService
{
    private readonly AppDbContext _context;

    public OrganismeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<OrganismeDto>> RechercherOrganismes(
        string? ville,
        string? type,
        string? motCle)
    {
        var query = _context.Organismes
            .Where(o => o.Actif)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(ville))
        {
            query = query.Where(o => o.Ville == ville);
        }

        if (!string.IsNullOrWhiteSpace(type))
        {
            query = query.Where(o => o.Type == type);
        }

        if (!string.IsNullOrWhiteSpace(motCle))
        {
            query = query.Where(o =>
                o.Nom.Contains(motCle) ||
                o.NomAbrege.Contains(motCle));
        }

        return await query
            .OrderBy(o => o.Nom)
            .Select(o => new OrganismeDto
            {
                Id = o.Id,
                Nom = o.Nom,
                NomAbrege = o.NomAbrege,
                Type = o.Type,
                Pays = o.Pays,
                Ville = o.Ville,
                Scope = o.Scope,
                Mode = o.Mode,
                Marche = o.Marche,
                Partenaire = o.Partenaire,
                Contrat = o.Contrat,
                DateAnniversaire = o.DateAnniversaire
            })
            .ToListAsync();
    }
}