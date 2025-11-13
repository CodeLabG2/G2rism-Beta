using System.ComponentModel.DataAnnotations;

namespace G2rismBeta.API.DTOs.Rol;

/// <summary>
/// DTO para crear un nuevo rol
/// Solo incluye los campos que el cliente puede enviar
/// </summary>
public class RolCreateDto
{
    /// <summary>
    /// Nombre del rol
    /// </summary>
    /// <example>Administrador</example>
    [Required(ErrorMessage = "El nombre del rol es obligatorio")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Descripción del rol
    /// </summary>
    /// <example>Usuario con acceso total al sistema</example>
    [StringLength(200, ErrorMessage = "La descripción no puede exceder 200 caracteres")]
    public string? Descripcion { get; set; }

    /// <summary>
    /// Nivel de acceso del rol (1-100)
    /// Valores menores = mayor nivel de acceso
    /// </summary>
    /// <example>1</example>
    [Range(1, 100, ErrorMessage = "El nivel de acceso debe estar entre 1 y 100")]
    public int NivelAcceso { get; set; } = 10;

    /// <summary>
    /// Estado del rol (true = activo, false = inactivo)
    /// </summary>
    /// <example>true</example>
    public bool Estado { get; set; } = true;
}
