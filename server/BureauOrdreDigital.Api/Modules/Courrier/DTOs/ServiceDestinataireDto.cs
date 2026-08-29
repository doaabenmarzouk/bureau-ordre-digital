namespace BureauOrdreDigital.Api.Modules.Courrier.DTOs;

public class ServiceDestinataireDto
{
    public int Id { get; set; }
    public string NomService { get; set; } = string.Empty;
    public string NomOrganisme { get; set; } = string.Empty;
    public string Ville { get; set; } = string.Empty;
    public string TypeOrganisme { get; set; } = string.Empty;
}