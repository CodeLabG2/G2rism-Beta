using G2rismBeta.API.DTOs.CategoriaCliente;

namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interface del servicio de Categorías de Cliente
/// Define la lógica de negocio para gestión de categorías CRM
/// </summary>
public interface ICategoriaClienteService
{
    // ========================================
    // OPERACIONES CRUD BÁSICAS
    // ========================================

    /// <summary>
    /// Obtener todas las categorías de cliente
    /// </summary>
    /// <returns>Lista de categorías en formato DTO</returns>
    Task<IEnumerable<CategoriaClienteResponseDto>> GetAllCategoriasAsync();

    /// <summary>
    /// Obtener solo categorías activas
    /// </summary>
    Task<IEnumerable<CategoriaClienteResponseDto>> GetCategoriasActivasAsync();

    /// <summary>
    /// Obtener una categoría por su ID
    /// </summary>
    /// <param name="idCategoria">ID de la categoría a buscar</param>
    /// <returns>Categoría encontrada o null</returns>
    Task<CategoriaClienteResponseDto?> GetCategoriaByIdAsync(int idCategoria);

    /// <summary>
    /// Crear una nueva categoría de cliente
    /// </summary>
    /// <param name="categoriaCreateDto">Datos de la categoría a crear</param>
    /// <returns>Categoría creada</returns>
    Task<CategoriaClienteResponseDto> CreateCategoriaAsync(CategoriaClienteCreateDto categoriaCreateDto);

    /// <summary>
    /// Actualizar una categoría existente
    /// </summary>
    /// <param name="idCategoria">ID de la categoría a actualizar</param>
    /// <param name="categoriaUpdateDto">Nuevos datos de la categoría</param>
    /// <returns>Categoría actualizada</returns>
    Task<CategoriaClienteResponseDto> UpdateCategoriaAsync(int idCategoria, CategoriaClienteUpdateDto categoriaUpdateDto);

    /// <summary>
    /// Eliminar una categoría (si no tiene clientes asignados)
    /// </summary>
    /// <param name="idCategoria">ID de la categoría a eliminar</param>
    /// <returns>True si se eliminó correctamente</returns>
    Task<bool> DeleteCategoriaAsync(int idCategoria);

    // ========================================
    // OPERACIONES ESPECIALES (CRM)
    // ========================================

    /// <summary>
    /// Cambiar el estado de una categoría (activar/desactivar)
    /// </summary>
    Task<bool> CambiarEstadoCategoriaAsync(int idCategoria, bool nuevoEstado);

    /// <summary>
    /// Obtener categorías ordenadas por descuento (mayor a menor)
    /// Útil para mostrar las mejores categorías primero
    /// </summary>
    Task<IEnumerable<CategoriaClienteResponseDto>> GetCategoriasOrdenadaPorDescuentoAsync();

    // ========================================
    // VALIDACIONES
    // ========================================

    /// <summary>
    /// Validar si un nombre de categoría ya existe
    /// </summary>
    Task<bool> NombreCategoriaExisteAsync(string nombre, int? idCategoriaExcluir = null);
}
