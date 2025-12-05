namespace G2rismBeta.API.DTOs.Hotel;

/// <summary>
/// DTO de respuesta con información completa del hotel
/// </summary>
public class HotelResponseDto
{
    /// <summary>
    /// Identificador único del hotel
    /// </summary>
    public int IdHotel { get; set; }

    /// <summary>
    /// ID del proveedor asociado
    /// </summary>
    public int IdProveedor { get; set; }

    /// <summary>
    /// Nombre del proveedor
    /// </summary>
    public string NombreProveedor { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del hotel
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Ciudad donde se ubica el hotel
    /// </summary>
    public string Ciudad { get; set; } = string.Empty;

    /// <summary>
    /// País donde se ubica el hotel
    /// </summary>
    public string? Pais { get; set; }

    /// <summary>
    /// Dirección física del hotel
    /// </summary>
    public string Direccion { get; set; } = string.Empty;

    /// <summary>
    /// Información de contacto
    /// </summary>
    public string? Contacto { get; set; }

    /// <summary>
    /// Descripción detallada del hotel
    /// </summary>
    public string? Descripcion { get; set; }

    /// <summary>
    /// Categoría del hotel
    /// </summary>
    public string? Categoria { get; set; }

    /// <summary>
    /// Clasificación por estrellas (1-5)
    /// </summary>
    public int? Estrellas { get; set; }

    /// <summary>
    /// Precio promedio por noche
    /// </summary>
    public decimal PrecioPorNoche { get; set; }

    /// <summary>
    /// Capacidad máxima de personas por habitación
    /// </summary>
    public int? CapacidadPorHabitacion { get; set; }

    /// <summary>
    /// Número total de habitaciones disponibles
    /// </summary>
    public int? NumeroHabitaciones { get; set; }

    /// <summary>
    /// Indica si el hotel tiene WiFi
    /// </summary>
    public bool TieneWifi { get; set; }

    /// <summary>
    /// Indica si el hotel tiene piscina
    /// </summary>
    public bool TienePiscina { get; set; }

    /// <summary>
    /// Indica si el hotel tiene restaurante
    /// </summary>
    public bool TieneRestaurante { get; set; }

    /// <summary>
    /// Indica si el hotel tiene gimnasio
    /// </summary>
    public bool TieneGimnasio { get; set; }

    /// <summary>
    /// Indica si el hotel tiene parqueadero
    /// </summary>
    public bool TieneParqueadero { get; set; }

    /// <summary>
    /// Políticas de cancelación del hotel
    /// </summary>
    public string? PoliticasCancelacion { get; set; }

    /// <summary>
    /// Hora de check-in en formato "HH:mm"
    /// </summary>
    public string? CheckInHora { get; set; }

    /// <summary>
    /// Hora de check-out en formato "HH:mm"
    /// </summary>
    public string? CheckOutHora { get; set; }

    /// <summary>
    /// URLs de fotos en formato JSON array
    /// </summary>
    public string? Fotos { get; set; }

    /// <summary>
    /// Servicios adicionales incluidos en formato JSON array
    /// </summary>
    public string? ServiciosIncluidos { get; set; }

    /// <summary>
    /// Estado del hotel
    /// </summary>
    public bool Estado { get; set; }

    /// <summary>
    /// Fecha de creación del registro
    /// </summary>
    public DateTime FechaCreacion { get; set; }

    /// <summary>
    /// Fecha de última modificación del registro
    /// </summary>
    public DateTime? FechaModificacion { get; set; }

    // ============================================
    // PROPIEDADES COMPUTADAS (desde el modelo)
    // ============================================

    /// <summary>
    /// Indica si el hotel está activo
    /// </summary>
    public bool EstaActivo { get; set; }

    /// <summary>
    /// Nombre completo con ciudad
    /// </summary>
    public string NombreCompleto { get; set; } = string.Empty;

    /// <summary>
    /// Indica si el hotel tiene servicios premium
    /// </summary>
    public bool TieneServiciosPremium { get; set; }

    /// <summary>
    /// Clasificación del hotel como texto
    /// </summary>
    public string ClasificacionTexto { get; set; } = string.Empty;
}
