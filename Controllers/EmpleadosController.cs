using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using G2rismBeta.API.DTOs.Empleado;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.Models;

namespace G2rismBeta.API.Controllers
{
    /// <summary>
    /// Controlador para la gestión de empleados y jerarquía organizacional
    /// Requiere autenticación. La autorización se maneja mediante políticas basadas en permisos.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmpleadosController : ControllerBase
    {
        private readonly IEmpleadoService _empleadoService;

        public EmpleadosController(IEmpleadoService empleadoService)
        {
            _empleadoService = empleadoService;
        }

        // ========================================
        // ENDPOINTS CRUD BÁSICOS
        // ========================================

        /// <summary>
        /// Obtener todos los empleados
        /// </summary>
        /// <returns>Lista de todos los empleados del sistema</returns>
        /// <response code="200">Lista de empleados obtenida exitosamente</response>
        [HttpGet]
        [Authorize(Policy = "RequirePermission:empleados.leer")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmpleadoResponseDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmpleadoResponseDto>>>> GetAllEmpleados()
        {
            try
            {
                var empleados = await _empleadoService.GetAllEmpleadosAsync();
                return Ok(new ApiResponse<IEnumerable<EmpleadoResponseDto>>
                {
                    Success = true,
                    Message = "Empleados obtenidos exitosamente",
                    Data = empleados
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiErrorResponse
                {
                    Message = "Error al obtener los empleados",
                    ErrorCode = ex.Message
                });
            }
        }

        /// <summary>
        /// Obtener solo empleados activos
        /// </summary>
        /// <returns>Lista de empleados con estado activo</returns>
        /// <response code="200">Lista de empleados activos obtenida exitosamente</response>
        [HttpGet("activos")]
        [Authorize(Policy = "RequirePermission:empleados.leer")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmpleadoResponseDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmpleadoResponseDto>>>> GetEmpleadosActivos()
        {
            try
            {
                var empleados = await _empleadoService.GetEmpleadosActivosAsync();
                return Ok(new ApiResponse<IEnumerable<EmpleadoResponseDto>>
                {
                    Success = true,
                    Message = "Empleados activos obtenidos exitosamente",
                    Data = empleados
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiErrorResponse
                {
                    Message = "Error al obtener los empleados activos",
                    ErrorCode = ex.Message
                });
            }
        }

        /// <summary>
        /// Obtener un empleado por su ID
        /// </summary>
        /// <param name="id">ID del empleado</param>
        /// <returns>Datos del empleado solicitado</returns>
        /// <response code="200">Empleado encontrado exitosamente</response>
        /// <response code="404">Empleado no encontrado</response>
        [HttpGet("{id}")]
        [Authorize(Policy = "RequirePermission:empleados.leer")]
        [ProducesResponseType(typeof(ApiResponse<EmpleadoResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<EmpleadoResponseDto>>> GetEmpleadoById(int id)
        {
            try
            {
                var empleado = await _empleadoService.GetEmpleadoByIdAsync(id);

                if (empleado == null)
                {
                    return NotFound(new ApiErrorResponse
                    {
                        Message = $"No se encontró un empleado con ID {id}"
                    });
                }

                return Ok(new ApiResponse<EmpleadoResponseDto>
                {
                    Success = true,
                    Message = "Empleado obtenido exitosamente",
                    Data = empleado
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiErrorResponse
                {
                    Message = "Error al obtener el empleado",
                    ErrorCode = ex.Message
                });
            }
        }

        /// <summary>
        /// Obtener un empleado con información completa (incluye jefe y subordinados)
        /// </summary>
        /// <param name="id">ID del empleado</param>
        /// <returns>Datos completos del empleado con sus relaciones</returns>
        /// <response code="200">Empleado completo encontrado exitosamente</response>
        /// <response code="404">Empleado no encontrado</response>
        [HttpGet("{id}/completo")]
        [Authorize(Policy = "RequirePermission:empleados.leer")]
        [ProducesResponseType(typeof(ApiResponse<EmpleadoResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<EmpleadoResponseDto>>> GetEmpleadoCompleto(int id)
        {
            try
            {
                var empleado = await _empleadoService.GetEmpleadoCompletoAsync(id);

                if (empleado == null)
                {
                    return NotFound(new ApiErrorResponse
                    {
                        Message = $"No se encontró un empleado con ID {id}"
                    });
                }

                return Ok(new ApiResponse<EmpleadoResponseDto>
                {
                    Success = true,
                    Message = "Empleado completo obtenido exitosamente",
                    Data = empleado
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiErrorResponse
                {
                    Message = "Error al obtener el empleado completo",
                    ErrorCode = ex.Message
                });
            }
        }

        /// <summary>
        /// Crear un nuevo empleado
        /// </summary>
        /// <param name="empleadoCreateDto">Datos del empleado a crear</param>
        /// <returns>Empleado creado con su ID asignado</returns>
        /// <response code="201">Empleado creado exitosamente</response>
        /// <response code="400">Datos inválidos o validación fallida</response>
        [HttpPost]
        [Authorize(Policy = "RequirePermission:empleados.crear")]
        [ProducesResponseType(typeof(ApiResponse<EmpleadoResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<EmpleadoResponseDto>>> CreateEmpleado(
            [FromBody] EmpleadoCreateDto empleadoCreateDto)
        {
            try
            {
                var empleado = await _empleadoService.CreateEmpleadoAsync(empleadoCreateDto);

                return CreatedAtAction(
                    nameof(GetEmpleadoById),
                    new { id = empleado.IdEmpleado },
                    new ApiResponse<EmpleadoResponseDto>
                    {
                        Success = true,
                        Message = "Empleado creado exitosamente",
                        Data = empleado
                    });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiErrorResponse
                {
                    Message = "Error de validación al crear el empleado",
                    ErrorCode = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiErrorResponse
                {
                    Message = "Error al crear el empleado",
                    ErrorCode = ex.Message
                });
            }
        }

        /// <summary>
        /// Actualizar un empleado existente
        /// </summary>
        /// <param name="id">ID del empleado a actualizar</param>
        /// <param name="empleadoUpdateDto">Datos del empleado a actualizar (campos opcionales)</param>
        /// <returns>Empleado actualizado</returns>
        /// <response code="200">Empleado actualizado exitosamente</response>
        /// <response code="400">Datos inválidos o validación fallida</response>
        /// <response code="404">Empleado no encontrado</response>
        [HttpPut("{id}")]
        [Authorize(Policy = "RequirePermission:empleados.actualizar")]
        [ProducesResponseType(typeof(ApiResponse<EmpleadoResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<EmpleadoResponseDto>>> UpdateEmpleado(
            int id,
            [FromBody] EmpleadoUpdateDto empleadoUpdateDto)
        {
            try
            {
                var empleado = await _empleadoService.UpdateEmpleadoAsync(id, empleadoUpdateDto);

                return Ok(new ApiResponse<EmpleadoResponseDto>
                {
                    Success = true,
                    Message = "Empleado actualizado exitosamente",
                    Data = empleado
                });
            }
            catch (InvalidOperationException ex)
            {
                // Puede ser 404 o 400 dependiendo del mensaje
                if (ex.Message.Contains("No existe"))
                {
                    return NotFound(new ApiErrorResponse
                    {
                        Message = ex.Message
                    });
                }

                return BadRequest(new ApiErrorResponse
                {
                    Message = "Error de validación al actualizar el empleado",
                    ErrorCode = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiErrorResponse
                {
                    Message = "Error al actualizar el empleado",
                    ErrorCode = ex.Message
                });
            }
        }

        /// <summary>
        /// Eliminar un empleado (solo si no tiene subordinados)
        /// </summary>
        /// <param name="id">ID del empleado a eliminar</param>
        /// <returns>Confirmación de eliminación</returns>
        /// <response code="200">Empleado eliminado exitosamente</response>
        /// <response code="400">No se puede eliminar porque tiene subordinados</response>
        /// <response code="404">Empleado no encontrado</response>
        [HttpDelete("{id}")]
        [Authorize(Policy = "RequirePermission:empleados.eliminar")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteEmpleado(int id)
        {
            try
            {
                var resultado = await _empleadoService.DeleteEmpleadoAsync(id);

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Empleado eliminado exitosamente",
                    Data = resultado
                });
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("No existe"))
                {
                    return NotFound(new ApiErrorResponse
                    {
                        Message = ex.Message
                    });
                }

                return BadRequest(new ApiErrorResponse
                {
                    Message = "No se puede eliminar el empleado",
                    ErrorCode = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiErrorResponse
                {
                    Message = "Error al eliminar el empleado",
                    ErrorCode = ex.Message
                });
            }
        }

        // ========================================
        // ENDPOINTS DE BÚSQUEDA
        // ========================================

        /// <summary>
        /// Buscar empleado por documento de identidad
        /// </summary>
        /// <param name="documento">Número de documento de identidad</param>
        /// <returns>Empleado con el documento especificado</returns>
        /// <response code="200">Empleado encontrado exitosamente</response>
        /// <response code="404">Empleado no encontrado</response>
        [HttpGet("documento/{documento}")]
        [Authorize(Policy = "RequirePermission:empleados.leer")]
        [ProducesResponseType(typeof(ApiResponse<EmpleadoResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<EmpleadoResponseDto>>> GetEmpleadoByDocumento(string documento)
        {
            try
            {
                var empleado = await _empleadoService.GetEmpleadoByDocumentoAsync(documento);

                if (empleado == null)
                {
                    return NotFound(new ApiErrorResponse
                    {
                        Message = $"No se encontró un empleado con documento {documento}"
                    });
                }

                return Ok(new ApiResponse<EmpleadoResponseDto>
                {
                    Success = true,
                    Message = "Empleado obtenido exitosamente",
                    Data = empleado
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiErrorResponse
                {
                    Message = "Error al buscar el empleado por documento",
                    ErrorCode = ex.Message
                });
            }
        }

        /// <summary>
        /// Buscar empleados por nombre o apellido
        /// </summary>
        /// <param name="termino">Término de búsqueda (nombre o apellido)</param>
        /// <returns>Lista de empleados que coinciden con el término de búsqueda</returns>
        /// <response code="200">Búsqueda realizada exitosamente</response>
        [HttpGet("buscar/{termino}")]
        [Authorize(Policy = "RequirePermission:empleados.leer")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmpleadoResponseDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmpleadoResponseDto>>>> BuscarEmpleados(string termino)
        {
            try
            {
                var empleados = await _empleadoService.BuscarEmpleadosPorNombreAsync(termino);

                return Ok(new ApiResponse<IEnumerable<EmpleadoResponseDto>>
                {
                    Success = true,
                    Message = $"Búsqueda completada. Se encontraron {empleados.Count()} empleados",
                    Data = empleados
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiErrorResponse
                {
                    Message = "Error al buscar empleados",
                    ErrorCode = ex.Message
                });
            }
        }

        /// <summary>
        /// Obtener empleados por cargo
        /// </summary>
        /// <param name="cargo">Cargo a filtrar</param>
        /// <returns>Lista de empleados con el cargo especificado</returns>
        /// <response code="200">Empleados filtrados exitosamente</response>
        [HttpGet("cargo/{cargo}")]
        [Authorize(Policy = "RequirePermission:empleados.leer")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmpleadoResponseDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmpleadoResponseDto>>>> GetEmpleadosPorCargo(string cargo)
        {
            try
            {
                var empleados = await _empleadoService.GetEmpleadosPorCargoAsync(cargo);

                return Ok(new ApiResponse<IEnumerable<EmpleadoResponseDto>>
                {
                    Success = true,
                    Message = $"Se encontraron {empleados.Count()} empleados con cargo {cargo}",
                    Data = empleados
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiErrorResponse
                {
                    Message = "Error al obtener empleados por cargo",
                    ErrorCode = ex.Message
                });
            }
        }

        /// <summary>
        /// Obtener empleados por estado
        /// </summary>
        /// <param name="estado">Estado a filtrar (activo, inactivo, vacaciones, licencia)</param>
        /// <returns>Lista de empleados con el estado especificado</returns>
        /// <response code="200">Empleados filtrados exitosamente</response>
        [HttpGet("estado/{estado}")]
        [Authorize(Policy = "RequirePermission:empleados.leer")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmpleadoResponseDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmpleadoResponseDto>>>> GetEmpleadosPorEstado(string estado)
        {
            try
            {
                var empleados = await _empleadoService.GetEmpleadosPorEstadoAsync(estado);

                return Ok(new ApiResponse<IEnumerable<EmpleadoResponseDto>>
                {
                    Success = true,
                    Message = $"Se encontraron {empleados.Count()} empleados con estado {estado}",
                    Data = empleados
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiErrorResponse
                {
                    Message = "Error al obtener empleados por estado",
                    ErrorCode = ex.Message
                });
            }
        }

        // ========================================
        // ENDPOINTS DE JERARQUÍA
        // ========================================

        /// <summary>
        /// Obtener subordinados directos de un empleado
        /// </summary>
        /// <param name="idJefe">ID del jefe</param>
        /// <returns>Lista de empleados que reportan directamente al jefe especificado</returns>
        /// <response code="200">Subordinados obtenidos exitosamente</response>
        [HttpGet("{idJefe}/subordinados")]
        [Authorize(Policy = "RequirePermission:empleados.leer")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmpleadoResponseDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmpleadoResponseDto>>>> GetSubordinadosDirectos(int idJefe)
        {
            try
            {
                var subordinados = await _empleadoService.GetSubordinadosDirectosAsync(idJefe);

                return Ok(new ApiResponse<IEnumerable<EmpleadoResponseDto>>
                {
                    Success = true,
                    Message = $"Se encontraron {subordinados.Count()} subordinados directos",
                    Data = subordinados
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiErrorResponse
                {
                    Message = "Error al obtener subordinados directos",
                    ErrorCode = ex.Message
                });
            }
        }

        /// <summary>
        /// Obtener todos los subordinados de un empleado (todos los niveles - recursivo)
        /// </summary>
        /// <param name="idJefe">ID del jefe</param>
        /// <returns>Lista completa de todos los subordinados en la jerarquía</returns>
        /// <response code="200">Todos los subordinados obtenidos exitosamente</response>
        [HttpGet("{idJefe}/subordinados/todos")]
        [Authorize(Policy = "RequirePermission:empleados.leer")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmpleadoResponseDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmpleadoResponseDto>>>> GetTodosLosSubordinados(int idJefe)
        {
            try
            {
                var subordinados = await _empleadoService.GetTodosLosSubordinadosAsync(idJefe);

                return Ok(new ApiResponse<IEnumerable<EmpleadoResponseDto>>
                {
                    Success = true,
                    Message = $"Se encontraron {subordinados.Count()} subordinados en total (todos los niveles)",
                    Data = subordinados
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiErrorResponse
                {
                    Message = "Error al obtener todos los subordinados",
                    ErrorCode = ex.Message
                });
            }
        }

        /// <summary>
        /// Obtener el jefe directo de un empleado
        /// </summary>
        /// <param name="idEmpleado">ID del empleado</param>
        /// <returns>Datos del jefe directo del empleado</returns>
        /// <response code="200">Jefe obtenido exitosamente</response>
        /// <response code="404">El empleado no tiene jefe asignado</response>
        [HttpGet("{idEmpleado}/jefe")]
        [Authorize(Policy = "RequirePermission:empleados.leer")]
        [ProducesResponseType(typeof(ApiResponse<EmpleadoResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<EmpleadoResponseDto>>> GetJefeDirecto(int idEmpleado)
        {
            try
            {
                var jefe = await _empleadoService.GetJefeDirectoAsync(idEmpleado);

                if (jefe == null)
                {
                    return NotFound(new ApiErrorResponse
                    {
                        Message = "El empleado no tiene un jefe asignado"
                    });
                }

                return Ok(new ApiResponse<EmpleadoResponseDto>
                {
                    Success = true,
                    Message = "Jefe directo obtenido exitosamente",
                    Data = jefe
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiErrorResponse
                {
                    Message = "Error al obtener el jefe directo",
                    ErrorCode = ex.Message
                });
            }
        }

        /// <summary>
        /// Obtener la cadena de jefes hasta el nivel más alto (CEO)
        /// </summary>
        /// <param name="idEmpleado">ID del empleado</param>
        /// <returns>Lista de jefes en orden ascendente (jefe inmediato → CEO)</returns>
        /// <response code="200">Cadena de jefes obtenida exitosamente</response>
        [HttpGet("{idEmpleado}/cadena-jefes")]
        [Authorize(Policy = "RequirePermission:empleados.leer")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmpleadoResponseDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmpleadoResponseDto>>>> GetCadenaDeJefes(int idEmpleado)
        {
            try
            {
                var cadenaJefes = await _empleadoService.GetCadenaDeJefesAsync(idEmpleado);

                return Ok(new ApiResponse<IEnumerable<EmpleadoResponseDto>>
                {
                    Success = true,
                    Message = $"Cadena de {cadenaJefes.Count()} jefes obtenida exitosamente",
                    Data = cadenaJefes
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiErrorResponse
                {
                    Message = "Error al obtener la cadena de jefes",
                    ErrorCode = ex.Message
                });
            }
        }

        /// <summary>
        /// Obtener empleados sin jefe (nivel más alto de la organización)
        /// </summary>
        /// <returns>Lista de empleados del nivel superior (CEO, Gerentes Generales, etc.)</returns>
        /// <response code="200">Empleados de nivel superior obtenidos exitosamente</response>
        [HttpGet("sin-jefe")]
        [Authorize(Policy = "RequirePermission:empleados.leer")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmpleadoResponseDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmpleadoResponseDto>>>> GetEmpleadosSinJefe()
        {
            try
            {
                var empleados = await _empleadoService.GetEmpleadosSinJefeAsync();

                return Ok(new ApiResponse<IEnumerable<EmpleadoResponseDto>>
                {
                    Success = true,
                    Message = $"Se encontraron {empleados.Count()} empleados de nivel superior",
                    Data = empleados
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiErrorResponse
                {
                    Message = "Error al obtener empleados sin jefe",
                    ErrorCode = ex.Message
                });
            }
        }

        /// <summary>
        /// Obtener empleados que son jefes (tienen subordinados)
        /// </summary>
        /// <returns>Lista de empleados que tienen al menos un subordinado</returns>
        /// <response code="200">Jefes obtenidos exitosamente</response>
        [HttpGet("jefes")]
        [Authorize(Policy = "RequirePermission:empleados.leer")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmpleadoResponseDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmpleadoResponseDto>>>> GetEmpleadosQuesonJefes()
        {
            try
            {
                var jefes = await _empleadoService.GetEmpleadosQuesonJefesAsync();

                return Ok(new ApiResponse<IEnumerable<EmpleadoResponseDto>>
                {
                    Success = true,
                    Message = $"Se encontraron {jefes.Count()} empleados que son jefes",
                    Data = jefes
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiErrorResponse
                {
                    Message = "Error al obtener empleados que son jefes",
                    ErrorCode = ex.Message
                });
            }
        }

        /// <summary>
        /// Obtener el organigrama completo de la empresa
        /// </summary>
        /// <returns>Estructura jerárquica completa de la organización</returns>
        /// <response code="200">Organigrama obtenido exitosamente</response>
        [HttpGet("organigrama")]
        [Authorize(Policy = "RequirePermission:empleados.leer")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmpleadoResponseDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmpleadoResponseDto>>>> GetOrganigramaCompleto()
        {
            try
            {
                var organigrama = await _empleadoService.GetOrganigramaCompletoAsync();

                return Ok(new ApiResponse<IEnumerable<EmpleadoResponseDto>>
                {
                    Success = true,
                    Message = "Organigrama completo obtenido exitosamente",
                    Data = organigrama
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiErrorResponse
                {
                    Message = "Error al obtener el organigrama completo",
                    ErrorCode = ex.Message
                });
            }
        }

        // ========================================
        // ENDPOINTS DE GESTIÓN DE JERARQUÍA
        // ========================================

        /// <summary>
        /// Cambiar el jefe de un empleado
        /// </summary>
        /// <param name="idEmpleado">ID del empleado</param>
        /// <param name="idNuevoJefe">ID del nuevo jefe (null para quitar jefe)</param>
        /// <returns>Confirmación del cambio</returns>
        /// <response code="200">Jefe cambiado exitosamente</response>
        /// <response code="400">No se puede cambiar el jefe (ciclo detectado)</response>
        [HttpPut("{idEmpleado}/cambiar-jefe")]
        [Authorize(Policy = "RequirePermission:empleados.actualizar")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<bool>>> CambiarJefe(int idEmpleado, [FromBody] int? idNuevoJefe)
        {
            try
            {
                var resultado = await _empleadoService.CambiarJefeAsync(idEmpleado, idNuevoJefe);

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Jefe cambiado exitosamente",
                    Data = resultado
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiErrorResponse
                {
                    Message = "No se pudo cambiar el jefe",
                    ErrorCode = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiErrorResponse
                {
                    Message = "Error al cambiar el jefe",
                    ErrorCode = ex.Message
                });
            }
        }

        /// <summary>
        /// Reasignar todos los subordinados de un jefe a otro jefe
        /// </summary>
        /// <param name="idJefeActual">ID del jefe actual</param>
        /// <param name="idNuevoJefe">ID del nuevo jefe (null para quitar jefe)</param>
        /// <returns>Confirmación de la reasignación</returns>
        /// <response code="200">Subordinados reasignados exitosamente</response>
        /// <response code="400">Error en la reasignación</response>
        [HttpPut("{idJefeActual}/reasignar-subordinados")]
        [Authorize(Policy = "RequirePermission:empleados.actualizar")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<bool>>> ReasignarSubordinados(
            int idJefeActual,
            [FromBody] int? idNuevoJefe)
        {
            try
            {
                var resultado = await _empleadoService.ReasignarSubordinadosAsync(idJefeActual, idNuevoJefe);

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Subordinados reasignados exitosamente",
                    Data = resultado
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiErrorResponse
                {
                    Message = "No se pudieron reasignar los subordinados",
                    ErrorCode = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiErrorResponse
                {
                    Message = "Error al reasignar subordinados",
                    ErrorCode = ex.Message
                });
            }
        }

        // ========================================
        // ENDPOINTS DE OPERACIONES ESPECIALES
        // ========================================

        /// <summary>
        /// Cambiar el estado de un empleado
        /// </summary>
        /// <param name="idEmpleado">ID del empleado</param>
        /// <param name="nuevoEstado">Nuevo estado (activo, inactivo, vacaciones, licencia)</param>
        /// <returns>Confirmación del cambio de estado</returns>
        /// <response code="200">Estado cambiado exitosamente</response>
        /// <response code="400">Estado inválido</response>
        [HttpPut("{idEmpleado}/cambiar-estado")]
        [Authorize(Policy = "RequirePermission:empleados.actualizar")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<bool>>> CambiarEstado(int idEmpleado, [FromBody] string nuevoEstado)
        {
            try
            {
                var resultado = await _empleadoService.CambiarEstadoEmpleadoAsync(idEmpleado, nuevoEstado);

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Message = $"Estado cambiado exitosamente a '{nuevoEstado}'",
                    Data = resultado
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiErrorResponse
                {
                    Message = "No se pudo cambiar el estado",
                    ErrorCode = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiErrorResponse
                {
                    Message = "Error al cambiar el estado",
                    ErrorCode = ex.Message
                });
            }
        }

        /// <summary>
        /// Actualizar el salario de un empleado
        /// </summary>
        /// <param name="idEmpleado">ID del empleado</param>
        /// <param name="nuevoSalario">Nuevo salario</param>
        /// <returns>Confirmación de la actualización</returns>
        /// <response code="200">Salario actualizado exitosamente</response>
        /// <response code="400">Salario inválido</response>
        [HttpPut("{idEmpleado}/actualizar-salario")]
        [Authorize(Policy = "RequirePermission:empleados.actualizar")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<bool>>> ActualizarSalario(int idEmpleado, [FromBody] decimal nuevoSalario)
        {
            try
            {
                var resultado = await _empleadoService.ActualizarSalarioAsync(idEmpleado, nuevoSalario);

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Salario actualizado exitosamente",
                    Data = resultado
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiErrorResponse
                {
                    Message = "No se pudo actualizar el salario",
                    ErrorCode = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiErrorResponse
                {
                    Message = "Error al actualizar el salario",
                    ErrorCode = ex.Message
                });
            }
        }

        // ========================================
        // ENDPOINTS DE ESTADÍSTICAS
        // ========================================

        /// <summary>
        /// Obtener estadísticas generales de empleados
        /// </summary>
        /// <returns>Estadísticas por cargo, por estado y totales</returns>
        /// <response code="200">Estadísticas obtenidas exitosamente</response>
        [HttpGet("estadisticas")]
        [Authorize(Policy = "RequirePermission:empleados.leer")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<object>>> GetEstadisticas()
        {
            try
            {
                var estadisticasCargo = await _empleadoService.GetEstadisticasPorCargoAsync();
                var estadisticasEstado = await _empleadoService.GetEstadisticasPorEstadoAsync();
                var totalActivos = await _empleadoService.ContarEmpleadosActivosAsync();
                var promedioAntiguedad = await _empleadoService.GetPromedioAntiguedadAsync();

                var estadisticas = new
                {
                    TotalEmpleadosActivos = totalActivos,
                    PromedioAntiguedadAños = Math.Round(promedioAntiguedad, 2),
                    EmpleadosPorCargo = estadisticasCargo,
                    EmpleadosPorEstado = estadisticasEstado
                };

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Estadísticas obtenidas exitosamente",
                    Data = estadisticas
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiErrorResponse
                {
                    Message = "Error al obtener estadísticas",
                    ErrorCode = ex.Message
                });
            }
        }

        /// <summary>
        /// Obtener empleados con mayor antigüedad
        /// </summary>
        /// <param name="cantidad">Cantidad de empleados a retornar (default: 10)</param>
        /// <returns>Lista de empleados con mayor antigüedad</returns>
        /// <response code="200">Empleados obtenidos exitosamente</response>
        [HttpGet("mayor-antiguedad")]
        [Authorize(Policy = "RequirePermission:empleados.leer")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmpleadoResponseDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmpleadoResponseDto>>>> GetEmpleadosConMayorAntiguedad(
            [FromQuery] int cantidad = 10)
        {
            try
            {
                var empleados = await _empleadoService.GetEmpleadosConMayorAntiguedadAsync(cantidad);

                return Ok(new ApiResponse<IEnumerable<EmpleadoResponseDto>>
                {
                    Success = true,
                    Message = $"Top {cantidad} empleados con mayor antigüedad obtenidos exitosamente",
                    Data = empleados
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiErrorResponse
                {
                    Message = "Error al obtener empleados con mayor antigüedad",
                    ErrorCode = ex.Message
                });
            }
        }
    }
}