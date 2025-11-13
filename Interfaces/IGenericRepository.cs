namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interface genérica para operaciones CRUD básicas
/// T representa cualquier entidad (Rol, Permiso, Usuario etc.)
/// </summary>
public interface IGenericRepository<T> where T : class
{
    /// <summary>
    /// Obtener todas las entidades
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Obtener una entidad por su ID
    /// </summary>
    Task<T?> GetByIdAsync(int id);

    /// <summary>
    /// Agregar una nueva entidad
    /// </summary>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Actualizar una entidad existente
    /// </summary>
    Task<T> UpdateAsync(T entity);

    /// <summary>
    /// Eliminar una entidad por su ID
    /// </summary>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Verificar si existe una entidad por su ID
    /// </summary>
    Task<bool> ExistsAsync(int id);

    /// <summary>
    /// Guardar cambios en la base de datos
    /// </summary>
    Task<bool> SaveChangesAsync();
}
