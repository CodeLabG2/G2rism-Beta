using System.ComponentModel.DataAnnotations;

namespace G2rismBeta.API.DTOs.CategoriaCliente;

/// <summary>
/// DTO para crear una nueva categoría de cliente
/// Solo incluye los campos que el cliente puede enviar
/// </summary>
public class CategoriaClienteCreateDto
{
    /// <summary>
    /// Nombre de la categoría
    /// </summary>
    /// <example>Oro</example>
    [Required(ErrorMessage = "El nombre de la categoría es obligatorio")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Descripción de la categoría
    /// </summary>
    /// <example>Categoría premium con beneficios exclusivos</example>
    [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
    public string? Descripcion { get; set; }

    /// <summary>
    /// Color hexadecimal para representación visual
    /// Formato: #RRGGBB
    /// </summary>
    /// <example>#FFD700</example>
    [StringLength(7, ErrorMessage = "El color debe estar en formato hexadecimal (#RRGGBB)")]
    [RegularExpression(@"^#[0-9A-Fa-f]{6}$", ErrorMessage = "El color debe estar en formato hexadecimal válido (#RRGGBB)")]
    public string? ColorHex { get; set; }

    /// <summary>
    /// Beneficios de la categoría
    /// </summary>
    /// <example>Descuentos del 15%, prioridad en atención, acceso a promociones exclusivas</example>
    [StringLength(1000, ErrorMessage = "Los beneficios no pueden exceder 1000 caracteres")]
    public string? Beneficios { get; set; }

    /// <summary>
    /// Criterios de clasificación
    /// </summary>
    /// <example>Más de 10 reservas anuales o gasto superior a $5000 USD</example>
    [StringLength(500, ErrorMessage = "Los criterios no pueden exceder 500 caracteres")]
    public string? CriteriosClasificacion { get; set; }

    /// <summary>
    /// Porcentaje de descuento (0-100)
    /// </summary>
    /// <example>15</example>
    [Range(0, 100, ErrorMessage = "El descuento debe estar entre 0 y 100")]
    public decimal DescuentoPorcentaje { get; set; } = 0;

    /// <summary>
    /// Estado de la categoría (true = activa, false = inactiva)
    /// </summary>
    /// <example>true</example>
    public bool Estado { get; set; } = true;
}
