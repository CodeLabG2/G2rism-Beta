using FluentValidation;
using G2rismBeta.API.DTOs.Rol;

namespace G2rismBeta.API.Validators;

/// <summary>
/// Validador para el DTO de creación de roles
/// </summary>
public class RolCreateDtoValidator : AbstractValidator<RolCreateDto>
{
    public RolCreateDtoValidator()
    {
        // ========================================
        // VALIDACIÓN DEL NOMBRE
        // ========================================

        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre del rol es obligatorio")
            .Length(3, 50)
            .WithMessage("El nombre debe tener entre 3 y 50 caracteres")
            .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$")
            .WithMessage("El nombre solo puede contener letras y espacios");

        // ========================================
        // VALIDACIÓN DE LA DESCRIPCIÓN
        // ========================================

        RuleFor(x => x.Descripcion)
            .MaximumLength(200)
            .WithMessage("La descripción no puede exceder 200 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Descripcion)); // Solo validar si no está vacía

        // ========================================
        // VALIDACIÓN DEL NIVEL DE ACCESO
        // ========================================

        RuleFor(x => x.NivelAcceso)
            .InclusiveBetween(1, 100)
            .WithMessage("El nivel de acceso debe estar entre 1 y 100")
            .NotEmpty()
            .WithMessage("El nivel de acceso es obligatorio");
    }
}
