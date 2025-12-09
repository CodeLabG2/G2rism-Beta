using G2rismBeta.API.Models;

namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interfaz para el repositorio de ReservaVuelo
/// Define las operaciones de acceso a datos para la relación Reservas-Vuelos
/// </summary>
public interface IReservaVueloRepository : IGenericRepository<ReservaVuelo>
{
    /// <summary>
    /// Obtiene todos los vuelos de una reserva específica con información del vuelo y aerolínea
    /// </summary>
    /// <param name="idReserva">ID de la reserva</param>
    /// <returns>Lista de vuelos de la reserva</returns>
    Task<IEnumerable<ReservaVuelo>> GetVuelosByReservaIdAsync(int idReserva);

    /// <summary>
    /// Obtiene una reserva de vuelo específica con información completa del vuelo
    /// </summary>
    /// <param name="id">ID de la reserva de vuelo</param>
    /// <returns>ReservaVuelo con información completa</returns>
    Task<ReservaVuelo?> GetReservaVueloConDetallesAsync(int id);

    /// <summary>
    /// Verifica si existe una reserva de vuelo
    /// </summary>
    /// <param name="id">ID de la reserva de vuelo</param>
    /// <returns>True si existe</returns>
    Task<bool> ExisteReservaVueloAsync(int id);

    /// <summary>
    /// Cuenta cuántos vuelos tiene una reserva
    /// </summary>
    /// <param name="idReserva">ID de la reserva</param>
    /// <returns>Cantidad de vuelos</returns>
    Task<int> ContarVuelosPorReservaAsync(int idReserva);
}
