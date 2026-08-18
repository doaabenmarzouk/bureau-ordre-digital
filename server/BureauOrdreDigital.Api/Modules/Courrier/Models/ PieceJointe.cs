namespace BureauOrdreDigital.Api.Modules.Courrier.Models;

public class PieceJointe
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid CourrierId { get; set; }
    public Courrier? Courrier { get; set; }

    public string NomFichier { get; set; } = string.Empty;
    public string UrlStockage { get; set; } = string.Empty;
    public long TailleOctets { get; set; }
    public string Format { get; set; } = string.Empty;

    public DateTime DateUpload { get; set; } = DateTime.UtcNow;
}