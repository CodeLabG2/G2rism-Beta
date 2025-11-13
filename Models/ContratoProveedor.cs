using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace G2rismBeta.API.Models;

/// <summary>
/// Entidad que representa un contrato con un proveedor
/// Gestiona acuerdos comerciales, vigencias y condiciones
/// </summary>
[Table("contratos_proveedor")]
public class ContratoProveedor
{
    #region Propiedades Principales

    /// <summary>
    /// Identificador único del contrato
    /// </summary>
    [Key]
    [Column("id_contrato")]
    public int IdContrato { get; set; }

    /// <summary>
    /// ID del proveedor asociado al contrato
    /// </summary>
    [Required(ErrorMessage = "El ID del proveedor es obligatorio")]
    [Column("id_proveedor")]
    public int IdProveedor { get; set; }

    /// <summary>
    /// Número único del contrato para identificación
    /// </summary>
    [Required(ErrorMessage = "El número de contrato es obligatorio")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El número de contrato debe tener entre 3 y 50 caracteres")]
    [Column("numero_contrato")]
    public string NumeroContrato { get; set; } = string.Empty;

    #endregion

    #region Vigencia del Contrato

    /// <summary>
    /// Fecha de inicio de vigencia del contrato
    /// </summary>
    [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
    [Column("fecha_inicio")]
    public DateTime FechaInicio { get; set; }

    /// <summary>
    /// Fecha de finalización del contrato
    /// </summary>
    [Required(ErrorMessage = "La fecha de fin es obligatoria")]
    [Column("fecha_fin")]
    public DateTime FechaFin { get; set; }

    #endregion

    #region Información del Contrato

    /// <summary>
    /// Tipo de contrato (ej: Servicios, Suministro, Exclusividad, etc.)
    /// </summary>
    [Required(ErrorMessage = "El tipo de contrato es obligatorio")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El tipo de contrato debe tener entre 3 y 50 caracteres")]
    [Column("tipo_contrato")]
    public string TipoContrato { get; set; } = string.Empty;

    /// <summary>
    /// Valor total del contrato
    /// </summary>
    [Required(ErrorMessage = "El valor del contrato es obligatorio")]
    [Range(0, double.MaxValue, ErrorMessage = "El valor del contrato debe ser mayor o igual a 0")]
    [Column("valor_contrato", TypeName = "decimal(12,2)")]
    public decimal ValorContrato { get; set; }

    /// <summary>
    /// Condiciones de pago acordadas
    /// </summary>
    [Required(ErrorMessage = "Las condiciones de pago son obligatorias")]
    [Column("condiciones_pago", TypeName = "text")]
    public string CondicionesPago { get; set; } = string.Empty;

    /// <summary>
    /// Términos y condiciones del contrato
    /// </summary>
    [Required(ErrorMessage = "Los términos del contrato son obligatorios")]
    [Column("terminos", TypeName = "text")]
    public string Terminos { get; set; } = string.Empty;

    /// <summary>
    /// Indica si el contrato se renueva automáticamente al vencer
    /// </summary>
    [Column("renovacion_automatica")]
    public bool RenovacionAutomatica { get; set; } = false;

    #endregion

    #region Estado y Control

    /// <summary>
    /// Estado actual del contrato
    /// Valores esperados: 'Vigente', 'Vencido', 'Cancelado', 'En_Negociacion'
    /// </summary>
    [Required(ErrorMessage = "El estado es obligatorio")]
    [StringLength(20, ErrorMessage = "El estado no puede exceder 20 caracteres")]
    [Column("estado")]
    public string Estado { get; set; } = "En_Negociacion";

    /// <summary>
    /// URL del archivo del contrato digitalizado (opcional)
    /// </summary>
    [StringLength(500, ErrorMessage = "La URL del archivo no puede exceder 500 caracteres")]
    [Column("archivo_contrato")]
    public string? ArchivoContrato { get; set; }

    /// <summary>
    /// Observaciones o notas adicionales sobre el contrato
    /// </summary>
    [Column("observaciones", TypeName = "text")]
    public string? Observaciones { get; set; }

    /// <summary>
    /// Fecha de creación del registro
    /// </summary>
    [Column("fecha_creacion")]
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    #endregion

    #region Propiedades Calculadas

    /// <summary>
    /// Indica si el contrato está vigente actualmente
    /// </summary>
    [NotMapped]
    public bool EstaVigente
    {
        get
        {
            var hoy = DateTime.Now.Date;
            return Estado == "Vigente" &&
                   FechaInicio.Date <= hoy &&
                   FechaFin.Date >= hoy;
        }
    }

    /// <summary>
    /// Días restantes hasta el vencimiento del contrato
    /// </summary>
    [NotMapped]
    public int DiasRestantes
    {
        get
        {
            var hoy = DateTime.Now.Date;
            if (FechaFin.Date < hoy)
                return 0;

            return (FechaFin.Date - hoy).Days;
        }
    }

    /// <summary>
    /// Indica si el contrato está próximo a vencer (menos de 30 días)
    /// </summary>
    [NotMapped]
    public bool ProximoAVencer
    {
        get
        {
            return EstaVigente && DiasRestantes <= 30 && DiasRestantes > 0;
        }
    }

    /// <summary>
    /// Duración total del contrato en días
    /// </summary>
    [NotMapped]
    public int DuracionDias
    {
        get
        {
            return (FechaFin.Date - FechaInicio.Date).Days;
        }
    }

    #endregion

    #region Propiedades de Navegación

    /// <summary>
    /// Proveedor asociado al contrato
    /// </summary>
    [ForeignKey("IdProveedor")]
    public virtual Proveedor? Proveedor { get; set; }

    #endregion
}
