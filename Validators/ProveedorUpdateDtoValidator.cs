using FluentValidation;
using G2rismBeta.API.DTOs.Proveedor;

namespace G2rismBeta.API.Validators
{
    /// <summary>
    /// Validador para la actualización de proveedores
    /// Todos los campos son opcionales, pero si se proporcionan deben ser válidos
    /// </summary>
    public class ProveedorUpdateDtoValidator : AbstractValidator<ProveedorUpdateDto>
    {
        public ProveedorUpdateDtoValidator()
        {
            // ========================================
            // VALIDACIONES DE NOMBRE DE EMPRESA
            // ========================================
            RuleFor(x => x.NombreEmpresa)
                .Length(3, 100).WithMessage("El nombre debe tener entre 3 y 100 caracteres")
                .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ0-9\s\.\,\-&]+$")
                .WithMessage("El nombre contiene caracteres no permitidos")
                .When(x => !string.IsNullOrWhiteSpace(x.NombreEmpresa));

            // ========================================
            // VALIDACIONES DE NOMBRE DE CONTACTO
            // ========================================
            RuleFor(x => x.NombreContacto)
                .Length(3, 100).WithMessage("El nombre del contacto debe tener entre 3 y 100 caracteres")
                .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$")
                .WithMessage("El nombre del contacto solo debe contener letras y espacios")
                .When(x => !string.IsNullOrWhiteSpace(x.NombreContacto));

            // ========================================
            // VALIDACIONES DE TELÉFONOS
            // ========================================
            RuleFor(x => x.Telefono)
                .Length(7, 20).WithMessage("El teléfono debe tener entre 7 y 20 caracteres")
                .Matches(@"^[\d\s\+\-\(\)]+$")
                .WithMessage("El teléfono contiene caracteres no válidos")
                .When(x => !string.IsNullOrWhiteSpace(x.Telefono));

            RuleFor(x => x.TelefonoAlternativo)
                .Length(7, 20).WithMessage("El teléfono alternativo debe tener entre 7 y 20 caracteres")
                .Matches(@"^[\d\s\+\-\(\)]+$")
                .WithMessage("El teléfono alternativo contiene caracteres no válidos")
                .When(x => !string.IsNullOrWhiteSpace(x.TelefonoAlternativo));

            // ========================================
            // VALIDACIONES DE CORREOS ELECTRÓNICOS
            // ========================================
            RuleFor(x => x.CorreoElectronico)
                .EmailAddress().WithMessage("El formato del correo electrónico no es válido")
                .MaximumLength(100).WithMessage("El correo no puede exceder 100 caracteres")
                .When(x => !string.IsNullOrWhiteSpace(x.CorreoElectronico));

            RuleFor(x => x.CorreoAlternativo)
                .EmailAddress().WithMessage("El formato del correo alternativo no es válido")
                .MaximumLength(100).WithMessage("El correo alternativo no puede exceder 100 caracteres")
                .When(x => !string.IsNullOrWhiteSpace(x.CorreoAlternativo));

            // ========================================
            // VALIDACIONES DE UBICACIÓN
            // ========================================
            RuleFor(x => x.Direccion)
                .MaximumLength(200).WithMessage("La dirección no puede exceder 200 caracteres")
                .When(x => !string.IsNullOrWhiteSpace(x.Direccion));

            RuleFor(x => x.Ciudad)
                .Length(2, 50).WithMessage("La ciudad debe tener entre 2 y 50 caracteres")
                .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$")
                .WithMessage("La ciudad solo debe contener letras y espacios")
                .When(x => !string.IsNullOrWhiteSpace(x.Ciudad));

            RuleFor(x => x.Pais)
                .Length(2, 50).WithMessage("El país debe tener entre 2 y 50 caracteres")
                .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$")
                .WithMessage("El país solo debe contener letras y espacios")
                .When(x => !string.IsNullOrWhiteSpace(x.Pais));

            // ========================================
            // VALIDACIONES DE NIT/RUT
            // ========================================
            RuleFor(x => x.NitRut)
                .Length(5, 20).WithMessage("El NIT/RUT debe tener entre 5 y 20 caracteres")
                .Matches(@"^[\d\-]+$")
                .WithMessage("El NIT/RUT solo debe contener números y guiones")
                .When(x => !string.IsNullOrWhiteSpace(x.NitRut));
            // Nota: La validación de unicidad se hace en el servicio

            // ========================================
            // VALIDACIONES DE TIPO DE PROVEEDOR
            // ========================================
            RuleFor(x => x.TipoProveedor)
                .Must(tipo => new[] { "Hotel", "Aerolinea", "Transporte", "Servicios", "Mixto" }.Contains(tipo!))
                .WithMessage("El tipo de proveedor debe ser: Hotel, Aerolinea, Transporte, Servicios o Mixto")
                .When(x => !string.IsNullOrWhiteSpace(x.TipoProveedor));

            // ========================================
            // VALIDACIONES DE SITIO WEB
            // ========================================
            RuleFor(x => x.SitioWeb)
                .Must(BeAValidUrl)
                .WithMessage("El formato de la URL no es válido")
                .MaximumLength(200).WithMessage("La URL del sitio web no puede exceder 200 caracteres")
                .When(x => !string.IsNullOrWhiteSpace(x.SitioWeb));

            // ========================================
            // VALIDACIONES DE CALIFICACIÓN
            // ========================================
            RuleFor(x => x.Calificacion)
                .InclusiveBetween(0.0m, 5.0m)
                .WithMessage("La calificación debe estar entre 0.0 y 5.0")
                .When(x => x.Calificacion.HasValue);

            // ========================================
            // VALIDACIONES DE ESTADO
            // ========================================
            RuleFor(x => x.Estado)
                .Must(estado => new[] { "Activo", "Inactivo", "Suspendido" }.Contains(estado!))
                .WithMessage("El estado debe ser: Activo, Inactivo o Suspendido")
                .When(x => !string.IsNullOrWhiteSpace(x.Estado));

            // ========================================
            // VALIDACIONES DE OBSERVACIONES
            // ========================================
            RuleFor(x => x.Observaciones)
                .MaximumLength(1000).WithMessage("Las observaciones no pueden exceder 1000 caracteres")
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
}