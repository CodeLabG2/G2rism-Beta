using System.ComponentModel.DataAnnotations.Schema;

namespace G2rismBeta.API.Models;

/// <summary>
/// Tabla intermedia para la relación muchos a muchos entre Roles y Permisos
/// Un rol puede tener muchos permisos y un permiso puede estar en muchos roles
/// </summary>
[Table("roles_permisos")]
public class RolPermiso
{
    /// <summary>
    /// ID del rol (parte de la clave primaria compuesta)
    /// </summary>
    [Column("id_rol")]
    public int IdRol { get; set; }

    /// <summary>
    /// ID del permiso (parte de la clave primaria compuesta)
    /// </summary>
    [Column("id_permiso")]
    public int IdPermiso { get; set; }

    /// <summary>
    /// Fecha en que se asignó este permiso al rol
    /// </summary>
    [Column("fecha_asignacion")]
    public DateTime FechaAsignacion { get; set; } = DateTime.Now;

    // ========================================
    // RELACIONES DE NAVEGACIÓN
    // ========================================

    /// <summary>
    /// Referencia al rol
    /// </summary>
    [ForeignKey("IdRol")]
    public virtual Rol Rol { get; set; } = null!;

    /// <summary>
    /// Referencia al permiso
    /// </summary>
    [ForeignKey("IdPermiso")]
    public virtual Permiso Permiso { get; set; } = null!;
}
