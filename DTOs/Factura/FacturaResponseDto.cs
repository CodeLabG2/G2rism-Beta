namespace G2rismBeta.API.DTOs.Factura;

/// <summary>
/// DTO de respuesta para factura con toda la información detallada.
/// Incluye propiedades computadas y datos relacionados.
/// </summary>
public class FacturaResponseDto
{
    /// <summary>
    /// ID de la factura
    /// </summary>
    public int IdFactura { get; set; }

    /// <summary>
    /// ID de la reserva asociada
    /// </summary>
    public int IdReserva { get; set; }

    /// <summary>
    /// Número único de la factura (formato: FAC-2025-00001)
    /// </summary>
    public string NumeroFactura { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de emisión de la factura
    /// </summary>
    public DateTime FechaEmision { get; set; }

    /// <summary>
    /// Fecha de vencimiento de la factura
    /// </summary>
    public DateTime? FechaVencimiento { get; set; }

    /// <summary>
    /// Resolución DIAN (opcional)
    /// </summary>
    public string? ResolucionDian { get; set; }

    /// <summary>
    /// CUFE/CUDE de facturación electrónica (opcional)
    /// </summary>
    public string? CufeCude { get; set; }

    /// <summary>
    /// Tipo de factura (venta, devolucion)
    /// </summary>
    public string TipoFactura { get; set; } = string.Empty;

    /// <summary>
    /// Estado actual de la factura (pendiente, pagada, cancelada, vencida)
    /// </summary>
    public string Estado { get; set; } = string.Empty;

    /// <summary>
    /// Subtotal de la factura (antes de impuestos y descuentos)
    /// </summary>
    public decimal Subtotal { get; set; }

    /// <summary>
    /// Total de impuestos aplicados
    /// </summary>
    public decimal Impuestos { get; set; }

    /// <summary>
    /// Porcentaje de IVA aplicado
    /// </summary>
    public decimal PorcentajeIva { get; set; }

    /// <summary>
    /// Total de descuentos aplicados
    /// </summary>
    public decimal Descuentos { get; set; }

    /// <summary>
    /// Total final de la factura
    /// </summary>
    public decimal Total { get; set; }

    /// <summary>
    /// Observaciones adicionales
    /// </summary>
    public string? Observaciones { get; set; }

    /// <summary>
    /// Fecha de creación del registro
    /// </summary>
    public DateTime FechaCreacion { get; set; }

    /// <summary>
    /// Fecha de última modificación
    /// </summary>
    public DateTime? FechaModificacion { get; set; }

    #region Propiedades Computadas

    /// <summary>
    /// Indica si la factura está completamente pagada
    /// </summary>
    public bool EstaPagada { get; set; }

    /// <summary>
    /// Indica si la factura está pendiente de pago
    /// </summary>
    public bool EstaPendiente { get; set; }

    /// <summary>
    /// Indica si la factura está cancelada
    /// </summary>
    public bool EstaCancelada { get; set; }

    /// <summary>
    /// Indica si la factura está vencida
    /// </summary>
    public bool EstaVencida { get; set; }

    /// <summary>
    /// Días restantes hasta el vencimiento
    /// </summary>
    public int DiasHastaVencimiento { get; set; }

    /// <summary>
    /// Monto total pagado (suma de pagos aprobados)
    /// </summary>
    public decimal MontoPagado { get; set; }

    /// <summary>
    /// Saldo pendiente de pago
    /// </summary>
    public decimal SaldoPendiente { get; set; }

    /// <summary>
    /// Porcentaje pagado de la factura (0-100)
    /// </summary>
    public decimal PorcentajePagado { get; set; }

    /// <summary>
    /// Indica si tiene pagos parciales
    /// </summary>
    public bool TienePagosParciales { get; set; }

    /// <summary>
    /// Base gravable (subtotal menos descuentos)
    /// </summary>
    public decimal BaseGravable { get; set; }

    #endregion

    #region Información Relacionada (Opcional)

    /// <summary>
    /// Información básica de la reserva asociada (opcional, se carga bajo demanda)
    /// </summary>
    public ReservaBasicInfoDto? Reserva { get; set; }

    /// <summary>
    /// Lista de pagos asociados a la factura (opcional, se carga bajo demanda)
    /// </summary>
    public List<PagoBasicInfoDto>? Pagos { get; set; }

    #endregion
}

/// <summary>
/// DTO con información básica de una reserva (para incluir en FacturaResponseDto)
/// </summary>
public class ReservaBasicInfoDto
{
    public int IdReserva { get; set; }
    public string? Descripcion { get; set; }
    public DateTime FechaInicioViaje { get; set; }
    public DateTime FechaFinViaje { get; set; }
    public int NumeroPasajeros { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string? NombreCliente { get; set; }
}

/// <summary>
/// DTO con información básica de un pago (para incluir en FacturaResponseDto)
/// </summary>
public class PagoBasicInfoDto
{
    public int IdPago { get; set; }
    public decimal Monto { get; set; }
    public DateTime FechaPago { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string? FormaDePago { get; set; }
    public string? ReferenciaTransaccion { get; set; }
}