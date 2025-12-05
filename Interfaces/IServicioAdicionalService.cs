using G2rismBeta.API.DTOs.ServicioAdicional;

namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interfaz del servicio de servicios adicionales con lógica de negocio
/// </summary>
public interface IServicioAdicionalService
{
    /// <summary>
    /// Obtiene todos los servicios adicionales con información del proveedor
    /// </summary>
    /// <returns>Lista de servicios adicionales</returns>
    Task<IEnumerable<ServicioAdicionalResponseDto>> GetAllAsync();

    /// <summary>
    /// Obtiene un servicio adicional por su ID con información del proveedor
    /// </summary>
    /// <param name="id">ID del servicio</param>
    /// <returns>Servicio encontrado</returns>
    /// <exception cref="KeyNotFoundException">Si el servicio no existe</exception>
    Task<ServicioAdicionalResponseDto> GetByIdAsync(int id);

    /// <summary>
    /// Busca servicios adicionales por tipo
    /// </summary>
    /// <param name="tipo">Tipo de servicio (tour, guia, actividad, transporte_interno)</param>
    /// <returns>Lista de servicios del tipo especificado</returns>
    Task<IEnumerable<ServicioAdicionalResponseDto>> GetByTipoAsync(string tipo);

    /// <summary>
    /// Obtiene todos los servicios disponibles (activos y con disponibilidad)
    /// </summary>
    /// <returns>Lista de servicios disponibles</returns>
    Task<IEnumerable<ServicioAdicionalResponseDto>> GetDisponiblesAsync();

    /// <summary>
    /// Obtiene todos los servicios activos
    /// </summary>
    /// <returns>Lista de servicios activos</returns>
    Task<IEnumerable<ServicioAdicionalResponseDto>> GetActivosAsync();

    /// <summary>
    /// Busca servicios por rango de precio
    /// </summary>
    /// <param name="precioMin">Precio mínimo</param>
    /// <param name="precioMax">Precio máximo</param>
    /// <returns>Lista de servicios en el rango de precio</returns>
    Task<IEnumerable<ServicioAdicionalResponseDto>> GetByRangoPrecioAsync(decimal precioMin, decimal precioMax);

    /// <summary>
    /// Busca servicios por unidad de medida
    /// </summary>
    /// <param name="unidad">Unidad de medida (persona, grupo, hora, dia)</param>
    /// <returns>Lista de servicios con la unidad especificada</returns>
    Task<IEnumerable<ServicioAdicionalResponseDto>> GetByUnidadAsync(string unidad);

    /// <summary>
    /// Busca servicios por duración máxima
    /// </summary>
    /// <param name="duracionMaxima">Duración máxima en minutos</param>
    /// <returns>Lista de servicios con duración menor o igual a la especificada</returns>
    Task<IEnumerable<ServicioAdicionalResponseDto>> GetByDuracionMaximaAsync(int duracionMaxima);

    /// <summary>
    /// Busca servicios por proveedor
    /// </summary>
    /// <param name="idProveedor">ID del proveedor</param>
    /// <returns>Lista de servicios del proveedor</returns>
    Task<IEnumerable<ServicioAdicionalResponseDto>> GetByProveedorAsync(int idProveedor);

    /// <summary>
    /// Crea un nuevo servicio adicional
    /// </summary>
    /// <param name="servicioDto">Datos del servicio a crear</param>
    /// <returns>Servicio creado</returns>
    /// <exception cref="ArgumentException">Si el proveedor no existe o si ya existe un servicio con el mismo nombre para el mismo proveedor</exception>
    Task<ServicioAdicionalResponseDto> CreateAsync(ServicioAdicionalCreateDto servicioDto);

    /// <summary>
    /// Actualiza un servicio adicional existente
    /// </summary>
    /// <param name="id">ID del servicio a actualizar</param>
    /// <param name="servicioDto">Datos a actualizar</param>
    /// <returns>Servicio actualizado</returns>
    /// <exception cref="KeyNotFoundException">Si el servicio no existe</exception>
    /// <exception cref="ArgumentException">Si el proveedor no existe o si ya existe otro servicio con el mismo nombre para el mismo proveedor</exception>
    Task<ServicioAdicionalResponseDto> UpdateAsync(int id, ServicioAdicionalUpdateDto servicioDto);

    /// <summary>
    /// Elimina un servicio adicional (soft delete - cambia estado a inactivo)
    /// </summary>
    /// <param name="id">ID del servicio a eliminar</param>
    /// <returns>True si se eliminó correctamente</returns>
    /// <exception cref="KeyNotFoundException">Si el servicio no existe</exception>
    Task<bool> DeleteAsync(int id);
}
