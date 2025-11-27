using G2rismBeta.API.Models;

namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interface del servicio de Autenticación
/// Gestiona registro, login, logout y recuperación de contraseña
/// </summary>
public interface IAuthService
{
    // ========================================
    // REGISTRO
    // ========================================

    /// <summary>
    /// Registrar un nuevo usuario (cliente por defecto)
    /// </summary>
    /// <param name="username">Nombre de usuario único</param>
    /// <param name="email">Correo electrónico único</param>
    /// <param name="password">Contraseña en texto plano</param>
    /// <param name="tipoUsuario">Tipo de usuario (cliente/empleado)</param>
    /// <param name="nombre">Nombre del usuario (opcional)</param>
    /// <param name="apellido">Apellido del usuario (opcional)</param>
    /// <returns>Usuario registrado con sus roles</returns>
    Task<Usuario> RegisterAsync(
        string username, 
        string email, 
        string password,
        string tipoUsuario = "cliente",
        string? nombre = null,
        string? apellido = null
    );

    // ========================================
    // AUTENTICACIÓN
    // ========================================

    /// <summary>
    /// Iniciar sesión (Login)
    /// </summary>
    /// <param name="usernameOrEmail">Username o email del usuario</param>
    /// <param name="password">Contraseña en texto plano</param>
    /// <returns>Usuario autenticado con sus roles</returns>
    Task<Usuario?> LoginAsync(string usernameOrEmail, string password);

    /// <summary>
    /// Generar tokens JWT (access token y refresh token) para un usuario
    /// </summary>
    /// <param name="usuario">Usuario autenticado</param>
    /// <param name="ipAddress">Dirección IP del cliente</param>
    /// <param name="userAgent">User Agent del navegador</param>
    /// <returns>Tupla con access token, refresh token y expiración</returns>
    Task<(string AccessToken, string RefreshToken, DateTime Expiration)> GenerarTokensAsync(
        Usuario usuario,
        string? ipAddress = null,
        string? userAgent = null);

    /// <summary>
    /// Renovar access token usando refresh token
    /// </summary>
    /// <param name="refreshToken">Refresh token válido</param>
    /// <param name="ipAddress">IP del cliente</param>
    /// <param name="userAgent">User Agent del navegador</param>
    /// <returns>Tupla con nuevo access token, nuevo refresh token y expiración</returns>
    Task<(string AccessToken, string RefreshToken, DateTime Expiration)> RefreshTokenAsync(
        string refreshToken,
        string? ipAddress = null,
        string? userAgent = null);

    /// <summary>
    /// Cerrar sesión y revocar refresh tokens
    /// </summary>
    /// <param name="idUsuario">ID del usuario</param>
    /// <param name="refreshToken">Refresh token específico a revocar (opcional)</param>
    Task LogoutAsync(int idUsuario, string? refreshToken = null);

    // ========================================
    // RECUPERACIÓN DE CONTRASEÑA
    // ========================================

    /// <summary>
    /// Solicitar recuperación de contraseña
    /// Genera un token y lo envía por email
    /// </summary>
    /// <param name="email">Email del usuario</param>
    /// <param name="frontendUrl">URL del frontend para construir el link de recuperación</param>
    /// <param name="ipSolicitud">IP desde donde se hace la solicitud (opcional)</param>
    Task<string> SolicitarRecuperacionPasswordAsync(string email, string frontendUrl, string? ipSolicitud = null);

    /// <summary>
    /// Validar un token de recuperación
    /// </summary>
    Task<bool> ValidarTokenRecuperacionAsync(string token);

    /// <summary>
    /// Restablecer contraseña usando un token
    /// </summary>
    /// <param name="token">Token de recuperación</param>
    /// <param name="nuevaPassword">Nueva contraseña</param>
    /// <param name="ipAddress">IP desde donde se realiza el cambio (opcional, para auditoría)</param>
    Task<bool> RestablecerPasswordAsync(string token, string nuevaPassword, string? ipAddress = null);

    // ========================================
    // UTILIDADES
    // ========================================

    /// <summary>
    /// Validar credenciales sin registrar el login
    /// Útil para operaciones sensibles que requieren reautenticación
    /// </summary>
    Task<bool> ValidarCredencialesAsync(string usernameOrEmail, string password);
}