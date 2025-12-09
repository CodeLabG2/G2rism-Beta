namespace G2rismBeta.API.DTOs.Factura;

/// <summary>
/// DTO para crear una nueva factura en el sistema.
/// Se genera automáticamente desde una reserva existente.
/// </summary>
public class FacturaCreateDto
{
    /// <summary>
    /// ID de la reserva asociada a esta factura
    /// </summary>
    /// <example>1</example>
    public int IdReserva { get; set; }

    /// <summary>
    /// Fecha de vencimiento de la factura (plazo de pago)
    /// Si no se proporciona, se calculará automáticamente (30 días desde emisión)
    /// </summary>
    /// <example>2025-01-15</example>
    public DateTime? FechaVencimiento { get; set; }

    /// <summary>
    /// Resolución de la DIAN (opcional en MVP)
    /// En producción debe contener la resolución de facturación electrónica
    /// </summary>
    /// <example>Resolución DIAN 123456 del 2024-01-01</example>
    public string? ResolucionDian { get; set; }

    /// <summary>
    /// Porcentaje de IVA a aplicar (por defecto 19%)
    /// </summary>
    /// <example>19</example>
    public decimal? PorcentajeIva { get; set; }

    /// <summary>
    /// Descuentos adicionales a aplicar (además del descuento de categoría del cliente)
    /// </summary>
    /// <example>50000</example>
    public decimal? DescuentosAdicionales { get; set; }

    /// <summary>
    /// Observaciones adicionales sobre la factura
    /// </summary>
    /// <example>Factura generada con descuento por cliente premium</example>
    public string? Observaciones { get; set; }
}