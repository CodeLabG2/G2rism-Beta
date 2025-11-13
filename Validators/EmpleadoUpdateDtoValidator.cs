using FluentValidation;
using G2rismBeta.API.DTOs.Empleado;
using G2rismBeta.API.Interfaces;

namespace G2rismBeta.API.Validators
{
    /// <summary>
    /// Validador para la actualización de empleados
    /// Los campos son opcionales, pero si se proporcionan deben cumplir las reglas de validación
    /// </summary>
    public class EmpleadoUpdateDtoValidator : AbstractValidator<EmpleadoUpdateDto>
    {
        private readonly IEmpleadoRepository _empleadoRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public EmpleadoUpdateDtoValidator(
            IEmpleadoRepository empleadoRepository,
            IUsuarioRepository usuarioRepository)
        {
            _empleadoRepository = empleadoRepository;
            _usuarioRepository = usuarioRepository;

            // ========================================
            // VALIDACIONES DE ID_USUARIO (OPCIONAL)
            // ========================================

            When(x => x.IdUsuario.HasValue, () =>
            {
                RuleFor(x => x.IdUsuario!.Value)
                    .GreaterThan(0)
                    .WithMessage("El ID de usuario debe ser mayor a 0")
                    .MustAsync(UsuarioExiste)
                    .WithMessage("No existe un usuario con el ID especificado");

                // Nota: La validación de "usuario único" se hace en el Service
                // porque necesita el ID del empleado que se está actualizando
            });

            // ========================================
            // VALIDACIONES DE ID_JEFE (OPCIONAL)
            // ========================================

            When(x => x.IdJefe.HasValue, () =>
            {
                RuleFor(x => x.IdJefe!.Value)
                    .GreaterThan(0)
                    .WithMessage("El ID de jefe debe ser mayor a 0")
                    .MustAsync(JefeExiste)
                    .WithMessage("No existe un empleado con el ID de jefe especificado");

                // Nota: La validación de "no crear ciclos" se hace en el Service
            });

            // ========================================
            // VALIDACIONES DE NOMBRE (OPCIONAL)
            // ========================================

            When(x => !string.IsNullOrEmpty(x.Nombre), () =>
            {
                RuleFor(x => x.Nombre)
                    .MinimumLength(2)
                    .WithMessage("El nombre debe tener al menos 2 caracteres")
                    .MaximumLength(100)
                    .WithMessage("El nombre no puede exceder 100 caracteres")
                    .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$")
                    .WithMessage("El nombre solo puede contener letras y espacios");
            });

            // ========================================
            // VALIDACIONES DE APELLIDO (OPCIONAL)
            // ========================================

            When(x => !string.IsNullOrEmpty(x.Apellido), () =>
            {
                RuleFor(x => x.Apellido)
                    .MinimumLength(2)
                    .WithMessage("El apellido debe tener al menos 2 caracteres")
                    .MaximumLength(100)
                    .WithMessage("El apellido no puede exceder 100 caracteres")
                    .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$")
                    .WithMessage("El apellido solo puede contener letras y espacios");
            });

            // ========================================
            // VALIDACIONES DE DOCUMENTO DE IDENTIDAD (OPCIONAL)
            // ========================================

            When(x => !string.IsNullOrEmpty(x.DocumentoIdentidad), () =>
            {
                RuleFor(x => x.DocumentoIdentidad)
                    .MinimumLength(6)
                    .WithMessage("El documento debe tener al menos 6 caracteres")
                    .MaximumLength(20)
                    .WithMessage("El documento no puede exceder 20 caracteres")
                    .Matches(@"^[a-zA-Z0-9-]+$")
                    .WithMessage("El documento solo puede contener letras, números y guiones");

                // Nota: La validación de "documento único" se hace en el Service
                // porque necesita el ID del empleado que se está actualizando
            });

            // ========================================
            // VALIDACIONES DE TIPO DE DOCUMENTO (OPCIONAL)
            // ========================================

            When(x => !string.IsNullOrEmpty(x.TipoDocumento), () =>
            {
                RuleFor(x => x.TipoDocumento)
                    .Must(TipoDocumentoValido!)
                    .WithMessage("Tipo de documento inválido. Valores permitidos: CC, TI, CE, PASAPORTE, NIT");
            });

            // ========================================
            // VALIDACIONES DE FECHA DE NACIMIENTO (OPCIONAL)
            // ========================================

            When(x => x.FechaNacimiento.HasValue, () =>
            {
                RuleFor(x => x.FechaNacimiento!.Value)
                    .LessThan(DateTime.Now)
                    .WithMessage("La fecha de nacimiento no puede ser una fecha futura")
                    .Must(EdadMinima18Años)
                    .WithMessage("El empleado debe ser mayor de 18 años")
                    .Must(EdadMaxima80Años)
                    .WithMessage("La fecha de nacimiento indica una edad mayor a 80 años, por favor verifique");
            });

            // ========================================
            // VALIDACIONES DE CORREO ELECTRÓNICO (OPCIONAL)
            // ========================================

            When(x => !string.IsNullOrEmpty(x.CorreoElectronico), () =>
            {
                RuleFor(x => x.CorreoElectronico)
                    .EmailAddress()
                    .WithMessage("El formato del correo electrónico no es válido")
                    .MaximumLength(100)
                    .WithMessage("El correo electrónico no puede exceder 100 caracteres");
            });

            // ========================================
            // VALIDACIONES DE TELÉFONO (OPCIONAL)
            // ========================================

            When(x => !string.IsNullOrEmpty(x.Telefono), () =>
            {
                RuleFor(x => x.Telefono)
                    .MinimumLength(7)
                    .WithMessage("El teléfono debe tener al menos 7 dígitos")
                    .MaximumLength(20)
                    .WithMessage("El teléfono no puede exceder 20 caracteres")
                    .Matches(@"^[\d\s\-\+\(\)]+$")
                    .WithMessage("El teléfono solo puede contener números, espacios, guiones, + y paréntesis");
            });

            // ========================================
            // VALIDACIONES DE CARGO (OPCIONAL)
            // ========================================

            When(x => !string.IsNullOrEmpty(x.Cargo), () =>
            {
                RuleFor(x => x.Cargo)
                    .MinimumLength(2)
                    .WithMessage("El cargo debe tener al menos 2 caracteres")
                    .MaximumLength(100)
                    .WithMessage("El cargo no puede exceder 100 caracteres");
            });

            // ========================================
            // VALIDACIONES DE FECHA DE INGRESO (OPCIONAL)
            // ========================================

            When(x => x.FechaIngreso.HasValue, () =>
            {
                RuleFor(x => x.FechaIngreso!.Value)
                    .LessThanOrEqualTo(DateTime.Now)
                    .WithMessage("La fecha de ingreso no puede ser una fecha futura");
            });

            // Validación cruzada: Fecha de ingreso coherente con edad
            When(x => x.FechaNacimiento.HasValue && x.FechaIngreso.HasValue, () =>
            {
                RuleFor(x => x)
                    .Must(dto => FechaIngresoCoherenteConEdad(dto.FechaNacimiento!.Value, dto.FechaIngreso!.Value))
                    .WithMessage("La fecha de ingreso debe ser posterior a los 15 años de edad");
            });

            // ========================================
            // VALIDACIONES DE SALARIO (OPCIONAL)
            // ========================================

            When(x => x.Salario.HasValue, () =>
            {
                RuleFor(x => x.Salario!.Value)
                    .GreaterThan(0)
                    .WithMessage("El salario debe ser mayor a cero")
                    .GreaterThanOrEqualTo(1300000)
                    .WithMessage("El salario debe ser al menos el salario mínimo legal vigente (COP $1,300,000)")
                    .LessThanOrEqualTo(100000000)
                    .WithMessage("El salario no puede exceder COP $100,000,000");
            });

            // ========================================
            // VALIDACIONES DE ESTADO (OPCIONAL)
            // ========================================

            When(x => !string.IsNullOrEmpty(x.Estado), () =>
            {
                RuleFor(x => x.Estado)
                    .Must(EstadoValido!)
                    .WithMessage("Estado inválido. Valores permitidos: activo, inactivo, vacaciones, licencia");
            });
        }

