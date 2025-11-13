namespace G2rismBeta.API.DTOs.ContratoProveedor;

/// <summary>
/// DTO de respuesta con toda la información del contrato de proveedor
/// Incluye datos calculados y relaciones
/// </summary>
public class ContratoProveedorResponseDto
{
    /// <summary>
    /// Identificador único del contrato
    /// </summary>
    /// <example>1</example>
    public int IdContrato { get; set; }

    /// <summary>
    /// ID del proveedor asociado al contrato
    /// </summary>
    /// <example>1</example>
    public int IdProveedor { get; set; }

    /// <summary>
    /// Nombre de la empresa proveedora
    /// </summary>
    /// <example>Hotel Paraíso del Caribe S.A.S.</example>
    public string NombreProveedor { get; set; } = string.Empty;

    /// <summary>
    /// Número único del contrato
    /// </summary>
    /// <example>CONT-2024-001</example>
    public string NumeroContrato { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de inicio de vigencia del contrato
    /// </summary>
    /// <example>2024-01-01T00:00:00</example>
    public DateTime FechaInicio { get; set; }

    /// <summary>
    /// Fecha de finalización del contrato
    /// </summary>
    /// <example>2025-12-31T23:59:59</example>
    public DateTime FechaFin { get; set; }

    /// <summary>
    /// Tipo de contrato
    /// </summary>
    /// <example>Servicios Hoteleros</example>
    public string TipoContrato { get; set; } = string.Empty;

    /// <summary>
    /// Valor total del contrato
    /// </summary>
    /// <example>50000000.00</example>
    public decimal ValorContrato { get; set; }

    /// <summary>
    /// Condiciones de pago acordadas
    /// </summary>
    /// <example>Pago mensual anticipado. Transferencia bancaria a 30 días.</example>
    public string CondicionesPago { get; set; } = string.Empty;

    /// <summary>
    /// Términos y condiciones del contrato
    /// </summary>
    /// <example>1. Prestación de servicios hoteleros. 2. Tarifas preferenciales...</example>
    public string Terminos { get; set; } = string.Empty;

    /// <summary>
    /// Indica si el contrato se renueva automáticamente al vencer
    /// </summary>
    /// <example>true</example>
    public bool RenovacionAutomatica { get; set; }

    /// <summary>
    /// Estado actual del contrato
    /// </summary>
    /// <example>Vigente</example>
    public string Estado { get; set; } = string.Empty;

    /// <summary>
    /// URL del archivo del contrato digitalizado
    /// </summary>
    /// <example>https://storage.g2rism.com/contratos/CONT-2024-001.pdf</example>
    public string? ArchivoContrato { get; set; }

    /// <summary>
    /// Observaciones o notas adicionales sobre el contrato
    /// </summary>
    /// <example>Contrato negociado con descuentos especiales por volumen.</example>
    public string? Observaciones { get; set; }

    /// <summary>
    /// Fecha de creación del registro
    /// </summary>
    /// <example>2024-01-01T10:30:00</example>
    public DateTime FechaCreacion { get; set; }

    /// <summary>
    /// Indica si el contrato está vigente actualmente
    /// </summary>
    /// <example>true</example>
    public bool EstaVigente { get; set; }

    /// <summary>
    /// Días restantes hasta el vencimiento del contrato
    /// </summary>
    /// <example>180</example>
    public int DiasRestantes { get; set; }

    /// <summary>
    /// Indica si el contrato está próximo a vencer (menos de 30 días)
    /// </summary>
    /// <example>false</example>
    public bool ProximoAVencer { get; set; }

    /// <summary>
    /// Duración total del contrato en días
    /// </summary>
    /// <example>730</example>
    public int DuracionDias { get; set; }
}
