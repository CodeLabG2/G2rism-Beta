using System.ComponentModel.DataAnnotations;

namespace G2rismBeta.API.DTOs.Empleado;

/// <summary>
/// DTO para actualizar la información de un empleado existente.
/// Todas las propiedades son opcionales para permitir actualizaciones parciales.
/// </summary>
public class EmpleadoUpdateDto
{
    #region Foreign Keys

    /// <summary>
    /// ID del jefe directo del empleado (opcional)
    /// Null si no tiene jefe o si no se desea cambiar
    /// </summary>
    /// <example>2</example>
    public int? IdJefe { get; set; }

    /// <summary>
    /// ID del usuario asociado al empleado (opcional)
    /// Null si no se desea cambiar la asociación
    /// </summary>
    /// <example>5</example>
    public int? IdUsuario { get; set; }

    #endregion

    #region Personal Information

    /// <summary>
    /// Nombre del empleado (opcional)
    /// </summary>
    /// <example>Carlos Alberto</example>
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
    public string? Nombre { get; set; }

    /// <summary>
    /// Apellido del empleado (opcional)
    /// </summary>
    /// <example>Rodríguez Pérez</example>
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 100 caracteres")]
    public string? Apellido { get; set; }

    /// <summary>
    /// Número de documento de identidad (opcional)
    /// Solo actualizar si hubo un error en el registro o cambio legal
    /// </summary>
    /// <example>1234567890</example>
    [StringLength(20, ErrorMessage = "El documento no puede exceder 20 caracteres")]
    [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "El documento solo puede contener letras y números")]
    public string? DocumentoIdentidad { get; set; }

    /// <summary>
    /// Tipo de documento de identidad (opcional)
    /// Valores permitidos: CC, CE, Pasaporte, NIT
    /// </summary>
    /// <example>CE</example>
    [RegularExpression(@"^(CC|CE|Pasaporte|NIT)$",
        ErrorMessage = "El tipo de documento debe ser: CC, CE, Pasaporte o NIT")]
    public string? TipoDocumento { get; set; }

    /// <summary>
    /// Fecha de nacimiento del empleado (opcional)
    /// Solo actualizar si hubo un error en el registro
    /// </summary>
    /// <example>1990-05-15</example>
    public DateTime? FechaNacimiento { get; set; }

    #endregion

    #region Contact Information

    /// <summary>
    /// Correo electrónico corporativo del empleado (opcional)
    /// </summary>
    /// <example>carlos.rodriguez.nuevo@g2rism.com</example>
    [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
    [StringLength(100, ErrorMessage = "El correo no puede exceder 100 caracteres")]
    public string? CorreoElectronico { get; set; }

    /// <summary>
    /// Teléfono de contacto del empleado (opcional)
    /// </summary>
    /// <example>+57 301 9876543</example>
    [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
    [RegularExpression(@"^[\d\s\+\-\(\)]+$",
        ErrorMessage = "El teléfono solo puede contener números, espacios y los caracteres: + - ( )")]
    public string? Telefono { get; set; }

    #endregion

    #region Employment Information

    /// <summary>
    /// Cargo o posición del empleado en la empresa (opcional)
    /// Se actualiza en caso de promoción o cambio de rol
    /// </summary>
    /// <example>Gerente de Ventas</example>
    [StringLength(100, ErrorMessage = "El cargo no puede exceder 100 caracteres")]
    public string? Cargo { get; set; }

    /// <summary>
    /// Fecha de ingreso a la empresa (opcional)
    /// Solo actualizar si hubo un error en el registro
    /// </summary>
    /// <example>2024-01-15</example>
    public DateTime? FechaIngreso { get; set; }

    /// <summary>
    /// Salario mensual del empleado (opcional - información sensible)
    /// Se actualiza en caso de aumento, promoción o ajuste salarial
    /// </summary>
    /// <example>3000000.00</example>
    [Range(0.01, 999999999.99, ErrorMessage = "El salario debe estar entre 0.01 y 999,999,999.99")]
    public decimal? Salario { get; set; }

    #endregion

    #region Status

    /// <summary>
    /// Estado del empleado (opcional)
    /// Valores permitidos: Activo, Inactivo, Vacaciones, Licencia
    /// </summary>
    /// <example>Vacaciones</example>
    [StringLength(20, ErrorMessage = "El estado no puede exceder 20 caracteres")]
    [RegularExpression(@"^(Activo|Inactivo|Vacaciones|Licencia)$",
        ErrorMessage = "El estado debe ser: Activo, Inactivo, Vacaciones o Licencia")]
    public string? Estado { get; set; }

    #endregion
}
