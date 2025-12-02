using G2rismBeta.API.DTOs.Usuario;

namespace G2rismBeta.API.DTOs.Auth;

/// <summary>
/// DTO para respuesta exitosa de login
/// </summary>
public class LoginResponseDto
{
    /// <summary>
    /// Indica si el login fue exitoso
    /// </summary>
    /// <example>true</example>
    public bool Success { get; set; }

    /// <summary>
    /// Mensaje de respuesta
    /// </summary>
    /// <example>Login exitoso</example>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Token JWT (access token)
    /// </summary>
    /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</example>
    public string? Token { get; set; }

    /// <summary>
    /// Tiempo en segundos hasta que expire el access token (estándar OAuth 2.0)
    /// </summary>
    /// <example>3600</example>
    public int ExpiresIn { get; set; }

    /// <summary>
    /// Fecha y hora de expiración del access token en UTC
    /// </summary>
    /// <example>2025-11-26T12:00:00Z</example>
    public DateTime? TokenExpiration { get; set; }

    /// <summary>
    /// Fecha y hora de expiración del access token en hora local del servidor
    /// </summary>
    /// <example>2025-11-26T07:00:00</example>
    public DateTime? TokenExpirationLocal { get; set; }

    /// <summary>
    /// Zona horaria del servidor (para referencia)
    /// </summary>
    /// <example>SA Pacific Standard Time</example>
    public string? TimeZone { get; set; }

    /// <summary>
    /// Refresh token para renovar el access token
    /// </summary>
    /// <example>abc123def456...</example>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Información del usuario autenticado
    /// </summary>
    public UsuarioLoginDto Usuario { get; set; } = new UsuarioLoginDto();

    /// <summary>
    /// Fecha y hora del login
    /// </summary>
    /// <example>2025-10-31T10:30:00</example>
    public DateTime FechaLogin { get; set; } = DateTime.Now;
}