namespace G2rismBeta.API.DTOs.PreferenciaCliente;

/// <summary>
/// DTO de respuesta con las preferencias de un cliente
/// Incluye información completa para la API
/// </summary>
public class PreferenciaClienteResponseDto
{
    /// <summary>
    /// ID de la preferencia
    /// </summary>
    /// <example>1</example>
    public int IdPreferencia { get; set; }

    /// <summary>
    /// ID del cliente
    /// </summary>
    /// <example>5</example>
    public int IdCliente { get; set; }

    /// <summary>
    /// Nombre completo del cliente
    /// </summary>
    /// <example>Juan Pérez</example>
    public string? NombreCliente { get; set; }

    /// <summary>
    /// Tipo de destino preferido
    /// </summary>
    /// <example>Playa</example>
    public string? TipoDestino { get; set; }

    /// <summary>
    /// Tipo de alojamiento preferido
    /// </summary>
    /// <example>Hotel</example>
    public string? TipoAlojamiento { get; set; }

    /// <summary>
    /// Presupuesto promedio por viaje
    /// </summary>
    /// <example>5000000</example>
    public decimal? PresupuestoPromedio { get; set; }

    /// <summary>
    /// Preferencias de alimentación
    /// </summary>
    /// <example>Vegetariano</example>
    public string? PreferenciasAlimentacion { get; set; }

    /// <summary>
    /// Lista de intereses del cliente
    /// </summary>
    /// <example>["Historia", "Gastronomía", "Deportes"]</example>
    public List<string>? Intereses { get; set; }

    /// <summary>
    /// Fecha de última actualización
    /// </summary>
    /// <example>2025-11-07T10:00:00</example>
    public DateTime FechaActualizacion { get; set; }
}