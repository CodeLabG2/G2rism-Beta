using G2rismBeta.API.DTOs.Empleado;

namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interface del servicio de Empleados
/// Define la lógica de negocio para gestión de empleados y jerarquía organizacional
/// </summary>
public interface IEmpleadoService
{
    // ========================================
    // OPERACIONES CRUD BÁSICAS
    // ========================================

    /// <summary>
    /// Obtener todos los empleados
    /// </summary>
    Task<IEnumerable<EmpleadoResponseDto>> GetAllEmpleadosAsync();

    /// <summary>
    /// Obtener solo empleados activos
    /// </summary>
    Task<IEnumerable<EmpleadoResponseDto>> GetEmpleadosActivosAsync();

    /// <summary>
    /// Obtener un empleado por su ID
    /// </summary>
    Task<EmpleadoResponseDto?> GetEmpleadoByIdAsync(int idEmpleado);

    /// <summary>
    /// Obtener empleado con información completa (incluye jefe y subordinados)
    /// </summary>
    Task<EmpleadoResponseDto?> GetEmpleadoCompletoAsync(int idEmpleado);

    /// <summary>
    /// Crear un nuevo empleado
    /// </summary>
    Task<EmpleadoResponseDto> CreateEmpleadoAsync(EmpleadoCreateDto empleadoCreateDto);

    /// <summary>
    /// Actualizar un empleado existente
    /// </summary>
    Task<EmpleadoResponseDto> UpdateEmpleadoAsync(int idEmpleado, EmpleadoUpdateDto empleadoUpdateDto);

    /// <summary>
    /// Eliminar un empleado (solo si no tiene subordinados)
    /// Si tiene subordinados, primero deben ser reasignados
    /// </summary>
    Task<bool> DeleteEmpleadoAsync(int idEmpleado);

    // ========================================
    // OPERACIONES DE BÚSQUEDA
    // ========================================

    /// <summary>
    /// Buscar empleado por documento de identidad
    /// </summary>
    Task<EmpleadoResponseDto?> GetEmpleadoByDocumentoAsync(string documentoIdentidad);

    /// <summary>
    /// Buscar empleado por ID de usuario
    /// </summary>
    Task<EmpleadoResponseDto?> GetEmpleadoByUsuarioIdAsync(int idUsuario);

    /// <summary>
    /// Buscar empleados por nombre o apellido
    /// </summary>
    Task<IEnumerable<EmpleadoResponseDto>> BuscarEmpleadosPorNombreAsync(string termino);

    /// <summary>
    /// Obtener empleados por cargo
    /// </summary>
    Task<IEnumerable<EmpleadoResponseDto>> GetEmpleadosPorCargoAsync(string cargo);

    /// <summary>
    /// Obtener empleados por estado (Activo, Inactivo, Vacaciones, Licencia)
    /// </summary>
    Task<IEnumerable<EmpleadoResponseDto>> GetEmpleadosPorEstadoAsync(string estado);

    // ========================================
    // OPERACIONES DE JERARQUÍA
    // ========================================

    /// <summary>
    /// Obtener todos los subordinados directos de un empleado
    /// </summary>
    Task<IEnumerable<EmpleadoResponseDto>> GetSubordinadosDirectosAsync(int idJefe);

    /// <summary>
    /// Obtener todos los subordinados de un empleado (todos los niveles)
    /// </summary>
    Task<IEnumerable<EmpleadoResponseDto>> GetTodosLosSubordinadosAsync(int idJefe);

    /// <summary>
    /// Obtener el jefe directo de un empleado
    /// </summary>
    Task<EmpleadoResponseDto?> GetJefeDirectoAsync(int idEmpleado);

    /// <summary>
    /// Obtener la cadena de jefes hasta el nivel más alto
    /// </summary>
    Task<IEnumerable<EmpleadoResponseDto>> GetCadenaDeJefesAsync(int idEmpleado);

    /// <summary>
    /// Obtener empleados sin jefe (nivel más alto)
    /// </summary>
    Task<IEnumerable<EmpleadoResponseDto>> GetEmpleadosSinJefeAsync();

