using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace G2rismBeta.API.Models
{
    /// <summary>
    /// Modelo para almacenar refresh tokens de los usuarios
    /// Permite renovar access tokens sin requerir login nuevamente
    /// </summary>
    [Table("refresh_tokens")]
    public class RefreshToken
    {
        /// <summary>
        /// ID único del refresh token
        /// </summary>
        [Key]
        [Column("id_refresh_token")]
        public int IdRefreshToken { get; set; }

        /// <summary>
        /// ID del usuario al que pertenece este refresh token
        /// </summary>
        [Required]
        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        /// <summary>
        /// Token de actualización (string aleatorio seguro de 64 bytes en base64)
        /// </summary>
        [Required]
        [MaxLength(128)]
        [Column("token")]
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de creación del refresh token
        /// </summary>
        [Required]
        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Fecha de expiración del refresh token (típicamente 7 días)
        /// </summary>
        [Required]
        [Column("fecha_expiracion")]
        public DateTime FechaExpiracion { get; set; }

        /// <summary>
        /// Indica si el token ha sido revocado manualmente (por logout o seguridad)
        /// </summary>
        [Required]
        [Column("revocado")]
        public bool Revocado { get; set; } = false;

        /// <summary>
        /// Fecha en que el token fue revocado (nullable)
        /// </summary>
        [Column("fecha_revocacion")]
        public DateTime? FechaRevocacion { get; set; }

        /// <summary>
        /// IP desde la cual se creó el refresh token (para auditoría)
        /// </summary>
        [MaxLength(45)]
        [Column("ip_creacion")]
        public string? IpCreacion { get; set; }

        /// <summary>
        /// User Agent del cliente que creó el token (para identificar dispositivo)
        /// </summary>
        [MaxLength(500)]
        [Column("user_agent")]
        public string? UserAgent { get; set; }

        /// <summary>
        /// Token que reemplazó a este (cuando se rota el refresh token)
        /// </summary>
        [MaxLength(128)]
        [Column("reemplazado_por")]
        public string? ReemplazadoPor { get; set; }

        // Navegación
        /// <summary>
        /// Usuario al que pertenece este refresh token
        /// </summary>
        [ForeignKey(nameof(IdUsuario))]
        public Usuario? Usuario { get; set; }

        // Propiedades calculadas (no mapeadas a BD)

        /// <summary>
        /// Indica si el refresh token está activo (no expirado y no revocado)
        /// </summary>
        [NotMapped]
        public bool EstaActivo => !Revocado && DateTime.UtcNow < FechaExpiracion;

        /// <summary>
        /// Indica si el refresh token ha expirado
        /// </summary>
        [NotMapped]
        public bool HaExpirado => DateTime.UtcNow >= FechaExpiracion;
    }
}
