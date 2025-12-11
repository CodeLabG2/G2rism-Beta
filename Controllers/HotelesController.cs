using AutoMapper;
using G2rismBeta.API.DTOs.Hotel;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace G2rismBeta.API.Controllers;

/// <summary>
/// Controlador para la gestión de hoteles
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class HotelesController : ControllerBase
{
    private readonly IHotelService _hotelService;
    private readonly IMapper _mapper;
    private readonly ILogger<HotelesController> _logger;

    public HotelesController(
        IHotelService hotelService,
        IMapper mapper,
        ILogger<HotelesController> logger)
    {
        _hotelService = hotelService;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los hoteles registrados
    /// </summary>
    /// <returns>Lista de hoteles con información del proveedor</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<HotelResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<HotelResponseDto>>>> GetAll()
    {
        _logger.LogInformation("🏨 GET /api/hoteles - Obteniendo todos los hoteles");

        var hoteles = await _hotelService.GetAllAsync();

        return Ok(new ApiResponse<IEnumerable<HotelResponseDto>>
        {
            Success = true,
            Message = "Hoteles obtenidos exitosamente",
            Data = hoteles,
            Timestamp = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Obtiene un hotel por su ID
    /// </summary>
    /// <param name="id">ID del hotel</param>
    /// <returns>Información completa del hotel</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<HotelResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<HotelResponseDto>>> GetById(int id)
    {
        _logger.LogInformation("🔍 GET /api/hoteles/{Id} - Buscando hotel", id);

        var hotel = await _hotelService.GetByIdAsync(id);

        return Ok(new ApiResponse<HotelResponseDto>
        {
            Success = true,
            Message = "Hotel encontrado exitosamente",
            Data = hotel,
            Timestamp = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Busca hoteles por ciudad
    /// </summary>
    /// <param name="ciudad">Nombre de la ciudad</param>
    /// <returns>Lista de hoteles en la ciudad especificada</returns>
    [HttpGet("ciudad/{ciudad}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<HotelResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<HotelResponseDto>>>> GetByCiudad(string ciudad)
    {
        _logger.LogInformation("🔍 GET /api/hoteles/ciudad/{Ciudad} - Buscando hoteles", ciudad);

        var hoteles = await _hotelService.GetByCiudadAsync(ciudad);

        return Ok(new ApiResponse<IEnumerable<HotelResponseDto>>
        {
            Success = true,
            Message = $"Hoteles en {ciudad} obtenidos exitosamente",
            Data = hoteles,
            Timestamp = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Busca hoteles por país
    /// </summary>
    /// <param name="pais">Nombre del país</param>
    /// <returns>Lista de hoteles en el país especificado</returns>
    [HttpGet("pais/{pais}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<HotelResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<HotelResponseDto>>>> GetByPais(string pais)
    {
        _logger.LogInformation("🔍 GET /api/hoteles/pais/{Pais} - Buscando hoteles", pais);

        var hoteles = await _hotelService.GetByPaisAsync(pais);

        return Ok(new ApiResponse<IEnumerable<HotelResponseDto>>
        {
            Success = true,
            Message = $"Hoteles en {pais} obtenidos exitosamente",
            Data = hoteles,
            Timestamp = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Busca hoteles por clasificación de estrellas
    /// </summary>
    /// <param name="estrellas">Número de estrellas (1-5)</param>
    /// <returns>Lista de hoteles con la clasificación especificada</returns>
    [HttpGet("estrellas/{estrellas}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<HotelResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<IEnumerable<HotelResponseDto>>>> GetByEstrellas(int estrellas)
    {
        _logger.LogInformation("⭐ GET /api/hoteles/estrellas/{Estrellas} - Buscando hoteles", estrellas);

        var hoteles = await _hotelService.GetByEstrellasAsync(estrellas);

        return Ok(new ApiResponse<IEnumerable<HotelResponseDto>>
        {
            Success = true,
            Message = $"Hoteles con {estrellas} estrellas obtenidos exitosamente",
            Data = hoteles,
            Timestamp = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Busca hoteles por categoría
    /// </summary>
    /// <param name="categoria">Categoría del hotel (economico, estandar, premium, lujo)</param>
    /// <returns>Lista de hoteles en la categoría</returns>
    [HttpGet("categoria/{categoria}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<HotelResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<IEnumerable<HotelResponseDto>>>> GetByCategoria(string categoria)
    {
        _logger.LogInformation("🔍 GET /api/hoteles/categoria/{Categoria} - Buscando hoteles", categoria);

        var hoteles = await _hotelService.GetByCategoriaAsync(categoria);

        return Ok(new ApiResponse<IEnumerable<HotelResponseDto>>
        {
            Success = true,
            Message = $"Hoteles de categoría {categoria} obtenidos exitosamente",
            Data = hoteles,
            Timestamp = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Obtiene todos los hoteles activos
    /// </summary>
    /// <returns>Lista de hoteles activos</returns>
    [HttpGet("activos")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<HotelResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<HotelResponseDto>>>> GetActivos()
    {
        _logger.LogInformation("✅ GET /api/hoteles/activos - Obteniendo hoteles activos");

        var hoteles = await _hotelService.GetActivosAsync();

        return Ok(new ApiResponse<IEnumerable<HotelResponseDto>>
        {
            Success = true,
            Message = "Hoteles activos obtenidos exitosamente",
            Data = hoteles,
            Timestamp = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Busca hoteles premium (con servicios adicionales)
    /// </summary>
    /// <returns>Lista de hoteles premium</returns>
    [HttpGet("premium")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<HotelResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<HotelResponseDto>>>> GetPremium()
    {
        _logger.LogInformation("⭐ GET /api/hoteles/premium - Obteniendo hoteles premium");

        var hoteles = await _hotelService.GetPremiumAsync();

        return Ok(new ApiResponse<IEnumerable<HotelResponseDto>>
        {
            Success = true,
            Message = "Hoteles premium obtenidos exitosamente",
            Data = hoteles,
            Timestamp = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Busca hoteles por rango de precio
    /// </summary>
    /// <param name="precioMin">Precio mínimo por noche</param>
    /// <param name="precioMax">Precio máximo por noche</param>
    /// <returns>Lista de hoteles en el rango de precio</returns>
    [HttpGet("buscar")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<HotelResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<IEnumerable<HotelResponseDto>>>> GetByRangoPrecio(
        [FromQuery] decimal precioMin,
        [FromQuery] decimal precioMax)
    {
        _logger.LogInformation("💰 GET /api/hoteles/buscar?precioMin={Min}&precioMax={Max}", precioMin, precioMax);

        var hoteles = await _hotelService.GetByRangoPrecioAsync(precioMin, precioMax);

        return Ok(new ApiResponse<IEnumerable<HotelResponseDto>>
        {
            Success = true,
            Message = $"Hoteles con precio entre {precioMin:C} y {precioMax:C} obtenidos exitosamente",
            Data = hoteles,
            Timestamp = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Busca hoteles con servicios específicos
    /// </summary>
    /// <param name="wifi">Requiere WiFi</param>
    /// <param name="piscina">Requiere piscina</param>
    /// <param name="restaurante">Requiere restaurante</param>
    /// <param name="gimnasio">Requiere gimnasio</param>
    /// <param name="parqueadero">Requiere parqueadero</param>
    /// <returns>Lista de hoteles que cumplen los requisitos</returns>
    [HttpGet("servicios")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<HotelResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<HotelResponseDto>>>> GetByServicios(
        [FromQuery] bool? wifi = null,
        [FromQuery] bool? piscina = null,
        [FromQuery] bool? restaurante = null,
        [FromQuery] bool? gimnasio = null,
        [FromQuery] bool? parqueadero = null)
    {
        _logger.LogInformation("🔍 GET /api/hoteles/servicios - Buscando hoteles con servicios específicos");

        var hoteles = await _hotelService.GetByServiciosAsync(wifi, piscina, restaurante, gimnasio, parqueadero);

        return Ok(new ApiResponse<IEnumerable<HotelResponseDto>>
        {
            Success = true,
            Message = "Hoteles con servicios específicos obtenidos exitosamente",
            Data = hoteles,
            Timestamp = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Crea un nuevo hotel
    /// </summary>
    /// <param name="hotelDto">Datos del hotel a crear</param>
    /// <returns>Hotel creado</returns>
    [HttpPost]
    [Authorize(Policy = "RequirePermission:hoteles.crear")]
    [ProducesResponseType(typeof(ApiResponse<HotelResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<HotelResponseDto>>> Create([FromBody] HotelCreateDto hotelDto)
    {
        _logger.LogInformation("📝 POST /api/hoteles - Creando nuevo hotel: {Nombre}", hotelDto.Nombre);

        var hotel = await _hotelService.CreateAsync(hotelDto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = hotel.IdHotel },
            new ApiResponse<HotelResponseDto>
            {
                Success = true,
                Message = "Hotel creado exitosamente",
                Data = hotel,
                Timestamp = DateTime.UtcNow
            });
    }

    /// <summary>
    /// Actualiza un hotel existente
    /// </summary>
    /// <param name="id">ID del hotel a actualizar</param>
    /// <param name="hotelDto">Datos a actualizar</param>
    /// <returns>Hotel actualizado</returns>
    [HttpPut("{id}")]
    [Authorize(Policy = "RequirePermission:hoteles.actualizar")]
    [ProducesResponseType(typeof(ApiResponse<HotelResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<HotelResponseDto>>> Update(
        int id,
        [FromBody] HotelUpdateDto hotelDto)
    {
        _logger.LogInformation("🔄 PUT /api/hoteles/{Id} - Actualizando hotel", id);

        var hotel = await _hotelService.UpdateAsync(id, hotelDto);

        return Ok(new ApiResponse<HotelResponseDto>
        {
            Success = true,
            Message = "Hotel actualizado exitosamente",
            Data = hotel,
            Timestamp = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Elimina un hotel (soft delete - cambia estado a inactivo)
    /// </summary>
    /// <param name="id">ID del hotel a eliminar</param>
    /// <returns>Confirmación de eliminación</returns>
    [HttpDelete("{id}")]
    [Authorize(Policy = "RequirePermission:hoteles.eliminar")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        _logger.LogInformation("🗑️ DELETE /api/hoteles/{Id} - Eliminando hotel", id);

        await _hotelService.DeleteAsync(id);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Hotel eliminado exitosamente (soft delete)",
            Data = new { IdHotel = id, Eliminado = true },
            Timestamp = DateTime.UtcNow
        });
    }
}
