using G2rismBeta.API.Models;

namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interface del repositorio de la relación Usuarios-Roles
/// Gestiona la asignación de roles a usuarios
/// </summary>
public interface IUsuarioRolRepository
{
    // ========================================
    // MÉTODOS DE CONSULTA
    // ========================================

    /// <summary>
    /// Obtener todos los roles de un usuario
    /// </summary>
    Task<IEnumerable<UsuarioRol>> GetRolesByUsuarioIdAsync(int idUsuario);

    /// <summary>
    /// Obtener todos los usuarios que tienen un rol específico
    /// </summary>
    Task<IEnumerable<UsuarioRol>> GetUsuariosByRolIdAsync(int idRol);

    /// <summary>
    /// Verificar si un usuario tiene un rol específico
    /// </summary>
    Task<bool> UsuarioTieneRolAsync(int idUsuario, int idRol);

    /// <summary>
    /// Obtener una relación específica usuario-rol
    /// </summary>
    Task<UsuarioRol?> GetByUsuarioAndRolAsync(int idUsuario, int idRol);

    // ========================================
    // MÉTODOS DE ASIGNACIÓN
    // ========================================

    /// <summary>
    /// Asignar un rol a un usuario
    /// </summary>
    Task<UsuarioRol> AsignarRolAsync(UsuarioRol usuarioRol);

    /// <summary>
    /// Asignar múltiples roles a un usuario de una vez
    /// </summary>
    Task AsignarRolesMultiplesAsync(int idUsuario, List<int> idsRoles, int? asignadoPor = null);

    /// <summary>
    /// Remover un rol de un usuario
    /// </summary>
    Task<bool> RemoverRolAsync(int idUsuario, int idRol);

    /// <summary>
    /// Remover todos los roles de un usuario
    /// </summary>
    Task RemoverTodosLosRolesAsync(int idUsuario);

    /// <summary>
    /// Reemplazar todos los roles de un usuario
    /// (Elimina los actuales y asigna los nuevos)
    /// </summary>
    Task ReemplazarRolesAsync(int idUsuario, List<int> nuevosIdsRoles, int? asignadoPor = null);

    // ========================================
    // MÉTODOS DE UTILIDAD
    // ========================================

    /// <summary>
    /// Guardar cambios en la base de datos
    /// </summary>
    Task<bool> SaveChangesAsync();
}