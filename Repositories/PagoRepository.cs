using G2rismBeta.API.Data;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.Models;
using Microsoft.EntityFrameworkCore;

namespace G2rismBeta.API.Repositories;

/// <summary>
/// Repositorio para la entidad Pago
/// Implementa operaciones específicas de acceso a datos para pagos
/// </summary>
public class PagoRepository : GenericRepository<Pago>, IPagoRepository
{
    public PagoRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <inheritdoc/>
    public async Task<Pago?> GetPagoConDetallesAsync(int id)
    {
        return await _context.Pagos
            .Include(p => p.Factura)
            .Include(p => p.FormaDePago)
            .FirstOrDefaultAsync(p => p.IdPago == id);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Pago>> GetPagosPorFacturaAsync(int idFactura)
    {
        return await _context.Pagos
            .Include(p => p.FormaDePago)
            .Where(p => p.IdFactura == idFactura)
            .OrderByDescending(p => p.FechaPago)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Pago>> GetPagosPorEstadoAsync(string estado)
    {
        return await _context.Pagos
            .Include(p => p.Factura)
            .Include(p => p.FormaDePago)
            .Where(p => p.Estado == estado)
            .OrderByDescending(p => p.FechaPago)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Pago>> GetPagosPorRangoFechasAsync(DateTime fechaInicio, DateTime fechaFin)
    {
        return await _context.Pagos
            .Include(p => p.Factura)
            .Include(p => p.FormaDePago)
            .Where(p => p.FechaPago >= fechaInicio && p.FechaPago <= fechaFin)
            .OrderByDescending(p => p.FechaPago)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Pago>> GetAllPagosConDetallesAsync()
    {
        return await _context.Pagos
            .Include(p => p.Factura)
            .Include(p => p.FormaDePago)
            .OrderByDescending(p => p.FechaPago)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<decimal> GetTotalPagadoPorFacturaAsync(int idFactura)
    {
        return await _context.Pagos
            .Where(p => p.IdFactura == idFactura && p.Estado == "aprobado")
            .SumAsync(p => (decimal?)p.Monto) ?? 0;
    }

    /// <inheritdoc/>
    public async Task<bool> ExistePorReferenciaAsync(string referenciaTransaccion)
    {
        return await _context.Pagos
            .AnyAsync(p => p.ReferenciaTransaccion == referenciaTransaccion);
    }
}
