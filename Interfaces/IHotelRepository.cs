using G2rismBeta.API.Models;

namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interfaz del repositorio de hoteles con métodos de búsqueda avanzada
/// </summary>
public interface IHotelRepository : IGenericRepository<Hotel>
{
    /// <summary>
    /// Obtiene un hotel por su ID con información del proveedor
    /// </summary>
    /// <param name="id">ID del hotel</param>
    /// <returns>Hotel con información del proveedor o null si no existe</returns>
    Task<Hotel?> GetByIdConProveedorAsync(int id);

    /// <summary>
    /// Obtiene todos los hoteles con información del proveedor
    /// </summary>
    /// <returns>Lista de hoteles con proveedores</returns>
    Task<IEnumerable<Hotel>> GetAllConProveedorAsync();

    /// <summary>
    /// Busca hoteles por ciudad
    /// </summary>
    /// <param name="ciudad">Nombre de la ciudad</param>
    /// <returns>Lista de hoteles en la ciudad especificada</returns>
    Task<IEnumerable<Hotel>> GetByCiudadAsync(string ciudad);

    /// <summary>
    /// Busca hoteles por país
    /// </summary>
    /// <param name="pais">Nombre del país</param>
    /// <returns>Lista de hoteles en el país especificado</returns>
    Task<IEnumerable<Hotel>> GetByPaisAsync(string pais);

    /// <summary>
    /// Busca hoteles por clasificación de estrellas
    /// </summary>
    /// <param name="estrellas">Número de estrellas (1-5)</param>
    /// <returns>Lista de hoteles con la clasificación especificada</returns>
    Task<IEnumerable<Hotel>> GetByEstrellasAsync(int estrellas);

    /// <summary>
    /// Busca hoteles por categoría
    /// </summary>
    /// <param name="categoria">Categoría del hotel (economico, estandar, premium, lujo)</param>
    /// <returns>Lista de hoteles en la categoría especificada</returns>
    Task<IEnumerable<Hotel>> GetByCategoriaAsync(string categoria);

    /// <summary>
    /// Obtiene todos los hoteles activos con información del proveedor
    /// </summary>
    /// <returns>Lista de hoteles activos</returns>
    Task<IEnumerable<Hotel>> GetActivosAsync();

    /// <summary>
    /// Busca hoteles por rango de precio por noche
    /// </summary>
    /// <param name="precioMin">Precio mínimo</param>
    /// <param name="precioMax">Precio máximo</param>
    /// <returns>Lista de hoteles en el rango de precio</returns>
    Task<IEnumerable<Hotel>> GetByRangoPrecioAsync(decimal precioMin, decimal precioMax);

    /// <summary>
    /// Busca hoteles con servicios específicos
    /// </summary>
    /// <param name="wifi">Requiere WiFi</param>
    /// <param name="piscina">Requiere piscina</param>
    /// <param name="restaurante">Requiere restaurante</param>
    /// <param name="gimnasio">Requiere gimnasio</param>
    /// <param name="parqueadero">Requiere parqueadero</param>
    /// <returns>Lista de hoteles que cumplen con los servicios requeridos</returns>
    Task<IEnumerable<Hotel>> GetByServiciosAsync(
        bool? wifi = null,
        bool? piscina = null,
        bool? restaurante = null,
        bool? gimnasio = null,
        bool? parqueadero = null
    );

    /// <summary>
    /// Busca hoteles premium (con servicios adicionales)
    /// </summary>
    /// <returns>Lista de hoteles premium</returns>
    Task<IEnumerable<Hotel>> GetPremiumAsync();

    /// <summary>
    /// Verifica si existe un hotel con el mismo nombre en la misma ciudad
    /// </summary>
    /// <param name="nombre">Nombre del hotel</param>
    /// <param name="ciudad">Ciudad del hotel</param>
    /// <param name="idHotelExcluir">ID del hotel a excluir (para actualizaciones)</param>
    /// <returns>True si existe, False si no</returns>
    Task<bool> ExistePorNombreYCiudadAsync(string nombre, string ciudad, int? idHotelExcluir = null);
}
