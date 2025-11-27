using System.ComponentModel.DataAnnotations;

namespace G2rismBeta.API.DTOs.Auth
{
    /// <summary>
    /// DTO para solicitar renovación de access token usando refresh token
    /// </summary>
    public class RefreshTokenRequestDto
    {
        /// <summary>
        /// Refresh token válido que se obtuvo en el login
        /// </summary>
        [Required(ErrorMessage = "El refresh token es requerido")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
