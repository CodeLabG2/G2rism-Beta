using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace G2rismBeta.API.Models;

/// <summary>
/// Tabla intermedia para la relación muchos a muchos entre Usuarios y Roles
/// Permite asignar múltiples roles a un usuario
/// </summary>
[Table("usuarios_roles")]
public class UsuarioRol
{
    /// <summary>
    /// ID del usuario
    /// Parte de la clave primaria compuesta
    /// </summary>
    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    /// <summary>
    /// ID del rol
    /// Parte de la clave primaria compuesta
    /// </summary>
    [Column("id_rol")]
    public int IdRol { get; set; }

    /// <summary>
    /// Fecha y hora en que se asignó el rol
    /// </summary>
    [Column("fecha_asignacion")]
    public DateTime FechaAsignacion { get; set; } = DateTime.Now;

    /// <summary>
    /// Fecha de expiración del rol
    /// NULL = el rol es permanente
    /// </summary>
    [Column("fecha_expiracion")]
    public DateTime? FechaExpiracion { get; set; }

    /// <summary>
    /// ID del usuario que asignó este rol
    /// Para auditoría y trazabilidad
    /// </summary>
    [Column("asignado_por")]
    public int? AsignadoPor { get; set; }

    // ========================================
    // RELACIONES DE NAVEGACIÓN
    // ========================================

    /// <summary>
    /// Usuario al que se le asignó el rol
    /// </summary>
    [ForeignKey("IdUsuario")]
    public virtual Usuario Usuario { get; set; } = null!;

    /// <summary>
    /// Rol asignado
    /// </summary>
    [ForeignKey("IdRol")]
    public virtual Rol Rol { get; set; } = null!;
}