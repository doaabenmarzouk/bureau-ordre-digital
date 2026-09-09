using BureauOrdreDigital.Api.Modules.Courrier.DTOs;

namespace BureauOrdreDigital.Api.Modules.Courrier.Services;

public interface ICourrierService
{
    Task<CourrierDetailDto> SoumettreCourrier(
        Guid utilisateurId,
        CourrierCreateDto dto,
        List<IFormFile>? fichiers);

    Task<List<CourrierListItemDto>> GetMesCourriers(
        Guid utilisateurId);

    Task<CourrierDetailDto> GetCourrierById(
        Guid utilisateurId,
        Guid courrierId);

    Task<List<CourrierListItemDto>> RechercherCourriers(
        Guid utilisateurId,
        string motCle);
}