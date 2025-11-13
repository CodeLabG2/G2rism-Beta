using FluentValidation;
using G2rismBeta.API.DTOs.PreferenciaCliente;

namespace G2rismBeta.API.Validators
{
    /// <summary>
    /// Validador para la creación de preferencias de cliente
    /// Valida los datos requeridos para crear las preferencias de un cliente en el sistema CRM
    /// </summary>
    public class PreferenciaClienteCreateDtoValidator : AbstractValidator<PreferenciaClienteCreateDto>
    {
        public PreferenciaClienteCreateDtoValidator()
        {
            // Validación del ID del cliente (requerido)
            RuleFor(x => x.IdCliente)
                .NotEmpty()
                .WithMessage("El ID del cliente es obligatorio")
                .GreaterThan(0)
                .WithMessage("El ID del cliente debe ser mayor a 0");

            // Validación del tipo de destino
            RuleFor(x => x.TipoDestino)
                .MaximumLength(50)
                .WithMessage("El tipo de destino no puede exceder 50 caracteres")
                .When(x => !string.IsNullOrWhiteSpace(x.TipoDestino));

            // Validación del tipo de alojamiento
            RuleFor(x => x.TipoAlojamiento)
                .MaximumLength(50)
                .WithMessage("El tipo de alojamiento no puede exceder 50 caracteres")
                .When(x => !string.IsNullOrWhiteSpace(x.TipoAlojamiento));

            // Validación del presupuesto promedio
            RuleFor(x => x.PresupuestoPromedio)
                .GreaterThanOrEqualTo(0)
                .WithMessage("El presupuesto promedio no puede ser negativo")
                .LessThanOrEqualTo(999999999.99m)
                .WithMessage("El presupuesto promedio excede el límite permitido")
                .When(x => x.PresupuestoPromedio.HasValue);

            // Validación de preferencias de alimentación
            RuleFor(x => x.PreferenciasAlimentacion)
                .MaximumLength(200)
                .WithMessage("Las preferencias de alimentación no pueden exceder 200 caracteres")
                .When(x => !string.IsNullOrWhiteSpace(x.PreferenciasAlimentacion));

            // Validación de cantidad de intereses + longitud total (JSON string)
            RuleFor(x => x.Intereses)
                .Must(list => list == null || list.Count <= 20)
                .WithMessage("No se pueden registrar más de 20 intereses");

            RuleForEach(x => x.Intereses)
                .MaximumLength(100)
                .WithMessage("Cada interés no puede exceder los 100 caracteres");
        }
    }
}