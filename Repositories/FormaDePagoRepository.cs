using G2rismBeta.API.Data;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.Models;
using Microsoft.EntityFrameworkCore;

namespace G2rismBeta.API.Repositories;

/// <summary>
/// Repositorio de Formas de Pago.
/// Implementa operaciones de acceso a datos específicas de formas de pago.
/// </summary>
public class FormaDePagoRepository : GenericRepository<FormaDePago>, IFormaDePagoRepository
{
    public FormaDePagoRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Obtener todas las formas de pago activas
    /// </summary>
    public async Task<IEnumerable<FormaDePago>> GetFormasDePagoActivasAsync()
    {
        return await _context.FormasDePago
            .Where(f => f.Activo)
            .OrderBy(f => f.Metodo)
            .ToListAsync();
    }

    /// <summary>
    /// Obtener una forma de pago por su método
    /// </summary>
    public async Task<FormaDePago?> GetPorMetodoAsync(string metodo)
    {
        return await _context.FormasDePago
            .FirstOrDefaultAsync(f => f.Metodo.ToLower() == metodo.ToLower());
    }

    /// <summary>
    /// Verificar si existe una forma de pago con un método específico
    /// </summary>
    public async Task<bool> ExistePorMetodoAsync(string metodo)
    {
        return await _context.FormasDePago
            .AnyAsync(f => f.Metodo.ToLower() == metodo.ToLower());
    }

    /// <summary>
    /// Verificar si existe una forma de pago con un método específico, excluyendo un ID
    /// </summary>
    public async Task<bool> ExistePorMetodoAsync(string metodo, int excludeId)
    {
        return await _context.FormasDePago
            .AnyAsync(f => f.Metodo.ToLower() == metodo.ToLower() && f.IdFormaPago != excludeId);
    }

    /// <summary>
    /// Obtener formas de pago que requieren verificación
    /// </summary>
    public async Task<IEnumerable<FormaDePago>> GetFormasQueRequierenVerificacionAsync()
    {
        return await _context.FormasDePago
            .Where(f => f.RequiereVerificacion && f.Activo)
            .OrderBy(f => f.Metodo)
            .ToListAsync();
    }
}