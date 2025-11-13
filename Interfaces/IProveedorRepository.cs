using G2rismBeta.API.Models;

namespace G2rismBeta.API.Interfaces
{
    /// <summary>
    /// Interfaz del repositorio de Proveedores
    /// Define las operaciones de acceso a datos para proveedores
    /// </summary>
    public interface IProveedorRepository : IGenericRepository<Proveedor>
    {
        // ========================================
        // BÚSQUEDAS BÁSICAS
        // ========================================

        /// <summary>
        /// Buscar proveedor por NIT/RUT
        /// </summary>
        Task<Proveedor?> GetByNitRutAsync(string nitRut);

        /// <summary>
        /// Buscar proveedores por nombre (búsqueda parcial)
        /// </summary>
        Task<IEnumerable<Proveedor>> SearchByNombreAsync(string nombre);

        // ========================================
        // FILTROS ESPECIALIZADOS
        // ========================================

        /// <summary>
        /// Obtener proveedores por tipo
        /// </summary>
        Task<IEnumerable<Proveedor>> GetByTipoAsync(string tipo);

        /// <summary>
        /// Obtener proveedores activos
        /// </summary>
        Task<IEnumerable<Proveedor>> GetActivosAsync();

        /// <summary>
        /// Obtener proveedores por estado
        /// </summary>
        Task<IEnumerable<Proveedor>> GetByEstadoAsync(string estado);

        /// <summary>
        /// Obtener proveedores por ciudad
        /// </summary>
        Task<IEnumerable<Proveedor>> GetByCiudadAsync(string ciudad);

        /// <summary>
        /// Obtener proveedores por calificación mínima
        /// </summary>
        Task<IEnumerable<Proveedor>> GetByCalificacionMinimaAsync(decimal calificacionMinima);

        // ========================================
        // ESTADÍSTICAS Y REPORTES
        // ========================================

        /// <summary>
        /// Obtener top proveedores mejor calificados
        /// </summary>
        Task<IEnumerable<Proveedor>> GetTopProveedoresAsync(int cantidad = 10);

        /// <summary>
        /// Contar proveedores por tipo
        /// </summary>
        Task<Dictionary<string, int>> CountByTipoAsync();

        /// <summary>
        /// Obtener proveedores con contratos vigentes
        /// </summary>
        Task<IEnumerable<Proveedor>> GetConContratosVigentesAsync();

        // ========================================
        // VALIDACIONES
        // ========================================

        /// <summary>
        /// Verificar si existe un proveedor con el NIT/RUT dado (excluyendo un ID específico)
        /// </summary>
        Task<bool> ExisteNitRutAsync(string nitRut, int? excludeId = null);
    }
}