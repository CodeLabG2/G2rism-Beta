using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace G2rismBeta.API.Models;

/// <summary>
/// Entidad que representa un proveedor de servicios turísticos
/// (Hoteles, Aerolíneas, Transporte, Servicios adicionales, etc.)
/// </summary>
[Table("proveedores")]
public class Proveedor
{
    #region Propiedades Principales

    /// <summary>
    /// Identificador único del proveedor
    /// </summary>
    [Key]
    [Column("id_proveedor")]
    public int IdProveedor { get; set; }

    /// <summary>
    /// Nombre o razón social de la empresa proveedora
    /// </summary>
    [Required(ErrorMessage = "El nombre de la empresa es obligatorio")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
    [Column("nombre_empresa")]
    public string NombreEmpresa { get; set; } = string.Empty;

    /// <summary>
    /// Nombre de la persona de contacto principal
    /// </summary>
    [Required(ErrorMessage = "El nombre del contacto es obligatorio")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre del contacto debe tener entre 3 y 100 caracteres")]
    [Column("nombre_contacto")]
    public string NombreContacto { get; set; } = string.Empty;

    #endregion

    #region Información de Contacto

    /// <summary>
    /// Teléfono principal del proveedor
    /// </summary>
    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [StringLength(20, MinimumLength = 7, ErrorMessage = "El teléfono debe tener entre 7 y 20 caracteres")]
    [Column("telefono")]
    public string Telefono { get; set; } = string.Empty;

    /// <summary>
    /// Teléfono alternativo del proveedor (opcional)
    /// </summary>
    [StringLength(20, MinimumLength = 7, ErrorMessage = "El teléfono alternativo debe tener entre 7 y 20 caracteres")]
    [Column("telefono_alternativo")]
    public string? TelefonoAlternativo { get; set; }

    /// <summary>
    /// Correo electrónico principal
    /// </summary>
    [Required(ErrorMessage = "El correo electrónico es obligatorio")]
    [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
    [StringLength(100, ErrorMessage = "El correo no puede exceder 100 caracteres")]
    [Column("correo_electronico")]
    public string CorreoElectronico { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico alternativo (opcional)
    /// </summary>
    [EmailAddress(ErrorMessage = "El formato del correo alternativo no es válido")]
    [StringLength(100, ErrorMessage = "El correo alternativo no puede exceder 100 caracteres")]
    [Column("correo_alternativo")]
    public string? CorreoAlternativo { get; set; }

    #endregion

    #region Información de Ubicación

    /// <summary>
    /// Dirección física del proveedor (opcional)
    /// </summary>
    [StringLength(200, ErrorMessage = "La dirección no puede exceder 200 caracteres")]
    [Column("direccion")]
    public string? Direccion { get; set; }

    /// <summary>
    /// Ciudad donde se encuentra el proveedor
    /// </summary>
    [Required(ErrorMessage = "La ciudad es obligatoria")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "La ciudad debe tener entre 2 y 50 caracteres")]
    [Column("ciudad")]
    public string Ciudad { get; set; } = string.Empty;

    /// <summary>
    /// País del proveedor
    /// </summary>
    [Required(ErrorMessage = "El país es obligatorio")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "El país debe tener entre 2 y 50 caracteres")]
    [Column("pais")]
    public string Pais { get; set; } = string.Empty;

    #endregion

    #region Información Legal y Comercial

    /// <summary>
    /// NIT (Colombia) o RUT (otros países) - Identificación tributaria única
    /// </summary>
    [Required(ErrorMessage = "El NIT/RUT es obligatorio")]
    [StringLength(20, MinimumLength = 5, ErrorMessage = "El NIT/RUT debe tener entre 5 y 20 caracteres")]
    [Column("nit_rut")]
    public string NitRut { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de proveedor según los servicios que ofrece
    /// </summary>
    [Required(ErrorMessage = "El tipo de proveedor es obligatorio")]
    [StringLength(20, ErrorMessage = "El tipo de proveedor no puede exceder 20 caracteres")]
    [Column("tipo_proveedor")]
    public string TipoProveedor { get; set; } = string.Empty;
    // Valores esperados: 'Hotel', 'Aerolinea', 'Transporte', 'Servicios', 'Mixto'

    /// <summary>
    /// Sitio web del proveedor (opcional)
    /// </summary>
    [Url(ErrorMessage = "El formato de la URL no es válido")]
    [StringLength(200, ErrorMessage = "La URL del sitio web no puede exceder 200 caracteres")]
    [Column("sitio_web")]
    public string? SitioWeb { get; set; }

    #endregion

    #region Evaluación y Estado

    /// <summary>
    /// Calificación del proveedor (0.0 a 5.0 estrellas)
    /// </summary>
    [Range(0.0, 5.0, ErrorMessage = "La calificación debe estar entre 0.0 y 5.0")]
    [Column("calificacion", TypeName = "decimal(2,1)")]
    public decimal Calificacion { get; set; } = 0.0m;

    /// <summary>
    /// Estado actual del proveedor
    /// </summary>
    [Required(ErrorMessage = "El estado es obligatorio")]
    [StringLength(15, ErrorMessage = "El estado no puede exceder 15 caracteres")]
    [Column("estado")]
    public string Estado { get; set; } = "Activo";
    // Valores esperados: 'Activo', 'Inactivo', 'Suspendido'

    /// <summary>
    /// Fecha de registro del proveedor en el sistema
    /// </summary>
    [Column("fecha_registro")]
    public DateTime FechaRegistro { get; set; } = DateTime.Now;

    /// <summary>
    /// Observaciones o notas adicionales sobre el proveedor
    /// </summary>
    [Column("observaciones", TypeName = "text")]
    public string? Observaciones { get; set; }

    #endregion

    #region Propiedades de Navegación

    /// <summary>
    /// Colección de contratos asociados a este proveedor
    /// </summary>
    public virtual ICollection<ContratoProveedor>? Contratos { get; set; }

    #endregion
}
