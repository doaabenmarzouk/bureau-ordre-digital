namespace BureauOrdreDigital.Api.Modules.Courrier.Services;

public interface IHistoriqueService
{
    Task AjouterAction(
        Guid courrierId,
        int statutId,
        string description);
}