    /// <summary>
    /// Obtener empleados que son jefes (tienen subordinados)
    /// </summary>
    Task<IEnumerable<EmpleadoResponseDto>> GetEmpleadosQuesonJefesAsync();

    /// <summary>
    /// Obtener el organigrama completo de la empresa
    /// Estructura jerárquica completa
    /// </summary>
    Task<IEnumerable<EmpleadoResponseDto>> GetOrganigramaCompletoAsync();

    // ========================================
    // OPERACIONES DE GESTIÓN DE JERARQUÍA
    // ========================================

    /// <summary>
    /// Cambiar el jefe de un empleado
    /// Valida que no se creen ciclos en la jerarquía
    /// </summary>
    Task<bool> CambiarJefeAsync(int idEmpleado, int? idNuevoJefe);

    /// <summary>
    /// Reasignar subordinados de un jefe a otro
    /// Útil antes de eliminar o cambiar el cargo de un jefe
    /// </summary>
    Task<bool> ReasignarSubordinadosAsync(int idJefeActual, int? idNuevoJefe);

    /// <summary>
    /// Promover un empleado (cambiar cargo y posiblemente jefe)
    /// </summary>
    Task<bool> PromoverEmpleadoAsync(int idEmpleado, string nuevoCargo, int? idNuevoJefe = null, decimal? nuevoSalario = null);

    // ========================================
    // OPERACIONES ESPECIALES
    // ========================================

    /// <summary>
    /// Cambiar el estado de un empleado (Activo/Inactivo/Vacaciones/Licencia)
    /// </summary>
    Task<bool> CambiarEstadoEmpleadoAsync(int idEmpleado, string nuevoEstado);

    /// <summary>
    /// Actualizar el salario de un empleado
    /// (Operación separada por ser información sensible)
    /// </summary>
    Task<bool> ActualizarSalarioAsync(int idEmpleado, decimal nuevoSalario);

    // ========================================
    // VALIDACIONES
    // ========================================

    /// <summary>
    /// Validar si un documento de identidad ya existe
    /// </summary>
    Task<bool> DocumentoExisteAsync(string documentoIdentidad, int? idEmpleadoExcluir = null);

    /// <summary>
    /// Validar si un usuario ya tiene un empleado asociado
    /// </summary>
    Task<bool> UsuarioTieneEmpleadoAsync(int idUsuario, int? idEmpleadoExcluir = null);

    /// <summary>
    /// Validar si un empleado puede ser jefe de otro
    /// (Previene ciclos: A no puede ser jefe de B si B ya es jefe de A)
    /// </summary>
    Task<bool> PuedeSerJefeDeAsync(int idNuevoJefe, int idEmpleado);

    /// <summary>
    /// Validar si un empleado tiene subordinados
    /// </summary>
    Task<bool> TieneSubordinadosAsync(int idEmpleado);

    /// <summary>
    /// Validar si un empleado es jefe de otro (directo o indirecto)
    /// </summary>
    Task<bool> EsJefeDeAsync(int idPosibleJefe, int idEmpleado);

    // ========================================
    // ESTADÍSTICAS Y REPORTES
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
    /// Contar total de empleados activos
    /// </summary>
    Task<int> ContarEmpleadosActivosAsync();

    /// <summary>
    /// Obtener promedio de antigüedad de empleados
    /// </summary>
    Task<double> GetPromedioAntiguedadAsync();

    /// <summary>
    /// Obtener empleados con mayor antigüedad
    /// </summary>
    Task<IEnumerable<EmpleadoResponseDto>> GetEmpleadosConMayorAntiguedadAsync(int cantidad = 10);

    /// <summary>
    /// Contar cuántos subordinados directos tiene un empleado
    /// </summary>
    Task<int> ContarSubordinadosDirectosAsync(int idJefe);

    /// <summary>
    /// Contar cuántos subordinados totales tiene un empleado (todos los niveles)
    /// </summary>
    Task<int> ContarTodosLosSubordinadosAsync(int idJefe);
}