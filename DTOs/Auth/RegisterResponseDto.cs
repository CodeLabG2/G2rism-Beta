namespace G2rismBeta.API.DTOs.Auth;

/// <summary>
/// Respuesta después de un registro exitoso
/// </summary>
public class RegisterResponseDto
{
    /// <summary>
    /// ID del usuario creado
    /// </summary>
    public int IdUsuario { get; set; }

    /// <summary>
    /// Username del usuario creado
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Email del usuario creado
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de usuario creado
    /// </summary>
    public string TipoUsuario { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de registro
    /// </summary>
    public DateTime FechaRegistro { get; set; }

    /// <summary>
    /// Roles asignados al usuario (por defecto: Cliente)
    /// </summary>
    public List<string> Roles { get; set; } = new();

    /// <summary>
    /// Mensaje informativo
    /// </summary>
    public string Mensaje { get; set; } = string.Empty;
}