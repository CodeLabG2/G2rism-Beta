using Microsoft.EntityFrameworkCore;
using G2rismBeta.API.Data;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.Models;

namespace G2rismBeta.API.Repositories;

    /// <summary>
    /// Repositorio para gestionar asignaciones de permisos a roles
    /// </summary>
    public class RolPermisoRepository : IRolPermisoRepository
{
    private readonly ApplicationDbContext _context;

    public RolPermisoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Asignar un permiso a un rol
    /// </summary>
    public async Task<RolPermiso> AsignarPermisoAsync(int idRol, int idPermiso)
    {
        // Verificar si ya existe la asignación
        var existe = await RolTienePermisoAsync(idRol, idPermiso);
        if (existe)
        {
            throw new InvalidOperationException("El permiso ya está asignado a este rol");
        }

        var rolPermiso = new RolPermiso
        {
            IdRol = idRol,
            IdPermiso = idPermiso,
            FechaAsignacion = DateTime.Now
        };

        await _context.RolesPermisos.AddAsync(rolPermiso);
        return rolPermiso;
    }

    /// <summary>
    /// Remover un permiso de un rol
    /// </summary>
    public async Task<bool> RemoverPermisoAsync(int idRol, int idPermiso)
    {
        var rolPermiso = await _context.RolesPermisos
            .FirstOrDefaultAsync(rp => rp.IdRol == idRol && rp.IdPermiso == idPermiso);

        if (rolPermiso == null)
            return false;

        _context.RolesPermisos.Remove(rolPermiso);
        return true;
    }

    /// <summary>
    /// Obtener todos los permisos asignados a un rol
    /// </summary>
    public async Task<IEnumerable<Permiso>> GetPermisosPorRolAsync(int idRol)
    {
        return await _context.RolesPermisos
            .Where(rp => rp.IdRol == idRol)
            .Include(rp => rp.Permiso)
            .Select(rp => rp.Permiso)
            .OrderBy(p => p.Modulo)
            .ThenBy(p => p.Accion)
            .ToListAsync();
    }

    /// <summary>
    /// Obtener todos los roles que tienen un permiso específico
    /// </summary>
    public async Task<IEnumerable<Rol>> GetRolesPorPermisoAsync(int idPermiso)
    {
        return await _context.RolesPermisos
            .Where(rp => rp.IdPermiso == idPermiso)
            .Include(rp => rp.Rol)
            .Select(rp => rp.Rol)
            .OrderBy(r => r.NivelAcceso)
            .ToListAsync();
    }

    /// <summary>
    /// Verificar si un rol tiene asignado un permiso específico
    /// </summary>
    public async Task<bool> RolTienePermisoAsync(int idRol, int idPermiso)
    {
        return await _context.RolesPermisos
            .AnyAsync(rp => rp.IdRol == idRol && rp.IdPermiso == idPermiso);
    }

    /// <summary>
    /// Asignar múltiples permisos a un rol (solo agrega permisos nuevos, no elimina los existentes)
    /// Retorna la cantidad de permisos nuevos agregados
    /// </summary>
    public async Task<int> AsignarPermisosMultiplesAsync(int idRol, List<int> idsPermisos)
    {
        // Obtener los permisos que ya están asignados
        var permisosExistentes = await _context.RolesPermisos
            .Where(rp => rp.IdRol == idRol)
            .Select(rp => rp.IdPermiso)
            .ToListAsync();

        // Filtrar solo los permisos que NO están asignados (para evitar duplicados)
        var permisosNuevos = idsPermisos
            .Where(idPermiso => !permisosExistentes.Contains(idPermiso))
            .ToList();

        // Si no hay permisos nuevos para agregar, retornar 0
        if (!permisosNuevos.Any())
        {
            return 0;
        }

        // Agregar solo los permisos nuevos
        var nuevasAsignaciones = permisosNuevos.Select(idPermiso => new RolPermiso
        {
            IdRol = idRol,
            IdPermiso = idPermiso,
            FechaAsignacion = DateTime.Now
        }).ToList();

        await _context.RolesPermisos.AddRangeAsync(nuevasAsignaciones);

        // Retornar la cantidad de permisos agregados
        return permisosNuevos.Count;
    }

    /// <summary>
    /// Remover todos los permisos de un rol
    /// </summary>
    public async Task<bool> RemoverTodosLosPermisosAsync(int idRol)
    {
        var permisos = await _context.RolesPermisos
            .Where(rp => rp.IdRol == idRol)
            .ToListAsync();

        if (permisos.Any())
        {
            _context.RolesPermisos.RemoveRange(permisos);
        }

        return true;
    }

    /// <summary>
    /// Guardar cambios en la base de datos
    /// </summary>
    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
