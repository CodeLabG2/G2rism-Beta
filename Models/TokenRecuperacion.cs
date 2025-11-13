using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace G2rismBeta.API.Models;

/// <summary>
/// Entidad para gestionar tokens de recuperación de contraseña y verificación
/// </summary>
[Table("tokens_recuperacion")]
public class TokenRecuperacion
{
    /// <summary>
    /// Identificador único del token
    /// </summary>
    [Key]
    [Column("id_token")]
    public int IdToken { get; set; }

    /// <summary>
    /// ID del usuario al que pertenece el token
    /// </summary>
    [Required(ErrorMessage = "El ID del usuario es obligatorio")]
    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    /// <summary>
    /// Token generado (GUID o hash único)
    /// </summary>
    [Required(ErrorMessage = "El token es obligatorio")]
    [StringLength(255, ErrorMessage = "El token no puede exceder 255 caracteres")]
    [Column("token")]
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de token
    /// Posibles valores: 'recuperacion_password', 'verificacion_email', 'activacion_cuenta'
    /// </summary>
    [Required(ErrorMessage = "El tipo de token es obligatorio")]
    [Column("tipo_token")]
    public string TipoToken { get; set; } = "recuperacion_password";

    /// <summary>
    /// Fecha y hora de generación del token
    /// </summary>
    [Column("fecha_generacion")]
    public DateTime FechaGeneracion { get; set; } = DateTime.Now;

    /// <summary>
    /// Fecha y hora de expiración del token
    /// Típicamente 1 hora después de la generación
    /// </summary>
    [Required(ErrorMessage = "La fecha de expiración es obligatoria")]
    [Column("fecha_expiracion")]
    public DateTime FechaExpiracion { get; set; }

    /// <summary>
    /// Indica si el token ya fue utilizado
    /// Un token solo puede usarse una vez
    /// </summary>
    [Column("usado")]
    public bool Usado { get; set; } = false;

    /// <summary>
    /// Fecha y hora en que se usó el token
    /// NULL si aún no se ha usado
    /// </summary>
    [Column("fecha_uso")]
    public DateTime? FechaUso { get; set; }

    /// <summary>
    /// IP desde la que se solicitó el token
    /// Para auditoría y seguridad
    /// </summary>
    [StringLength(45, ErrorMessage = "La IP no puede exceder 45 caracteres")]
    [Column("ip_solicitud")]
    public string? IpSolicitud { get; set; }

    // ========================================
    // RELACIONES DE NAVEGACIÓN
    // ========================================

    /// <summary>
    /// Usuario al que pertenece el token
    /// </summary>
    [ForeignKey("IdUsuario")]
    public virtual Usuario Usuario { get; set; } = null!;
}