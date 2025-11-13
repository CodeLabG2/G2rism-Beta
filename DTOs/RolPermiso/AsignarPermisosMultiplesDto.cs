using System.ComponentModel.DataAnnotations;

namespace G2rismBeta.API.DTOs.RolPermiso;

/// <summary>
/// DTO para asignar múltiples permisos a un rol de una sola vez
/// </summary>
public class AsignarPermisosMultiplesDto
{
    /// <summary>
    /// ID del rol
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "El ID del rol es obligatorio")]
    public int IdRol { get; set; }

    /// <summary>
    /// Lista de IDs de permisos a asignar
    /// </summary>
    /// <example>[1, 2, 3, 5, 8]</example>
    [Required(ErrorMessage = "Debe proporcionar al menos un permiso")]
    [MinLength(1, ErrorMessage = "Debe asignar al menos un permiso")]
    public List<int> IdsPermisos { get; set; } = new List<int>();
}
