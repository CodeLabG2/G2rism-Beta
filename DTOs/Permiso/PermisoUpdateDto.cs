using System.ComponentModel.DataAnnotations;

namespace G2rismBeta.API.DTOs.Permiso;

/// <summary>
/// DTO para actualizar un permiso existente
/// Todos los campos son opcionales excepto IdPermiso, permitiendo actualizaciones parciales
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
    /// Nuevo módulo (opcional)
    /// </summary>
    /// <example>Usuarios</example>
    [StringLength(50, ErrorMessage = "El módulo no puede exceder 50 caracteres")]
    public string? Modulo { get; set; }

    /// <summary>
    /// Nueva acción (opcional)
    /// </summary>
    /// <example>Crear</example>
    [StringLength(50, ErrorMessage = "La acción no puede exceder 50 caracteres")]
    public string? Accion { get; set; }

    /// <summary>
    /// Nueva descripción (opcional)
    /// </summary>
    /// <example>Permite crear y registrar nuevos usuarios</example>
    [StringLength(200, ErrorMessage = "La descripción no puede exceder 200 caracteres")]
    public string? Descripcion { get; set; }

    /// <summary>
    /// Nuevo estado (opcional)
    /// </summary>
    /// <example>true</example>
    public bool? Estado { get; set; }
}
