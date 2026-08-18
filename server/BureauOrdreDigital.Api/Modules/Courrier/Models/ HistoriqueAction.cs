namespace BureauOrdreDigital.Api.Modules.Courrier.Models;

public class HistoriqueAction
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid CourrierId { get; set; }
    public Courrier? Courrier { get; set; }

    public int StatutId { get; set; }
    public StatutCourrier? StatutCourrier { get; set; }

    public DateTime DateAction { get; set; } = DateTime.UtcNow;
    public string? Description { get; set; }
}