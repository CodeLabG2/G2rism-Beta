using G2rismBeta.API.Models;

namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interface específica para operaciones con Roles
/// Extiende el repositorio genérico y agrega métodos específicos
/// </summary>
public interface IRolRepository : IGenericRepository<Rol>
{
    /// <summary>
    /// Buscar rol por nombre
    /// </summary>
    Task<Rol?> GetByNombreAsync(string nombre);

    /// <summary>
    /// Obtener roles activos
    /// </summary>
    Task<IEnumerable<Rol>> GetRolesActivosAsync();

    /// <summary>
    /// Obtener roles por nivel de acceso
    /// </summary>
    Task<IEnumerable<Rol>> GetByNivelAccesoAsync(int nivelAcceso);

    /// <summary>
    /// Obtener rol con sus permisos
    /// </summary>
    Task<Rol?> GetRolConPermisosAsync(int idRol);

    /// <summary>
    /// Verificar si existe un rol con ese nombre
    /// </summary>
    Task<bool> ExisteNombreAsync(string nombre, int? idRolExcluir = null);

    /// <summary>
    /// Cambiar estado del rol (activar/desactivar)
    /// </summary>
    Task<bool> CambiarEstadoAsync(int idRol, bool estado);
}
