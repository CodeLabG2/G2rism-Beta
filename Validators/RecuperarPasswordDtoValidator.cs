using FluentValidation;
using G2rismBeta.API.DTOs.Auth;

namespace G2rismBeta.API.Validators;

/// <summary>
/// Validador para RecuperarPasswordRequestDto
/// </summary>
public class RecuperarPasswordDtoValidator : AbstractValidator<RecuperarPasswordRequestDto>
{
    public RecuperarPasswordDtoValidator()
    {
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
        // VALIDACIÓN DE FRONTEND URL
        // ========================================

        RuleFor(x => x.FrontendUrl)
            .NotEmpty().WithMessage("La URL del frontend es obligatoria")
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
                .WithMessage("La URL del frontend debe ser una URL válida (http o https)")
            .Must(url => !url.EndsWith("/"))
                .WithMessage("La URL del frontend no debe terminar con '/'");
    }
}