using G2rismBeta.API.Models;

namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interface del repositorio de Facturas.
/// Extiende el repositorio genérico con operaciones específicas de facturas.
/// </summary>
public interface IFacturaRepository : IGenericRepository<Factura>
{
    /// <summary>
    /// Obtener una factura por su ID con la reserva y pagos incluidos
    /// </summary>
    /// <param name="id">ID de la factura</param>
    /// <returns>Factura con navegación completa o null si no existe</returns>
    Task<Factura?> GetFacturaConDetallesAsync(int id);

    /// <summary>
    /// Obtener todas las facturas de una reserva específica
    /// </summary>
    /// <param name="idReserva">ID de la reserva</param>
    /// <returns>Lista de facturas de la reserva</returns>
    Task<IEnumerable<Factura>> GetFacturasPorReservaAsync(int idReserva);

    /// <summary>
    /// Obtener una factura por su número de factura
    /// </summary>
    /// <param name="numeroFactura">Número único de la factura</param>
    /// <returns>Factura encontrada o null</returns>
    Task<Factura?> GetFacturaPorNumeroAsync(string numeroFactura);

    /// <summary>
    /// Obtener facturas por estado
    /// </summary>
    /// <param name="estado">Estado de la factura (pendiente, pagada, cancelada, vencida)</param>
    /// <returns>Lista de facturas con ese estado</returns>
    Task<IEnumerable<Factura>> GetFacturasPorEstadoAsync(string estado);

    /// <summary>
    /// Obtener facturas vencidas (fecha de vencimiento pasada y aún pendientes)
    /// </summary>
    /// <returns>Lista de facturas vencidas</returns>
    Task<IEnumerable<Factura>> GetFacturasVencidasAsync();

    /// <summary>
    /// Obtener facturas próximas a vencer (en los próximos N días)
    /// </summary>
    /// <param name="dias">Número de días para considerar "próximo a vencer"</param>
    /// <returns>Lista de facturas próximas a vencer</returns>
    Task<IEnumerable<Factura>> GetFacturasProximasAVencerAsync(int dias = 7);

    /// <summary>
    /// Verificar si existe una factura con un número específico
    /// </summary>
    /// <param name="numeroFactura">Número de factura a verificar</param>
    /// <returns>True si existe, False si no</returns>
    Task<bool> ExistePorNumeroAsync(string numeroFactura);

    /// <summary>
    /// Obtener el último número de factura del año actual para generar consecutivo
    /// </summary>
    /// <param name="anio">Año para buscar el consecutivo</param>
    /// <returns>Último número de factura del año o null si no hay</returns>
    Task<string?> GetUltimoNumeroFacturaDelAnioAsync(int anio);

    /// <summary>
    /// Verificar si ya existe una factura para una reserva
    /// </summary>
    /// <param name="idReserva">ID de la reserva</param>
    /// <returns>True si ya existe, False si no</returns>
    Task<bool> ExisteFacturaParaReservaAsync(int idReserva);
}