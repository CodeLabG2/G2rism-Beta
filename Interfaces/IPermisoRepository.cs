using G2rismBeta.API.Models;

namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interface específica para operaciones con Permisos
/// </summary>
public interface IPermisoRepository : IGenericRepository<Permiso>
{
    /// <summary>
    /// Buscar permiso por nombre
    /// </summary>
    Task<Permiso?> GetByNombrePermisoAsync(string nombrePermiso);

    /// <summary>
    /// Obtener permisos activos
    /// </summary>
    Task<IEnumerable<Permiso>> GetPermisosActivosAsync();

    /// <summary>
    /// Obtener permisos por módulo
    /// </summary>
    Task<IEnumerable<Permiso>> GetByModuloAsync(string modulo);

    /// <summary>
    /// Obtener permisos por módulo y acción
    /// </summary>
    Task<Permiso?> GetByModuloYAccionAsync(string modulo, string accion);

    /// <summary>
    /// Verificar si existe un permiso con ese nombre
    /// </summary>
    Task<bool> ExisteNombrePermisoAsync(string nombrePermiso, int? idPermisoExcluir = null);

    /// <summary>
    /// Obtener todos los módulos únicos
    /// </summary>
    Task<IEnumerable<string>> GetModulosAsync();

    /// <summary>
    /// Cambiar estado del permiso
    /// </summary>
    Task<bool> CambiarEstadoAsync(int idPermiso, bool estado);
}
