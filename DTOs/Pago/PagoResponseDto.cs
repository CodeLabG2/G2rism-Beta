namespace G2rismBeta.API.DTOs.Pago;

/// <summary>
/// DTO para respuesta de un pago
/// </summary>
public class PagoResponseDto
{
    /// <summary>
    /// Identificador único del pago
    /// </summary>
    public int IdPago { get; set; }

    /// <summary>
    /// ID de la factura a la que corresponde este pago
    /// </summary>
    public int IdFactura { get; set; }

    /// <summary>
    /// Número de la factura asociada
    /// </summary>
    public string? NumeroFactura { get; set; }

    /// <summary>
    /// ID de la forma de pago utilizada
    /// </summary>
    public int IdFormaPago { get; set; }

    /// <summary>
    /// Nombre del método de pago utilizado
    /// </summary>
    public string? MetodoPago { get; set; }

    /// <summary>
    /// Monto del pago
    /// </summary>
    public decimal Monto { get; set; }

    /// <summary>
    /// Fecha y hora en que se realizó el pago
    /// </summary>
    public DateTime FechaPago { get; set; }

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
    public string Estado { get; set; } = string.Empty;

    /// <summary>
    /// Observaciones adicionales sobre el pago
    /// </summary>
    public string? Observaciones { get; set; }

    /// <summary>
    /// Fecha y hora de registro del pago en el sistema
    /// </summary>
    public DateTime FechaCreacion { get; set; }

    /// <summary>
    /// Fecha y hora de la última modificación del pago
    /// </summary>
    public DateTime? FechaModificacion { get; set; }

    /// <summary>
    /// Indica si el pago está aprobado
    /// </summary>
    public bool EstaAprobado { get; set; }

    /// <summary>
    /// Indica si el pago está pendiente
    /// </summary>
    public bool EstaPendiente { get; set; }

    /// <summary>
    /// Indica si el pago fue rechazado
    /// </summary>
    public bool EstaRechazado { get; set; }

    /// <summary>
    /// Días transcurridos desde el pago
    /// </summary>
    public int DiasDesdeElPago { get; set; }

    /// <summary>
    /// Indica si el pago tiene comprobante adjunto
    /// </summary>
    public bool TieneComprobante { get; set; }
}