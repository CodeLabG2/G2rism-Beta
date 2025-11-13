using System.ComponentModel.DataAnnotations;

namespace G2rismBeta.API.DTOs.Auth;

/// <summary>
/// DTO para cambiar la contraseña de un usuario autenticado
/// Requiere la contraseña actual para validar
/// </summary>
public class CambiarPasswordDto
{
    /// <summary>
    /// ID del usuario que cambia su contraseña
    /// </summary>
    [Required(ErrorMessage = "El ID de usuario es obligatorio")]
    public int IdUsuario { get; set; }

    /// <summary>
    /// Contraseña actual del usuario
    /// </summary>
    /// <example>OldPassword123!</example>
    [Required(ErrorMessage = "La contraseña actual es obligatoria")]
    [StringLength(100, ErrorMessage = "La contraseña actual no puede exceder 100 caracteres")]
    public string CurrentPassword { get; set; } = string.Empty;

    /// <summary>
    /// Nueva contraseña
    /// Debe cumplir con los requisitos de seguridad
    /// </summary>
    /// <example>NewPassword123!</example>
    [Required(ErrorMessage = "La nueva contraseña es obligatoria")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "La nueva contraseña debe tener entre 8 y 100 caracteres")]
    public string NewPassword { get; set; } = string.Empty;

    /// <summary>
    /// Confirmación de la nueva contraseña
    /// </summary>
    /// <example>NewPassword123!</example>
    [Required(ErrorMessage = "La confirmación de contraseña es obligatoria")]
    [Compare(nameof(NewPassword), ErrorMessage = "Las contraseñas no coinciden")]
    public string ConfirmPassword { get; set; } = string.Empty;
}