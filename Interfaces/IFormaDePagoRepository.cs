using G2rismBeta.API.Models;

namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interface del repositorio de Formas de Pago.
/// Extiende el repositorio genérico con operaciones específicas de formas de pago.
/// </summary>
public interface IFormaDePagoRepository : IGenericRepository<FormaDePago>
{
    /// <summary>
    /// Obtener todas las formas de pago activas
    /// </summary>
    /// <returns>Lista de formas de pago activas</returns>
    Task<IEnumerable<FormaDePago>> GetFormasDePagoActivasAsync();

    /// <summary>
    /// Obtener una forma de pago por su método
    /// </summary>
    /// <param name="metodo">Método de pago (ej: "Efectivo", "Tarjeta de Crédito")</param>
    /// <returns>Forma de pago encontrada o null</returns>
    Task<FormaDePago?> GetPorMetodoAsync(string metodo);

    /// <summary>
    /// Verificar si existe una forma de pago con un método específico
    /// </summary>
    /// <param name="metodo">Método de pago a verificar</param>
    /// <returns>True si existe, False si no</returns>
    Task<bool> ExistePorMetodoAsync(string metodo);

    /// <summary>
    /// Verificar si existe una forma de pago con un método específico, excluyendo un ID
    /// (útil para validaciones en actualizaciones)
    /// </summary>
    /// <param name="metodo">Método de pago a verificar</param>
    /// <param name="excludeId">ID a excluir de la búsqueda</param>
    /// <returns>True si existe, False si no</returns>
    Task<bool> ExistePorMetodoAsync(string metodo, int excludeId);

    /// <summary>
    /// Obtener formas de pago que requieren verificación
    /// </summary>
    /// <returns>Lista de formas de pago que requieren verificación</returns>
    Task<IEnumerable<FormaDePago>> GetFormasQueRequierenVerificacionAsync();
}
