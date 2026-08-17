namespace BureauOrdreDigital.Api.Modules.Utilisateur.Models;

public class Utilisateur
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telephone { get; set; }
    public string MotDePasseHash { get; set; } = string.Empty;
    public bool EmailVerifie { get; set; } = false;
    public string? TokenVerification { get; set; }
    public string? TokenResetPassword { get; set; }
    public DateTime? TokenResetExpiration { get; set; }
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;
}