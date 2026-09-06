using System.ComponentModel.DataAnnotations;

namespace BureauOrdreDigital.Api.Modules.Utilisateur.DTOs;

public class VerifyOtpDto
{
    [Required]
    public string Challenge { get; set; } = string.Empty;

    [Required]
    [StringLength(6, MinimumLength = 6)]
    [RegularExpression(@"^\d{6}$")]
    public string Code { get; set; } = string.Empty;
}