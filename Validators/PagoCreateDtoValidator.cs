using FluentValidation;
using G2rismBeta.API.DTOs.Pago;

namespace G2rismBeta.API.Validators;

/// <summary>
/// Validador para PagoCreateDto
/// </summary>
public class PagoCreateDtoValidator : AbstractValidator<PagoCreateDto>
{
    private static readonly string[] EstadosValidos = { "pendiente", "aprobado", "rechazado" };

    public PagoCreateDtoValidator()
    {
        RuleFor(x => x.IdFactura)
            .GreaterThan(0)
            .WithMessage("El ID de la factura debe ser mayor a 0");

        RuleFor(x => x.IdFormaPago)
            .GreaterThan(0)
            .WithMessage("El ID de la forma de pago debe ser mayor a 0");

        RuleFor(x => x.Monto)
            .GreaterThan(0)
            .WithMessage("El monto del pago debe ser mayor a 0")
            .LessThanOrEqualTo(999999999.99m)
            .WithMessage("El monto del pago no puede exceder 999,999,999.99");

        RuleFor(x => x.FechaPago)
            .LessThanOrEqualTo(DateTime.Now.AddDays(1))
            .When(x => x.FechaPago.HasValue)
            .WithMessage("La fecha de pago no puede ser futura");

        RuleFor(x => x.ReferenciaTransaccion)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.ReferenciaTransaccion))
            .WithMessage("La referencia de transacción no puede exceder 100 caracteres");

        RuleFor(x => x.ComprobantePago)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.ComprobantePago))
            .WithMessage("El comprobante de pago no puede exceder 500 caracteres");

        RuleFor(x => x.Estado)
            .Must(estado => string.IsNullOrWhiteSpace(estado) || EstadosValidos.Contains(estado.ToLower()))
            .When(x => !string.IsNullOrWhiteSpace(x.Estado))
            .WithMessage($"El estado debe ser uno de los siguientes: {string.Join(", ", EstadosValidos)}");

        RuleFor(x => x.Observaciones)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Observaciones))
            .WithMessage("Las observaciones no pueden exceder 500 caracteres");
    }
}