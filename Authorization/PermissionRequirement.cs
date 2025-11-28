using Microsoft.AspNetCore.Authorization;

namespace G2rismBeta.API.Authorization;

/// <summary>
/// Requisito de autorización que verifica si el usuario tiene un permiso específico.
/// </summary>
/// <remarks>
/// Este requisito se utiliza con el <see cref="PermissionAuthorizationHandler"/>
/// para implementar autorización basada en permisos almacenados en la base de datos.
///
/// Ejemplo de uso en un endpoint:
/// <code>
/// [Authorize(Policy = "RequirePermission:roles.eliminar")]
/// [HttpDelete("{id}")]
/// public async Task&lt;ActionResult&gt; DeleteRole(int id) { ... }
/// </code>
/// </remarks>
public class PermissionRequirement : IAuthorizationRequirement
{
    /// <summary>
    /// Nombre del permiso requerido (formato: "modulo.accion")
    /// </summary>
    /// <example>
    /// "roles.crear", "roles.leer", "roles.actualizar", "roles.eliminar"
    /// </example>
    public string PermissionName { get; }

    /// <summary>
    /// Constructor del requisito de permiso
    /// </summary>
    /// <param name="permissionName">Nombre del permiso en formato "modulo.accion"</param>
    /// <exception cref="ArgumentException">Si el nombre del permiso es nulo o vacío</exception>
    public PermissionRequirement(string permissionName)
    {
        if (string.IsNullOrWhiteSpace(permissionName))
        {
            throw new ArgumentException("El nombre del permiso no puede ser nulo o vacío", nameof(permissionName));
        }

        PermissionName = permissionName;
    }
}
