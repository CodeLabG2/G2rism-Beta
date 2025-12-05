using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace G2rismBeta.API.Models;

/// <summary>
/// Representa un hotel registrado en el sistema
/// </summary>
[Table("hoteles")]
public class Hotel
{
    /// <summary>
    /// Identificador único del hotel
    /// </summary>
    [Key]
    [Column("id_hotel")]
    public int IdHotel { get; set; }

    /// <summary>
    /// ID del proveedor asociado (FK)
    /// </summary>
    [Required]
    [Column("id_proveedor")]
    public int IdProveedor { get; set; }

    /// <summary>
    /// Nombre del hotel
    /// </summary>
    [Required]
    [StringLength(200)]
    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Ciudad donde se ubica el hotel
    /// </summary>
    [Required]
    [StringLength(100)]
    [Column("ciudad")]
    public string Ciudad { get; set; } = string.Empty;

    /// <summary>
    /// País donde se ubica el hotel
    /// </summary>
    [StringLength(100)]
    [Column("pais")]
    public string? Pais { get; set; }

    /// <summary>
    /// Dirección física del hotel
    /// </summary>
    [Required]
    [StringLength(500)]
    [Column("direccion")]
    public string Direccion { get; set; } = string.Empty;

    /// <summary>
    /// Información de contacto (teléfono, email, etc.)
    /// </summary>
    [StringLength(200)]
    [Column("contacto")]
    public string? Contacto { get; set; }

    /// <summary>
    /// Descripción detallada del hotel
    /// </summary>
    [Column("descripcion", TypeName = "text")]
    public string? Descripcion { get; set; }

    /// <summary>
    /// Categoría del hotel (económico, estándar, premium, lujo)
    /// </summary>
    [StringLength(50)]
    [Column("categoria")]
    public string? Categoria { get; set; }

    /// <summary>
    /// Clasificación por estrellas (1-5)
    /// </summary>
    [Column("estrellas")]
    public int? Estrellas { get; set; }

    /// <summary>
    /// Precio promedio por noche
    /// </summary>
    [Required]
    [Column("precio_por_noche", TypeName = "decimal(10,2)")]
    public decimal PrecioPorNoche { get; set; }

    /// <summary>
    /// Capacidad máxima de personas por habitación
    /// </summary>
    [Column("capacidad_por_habitacion")]
    public int? CapacidadPorHabitacion { get; set; }

    /// <summary>
    /// Número total de habitaciones disponibles
    /// </summary>
    [Column("numero_habitaciones")]
    public int? NumeroHabitaciones { get; set; }

    /// <summary>
    /// Indica si el hotel tiene WiFi
    /// </summary>
    [Required]
    [Column("tiene_wifi")]
    public bool TieneWifi { get; set; } = true;

    /// <summary>
    /// Indica si el hotel tiene piscina
    /// </summary>
    [Required]
    [Column("tiene_piscina")]
    public bool TienePiscina { get; set; } = false;

    /// <summary>
    /// Indica si el hotel tiene restaurante
    /// </summary>
    [Required]
    [Column("tiene_restaurante")]
    public bool TieneRestaurante { get; set; } = false;

    /// <summary>
    /// Indica si el hotel tiene gimnasio
    /// </summary>
    [Column("tiene_gimnasio")]
    public bool TieneGimnasio { get; set; } = false;

    /// <summary>
    /// Indica si el hotel tiene parqueadero
    /// </summary>
    [Column("tiene_parqueadero")]
    public bool TieneParqueadero { get; set; } = false;

    /// <summary>
    /// Políticas de cancelación del hotel
    /// </summary>
    [Column("politicas_cancelacion", TypeName = "text")]
    public string? PoliticasCancelacion { get; set; }

    /// <summary>
    /// Hora de check-in
    /// </summary>
    [Column("check_in_hora", TypeName = "time")]
    public TimeSpan? CheckInHora { get; set; }

    /// <summary>
    /// Hora de check-out
    /// </summary>
    [Column("check_out_hora", TypeName = "time")]
    public TimeSpan? CheckOutHora { get; set; }

    /// <summary>
    /// URLs de fotos en formato JSON (array de strings)
    /// </summary>
    [Column("fotos", TypeName = "json")]
    public string? Fotos { get; set; }

    /// <summary>
    /// Servicios adicionales incluidos (JSON array)
    /// </summary>
    [Column("servicios_incluidos", TypeName = "json")]
    public string? ServiciosIncluidos { get; set; }

    /// <summary>
    /// Estado del hotel (activo/inactivo)
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
    /// Proveedor asociado al hotel
    /// </summary>
    [ForeignKey(nameof(IdProveedor))]
    public virtual Proveedor Proveedor { get; set; } = null!;

    // ============================================
    // PROPIEDADES COMPUTADAS
    // ============================================

    /// <summary>
    /// Indica si el hotel está activo
    /// </summary>
    [NotMapped]
    public bool EstaActivo => Estado;

    /// <summary>
    /// Nombre completo con ciudad
    /// </summary>
    [NotMapped]
    public string NombreCompleto => $"{Nombre} - {Ciudad}";

    /// <summary>
    /// Indica si el hotel tiene servicios premium (piscina, gimnasio, restaurante)
    /// </summary>
    [NotMapped]
    public bool TieneServiciosPremium => TienePiscina || TieneGimnasio || TieneRestaurante;

    /// <summary>
    /// Clasificación del hotel como texto
    /// </summary>
    [NotMapped]
    public string ClasificacionTexto
    {
        get
        {
            if (!Estrellas.HasValue || Estrellas.Value < 1)
                return "Sin clasificación";

            return Estrellas.Value switch
            {
                1 => "⭐ Económico",
                2 => "⭐⭐ Básico",
                3 => "⭐⭐⭐ Estándar",
                4 => "⭐⭐⭐⭐ Superior",
                5 => "⭐⭐⭐⭐⭐ Lujo",
                _ => "Sin clasificación"
            };
        }
    }
}
