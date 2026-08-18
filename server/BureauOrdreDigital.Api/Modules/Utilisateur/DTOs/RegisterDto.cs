using System.ComponentModel.DataAnnotations;

namespace BureauOrdreDigital.Api.Modules.Utilisateur.DTOs;

public class RegisterDto
{
    [Required]
    public string Nom { get; set; } = string.Empty;

    [Required]
    public string Prenom { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string? Telephone { get; set; }

    [Required]
    [MinLength(8, ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères.")]
    public string MotDePasse { get; set; } = string.Empty;
}