using Microsoft.EntityFrameworkCore;
using G2rismBeta.API.Data;
using G2rismBeta.API.Models;
using G2rismBeta.API.Interfaces;

namespace G2rismBeta.API.Repositories
{
    /// <summary>
    /// Repositorio para gestión de empleados con jerarquía organizacional
    /// Implementa TODOS los métodos definidos en IEmpleadoRepository
    /// </summary>
    public class EmpleadoRepository : GenericRepository<Empleado>, IEmpleadoRepository
    {
        public EmpleadoRepository(ApplicationDbContext context) : base(context)
        {
        }

        // ========================================
        // MÉTODOS DE BÚSQUEDA BÁSICA
        // ========================================

        /// <summary>
        /// Buscar empleado por documento de identidad
        /// </summary>
        public async Task<Empleado?> GetByDocumentoAsync(string documentoIdentidad)
        {
            return await _context.Empleados
                .Include(e => e.Usuario)
                .Include(e => e.Jefe)
                .FirstOrDefaultAsync(e => e.DocumentoIdentidad == documentoIdentidad);
        }

        /// <summary>
        /// Buscar empleado por ID de usuario
        /// </summary>
        public async Task<Empleado?> GetByUsuarioIdAsync(int idUsuario)
        {
            return await _context.Empleados
                .Include(e => e.Usuario)
                .Include(e => e.Jefe)
                .FirstOrDefaultAsync(e => e.IdUsuario == idUsuario);
        }

        /// <summary>
        /// Obtener empleados por cargo
        /// </summary>
        public async Task<IEnumerable<Empleado>> GetEmpleadosPorCargoAsync(string cargo)
        {
            return await _context.Empleados
                .Include(e => e.Usuario)
                .Include(e => e.Jefe)
                .Where(e => e.Cargo == cargo)
                .OrderBy(e => e.Apellido)
                .ThenBy(e => e.Nombre)
                .ToListAsync();
        }

        /// <summary>
        /// Obtener empleados por estado (Activo, Inactivo, Vacaciones, Licencia)
        /// </summary>
        public async Task<IEnumerable<Empleado>> GetEmpleadosPorEstadoAsync(string estado)
        {
            return await _context.Empleados
                .Include(e => e.Usuario)
                .Include(e => e.Jefe)
                .Where(e => e.Estado == estado)
                .OrderBy(e => e.Apellido)
                .ThenBy(e => e.Nombre)
                .ToListAsync();
        }

        /// <summary>
        /// Buscar empleados por nombre o apellido
        /// </summary>
        public async Task<IEnumerable<Empleado>> BuscarPorNombreAsync(string termino)
        {
            var terminoLower = termino.ToLower();

            return await _context.Empleados
                .Include(e => e.Usuario)
                .Include(e => e.Jefe)
                .Where(e => e.Nombre.ToLower().Contains(terminoLower) ||
                           e.Apellido.ToLower().Contains(terminoLower))
                .OrderBy(e => e.Apellido)
                .ThenBy(e => e.Nombre)
                .ToListAsync();
        }

        // ========================================
        // MÉTODOS CON RELACIONES (INCLUDE)
        // ========================================

        /// <summary>
        /// Obtener empleado con su usuario y jefe incluidos
        /// </summary>
        public async Task<Empleado?> GetEmpleadoConRelacionesAsync(int idEmpleado)
        {
            return await _context.Empleados
                .Include(e => e.Usuario)
                .Include(e => e.Jefe)
                .FirstOrDefaultAsync(e => e.IdEmpleado == idEmpleado);
        }

        /// <summary>
        /// Obtener empleado con todas sus relaciones (usuario, jefe, subordinados)
        /// Ideal para mostrar información completa del empleado
        /// </summary>
        public async Task<Empleado?> GetEmpleadoCompletoAsync(int idEmpleado)
        {
            return await _context.Empleados
                .Include(e => e.Usuario)
                .Include(e => e.Jefe)
                .Include(e => e.Subordinados)
                .FirstOrDefaultAsync(e => e.IdEmpleado == idEmpleado);
        }

