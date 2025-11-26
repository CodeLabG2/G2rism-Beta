namespace G2rismBeta.API.Helpers;

/// <summary>
/// Helper para envío de emails
/// TODO: Implementar cuando se configure el servicio de emails
/// </summary>
public static class EmailHelper
{
    /// <summary>
    /// Envía un email de recuperación de contraseña
    /// </summary>
    /// <param name="email">Email del destinatario</param>
    /// <param name="token">Token de recuperación</param>
    /// <param name="frontendUrl">URL del frontend para construir el link de recuperación</param>
    /// <returns>True si el email se envió exitosamente</returns>
    public static async Task<bool> EnviarEmailRecuperacion(string email, string token, string frontendUrl)
    {
        // TODO: Implementar con un servicio como SendGrid, Mailgun, etc.
        // Por ahora solo loguear en consola

        // ✅ SEGURIDAD: Construir link usando frontendUrl validado por whitelist
        var resetLink = $"{frontendUrl.TrimEnd('/')}/reset-password?token={token}";

        Console.WriteLine($"📧 Email de recuperación enviado a: {email}");
        Console.WriteLine($"🔑 Token: {token}");
        Console.WriteLine($"🔗 Link de recuperación: {resetLink}");

        await Task.CompletedTask;
        return true;
    }

    /// <summary>
    /// Envía un email de bienvenida al nuevo usuario
    /// </summary>
    public static async Task<bool> EnviarEmailBienvenida(string email, string username)
    {
        // TODO: Implementar
        Console.WriteLine($"👋 Email de bienvenida enviado a: {email} ({username})");
        await Task.CompletedTask;
        return true;
    }
}