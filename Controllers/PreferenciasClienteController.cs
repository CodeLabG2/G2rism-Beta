using Microsoft.AspNetCore.Mvc;
using G2rismBeta.API.DTOs.PreferenciaCliente;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.Models;

namespace G2rismBeta.API.Controllers
{
    /// <summary>
    /// Controlador para gestionar las preferencias de clientes
    /// Proporciona endpoints para el módulo CRM de seguimiento y personalización
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PreferenciasClienteController : ControllerBase
    {
        private readonly IPreferenciaClienteService _preferenciaClienteService;

        public PreferenciasClienteController(IPreferenciaClienteService preferenciaClienteService)
        {
            _preferenciaClienteService = preferenciaClienteService;
        }

        /// <summary>
        /// Obtiene todas las preferencias de clientes
        /// </summary>
        /// <returns>Lista de todas las preferencias registradas</returns>
        /// <response code="200">Retorna la lista de preferencias</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PreferenciaClienteResponseDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<PreferenciaClienteResponseDto>>>> GetAll()
        {
            var preferencias = await _preferenciaClienteService.GetAllPreferenciasAsync();
            return Ok(new ApiResponse<IEnumerable<PreferenciaClienteResponseDto>>
            {
                Success = true,
                Message = "Preferencias obtenidas exitosamente",
                Data = preferencias
            });
        }

        /// <summary>
        /// Obtiene una preferencia específica por su ID
        /// </summary>
        /// <param name="id">ID de la preferencia</param>
        /// <returns>Datos de la preferencia encontrada</returns>
        /// <response code="200">Preferencia encontrada</response>
        /// <response code="404">Preferencia no encontrada</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<PreferenciaClienteResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<PreferenciaClienteResponseDto>>> GetById(int id)
        {
            var preferencia = await _preferenciaClienteService.GetPreferenciaByIdAsync(id);

            if (preferencia == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Preferencia no encontrada"
                });
            }

