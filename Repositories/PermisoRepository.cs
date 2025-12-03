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
    /// Obtener todos los permisos con eager loading de RolesPermisos
    /// </summary>
    public override async Task<IEnumerable<Permiso>> GetAllAsync()
    {
        return await _dbSet
            .Include(p => p.RolesPermisos)
            .OrderBy(p => p.Modulo)
            .ThenBy(p => p.Accion)
            .ToListAsync();
    }

    /// <summary>
    /// Obtener un permiso por ID con eager loading de RolesPermisos
    /// </summary>
    public override async Task<Permiso?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(p => p.RolesPermisos)
            .FirstOrDefaultAsync(p => p.IdPermiso == id);
    }

    /// <summary>
    /// Buscar permiso por su nombre completo con eager loading
    /// </summary>
    public async Task<Permiso?> GetByNombrePermisoAsync(string nombrePermiso)
    {
        return await _dbSet
            .Include(p => p.RolesPermisos)
            .FirstOrDefaultAsync(p => p.NombrePermiso.ToLower() == nombrePermiso.ToLower());
    }

    /// <summary>
    /// Obtener solo permisos activos con eager loading
    /// </summary>
    public async Task<IEnumerable<Permiso>> GetPermisosActivosAsync()
    {
        return await _dbSet
            .Include(p => p.RolesPermisos)
            .Where(p => p.Estado == true)
            .OrderBy(p => p.Modulo)
            .ThenBy(p => p.Accion)
            .ToListAsync();
    }

    /// <summary>
    /// Obtener todos los permisos de un módulo específico con eager loading
    /// </summary>
    public async Task<IEnumerable<Permiso>> GetByModuloAsync(string modulo)
    {
        return await _dbSet
            .Include(p => p.RolesPermisos)
            .Where(p => p.Modulo.ToLower() == modulo.ToLower())
            .OrderBy(p => p.Accion)
            .ToListAsync();
    }

    /// <summary>
    /// Buscar un permiso por módulo y acción con eager loading
    /// </summary>
    public async Task<Permiso?> GetByModuloYAccionAsync(string modulo, string accion)
    {
        return await _dbSet
            .Include(p => p.RolesPermisos)
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
    /// Cambiar estado del permiso y persistir cambios
    /// </summary>
    public async Task<bool> CambiarEstadoAsync(int idPermiso, bool estado)
    {
        var permiso = await GetByIdAsync(idPermiso);
        if (permiso == null)
            return false;

        permiso.Estado = estado;
        permiso.FechaModificacion = DateTime.Now;

        await SaveChangesAsync();
        return true;
    }
}
