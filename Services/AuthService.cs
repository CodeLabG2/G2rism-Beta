using G2rismBeta.API.Interfaces;
using G2rismBeta.API.Models;
using G2rismBeta.API.Helpers;

namespace G2rismBeta.API.Services;

/// <summary>
/// Implementación del servicio de Autenticación
/// Incluye Register, Login, Logout y Recuperación de contraseña
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IUsuarioRolRepository _usuarioRolRepository;
    private readonly ITokenRecuperacionRepository _tokenRepository;
    private readonly IRolRepository _rolRepository;

    // Configuración de seguridad
    private const int MAX_INTENTOS_FALLIDOS = 5;
    private const int HORAS_EXPIRACION_TOKEN = 1;

    public AuthService(
        IUsuarioRepository usuarioRepository,
        IUsuarioRolRepository usuarioRolRepository,
        ITokenRecuperacionRepository tokenRepository,
        IRolRepository rolRepository)
    {
        _usuarioRepository = usuarioRepository;
        _usuarioRolRepository = usuarioRolRepository;
        _tokenRepository = tokenRepository;
        _rolRepository = rolRepository;
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
    /// Cerrar sesión
    /// </summary>
    public async Task LogoutAsync(int idUsuario)
    {
        // Por ahora solo registramos el logout
        // Más adelante aquí se invalidará el JWT token
        var usuario = await _usuarioRepository.GetByIdAsync(idUsuario);
        if (usuario != null)
        {
            // Opcional: registrar en una tabla de auditoría
            Console.WriteLine($"Usuario {usuario.Username} cerró sesión");
        }

        await Task.CompletedTask;
    }

    // ========================================
    // RECUPERACIÓN DE CONTRASEÑA
    // ========================================

    /// <summary>
    /// Solicitar recuperación de contraseña
    /// </summary>
    public async Task<string> SolicitarRecuperacionPasswordAsync(string email, string? ipSolicitud = null)
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

        // 4. Enviar email con el token
        await EmailHelper.EnviarEmailRecuperacion(usuario.Email, token);

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
    public async Task<bool> RestablecerPasswordAsync(string token, string nuevaPassword)
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