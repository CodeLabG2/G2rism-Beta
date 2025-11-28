using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.DTOs.ContratoProveedor;
using G2rismBeta.API.Models;

namespace G2rismBeta.API.Controllers;

/// <summary>
/// Controlador para gestión de Contratos con Proveedores
/// Maneja la creación, consulta, actualización y gestión del ciclo de vida de contratos
/// Requiere autenticación. Accesible para empleados (Super Admin, Admin, Empleado).
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Super Administrador,Administrador,Empleado")]
public class ContratosProveedorController : ControllerBase
{
    private readonly IContratoProveedorService _contratoService;
    private readonly ILogger<ContratosProveedorController> _logger;

    public ContratosProveedorController(
        IContratoProveedorService contratoService,
        ILogger<ContratosProveedorController> logger)
    {
        _contratoService = contratoService;
        _logger = logger;
    }

    // ========================================
    // OPERACIONES CRUD
    // ========================================

    /// <summary>
    /// Obtener todos los contratos
    /// </summary>
    /// <returns>Lista de todos los contratos</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ContratoProveedorResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<ContratoProveedorResponseDto>>>> GetAllContratos()
    {
        try
        {
            _logger.LogInformation("📄 Obteniendo todos los contratos");

            var contratos = await _contratoService.GetAllAsync();

            return Ok(new ApiResponse<IEnumerable<ContratoProveedorResponseDto>>
            {
                Success = true,
                Message = "Contratos obtenidos exitosamente",
                Data = contratos
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al obtener contratos");
            return StatusCode(500, new ApiErrorResponse
            {
                Message = "Error interno del servidor al obtener contratos",
                StatusCode = 500
            });
        }
    }

    /// <summary>
    /// Obtener un contrato por ID
    /// </summary>
    /// <param name="id">ID del contrato</param>
    /// <returns>Datos del contrato</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<ContratoProveedorResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ContratoProveedorResponseDto>>> GetContratoById(int id)
    {
        try
        {
            _logger.LogInformation("🔍 Buscando contrato con ID: {ContratoId}", id);

            var contrato = await _contratoService.GetByIdAsync(id);

            if (contrato == null)
            {
                _logger.LogWarning("⚠️ Contrato con ID {ContratoId} no encontrado", id);
                return NotFound(new ApiErrorResponse
                {
                    Message = $"Contrato con ID {id} no encontrado",
                    StatusCode = 404,
                });
            }

            return Ok(new ApiResponse<ContratoProveedorResponseDto>
            {
                Success = true,
                Message = "Contrato encontrado",
                Data = contrato
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al obtener contrato {ContratoId}", id);
            return StatusCode(500, new ApiErrorResponse
            {
                Message = "Error interno del servidor",
                StatusCode = 500
            });
        }
    }

    /// <summary>
    /// Crear un nuevo contrato
    /// </summary>
    /// <param name="contratoDto">Datos del contrato a crear</param>
    /// <returns>Contrato creado</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ContratoProveedorResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<ContratoProveedorResponseDto>>> CreateContrato(
        [FromBody] ContratoProveedorCreateDto contratoDto)
    {
        try
        {
            _logger.LogInformation("➕ Creando nuevo contrato para proveedor {ProveedorId}",
                contratoDto.IdProveedor);

            var contrato = await _contratoService.CreateAsync(contratoDto);

            _logger.LogInformation("✅ Contrato creado exitosamente con ID: {ContratoId}",
                contrato.IdContrato);

            return CreatedAtAction(
                nameof(GetContratoById),
                new { id = contrato.IdContrato },
                new ApiResponse<ContratoProveedorResponseDto>
                {
                    Success = true,
                    Message = "Contrato creado exitosamente",
                    Data = contrato
                });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("⚠️ Proveedor no encontrado: {Message}", ex.Message);
            return NotFound(new ApiErrorResponse
            {
                Message = ex.Message,
                StatusCode = 404,
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("⚠️ Error de validación al crear contrato: {Message}", ex.Message);
            return BadRequest(new ApiErrorResponse
            {
                Message = ex.Message,
                StatusCode = 400,
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("⚠️ Argumento inválido: {Message}", ex.Message);
            return BadRequest(new ApiErrorResponse
            {
                Message = ex.Message,
                StatusCode = 400,
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al crear contrato");
            return StatusCode(500, new ApiErrorResponse
            {
                Message = "Error interno del servidor al crear contrato",
                StatusCode = 500
            });
        }
    }

    /// <summary>
    /// Actualizar un contrato existente
    /// </summary>
    /// <param name="id">ID del contrato</param>
    /// <param name="contratoDto">Datos actualizados del contrato</param>
    /// <returns>Contrato actualizado</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<ContratoProveedorResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ContratoProveedorResponseDto>>> UpdateContrato(
        int id,
        [FromBody] ContratoProveedorUpdateDto contratoDto)
    {
        try
        {
            _logger.LogInformation("✏️ Actualizando contrato con ID: {ContratoId}", id);

            var contrato = await _contratoService.UpdateAsync(id, contratoDto);

            _logger.LogInformation("✅ Contrato {ContratoId} actualizado exitosamente", id);

            return Ok(new ApiResponse<ContratoProveedorResponseDto>
            {
                Success = true,
                Message = "Contrato actualizado exitosamente",
                Data = contrato
            });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("⚠️ Contrato no encontrado: {Message}", ex.Message);
            return NotFound(new ApiErrorResponse
            {
                Message = ex.Message,
                StatusCode = 404,
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("⚠️ Error de validación al actualizar contrato: {Message}", ex.Message);
            return BadRequest(new ApiErrorResponse
            {
                Message = ex.Message,
                StatusCode = 400,
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("⚠️ Argumento inválido: {Message}", ex.Message);
            return BadRequest(new ApiErrorResponse
            {
                Message = ex.Message,
                StatusCode = 400,
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al actualizar contrato {ContratoId}", id);
            return StatusCode(500, new ApiErrorResponse
            {
                Message = "Error interno del servidor al actualizar contrato",
                StatusCode = 500
            });
        }
    }

    /// <summary>
    /// Eliminar un contrato
    /// </summary>
    /// <param name="id">ID del contrato</param>
    /// <returns>Confirmación de eliminación</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> DeleteContrato(int id)
    {
        try
        {
            _logger.LogInformation("🗑️ Eliminando contrato con ID: {ContratoId}", id);

            var result = await _contratoService.DeleteAsync(id);

            if (!result)
            {
                _logger.LogWarning("⚠️ Contrato con ID {ContratoId} no encontrado para eliminar", id);
                return NotFound(new ApiErrorResponse
                {
                    Message = $"Contrato con ID {id} no encontrado",
                    StatusCode = 404,
                });
            }

            _logger.LogInformation("✅ Contrato {ContratoId} eliminado exitosamente", id);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Contrato eliminado exitosamente",
                Data = new { IdContrato = id, Eliminado = true }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al eliminar contrato {ContratoId}", id);
            return StatusCode(500, new ApiErrorResponse
            {
                Message = "Error interno del servidor al eliminar contrato",
                StatusCode = 500
            });
        }
    }

    // ========================================
    // BÚSQUEDAS Y FILTROS
    // ========================================

    /// <summary>
    /// Buscar contrato por número de contrato
    /// </summary>
    /// <param name="numeroContrato">Número de contrato</param>
    /// <returns>Datos del contrato</returns>
    [HttpGet("numero/{numeroContrato}")]
    [ProducesResponseType(typeof(ApiResponse<ContratoProveedorResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ContratoProveedorResponseDto>>> GetContratoByNumero(string numeroContrato)
    {
        try
        {
            _logger.LogInformation("🔍 Buscando contrato con número: {NumeroContrato}", numeroContrato);

            var contrato = await _contratoService.GetByNumeroContratoAsync(numeroContrato);

            if (contrato == null)
            {
                _logger.LogWarning("⚠️ Contrato con número {NumeroContrato} no encontrado", numeroContrato);
                return NotFound(new ApiErrorResponse
                {
                    Message = $"Contrato con número {numeroContrato} no encontrado",
                    StatusCode = 404,
                });
            }

            return Ok(new ApiResponse<ContratoProveedorResponseDto>
            {
                Success = true,
                Message = "Contrato encontrado",
                Data = contrato
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al buscar contrato por número {NumeroContrato}", numeroContrato);
            return StatusCode(500, new ApiErrorResponse
            {
                Message = "Error interno del servidor",
                StatusCode = 500
            });
        }
    }

    /// <summary>
    /// Obtener contratos por proveedor
    /// </summary>
    /// <param name="idProveedor">ID del proveedor</param>
    /// <returns>Lista de contratos del proveedor</returns>
    [HttpGet("proveedor/{idProveedor}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ContratoProveedorResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<ContratoProveedorResponseDto>>>> GetContratosByProveedor(int idProveedor)
    {
        try
        {
            _logger.LogInformation("🔍 Obteniendo contratos del proveedor: {ProveedorId}", idProveedor);

            var contratos = await _contratoService.GetByProveedorAsync(idProveedor);

            return Ok(new ApiResponse<IEnumerable<ContratoProveedorResponseDto>>
            {
                Success = true,
                Message = "Contratos del proveedor obtenidos exitosamente",
                Data = contratos
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al obtener contratos del proveedor {ProveedorId}", idProveedor);
            return StatusCode(500, new ApiErrorResponse
            {
                Message = "Error interno del servidor",
                StatusCode = 500
            });
        }
    }

    /// <summary>
    /// Obtener contratos por estado
    /// </summary>
    /// <param name="estado">Estado del contrato</param>
    /// <returns>Lista de contratos con el estado especificado</returns>
    [HttpGet("estado/{estado}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ContratoProveedorResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<ContratoProveedorResponseDto>>>> GetContratosByEstado(string estado)
    {
        try
        {
            _logger.LogInformation("🔍 Obteniendo contratos con estado: {Estado}", estado);

            var contratos = await _contratoService.GetByEstadoAsync(estado);

            return Ok(new ApiResponse<IEnumerable<ContratoProveedorResponseDto>>
            {
                Success = true,
                Message = $"Contratos con estado '{estado}' obtenidos exitosamente",
                Data = contratos
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al obtener contratos por estado {Estado}", estado);
            return StatusCode(500, new ApiErrorResponse
            {
                Message = "Error interno del servidor",
                StatusCode = 500
            });
        }
    }

    /// <summary>
    /// Obtener contratos vigentes
    /// </summary>
    /// <returns>Lista de contratos vigentes</returns>
    [HttpGet("vigentes")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ContratoProveedorResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<ContratoProveedorResponseDto>>>> GetContratosVigentes()
    {
        try
        {
            _logger.LogInformation("🔍 Obteniendo contratos vigentes");

            var contratos = await _contratoService.GetVigentesAsync();

            return Ok(new ApiResponse<IEnumerable<ContratoProveedorResponseDto>>
            {
                Success = true,
                Message = "Contratos vigentes obtenidos exitosamente",
                Data = contratos
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al obtener contratos vigentes");
            return StatusCode(500, new ApiErrorResponse
            {
                Message = "Error interno del servidor",
                StatusCode = 500
            });
        }
    }

    /// <summary>
    /// Obtener contratos próximos a vencer
    /// </summary>
    /// <param name="diasAnticipacion">Días de anticipación (default: 30)</param>
    /// <returns>Lista de contratos próximos a vencer</returns>
    [HttpGet("proximos-vencer")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ContratoProveedorResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<ContratoProveedorResponseDto>>>> GetContratosProximosVencer(
        [FromQuery] int diasAnticipacion = 30)
    {
        try
        {
            _logger.LogInformation("🔍 Obteniendo contratos próximos a vencer en {Dias} días", diasAnticipacion);

            var contratos = await _contratoService.GetProximosAVencerAsync(diasAnticipacion);

            return Ok(new ApiResponse<IEnumerable<ContratoProveedorResponseDto>>
            {
                Success = true,
                Message = $"Contratos próximos a vencer en {diasAnticipacion} días obtenidos exitosamente",
                Data = contratos
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al obtener contratos próximos a vencer");
            return StatusCode(500, new ApiErrorResponse
            {
                Message = "Error interno del servidor",
                StatusCode = 500
            });
        }
    }

    /// <summary>
    /// Obtener contratos vencidos
    /// </summary>
    /// <returns>Lista de contratos vencidos</returns>
    [HttpGet("vencidos")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ContratoProveedorResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<ContratoProveedorResponseDto>>>> GetContratosVencidos()
    {
        try
        {
            _logger.LogInformation("🔍 Obteniendo contratos vencidos");

            var contratos = await _contratoService.GetVencidosAsync();

            return Ok(new ApiResponse<IEnumerable<ContratoProveedorResponseDto>>
            {
                Success = true,
                Message = "Contratos vencidos obtenidos exitosamente",
                Data = contratos
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al obtener contratos vencidos");
            return StatusCode(500, new ApiErrorResponse
            {
                Message = "Error interno del servidor",
                StatusCode = 500
            });
        }
    }

    /// <summary>
    /// Obtener contratos con renovación automática
    /// </summary>
    /// <returns>Lista de contratos con renovación automática</returns>
    [HttpGet("renovacion-automatica")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ContratoProveedorResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<ContratoProveedorResponseDto>>>> GetContratosConRenovacionAutomatica()
    {
        try
        {
            _logger.LogInformation("🔍 Obteniendo contratos con renovación automática");

            var contratos = await _contratoService.GetConRenovacionAutomaticaAsync();

            return Ok(new ApiResponse<IEnumerable<ContratoProveedorResponseDto>>
            {
                Success = true,
                Message = "Contratos con renovación automática obtenidos exitosamente",
                Data = contratos
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al obtener contratos con renovación automática");
            return StatusCode(500, new ApiErrorResponse
            {
                Message = "Error interno del servidor",
                StatusCode = 500
            });
        }
    }

    /// <summary>
    /// Obtener contratos de un proveedor en estado específico
    /// </summary>
    /// <param name="idProveedor">ID del proveedor</param>
    /// <param name="estado">Estado del contrato</param>
    /// <returns>Lista de contratos filtrados</returns>
    [HttpGet("proveedor/{idProveedor}/estado/{estado}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ContratoProveedorResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<ContratoProveedorResponseDto>>>> GetContratosByProveedorYEstado(
        int idProveedor,
        string estado)
    {
        try
        {
            _logger.LogInformation("🔍 Obteniendo contratos del proveedor {ProveedorId} con estado {Estado}",
                idProveedor, estado);

            var contratos = await _contratoService.GetByProveedorYEstadoAsync(idProveedor, estado);

            return Ok(new ApiResponse<IEnumerable<ContratoProveedorResponseDto>>
            {
                Success = true,
                Message = $"Contratos del proveedor con estado '{estado}' obtenidos exitosamente",
                Data = contratos
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al obtener contratos por proveedor y estado");
            return StatusCode(500, new ApiErrorResponse
            {
                Message = "Error interno del servidor",
                StatusCode = 500
            });
        }
    }

    // ========================================
    // GESTIÓN DE ESTADO
    // ========================================

    /// <summary>
    /// Cambiar estado de un contrato
    /// </summary>
    /// <param name="id">ID del contrato</param>
    /// <param name="nuevoEstado">Nuevo estado</param>
    /// <returns>Contrato con estado actualizado</returns>
    [HttpPatch("{id}/cambiar-estado")]
    [ProducesResponseType(typeof(ApiResponse<ContratoProveedorResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ContratoProveedorResponseDto>>> CambiarEstado(
        int id,
        [FromBody] string nuevoEstado)
    {
        try
        {
            _logger.LogInformation("🔄 Cambiando estado del contrato {ContratoId} a {NuevoEstado}",
                id, nuevoEstado);

            var contrato = await _contratoService.CambiarEstadoAsync(id, nuevoEstado);

            _logger.LogInformation("✅ Estado del contrato {ContratoId} cambiado exitosamente", id);

            return Ok(new ApiResponse<ContratoProveedorResponseDto>
            {
                Success = true,
                Message = $"Estado del contrato cambiado a '{nuevoEstado}' exitosamente",
                Data = contrato
            });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("⚠️ Contrato no encontrado: {Message}", ex.Message);
            return NotFound(new ApiErrorResponse
            {
                Message = ex.Message,
                StatusCode = 404,
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("⚠️ Estado inválido: {Message}", ex.Message);
            return BadRequest(new ApiErrorResponse
            {
                Message = ex.Message,
                StatusCode = 400,
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al cambiar estado del contrato {ContratoId}", id);
            return StatusCode(500, new ApiErrorResponse
            {
                Message = "Error interno del servidor",
                StatusCode = 500
            });
        }
    }

    /// <summary>
    /// Renovar un contrato
    /// </summary>
    /// <param name="id">ID del contrato</param>
    /// <param name="nuevaFechaFin">Nueva fecha de finalización</param>
    /// <returns>Contrato renovado</returns>
    [HttpPost("{id}/renovar")]
    [ProducesResponseType(typeof(ApiResponse<ContratoProveedorResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ContratoProveedorResponseDto>>> RenovarContrato(
        int id,
        [FromBody] DateTime nuevaFechaFin)
    {
        try
        {
            _logger.LogInformation("🔄 Renovando contrato {ContratoId} hasta {NuevaFecha}",
                id, nuevaFechaFin);

            var contrato = await _contratoService.RenovarContratoAsync(id, nuevaFechaFin);

            _logger.LogInformation("✅ Contrato {ContratoId} renovado exitosamente", id);

            return Ok(new ApiResponse<ContratoProveedorResponseDto>
            {
                Success = true,
                Message = $"Contrato renovado exitosamente hasta {nuevaFechaFin:yyyy-MM-dd}",
                Data = contrato
            });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("⚠️ Contrato no encontrado: {Message}", ex.Message);
            return NotFound(new ApiErrorResponse
            {
                Message = ex.Message,
                StatusCode = 404,
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("⚠️ Error al renovar contrato: {Message}", ex.Message);
            return BadRequest(new ApiErrorResponse
            {
                Message = ex.Message,
                StatusCode = 400,
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("⚠️ Fecha inválida: {Message}", ex.Message);
            return BadRequest(new ApiErrorResponse
            {
                Message = ex.Message,
                StatusCode = 400,
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al renovar contrato {ContratoId}", id);
            return StatusCode(500, new ApiErrorResponse
            {
                Message = "Error interno del servidor",
                StatusCode = 500
            });
        }
    }

    /// <summary>
    /// Cancelar un contrato
    /// </summary>
    /// <param name="id">ID del contrato</param>
    /// <param name="motivo">Motivo de la cancelación</param>
    /// <returns>Contrato cancelado</returns>
    [HttpPost("{id}/cancelar")]
    [ProducesResponseType(typeof(ApiResponse<ContratoProveedorResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ContratoProveedorResponseDto>>> CancelarContrato(
        int id,
        [FromBody] string motivo)
    {
        try
        {
            _logger.LogInformation("❌ Cancelando contrato {ContratoId}. Motivo: {Motivo}",
                id, motivo);

            var contrato = await _contratoService.CancelarContratoAsync(id, motivo);

            _logger.LogInformation("✅ Contrato {ContratoId} cancelado exitosamente", id);

            return Ok(new ApiResponse<ContratoProveedorResponseDto>
            {
                Success = true,
                Message = "Contrato cancelado exitosamente",
                Data = contrato
            });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("⚠️ Contrato no encontrado: {Message}", ex.Message);
            return NotFound(new ApiErrorResponse
            {
                Message = ex.Message,
                StatusCode = 404,
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("⚠️ Error al cancelar contrato: {Message}", ex.Message);
            return BadRequest(new ApiErrorResponse
            {
                Message = ex.Message,
                StatusCode = 400,
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al cancelar contrato {ContratoId}", id);
            return StatusCode(500, new ApiErrorResponse
            {
                Message = "Error interno del servidor",
                StatusCode = 500
            });
        }
    }

    // ========================================
    // ESTADÍSTICAS Y REPORTES
    // ========================================

    /// <summary>
    /// Obtener estadísticas de contratos por estado
    /// </summary>
    /// <returns>Diccionario con cantidad de contratos por estado</returns>
    [HttpGet("estadisticas/por-estado")]
    [ProducesResponseType(typeof(ApiResponse<Dictionary<string, int>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<Dictionary<string, int>>>> GetEstadisticasPorEstado()
    {
        try
        {
            _logger.LogInformation("📊 Obteniendo estadísticas de contratos por estado");

            var estadisticas = await _contratoService.GetEstadisticasPorEstadoAsync();

            return Ok(new ApiResponse<Dictionary<string, int>>
            {
                Success = true,
                Message = "Estadísticas obtenidas exitosamente",
                Data = estadisticas
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al obtener estadísticas");
            return StatusCode(500, new ApiErrorResponse
            {
                Message = "Error interno del servidor",
                StatusCode = 500
            });
        }
    }

    /// <summary>
    /// Obtener valor total de contratos vigentes
    /// </summary>
    /// <returns>Suma del valor de todos los contratos vigentes</returns>
    [HttpGet("estadisticas/valor-total-vigentes")]
    [ProducesResponseType(typeof(ApiResponse<decimal>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<decimal>>> GetValorTotalContratosVigentes()
    {
        try
        {
            _logger.LogInformation("💰 Calculando valor total de contratos vigentes");

            var valorTotal = await _contratoService.GetValorTotalContratosVigentesAsync();

            return Ok(new ApiResponse<decimal>
            {
                Success = true,
                Message = "Valor total calculado exitosamente",
                Data = valorTotal
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al calcular valor total");
            return StatusCode(500, new ApiErrorResponse
            {
                Message = "Error interno del servidor",
                StatusCode = 500
            });
        }
    }
}