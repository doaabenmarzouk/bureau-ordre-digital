using BureauOrdreDigital.Api.Shared.Data;
using BureauOrdreDigital.Api.Modules.Courrier.Models;

namespace BureauOrdreDigital.Api.Modules.Courrier.Services;

public class HistoriqueService : IHistoriqueService
{
    private readonly AppDbContext _context;

    public HistoriqueService(AppDbContext context)
    {
        _context = context;
    }

    public async Task AjouterAction(
        Guid courrierId,
        int statutId,
        string description)
    {
        var historique = new HistoriqueAction
        {
            CourrierId = courrierId,
            StatutId = statutId,
            Description = description,
            DateAction = DateTime.UtcNow
        };

        await _context. HistoriqueActions.AddAsync(historique);

        await _context.SaveChangesAsync();
    }
}