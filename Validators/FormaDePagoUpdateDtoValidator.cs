using FluentValidation;
using G2rismBeta.API.DTOs.FormaDePago;

namespace G2rismBeta.API.Validators;

/// <summary>
/// Validador para el DTO de actualización de formas de pago.
/// Valida las reglas de negocio antes de actualizar una forma de pago existente.
/// Todos los campos son opcionales, pero si se proporcionan deben ser válidos.
/// </summary>
public class FormaDePagoUpdateDtoValidator : AbstractValidator<FormaDePagoUpdateDto>
{
    public FormaDePagoUpdateDtoValidator()
    {
        // =============================================
        // VALIDACIÓN: Método (Opcional)
        // =============================================
        When(x => !string.IsNullOrWhiteSpace(x.Metodo), () =>
        {
            RuleFor(x => x.Metodo!)
                .MaximumLength(50)
                .WithMessage("El método de pago no puede exceder 50 caracteres")
                .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$")
                .WithMessage("El método de pago solo puede contener letras y espacios");
        });

        // =============================================
        // VALIDACIÓN: Descripción (Opcional)
        // =============================================
        When(x => !string.IsNullOrWhiteSpace(x.Descripcion), () =>
        {
            RuleFor(x => x.Descripcion!)
                .MaximumLength(200)
                .WithMessage("La descripción no puede exceder 200 caracteres");
        });
    }
}