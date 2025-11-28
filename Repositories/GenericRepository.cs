using Microsoft.EntityFrameworkCore;
using G2rismBeta.API.Data;
using G2rismBeta.API.Interfaces;

namespace G2rismBeta.API.Repositories;

/// <summary>
/// Implementación genérica del repositorio con operaciones CRUD básicas
/// </summary>
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    /// <summary>
    /// Obtener todas las entidades de forma asíncrona
    /// </summary>
    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    /// <summary>
    /// Obtener una entidad por su ID
    /// </summary>
    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    /// <summary>
    /// Agregar una nueva entidad
    /// </summary>
    public virtual async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    /// <summary>
    /// Actualizar una entidad existente
    /// </summary>
    public virtual Task<T> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return Task.FromResult(entity);
    }

    /// <summary>
    /// Eliminar una entidad por su ID
    /// </summary>
    public virtual async Task<bool> DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null)
            return false;

        _dbSet.Remove(entity);
        return true;
    }

    /// <summary>
    /// Verificar si existe una entidad con el ID dado
    /// </summary>
    public virtual async Task<bool> ExistsAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        return entity != null;
    }

    /// <summary>
    /// Guardar todos los cambios pendientes en la base de datos
    /// </summary>
    public virtual async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
