using FluentValidation;
using G2rismBeta.API.DTOs.ReservaVuelo;

namespace G2rismBeta.API.Validators;

/// <summary>
/// Validador para el DTO de creación de ReservaVuelo
/// Valida las reglas de negocio antes de agregar un vuelo a una reserva
/// </summary>
public class ReservaVueloCreateDtoValidator : AbstractValidator<ReservaVueloCreateDto>
{
    public ReservaVueloCreateDtoValidator()
    {
        // Validación de IdReserva
        RuleFor(x => x.IdReserva)
            .GreaterThan(0)
            .WithMessage("El ID de la reserva debe ser mayor a 0");

        // Validación de IdVuelo
        RuleFor(x => x.IdVuelo)
            .GreaterThan(0)
            .WithMessage("El ID del vuelo debe ser mayor a 0");

        // Validación de NumeroPasajeros
        RuleFor(x => x.NumeroPasajeros)
            .GreaterThan(0)
            .WithMessage("El número de pasajeros debe ser mayor a 0")
            .LessThanOrEqualTo(100)
            .WithMessage("El número de pasajeros no puede exceder 100");

        // Validación de Clase
        RuleFor(x => x.Clase)
            .NotEmpty()
            .WithMessage("La clase del vuelo es obligatoria")
            .Must(clase => clase.ToLower() == "economica" || clase.ToLower() == "ejecutiva")
            .WithMessage("La clase debe ser 'economica' o 'ejecutiva'");

        // Validación de AsientosAsignados (opcional, pero si se provee, validar formato)
        When(x => !string.IsNullOrWhiteSpace(x.AsientosAsignados), () =>
        {
            RuleFor(x => x.AsientosAsignados)
                .MaximumLength(500)
                .WithMessage("Los asientos asignados no pueden exceder 500 caracteres");
        });

        // Validación de EquipajeExtra
        When(x => x.EquipajeExtra.HasValue, () =>
        {
            RuleFor(x => x.EquipajeExtra)
                .GreaterThanOrEqualTo(0)
                .WithMessage("El equipaje extra no puede ser negativo")
                .LessThanOrEqualTo(200)
                .WithMessage("El equipaje extra no puede exceder 200 kg");
        });

        // Validación de CostoEquipajeExtra
        RuleFor(x => x.CostoEquipajeExtra)
            .GreaterThanOrEqualTo(0)
            .WithMessage("El costo del equipaje extra no puede ser negativo");

        // Validación cruzada: Si hay equipaje extra, debe haber un costo (opcional, según reglas de negocio)
        When(x => x.EquipajeExtra.HasValue && x.EquipajeExtra.Value > 0, () =>
        {
            RuleFor(x => x.CostoEquipajeExtra)
                .GreaterThan(0)
                .WithMessage("Si hay equipaje extra, debe especificar el costo adicional");
        });
    }
}
