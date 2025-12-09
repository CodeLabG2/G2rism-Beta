using AutoMapper;
using G2rismBeta.API.DTOs.ReservaVuelo;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.Models;

namespace G2rismBeta.API.Services;

/// <summary>
/// Servicio para la gestión de vuelos en reservas
/// Maneja la lógica de negocio para agregar, consultar y eliminar vuelos de reservas
/// </summary>
public class ReservaVueloService : IReservaVueloService
{
    private readonly IReservaVueloRepository _reservaVueloRepository;
    private readonly IReservaRepository _reservaRepository;
    private readonly IVueloRepository _vueloRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ReservaVueloService> _logger;

    public ReservaVueloService(
        IReservaVueloRepository reservaVueloRepository,
        IReservaRepository reservaRepository,
        IVueloRepository vueloRepository,
        IMapper mapper,
        ILogger<ReservaVueloService> logger)
    {
        _reservaVueloRepository = reservaVueloRepository;
        _reservaRepository = reservaRepository;
        _vueloRepository = vueloRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Agrega un vuelo a una reserva existente
    /// </summary>
    public async Task<ReservaVueloResponseDto> AgregarVueloAReservaAsync(ReservaVueloCreateDto dto)
    {
        _logger.LogInformation("🛫 Agregando vuelo {IdVuelo} a reserva {IdReserva}", dto.IdVuelo, dto.IdReserva);

        // 1. Validar que la reserva exista
        var reserva = await _reservaRepository.GetByIdAsync(dto.IdReserva);
        if (reserva == null)
        {
            _logger.LogWarning("⚠️ Reserva {IdReserva} no encontrada", dto.IdReserva);
            throw new KeyNotFoundException($"La reserva con ID {dto.IdReserva} no existe");
        }

        // 2. Validar que el vuelo exista y tenga información completa
        var vuelo = await _vueloRepository.GetVueloConDetallesAsync(dto.IdVuelo);
        if (vuelo == null)
        {
            _logger.LogWarning("⚠️ Vuelo {IdVuelo} no encontrado", dto.IdVuelo);
            throw new KeyNotFoundException($"El vuelo con ID {dto.IdVuelo} no existe");
        }

        // 3. Validar que el vuelo esté activo
        if (!vuelo.Estado)
        {
            _logger.LogWarning("⚠️ Vuelo {IdVuelo} no está activo", dto.IdVuelo);
            throw new InvalidOperationException($"El vuelo {vuelo.NumeroVuelo} no está activo");
        }

        // 4. Validar que el vuelo no esté en el pasado
        if (vuelo.FechaSalida.Date < DateTime.Now.Date)
        {
            _logger.LogWarning("⚠️ Vuelo {IdVuelo} ya ha partido", dto.IdVuelo);
            throw new InvalidOperationException($"El vuelo {vuelo.NumeroVuelo} ya ha partido");
        }

        // 5. Validar cupos disponibles
        if (vuelo.CuposDisponibles < dto.NumeroPasajeros)
        {
            _logger.LogWarning("⚠️ Cupos insuficientes. Disponibles: {Disponibles}, Solicitados: {Solicitados}",
                vuelo.CuposDisponibles, dto.NumeroPasajeros);
            throw new InvalidOperationException(
                $"Cupos insuficientes. Disponibles: {vuelo.CuposDisponibles}, Solicitados: {dto.NumeroPasajeros}");
        }

        // 6. Validar que el número de pasajeros no exceda el total de la reserva
        if (dto.NumeroPasajeros > reserva.NumeroPasajeros)
        {
            _logger.LogWarning("⚠️ Número de pasajeros del vuelo ({Vuelo}) excede total de reserva ({Reserva})",
                dto.NumeroPasajeros, reserva.NumeroPasajeros);
            throw new InvalidOperationException(
                $"El número de pasajeros del vuelo ({dto.NumeroPasajeros}) no puede exceder el total de la reserva ({reserva.NumeroPasajeros})");
        }

        // 7. Determinar el precio según la clase
        decimal precioPorPasajero;
        if (dto.Clase.ToLower() == "ejecutiva")
        {
            if (vuelo.PrecioEjecutiva == null || vuelo.PrecioEjecutiva <= 0)
            {
                _logger.LogWarning("⚠️ Vuelo {IdVuelo} no tiene clase ejecutiva disponible", dto.IdVuelo);
                throw new InvalidOperationException("Este vuelo no tiene clase ejecutiva disponible");
            }
            precioPorPasajero = vuelo.PrecioEjecutiva.Value;
        }
        else
        {
            precioPorPasajero = vuelo.PrecioEconomica;
        }

        // 8. Calcular el subtotal
        decimal subtotal = precioPorPasajero * dto.NumeroPasajeros;

        // 9. Crear la entidad ReservaVuelo
        var reservaVuelo = new ReservaVuelo
        {
            IdReserva = dto.IdReserva,
            IdVuelo = dto.IdVuelo,
            NumeroPasajeros = dto.NumeroPasajeros,
            Clase = dto.Clase.ToLower(),
            AsientosAsignados = dto.AsientosAsignados,
            PrecioPorPasajero = precioPorPasajero,
            Subtotal = subtotal,
            EquipajeIncluido = dto.EquipajeIncluido,
            EquipajeExtra = dto.EquipajeExtra,
            CostoEquipajeExtra = dto.CostoEquipajeExtra,
            FechaAgregado = DateTime.Now
        };

        // 10. Guardar la reserva de vuelo
        await _reservaVueloRepository.AddAsync(reservaVuelo);
        await _reservaVueloRepository.SaveChangesAsync();

        _logger.LogInformation("✅ Vuelo agregado a la reserva. Subtotal: {Subtotal:C}", subtotal);

        // 11. Descontar cupos del vuelo
        vuelo.CuposDisponibles -= dto.NumeroPasajeros;
        await _vueloRepository.SaveChangesAsync();

        _logger.LogInformation("📊 Cupos actualizados. Restantes: {CuposDisponibles}", vuelo.CuposDisponibles);

        // 12. Actualizar el MontoTotal de la reserva
        decimal costoTotal = subtotal + dto.CostoEquipajeExtra;
        reserva.MontoTotal += costoTotal;
        reserva.SaldoPendiente = reserva.MontoTotal - reserva.MontoPagado;
        reserva.FechaModificacion = DateTime.Now;
        await _reservaRepository.SaveChangesAsync();

        _logger.LogInformation("💰 MontoTotal actualizado: {MontoTotal:C}", reserva.MontoTotal);

        // 13. Obtener la reserva de vuelo completa para la respuesta
        var reservaVueloCompleta = await _reservaVueloRepository.GetReservaVueloConDetallesAsync(reservaVuelo.Id);

        // 14. Mapear a DTO de respuesta
        var response = _mapper.Map<ReservaVueloResponseDto>(reservaVueloCompleta);

        return response;
    }

    /// <summary>
    /// Obtiene todos los vuelos de una reserva
    /// </summary>
    public async Task<IEnumerable<ReservaVueloResponseDto>> GetVuelosPorReservaAsync(int idReserva)
    {
        _logger.LogInformation("🔍 Obteniendo vuelos de la reserva {IdReserva}", idReserva);

        // Validar que la reserva exista
        var reservaExiste = await _reservaRepository.ExisteReservaAsync(idReserva);
        if (!reservaExiste)
        {
            _logger.LogWarning("⚠️ Reserva {IdReserva} no encontrada", idReserva);
            throw new KeyNotFoundException($"La reserva con ID {idReserva} no existe");
        }

        var reservasVuelos = await _reservaVueloRepository.GetVuelosByReservaIdAsync(idReserva);
        var response = _mapper.Map<IEnumerable<ReservaVueloResponseDto>>(reservasVuelos);

        _logger.LogInformation("✅ Se encontraron {Count} vuelos en la reserva", response.Count());

        return response;
    }

    /// <summary>
    /// Obtiene un vuelo específico de una reserva
    /// </summary>
    public async Task<ReservaVueloResponseDto> GetReservaVueloPorIdAsync(int id)
    {
        _logger.LogInformation("🔍 Obteniendo detalles de reserva de vuelo {Id}", id);

        var reservaVuelo = await _reservaVueloRepository.GetReservaVueloConDetallesAsync(id);
        if (reservaVuelo == null)
        {
            _logger.LogWarning("⚠️ Reserva de vuelo {Id} no encontrada", id);
            throw new KeyNotFoundException($"La reserva de vuelo con ID {id} no existe");
        }

        var response = _mapper.Map<ReservaVueloResponseDto>(reservaVuelo);

        _logger.LogInformation("✅ Reserva de vuelo encontrada");

        return response;
    }

    /// <summary>
    /// Elimina un vuelo de una reserva
    /// </summary>
    public async Task<bool> EliminarVueloDeReservaAsync(int id)
    {
        _logger.LogInformation("🗑️ Eliminando vuelo de reserva {Id}", id);

        // 1. Obtener la reserva de vuelo con todos sus datos
        var reservaVuelo = await _reservaVueloRepository.GetReservaVueloConDetallesAsync(id);
        if (reservaVuelo == null)
        {
            _logger.LogWarning("⚠️ Reserva de vuelo {Id} no encontrada", id);
            throw new KeyNotFoundException($"La reserva de vuelo con ID {id} no existe");
        }

        // 2. Obtener la reserva para actualizar el monto total
        var reserva = await _reservaRepository.GetByIdAsync(reservaVuelo.IdReserva);
        if (reserva == null)
        {
            _logger.LogWarning("⚠️ Reserva {IdReserva} no encontrada", reservaVuelo.IdReserva);
            throw new KeyNotFoundException($"La reserva con ID {reservaVuelo.IdReserva} no existe");
        }

        // 3. Obtener el vuelo para devolver los cupos
        var vuelo = await _vueloRepository.GetByIdAsync(reservaVuelo.IdVuelo);

        // 4. Calcular el monto a restar
        decimal costoTotal = reservaVuelo.Subtotal + reservaVuelo.CostoEquipajeExtra;

        // 5. Eliminar la reserva de vuelo
        await _reservaVueloRepository.DeleteAsync(id);
        await _reservaVueloRepository.SaveChangesAsync();

        _logger.LogInformation("✅ Vuelo eliminado de la reserva");

        // 6. Devolver los cupos al vuelo (si existe)
        if (vuelo != null)
        {
            vuelo.CuposDisponibles += reservaVuelo.NumeroPasajeros;
            await _vueloRepository.SaveChangesAsync();
            _logger.LogInformation("📊 Cupos devueltos. Disponibles ahora: {CuposDisponibles}", vuelo.CuposDisponibles);
        }

        // 7. Actualizar el MontoTotal de la reserva
        reserva.MontoTotal -= costoTotal;
        reserva.SaldoPendiente = reserva.MontoTotal - reserva.MontoPagado;
        reserva.FechaModificacion = DateTime.Now;
        await _reservaRepository.SaveChangesAsync();

        _logger.LogInformation("💰 MontoTotal actualizado: {MontoTotal:C}", reserva.MontoTotal);

        return true;
    }
}
