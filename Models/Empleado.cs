using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace G2rismBeta.API.Models
{
    /// <summary>
    /// Representa un empleado de la agencia de turismo.
    /// Incluye jerarquía organizacional (jefe-subordinados) y datos sensibles como salario.
    /// </summary>
    [Table("empleados")]
    public class Empleado
    {
        #region Primary Key

        /// <summary>
        /// Identificador único del empleado
        /// </summary>
        [Key]
        [Column("id_empleado")]
        public int IdEmpleado { get; set; }

        #endregion

        #region Foreign Keys

        /// <summary>
        /// Referencia al usuario asociado (sistema de autenticación)
        /// </summary>
        [Required(ErrorMessage = "El usuario es obligatorio")]
        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        /// <summary>
        /// Referencia al empleado que es jefe directo (auto-referencia)
        /// Null si el empleado no tiene jefe (ej. CEO, Gerente General)
        /// </summary>
        [Column("id_jefe")]
        public int? IdJefe { get; set; }

        #endregion

        #region Personal Information

        /// <summary>
        /// Nombre del empleado
        /// </summary>
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Apellido del empleado
        /// </summary>
        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 100 caracteres")]
        [Column("apellido")]
        public string Apellido { get; set; } = string.Empty;

        /// <summary>
        /// Número de documento de identidad (único)
        /// </summary>
        [Required(ErrorMessage = "El documento de identidad es obligatorio")]
        [StringLength(20, ErrorMessage = "El documento no puede exceder 20 caracteres")]
        [Column("documento_identidad")]
        public string DocumentoIdentidad { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de documento: CC (Cédula), CE (Cédula Extranjería), Pasaporte, etc.
        /// </summary>
        [Required(ErrorMessage = "El tipo de documento es obligatorio")]
        [StringLength(20, ErrorMessage = "El tipo de documento no puede exceder 20 caracteres")]
        [Column("tipo_documento")]
        public string TipoDocumento { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de nacimiento del empleado
        /// </summary>
        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [Column("fecha_nacimiento", TypeName = "DATE")]
        public DateTime FechaNacimiento { get; set; }

        #endregion

        #region Contact Information

        /// <summary>
        /// Correo electrónico corporativo del empleado
        /// </summary>
        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
        [StringLength(100, ErrorMessage = "El correo no puede exceder 100 caracteres")]
        [Column("correo_electronico")]
        public string CorreoElectronico { get; set; } = string.Empty;

        /// <summary>
        /// Teléfono de contacto del empleado
        /// </summary>
        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
        [Column("telefono")]
        public string Telefono { get; set; } = string.Empty;

        #endregion

        #region Employment Information

        /// <summary>
        /// Cargo o posición del empleado en la empresa
        /// Ejemplos: Gerente, Vendedor, Contador, Asesor de Viajes, etc.
        /// </summary>
        [Required(ErrorMessage = "El cargo es obligatorio")]
        [StringLength(100, ErrorMessage = "El cargo no puede exceder 100 caracteres")]
        [Column("cargo")]
        public string Cargo { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de ingreso a la empresa
        /// </summary>
        [Required(ErrorMessage = "La fecha de ingreso es obligatoria")]
        [Column("fecha_ingreso", TypeName = "DATE")]
        public DateTime FechaIngreso { get; set; }

        /// <summary>
        /// Salario mensual del empleado (información sensible)
        /// </summary>
        [Required(ErrorMessage = "El salario es obligatorio")]
        [Column("salario", TypeName = "DECIMAL(10,2)")]
        [Range(0, 999999999.99, ErrorMessage = "El salario debe estar entre 0 y 999,999,999.99")]
        public decimal Salario { get; set; }

        #endregion

        #region Status

        /// <summary>
        /// Estado del empleado: Activo, Inactivo, Vacaciones, Licencia
        /// </summary>
        [Required(ErrorMessage = "El estado es obligatorio")]
        [StringLength(20, ErrorMessage = "El estado no puede exceder 20 caracteres")]
        [Column("estado")]
        public string Estado { get; set; } = "Activo";

        #endregion

        #region Navigation Properties

        /// <summary>
        /// Usuario asociado al empleado (para login y permisos)
        /// </summary>
        [ForeignKey("IdUsuario")]
        public Usuario? Usuario { get; set; }

        /// <summary>
        /// Jefe directo del empleado (auto-referencia)
        /// Null si el empleado no tiene jefe
        /// </summary>
        [ForeignKey("IdJefe")]
        public Empleado? Jefe { get; set; }

        /// <summary>
        /// Empleados que reportan a este empleado (subordinados)
        /// </summary>
        [InverseProperty("Jefe")]
        public ICollection<Empleado> Subordinados { get; set; } = new List<Empleado>();

        // TODO: Agregar en futuras fases
        // public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
        // public ICollection<Cotizacion> Cotizaciones { get; set; } = new List<Cotizacion>();

        #endregion

        #region Computed Properties

        /// <summary>
        /// Nombre completo del empleado (calculado)
        /// </summary>
        [NotMapped]
        public string NombreCompleto => $"{Nombre} {Apellido}";

        /// <summary>
        /// Edad del empleado (calculada automáticamente)
        /// </summary>
        [NotMapped]
        public int Edad
        {
            get
            {
                var hoy = DateTime.Today;
                var edad = hoy.Year - FechaNacimiento.Year;

                // Ajustar si aún no ha cumplido años este año
                if (FechaNacimiento.Date > hoy.AddYears(-edad))
                    edad--;

                return edad;
            }
        }

        /// <summary>
        /// Antigüedad en años del empleado (calculada automáticamente)
        /// </summary>
        [NotMapped]
        public int AntiguedadAnios
        {
            get
            {
                var hoy = DateTime.Today;
                var antiguedad = hoy.Year - FechaIngreso.Year;

                // Ajustar si aún no ha cumplido el aniversario este año
                if (FechaIngreso.Date > hoy.AddYears(-antiguedad))
                    antiguedad--;

                return antiguedad < 0 ? 0 : antiguedad;
            }
        }

        /// <summary>
        /// Antigüedad en meses del empleado (calculada automáticamente)
        /// </summary>
        [NotMapped]
        public int AntiguedadMeses
        {
            get
            {
                var hoy = DateTime.Today;
                var meses = ((hoy.Year - FechaIngreso.Year) * 12) + hoy.Month - FechaIngreso.Month;

                if (hoy.Day < FechaIngreso.Day)
                    meses--;

                return meses < 0 ? 0 : meses;
            }
        }

        /// <summary>
        /// Indica si el empleado tiene subordinados (es jefe)
        /// </summary>
        [NotMapped]
        public bool EsJefe => Subordinados?.Any() ?? false;

        /// <summary>
        /// Cantidad de subordinados directos
        /// </summary>
        [NotMapped]
        public int CantidadSubordinados => Subordinados?.Count ?? 0;

        #endregion
    }
}