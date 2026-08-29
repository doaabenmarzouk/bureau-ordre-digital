using BureauOrdreDigital.Api.Modules.Courrier.DTOs;
using BureauOrdreDigital.Api.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace BureauOrdreDigital.Api.Modules.Courrier.Services;

public class TypeCourrierService : ITypeCourrierService
{
    private readonly AppDbContext _context;

    public TypeCourrierService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<TypeCourrierDto>> GetTousLesTypes()
    {
        return await _context.TypesCourrier
            .OrderBy(t => t.Libelle)
            .Select(t => new TypeCourrierDto
            {
                Id = t.Id,
                Libelle = t.Libelle
            })
            .ToListAsync<TypeCourrierDto>();
    }
}