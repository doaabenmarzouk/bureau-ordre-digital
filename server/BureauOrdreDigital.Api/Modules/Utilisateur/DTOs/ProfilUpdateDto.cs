using System.ComponentModel.DataAnnotations;

namespace BureauOrdreDigital.Api.Modules.Utilisateur.DTOs;

public class ProfilUpdateDto
{
    [Required]
    public string Nom { get; set; } = string.Empty;

    [Required]
    public string Prenom { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string? Telephone { get; set; }
}