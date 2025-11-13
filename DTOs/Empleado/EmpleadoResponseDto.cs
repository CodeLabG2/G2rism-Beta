namespace G2rismBeta.API.DTOs.Empleado;

/// <summary>
/// DTO para devolver información completa de un empleado.
/// Incluye propiedades calculadas y datos relacionados (jefe).
/// IMPORTANTE: El salario solo se incluye si el usuario tiene permisos adecuados.
/// </summary>
public class EmpleadoResponseDto
{
    #region Primary Key

    /// <summary>
    /// Identificador único del empleado
    /// </summary>
    /// <example>1</example>
    public int IdEmpleado { get; set; }

    #endregion

    #region Foreign Keys

    /// <summary>
    /// ID del usuario asociado
    /// </summary>
    /// <example>5</example>
    public int IdUsuario { get; set; }

    /// <summary>
    /// ID del jefe directo (null si no tiene jefe)
    /// </summary>
    /// <example>2</example>
    public int? IdJefe { get; set; }

    #endregion

    #region Personal Information

    /// <summary>
    /// Nombre del empleado
    /// </summary>
    /// <example>Carlos</example>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Apellido del empleado
    /// </summary>
    /// <example>Rodríguez</example>
    public string Apellido { get; set; } = string.Empty;

    /// <summary>
    /// Nombre completo del empleado (calculado)
    /// </summary>
    /// <example>Carlos Rodríguez</example>
    public string NombreCompleto { get; set; } = string.Empty;

    /// <summary>
    /// Número de documento de identidad
    /// </summary>
    /// <example>1234567890</example>
    public string DocumentoIdentidad { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de documento: CC, CE, Pasaporte, NIT
    /// </summary>
    /// <example>CC</example>
    public string TipoDocumento { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de nacimiento
    /// </summary>
    /// <example>1990-05-15</example>
    public DateTime FechaNacimiento { get; set; }

    /// <summary>
    /// Edad del empleado (calculada automáticamente)
    /// </summary>
    /// <example>34</example>
    public int Edad { get; set; }

    #endregion

    #region Contact Information

    /// <summary>
    /// Correo electrónico corporativo
    /// </summary>
    /// <example>carlos.rodriguez@g2rism.com</example>
    public string CorreoElectronico { get; set; } = string.Empty;

    /// <summary>
    /// Teléfono de contacto
    /// </summary>
    /// <example>+57 300 1234567</example>
    public string Telefono { get; set; } = string.Empty;

    #endregion

    #region Employment Information

    /// <summary>
    /// Cargo o posición del empleado
    /// </summary>
    /// <example>Asesor de Viajes</example>
    public string Cargo { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de ingreso a la empresa
    /// </summary>
    /// <example>2024-01-15</example>
    public DateTime FechaIngreso { get; set; }

    /// <summary>
    /// Antigüedad en años (calculada automáticamente)
    /// </summary>
    /// <example>1</example>
    public int AntiguedadAnios { get; set; }

    /// <summary>
    /// Antigüedad en meses (calculada automáticamente)
    /// </summary>
    /// <example>10</example>
    public int AntiguedadMeses { get; set; }

    /// <summary>
    /// Salario mensual (INFORMACIÓN SENSIBLE)
    /// Este campo solo se incluye si el usuario tiene permisos para verlo
    /// Puede ser null si el usuario no tiene autorización
    /// </summary>
    /// <example>2500000.00</example>
    public decimal? Salario { get; set; }

    #endregion

    #region Status

    /// <summary>
    /// Estado del empleado: Activo, Inactivo, Vacaciones, Licencia
    /// </summary>
    /// <example>Activo</example>
    public string Estado { get; set; } = string.Empty;

    #endregion

    #region Hierarchy Information

    /// <summary>
    /// Indica si el empleado tiene subordinados (es jefe)
    /// </summary>
    /// <example>true</example>
    public bool EsJefe { get; set; }

    /// <summary>
    /// Cantidad de subordinados directos
    /// </summary>
    /// <example>5</example>
    public int CantidadSubordinados { get; set; }

    /// <summary>
    /// Información básica del jefe directo (si tiene)
    /// </summary>
    public JefeBasicInfoDto? Jefe { get; set; }

    #endregion

    #region Related Information

    /// <summary>
    /// Nombre de usuario para login (del usuario asociado)
    /// </summary>
    /// <example>carlos.rodriguez</example>
    public string? Username { get; set; }

    #endregion
}
