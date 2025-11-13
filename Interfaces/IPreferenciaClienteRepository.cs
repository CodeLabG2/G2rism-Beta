using G2rismBeta.API.Models;

namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interface específica para operaciones con Preferencias de Cliente
/// Extiende el repositorio genérico y agrega métodos específicos
/// </summary>
public interface IPreferenciaClienteRepository : IGenericRepository<PreferenciaCliente>
{
    /// <summary>
    /// Obtener preferencia por ID de cliente (relación 1:1)
    /// </summary>
    Task<PreferenciaCliente?> GetByClienteIdAsync(int idCliente);

    /// <summary>
    /// Obtener preferencia con información del cliente incluida
    /// </summary>
    Task<PreferenciaCliente?> GetPreferenciaConClienteAsync(int idPreferencia);

    /// <summary>
    /// Verificar si un cliente ya tiene preferencias
    /// </summary>
    Task<bool> ClienteTienePreferenciasAsync(int idCliente);

    /// <summary>
    /// Obtener todas las preferencias con información de clientes
    /// </summary>
    Task<IEnumerable<PreferenciaCliente>> GetAllConClientesAsync();

    /// <summary>
    /// Buscar preferencias por tipo de destino
    /// </summary>
    Task<IEnumerable<PreferenciaCliente>> GetByTipoDestinoAsync(string tipoDestino);

    /// <summary>
    /// Buscar preferencias por rango de presupuesto
    /// </summary>
    Task<IEnumerable<PreferenciaCliente>> GetByRangoPresupuestoAsync(decimal min, decimal max);
}