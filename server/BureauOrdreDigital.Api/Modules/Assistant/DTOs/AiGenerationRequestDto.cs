using System.Collections.Generic;

namespace BureauOrdreDigital.Api.Modules.Assistant.DTOs
{
    public class AiGenerationRequestDto
    {
        public string TexteLibre { get; set; } = string.Empty;
        public List<AiTypeCourrierDto> TypesDisponibles { get; set; } = new();
    }
}