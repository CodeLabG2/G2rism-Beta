namespace G2rismBeta.API.DTOs.Cliente;

/// <summary>
/// DTO de respuesta para un cliente
/// Incluye toda la información que se devuelve al cliente
/// </summary>
public class ClienteResponseDto
{
    /// <summary>
    /// ID del cliente
    /// </summary>
    /// <example>1</example>
    public int IdCliente { get; set; }

    /// <summary>
    /// ID del usuario asociado
    /// </summary>
    /// <example>5</example>
    public int IdUsuario { get; set; }

    /// <summary>
    /// ID de la categoría del cliente
    /// </summary>
    /// <example>2</example>
    public int? IdCategoria { get; set; }

    /// <summary>
    /// Nombre del cliente
    /// </summary>
    /// <example>Juan</example>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Apellido del cliente
    /// </summary>
    /// <example>Pérez García</example>
    public string Apellido { get; set; } = string.Empty;

    /// <summary>
    /// Nombre completo del cliente
    /// </summary>
    /// <example>Juan Pérez García</example>
    public string NombreCompleto { get; set; } = string.Empty;

    /// <summary>
    /// Documento de identidad
    /// </summary>
    /// <example>1234567890</example>
    public string DocumentoIdentidad { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de documento
    /// </summary>
    /// <example>CC</example>
    public string TipoDocumento { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de nacimiento
    /// </summary>
    /// <example>1990-05-15</example>
    public DateTime FechaNacimiento { get; set; }

    /// <summary>
    /// Edad del cliente
    /// </summary>
    /// <example>33</example>
    public int Edad { get; set; }

    /// <summary>
    /// Correo electrónico
    /// </summary>
    /// <example>juan.perez@example.com</example>
    public string CorreoElectronico { get; set; } = string.Empty;

    /// <summary>
    /// Teléfono
    /// </summary>
    /// <example>+57 300 123 4567</example>
    public string Telefono { get; set; } = string.Empty;

    /// <summary>
    /// Dirección
    /// </summary>
    /// <example>Calle 123 #45-67</example>
    public string? Direccion { get; set; }

    /// <summary>
    /// Ciudad
    /// </summary>
    /// <example>Medellín</example>
    public string Ciudad { get; set; } = string.Empty;

    /// <summary>
    /// País
    /// </summary>
    /// <example>Colombia</example>
    public string Pais { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de registro
    /// </summary>
    /// <example>2024-01-15T10:30:00</example>
    public DateTime FechaRegistro { get; set; }

    /// <summary>
    /// Estado del cliente
    /// </summary>
    /// <example>true</example>
    public bool Estado { get; set; }

    /// <summary>
    /// Nombre de la categoría (si tiene)
    /// </summary>
    /// <example>Oro</example>
    public string? NombreCategoria { get; set; }

    /// <summary>
    /// Descuento por categoría (si tiene)
    /// </summary>
    /// <example>15</example>
    public decimal? DescuentoCategoria { get; set; }
}