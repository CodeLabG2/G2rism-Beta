namespace G2rismBeta.API.DTOs.ReservaVuelo;

/// <summary>
/// DTO de respuesta para mostrar información de una relación Reserva-Vuelo
/// </summary>
public class ReservaVueloResponseDto
{
    /// <summary>
    /// Identificador único
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ID de la reserva asociada
    /// </summary>
    public int IdReserva { get; set; }

    /// <summary>
    /// ID del vuelo asociado
    /// </summary>
    public int IdVuelo { get; set; }

    /// <summary>
    /// Número de vuelo (para facilitar visualización)
    /// </summary>
    public string? NumeroVuelo { get; set; }

    /// <summary>
    /// Origen del vuelo
    /// </summary>
    public string? Origen { get; set; }

    /// <summary>
    /// Destino del vuelo
    /// </summary>
    public string? Destino { get; set; }

    /// <summary>
    /// Fecha de salida del vuelo
    /// </summary>
    public DateTime? FechaSalida { get; set; }

    /// <summary>
    /// Hora de salida del vuelo
    /// </summary>
    public TimeSpan? HoraSalida { get; set; }

    /// <summary>
    /// Fecha de llegada del vuelo
    /// </summary>
    public DateTime? FechaLlegada { get; set; }

    /// <summary>
    /// Nombre de la aerolínea
    /// </summary>
    public string? NombreAerolinea { get; set; }

    /// <summary>
    /// Número de pasajeros
    /// </summary>
    public int NumeroPasajeros { get; set; }

    /// <summary>
    /// Clase del vuelo
    /// </summary>
    public string Clase { get; set; } = string.Empty;

    /// <summary>
    /// Asientos asignados
    /// </summary>
    public string? AsientosAsignados { get; set; }

    /// <summary>
    /// Precio por pasajero
    /// </summary>
    public decimal PrecioPorPasajero { get; set; }

    /// <summary>
    /// Subtotal (NumeroPasajeros * PrecioPorPasajero)
    /// </summary>
    public decimal Subtotal { get; set; }

    /// <summary>
    /// Indica si el equipaje está incluido
    /// </summary>
    public bool EquipajeIncluido { get; set; }

    /// <summary>
    /// Equipaje extra en kilogramos
    /// </summary>
    public int? EquipajeExtra { get; set; }

    /// <summary>
    /// Costo adicional por equipaje extra
    /// </summary>
    public decimal CostoEquipajeExtra { get; set; }

    /// <summary>
    /// Costo total (Subtotal + CostoEquipajeExtra)
    /// </summary>
    public decimal CostoTotal { get; set; }

    /// <summary>
    /// Fecha en que se agregó el vuelo a la reserva
    /// </summary>
    public DateTime FechaAgregado { get; set; }

    /// <summary>
    /// Indica si es clase ejecutiva
    /// </summary>
    public bool EsClaseEjecutiva { get; set; }

    /// <summary>
    /// Indica si tiene equipaje extra
    /// </summary>
    public bool TieneEquipajeExtra { get; set; }
}