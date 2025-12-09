using FluentValidation;
using G2rismBeta.API.DTOs.Factura;

namespace G2rismBeta.API.Validators;

/// <summary>
/// Validador para el DTO de creación de facturas.
/// Valida las reglas de negocio antes de crear una nueva factura.
/// </summary>
public class FacturaCreateDtoValidator : AbstractValidator<FacturaCreateDto>
{
    public FacturaCreateDtoValidator()
    {
        // =============================================
        // VALIDACIÓN: IdReserva
        // =============================================
        RuleFor(x => x.IdReserva)
            .NotEmpty().WithMessage("El ID de la reserva es obligatorio")
            .GreaterThan(0).WithMessage("El ID de la reserva debe ser mayor a 0");

        // =============================================
        // VALIDACIÓN: FechaVencimiento (Opcional)
        // =============================================
        When(x => x.FechaVencimiento.HasValue, () =>
        {
            RuleFor(x => x.FechaVencimiento!.Value)
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("La fecha de vencimiento no puede ser anterior a hoy");
        });

        // =============================================
        // VALIDACIÓN: ResolucionDian (Opcional)
        // =============================================
        When(x => !string.IsNullOrWhiteSpace(x.ResolucionDian), () =>
        {
            RuleFor(x => x.ResolucionDian!)
                .MaximumLength(100)
                .WithMessage("La resolución DIAN no puede exceder 100 caracteres");
        });

        // =============================================
        // VALIDACIÓN: PorcentajeIva (Opcional)
        // =============================================
        When(x => x.PorcentajeIva.HasValue, () =>
        {
            RuleFor(x => x.PorcentajeIva!.Value)
                .InclusiveBetween(0, 100)
                .WithMessage("El porcentaje de IVA debe estar entre 0 y 100");
        });

        // =============================================
        // VALIDACIÓN: DescuentosAdicionales (Opcional)
        // =============================================
        When(x => x.DescuentosAdicionales.HasValue, () =>
        {
            RuleFor(x => x.DescuentosAdicionales!.Value)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Los descuentos adicionales no pueden ser negativos");
        });

        // =============================================
        // VALIDACIÓN: Observaciones (Opcional)
        // =============================================
        When(x => !string.IsNullOrWhiteSpace(x.Observaciones), () =>
        {
            RuleFor(x => x.Observaciones!)
                .MaximumLength(1000)
                .WithMessage("Las observaciones no pueden exceder 1000 caracteres");
        });
    }
}