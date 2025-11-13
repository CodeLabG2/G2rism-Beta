namespace G2rismBeta.API.DTOs.Permiso;

/// <summary>
/// DTO de respuesta para un permiso
/// </summary>
public class PermisoResponseDto
{
    /// <summary>
    /// ID del permiso
    /// </summary>
    /// <example>1</example>
    public int IdPermiso { get; set; }

    /// <summary>
    /// Módulo del permiso
    /// </summary>
    /// <example>Usuarios</example>
    public string Modulo { get; set; } = string.Empty;

    /// <summary>
    /// Acción del permiso
    /// </summary>
    /// <example>Crear</example>
    public string Accion { get; set; } = string.Empty;

    /// <summary>
    /// Nombre completo del permiso
    /// </summary>
    /// <example>usuarios.crear</example>
    public string NombrePermiso { get; set; } = string.Empty;

    /// <summary>
    /// Descripción del permiso
    /// </summary>
    /// <example>Permite crear nuevos usuarios en el sistema</example>
    public string? Descripcion { get; set; }

    /// <summary>
    /// Estado del permiso
    /// </summary>
    /// <example>true</example>
    public bool Estado { get; set; }

    /// <summary>
    /// Cantidad de roles que tienen este permiso
    /// </summary>
    /// <example>3</example>
    public int CantidadRoles { get; set; }
}
