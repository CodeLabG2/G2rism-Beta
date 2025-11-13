using Microsoft.EntityFrameworkCore;
using G2rismBeta.API.Data;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.Models;

namespace G2rismBeta.API.Repositories;

/// <summary>
/// Repositorio específico para operaciones con Permisos
/// </summary>
public class PermisoRepository : GenericRepository<Permiso>, IPermisoRepository
{
    public PermisoRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Buscar permiso por su nombre completo
    /// </summary>
    public async Task<Permiso?> GetByNombrePermisoAsync(string nombrePermiso)
    {
        return await _dbSet
            .FirstOrDefaultAsync(p => p.NombrePermiso.ToLower() == nombrePermiso.ToLower());
    }

    /// <summary>
    /// Obtener solo permisos activos
    /// </summary>
    public async Task<IEnumerable<Permiso>> GetPermisosActivosAsync()
    {
        return await _dbSet
            .Where(p => p.Estado == true)
            .OrderBy(p => p.Modulo)
            .ThenBy(p => p.Accion)
            .ToListAsync();
    }

    /// <summary>
    /// Obtener todos los permisos de un módulo específico
    /// </summary>
    public async Task<IEnumerable<Permiso>> GetByModuloAsync(string modulo)
    {
        return await _dbSet
            .Where(p => p.Modulo.ToLower() == modulo.ToLower())
            .OrderBy(p => p.Accion)
            .ToListAsync();
    }

    /// <summary>
    /// Buscar un permiso por módulo y acción
    /// </summary>
    public async Task<Permiso?> GetByModuloYAccionAsync(string modulo, string accion)
    {
        return await _dbSet
            .FirstOrDefaultAsync(p =>
                p.Modulo.ToLower() == modulo.ToLower() &&
                p.Accion.ToLower() == accion.ToLower());
    }

    /// <summary>
    /// Verificar si ya existe un permiso con ese nombre
    /// </summary>
    public async Task<bool> ExisteNombrePermisoAsync(string nombrePermiso, int? idPermisoExcluir = null)
    {
        var query = _dbSet.Where(p => p.NombrePermiso.ToLower() == nombrePermiso.ToLower());

        if (idPermisoExcluir.HasValue)
        {
            query = query.Where(p => p.IdPermiso != idPermisoExcluir.Value);
        }

        return await query.AnyAsync();
    }

    /// <summary>
    /// Obtener lista de módulos únicos (sin duplicados)
    /// </summary>
    public async Task<IEnumerable<string>> GetModulosAsync()
    {
        return await _dbSet
            .Select(p => p.Modulo)
            .Distinct()
            .OrderBy(m => m)
            .ToListAsync();
    }

    /// <summary>
    /// Cambiar estado del permiso
    /// </summary>
    public async Task<bool> CambiarEstadoAsync(int idPermiso, bool estado)
    {
        var permiso = await GetByIdAsync(idPermiso);
        if (permiso == null)
            return false;

        permiso.Estado = estado;

        return true;
    }
}
