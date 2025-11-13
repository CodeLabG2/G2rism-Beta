using System.ComponentModel.DataAnnotations;

namespace G2rismBeta.API.DTOs.Empleado;

/// <summary>
/// DTO para crear un nuevo empleado en el sistema.
/// Incluye todas las propiedades necesarias para el registro inicial.
/// </summary>
public class EmpleadoCreateDto
{
    #region Foreign Keys

    /// <summary>
    /// ID del usuario asociado al empleado (para autenticación y permisos)
    /// </summary>
    /// <example>5</example>
    [Required(ErrorMessage = "El ID de usuario es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "El ID de usuario debe ser mayor a 0")]
    public int IdUsuario { get; set; }

    /// <summary>
    /// ID del jefe directo del empleado (opcional)
    /// Null si no tiene jefe (ej. CEO, Gerente General)
    /// </summary>
    /// <example>2</example>
    public int? IdJefe { get; set; }

    #endregion

    #region Personal Information

    /// <summary>
    /// Nombre del empleado
    /// </summary>
    /// <example>Carlos</example>
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Apellido del empleado
    /// </summary>
    /// <example>Rodríguez</example>
    [Required(ErrorMessage = "El apellido es obligatorio")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 100 caracteres")]
    public string Apellido { get; set; } = string.Empty;

    /// <summary>
    /// Número de documento de identidad (debe ser único)
    /// </summary>
    /// <example>1234567890</example>
    [Required(ErrorMessage = "El documento de identidad es obligatorio")]
    [StringLength(20, ErrorMessage = "El documento no puede exceder 20 caracteres")]
    [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "El documento solo puede contener letras y números")]
    public string DocumentoIdentidad { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de documento de identidad
    /// Valores permitidos: CC, CE, Pasaporte, NIT
    /// </summary>
    /// <example>CC</example>
    [Required(ErrorMessage = "El tipo de documento es obligatorio")]
    [RegularExpression(@"^(CC|CE|Pasaporte|NIT)$",
        ErrorMessage = "El tipo de documento debe ser: CC, CE, Pasaporte o NIT")]
    public string TipoDocumento { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de nacimiento del empleado
    /// Debe ser mayor de edad (18 años) y menor de 100 años
    /// </summary>
    /// <example>1990-05-15</example>
    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
    public DateTime FechaNacimiento { get; set; }

    #endregion

    #region Contact Information

    /// <summary>
    /// Correo electrónico corporativo del empleado
    /// </summary>
    /// <example>carlos.rodriguez@g2rism.com</example>
    [Required(ErrorMessage = "El correo electrónico es obligatorio")]
    [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
    [StringLength(100, ErrorMessage = "El correo no puede exceder 100 caracteres")]
    public string CorreoElectronico { get; set; } = string.Empty;

    /// <summary>
    /// Teléfono de contacto del empleado
    /// Formato: Puede incluir código de país y extensiones
    /// </summary>
    /// <example>+57 300 1234567</example>
    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
    [RegularExpression(@"^[\d\s\+\-\(\)]+$",
        ErrorMessage = "El teléfono solo puede contener números, espacios y los caracteres: + - ( )")]
    public string Telefono { get; set; } = string.Empty;

    #endregion

    #region Employment Information

    /// <summary>
    /// Cargo o posición del empleado en la empresa
    /// </summary>
    /// <example>Asesor de Viajes</example>
    [Required(ErrorMessage = "El cargo es obligatorio")]
    [StringLength(100, ErrorMessage = "El cargo no puede exceder 100 caracteres")]
    public string Cargo { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de ingreso a la empresa
    /// No puede ser fecha futura
    /// </summary>
    /// <example>2024-01-15</example>
    [Required(ErrorMessage = "La fecha de ingreso es obligatoria")]
    public DateTime FechaIngreso { get; set; }

    /// <summary>
    /// Salario mensual del empleado (información sensible)
    /// </summary>
    /// <example>2500000.00</example>
    [Required(ErrorMessage = "El salario es obligatorio")]
    [Range(0.01, 999999999.99, ErrorMessage = "El salario debe estar entre 0.01 y 999,999,999.99")]
    public decimal Salario { get; set; }

    #endregion

    #region Status

    /// <summary>
    /// Estado del empleado
    /// Valores permitidos: Activo, Inactivo, Vacaciones, Licencia
    /// Por defecto: Activo
    /// </summary>
    /// <example>Activo</example>
    [StringLength(20, ErrorMessage = "El estado no puede exceder 20 caracteres")]
    [RegularExpression(@"^(Activo|Inactivo|Vacaciones|Licencia)$",
        ErrorMessage = "El estado debe ser: Activo, Inactivo, Vacaciones o Licencia")]
    public string Estado { get; set; } = "Activo";

    #endregion
}
