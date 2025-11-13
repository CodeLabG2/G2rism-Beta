using FluentValidation;
using G2rismBeta.API.DTOs.Rol;

namespace G2rismBeta.API.Validators;

/// <summary>
/// Validador para el DTO de actualización de roles
/// </summary>
public class RolUpdateDtoValidator : AbstractValidator<RolUpdateDto>
{
    public RolUpdateDtoValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre del rol es obligatorio")
            .Length(3, 50)
            .WithMessage("El nombre debe tener entre 3 y 50 caracteres")
            .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$")
            .WithMessage("El nombre solo puede contener letras y espacios");

        RuleFor(x => x.Descripcion)
            .MaximumLength(200)
            .WithMessage("La descripción no puede exceder 200 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Descripcion));

        RuleFor(x => x.NivelAcceso)
            .InclusiveBetween(1, 100)
            .WithMessage("El nivel de acceso debe estar entre 1 y 100")
            .NotEmpty()
            .WithMessage("El nivel de acceso es obligatorio");
    }
}
