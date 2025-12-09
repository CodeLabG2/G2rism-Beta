using AutoMapper;
using G2rismBeta.API.DTOs.Factura;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.Models;

namespace G2rismBeta.API.Services;

/// <summary>
/// Servicio de Facturas.
/// Implementa la lógica de negocio para la gestión de facturas.
/// </summary>
public class FacturaService : IFacturaService
{
    private readonly IFacturaRepository _facturaRepository;
    private readonly IReservaRepository _reservaRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<FacturaService> _logger;

    public FacturaService(
        IFacturaRepository facturaRepository,
        IReservaRepository reservaRepository,
        IClienteRepository clienteRepository,
        IMapper mapper,
        ILogger<FacturaService> logger)
    {
        _facturaRepository = facturaRepository;
        _reservaRepository = reservaRepository;
        _clienteRepository = clienteRepository;
        _mapper = mapper;
        _logger = logger;
    }

    #region Consultas

    /// <summary>
    /// Obtener todas las facturas
    /// </summary>
    public async Task<IEnumerable<FacturaResponseDto>> GetAllFacturasAsync()
    {
        _logger.LogInformation("📋 Obteniendo todas las facturas");

        var facturas = await _facturaRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<FacturaResponseDto>>(facturas);
    }

    /// <summary>
    /// Obtener factura por ID
    /// </summary>
    public async Task<FacturaResponseDto> GetFacturaByIdAsync(int id)
    {
        _logger.LogInformation($"🔍 Buscando factura con ID: {id}");

        var factura = await _facturaRepository.GetFacturaConDetallesAsync(id);

        if (factura == null)
        {
            _logger.LogWarning($"⚠️ Factura con ID {id} no encontrada");
            throw new KeyNotFoundException($"No se encontró una factura con el ID {id}");
        }

        return _mapper.Map<FacturaResponseDto>(factura);
    }

    /// <summary>
    /// Obtener facturas por reserva
    /// </summary>
    public async Task<IEnumerable<FacturaResponseDto>> GetFacturasPorReservaAsync(int idReserva)
    {
        _logger.LogInformation($"🔍 Buscando facturas para la reserva ID: {idReserva}");

        // Verificar que la reserva existe
        if (!await _reservaRepository.ExistsAsync(idReserva))
        {
            throw new KeyNotFoundException($"No se encontró una reserva con el ID {idReserva}");
        }

        var facturas = await _facturaRepository.GetFacturasPorReservaAsync(idReserva);
        return _mapper.Map<IEnumerable<FacturaResponseDto>>(facturas);
    }

    /// <summary>
    /// Obtener factura por número
    /// </summary>
    public async Task<FacturaResponseDto> GetFacturaPorNumeroAsync(string numeroFactura)
    {
        _logger.LogInformation($"🔍 Buscando factura con número: {numeroFactura}");

        var factura = await _facturaRepository.GetFacturaPorNumeroAsync(numeroFactura);

        if (factura == null)
        {
            _logger.LogWarning($"⚠️ Factura con número {numeroFactura} no encontrada");
            throw new KeyNotFoundException($"No se encontró una factura con el número {numeroFactura}");
        }

        return _mapper.Map<FacturaResponseDto>(factura);
    }

    /// <summary>
    /// Obtener facturas por estado
    /// </summary>
    public async Task<IEnumerable<FacturaResponseDto>> GetFacturasPorEstadoAsync(string estado)
    {
        _logger.LogInformation($"🔍 Buscando facturas con estado: {estado}");

        var estadosValidos = new[] { "pendiente", "pagada", "cancelada", "vencida" };
        if (!estadosValidos.Contains(estado.ToLower()))
        {
            throw new ArgumentException($"Estado '{estado}' no válido. Estados permitidos: {string.Join(", ", estadosValidos)}");
        }

        var facturas = await _facturaRepository.GetFacturasPorEstadoAsync(estado);
        return _mapper.Map<IEnumerable<FacturaResponseDto>>(facturas);
    }

    /// <summary>
    /// Obtener facturas vencidas
    /// </summary>
    public async Task<IEnumerable<FacturaResponseDto>> GetFacturasVencidasAsync()
    {
        _logger.LogInformation("🔍 Buscando facturas vencidas");

        var facturas = await _facturaRepository.GetFacturasVencidasAsync();
        return _mapper.Map<IEnumerable<FacturaResponseDto>>(facturas);
    }

    /// <summary>
    /// Obtener facturas próximas a vencer
    /// </summary>
    public async Task<IEnumerable<FacturaResponseDto>> GetFacturasProximasAVencerAsync(int dias = 7)
    {
        _logger.LogInformation($"🔍 Buscando facturas próximas a vencer (próximos {dias} días)");

        if (dias <= 0)
        {
            throw new ArgumentException("El número de días debe ser mayor a 0");
        }

        var facturas = await _facturaRepository.GetFacturasProximasAVencerAsync(dias);
        return _mapper.Map<IEnumerable<FacturaResponseDto>>(facturas);
    }

