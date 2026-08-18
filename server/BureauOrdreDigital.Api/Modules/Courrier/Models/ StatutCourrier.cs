namespace BureauOrdreDigital.Api.Modules.Courrier.Models;

public class StatutCourrier
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Libelle { get; set; } = string.Empty;
    public int Ordre { get; set; }
}