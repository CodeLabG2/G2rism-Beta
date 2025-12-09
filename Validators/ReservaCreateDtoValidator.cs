using FluentValidation;
using G2rismBeta.API.DTOs.Reserva;

namespace G2rismBeta.API.Validators;

/// <summary>
/// Validador para el DTO de creación de reservas
/// Valida todas las reglas estructurales para crear una reserva
/// </summary>
public class ReservaCreateDtoValidator : AbstractValidator<ReservaCreateDto>
{
    public ReservaCreateDtoValidator()
    {
        // ========================================
        // VALIDACIÓN DEL ID DE CLIENTE
        // ========================================

        RuleFor(x => x.IdCliente)
            .GreaterThan(0)
            .WithMessage("El ID del cliente debe ser mayor a 0");

        // ========================================
        // VALIDACIÓN DEL ID DE EMPLEADO
        // ========================================

        RuleFor(x => x.IdEmpleado)
            .GreaterThan(0)
            .WithMessage("El ID del empleado debe ser mayor a 0");

        // ========================================
        // VALIDACIÓN DE LA DESCRIPCIÓN
        // ========================================

        RuleFor(x => x.Descripcion)
            .MaximumLength(500)
            .WithMessage("La descripción no puede exceder 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Descripcion));

        // ========================================
        // VALIDACIÓN DE FECHA DE INICIO
        // ========================================

        RuleFor(x => x.FechaInicioViaje)
            .NotEmpty()
            .WithMessage("La fecha de inicio del viaje es obligatoria")
            .Must(fecha => fecha.Date >= DateTime.Today)
            .WithMessage("La fecha de inicio del viaje no puede ser en el pasado");

        // ========================================
        // VALIDACIÓN DE FECHA DE FIN
        // ========================================

        RuleFor(x => x.FechaFinViaje)
            .NotEmpty()
            .WithMessage("La fecha de fin del viaje es obligatoria")
            .GreaterThan(x => x.FechaInicioViaje)
            .WithMessage("La fecha de fin debe ser posterior a la fecha de inicio");

        // ========================================
        // VALIDACIÓN CRUZADA DE FECHAS
        // ========================================

        RuleFor(x => x)
            .Must(x => (x.FechaFinViaje - x.FechaInicioViaje).Days <= 365)
            .WithMessage("La duración del viaje no puede exceder 365 días")
            .Must(x => (x.FechaFinViaje - x.FechaInicioViaje).Days >= 1)
            .WithMessage("El viaje debe tener al menos 1 día de duración");

        // ========================================
        // VALIDACIÓN DE NÚMERO DE PASAJEROS
        // ========================================

        RuleFor(x => x.NumeroPasajeros)
            .GreaterThan(0)
            .WithMessage("El número de pasajeros debe ser mayor a 0")
            .LessThanOrEqualTo(100)
            .WithMessage("El número de pasajeros no puede exceder 100");

        // ========================================
        // VALIDACIÓN DEL ESTADO
        // ========================================

        RuleFor(x => x.Estado)
            .NotEmpty()
            .WithMessage("El estado es obligatorio")
            .Must(estado => new[] { "pendiente", "confirmada" }.Contains(estado.ToLower()))
            .WithMessage("El estado inicial debe ser 'pendiente' o 'confirmada'");

        // ========================================
        // VALIDACIÓN DE OBSERVACIONES
        // ========================================

        RuleFor(x => x.Observaciones)
            .MaximumLength(1000)
            .WithMessage("Las observaciones no pueden exceder 1000 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Observaciones));
    }
}