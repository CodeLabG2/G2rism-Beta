using G2rismBeta.API.Models;

namespace G2rismBeta.API.Interfaces
{
    /// <summary>
    /// Interfaz para el repositorio de RefreshTokens
    /// Proporciona métodos específicos para gestión de tokens de actualización
    /// </summary>
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        /// <summary>
        /// Obtiene un refresh token activo por su valor
        /// </summary>
        /// <param name="token">Valor del refresh token</param>
        /// <returns>RefreshToken si existe y está activo, null en caso contrario</returns>
        Task<RefreshToken?> GetActiveTokenAsync(string token);

        /// <summary>
        /// Obtiene todos los refresh tokens activos de un usuario
        /// </summary>
        /// <param name="idUsuario">ID del usuario</param>
        /// <returns>Lista de refresh tokens activos</returns>
        Task<IEnumerable<RefreshToken>> GetActiveTokensByUserAsync(int idUsuario);

        /// <summary>
        /// Revoca un refresh token específico
        /// </summary>
        /// <param name="token">Valor del token a revocar</param>
        /// <returns>True si se revocó exitosamente, false si no se encontró</returns>
        Task<bool> RevokeTokenAsync(string token);

        /// <summary>
        /// Revoca todos los refresh tokens de un usuario
        /// </summary>
        /// <param name="idUsuario">ID del usuario</param>
        /// <returns>Cantidad de tokens revocados</returns>
        Task<int> RevokeAllUserTokensAsync(int idUsuario);

        /// <summary>
        /// Elimina refresh tokens expirados de la base de datos (limpieza)
        /// </summary>
        /// <returns>Cantidad de tokens eliminados</returns>
        Task<int> DeleteExpiredTokensAsync();

        /// <summary>
        /// Verifica si un token existe y está activo
        /// </summary>
        /// <param name="token">Valor del token</param>
        /// <returns>True si el token existe y está activo</returns>
        Task<bool> IsTokenActiveAsync(string token);
    }
}
