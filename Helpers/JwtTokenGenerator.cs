using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace G2rismBeta.API.Helpers
{
    /// <summary>
    /// Helper para generar y validar tokens JWT
    /// </summary>
    public class JwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Genera un access token JWT para el usuario con sus roles y permisos
        /// </summary>
        /// <param name="idUsuario">ID del usuario</param>
        /// <param name="username">Nombre de usuario</param>
        /// <param name="email">Email del usuario</param>
        /// <param name="tipoUsuario">Tipo de usuario (empleado/cliente)</param>
        /// <param name="roles">Lista de roles del usuario</param>
        /// <param name="permisos">Lista de permisos del usuario</param>
        /// <returns>Token JWT firmado</returns>
        public string GenerateAccessToken(
            int idUsuario,
            string username,
            string email,
            string tipoUsuario,
            IEnumerable<string> roles,
            IEnumerable<string> permisos)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, idUsuario.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Email, email),
                new Claim("tipo_usuario", tipoUsuario),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // ID único del token
            };

            // Agregar roles como claims
            foreach (var rol in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            // Agregar permisos como claims
            foreach (var permiso in permisos)
            {
                claims.Add(new Claim("permission", permiso));
            }

            var secretKey = _configuration["Jwt:SecretKey"]
                ?? throw new InvalidOperationException("JWT SecretKey no está configurada en appsettings.json");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expirationMinutes = int.Parse(_configuration["Jwt:AccessTokenExpirationMinutes"] ?? "60");
            var expiration = DateTime.UtcNow.AddMinutes(expirationMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Genera un refresh token seguro aleatorio
        /// </summary>
        /// <returns>Refresh token como string base64 URL-safe</returns>
        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            // Convertir a base64 URL-safe (reemplazar caracteres problemáticos)
            return Convert.ToBase64String(randomBytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }

        /// <summary>
        /// Valida un token JWT y extrae los claims
        /// </summary>
        /// <param name="token">Token JWT a validar</param>
        /// <returns>ClaimsPrincipal si el token es válido, null si es inválido</returns>
        public ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                var secretKey = _configuration["Jwt:SecretKey"]
                    ?? throw new InvalidOperationException("JWT SecretKey no está configurada");

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(secretKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // No tolerancia para expiración
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                // Verificar que sea un JWT válido con algoritmo correcto
                if (validatedToken is JwtSecurityToken jwtToken &&
                    jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return principal;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Extrae el ID de usuario de un token JWT sin validar completamente
        /// (útil para refresh tokens cuando el access token ya expiró)
        /// </summary>
        /// <param name="expiredToken">Token JWT expirado</param>
        /// <returns>ID del usuario o null si no se puede extraer</returns>
        public int? GetUserIdFromExpiredToken(string expiredToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(expiredToken);

                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    return userId;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
