using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace G2rismBeta.API.Models;

/// <summary>
/// Entidad que representa un Cliente en el sistema
/// Un cliente es un usuario del tipo 'cliente' con información extendida
/// </summary>
[Table("clientes")]
public class Cliente
{
    /// <summary>
    /// Identificador único del cliente
    /// </summary>
    [Key]
    [Column("id_cliente")]
    public int IdCliente { get; set; }

    /// <summary>
    /// ID del usuario asociado (relación 1:1 con Usuarios)
    /// </summary>
    [Required(ErrorMessage = "El ID de usuario es obligatorio")]
    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    /// <summary>
    /// ID de la categoría a la que pertenece el cliente (opcional)
    /// </summary>
    [Column("id_categoria")]
    public int? IdCategoria { get; set; }

    /// <summary>
    /// Nombre del cliente
    /// </summary>
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Apellido del cliente
    /// </summary>
    [Required(ErrorMessage = "El apellido es obligatorio")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 100 caracteres")]
    [Column("apellido")]
    public string Apellido { get; set; } = string.Empty;

    /// <summary>
    /// Documento de identidad único (cédula, pasaporte, etc.)
    /// </summary>
    [Required(ErrorMessage = "El documento de identidad es obligatorio")]
    [StringLength(50, ErrorMessage = "El documento no puede exceder 50 caracteres")]
    [Column("documento_identidad")]
    public string DocumentoIdentidad { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de documento de identidad
    /// Ejemplos: CC (Cédula), CE (Cédula Extranjería), PA (Pasaporte), TI (Tarjeta Identidad)
    /// </summary>
    [Required(ErrorMessage = "El tipo de documento es obligatorio")]
    [StringLength(10, ErrorMessage = "El tipo de documento no puede exceder 10 caracteres")]
    [Column("tipo_documento")]
    public string TipoDocumento { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de nacimiento del cliente
    /// </summary>
    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
    [Column("fecha_nacimiento")]
    public DateTime FechaNacimiento { get; set; }

    /// <summary>
    /// Correo electrónico del cliente
    /// </summary>
    [Required(ErrorMessage = "El correo electrónico es obligatorio")]
    [EmailAddress(ErrorMessage = "Formato de correo electrónico inválido")]
    [StringLength(150, ErrorMessage = "El correo no puede exceder 150 caracteres")]
    [Column("correo_electronico")]
    public string CorreoElectronico { get; set; } = string.Empty;

    /// <summary>
    /// Número de teléfono del cliente
    /// </summary>
    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
    [Column("telefono")]
    public string Telefono { get; set; } = string.Empty;

    /// <summary>
    /// Dirección completa del cliente
    /// </summary>
    [StringLength(200, ErrorMessage = "La dirección no puede exceder 200 caracteres")]
    [Column("direccion")]
    public string? Direccion { get; set; }

    /// <summary>
    /// Ciudad de residencia del cliente
    /// </summary>
    [Required(ErrorMessage = "La ciudad es obligatoria")]
    [StringLength(100, ErrorMessage = "La ciudad no puede exceder 100 caracteres")]
    [Column("ciudad")]
    public string Ciudad { get; set; } = string.Empty;

    /// <summary>
    /// País de residencia del cliente
    /// </summary>
    [Required(ErrorMessage = "El país es obligatorio")]
    [StringLength(100, ErrorMessage = "El país no puede exceder 100 caracteres")]
    [Column("pais")]
    public string Pais { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de registro del cliente en el sistema
    /// </summary>
    [Column("fecha_registro")]
    public DateTime FechaRegistro { get; set; } = DateTime.Now;

    /// <summary>
    /// Estado del cliente (true = activo, false = inactivo)
    /// </summary>
    [Column("estado")]
    public bool Estado { get; set; } = true;

    // ========================================
    // RELACIONES DE NAVEGACIÓN
    // ========================================

    /// <summary>
    /// Usuario asociado a este cliente (relación 1:1)
    /// </summary>
    public virtual Usuario? Usuario { get; set; }

    /// <summary>
    /// Categoría a la que pertenece el cliente (relación N:1)
    /// </summary>
    public virtual CategoriaCliente? Categoria { get; set; }

    /// <summary>
    /// Preferencias del cliente (relación 1:1)
    /// Un cliente puede tener una preferencia
    /// </summary>
    public virtual PreferenciaCliente? Preferencia { get; set; }

    // ========================================
    // PROPIEDADES CALCULADAS
    // ========================================

    /// <summary>
    /// Nombre completo del cliente (solo lectura)
    /// </summary>
    [NotMapped]
    public string NombreCompleto => $"{Nombre} {Apellido}";

    /// <summary>
    /// Edad del cliente calculada a partir de la fecha de nacimiento (solo lectura)
    /// </summary>
    [NotMapped]
    public int Edad
    {
        get
        {
            var hoy = DateTime.Today;
            var edad = hoy.Year - FechaNacimiento.Year;
            if (FechaNacimiento.Date > hoy.AddYears(-edad)) edad--;
            return edad;
        }
    }
}