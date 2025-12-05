using G2rismBeta.API.Data;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.Models;
using Microsoft.EntityFrameworkCore;

namespace G2rismBeta.API.Repositories;

/// <summary>
/// Implementación del repositorio de servicios adicionales con búsquedas avanzadas
/// </summary>
public class ServicioAdicionalRepository : GenericRepository<ServicioAdicional>, IServicioAdicionalRepository
{
    public ServicioAdicionalRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <inheritdoc/>
    public async Task<ServicioAdicional?> GetByIdConProveedorAsync(int id)
    {
        return await _context.ServiciosAdicionales
            .Include(s => s.Proveedor)
            .FirstOrDefaultAsync(s => s.IdServicio == id);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ServicioAdicional>> GetAllConProveedorAsync()
    {
        return await _context.ServiciosAdicionales
            .Include(s => s.Proveedor)
            .OrderBy(s => s.Tipo)
            .ThenBy(s => s.Nombre)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ServicioAdicional>> GetByTipoAsync(string tipo)
    {
        return await _context.ServiciosAdicionales
            .Include(s => s.Proveedor)
            .Where(s => s.Tipo.ToLower() == tipo.ToLower())
            .OrderBy(s => s.Nombre)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ServicioAdicional>> GetDisponiblesAsync()
    {
        return await _context.ServiciosAdicionales
            .Include(s => s.Proveedor)
            .Where(s => s.Estado && s.Disponibilidad)
            .OrderBy(s => s.Tipo)
            .ThenBy(s => s.Nombre)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ServicioAdicional>> GetActivosAsync()
    {
        return await _context.ServiciosAdicionales
            .Include(s => s.Proveedor)
            .Where(s => s.Estado)
            .OrderBy(s => s.Tipo)
            .ThenBy(s => s.Nombre)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ServicioAdicional>> GetByRangoPrecioAsync(decimal precioMin, decimal precioMax)
    {
        return await _context.ServiciosAdicionales
            .Include(s => s.Proveedor)
            .Where(s => s.Precio >= precioMin && s.Precio <= precioMax)
            .OrderBy(s => s.Precio)
            .ThenBy(s => s.Nombre)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ServicioAdicional>> GetByUnidadAsync(string unidad)
    {
        return await _context.ServiciosAdicionales
            .Include(s => s.Proveedor)
            .Where(s => s.Unidad.ToLower() == unidad.ToLower())
            .OrderBy(s => s.Tipo)
            .ThenBy(s => s.Nombre)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ServicioAdicional>> GetByDuracionMaximaAsync(int duracionMaxima)
    {
        return await _context.ServiciosAdicionales
            .Include(s => s.Proveedor)
            .Where(s => s.TiempoEstimado.HasValue && s.TiempoEstimado.Value <= duracionMaxima)
            .OrderBy(s => s.TiempoEstimado)
            .ThenBy(s => s.Nombre)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ServicioAdicional>> GetByProveedorAsync(int idProveedor)
    {
        return await _context.ServiciosAdicionales
            .Include(s => s.Proveedor)
            .Where(s => s.IdProveedor == idProveedor)
            .OrderBy(s => s.Tipo)
            .ThenBy(s => s.Nombre)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<bool> ExistePorNombreYProveedorAsync(string nombre, int idProveedor, int? idServicioExcluir = null)
    {
        var query = _context.ServiciosAdicionales
            .Where(s => s.Nombre.ToLower() == nombre.ToLower() && s.IdProveedor == idProveedor);

        if (idServicioExcluir.HasValue)
        {
            query = query.Where(s => s.IdServicio != idServicioExcluir.Value);
        }

        return await query.AnyAsync();
    }
}
