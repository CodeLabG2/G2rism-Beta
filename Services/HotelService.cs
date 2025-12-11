using AutoMapper;
using G2rismBeta.API.DTOs.Hotel;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.Models;
using Microsoft.Extensions.Logging;

namespace G2rismBeta.API.Services;

/// <summary>
/// Servicio de hoteles con lógica de negocio y validaciones
/// </summary>
public class HotelService : IHotelService
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IProveedorRepository _proveedorRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<HotelService> _logger;

    public HotelService(
        IHotelRepository hotelRepository,
        IProveedorRepository proveedorRepository,
        IMapper mapper,
        ILogger<HotelService> logger)
    {
        _hotelRepository = hotelRepository;
        _proveedorRepository = proveedorRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<HotelResponseDto>> GetAllAsync()
    {
        _logger.LogInformation("🏨 Obteniendo todos los hoteles");
        var hoteles = await _hotelRepository.GetAllConProveedorAsync();
        return _mapper.Map<IEnumerable<HotelResponseDto>>(hoteles);
    }

    /// <inheritdoc/>
    public async Task<HotelResponseDto> GetByIdAsync(int id)
    {
        _logger.LogInformation("🔍 Buscando hotel con ID: {Id}", id);

        var hotel = await _hotelRepository.GetByIdConProveedorAsync(id);
        if (hotel == null)
        {
            _logger.LogWarning("⚠️ Hotel con ID {Id} no encontrado", id);
            throw new KeyNotFoundException($"Hotel con ID {id} no encontrado");
        }

        return _mapper.Map<HotelResponseDto>(hotel);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<HotelResponseDto>> GetByCiudadAsync(string ciudad)
    {
        _logger.LogInformation("🔍 Buscando hoteles en ciudad: {Ciudad}", ciudad);
        var hoteles = await _hotelRepository.GetByCiudadAsync(ciudad);
        return _mapper.Map<IEnumerable<HotelResponseDto>>(hoteles);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<HotelResponseDto>> GetByPaisAsync(string pais)
    {
        _logger.LogInformation("🔍 Buscando hoteles en país: {Pais}", pais);
        var hoteles = await _hotelRepository.GetByPaisAsync(pais);
        return _mapper.Map<IEnumerable<HotelResponseDto>>(hoteles);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<HotelResponseDto>> GetByEstrellasAsync(int estrellas)
    {
        if (estrellas < 1 || estrellas > 5)
        {
            _logger.LogWarning("⚠️ Clasificación de estrellas inválida: {Estrellas}", estrellas);
            throw new ArgumentException("La clasificación debe estar entre 1 y 5 estrellas", nameof(estrellas));
        }

        _logger.LogInformation("⭐ Buscando hoteles con {Estrellas} estrellas", estrellas);
        var hoteles = await _hotelRepository.GetByEstrellasAsync(estrellas);
        return _mapper.Map<IEnumerable<HotelResponseDto>>(hoteles);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<HotelResponseDto>> GetByCategoriaAsync(string categoria)
    {
        var categoriasValidas = new[] { "economico", "estandar", "premium", "lujo" };
        if (!categoriasValidas.Contains(categoria.ToLower()))
        {
            _logger.LogWarning("⚠️ Categoría inválida: {Categoria}", categoria);
            throw new ArgumentException(
                $"Categoría inválida. Valores permitidos: {string.Join(", ", categoriasValidas)}",
                nameof(categoria));
        }

        _logger.LogInformation("🔍 Buscando hoteles en categoría: {Categoria}", categoria);
        var hoteles = await _hotelRepository.GetByCategoriaAsync(categoria);
        return _mapper.Map<IEnumerable<HotelResponseDto>>(hoteles);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<HotelResponseDto>> GetActivosAsync()
    {
        _logger.LogInformation("✅ Obteniendo hoteles activos");
        var hoteles = await _hotelRepository.GetActivosAsync();
        return _mapper.Map<IEnumerable<HotelResponseDto>>(hoteles);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<HotelResponseDto>> GetByRangoPrecioAsync(decimal precioMin, decimal precioMax)
    {
        if (precioMin < 0 || precioMax < 0)
        {
            _logger.LogWarning("⚠️ Precios negativos no permitidos");
            throw new ArgumentException("Los precios deben ser mayores o iguales a cero");
        }

        if (precioMin > precioMax)
        {
            _logger.LogWarning("⚠️ Precio mínimo mayor que precio máximo");
            throw new ArgumentException("El precio mínimo no puede ser mayor que el precio máximo");
        }

        _logger.LogInformation("💰 Buscando hoteles con precio entre {Min} y {Max}", precioMin, precioMax);
        var hoteles = await _hotelRepository.GetByRangoPrecioAsync(precioMin, precioMax);
        return _mapper.Map<IEnumerable<HotelResponseDto>>(hoteles);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<HotelResponseDto>> GetByServiciosAsync(
        bool? wifi = null,
        bool? piscina = null,
        bool? restaurante = null,
        bool? gimnasio = null,
        bool? parqueadero = null)
    {
        _logger.LogInformation("🔍 Buscando hoteles con servicios específicos");
        var hoteles = await _hotelRepository.GetByServiciosAsync(wifi, piscina, restaurante, gimnasio, parqueadero);
        return _mapper.Map<IEnumerable<HotelResponseDto>>(hoteles);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<HotelResponseDto>> GetPremiumAsync()
    {
        _logger.LogInformation("⭐ Obteniendo hoteles premium");
        var hoteles = await _hotelRepository.GetPremiumAsync();
        return _mapper.Map<IEnumerable<HotelResponseDto>>(hoteles);
    }

    /// <inheritdoc/>
    public async Task<HotelResponseDto> CreateAsync(HotelCreateDto hotelDto)
    {
        _logger.LogInformation("📝 Creando nuevo hotel: {Nombre}", hotelDto.Nombre);

        // Validar que el proveedor existe
        var proveedor = await _proveedorRepository.GetByIdAsync(hotelDto.IdProveedor);
        if (proveedor == null)
        {
            _logger.LogWarning("⚠️ Proveedor con ID {Id} no encontrado", hotelDto.IdProveedor);
            throw new ArgumentException($"Proveedor con ID {hotelDto.IdProveedor} no encontrado", nameof(hotelDto.IdProveedor));
        }

        // Validar que no exista un hotel con el mismo nombre en la misma ciudad
        if (await _hotelRepository.ExistePorNombreYCiudadAsync(hotelDto.Nombre, hotelDto.Ciudad))
        {
            _logger.LogWarning("⚠️ Ya existe un hotel con nombre '{Nombre}' en {Ciudad}", hotelDto.Nombre, hotelDto.Ciudad);
            throw new ArgumentException($"Ya existe un hotel con el nombre '{hotelDto.Nombre}' en {hotelDto.Ciudad}");
        }

        var hotel = _mapper.Map<Hotel>(hotelDto);
        hotel.FechaCreacion = DateTime.Now;

        // Convertir horas de string a TimeSpan
        if (!string.IsNullOrEmpty(hotelDto.CheckInHora) && TimeSpan.TryParse(hotelDto.CheckInHora, out var checkIn))
        {
            hotel.CheckInHora = checkIn;
        }

        if (!string.IsNullOrEmpty(hotelDto.CheckOutHora) && TimeSpan.TryParse(hotelDto.CheckOutHora, out var checkOut))
        {
            hotel.CheckOutHora = checkOut;
        }

        await _hotelRepository.AddAsync(hotel);
        await _hotelRepository.SaveChangesAsync();

        _logger.LogInformation("✅ Hotel '{Nombre}' creado exitosamente con ID {Id}", hotel.Nombre, hotel.IdHotel);

        return await GetByIdAsync(hotel.IdHotel);
    }

    /// <inheritdoc/>
    public async Task<HotelResponseDto> UpdateAsync(int id, HotelUpdateDto hotelDto)
    {
        _logger.LogInformation("🔄 Actualizando hotel con ID: {Id}", id);

        var hotel = await _hotelRepository.GetByIdAsync(id);
        if (hotel == null)
        {
            _logger.LogWarning("⚠️ Hotel con ID {Id} no encontrado", id);
            throw new KeyNotFoundException($"Hotel con ID {id} no encontrado");
        }

        // Validar proveedor si se está actualizando
        if (hotelDto.IdProveedor.HasValue)
        {
            var proveedor = await _proveedorRepository.GetByIdAsync(hotelDto.IdProveedor.Value);
            if (proveedor == null)
            {
                _logger.LogWarning("⚠️ Proveedor con ID {Id} no encontrado", hotelDto.IdProveedor.Value);
                throw new ArgumentException($"Proveedor con ID {hotelDto.IdProveedor.Value} no encontrado");
            }
        }

        // Validar nombre y ciudad si se están actualizando
        var nombreActualizado = hotelDto.Nombre ?? hotel.Nombre;
        var ciudadActualizada = hotelDto.Ciudad ?? hotel.Ciudad;

        if (await _hotelRepository.ExistePorNombreYCiudadAsync(nombreActualizado, ciudadActualizada, id))
        {
            _logger.LogWarning("⚠️ Ya existe otro hotel con nombre '{Nombre}' en {Ciudad}", nombreActualizado, ciudadActualizada);
            throw new ArgumentException($"Ya existe otro hotel con el nombre '{nombreActualizado}' en {ciudadActualizada}");
        }

        // Actualizar campos individualmente solo si no son null
        if (hotelDto.IdProveedor.HasValue && hotelDto.IdProveedor.Value > 0)
            hotel.IdProveedor = hotelDto.IdProveedor.Value;

        if (!string.IsNullOrEmpty(hotelDto.Nombre))
            hotel.Nombre = hotelDto.Nombre;

        if (!string.IsNullOrEmpty(hotelDto.Ciudad))
            hotel.Ciudad = hotelDto.Ciudad;

        if (hotelDto.Pais != null)
            hotel.Pais = hotelDto.Pais;

        if (!string.IsNullOrEmpty(hotelDto.Direccion))
            hotel.Direccion = hotelDto.Direccion;

        if (hotelDto.Contacto != null)
            hotel.Contacto = hotelDto.Contacto;

        if (hotelDto.Descripcion != null)
            hotel.Descripcion = hotelDto.Descripcion;

        if (hotelDto.Categoria != null)
            hotel.Categoria = hotelDto.Categoria;

        if (hotelDto.Estrellas.HasValue)
            hotel.Estrellas = hotelDto.Estrellas.Value;

        if (hotelDto.PrecioPorNoche.HasValue)
            hotel.PrecioPorNoche = hotelDto.PrecioPorNoche.Value;

        if (hotelDto.CapacidadPorHabitacion.HasValue)
            hotel.CapacidadPorHabitacion = hotelDto.CapacidadPorHabitacion.Value;

        if (hotelDto.NumeroHabitaciones.HasValue)
            hotel.NumeroHabitaciones = hotelDto.NumeroHabitaciones.Value;

        if (hotelDto.TieneWifi.HasValue)
            hotel.TieneWifi = hotelDto.TieneWifi.Value;

        if (hotelDto.TienePiscina.HasValue)
            hotel.TienePiscina = hotelDto.TienePiscina.Value;

        if (hotelDto.TieneRestaurante.HasValue)
            hotel.TieneRestaurante = hotelDto.TieneRestaurante.Value;

        if (hotelDto.TieneGimnasio.HasValue)
            hotel.TieneGimnasio = hotelDto.TieneGimnasio.Value;

        if (hotelDto.TieneParqueadero.HasValue)
            hotel.TieneParqueadero = hotelDto.TieneParqueadero.Value;

        if (hotelDto.PoliticasCancelacion != null)
            hotel.PoliticasCancelacion = hotelDto.PoliticasCancelacion;

        if (hotelDto.Fotos != null)
            hotel.Fotos = hotelDto.Fotos;

        if (hotelDto.ServiciosIncluidos != null)
            hotel.ServiciosIncluidos = hotelDto.ServiciosIncluidos;

        if (hotelDto.Estado.HasValue)
            hotel.Estado = hotelDto.Estado.Value;

        // Actualizar horas si se proporcionaron
        if (!string.IsNullOrEmpty(hotelDto.CheckInHora) && TimeSpan.TryParse(hotelDto.CheckInHora, out var checkIn))
        {
            hotel.CheckInHora = checkIn;
        }

        if (!string.IsNullOrEmpty(hotelDto.CheckOutHora) && TimeSpan.TryParse(hotelDto.CheckOutHora, out var checkOut))
        {
            hotel.CheckOutHora = checkOut;
        }

        hotel.FechaModificacion = DateTime.Now;

        await _hotelRepository.UpdateAsync(hotel);
        await _hotelRepository.SaveChangesAsync();

        _logger.LogInformation("✅ Hotel con ID {Id} actualizado exitosamente", id);

        return await GetByIdAsync(id);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation("🗑️ Eliminando hotel con ID: {Id}", id);

        var hotel = await _hotelRepository.GetByIdAsync(id);
        if (hotel == null)
        {
            _logger.LogWarning("⚠️ Hotel con ID {Id} no encontrado", id);
            throw new KeyNotFoundException($"Hotel con ID {id} no encontrado");
        }

        // Soft delete - cambiar estado a inactivo
        hotel.Estado = false;
        hotel.FechaModificacion = DateTime.Now;

        await _hotelRepository.UpdateAsync(hotel);
        await _hotelRepository.SaveChangesAsync();

        _logger.LogInformation("✅ Hotel con ID {Id} eliminado exitosamente (soft delete)", id);

        return true;
    }
}
