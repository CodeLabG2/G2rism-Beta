using FluentValidation;
using G2rismBeta.API.DTOs.Auth;

namespace G2rismBeta.API.Validators;

/// <summary>
/// Validador para LoginRequestDto
/// </summary>
public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestDtoValidator()
    {
        // ========================================
        // VALIDACIÓN DE USERNAME OR EMAIL
        // ========================================

        RuleFor(x => x.UsernameOrEmail)
            .NotEmpty().WithMessage("El username o email es obligatorio")
            .MaximumLength(100).WithMessage("El username o email no puede exceder 100 caracteres")
            .Must(value => !string.IsNullOrWhiteSpace(value) && value.Trim().Length >= 3)
                .WithMessage("El username o email debe tener al menos 3 caracteres");

        // ========================================
        // VALIDACIÓN DE PASSWORD
        // ========================================

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es obligatoria")
            .MaximumLength(100).WithMessage("La contraseña no puede exceder 100 caracteres")
            .Must(password => !string.IsNullOrWhiteSpace(password))
                .WithMessage("La contraseña no puede estar vacía");

        // ========================================
        // VALIDACIÓN DE REMEMBER ME
        // ========================================

        // RememberMe es opcional, no necesita validación adicional
        // Solo verificamos que sea un booleano válido (lo cual ya está garantizado por el tipo)
    }
}