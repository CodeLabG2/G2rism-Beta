namespace G2rismBeta.API.Models;

/// <summary>
/// Modelo estándar para respuestas exitosas de la API
/// </summary>
/// <typeparam name="T">Tipo de datos que se devuelven</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Indica si la operación fue exitosa
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Mensaje descriptivo de la operación
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Datos devueltos (puede ser null)
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Timestamp de la respuesta
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.Now;

    /// <summary>
    /// Constructor para respuesta exitosa con datos
    /// </summary>
    public static ApiResponse<T> SuccessResponse(T data, string message = "Operación exitosa")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data,
            Timestamp = DateTime.Now
        };
    }

    /// <summary>
    /// Constructor para respuesta exitosa sin datos
    /// </summary>
    public static ApiResponse<T> SuccessResponse(string message = "Operación exitosa")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = default,
            Timestamp = DateTime.Now
        };
    }
}
