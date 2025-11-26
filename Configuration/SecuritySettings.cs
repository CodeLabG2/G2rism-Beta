namespace G2rismBeta.API.Configuration;

/// <summary>
/// Configuración de seguridad de la aplicación
/// </summary>
public class SecuritySettings
{
    /// <summary>
    /// Lista de URLs permitidas para el frontend (whitelist)
    /// Previene ataques de phishing mediante redirección a sitios maliciosos
    /// </summary>
    public List<string> AllowedFrontendUrls { get; set; } = new();
}
