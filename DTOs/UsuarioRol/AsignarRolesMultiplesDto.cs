using System.ComponentModel.DataAnnotations;

namespace G2rismBeta.API.DTOs.UsuarioRol;

/// <summary>
/// DTO para asignar múltiples roles a un usuario
/// </summary>
public class AsignarRolesMultiplesDto
{
    /// <summary>
    /// Lista de IDs de roles a asignar al usuario
    /// </summary>
    /// <example>[1, 2, 3]</example>
    [Required(ErrorMessage = "Debe proporcionar al menos un rol")]
    [MinLength(1, ErrorMessage = "Debe asignar al menos un rol")]
    public List<int> RolesIds { get; set; } = new();

    /// <summary>
    /// ID del usuario que está realizando la asignación (opcional, para auditoría)
    /// </summary>
    public int? AsignadoPor { get; set; }
}