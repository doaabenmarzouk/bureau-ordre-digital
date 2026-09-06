using System.Net;
using System.Text.Json;

namespace BureauOrdreDigital.Api.Shared.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, publicMessage) = MapException(exception);

        // Log complet côté serveur, avec le niveau de gravité adapté
        if (statusCode == HttpStatusCode.InternalServerError)
        {
            _logger.LogError(
                exception,
                "Erreur non gérée sur {Method} {Path} — {Message}",
                context.Request.Method,
                context.Request.Path,
                exception.Message
            );
        }
        else
        {
            _logger.LogWarning(
                "Erreur applicative sur {Method} {Path} — {StatusCode} — {Message}",
                context.Request.Method,
                context.Request.Path,
                (int)statusCode,
                exception.Message
            );
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new ErrorResponse
        {
            Message = publicMessage,
            StatusCode = (int)statusCode,
            TraceId = context.TraceIdentifier,
            // En développement uniquement : détail technique pour faciliter le debug
            Detail = _environment.IsDevelopment() ? exception.ToString() : null
        };

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = JsonSerializer.Serialize(response, options);

        await context.Response.WriteAsync(json);
    }

    private static (HttpStatusCode statusCode, string message) MapException(Exception exception)
    {
        return exception switch
        {
            UnauthorizedAccessException =>
                (HttpStatusCode.Unauthorized, "Vous n'êtes pas autorisé à effectuer cette action."),

            ArgumentException or ArgumentNullException =>
                (HttpStatusCode.BadRequest, exception.Message),

            KeyNotFoundException =>
                (HttpStatusCode.NotFound, "La ressource demandée est introuvable."),

            InvalidOperationException =>
                (HttpStatusCode.BadRequest, exception.Message),

            TimeoutException =>
                (HttpStatusCode.GatewayTimeout, "Le service a mis trop de temps à répondre. Veuillez réessayer."),

            HttpRequestException =>
                (HttpStatusCode.BadGateway, "Un service externe n'a pas répondu correctement. Veuillez réessayer plus tard."),

            _ =>
                (HttpStatusCode.InternalServerError, "Une erreur interne est survenue. Veuillez réessayer plus tard.")
        };
    }
}

public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public string TraceId { get; set; } = string.Empty;
    public string? Detail { get; set; }
}