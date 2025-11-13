using Microsoft.EntityFrameworkCore;
using G2rismBeta.API.Data;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.Models;

namespace G2rismBeta.API.Repositories;

/// <summary>
/// Implementación del repositorio de Usuarios
/// Hereda de GenericRepository y agrega métodos específicos
/// </summary>
public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(ApplicationDbContext context) : base(context)
    {
    }

    // ========================================
    // MÉTODOS DE CONSULTA
    // ========================================

    /// <summary>
    /// Obtener un usuario por su username
    /// </summary>
    public async Task<Usuario?> GetByUsernameAsync(string username)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
    }

    /// <summary>
    /// Obtener un usuario por su email
    /// </summary>
    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }

    /// <summary>
    /// Obtener un usuario por username o email
    /// </summary>
    public async Task<Usuario?> GetByUsernameOrEmailAsync(string usernameOrEmail)
    {
        var lowerValue = usernameOrEmail.ToLower();

        return await _dbSet
            .FirstOrDefaultAsync(u =>
                u.Username.ToLower() == lowerValue ||
                u.Email.ToLower() == lowerValue
            );
    }

    /// <summary>
    /// Obtener un usuario con sus roles asignados (navegación eager loading)
    /// </summary>
    public async Task<Usuario?> GetByIdWithRolesAsync(int idUsuario)
    {
        return await _dbSet
            .Include(u => u.UsuariosRoles) // Incluir la tabla intermedia
                .ThenInclude(ur => ur.Rol) // Incluir los roles
            .FirstOrDefaultAsync(u => u.IdUsuario == idUsuario);
    }

    /// <summary>
    /// Obtener todos los usuarios con sus roles
    /// </summary>
    public async Task<IEnumerable<Usuario>> GetAllWithRolesAsync()
    {
        return await _dbSet
            .Include(u => u.UsuariosRoles)
                .ThenInclude(ur => ur.Rol)
            .ToListAsync();
    }

    /// <summary>
    /// Obtener solo usuarios activos
    /// </summary>
    public async Task<IEnumerable<Usuario>> GetUsuariosActivosAsync()
    {
        return await _dbSet
            .Where(u => u.Estado == true)
            .ToListAsync();
    }

    /// <summary>
    /// Obtener usuarios por tipo (cliente/empleado)
    /// </summary>
    public async Task<IEnumerable<Usuario>> GetUsuariosByTipoAsync(string tipo)
    {
        return await _dbSet
            .Where(u => u.TipoUsuario.ToLower() == tipo.ToLower())
            .ToListAsync();
    }

    /// <summary>
    /// Obtener usuarios bloqueados
    /// </summary>
    public async Task<IEnumerable<Usuario>> GetUsuariosBloqueadosAsync()
    {
        return await _dbSet
            .Where(u => u.Bloqueado == true)
            .ToListAsync();
    }

    // ========================================
    // MÉTODOS DE VALIDACIÓN
    // ========================================

    /// <summary>
    /// Verificar si existe un username
    /// </summary>
    public async Task<bool> ExistsByUsernameAsync(string username)
    {
        return await _dbSet
            .AnyAsync(u => u.Username.ToLower() == username.ToLower());
    }

    /// <summary>
    /// Verificar si existe un email
    /// </summary>
    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _dbSet
            .AnyAsync(u => u.Email.ToLower() == email.ToLower());
    }

    /// <summary>
    /// Verificar si existe un username excluyendo un ID
    /// </summary>
    public async Task<bool> ExistsByUsernameExceptIdAsync(string username, int idUsuario)
    {
        return await _dbSet
            .AnyAsync(u =>
                u.Username.ToLower() == username.ToLower() &&
                u.IdUsuario != idUsuario
            );
    }

    /// <summary>
    /// Verificar si existe un email excluyendo un ID
    /// </summary>
    public async Task<bool> ExistsByEmailExceptIdAsync(string email, int idUsuario)
    {
        return await _dbSet
            .AnyAsync(u =>
                u.Email.ToLower() == email.ToLower() &&
                u.IdUsuario != idUsuario
            );
    }

    // ========================================
    // MÉTODOS DE SEGURIDAD
    // ========================================

    /// <summary>
    /// Actualizar el último acceso del usuario
    /// </summary>
    public async Task UpdateUltimoAccesoAsync(int idUsuario)
    {
        var usuario = await GetByIdAsync(idUsuario);
        if (usuario != null)
        {
            usuario.UltimoAcceso = DateTime.Now;
            await SaveChangesAsync();
        }
    }

    /// <summary>
    /// Incrementar intentos fallidos
    /// </summary>
    public async Task IncrementarIntentosFallidosAsync(int idUsuario)
    {
        var usuario = await GetByIdAsync(idUsuario);
        if (usuario != null)
        {
            usuario.IntentosFallidos++;
            usuario.FechaModificacion = DateTime.Now;
            await SaveChangesAsync();
        }
    }

    /// <summary>
    /// Reiniciar intentos fallidos a 0
    /// </summary>
    public async Task ReiniciarIntentosFallidosAsync(int idUsuario)
    {
        var usuario = await GetByIdAsync(idUsuario);
        if (usuario != null)
        {
            usuario.IntentosFallidos = 0;
            usuario.FechaModificacion = DateTime.Now;
            await SaveChangesAsync();
        }
    }

    /// <summary>
    /// Bloquear un usuario
    /// </summary>
    public async Task BloquearUsuarioAsync(int idUsuario)
    {
        var usuario = await GetByIdAsync(idUsuario);
        if (usuario != null)
        {
            usuario.Bloqueado = true;
            usuario.FechaModificacion = DateTime.Now;
            await SaveChangesAsync();
        }
    }

    /// <summary>
    /// Desbloquear un usuario y reiniciar intentos fallidos
    /// </summary>
    public async Task DesbloquearUsuarioAsync(int idUsuario)
    {
        var usuario = await GetByIdAsync(idUsuario);
        if (usuario != null)
        {
            usuario.Bloqueado = false;
            usuario.IntentosFallidos = 0;
            usuario.FechaModificacion = DateTime.Now;
            await SaveChangesAsync();
        }
    }
}