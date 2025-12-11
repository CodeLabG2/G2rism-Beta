using G2rismBeta.API.Models;

namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interfaz del repositorio para la entidad Pago
/// Extiende el repositorio genérico y agrega métodos específicos para pagos
/// </summary>
public interface IPagoRepository : IGenericRepository<Pago>
{
    /// <summary>
    /// Obtener pago con información de factura y forma de pago
    /// </summary>
    /// <param name="id">ID del pago</param>
    /// <returns>Pago con relaciones cargadas</returns>
    Task<Pago?> GetPagoConDetallesAsync(int id);

    /// <summary>
    /// Obtener todos los pagos de una factura
    /// </summary>
    /// <param name="idFactura">ID de la factura</param>
    /// <returns>Lista de pagos</returns>
    Task<IEnumerable<Pago>> GetPagosPorFacturaAsync(int idFactura);

    /// <summary>
    /// Obtener pagos por estado
    /// </summary>
    /// <param name="estado">Estado del pago (pendiente, aprobado, rechazado)</param>
    /// <returns>Lista de pagos con ese estado</returns>
    Task<IEnumerable<Pago>> GetPagosPorEstadoAsync(string estado);

    /// <summary>
    /// Obtener pagos en un rango de fechas
    /// </summary>
    /// <param name="fechaInicio">Fecha inicio</param>
    /// <param name="fechaFin">Fecha fin</param>
    /// <returns>Lista de pagos en el rango</returns>
    Task<IEnumerable<Pago>> GetPagosPorRangoFechasAsync(DateTime fechaInicio, DateTime fechaFin);

    /// <summary>
    /// Obtener todos los pagos con detalles (factura y forma de pago)
    /// </summary>
    /// <returns>Lista de pagos con relaciones cargadas</returns>
    Task<IEnumerable<Pago>> GetAllPagosConDetallesAsync();

    /// <summary>
    /// Calcular total pagado de una factura
    /// </summary>
    /// <param name="idFactura">ID de la factura</param>
    /// <returns>Total pagado (solo pagos aprobados)</returns>
    Task<decimal> GetTotalPagadoPorFacturaAsync(int idFactura);

    /// <summary>
    /// Verificar si existe un pago con una referencia de transacción específica
    /// </summary>
    /// <param name="referenciaTransaccion">Referencia a buscar</param>
    /// <returns>True si existe</returns>
    Task<bool> ExistePorReferenciaAsync(string referenciaTransaccion);
}
