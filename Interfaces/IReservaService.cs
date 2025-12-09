using G2rismBeta.API.DTOs.Reserva;

namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interface del servicio de Reservas
/// Define la lógica de negocio para gestión de reservas
/// </summary>
public interface IReservaService
{
    // ========================================
    // OPERACIONES CRUD BÁSICAS
    // ========================================

    /// <summary>
    /// Obtener todas las reservas
    /// </summary>
    /// <returns>Lista de todas las reservas</returns>
    Task<IEnumerable<ReservaResponseDto>> GetAllReservasAsync();

    /// <summary>
    /// Obtener una reserva por su ID
    /// </summary>
    /// <param name="idReserva">ID de la reserva</param>
    /// <returns>Reserva encontrada o null</returns>
    Task<ReservaResponseDto?> GetReservaByIdAsync(int idReserva);

    /// <summary>
    /// Crear una nueva reserva básica (sin servicios)
    /// </summary>
    /// <param name="reservaCreateDto">Datos de la reserva</param>
    /// <returns>Reserva creada</returns>
    Task<ReservaResponseDto> CreateReservaAsync(ReservaCreateDto reservaCreateDto);

    /// <summary>
    /// Crear una reserva completa con todos los servicios en una sola transacción
    /// Incluye: reserva base, hoteles, vuelos, paquetes y servicios adicionales
    /// </summary>
    /// <param name="reservaCompletaDto">Datos completos de la reserva</param>
    /// <returns>Reserva creada con todos los servicios asociados</returns>
    Task<ReservaResponseDto> CreateReservaCompletaAsync(ReservaCompletaCreateDto reservaCompletaDto);

    /// <summary>
    /// Actualizar una reserva existente
    /// </summary>
    /// <param name="idReserva">ID de la reserva</param>
    /// <param name="reservaUpdateDto">Datos a actualizar</param>
    /// <returns>Reserva actualizada</returns>
    Task<ReservaResponseDto> UpdateReservaAsync(int idReserva, ReservaUpdateDto reservaUpdateDto);

    /// <summary>
    /// Eliminar una reserva (soft delete)
    /// Solo si está en estado "pendiente"
    /// </summary>
    /// <param name="idReserva">ID de la reserva</param>
    /// <returns>True si se eliminó correctamente</returns>
    Task<bool> DeleteReservaAsync(int idReserva);

    // ========================================
    // CONSULTAS ESPECIALIZADAS
    // ========================================

    /// <summary>
    /// Obtener todas las reservas de un cliente
    /// </summary>
    /// <param name="idCliente">ID del cliente</param>
    /// <returns>Lista de reservas del cliente</returns>
    Task<IEnumerable<ReservaResponseDto>> GetReservasByClienteAsync(int idCliente);

    /// <summary>
    /// Obtener todas las reservas gestionadas por un empleado
    /// </summary>
    /// <param name="idEmpleado">ID del empleado</param>
    /// <returns>Lista de reservas del empleado</returns>
    Task<IEnumerable<ReservaResponseDto>> GetReservasByEmpleadoAsync(int idEmpleado);

    /// <summary>
    /// Obtener reservas por estado
    /// </summary>
    /// <param name="estado">Estado (pendiente, confirmada, cancelada, completada)</param>
    /// <returns>Lista de reservas con ese estado</returns>
    Task<IEnumerable<ReservaResponseDto>> GetReservasByEstadoAsync(string estado);

    /// <summary>
    /// Obtener reservas en un rango de fechas
    /// </summary>
    /// <param name="fechaInicio">Fecha inicial</param>
    /// <param name="fechaFin">Fecha final</param>
    /// <returns>Lista de reservas en el rango</returns>
    Task<IEnumerable<ReservaResponseDto>> GetReservasByRangoFechasAsync(DateTime fechaInicio, DateTime fechaFin);

    // ========================================
    // OPERACIONES DE NEGOCIO
    // ========================================

    /// <summary>
    /// Cambiar el estado de una reserva
    /// </summary>
    /// <param name="idReserva">ID de la reserva</param>
    /// <param name="nuevoEstado">Nuevo estado</param>
    /// <returns>Reserva actualizada</returns>
    Task<ReservaResponseDto> CambiarEstadoReservaAsync(int idReserva, string nuevoEstado);

    /// <summary>
    /// Cancelar una reserva
    /// </summary>
    /// <param name="idReserva">ID de la reserva</param>
    /// <param name="motivoCancelacion">Motivo de cancelación</param>
    /// <returns>Reserva cancelada</returns>
    Task<ReservaResponseDto> CancelarReservaAsync(int idReserva, string motivoCancelacion);

    /// <summary>
    /// Confirmar una reserva (cambiar de pendiente a confirmada)
    /// </summary>
    /// <param name="idReserva">ID de la reserva</param>
    /// <returns>Reserva confirmada</returns>
    Task<ReservaResponseDto> ConfirmarReservaAsync(int idReserva);

    /// <summary>
    /// Recalcular los montos de una reserva
    /// Se usa después de agregar/quitar servicios
    /// </summary>
    /// <param name="idReserva">ID de la reserva</param>
    /// <returns>True si se recalculó correctamente</returns>
    Task<bool> RecalcularMontosAsync(int idReserva);

    // ========================================
    // VALIDACIONES
    // ========================================

    /// <summary>
    /// Verificar si un cliente existe
    /// </summary>
    /// <param name="idCliente">ID del cliente</param>
    /// <returns>True si existe</returns>
    Task<bool> ClienteExisteAsync(int idCliente);

    /// <summary>
    /// Verificar si un empleado existe
    /// </summary>
    /// <param name="idEmpleado">ID del empleado</param>
    /// <returns>True si existe</returns>
    Task<bool> EmpleadoExisteAsync(int idEmpleado);

    /// <summary>
    /// Verificar si una reserva existe
    /// </summary>
    /// <param name="idReserva">ID de la reserva</param>
    /// <returns>True si existe</returns>
    Task<bool> ReservaExisteAsync(int idReserva);
}