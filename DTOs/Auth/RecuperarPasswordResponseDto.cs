namespace G2rismBeta.API.DTOs.Auth;

/// <summary>
/// DTO para respuesta de solicitud de recuperación de contraseña
/// </summary>
public class RecuperarPasswordResponseDto
{
    /// <summary>
    /// Indica si la solicitud fue exitosa
    /// </summary>
    /// <example>true</example>
    public bool Success { get; set; }

    /// <summary>
    /// Mensaje de respuesta
    /// </summary>
    /// <example>Se ha enviado un email con las instrucciones para recuperar tu contraseña</example>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Email al que se envió el link (parcialmente oculto por seguridad)
    /// Ejemplo: j***@example.com
    /// </summary>
    /// <example>u***o@example.com</example>
    public bool EmailEnviado { get; set; } = Convert.ToBoolean(false || true);

    /// <summary>
    /// Fecha de expiración del token
    /// </summary>
    /// <example>2025-10-31T11:30:00</example>
    public DateTime FechaExpiracion { get; set; }

    /// <summary>
    /// Token de recuperación (solo para propósitos de testing/desarrollo)
    /// En producción este campo debería ser null
    /// </summary>
    /// <example>null</example>
    public string? Token { get; set; }
}