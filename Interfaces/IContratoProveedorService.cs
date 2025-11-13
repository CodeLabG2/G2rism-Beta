using G2rismBeta.API.DTOs.ContratoProveedor;

namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interfaz del servicio de Contratos de Proveedor
/// Define la lógica de negocio para la gestión de contratos
/// </summary>
public interface IContratoProveedorService
{
    // ========================================
    // OPERACIONES CRUD
    // ========================================

    /// <summary>
    /// Obtener todos los contratos
    /// </summary>
    Task<IEnumerable<ContratoProveedorResponseDto>> GetAllAsync();

    /// <summary>
    /// Obtener un contrato por ID
    /// </summary>
    Task<ContratoProveedorResponseDto?> GetByIdAsync(int id);

    /// <summary>
    /// Crear un nuevo contrato
    /// </summary>
    Task<ContratoProveedorResponseDto> CreateAsync(ContratoProveedorCreateDto dto);

    /// <summary>
    /// Actualizar un contrato existente
    /// </summary>
    Task<ContratoProveedorResponseDto> UpdateAsync(int id, ContratoProveedorUpdateDto dto);

    /// <summary>
    /// Eliminar un contrato
    /// </summary>
    Task<bool> DeleteAsync(int id);

    // ========================================
    // BÚSQUEDAS Y FILTROS
    // ========================================

    /// <summary>
    /// Buscar contrato por número de contrato
    /// </summary>
    Task<ContratoProveedorResponseDto?> GetByNumeroContratoAsync(string numeroContrato);

    /// <summary>
    /// Obtener contratos de un proveedor específico
    /// </summary>
    Task<IEnumerable<ContratoProveedorResponseDto>> GetByProveedorAsync(int idProveedor);

    /// <summary>
    /// Obtener contratos por estado
    /// </summary>
    Task<IEnumerable<ContratoProveedorResponseDto>> GetByEstadoAsync(string estado);

    /// <summary>
    /// Obtener contratos vigentes
    /// </summary>
    Task<IEnumerable<ContratoProveedorResponseDto>> GetVigentesAsync();

    /// <summary>
    /// Obtener contratos próximos a vencer
    /// </summary>
    Task<IEnumerable<ContratoProveedorResponseDto>> GetProximosAVencerAsync(int diasAnticipacion = 30);

    /// <summary>
    /// Obtener contratos vencidos
    /// </summary>
    Task<IEnumerable<ContratoProveedorResponseDto>> GetVencidosAsync();

    /// <summary>
    /// Obtener contratos con renovación automática
    /// </summary>
    Task<IEnumerable<ContratoProveedorResponseDto>> GetConRenovacionAutomaticaAsync();

    // ========================================
    // GESTIÓN DE ESTADO
    // ========================================

    /// <summary>
    /// Cambiar estado de un contrato
    /// </summary>
    Task<ContratoProveedorResponseDto> CambiarEstadoAsync(int id, string nuevoEstado);

    /// <summary>
    /// Renovar un contrato (crear nuevo contrato basado en uno existente)
    /// </summary>
    Task<ContratoProveedorResponseDto> RenovarContratoAsync(int id, DateTime nuevaFechaFin);

    /// <summary>
    /// Cancelar un contrato
    /// </summary>
    Task<ContratoProveedorResponseDto> CancelarContratoAsync(int id, string motivo);

    // ========================================
    // ESTADÍSTICAS Y REPORTES
    // ========================================

    /// <summary>
    /// Obtener estadísticas de contratos por estado
    /// </summary>
    Task<Dictionary<string, int>> GetEstadisticasPorEstadoAsync();

    /// <summary>
    /// Obtener valor total de contratos vigentes
    /// </summary>
    Task<decimal> GetValorTotalContratosVigentesAsync();

    /// <summary>
    /// Obtener contratos de un proveedor en estado específico
    /// </summary>
    Task<IEnumerable<ContratoProveedorResponseDto>> GetByProveedorYEstadoAsync(int idProveedor, string estado);
}
