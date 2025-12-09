using FluentValidation;
using G2rismBeta.API.DTOs.Reserva;

namespace G2rismBeta.API.Validators;

/// <summary>
/// Validador para la creación de reservas completas
/// Valida la estructura y formato de los datos de entrada
/// </summary>
public class ReservaCompletaCreateDtoValidator : AbstractValidator<ReservaCompletaCreateDto>
{
    public ReservaCompletaCreateDtoValidator()
    {
        // ========================================
        // VALIDACIONES DE DATOS BÁSICOS
        // ========================================

        RuleFor(x => x.IdCliente)
            .GreaterThan(0)
            .WithMessage("El ID del cliente debe ser mayor a 0");

        RuleFor(x => x.IdEmpleado)
            .GreaterThan(0)
            .WithMessage("El ID del empleado debe ser mayor a 0");

        RuleFor(x => x.Descripcion)
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.Descripcion))
            .WithMessage("La descripción no puede exceder 500 caracteres");

        RuleFor(x => x.FechaInicioViaje)
            .NotEmpty()
            .WithMessage("La fecha de inicio del viaje es obligatoria")
            .Must(BeValidDate)
            .WithMessage("La fecha de inicio debe ser una fecha válida");

        RuleFor(x => x.FechaFinViaje)
            .NotEmpty()
            .WithMessage("La fecha de fin del viaje es obligatoria")
            .Must(BeValidDate)
            .WithMessage("La fecha de fin debe ser una fecha válida")
            .GreaterThan(x => x.FechaInicioViaje)
            .WithMessage("La fecha de fin debe ser posterior a la fecha de inicio");

        RuleFor(x => x.NumeroPasajeros)
            .GreaterThan(0)
            .WithMessage("El número de pasajeros debe ser mayor a 0")
            .LessThanOrEqualTo(100)
            .WithMessage("El número de pasajeros no puede exceder 100");

        RuleFor(x => x.Estado)
            .NotEmpty()
            .WithMessage("El estado es obligatorio")
            .Must(BeValidEstado)
            .WithMessage("El estado debe ser 'pendiente' o 'confirmada'");

        RuleFor(x => x.Observaciones)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Observaciones))
            .WithMessage("Las observaciones no pueden exceder 1000 caracteres");

        // ========================================
        // VALIDACIONES DE SERVICIOS
        // ========================================

        // Validar que al menos haya un servicio
        RuleFor(x => x)
            .Must(HaveAtLeastOneService)
            .WithMessage("Debe incluir al menos un servicio (hotel, vuelo, paquete o servicio adicional)");

        // Validar listas de servicios
        RuleFor(x => x.Hoteles)
            .NotNull()
            .WithMessage("La lista de hoteles no puede ser nula");

        RuleFor(x => x.Vuelos)
            .NotNull()
            .WithMessage("La lista de vuelos no puede ser nula");

        RuleFor(x => x.Paquetes)
            .NotNull()
            .WithMessage("La lista de paquetes no puede ser nula");

        RuleFor(x => x.Servicios)
            .NotNull()
            .WithMessage("La lista de servicios no puede ser nula");

        // Validar que no exceda un número razonable de servicios
        RuleFor(x => x.Hoteles.Count)
            .LessThanOrEqualTo(10)
            .WithMessage("No se pueden agregar más de 10 hoteles en una sola reserva");

        RuleFor(x => x.Vuelos.Count)
            .LessThanOrEqualTo(10)
            .WithMessage("No se pueden agregar más de 10 vuelos en una sola reserva");

        RuleFor(x => x.Paquetes.Count)
            .LessThanOrEqualTo(5)
            .WithMessage("No se pueden agregar más de 5 paquetes en una sola reserva");

        RuleFor(x => x.Servicios.Count)
            .LessThanOrEqualTo(20)
            .WithMessage("No se pueden agregar más de 20 servicios adicionales en una sola reserva");
    }

    /// <summary>
    /// Validar que la fecha sea válida (no sea DateTime.MinValue o DateTime.MaxValue)
    /// </summary>
    private bool BeValidDate(DateTime fecha)
    {
        return fecha != DateTime.MinValue && fecha != DateTime.MaxValue;
    }

    /// <summary>
    /// Validar que el estado sea válido para creación
    /// Solo se permiten 'pendiente' o 'confirmada' al crear
    /// </summary>
    private bool BeValidEstado(string estado)
    {
        if (string.IsNullOrWhiteSpace(estado))
            return false;

        var estadosValidos = new[] { "pendiente", "confirmada" };
        return estadosValidos.Contains(estado.ToLower());
    }

    /// <summary>
    /// Validar que al menos haya un servicio en la reserva
    /// </summary>
    private bool HaveAtLeastOneService(ReservaCompletaCreateDto dto)
    {
        return (dto.Hoteles?.Count ?? 0) > 0 ||
               (dto.Vuelos?.Count ?? 0) > 0 ||
               (dto.Paquetes?.Count ?? 0) > 0 ||
               (dto.Servicios?.Count ?? 0) > 0;
    }
}
