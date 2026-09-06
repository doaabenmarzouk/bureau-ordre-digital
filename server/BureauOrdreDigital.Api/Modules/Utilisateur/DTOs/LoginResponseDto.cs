namespace BureauOrdreDigital.Api.Modules.Utilisateur.DTOs;

public class LoginResponseDto
{
    public Guid Id { get; set; }

    public string Nom { get; set; } = string.Empty;

    public string Prenom { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Telephone { get; set; }

    // JWT définitif
    // Null tant que le OTP n'est pas vérifié
    public string? Token { get; set; }

    // Indique au frontend qu'un code OTP est nécessaire
    public bool RequiresOtp { get; set; }

    // Identifie la tentative de connexion en cours
    public string? Challenge { get; set; }
}