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
    /// <returns>True si el email se envió exitosamente</returns>
    public static async Task<bool> EnviarEmailRecuperacion(string email, string token)
    {
        // TODO: Implementar con un servicio como SendGrid, Mailgun, etc.
        // Por ahora solo loguear en consola
        Console.WriteLine($"📧 Email de recuperación enviado a: {email}");
        Console.WriteLine($"🔑 Token: {token}");
        Console.WriteLine($"🔗 Link: https://tu-frontend.com/reset-password?token={token}");
        
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