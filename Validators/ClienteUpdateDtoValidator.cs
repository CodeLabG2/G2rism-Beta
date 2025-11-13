using FluentValidation;
using G2rismBeta.API.DTOs.Cliente;

namespace G2rismBeta.API.Validators;

/// <summary>
/// Validador para el DTO de actualización de clientes
/// Similar al de creación pero incluye validación del ID
/// </summary>
public class ClienteUpdateDtoValidator : AbstractValidator<ClienteUpdateDto>
{
    public ClienteUpdateDtoValidator()
    {
        // ========================================
        // VALIDACIÓN DEL ID DEL CLIENTE
        // ========================================

        RuleFor(x => x.IdCliente)
            .GreaterThan(0)
            .WithMessage("El ID del cliente debe ser mayor a 0");

        // ========================================
        // VALIDACIÓN DEL ID DE CATEGORÍA (OPCIONAL)
        // ========================================

        RuleFor(x => x.IdCategoria)
            .GreaterThan(0)
            .WithMessage("El ID de categoría debe ser mayor a 0")
            .When(x => x.IdCategoria.HasValue);

        // ========================================
        // VALIDACIÓN DEL NOMBRE
        // ========================================

        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio")
            .Length(2, 100)
            .WithMessage("El nombre debe tener entre 2 y 100 caracteres")
            .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$")
            .WithMessage("El nombre solo puede contener letras y espacios");

        // ========================================
        // VALIDACIÓN DEL APELLIDO
        // ========================================

        RuleFor(x => x.Apellido)
            .NotEmpty()
            .WithMessage("El apellido es obligatorio")
            .Length(2, 100)
            .WithMessage("El apellido debe tener entre 2 y 100 caracteres")
            .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$")
            .WithMessage("El apellido solo puede contener letras y espacios");

        // ========================================
        // VALIDACIÓN DEL DOCUMENTO DE IDENTIDAD
        // ========================================

        RuleFor(x => x.DocumentoIdentidad)
            .NotEmpty()
            .WithMessage("El documento de identidad es obligatorio")
            .MaximumLength(50)
            .WithMessage("El documento no puede exceder 50 caracteres")
            .Matches("^[a-zA-Z0-9-]+$")
            .WithMessage("El documento solo puede contener letras, números y guiones");

        // ========================================
        // VALIDACIÓN DEL TIPO DE DOCUMENTO
        // ========================================

        RuleFor(x => x.TipoDocumento)
            .NotEmpty()
            .WithMessage("El tipo de documento es obligatorio")
            .MaximumLength(10)
            .WithMessage("El tipo de documento no puede exceder 10 caracteres")
            .Must(tipo => new[] { "CC", "CE", "PA", "TI", "NIT", "PEP" }.Contains(tipo.ToUpper()))
            .WithMessage("El tipo de documento debe ser: CC (Cédula), CE (Cédula Extranjería), PA (Pasaporte), TI (Tarjeta Identidad), NIT, o PEP");

        // ========================================
        // VALIDACIÓN DE LA FECHA DE NACIMIENTO
        // ========================================

        RuleFor(x => x.FechaNacimiento)
            .NotEmpty()
            .WithMessage("La fecha de nacimiento es obligatoria")
            .LessThan(DateTime.Today)
            .WithMessage("La fecha de nacimiento debe ser anterior a hoy")
            .Must(BeAtLeast18YearsOld)
            .WithMessage("El cliente debe ser mayor de 18 años")
            .GreaterThan(DateTime.Today.AddYears(-120))
            .WithMessage("La fecha de nacimiento no puede ser mayor a 120 años");

        // ========================================
        // VALIDACIÓN DEL CORREO ELECTRÓNICO
        // ========================================

        RuleFor(x => x.CorreoElectronico)
            .NotEmpty()
            .WithMessage("El correo electrónico es obligatorio")
            .EmailAddress()
            .WithMessage("Formato de correo electrónico inválido")
            .MaximumLength(150)
            .WithMessage("El correo no puede exceder 150 caracteres");

        // ========================================
        // VALIDACIÓN DEL TELÉFONO
        // ========================================

        RuleFor(x => x.Telefono)
            .NotEmpty()
            .WithMessage("El teléfono es obligatorio")
            .MaximumLength(20)
            .WithMessage("El teléfono no puede exceder 20 caracteres")
            .Matches(@"^[\d\s\+\-\(\)]+$")
            .WithMessage("El teléfono solo puede contener números, espacios y los símbolos: + - ( )");

        // ========================================
        // VALIDACIÓN DE LA DIRECCIÓN (OPCIONAL)
        // ========================================

        RuleFor(x => x.Direccion)
            .MaximumLength(200)
            .WithMessage("La dirección no puede exceder 200 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Direccion));

        // ========================================
        // VALIDACIÓN DE LA CIUDAD
        // ========================================

        RuleFor(x => x.Ciudad)
            .NotEmpty()
            .WithMessage("La ciudad es obligatoria")
            .MaximumLength(100)
            .WithMessage("La ciudad no puede exceder 100 caracteres")
            .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$")
            .WithMessage("La ciudad solo puede contener letras y espacios");

        // ========================================
        // VALIDACIÓN DEL PAÍS
        // ========================================

        RuleFor(x => x.Pais)
            .NotEmpty()
            .WithMessage("El país es obligatorio")
            .MaximumLength(100)
            .WithMessage("El país no puede exceder 100 caracteres")
            .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$")
            .WithMessage("El país solo puede contener letras y espacios");
    }

    // ========================================
    // MÉTODOS DE VALIDACIÓN PERSONALIZADOS
    // ========================================

    /// <summary>
    /// Validar que el cliente sea mayor de 18 años
    /// </summary>
    private bool BeAtLeast18YearsOld(DateTime fechaNacimiento)
    {
        var hoy = DateTime.Today;
        var edad = hoy.Year - fechaNacimiento.Year;
        if (fechaNacimiento.Date > hoy.AddYears(-edad)) edad--;
        return edad >= 18;
    }
}
