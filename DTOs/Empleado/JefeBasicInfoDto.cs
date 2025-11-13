namespace G2rismBeta.API.DTOs.Empleado;

/// <summary>
/// DTO con información básica del jefe directo.
/// Se usa para evitar recursión infinita en la jerarquía.
/// </summary>
public class JefeBasicInfoDto
{
    /// <summary>
    /// ID del jefe
    /// </summary>
    /// <example>2</example>
    public int IdEmpleado { get; set; }

    /// <summary>
    /// Nombre completo del jefe
    /// </summary>
    /// <example>María García</example>
    public string NombreCompleto { get; set; } = string.Empty;

    /// <summary>
    /// Cargo del jefe
    /// </summary>
    /// <example>Gerente de Ventas</example>
    public string Cargo { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico del jefe
    /// </summary>
    /// <example>maria.garcia@g2rism.com</example>
    public string CorreoElectronico { get; set; } = string.Empty;

    /// <summary>
    /// Teléfono del jefe
    /// </summary>
    /// <example>+57 301 7654321</example>
    public string Telefono { get; set; } = string.Empty;
}