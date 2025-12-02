namespace G2rismBeta.API.DTOs.Auth
{
    /// <summary>
    /// DTO de respuesta para renovación de tokens
    /// </summary>
    public class RefreshTokenResponseDto
    {
        /// <summary>
        /// Nuevo access token (JWT)
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// Nuevo refresh token (reemplaza al anterior)
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;

        /// <summary>
        /// Tiempo en segundos hasta que expire el access token (estándar OAuth 2.0)
        /// </summary>
        /// <example>3600</example>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Fecha de expiración del nuevo access token en UTC
        /// </summary>
        public DateTime TokenExpiration { get; set; }

        /// <summary>
        /// Fecha de expiración del nuevo access token en hora local del servidor
        /// </summary>
        public DateTime TokenExpirationLocal { get; set; }

        /// <summary>
        /// Zona horaria del servidor
        /// </summary>
        public string TimeZone { get; set; } = string.Empty;

        /// <summary>
        /// ID del usuario
        /// </summary>
        public int IdUsuario { get; set; }

        /// <summary>
        /// Username del usuario
        /// </summary>
        public string Username { get; set; } = string.Empty;
    }
}
