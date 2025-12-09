using FluentValidation;
using G2rismBeta.API.DTOs.Reserva;

namespace G2rismBeta.API.Validators;

/// <summary>
/// Validador para el DTO de actualización de reservas
/// Permite actualizaciones parciales (todos los campos son opcionales)
/// </summary>
public class ReservaUpdateDtoValidator : AbstractValidator<ReservaUpdateDto>
{
    public ReservaUpdateDtoValidator()
    {
        // ========================================
        // VALIDACIÓN DEL ID DE EMPLEADO (OPCIONAL)
        // ========================================

        RuleFor(x => x.IdEmpleado)
            .GreaterThan(0)
            .WithMessage("El ID del empleado debe ser mayor a 0")
            .When(x => x.IdEmpleado.HasValue);

        // ========================================
        // VALIDACIÓN DE LA DESCRIPCIÓN (OPCIONAL)
        // ========================================

        RuleFor(x => x.Descripcion)
            .MaximumLength(500)
            .WithMessage("La descripción no puede exceder 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Descripcion));

        // ========================================
        // VALIDACIÓN DE FECHA DE INICIO (OPCIONAL)
        // ========================================

        RuleFor(x => x.FechaInicioViaje)
            .Must(fecha => fecha.HasValue && fecha.Value.Date >= DateTime.Today)
            .WithMessage("La fecha de inicio del viaje no puede ser en el pasado")
            .When(x => x.FechaInicioViaje.HasValue);

        // ========================================
        // VALIDACIÓN DE FECHA DE FIN (OPCIONAL)
        // ========================================

        // Si se proporciona FechaFinViaje, debe ser válida
        RuleFor(x => x.FechaFinViaje)
            .NotEmpty()
            .WithMessage("La fecha de fin del viaje no puede estar vacía")
            .When(x => x.FechaFinViaje.HasValue);

        // ========================================
        // VALIDACIÓN CRUZADA DE FECHAS
        // ========================================

        // Si se proporcionan ambas fechas, validar que sean coherentes
        RuleFor(x => x)
            .Must(x => x.FechaFinViaje!.Value > x.FechaInicioViaje!.Value)
            .WithMessage("La fecha de fin debe ser posterior a la fecha de inicio")
            .When(x => x.FechaInicioViaje.HasValue && x.FechaFinViaje.HasValue);

        // ========================================
        // VALIDACIÓN DE NÚMERO DE PASAJEROS (OPCIONAL)
        // ========================================

        RuleFor(x => x.NumeroPasajeros)
            .GreaterThan(0)
            .WithMessage("El número de pasajeros debe ser mayor a 0")
            .LessThanOrEqualTo(100)
            .WithMessage("El número de pasajeros no puede exceder 100")
            .When(x => x.NumeroPasajeros.HasValue);

        // ========================================
        // VALIDACIÓN DEL ESTADO (OPCIONAL)
        // ========================================

        RuleFor(x => x.Estado)
            .NotEmpty()
            .WithMessage("El estado no puede estar vacío")
            .Must(estado => new[] { "pendiente", "confirmada", "cancelada", "completada" }.Contains(estado!.ToLower()))
            .WithMessage("Estado no válido. Valores permitidos: pendiente, confirmada, cancelada, completada")
            .When(x => !string.IsNullOrEmpty(x.Estado));

        // ========================================
        // VALIDACIÓN DE OBSERVACIONES (OPCIONAL)
        // ========================================

        RuleFor(x => x.Observaciones)
            .MaximumLength(1000)
            .WithMessage("Las observaciones no pueden exceder 1000 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Observaciones));
    }
}