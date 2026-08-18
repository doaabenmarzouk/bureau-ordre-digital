namespace BureauOrdreDigital.Api.Modules.Utilisateur.DTOs;

public class LoginResponseDto
{
    public Guid Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}