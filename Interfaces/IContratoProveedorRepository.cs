using G2rismBeta.API.Models;

namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interfaz del repositorio de Contratos de Proveedor
/// Define las operaciones de acceso a datos para contratos
/// </summary>
public interface IContratoProveedorRepository : IGenericRepository<ContratoProveedor>
{
    // ========================================
    // BÚSQUEDAS BÁSICAS
    // ========================================

    /// <summary>
    /// Buscar contrato por número de contrato
    /// </summary>
    Task<ContratoProveedor?> GetByNumeroContratoAsync(string numeroContrato);

    /// <summary>
    /// Obtener contratos de un proveedor específico
    /// </summary>
    Task<IEnumerable<ContratoProveedor>> GetByProveedorAsync(int idProveedor);

    // ========================================
    // FILTROS ESPECIALIZADOS
    // ========================================

    /// <summary>
    /// Obtener contratos por estado
    /// </summary>
    Task<IEnumerable<ContratoProveedor>> GetByEstadoAsync(string estado);

    /// <summary>
    /// Obtener contratos vigentes
    /// </summary>
    Task<IEnumerable<ContratoProveedor>> GetVigentesAsync();

    /// <summary>
    /// Obtener contratos próximos a vencer (menos de X días)
    /// </summary>
    Task<IEnumerable<ContratoProveedor>> GetProximosAVencerAsync(int diasAnticipacion = 30);

    /// <summary>
    /// Obtener contratos vencidos
    /// </summary>
    Task<IEnumerable<ContratoProveedor>> GetVencidosAsync();

    /// <summary>
    /// Obtener contratos con renovación automática
    /// </summary>
    Task<IEnumerable<ContratoProveedor>> GetConRenovacionAutomaticaAsync();

    /// <summary>
    /// Obtener contratos por rango de fechas
    /// </summary>
    Task<IEnumerable<ContratoProveedor>> GetByRangoFechasAsync(DateTime fechaInicio, DateTime fechaFin);

    // ========================================
    // ESTADÍSTICAS Y REPORTES
    // ========================================

    /// <summary>
    /// Contar contratos por estado
    /// </summary>
    Task<Dictionary<string, int>> CountByEstadoAsync();

    /// <summary>
    /// Obtener valor total de contratos vigentes
    /// </summary>
    Task<decimal> GetValorTotalContratosVigentesAsync();

    /// <summary>
    /// Obtener contratos de un proveedor en estado específico
    /// </summary>
    Task<IEnumerable<ContratoProveedor>> GetByProveedorYEstadoAsync(int idProveedor, string estado);

    // ========================================
    // VALIDACIONES
    // ========================================

    /// <summary>
    /// Verificar si existe un contrato con el número dado (excluyendo un ID específico)
    /// </summary>
    Task<bool> ExisteNumeroContratoAsync(string numeroContrato, int? excludeId = null);

    /// <summary>
    /// Verificar si un proveedor tiene contratos vigentes
    /// </summary>
    Task<bool> ProveedorTieneContratosVigentesAsync(int idProveedor);
}
