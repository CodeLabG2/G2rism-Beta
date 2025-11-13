namespace G2rismBeta.API.DTOs.ContratoProveedor;

/// <summary>
/// DTO para crear un nuevo contrato con un proveedor
/// </summary>
public class ContratoProveedorCreateDto
{
    /// <summary>
    /// ID del proveedor asociado al contrato
    /// </summary>
    /// <example>1</example>
    public int IdProveedor { get; set; }

    /// <summary>
    /// Número único del contrato para identificación
    /// </summary>
    /// <example>CONT-2024-001</example>
    public string NumeroContrato { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de inicio de vigencia del contrato
    /// </summary>
    /// <example>2024-01-01</example>
    public DateTime FechaInicio { get; set; }

    /// <summary>
    /// Fecha de finalización del contrato
    /// </summary>
    /// <example>2025-12-31</example>
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
    /// <example>1. Prestación de servicios hoteleros. 2. Tarifas preferenciales para clientes. 3. Disponibilidad garantizada.</example>
    public string Terminos { get; set; } = string.Empty;

    /// <summary>
    /// Indica si el contrato se renueva automáticamente al vencer
    /// </summary>
    /// <example>true</example>
    public bool RenovacionAutomatica { get; set; } = false;

    /// <summary>
    /// Estado inicial del contrato
    /// Valores permitidos: 'Vigente', 'Vencido', 'Cancelado', 'En_Negociacion'
    /// Por defecto: 'En_Negociacion'
    /// </summary>
    /// <example>En_Negociacion</example>
    public string Estado { get; set; } = "En_Negociacion";

    /// <summary>
    /// URL del archivo del contrato digitalizado (opcional)
    /// </summary>
    /// <example>https://storage.g2rism.com/contratos/CONT-2024-001.pdf</example>
    public string? ArchivoContrato { get; set; }

    /// <summary>
    /// Observaciones o notas adicionales sobre el contrato
    /// </summary>
    /// <example>Contrato negociado con descuentos especiales por volumen.</example>
    public string? Observaciones { get; set; }
}
