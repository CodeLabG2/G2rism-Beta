using FluentValidation;
using G2rismBeta.API.DTOs.Permiso;

namespace G2rismBeta.API.Validators;

/// <summary>
/// Validador para el DTO de actualización de permisos
/// </summary>
public class PermisoUpdateDtoValidator : AbstractValidator<PermisoUpdateDto>
{
    public PermisoUpdateDtoValidator()
    {
        RuleFor(x => x.Modulo)
            .NotEmpty()
            .WithMessage("El módulo es obligatorio")
            .Length(3, 50)
            .WithMessage("El módulo debe tener entre 3 y 50 caracteres")
            .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$")
            .WithMessage("El módulo solo puede contener letras y espacios");

        RuleFor(x => x.Accion)
            .NotEmpty()
            .WithMessage("La acción es obligatoria")
            .Length(3, 50)
            .WithMessage("La acción debe tener entre 3 y 50 caracteres")
            .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$")
            .WithMessage("La acción solo puede contener letras y espacios")
            .Must(BeValidAction)
            .WithMessage("La acción debe ser una de: Crear, Leer, Actualizar, Eliminar, Listar");

        RuleFor(x => x.Descripcion)
            .MaximumLength(200)
            .WithMessage("La descripción no puede exceder 200 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Descripcion));
    }

    private bool BeValidAction(string accion)
    {
        var accionesValidas = new[] { "Crear", "Leer", "Actualizar", "Eliminar", "Listar", "Exportar", "Importar" };
        return accionesValidas.Any(a => a.Equals(accion, StringComparison.OrdinalIgnoreCase));
    }
}
