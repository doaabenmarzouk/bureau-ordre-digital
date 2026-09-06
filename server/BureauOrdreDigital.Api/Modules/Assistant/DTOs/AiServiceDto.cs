namespace BureauOrdreDigital.Api.Modules.Assistant.DTOs;

public class AiServiceDto
{
    public int Id { get; set; }

    public string NomOrganisme { get; set; } = string.Empty;

    public string NomService { get; set; } = string.Empty;

    public string Ville { get; set; } = string.Empty;

    public string TypeOrganisme { get; set; } = string.Empty;
}