    #endregion

    #region Operaciones de Escritura

    /// <summary>
    /// Crear una nueva factura desde una reserva
    /// </summary>
    public async Task<FacturaResponseDto> CrearFacturaAsync(FacturaCreateDto dto)
    {
        _logger.LogInformation($"📝 Creando factura para la reserva ID: {dto.IdReserva}");

        // 1. Verificar que la reserva existe
        var reserva = await _reservaRepository.GetReservaConDetallesAsync(dto.IdReserva);
        if (reserva == null)
        {
            throw new KeyNotFoundException($"No se encontró una reserva con el ID {dto.IdReserva}");
        }

        // 2. Verificar que la reserva no tenga ya una factura activa
        if (await _facturaRepository.ExisteFacturaParaReservaAsync(dto.IdReserva))
        {
            throw new InvalidOperationException($"La reserva {dto.IdReserva} ya tiene una factura generada. No se puede generar una nueva.");
        }

        // 3. Verificar que la reserva esté confirmada
        if (reserva.Estado.ToLower() != "confirmada")
        {
            throw new InvalidOperationException($"La reserva debe estar confirmada para generar una factura. Estado actual: {reserva.Estado}");
        }

        // 4. Obtener cliente para aplicar descuentos
        var cliente = await _clienteRepository.GetClienteConCategoriaAsync(reserva.IdCliente);
        if (cliente == null)
        {
            throw new KeyNotFoundException($"No se encontró el cliente con ID {reserva.IdCliente}");
        }

        // 5. Generar número de factura único
        var numeroFactura = await GenerarNumeroFacturaAsync();

        // 6. Calcular montos
        var subtotal = reserva.MontoTotal;
        var descuentoCategoria = CalcularDescuentoCategoria(subtotal, cliente);
        var descuentosAdicionales = dto.DescuentosAdicionales ?? 0;
        var descuentoTotal = descuentoCategoria + descuentosAdicionales;
        var baseGravable = subtotal - descuentoTotal;
        var porcentajeIva = dto.PorcentajeIva ?? 19; // Por defecto 19% en Colombia
        var impuestos = Math.Round(baseGravable * (porcentajeIva / 100), 2);
        var total = baseGravable + impuestos;

        // 7. Calcular fecha de vencimiento (por defecto 30 días desde emisión)
        var fechaVencimiento = dto.FechaVencimiento ?? DateTime.Today.AddDays(30);

        // 8. Crear la factura
        var factura = new Factura
        {
            IdReserva = dto.IdReserva,
            NumeroFactura = numeroFactura,
            FechaEmision = DateTime.Today,
            FechaVencimiento = fechaVencimiento,
            ResolucionDian = dto.ResolucionDian,
            TipoFactura = "venta",
            Estado = "pendiente",
            Subtotal = subtotal,
            Descuentos = descuentoTotal,
            PorcentajeIva = porcentajeIva,
            Impuestos = impuestos,
            Total = total,
            Observaciones = dto.Observaciones,
            FechaCreacion = DateTime.Now
        };

        // 9. Guardar en base de datos
        var facturaCreada = await _facturaRepository.AddAsync(factura);
        await _facturaRepository.SaveChangesAsync();

        _logger.LogInformation($"✅ Factura creada exitosamente con número: {numeroFactura}");

        // 10. Retornar DTO de respuesta
        return await GetFacturaByIdAsync(facturaCreada.IdFactura);
    }

    /// <summary>
    /// Actualizar una factura existente
    /// </summary>
    public async Task<FacturaResponseDto> ActualizarFacturaAsync(int id, FacturaUpdateDto dto)
    {
        _logger.LogInformation($"📝 Actualizando factura ID: {id}");

        // 1. Verificar que la factura existe
        var factura = await _facturaRepository.GetByIdAsync(id);
        if (factura == null)
        {
            throw new KeyNotFoundException($"No se encontró una factura con el ID {id}");
        }

        // 2. No permitir actualizar facturas pagadas o canceladas
        if (factura.Estado == "pagada")
        {
            throw new InvalidOperationException("No se puede actualizar una factura que ya está pagada");
        }

        if (factura.Estado == "cancelada")
        {
            throw new InvalidOperationException("No se puede actualizar una factura cancelada");
        }

        // 3. Aplicar actualizaciones parciales
        if (dto.FechaVencimiento.HasValue)
        {
            factura.FechaVencimiento = dto.FechaVencimiento.Value;
        }

        if (!string.IsNullOrWhiteSpace(dto.ResolucionDian))
        {
            factura.ResolucionDian = dto.ResolucionDian;
        }

        if (!string.IsNullOrWhiteSpace(dto.CufeCude))
        {
            factura.CufeCude = dto.CufeCude;
        }

        if (!string.IsNullOrWhiteSpace(dto.Estado))
        {
            var estadosValidos = new[] { "pendiente", "pagada", "cancelada", "vencida" };
            if (!estadosValidos.Contains(dto.Estado.ToLower()))
            {
                throw new ArgumentException($"Estado '{dto.Estado}' no válido");
            }
            factura.Estado = dto.Estado.ToLower();
        }

        if (dto.Observaciones != null)
        {
            factura.Observaciones = dto.Observaciones;
        }

        factura.FechaModificacion = DateTime.Now;

        // 4. Guardar cambios
        await _facturaRepository.UpdateAsync(factura);
        await _facturaRepository.SaveChangesAsync();

        _logger.LogInformation($"✅ Factura ID {id} actualizada exitosamente");

        return await GetFacturaByIdAsync(id);
    }

