namespace BureauOrdreDigital.Api.Modules.Courrier.Models;

public class ServiceDestinataire
{
    public int Id { get; set; }
    public string NomService { get; set; } = string.Empty;
    public string NomOrganisme { get; set; } = string.Empty;
    public bool Actif { get; set; } = true;
}