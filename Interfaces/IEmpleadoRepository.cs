using G2rismBeta.API.Models;

namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interface específica para operaciones con Empleados
/// Extiende el repositorio genérico y agrega métodos específicos para jerarquía organizacional
/// </summary>
public interface IEmpleadoRepository : IGenericRepository<Empleado>
{
    // ========================================
    // MÉTODOS DE BÚSQUEDA BÁSICA
    // ========================================

    /// <summary>
    /// Buscar empleado por documento de identidad
    /// </summary>
    Task<Empleado?> GetByDocumentoAsync(string documentoIdentidad);

    /// <summary>
    /// Buscar empleado por ID de usuario
    /// </summary>
    Task<Empleado?> GetByUsuarioIdAsync(int idUsuario);

    /// <summary>
    /// Obtener empleados por cargo
    /// </summary>
    Task<IEnumerable<Empleado>> GetEmpleadosPorCargoAsync(string cargo);

    /// <summary>
    /// Obtener empleados por estado (Activo, Inactivo, Vacaciones, Licencia)
    /// </summary>
    Task<IEnumerable<Empleado>> GetEmpleadosPorEstadoAsync(string estado);

    /// <summary>
    /// Buscar empleados por nombre o apellido
    /// </summary>
    Task<IEnumerable<Empleado>> BuscarPorNombreAsync(string termino);

    // ========================================
    // MÉTODOS CON RELACIONES (INCLUDE)
    // ========================================

    /// <summary>
    /// Obtener empleado con su usuario y jefe incluidos
    /// </summary>
    Task<Empleado?> GetEmpleadoConRelacionesAsync(int idEmpleado);

    /// <summary>
    /// Obtener empleado con todas sus relaciones (usuario, jefe, subordinados)
    /// Ideal para mostrar información completa del empleado
    /// </summary>
    Task<Empleado?> GetEmpleadoCompletoAsync(int idEmpleado);

    // ========================================
    // MÉTODOS DE JERARQUÍA ORGANIZACIONAL
    // ========================================

    /// <summary>
    /// Obtener todos los subordinados directos de un empleado (un nivel)
    /// </summary>
    Task<IEnumerable<Empleado>> GetSubordinadosDirectosAsync(int idJefe);

    /// <summary>
    /// Obtener todos los subordinados de un empleado (todos los niveles - recursivo)
    /// Incluye subordinados directos y sus subordinados
    /// </summary>
    Task<IEnumerable<Empleado>> GetTodosLosSubordinadosAsync(int idJefe);

    /// <summary>
    /// Obtener el jefe directo de un empleado
    /// </summary>
    Task<Empleado?> GetJefeDirectoAsync(int idEmpleado);

    /// <summary>
    /// Obtener toda la cadena de jefes hasta el nivel más alto (CEO)
    /// Retorna la lista de jefes en orden ascendente (jefe inmediato → CEO)
    /// </summary>
    Task<IEnumerable<Empleado>> GetCadenaDeJefesAsync(int idEmpleado);

    /// <summary>
    /// Obtener empleados que no tienen jefe (nivel más alto de la jerarquía)
    /// Generalmente retorna CEO, Gerente General, etc.
    /// </summary>
    Task<IEnumerable<Empleado>> GetEmpleadosSinJefeAsync();

    /// <summary>
    /// Obtener empleados que son jefes (tienen al menos un subordinado)
    /// </summary>
    Task<IEnumerable<Empleado>> GetEmpleadosQuesonJefesAsync();

    /// <summary>
    /// Contar cuántos subordinados directos tiene un empleado
    /// </summary>
    Task<int> ContarSubordinadosDirectosAsync(int idJefe);

    /// <summary>
    /// Contar cuántos subordinados totales tiene un empleado (todos los niveles)
    /// </summary>
    Task<int> ContarTodosLosSubordinadosAsync(int idJefe);

    /// <summary>
    /// Obtener el organigrama completo de la empresa
    /// Retorna todos los empleados con sus relaciones de jerarquía
    /// </summary>
    Task<IEnumerable<Empleado>> GetOrganigramaCompletoAsync();

    /// <summary>
    /// Verificar si un empleado es jefe de otro (directo o indirecto)
    /// Útil para validar operaciones
    /// </summary>
    Task<bool> EsJefeDeAsync(int idPosibleJefe, int idEmpleado);

    // ========================================
    // MÉTODOS DE VALIDACIÓN
    // ========================================

    /// <summary>
    /// Verificar si existe un documento de identidad
    /// </summary>
    Task<bool> ExisteDocumentoAsync(string documentoIdentidad, int? idEmpleadoExcluir = null);

    /// <summary>
    /// Verificar si un usuario ya tiene un empleado asociado
    /// </summary>
    Task<bool> UsuarioTieneEmpleadoAsync(int idUsuario, int? idEmpleadoExcluir = null);

    /// <summary>
    /// Verificar si un empleado puede ser asignado como jefe de otro
    /// (Previene ciclos en la jerarquía: A no puede ser jefe de B si B es jefe de A)
    /// </summary>
    Task<bool> PuedeSerJefeDeAsync(int idNuevoJefe, int idEmpleado);

    /// <summary>
    /// Verificar si un empleado tiene subordinados
    /// </summary>
    Task<bool> TieneSubordinadosAsync(int idEmpleado);

    // ========================================
    // MÉTODOS DE GESTIÓN DE JERARQUÍA
    // ========================================

    /// <summary>
    /// Reasignar todos los subordinados de un jefe a otro jefe
    /// Útil antes de eliminar o cambiar de cargo a un empleado
    /// </summary>
    Task<bool> ReasignarSubordinadosAsync(int idJefeActual, int? idNuevoJefe);

    /// <summary>
    /// Cambiar el jefe de un empleado
    /// </summary>
    Task<bool> CambiarJefeAsync(int idEmpleado, int? idNuevoJefe);

    /// <summary>
    /// Cambiar estado del empleado (Activo/Inactivo/Vacaciones/Licencia)
    /// </summary>
    Task<bool> CambiarEstadoAsync(int idEmpleado, string nuevoEstado);

    // ========================================
    // MÉTODOS ESTADÍSTICOS
    // ========================================

    /// <summary>
    /// Obtener estadísticas de empleados por cargo
    /// </summary>
    Task<Dictionary<string, int>> GetEstadisticasPorCargoAsync();

    /// <summary>
    /// Obtener estadísticas de empleados por estado
    /// </summary>
    Task<Dictionary<string, int>> GetEstadisticasPorEstadoAsync();

    /// <summary>
    /// Obtener el promedio de antigüedad en años de los empleados
    /// </summary>
    Task<double> GetPromedioAntiguedadAsync();

    /// <summary>
    /// Obtener empleados con mayor antigüedad
    /// </summary>
    Task<IEnumerable<Empleado>> GetEmpleadosConMayorAntiguedadAsync(int cantidad = 10);
}