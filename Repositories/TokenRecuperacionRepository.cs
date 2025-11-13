using Microsoft.EntityFrameworkCore;
using G2rismBeta.API.Data;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.Models;

namespace G2rismBeta.API.Repositories;

/// <summary>
/// Implementación del repositorio de Tokens de Recuperación
/// </summary>
public class TokenRecuperacionRepository : ITokenRecuperacionRepository
{
    private readonly ApplicationDbContext _context;

    public TokenRecuperacionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // ========================================
    // MÉTODOS DE CONSULTA
    // ========================================

    /// <summary>
    /// Obtener un token por su valor
    /// </summary>
    public async Task<TokenRecuperacion?> GetByTokenAsync(string token)
    {
        return await _context.TokensRecuperacion
            .Include(t => t.Usuario)
            .FirstOrDefaultAsync(t => t.Token == token);
    }

    /// <summary>
    /// Obtener un token por su ID
    /// </summary>
    public async Task<TokenRecuperacion?> GetByIdAsync(int idToken)
    {
        return await _context.TokensRecuperacion
            .Include(t => t.Usuario)
            .FirstOrDefaultAsync(t => t.IdToken == idToken);
    }

    /// <summary>
    /// Obtener todos los tokens de un usuario
    /// </summary>
    public async Task<IEnumerable<TokenRecuperacion>> GetByUsuarioIdAsync(int idUsuario)
    {
        return await _context.TokensRecuperacion
            .Where(t => t.IdUsuario == idUsuario)
            .OrderByDescending(t => t.FechaGeneracion)
            .ToListAsync();
    }

    /// <summary>
    /// Obtener tokens activos de un usuario
    /// </summary>
    public async Task<IEnumerable<TokenRecuperacion>> GetTokensActivosAsync(int idUsuario)
    {
        var ahora = DateTime.Now;

        return await _context.TokensRecuperacion
            .Where(t => t.IdUsuario == idUsuario &&
                       !t.Usado &&
                       t.FechaExpiracion > ahora)
            .OrderByDescending(t => t.FechaGeneracion)
            .ToListAsync();
    }

    // ========================================
    // MÉTODOS DE VALIDACIÓN
    // ========================================

    /// <summary>
    /// Validar si un token es válido
    /// </summary>
    public async Task<bool> ValidarTokenAsync(string token)
    {
        var ahora = DateTime.Now;

        return await _context.TokensRecuperacion
            .AnyAsync(t => t.Token == token &&
                          !t.Usado &&
                          t.FechaExpiracion > ahora);
    }

    /// <summary>
    /// Verificar si un token está expirado
    /// </summary>
    public async Task<bool> EstaExpiradoAsync(string token)
    {
        var tokenObj = await _context.TokensRecuperacion
            .FirstOrDefaultAsync(t => t.Token == token);

        if (tokenObj == null)
            return true; // Si no existe, considerarlo expirado

        return tokenObj.FechaExpiracion <= DateTime.Now;
    }

    /// <summary>
    /// Verificar si un token ya fue usado
    /// </summary>
    public async Task<bool> EstaUsadoAsync(string token)
    {
        var tokenObj = await _context.TokensRecuperacion
            .FirstOrDefaultAsync(t => t.Token == token);

        return tokenObj?.Usado ?? false;
    }

    // ========================================
    // MÉTODOS DE GESTIÓN
    // ========================================

    /// <summary>
    /// Crear un nuevo token
    /// </summary>
    public async Task<TokenRecuperacion> CrearTokenAsync(TokenRecuperacion token)
    {
        await _context.TokensRecuperacion.AddAsync(token);
        await _context.SaveChangesAsync();
        return token;
    }

    /// <summary>
    /// Marcar un token como usado
    /// </summary>
    public async Task MarcarComoUsadoAsync(string token)
    {
        var tokenObj = await _context.TokensRecuperacion
            .FirstOrDefaultAsync(t => t.Token == token);

        if (tokenObj != null)
        {
            tokenObj.Usado = true;
            tokenObj.FechaUso = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Invalidar todos los tokens activos de un usuario
    /// </summary>
    public async Task InvalidarTokensActivosAsync(int idUsuario)
    {
        var tokensActivos = await GetTokensActivosAsync(idUsuario);

        foreach (var token in tokensActivos)
        {
            token.Usado = true;
            token.FechaUso = DateTime.Now;
        }

        if (tokensActivos.Any())
        {
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Limpiar tokens expirados (tarea de mantenimiento)
    /// </summary>
    public async Task LimpiarTokensExpiradosAsync()
    {
        var ahora = DateTime.Now;

        var tokensExpirados = await _context.TokensRecuperacion
            .Where(t => t.FechaExpiracion < ahora)
            .ToListAsync();

        if (tokensExpirados.Any())
        {
            _context.TokensRecuperacion.RemoveRange(tokensExpirados);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Eliminar todos los tokens de un usuario
    /// </summary>
    public async Task EliminarTokensDeUsuarioAsync(int idUsuario)
    {
        var tokens = await _context.TokensRecuperacion
            .Where(t => t.IdUsuario == idUsuario)
            .ToListAsync();

        if (tokens.Any())
        {
            _context.TokensRecuperacion.RemoveRange(tokens);
            await _context.SaveChangesAsync();
        }
    }

    // ========================================
    // MÉTODOS DE UTILIDAD
    // ========================================

    /// <summary>
    /// Guardar cambios en la base de datos
    /// </summary>
    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}