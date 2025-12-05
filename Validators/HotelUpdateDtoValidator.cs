using FluentValidation;
using G2rismBeta.API.DTOs.Hotel;
using System.Text.Json;

namespace G2rismBeta.API.Validators;

/// <summary>
/// Validador para HotelUpdateDto
/// Todos los campos son opcionales pero deben ser válidos si se proporcionan
/// </summary>
public class HotelUpdateDtoValidator : AbstractValidator<HotelUpdateDto>
{
    public HotelUpdateDtoValidator()
    {
        // ID Proveedor
        RuleFor(x => x.IdProveedor)
            .GreaterThan(0)
            .WithMessage("El ID del proveedor debe ser mayor a 0")
            .When(x => x.IdProveedor.HasValue);

        // Nombre
        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre del hotel no puede estar vacío")
            .MaximumLength(200)
            .WithMessage("El nombre no puede exceder 200 caracteres")
            .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s\-&'.0-9]+$")
            .WithMessage("El nombre solo puede contener letras, números, espacios y caracteres: - & ' .")
            .When(x => !string.IsNullOrEmpty(x.Nombre));

        // Ciudad
        RuleFor(x => x.Ciudad)
            .NotEmpty()
            .WithMessage("La ciudad no puede estar vacía")
            .MaximumLength(100)
            .WithMessage("La ciudad no puede exceder 100 caracteres")
            .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s\-'.]+$")
            .WithMessage("La ciudad solo puede contener letras, espacios y caracteres: - ' .")
            .When(x => !string.IsNullOrEmpty(x.Ciudad));

        // País
        RuleFor(x => x.Pais)
            .MaximumLength(100)
            .WithMessage("El país no puede exceder 100 caracteres")
            .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s\-'.]+$")
            .WithMessage("El país solo puede contener letras, espacios y caracteres: - ' .")
            .When(x => !string.IsNullOrEmpty(x.Pais));

        // Dirección
        RuleFor(x => x.Direccion)
            .NotEmpty()
            .WithMessage("La dirección no puede estar vacía")
            .MaximumLength(500)
            .WithMessage("La dirección no puede exceder 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Direccion));

        // Contacto
        RuleFor(x => x.Contacto)
            .MaximumLength(200)
            .WithMessage("El contacto no puede exceder 200 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Contacto));

        // Descripción
        RuleFor(x => x.Descripcion)
            .MaximumLength(5000)
            .WithMessage("La descripción no puede exceder 5000 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Descripcion));

        // Categoría
        RuleFor(x => x.Categoria)
            .MaximumLength(50)
            .WithMessage("La categoría no puede exceder 50 caracteres")
            .Must(cat => cat == null || new[] { "economico", "estandar", "premium", "lujo" }.Contains(cat.ToLower()))
            .WithMessage("Categoría inválida. Valores permitidos: economico, estandar, premium, lujo")
            .When(x => !string.IsNullOrEmpty(x.Categoria));

        // Estrellas
        RuleFor(x => x.Estrellas)
            .InclusiveBetween(1, 5)
            .WithMessage("La clasificación debe estar entre 1 y 5 estrellas")
            .When(x => x.Estrellas.HasValue);

        // Precio por noche
        RuleFor(x => x.PrecioPorNoche)
            .GreaterThan(0)
            .WithMessage("El precio por noche debe ser mayor a 0")
            .LessThanOrEqualTo(9999999.99m)
            .WithMessage("El precio por noche no puede exceder 9,999,999.99")
            .When(x => x.PrecioPorNoche.HasValue);

        // Capacidad por habitación
        RuleFor(x => x.CapacidadPorHabitacion)
            .GreaterThan(0)
            .WithMessage("La capacidad por habitación debe ser mayor a 0")
            .LessThanOrEqualTo(20)
            .WithMessage("La capacidad por habitación no puede exceder 20 personas")
            .When(x => x.CapacidadPorHabitacion.HasValue);

        // Número de habitaciones
        RuleFor(x => x.NumeroHabitaciones)
            .GreaterThan(0)
            .WithMessage("El número de habitaciones debe ser mayor a 0")
            .LessThanOrEqualTo(10000)
            .WithMessage("El número de habitaciones no puede exceder 10,000")
            .When(x => x.NumeroHabitaciones.HasValue);

        // Políticas de cancelación
        RuleFor(x => x.PoliticasCancelacion)
            .MaximumLength(2000)
            .WithMessage("Las políticas de cancelación no pueden exceder 2000 caracteres")
            .When(x => !string.IsNullOrEmpty(x.PoliticasCancelacion));

        // Check-in hora
        RuleFor(x => x.CheckInHora)
            .Must(BeValidTimeFormat)
            .WithMessage("La hora de check-in debe tener formato HH:mm (ej: 14:00)")
            .When(x => !string.IsNullOrEmpty(x.CheckInHora));

        // Check-out hora
        RuleFor(x => x.CheckOutHora)
            .Must(BeValidTimeFormat)
            .WithMessage("La hora de check-out debe tener formato HH:mm (ej: 12:00)")
            .When(x => !string.IsNullOrEmpty(x.CheckOutHora));

        // Fotos (JSON)
        RuleFor(x => x.Fotos)
            .Must(BeValidJson)
            .WithMessage("Las fotos deben ser un array JSON válido (ej: [\"url1\", \"url2\"])")
            .When(x => !string.IsNullOrEmpty(x.Fotos));

        // Servicios incluidos (JSON)
        RuleFor(x => x.ServiciosIncluidos)
            .Must(BeValidJson)
            .WithMessage("Los servicios incluidos deben ser un array JSON válido (ej: [\"desayuno\", \"spa\"])")
            .When(x => !string.IsNullOrEmpty(x.ServiciosIncluidos));
    }

    /// <summary>
    /// Valida que una cadena tenga formato de hora HH:mm
    /// </summary>
    private bool BeValidTimeFormat(string? time)
    {
        if (string.IsNullOrEmpty(time))
            return true;

        return TimeSpan.TryParse(time, out _);
    }

    /// <summary>
    /// Valida que una cadena sea JSON válido
    /// </summary>
    private bool BeValidJson(string? json)
    {
        if (string.IsNullOrEmpty(json))
            return true;

        try
        {
            JsonDocument.Parse(json);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
