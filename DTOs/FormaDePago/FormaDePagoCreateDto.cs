namespace G2rismBeta.API.DTOs.FormaDePago;

/// <summary>
/// DTO para crear una nueva forma de pago en el sistema.
/// </summary>
public class FormaDePagoCreateDto
{
    /// <summary>
    /// Método de pago
    /// Valores comunes: "Efectivo", "Tarjeta de Crédito", "Tarjeta de Débito", "Transferencia Bancaria", "PSE", "Nequi", "Daviplata"
    /// </summary>
    /// <example>Tarjeta de Crédito</example>
    public string Metodo { get; set; } = string.Empty;

    /// <summary>
    /// Descripción adicional del método de pago
    /// </summary>
    /// <example>Acepta Visa, Mastercard y American Express</example>
    public string? Descripcion { get; set; }

    /// <summary>
    /// Indica si el método de pago requiere verificación externa
    /// Ejemplo: Tarjetas requieren verificación bancaria, efectivo no
    /// </summary>
    /// <example>true</example>
    public bool RequiereVerificacion { get; set; } = false;

    /// <summary>
    /// Indica si el método de pago está activo y disponible para usarse
    /// </summary>
    /// <example>true</example>
    public bool Activo { get; set; } = true;
}