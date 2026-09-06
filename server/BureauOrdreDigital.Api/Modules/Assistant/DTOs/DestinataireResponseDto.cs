namespace BureauOrdreDigital.Api.Modules.Assistant.DTOs;

public class DestinataireResponseDto
{
    public int? ServiceDestinataireId { get; set; }

    public double Confidence { get; set; }

    public string Reason { get; set; } = string.Empty;
}