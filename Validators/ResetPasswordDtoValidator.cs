using FluentValidation;
using G2rismBeta.API.DTOs.Auth;
using G2rismBeta.API.Helpers;

namespace G2rismBeta.API.Validators;

/// <summary>
/// Validador para ResetPasswordDto
/// </summary>
public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
{
    public ResetPasswordDtoValidator()
    {
        // ========================================
        // VALIDACIÓN DE TOKEN
        // ========================================

        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("El token es obligatorio")
            .MaximumLength(255).WithMessage("El token no puede exceder 255 caracteres")
            .Must(token => !string.IsNullOrWhiteSpace(token) && token.Trim() == token)
                .WithMessage("El token no debe contener espacios")
            .Must(token => token.Length >= 20)
                .WithMessage("El token debe tener al menos 20 caracteres");

        // ========================================
        // VALIDACIÓN DE NEW PASSWORD
        // ========================================

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("La nueva contraseña es obligatoria")
            .Length(8, 100).WithMessage("La nueva contraseña debe tener entre 8 y 100 caracteres")
            .Must(password =>
            {
                var (isValid, _) = PasswordHasher.ValidatePasswordStrength(password);
                return isValid;
            })
            .WithMessage("La nueva contraseña debe contener al menos: una mayúscula, una minúscula, un número y un carácter especial");

        // ========================================
        // VALIDACIÓN DE CONFIRM PASSWORD
        // ========================================

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("La confirmación de contraseña es obligatoria")
            .Equal(x => x.NewPassword).WithMessage("Las contraseñas no coinciden");
    }
}