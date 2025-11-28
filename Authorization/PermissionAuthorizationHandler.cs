using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace G2rismBeta.API.Authorization;

/// <summary>
/// Handler de autorización que verifica si el usuario tiene el permiso requerido.
/// </summary>
/// <remarks>
/// Este handler verifica los claims del tipo "permission" en el JWT del usuario.
/// Los permisos se agregan al JWT durante el login en AuthService.GenerarTokensAsync().
///
/// Flujo de autorización:
/// 1. Usuario se autentica → JWT generado con claims de permisos
/// 2. Usuario hace request a endpoint protegido con [Authorize(Policy = "RequirePermission:xxx")]
/// 3. Este handler extrae los claims de permisos del JWT
/// 4. Verifica si el permiso requerido está presente
/// 5. Autoriza o deniega el acceso
/// </remarks>
public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly ILogger<PermissionAuthorizationHandler> _logger;

    public PermissionAuthorizationHandler(ILogger<PermissionAuthorizationHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Maneja la verificación del requisito de permiso
    /// </summary>
    /// <param name="context">Contexto de autorización con información del usuario</param>
    /// <param name="requirement">Requisito que especifica el permiso necesario</param>
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        // Obtener el usuario autenticado
        var user = context.User;

        if (user?.Identity == null || !user.Identity.IsAuthenticated)
        {
            _logger.LogWarning("🔒 Usuario no autenticado intentando acceder a recurso protegido");
            return Task.CompletedTask; // No autorizado
        }

        // Obtener el nombre de usuario para logging
        var username = user.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown";
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Unknown";

        // Obtener todos los permisos del usuario (claims de tipo "permission")
        var userPermissions = user.FindAll("permission")
                                  .Select(c => c.Value)
                                  .ToList();

        // Log de diagnóstico
        _logger.LogInformation(
            "🔐 Verificando permiso '{PermissionRequired}' para usuario '{Username}' (ID: {UserId})",
            requirement.PermissionName,
            username,
            userId
        );

        // Verificar si el usuario tiene el permiso requerido
        if (userPermissions.Contains(requirement.PermissionName, StringComparer.OrdinalIgnoreCase))
        {
            _logger.LogInformation(
                "✅ Permiso '{PermissionRequired}' CONCEDIDO para usuario '{Username}'",
                requirement.PermissionName,
                username
            );

            context.Succeed(requirement);
        }
        else
        {
            _logger.LogWarning(
                "❌ Permiso '{PermissionRequired}' DENEGADO para usuario '{Username}'. " +
                "Permisos disponibles: [{Permissions}]",
                requirement.PermissionName,
                username,
                string.Join(", ", userPermissions)
            );

            // No llamar context.Fail() - dejar que otros handlers puedan intentar
            // Si ningún handler hace Succeed(), la autorización fallará automáticamente
        }

        return Task.CompletedTask;
    }
}
