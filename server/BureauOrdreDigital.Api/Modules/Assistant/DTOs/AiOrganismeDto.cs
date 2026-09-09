namespace BureauOrdreDigital.Api.Modules.Assistant.DTOs;

public class AiOrganismeDto
{
    public int Id { get; set; }

    public string Nom { get; set; } = string.Empty;

    public string NomAbrege { get; set; } = string.Empty;

    public string Ville { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;
}