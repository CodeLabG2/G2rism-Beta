using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace G2rismBeta.API.Models;

/// <summary>
/// Representa un servicio adicional ofrecido en el sistema
/// (tours, guías, actividades, transporte interno, etc.)
/// </summary>
[Table("servicios_adicionales")]
public class ServicioAdicional
{
    /// <summary>
    /// Identificador único del servicio adicional
    /// </summary>
    [Key]
    [Column("id_servicio")]
    public int IdServicio { get; set; }

    /// <summary>
    /// ID del proveedor asociado (FK)
    /// </summary>
    [Required]
    [Column("id_proveedor")]
    public int IdProveedor { get; set; }

    /// <summary>
    /// Nombre del servicio
    /// </summary>
    [Required]
    [StringLength(200)]
    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de servicio: tour, guia, actividad, transporte_interno
    /// </summary>
    [Required]
    [StringLength(50)]
    [Column("tipo")]
    public string Tipo { get; set; } = string.Empty;

    /// <summary>
    /// Descripción detallada del servicio
    /// </summary>
    [Column("descripcion", TypeName = "text")]
    public string? Descripcion { get; set; }

    /// <summary>
    /// Precio del servicio
    /// </summary>
    [Required]
    [Column("precio", TypeName = "decimal(10,2)")]
    public decimal Precio { get; set; }

    /// <summary>
    /// Unidad de medida: persona, grupo, hora, dia
    /// </summary>
    [Required]
    [StringLength(50)]
    [Column("unidad")]
    public string Unidad { get; set; } = string.Empty;

    /// <summary>
    /// Indica si el servicio está disponible
    /// </summary>
    [Required]
    [Column("disponibilidad")]
    public bool Disponibilidad { get; set; } = true;

    /// <summary>
    /// Tiempo estimado del servicio en minutos
    /// </summary>
    [Column("tiempo_estimado")]
    public int? TiempoEstimado { get; set; }

    /// <summary>
    /// Ubicación o punto de encuentro del servicio
    /// </summary>
    [StringLength(500)]
    [Column("ubicacion")]
    public string? Ubicacion { get; set; }

    /// <summary>
    /// Requisitos o condiciones del servicio
    /// </summary>
    [Column("requisitos", TypeName = "text")]
    public string? Requisitos { get; set; }

    /// <summary>
    /// Capacidad máxima (personas, grupos, etc.)
    /// </summary>
    [Column("capacidad_maxima")]
    public int? CapacidadMaxima { get; set; }

    /// <summary>
    /// Edad mínima requerida
    /// </summary>
    [Column("edad_minima")]
    public int? EdadMinima { get; set; }

    /// <summary>
    /// Idiomas disponibles (JSON array)
    /// </summary>
    [Column("idiomas_disponibles", TypeName = "json")]
    public string? IdiomasDisponibles { get; set; }

    /// <summary>
    /// Estado del servicio (activo/inactivo)
    /// </summary>
    [Required]
    [Column("estado")]
    public bool Estado { get; set; } = true;

    /// <summary>
    /// Fecha de creación del registro
    /// </summary>
    [Column("fecha_creacion")]
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    /// <summary>
    /// Fecha de última modificación del registro
    /// </summary>
    [Column("fecha_modificacion")]
    public DateTime? FechaModificacion { get; set; }

    // ============================================
    // NAVEGACIÓN
    // ============================================

    /// <summary>
    /// Proveedor asociado al servicio
    /// </summary>
    [ForeignKey(nameof(IdProveedor))]
    public virtual Proveedor Proveedor { get; set; } = null!;

    // ============================================
    // PROPIEDADES COMPUTADAS
    // ============================================

    /// <summary>
    /// Indica si el servicio está activo
    /// </summary>
    [NotMapped]
    public bool EstaActivo => Estado;

    /// <summary>
    /// Indica si el servicio está disponible
    /// </summary>
    [NotMapped]
    public bool EstaDisponible => Estado && Disponibilidad;

    /// <summary>
    /// Nombre completo con tipo
    /// </summary>
    [NotMapped]
    public string NombreCompleto => $"{Nombre} ({Tipo})";

    /// <summary>
    /// Precio formateado con unidad
    /// </summary>
    [NotMapped]
    public string PrecioFormateado => $"${Precio:N0} / {Unidad}";

    /// <summary>
    /// Duración estimada formateada
    /// </summary>
    [NotMapped]
    public string? DuracionFormateada
    {
        get
        {
            if (!TiempoEstimado.HasValue)
                return null;

            if (TiempoEstimado.Value < 60)
                return $"{TiempoEstimado.Value} minutos";

            int horas = TiempoEstimado.Value / 60;
            int minutos = TiempoEstimado.Value % 60;

            if (minutos == 0)
                return $"{horas} hora{(horas > 1 ? "s" : "")}";

            return $"{horas}h {minutos}min";
        }
    }
}
