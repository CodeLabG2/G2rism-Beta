namespace G2rismBeta.API.DTOs.Usuario;

/// <summary>
/// DTO con información del usuario para respuesta de login
/// Similar a UsuarioConRolesDto pero con menos campos para respuesta de autenticación
/// </summary>
public class UsuarioLoginDto
{
    /// <summary>
    /// ID del usuario
    /// </summary>
    /// <example>1</example>
    public int IdUsuario { get; set; }

    /// <summary>
    /// Nombre de usuario
    /// </summary>
    /// <example>admin</example>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Email del usuario
    /// </summary>
    /// <example>admin@g2rism.com</example>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de usuario
    /// </summary>
    /// <example>empleado</example>
    public string TipoUsuario { get; set; } = string.Empty;

    /// <summary>
    /// Lista de nombres de roles asignados
    /// </summary>
    /// <example>["Super Administrador", "Empleado"]</example>
    public List<string> Roles { get; set; } = new List<string>();

    /// <summary>
    /// Lista de permisos únicos del usuario (derivados de sus roles)
    /// </summary>
    /// <example>["crear_usuarios", "editar_usuarios", "ver_reportes"]</example>
    public List<string> Permisos { get; set; } = new List<string>();
}