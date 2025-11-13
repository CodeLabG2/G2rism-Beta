using Microsoft.EntityFrameworkCore;
using G2rismBeta.API.Data;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.Models;

namespace G2rismBeta.API.Repositories;

/// <summary>
/// Repositorio específico para operaciones con Roles
/// </summary>
public class RolRepository : GenericRepository<Rol>, IRolRepository
{
    public RolRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Buscar rol por nombre (case insensitive)
    /// </summary>
    public async Task<Rol?> GetByNombreAsync(string nombre)
    {
        return await _dbSet
            .FirstOrDefaultAsync(r => r.Nombre.ToLower() == nombre.ToLower());
    }

    /// <summary>
    /// Obtener solo los roles activos
    /// </summary>
    public async Task<IEnumerable<Rol>> GetRolesActivosAsync()
    {
        return await _dbSet
            .Where(r => r.Estado == true)
            .OrderBy(r => r.NivelAcceso)
            .ToListAsync();
    }

    /// <summary>
    /// Obtener roles por nivel de acceso específico
    /// </summary>
    public async Task<IEnumerable<Rol>> GetByNivelAccesoAsync(int nivelAcceso)
    {
        return await _dbSet
            .Where(r => r.NivelAcceso == nivelAcceso)
            .ToListAsync();
    }

    /// <summary>
    /// Obtener un rol con todos sus permisos incluidos
    /// </summary>
    public async Task<Rol?> GetRolConPermisosAsync(int idRol)
    {
        return await _dbSet
            .Include(r => r.RolesPermisos)
                .ThenInclude(rp => rp.Permiso)
            .FirstOrDefaultAsync(r => r.IdRol == idRol);
    }

    /// <summary>
    /// Verificar si ya existe un rol con ese nombre
    /// Útil para evitar duplicados al crear o actualizar
    /// </summary>
    public async Task<bool> ExisteNombreAsync(string nombre, int? idRolExcluir = null)
    {
        var query = _dbSet.Where(r => r.Nombre.ToLower() == nombre.ToLower());

        // Si estamos actualizando, excluir el rol actual de la búsqueda
        if (idRolExcluir.HasValue)
        {
            query = query.Where(r => r.IdRol != idRolExcluir.Value);
        }

        return await query.AnyAsync();
    }

    /// <summary>
    /// Cambiar el estado de un rol (activar/desactivar)
    /// </summary>
    public async Task<bool> CambiarEstadoAsync(int idRol, bool estado)
    {
        var rol = await GetByIdAsync(idRol);
        if (rol == null)
            return false;

        rol.Estado = estado;
        rol.FechaModificacion = DateTime.Now;

        return true; // Los cambios se guardan con SaveChangesAsync()
    }
}
