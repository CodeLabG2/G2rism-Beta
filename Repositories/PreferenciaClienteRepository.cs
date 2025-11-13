using Microsoft.EntityFrameworkCore;
using G2rismBeta.API.Data;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.Models;

namespace G2rismBeta.API.Repositories;

/// <summary>
/// Repositorio específico para operaciones con Preferencias de Cliente
/// Maneja la persistencia y consultas especializadas del CRM
/// </summary>
public class PreferenciaClienteRepository : GenericRepository<PreferenciaCliente>, IPreferenciaClienteRepository
{
    public PreferenciaClienteRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Obtener preferencia por ID de cliente (relación 1:1)
    /// Un cliente solo puede tener una preferencia
    /// </summary>
    public async Task<PreferenciaCliente?> GetByClienteIdAsync(int idCliente)
    {
        return await _dbSet
            .Include(p => p.Cliente)
            .FirstOrDefaultAsync(p => p.IdCliente == idCliente);
    }

    /// <summary>
    /// Obtener preferencia con información del cliente incluida
    /// Útil para mostrar datos completos en la UI
    /// </summary>
    public async Task<PreferenciaCliente?> GetPreferenciaConClienteAsync(int idPreferencia)
    {
        return await _dbSet
            .Include(p => p.Cliente)
                .ThenInclude(c => c.Categoria)
            .FirstOrDefaultAsync(p => p.IdPreferencia == idPreferencia);
    }

    /// <summary>
    /// Verificar si un cliente ya tiene preferencias registradas
    /// Evita duplicados (relación 1:1)
    /// </summary>
    public async Task<bool> ClienteTienePreferenciasAsync(int idCliente)
    {
        return await _dbSet.AnyAsync(p => p.IdCliente == idCliente);
    }

    /// <summary>
    /// Obtener todas las preferencias con información de clientes
    /// Para análisis CRM y segmentación
    /// </summary>
    public async Task<IEnumerable<PreferenciaCliente>> GetAllConClientesAsync()
    {
        return await _dbSet
            .Include(p => p.Cliente)
            .OrderBy(p => p.Cliente.Apellido)
            .ThenBy(p => p.Cliente.Nombre)
            .ToListAsync();
    }

    /// <summary>
    /// Buscar preferencias por tipo de destino
    /// Útil para campañas de marketing segmentadas
    /// </summary>
    public async Task<IEnumerable<PreferenciaCliente>> GetByTipoDestinoAsync(string tipoDestino)
    {
        return await _dbSet
            .Include(p => p.Cliente)
            .Where(p => p.TipoDestino.ToLower() == tipoDestino.ToLower())
            .ToListAsync();
    }

    /// <summary>
    /// Buscar preferencias por rango de presupuesto
    /// Permite segmentar clientes por poder adquisitivo
    /// </summary>
    public async Task<IEnumerable<PreferenciaCliente>> GetByRangoPresupuestoAsync(decimal min, decimal max)
    {
        return await _dbSet
            .Include(p => p.Cliente)
            .Where(p => p.PresupuestoPromedio >= min && p.PresupuestoPromedio <= max)
            .OrderBy(p => p.PresupuestoPromedio)
            .ToListAsync();
    }
}