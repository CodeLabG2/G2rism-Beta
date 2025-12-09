using G2rismBeta.API.DTOs.ReservaVuelo;

namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interfaz para el servicio de ReservaVuelo
/// Define la lógica de negocio para la gestión de vuelos en reservas
/// </summary>
public interface IReservaVueloService
{
    /// <summary>
    /// Agrega un vuelo a una reserva existente
    /// </summary>
    /// <param name="dto">Datos del vuelo a agregar</param>
    /// <returns>ReservaVueloResponseDto con el vuelo agregado</returns>
    Task<ReservaVueloResponseDto> AgregarVueloAReservaAsync(ReservaVueloCreateDto dto);

    /// <summary>
    /// Obtiene todos los vuelos de una reserva
    /// </summary>
    /// <param name="idReserva">ID de la reserva</param>
    /// <returns>Lista de vuelos de la reserva</returns>
    Task<IEnumerable<ReservaVueloResponseDto>> GetVuelosPorReservaAsync(int idReserva);

    /// <summary>
    /// Obtiene un vuelo específico de una reserva
    /// </summary>
    /// <param name="id">ID de la reserva de vuelo</param>
    /// <returns>Detalles del vuelo en la reserva</returns>
    Task<ReservaVueloResponseDto> GetReservaVueloPorIdAsync(int id);

    /// <summary>
    /// Elimina un vuelo de una reserva
    /// </summary>
    /// <param name="id">ID de la reserva de vuelo</param>
    /// <returns>True si se eliminó correctamente</returns>
    Task<bool> EliminarVueloDeReservaAsync(int id);
}
