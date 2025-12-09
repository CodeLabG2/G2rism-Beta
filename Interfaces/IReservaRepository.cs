using G2rismBeta.API.Models;

namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interface para el repositorio de Reserva
/// Extiende el repositorio genérico con métodos específicos para Reserva
/// </summary>
public interface IReservaRepository : IGenericRepository<Reserva>
{
    /// <summary>
    /// Obtener una reserva por ID con información relacionada (Cliente y Empleado)
    /// </summary>
    /// <param name="id">ID de la reserva</param>
    /// <returns>Reserva con navegación cargada</returns>
    Task<Reserva?> GetReservaConDetallesAsync(int id);

    /// <summary>
    /// Obtener todas las reservas de un cliente específico
    /// </summary>
    /// <param name="idCliente">ID del cliente</param>
    /// <returns>Lista de reservas del cliente</returns>
    Task<IEnumerable<Reserva>> GetReservasByClienteAsync(int idCliente);

    /// <summary>
    /// Obtener todas las reservas gestionadas por un empleado
    /// </summary>
    /// <param name="idEmpleado">ID del empleado</param>
    /// <returns>Lista de reservas del empleado</returns>
    Task<IEnumerable<Reserva>> GetReservasByEmpleadoAsync(int idEmpleado);

    /// <summary>
    /// Obtener reservas filtradas por estado
    /// </summary>
    /// <param name="estado">Estado de la reserva (pendiente, confirmada, cancelada, completada)</param>
    /// <returns>Lista de reservas con ese estado</returns>
    Task<IEnumerable<Reserva>> GetReservasByEstadoAsync(string estado);

    /// <summary>
    /// Obtener reservas en un rango de fechas
    /// </summary>
    /// <param name="fechaInicio">Fecha inicial</param>
    /// <param name="fechaFin">Fecha final</param>
    /// <returns>Lista de reservas en el rango</returns>
    Task<IEnumerable<Reserva>> GetReservasByRangoFechasAsync(DateTime fechaInicio, DateTime fechaFin);

    /// <summary>
    /// Obtener todas las reservas con información completa (Cliente, Empleado, Servicios)
    /// </summary>
    /// <returns>Lista de reservas con navegación cargada</returns>
    Task<IEnumerable<Reserva>> GetAllConDetallesAsync();

    /// <summary>
    /// Actualizar los montos de una reserva (total, pagado, pendiente)
    /// </summary>
    /// <param name="idReserva">ID de la reserva</param>
    /// <param name="montoTotal">Monto total</param>
    /// <param name="montoPagado">Monto pagado</param>
    /// <returns>True si se actualizó correctamente</returns>
    Task<bool> ActualizarMontosAsync(int idReserva, decimal montoTotal, decimal montoPagado);

    /// <summary>
    /// Verificar si un cliente tiene reservas activas
    /// </summary>
    /// <param name="idCliente">ID del cliente</param>
    /// <returns>True si tiene reservas activas</returns>
    Task<bool> ClienteTieneReservasActivasAsync(int idCliente);

    /// <summary>
    /// Verificar si existe una reserva con el ID especificado
    /// </summary>
    /// <param name="idReserva">ID de la reserva</param>
    /// <returns>True si existe</returns>
    Task<bool> ExisteReservaAsync(int idReserva);
}