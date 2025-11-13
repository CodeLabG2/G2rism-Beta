namespace G2rismBeta.API.DTOs.CategoriaCliente;

/// <summary>
/// DTO de respuesta para una categoría de cliente
/// Incluye toda la información que se devuelve al cliente
/// </summary>
public class CategoriaClienteResponseDto
{
    /// <summary>
    /// ID de la categoría
    /// </summary>
    /// <example>1</example>
    public int IdCategoria { get; set; }

    /// <summary>
    /// Nombre de la categoría
    /// </summary>
    /// <example>Oro</example>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Descripción de la categoría
    /// </summary>
    /// <example>Categoría premium con beneficios exclusivos</example>
    public string? Descripcion { get; set; }

    /// <summary>
    /// Color hexadecimal para representación visual
    /// </summary>
    /// <example>#FFD700</example>
    public string? ColorHex { get; set; }

    /// <summary>
    /// Beneficios de la categoría
    /// </summary>
    /// <example>Descuentos del 15%, prioridad en atención, acceso a promociones exclusivas</example>
    public string? Beneficios { get; set; }

    /// <summary>
    /// Criterios de clasificación
    /// </summary>
    /// <example>Más de 10 reservas anuales o gasto superior a $5000 USD</example>
    public string? CriteriosClasificacion { get; set; }

    /// <summary>
    /// Porcentaje de descuento
    /// </summary>
    /// <example>15</example>
    public decimal DescuentoPorcentaje { get; set; }

    /// <summary>
    /// Estado de la categoría
    /// </summary>
    /// <example>true</example>
    public bool Estado { get; set; }

    /// <summary>
    /// Cantidad de clientes en esta categoría
    /// </summary>
    /// <example>45</example>
    public int CantidadClientes { get; set; }
}