    /// <summary>
    /// Cambiar el estado de una factura
    /// </summary>
    public async Task<FacturaResponseDto> CambiarEstadoFacturaAsync(int id, string nuevoEstado)
    {
        _logger.LogInformation($"📝 Cambiando estado de factura ID {id} a: {nuevoEstado}");

        // 1. Verificar que la factura existe
        var factura = await _facturaRepository.GetByIdAsync(id);
        if (factura == null)
        {
            throw new KeyNotFoundException($"No se encontró una factura con el ID {id}");
        }

        // 2. Validar estado
        var estadosValidos = new[] { "pendiente", "pagada", "cancelada", "vencida" };
        if (!estadosValidos.Contains(nuevoEstado.ToLower()))
        {
            throw new ArgumentException($"Estado '{nuevoEstado}' no válido. Estados permitidos: {string.Join(", ", estadosValidos)}");
        }

        // 3. Actualizar estado
        factura.Estado = nuevoEstado.ToLower();
        factura.FechaModificacion = DateTime.Now;

        await _facturaRepository.UpdateAsync(factura);
        await _facturaRepository.SaveChangesAsync();

        _logger.LogInformation($"✅ Estado de factura ID {id} cambiado a: {nuevoEstado}");

        return await GetFacturaByIdAsync(id);
    }

    /// <summary>
    /// Eliminar (cancelar) una factura
    /// </summary>
    public async Task<bool> EliminarFacturaAsync(int id)
    {
        _logger.LogInformation($"🗑️ Eliminando (cancelando) factura ID: {id}");

        // 1. Verificar que la factura existe
        var factura = await _facturaRepository.GetFacturaConDetallesAsync(id);
        if (factura == null)
        {
            throw new KeyNotFoundException($"No se encontró una factura con el ID {id}");
        }

        // 2. No permitir eliminar facturas ya pagadas
        if (factura.Estado == "pagada")
        {
            throw new InvalidOperationException("No se puede eliminar una factura que ya está pagada");
        }

        // 3. Verificar si tiene pagos asociados
        if (factura.Pagos.Any(p => p.Estado == "aprobado"))
        {
            throw new InvalidOperationException("No se puede eliminar una factura que tiene pagos aprobados. Debe cancelar los pagos primero.");
        }

        // 4. Cancelar la factura en lugar de eliminarla (soft delete)
        factura.Estado = "cancelada";
        factura.FechaModificacion = DateTime.Now;

        await _facturaRepository.UpdateAsync(factura);
        await _facturaRepository.SaveChangesAsync();

        _logger.LogInformation($"✅ Factura ID {id} cancelada exitosamente");

        return true;
    }

    #endregion

    #region Métodos Auxiliares Privados

    /// <summary>
    /// Generar número de factura único con formato: FAC-{año}-{consecutivo}
    /// </summary>
    private async Task<string> GenerarNumeroFacturaAsync()
    {
        var anioActual = DateTime.Now.Year;
        var ultimoNumero = await _facturaRepository.GetUltimoNumeroFacturaDelAnioAsync(anioActual);

        int consecutivo = 1;

        if (!string.IsNullOrEmpty(ultimoNumero))
        {
            // Extraer el consecutivo del último número (formato: FAC-2025-00001)
            var partes = ultimoNumero.Split('-');
            if (partes.Length == 3 && int.TryParse(partes[2], out int ultimoConsecutivo))
            {
                consecutivo = ultimoConsecutivo + 1;
            }
        }

        // Formato: FAC-2025-00001
        return $"FAC-{anioActual}-{consecutivo:D5}";
    }

    /// <summary>
    /// Calcular descuento según la categoría del cliente
    /// </summary>
    private decimal CalcularDescuentoCategoria(decimal subtotal, Cliente cliente)
    {
        if (cliente.Categoria == null || cliente.Categoria.DescuentoPorcentaje == 0)
        {
            return 0;
        }

        var descuento = Math.Round(subtotal * (cliente.Categoria.DescuentoPorcentaje / 100), 2);
        _logger.LogInformation($"💰 Aplicando descuento de {cliente.Categoria.DescuentoPorcentaje}% ({cliente.Categoria.Nombre}): ${descuento}");

        return descuento;
    }

    #endregion
}