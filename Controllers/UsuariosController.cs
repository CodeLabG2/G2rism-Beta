using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.DTOs.Usuario;
using G2rismBeta.API.DTOs.UsuarioRol;
using G2rismBeta.API.Models;

namespace G2rismBeta.API.Controllers;

/// <summary>
/// Controlador para la gestión de usuarios
/// Requiere autenticación. Solo accesible para Super Administrador y Administrador.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize(Roles = "Super Administrador,Administrador")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    private readonly IMapper _mapper;
    private readonly ILogger<UsuariosController> _logger;

    public UsuariosController(
        IUsuarioService usuarioService,
        IMapper mapper,
        ILogger<UsuariosController> logger)
    {
        _usuarioService = usuarioService;
        _mapper = mapper;
        _logger = logger;
    }

    // ========================================
    // ENDPOINT 1: CREAR USUARIO
    // ========================================

    /// <summary>
    /// Crear un nuevo usuario
    /// </summary>
    /// <param name="dto">Datos del usuario a crear</param>
    /// <returns>Usuario creado</returns>
    /// <response code="201">Usuario creado exitosamente</response>
    /// <response code="400">Datos inválidos o usuario duplicado</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<UsuarioResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<UsuarioResponseDto>>> CrearUsuario(
        [FromBody] UsuarioCreateDto dto)
    {
        _logger.LogInformation("📝 Creando nuevo usuario: {Username}", dto.Username);

        // Mapear DTO a Usuario
        var usuario = _mapper.Map<Usuario>(dto);

        // Crear el usuario con contraseña hasheada
        var usuarioCreado = await _usuarioService.CrearUsuarioAsync(
            usuario,
            dto.Password,
            dto.RolesIds
        );

        // Mapear a DTO de respuesta
        var usuarioDto = _mapper.Map<UsuarioResponseDto>(usuarioCreado);

        var response = new ApiResponse<UsuarioResponseDto>
        {
            Success = true,
            Message = "Usuario creado exitosamente",
            Data = usuarioDto
        };

        _logger.LogInformation("✅ Usuario creado: ID={IdUsuario}, Username={Username}",
            usuarioCreado.IdUsuario, usuarioCreado.Username);

        return CreatedAtAction(
            nameof(ObtenerUsuarioPorId),
            new { id = usuarioCreado.IdUsuario },
            response
        );
    }

    // ========================================
    // ENDPOINT 2: OBTENER TODOS LOS USUARIOS
    // ========================================

    /// <summary>
    /// Obtener todos los usuarios
    /// </summary>
    /// <param name="incluirInactivos">Si es true, incluye usuarios inactivos</param>
    /// <returns>Lista de usuarios</returns>
    /// <response code="200">Lista de usuarios obtenida exitosamente</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<UsuarioResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<IEnumerable<UsuarioResponseDto>>>> ObtenerTodosLosUsuarios(
        [FromQuery] bool incluirInactivos = false)
    {
        _logger.LogInformation("📋 Obteniendo todos los usuarios (incluirInactivos: {IncluirInactivos})",
            incluirInactivos);

        // Obtener usuarios según si se incluyen inactivos o no
        var usuarios = incluirInactivos
            ? await _usuarioService.GetAllUsuariosAsync()
            : await _usuarioService.GetUsuariosActivosAsync();

        var usuariosDto = _mapper.Map<IEnumerable<UsuarioResponseDto>>(usuarios);

        var response = new ApiResponse<IEnumerable<UsuarioResponseDto>>
        {
            Success = true,
            Message = $"Se encontraron {usuariosDto.Count()} usuario(s)",
            Data = usuariosDto
        };

        return Ok(response);
    }

    // ========================================
    // ENDPOINT 3: OBTENER USUARIO POR ID
    // ========================================

    /// <summary>
    /// Obtener un usuario por su ID
    /// </summary>
    /// <param name="id">ID del usuario</param>
    /// <returns>Usuario encontrado</returns>
    /// <response code="200">Usuario encontrado</response>
    /// <response code="404">Usuario no encontrado</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<UsuarioResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<UsuarioResponseDto>>> ObtenerUsuarioPorId(int id)
    {
        _logger.LogInformation("🔍 Buscando usuario con ID: {IdUsuario}", id);

        var usuario = await _usuarioService.GetUsuarioByIdAsync(id);

        if (usuario == null)
        {
            _logger.LogWarning("⚠️ Usuario no encontrado: ID={IdUsuario}", id);
            return NotFound(new ApiErrorResponse
            {
                Success = false,
                Message = $"Usuario con ID {id} no encontrado",
                StatusCode = 404
            });
        }

        var usuarioDto = _mapper.Map<UsuarioResponseDto>(usuario);

        var response = new ApiResponse<UsuarioResponseDto>
        {
            Success = true,
            Message = "Usuario encontrado",
            Data = usuarioDto
        };

        return Ok(response);
    }

    // ========================================
    // ENDPOINT 4: OBTENER USUARIO CON ROLES
    // ========================================

    /// <summary>
    /// Obtener un usuario con sus roles asignados
    /// </summary>
    /// <param name="id">ID del usuario</param>
    /// <returns>Usuario con sus roles</returns>
    /// <response code="200">Usuario con roles encontrado</response>
    /// <response code="404">Usuario no encontrado</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpGet("{id}/roles")]
    [ProducesResponseType(typeof(ApiResponse<UsuarioConRolesDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<UsuarioConRolesDto>>> ObtenerUsuarioConRoles(int id)
    {
        _logger.LogInformation("🔍 Buscando usuario con roles: ID={IdUsuario}", id);

        var usuario = await _usuarioService.GetUsuarioByIdConRolesAsync(id);

        if (usuario == null)
        {
            _logger.LogWarning("⚠️ Usuario no encontrado: ID={IdUsuario}", id);
            return NotFound(new ApiErrorResponse
            {
                Success = false,
                Message = $"Usuario con ID {id} no encontrado",
                StatusCode = 404
            });
        }

        var usuarioDto = _mapper.Map<UsuarioConRolesDto>(usuario);

        var response = new ApiResponse<UsuarioConRolesDto>
        {
            Success = true,
            Message = "Usuario con roles encontrado",
            Data = usuarioDto
        };

        return Ok(response);
    }

    // ========================================
    // ENDPOINT 5: ACTUALIZAR USUARIO
    // ========================================

    /// <summary>
    /// Actualizar un usuario existente
    /// </summary>
    /// <param name="id">ID del usuario a actualizar</param>
    /// <param name="dto">Datos a actualizar</param>
    /// <returns>Usuario actualizado</returns>
    /// <response code="200">Usuario actualizado exitosamente</response>
    /// <response code="400">Datos inválidos</response>
    /// <response code="404">Usuario no encontrado</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<UsuarioResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<UsuarioResponseDto>>> ActualizarUsuario(
        int id,
        [FromBody] UsuarioUpdateDto dto)
    {
        _logger.LogInformation("✏️ Actualizando usuario: ID={IdUsuario}", id);

        // Obtener el usuario existente
        var usuarioExistente = await _usuarioService.GetUsuarioByIdAsync(id);

        if (usuarioExistente == null)
        {
            _logger.LogWarning("⚠️ Usuario no encontrado para actualizar: ID={IdUsuario}", id);
            return NotFound(new ApiErrorResponse
            {
                Success = false,
                Message = $"Usuario con ID {id} no encontrado",
                StatusCode = 404
            });
        }

        // Actualizar propiedades desde el DTO
        _mapper.Map(dto, usuarioExistente);
        usuarioExistente.FechaModificacion = DateTime.Now;

        // Actualizar en la base de datos
        var usuarioActualizado = await _usuarioService.ActualizarUsuarioAsync(usuarioExistente);
        var usuarioDto = _mapper.Map<UsuarioResponseDto>(usuarioActualizado);

        var response = new ApiResponse<UsuarioResponseDto>
        {
            Success = true,
            Message = "Usuario actualizado exitosamente",
            Data = usuarioDto
        };

        _logger.LogInformation("✅ Usuario actualizado: ID={IdUsuario}", id);

        return Ok(response);
    }

    // ========================================
    // ENDPOINT 6: ELIMINAR USUARIO (SOFT DELETE)
    // ========================================

    /// <summary>
    /// Eliminar un usuario (cambiar estado a inactivo)
    /// </summary>
    /// <param name="id">ID del usuario a eliminar</param>
    /// <returns>Confirmación de eliminación</returns>
    /// <response code="200">Usuario eliminado exitosamente</response>
    /// <response code="404">Usuario no encontrado</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<object>>> EliminarUsuario(int id)
    {
        _logger.LogInformation("🗑️ Eliminando usuario: ID={IdUsuario}", id);

        var resultado = await _usuarioService.EliminarUsuarioAsync(id);

        if (!resultado)
        {
            _logger.LogWarning("⚠️ Usuario no encontrado para eliminar: ID={IdUsuario}", id);
            return NotFound(new ApiErrorResponse
            {
                Success = false,
                Message = $"Usuario con ID {id} no encontrado",
                StatusCode = 404
            });
        }

        var response = new ApiResponse<object>
        {
            Success = true,
            Message = "Usuario eliminado exitosamente (estado cambiado a inactivo)",
            Data = new { IdUsuario = id }
        };

        _logger.LogInformation("✅ Usuario eliminado: ID={IdUsuario}", id);

        return Ok(response);
    }

    // ========================================
    // ENDPOINT 7: BLOQUEAR USUARIO
    // ========================================

    /// <summary>
    /// Bloquear un usuario (por seguridad)
    /// </summary>
    /// <param name="id">ID del usuario a bloquear</param>
    /// <returns>Confirmación de bloqueo</returns>
    /// <response code="200">Usuario bloqueado exitosamente</response>
    /// <response code="404">Usuario no encontrado</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpPost("{id}/bloquear")]
    [ProducesResponseType(typeof(ApiResponse<UsuarioResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<UsuarioResponseDto>>> BloquearUsuario(int id)
    {
        _logger.LogInformation("🔒 Bloqueando usuario: ID={IdUsuario}", id);

        var resultado = await _usuarioService.BloquearUsuarioAsync(id);

        if (!resultado)
        {
            _logger.LogWarning("⚠️ Usuario no encontrado para bloquear: ID={IdUsuario}", id);
            return NotFound(new ApiErrorResponse
            {
                Success = false,
                Message = $"Usuario con ID {id} no encontrado",
                StatusCode = 404
            });
        }

        // Obtener el usuario actualizado
        var usuario = await _usuarioService.GetUsuarioByIdAsync(id);
        var usuarioDto = _mapper.Map<UsuarioResponseDto>(usuario);

        var response = new ApiResponse<UsuarioResponseDto>
        {
            Success = true,
            Message = "Usuario bloqueado exitosamente",
            Data = usuarioDto
        };

        _logger.LogInformation("✅ Usuario bloqueado: ID={IdUsuario}", id);

        return Ok(response);
    }

    // ========================================
    // ENDPOINT 8: DESBLOQUEAR USUARIO
    // ========================================

    /// <summary>
    /// Desbloquear un usuario
    /// </summary>
    /// <param name="id">ID del usuario a desbloquear</param>
    /// <returns>Confirmación de desbloqueo</returns>
    /// <response code="200">Usuario desbloqueado exitosamente</response>
    /// <response code="404">Usuario no encontrado</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpPost("{id}/desbloquear")]
    [ProducesResponseType(typeof(ApiResponse<UsuarioResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<UsuarioResponseDto>>> DesbloquearUsuario(int id)
    {
        _logger.LogInformation("🔓 Desbloqueando usuario: ID={IdUsuario}", id);

        var resultado = await _usuarioService.DesbloquearUsuarioAsync(id);

        if (!resultado)
        {
            _logger.LogWarning("⚠️ Usuario no encontrado para desbloquear: ID={IdUsuario}", id);
            return NotFound(new ApiErrorResponse
            {
                Success = false,
                Message = $"Usuario con ID {id} no encontrado",
                StatusCode = 404
            });
        }

        // Obtener el usuario actualizado
        var usuario = await _usuarioService.GetUsuarioByIdAsync(id);
        var usuarioDto = _mapper.Map<UsuarioResponseDto>(usuario);

        var response = new ApiResponse<UsuarioResponseDto>
        {
            Success = true,
            Message = "Usuario desbloqueado exitosamente. Intentos fallidos reiniciados.",
            Data = usuarioDto
        };

        _logger.LogInformation("✅ Usuario desbloqueado: ID={IdUsuario}", id);

        return Ok(response);
    }

    // ========================================
    // ENDPOINT 9: ACTIVAR USUARIO
    // ========================================

    /// <summary>
    /// Activar un usuario (cambiar estado a activo)
    /// </summary>
    /// <param name="id">ID del usuario a activar</param>
    /// <returns>Confirmación de activación</returns>
    /// <response code="200">Usuario activado exitosamente</response>
    /// <response code="404">Usuario no encontrado</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpPost("{id}/activar")]
    [ProducesResponseType(typeof(ApiResponse<UsuarioResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<UsuarioResponseDto>>> ActivarUsuario(int id)
    {
        _logger.LogInformation("✅ Activando usuario: ID={IdUsuario}", id);

        var resultado = await _usuarioService.CambiarEstadoUsuarioAsync(id, true);

        if (!resultado)
        {
            _logger.LogWarning("⚠️ Usuario no encontrado para activar: ID={IdUsuario}", id);
            return NotFound(new ApiErrorResponse
            {
                Success = false,
                Message = $"Usuario con ID {id} no encontrado",
                StatusCode = 404
            });
        }

        // Obtener el usuario actualizado
        var usuario = await _usuarioService.GetUsuarioByIdAsync(id);
        var usuarioDto = _mapper.Map<UsuarioResponseDto>(usuario);

        var response = new ApiResponse<UsuarioResponseDto>
        {
            Success = true,
            Message = "Usuario activado exitosamente",
            Data = usuarioDto
        };

        _logger.LogInformation("✅ Usuario activado: ID={IdUsuario}", id);

        return Ok(response);
    }

    // ========================================
    // ENDPOINT 10: DESACTIVAR USUARIO
    // ========================================

    /// <summary>
    /// Desactivar un usuario (cambiar estado a inactivo)
    /// </summary>
    /// <param name="id">ID del usuario a desactivar</param>
    /// <returns>Confirmación de desactivación</returns>
    /// <response code="200">Usuario desactivado exitosamente</response>
    /// <response code="404">Usuario no encontrado</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpPost("{id}/desactivar")]
    [ProducesResponseType(typeof(ApiResponse<UsuarioResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<UsuarioResponseDto>>> DesactivarUsuario(int id)
    {
        _logger.LogInformation("⏸️ Desactivando usuario: ID={IdUsuario}", id);

        var resultado = await _usuarioService.CambiarEstadoUsuarioAsync(id, false);

        if (!resultado)
        {
            _logger.LogWarning("⚠️ Usuario no encontrado para desactivar: ID={IdUsuario}", id);
            return NotFound(new ApiErrorResponse
            {
                Success = false,
                Message = $"Usuario con ID {id} no encontrado",
                StatusCode = 404
            });
        }

        // Obtener el usuario actualizado
        var usuario = await _usuarioService.GetUsuarioByIdAsync(id);
        var usuarioDto = _mapper.Map<UsuarioResponseDto>(usuario);

        var response = new ApiResponse<UsuarioResponseDto>
        {
            Success = true,
            Message = "Usuario desactivado exitosamente",
            Data = usuarioDto
        };

        _logger.LogInformation("✅ Usuario desactivado: ID={IdUsuario}", id);

        return Ok(response);
    }

    // ========================================
    // ENDPOINT 11: ASIGNAR ROLES A USUARIO
    // ========================================

    /// <summary>
    /// Asignar múltiples roles a un usuario (reemplaza los existentes)
    /// </summary>
    /// <param name="id">ID del usuario</param>
    /// <param name="dto">IDs de los roles a asignar</param>
    /// <returns>Usuario con roles actualizados</returns>
    /// <response code="200">Roles asignados exitosamente</response>
    /// <response code="400">Datos inválidos</response>
    /// <response code="404">Usuario no encontrado</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpPost("{id}/asignar-roles")]
    [ProducesResponseType(typeof(ApiResponse<UsuarioConRolesDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<UsuarioConRolesDto>>> AsignarRoles(
        int id,
        [FromBody] AsignarRolesMultiplesDto dto)
    {
        _logger.LogInformation("🔗 Asignando {Cantidad} rol(es) al usuario: ID={IdUsuario}",
            dto.RolesIds.Count, id);

        // Verificar que el usuario existe
        var usuarioExiste = await _usuarioService.GetUsuarioByIdAsync(id);
        if (usuarioExiste == null)
        {
            _logger.LogWarning("⚠️ Usuario no encontrado para asignar roles: ID={IdUsuario}", id);
            return NotFound(new ApiErrorResponse
            {
                Success = false,
                Message = $"Usuario con ID {id} no encontrado",
                StatusCode = 404
            });
        }

        // Asignar roles
        await _usuarioService.AsignarRolesAsync(id, dto.RolesIds);

        // Obtener el usuario actualizado con roles
        var usuario = await _usuarioService.GetUsuarioByIdConRolesAsync(id);
        var usuarioDto = _mapper.Map<UsuarioConRolesDto>(usuario);

        var response = new ApiResponse<UsuarioConRolesDto>
        {
            Success = true,
            Message = $"Se asignaron {dto.RolesIds.Count} rol(es) exitosamente",
            Data = usuarioDto
        };

        _logger.LogInformation("✅ Roles asignados al usuario: ID={IdUsuario}", id);

        return Ok(response);
    }

    // ========================================
    // ENDPOINT 12: REMOVER ROL DE USUARIO
    // ========================================

    /// <summary>
    /// Remover un rol específico de un usuario
    /// </summary>
    /// <param name="id">ID del usuario</param>
    /// <param name="idRol">ID del rol a remover</param>
    /// <returns>Confirmación de remoción</returns>
    /// <response code="200">Rol removido exitosamente</response>
    /// <response code="404">Usuario o rol no encontrado</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpDelete("{id}/remover-rol/{idRol}")]
    [ProducesResponseType(typeof(ApiResponse<UsuarioConRolesDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<UsuarioConRolesDto>>> RemoverRol(
        int id,
        int idRol)
    {
        _logger.LogInformation("🔓 Removiendo rol ID={IdRol} del usuario ID={IdUsuario}",
            idRol, id);

        var resultado = await _usuarioService.RemoverRolAsync(id, idRol);

        if (!resultado)
        {
            _logger.LogWarning("⚠️ Usuario o rol no encontrado: Usuario ID={IdUsuario}, Rol ID={IdRol}",
                id, idRol);
            return NotFound(new ApiErrorResponse
            {
                Success = false,
                Message = $"Usuario con ID {id} o Rol con ID {idRol} no encontrado",
                StatusCode = 404
            });
        }

        // Obtener el usuario actualizado con roles
        var usuario = await _usuarioService.GetUsuarioByIdConRolesAsync(id);
        var usuarioDto = _mapper.Map<UsuarioConRolesDto>(usuario);

        var response = new ApiResponse<UsuarioConRolesDto>
        {
            Success = true,
            Message = "Rol removido exitosamente",
            Data = usuarioDto
        };

        _logger.LogInformation("✅ Rol removido del usuario: Usuario ID={IdUsuario}, Rol ID={IdRol}",
            id, idRol);

        return Ok(response);
    }
}