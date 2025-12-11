using FluentValidation;
using G2rismBeta.API.DTOs.Pago;

namespace G2rismBeta.API.Validators;

/// <summary>
/// Validador para PagoUpdateDto
/// </summary>
public class PagoUpdateDtoValidator : AbstractValidator<PagoUpdateDto>
{
    private static readonly string[] EstadosValidos = { "pendiente", "aprobado", "rechazado" };

    public PagoUpdateDtoValidator()
    {
        RuleFor(x => x.Monto)
            .GreaterThan(0)
            .When(x => x.Monto.HasValue)
            .WithMessage("El monto del pago debe ser mayor a 0")
            .LessThanOrEqualTo(999999999.99m)
            .When(x => x.Monto.HasValue)
            .WithMessage("El monto del pago no puede exceder 999,999,999.99");

        RuleFor(x => x.ReferenciaTransaccion)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.ReferenciaTransaccion))
            .WithMessage("La referencia de transacción no puede exceder 100 caracteres");

        RuleFor(x => x.ComprobantePago)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.ComprobantePago))
            .WithMessage("El comprobante de pago no puede exceder 500 caracteres");

        RuleFor(x => x.Estado)
            .Must(estado => EstadosValidos.Contains(estado!.ToLower()))
            .When(x => !string.IsNullOrWhiteSpace(x.Estado))
            .WithMessage($"El estado debe ser uno de los siguientes: {string.Join(", ", EstadosValidos)}");

        RuleFor(x => x.Observaciones)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Observaciones))
            .WithMessage("Las observaciones no pueden exceder 500 caracteres");
    }
}