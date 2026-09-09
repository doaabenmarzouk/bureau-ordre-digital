using BureauOrdreDigital.Api.Modules.Courrier.DTOs;

public interface IOrganismeService
{
    Task<List<OrganismeDto>> RechercherOrganismes(
        string? ville,
        string? type,
        string? motCle);
}