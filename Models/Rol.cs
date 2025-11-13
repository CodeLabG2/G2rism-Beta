using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace G2rismBeta.API.Models;

/// <summary>
/// Entidad que representa un Rol en el sistema
/// Un rol define un conjunto de permisos para los usuarios
/// Ejemplos: Administrador, Empleado, Cliente
/// </summary>
[Table("roles")]
public class Rol
{
    /// <summary>
    /// Identificador único del rol
    /// </summary>
    [Key]
    [Column("id_rol")]
    public int IdRol { get; set; }

    /// <summary>
    /// Nombre del rol (único en el sistema)
    /// Ejemplos: "Administrador", "Empleado", "Cliente"
    /// </summary>
    [Required(ErrorMessage = "El nombre del rol es obligatorio")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres")]
    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Descripción detallada del rol y sus responsabilidades
    /// </summary>
    [StringLength(200, ErrorMessage = "La descripción no puede exceder 200 caracteres")]
    [Column("descripcion")]
    public string? Descripcion { get; set; }

    /// <summary>
    /// Nivel de acceso del rol (usado para jerarquías)
    /// Valores comunes: 1 = Admin, 2 = Gerente, 3 = Empleado, 4 = Cliente
    /// </summary>
    [Range(1, 100, ErrorMessage = "El nivel de acceso debe estar entre 1 y 100")]
    [Column("nivel_acceso")]
    public int NivelAcceso { get; set; } = 10;

    /// <summary>
    /// Estado del rol (true = activo, false = inactivo)
    /// </summary>
    [Column("estado")]
    public bool Estado { get; set; } = true;

    /// <summary>
    /// Fecha y hora de creación del rol
    /// </summary>
    [Column("fecha_creacion")]
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    /// <summary>
    /// Fecha y hora de la última modificación
    /// </summary>
    [Column("fecha_modificacion")]
    public DateTime? FechaModificacion { get; set; }

    // ========================================
    // RELACIONES DE NAVEGACIÓN
    // ========================================

    /// <summary>
    /// Lista de permisos asociados a este rol
    /// Relación muchos a muchos a través de RolPermiso
    /// </summary>
    public virtual ICollection<RolPermiso> RolesPermisos { get; set; } = new List<RolPermiso>();
}
