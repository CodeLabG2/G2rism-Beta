using FluentValidation;
using G2rismBeta.API.DTOs.Rol;

namespace G2rismBeta.API.Validators;

/// <summary>
/// Validador para el DTO de actualización de roles
/// IMPORTANTE: Todos los campos son opcionales para soportar actualizaciones parciales
/// Las validaciones solo se aplican cuando el campo NO es null
/// </summary>
public class RolUpdateDtoValidator : AbstractValidator<RolUpdateDto>
{
    public RolUpdateDtoValidator()
    {
        // Validar nombre SOLO si se proporciona
        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre del rol no puede estar vacío")
            .Length(3, 50)
            .WithMessage("El nombre debe tener entre 3 y 50 caracteres")
            .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$")
            .WithMessage("El nombre solo puede contener letras y espacios")
            .When(x => x.Nombre != null);

        // Validar descripción SOLO si se proporciona
        RuleFor(x => x.Descripcion)
            .MaximumLength(200)
            .WithMessage("La descripción no puede exceder 200 caracteres")
            .When(x => x.Descripcion != null);

        // Validar nivel de acceso SOLO si se proporciona
        RuleFor(x => x.NivelAcceso)
            .InclusiveBetween(1, 100)
            .WithMessage("El nivel de acceso debe estar entre 1 y 100")
            .When(x => x.NivelAcceso.HasValue);

        // Validar estado SOLO si se proporciona (no hay validaciones específicas para bool)
        // El estado puede ser true o false, ambos son válidos
    }
}
