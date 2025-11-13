using System.ComponentModel.DataAnnotations;

namespace G2rismBeta.API.DTOs.PreferenciaCliente;

/// <summary>
/// DTO para actualizar preferencias de un cliente
/// Permite modificar las preferencias existentes
/// </summary>
public class PreferenciaClienteUpdateDto
{
    /// <summary>
    /// ID de la preferencia a actualizar
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "El ID de la preferencia es obligatorio")]
    public int IdPreferencia { get; set; }

    /// <summary>
    /// Tipo de destino preferido
    /// </summary>
    /// <example>Montaña</example>
    [StringLength(50, ErrorMessage = "El tipo de destino no puede exceder 50 caracteres")]
    public string? TipoDestino { get; set; }

    /// <summary>
    /// Tipo de alojamiento preferido
    /// </summary>
    /// <example>Cabaña</example>
    [StringLength(50, ErrorMessage = "El tipo de alojamiento no puede exceder 50 caracteres")]
    public string? TipoAlojamiento { get; set; }

    /// <summary>
    /// Presupuesto promedio por viaje
    /// </summary>
    /// <example>7500000</example>
    [Range(0, 999999999, ErrorMessage = "El presupuesto debe ser un valor positivo")]
    public decimal? PresupuestoPromedio { get; set; }

    /// <summary>
    /// Preferencias de alimentación
    /// </summary>
    /// <example>Vegano</example>
    [StringLength(200, ErrorMessage = "Las preferencias de alimentación no pueden exceder 200 caracteres")]
    public string? PreferenciasAlimentacion { get; set; }

    /// <summary>
    /// Lista de intereses del cliente (se guardará como JSON)
    /// </summary>
    /// <example>["Naturaleza", "Fotografía", "Senderismo"]</example>
    public List<string>? Intereses { get; set; }
}