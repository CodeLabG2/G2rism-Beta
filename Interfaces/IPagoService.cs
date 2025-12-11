using G2rismBeta.API.DTOs.Pago;

namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interfaz del servicio para la gestión de Pagos
/// Define las operaciones de negocio para el registro y control de pagos
/// </summary>
public interface IPagoService
{
    /// <summary>
    /// Obtener todos los pagos con sus detalles
    /// </summary>
    /// <returns>Lista de pagos</returns>
    Task<IEnumerable<PagoResponseDto>> GetAllPagosAsync();

    /// <summary>
    /// Obtener un pago por su ID
    /// </summary>
    /// <param name="id">ID del pago</param>
    /// <returns>Pago encontrado</returns>
    Task<PagoResponseDto> GetPagoByIdAsync(int id);

    /// <summary>
    /// Obtener todos los pagos de una factura
    /// </summary>
    /// <param name="idFactura">ID de la factura</param>
    /// <returns>Lista de pagos de la factura</returns>
    Task<IEnumerable<PagoResponseDto>> GetPagosPorFacturaAsync(int idFactura);

    /// <summary>
    /// Obtener pagos por estado
    /// </summary>
    /// <param name="estado">Estado del pago (pendiente, aprobado, rechazado)</param>
    /// <returns>Lista de pagos con ese estado</returns>
    Task<IEnumerable<PagoResponseDto>> GetPagosPorEstadoAsync(string estado);

    /// <summary>
    /// Crear un nuevo pago
    /// Actualiza automáticamente el monto pagado de la factura y reserva
    /// </summary>
    /// <param name="createDto">Datos del nuevo pago</param>
    /// <returns>Pago creado</returns>
    Task<PagoResponseDto> CreatePagoAsync(PagoCreateDto createDto);

    /// <summary>
    /// Actualizar un pago existente
    /// </summary>
    /// <param name="id">ID del pago a actualizar</param>
    /// <param name="updateDto">Datos a actualizar</param>
    /// <returns>Pago actualizado</returns>
    Task<PagoResponseDto> UpdatePagoAsync(int id, PagoUpdateDto updateDto);

    /// <summary>
    /// Cambiar el estado de un pago
    /// Actualiza automáticamente los montos de factura y reserva
    /// </summary>
    /// <param name="id">ID del pago</param>
    /// <param name="nuevoEstado">Nuevo estado del pago</param>
    /// <returns>Pago actualizado</returns>
    Task<PagoResponseDto> CambiarEstadoPagoAsync(int id, string nuevoEstado);

    /// <summary>
    /// Eliminar un pago (solo si está pendiente)
    /// </summary>
    /// <param name="id">ID del pago a eliminar</param>
    Task DeletePagoAsync(int id);
}