namespace G2rismBeta.API.DTOs.Pago;

/// <summary>
/// DTO para crear un nuevo pago en el sistema
/// </summary>
public class PagoCreateDto
{
    /// <summary>
    /// ID de la factura a la que corresponde este pago
    /// </summary>
    public int IdFactura { get; set; }

    /// <summary>
    /// ID de la forma de pago utilizada
    /// </summary>
    public int IdFormaPago { get; set; }

    /// <summary>
    /// Monto del pago (puede ser parcial o total)
    /// </summary>
    public decimal Monto { get; set; }

    /// <summary>
    /// Fecha y hora en que se realizó el pago
    /// Si no se proporciona, se usa la fecha actual
    /// </summary>
    public DateTime? FechaPago { get; set; }

    /// <summary>
    /// Referencia o código de transacción del pago
    /// Ejemplo: número de autorización bancaria, número de recibo
    /// </summary>
    public string? ReferenciaTransaccion { get; set; }

    /// <summary>
    /// Comprobante de pago (URL a archivo o ruta)
    /// </summary>
    public string? ComprobantePago { get; set; }

    /// <summary>
    /// Estado del pago: pendiente, aprobado, rechazado
    /// Por defecto: pendiente
    /// </summary>
    public string? Estado { get; set; }

    /// <summary>
    /// Observaciones adicionales sobre el pago
    /// </summary>
    public string? Observaciones { get; set; }
}
