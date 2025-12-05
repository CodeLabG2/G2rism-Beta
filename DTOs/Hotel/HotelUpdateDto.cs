namespace G2rismBeta.API.DTOs.Hotel;

/// <summary>
/// DTO para actualizar un hotel existente
/// Todos los campos son opcionales (nullable) para permitir actualizaciones parciales
/// </summary>
public class HotelUpdateDto
{
    /// <summary>
    /// ID del proveedor asociado (opcional)
    /// </summary>
    public int? IdProveedor { get; set; }

    /// <summary>
    /// Nombre del hotel (opcional, máx 200 caracteres)
    /// </summary>
    public string? Nombre { get; set; }

    /// <summary>
    /// Ciudad donde se ubica el hotel (opcional, máx 100 caracteres)
    /// </summary>
    public string? Ciudad { get; set; }

    /// <summary>
    /// País donde se ubica el hotel (opcional, máx 100 caracteres)
    /// </summary>
    public string? Pais { get; set; }

    /// <summary>
    /// Dirección física del hotel (opcional, máx 500 caracteres)
    /// </summary>
    public string? Direccion { get; set; }

    /// <summary>
    /// Información de contacto (opcional, máx 200 caracteres)
    /// </summary>
    public string? Contacto { get; set; }

    /// <summary>
    /// Descripción detallada del hotel (opcional)
    /// </summary>
    public string? Descripcion { get; set; }

    /// <summary>
    /// Categoría del hotel (opcional, máx 50 caracteres)
    /// </summary>
    public string? Categoria { get; set; }

    /// <summary>
    /// Clasificación por estrellas: 1-5 (opcional)
    /// </summary>
    public int? Estrellas { get; set; }

    /// <summary>
    /// Precio promedio por noche (opcional, debe ser mayor a 0)
    /// </summary>
    public decimal? PrecioPorNoche { get; set; }

    /// <summary>
    /// Capacidad máxima de personas por habitación (opcional)
    /// </summary>
    public int? CapacidadPorHabitacion { get; set; }

    /// <summary>
    /// Número total de habitaciones disponibles (opcional)
    /// </summary>
    public int? NumeroHabitaciones { get; set; }

    /// <summary>
    /// Indica si el hotel tiene WiFi (opcional)
    /// </summary>
    public bool? TieneWifi { get; set; }

    /// <summary>
    /// Indica si el hotel tiene piscina (opcional)
    /// </summary>
    public bool? TienePiscina { get; set; }

    /// <summary>
    /// Indica si el hotel tiene restaurante (opcional)
    /// </summary>
    public bool? TieneRestaurante { get; set; }

    /// <summary>
    /// Indica si el hotel tiene gimnasio (opcional)
    /// </summary>
    public bool? TieneGimnasio { get; set; }

    /// <summary>
    /// Indica si el hotel tiene parqueadero (opcional)
    /// </summary>
    public bool? TieneParqueadero { get; set; }

    /// <summary>
    /// Políticas de cancelación del hotel (opcional)
    /// </summary>
    public string? PoliticasCancelacion { get; set; }

    /// <summary>
    /// Hora de check-in en formato "HH:mm" (opcional)
    /// </summary>
    public string? CheckInHora { get; set; }

    /// <summary>
    /// Hora de check-out en formato "HH:mm" (opcional)
    /// </summary>
    public string? CheckOutHora { get; set; }

    /// <summary>
    /// URLs de fotos en formato JSON array (opcional)
    /// </summary>
    public string? Fotos { get; set; }

    /// <summary>
    /// Servicios adicionales incluidos en formato JSON array (opcional)
    /// </summary>
    public string? ServiciosIncluidos { get; set; }

    /// <summary>
    /// Estado del hotel (opcional)
    /// </summary>
    public bool? Estado { get; set; }
}
