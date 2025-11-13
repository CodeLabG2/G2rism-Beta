using G2rismBeta.API.Data;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.Models;
using Microsoft.EntityFrameworkCore;

namespace G2rismBeta.API.Repositories
{
    /// <summary>
    /// Repositorio de Proveedores
    /// Implementa operaciones de acceso a datos para proveedores
    /// </summary>
    public class ProveedorRepository : GenericRepository<Proveedor>, IProveedorRepository
    {
        public ProveedorRepository(ApplicationDbContext context) : base(context)
        {
        }

        // ========================================
        // BÚSQUEDAS BÁSICAS
        // ========================================

        /// <summary>
        /// Buscar proveedor por NIT/RUT
        /// </summary>
        public async Task<Proveedor?> GetByNitRutAsync(string nitRut)
        {
            return await _context.Proveedores
                .Include(p => p.Contratos)
                .FirstOrDefaultAsync(p => p.NitRut == nitRut);
        }

        /// <summary>
        /// Buscar proveedores por nombre (búsqueda parcial case-insensitive)
        /// </summary>
        public async Task<IEnumerable<Proveedor>> SearchByNombreAsync(string nombre)
        {
            return await _context.Proveedores
                .Include(p => p.Contratos)
                .Where(p => p.NombreEmpresa.ToLower().Contains(nombre.ToLower()))
                .OrderBy(p => p.NombreEmpresa)
                .ToListAsync();
        }

        // ========================================
        // FILTROS ESPECIALIZADOS
        // ========================================

        /// <summary>
        /// Obtener proveedores por tipo
        /// </summary>
        public async Task<IEnumerable<Proveedor>> GetByTipoAsync(string tipo)
        {
            return await _context.Proveedores
                .Include(p => p.Contratos)
                .Where(p => p.TipoProveedor == tipo)
                .OrderBy(p => p.NombreEmpresa)
                .ToListAsync();
        }

        /// <summary>
        /// Obtener proveedores activos
        /// </summary>
        public async Task<IEnumerable<Proveedor>> GetActivosAsync()
        {
            return await _context.Proveedores
                .Include(p => p.Contratos)
                .Where(p => p.Estado == "Activo")
                .OrderByDescending(p => p.Calificacion)
                .ThenBy(p => p.NombreEmpresa)
                .ToListAsync();
        }

        /// <summary>
        /// Obtener proveedores por estado
        /// </summary>
        public async Task<IEnumerable<Proveedor>> GetByEstadoAsync(string estado)
        {
            return await _context.Proveedores
                .Include(p => p.Contratos)
                .Where(p => p.Estado == estado)
                .OrderBy(p => p.NombreEmpresa)
                .ToListAsync();
        }

        /// <summary>
        /// Obtener proveedores por ciudad
        /// </summary>
        public async Task<IEnumerable<Proveedor>> GetByCiudadAsync(string ciudad)
        {
            return await _context.Proveedores
                .Include(p => p.Contratos)
                .Where(p => p.Ciudad.ToLower() == ciudad.ToLower())
                .OrderBy(p => p.NombreEmpresa)
                .ToListAsync();
        }

        /// <summary>
        /// Obtener proveedores por calificación mínima
        /// </summary>
        public async Task<IEnumerable<Proveedor>> GetByCalificacionMinimaAsync(decimal calificacionMinima)
        {
            return await _context.Proveedores
                .Include(p => p.Contratos)
                .Where(p => p.Calificacion >= calificacionMinima)
                .OrderByDescending(p => p.Calificacion)
                .ThenBy(p => p.NombreEmpresa)
                .ToListAsync();
        }

        // ========================================
        // ESTADÍSTICAS Y REPORTES
        // ========================================

        /// <summary>
        /// Obtener top proveedores mejor calificados
        /// </summary>
        public async Task<IEnumerable<Proveedor>> GetTopProveedoresAsync(int cantidad = 10)
        {
            return await _context.Proveedores
                .Include(p => p.Contratos)
                .Where(p => p.Estado == "Activo")
                .OrderByDescending(p => p.Calificacion)
                .ThenBy(p => p.NombreEmpresa)
                .Take(cantidad)
                .ToListAsync();
        }

        /// <summary>
        /// Contar proveedores por tipo
        /// </summary>
        public async Task<Dictionary<string, int>> CountByTipoAsync()
        {
            return await _context.Proveedores
                .GroupBy(p => p.TipoProveedor)
                .Select(g => new { Tipo = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Tipo, x => x.Count);
        }

        /// <summary>
        /// Obtener proveedores con contratos vigentes
        /// </summary>
        public async Task<IEnumerable<Proveedor>> GetConContratosVigentesAsync()
        {
            var hoy = DateTime.Now;

            return await _context.Proveedores
                .Include(p => p.Contratos)
                .Where(p => p.Contratos != null &&
                            p.Contratos.Any(c => c.Estado == "Vigente" &&
                                               c.FechaFin >= hoy))
                .OrderBy(p => p.NombreEmpresa)
                .ToListAsync();
        }

        // ========================================
        // VALIDACIONES
        // ========================================

        /// <summary>
        /// Verificar si existe un proveedor con el NIT/RUT dado
        /// </summary>
        public async Task<bool> ExisteNitRutAsync(string nitRut, int? excludeId = null)
        {
            var query = _context.Proveedores.Where(p => p.NitRut == nitRut);

            if (excludeId.HasValue)
            {
                query = query.Where(p => p.IdProveedor != excludeId.Value);
            }

            return await query.AnyAsync();
        }
    }
}