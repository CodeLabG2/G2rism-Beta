namespace G2rismBeta.API.DTOs.Reserva;

/// <summary>
/// DTO para respuestas de Reserva
/// Incluye información completa de la reserva con datos calculados
/// </summary>
public class ReservaResponseDto
{
    /// <summary>
    /// Identificador único de la reserva
    /// </summary>
    public int IdReserva { get; set; }

    /// <summary>
    /// ID del cliente
    /// </summary>
    public int IdCliente { get; set; }

    /// <summary>
    /// Nombre completo del cliente
    /// </summary>
    public string NombreCliente { get; set; } = string.Empty;

    /// <summary>
    /// ID del empleado que gestiona
    /// </summary>
    public int IdEmpleado { get; set; }

    /// <summary>
    /// Nombre completo del empleado
    /// </summary>
    public string NombreEmpleado { get; set; } = string.Empty;

    /// <summary>
    /// Descripción de la reserva
    /// </summary>
    public string? Descripcion { get; set; }

    /// <summary>
    /// Fecha de inicio del viaje
    /// </summary>
    public DateTime FechaInicioViaje { get; set; }

    /// <summary>
    /// Fecha de fin del viaje
    /// </summary>
    public DateTime FechaFinViaje { get; set; }

    /// <summary>
    /// Duración del viaje en días (calculado)
    /// </summary>
    public int DuracionDias { get; set; }

    /// <summary>
    /// Número de pasajeros
    /// </summary>
    public int NumeroPasajeros { get; set; }

    /// <summary>
    /// Monto total de la reserva
    /// </summary>
    public decimal MontoTotal { get; set; }

    /// <summary>
    /// Monto pagado hasta el momento
    /// </summary>
    public decimal MontoPagado { get; set; }

    /// <summary>
    /// Saldo pendiente de pago
    /// </summary>
    public decimal SaldoPendiente { get; set; }

    /// <summary>
    /// Porcentaje pagado (0-100)
    /// </summary>
    public decimal PorcentajePagado { get; set; }

    /// <summary>
    /// Estado de la reserva
    /// </summary>
    public string Estado { get; set; } = string.Empty;

    /// <summary>
    /// Indica si está completamente pagada
    /// </summary>
    public bool EstaPagada { get; set; }

    /// <summary>
    /// Indica si tiene saldo pendiente
    /// </summary>
    public bool TieneSaldoPendiente { get; set; }

    /// <summary>
    /// Indica si el viaje ya comenzó
    /// </summary>
    public bool ViajeIniciado { get; set; }

    /// <summary>
    /// Indica si el viaje ya finalizó
    /// </summary>
    public bool ViajeCompleto { get; set; }

    /// <summary>
    /// Días restantes hasta el inicio del viaje
    /// </summary>
    public int DiasHastaViaje { get; set; }

    /// <summary>
    /// Observaciones
    /// </summary>
    public string? Observaciones { get; set; }

    /// <summary>
    /// Fecha de creación de la reserva
    /// </summary>
    public DateTime FechaHora { get; set; }

    /// <summary>
    /// Fecha de última modificación
    /// </summary>
    public DateTime? FechaModificacion { get; set; }
}
