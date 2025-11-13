using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.DTOs.Auth;
using G2rismBeta.API.Models;

namespace G2rismBeta.API.Controllers;

/// <summary>
/// Controlador de Autenticación
/// Gestiona registro, login, logout y recuperación de contraseña
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IAuthService authService,
        IMapper mapper,
        ILogger<AuthController> logger)
    {
        _authService = authService;
        _mapper = mapper;
        _logger = logger;
    }

    // ========================================
    // ENDPOINT 1: REGISTER (REGISTRO)
    // ========================================

    /// <summary>
    /// Registrar un nuevo usuario en el sistema
    /// </summary>
    /// <param name="dto">Datos del nuevo usuario</param>
    /// <returns>Usuario registrado exitosamente</returns>
    /// <response code="201">Usuario registrado exitosamente</response>
    /// <response code="400">Datos inválidos o usuario ya existe</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<RegisterResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<RegisterResponseDto>>> Register(
        [FromBody] RegisterRequestDto dto)
    {
        _logger.LogInformation("📝 Iniciando registro de usuario: {Username}", dto.Username);

        try
        {
            // Registrar el usuario
            var usuario = await _authService.RegisterAsync(
                username: dto.Username,
                email: dto.Email,
                password: dto.Password,
                tipoUsuario: dto.TipoUsuario,
                nombre: dto.Nombre,
                apellido: dto.Apellido
            );

            // Mapear a DTO de respuesta
            var responseDto = new RegisterResponseDto
            {
                IdUsuario = usuario.IdUsuario,
                Username = usuario.Username,
                Email = usuario.Email,
                TipoUsuario = usuario.TipoUsuario,
                FechaRegistro = usuario.FechaCreacion,
                Roles = usuario.UsuariosRoles?.Select(ur => ur.Rol?.Nombre ?? "").ToList() ?? new List<string>(),
                Mensaje = "Usuario registrado exitosamente. Por favor inicia sesión."
            };

            var response = new ApiResponse<RegisterResponseDto>
            {
                Success = true,
                Message = "¡Bienvenido! Tu cuenta ha sido creada exitosamente",
                Data = responseDto
            };

            _logger.LogInformation("✅ Usuario registrado exitosamente: ID={IdUsuario}, Username={Username}",
                usuario.IdUsuario, usuario.Username);

            return CreatedAtAction(
                nameof(GetProfile),
                new { id = usuario.IdUsuario },
                response
            );
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("⚠️ Error en registro: {Message}", ex.Message);
            return BadRequest(new ApiErrorResponse
            {
                Success = false,
                Message = ex.Message,
                StatusCode = 400,
                ErrorCode = "REGISTRO_ERROR"
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("⚠️ Validación de contraseña fallida: {Message}", ex.Message);
            return BadRequest(new ApiErrorResponse
            {
                Success = false,
                Message = ex.Message,
                StatusCode = 400,
                ErrorCode = "PASSWORD_INVALIDA"
            });
        }
    }

    // ========================================
    // ENDPOINT 2: LOGIN (INICIAR SESIÓN)
    // ========================================

    /// <summary>
    /// Iniciar sesión en el sistema
    /// </summary>
    /// <param name="dto">Credenciales de acceso</param>
    /// <returns>Datos del usuario autenticado</returns>
    /// <response code="200">Login exitoso</response>
    /// <response code="401">Credenciales inválidas</response>
    /// <response code="403">Cuenta bloqueada o inactiva</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login(
        [FromBody] LoginRequestDto dto)
    {
        _logger.LogInformation("🔐 Intento de login: {UsernameOrEmail}", dto.UsernameOrEmail);

        try
        {
            var usuario = await _authService.LoginAsync(dto.UsernameOrEmail, dto.Password);

            if (usuario == null)
            {
                _logger.LogWarning("⚠️ Login fallido: credenciales incorrectas para {UsernameOrEmail}",
                    dto.UsernameOrEmail);

                return Unauthorized(new ApiErrorResponse
                {
                    Success = false,
                    Message = "Credenciales incorrectas. Verifica tu usuario/email y contraseña.",
                    StatusCode = 401,
                    ErrorCode = "CREDENCIALES_INVALIDAS"
                });
            }

            // Mapear a DTO de respuesta
            var responseDto = _mapper.Map<LoginResponseDto>(usuario);

            var response = new ApiResponse<LoginResponseDto>
            {
                Success = true,
                Message = $"¡Bienvenido de vuelta, {usuario.Username}!",
                Data = responseDto
            };

            _logger.LogInformation("✅ Login exitoso: Usuario={Username}, ID={IdUsuario}",
                usuario.Username, usuario.IdUsuario);

            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("⚠️ Login bloqueado: {Message}", ex.Message);
            return StatusCode(403, new ApiErrorResponse
            {
                Success = false,
                Message = ex.Message,
                StatusCode = 403,
                ErrorCode = "CUENTA_BLOQUEADA"
            });
        }
    }

    // ========================================
    // ENDPOINT 3: LOGOUT (CERRAR SESIÓN)
    // ========================================

    /// <summary>
    /// Cerrar sesión del sistema
    /// </summary>
    /// <param name="idUsuario">ID del usuario que cierra sesión</param>
    /// <returns>Confirmación de cierre de sesión</returns>
    /// <response code="200">Logout exitoso</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpPost("logout")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<object>>> Logout([FromBody] int idUsuario)
    {
        _logger.LogInformation("👋 Usuario cerrando sesión: ID={IdUsuario}", idUsuario);

        await _authService.LogoutAsync(idUsuario);

        var response = new ApiResponse<object>
        {
            Success = true,
            Message = "Sesión cerrada exitosamente",
            Data = new { IdUsuario = idUsuario, LogoutTime = DateTime.Now }
        };

        _logger.LogInformation("✅ Logout exitoso: ID={IdUsuario}", idUsuario);

        return Ok(response);
    }

    // ========================================
    // ENDPOINT 4: RECUPERAR PASSWORD
    // ========================================

    /// <summary>
    /// Solicitar recuperación de contraseña
    /// Se enviará un token al email registrado
    /// </summary>
    /// <param name="dto">Email del usuario</param>
    /// <returns>Confirmación de envío de token</returns>
    /// <response code="200">Token enviado al email</response>
    /// <response code="404">Email no encontrado</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpPost("recuperar-password")]
    [ProducesResponseType(typeof(ApiResponse<RecuperarPasswordResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<RecuperarPasswordResponseDto>>> RecuperarPassword(
        [FromBody] RecuperarPasswordRequestDto dto)
    {
        _logger.LogInformation("🔑 Solicitud de recuperación de contraseña: {Email}", dto.Email);

        try
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var token = await _authService.SolicitarRecuperacionPasswordAsync(dto.Email, ipAddress);

            var responseDto = new RecuperarPasswordResponseDto
            {
                Success = true,
                Message = "Si el email existe, recibirás un correo con instrucciones para recuperar tu contraseña",
                EmailEnviado = true,
                FechaExpiracion = DateTime.Now.AddHours(1),
                Token = token // ⚠️ En producción, NO enviar el token en la respuesta
            };

            var response = new ApiResponse<RecuperarPasswordResponseDto>
            {
                Success = true,
                Message = "Solicitud procesada exitosamente",
                Data = responseDto
            };

            _logger.LogInformation("✅ Token de recuperación generado para: {Email}", dto.Email);

            return Ok(response);
        }
        catch (KeyNotFoundException)
        {
            _logger.LogWarning("⚠️ Email no encontrado: {Email}", dto.Email);

            // Por seguridad, devolver la misma respuesta aunque el email no exista
            var responseDto = new RecuperarPasswordResponseDto
            {
                Success = true,
                Message = "Si el email existe, recibirás un correo con instrucciones para recuperar tu contraseña",
                EmailEnviado = false,
                FechaExpiracion = DateTime.Now.AddHours(1),
                Token = null
            };

            return Ok(new ApiResponse<RecuperarPasswordResponseDto>
            {
                Success = true,
                Message = "Solicitud procesada exitosamente",
                Data = responseDto
            });
        }
    }

    // ========================================
    // ENDPOINT 5: RESET PASSWORD (CON TOKEN)
    // ========================================

    /// <summary>
    /// Restablecer contraseña usando token de recuperación
    /// </summary>
    /// <param name="dto">Token y nueva contraseña</param>
    /// <returns>Confirmación de cambio de contraseña</returns>
    /// <response code="200">Contraseña cambiada exitosamente</response>
    /// <response code="400">Token inválido o contraseña débil</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpPost("reset-password")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<object>>> ResetPassword(
        [FromBody] ResetPasswordDto dto)
    {
        _logger.LogInformation("🔐 Restableciendo contraseña con token");

        try
        {
            var resultado = await _authService.RestablecerPasswordAsync(dto.Token, dto.NewPassword);

            if (!resultado)
            {
                return BadRequest(new ApiErrorResponse
                {
                    Success = false,
                    Message = "No se pudo restablecer la contraseña",
                    StatusCode = 400,
                    ErrorCode = "RESET_FALLIDO"
                });
            }

            var response = new ApiResponse<object>
            {
                Success = true,
                Message = "Contraseña restablecida exitosamente. Ya puedes iniciar sesión con tu nueva contraseña.",
                Data = new { PasswordChanged = true, Timestamp = DateTime.Now }
            };

            _logger.LogInformation("✅ Contraseña restablecida exitosamente");

            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("⚠️ Token inválido o expirado");
            return BadRequest(new ApiErrorResponse
            {
                Success = false,
                Message = ex.Message,
                StatusCode = 400,
                ErrorCode = "TOKEN_INVALIDO"
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("⚠️ Contraseña débil: {Message}", ex.Message);
            return BadRequest(new ApiErrorResponse
            {
                Success = false,
                Message = ex.Message,
                StatusCode = 400,
                ErrorCode = "PASSWORD_DEBIL"
            });
        }
    }

    // ========================================
    // ENDPOINT 6: CAMBIAR PASSWORD (AUTENTICADO)
    // ========================================

    /// <summary>
    /// Cambiar contraseña estando autenticado
    /// Requiere la contraseña actual para validación
    /// </summary>
    /// <param name="dto">Contraseña actual y nueva contraseña</param>
    /// <returns>Confirmación de cambio de contraseña</returns>
    /// <response code="200">Contraseña cambiada exitosamente</response>
    /// <response code="400">Contraseña actual incorrecta</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpPost("cambiar-password")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<object>>> CambiarPassword(
        [FromBody] CambiarPasswordDto dto)
    {
        _logger.LogInformation("🔐 Usuario cambiando contraseña: ID={IdUsuario}", dto.IdUsuario);

        try
        {
            // Validar la contraseña actual
            var usuario = await _authService.LoginAsync(dto.IdUsuario.ToString(), dto.CurrentPassword);

            if (usuario == null)
            {
                _logger.LogWarning("⚠️ Contraseña actual incorrecta");
                return BadRequest(new ApiErrorResponse
                {
                    Success = false,
                    Message = "La contraseña actual es incorrecta",
                    StatusCode = 400,
                    ErrorCode = "PASSWORD_ACTUAL_INCORRECTA"
                });
            }

            // Generar un token temporal para restablecer
            var token = await _authService.SolicitarRecuperacionPasswordAsync(usuario.Email);

            // Usar el token para cambiar la contraseña
            var resultado = await _authService.RestablecerPasswordAsync(token, dto.NewPassword);

            if (!resultado)
            {
                return BadRequest(new ApiErrorResponse
                {
                    Success = false,
                    Message = "No se pudo cambiar la contraseña",
                    StatusCode = 400,
                    ErrorCode = "CAMBIO_FALLIDO"
                });
            }

            var response = new ApiResponse<object>
            {
                Success = true,
                Message = "Contraseña cambiada exitosamente",
                Data = new { PasswordChanged = true, UserId = dto.IdUsuario, Timestamp = DateTime.Now }
            };

            _logger.LogInformation("✅ Contraseña cambiada exitosamente: Usuario ID={IdUsuario}", dto.IdUsuario);

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al cambiar contraseña");
            return BadRequest(new ApiErrorResponse
            {
                Success = false,
                Message = ex.Message,
                StatusCode = 400,
                ErrorCode = "ERROR_CAMBIO_PASSWORD"
            });
        }
    }

    // ========================================
    // ENDPOINT AUXILIAR: GET PROFILE
    // ========================================

    /// <summary>
    /// Obtener perfil del usuario (usado por CreatedAtAction en Register)
    /// </summary>
    [HttpGet("profile/{id}")]
    [ApiExplorerSettings(IgnoreApi = true)] // Ocultar de Swagger
    public async Task<ActionResult> GetProfile(int id)
    {
        // Este endpoint es solo para completar el CreatedAtAction
        // En una implementación real, aquí iría la lógica para obtener el perfil
        return Ok(new { Message = "Ver perfil en /api/usuarios/{id}" });
    }
}