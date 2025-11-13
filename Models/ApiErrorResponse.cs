namespace G2rismBeta.API.Models;

/// <summary>
/// Modelo estándar para respuestas de error de la API
/// </summary>
public class ApiErrorResponse
{
    /// <summary>
    /// Siempre false en errores
    /// </summary>
    public bool Success { get; set; } = false;

    /// <summary>
    /// Mensaje principal del error
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Código de estado HTTP (404, 400, 500, etc.)
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Lista de errores detallados (validaciones)
    /// </summary>
    public List<string>? Errors { get; set; }

    /// <summary>
    /// Código de error (opcional)
    /// </summary>
    public string? ErrorCode { get; set; }

    /// <summary>
    /// Timestamp del error
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.Now;

    /// <summary>
    /// Información de debugging (solo en desarrollo)
    /// </summary>
    public string? StackTrace { get; set; }

    /// <summary>
    /// Constructor para error simple con código de estado
    /// </summary>
    public static ApiErrorResponse ErrorResponse(string message, int statusCode, string? errorCode = null)
    {
        return new ApiErrorResponse
        {
            Success = false,
            Message = message,
            StatusCode = statusCode,
            ErrorCode = errorCode,
            Timestamp = DateTime.Now
        };
    }

    /// <summary>
    /// Constructor para múltiples errores (validaciones)
    /// </summary>
    public static ApiErrorResponse ValidationErrorResponse(List<string> errors, string message = "Errores de validación")
    {
        return new ApiErrorResponse
        {
            Success = false,
            Message = message,
            StatusCode = 400,
            Errors = errors,
            ErrorCode = "VALIDATION_ERROR",
            Timestamp = DateTime.Now
        };
    }
}