using FluentValidation;
using G2rismBeta.API.DTOs.CategoriaCliente;

namespace G2rismBeta.API.Validators;

/// <summary>
/// Validador para el DTO de creación de categorías de cliente
/// </summary>
public class CategoriaClienteCreateDtoValidator : AbstractValidator<CategoriaClienteCreateDto>
{
    public CategoriaClienteCreateDtoValidator()
    {
        // ========================================
        // VALIDACIÓN DEL NOMBRE
        // ========================================

        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre de la categoría es obligatorio")
            .Length(2, 50)
            .WithMessage("El nombre debe tener entre 2 y 50 caracteres")
            .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$")
            .WithMessage("El nombre solo puede contener letras y espacios");

        // ========================================
        // VALIDACIÓN DE LA DESCRIPCIÓN
        // ========================================

        RuleFor(x => x.Descripcion)
            .MaximumLength(500)
            .WithMessage("La descripción no puede exceder 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Descripcion));

        // ========================================
        // VALIDACIÓN DEL COLOR HEXADECIMAL
        // ========================================

        RuleFor(x => x.ColorHex)
            .Matches(@"^#[0-9A-Fa-f]{6}$")
            .WithMessage("El color debe estar en formato hexadecimal válido (#RRGGBB)")
            .When(x => !string.IsNullOrEmpty(x.ColorHex));

        // ========================================
        // VALIDACIÓN DE BENEFICIOS
        // ========================================

        RuleFor(x => x.Beneficios)
            .MaximumLength(1000)
            .WithMessage("Los beneficios no pueden exceder 1000 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Beneficios));

        // ========================================
        // VALIDACIÓN DE CRITERIOS DE CLASIFICACIÓN
        // ========================================

        RuleFor(x => x.CriteriosClasificacion)
            .MaximumLength(500)
            .WithMessage("Los criterios de clasificación no pueden exceder 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.CriteriosClasificacion));

        // ========================================
        // VALIDACIÓN DEL DESCUENTO
        // ========================================

        RuleFor(x => x.DescuentoPorcentaje)
            .InclusiveBetween(0, 100)
            .WithMessage("El descuento debe estar entre 0 y 100")
            .NotNull()
            .WithMessage("El descuento es obligatorio");
    }
}
