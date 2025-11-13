namespace G2rismBeta.API.DTOs.Proveedor;

/// <summary>
/// DTO para crear un nuevo proveedor en el sistema
/// </summary>
public class ProveedorCreateDto
{
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
    /// Teléfono alternativo del proveedor (opcional)
    /// </summary>
    /// <example>+57 300 765 4321</example>
    public string? TelefonoAlternativo { get; set; }

    /// <summary>
    /// Correo electrónico principal
    /// </summary>
    /// <example>contacto@hotelparaiso.com</example>
    public string CorreoElectronico { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico alternativo (opcional)
    /// </summary>
    /// <example>ventas@hotelparaiso.com</example>
    public string? CorreoAlternativo { get; set; }

    /// <summary>
    /// Dirección física del proveedor (opcional)
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
    /// Valores permitidos: 'Hotel', 'Aerolinea', 'Transporte', 'Servicios', 'Mixto'
    /// </summary>
    /// <example>Hotel</example>
    public string TipoProveedor { get; set; } = string.Empty;

    /// <summary>
    /// Sitio web del proveedor (opcional)
    /// </summary>
    /// <example>https://www.hotelparaiso.com</example>
    public string? SitioWeb { get; set; }

    /// <summary>
    /// Calificación inicial del proveedor (0.0 a 5.0 estrellas)
    /// Por defecto: 0.0
    /// </summary>
    /// <example>4.5</example>
    public decimal Calificacion { get; set; } = 0.0m;

    /// <summary>
    /// Estado inicial del proveedor
    /// Valores permitidos: 'Activo', 'Inactivo', 'Suspendido'
    /// Por defecto: 'Activo'
    /// </summary>
    /// <example>Activo</example>
    public string Estado { get; set; } = "Activo";

    /// <summary>
    /// Observaciones o notas adicionales sobre el proveedor
    /// </summary>
    /// <example>Proveedor recomendado por la asociación hotelera. Excelente servicio.</example>
    public string? Observaciones { get; set; }
}
