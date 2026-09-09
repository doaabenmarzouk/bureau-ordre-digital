using BureauOrdreDigital.Api.Modules.Assistant.DTOs;

namespace BureauOrdreDigital.Api.Modules.Assistant.Services;

public interface IAssistantIaService
{
    Task<RedactionResponseDto?> AmeliorerRedactionAsync(
        RedactionRequestDto request);

    Task<GenerationResponseDto?> GenererAsync(
        GenerationRequestDto request,
        List<AiTypeCourrierDto> typesDisponibles);

    Task<DestinataireResponseDto?> SuggererDestinataireAsync(
        DestinataireRequestDto request,
        List<AiOrganismeDto> organismesDisponibles);
}