using FluentValidation;
using G2rismBeta.API.DTOs.Auth;
using G2rismBeta.API.Configuration;
using Microsoft.Extensions.Options;

namespace G2rismBeta.API.Validators;

/// <summary>
/// Validador para RecuperarPasswordRequestDto
/// Incluye validación de whitelist para prevenir ataques de phishing
/// </summary>
public class RecuperarPasswordDtoValidator : AbstractValidator<RecuperarPasswordRequestDto>
{
    private readonly SecuritySettings _securitySettings;

    public RecuperarPasswordDtoValidator(IOptions<SecuritySettings> securitySettings)
    {
        _securitySettings = securitySettings.Value;

        // ========================================
        // VALIDACIÓN DE EMAIL
        // ========================================

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es obligatorio")
            .EmailAddress().WithMessage("El formato del email no es válido")
            .MaximumLength(100).WithMessage("El email no puede exceder 100 caracteres")
            .Must(email => !string.IsNullOrWhiteSpace(email) && email.Trim() == email)
                .WithMessage("El email no debe contener espacios al inicio o al final");

        // ========================================
        // VALIDACIÓN DE FRONTEND URL (CON WHITELIST)
        // ========================================

        RuleFor(x => x.FrontendUrl)
            .NotEmpty().WithMessage("La URL del frontend es obligatoria")
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
                .WithMessage("La URL del frontend debe ser una URL válida (http o https)")
            .Must(url => !url.EndsWith("/"))
                .WithMessage("La URL del frontend no debe terminar con '/'")
            .Must(url =>
            {
                // ✅ SEGURIDAD: Validación contra whitelist para prevenir phishing
                if (_securitySettings.AllowedFrontendUrls == null || !_securitySettings.AllowedFrontendUrls.Any())
                {
                    // Si no hay whitelist configurada, permitir solo localhost en desarrollo
                    return url.Contains("localhost");
                }

                // Normalizar URL eliminando barra final si existe (para comparación)
                var normalizedUrl = url.TrimEnd('/');

                return _securitySettings.AllowedFrontendUrls.Any(allowedUrl =>
                    normalizedUrl.Equals(allowedUrl.TrimEnd('/'), StringComparison.OrdinalIgnoreCase));
            })
            .WithMessage("La URL del frontend no está autorizada. Solo se permiten URLs configuradas en la whitelist de seguridad");
    }
}