        // ========================================
        // MÉTODOS DE JERARQUÍA ORGANIZACIONAL
        // ========================================

        /// <summary>
        /// Obtener todos los subordinados directos de un empleado (un nivel)
        /// </summary>
        public async Task<IEnumerable<Empleado>> GetSubordinadosDirectosAsync(int idJefe)
        {
            return await _context.Empleados
                .Include(e => e.Usuario)
                .Where(e => e.IdJefe == idJefe)
                .OrderBy(e => e.Apellido)
                .ThenBy(e => e.Nombre)
                .ToListAsync();
        }

        /// <summary>
        /// Obtener todos los subordinados de un empleado (todos los niveles - recursivo)
        /// Incluye subordinados directos y sus subordinados
        /// </summary>
        public async Task<IEnumerable<Empleado>> GetTodosLosSubordinadosAsync(int idJefe)
        {
            var subordinados = new List<Empleado>();
            await ObtenerSubordinadosRecursivoAsync(idJefe, subordinados);
            return subordinados;
        }

        /// <summary>
        /// Método auxiliar recursivo para obtener todos los subordinados
        /// </summary>
        private async Task ObtenerSubordinadosRecursivoAsync(int idJefe, List<Empleado> subordinados)
        {
            var subordinadosDirectos = await _context.Empleados
                .Include(e => e.Usuario)
                .Include(e => e.Jefe)
                .Where(e => e.IdJefe == idJefe)
                .ToListAsync();

            foreach (var subordinado in subordinadosDirectos)
            {
                subordinados.Add(subordinado);
                // Llamada recursiva para obtener subordinados del subordinado
                await ObtenerSubordinadosRecursivoAsync(subordinado.IdEmpleado, subordinados);
            }
        }

        /// <summary>
        /// Obtener el jefe directo de un empleado
        /// </summary>
        public async Task<Empleado?> GetJefeDirectoAsync(int idEmpleado)
        {
            var empleado = await _context.Empleados
                .Include(e => e.Jefe)
                .FirstOrDefaultAsync(e => e.IdEmpleado == idEmpleado);

            return empleado?.Jefe;
        }

        /// <summary>
        /// Obtener toda la cadena de jefes hasta el nivel más alto (CEO)
        /// Retorna la lista de jefes en orden ascendente (jefe inmediato → CEO)
        /// </summary>
        public async Task<IEnumerable<Empleado>> GetCadenaDeJefesAsync(int idEmpleado)
        {
            var cadena = new List<Empleado>();
            var empleadoActual = await _context.Empleados
                .Include(e => e.Jefe)
                .FirstOrDefaultAsync(e => e.IdEmpleado == idEmpleado);

            int nivel = 0;
            while (empleadoActual?.Jefe != null)
            {
                empleadoActual = await _context.Empleados
                    .Include(e => e.Jefe)
                    .FirstOrDefaultAsync(e => e.IdEmpleado == empleadoActual.IdJefe);

                if (empleadoActual != null)
                {
                    cadena.Add(empleadoActual);
                }

                // Protección contra ciclos infinitos
                nivel++;
                if (nivel > 20)
                {
                    throw new InvalidOperationException("Se detectó una jerarquía circular o excesivamente profunda");
                }
            }

            return cadena;
        }

        /// <summary>
        /// Obtener empleados que no tienen jefe (nivel más alto de la jerarquía)
        /// Generalmente retorna CEO, Gerente General, etc.
        /// </summary>
        public async Task<IEnumerable<Empleado>> GetEmpleadosSinJefeAsync()
        {
            return await _context.Empleados
                .Include(e => e.Usuario)
                .Where(e => e.IdJefe == null)
                .OrderBy(e => e.Apellido)
                .ThenBy(e => e.Nombre)
                .ToListAsync();
        }

