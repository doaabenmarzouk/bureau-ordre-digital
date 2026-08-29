namespace BureauOrdreDigital.Api.Modules.Courrier.DTOs;

public class CourrierDetailDto
{
    public Guid Id { get; set; }
    public string NumReference { get; set; } = string.Empty;
    public string Objet { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string TypeCourrier { get; set; } = string.Empty;
    public string ServiceDestinataire { get; set; } = string.Empty;
    public string Statut { get; set; } = string.Empty;
    public DateTime DateSoumission { get; set; }

    public List<HistoriqueActionDto> Historique { get; set; } = new();
    public List<PieceJointeDto> PiecesJointes { get; set; } = new();
}