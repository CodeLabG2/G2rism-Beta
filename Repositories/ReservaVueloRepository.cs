using G2rismBeta.API.Data;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.Models;
using Microsoft.EntityFrameworkCore;

namespace G2rismBeta.API.Repositories;

/// <summary>
/// Repositorio para la entidad ReservaVuelo
/// Implementa operaciones de acceso a datos para la relación Reservas-Vuelos
/// </summary>
public class ReservaVueloRepository : GenericRepository<ReservaVuelo>, IReservaVueloRepository
{
    public ReservaVueloRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Obtiene todos los vuelos de una reserva con información del vuelo y aerolínea
    /// </summary>
    public async Task<IEnumerable<ReservaVuelo>> GetVuelosByReservaIdAsync(int idReserva)
    {
        return await _context.ReservasVuelos
            .Where(rv => rv.IdReserva == idReserva)
            .Include(rv => rv.Vuelo)
                .ThenInclude(v => v!.Aerolinea)
            .OrderBy(rv => rv.FechaAgregado)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene una reserva de vuelo con información completa del vuelo
    /// </summary>
    public async Task<ReservaVuelo?> GetReservaVueloConDetallesAsync(int id)
    {
        return await _context.ReservasVuelos
            .Include(rv => rv.Reserva)
            .Include(rv => rv.Vuelo)
                .ThenInclude(v => v!.Aerolinea)
            .FirstOrDefaultAsync(rv => rv.Id == id);
    }

    /// <summary>
    /// Verifica si existe una reserva de vuelo
    /// </summary>
    public async Task<bool> ExisteReservaVueloAsync(int id)
    {
        return await _context.ReservasVuelos.AnyAsync(rv => rv.Id == id);
    }

    /// <summary>
    /// Cuenta cuántos vuelos tiene una reserva
    /// </summary>
    public async Task<int> ContarVuelosPorReservaAsync(int idReserva)
    {
        return await _context.ReservasVuelos
            .Where(rv => rv.IdReserva == idReserva)
            .CountAsync();
    }
}
