using G2rismBeta.API.Models;

namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interface del repositorio de Usuarios
/// Define operaciones específicas para la gestión de usuarios
/// </summary>
public interface IUsuarioRepository : IGenericRepository<Usuario>
{
    // ========================================
    // MÉTODOS DE CONSULTA
    // ========================================

    /// <summary>
    /// Obtener un usuario por su username
    /// </summary>
    Task<Usuario?> GetByUsernameAsync(string username);

    /// <summary>
    /// Obtener un usuario por su email
    /// </summary>
    Task<Usuario?> GetByEmailAsync(string email);

    /// <summary>
    /// Obtener un usuario por username o email
    /// Útil para el login donde el usuario puede ingresar cualquiera de los dos
    /// </summary>
    Task<Usuario?> GetByUsernameOrEmailAsync(string usernameOrEmail);

    /// <summary>
    /// Obtener un usuario con sus roles asignados (incluye navegación)
    /// </summary>
    Task<Usuario?> GetByIdWithRolesAsync(int idUsuario);

    /// <summary>
    /// Obtener todos los usuarios con sus roles
    /// </summary>
    Task<IEnumerable<Usuario>> GetAllWithRolesAsync();

    /// <summary>
    /// Obtener usuarios activos
    /// </summary>
    Task<IEnumerable<Usuario>> GetUsuariosActivosAsync();

    /// <summary>
    /// Obtener usuarios por tipo (cliente/empleado)
    /// </summary>
    Task<IEnumerable<Usuario>> GetUsuariosByTipoAsync(string tipo);

    /// <summary>
    /// Obtener usuarios bloqueados
    /// </summary>
    Task<IEnumerable<Usuario>> GetUsuariosBloqueadosAsync();

    // ========================================
    // MÉTODOS DE VALIDACIÓN
    // ========================================

    /// <summary>
    /// Verificar si existe un username (para validación de duplicados)
    /// </summary>
    Task<bool> ExistsByUsernameAsync(string username);

    /// <summary>
    /// Verificar si existe un email (para validación de duplicados)
    /// </summary>
    Task<bool> ExistsByEmailAsync(string email);

    /// <summary>
    /// Verificar si existe un username excluyendo un ID específico
    /// Útil para la actualización (no validar contra sí mismo)
    /// </summary>
    Task<bool> ExistsByUsernameExceptIdAsync(string username, int idUsuario);

    /// <summary>
    /// Verificar si existe un email excluyendo un ID específico
    /// </summary>
    Task<bool> ExistsByEmailExceptIdAsync(string email, int idUsuario);

    // ========================================
    // MÉTODOS DE SEGURIDAD
    // ========================================

    /// <summary>
    /// Actualizar el último acceso del usuario
    /// Se llama cuando el usuario hace login exitosamente
    /// </summary>
    Task UpdateUltimoAccesoAsync(int idUsuario);

    /// <summary>
    /// Incrementar el contador de intentos fallidos
    /// Se llama cuando el login falla
    /// </summary>
    Task IncrementarIntentosFallidosAsync(int idUsuario);

    /// <summary>
    /// Reiniciar los intentos fallidos a 0
    /// Se llama cuando el login es exitoso
    /// </summary>
    Task ReiniciarIntentosFallidosAsync(int idUsuario);

    /// <summary>
    /// Bloquear un usuario
    /// Se llama automáticamente cuando excede los intentos permitidos
    /// </summary>
    Task BloquearUsuarioAsync(int idUsuario);

    /// <summary>
    /// Desbloquear un usuario
    /// Solo puede hacerlo un administrador
    /// </summary>
    Task DesbloquearUsuarioAsync(int idUsuario);
}