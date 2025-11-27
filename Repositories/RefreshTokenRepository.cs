using G2rismBeta.API.Data;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.Models;
using Microsoft.EntityFrameworkCore;

namespace G2rismBeta.API.Repositories
{
    /// <summary>
    /// Repositorio para gestión de RefreshTokens en la base de datos
    /// </summary>
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Obtiene un refresh token activo por su valor
        /// </summary>
        public async Task<RefreshToken?> GetActiveTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .Include(rt => rt.Usuario)
                .FirstOrDefaultAsync(rt =>
                    rt.Token == token &&
                    !rt.Revocado &&
                    rt.FechaExpiracion > DateTime.UtcNow);
        }

        /// <summary>
        /// Obtiene todos los refresh tokens activos de un usuario
        /// </summary>
        public async Task<IEnumerable<RefreshToken>> GetActiveTokensByUserAsync(int idUsuario)
        {
            return await _context.RefreshTokens
                .Where(rt =>
                    rt.IdUsuario == idUsuario &&
                    !rt.Revocado &&
                    rt.FechaExpiracion > DateTime.UtcNow)
                .OrderByDescending(rt => rt.FechaCreacion)
                .ToListAsync();
        }

        /// <summary>
        /// Revoca un refresh token específico
        /// </summary>
        public async Task<bool> RevokeTokenAsync(string token)
        {
            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == token);

            if (refreshToken == null)
                return false;

            refreshToken.Revocado = true;
            refreshToken.FechaRevocacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Revoca todos los refresh tokens de un usuario
        /// </summary>
        public async Task<int> RevokeAllUserTokensAsync(int idUsuario)
        {
            var tokens = await _context.RefreshTokens
                .Where(rt => rt.IdUsuario == idUsuario && !rt.Revocado)
                .ToListAsync();

            foreach (var token in tokens)
            {
                token.Revocado = true;
                token.FechaRevocacion = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return tokens.Count;
        }

        /// <summary>
        /// Elimina refresh tokens expirados de la base de datos (limpieza)
        /// </summary>
        public async Task<int> DeleteExpiredTokensAsync()
        {
            var expiredTokens = await _context.RefreshTokens
                .Where(rt => rt.FechaExpiracion < DateTime.UtcNow)
                .ToListAsync();

            _context.RefreshTokens.RemoveRange(expiredTokens);
            await _context.SaveChangesAsync();

            return expiredTokens.Count;
        }

        /// <summary>
        /// Verifica si un token existe y está activo
        /// </summary>
        public async Task<bool> IsTokenActiveAsync(string token)
        {
            return await _context.RefreshTokens
                .AnyAsync(rt =>
                    rt.Token == token &&
                    !rt.Revocado &&
                    rt.FechaExpiracion > DateTime.UtcNow);
        }
    }
}
