using System.ComponentModel.DataAnnotations;

namespace G2rismBeta.API.DTOs.Rol;

/// <summary>
/// DTO para actualizar un rol existente
/// IMPORTANTE: Todos los campos son opcionales para permitir actualizaciones parciales
/// Solo se actualizarán los campos que se envíen en el request
/// </summary>
public class RolUpdateDto
{
    /// <summary>
    /// Nuevo nombre del rol (opcional)
    /// </summary>
    /// <example>Administrador General</example>
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres")]
    public string? Nombre { get; set; }

    /// <summary>
    /// Nueva descripción del rol (opcional)
    /// </summary>
    /// <example>Usuario con acceso total al sistema y gestión de usuarios</example>
    [StringLength(200, ErrorMessage = "La descripción no puede exceder 200 caracteres")]
    public string? Descripcion { get; set; }

    /// <summary>
    /// Nuevo nivel de acceso (opcional)
    /// </summary>
    /// <example>1</example>
    [Range(1, 100, ErrorMessage = "El nivel de acceso debe estar entre 1 y 100")]
    public int? NivelAcceso { get; set; }

    /// <summary>
    /// Nuevo estado del rol (opcional)
    /// </summary>
    /// <example>true</example>
    public bool? Estado { get; set; }
}
