using G2rismBeta.API.Models;

namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interface para gestión de asignaciones de permisos a roles
/// </summary>
public interface IRolPermisoRepository
{
    /// <summary>
    /// Asignar un permiso a un rol
    /// </summary>
    Task<RolPermiso> AsignarPermisoAsync(int idRol, int idPermiso);

    /// <summary>
    /// Remover un permiso de un rol
    /// </summary>
    Task<bool> RemoverPermisoAsync(int idRol, int idPermiso);

    /// <summary>
    /// Obtener todos los permisos de un rol
    /// </summary>
    Task<IEnumerable<Permiso>> GetPermisosPorRolAsync(int idRol);

    /// <summary>
    /// Obtener todos los roles que tienen un permiso
    /// </summary>
    Task<IEnumerable<Rol>> GetRolesPorPermisoAsync(int idPermiso);

    /// <summary>
    /// Verificar si un rol tiene un permiso específico
    /// </summary>
    Task<bool> RolTienePermisoAsync(int idRol, int idPermiso);

    /// <summary>
    /// Asignar múltiples permisos a un rol (reemplaza los existentes)
    /// </summary>
    Task<bool> AsignarPermisosMultiplesAsync(int idRol, List<int> idsPermisos);

    /// <summary>
    /// Remover todos los permisos de un rol
    /// </summary>
    Task<bool> RemoverTodosLosPermisosAsync(int idRol);

    /// <summary>
    /// Guardar cambios
    /// </summary>
    Task<bool> SaveChangesAsync();
}
