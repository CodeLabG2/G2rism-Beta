using System.ComponentModel.DataAnnotations;

namespace G2rismBeta.API.DTOs.Permiso;

/// <summary>
/// DTO para actualizar un permiso existente
/// </summary>
public class PermisoUpdateDto
{
    /// <summary>
    /// ID del permiso a actualizar
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "El ID del permiso es obligatorio")]
    public int IdPermiso { get; set; }

    /// <summary>
    /// Nuevo módulo
    /// </summary>
    /// <example>Usuarios</example>
    [Required(ErrorMessage = "El módulo es obligatorio")]
    [StringLength(50, ErrorMessage = "El módulo no puede exceder 50 caracteres")]
    public string Modulo { get; set; } = string.Empty;

    /// <summary>
    /// Nueva acción
    /// </summary>
    /// <example>Crear</example>
    [Required(ErrorMessage = "La acción es obligatoria")]
    [StringLength(50, ErrorMessage = "La acción no puede exceder 50 caracteres")]
    public string Accion { get; set; } = string.Empty;

    /// <summary>
    /// Nuevo nombre del permiso
    /// </summary>
    /// <example>usuarios.crear</example>
    [Required(ErrorMessage = "El nombre del permiso es obligatorio")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string NombrePermiso { get; set; } = string.Empty;

    /// <summary>
    /// Nueva descripción
    /// </summary>
    /// <example>Permite crear y registrar nuevos usuarios</example>
    [StringLength(200, ErrorMessage = "La descripción no puede exceder 200 caracteres")]
    public string? Descripcion { get; set; }

    /// <summary>
    /// Nuevo estado
    /// </summary>
    /// <example>true</example>
    public bool Estado { get; set; }
}
