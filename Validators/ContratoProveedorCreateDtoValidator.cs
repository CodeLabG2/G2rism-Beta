using FluentValidation;
using G2rismBeta.API.DTOs.ContratoProveedor;

namespace G2rismBeta.API.Validators;

/// <summary>
/// Validador para la creación de contratos de proveedor
/// Implementa validaciones estructurales síncronas
/// Las validaciones que requieren acceso a BD se realizan en el servicio
/// </summary>
public class ContratoProveedorCreateDtoValidator : AbstractValidator<ContratoProveedorCreateDto>
{
    public ContratoProveedorCreateDtoValidator()
    {
        // ========================================
        // VALIDACIONES DE PROVEEDOR
        // ========================================
        RuleFor(x => x.IdProveedor)
            .GreaterThan(0).WithMessage("El ID del proveedor debe ser mayor a 0");
        // Nota: La validación de existencia del proveedor se hace en el servicio

        // ========================================
        // VALIDACIONES DE NÚMERO DE CONTRATO
        // ========================================
        RuleFor(x => x.NumeroContrato)
            .NotEmpty().WithMessage("El número de contrato es obligatorio")
            .Length(3, 50).WithMessage("El número de contrato debe tener entre 3 y 50 caracteres")
            .Matches(@"^[a-zA-Z0-9\-]+$")
            .WithMessage("El número de contrato solo debe contener letras, números y guiones");
        // Nota: La validación de unicidad se hace en el servicio

        // ========================================
        // VALIDACIONES DE FECHAS
        // ========================================
        RuleFor(x => x.FechaInicio)
            .NotEmpty().WithMessage("La fecha de inicio es obligatoria");

        RuleFor(x => x.FechaFin)
            .NotEmpty().WithMessage("La fecha de fin es obligatoria")
            .GreaterThan(x => x.FechaInicio)
            .WithMessage("La fecha de fin debe ser posterior a la fecha de inicio");

        // Validación de duración mínima (al menos 1 día)
        RuleFor(x => x)
            .Must(x => (x.FechaFin - x.FechaInicio).TotalDays >= 1)
            .WithMessage("La duración del contrato debe ser de al menos 1 día");

        // ========================================
        // VALIDACIONES DE TIPO DE CONTRATO
        // ========================================
        RuleFor(x => x.TipoContrato)
            .NotEmpty().WithMessage("El tipo de contrato es obligatorio")
            .Length(3, 50).WithMessage("El tipo de contrato debe tener entre 3 y 50 caracteres");

        // ========================================
        // VALIDACIONES DE VALOR DEL CONTRATO
        // ========================================
        RuleFor(x => x.ValorContrato)
            .GreaterThanOrEqualTo(0).WithMessage("El valor del contrato debe ser mayor o igual a 0")
            .LessThanOrEqualTo(999999999999.99m).WithMessage("El valor del contrato excede el límite permitido");

        // ========================================
        // VALIDACIONES DE CONDICIONES DE PAGO
        // ========================================
        RuleFor(x => x.CondicionesPago)
            .NotEmpty().WithMessage("Las condiciones de pago son obligatorias")
            .MinimumLength(10).WithMessage("Las condiciones de pago deben tener al menos 10 caracteres")
            .MaximumLength(2000).WithMessage("Las condiciones de pago no pueden exceder 2000 caracteres");

        // ========================================
        // VALIDACIONES DE TÉRMINOS
        // ========================================
        RuleFor(x => x.Terminos)
            .NotEmpty().WithMessage("Los términos del contrato son obligatorios")
            .MinimumLength(20).WithMessage("Los términos deben tener al menos 20 caracteres")
            .MaximumLength(5000).WithMessage("Los términos no pueden exceder 5000 caracteres");

        // ========================================
        // VALIDACIONES DE ESTADO
        // ========================================
        RuleFor(x => x.Estado)
            .NotEmpty().WithMessage("El estado es obligatorio")
            .Must(estado => new[] { "Vigente", "Vencido", "Cancelado", "En_Negociacion" }.Contains(estado))
            .WithMessage("El estado debe ser: Vigente, Vencido, Cancelado o En_Negociacion");

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
