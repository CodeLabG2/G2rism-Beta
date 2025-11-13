using FluentValidation;
using G2rismBeta.API.DTOs.ContratoProveedor;

namespace G2rismBeta.API.Validators;

/// <summary>
/// Validador para la actualización de contratos de proveedor
/// Todos los campos son opcionales, pero si se proporcionan deben ser válidos
/// </summary>
public class ContratoProveedorUpdateDtoValidator : AbstractValidator<ContratoProveedorUpdateDto>
{
    public ContratoProveedorUpdateDtoValidator()
    {
        // ========================================
        // VALIDACIONES DE NÚMERO DE CONTRATO
        // ========================================
        RuleFor(x => x.NumeroContrato)
            .Length(3, 50).WithMessage("El número de contrato debe tener entre 3 y 50 caracteres")
            .Matches(@"^[a-zA-Z0-9\-]+$")
            .WithMessage("El número de contrato solo debe contener letras, números y guiones")
            .When(x => !string.IsNullOrWhiteSpace(x.NumeroContrato));
        // Nota: La validación de unicidad se hace en el servicio

        // ========================================
        // VALIDACIONES DE FECHAS
        // ========================================
        RuleFor(x => x.FechaInicio)
            .NotEmpty().WithMessage("La fecha de inicio no puede estar vacía")
            .When(x => x.FechaInicio.HasValue);

        RuleFor(x => x.FechaFin)
            .NotEmpty().WithMessage("La fecha de fin no puede estar vacía")
            .When(x => x.FechaFin.HasValue);

        // Validación cruzada de fechas (cuando ambas están presentes)
        RuleFor(x => x)
            .Must(x => !x.FechaInicio.HasValue || !x.FechaFin.HasValue || x.FechaFin > x.FechaInicio)
            .WithMessage("La fecha de fin debe ser posterior a la fecha de inicio")
            .When(x => x.FechaInicio.HasValue && x.FechaFin.HasValue);

        // ========================================
        // VALIDACIONES DE TIPO DE CONTRATO
        // ========================================
        RuleFor(x => x.TipoContrato)
            .Length(3, 50).WithMessage("El tipo de contrato debe tener entre 3 y 50 caracteres")
            .When(x => !string.IsNullOrWhiteSpace(x.TipoContrato));

        // ========================================
        // VALIDACIONES DE VALOR DEL CONTRATO
        // ========================================
        RuleFor(x => x.ValorContrato)
            .GreaterThanOrEqualTo(0).WithMessage("El valor del contrato debe ser mayor o igual a 0")
            .LessThanOrEqualTo(999999999999.99m).WithMessage("El valor del contrato excede el límite permitido")
            .When(x => x.ValorContrato.HasValue);

        // ========================================
        // VALIDACIONES DE CONDICIONES DE PAGO
        // ========================================
        RuleFor(x => x.CondicionesPago)
            .MinimumLength(10).WithMessage("Las condiciones de pago deben tener al menos 10 caracteres")
            .MaximumLength(2000).WithMessage("Las condiciones de pago no pueden exceder 2000 caracteres")
            .When(x => !string.IsNullOrWhiteSpace(x.CondicionesPago));

        // ========================================
        // VALIDACIONES DE TÉRMINOS
        // ========================================
        RuleFor(x => x.Terminos)
            .MinimumLength(20).WithMessage("Los términos deben tener al menos 20 caracteres")
            .MaximumLength(5000).WithMessage("Los términos no pueden exceder 5000 caracteres")
            .When(x => !string.IsNullOrWhiteSpace(x.Terminos));

        // ========================================
        // VALIDACIONES DE ESTADO
        // ========================================
        RuleFor(x => x.Estado)
            .Must(estado => new[] { "Vigente", "Vencido", "Cancelado", "En_Negociacion" }.Contains(estado!))
            .WithMessage("El estado debe ser: Vigente, Vencido, Cancelado o En_Negociacion")
            .When(x => !string.IsNullOrWhiteSpace(x.Estado));

        // ========================================
        // VALIDACIONES DE ARCHIVO CONTRATO
        // ========================================
        RuleFor(x => x.ArchivoContrato)
            .Must(BeAValidUrl)
            .WithMessage("El formato de la URL del archivo no es válido")
            .MaximumLength(500).WithMessage("La URL del archivo no puede exceder 500 caracteres")
            .When(x => !string.IsNullOrWhiteSpace(x.ArchivoContrato));

        // ========================================
        // VALIDACIONES DE OBSERVACIONES
        // ========================================
        RuleFor(x => x.Observaciones)
            .MaximumLength(2000).WithMessage("Las observaciones no pueden exceder 2000 caracteres")
            .When(x => !string.IsNullOrWhiteSpace(x.Observaciones));
    }

    /// <summary>
    /// Validar formato de URL
    /// </summary>
    private bool BeAValidUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return true;

        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}
