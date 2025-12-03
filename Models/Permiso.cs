using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace G2rismBeta.API.Models;

/// <summary>
/// Entidad que representa un Permiso en el sistema
/// Un permiso define una acción específica que se puede realizar
/// Ejemplos: "usuarios.crear", "reservas.leer", "facturas.eliminar"
/// </summary>
[Table("permisos")]
public class Permiso
{
    /// <summary>
    /// Identificador único del permiso
    /// </summary>
    [Key]
    [Column("id_permiso")]
    public int IdPermiso { get; set; }

    /// <summary>
    /// Módulo al que pertenece el permiso
    /// Ejemplos: "Usuarios", "Reservas", "Paquetes", "Facturas"
    /// </summary>
    [Required(ErrorMessage = "El módulo es obligatorio")]
    [StringLength(50, ErrorMessage = "El módulo no puede exceder 50 caracteres")]
    [Column("modulo")]
    public string Modulo { get; set; } = string.Empty;

    /// <summary>
    /// Acción específica del permiso
    /// Ejemplos: "Crear", "Leer", "Actualizar", "Eliminar"
    /// </summary>
    [Required(ErrorMessage = "La acción es obligatoria")]
    [StringLength(50, ErrorMessage = "La acción no puede exceder 50 caracteres")]
    [Column("accion")]
    public string Accion { get; set; } = string.Empty;

    /// <summary>
    /// Nombre completo del permiso (único)
    /// Formato recomendado: "modulo.accion" (ej: "usuarios.crear")
    /// </summary>
    [Required(ErrorMessage = "El nombre del permiso es obligatorio")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    [Column("nombre_permiso")]
    public string NombrePermiso { get; set; } = string.Empty;

    /// <summary>
    /// Descripción detallada de qué permite hacer este permiso
    /// </summary>
    [StringLength(200, ErrorMessage = "La descripción no puede exceder 200 caracteres")]
    [Column("descripcion")]
    public string? Descripcion { get; set; }

    /// <summary>
    /// Estado del permiso (true = activo, false = inactivo)
    /// </summary>
    [Column("estado")]
    public bool Estado { get; set; } = true;

    /// <summary>
    /// Fecha y hora de creación del permiso
    /// </summary>
    [Column("fecha_creacion")]
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    /// <summary>
    /// Fecha y hora de la última modificación del permiso
    /// </summary>
    [Column("fecha_modificacion")]
    public DateTime? FechaModificacion { get; set; }

    // ========================================
    // RELACIONES DE NAVEGACIÓN
    // ========================================

    /// <summary>
    /// Lista de roles que tienen este permiso
    /// Relación muchos a muchos a través de RolPermiso
    /// </summary>
    public virtual ICollection<RolPermiso> RolesPermisos { get; set; } = new List<RolPermiso>();
}
