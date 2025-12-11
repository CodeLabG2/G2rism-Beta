using G2rismBeta.API.DTOs.Rol;
using G2rismBeta.API.DTOs.RolPermiso;

namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interface del servicio de Roles
/// Define la lógica de negocio para gestión de roles
/// </summary>
public interface IRolService
{
    // ========================================
    // OPERACIONES CRUD BÁSICAS
    // ========================================

    /// <summary>
    /// Obtener todos los roles
    /// </summary>
    /// <returns>Lista de roles en formato DTO</returns>
    Task<IEnumerable<RolResponseDto>> GetAllRolesAsync();

    /// <summary>
    /// Obtener solo roles activos
    /// </summary>
    Task<IEnumerable<RolResponseDto>> GetRolesActivosAsync();

    /// <summary>
    /// Obtener un rol por su ID
    /// </summary>
    /// <param name="idRol">ID del rol a buscar</param>
    /// <returns>Rol encontrado o null</returns>
    Task<RolResponseDto?> GetRolByIdAsync(int idRol);

    /// <summary>
    /// Obtener un rol con todos sus permisos
    /// </summary>
    Task<RolConPermisosDto?> GetRolConPermisosAsync(int idRol);

    /// <summary>
    /// Crear un nuevo rol
    /// </summary>
    /// <param name="rolCreateDto">Datos del rol a crear</param>
    /// <returns>Rol creado</returns>
    Task<RolResponseDto> CreateRolAsync(RolCreateDto rolCreateDto);

    /// <summary>
    /// Actualizar un rol existente
    /// </summary>
    /// <param name="idRol">ID del rol a actualizar</param>
    /// <param name="rolUpdateDto">Nuevos datos del rol</param>
    /// <returns>Rol actualizado</returns>
    Task<RolResponseDto> UpdateRolAsync(int idRol, RolUpdateDto rolUpdateDto);

    /// <summary>
    /// Eliminar un rol (si no tiene usuarios asignados)
    /// </summary>
    /// <param name="idRol">ID del rol a eliminar</param>
    /// <returns>True si se eliminó correctamente</returns>
    Task<bool> DeleteRolAsync(int idRol);

    // ========================================
    // OPERACIONES ESPECIALES
    // ========================================

    /// <summary>
    /// Cambiar el estado de un rol (activar/desactivar)
    /// </summary>
    Task<bool> CambiarEstadoRolAsync(int idRol, bool nuevoEstado);

    /// <summary>
    /// Asignar permisos a un rol (solo agrega nuevos, no elimina existentes)
    /// Retorna la cantidad de permisos nuevos agregados
    /// </summary>
    Task<int> AsignarPermisosAsync(int idRol, List<int> idsPermisos);

    /// <summary>
    /// Remover un permiso específico de un rol
    /// </summary>
    Task<bool> RemoverPermisoAsync(int idRol, int idPermiso);

    // ========================================
    // VALIDACIONES
    // ========================================

    /// <summary>
    /// Validar si un nombre de rol ya existe
    /// </summary>
    Task<bool> NombreRolExisteAsync(string nombre, int? idRolExcluir = null);
}
