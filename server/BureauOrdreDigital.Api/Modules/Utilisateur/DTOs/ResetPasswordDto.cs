namespace BureauOrdreDigital.Api.Modules.Utilisateur.DTOs;

public class ResetPasswordDto
{
    public string Token { get; set; } = string.Empty;

    public string NouveauMotDePasse { get; set; } = string.Empty;

    public string ConfirmationMotDePasse { get; set; } = string.Empty;
}