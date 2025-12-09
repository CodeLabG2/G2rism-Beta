namespace G2rismBeta.API.DTOs.Factura;

/// <summary>
/// DTO para actualizar una factura existente.
/// Todos los campos son opcionales para permitir actualizaciones parciales.
/// </summary>
public class FacturaUpdateDto
{
    /// <summary>
    /// Fecha de vencimiento de la factura
    /// </summary>
    /// <example>2025-01-20</example>
    public DateTime? FechaVencimiento { get; set; }

    /// <summary>
    /// Resolución de la DIAN (opcional)
    /// </summary>
    /// <example>Resolución DIAN 123456 del 2024-01-01</example>
    public string? ResolucionDian { get; set; }

    /// <summary>
    /// CUFE/CUDE de facturación electrónica (opcional)
    /// </summary>
    /// <example>CUFE123456789ABC</example>
    public string? CufeCude { get; set; }

    /// <summary>
    /// Estado de la factura
    /// Valores permitidos: pendiente, pagada, cancelada, vencida
    /// </summary>
    /// <example>pagada</example>
    public string? Estado { get; set; }

    /// <summary>
    /// Observaciones adicionales
    /// </summary>
    /// <example>Factura actualizada por cambio en la reserva</example>
    public string? Observaciones { get; set; }
}