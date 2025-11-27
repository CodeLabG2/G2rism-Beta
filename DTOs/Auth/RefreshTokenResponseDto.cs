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
        /// Fecha de expiración del nuevo access token
        /// </summary>
        public DateTime TokenExpiration { get; set; }

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
