using FluentValidation;
using G2rismBeta.API.DTOs.FormaDePago;

namespace G2rismBeta.API.Validators;

/// <summary>
/// Validador para el DTO de creación de formas de pago.
/// Valida las reglas de negocio antes de crear una nueva forma de pago.
/// </summary>
public class FormaDePagoCreateDtoValidator : AbstractValidator<FormaDePagoCreateDto>
{
    public FormaDePagoCreateDtoValidator()
    {
        // =============================================
        // VALIDACIÓN: Método
        // =============================================
        RuleFor(x => x.Metodo)
            .NotEmpty().WithMessage("El método de pago es obligatorio")
            .MaximumLength(50).WithMessage("El método de pago no puede exceder 50 caracteres")
            .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$")
            .WithMessage("El método de pago solo puede contener letras y espacios");

        // =============================================
        // VALIDACIÓN: Descripción (Opcional)
        // =============================================
        When(x => !string.IsNullOrWhiteSpace(x.Descripcion), () =>
        {
            RuleFor(x => x.Descripcion!)
                .MaximumLength(200)
                .WithMessage("La descripción no puede exceder 200 caracteres");
        });

        // =============================================
        // VALIDACIÓN: RequiereVerificacion
        // =============================================
        RuleFor(x => x.RequiereVerificacion)
            .NotNull().WithMessage("Debe indicar si requiere verificación");

        // =============================================
        // VALIDACIÓN: Activo
        // =============================================
        RuleFor(x => x.Activo)
            .NotNull().WithMessage("Debe indicar si está activo");
    }
}