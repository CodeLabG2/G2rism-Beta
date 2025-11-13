using G2rismBeta.API.DTOs.Rol;

namespace G2rismBeta.API.DTOs.Usuario;

/// <summary>
/// DTO de usuario con sus roles asignados
/// Extiende UsuarioResponseDto agregando la lista de roles
/// </summary>
public class UsuarioConRolesDto : UsuarioResponseDto
{
    /// <summary>
    /// Lista de roles asignados al usuario
    /// </summary>
    public List<RolResponseDto> Roles { get; set; } = new List<RolResponseDto>();

    /// <summary>
    /// Cantidad de roles asignados
    /// </summary>
    /// <example>2</example>
    public int CantidadRoles => Roles?.Count ?? 0;
}