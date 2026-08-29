using BureauOrdreDigital.Api.Modules.Courrier.DTOs;
using BureauOrdreDigital.Api.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace BureauOrdreDigital.Api.Modules.Courrier.Services;

public class ServiceDestinataireService : IServiceDestinataireService
{
    private readonly AppDbContext _context;

    public ServiceDestinataireService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ServiceDestinataireDto>> RechercherServices(
        string? ville,
        string? type,
        string? motCle)
    {
        var query = _context.ServicesDestinataires
            .Where(s => s.Actif)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(ville))
        {
            query = query.Where(s => s.Ville == ville);
        }

        if (!string.IsNullOrWhiteSpace(type))
        {
            query = query.Where(s => s.TypeOrganisme == type);
        }

        if (!string.IsNullOrWhiteSpace(motCle))
        {
            query = query.Where(s =>
                s.NomService.Contains(motCle) ||
                s.NomOrganisme.Contains(motCle));
        }

        return await query
            .OrderBy(s => s.NomOrganisme)
            .Select(s => new ServiceDestinataireDto
            {
                Id = s.Id,
                NomService = s.NomService,
                NomOrganisme = s.NomOrganisme,
                Ville = s.Ville,
                TypeOrganisme = s.TypeOrganisme
            })
            .ToListAsync();
    }
}