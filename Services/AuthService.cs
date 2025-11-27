using G2rismBeta.API.Interfaces;
using G2rismBeta.API.Models;
using G2rismBeta.API.Helpers;

namespace G2rismBeta.API.Services;

/// <summary>
/// Implementación del servicio de Autenticación
/// Incluye Register, Login, Logout, Recuperación de contraseña y JWT
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IUsuarioRolRepository _usuarioRolRepository;
    private readonly ITokenRecuperacionRepository _tokenRepository;
    private readonly IRolRepository _rolRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly JwtTokenGenerator _jwtTokenGenerator;
    private readonly IConfiguration _configuration;

    // Configuración de seguridad
    private const int MAX_INTENTOS_FALLIDOS = 5;
    private const int HORAS_EXPIRACION_TOKEN = 1;

    public AuthService(
        IUsuarioRepository usuarioRepository,
        IUsuarioRolRepository usuarioRolRepository,
        ITokenRecuperacionRepository tokenRepository,
        IRolRepository rolRepository,
        IRefreshTokenRepository refreshTokenRepository,
        JwtTokenGenerator jwtTokenGenerator,
        IConfiguration configuration)
    {
        _usuarioRepository = usuarioRepository;
        _usuarioRolRepository = usuarioRolRepository;
        _tokenRepository = tokenRepository;
        _rolRepository = rolRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _configuration = configuration;
    }

    // ========================================
    // REGISTRO
    // ========================================

    /// <summary>
    /// Registrar un nuevo usuario
    /// </summary>
    public async Task<Usuario> RegisterAsync(
        string username,
        string email,
        string password,
        string tipoUsuario = "cliente",
        string? nombre = null,
        string? apellido = null)
    {
        // 1. Validar que el username no exista
        if (await _usuarioRepository.ExistsByUsernameAsync(username))
        {
            throw new InvalidOperationException($"El username '{username}' ya está en uso");
        }

        // 2. Validar que el email no exista
        if (await _usuarioRepository.ExistsByEmailAsync(email))
        {
            throw new InvalidOperationException($"El email '{email}' ya está registrado");
        }

        // 3. Validar fortaleza de la contraseña
        var (esValidaPassword, errorPassword) = PasswordHasher.ValidatePasswordStrength(password);
        if (!esValidaPassword)
        {
            throw new ArgumentException(errorPassword ?? "La contraseña no cumple los requisitos de seguridad");
        }

        // 4. Crear el nuevo usuario
        var nuevoUsuario = new Usuario
        {
            Username = username.Trim(),
            Email = email.Trim().ToLower(),
            PasswordHash = PasswordHasher.HashPassword(password),
            TipoUsuario = tipoUsuario.ToLower(),
            Estado = true,
            Bloqueado = false,
            IntentosFallidos = 0,
            FechaCreacion = DateTime.Now
        };

        // 5. Guardar el usuario en la base de datos
        await _usuarioRepository.AddAsync(nuevoUsuario);
        await _usuarioRepository.SaveChangesAsync();

        // 6. Asignar el rol por defecto según el tipo de usuario
        string nombreRol = tipoUsuario.ToLower() == "cliente" ? "Cliente" : "Empleado";
        var rol = await _rolRepository.GetByNombreAsync(nombreRol);

        if (rol != null)
        {
            var usuarioRol = new UsuarioRol
            {
                IdUsuario = nuevoUsuario.IdUsuario,
                IdRol = rol.IdRol,
                FechaAsignacion = DateTime.Now
            };

            await _usuarioRolRepository.AsignarRolAsync(usuarioRol);
            await _usuarioRolRepository.SaveChangesAsync();
        }

        // 7. Enviar email de bienvenida (opcional)
        await EmailHelper.EnviarEmailBienvenida(nuevoUsuario.Email, nuevoUsuario.Username);

        // 8. Retornar el usuario con sus roles
        return await _usuarioRepository.GetByIdWithRolesAsync(nuevoUsuario.IdUsuario)
            ?? throw new InvalidOperationException("Error al recuperar el usuario creado");
    }

    // ========================================
    // AUTENTICACIÓN
    // ========================================

    /// <summary>
    /// Iniciar sesión con username o email
    /// </summary>
    public async Task<Usuario?> LoginAsync(string usernameOrEmail, string password)
    {
        // 1. Buscar el usuario por username o email
        var usuario = await _usuarioRepository.GetByUsernameOrEmailAsync(usernameOrEmail);

        if (usuario == null)
        {
            // Usuario no encontrado
            return null;
        }

        // 2. Verificar si la cuenta está bloqueada
        if (usuario.Bloqueado)
        {
            throw new InvalidOperationException("La cuenta está bloqueada. Contacte al administrador.");
        }

        // 3. Verificar si la cuenta está inactiva
        if (!usuario.Estado)
        {
            throw new InvalidOperationException("La cuenta está inactiva.");
        }

        // 4. Verificar la contraseña
        bool passwordValida = PasswordHasher.VerifyPassword(password, usuario.PasswordHash);

        if (!passwordValida)
        {
            // Contraseña incorrecta: incrementar intentos fallidos
            await _usuarioRepository.IncrementarIntentosFallidosAsync(usuario.IdUsuario);

            // Recargar el usuario para obtener los intentos actualizados
            usuario = await _usuarioRepository.GetByIdAsync(usuario.IdUsuario);

            // Si excede el máximo, bloquear la cuenta
            if (usuario != null && usuario.IntentosFallidos >= MAX_INTENTOS_FALLIDOS)
            {
                await _usuarioRepository.BloquearUsuarioAsync(usuario.IdUsuario);
                throw new InvalidOperationException(
                    $"Cuenta bloqueada por exceder {MAX_INTENTOS_FALLIDOS} intentos fallidos. " +
                    "Contacte al administrador."
                );
            }

            return null;
        }

        // 5. Login exitoso
        await _usuarioRepository.ReiniciarIntentosFallidosAsync(usuario.IdUsuario);
        await _usuarioRepository.UpdateUltimoAccesoAsync(usuario.IdUsuario);

        // 6. Obtener el usuario con sus roles
        return await _usuarioRepository.GetByIdWithRolesAsync(usuario.IdUsuario);
    }

    /// <summary>
    /// Generar tokens JWT (access token y refresh token) para un usuario
    /// </summary>
    /// <param name="usuario">Usuario autenticado</param>
    /// <param name="ipAddress">Dirección IP del cliente</param>
    /// <param name="userAgent">User Agent del navegador</param>
    /// <returns>Tupla con access token, refresh token y expiración</returns>
    public async Task<(string AccessToken, string RefreshToken, DateTime Expiration)> GenerarTokensAsync(
        Usuario usuario,
        string? ipAddress = null,
        string? userAgent = null)
    {
        // 1. Obtener roles y permisos del usuario
        var usuarioConRoles = await _usuarioRepository.GetByIdWithRolesAsync(usuario.IdUsuario);
        if (usuarioConRoles == null)
        {
            throw new InvalidOperationException("No se pudo obtener la información del usuario");
        }

        var roles = usuarioConRoles.UsuariosRoles
            .Select(ur => ur.Rol?.Nombre ?? "")
            .Where(r => !string.IsNullOrEmpty(r))
            .ToList();

        var permisos = usuarioConRoles.UsuariosRoles
            .SelectMany(ur => ur.Rol?.RolesPermisos ?? new List<RolPermiso>())
            .Select(rp => rp.Permiso?.NombrePermiso ?? "")
            .Where(p => !string.IsNullOrEmpty(p))
            .Distinct()
            .ToList();

        // 2. Generar Access Token (JWT)
        var accessToken = _jwtTokenGenerator.GenerateAccessToken(
            usuario.IdUsuario,
            usuario.Username,
            usuario.Email,
            usuario.TipoUsuario,
            roles,
            permisos
        );

        // 3. Generar Refresh Token
        var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

        // 4. Calcular fecha de expiración
        var expirationDays = int.Parse(_configuration["Jwt:RefreshTokenExpirationDays"] ?? "7");
        var expiration = DateTime.UtcNow.AddDays(expirationDays);

        // 5. Guardar Refresh Token en la base de datos
        var refreshTokenEntity = new RefreshToken
        {
            IdUsuario = usuario.IdUsuario,
            Token = refreshToken,
            FechaCreacion = DateTime.UtcNow,
            FechaExpiracion = expiration,
            Revocado = false,
            IpCreacion = ipAddress,
            UserAgent = userAgent
        };

        await _refreshTokenRepository.AddAsync(refreshTokenEntity);
        await _refreshTokenRepository.SaveChangesAsync();

        // 6. Calcular expiración del access token
        var accessTokenExpiration = DateTime.UtcNow.AddMinutes(
            int.Parse(_configuration["Jwt:AccessTokenExpirationMinutes"] ?? "60")
        );

        return (accessToken, refreshToken, accessTokenExpiration);
    }

    /// <summary>
    /// Renovar access token usando refresh token
    /// </summary>
    /// <param name="refreshToken">Refresh token válido</param>
    /// <param name="ipAddress">IP del cliente</param>
    /// <param name="userAgent">User Agent del navegador</param>
    /// <returns>Tupla con nuevo access token, nuevo refresh token y expiración</returns>
    public async Task<(string AccessToken, string RefreshToken, DateTime Expiration)> RefreshTokenAsync(
        string refreshToken,
        string? ipAddress = null,
        string? userAgent = null)
    {
        // 1. Validar el refresh token
        var tokenEntity = await _refreshTokenRepository.GetActiveTokenAsync(refreshToken);

        if (tokenEntity == null)
        {
            throw new UnauthorizedAccessException("Refresh token inválido o expirado");
        }

        // 2. Obtener el usuario
        var usuario = await _usuarioRepository.GetByIdAsync(tokenEntity.IdUsuario);

        if (usuario == null)
        {
            throw new KeyNotFoundException("Usuario no encontrado");
        }

        // Verificar que el usuario esté activo
        if (!usuario.Estado || usuario.Bloqueado)
        {
            throw new UnauthorizedAccessException("Usuario inactivo o bloqueado");
        }

        // 3. Revocar el refresh token anterior (rotación de tokens)
        tokenEntity.Revocado = true;
        tokenEntity.FechaRevocacion = DateTime.UtcNow;
        await _refreshTokenRepository.UpdateAsync(tokenEntity);
        await _refreshTokenRepository.SaveChangesAsync();

        // 4. Generar nuevos tokens
        var (newAccessToken, newRefreshToken, expiration) = await GenerarTokensAsync(
            usuario,
            ipAddress,
            userAgent
        );

        // 5. Registrar el token que reemplazó al anterior
        tokenEntity.ReemplazadoPor = newRefreshToken;
        await _refreshTokenRepository.UpdateAsync(tokenEntity);
        await _refreshTokenRepository.SaveChangesAsync();

        return (newAccessToken, newRefreshToken, expiration);
    }

    /// <summary>
    /// Cerrar sesión y revocar refresh tokens
    /// </summary>
    public async Task LogoutAsync(int idUsuario, string? refreshToken = null)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(idUsuario);

        if (usuario != null)
        {
            // Si se proporciona un refresh token específico, revocarlo
            if (!string.IsNullOrEmpty(refreshToken))
            {
                await _refreshTokenRepository.RevokeTokenAsync(refreshToken);
            }
            else
            {
                // Si no, revocar todos los tokens del usuario
                await _refreshTokenRepository.RevokeAllUserTokensAsync(idUsuario);
            }

            Console.WriteLine($"🔓 Usuario {usuario.Username} cerró sesión");
        }

        await Task.CompletedTask;
    }

    // ========================================
    // RECUPERACIÓN DE CONTRASEÑA
    // ========================================

    /// <summary>
    /// Solicitar recuperación de contraseña
    /// </summary>
    /// <param name="email">Email del usuario</param>
    /// <param name="frontendUrl">URL del frontend para construir el link de recuperación</param>
    /// <param name="ipSolicitud">IP desde donde se hace la solicitud (opcional)</param>
    public async Task<string> SolicitarRecuperacionPasswordAsync(string email, string frontendUrl, string? ipSolicitud = null)
    {
        // 1. Buscar el usuario por email
        var usuario = await _usuarioRepository.GetByEmailAsync(email);

        if (usuario == null)
        {
            // Por seguridad, no revelar si el email existe o no
            // Devolver OK pero no hacer nada
            throw new KeyNotFoundException("Si el email existe, se enviará un correo de recuperación");
        }

        // 2. Invalidar tokens activos anteriores
        await _tokenRepository.InvalidarTokensActivosAsync(usuario.IdUsuario);

        // 3. Generar un nuevo token
        var token = TokenGenerator.GenerateToken();
        var tokenRecuperacion = new TokenRecuperacion
        {
            IdUsuario = usuario.IdUsuario,
            Token = token,
            TipoToken = "recuperacion_password",
            FechaGeneracion = DateTime.Now,
            FechaExpiracion = DateTime.Now.AddHours(HORAS_EXPIRACION_TOKEN),
            Usado = false,
            IpSolicitud = ipSolicitud
        };

        await _tokenRepository.CrearTokenAsync(tokenRecuperacion);

        // 4. Enviar email con el token y el link del frontend
        await EmailHelper.EnviarEmailRecuperacion(usuario.Email, token, frontendUrl);

        return token;
    }

    /// <summary>
    /// Validar un token de recuperación
    /// </summary>
    public async Task<bool> ValidarTokenRecuperacionAsync(string token)
    {
        return await _tokenRepository.ValidarTokenAsync(token);
    }

    /// <summary>
    /// Restablecer contraseña con token
    /// </summary>
    /// <param name="token">Token de recuperación</param>
    /// <param name="nuevaPassword">Nueva contraseña</param>
    /// <param name="ipAddress">IP desde donde se realiza el cambio (opcional, para auditoría)</param>
    public async Task<bool> RestablecerPasswordAsync(string token, string nuevaPassword, string? ipAddress = null)
    {
        // 1. Validar el token
        var esValido = await _tokenRepository.ValidarTokenAsync(token);
        if (!esValido)
        {
            throw new InvalidOperationException("El token es inválido o ha expirado");
        }

        // 2. Obtener el token con el usuario
        var tokenObj = await _tokenRepository.GetByTokenAsync(token);
        if (tokenObj == null)
        {
            throw new KeyNotFoundException("Token no encontrado");
        }

        // 3. Validar fortaleza de la nueva contraseña
        var (esValidaPassword, errorPassword) = PasswordHasher.ValidatePasswordStrength(nuevaPassword);
        if (!esValidaPassword)
        {
            throw new ArgumentException(errorPassword ?? "La contraseña no cumple los requisitos");
        }

        // 4. Obtener el usuario
        var usuario = await _usuarioRepository.GetByIdAsync(tokenObj.IdUsuario);
        if (usuario == null)
        {
            throw new KeyNotFoundException("Usuario no encontrado");
        }

        // 5. Actualizar la contraseña
        usuario.PasswordHash = PasswordHasher.HashPassword(nuevaPassword);
        usuario.FechaModificacion = DateTime.Now;

        // Desbloquear y resetear intentos
        usuario.Bloqueado = false;
        usuario.IntentosFallidos = 0;

        // 📊 AUDITORÍA: Registrar la IP para trazabilidad
        Console.WriteLine($"🔐 Contraseña restablecida via token | Usuario: {usuario.Username} (ID: {usuario.IdUsuario}) | IP: {ipAddress ?? "No registrada"}");

        await _usuarioRepository.UpdateAsync(usuario);
        await _usuarioRepository.SaveChangesAsync();

        // 6. Marcar el token como usado
        await _tokenRepository.MarcarComoUsadoAsync(token);

        // 7. Invalidar otros tokens activos
        await _tokenRepository.InvalidarTokensActivosAsync(usuario.IdUsuario);

        return true;
    }

    // ========================================
    // UTILIDADES
    // ========================================

    /// <summary>
    /// Validar credenciales sin registrar el login
    /// </summary>
    public async Task<bool> ValidarCredencialesAsync(string usernameOrEmail, string password)
    {
        var usuario = await _usuarioRepository.GetByUsernameOrEmailAsync(usernameOrEmail);

        if (usuario == null)
            return false;

        return PasswordHasher.VerifyPassword(password, usuario.PasswordHash);
    }
}