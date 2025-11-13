using G2rismBeta.API.Models;
using System.Net;
using System.Text.Json;

namespace G2rismBeta.API.Middleware;

/// <summary>
/// Middleware global para capturar y manejar todas las excepciones
/// </summary>
public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    public GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleware> logger,
        IWebHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    /// <summary>
    /// Método que se ejecuta en cada request
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Continuar con el siguiente middleware
            await _next(context);
        }
        catch (Exception ex)
        {
            // Si hay un error, capturarlo aquí
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Maneja la excepción y devuelve una respuesta formateada
    /// </summary>
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Log del error
        _logger.LogError(exception, "Ocurrió un error no controlado: {Message}", exception.Message);

        // Configurar respuesta HTTP
        context.Response.ContentType = "application/json";

        // Determinar el código de estado según el tipo de excepción
        var statusCode = exception switch
        {
            ArgumentException => HttpStatusCode.BadRequest,        // 400
            KeyNotFoundException => HttpStatusCode.NotFound,       // 404
            InvalidOperationException => HttpStatusCode.Conflict,  // 409
            UnauthorizedAccessException => HttpStatusCode.Unauthorized, // 401
            _ => HttpStatusCode.InternalServerError                // 500
        };

        context.Response.StatusCode = (int)statusCode;

        // Crear respuesta de error
        var errorResponse = new ApiErrorResponse
        {
            Success = false,
            Message = GetUserFriendlyMessage(exception),
            ErrorCode = exception.GetType().Name,
            Timestamp = DateTime.Now
        };

        // En desarrollo, incluir el StackTrace para debugging
        if (_environment.IsDevelopment())
        {
            errorResponse.StackTrace = exception.StackTrace;
        }

        // Serializar y enviar respuesta
        var json = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });

        await context.Response.WriteAsync(json);
    }

    /// <summary>
    /// Obtiene un mensaje amigable según el tipo de excepción
    /// </summary>
    private string GetUserFriendlyMessage(Exception exception)
    {
        return exception switch
        {
            ArgumentException => exception.Message,
            KeyNotFoundException => exception.Message,
            InvalidOperationException => exception.Message,
            UnauthorizedAccessException => "No tiene permisos para realizar esta acción",
            _ => "Ocurrió un error inesperado. Por favor, intente nuevamente más tarde."
        };
    }
}
