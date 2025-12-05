using G2rismBeta.API.Data;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.Models;
using Microsoft.EntityFrameworkCore;

namespace G2rismBeta.API.Repositories;

/// <summary>
/// Implementación del repositorio de hoteles con búsquedas avanzadas
/// </summary>
public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
{
    public HotelRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <inheritdoc/>
    public async Task<Hotel?> GetByIdConProveedorAsync(int id)
    {
        return await _context.Hoteles
            .Include(h => h.Proveedor)
            .FirstOrDefaultAsync(h => h.IdHotel == id);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Hotel>> GetAllConProveedorAsync()
    {
        return await _context.Hoteles
            .Include(h => h.Proveedor)
            .OrderBy(h => h.Ciudad)
            .ThenBy(h => h.Nombre)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Hotel>> GetByCiudadAsync(string ciudad)
    {
        return await _context.Hoteles
            .Include(h => h.Proveedor)
            .Where(h => h.Ciudad.ToLower() == ciudad.ToLower())
            .OrderBy(h => h.Nombre)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Hotel>> GetByPaisAsync(string pais)
    {
        return await _context.Hoteles
            .Include(h => h.Proveedor)
            .Where(h => h.Pais != null && h.Pais.ToLower() == pais.ToLower())
            .OrderBy(h => h.Ciudad)
            .ThenBy(h => h.Nombre)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Hotel>> GetByEstrellasAsync(int estrellas)
    {
        return await _context.Hoteles
            .Include(h => h.Proveedor)
            .Where(h => h.Estrellas == estrellas)
            .OrderBy(h => h.Ciudad)
            .ThenBy(h => h.Nombre)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Hotel>> GetByCategoriaAsync(string categoria)
    {
        return await _context.Hoteles
            .Include(h => h.Proveedor)
            .Where(h => h.Categoria != null && h.Categoria.ToLower() == categoria.ToLower())
            .OrderBy(h => h.Ciudad)
            .ThenBy(h => h.Nombre)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Hotel>> GetActivosAsync()
    {
        return await _context.Hoteles
            .Include(h => h.Proveedor)
            .Where(h => h.Estado == true)
            .OrderBy(h => h.Ciudad)
            .ThenBy(h => h.Nombre)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Hotel>> GetByRangoPrecioAsync(decimal precioMin, decimal precioMax)
    {
        return await _context.Hoteles
            .Include(h => h.Proveedor)
            .Where(h => h.PrecioPorNoche >= precioMin && h.PrecioPorNoche <= precioMax)
            .OrderBy(h => h.PrecioPorNoche)
            .ThenBy(h => h.Nombre)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Hotel>> GetByServiciosAsync(
        bool? wifi = null,
        bool? piscina = null,
        bool? restaurante = null,
        bool? gimnasio = null,
        bool? parqueadero = null)
    {
        var query = _context.Hoteles
            .Include(h => h.Proveedor)
            .AsQueryable();

        if (wifi.HasValue)
            query = query.Where(h => h.TieneWifi == wifi.Value);

        if (piscina.HasValue)
            query = query.Where(h => h.TienePiscina == piscina.Value);

        if (restaurante.HasValue)
            query = query.Where(h => h.TieneRestaurante == restaurante.Value);

        if (gimnasio.HasValue)
            query = query.Where(h => h.TieneGimnasio == gimnasio.Value);

        if (parqueadero.HasValue)
            query = query.Where(h => h.TieneParqueadero == parqueadero.Value);

        return await query
            .OrderBy(h => h.Ciudad)
            .ThenBy(h => h.Nombre)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Hotel>> GetPremiumAsync()
    {
        return await _context.Hoteles
            .Include(h => h.Proveedor)
            .Where(h => h.TienePiscina || h.TieneGimnasio || h.TieneRestaurante)
            .OrderByDescending(h => h.Estrellas ?? 0)
            .ThenBy(h => h.Nombre)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<bool> ExistePorNombreYCiudadAsync(string nombre, string ciudad, int? idHotelExcluir = null)
    {
        var query = _context.Hoteles
            .Where(h => h.Nombre.ToLower() == nombre.ToLower()
                     && h.Ciudad.ToLower() == ciudad.ToLower());

        if (idHotelExcluir.HasValue)
            query = query.Where(h => h.IdHotel != idHotelExcluir.Value);

        return await query.AnyAsync();
    }
}
