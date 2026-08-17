namespace BureauOrdreDigital.Api.Modules.Utilisateur.DTOs;

public class RegisterDto
{
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telephone { get; set; }
    public string MotDePasse { get; set; } = string.Empty;
}