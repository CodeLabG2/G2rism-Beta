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
    /// Token JWT (para futuras implementaciones)
    /// Por ahora será null hasta implementar JWT
    /// </summary>
    /// <example>null</example>
    public string? Token { get; set; }

    /// <summary>
    /// Fecha de expiración del token (para futuras implementaciones)
    /// </summary>
    /// <example>null</example>
    public DateTime? TokenExpiration { get; set; }

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