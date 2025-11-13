using System.ComponentModel.DataAnnotations;

namespace G2rismBeta.API.DTOs.Auth;

/// <summary>
/// DTO para solicitar recuperación de contraseña
/// </summary>
public class RecuperarPasswordRequestDto
{
    /// <summary>
    /// Email del usuario que solicita recuperar su contraseña
    /// </summary>
    /// <example>usuario@example.com</example>
    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    [StringLength(100, ErrorMessage = "El email no puede exceder 100 caracteres")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// URL base del frontend para construir el link de recuperación
    /// Ejemplo: https://tu-frontend.com
    /// El sistema agregará automáticamente: /reset-password?token=xxx
    /// </summary>
    /// <example>https://g2rism-frontend.com</example>
    [Required(ErrorMessage = "La URL del frontend es obligatoria")]
    [Url(ErrorMessage = "La URL del frontend no es válida")]
    public string FrontendUrl { get; set; } = string.Empty;
}