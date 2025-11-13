using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace G2rismBeta.API.Models;

/// <summary>
/// Entidad que representa las preferencias de viaje de un Cliente (CRM - Seguimiento)
/// Relación 1:1 con Cliente - permite personalizar ofertas y servicios
/// </summary>
[Table("preferencias_cliente")]
public class PreferenciaCliente
{
    /// <summary>
    /// Identificador único de la preferencia
    /// </summary>
    [Key]
    [Column("id_preferencia")]
    public int IdPreferencia { get; set; }

    /// <summary>
    /// ID del cliente al que pertenecen estas preferencias (relación 1:1)
    /// </summary>
    [Required(ErrorMessage = "El ID del cliente es obligatorio")]
    [Column("id_cliente")]
    public int IdCliente { get; set; }

    /// <summary>
    /// Tipo de destino preferido
    /// Ejemplos: Playa, Montaña, Ciudad, Rural, Aventura, Cultural
    /// </summary>
    [StringLength(50, ErrorMessage = "El tipo de destino no puede exceder 50 caracteres")]
    [Column("tipo_destino")]
    public string? TipoDestino { get; set; }

    /// <summary>
    /// Tipo de alojamiento preferido
    /// Ejemplos: Hotel, Hostal, Apartamento, Resort, Cabaña
    /// </summary>
    [StringLength(50, ErrorMessage = "El tipo de alojamiento no puede exceder 50 caracteres")]
    [Column("tipo_alojamiento")]
    public string? TipoAlojamiento { get; set; }

    /// <summary>
    /// Presupuesto promedio por viaje
    /// </summary>
    [Column("presupuesto_promedio", TypeName = "decimal(10,2)")]
    public decimal? PresupuestoPromedio { get; set; }

    /// <summary>
    /// Preferencias de alimentación
    /// Ejemplos: Vegetariano, Vegano, Sin Gluten, Todo, Kosher, Halal
    /// </summary>
    [StringLength(100, ErrorMessage = "Las preferencias de alimentación no pueden exceder 100 caracteres")]
    [Column("preferencias_alimentacion")]
    public string? PreferenciasAlimentacion { get; set; }

    /// <summary>
    /// Intereses del cliente en formato JSON
    /// Ejemplos: ["Historia", "Gastronomía", "Deportes", "Naturaleza"]
    /// </summary>
    [Column("intereses", TypeName = "json")]
    public string? Intereses { get; set; }

    /// <summary>
    /// Fecha de última actualización de las preferencias
    /// </summary>
    [Column("fecha_actualizacion")]
    public DateTime FechaActualizacion { get; set; } = DateTime.Now;

    // ========================================
    // RELACIONES DE NAVEGACIÓN
    // ========================================

    /// <summary>
    /// Cliente al que pertenecen estas preferencias (relación 1:1)
    /// </summary>
    public virtual Cliente? Cliente { get; set; }
}