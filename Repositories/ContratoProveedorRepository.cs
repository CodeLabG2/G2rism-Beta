using G2rismBeta.API.Data;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.Models;
using Microsoft.EntityFrameworkCore;

namespace G2rismBeta.API.Repositories;

/// <summary>
/// Repositorio de Contratos de Proveedor
/// Implementa operaciones de acceso a datos para contratos
/// </summary>
public class ContratoProveedorRepository : GenericRepository<ContratoProveedor>, IContratoProveedorRepository
{
    public ContratoProveedorRepository(ApplicationDbContext context) : base(context)
    {
    }

    // ========================================
    // BÚSQUEDAS BÁSICAS
    // ========================================

    /// <summary>
    /// Buscar contrato por número de contrato
    /// </summary>
    public async Task<ContratoProveedor?> GetByNumeroContratoAsync(string numeroContrato)
    {
        return await _context.ContratosProveedor
            .Include(c => c.Proveedor)
            .FirstOrDefaultAsync(c => c.NumeroContrato == numeroContrato);
    }

    /// <summary>
    /// Obtener contratos de un proveedor específico
    /// </summary>
    public async Task<IEnumerable<ContratoProveedor>> GetByProveedorAsync(int idProveedor)
    {
        return await _context.ContratosProveedor
            .Include(c => c.Proveedor)
            .Where(c => c.IdProveedor == idProveedor)
            .OrderByDescending(c => c.FechaInicio)
            .ToListAsync();
    }

    // ========================================
    // FILTROS ESPECIALIZADOS
    // ========================================

    /// <summary>
    /// Obtener contratos por estado
    /// </summary>
    public async Task<IEnumerable<ContratoProveedor>> GetByEstadoAsync(string estado)
    {
        return await _context.ContratosProveedor
            .Include(c => c.Proveedor)
            .Where(c => c.Estado == estado)
            .OrderByDescending(c => c.FechaInicio)
            .ToListAsync();
    }

    /// <summary>
    /// Obtener contratos vigentes
    /// </summary>
    public async Task<IEnumerable<ContratoProveedor>> GetVigentesAsync()
    {
        var hoy = DateTime.Now.Date;

        return await _context.ContratosProveedor
            .Include(c => c.Proveedor)
            .Where(c => c.Estado == "Vigente" &&
                       c.FechaInicio.Date <= hoy &&
                       c.FechaFin.Date >= hoy)
            .OrderBy(c => c.FechaFin)
            .ToListAsync();
    }

    /// <summary>
    /// Obtener contratos próximos a vencer (menos de X días)
    /// </summary>
    public async Task<IEnumerable<ContratoProveedor>> GetProximosAVencerAsync(int diasAnticipacion = 30)
    {
        var hoy = DateTime.Now.Date;
        var fechaLimite = hoy.AddDays(diasAnticipacion);

        return await _context.ContratosProveedor
            .Include(c => c.Proveedor)
            .Where(c => c.Estado == "Vigente" &&
                       c.FechaFin.Date > hoy &&
                       c.FechaFin.Date <= fechaLimite)
            .OrderBy(c => c.FechaFin)
            .ToListAsync();
    }

    /// <summary>
    /// Obtener contratos vencidos
    /// </summary>
    public async Task<IEnumerable<ContratoProveedor>> GetVencidosAsync()
    {
        var hoy = DateTime.Now.Date;

        return await _context.ContratosProveedor
            .Include(c => c.Proveedor)
            .Where(c => c.FechaFin.Date < hoy &&
                       (c.Estado == "Vigente" || c.Estado == "Vencido"))
            .OrderByDescending(c => c.FechaFin)
            .ToListAsync();
    }

    /// <summary>
    /// Obtener contratos con renovación automática
    /// </summary>
    public async Task<IEnumerable<ContratoProveedor>> GetConRenovacionAutomaticaAsync()
    {
        return await _context.ContratosProveedor
            .Include(c => c.Proveedor)
            .Where(c => c.RenovacionAutomatica == true &&
                       c.Estado == "Vigente")
            .OrderBy(c => c.FechaFin)
            .ToListAsync();
    }

    /// <summary>
    /// Obtener contratos por rango de fechas
    /// </summary>
    public async Task<IEnumerable<ContratoProveedor>> GetByRangoFechasAsync(DateTime fechaInicio, DateTime fechaFin)
    {
        return await _context.ContratosProveedor
            .Include(c => c.Proveedor)
            .Where(c => c.FechaInicio.Date >= fechaInicio.Date &&
                       c.FechaFin.Date <= fechaFin.Date)
            .OrderBy(c => c.FechaInicio)
            .ToListAsync();
    }

    // ========================================
    // ESTADÍSTICAS Y REPORTES
    // ========================================

    /// <summary>
    /// Contar contratos por estado
    /// </summary>
    public async Task<Dictionary<string, int>> CountByEstadoAsync()
    {
        return await _context.ContratosProveedor
            .GroupBy(c => c.Estado)
            .Select(g => new { Estado = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Estado, x => x.Count);
    }

    /// <summary>
    /// Obtener valor total de contratos vigentes
    /// </summary>
    public async Task<decimal> GetValorTotalContratosVigentesAsync()
    {
        var hoy = DateTime.Now.Date;

        return await _context.ContratosProveedor
            .Where(c => c.Estado == "Vigente" &&
                       c.FechaInicio.Date <= hoy &&
                       c.FechaFin.Date >= hoy)
            .SumAsync(c => c.ValorContrato);
    }

    /// <summary>
    /// Obtener contratos de un proveedor en estado específico
    /// </summary>
    public async Task<IEnumerable<ContratoProveedor>> GetByProveedorYEstadoAsync(int idProveedor, string estado)
    {
        return await _context.ContratosProveedor
            .Include(c => c.Proveedor)
            .Where(c => c.IdProveedor == idProveedor && c.Estado == estado)
            .OrderByDescending(c => c.FechaInicio)
            .ToListAsync();
    }

    // ========================================
    // VALIDACIONES
    // ========================================

    /// <summary>
    /// Verificar si existe un contrato con el número dado
    /// </summary>
    public async Task<bool> ExisteNumeroContratoAsync(string numeroContrato, int? excludeId = null)
    {
        var query = _context.ContratosProveedor.Where(c => c.NumeroContrato == numeroContrato);

        if (excludeId.HasValue)
        {
            query = query.Where(c => c.IdContrato != excludeId.Value);
        }

        return await query.AnyAsync();
    }

    /// <summary>
    /// Verificar si un proveedor tiene contratos vigentes
    /// </summary>
    public async Task<bool> ProveedorTieneContratosVigentesAsync(int idProveedor)
    {
        var hoy = DateTime.Now.Date;

        return await _context.ContratosProveedor
            .AnyAsync(c => c.IdProveedor == idProveedor &&
                          c.Estado == "Vigente" &&
                          c.FechaInicio.Date <= hoy &&
                          c.FechaFin.Date >= hoy);
    }
}
