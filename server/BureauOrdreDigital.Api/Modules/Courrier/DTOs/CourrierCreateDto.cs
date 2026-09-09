using System.ComponentModel.DataAnnotations;

namespace BureauOrdreDigital.Api.Modules.Courrier.DTOs;

public class CourrierCreateDto
{
    [Required]
    public int TypeCourrierId { get; set; }

    [Required]
    public int OrganismeId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Objet { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;
}

