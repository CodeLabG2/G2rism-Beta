using System.ComponentModel.DataAnnotations;

namespace G2rismBeta.API.DTOs.Rol;

/// <summary>
/// DTO de respuesta para un rol
/// Incluye toda la información que se devuelve al cliente
/// </summary>
public class RolResponseDto
{
    /// <summary>
    /// ID del rol
    /// </summary>
    /// <example>1</example>
    public int IdRol { get; set; }

    /// <summary>
    /// Nombre del rol
    /// </summary>
    /// <example>Administrador</example>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Descripción del rol
    /// </summary>
    /// <example>Usuario con acceso total al sistema</example>
    public string? Descripcion { get; set; }

    /// <summary>
    /// Nivel de acceso
    /// </summary>
    /// <example>1</example>
    public int NivelAcceso { get; set; }

    /// <summary>
    /// Estado del rol
    /// </summary>
    /// <example>true</example>
    public bool Estado { get; set; }

    /// <summary>
    /// Fecha de creación
    /// </summary>
    /// <example>2025-10-28</example>
    [DataType(DataType.Date)]
    public DateTime FechaCreacion { get; set; }

    /// <summary>
    /// Fecha de última modificación
    /// </summary>
    /// <example>2025-10-28</example>
    [DataType(DataType.Date)]
    public DateTime? FechaModificacion { get; set; }

    /// <summary>
    /// Cantidad de permisos asignados a este rol
    /// </summary>
    /// <example>15</example>
    public int CantidadPermisos { get; set; }
}
