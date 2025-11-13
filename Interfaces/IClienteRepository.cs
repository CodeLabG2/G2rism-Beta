using G2rismBeta.API.Models;

namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interface específica para operaciones con Clientes
/// Extiende el repositorio genérico y agrega métodos específicos
/// </summary>
public interface IClienteRepository : IGenericRepository<Cliente>
{
    /// <summary>
    /// Buscar cliente por documento de identidad
    /// </summary>
    Task<Cliente?> GetByDocumentoAsync(string documentoIdentidad);

    /// <summary>
    /// Buscar cliente por ID de usuario
    /// </summary>
    Task<Cliente?> GetByUsuarioIdAsync(int idUsuario);

    /// <summary>
    /// Obtener clientes activos
    /// </summary>
    Task<IEnumerable<Cliente>> GetClientesActivosAsync();

    /// <summary>
    /// Obtener clientes por categoría
    /// </summary>
    Task<IEnumerable<Cliente>> GetClientesPorCategoriaAsync(int idCategoria);

    /// <summary>
    /// Obtener cliente con su categoría incluida
    /// </summary>
    Task<Cliente?> GetClienteConCategoriaAsync(int idCliente);

    /// <summary>
    /// Obtener cliente con todas sus relaciones (categoría, preferencias, usuario)
    /// </summary>
    Task<Cliente?> GetClienteCompletoAsync(int idCliente);

    /// <summary>
    /// Verificar si existe un documento de identidad
    /// </summary>
    Task<bool> ExisteDocumentoAsync(string documentoIdentidad, int? idClienteExcluir = null);

    /// <summary>
    /// Verificar si un usuario ya tiene un cliente asociado
    /// </summary>
    Task<bool> UsuarioTieneClienteAsync(int idUsuario, int? idClienteExcluir = null);

    /// <summary>
    /// Cambiar estado del cliente (activar/desactivar)
    /// </summary>
    Task<bool> CambiarEstadoAsync(int idCliente, bool estado);

    /// <summary>
    /// Buscar clientes por nombre o apellido
    /// </summary>
    Task<IEnumerable<Cliente>> BuscarPorNombreAsync(string termino);

    /// <summary>
    /// Obtener clientes por ciudad
    /// </summary>
    Task<IEnumerable<Cliente>> GetClientesPorCiudadAsync(string ciudad);

    /// <summary>
    /// Obtener clientes por país
    /// </summary>
    Task<IEnumerable<Cliente>> GetClientesPorPaisAsync(string pais);
}