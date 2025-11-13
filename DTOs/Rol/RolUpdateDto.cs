using System.ComponentModel.DataAnnotations;

namespace G2rismBeta.API.DTOs.Rol;

/// <summary>
/// DTO para actualizar un rol existente
/// Incluye el ID y los campos modificables
/// </summary>
public class RolUpdateDto
{
    /// <summary>
    /// ID del rol a actualizar
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "El ID del rol es obligatorio")]
    public int IdRol { get; set; }

    /// <summary>
    /// Nuevo nombre del rol
    /// </summary>
    /// <example>Administrador General</example>
    [Required(ErrorMessage = "El nombre del rol es obligatorio")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Nueva descripción del rol
    /// </summary>
    /// <example>Usuario con acceso total al sistema y gestión de usuarios</example>
    [StringLength(200, ErrorMessage = "La descripción no puede exceder 200 caracteres")]
    public string? Descripcion { get; set; }

    /// <summary>
    /// Nuevo nivel de acceso
    /// </summary>
    /// <example>1</example>
    [Range(1, 100, ErrorMessage = "El nivel de acceso debe estar entre 1 y 100")]
    public int NivelAcceso { get; set; }

    /// <summary>
    /// Nuevo estado del rol
    /// </summary>
    /// <example>true</example>
    public bool Estado { get; set; }
}
