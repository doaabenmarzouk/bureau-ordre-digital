public class HistoriqueActionDto
{
    public Guid Id { get; set; }
    public string Statut { get; set; } = string.Empty;
    public DateTime DateAction { get; set; }
    public string? Description { get; set; }
   
}