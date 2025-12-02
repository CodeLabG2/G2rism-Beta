using System.ComponentModel.DataAnnotations;

namespace G2rismBeta.API.DTOs.Cliente;

/// <summary>
/// DTO para crear un nuevo cliente
/// Solo incluye los campos que el cliente puede enviar
/// </summary>
public class ClienteCreateDto
{
    /// <summary>
    /// ID del usuario asociado
    /// </summary>
    /// <example>5</example>
    [Required(ErrorMessage = "El ID de usuario es obligatorio")]
    public int IdUsuario { get; set; }

    /// <summary>
    /// ID de la categoría del cliente (opcional)
    /// Si no se especifica, se puede asignar una por defecto
    /// </summary>
    /// <example>2</example>
    public int? IdCategoria { get; set; }

    /// <summary>
    /// Nombre del cliente
    /// </summary>
    /// <example>Juan</example>
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Apellido del cliente
    /// </summary>
    /// <example>Pérez García</example>
    [Required(ErrorMessage = "El apellido es obligatorio")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 100 caracteres")]
    public string Apellido { get; set; } = string.Empty;

    /// <summary>
    /// Documento de identidad único
    /// </summary>
    /// <example>1234567890</example>
    [Required(ErrorMessage = "El documento de identidad es obligatorio")]
    [StringLength(50, ErrorMessage = "El documento no puede exceder 50 caracteres")]
    public string DocumentoIdentidad { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de documento (CC, CE, PA, TI)
    /// </summary>
    /// <example>CC</example>
    [Required(ErrorMessage = "El tipo de documento es obligatorio")]
    [StringLength(10, ErrorMessage = "El tipo de documento no puede exceder 10 caracteres")]
    public string TipoDocumento { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de nacimiento (formato: YYYY-MM-DD)
    /// </summary>
    /// <example>1990-05-15</example>
    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
    [DataType(DataType.Date)]
    public DateTime FechaNacimiento { get; set; }

    /// <summary>
    /// Correo electrónico
    /// </summary>
    /// <example>juan.perez@example.com</example>
    [Required(ErrorMessage = "El correo electrónico es obligatorio")]
    [EmailAddress(ErrorMessage = "Formato de correo electrónico inválido")]
    [StringLength(150, ErrorMessage = "El correo no puede exceder 150 caracteres")]
    public string CorreoElectronico { get; set; } = string.Empty;

    /// <summary>
    /// Teléfono
    /// </summary>
    /// <example>+57 300 123 4567</example>
    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
    public string Telefono { get; set; } = string.Empty;

    /// <summary>
    /// Dirección (opcional)
    /// </summary>
    /// <example>Calle 123 #45-67</example>
    [StringLength(200, ErrorMessage = "La dirección no puede exceder 200 caracteres")]
    public string? Direccion { get; set; }

    /// <summary>
    /// Ciudad
    /// </summary>
    /// <example>Medellín</example>
    [Required(ErrorMessage = "La ciudad es obligatoria")]
    [StringLength(100, ErrorMessage = "La ciudad no puede exceder 100 caracteres")]
    public string Ciudad { get; set; } = string.Empty;

    /// <summary>
    /// País
    /// </summary>
    /// <example>Colombia</example>
    [Required(ErrorMessage = "El país es obligatorio")]
    [StringLength(100, ErrorMessage = "El país no puede exceder 100 caracteres")]
    public string Pais { get; set; } = string.Empty;

    /// <summary>
    /// Estado inicial del cliente
    /// </summary>
    /// <example>true</example>
    public bool Estado { get; set; } = true;
}