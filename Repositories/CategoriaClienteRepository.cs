using Microsoft.EntityFrameworkCore;
using G2rismBeta.API.Data;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.Models;

namespace G2rismBeta.API.Repositories;

/// <summary>
/// Repositorio específico para operaciones con Categorías de Cliente
/// Maneja la persistencia y consultas especializadas para el CRM
/// </summary>
public class CategoriaClienteRepository : GenericRepository<CategoriaCliente>, ICategoriaClienteRepository
{
    public CategoriaClienteRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Buscar categoría por nombre (case insensitive)
    /// </summary>
    public async Task<CategoriaCliente?> GetByNombreAsync(string nombre)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.Nombre.ToLower() == nombre.ToLower());
    }

    /// <summary>
    /// Obtener solo las categorías activas
    /// Ordenadas por descuento de mayor a menor
    /// </summary>
    public async Task<IEnumerable<CategoriaCliente>> GetCategoriasActivasAsync()
    {
        return await _dbSet
            .Where(c => c.Estado == true)
            .OrderByDescending(c => c.DescuentoPorcentaje)
            .ToListAsync();
    }

    /// <summary>
    /// Obtener una categoría con todos sus clientes incluidos
    /// Útil para análisis CRM y reportes
    /// </summary>
    public async Task<CategoriaCliente?> GetCategoriaConClientesAsync(int idCategoria)
    {
        return await _dbSet
            .Include(c => c.Clientes)
                .ThenInclude(cl => cl.Usuario)
            .FirstOrDefaultAsync(c => c.IdCategoria == idCategoria);
    }

    /// <summary>
    /// Verificar si ya existe una categoría con ese nombre
    /// Útil para evitar duplicados al crear o actualizar
    /// </summary>
    public async Task<bool> ExisteNombreAsync(string nombre, int? idCategoriaExcluir = null)
    {
        var query = _dbSet.Where(c => c.Nombre.ToLower() == nombre.ToLower());

        // Si estamos actualizando, excluir la categoría actual de la búsqueda
        if (idCategoriaExcluir.HasValue)
        {
            query = query.Where(c => c.IdCategoria != idCategoriaExcluir.Value);
        }

        return await query.AnyAsync();
    }

    /// <summary>
    /// Cambiar el estado de una categoría (activar/desactivar)
    /// </summary>
    public async Task<bool> CambiarEstadoAsync(int idCategoria, bool estado)
    {
        var categoria = await GetByIdAsync(idCategoria);
        if (categoria == null)
            return false;

        categoria.Estado = estado;

        return true; // Los cambios se guardan con SaveChangesAsync()
    }

    /// <summary>
    /// Obtener categorías ordenadas por descuento (mayor a menor)
    /// Útil para mostrar las mejores categorías primero en la UI
    /// </summary>
    public async Task<IEnumerable<CategoriaCliente>> GetCategoriasOrdenadaPorDescuentoAsync()
    {
        return await _dbSet
            .Where(c => c.Estado == true)
            .OrderByDescending(c => c.DescuentoPorcentaje)
            .ToListAsync();
    }

    /// <summary>
    /// Contar la cantidad de clientes que pertenecen a una categoría
    /// Útil para estadísticas CRM
    /// </summary>
    public async Task<int> ContarClientesPorCategoriaAsync(int idCategoria)
    {
        var categoria = await _dbSet
            .Include(c => c.Clientes)
            .FirstOrDefaultAsync(c => c.IdCategoria == idCategoria);

        return categoria?.Clientes.Count ?? 0;
    }
}