        /// <summary>
        /// Obtener empleados que son jefes (tienen al menos un subordinado)
        /// </summary>
        public async Task<IEnumerable<Empleado>> GetEmpleadosQuesonJefesAsync()
        {
            // Obtener IDs únicos de jefes
            var idsJefes = await _context.Empleados
                .Where(e => e.IdJefe != null)
                .Select(e => e.IdJefe!.Value)
                .Distinct()
                .ToListAsync();

            // Obtener los empleados que son jefes
            return await _context.Empleados
                .Include(e => e.Usuario)
                .Where(e => idsJefes.Contains(e.IdEmpleado))
                .OrderBy(e => e.Apellido)
                .ThenBy(e => e.Nombre)
                .ToListAsync();
        }

        /// <summary>
        /// Contar cuántos subordinados directos tiene un empleado
        /// </summary>
        public async Task<int> ContarSubordinadosDirectosAsync(int idJefe)
        {
            return await _context.Empleados
                .CountAsync(e => e.IdJefe == idJefe);
        }

        /// <summary>
        /// Contar cuántos subordinados totales tiene un empleado (todos los niveles)
        /// </summary>
        public async Task<int> ContarTodosLosSubordinadosAsync(int idJefe)
        {
            var subordinados = await GetTodosLosSubordinadosAsync(idJefe);
            return subordinados.Count();
        }

        /// <summary>
        /// Obtener el organigrama completo de la empresa
        /// Retorna todos los empleados con sus relaciones de jerarquía
        /// </summary>
        public async Task<IEnumerable<Empleado>> GetOrganigramaCompletoAsync()
        {
            // Obtener todos los empleados activos con sus relaciones
            return await _context.Empleados
                .Include(e => e.Usuario)
                .Include(e => e.Jefe)
                .Include(e => e.Subordinados)
                .Where(e => e.Estado == "activo")
                .OrderBy(e => e.Apellido)
                .ThenBy(e => e.Nombre)
                .ToListAsync();
        }

        /// <summary>
        /// Verificar si un empleado es jefe de otro (directo o indirecto)
        /// Útil para validar operaciones
        /// </summary>
        public async Task<bool> EsJefeDeAsync(int idPosibleJefe, int idEmpleado)
        {
            // Obtener todos los subordinados del posible jefe
            var subordinados = await GetTodosLosSubordinadosAsync(idPosibleJefe);

            // Verificar si el empleado está en la lista de subordinados
            return subordinados.Any(s => s.IdEmpleado == idEmpleado);
        }

        // ========================================
        // MÉTODOS DE VALIDACIÓN
        // ========================================

        /// <summary>
        /// Verificar si existe un documento de identidad
        /// </summary>
        public async Task<bool> ExisteDocumentoAsync(string documentoIdentidad, int? idEmpleadoExcluir = null)
        {
            if (idEmpleadoExcluir.HasValue)
            {
                return await _context.Empleados
                    .AnyAsync(e => e.DocumentoIdentidad == documentoIdentidad &&
                                  e.IdEmpleado != idEmpleadoExcluir.Value);
            }

            return await _context.Empleados
                .AnyAsync(e => e.DocumentoIdentidad == documentoIdentidad);
        }

        /// <summary>
        /// Verificar si un usuario ya tiene un empleado asociado
        /// </summary>
        public async Task<bool> UsuarioTieneEmpleadoAsync(int idUsuario, int? idEmpleadoExcluir = null)
        {
            if (idEmpleadoExcluir.HasValue)
            {
                return await _context.Empleados
                    .AnyAsync(e => e.IdUsuario == idUsuario &&
                                  e.IdEmpleado != idEmpleadoExcluir.Value);
            }

            return await _context.Empleados
                .AnyAsync(e => e.IdUsuario == idUsuario);
        }

