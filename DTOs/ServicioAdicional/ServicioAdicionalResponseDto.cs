namespace G2rismBeta.API.DTOs.ServicioAdicional;

/// <summary>
/// DTO de respuesta con información completa del servicio adicional
/// </summary>
public class ServicioAdicionalResponseDto
{
    /// <summary>
    /// Identificador único del servicio
    /// </summary>
    public int IdServicio { get; set; }

    /// <summary>
    /// ID del proveedor asociado
    /// </summary>
    public int IdProveedor { get; set; }

    /// <summary>
    /// Nombre del proveedor (desde relación)
    /// </summary>
    public string NombreProveedor { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del servicio
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de servicio (tour, guia, actividad, transporte_interno)
    /// </summary>
    public string Tipo { get; set; } = string.Empty;

    /// <summary>
    /// Descripción detallada del servicio
    /// </summary>
    public string? Descripcion { get; set; }

    /// <summary>
    /// Precio del servicio
    /// </summary>
    public decimal Precio { get; set; }

    /// <summary>
    /// Unidad de medida (persona, grupo, hora, dia)
    /// </summary>
    public string Unidad { get; set; } = string.Empty;

    /// <summary>
    /// Indica si el servicio está disponible
    /// </summary>
    public bool Disponibilidad { get; set; }

    /// <summary>
    /// Tiempo estimado del servicio en minutos
    /// </summary>
    public int? TiempoEstimado { get; set; }

    /// <summary>
    /// Ubicación o punto de encuentro
    /// </summary>
    public string? Ubicacion { get; set; }

    /// <summary>
    /// Requisitos o condiciones
    /// </summary>
    public string? Requisitos { get; set; }

    /// <summary>
    /// Capacidad máxima
    /// </summary>
    public int? CapacidadMaxima { get; set; }

    /// <summary>
    /// Edad mínima requerida
    /// </summary>
    public int? EdadMinima { get; set; }

    /// <summary>
    /// Idiomas disponibles (JSON array)
    /// </summary>
    public string? IdiomasDisponibles { get; set; }

    /// <summary>
    /// Estado del servicio (activo/inactivo)
    /// </summary>
    public bool Estado { get; set; }

    /// <summary>
    /// Fecha de creación del registro
    /// </summary>
    public DateTime FechaCreacion { get; set; }

    /// <summary>
    /// Fecha de última modificación
    /// </summary>
    public DateTime? FechaModificacion { get; set; }

    // ============================================
    // PROPIEDADES COMPUTADAS (desde el modelo)
    // ============================================

    /// <summary>
    /// Indica si el servicio está activo
    /// </summary>
    public bool EstaActivo { get; set; }

    /// <summary>
    /// Indica si el servicio está disponible (activo y con disponibilidad)
    /// </summary>
    public bool EstaDisponible { get; set; }

    /// <summary>
    /// Nombre completo con tipo
    /// </summary>
    public string NombreCompleto { get; set; } = string.Empty;

    /// <summary>
    /// Precio formateado con unidad
    /// </summary>
    public string PrecioFormateado { get; set; } = string.Empty;

    /// <summary>
    /// Duración estimada formateada
    /// </summary>
    public string? DuracionFormateada { get; set; }
}
