namespace G2rismBeta.API.DTOs.FormaDePago;

/// <summary>
/// DTO de respuesta para una forma de pago.
/// Incluye todos los datos de la forma de pago y propiedades computadas.
/// </summary>
public class FormaDePagoResponseDto
{
    /// <summary>
    /// Identificador único de la forma de pago
    /// </summary>
    public int IdFormaPago { get; set; }

    /// <summary>
    /// Método de pago
    /// </summary>
    public string Metodo { get; set; } = string.Empty;

    /// <summary>
    /// Descripción adicional del método de pago
    /// </summary>
    public string? Descripcion { get; set; }

    /// <summary>
    /// Indica si el método de pago requiere verificación externa
    /// </summary>
    public bool RequiereVerificacion { get; set; }

    /// <summary>
    /// Indica si el método de pago está activo y disponible
    /// </summary>
    public bool Activo { get; set; }

    /// <summary>
    /// Fecha de creación del registro
    /// </summary>
    public DateTime FechaCreacion { get; set; }

    /// <summary>
    /// Fecha de última modificación del registro
    /// </summary>
    public DateTime? FechaModificacion { get; set; }

    /// <summary>
    /// Indica si es un método de pago electrónico (propiedad computada)
    /// </summary>
    public bool EsMetodoElectronico { get; set; }

    /// <summary>
    /// Indica si es pago en efectivo (propiedad computada)
    /// </summary>
    public bool EsEfectivo { get; set; }
}