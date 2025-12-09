using FluentValidation;
using G2rismBeta.API.DTOs.Factura;

namespace G2rismBeta.API.Validators;

/// <summary>
/// Validador para el DTO de actualización de facturas.
/// Valida las reglas de negocio antes de actualizar una factura existente.
/// </summary>
public class FacturaUpdateDtoValidator : AbstractValidator<FacturaUpdateDto>
{
    // Estados válidos para una factura
    private readonly string[] _estadosValidos = { "pendiente", "pagada", "cancelada", "vencida" };

    public FacturaUpdateDtoValidator()
    {
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
        // VALIDACIÓN: CufeCude (Opcional)
        // =============================================
        When(x => !string.IsNullOrWhiteSpace(x.CufeCude), () =>
        {
            RuleFor(x => x.CufeCude!)
                .MaximumLength(200)
                .WithMessage("El CUFE/CUDE no puede exceder 200 caracteres");
        });

        // =============================================
        // VALIDACIÓN: Estado (Opcional)
        // =============================================
        When(x => !string.IsNullOrWhiteSpace(x.Estado), () =>
        {
            RuleFor(x => x.Estado!)
                .Must(estado => _estadosValidos.Contains(estado.ToLower()))
                .WithMessage($"El estado debe ser uno de los siguientes: {string.Join(", ", _estadosValidos)}");
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