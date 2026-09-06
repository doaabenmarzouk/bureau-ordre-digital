namespace BureauOrdreDigital.Api.Modules.Assistant.DTOs
{
    public class RedactionRequestDto
    {
        public string Texte { get; set; } = string.Empty;
        public string Champ { get; set; } = string.Empty; // "objet" ou "description"
    }
}