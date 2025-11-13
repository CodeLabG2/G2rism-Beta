using System.ComponentModel.DataAnnotations;

namespace G2rismBeta.API.DTOs.RolPermiso;

/// <summary>
/// DTO para asignar un permiso a un rol
/// </summary>
public class AsignarPermisoDto
{
    /// <summary>
    /// ID del rol
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "El ID del rol es obligatorio")]
    public int IdRol { get; set; }

    /// <summary>
    /// ID del permiso a asignar
    /// </summary>
    /// <example>5</example>
    [Required(ErrorMessage = "El ID del permiso es obligatorio")]
    public int IdPermiso { get; set; }
}
