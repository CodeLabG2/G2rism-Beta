using Microsoft.EntityFrameworkCore;
using G2rismBeta.API.Data;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.Models;

namespace G2rismBeta.API.Repositories;

/// <summary>
/// Repositorio específico para operaciones con Clientes
/// Maneja la persistencia y consultas especializadas del módulo CRM
/// </summary>
public class ClienteRepository : GenericRepository<Cliente>, IClienteRepository
{
    public ClienteRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Buscar cliente por documento de identidad único
    /// </summary>
    public async Task<Cliente?> GetByDocumentoAsync(string documentoIdentidad)
    {
        return await _dbSet
            .Include(c => c.Usuario)
            .Include(c => c.Categoria)
            .FirstOrDefaultAsync(c => c.DocumentoIdentidad == documentoIdentidad);
    }

    /// <summary>
    /// Buscar cliente por ID de usuario (relación 1:1)
    /// </summary>
    public async Task<Cliente?> GetByUsuarioIdAsync(int idUsuario)
    {
        return await _dbSet
            .Include(c => c.Usuario)
            .Include(c => c.Categoria)
            .FirstOrDefaultAsync(c => c.IdUsuario == idUsuario);
    }

    /// <summary>
    /// Obtener solo clientes activos ordenados por fecha de registro
    /// </summary>
    public async Task<IEnumerable<Cliente>> GetClientesActivosAsync()
    {
        return await _dbSet
            .Where(c => c.Estado == true)
            .Include(c => c.Usuario)
            .Include(c => c.Categoria)
            .OrderByDescending(c => c.FechaRegistro)
            .ToListAsync();
    }

    /// <summary>
    /// Obtener clientes filtrados por categoría
    /// Útil para segmentación CRM
    /// </summary>
    public async Task<IEnumerable<Cliente>> GetClientesPorCategoriaAsync(int idCategoria)
    {
        return await _dbSet
            .Where(c => c.IdCategoria == idCategoria)
            .Include(c => c.Usuario)
            .Include(c => c.Categoria)
            .OrderBy(c => c.Apellido)
            .ThenBy(c => c.Nombre)
            .ToListAsync();
    }

    /// <summary>
    /// Obtener cliente con su categoría incluida
    /// Para mostrar información completa en UI
    /// </summary>
    public async Task<Cliente?> GetClienteConCategoriaAsync(int idCliente)
    {
        return await _dbSet
            .Include(c => c.Categoria)
            .Include(c => c.Usuario)
            .FirstOrDefaultAsync(c => c.IdCliente == idCliente);
    }

    /// <summary>
    /// Obtener cliente con todas sus relaciones cargadas
    /// (Usuario, Categoría, Preferencias)
    /// </summary>
    public async Task<Cliente?> GetClienteCompletoAsync(int idCliente)
    {
        return await _dbSet
            .Include(c => c.Usuario)
            .Include(c => c.Categoria)
            .Include(c => c.Preferencia)
            .FirstOrDefaultAsync(c => c.IdCliente == idCliente);
    }

    /// <summary>
    /// Verificar si ya existe un documento de identidad registrado
    /// Útil para evitar duplicados al crear o actualizar
    /// </summary>
    public async Task<bool> ExisteDocumentoAsync(string documentoIdentidad, int? idClienteExcluir = null)
    {
        var query = _dbSet.Where(c => c.DocumentoIdentidad == documentoIdentidad);

        // Si estamos actualizando, excluir el cliente actual
        if (idClienteExcluir.HasValue)
        {
            query = query.Where(c => c.IdCliente != idClienteExcluir.Value);
        }

        return await query.AnyAsync();
    }

    /// <summary>
    /// Verificar si un usuario ya tiene un cliente asociado
    /// Un usuario solo puede tener un cliente (relación 1:1)
    /// </summary>
    public async Task<bool> UsuarioTieneClienteAsync(int idUsuario, int? idClienteExcluir = null)
    {
        var query = _dbSet.Where(c => c.IdUsuario == idUsuario);

        // Si estamos actualizando, excluir el cliente actual
        if (idClienteExcluir.HasValue)
        {
            query = query.Where(c => c.IdCliente != idClienteExcluir.Value);
        }

        return await query.AnyAsync();
    }

    /// <summary>
    /// Cambiar el estado de un cliente (activar/desactivar)
    /// </summary>
    public async Task<bool> CambiarEstadoAsync(int idCliente, bool estado)
    {
        var cliente = await GetByIdAsync(idCliente);
        if (cliente == null)
            return false;

        cliente.Estado = estado;

        return true; // Los cambios se guardan con SaveChangesAsync()
    }

    /// <summary>
    /// Buscar clientes por nombre o apellido (búsqueda parcial)
    /// Case insensitive para mejor experiencia de usuario
    /// </summary>
    public async Task<IEnumerable<Cliente>> BuscarPorNombreAsync(string termino)
    {
        var terminoLower = termino.ToLower();

        return await _dbSet
            .Where(c =>
                c.Nombre.ToLower().Contains(terminoLower) ||
                c.Apellido.ToLower().Contains(terminoLower))
            .Include(c => c.Usuario)
            .Include(c => c.Categoria)
            .OrderBy(c => c.Apellido)
            .ThenBy(c => c.Nombre)
            .ToListAsync();
    }

    /// <summary>
    /// Obtener clientes filtrados por ciudad
    /// </summary>
    public async Task<IEnumerable<Cliente>> GetClientesPorCiudadAsync(string ciudad)
    {
        return await _dbSet
            .Where(c => c.Ciudad.ToLower() == ciudad.ToLower())
            .Include(c => c.Usuario)
            .Include(c => c.Categoria)
            .OrderBy(c => c.Apellido)
            .ThenBy(c => c.Nombre)
            .ToListAsync();
    }

    /// <summary>
    /// Obtener clientes filtrados por país
    /// </summary>
    public async Task<IEnumerable<Cliente>> GetClientesPorPaisAsync(string pais)
    {
        return await _dbSet
            .Where(c => c.Pais.ToLower() == pais.ToLower())
            .Include(c => c.Usuario)
            .Include(c => c.Categoria)
            .OrderBy(c => c.Ciudad)
            .ThenBy(c => c.Apellido)
            .ThenBy(c => c.Nombre)
            .ToListAsync();
    }
}
