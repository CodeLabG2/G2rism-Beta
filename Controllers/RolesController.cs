using Microsoft.AspNetCore.Mvc;
using G2rismBeta.API.DTOs.Rol;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.DTOs.RolPermiso;

namespace G2rismBeta.API.Controllers;

/// <summary>
/// Controlador para la gestión de Roles
/// Endpoints para operaciones CRUD de roles y asignación de permisos
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRolService _rolService;

    /// <summary>
    /// Constructor: Recibe el servicio de roles por inyección de dependencias
    /// </summary>
    public RolesController(IRolService rolService)
    {
        _rolService = rolService;
    }

    // ========================================
    // ENDPOINTS DE CONSULTA (GET)
    // ========================================

    /// <summary>
    /// Obtener todos los roles del sistema
    /// </summary>
    /// <remarks>
    /// Ejemplo de request:
    /// 
    ///     GET /api/roles
    /// 
    /// </remarks>
    /// <response code="200">Lista de roles obtenida exitosamente</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RolResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RolResponseDto>>> GetAllRoles()
    {
        try
        {
            var roles = await _rolService.GetAllRolesAsync();
            return Ok(roles);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener los roles", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtener solo roles activos
    /// </summary>
    /// <response code="200">Lista de roles activos obtenida exitosamente</response>
    [HttpGet("activos")]
    [ProducesResponseType(typeof(IEnumerable<RolResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RolResponseDto>>> GetRolesActivos()
    {
        try
        {
            var roles = await _rolService.GetRolesActivosAsync();
            return Ok(roles);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener los roles activos", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtener un rol específico por su ID
    /// </summary>
    /// <param name="id">ID del rol a buscar</param>
    /// <remarks>
    /// Ejemplo de request:
    /// 
    ///     GET /api/roles/1
    /// 
    /// </remarks>
    /// <response code="200">Rol encontrado</response>
    /// <response code="404">Rol no encontrado</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RolResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RolResponseDto>> GetRolById(int id)
    {
        try
        {
            var rol = await _rolService.GetRolByIdAsync(id);

            if (rol == null)
            {
                return NotFound(new { message = $"No se encontró el rol con ID {id}" });
            }

            return Ok(rol);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener el rol", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtener un rol con todos sus permisos incluidos
    /// </summary>
    /// <param name="id">ID del rol</param>
    /// <response code="200">Rol con permisos obtenido exitosamente</response>
    /// <response code="404">Rol no encontrado</response>
    [HttpGet("{id}/permisos")]
    [ProducesResponseType(typeof(RolConPermisosDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RolConPermisosDto>> GetRolConPermisos(int id)
    {
        try
        {
            var rol = await _rolService.GetRolConPermisosAsync(id);

            if (rol == null)
            {
                return NotFound(new { message = $"No se encontró el rol con ID {id}" });
            }

            return Ok(rol);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener el rol con permisos", error = ex.Message });
        }
    }

    // ========================================
    // ENDPOINTS DE CREACIÓN (POST)
    // ========================================

    /// <summary>
    /// Crear un nuevo rol
    /// </summary>
    /// <param name="rolCreateDto">Datos del rol a crear</param>
    /// <remarks>
    /// Ejemplo de request:
    /// 
    ///     POST /api/roles
    ///     {
    ///         "nombre": "Gerente",
    ///         "descripcion": "Rol con permisos de gestión",
    ///         "nivelAcceso": 5
    ///     }
    /// 
    /// </remarks>
    /// <response code="201">Rol creado exitosamente</response>
    /// <response code="400">Datos inválidos o nombre duplicado</response>
    [HttpPost]
    [ProducesResponseType(typeof(RolResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RolResponseDto>> CreateRol([FromBody] RolCreateDto rolCreateDto)
    {
        try
        {
            // Validar que el modelo sea válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rolCreado = await _rolService.CreateRolAsync(rolCreateDto);

            // Retornar 201 Created con la ubicación del recurso creado
            return CreatedAtAction(
                nameof(GetRolById),           // Nombre del método para obtener el recurso
                new { id = rolCreado.IdRol }, // Parámetros de la ruta
                rolCreado);                   // El objeto creado
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al crear el rol", error = ex.Message });
        }
    }

    // ========================================
    // ENDPOINTS DE ACTUALIZACIÓN (PUT)
    // ========================================

    /// <summary>
    /// Actualizar un rol existente
    /// </summary>
    /// <param name="id">ID del rol a actualizar</param>
    /// <param name="rolUpdateDto">Nuevos datos del rol</param>
    /// <remarks>
    /// Ejemplo de request:
    /// 
    ///     PUT /api/roles/1
    ///     {
    ///         "nombre": "Gerente General",
    ///         "descripcion": "Rol con permisos administrativos completos",
    ///         "nivelAcceso": 3
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">Rol actualizado exitosamente</response>
    /// <response code="400">Datos inválidos</response>
    /// <response code="404">Rol no encontrado</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(RolResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RolResponseDto>> UpdateRol(int id, [FromBody] RolUpdateDto rolUpdateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rolActualizado = await _rolService.UpdateRolAsync(id, rolUpdateDto);

            return Ok(rolActualizado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al actualizar el rol", error = ex.Message });
        }
    }

    // ========================================
    // ENDPOINTS DE ELIMINACIÓN (DELETE)
    // ========================================

    /// <summary>
    /// Eliminar un rol (si no tiene usuarios asignados)
    /// </summary>
    /// <param name="id">ID del rol a eliminar</param>
    /// <remarks>
    /// Ejemplo de request:
    /// 
    ///     DELETE /api/roles/5
    /// 
    /// </remarks>
    /// <response code="204">Rol eliminado exitosamente</response>
    /// <response code="404">Rol no encontrado</response>
    /// <response code="409">No se puede eliminar (tiene usuarios o es rol del sistema)</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> DeleteRol(int id)
    {
        try
        {
            var resultado = await _rolService.DeleteRolAsync(id);

            if (!resultado)
            {
                return NotFound(new { message = $"No se encontró el rol con ID {id}" });
            }

            // 204 No Content = Eliminado exitosamente (sin contenido en la respuesta)
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al eliminar el rol", error = ex.Message });
        }
    }

    // ========================================
    // ENDPOINTS ESPECIALES (PATCH)
    // ========================================

    /// <summary>
    /// Cambiar el estado de un rol (activar/desactivar)
    /// </summary>
    /// <param name="id">ID del rol</param>
    /// <param name="estado">Nuevo estado (true = activo, false = inactivo)</param>
    /// <remarks>
    /// Ejemplo de request:
    /// 
    ///     PATCH /api/roles/1/estado?estado=false
    /// 
    /// </remarks>
    /// <response code="200">Estado cambiado exitosamente</response>
    /// <response code="404">Rol no encontrado</response>
    [HttpPatch("{id}/estado")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> CambiarEstadoRol(int id, [FromQuery] bool estado)
    {
        try
        {
            var resultado = await _rolService.CambiarEstadoRolAsync(id, estado);

            if (!resultado)
            {
                return NotFound(new { message = $"No se encontró el rol con ID {id}" });
            }

            return Ok(new
            {
                message = $"Estado del rol cambiado a {(estado ? "activo" : "inactivo")}"
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al cambiar el estado", error = ex.Message });
        }
    }

    /// <summary>
    /// Asignar múltiples permisos a un rol (reemplaza los permisos anteriores)
    /// </summary>
    /// <param name="id">ID del rol</param>
    /// <param name="asignarPermisosDto">Lista de IDs de permisos a asignar</param>
    /// <remarks>
    /// Ejemplo de request:
    /// 
    ///     POST /api/roles/1/permisos
    ///     {
    ///         "idsPermisos": [1, 2, 3, 5, 8]
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">Permisos asignados exitosamente</response>
    /// <response code="400">Datos inválidos</response>
    /// <response code="404">Rol o permiso no encontrado</response>
    [HttpPost("{id}/permisos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> AsignarPermisos(int id, [FromBody] AsignarPermisosMultiplesDto asignarPermisosDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resultado = await _rolService.AsignarPermisosAsync(id, asignarPermisosDto.IdsPermisos);

            if (!resultado)
            {
                return BadRequest(new { message = "No se pudieron asignar los permisos" });
            }

            return Ok(new
            {
                message = $"Se asignaron {asignarPermisosDto.IdsPermisos.Count} permiso(s) al rol"
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al asignar permisos", error = ex.Message });
        }
    }

    /// <summary>
    /// Remover un permiso específico de un rol
    /// </summary>
    /// <param name="idRol">ID del rol</param>
    /// <param name="idPermiso">ID del permiso a remover</param>
    /// <remarks>
    /// Ejemplo de request:
    /// 
    ///     DELETE /api/roles/1/permisos/5
    /// 
    /// </remarks>
    /// <response code="200">Permiso removido exitosamente</response>
    /// <response code="404">Rol o permiso no encontrado</response>
    [HttpDelete("{idRol}/permisos/{idPermiso}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> RemoverPermiso(int idRol, int idPermiso)
    {
        try
        {
            var resultado = await _rolService.RemoverPermisoAsync(idRol, idPermiso);

            if (!resultado)
            {
                return NotFound(new { message = "No se pudo remover el permiso" });
            }

            return Ok(new { message = "Permiso removido exitosamente del rol" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al remover el permiso", error = ex.Message });
        }
    }

    // ========================================
    // ENDPOINTS DE VALIDACIÓN
    // ========================================

    /// <summary>
    /// Verificar si un nombre de rol ya existe
    /// </summary>
    /// <param name="nombre">Nombre a verificar</param>
    /// <param name="idExcluir">ID del rol a excluir de la búsqueda (opcional)</param>
    /// <remarks>
    /// Ejemplo de request:
    /// 
    ///     GET /api/roles/existe?nombre=Administrador
    ///     GET /api/roles/existe?nombre=Gerente&amp;idExcluir=5
    /// 
    /// </remarks>
    /// <response code="200">Resultado de la validación</response>
    [HttpGet("existe")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> NombreRolExiste([FromQuery] string nombre, [FromQuery] int? idExcluir = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return BadRequest(new { message = "El nombre es obligatorio" });
            }

            var existe = await _rolService.NombreRolExisteAsync(nombre, idExcluir);
            return Ok(new { nombre, existe });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al verificar el nombre", error = ex.Message });
        }
    }
}
