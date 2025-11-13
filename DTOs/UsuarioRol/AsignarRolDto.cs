using System.ComponentModel.DataAnnotations;

namespace G2rismBeta.API.DTOs.UsuarioRol;

/// <summary>
/// DTO para asignar un solo rol a un usuario
/// </summary>
public class AsignarRolDto
{
    /// <summary>
    /// ID del rol a asignar
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "El ID del rol es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "El ID del rol debe ser un número positivo")]
    public int IdRol { get; set; }

    /// <summary>
    /// ID del usuario que está realizando la asignación (opcional, para auditoría)
    /// </summary>
    public int? AsignadoPor { get; set; }

    /// <summary>
    /// Fecha de expiración del rol (opcional)
    /// </summary>
    public DateTime? FechaExpiracion { get; set; }
}