namespace G2rismBeta.API.DTOs.Cliente;

/// <summary>
/// DTO para cliente con información detallada de su categoría
/// Útil para reportes y vistas detalladas
/// </summary>
public class ClienteConCategoriaDto
{
    /// <summary>
    /// Información básica del cliente
    /// </summary>
    public ClienteResponseDto Cliente { get; set; } = new();

    /// <summary>
    /// Información de la categoría (si tiene)
    /// </summary>
    public CategoriaInfo? Categoria { get; set; }

    /// <summary>
    /// Clase interna con información de categoría
    /// </summary>
    public class CategoriaInfo
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? ColorHex { get; set; }
        public decimal DescuentoPorcentaje { get; set; }
        public string? Beneficios { get; set; }
    }
}