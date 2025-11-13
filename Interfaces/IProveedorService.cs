using G2rismBeta.API.DTOs.Proveedor;

namespace G2rismBeta.API.Interfaces
{
    /// <summary>
    /// Interfaz del servicio de Proveedores
    /// Define la lógica de negocio para la gestión de proveedores
    /// </summary>
    public interface IProveedorService
    {
        // ========================================
        // OPERACIONES CRUD
        // ========================================

        /// <summary>
        /// Obtener todos los proveedores
        /// </summary>
        Task<IEnumerable<ProveedorResponseDto>> GetAllAsync();

        /// <summary>
        /// Obtener un proveedor por ID
        /// </summary>
        Task<ProveedorResponseDto?> GetByIdAsync(int id);

        /// <summary>
        /// Crear un nuevo proveedor
        /// </summary>
        Task<ProveedorResponseDto> CreateAsync(ProveedorCreateDto dto);

        /// <summary>
        /// Actualizar un proveedor existente
        /// </summary>
        Task<ProveedorResponseDto> UpdateAsync(int id, ProveedorUpdateDto dto);

        /// <summary>
        /// Eliminar un proveedor
        /// </summary>
        Task<bool> DeleteAsync(int id);

        // ========================================
        // BÚSQUEDAS Y FILTROS
        // ========================================

        /// <summary>
        /// Buscar proveedor por NIT/RUT
        /// </summary>
        Task<ProveedorResponseDto?> GetByNitRutAsync(string nitRut);

        /// <summary>
        /// Buscar proveedores por nombre
        /// </summary>
        Task<IEnumerable<ProveedorResponseDto>> SearchByNombreAsync(string nombre);

        /// <summary>
        /// Obtener proveedores por tipo
        /// </summary>
        Task<IEnumerable<ProveedorResponseDto>> GetByTipoAsync(string tipo);

        /// <summary>
        /// Obtener proveedores activos
        /// </summary>
        Task<IEnumerable<ProveedorResponseDto>> GetActivosAsync();

        /// <summary>
        /// Obtener proveedores por ciudad
        /// </summary>
        Task<IEnumerable<ProveedorResponseDto>> GetByCiudadAsync(string ciudad);

        /// <summary>
        /// Obtener proveedores por calificación mínima
        /// </summary>
        Task<IEnumerable<ProveedorResponseDto>> GetByCalificacionMinimaAsync(decimal calificacionMinima);

        // ========================================
        // GESTIÓN DE ESTADO Y CALIFICACIÓN
        // ========================================

        /// <summary>
        /// Cambiar estado de un proveedor
        /// </summary>
        Task<ProveedorResponseDto> CambiarEstadoAsync(int id, string nuevoEstado);

        /// <summary>
        /// Actualizar calificación de un proveedor
        /// </summary>
        Task<ProveedorResponseDto> ActualizarCalificacionAsync(int id, decimal nuevaCalificacion);

        // ========================================
        // ESTADÍSTICAS Y REPORTES
        // ========================================

        /// <summary>
        /// Obtener top proveedores mejor calificados
        /// </summary>
        Task<IEnumerable<ProveedorResponseDto>> GetTopProveedoresAsync(int cantidad = 10);

        /// <summary>
        /// Obtener estadísticas de proveedores por tipo
        /// </summary>
        Task<Dictionary<string, int>> GetEstadisticasPorTipoAsync();

        /// <summary>
        /// Obtener proveedores con contratos vigentes
        /// </summary>
        Task<IEnumerable<ProveedorResponseDto>> GetConContratosVigentesAsync();
    }
}