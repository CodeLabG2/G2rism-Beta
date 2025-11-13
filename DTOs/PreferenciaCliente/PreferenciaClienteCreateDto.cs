using System.ComponentModel.DataAnnotations;

namespace G2rismBeta.API.DTOs.PreferenciaCliente;

/// <summary>
/// DTO para crear preferencias de un cliente
/// Permite personalizar la experiencia del cliente (CRM)
/// </summary>
public class PreferenciaClienteCreateDto
{
    /// <summary>
    /// ID del cliente al que pertenecen estas preferencias
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "El ID del cliente es obligatorio")]
    public int IdCliente { get; set; }

    /// <summary>
    /// Tipo de destino preferido
    /// </summary>
    /// <example>Playa</example>
    [StringLength(50, ErrorMessage = "El tipo de destino no puede exceder 50 caracteres")]
    public string? TipoDestino { get; set; }

    /// <summary>
    /// Tipo de alojamiento preferido
    /// </summary>
    /// <example>Hotel</example>
    [StringLength(50, ErrorMessage = "El tipo de alojamiento no puede exceder 50 caracteres")]
    public string? TipoAlojamiento { get; set; }

    /// <summary>
    /// Presupuesto promedio por viaje
    /// </summary>
    /// <example>5000000</example>
    [Range(0, 999999999, ErrorMessage = "El presupuesto debe ser un valor positivo")]
    public decimal? PresupuestoPromedio { get; set; }

    /// <summary>
    /// Preferencias de alimentación
    /// </summary>
    /// <example>Vegetariano</example>
    [StringLength(100, ErrorMessage = "Las preferencias de alimentación no pueden exceder 100 caracteres")]
    public string? PreferenciasAlimentacion { get; set; }

    /// <summary>
    /// Lista de intereses del cliente (se guardará como JSON)
    /// </summary>
    /// <example>["Historia", "Gastronomía", "Deportes"]</example>
    public List<string>? Intereses { get; set; }
}