namespace G2rismBeta.API.DTOs.FormaDePago;

/// <summary>
/// DTO para actualizar una forma de pago existente.
/// Todos los campos son opcionales (nullable) para permitir actualizaciones parciales.
/// </summary>
public class FormaDePagoUpdateDto
{
    /// <summary>
    /// Método de pago
    /// </summary>
    /// <example>Tarjeta de Débito</example>
    public string? Metodo { get; set; }

    /// <summary>
    /// Descripción adicional del método de pago
    /// </summary>
    /// <example>Acepta débito de todas las entidades bancarias</example>
    public string? Descripcion { get; set; }

    /// <summary>
    /// Indica si el método de pago requiere verificación externa
    /// </summary>
    /// <example>true</example>
    public bool? RequiereVerificacion { get; set; }

    /// <summary>
    /// Indica si el método de pago está activo y disponible
    /// </summary>
    /// <example>true</example>
    public bool? Activo { get; set; }
}
