namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interfaz para servicio de envío de emails
/// Abstracción para permitir diferentes implementaciones (SendGrid, SMTP, etc.)
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Enviar email de recuperación de contraseña
    /// </summary>
    /// <param name="email">Email destino</param>
    /// <param name="username">Nombre de usuario</param>
    /// <param name="token">Token de recuperación</param>
    /// <param name="resetLink">Link completo para reset (opcional)</param>
    /// <returns>True si el email se envió exitosamente</returns>
    Task<bool> SendPasswordResetEmailAsync(string email, string username, string token, string? resetLink = null);

    /// <summary>
    /// Enviar email de bienvenida al registrarse
    /// </summary>
    /// <param name="email">Email destino</param>
    /// <param name="username">Nombre de usuario</param>
    /// <param name="nombre">Nombre completo del usuario</param>
    /// <returns>True si el email se envió exitosamente</returns>
    Task<bool> SendWelcomeEmailAsync(string email, string username, string nombre);

    /// <summary>
    /// Enviar email genérico
    /// </summary>
    /// <param name="email">Email destino</param>
    /// <param name="subject">Asunto del email</param>
    /// <param name="htmlContent">Contenido HTML del email</param>
    /// <param name="plainTextContent">Contenido en texto plano (opcional)</param>
    /// <returns>True si el email se envió exitosamente</returns>
    Task<bool> SendEmailAsync(string email, string subject, string htmlContent, string? plainTextContent = null);
}
