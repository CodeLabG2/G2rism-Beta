namespace G2rismBeta.API.DTOs.Reserva;

/// <summary>
/// DTO para crear una nueva Reserva
/// </summary>
public class ReservaCreateDto
{
    /// <summary>
    /// ID del cliente que realiza la reserva
    /// </summary>
    public int IdCliente { get; set; }

    /// <summary>
    /// ID del empleado que gestiona la reserva
    /// </summary>
    public int IdEmpleado { get; set; }

    /// <summary>
    /// Descripción general de la reserva
    /// </summary>
    public string? Descripcion { get; set; }

    /// <summary>
    /// Fecha de inicio del viaje (formato: yyyy-MM-dd)
    /// </summary>
    public DateTime FechaInicioViaje { get; set; }

    /// <summary>
    /// Fecha de fin del viaje (formato: yyyy-MM-dd)
    /// </summary>
    public DateTime FechaFinViaje { get; set; }

    /// <summary>
    /// Número total de pasajeros
    /// </summary>
    public int NumeroPasajeros { get; set; }

    /// <summary>
    /// Estado inicial de la reserva (pendiente, confirmada)
    /// Por defecto: "pendiente"
    /// </summary>
    public string Estado { get; set; } = "pendiente";

    /// <summary>
    /// Observaciones adicionales sobre la reserva
    /// </summary>
    public string? Observaciones { get; set; }
}
