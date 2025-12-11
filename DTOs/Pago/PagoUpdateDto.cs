namespace G2rismBeta.API.DTOs.Pago;

/// <summary>
/// DTO para actualizar un pago existente
/// Todos los campos son opcionales para permitir actualizaciones parciales
/// </summary>
public class PagoUpdateDto
{
    /// <summary>
    /// Monto del pago (puede ser parcial o total)
    /// </summary>
    public decimal? Monto { get; set; }

    /// <summary>
    /// Referencia o código de transacción del pago
    /// </summary>
    public string? ReferenciaTransaccion { get; set; }

    /// <summary>
    /// Comprobante de pago (URL a archivo o ruta)
    /// </summary>
    public string? ComprobantePago { get; set; }

    /// <summary>
    /// Estado del pago: pendiente, aprobado, rechazado
    /// </summary>
    public string? Estado { get; set; }

    /// <summary>
    /// Observaciones adicionales sobre el pago
    /// </summary>
    public string? Observaciones { get; set; }
}