using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using G2rismBeta.API.DTOs.Permiso;
using G2rismBeta.API.Interfaces;

namespace G2rismBeta.API.Controllers;

/// <summary>
/// Controlador para la gestión de Permisos
/// Endpoints para operaciones CRUD de permisos
/// Requiere autenticación. Solo accesible para Super Administrador y Administrador.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Super Administrador,Administrador")]
public class PermisosController : ControllerBase
{
    private readonly IPermisoService _permisoService;

    public PermisosController(IPermisoService permisoService)
    {
        _permisoService = permisoService;
    }

    // ========================================
    // ENDPOINTS DE CONSULTA (GET)
    // ========================================

    /// <summary>
    /// Obtener todos los permisos del sistema
    /// </summary>
    [HttpGet]
    [Authorize(Policy = "RequirePermission:permisos.leer")]
    [ProducesResponseType(typeof(IEnumerable<PermisoResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PermisoResponseDto>>> GetAllPermisos()
    {
        try
        {
            var permisos = await _permisoService.GetAllPermisosAsync();
            return Ok(permisos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener los permisos", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtener solo permisos activos
    /// </summary>
    [HttpGet("activos")]
    [ProducesResponseType(typeof(IEnumerable<PermisoResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PermisoResponseDto>>> GetPermisosActivos()
    {
        try
        {
            var permisos = await _permisoService.GetPermisosActivosAsync();
            return Ok(permisos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener los permisos activos", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtener un permiso por su ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PermisoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PermisoResponseDto>> GetPermisoById(int id)
    {
        try
        {
            var permiso = await _permisoService.GetPermisoByIdAsync(id);

            if (permiso == null)
            {
                return NotFound(new { message = $"No se encontró el permiso con ID {id}" });
            }

            return Ok(permiso);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener el permiso", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtener permisos por módulo
    /// </summary>
    [HttpGet("modulo/{modulo}")]
    [ProducesResponseType(typeof(IEnumerable<PermisoResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PermisoResponseDto>>> GetPermisosPorModulo(string modulo)
    {
        try
        {
            var permisos = await _permisoService.GetPermisosPorModuloAsync(modulo);
            return Ok(permisos);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener permisos del módulo", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtener lista de módulos únicos
    /// </summary>
    [HttpGet("modulos")]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<string>>> GetModulos()
    {
        try
        {
            var modulos = await _permisoService.GetModulosAsync();
            return Ok(modulos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener los módulos", error = ex.Message });
        }
    }

    /// <summary>
    /// Buscar un permiso por módulo y acción
    /// </summary>
    /// <param name="modulo">Nombre del módulo</param>
    /// <param name="accion">Nombre de la acción</param>
    /// <returns>El permiso encontrado o 404 si no existe</returns>
    [HttpGet("modulo/{modulo}/accion/{accion}")]
    [ProducesResponseType(typeof(PermisoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PermisoResponseDto>> GetPermisoByModuloYAccion(string modulo, string accion)
    {
        try
        {
            var permiso = await _permisoService.GetPermisoByModuloYAccionAsync(modulo, accion);

            if (permiso == null)
            {
                return NotFound(new { message = $"No se encontró el permiso '{modulo}.{accion}'" });
            }

            return Ok(permiso);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al buscar el permiso", error = ex.Message });
        }
    }

    /// <summary>
    /// Buscar permisos por término (busca en módulo, acción, nombre y descripción)
    /// </summary>
    /// <param name="termino">Término de búsqueda</param>
    /// <returns>Lista de permisos que coinciden con el término</returns>
    [HttpGet("buscar")]
    [ProducesResponseType(typeof(IEnumerable<PermisoResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PermisoResponseDto>>> BuscarPermisos([FromQuery] string termino)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(termino))
            {
                return BadRequest(new { message = "El término de búsqueda es obligatorio" });
            }

            var permisos = await _permisoService.BuscarPermisosAsync(termino);
            return Ok(new { termino, cantidad = permisos.Count(), permisos });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al buscar permisos", error = ex.Message });
        }
    }

    // ========================================
    // ENDPOINTS DE CREACIÓN (POST)
    // ========================================

    /// <summary>
    /// Crear un nuevo permiso
    /// </summary>
    /// <remarks>
    /// Ejemplo de request:
    /// 
    ///     POST /api/permisos
    ///     {
    ///         "modulo": "Usuarios",
    ///         "accion": "Crear",
    ///         "descripcion": "Permite crear nuevos usuarios"
    ///     }
    /// 
    /// Nota: El campo nombrePermiso se genera automáticamente como "modulo.accion"
    /// </remarks>
    [HttpPost]
    [Authorize(Policy = "RequirePermission:permisos.crear")]
    [ProducesResponseType(typeof(PermisoResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PermisoResponseDto>> CreatePermiso([FromBody] PermisoCreateDto permisoCreateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var permisoCreado = await _permisoService.CreatePermisoAsync(permisoCreateDto);

            return CreatedAtAction(
                nameof(GetPermisoById),
                new { id = permisoCreado.IdPermiso },
                permisoCreado);
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
            return StatusCode(500, new { message = "Error al crear el permiso", error = ex.Message });
        }
    }

    // ========================================
    // ENDPOINTS DE ACTUALIZACIÓN (PUT)
    // ========================================

    /// <summary>
    /// Actualizar un permiso existente
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Policy = "RequirePermission:permisos.actualizar")]
    [ProducesResponseType(typeof(PermisoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PermisoResponseDto>> UpdatePermiso(int id, [FromBody] PermisoUpdateDto permisoUpdateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var permisoActualizado = await _permisoService.UpdatePermisoAsync(id, permisoUpdateDto);

            return Ok(permisoActualizado);
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
            return StatusCode(500, new { message = "Error al actualizar el permiso", error = ex.Message });
        }
    }

    // ========================================
    // ENDPOINTS DE ELIMINACIÓN (DELETE)
    // ========================================

    /// <summary>
    /// Eliminar un permiso (si no está asignado a ningún rol)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Policy = "RequirePermission:permisos.eliminar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> DeletePermiso(int id)
    {
        try
        {
            var resultado = await _permisoService.DeletePermisoAsync(id);

            if (!resultado)
            {
                return NotFound(new { message = $"No se encontró el permiso con ID {id}" });
            }

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
            return StatusCode(500, new { message = "Error al eliminar el permiso", error = ex.Message });
        }
    }

    // ========================================
    // ENDPOINTS ESPECIALES (PATCH)
    // ========================================

    /// <summary>
    /// Cambiar el estado de un permiso
    /// </summary>
    [HttpPatch("{id}/estado")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> CambiarEstadoPermiso(int id, [FromQuery] bool estado)
    {
        try
        {
            var resultado = await _permisoService.CambiarEstadoPermisoAsync(id, estado);

            if (!resultado)
            {
                return NotFound(new { message = $"No se encontró el permiso con ID {id}" });
            }

            return Ok(new
            {
                message = $"Estado del permiso cambiado a {(estado ? "activo" : "inactivo")}"
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al cambiar el estado", error = ex.Message });
        }
    }

    // ========================================
    // ENDPOINTS DE VALIDACIÓN
    // ========================================

    /// <summary>
    /// Verificar si un nombre de permiso ya existe
    /// </summary>
    [HttpGet("existe")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> NombrePermisoExiste([FromQuery] string nombrePermiso, [FromQuery] int? idExcluir = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(nombrePermiso))
            {
                return BadRequest(new { message = "El nombre del permiso es obligatorio" });
            }

            var existe = await _permisoService.NombrePermisoExisteAsync(nombrePermiso, idExcluir);
            return Ok(new { nombrePermiso, existe });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al verificar el nombre", error = ex.Message });
        }
    }
}
