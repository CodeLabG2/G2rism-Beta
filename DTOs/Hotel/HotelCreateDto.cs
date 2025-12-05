namespace G2rismBeta.API.DTOs.Hotel;

/// <summary>
/// DTO para crear un nuevo hotel
/// </summary>
public class HotelCreateDto
{
    /// <summary>
    /// ID del proveedor asociado (debe existir en la tabla proveedores)
    /// </summary>
    public int IdProveedor { get; set; }

    /// <summary>
    /// Nombre del hotel (máx 200 caracteres)
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Ciudad donde se ubica el hotel (máx 100 caracteres)
    /// </summary>
    public string Ciudad { get; set; } = string.Empty;

    /// <summary>
    /// País donde se ubica el hotel (opcional, máx 100 caracteres)
    /// </summary>
    public string? Pais { get; set; }

    /// <summary>
    /// Dirección física del hotel (máx 500 caracteres)
    /// </summary>
    public string Direccion { get; set; } = string.Empty;

    /// <summary>
    /// Información de contacto: teléfono, email, etc. (opcional, máx 200 caracteres)
    /// </summary>
    public string? Contacto { get; set; }

    /// <summary>
    /// Descripción detallada del hotel (opcional)
    /// </summary>
    public string? Descripcion { get; set; }

    /// <summary>
    /// Categoría del hotel: economico, estandar, premium, lujo (opcional, máx 50 caracteres)
    /// </summary>
    public string? Categoria { get; set; }

    /// <summary>
    /// Clasificación por estrellas: 1-5 (opcional)
    /// </summary>
    public int? Estrellas { get; set; }

    /// <summary>
    /// Precio promedio por noche (debe ser mayor a 0)
    /// </summary>
    public decimal PrecioPorNoche { get; set; }

    /// <summary>
    /// Capacidad máxima de personas por habitación (opcional)
    /// </summary>
    public int? CapacidadPorHabitacion { get; set; }

    /// <summary>
    /// Número total de habitaciones disponibles (opcional)
    /// </summary>
    public int? NumeroHabitaciones { get; set; }

    /// <summary>
    /// Indica si el hotel tiene WiFi (default: true)
    /// </summary>
    public bool TieneWifi { get; set; } = true;

    /// <summary>
    /// Indica si el hotel tiene piscina (default: false)
    /// </summary>
    public bool TienePiscina { get; set; } = false;

    /// <summary>
    /// Indica si el hotel tiene restaurante (default: false)
    /// </summary>
    public bool TieneRestaurante { get; set; } = false;

    /// <summary>
    /// Indica si el hotel tiene gimnasio (default: false)
    /// </summary>
    public bool TieneGimnasio { get; set; } = false;

    /// <summary>
    /// Indica si el hotel tiene parqueadero (default: false)
    /// </summary>
    public bool TieneParqueadero { get; set; } = false;

    /// <summary>
    /// Políticas de cancelación del hotel (opcional)
    /// </summary>
    public string? PoliticasCancelacion { get; set; }

    /// <summary>
    /// Hora de check-in en formato "HH:mm" (ej: "14:00")
    /// </summary>
    public string? CheckInHora { get; set; }

    /// <summary>
    /// Hora de check-out en formato "HH:mm" (ej: "12:00")
    /// </summary>
    public string? CheckOutHora { get; set; }

    /// <summary>
    /// URLs de fotos en formato JSON array (ej: ["url1", "url2"])
    /// </summary>
    public string? Fotos { get; set; }

    /// <summary>
    /// Servicios adicionales incluidos en formato JSON array (ej: ["desayuno", "spa"])
    /// </summary>
    public string? ServiciosIncluidos { get; set; }

    /// <summary>
    /// Estado del hotel: true = activo, false = inactivo (default: true)
    /// </summary>
    public bool Estado { get; set; } = true;
}
