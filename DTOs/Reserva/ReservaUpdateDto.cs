namespace G2rismBeta.API.DTOs.Reserva;

/// <summary>
/// DTO para actualizar una Reserva existente
/// Todos los campos son opcionales (nullable) para permitir actualizaciones parciales
/// </summary>
public class ReservaUpdateDto
{
    /// <summary>
    /// ID del empleado que gestiona la reserva (opcional)
    /// </summary>
    public int? IdEmpleado { get; set; }

    /// <summary>
    /// Descripción general de la reserva (opcional)
    /// </summary>
    public string? Descripcion { get; set; }

    /// <summary>
    /// Fecha de inicio del viaje (opcional)
    /// </summary>
    public DateTime? FechaInicioViaje { get; set; }

    /// <summary>
    /// Fecha de fin del viaje (opcional)
    /// </summary>
    public DateTime? FechaFinViaje { get; set; }

    /// <summary>
    /// Número total de pasajeros (opcional)
    /// </summary>
    public int? NumeroPasajeros { get; set; }

    /// <summary>
    /// Estado de la reserva (pendiente, confirmada, cancelada, completada)
    /// </summary>
    public string? Estado { get; set; }

    /// <summary>
    /// Observaciones adicionales (opcional)
    /// </summary>
    public string? Observaciones { get; set; }

    // Nota: MontoTotal, MontoPagado y SaldoPendiente se calculan automáticamente
    // No se permiten actualizar manualmente desde este DTO
}
