using System.Net.Http.Json;
using System.Text.Json;

using BureauOrdreDigital.Api.Modules.Assistant.DTOs;

namespace BureauOrdreDigital.Api.Modules.Assistant.Services;

public class AssistantIaService : IAssistantIaService
{
    private readonly HttpClient _httpClient;

    private readonly JsonSerializerOptions _jsonOptions;

    public AssistantIaService(HttpClient httpClient)
    {
        _httpClient = httpClient;

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    // ==========================================================
    // REDACTION
    // ==========================================================

    public async Task<RedactionResponseDto?>
        AmeliorerRedactionAsync(
            RedactionRequestDto request)
    {
        var response =
            await _httpClient.PostAsJsonAsync(
                "assistant/redaction",
                request,
                _jsonOptions);

        response.EnsureSuccessStatusCode();

        return await response.Content
            .ReadFromJsonAsync<RedactionResponseDto>(
                _jsonOptions);
    }

    // ==========================================================
    // GENERATION
    // ==========================================================

    public async Task<GenerationResponseDto?>
        GenererAsync(
            GenerationRequestDto request,
            List<AiTypeCourrierDto> typesDisponibles)
    {
        var aiRequest = new AiGenerationRequestDto
        {
            TexteLibre = request.TexteLibre,
            TypesDisponibles = typesDisponibles
        };

        var response =
            await _httpClient.PostAsJsonAsync(
                "assistant/generation",
                aiRequest,
                _jsonOptions);

        response.EnsureSuccessStatusCode();

        return await response.Content
            .ReadFromJsonAsync<GenerationResponseDto>(
                _jsonOptions);
    }

    // ==========================================================
    // DESTINATAIRE
    // ==========================================================

    public async Task<DestinataireResponseDto?>
        SuggererDestinataireAsync(
            DestinataireRequestDto request,
            List<AiServiceDto> servicesDisponibles)
    {
        using var content =
            new MultipartFormDataContent();

        // ------------------------------------------------------
        // Texte citoyen
        // ------------------------------------------------------

        content.Add(
            new StringContent(
                request.Texte ?? string.Empty),
            "texte");

        // ------------------------------------------------------
        // Liste des services
        // ------------------------------------------------------

        var servicesJson =
            JsonSerializer.Serialize(
                servicesDisponibles,
                _jsonOptions);

        content.Add(
            new StringContent(servicesJson),
            "servicesDisponibles");

        // ------------------------------------------------------
        // PDF facultatif
        // ------------------------------------------------------

        if (request.Fichier is not null &&
            request.Fichier.Length > 0)
        {
            var fileStream =
                request.Fichier.OpenReadStream();

            var fileContent =
                new StreamContent(fileStream);

            if (!string.IsNullOrEmpty(
                request.Fichier.ContentType))
            {
                fileContent.Headers.ContentType =
                    new System.Net.Http.Headers
                        .MediaTypeHeaderValue(
                            request.Fichier.ContentType);
            }

            content.Add(
                fileContent,
                "fichier",
                request.Fichier.FileName);
        }

        // ------------------------------------------------------
        // Appel FastAPI
        // ------------------------------------------------------

        var response =
            await _httpClient.PostAsync(
                "assistant/destinataire",
                content);

        var responseBody =
            await response.Content
                .ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Erreur FastAPI : " +
                $"{(int)response.StatusCode} - " +
                $"{responseBody}");
        }

        return JsonSerializer.Deserialize
            <DestinataireResponseDto>(
                responseBody,
                _jsonOptions);
    }
}