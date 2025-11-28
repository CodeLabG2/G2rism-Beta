using SendGrid;
using SendGrid.Helpers.Mail;
using G2rismBeta.API.Interfaces;

namespace G2rismBeta.API.Services;

/// <summary>
/// Implementación de IEmailService usando SendGrid
/// SendGrid es un servicio profesional de envío de emails
/// Plan gratuito: 100 emails/día
/// </summary>
public class SendGridEmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<SendGridEmailService> _logger;
    private readonly string _apiKey;
    private readonly string _fromEmail;
    private readonly string _fromName;
    private readonly bool _isEnabled;

    public SendGridEmailService(
        IConfiguration configuration,
        ILogger<SendGridEmailService> _logger)
    {
        _configuration = configuration;
        this._logger = _logger;

        // Cargar configuración de SendGrid
        var sendGridSection = _configuration.GetSection("SendGrid");
        _apiKey = sendGridSection["ApiKey"] ?? string.Empty;
        _fromEmail = sendGridSection["FromEmail"] ?? "noreply@g2rism.com";
        _fromName = sendGridSection["FromName"] ?? "G2rism Beta";
        _isEnabled = !string.IsNullOrEmpty(_apiKey) && _apiKey != "YOUR_SENDGRID_API_KEY";

        if (!_isEnabled)
        {
            _logger.LogWarning("⚠️ SendGrid no está configurado correctamente. Los emails se simularán en consola.");
        }
    }

    /// <summary>
    /// Enviar email de recuperación de contraseña
    /// </summary>
    public async Task<bool> SendPasswordResetEmailAsync(string email, string username, string token, string? resetLink = null)
    {
        _logger.LogInformation("📧 Enviando email de recuperación de contraseña a: {Email}", email);

        var subject = "Recuperación de Contraseña - G2rism Beta";

        var htmlContent = $@"
<!DOCTYPE html>
<html lang='es'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Recuperación de Contraseña</title>
</head>
<body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px;'>
    <div style='background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); padding: 30px; text-align: center; border-radius: 10px 10px 0 0;'>
        <h1 style='color: white; margin: 0;'>G2rism Beta</h1>
        <p style='color: white; margin: 10px 0 0 0;'>Sistema de Turismo</p>
    </div>

    <div style='background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px;'>
        <h2 style='color: #667eea;'>Recuperación de Contraseña</h2>

        <p>Hola <strong>{username}</strong>,</p>

        <p>Recibimos una solicitud para restablecer la contraseña de tu cuenta en G2rism Beta.</p>

        <div style='background: white; padding: 20px; border-radius: 8px; border-left: 4px solid #667eea; margin: 20px 0;'>
            <p style='margin: 0 0 10px 0;'><strong>Tu token de recuperación es:</strong></p>
            <p style='font-size: 24px; font-weight: bold; color: #667eea; letter-spacing: 2px; margin: 0;'>{token}</p>
        </div>

        {(string.IsNullOrEmpty(resetLink) ? "" : $@"
        <p>O puedes hacer clic en el siguiente enlace:</p>
        <div style='text-align: center; margin: 30px 0;'>
            <a href='{resetLink}' style='background: #667eea; color: white; padding: 15px 30px; text-decoration: none; border-radius: 5px; display: inline-block; font-weight: bold;'>Restablecer Contraseña</a>
        </div>
        ")}

        <p><strong>Por seguridad:</strong></p>
        <ul>
            <li>Este token expira en <strong>1 hora</strong></li>
            <li>Solo se puede usar <strong>una vez</strong></li>
            <li>Si no solicitaste este cambio, ignora este email</li>
        </ul>

        <hr style='border: none; border-top: 1px solid #ddd; margin: 30px 0;'>

        <p style='font-size: 12px; color: #666;'>
            Este es un email automático, por favor no respondas a este mensaje.<br>
            Si tienes problemas, contacta a soporte técnico.
        </p>
    </div>
</body>
</html>";

        var plainTextContent = $@"
G2rism Beta - Recuperación de Contraseña

Hola {username},

Recibimos una solicitud para restablecer la contraseña de tu cuenta.

Tu token de recuperación es: {token}

{(string.IsNullOrEmpty(resetLink) ? "" : $"O puedes acceder a: {resetLink}")}

Por seguridad:
- Este token expira en 1 hora
- Solo se puede usar una vez
- Si no solicitaste este cambio, ignora este email

---
Este es un email automático, por favor no respondas a este mensaje.
";

        return await SendEmailAsync(email, subject, htmlContent, plainTextContent);
    }

    /// <summary>
    /// Enviar email de bienvenida
    /// </summary>
    public async Task<bool> SendWelcomeEmailAsync(string email, string username, string nombre)
    {
        _logger.LogInformation("📧 Enviando email de bienvenida a: {Email}", email);

        var subject = "¡Bienvenido a G2rism Beta!";

        var htmlContent = $@"
<!DOCTYPE html>
<html lang='es'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Bienvenido a G2rism Beta</title>
</head>
<body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px;'>
    <div style='background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); padding: 30px; text-align: center; border-radius: 10px 10px 0 0;'>
        <h1 style='color: white; margin: 0;'>¡Bienvenido a G2rism Beta!</h1>
        <p style='color: white; margin: 10px 0 0 0;'>Sistema de Turismo</p>
    </div>

    <div style='background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px;'>
        <h2 style='color: #667eea;'>¡Hola {nombre}!</h2>

        <p>Tu cuenta ha sido creada exitosamente en <strong>G2rism Beta</strong>.</p>

        <div style='background: white; padding: 20px; border-radius: 8px; border-left: 4px solid #667eea; margin: 20px 0;'>
            <p style='margin: 0;'><strong>Nombre de usuario:</strong> {username}</p>
            <p style='margin: 10px 0 0 0;'><strong>Email:</strong> {email}</p>
        </div>

        <h3 style='color: #667eea;'>¿Qué puedes hacer ahora?</h3>
        <ul>
            <li>Gestionar tus preferencias de viaje</li>
            <li>Explorar destinos turísticos</li>
            <li>Reservar servicios de turismo</li>
            <li>Acceder a ofertas exclusivas</li>
        </ul>

        <div style='text-align: center; margin: 30px 0;'>
            <a href='http://localhost:5000/' style='background: #667eea; color: white; padding: 15px 30px; text-decoration: none; border-radius: 5px; display: inline-block; font-weight: bold;'>Acceder a la Plataforma</a>
        </div>

        <hr style='border: none; border-top: 1px solid #ddd; margin: 30px 0;'>

        <p style='font-size: 12px; color: #666;'>
            Este es un email automático, por favor no respondas a este mensaje.<br>
            Si tienes problemas, contacta a soporte técnico.
        </p>

        <p style='font-size: 12px; color: #666; text-align: center; margin-top: 20px;'>
            &copy; 2025 G2rism Beta - CodeLabG2. Todos los derechos reservados.
        </p>
    </div>
</body>
</html>";

        var plainTextContent = $@"
¡Bienvenido a G2rism Beta!

Hola {nombre},

Tu cuenta ha sido creada exitosamente en G2rism Beta.

Nombre de usuario: {username}
Email: {email}

¿Qué puedes hacer ahora?
- Gestionar tus preferencias de viaje
- Explorar destinos turísticos
- Reservar servicios de turismo
- Acceder a ofertas exclusivas

Accede a la plataforma en: http://localhost:5000/

---
Este es un email automático, por favor no respondas a este mensaje.

© 2025 G2rism Beta - CodeLabG2. Todos los derechos reservados.
";

        return await SendEmailAsync(email, subject, htmlContent, plainTextContent);
    }

    /// <summary>
    /// Enviar email genérico usando SendGrid
    /// </summary>
    public async Task<bool> SendEmailAsync(string email, string subject, string htmlContent, string? plainTextContent = null)
    {
        // Si SendGrid no está configurado, simular envío en consola
        if (!_isEnabled)
        {
            _logger.LogWarning("⚠️ SendGrid no configurado. Simulando envío de email:");
            _logger.LogInformation("📧 De: {FromEmail} ({FromName})", _fromEmail, _fromName);
            _logger.LogInformation("📧 Para: {ToEmail}", email);
            _logger.LogInformation("📧 Asunto: {Subject}", subject);
            _logger.LogInformation("📧 Contenido (texto plano):");
            _logger.LogInformation("{Content}", plainTextContent ?? "Sin contenido en texto plano");
            return true;
        }

        try
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress(_fromEmail, _fromName);
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(
                from,
                to,
                subject,
                plainTextContent ?? htmlContent,
                htmlContent
            );

            var response = await client.SendEmailAsync(msg);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("✅ Email enviado exitosamente a: {Email}", email);
                return true;
            }
            else
            {
                var body = await response.Body.ReadAsStringAsync();
                _logger.LogError("❌ Error al enviar email a {Email}. Status: {StatusCode}, Body: {Body}",
                    email, response.StatusCode, body);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Excepción al enviar email a {Email}", email);
            return false;
        }
    }
}
