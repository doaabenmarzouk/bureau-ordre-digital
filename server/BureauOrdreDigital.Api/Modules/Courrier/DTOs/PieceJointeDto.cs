namespace BureauOrdreDigital.Api.Modules.Courrier.DTOs;

public class PieceJointeDto
{
    public Guid Id { get; set; }

    public string NomFichier { get; set; } = string.Empty;

    public string UrlStockage { get; set; } = string.Empty;

    public long TailleOctets { get; set; }

    public string Format { get; set; } = string.Empty;

    public DateTime DateUpload { get; set; }
}