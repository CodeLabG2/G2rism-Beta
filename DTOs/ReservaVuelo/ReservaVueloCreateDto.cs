using System.ComponentModel.DataAnnotations;

namespace G2rismBeta.API.DTOs.ReservaVuelo;

/// <summary>
/// DTO para crear una nueva relación entre Reserva y Vuelo
/// </summary>
public class ReservaVueloCreateDto
{
    /// <summary>
    /// ID de la reserva (se pasa por URL)
    /// </summary>
    [Required(ErrorMessage = "La reserva es obligatoria")]
    public int IdReserva { get; set; }

    /// <summary>
    /// ID del vuelo a agregar
    /// </summary>
    [Required(ErrorMessage = "El vuelo es obligatorio")]
    public int IdVuelo { get; set; }

    /// <summary>
    /// Número de pasajeros para este vuelo
    /// </summary>
    [Required(ErrorMessage = "El número de pasajeros es obligatorio")]
    [Range(1, 100, ErrorMessage = "El número de pasajeros debe estar entre 1 y 100")]
    public int NumeroPasajeros { get; set; }

    /// <summary>
    /// Clase del vuelo: economica, ejecutiva
    /// </summary>
    [Required(ErrorMessage = "La clase es obligatoria")]
    [StringLength(20, ErrorMessage = "La clase no puede exceder 20 caracteres")]
    [RegularExpression("^(economica|ejecutiva)$", ErrorMessage = "La clase debe ser 'economica' o 'ejecutiva'")]
    public string Clase { get; set; } = "economica";

    /// <summary>
    /// Asientos asignados (opcional, puede ser JSON array)
    /// Ejemplo: ["12A", "12B", "13A"]
    /// </summary>
    [StringLength(500, ErrorMessage = "Los asientos asignados no pueden exceder 500 caracteres")]
    public string? AsientosAsignados { get; set; }

    /// <summary>
    /// Indica si el equipaje está incluido en el precio
    /// </summary>
    public bool EquipajeIncluido { get; set; } = true;

    /// <summary>
    /// Kilogramos de equipaje extra contratado (opcional)
    /// </summary>
    [Range(0, 200, ErrorMessage = "El equipaje extra debe estar entre 0 y 200 kg")]
    public int? EquipajeExtra { get; set; }

    /// <summary>
    /// Costo adicional por equipaje extra (si aplica)
    /// </summary>
    [Range(0, 999999.99, ErrorMessage = "El costo de equipaje extra debe ser positivo")]
    public decimal CostoEquipajeExtra { get; set; } = 0;
}