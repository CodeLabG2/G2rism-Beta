using G2rismBeta.API.DTOs.ReservaHotel;
using G2rismBeta.API.DTOs.ReservaVuelo;
using G2rismBeta.API.DTOs.ReservaPaquete;
using G2rismBeta.API.DTOs.ReservaServicio;

namespace G2rismBeta.API.DTOs.Reserva;

/// <summary>
/// DTO para crear una reserva completa con todos los servicios en una sola petición
/// Este DTO permite crear una reserva y asignarle hoteles, vuelos, paquetes y servicios adicionales
/// en una única transacción atómica
/// </summary>
public class ReservaCompletaCreateDto
{
    // ========================================
    // DATOS BÁSICOS DE LA RESERVA
    // ========================================

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
    /// Fecha de inicio del viaje
    /// </summary>
    public DateTime FechaInicioViaje { get; set; }

    /// <summary>
    /// Fecha de fin del viaje
    /// </summary>
    public DateTime FechaFinViaje { get; set; }

    /// <summary>
    /// Número total de pasajeros
    /// </summary>
    public int NumeroPasajeros { get; set; }

    /// <summary>
    /// Estado inicial de la reserva (pendiente o confirmada)
    /// Por defecto: pendiente
    /// </summary>
    public string Estado { get; set; } = "pendiente";

    /// <summary>
    /// Observaciones adicionales de la reserva
    /// </summary>
    public string? Observaciones { get; set; }

    // ========================================
    // SERVICIOS A INCLUIR EN LA RESERVA
    // ========================================

    /// <summary>
    /// Lista de hoteles a agregar a la reserva
    /// Puede estar vacía si no se requieren hoteles
    /// </summary>
    public List<ReservaHotelCreateDto> Hoteles { get; set; } = new();

    /// <summary>
    /// Lista de vuelos a agregar a la reserva
    /// Puede estar vacía si no se requieren vuelos
    /// </summary>
    public List<ReservaVueloCreateDto> Vuelos { get; set; } = new();

    /// <summary>
    /// Lista de paquetes turísticos a agregar a la reserva
    /// Puede estar vacía si no se requieren paquetes
    /// </summary>
    public List<ReservaPaqueteCreateDto> Paquetes { get; set; } = new();

    /// <summary>
    /// Lista de servicios adicionales a agregar a la reserva
    /// Puede estar vacía si no se requieren servicios adicionales
    /// </summary>
    public List<ReservaServicioCreateDto> Servicios { get; set; } = new();
}
