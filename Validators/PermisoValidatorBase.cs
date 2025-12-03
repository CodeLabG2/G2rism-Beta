using FluentValidation;

namespace G2rismBeta.API.Validators;

/// <summary>
/// Clase base abstracta que contiene las validaciones comunes para Permisos
/// </summary>
public abstract class PermisoValidatorBase
{
    /// <summary>
    /// Lista de acciones válidas para permisos
    /// </summary>
    protected static readonly string[] AccionesValidas =
    {
        "Crear", "Leer", "Actualizar", "Eliminar", "Listar", "Exportar", "Importar"
    };

    /// <summary>
    /// Validación personalizada: verifica si la acción es válida
    /// </summary>
    public static bool EsAccionValida(string? accion)
    {
        if (string.IsNullOrEmpty(accion))
            return true;

        return AccionesValidas.Any(a => a.Equals(accion, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Aplica las reglas de validación para el campo Módulo
    /// </summary>
    public static void ConfigurarValidacionModulo<T>(IRuleBuilder<T, string> ruleBuilder, bool esRequerido = true)
    {
        if (esRequerido)
        {
            ruleBuilder
                .NotEmpty()
                .WithMessage("El módulo es obligatorio");
        }

        ruleBuilder
            .Length(3, 50)
            .WithMessage("El módulo debe tener entre 3 y 50 caracteres")
            .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$")
            .WithMessage("El módulo solo puede contener letras y espacios");
    }

    /// <summary>
    /// Aplica las reglas de validación para el campo Acción
    /// </summary>
    public static void ConfigurarValidacionAccion<T>(IRuleBuilder<T, string> ruleBuilder, bool esRequerido = true)
    {
        if (esRequerido)
        {
            ruleBuilder
                .NotEmpty()
                .WithMessage("La acción es obligatoria");
        }

        ruleBuilder
            .Length(3, 50)
            .WithMessage("La acción debe tener entre 3 y 50 caracteres")
            .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$")
            .WithMessage("La acción solo puede contener letras y espacios")
            .Must(EsAccionValida)
            .WithMessage($"La acción debe ser una de: {string.Join(", ", AccionesValidas)}");
    }

    /// <summary>
    /// Aplica las reglas de validación para el campo Descripción
    /// </summary>
    public static void ConfigurarValidacionDescripcion<T>(IRuleBuilder<T, string?> ruleBuilder)
    {
        ruleBuilder
            .MaximumLength(200)
            .WithMessage("La descripción no puede exceder 200 caracteres");
    }
}
