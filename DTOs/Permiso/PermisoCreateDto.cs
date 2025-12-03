using System.ComponentModel.DataAnnotations;

namespace G2rismBeta.API.DTOs.Permiso;

/// <summary>
/// DTO para crear un nuevo permiso
/// </summary>
public class PermisoCreateDto
{
    /// <summary>
    /// Módulo al que pertenece el permiso
    /// </summary>
    /// <example>Usuarios</example>
    [Required(ErrorMessage = "El módulo es obligatorio")]
    [StringLength(50, ErrorMessage = "El módulo no puede exceder 50 caracteres")]
    public string Modulo { get; set; } = string.Empty;

    /// <summary>
    /// Acción del permiso
    /// </summary>
    /// <example>Crear</example>
    [Required(ErrorMessage = "La acción es obligatoria")]
    [StringLength(50, ErrorMessage = "La acción no puede exceder 50 caracteres")]
    public string Accion { get; set; } = string.Empty;

    /// <summary>
    /// Descripción del permiso
    /// </summary>
    /// <example>Permite crear nuevos usuarios en el sistema</example>
    [StringLength(200, ErrorMessage = "La descripción no puede exceder 200 caracteres")]
    public string? Descripcion { get; set; }

    /// <summary>
    /// Estado del permiso
    /// </summary>
    /// <example>true</example>
    public bool Estado { get; set; } = true;
}
