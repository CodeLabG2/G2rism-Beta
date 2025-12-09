using G2rismBeta.API.DTOs.Factura;

namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interface del servicio de Facturas.
/// Define la lógica de negocio para la gestión de facturas.
/// </summary>
public interface IFacturaService
{
    /// <summary>
    /// Obtener todas las facturas
    /// </summary>
    /// <returns>Lista de facturas</returns>
    Task<IEnumerable<FacturaResponseDto>> GetAllFacturasAsync();

    /// <summary>
    /// Obtener una factura por su ID
    /// </summary>
    /// <param name="id">ID de la factura</param>
    /// <returns>Factura encontrada</returns>
    Task<FacturaResponseDto> GetFacturaByIdAsync(int id);

    /// <summary>
    /// Obtener facturas por ID de reserva
    /// </summary>
    /// <param name="idReserva">ID de la reserva</param>
    /// <returns>Lista de facturas de la reserva</returns>
    Task<IEnumerable<FacturaResponseDto>> GetFacturasPorReservaAsync(int idReserva);

    /// <summary>
    /// Obtener factura por número de factura
    /// </summary>
    /// <param name="numeroFactura">Número único de la factura</param>
    /// <returns>Factura encontrada</returns>
    Task<FacturaResponseDto> GetFacturaPorNumeroAsync(string numeroFactura);

    /// <summary>
    /// Obtener facturas por estado
    /// </summary>
    /// <param name="estado">Estado de la factura</param>
    /// <returns>Lista de facturas con ese estado</returns>
    Task<IEnumerable<FacturaResponseDto>> GetFacturasPorEstadoAsync(string estado);

    /// <summary>
    /// Obtener facturas vencidas
    /// </summary>
    /// <returns>Lista de facturas vencidas</returns>
    Task<IEnumerable<FacturaResponseDto>> GetFacturasVencidasAsync();

    /// <summary>
    /// Obtener facturas próximas a vencer
    /// </summary>
    /// <param name="dias">Días para considerar próximo a vencer</param>
    /// <returns>Lista de facturas próximas a vencer</returns>
    Task<IEnumerable<FacturaResponseDto>> GetFacturasProximasAVencerAsync(int dias = 7);

    /// <summary>
    /// Crear una nueva factura desde una reserva
    /// </summary>
    /// <param name="dto">Datos para crear la factura</param>
    /// <returns>Factura creada</returns>
    Task<FacturaResponseDto> CrearFacturaAsync(FacturaCreateDto dto);

    /// <summary>
    /// Actualizar una factura existente
    /// </summary>
    /// <param name="id">ID de la factura</param>
    /// <param name="dto">Datos actualizados</param>
    /// <returns>Factura actualizada</returns>
    Task<FacturaResponseDto> ActualizarFacturaAsync(int id, FacturaUpdateDto dto);

    /// <summary>
    /// Cambiar el estado de una factura
    /// </summary>
    /// <param name="id">ID de la factura</param>
    /// <param name="nuevoEstado">Nuevo estado</param>
    /// <returns>Factura actualizada</returns>
    Task<FacturaResponseDto> CambiarEstadoFacturaAsync(int id, string nuevoEstado);

    /// <summary>
    /// Eliminar (cancelar) una factura
    /// </summary>
    /// <param name="id">ID de la factura</param>
    /// <returns>True si se eliminó correctamente</returns>
    Task<bool> EliminarFacturaAsync(int id);
}