namespace BureauOrdreDigital.Api.Modules.Courrier.Models;

public class Courrier
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string NumReference { get; set; } = string.Empty;

    // Citoyen / utilisateur qui crée la demande
    public Guid UtilisateurId { get; set; }
    public Utilisateur.Models.Utilisateur? Utilisateur { get; set; }

    // Type de courrier
    public int TypeCourrierId { get; set; }
    public TypeCourrier? TypeCourrier { get; set; }

    // Organisme destinataire
    public int OrganismeId { get; set; }
    public Organisme? Organisme { get; set; }

    // Statut
    public int StatutId { get; set; }
    public StatutCourrier? StatutCourrier { get; set; }

    public string Objet { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime DateSoumission { get; set; } = DateTime.UtcNow;

    public DateTime DateDerniereMaj { get; set; } = DateTime.UtcNow;
}