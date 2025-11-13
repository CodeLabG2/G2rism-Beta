using G2rismBeta.API.Models;

namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interface específica para operaciones con Categorías de Cliente
/// Extiende el repositorio genérico y agrega métodos específicos para CRM
/// </summary>
public interface ICategoriaClienteRepository : IGenericRepository<CategoriaCliente>
{
    /// <summary>
    /// Buscar categoría por nombre
    /// </summary>
    Task<CategoriaCliente?> GetByNombreAsync(string nombre);

    /// <summary>
    /// Obtener categorías activas
    /// </summary>
    Task<IEnumerable<CategoriaCliente>> GetCategoriasActivasAsync();

    /// <summary>
    /// Obtener categoría con sus clientes
    /// </summary>
    Task<CategoriaCliente?> GetCategoriaConClientesAsync(int idCategoria);

    /// <summary>
    /// Verificar si existe una categoría con ese nombre
    /// </summary>
    Task<bool> ExisteNombreAsync(string nombre, int? idCategoriaExcluir = null);

    /// <summary>
    /// Cambiar estado de la categoría (activar/desactivar)
    /// </summary>
    Task<bool> CambiarEstadoAsync(int idCategoria, bool estado);

    /// <summary>
    /// Obtener categorías ordenadas por descuento (mayor a menor)
    /// </summary>
    Task<IEnumerable<CategoriaCliente>> GetCategoriasOrdenadaPorDescuentoAsync();

    /// <summary>
    /// Contar clientes por categoría
    /// </summary>
    Task<int> ContarClientesPorCategoriaAsync(int idCategoria);
}
