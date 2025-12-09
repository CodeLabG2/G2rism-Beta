using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace G2rismBeta.API.Models;

/// <summary>
/// Entidad que representa la relación entre Reservas y Vuelos.
/// Permite asociar múltiples vuelos a una reserva (relación N:M).
/// </summary>
[Table("reservas_vuelos")]
[Index(nameof(IdReserva), Name = "idx_reservavuelo_reserva")]
[Index(nameof(IdVuelo), Name = "idx_reservavuelo_vuelo")]
public class ReservaVuelo
{
    #region Primary Key

    /// <summary>
    /// Identificador único de la relación Reserva-Vuelo
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    #endregion

    #region Foreign Keys

    /// <summary>
    /// ID de la reserva asociada
    /// </summary>
    [Required(ErrorMessage = "La reserva es obligatoria")]
    [Column("id_reserva")]
    public int IdReserva { get; set; }

    /// <summary>
    /// ID del vuelo asociado
    /// </summary>
    [Required(ErrorMessage = "El vuelo es obligatorio")]
    [Column("id_vuelo")]
    public int IdVuelo { get; set; }

    #endregion

    #region Flight Details

    /// <summary>
    /// Número de pasajeros para este vuelo
    /// </summary>
    [Required(ErrorMessage = "El número de pasajeros es obligatorio")]
    [Range(1, 100, ErrorMessage = "El número de pasajeros debe estar entre 1 y 100")]
    [Column("numero_pasajeros")]
    public int NumeroPasajeros { get; set; }

    /// <summary>
    /// Clase del vuelo: economica, ejecutiva
    /// </summary>
    [Required(ErrorMessage = "La clase es obligatoria")]
    [StringLength(20, ErrorMessage = "La clase no puede exceder 20 caracteres")]
    [Column("clase")]
    public string Clase { get; set; } = "economica";

    /// <summary>
    /// Asientos asignados en formato JSON array
    /// Ejemplo: ["12A", "12B", "13A"]
    /// </summary>
    [StringLength(500, ErrorMessage = "Los asientos asignados no pueden exceder 500 caracteres")]
    [Column("asientos_asignados")]
    public string? AsientosAsignados { get; set; }

    #endregion

    #region Financial Information

    /// <summary>
    /// Precio por pasajero al momento de la reserva
    /// Se toma del vuelo según la clase seleccionada
    /// </summary>
    [Required]
    [Column("precio_por_pasajero", TypeName = "DECIMAL(10,2)")]
    [Range(0.01, 999999999.99, ErrorMessage = "El precio por pasajero debe ser mayor a 0")]
    public decimal PrecioPorPasajero { get; set; }

    /// <summary>
    /// Subtotal calculado: NumeroPasajeros * PrecioPorPasajero + EquipajeExtra
    /// Se calcula automáticamente en el servicio
    /// </summary>
    [Required]
    [Column("subtotal", TypeName = "DECIMAL(10,2)")]
    [Range(0, 999999999.99, ErrorMessage = "El subtotal debe ser positivo")]
    public decimal Subtotal { get; set; }

    #endregion

    #region Baggage Information

    /// <summary>
    /// Indica si el equipaje está incluido en el precio
    /// </summary>
    [Required]
    [Column("equipaje_incluido")]
    public bool EquipajeIncluido { get; set; } = true;

    /// <summary>
    /// Kilogramos de equipaje extra contratado
    /// </summary>
    [Range(0, 200, ErrorMessage = "El equipaje extra debe estar entre 0 y 200 kg")]
    [Column("equipaje_extra")]
    public int? EquipajeExtra { get; set; }

    /// <summary>
    /// Costo adicional por equipaje extra (si aplica)
    /// </summary>
    [Column("costo_equipaje_extra", TypeName = "DECIMAL(10,2)")]
    [Range(0, 999999.99, ErrorMessage = "El costo de equipaje extra debe ser positivo")]
    public decimal CostoEquipajeExtra { get; set; } = 0;

    #endregion

    #region Audit Fields

    /// <summary>
    /// Fecha y hora en que se agregó el vuelo a la reserva
    /// </summary>
    [Required]
    [Column("fecha_agregado")]
    public DateTime FechaAgregado { get; set; } = DateTime.Now;

    #endregion

    #region Navigation Properties

    /// <summary>
    /// Reserva asociada (relación N:1)
    /// </summary>
    [ForeignKey("IdReserva")]
    public virtual Reserva? Reserva { get; set; }

    /// <summary>
    /// Vuelo asociado (relación N:1)
    /// </summary>
    [ForeignKey("IdVuelo")]
    public virtual Vuelo? Vuelo { get; set; }

    #endregion

    #region Computed Properties

    /// <summary>
    /// Indica si el vuelo es de clase ejecutiva
    /// </summary>
    [NotMapped]
    public bool EsClaseEjecutiva => Clase?.ToLower() == "ejecutiva";

    /// <summary>
    /// Indica si tiene equipaje extra contratado
    /// </summary>
    [NotMapped]
    public bool TieneEquipajeExtra => EquipajeExtra.HasValue && EquipajeExtra.Value > 0;

    /// <summary>
    /// Costo total del vuelo (incluye equipaje extra)
    /// </summary>
    [NotMapped]
    public decimal CostoTotal => Subtotal + CostoEquipajeExtra;

    #endregion
}
