using System.ComponentModel.DataAnnotations;

namespace G2rismBeta.API.DTOs.Auth;

/// <summary>
/// DTO para el registro de nuevos usuarios (clientes)
/// </summary>
public class RegisterRequestDto
{
    /// <summary>
    /// Nombre de usuario único
    /// </summary>
    /// <example>juanperez</example>
    [Required(ErrorMessage = "El username es obligatorio")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El username debe tener entre 3 y 50 caracteres")]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "El username solo puede contener letras, números y guiones bajos")]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico único
    /// </summary>
    /// <example>juan.perez@gmail.com</example>
    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    [StringLength(100, ErrorMessage = "El email no puede exceder 100 caracteres")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Contraseña del usuario
    /// Debe cumplir con requisitos de seguridad
    /// </summary>
    /// <example>MiPassword123!</example>
    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre 8 y 100 caracteres")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Confirmación de la contraseña (debe coincidir con Password)
    /// </summary>
    /// <example>MiPassword123!</example>
    [Required(ErrorMessage = "Debe confirmar la contraseña")]
    [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
    public string ConfirmPassword { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del usuario (opcional durante registro)
    /// </summary>
    [StringLength(50, ErrorMessage = "El nombre no puede exceder 50 caracteres")]
    public string? Nombre { get; set; }

    /// <summary>
    /// Apellido del usuario (opcional durante registro)
    /// </summary>
    [StringLength(50, ErrorMessage = "El apellido no puede exceder 50 caracteres")]
    public string? Apellido { get; set; }

    /// <summary>
    /// Tipo de usuario a crear (por defecto: cliente)
    /// </summary>
    /// <example>cliente</example>
    public string TipoUsuario { get; set; } = "cliente";

    /// <summary>
    /// Acepta términos y condiciones
    /// </summary>
    [Range(typeof(bool), "true", "true", ErrorMessage = "Debe aceptar los términos y condiciones")]
    public bool AceptaTerminos { get; set; }
}