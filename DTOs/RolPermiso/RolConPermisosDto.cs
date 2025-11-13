using G2rismBeta.API.DTOs.Permiso;

namespace G2rismBeta.API.DTOs.RolPermiso;

/// <summary>
/// DTO que devuelve un rol con todos sus permisos asignados
/// </summary>
public class RolConPermisosDto
{
    /// <summary>
    /// ID del rol
    /// </summary>
    /// <example>1</example>
    public int IdRol { get; set; }

    /// <summary>
    /// Nombre del rol
    /// </summary>
    /// <example>Administrador</example>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Descripción del rol
    /// </summary>
    /// <example>Usuario con acceso total</example>
    public string? Descripcion { get; set; }

    /// <summary>
    /// Lista de permisos asignados al rol
    /// </summary>
    public List<PermisoResponseDto> Permisos { get; set; } = new List<PermisoResponseDto>();

    /// <summary>
    /// Cantidad total de permisos
    /// </summary>
    /// <example>15</example>
    public int TotalPermisos => Permisos.Count;
}
