using FluentValidation;
using G2rismBeta.API.DTOs.Permiso;

namespace G2rismBeta.API.Validators;

/// <summary>
/// Validador para el DTO de creación de permisos
/// Utiliza la clase base PermisoValidatorBase para reutilizar validaciones comunes
/// </summary>
public class PermisoCreateDtoValidator : AbstractValidator<PermisoCreateDto>
{
    public PermisoCreateDtoValidator()
    {
        // Validación del Módulo (requerido)
        PermisoValidatorBase.ConfigurarValidacionModulo(RuleFor(x => x.Modulo), esRequerido: true);

        // Validación de la Acción (requerida)
        PermisoValidatorBase.ConfigurarValidacionAccion(RuleFor(x => x.Accion), esRequerido: true);

        // Validación de la Descripción (opcional)
        RuleFor(x => x.Descripcion)
            .Must(desc => desc == null || desc.Length <= 200)
            .WithMessage("La descripción no puede exceder 200 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Descripcion));
    }
}
