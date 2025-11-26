using FluentValidation;
using G2rismBeta.API.DTOs.Auth;
using G2rismBeta.API.Helpers;

namespace G2rismBeta.API.Validators;

/// <summary>
/// Validador para CambiarPasswordDto
/// Valida que la nueva contraseña cumpla con los requisitos de seguridad
/// </summary>
public class CambiarPasswordDtoValidator : AbstractValidator<CambiarPasswordDto>
{
    public CambiarPasswordDtoValidator()
    {
        // ========================================
        // VALIDACIÓN DE ID USUARIO
        // ========================================

        RuleFor(x => x.IdUsuario)
            .GreaterThan(0)
            .WithMessage("El ID de usuario debe ser mayor a 0");

        // ========================================
        // VALIDACIÓN DE CONTRASEÑA ACTUAL
        // ========================================

        RuleFor(x => x.CurrentPassword)
            .NotEmpty()
            .WithMessage("La contraseña actual es obligatoria")
            .MaximumLength(100)
            .WithMessage("La contraseña actual no puede exceder 100 caracteres");

        // ========================================
        // VALIDACIÓN DE NUEVA CONTRASEÑA
        // ========================================

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage("La nueva contraseña es obligatoria")
            .Length(8, 100)
            .WithMessage("La nueva contraseña debe tener entre 8 y 100 caracteres")
            .Must(password =>
            {
                var (isValid, _) = PasswordHasher.ValidatePasswordStrength(password);
                return isValid;
            })
            .WithMessage("La nueva contraseña debe contener al menos: una mayúscula, una minúscula, un número y un carácter especial");

        // ========================================
        // VALIDACIÓN DE CONFIRMACIÓN DE CONTRASEÑA
        // ========================================

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .WithMessage("La confirmación de contraseña es obligatoria")
            .Equal(x => x.NewPassword)
            .WithMessage("Las contraseñas no coinciden");

        // ========================================
        // VALIDACIÓN ADICIONAL: Nueva contraseña diferente a la actual
        // ========================================

        RuleFor(x => x.NewPassword)
            .NotEqual(x => x.CurrentPassword)
            .WithMessage("La nueva contraseña debe ser diferente a la contraseña actual")
            .When(x => !string.IsNullOrEmpty(x.CurrentPassword) && !string.IsNullOrEmpty(x.NewPassword));
    }
}
