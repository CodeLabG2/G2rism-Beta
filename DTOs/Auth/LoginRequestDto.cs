using System.ComponentModel.DataAnnotations;

namespace G2rismBeta.API.DTOs.Auth;

/// <summary>
/// DTO para solicitud de login
/// El usuario puede ingresar username o email
/// </summary>
public class LoginRequestDto
{
    /// <summary>
    /// Username o email del usuario
    /// </summary>
    /// <example>admin</example>
    [Required(ErrorMessage = "El username o email es obligatorio")]
    [StringLength(100, ErrorMessage = "El username o email no puede exceder 100 caracteres")]
    public string UsernameOrEmail { get; set; } = string.Empty;

    /// <summary>
    /// Contraseña del usuario
    /// </summary>
    /// <example>Admin123!</example>
    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [StringLength(100, ErrorMessage = "La contraseña no puede exceder 100 caracteres")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Recordar sesión (opcional, para futuras implementaciones)
    /// </summary>
    /// <example>true</example>
    public bool RememberMe { get; set; } = false;
}