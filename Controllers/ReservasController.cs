using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using G2rismBeta.API.DTOs.Reserva;
using G2rismBeta.API.Interfaces;

namespace G2rismBeta.API.Controllers;

/// <summary>
/// Controlador para la gestión de Reservas
/// Endpoints para operaciones CRUD básicas de reservas
/// Requiere autenticación. Accesible para empleados (Super Admin, Admin, Empleado).
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Super Administrador,Administrador,Empleado")]
public class ReservasController : ControllerBase
{
    private readonly IReservaService _reservaService;
    private readonly ILogger<ReservasController> _logger;

    /// <summary>
    /// Constructor: Recibe el servicio de reservas y logger por inyección de dependencias
    /// </summary>
    public ReservasController(IReservaService reservaService, ILogger<ReservasController> logger)
    {
        _reservaService = reservaService;
        _logger = logger;
    }

    // ========================================
    // ENDPOINTS DE CONSULTA (GET)
    // ========================================

    /// <summary>
    /// Obtener todas las reservas del sistema
    /// </summary>
    /// <remarks>
    /// Ejemplo de request:
    ///
    ///     GET /api/reservas
    ///
    /// </remarks>
    /// <response code="200">Lista de reservas obtenida exitosamente</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ReservaResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ReservaResponseDto>>> GetAllReservas()
    {
        try
        {
            _logger.LogInformation("📋 Obteniendo todas las reservas");
            var reservas = await _reservaService.GetAllReservasAsync();
            _logger.LogInformation($"✅ Se obtuvieron {reservas.Count()} reservas");
            return Ok(reservas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al obtener las reservas");
            return StatusCode(500, new { message = "Error al obtener las reservas", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtener una reserva específica por su ID
    /// </summary>
    /// <param name="id">ID de la reserva a buscar</param>
    /// <remarks>
    /// Ejemplo de request:
    ///
    ///     GET /api/reservas/1
    ///
    /// </remarks>
    /// <response code="200">Reserva encontrada</response>
    /// <response code="404">Reserva no encontrada</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ReservaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReservaResponseDto>> GetReservaById(int id)
    {
        try
        {
            _logger.LogInformation($"🔍 Buscando reserva con ID: {id}");
            var reserva = await _reservaService.GetReservaByIdAsync(id);

            if (reserva == null)
            {
                _logger.LogWarning($"⚠️ No se encontró la reserva con ID {id}");
                return NotFound(new { message = $"No se encontró la reserva con ID {id}" });
            }

            _logger.LogInformation($"✅ Reserva encontrada: {reserva.IdReserva}");
            return Ok(reserva);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "⚠️ Argumento inválido");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al obtener la reserva");
            return StatusCode(500, new { message = "Error al obtener la reserva", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtener todas las reservas de un cliente específico
    /// </summary>
    /// <param name="idCliente">ID del cliente</param>
    /// <remarks>
    /// Ejemplo de request:
    ///
    ///     GET /api/reservas/cliente/5
    ///
    /// </remarks>
    /// <response code="200">Lista de reservas del cliente obtenida exitosamente</response>
    /// <response code="404">Cliente no encontrado</response>
    [HttpGet("cliente/{idCliente}")]
    [ProducesResponseType(typeof(IEnumerable<ReservaResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ReservaResponseDto>>> GetReservasByCliente(int idCliente)
    {
        try
        {
            _logger.LogInformation($"🔍 Obteniendo reservas del cliente ID: {idCliente}");
            var reservas = await _reservaService.GetReservasByClienteAsync(idCliente);
            _logger.LogInformation($"✅ Se obtuvieron {reservas.Count()} reservas del cliente {idCliente}");
            return Ok(reservas);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, $"⚠️ Cliente no encontrado: {idCliente}");
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al obtener las reservas del cliente");
            return StatusCode(500, new { message = "Error al obtener las reservas del cliente", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtener reservas filtradas por estado
    /// </summary>
    /// <param name="estado">Estado de la reserva (pendiente, confirmada, cancelada, completada)</param>
    /// <remarks>
    /// Ejemplo de request:
    ///
    ///     GET /api/reservas/estado/pendiente
    ///
    /// </remarks>
    /// <response code="200">Lista de reservas con el estado especificado</response>
    /// <response code="400">Estado inválido</response>
    [HttpGet("estado/{estado}")]
    [ProducesResponseType(typeof(IEnumerable<ReservaResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ReservaResponseDto>>> GetReservasByEstado(string estado)
    {
        try
        {
            _logger.LogInformation($"🔍 Obteniendo reservas con estado: {estado}");
            var reservas = await _reservaService.GetReservasByEstadoAsync(estado);
            _logger.LogInformation($"✅ Se obtuvieron {reservas.Count()} reservas con estado '{estado}'");
            return Ok(reservas);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, $"⚠️ Estado inválido: {estado}");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al obtener las reservas por estado");
            return StatusCode(500, new { message = "Error al obtener las reservas por estado", error = ex.Message });
        }
    }

    // ========================================
    // ENDPOINTS DE MODIFICACIÓN (POST, PUT, DELETE)
    // ========================================

    /// <summary>
    /// Crear una nueva reserva básica (sin servicios)
    /// </summary>
    /// <param name="reservaCreateDto">Datos de la reserva a crear</param>
    /// <remarks>
    /// Ejemplo de request:
    ///
    ///     POST /api/reservas
    ///     {
    ///         "idCliente": 1,
    ///         "idEmpleado": 2,
    ///         "descripcion": "Viaje familiar a Cartagena",
    ///         "fechaInicioViaje": "2025-12-20",
    ///         "fechaFinViaje": "2025-12-27",
    ///         "numeroPasajeros": 4,
    ///         "estado": "pendiente",
    ///         "observaciones": "Requieren habitación con vista al mar"
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Reserva creada exitosamente</response>
    /// <response code="400">Datos inválidos o reglas de negocio no cumplidas</response>
    /// <response code="404">Cliente o empleado no encontrado</response>
    [HttpPost]
    [ProducesResponseType(typeof(ReservaResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReservaResponseDto>> CreateReserva([FromBody] ReservaCreateDto reservaCreateDto)
    {
        try
        {
            _logger.LogInformation("📝 Creando nueva reserva");
            var reservaCreada = await _reservaService.CreateReservaAsync(reservaCreateDto);
            _logger.LogInformation($"✅ Reserva creada exitosamente con ID: {reservaCreada.IdReserva}");

            return CreatedAtAction(
                nameof(GetReservaById),
                new { id = reservaCreada.IdReserva },
                reservaCreada
            );
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "⚠️ Entidad relacionada no encontrada");
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "⚠️ Argumento inválido");
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "⚠️ Operación inválida");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al crear la reserva");
            return StatusCode(500, new { message = "Error al crear la reserva", error = ex.Message });
        }
    }

    /// <summary>
    /// Actualizar una reserva existente
    /// </summary>
    /// <param name="id">ID de la reserva a actualizar</param>
    /// <param name="reservaUpdateDto">Datos a actualizar (solo campos proporcionados)</param>
    /// <remarks>
    /// Ejemplo de request (actualización parcial):
    ///
    ///     PUT /api/reservas/1
    ///     {
    ///         "estado": "confirmada",
    ///         "observaciones": "Cliente confirmó el pago inicial"
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Reserva actualizada exitosamente</response>
    /// <response code="400">Datos inválidos o reglas de negocio no cumplidas</response>
    /// <response code="404">Reserva no encontrada</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ReservaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReservaResponseDto>> UpdateReserva(int id, [FromBody] ReservaUpdateDto reservaUpdateDto)
    {
        try
        {
            _logger.LogInformation($"📝 Actualizando reserva con ID: {id}");
            var reservaActualizada = await _reservaService.UpdateReservaAsync(id, reservaUpdateDto);
            _logger.LogInformation($"✅ Reserva {id} actualizada exitosamente");
            return Ok(reservaActualizada);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, $"⚠️ Reserva no encontrada: {id}");
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "⚠️ Argumento inválido");
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "⚠️ Operación inválida");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al actualizar la reserva");
            return StatusCode(500, new { message = "Error al actualizar la reserva", error = ex.Message });
        }
    }

    // ========================================
    // ENDPOINTS ADICIONALES DE OPERACIONES DE NEGOCIO
    // ========================================

    /// <summary>
    /// Confirmar una reserva (cambiar de pendiente a confirmada)
    /// </summary>
    /// <param name="id">ID de la reserva a confirmar</param>
    /// <remarks>
    /// Ejemplo de request:
    ///
    ///     POST /api/reservas/1/confirmar
    ///
    /// </remarks>
    /// <response code="200">Reserva confirmada exitosamente</response>
    /// <response code="400">La reserva no puede ser confirmada (estado inválido)</response>
    /// <response code="404">Reserva no encontrada</response>
    [HttpPost("{id}/confirmar")]
    [ProducesResponseType(typeof(ReservaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReservaResponseDto>> ConfirmarReserva(int id)
    {
        try
        {
            _logger.LogInformation($"✅ Confirmando reserva ID: {id}");
            var reservaConfirmada = await _reservaService.ConfirmarReservaAsync(id);
            _logger.LogInformation($"✅ Reserva {id} confirmada exitosamente");
            return Ok(reservaConfirmada);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, $"⚠️ Reserva no encontrada: {id}");
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "⚠️ No se puede confirmar la reserva");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al confirmar la reserva");
            return StatusCode(500, new { message = "Error al confirmar la reserva", error = ex.Message });
        }
    }

    /// <summary>
    /// Cancelar una reserva
    /// </summary>
    /// <param name="id">ID de la reserva a cancelar</param>
    /// <param name="motivoCancelacion">Motivo de la cancelación</param>
    /// <remarks>
    /// Ejemplo de request:
    ///
    ///     POST /api/reservas/1/cancelar
    ///     {
    ///         "motivoCancelacion": "Cliente solicitó cambio de fechas"
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Reserva cancelada exitosamente</response>
    /// <response code="400">La reserva no puede ser cancelada (ya está cancelada o completada)</response>
    /// <response code="404">Reserva no encontrada</response>
    [HttpPost("{id}/cancelar")]
    [ProducesResponseType(typeof(ReservaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReservaResponseDto>> CancelarReserva(int id, [FromBody] CancelarReservaDto cancelarDto)
    {
        try
        {
            _logger.LogInformation($"❌ Cancelando reserva ID: {id}");
            var reservaCancelada = await _reservaService.CancelarReservaAsync(id, cancelarDto.MotivoCancelacion);
            _logger.LogInformation($"✅ Reserva {id} cancelada exitosamente");
            return Ok(reservaCancelada);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, $"⚠️ Reserva no encontrada: {id}");
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "⚠️ No se puede cancelar la reserva");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al cancelar la reserva");
            return StatusCode(500, new { message = "Error al cancelar la reserva", error = ex.Message });
        }
    }
}

/// <summary>
/// DTO auxiliar para la cancelación de reservas
/// </summary>
public class CancelarReservaDto
{
    /// <summary>
    /// Motivo de la cancelación
    /// </summary>
    public string MotivoCancelacion { get; set; } = string.Empty;
}