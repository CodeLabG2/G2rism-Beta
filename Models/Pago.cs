using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace G2rismBeta.API.Models;

/// <summary>
/// Entidad que representa un Pago realizado en el sistema.
/// Los pagos se asocian a facturas y pueden ser parciales o totales.
/// </summary>
[Table("pagos")]
public class Pago
{
    #region Primary Key

    /// <summary>
    /// Identificador único del pago
    /// </summary>
    [Key]
    [Column("id_pago")]
    public int IdPago { get; set; }

    #endregion

    #region Foreign Keys

    /// <summary>
    /// ID de la factura a la que corresponde este pago
    /// </summary>
    [Required(ErrorMessage = "La factura es obligatoria")]
    [Column("id_factura")]
    public int IdFactura { get; set; }

    /// <summary>
    /// ID de la forma de pago utilizada
    /// </summary>
    [Required(ErrorMessage = "La forma de pago es obligatoria")]
    [Column("id_forma_pago")]
    public int IdFormaPago { get; set; }

    #endregion

    #region Payment Information

    /// <summary>
    /// Monto del pago
    /// </summary>
    [Required(ErrorMessage = "El monto es obligatorio")]
    [Column("monto", TypeName = "DECIMAL(10,2)")]
    [Range(0.01, 999999999.99, ErrorMessage = "El monto debe ser mayor a 0")]
    public decimal Monto { get; set; }

    /// <summary>
    /// Fecha y hora en que se realizó el pago
    /// </summary>
    [Required(ErrorMessage = "La fecha de pago es obligatoria")]
    [Column("fecha_pago")]
    public DateTime FechaPago { get; set; } = DateTime.Now;

    /// <summary>
    /// Referencia o código de transacción del pago
    /// Puede ser número de autorización bancaria, número de recibo, etc.
    /// </summary>
    [StringLength(100, ErrorMessage = "La referencia de transacción no puede exceder 100 caracteres")]
    [Column("referencia_transaccion")]
    public string? ReferenciaTransaccion { get; set; }

    /// <summary>
    /// Comprobante de pago (puede ser URL a archivo o base64)
    /// </summary>
    [StringLength(500, ErrorMessage = "El comprobante de pago no puede exceder 500 caracteres")]
    [Column("comprobante_pago")]
    public string? ComprobantePago { get; set; }

    /// <summary>
    /// Estado del pago
    /// Valores: pendiente, aprobado, rechazado
    /// </summary>
    [Required(ErrorMessage = "El estado del pago es obligatorio")]
    [StringLength(20, ErrorMessage = "El estado no puede exceder 20 caracteres")]
    [Column("estado")]
    public string Estado { get; set; } = "pendiente";

    /// <summary>
    /// Observaciones adicionales sobre el pago
    /// </summary>
    [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder 500 caracteres")]
    [Column("observaciones")]
    public string? Observaciones { get; set; }

    #endregion

    #region Audit Fields

    /// <summary>
    /// Fecha y hora de registro del pago en el sistema
    /// </summary>
    [Required]
    [Column("fecha_creacion")]
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    /// <summary>
    /// Fecha y hora de la última modificación del pago
    /// </summary>
    [Column("fecha_modificacion")]
    public DateTime? FechaModificacion { get; set; }

    #endregion

    #region Navigation Properties

    /// <summary>
    /// Factura a la que pertenece este pago (relación N:1)
    /// </summary>
    [ForeignKey("IdFactura")]
    public virtual Factura? Factura { get; set; }

    /// <summary>
    /// Forma de pago utilizada (relación N:1)
    /// </summary>
    [ForeignKey("IdFormaPago")]
    public virtual FormaDePago? FormaDePago { get; set; }

    #endregion

    #region Computed Properties

    /// <summary>
    /// Indica si el pago está aprobado
    /// </summary>
    [NotMapped]
    public bool EstaAprobado => Estado == "aprobado";

    /// <summary>
    /// Indica si el pago está pendiente
    /// </summary>
    [NotMapped]
    public bool EstaPendiente => Estado == "pendiente";

    /// <summary>
    /// Indica si el pago fue rechazado
    /// </summary>
    [NotMapped]
    public bool EstaRechazado => Estado == "rechazado";

    /// <summary>
    /// Días transcurridos desde el pago
    /// </summary>
    [NotMapped]
    public int DiasDesdeElPago => (DateTime.Today - FechaPago.Date).Days;

    /// <summary>
    /// Indica si el pago tiene comprobante adjunto
    /// </summary>
    [NotMapped]
    public bool TieneComprobante => !string.IsNullOrWhiteSpace(ComprobantePago);

    #endregion
}