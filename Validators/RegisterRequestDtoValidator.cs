using FluentValidation;
using G2rismBeta.API.DTOs.Auth;

namespace G2rismBeta.API.Validators;

/// <summary>
/// Validador para el DTO de registro de usuarios
/// </summary>
public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterRequestDtoValidator()
    {
        // Username
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("El username es obligatorio")
            .Length(3, 50).WithMessage("El username debe tener entre 3 y 50 caracteres")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("El username solo puede contener letras, números y guiones bajos");

        // Email
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es obligatorio")
            .EmailAddress().WithMessage("El formato del email no es válido")
            .MaximumLength(100).WithMessage("El email no puede exceder 100 caracteres");

        // Password
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es obligatoria")
            .Length(8, 100).WithMessage("La contraseña debe tener entre 8 y 100 caracteres")
            .Matches(@"[A-Z]").WithMessage("La contraseña debe contener al menos una letra mayúscula")
            .Matches(@"[a-z]").WithMessage("La contraseña debe contener al menos una letra minúscula")
            .Matches(@"[0-9]").WithMessage("La contraseña debe contener al menos un número")
            .Matches(@"[!@#$%^&*()_+\-=\[\]{}|;:,.<>?]").WithMessage("La contraseña debe contener al menos un carácter especial");

        // ConfirmPassword
        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Debe confirmar la contraseña")
            .Equal(x => x.Password).WithMessage("Las contraseñas no coinciden");

        // Nombre (opcional)
        When(x => !string.IsNullOrEmpty(x.Nombre), () =>
        {
            RuleFor(x => x.Nombre)
                .MaximumLength(50).WithMessage("El nombre no puede exceder 50 caracteres");
        });

        // Apellido (opcional)
        When(x => !string.IsNullOrEmpty(x.Apellido), () =>
        {
            RuleFor(x => x.Apellido)
                .MaximumLength(50).WithMessage("El apellido no puede exceder 50 caracteres");
        });

        // TipoUsuario
        RuleFor(x => x.TipoUsuario)
            .NotEmpty().WithMessage("El tipo de usuario es obligatorio")
            .Must(tipo => tipo.ToLower() == "cliente" || tipo.ToLower() == "empleado")
            .WithMessage("El tipo de usuario debe ser 'cliente' o 'empleado'");

        // AceptaTerminos
        RuleFor(x => x.AceptaTerminos)
            .Equal(true).WithMessage("Debe aceptar los términos y condiciones");
    }
}