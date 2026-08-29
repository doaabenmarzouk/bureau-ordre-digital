using BureauOrdreDigital.Api.Modules.Courrier.DTOs;

namespace BureauOrdreDigital.Api.Modules.Courrier.Services;

public interface IServiceDestinataireService
{
    Task<List<ServiceDestinataireDto>> RechercherServices(
        string? ville,
        string? type,
        string? motCle);
}