        // ========================================
        // MÉTODOS DE VALIDACIÓN ASÍNCRONOS
        // ========================================

        /// <summary>
        /// Validar que el usuario existe en la base de datos
        /// </summary>
        private async Task<bool> UsuarioExiste(int idUsuario, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(idUsuario);
            return usuario != null;
        }

        /// <summary>
        /// Validar que el jefe existe en la base de datos
        /// </summary>
        private async Task<bool> JefeExiste(int idJefe, CancellationToken cancellationToken)
        {
            var jefe = await _empleadoRepository.GetByIdAsync(idJefe);
            return jefe != null;
        }

        // ========================================
        // MÉTODOS DE VALIDACIÓN SÍNCRONOS
        // ========================================

        /// <summary>
        /// Validar que el tipo de documento sea uno de los valores permitidos
        /// </summary>
        private bool TipoDocumentoValido(string tipoDocumento)
        {
            var tiposValidos = new[] { "CC", "TI", "CE", "PASAPORTE", "NIT" };
            return tiposValidos.Contains(tipoDocumento.ToUpper());
        }

        /// <summary>
        /// Validar que el empleado tenga al menos 18 años de edad
        /// </summary>
        private bool EdadMinima18Años(DateTime fechaNacimiento)
        {
            var edad = DateTime.Now.Year - fechaNacimiento.Year;
            if (fechaNacimiento > DateTime.Now.AddYears(-edad))
                edad--;

            return edad >= 18;
        }

        /// <summary>
        /// Validar que la fecha de nacimiento no indique una edad mayor a 80 años
        /// (Prevención de errores de digitación)
        /// </summary>
        private bool EdadMaxima80Años(DateTime fechaNacimiento)
        {
            var edad = DateTime.Now.Year - fechaNacimiento.Year;
            if (fechaNacimiento > DateTime.Now.AddYears(-edad))
                edad--;

            return edad <= 80;
        }

        /// <summary>
        /// Validar que la fecha de ingreso sea coherente con la edad
        /// (No puede ingresar antes de los 15 años)
        /// </summary>
        private bool FechaIngresoCoherenteConEdad(DateTime fechaNacimiento, DateTime fechaIngreso)
        {
            var edadAlIngreso = fechaIngreso.Year - fechaNacimiento.Year;
            if (fechaNacimiento > fechaIngreso.AddYears(-edadAlIngreso))
                edadAlIngreso--;

            return edadAlIngreso >= 15; // Edad mínima legal para trabajar
        }

        /// <summary>
        /// Validar que el estado sea uno de los valores permitidos
        /// </summary>
        private bool EstadoValido(string estado)
        {
            var estadosValidos = new[] { "activo", "inactivo", "vacaciones", "licencia" };
            return estadosValidos.Contains(estado.ToLower());
        }
    }
}