            return Ok(new ApiResponse<PreferenciaClienteResponseDto>
            {
                Success = true,
                Message = "Preferencia obtenida exitosamente",
                Data = preferencia
            });
        }

        /// <summary>
        /// Obtiene las preferencias de un cliente específico
        /// </summary>
        /// <param name="idCliente">ID del cliente</param>
        /// <returns>Preferencias del cliente</returns>
        /// <response code="200">Preferencias encontradas</response>
        /// <response code="404">Cliente no tiene preferencias registradas</response>
        [HttpGet("cliente/{idCliente}")]
        [ProducesResponseType(typeof(ApiResponse<PreferenciaClienteResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<PreferenciaClienteResponseDto>>> GetByClienteId(int idCliente)
        {
            var preferencia = await _preferenciaClienteService.GetPreferenciaByClienteIdAsync(idCliente);

            if (preferencia == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "El cliente no tiene preferencias registradas"
                });
            }

            return Ok(new ApiResponse<PreferenciaClienteResponseDto>
            {
                Success = true,
                Message = "Preferencias del cliente obtenidas exitosamente",
                Data = preferencia
            });
        }

        /// <summary>
        /// Busca clientes por tipo de destino preferido
        /// </summary>
        /// <param name="tipoDestino">Tipo de destino a buscar</param>
        /// <returns>Lista de preferencias que coinciden con el tipo de destino</returns>
        /// <response code="200">Búsqueda realizada exitosamente</response>
        [HttpGet("buscar/destino/{tipoDestino}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PreferenciaClienteResponseDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<PreferenciaClienteResponseDto>>>> GetByTipoDestino(string tipoDestino)
        {
            var preferencias = await _preferenciaClienteService.GetPreferenciasByTipoDestinoAsync(tipoDestino);
            return Ok(new ApiResponse<IEnumerable<PreferenciaClienteResponseDto>>
            {
                Success = true,
                Message = $"Se encontraron {preferencias.Count()} preferencias con destino '{tipoDestino}'",
                Data = preferencias
            });
        }

        /// <summary>
        /// Busca clientes por rango de presupuesto
        /// </summary>
        /// <param name="minimo">Presupuesto mínimo</param>
        /// <param name="maximo">Presupuesto máximo</param>
        /// <returns>Lista de preferencias dentro del rango de presupuesto</returns>
        /// <response code="200">Búsqueda realizada exitosamente</response>
        [HttpGet("buscar/presupuesto")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PreferenciaClienteResponseDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<PreferenciaClienteResponseDto>>>> GetByPresupuestoRange(
            [FromQuery] decimal minimo,
            [FromQuery] decimal maximo)
        {
            var preferencias = await _preferenciaClienteService.GetPreferenciasByRangoPresupuestoAsync(minimo, maximo);
            return Ok(new ApiResponse<IEnumerable<PreferenciaClienteResponseDto>>
            {
                Success = true,
                Message = $"Se encontraron {preferencias.Count()} clientes en el rango de presupuesto ${minimo:N2} - ${maximo:N2}",
                Data = preferencias
            });
        }

        /// <summary>
        /// Crea nuevas preferencias para un cliente
        /// </summary>
        /// <param name="dto">Datos de las preferencias a crear</param>
        /// <returns>Preferencias creadas</returns>
        /// <response code="201">Preferencias creadas exitosamente</response>
        /// <response code="400">Datos inválidos o el cliente ya tiene preferencias</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PreferenciaClienteResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<PreferenciaClienteResponseDto>>> Create(
            [FromBody] PreferenciaClienteCreateDto dto)
        {
            var preferencia = await _preferenciaClienteService.CreatePreferenciaAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = preferencia.IdPreferencia },
                new ApiResponse<PreferenciaClienteResponseDto>
                {
                    Success = true,
                    Message = "Preferencias del cliente creadas exitosamente",
                    Data = preferencia
                });
        }

        /// <summary>
        /// Actualiza las preferencias de un cliente
        /// </summary>
        /// <param name="id">ID de la preferencia</param>
        /// <param name="dto">Nuevos datos de las preferencias</param>
        /// <returns>Preferencias actualizadas</returns>
        /// <response code="200">Preferencias actualizadas exitosamente</response>
        /// <response code="404">Preferencia no encontrada</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<PreferenciaClienteResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<PreferenciaClienteResponseDto>>> Update(
            int id,
            [FromBody] PreferenciaClienteUpdateDto dto)
        {
            var preferencia = await _preferenciaClienteService.UpdatePreferenciaAsync(id, dto);

            if (preferencia == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Preferencia no encontrada"
                });
            }

            return Ok(new ApiResponse<PreferenciaClienteResponseDto>
            {
                Success = true,
                Message = "Preferencias actualizadas exitosamente",
                Data = preferencia
            });
        }

        /// <summary>
        /// Elimina las preferencias de un cliente
        /// </summary>
        /// <param name="id">ID de la preferencia</param>
        /// <returns>Confirmación de eliminación</returns>
        /// <response code="200">Preferencias eliminadas exitosamente</response>
        /// <response code="404">Preferencia no encontrada</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            var result = await _preferenciaClienteService.DeletePreferenciaAsync(id);

            if (!result)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Preferencia no encontrada"
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Preferencias del cliente eliminadas exitosamente"
            });
        }

        /// <summary>
        /// Obtiene estadísticas generales de preferencias de clientes
        /// </summary>
        /// <returns>Estadísticas agregadas</returns>
        /// <response code="200">Estadísticas obtenidas exitosamente</response>
        [HttpGet("estadisticas")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<object>>> GetEstadisticas()
        {
            var todasPreferencias = await _preferenciaClienteService.GetAllPreferenciasAsync();

            var estadisticas = new
            {
                TotalPreferencias = todasPreferencias.Count(),
                PresupuestoPromedio = todasPreferencias
                    .Where(p => p.PresupuestoPromedio.HasValue)
                    .Average(p => p.PresupuestoPromedio),
                DestinosMasPopulares = todasPreferencias
                    .Where(p => !string.IsNullOrEmpty(p.TipoDestino))
                    .GroupBy(p => p.TipoDestino)
                    .Select(g => new { Destino = g.Key, Cantidad = g.Count() })
                    .OrderByDescending(x => x.Cantidad)
                    .Take(5),
                AlojamientosMasPopulares = todasPreferencias
                    .Where(p => !string.IsNullOrEmpty(p.TipoAlojamiento))
                    .GroupBy(p => p.TipoAlojamiento)
                    .Select(g => new { Alojamiento = g.Key, Cantidad = g.Count() })
                    .OrderByDescending(x => x.Cantidad)
                    .Take(5)
            };

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Estadísticas de preferencias obtenidas exitosamente",
                Data = estadisticas
            });
        }
    }
}