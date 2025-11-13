using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace G2rismBeta.API.Models;

/// <summary>
/// Entidad que representa una Categoría de Cliente en el sistema CRM
/// Permite segmentar clientes para ofrecer beneficios diferenciados
/// Ejemplos: Oro, Plata, Bronce, VIP, Corporativo
/// </summary>
[Table("categorias_cliente")]
public class CategoriaCliente
{
    /// <summary>
    /// Identificador único de la categoría
    /// </summary>
    [Key]
    [Column("id_categoria")]
    public int IdCategoria { get; set; }

    /// <summary>
    /// Nombre de la categoría (único en el sistema)
    /// Ejemplos: "Oro", "Plata", "Bronce", "VIP", "Corporativo"
    /// </summary>
    [Required(ErrorMessage = "El nombre de la categoría es obligatorio")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres")]
    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Descripción detallada de la categoría y sus características
    /// </summary>
    [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
    [Column("descripcion")]
    public string? Descripcion { get; set; }

    /// <summary>
    /// Color hexadecimal para representación visual de la categoría
    /// Formato: #RRGGBB (ejemplo: #FFD700 para dorado)
    /// </summary>
    [StringLength(7, ErrorMessage = "El color debe estar en formato hexadecimal (#RRGGBB)")]
    [RegularExpression(@"^#[0-9A-Fa-f]{6}$", ErrorMessage = "El color debe estar en formato hexadecimal válido (#RRGGBB)")]
    [Column("color_hex")]
    public string? ColorHex { get; set; }

    /// <summary>
    /// Texto que describe los beneficios de esta categoría
    /// Ejemplo: "Descuentos del 15%, prioridad en atención, acceso a promociones exclusivas"
    /// </summary>
    [StringLength(1000, ErrorMessage = "Los beneficios no pueden exceder 1000 caracteres")]
    [Column("beneficios")]
    public string? Beneficios { get; set; }

    /// <summary>
    /// Criterios para que un cliente pertenezca a esta categoría
    /// Ejemplo: "Más de 10 reservas anuales o gasto superior a $5000 USD"
    /// </summary>
    [StringLength(500, ErrorMessage = "Los criterios no pueden exceder 500 caracteres")]
    [Column("criterios_clasificacion")]
    public string? CriteriosClasificacion { get; set; }

    /// <summary>
    /// Porcentaje de descuento que aplica a esta categoría
    /// Rango: 0 a 100 (representa porcentaje)
    /// </summary>
    [Range(0, 100, ErrorMessage = "El descuento debe estar entre 0 y 100")]
    [Column("descuento_porcentaje")]
    public decimal DescuentoPorcentaje { get; set; } = 0;

    /// <summary>
    /// Estado de la categoría (true = activa, false = inactiva)
    /// </summary>
    [Column("estado")]
    public bool Estado { get; set; } = true;

    // ========================================
    // RELACIONES DE NAVEGACIÓN
    // ========================================

    /// <summary>
    /// Lista de clientes que pertenecen a esta categoría
    /// Relación uno a muchos (una categoría puede tener muchos clientes)
    /// </summary>
    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
}
