using Microsoft.EntityFrameworkCore;
using G2rismBeta.API.Data;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.Models;

namespace G2rismBeta.API.Repositories;

/// <summary>
/// Repositorio para operaciones de datos de Reserva
/// Extiende GenericRepository con métodos específicos para Reserva
/// </summary>
public class ReservaRepository : GenericRepository<Reserva>, IReservaRepository
{
    public ReservaRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Obtener todas las reservas con información completa (Cliente, Empleado, Servicios)
    /// </summary>
    public async Task<IEnumerable<Reserva>> GetAllConDetallesAsync()
    {
        return await _dbSet
            .Include(r => r.Cliente)
            .Include(r => r.Empleado)
            // TODO: Incluir servicios cuando se implementen en Día 3 Tarea 2-4
            // .Include(r => r.ReservasHoteles).ThenInclude(rh => rh.Hotel)
            // .Include(r => r.ReservasVuelos).ThenInclude(rv => rv.Vuelo)
            // .Include(r => r.ReservasPaquetes).ThenInclude(rp => rp.Paquete)
            // .Include(r => r.ReservasServicios).ThenInclude(rs => rs.Servicio)
            .OrderByDescending(r => r.FechaHora) // Más recientes primero
            .ToListAsync();
    }

    /// <summary>
    /// Obtener una reserva por ID con información relacionada
    /// </summary>
    public async Task<Reserva?> GetReservaConDetallesAsync(int id)
    {
        return await _dbSet
            .Include(r => r.Cliente)
            .Include(r => r.Empleado)
            // TODO: Incluir servicios cuando se implementen
            .FirstOrDefaultAsync(r => r.IdReserva == id);
    }

    /// <summary>
    /// Obtener todas las reservas de un cliente específico
    /// </summary>
    public async Task<IEnumerable<Reserva>> GetReservasByClienteAsync(int idCliente)
    {
        return await _dbSet
            .Include(r => r.Cliente)
            .Include(r => r.Empleado)
            .Where(r => r.IdCliente == idCliente)
            .OrderByDescending(r => r.FechaHora)
            .ToListAsync();
    }

    /// <summary>
    /// Obtener todas las reservas gestionadas por un empleado
    /// </summary>
    public async Task<IEnumerable<Reserva>> GetReservasByEmpleadoAsync(int idEmpleado)
    {
        return await _dbSet
            .Include(r => r.Cliente)
            .Include(r => r.Empleado)
            .Where(r => r.IdEmpleado == idEmpleado)
            .OrderByDescending(r => r.FechaHora)
            .ToListAsync();
    }

    /// <summary>
    /// Obtener reservas filtradas por estado
    /// </summary>
    public async Task<IEnumerable<Reserva>> GetReservasByEstadoAsync(string estado)
    {
        return await _dbSet
            .Include(r => r.Cliente)
            .Include(r => r.Empleado)
            .Where(r => r.Estado.ToLower() == estado.ToLower())
            .OrderByDescending(r => r.FechaHora)
            .ToListAsync();
    }

    /// <summary>
    /// Obtener reservas en un rango de fechas
    /// </summary>
    public async Task<IEnumerable<Reserva>> GetReservasByRangoFechasAsync(DateTime fechaInicio, DateTime fechaFin)
    {
        return await _dbSet
            .Include(r => r.Cliente)
            .Include(r => r.Empleado)
            .Where(r => r.FechaInicioViaje >= fechaInicio && r.FechaFinViaje <= fechaFin)
            .OrderBy(r => r.FechaInicioViaje)
            .ToListAsync();
    }

    /// <summary>
    /// Actualizar los montos de una reserva
    /// </summary>
    public async Task<bool> ActualizarMontosAsync(int idReserva, decimal montoTotal, decimal montoPagado)
    {
        var reserva = await GetByIdAsync(idReserva);

        if (reserva == null)
            return false;

        reserva.MontoTotal = montoTotal;
        reserva.MontoPagado = montoPagado;
        reserva.SaldoPendiente = montoTotal - montoPagado;
        reserva.FechaModificacion = DateTime.Now;

        _dbSet.Update(reserva);
        return await SaveChangesAsync();
    }

    /// <summary>
    /// Verificar si un cliente tiene reservas activas
    /// </summary>
    public async Task<bool> ClienteTieneReservasActivasAsync(int idCliente)
    {
        return await _dbSet.AnyAsync(r =>
            r.IdCliente == idCliente &&
            (r.Estado == "pendiente" || r.Estado == "confirmada")
        );
    }

    /// <summary>
    /// Override del método GetAllAsync para incluir navegación básica
    /// </summary>
    public override async Task<IEnumerable<Reserva>> GetAllAsync()
    {
        return await _dbSet
            .Include(r => r.Cliente)
            .Include(r => r.Empleado)
            .OrderByDescending(r => r.FechaHora)
            .ToListAsync();
    }

    /// <summary>
    /// Override del método GetByIdAsync para incluir navegación
    /// </summary>
    public override async Task<Reserva?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(r => r.Cliente)
            .Include(r => r.Empleado)
            .FirstOrDefaultAsync(r => r.IdReserva == id);
    }
}
