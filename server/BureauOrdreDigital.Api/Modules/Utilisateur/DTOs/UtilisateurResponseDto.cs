namespace BureauOrdreDigital.Api.Modules.Utilisateur.DTOs;

public class UtilisateurResponseDto
{
    public Guid Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telephone { get; set; }
    public bool EmailVerifie { get; set; }
}