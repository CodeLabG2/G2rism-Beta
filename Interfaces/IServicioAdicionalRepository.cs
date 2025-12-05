using G2rismBeta.API.Models;

namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interfaz del repositorio de servicios adicionales con métodos de búsqueda avanzada
/// </summary>
public interface IServicioAdicionalRepository : IGenericRepository<ServicioAdicional>
{
    /// <summary>
    /// Obtiene un servicio adicional por su ID con información del proveedor
    /// </summary>
    /// <param name="id">ID del servicio</param>
    /// <returns>Servicio con información del proveedor o null si no existe</returns>
    Task<ServicioAdicional?> GetByIdConProveedorAsync(int id);

    /// <summary>
    /// Obtiene todos los servicios adicionales con información del proveedor
    /// </summary>
    /// <returns>Lista de servicios con proveedores</returns>
    Task<IEnumerable<ServicioAdicional>> GetAllConProveedorAsync();

    /// <summary>
    /// Busca servicios adicionales por tipo
    /// </summary>
    /// <param name="tipo">Tipo de servicio (tour, guia, actividad, transporte_interno)</param>
    /// <returns>Lista de servicios del tipo especificado</returns>
    Task<IEnumerable<ServicioAdicional>> GetByTipoAsync(string tipo);

    /// <summary>
    /// Obtiene todos los servicios disponibles
    /// </summary>
    /// <returns>Lista de servicios disponibles (activos y con disponibilidad)</returns>
    Task<IEnumerable<ServicioAdicional>> GetDisponiblesAsync();

    /// <summary>
    /// Obtiene todos los servicios activos con información del proveedor
    /// </summary>
    /// <returns>Lista de servicios activos</returns>
    Task<IEnumerable<ServicioAdicional>> GetActivosAsync();

    /// <summary>
    /// Busca servicios por rango de precio
    /// </summary>
    /// <param name="precioMin">Precio mínimo</param>
    /// <param name="precioMax">Precio máximo</param>
    /// <returns>Lista de servicios en el rango de precio</returns>
    Task<IEnumerable<ServicioAdicional>> GetByRangoPrecioAsync(decimal precioMin, decimal precioMax);

    /// <summary>
    /// Busca servicios por unidad de medida
    /// </summary>
    /// <param name="unidad">Unidad de medida (persona, grupo, hora, dia)</param>
    /// <returns>Lista de servicios con la unidad especificada</returns>
    Task<IEnumerable<ServicioAdicional>> GetByUnidadAsync(string unidad);

    /// <summary>
    /// Busca servicios por duración máxima en minutos
    /// </summary>
    /// <param name="duracionMaxima">Duración máxima en minutos</param>
    /// <returns>Lista de servicios con duración menor o igual a la especificada</returns>
    Task<IEnumerable<ServicioAdicional>> GetByDuracionMaximaAsync(int duracionMaxima);

    /// <summary>
    /// Busca servicios por proveedor
    /// </summary>
    /// <param name="idProveedor">ID del proveedor</param>
    /// <returns>Lista de servicios del proveedor especificado</returns>
    Task<IEnumerable<ServicioAdicional>> GetByProveedorAsync(int idProveedor);

    /// <summary>
    /// Verifica si existe un servicio con el mismo nombre para el mismo proveedor
    /// </summary>
    /// <param name="nombre">Nombre del servicio</param>
    /// <param name="idProveedor">ID del proveedor</param>
    /// <param name="idServicioExcluir">ID del servicio a excluir (para actualizaciones)</param>
    /// <returns>True si existe, False si no</returns>
    Task<bool> ExistePorNombreYProveedorAsync(string nombre, int idProveedor, int? idServicioExcluir = null);
}
