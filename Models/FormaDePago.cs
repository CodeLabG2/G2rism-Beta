using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace G2rismBeta.API.Models;

/// <summary>
/// Entidad que representa una Forma de Pago disponible en el sistema.
/// Define los métodos de pago aceptados (efectivo, tarjetas, transferencias, etc.)
/// </summary>
[Table("formas_de_pago")]
public class FormaDePago
{
    #region Primary Key

    /// <summary>
    /// Identificador único de la forma de pago
    /// </summary>
    [Key]
    [Column("id_forma_pago")]
    public int IdFormaPago { get; set; }

    #endregion

    #region Basic Information

    /// <summary>
    /// Método de pago
    /// Valores: efectivo, tarjeta_credito, tarjeta_debito, transferencia, pse, nequi, daviplata, etc.
    /// </summary>
    [Required(ErrorMessage = "El método de pago es obligatorio")]
    [StringLength(50, ErrorMessage = "El método de pago no puede exceder 50 caracteres")]
    [Column("metodo")]
    public string Metodo { get; set; } = string.Empty;

    /// <summary>
    /// Descripción adicional del método de pago
    /// </summary>
    [StringLength(200, ErrorMessage = "La descripción no puede exceder 200 caracteres")]
    [Column("descripcion")]
    public string? Descripcion { get; set; }

    /// <summary>
    /// Indica si el método de pago requiere verificación externa
    /// Ejemplo: Tarjetas requieren verificación bancaria, efectivo no
    /// </summary>
    [Required]
    [Column("requiere_verificacion")]
    public bool RequiereVerificacion { get; set; } = false;

    /// <summary>
    /// Indica si el método de pago está activo y disponible
    /// </summary>
    [Required]
    [Column("activo")]
    public bool Activo { get; set; } = true;

    #endregion

    #region Audit Fields

    /// <summary>
    /// Fecha de creación del registro
    /// </summary>
    [Required]
    [Column("fecha_creacion")]
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    /// <summary>
    /// Fecha de última modificación del registro
    /// </summary>
    [Column("fecha_modificacion")]
    public DateTime? FechaModificacion { get; set; }

    #endregion

    #region Navigation Properties

    /// <summary>
    /// Pagos realizados con esta forma de pago (relación 1:N)
    /// </summary>
    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    #endregion

    #region Computed Properties

    /// <summary>
    /// Indica si es un método de pago electrónico
    /// </summary>
    [NotMapped]
    public bool EsMetodoElectronico
    {
        get
        {
            var metodosElectronicos = new[] { "tarjeta_credito", "tarjeta_debito", "transferencia", "pse", "nequi", "daviplata" };
            return metodosElectronicos.Contains(Metodo.ToLower());
        }
    }

    /// <summary>
    /// Indica si es pago en efectivo
    /// </summary>
    [NotMapped]
    public bool EsEfectivo => Metodo.ToLower() == "efectivo";

    #endregion
}
