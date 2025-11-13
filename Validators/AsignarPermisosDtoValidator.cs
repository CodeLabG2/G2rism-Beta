using FluentValidation;
using G2rismBeta.API.DTOs.RolPermiso;

namespace G2rismBeta.API.Validators;

/// <summary>
/// Validador para el DTO de asignación de permisos
/// </summary>
public class AsignarPermisosDtoValidator : AbstractValidator<AsignarPermisosMultiplesDto>
{
    public AsignarPermisosDtoValidator()
    {
        RuleFor(x => x.IdsPermisos)
            .NotNull()
            .WithMessage("La lista de permisos no puede ser nula")
            .NotEmpty()
            .WithMessage("Debe proporcionar al menos un permiso")
            .Must(x => x.Count > 0)
            .WithMessage("Debe proporcionar al menos un permiso");

        RuleForEach(x => x.IdsPermisos)
            .GreaterThan(0)
            .WithMessage("Todos los IDs de permisos deben ser mayores a 0");
    }
}
