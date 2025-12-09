using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace G2rismBeta.API.Models;

/// <summary>
/// Entidad que representa una Factura en el sistema de turismo.
/// Las facturas se generan a partir de las reservas y contienen la información fiscal y contable.
/// </summary>
[Table("facturas")]
public class Factura
{
    #region Primary Key

    /// <summary>
    /// Identificador único de la factura
    /// </summary>
    [Key]
    [Column("id_factura")]
    public int IdFactura { get; set; }

    #endregion

    #region Foreign Keys

    /// <summary>
    /// ID de la reserva asociada a esta factura
    /// </summary>
    [Required(ErrorMessage = "La reserva es obligatoria")]
    [Column("id_reserva")]
    public int IdReserva { get; set; }

    #endregion

    #region Basic Information

    /// <summary>
    /// Número único de la factura (autogenerado)
    /// Formato: FAC-{año}-{consecutivo} (ej: FAC-2025-00001)
    /// </summary>
    [Required(ErrorMessage = "El número de factura es obligatorio")]
    [StringLength(50, ErrorMessage = "El número de factura no puede exceder 50 caracteres")]
    [Column("numero_factura")]
    public string NumeroFactura { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de emisión de la factura
    /// </summary>
    [Required(ErrorMessage = "La fecha de emisión es obligatoria")]
    [Column("fecha_emision", TypeName = "DATE")]
    public DateTime FechaEmision { get; set; } = DateTime.Now;

    /// <summary>
    /// Fecha de vencimiento de la factura (plazo de pago)
    /// </summary>
    [Column("fecha_vencimiento", TypeName = "DATE")]
    public DateTime? FechaVencimiento { get; set; }

    #endregion

    #region DIAN Information (Optional for MVP)

    /// <summary>
    /// Resolución de la DIAN (opcional en MVP)
    /// En producción debe contener la resolución de facturación electrónica
    /// </summary>
    [StringLength(100, ErrorMessage = "La resolución DIAN no puede exceder 100 caracteres")]
    [Column("resolucion_dian")]
    public string? ResolucionDian { get; set; }

    /// <summary>
    /// CUFE (Código Único de Factura Electrónica) o CUDE (opcional en MVP)
    /// En producción debe generarse según normativa DIAN
    /// </summary>
    [StringLength(200, ErrorMessage = "El CUFE/CUDE no puede exceder 200 caracteres")]
    [Column("cufe_cude")]
    public string? CufeCude { get; set; }

    #endregion

    #region Financial Information

    /// <summary>
    /// Tipo de factura
    /// Valores: venta, devolucion
    /// </summary>
    [Required(ErrorMessage = "El tipo de factura es obligatorio")]
    [StringLength(20, ErrorMessage = "El tipo de factura no puede exceder 20 caracteres")]
    [Column("tipo_factura")]
    public string TipoFactura { get; set; } = "venta";

    /// <summary>
    /// Estado actual de la factura
    /// Valores: pendiente, pagada, cancelada, vencida
    /// </summary>
    [Required(ErrorMessage = "El estado es obligatorio")]
    [StringLength(20, ErrorMessage = "El estado no puede exceder 20 caracteres")]
    [Column("estado")]
    public string Estado { get; set; } = "pendiente";

    /// <summary>
    /// Subtotal de la factura (antes de impuestos y descuentos)
    /// </summary>
    [Required]
    [Column("subtotal", TypeName = "DECIMAL(10,2)")]
    [Range(0, 999999999.99, ErrorMessage = "El subtotal debe ser positivo")]
    public decimal Subtotal { get; set; } = 0;

    /// <summary>
    /// Total de impuestos aplicados (principalmente IVA)
    /// </summary>
    [Required]
    [Column("impuestos", TypeName = "DECIMAL(10,2)")]
    [Range(0, 999999999.99, ErrorMessage = "Los impuestos deben ser positivos")]
    public decimal Impuestos { get; set; } = 0;

    /// <summary>
    /// Porcentaje de IVA aplicado (19% por defecto en Colombia)
    /// </summary>
    [Required]
    [Column("porcentaje_iva", TypeName = "DECIMAL(5,2)")]
    [Range(0, 100, ErrorMessage = "El porcentaje de IVA debe estar entre 0 y 100")]
    public decimal PorcentajeIva { get; set; } = 19;

    /// <summary>
    /// Total de descuentos aplicados (por ejemplo, descuentos de categoría de cliente)
    /// </summary>
    [Required]
    [Column("descuentos", TypeName = "DECIMAL(10,2)")]
    [Range(0, 999999999.99, ErrorMessage = "Los descuentos deben ser positivos")]
    public decimal Descuentos { get; set; } = 0;

    /// <summary>
    /// Total final de la factura (Subtotal + Impuestos - Descuentos)
    /// </summary>
    [Required]
    [Column("total", TypeName = "DECIMAL(10,2)")]
    [Range(0, 999999999.99, ErrorMessage = "El total debe ser positivo")]
    public decimal Total { get; set; } = 0;

    #endregion

    #region Observations

    /// <summary>
    /// Observaciones adicionales sobre la factura
    /// </summary>
    [StringLength(1000, ErrorMessage = "Las observaciones no pueden exceder 1000 caracteres")]
    [Column("observaciones")]
    public string? Observaciones { get; set; }

    #endregion

    #region Audit Fields

    /// <summary>
    /// Fecha y hora de creación de la factura
    /// </summary>
    [Required]
    [Column("fecha_creacion")]
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    /// <summary>
    /// Fecha y hora de la última modificación de la factura
    /// </summary>
    [Column("fecha_modificacion")]
    public DateTime? FechaModificacion { get; set; }

    #endregion

    #region Navigation Properties

    /// <summary>
    /// Reserva asociada a esta factura (relación N:1)
    /// Una factura pertenece a una sola reserva
    /// </summary>
    [ForeignKey("IdReserva")]
    public virtual Reserva? Reserva { get; set; }

    /// <summary>
    /// Pagos asociados a esta factura (relación 1:N)
    /// Una factura puede tener múltiples pagos (abonos parciales)
    /// </summary>
    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    #endregion

    #region Computed Properties

    /// <summary>
    /// Indica si la factura está completamente pagada
    /// </summary>
    [NotMapped]
    public bool EstaPagada => Estado == "pagada";

    /// <summary>
    /// Indica si la factura está pendiente de pago
    /// </summary>
    [NotMapped]
    public bool EstaPendiente => Estado == "pendiente";

    /// <summary>
    /// Indica si la factura está cancelada
    /// </summary>
    [NotMapped]
    public bool EstaCancelada => Estado == "cancelada";

    /// <summary>
    /// Indica si la factura está vencida
    /// </summary>
    [NotMapped]
    public bool EstaVencida
    {
        get
        {
            if (Estado == "pagada" || Estado == "cancelada") return false;
            if (!FechaVencimiento.HasValue) return false;
            return DateTime.Today > FechaVencimiento.Value && Estado != "pagada";
        }
    }

    /// <summary>
    /// Días restantes hasta el vencimiento (0 si ya venció o no tiene fecha)
    /// </summary>
    [NotMapped]
    public int DiasHastaVencimiento
    {
        get
        {
            if (!FechaVencimiento.HasValue) return 0;
            if (Estado == "pagada" || Estado == "cancelada") return 0;

            var dias = (FechaVencimiento.Value - DateTime.Today).Days;
            return dias < 0 ? 0 : dias;
        }
    }

    /// <summary>
    /// Monto total pagado (suma de todos los pagos aprobados)
    /// </summary>
    [NotMapped]
    public decimal MontoPagado
    {
        get
        {
            if (Pagos == null || !Pagos.Any()) return 0;
            return Pagos.Where(p => p.Estado == "aprobado").Sum(p => p.Monto);
        }
    }

    /// <summary>
    /// Saldo pendiente de pago
    /// </summary>
    [NotMapped]
    public decimal SaldoPendiente => Total - MontoPagado;

    /// <summary>
    /// Porcentaje pagado de la factura (0-100)
    /// </summary>
    [NotMapped]
    public decimal PorcentajePagado
    {
        get
        {
            if (Total == 0) return 0;
            return Math.Round((MontoPagado / Total) * 100, 2);
        }
    }

    /// <summary>
    /// Indica si la factura tiene pagos parciales
    /// </summary>
    [NotMapped]
    public bool TienePagosParciales => MontoPagado > 0 && MontoPagado < Total;

    /// <summary>
    /// Base gravable (subtotal menos descuentos, antes de impuestos)
    /// </summary>
    [NotMapped]
    public decimal BaseGravable => Subtotal - Descuentos;

    #endregion
}
