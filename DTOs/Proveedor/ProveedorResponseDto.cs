using System.ComponentModel.DataAnnotations;

namespace G2rismBeta.API.DTOs.Proveedor;

/// <summary>
/// DTO de respuesta con toda la información del proveedor
/// Incluye datos calculados y relaciones
/// </summary>
public class ProveedorResponseDto
{
    /// <summary>
    /// Identificador único del proveedor
    /// </summary>
    /// <example>1</example>
    public int IdProveedor { get; set; }

    /// <summary>
    /// Nombre o razón social de la empresa proveedora
    /// </summary>
    /// <example>Hotel Paraíso del Caribe S.A.S.</example>
    public string NombreEmpresa { get; set; } = string.Empty;

    /// <summary>
    /// Nombre de la persona de contacto principal
    /// </summary>
    /// <example>Carlos Pérez García</example>
    public string NombreContacto { get; set; } = string.Empty;

    /// <summary>
    /// Teléfono principal del proveedor
    /// </summary>
    /// <example>+57 300 123 4567</example>
    public string Telefono { get; set; } = string.Empty;

    /// <summary>
    /// Teléfono alternativo del proveedor
    /// </summary>
    /// <example>+57 300 765 4321</example>
    public string? TelefonoAlternativo { get; set; }

    /// <summary>
    /// Correo electrónico principal
    /// </summary>
    /// <example>contacto@hotelparaiso.com</example>
    public string CorreoElectronico { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico alternativo
    /// </summary>
    /// <example>ventas@hotelparaiso.com</example>
    public string? CorreoAlternativo { get; set; }

    /// <summary>
    /// Dirección física del proveedor
    /// </summary>
    /// <example>Calle 10 # 5-25, Centro Histórico</example>
    public string? Direccion { get; set; }

    /// <summary>
    /// Ciudad donde se encuentra el proveedor
    /// </summary>
    /// <example>Cartagena</example>
    public string Ciudad { get; set; } = string.Empty;

    /// <summary>
    /// País del proveedor
    /// </summary>
    /// <example>Colombia</example>
    public string Pais { get; set; } = string.Empty;

    /// <summary>
    /// NIT (Colombia) o RUT (otros países) - Identificación tributaria única
    /// </summary>
    /// <example>900123456-7</example>
    public string NitRut { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de proveedor según los servicios que ofrece
    /// </summary>
    /// <example>Hotel</example>
    public string TipoProveedor { get; set; } = string.Empty;

    /// <summary>
    /// Sitio web del proveedor
    /// </summary>
    /// <example>https://www.hotelparaiso.com</example>
    public string? SitioWeb { get; set; }

    /// <summary>
    /// Calificación del proveedor (0.0 a 5.0 estrellas)
    /// </summary>
    /// <example>4.5</example>
    public decimal Calificacion { get; set; }

    /// <summary>
    /// Estado actual del proveedor
    /// </summary>
    /// <example>Activo</example>
    public string Estado { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de registro del proveedor en el sistema
    /// </summary>
    /// <example>2024-01-15</example>
    [DataType(DataType.Date)]
    public DateTime FechaRegistro { get; set; }

    /// <summary>
    /// Observaciones o notas adicionales sobre el proveedor
    /// </summary>
    /// <example>Proveedor recomendado por la asociación hotelera.</example>
    public string? Observaciones { get; set; }

    /// <summary>
    /// Cantidad de contratos activos que tiene el proveedor
    /// </summary>
    /// <example>3</example>
    public int ContratosActivos { get; set; } = 0;

    /// <summary>
    /// Indica si el proveedor tiene contratos vigentes
    /// </summary>
    /// <example>true</example>
    public bool TieneContratosVigentes { get; set; } = false;
}
