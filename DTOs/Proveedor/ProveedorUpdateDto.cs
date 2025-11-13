namespace G2rismBeta.API.DTOs.Proveedor;

/// <summary>
/// DTO para actualizar información de un proveedor existente
/// Todos los campos son opcionales para permitir actualizaciones parciales
/// </summary>
public class ProveedorUpdateDto
{
    /// <summary>
    /// Nombre o razón social de la empresa proveedora
    /// </summary>
    /// <example>Hotel Paraíso del Caribe S.A.S.</example>
    public string? NombreEmpresa { get; set; }

    /// <summary>
    /// Nombre de la persona de contacto principal
    /// </summary>
    /// <example>Carlos Pérez García</example>
    public string? NombreContacto { get; set; }

    /// <summary>
    /// Teléfono principal del proveedor
    /// </summary>
    /// <example>+57 300 123 4567</example>
    public string? Telefono { get; set; }

    /// <summary>
    /// Teléfono alternativo del proveedor
    /// </summary>
    /// <example>+57 300 765 4321</example>
    public string? TelefonoAlternativo { get; set; }

    /// <summary>
    /// Correo electrónico principal
    /// </summary>
    /// <example>contacto@hotelparaiso.com</example>
    public string? CorreoElectronico { get; set; }

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
    public string? Ciudad { get; set; }

    /// <summary>
    /// País del proveedor
    /// </summary>
    /// <example>Colombia</example>
    public string? Pais { get; set; }

    /// <summary>
    /// NIT (Colombia) o RUT (otros países) - Identificación tributaria única
    /// </summary>
    /// <example>900123456-7</example>
    public string? NitRut { get; set; }

    /// <summary>
    /// Tipo de proveedor según los servicios que ofrece
    /// Valores permitidos: 'Hotel', 'Aerolinea', 'Transporte', 'Servicios', 'Mixto'
    /// </summary>
    /// <example>Hotel</example>
    public string? TipoProveedor { get; set; }

    /// <summary>
    /// Sitio web del proveedor
    /// </summary>
    /// <example>https://www.hotelparaiso.com</example>
    public string? SitioWeb { get; set; }

    /// <summary>
    /// Calificación del proveedor (0.0 a 5.0 estrellas)
    /// </summary>
    /// <example>4.5</example>
    public decimal? Calificacion { get; set; }

    /// <summary>
    /// Estado del proveedor
    /// Valores permitidos: 'Activo', 'Inactivo', 'Suspendido'
    /// </summary>
    /// <example>Activo</example>
    public string? Estado { get; set; }

    /// <summary>
    /// Observaciones o notas adicionales sobre el proveedor
    /// </summary>
    /// <example>Proveedor actualizado con nueva calificación por excelente servicio.</example>
    public string? Observaciones { get; set; }
}
