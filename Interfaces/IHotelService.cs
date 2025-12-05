using G2rismBeta.API.DTOs.Hotel;

namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interfaz del servicio de hoteles con lógica de negocio
/// </summary>
public interface IHotelService
{
    /// <summary>
    /// Obtiene todos los hoteles con información del proveedor
    /// </summary>
    /// <returns>Lista de hoteles</returns>
    Task<IEnumerable<HotelResponseDto>> GetAllAsync();

    /// <summary>
    /// Obtiene un hotel por su ID con información del proveedor
    /// </summary>
    /// <param name="id">ID del hotel</param>
    /// <returns>Hotel encontrado</returns>
    /// <exception cref="KeyNotFoundException">Si el hotel no existe</exception>
    Task<HotelResponseDto> GetByIdAsync(int id);

    /// <summary>
    /// Busca hoteles por ciudad
    /// </summary>
    /// <param name="ciudad">Nombre de la ciudad</param>
    /// <returns>Lista de hoteles en la ciudad</returns>
    Task<IEnumerable<HotelResponseDto>> GetByCiudadAsync(string ciudad);

    /// <summary>
    /// Busca hoteles por país
    /// </summary>
    /// <param name="pais">Nombre del país</param>
    /// <returns>Lista de hoteles en el país</returns>
    Task<IEnumerable<HotelResponseDto>> GetByPaisAsync(string pais);

    /// <summary>
    /// Busca hoteles por clasificación de estrellas
    /// </summary>
    /// <param name="estrellas">Número de estrellas (1-5)</param>
    /// <returns>Lista de hoteles con la clasificación</returns>
    Task<IEnumerable<HotelResponseDto>> GetByEstrellasAsync(int estrellas);

    /// <summary>
    /// Busca hoteles por categoría
    /// </summary>
    /// <param name="categoria">Categoría del hotel</param>
    /// <returns>Lista de hoteles en la categoría</returns>
    Task<IEnumerable<HotelResponseDto>> GetByCategoriaAsync(string categoria);

    /// <summary>
    /// Obtiene todos los hoteles activos
    /// </summary>
    /// <returns>Lista de hoteles activos</returns>
    Task<IEnumerable<HotelResponseDto>> GetActivosAsync();

    /// <summary>
    /// Busca hoteles por rango de precio
    /// </summary>
    /// <param name="precioMin">Precio mínimo por noche</param>
    /// <param name="precioMax">Precio máximo por noche</param>
    /// <returns>Lista de hoteles en el rango de precio</returns>
    Task<IEnumerable<HotelResponseDto>> GetByRangoPrecioAsync(decimal precioMin, decimal precioMax);

    /// <summary>
    /// Busca hoteles con servicios específicos
    /// </summary>
    /// <param name="wifi">Requiere WiFi</param>
    /// <param name="piscina">Requiere piscina</param>
    /// <param name="restaurante">Requiere restaurante</param>
    /// <param name="gimnasio">Requiere gimnasio</param>
    /// <param name="parqueadero">Requiere parqueadero</param>
    /// <returns>Lista de hoteles que cumplen los requisitos</returns>
    Task<IEnumerable<HotelResponseDto>> GetByServiciosAsync(
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
    Task<IEnumerable<HotelResponseDto>> GetPremiumAsync();

    /// <summary>
    /// Crea un nuevo hotel
    /// </summary>
    /// <param name="hotelDto">Datos del hotel a crear</param>
    /// <returns>Hotel creado</returns>
    /// <exception cref="ArgumentException">Si el proveedor no existe o si ya existe un hotel con el mismo nombre en la ciudad</exception>
    Task<HotelResponseDto> CreateAsync(HotelCreateDto hotelDto);

    /// <summary>
    /// Actualiza un hotel existente
    /// </summary>
    /// <param name="id">ID del hotel a actualizar</param>
    /// <param name="hotelDto">Datos a actualizar</param>
    /// <returns>Hotel actualizado</returns>
    /// <exception cref="KeyNotFoundException">Si el hotel no existe</exception>
    /// <exception cref="ArgumentException">Si el proveedor no existe o si ya existe otro hotel con el mismo nombre en la ciudad</exception>
    Task<HotelResponseDto> UpdateAsync(int id, HotelUpdateDto hotelDto);

    /// <summary>
    /// Elimina un hotel (soft delete - cambia estado a inactivo)
    /// </summary>
    /// <param name="id">ID del hotel a eliminar</param>
    /// <returns>True si se eliminó correctamente</returns>
    /// <exception cref="KeyNotFoundException">Si el hotel no existe</exception>
    Task<bool> DeleteAsync(int id);
}
