namespace BureauOrdreDigital.Api.Modules.Assistant.DTOs
{
    public class GenerationResponseDto
    {
        public int? TypeCourrierId { get; set; }
        public string Objet { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}