using G2rismBeta.API.DTOs.FormaDePago;

namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interface del servicio de Formas de Pago.
/// Define las operaciones de negocio para la gestión de formas de pago.
/// </summary>
public interface IFormaDePagoService
{
    /// <summary>
    /// Obtener todas las formas de pago
    /// </summary>
    /// <returns>Lista de formas de pago</returns>
    Task<IEnumerable<FormaDePagoResponseDto>> GetAllFormasDePagoAsync();

    /// <summary>
    /// Obtener todas las formas de pago activas
    /// </summary>
    /// <returns>Lista de formas de pago activas</returns>
    Task<IEnumerable<FormaDePagoResponseDto>> GetFormasDePagoActivasAsync();

    /// <summary>
    /// Obtener forma de pago por ID
    /// </summary>
    /// <param name="id">ID de la forma de pago</param>
    /// <returns>Forma de pago encontrada</returns>
    /// <exception cref="KeyNotFoundException">Si no se encuentra la forma de pago</exception>
    Task<FormaDePagoResponseDto> GetFormaDePagoByIdAsync(int id);

    /// <summary>
    /// Obtener forma de pago por método
    /// </summary>
    /// <param name="metodo">Método de pago</param>
    /// <returns>Forma de pago encontrada</returns>
    /// <exception cref="KeyNotFoundException">Si no se encuentra la forma de pago</exception>
    Task<FormaDePagoResponseDto> GetFormaDePagoPorMetodoAsync(string metodo);

    /// <summary>
    /// Obtener formas de pago que requieren verificación
    /// </summary>
    /// <returns>Lista de formas de pago que requieren verificación</returns>
    Task<IEnumerable<FormaDePagoResponseDto>> GetFormasQueRequierenVerificacionAsync();

    /// <summary>
    /// Crear una nueva forma de pago
    /// </summary>
    /// <param name="createDto">Datos para crear la forma de pago</param>
    /// <returns>Forma de pago creada</returns>
    /// <exception cref="InvalidOperationException">Si el método ya existe</exception>
    Task<FormaDePagoResponseDto> CreateFormaDePagoAsync(FormaDePagoCreateDto createDto);

    /// <summary>
    /// Actualizar una forma de pago existente
    /// </summary>
    /// <param name="id">ID de la forma de pago a actualizar</param>
    /// <param name="updateDto">Datos para actualizar</param>
    /// <returns>Forma de pago actualizada</returns>
    /// <exception cref="KeyNotFoundException">Si no se encuentra la forma de pago</exception>
    /// <exception cref="InvalidOperationException">Si el nuevo método ya existe</exception>
    Task<FormaDePagoResponseDto> UpdateFormaDePagoAsync(int id, FormaDePagoUpdateDto updateDto);

    /// <summary>
    /// Eliminar una forma de pago (soft delete)
    /// </summary>
    /// <param name="id">ID de la forma de pago a eliminar</param>
    /// <exception cref="KeyNotFoundException">Si no se encuentra la forma de pago</exception>
    /// <exception cref="InvalidOperationException">Si la forma de pago tiene pagos asociados</exception>
    Task DeleteFormaDePagoAsync(int id);
}