using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using G2rismBeta.API.DTOs.Proveedor;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.Models;

namespace G2rismBeta.API.Controllers
{
    /// <summary>
    /// Controlador para la gestión de proveedores de servicios turísticos
    /// Requiere autenticación. Accesible para empleados (Super Admin, Admin, Empleado).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Super Administrador,Administrador,Empleado")]
    public class ProveedoresController : ControllerBase
    {
        private readonly IProveedorService _proveedorService;
        private readonly ILogger<ProveedoresController> _logger;

        public ProveedoresController(
            IProveedorService proveedorService,
            ILogger<ProveedoresController> logger)
        {
            _proveedorService = proveedorService;
            _logger = logger;
        }

        // ========================================
        // OPERACIONES CRUD BÁSICAS
        // ========================================

        /// <summary>
        /// Obtener todos los proveedores
        /// </summary>
        /// <returns>Lista de proveedores</returns>
        /// <response code="200">Lista de proveedores obtenida exitosamente</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProveedorResponseDto>>), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProveedorResponseDto>>>> GetAll()
        {
            try
            {
                var proveedores = await _proveedorService.GetAllAsync();
                return Ok((proveedores, "Proveedores obtenidos exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los proveedores");
                return StatusCode(500, ("Error al obtener los proveedores", ex.Message));
            }
        }

        /// <summary>
        /// Obtener un proveedor por ID
        /// </summary>
        /// <param name="id">ID del proveedor</param>
        /// <returns>Proveedor encontrado</returns>
        /// <response code="200">Proveedor encontrado</response>
        /// <response code="404">Proveedor no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ProveedorResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 404)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public async Task<ActionResult<ApiResponse<ProveedorResponseDto>>> GetById(int id)
        {
            try
            {
                var proveedor = await _proveedorService.GetByIdAsync(id);

                if (proveedor == null)
                {
                    return NotFound(("Proveedor no encontrado", $"No existe un proveedor con ID {id}"));
                }

                return Ok((proveedor, "Proveedor obtenido exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener proveedor con ID {id}");
                return StatusCode(500, ("Error al obtener el proveedor", ex.Message));
            }
        }

        /// <summary>
        /// Crear un nuevo proveedor
        /// </summary>
        /// <param name="dto">Datos del proveedor a crear</param>
        /// <returns>Proveedor creado</returns>
        /// <response code="201">Proveedor creado exitosamente</response>
        /// <response code="400">Datos inválidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ProveedorResponseDto>), 201)]
        [ProducesResponseType(typeof(ApiErrorResponse), 400)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public async Task<ActionResult<ApiResponse<ProveedorResponseDto>>> Create([FromBody] ProveedorCreateDto dto)
        {
            try
            {
                var proveedorCreado = await _proveedorService.CreateAsync(dto);
                return CreatedAtAction(
                    nameof(GetById),
                    new { id = proveedorCreado.IdProveedor },
                    (proveedorCreado, "Proveedor creado exitosamente"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(("Error de validación", ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(("Datos inválidos", ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear proveedor");
                return StatusCode(500, ("Error al crear el proveedor", ex.Message));
            }
        }

        /// <summary>
        /// Actualizar un proveedor existente
        /// </summary>
        /// <param name="id">ID del proveedor a actualizar</param>
        /// <param name="dto">Datos actualizados del proveedor</param>
        /// <returns>Proveedor actualizado</returns>
        /// <response code="200">Proveedor actualizado exitosamente</response>
        /// <response code="400">Datos inválidos</response>
        /// <response code="404">Proveedor no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ProveedorResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 400)]
        [ProducesResponseType(typeof(ApiErrorResponse), 404)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public async Task<ActionResult<ApiResponse<ProveedorResponseDto>>> Update(int id, [FromBody] ProveedorUpdateDto dto)
        {
            try
            {
                var proveedorActualizado = await _proveedorService.UpdateAsync(id, dto);
                return Ok((proveedorActualizado, "Proveedor actualizado exitosamente"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(("Proveedor no encontrado", ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(("Error de validación", ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(("Datos inválidos", ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar proveedor con ID {id}");
                return StatusCode(500, ("Error al actualizar el proveedor", ex.Message));
            }
        }

        /// <summary>
        /// Eliminar un proveedor
        /// </summary>
        /// <param name="id">ID del proveedor a eliminar</param>
        /// <returns>Confirmación de eliminación</returns>
        /// <response code="200">Proveedor eliminado exitosamente</response>
        /// <response code="400">No se puede eliminar el proveedor</response>
        /// <response code="404">Proveedor no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 400)]
        [ProducesResponseType(typeof(ApiErrorResponse), 404)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            try
            {
                await _proveedorService.DeleteAsync(id);
                return Ok((true, "Proveedor eliminado exitosamente"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(("Proveedor no encontrado", ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(("No se puede eliminar el proveedor", ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar proveedor con ID {id}");
                return StatusCode(500, ("Error al eliminar el proveedor", ex.Message));
            }
        }

        // ========================================
        // BÚSQUEDAS Y FILTROS
        // ========================================

        /// <summary>
        /// Buscar proveedor por NIT/RUT
        /// </summary>
        /// <param name="nitRut">NIT o RUT del proveedor</param>
        /// <returns>Proveedor encontrado</returns>
        /// <response code="200">Proveedor encontrado</response>
        /// <response code="404">Proveedor no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("nit/{nitRut}")]
        [ProducesResponseType(typeof(ApiResponse<ProveedorResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 404)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public async Task<ActionResult<ApiResponse<ProveedorResponseDto>>> GetByNitRut(string nitRut)
        {
            try
            {
                var proveedor = await _proveedorService.GetByNitRutAsync(nitRut);

                if (proveedor == null)
                {
                    return NotFound(("Proveedor no encontrado", $"No existe un proveedor con NIT/RUT {nitRut}"));
                }

                return Ok((proveedor, "Proveedor encontrado"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al buscar proveedor por NIT/RUT {nitRut}");
                return StatusCode(500, ("Error al buscar el proveedor", ex.Message));
            }
        }

        /// <summary>
        /// Buscar proveedores por nombre
        /// </summary>
        /// <param name="nombre">Nombre o parte del nombre del proveedor</param>
        /// <returns>Lista de proveedores que coinciden con el nombre</returns>
        /// <response code="200">Búsqueda realizada exitosamente</response>
        /// <response code="400">Parámetro de búsqueda inválido</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("buscar")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProveedorResponseDto>>), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 400)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProveedorResponseDto>>>> SearchByNombre([FromQuery] string nombre)
        {
            try
            {
                var proveedores = await _proveedorService.SearchByNombreAsync(nombre);
                return Ok((proveedores, "Búsqueda realizada exitosamente"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(("Parámetro inválido", ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al buscar proveedores por nombre: {nombre}");
                return StatusCode(500, ("Error al realizar la búsqueda", ex.Message));
            }
        }

        /// <summary>
        /// Obtener proveedores por tipo
        /// </summary>
        /// <param name="tipo">Tipo de proveedor (Hotel, Aerolinea, Transporte, Servicios, Mixto)</param>
        /// <returns>Lista de proveedores del tipo especificado</returns>
        /// <response code="200">Proveedores obtenidos exitosamente</response>
        /// <response code="400">Tipo de proveedor inválido</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("tipo/{tipo}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProveedorResponseDto>>), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 400)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProveedorResponseDto>>>> GetByTipo(string tipo)
        {
            try
            {
                var proveedores = await _proveedorService.GetByTipoAsync(tipo);
                return Ok((proveedores, $"Proveedores de tipo '{tipo}' obtenidos exitosamente"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(("Tipo inválido", ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener proveedores por tipo: {tipo}");
                return StatusCode(500, ("Error al obtener los proveedores", ex.Message));
            }
        }

        /// <summary>
        /// Obtener proveedores activos
        /// </summary>
        /// <returns>Lista de proveedores activos</returns>
        /// <response code="200">Proveedores activos obtenidos exitosamente</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("activos")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProveedorResponseDto>>), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProveedorResponseDto>>>> GetActivos()
        {
            try
            {
                var proveedores = await _proveedorService.GetActivosAsync();
                return Ok((proveedores, "Proveedores activos obtenidos exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener proveedores activos");
                return StatusCode(500, ("Error al obtener los proveedores", ex.Message));
            }
        }

        /// <summary>
        /// Obtener proveedores por ciudad
        /// </summary>
        /// <param name="ciudad">Nombre de la ciudad</param>
        /// <returns>Lista de proveedores de la ciudad especificada</returns>
        /// <response code="200">Proveedores obtenidos exitosamente</response>
        /// <response code="400">Parámetro de ciudad inválido</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("ciudad/{ciudad}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProveedorResponseDto>>), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 400)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProveedorResponseDto>>>> GetByCiudad(string ciudad)
        {
            try
            {
                var proveedores = await _proveedorService.GetByCiudadAsync(ciudad);
                return Ok((proveedores, $"Proveedores de {ciudad} obtenidos exitosamente"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(("Parámetro inválido", ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener proveedores por ciudad: {ciudad}");
                return StatusCode(500, ("Error al obtener los proveedores", ex.Message));
            }
        }

        /// <summary>
        /// Obtener proveedores por calificación mínima
        /// </summary>
        /// <param name="calificacion">Calificación mínima (0.0 - 5.0)</param>
        /// <returns>Lista de proveedores con calificación igual o superior</returns>
        /// <response code="200">Proveedores obtenidos exitosamente</response>
        /// <response code="400">Calificación inválida</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("calificacion/{calificacion}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProveedorResponseDto>>), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 400)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProveedorResponseDto>>>> GetByCalificacion(decimal calificacion)
        {
            try
            {
                var proveedores = await _proveedorService.GetByCalificacionMinimaAsync(calificacion);
                return Ok((proveedores, $"Proveedores con calificación ≥ {calificacion} obtenidos exitosamente"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(("Calificación inválida", ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener proveedores por calificación: {calificacion}");
                return StatusCode(500, ("Error al obtener los proveedores", ex.Message));
            }
        }

        // ========================================
        // GESTIÓN DE ESTADO Y CALIFICACIÓN
        // ========================================

        /// <summary>
        /// Cambiar estado de un proveedor
        /// </summary>
        /// <param name="id">ID del proveedor</param>
        /// <param name="nuevoEstado">Nuevo estado (Activo, Inactivo, Suspendido)</param>
        /// <returns>Proveedor con estado actualizado</returns>
        /// <response code="200">Estado actualizado exitosamente</response>
        /// <response code="400">Estado inválido o no se puede cambiar</response>
        /// <response code="404">Proveedor no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPatch("{id}/estado")]
        [ProducesResponseType(typeof(ApiResponse<ProveedorResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 400)]
        [ProducesResponseType(typeof(ApiErrorResponse), 404)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public async Task<ActionResult<ApiResponse<ProveedorResponseDto>>> CambiarEstado(int id, [FromQuery] string nuevoEstado)
        {
            try
            {
                var proveedor = await _proveedorService.CambiarEstadoAsync(id, nuevoEstado);
                return Ok((proveedor, $"Estado cambiado a '{nuevoEstado}' exitosamente"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(("Proveedor no encontrado", ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(("No se puede cambiar el estado", ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(("Estado inválido", ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al cambiar estado del proveedor ID {id}");
                return StatusCode(500, ("Error al cambiar el estado", ex.Message));
            }
        }

        /// <summary>
        /// Actualizar calificación de un proveedor
        /// </summary>
        /// <param name="id">ID del proveedor</param>
        /// <param name="nuevaCalificacion">Nueva calificación (0.0 - 5.0)</param>
        /// <returns>Proveedor con calificación actualizada</returns>
        /// <response code="200">Calificación actualizada exitosamente</response>
        /// <response code="400">Calificación inválida</response>
        /// <response code="404">Proveedor no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPatch("{id}/calificacion")]
        [ProducesResponseType(typeof(ApiResponse<ProveedorResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 400)]
        [ProducesResponseType(typeof(ApiErrorResponse), 404)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public async Task<ActionResult<ApiResponse<ProveedorResponseDto>>> ActualizarCalificacion(int id, [FromQuery] decimal nuevaCalificacion)
        {
            try
            {
                var proveedor = await _proveedorService.ActualizarCalificacionAsync(id, nuevaCalificacion);
                return Ok((proveedor, $"Calificación actualizada a {nuevaCalificacion} exitosamente"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(("Proveedor no encontrado", ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(("Calificación inválida", ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar calificación del proveedor ID {id}");
                return StatusCode(500, ("Error al actualizar la calificación", ex.Message));
            }
        }

        // ========================================
        // ESTADÍSTICAS Y REPORTES
        // ========================================

        /// <summary>
        /// Obtener top proveedores mejor calificados
        /// </summary>
        /// <param name="cantidad">Cantidad de proveedores a obtener (default: 10)</param>
        /// <returns>Lista de top proveedores</returns>
        /// <response code="200">Top proveedores obtenidos exitosamente</response>
        /// <response code="400">Cantidad inválida</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("top")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProveedorResponseDto>>), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 400)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProveedorResponseDto>>>> GetTopProveedores([FromQuery] int cantidad = 10)
        {
            try
            {
                var proveedores = await _proveedorService.GetTopProveedoresAsync(cantidad);
                return Ok((proveedores, $"Top {cantidad} proveedores obtenidos exitosamente"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(("Cantidad inválida", ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener top {cantidad} proveedores");
                return StatusCode(500, ("Error al obtener los proveedores", ex.Message));
            }
        }

        /// <summary>
        /// Obtener estadísticas de proveedores por tipo
        /// </summary>
        /// <returns>Diccionario con cantidad de proveedores por tipo</returns>
        /// <response code="200">Estadísticas obtenidas exitosamente</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("estadisticas/tipo")]
        [ProducesResponseType(typeof(ApiResponse<Dictionary<string, int>>), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public async Task<ActionResult<ApiResponse<Dictionary<string, int>>>> GetEstadisticasPorTipo()
        {
            try
            {
                var estadisticas = await _proveedorService.GetEstadisticasPorTipoAsync();
                return Ok((estadisticas, "Estadísticas obtenidas exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estadísticas por tipo");
                return StatusCode(500, ("Error al obtener las estadísticas", ex.Message));
            }
        }

        /// <summary>
        /// Obtener proveedores con contratos vigentes
        /// </summary>
        /// <returns>Lista de proveedores con contratos vigentes</returns>
        /// <response code="200">Proveedores obtenidos exitosamente</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("con-contratos")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProveedorResponseDto>>), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProveedorResponseDto>>>> GetConContratosVigentes()
        {
            try
            {
                var proveedores = await _proveedorService.GetConContratosVigentesAsync();
                return Ok((proveedores, "Proveedores con contratos vigentes obtenidos exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener proveedores con contratos vigentes");
                return StatusCode(500, ("Error al obtener los proveedores", ex.Message));
            }
        }
    }
}