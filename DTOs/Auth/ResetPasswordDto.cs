using System.ComponentModel.DataAnnotations;

namespace G2rismBeta.API.DTOs.Auth;

/// <summary>
/// DTO para resetear la contraseña usando el token de recuperación
/// </summary>
public class ResetPasswordDto
{
    /// <summary>
    /// Token de recuperación recibido por email
    /// </summary>
    /// <example>a1b2c3d4e5f6g7h8i9j0</example>
    [Required(ErrorMessage = "El token es obligatorio")]
    [StringLength(255, ErrorMessage = "El token no puede exceder 255 caracteres")]
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Nueva contraseña
    /// Debe cumplir con los requisitos de seguridad
    /// </summary>
    /// <example>NewPassword123!</example>
    [Required(ErrorMessage = "La nueva contraseña es obligatoria")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre 8 y 100 caracteres")]
    public string NewPassword { get; set; } = string.Empty;

    /// <summary>
    /// Confirmación de la nueva contraseña
    /// </summary>
    /// <example>NewPassword123!</example>
    [Required(ErrorMessage = "La confirmación de contraseña es obligatoria")]
    [Compare(nameof(NewPassword), ErrorMessage = "Las contraseñas no coinciden")]
    public string ConfirmPassword { get; set; } = string.Empty;
}