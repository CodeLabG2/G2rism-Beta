using System.Security.Cryptography;

namespace G2rismBeta.API.Helpers;

/// <summary>
/// Helper para generar tokens seguros
/// </summary>
public static class TokenGenerator
{
    /// <summary>
    /// Genera un token aleatorio seguro usando GUID
    /// </summary>
    /// <returns>Token único como string</returns>
    public static string GenerateToken()
    {
        return Guid.NewGuid().ToString("N"); // "N" = sin guiones
    }

    /// <summary>
    /// Genera un token seguro más complejo usando RandomNumberGenerator
    /// </summary>
    /// <param name="length">Longitud del token en bytes (default: 32)</param>
    /// <returns>Token en formato Base64</returns>
    public static string GenerateSecureToken(int length = 32)
    {
        var tokenBytes = new byte[length];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(tokenBytes);
        }
        return Convert.ToBase64String(tokenBytes);
    }

    /// <summary>
    /// Genera un código numérico aleatorio (útil para verificación por SMS)
    /// </summary>
    /// <param name="length">Cantidad de dígitos (default: 6)</param>
    /// <returns>Código numérico como string</returns>
    public static string GenerateNumericCode(int length = 6)
    {
        if (length < 4 || length > 10)
        {
            throw new ArgumentException("La longitud debe estar entre 4 y 10", nameof(length));
        }

        var random = new Random();
        var min = (int)Math.Pow(10, length - 1);
        var max = (int)Math.Pow(10, length) - 1;
        return random.Next(min, max).ToString();
    }
}