        /// <summary>
        /// Verificar si un empleado puede ser asignado como jefe de otro
        /// (Previene ciclos en la jerarquía: A no puede ser jefe de B si B es jefe de A)
        /// </summary>
        public async Task<bool> PuedeSerJefeDeAsync(int idNuevoJefe, int idEmpleado)
        {
            // Un empleado no puede ser su propio jefe
            if (idNuevoJefe == idEmpleado)
                return false;

            // Verificar si el nuevo jefe es subordinado del empleado
            // (esto crearía un ciclo)
            var subordinados = await GetTodosLosSubordinadosAsync(idEmpleado);
            return !subordinados.Any(s => s.IdEmpleado == idNuevoJefe);
        }

        /// <summary>
        /// Verificar si un empleado tiene subordinados
        /// </summary>
        public async Task<bool> TieneSubordinadosAsync(int idEmpleado)
        {
            return await _context.Empleados
                .AnyAsync(e => e.IdJefe == idEmpleado);
        }

        // ========================================
        // MÉTODOS DE GESTIÓN DE JERARQUÍA
        // ========================================

        /// <summary>
        /// Reasignar todos los subordinados de un jefe a otro jefe
        /// Útil antes de eliminar o cambiar de cargo a un empleado
        /// </summary>
        public async Task<bool> ReasignarSubordinadosAsync(int idJefeActual, int? idNuevoJefe)
        {
            var subordinados = await _context.Empleados
                .Where(e => e.IdJefe == idJefeActual)
                .ToListAsync();

            foreach (var subordinado in subordinados)
            {
                subordinado.IdJefe = idNuevoJefe;
            }

            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Cambiar el jefe de un empleado
        /// </summary>
        public async Task<bool> CambiarJefeAsync(int idEmpleado, int? idNuevoJefe)
        {
            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(e => e.IdEmpleado == idEmpleado);

            if (empleado == null)
                return false;

            empleado.IdJefe = idNuevoJefe;
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Cambiar estado del empleado (Activo/Inactivo/Vacaciones/Licencia)
        /// </summary>
        public async Task<bool> CambiarEstadoAsync(int idEmpleado, string nuevoEstado)
        {
            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(e => e.IdEmpleado == idEmpleado);

            if (empleado == null)
                return false;

            empleado.Estado = nuevoEstado;
            return await _context.SaveChangesAsync() > 0;
        }

        // ========================================
        // MÉTODOS ESTADÍSTICOS
        // ========================================

        /// <summary>
        /// Obtener estadísticas de empleados por cargo
        /// </summary>
        public async Task<Dictionary<string, int>> GetEstadisticasPorCargoAsync()
        {
            return await _context.Empleados
                .Where(e => e.Estado == "activo")
                .GroupBy(e => e.Cargo)
                .Select(g => new { Cargo = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Cargo, x => x.Count);
        }

        /// <summary>
        /// Obtener estadísticas de empleados por estado
        /// </summary>
        public async Task<Dictionary<string, int>> GetEstadisticasPorEstadoAsync()
        {
            return await _context.Empleados
                .GroupBy(e => e.Estado)
                .Select(g => new { Estado = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Estado, x => x.Count);
        }

        /// <summary>
        /// Obtener el promedio de antigüedad en años de los empleados
        /// </summary>
        public async Task<double> GetPromedioAntiguedadAsync()
        {
            var empleados = await _context.Empleados
                .Where(e => e.Estado == "activo")
                .ToListAsync();

            if (!empleados.Any())
                return 0;

            var antiguedades = empleados
                .Select(e => (DateTime.Now - e.FechaIngreso).TotalDays / 365.25)
                .ToList();

            return antiguedades.Average();
        }

        /// <summary>
        /// Obtener empleados con mayor antigüedad
        /// </summary>
        public async Task<IEnumerable<Empleado>> GetEmpleadosConMayorAntiguedadAsync(int cantidad = 10)
        {
            return await _context.Empleados
                .Include(e => e.Usuario)
                .Include(e => e.Jefe)
                .Where(e => e.Estado == "activo")
                .OrderBy(e => e.FechaIngreso)
                .Take(cantidad)
                .ToListAsync();
        }
    }
}