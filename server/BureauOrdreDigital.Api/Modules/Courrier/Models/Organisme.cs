namespace BureauOrdreDigital.Api.Modules.Courrier.Models;

public class Organisme
{
    public int Id { get; set; }

    public string Nom { get; set; } = string.Empty;

    public string NomAbrege { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public string Pays { get; set; } = string.Empty;

    public string Ville { get; set; } = string.Empty;

    public string Scope { get; set; } = string.Empty;

    public string Mode { get; set; } = string.Empty;

    public string Marche { get; set; } = string.Empty;

    public string Partenaire { get; set; } = string.Empty;

    public string Contrat { get; set; } = string.Empty;

    public DateTime? DateAnniversaire { get; set; }

    public string VersionElise { get; set; } = string.Empty;

    public int? NombreUtilisateurs { get; set; }

    public decimal? MontantContrat { get; set; }

    public string ChefProjetCsm { get; set; } = string.Empty;

    public string CodeDivalto { get; set; } = string.Empty;

    public string NomContact { get; set; } = string.Empty;

    public string MailContact { get; set; } = string.Empty;

    public string TelContact { get; set; } = string.Empty;

    public string FonctionContact { get; set; } = string.Empty;

    public bool Actif { get; set; } = true;
}