using G2rismBeta.API.DTOs.PreferenciaCliente;

namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interface del servicio de Preferencias de Cliente
/// Define la lógica de negocio para gestión de preferencias (CRM)
/// </summary>
public interface IPreferenciaClienteService
{
    // ========================================
    // OPERACIONES CRUD BÁSICAS
    // ========================================

    /// <summary>
    /// Obtener todas las preferencias
    /// </summary>
    Task<IEnumerable<PreferenciaClienteResponseDto>> GetAllPreferenciasAsync();

    /// <summary>
    /// Obtener una preferencia por su ID
    /// </summary>
    Task<PreferenciaClienteResponseDto?> GetPreferenciaByIdAsync(int idPreferencia);

    /// <summary>
    /// Obtener preferencia por ID de cliente (relación 1:1)
    /// </summary>
    Task<PreferenciaClienteResponseDto?> GetPreferenciaByClienteIdAsync(int idCliente);

    /// <summary>
    /// Crear nuevas preferencias para un cliente
    /// </summary>
    Task<PreferenciaClienteResponseDto> CreatePreferenciaAsync(PreferenciaClienteCreateDto preferenciaCreateDto);

    /// <summary>
    /// Actualizar preferencias existentes
    /// </summary>
    Task<PreferenciaClienteResponseDto> UpdatePreferenciaAsync(int idPreferencia, PreferenciaClienteUpdateDto preferenciaUpdateDto);

    /// <summary>
    /// Eliminar preferencias de un cliente
    /// </summary>
    Task<bool> DeletePreferenciaAsync(int idPreferencia);

    // ========================================
    // OPERACIONES DE BÚSQUEDA (CRM)
    // ========================================

    /// <summary>
    /// Buscar preferencias por tipo de destino
    /// </summary>
    Task<IEnumerable<PreferenciaClienteResponseDto>> GetPreferenciasByTipoDestinoAsync(string tipoDestino);

    /// <summary>
    /// Buscar preferencias por rango de presupuesto
    /// </summary>
    Task<IEnumerable<PreferenciaClienteResponseDto>> GetPreferenciasByRangoPresupuestoAsync(decimal min, decimal max);

    // ========================================
    // VALIDACIONES
    // ========================================

    /// <summary>
    /// Verificar si un cliente ya tiene preferencias registradas
    /// </summary>
    Task<bool> ClienteTienePreferenciasAsync(int idCliente);
}