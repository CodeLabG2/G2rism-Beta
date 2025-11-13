using G2rismBeta.API.Models;

namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interface del repositorio de Tokens de Recuperación
/// Gestiona tokens para recuperación de contraseña y verificación
/// </summary>
public interface ITokenRecuperacionRepository
{
    // ========================================
    // MÉTODOS DE CONSULTA
    // ========================================

    /// <summary>
    /// Obtener un token por su valor
    /// </summary>
    Task<TokenRecuperacion?> GetByTokenAsync(string token);

    /// <summary>
    /// Obtener un token por su ID
    /// </summary>
    Task<TokenRecuperacion?> GetByIdAsync(int idToken);

    /// <summary>
    /// Obtener todos los tokens de un usuario
    /// </summary>
    Task<IEnumerable<TokenRecuperacion>> GetByUsuarioIdAsync(int idUsuario);

    /// <summary>
    /// Obtener tokens activos (no usados y no expirados) de un usuario
    /// </summary>
    Task<IEnumerable<TokenRecuperacion>> GetTokensActivosAsync(int idUsuario);

    // ========================================
    // MÉTODOS DE VALIDACIÓN
    // ========================================

    /// <summary>
    /// Validar si un token es válido (existe, no está usado, no está expirado)
    /// </summary>
    Task<bool> ValidarTokenAsync(string token);

    /// <summary>
    /// Verificar si un token está expirado
    /// </summary>
    Task<bool> EstaExpiradoAsync(string token);

    /// <summary>
    /// Verificar si un token ya fue usado
    /// </summary>
    Task<bool> EstaUsadoAsync(string token);

    // ========================================
    // MÉTODOS DE GESTIÓN
    // ========================================

    /// <summary>
    /// Crear un nuevo token
    /// </summary>
    Task<TokenRecuperacion> CrearTokenAsync(TokenRecuperacion token);

    /// <summary>
    /// Marcar un token como usado
    /// </summary>
    Task MarcarComoUsadoAsync(string token);

    /// <summary>
    /// Invalidar todos los tokens activos de un usuario
    /// (Útil cuando el usuario cambia la contraseña)
    /// </summary>
    Task InvalidarTokensActivosAsync(int idUsuario);

    /// <summary>
    /// Eliminar tokens expirados del sistema (limpieza)
    /// </summary>
    Task LimpiarTokensExpiradosAsync();

    /// <summary>
    /// Eliminar todos los tokens de un usuario
    /// </summary>
    Task EliminarTokensDeUsuarioAsync(int idUsuario);

    // ========================================
    // MÉTODOS DE UTILIDAD
    // ========================================

    /// <summary>
    /// Guardar cambios en la base de datos
    /// </summary>
    Task<bool> SaveChangesAsync();
}