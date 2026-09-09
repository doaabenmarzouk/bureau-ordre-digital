
namespace BureauOrdreDigital.Api.Modules.Courrier.DTOs;

public class CourrierListItemDto
{
    public Guid Id { get; set; }

    public string NumReference { get; set; } = string.Empty;

    public string Objet { get; set; } = string.Empty;

    public string Statut { get; set; } = string.Empty;

    public DateTime DateSoumission { get; set; }

    // Organisme destinataire
    public string OrganismeName { get; set; } = string.Empty;
}

