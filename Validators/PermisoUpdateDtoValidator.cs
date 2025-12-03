using FluentValidation;
using G2rismBeta.API.DTOs.Permiso;

namespace G2rismBeta.API.Validators;

/// <summary>
/// Validador para el DTO de actualización de permisos
/// Soporta actualizaciones parciales (campos opcionales)
/// Utiliza la clase base PermisoValidatorBase para reutilizar validaciones comunes
/// </summary>
public class PermisoUpdateDtoValidator : AbstractValidator<PermisoUpdateDto>
{
    public PermisoUpdateDtoValidator()
    {
        // ID del permiso es obligatorio
        RuleFor(x => x.IdPermiso)
            .GreaterThan(0)
            .WithMessage("El ID del permiso debe ser mayor a 0");

        // Módulo es opcional, pero si se proporciona debe ser válido
        When(x => !string.IsNullOrEmpty(x.Modulo), () =>
        {
            PermisoValidatorBase.ConfigurarValidacionModulo(RuleFor(x => x.Modulo!), esRequerido: false);
        });

        // Acción es opcional, pero si se proporciona debe ser válida
        When(x => !string.IsNullOrEmpty(x.Accion), () =>
        {
            PermisoValidatorBase.ConfigurarValidacionAccion(RuleFor(x => x.Accion!), esRequerido: false);
        });

        // Descripción es opcional
        When(x => !string.IsNullOrEmpty(x.Descripcion), () =>
        {
            PermisoValidatorBase.ConfigurarValidacionDescripcion(RuleFor(x => x.Descripcion));
        });
    }
}
