namespace G2rismBeta.API.DTOs.ContratoProveedor;

/// <summary>
/// DTO para actualizar un contrato con un proveedor existente
/// Todos los campos son opcionales para permitir actualizaciones parciales
/// </summary>
public class ContratoProveedorUpdateDto
{
    /// <summary>
    /// Número único del contrato
    /// </summary>
    /// <example>CONT-2024-001-MOD</example>
    public string? NumeroContrato { get; set; }

    /// <summary>
    /// Fecha de inicio de vigencia del contrato
    /// </summary>
    /// <example>2024-01-01</example>
    public DateTime? FechaInicio { get; set; }

    /// <summary>
    /// Fecha de finalización del contrato
    /// </summary>
    /// <example>2026-12-31</example>
    public DateTime? FechaFin { get; set; }

    /// <summary>
    /// Tipo de contrato
    /// </summary>
    /// <example>Servicios Hoteleros Premium</example>
    public string? TipoContrato { get; set; }

    /// <summary>
    /// Valor total del contrato
    /// </summary>
    /// <example>60000000.00</example>
    public decimal? ValorContrato { get; set; }

    /// <summary>
    /// Condiciones de pago acordadas
    /// </summary>
    /// <example>Pago mensual anticipado. Transferencia bancaria a 15 días.</example>
    public string? CondicionesPago { get; set; }

    /// <summary>
    /// Términos y condiciones del contrato
    /// </summary>
    /// <example>Términos actualizados con nuevas cláusulas.</example>
    public string? Terminos { get; set; }

    /// <summary>
    /// Indica si el contrato se renueva automáticamente al vencer
    /// </summary>
    /// <example>true</example>
    public bool? RenovacionAutomatica { get; set; }

    /// <summary>
    /// Estado del contrato
    /// Valores permitidos: 'Vigente', 'Vencido', 'Cancelado', 'En_Negociacion'
    /// </summary>
    /// <example>Vigente</example>
    public string? Estado { get; set; }

    /// <summary>
    /// URL del archivo del contrato digitalizado
    /// </summary>
    /// <example>https://storage.g2rism.com/contratos/CONT-2024-001-v2.pdf</example>
    public string? ArchivoContrato { get; set; }

    /// <summary>
    /// Observaciones o notas adicionales sobre el contrato
    /// </summary>
    /// <example>Contrato renovado con condiciones mejoradas.</example>
    public string? Observaciones { get; set; }
}
