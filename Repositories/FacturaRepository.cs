using G2rismBeta.API.Data;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.Models;
using Microsoft.EntityFrameworkCore;

namespace G2rismBeta.API.Repositories;

/// <summary>
/// Repositorio de Facturas.
/// Implementa operaciones de acceso a datos específicas de facturas.
/// </summary>
public class FacturaRepository : GenericRepository<Factura>, IFacturaRepository
{
    public FacturaRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Obtener una factura con reserva y pagos incluidos
    /// </summary>
    public async Task<Factura?> GetFacturaConDetallesAsync(int id)
    {
        return await _context.Facturas
            .Include(f => f.Reserva)
                .ThenInclude(r => r!.Cliente)
            .Include(f => f.Reserva)
                .ThenInclude(r => r!.Empleado)
            .Include(f => f.Pagos)
                .ThenInclude(p => p.FormaDePago)
            .FirstOrDefaultAsync(f => f.IdFactura == id);
    }

    /// <summary>
    /// Obtener todas las facturas de una reserva específica
    /// </summary>
    public async Task<IEnumerable<Factura>> GetFacturasPorReservaAsync(int idReserva)
    {
        return await _context.Facturas
            .Include(f => f.Pagos)
            .Where(f => f.IdReserva == idReserva)
            .OrderByDescending(f => f.FechaEmision)
            .ToListAsync();
    }

    /// <summary>
    /// Obtener una factura por su número
    /// </summary>
    public async Task<Factura?> GetFacturaPorNumeroAsync(string numeroFactura)
    {
        return await _context.Facturas
            .Include(f => f.Reserva)
            .Include(f => f.Pagos)
            .FirstOrDefaultAsync(f => f.NumeroFactura == numeroFactura);
    }

    /// <summary>
    /// Obtener facturas por estado
    /// </summary>
    public async Task<IEnumerable<Factura>> GetFacturasPorEstadoAsync(string estado)
    {
        return await _context.Facturas
            .Include(f => f.Reserva)
                .ThenInclude(r => r!.Cliente)
            .Include(f => f.Pagos)
            .Where(f => f.Estado.ToLower() == estado.ToLower())
            .OrderByDescending(f => f.FechaEmision)
            .ToListAsync();
    }

    /// <summary>
    /// Obtener facturas vencidas
    /// </summary>
    public async Task<IEnumerable<Factura>> GetFacturasVencidasAsync()
    {
        var hoy = DateTime.Today;
        return await _context.Facturas
            .Include(f => f.Reserva)
                .ThenInclude(r => r!.Cliente)
            .Include(f => f.Pagos)
            .Where(f => f.FechaVencimiento.HasValue &&
                       f.FechaVencimiento.Value < hoy &&
                       f.Estado != "pagada" &&
                       f.Estado != "cancelada")
            .OrderBy(f => f.FechaVencimiento)
            .ToListAsync();
    }

    /// <summary>
    /// Obtener facturas próximas a vencer
    /// </summary>
    public async Task<IEnumerable<Factura>> GetFacturasProximasAVencerAsync(int dias = 7)
    {
        var hoy = DateTime.Today;
        var fechaLimite = hoy.AddDays(dias);

        return await _context.Facturas
            .Include(f => f.Reserva)
                .ThenInclude(r => r!.Cliente)
            .Include(f => f.Pagos)
            .Where(f => f.FechaVencimiento.HasValue &&
                       f.FechaVencimiento.Value >= hoy &&
                       f.FechaVencimiento.Value <= fechaLimite &&
                       f.Estado == "pendiente")
            .OrderBy(f => f.FechaVencimiento)
            .ToListAsync();
    }

    /// <summary>
    /// Verificar si existe una factura con un número específico
    /// </summary>
    public async Task<bool> ExistePorNumeroAsync(string numeroFactura)
    {
        return await _context.Facturas
            .AnyAsync(f => f.NumeroFactura == numeroFactura);
    }

    /// <summary>
    /// Obtener el último número de factura del año
    /// </summary>
    public async Task<string?> GetUltimoNumeroFacturaDelAnioAsync(int anio)
    {
        var prefijo = $"FAC-{anio}-";

        return await _context.Facturas
            .Where(f => f.NumeroFactura.StartsWith(prefijo))
            .OrderByDescending(f => f.NumeroFactura)
            .Select(f => f.NumeroFactura)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Verificar si ya existe una factura para una reserva
    /// </summary>
    public async Task<bool> ExisteFacturaParaReservaAsync(int idReserva)
    {
        return await _context.Facturas
            .AnyAsync(f => f.IdReserva == idReserva && f.Estado != "cancelada");
    }
}