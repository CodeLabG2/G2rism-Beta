using BCrypt.Net;

namespace G2rismBeta.API.Helpers;

/// <summary>
/// Helper para encriptar y verificar contraseñas usando BCrypt
/// BCrypt es un algoritmo de hash seguro diseñado específicamente para contraseñas
/// </summary>
public static class PasswordHasher
{
    /// <summary>
    /// Hashea una contraseña en texto plano usando BCrypt
    /// </summary>
    /// <param name="password">Contraseña en texto plano</param>
    /// <returns>Hash de la contraseña</returns>
    /// <example>
    /// string hashedPassword = PasswordHasher.HashPassword("MiPassword123!");
    /// </example>
    public static string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("La contraseña no puede estar vacía", nameof(password));
        }

        // BCrypt genera automáticamente un salt único
        // WorkFactor 11 es un buen balance entre seguridad y rendimiento
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 11);
    }

    /// <summary>
    /// Verifica si una contraseña en texto plano coincide con un hash
    /// </summary>
    /// <param name="password">Contraseña en texto plano a verificar</param>
    /// <param name="hashedPassword">Hash almacenado en la base de datos</param>
    /// <returns>True si la contraseña es correcta, False en caso contrario</returns>
    /// <example>
    /// bool esValida = PasswordHasher.VerifyPassword("MiPassword123!", usuario.PasswordHash);
    /// </example>
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("La contraseña no puede estar vacía", nameof(password));
        }

        if (string.IsNullOrWhiteSpace(hashedPassword))
        {
            throw new ArgumentException("El hash no puede estar vacío", nameof(hashedPassword));
        }

        try
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
        catch (SaltParseException)
        {
            // El hash no es válido o no fue generado con BCrypt
            return false;
        }
    }

    /// <summary>
    /// Valida que una contraseña cumple con los requisitos de seguridad
    /// </summary>
    /// <param name="password">Contraseña a validar</param>
    /// <returns>Tupla con (esValida, mensaje de error si aplica)</returns>
    public static (bool isValid, string? errorMessage) ValidatePasswordStrength(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return (false, "La contraseña es obligatoria");
        }

        if (password.Length < 8)
        {
            return (false, "La contraseña debe tener al menos 8 caracteres");
        }

        if (password.Length > 100)
        {
            return (false, "La contraseña no puede exceder 100 caracteres");
        }

        // Al menos una mayúscula
        if (!password.Any(char.IsUpper))
        {
            return (false, "La contraseña debe contener al menos una letra mayúscula");
        }

        // Al menos una minúscula
        if (!password.Any(char.IsLower))
        {
            return (false, "La contraseña debe contener al menos una letra minúscula");
        }

        // Al menos un número
        if (!password.Any(char.IsDigit))
        {
            return (false, "La contraseña debe contener al menos un número");
        }

        // Al menos un carácter especial
        var specialChars = "!@#$%^&*()_+-=[]{}|;:,.<>?";
        if (!password.Any(c => specialChars.Contains(c)))
        {
            return (false, "La contraseña debe contener al menos un carácter especial (!@#$%^&*...)");
        }

        return (true, null);
    }
}